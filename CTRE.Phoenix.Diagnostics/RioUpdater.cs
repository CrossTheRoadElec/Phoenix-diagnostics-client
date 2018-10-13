using System;
using System.Text;
using System.Threading;
using Renci.SshNet;
using System.Diagnostics;
using System.Collections;

namespace CTRE.Phoenix.Diagnostics
{
    public class RioUpdater
    {
        private bool _stop = false;
        private bool _isStopped = false;
        private int _error = 0;
        private StringBuilder _sb = new StringBuilder();
        private readonly object _lock = new object();
        private bool _started;
        HostNameAndPort _host;

        public RioUpdater(HostNameAndPort host)
        {
            if (host != null)
            {
                _started = true;
                _host = host;
            }
        }
        
        public void StartUpdate()
        {
            ThreadProcUpdate();
        }
        public void StartRevert()
        {
            ThreadProcRestore();
        }
        /// <summary>
        /// Method to send arbitrary files to server at target location
        /// </summary>
        /// <param name="hostPath">Location of file on PC</param>
        /// <param name="targetPath">Target location of file on server</param>
        /// <returns></returns>
        public Status SendFileContents(byte[] content, string targetPath)
        {
            /* This will only happen on second tab, so don't bother logging */
            Status error = Status.Ok;
            SftpClient client = new SftpClient(_host.GetHostName(), "admin", "");

            /* Try to connect to server */
            if (error == Status.Ok)
            {
                client.ConnectionInfo.Timeout = new TimeSpan(0, 0, 0, 0, 6000);
                try
                {
                    client.Connect();
                }
                catch (Exception)
                {
                    error = Status.CouldNotSFTPToServer;
                }

            }

            /* If connected to server, try to read the file */
            if (error == Status.Ok)
            {
                if (content == null)
                {
                    error = Status.CouldNotOpenFile;
                }
                /* If file has contents, delete and place it */
                else
                {
                    /* delete file first, regardless of success, continue */
                    try
                    {
                        if (client.Exists(targetPath))
                        {
                            client.DeleteFile(targetPath);
                        }
                    }
                    catch (Exception) { }
                    /* write it */
                    try
                    {
                        client.WriteAllBytes(targetPath, content);
                    }
                    catch (Exception)
                    {
                        error = Status.CouldNotWriteFile;
                    }
                }
            }
            return error;
        }
        public Status SendFile(RioFile file)
        {
            /* This will only happen on second tab, so don't bother logging */
            Status error = Status.Ok;
            SftpClient client = new SftpClient(_host.GetHostName(), "admin", "");

            /* Check if file exists on PC */
            if (error == Status.Ok)
            {
                if (file.CheckExistanceOnPC() == false)
                {
                    /* could not open file? */
                    Log("  Could not open file:" + file.TargetPath);
                    error = Status.CouldNotOpenFile;
                }
            }

            return SendFileContents(file.GetContents(), file.TargetPath);
        }
        public Status StartServer()
        {
            Status error = Status.Ok;
            CtrSshClient term = null;
            try
            {
                term = new CtrSshClient(_host, this);

                Log("Starting Server");
                string output = "";
                if (false == term.SendCommand("/etc/init.d/Phoenix-diagnostics-server start", out output))
                {
                    error = Status.TimedOut;
                }
                if (output == "")
                    error = Status.ProcessNotRunning;
            }
            catch (Exception)
            {
                error = Status.GeneralError;
            }
            finally
            {
                if (term != null)
                {
                    term.Close();
                }
            }
            _isStopped = true;
            return error;
        }
        public Status StopServer()
        {
            Status error = Status.Ok;
            CtrSshClient term = null;
            try
            {
                term = new CtrSshClient(_host, this);

                Log("Stopping Server");
                string output = "";
                if (false == term.SendCommand("/etc/init.d/Phoenix-diagnostics-server stop", out output))
                {
                    error = Status.TimedOut;
                }
                if (output == "")
                    error = Status.ProcessNotRunning;
            }
            catch (Exception)
            {
                error = Status.GeneralError;
            }
            finally
            {
                if (term != null)
                {
                    term.Close();
                }
            }
            _isStopped = true;
            return error;
        }
        public Status RebootRio()
        {
            Status error = Status.Ok;
            if (error == Status.Ok)
            {
                CtrSshClient term = null;
                try
                {
                    term = new CtrSshClient(_host, this);

                    Log("Restarting RIO");
                    string output = "";
                    if (false == term.SendCommand("reboot", out output))
                    {
                        error = Status.TimedOut;
                    }
                }
                catch (Exception)
                {
                    error = Status.GeneralError;
                }
                finally
                {
                    if (term != null)
                    {
                        term.Close();
                    }
                }
            }
            _isStopped = true;
            return error;
        }
        public Status CheckProcessStarted()
        {
            Status error = Status.Ok;
            if (error == Status.Ok)
            {
                CtrSshClient term = null;
                try
                {
                    term = new CtrSshClient(_host, this);

                    Log("Checking Process");
                    string output = "";
                    if (false == term.SendCommand("pidof /usr/local/frc/bin/Phoenix-diagnostics-server", out output))
                    {
                        error = Status.TimedOut;
                    }
                    if (output == "")
                        error = Status.ProcessNotRunning;
                }
                catch (Exception)
                {
                    error = Status.GeneralError;
                }
                finally
                {
                    if (term != null)
                    {
                        term.Close();
                    }
                }
            }
            _isStopped = true;
            return error;
        }
        public bool Poll(ref StringBuilder sb, ref int error)
        {
            if(_started)
            {
                sb.Clear();
                _started = false;
            }
            string temp = string.Empty;
            bool retval = false;
            /* lock and copy */
            lock (_lock)
            {
                temp = _sb.ToString();
                _sb.Clear();

                retval = _isStopped;
                error = _error;
            }
            /* append to caller */
            sb.Append(temp);
            return retval;
        }

        private bool CheckForStop()
        {
            lock (_lock)
            {
                return _stop;
            }
        }
        private void OnFinished(int error)
        {
            lock (_lock)
            {
                _isStopped = true;
                _error = error;
            }
        }
        public void Log(string s)
        {
            lock (_lock)
            {
                _sb.AppendLine(s);
            }
        }

        string TrimNewlines(string term)
        {
            /* throw out newlines */
            return term.Replace('\n', ' ').Replace('\r', ' ');
        }

        private void ThreadProcUpdate()
        {
            SftpClient client = null;
            int error = 0;

            /* start stop watch */
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            /* copy a mutable list of File objects */
            ArrayList files = new ArrayList();
            foreach (RioFile sourceFile in RioFiles.kFiles)
            {
                files.Add(sourceFile);
            }

            /* start connection */
            if (error == 0)
            {
                Log("Connecting to roboRIO " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                client = new SftpClient(_host.GetHostName(), "admin", "");
                client.ConnectionInfo.Timeout = new TimeSpan(0, 0, 0, 0, 6000);
                try
                {
                    client.Connect();
                }
                catch (Exception)
                {
                    error = -1;
                }
                if (error == 0)
                    Log("  Connected sucessfully.");
                else
                    Log("  Could not connect.");
            }

            /* ldconfig */
            if (error == 0)
            {
                CtrSshClient term = null;
                try
                {
                    term = new CtrSshClient(_host, this);

                    Log("Attempting to close server");
                    term.SendCommand("/etc/init.d/Phoenix-diagnostics-server stop");

                }
                catch (Exception)
                {
                }
                finally
                {
                    if (term != null)
                    {
                        term.Close();
                    }
                }
            }

            /* check file presence */
            if (error == 0)
            {
                foreach (RioFile file in files)
                {
                    bool exists = file.CheckExistanceOnPC();
                    if (exists == false)
                    {
                        /* could not open file? */
                        Log("  Could not open file:" + file.TargetPath);
                        error = -10;
                    }
                }
            }
            /* write new files  */
            if (error == 0)
            {
                foreach (RioFile file in files)
                {
                    byte[] content = file.GetContents();
                    if (content == null)
                    {
                        Log("Failed to read " + file.SourcePath);
                        error = -30;
                    }
                    else
                    {
                        /* delete file first, regardless of success, continue */
                        try
                        {
                            if (client.Exists(file.TargetPath))
                            {
                                client.DeleteFile(file.TargetPath);
                            }
                        }
                        catch (Exception) { }
                        /* write it */
                        try
                        {
                            client.WriteAllBytes(file.TargetPath, content);
                            Log("Written file: " + file.TargetPath + " (" + file.Sourcename + ")");
                        }
                        catch (Exception)
                        {
                            Log("Failed to write " + file.TargetPath);
                            error = -70;
                        }
                    }
                }
            }

            /* always close the sftp */
            if (client != null)
            {
                try
                {
                    client.Disconnect();
                }
                catch (Exception) { }
            }

            /* ldconfig */
            if (error == 0)
            {
                CtrSshClient term = null;
                try
                {
                    term = new CtrSshClient(_host, this);
                    
                    Log("Updating File Write Permissions");
                    term.SendCommand("chmod 777 /etc/init.d/Phoenix-diagnostics-server");
                    term.SendCommand("chmod 777 /etc/rc5.d/S25Phoenix-diagnostics-server");
                    term.SendCommand("chmod 777 /usr/local/frc/bin/Phoenix-diagnostics-server");


                    //Wait a bit to make sure any file changes actually took
                    Thread.Sleep(1000);
                    Log("Syncing RoboRIO to ensure files are on the flash");
                    term.SendCommand("sync");
                    
                }
                catch (Exception)
                {
                }
                finally
                {
                    if (term != null)
                    {
                        term.Close();
                    }
                }
            }
            //Wait some more to allow sync ot take affect
            Thread.Sleep(1000);
            /* restart server */
            if (error == 0)
            {
                bool webRestartOk = true;
                CtrSshClient term = null;
                try
                {
                    term = new CtrSshClient(_host, this);
                    
                    Log("Starting Server");
                    if (false == term.SendCommand("/etc/init.d/Phoenix-diagnostics-server start"))
                    {
                        webRestartOk = false;
                    }
                }
                catch (Exception)
                {
                    webRestartOk = false;
                }
                finally
                {
                    if (term != null)
                    {
                        term.Close();
                    }
                }
                if (false == webRestartOk)
                {
                    error = -40;
                    Log("Server could not be started, you many need to restart the RoboRIO. ");
                }
                else
                {
                    Log("Server has successfully started.");
                }
            }

            /* stop and report time */
            OnFinished(error);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Log("\nDuration: " + elapsedTime);
        }

        private void ThreadProcRestore()
        {
            SftpClient client = null;
            int error = 0;

            /* start stop watch */
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            /* copy a mutable list of File objects */
            ArrayList files = new ArrayList();
            foreach (RioFile sourceFile in RioFiles.kFiles)
            {
                files.Add(sourceFile);
            }

            /* start connection */
            if (error == 0)
            {
                Log("Connecting to roboRIO " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                client = new SftpClient(_host.GetHostName(), "admin", "");
                client.ConnectionInfo.Timeout = new TimeSpan(0, 0, 0, 0, 6000);
                try
                {
                    client.Connect();
                }
                catch (Exception)
                {
                    error = -1;
                }
                if (error == 0)
                    Log("  Connected sucessfully.");
                else
                    Log("  Could not connect.");
            }

            /* ldconfig */
            if (error == 0)
            {
                CtrSshClient term = null;
                try
                {
                    term = new CtrSshClient(_host, this);

                    Log("Attempting to close server");
                    term.SendCommand("/etc/init.d/Phoenix-diagnostics-server stop");

                }
                catch (Exception)
                {
                }
                finally
                {
                    if (term != null)
                    {
                        term.Close();
                    }
                }
            }

            /* check file presence */
            if (error == 0)
            {
                foreach (RioFile file in files)
                {
                    bool exists = file.CheckExistanceOnPC();
                    if (exists == false)
                    {
                        /* could not open file? */
                        Log("  Could not open file:" + file.TargetPath);
                        error = -10;
                    }
                }
            }
            /* old files, make them or restore them depending on action  */
            if (error == 0)
            {
                foreach (RioFile file in files)
                {
                    if (client.Exists(file.TargetPath) == false)
                    {
                        /* do nothing */
                        Log("File does not exist for " + file.TargetPath);
                    }
                    else
                    {
                        /* delete the target */
                        try { client.DeleteFile(file.TargetPath); } catch (Exception) { }
                    }
                }
            }

            /* always close the sftp */
            if (client != null)
            {
                try
                {
                    client.Disconnect();
                }
                catch (Exception) { }
            }
            /* restart server */
            if (error == 0)
            {
                Log("Binaries have successfully been removed from the RIO");
            }

            /* stop and report time */
            OnFinished(error);
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            Log("\nDuration: " + elapsedTime);
        }


    }
}
