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

			if (error == Status.Ok) 
			{
				error = SendFileContents (file.GetContents (), file.TargetPath);
			}
			return error;
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
            if (_started)
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
        bool SendCommand(string text, string command, out string response)
        {
            CtrSshClient term = null;
            response = null;
            try
            {
                term = new CtrSshClient(_host, this);

                Log(text);
                term.SendCommand(command, out response);

                return true;
            }
            catch (Exception) { }
            finally
            {
                if (term != null)
                {
                    term.Close();
                }
            }

            return false;
        }
        bool SendCommand(string text, string command)
        {
            string response;
            return SendCommand(text, command, out response);
        }
        bool SendCommand(string command)
        {
            return SendCommand(command, command);
        }
        public void UpdateRobotController(bool bInstallAnimIntoRioWebServer)
        {
            SftpClient ftpClient = null;
            int error = 0;

            /* start stop watch */
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            /* copy a mutable list of File objects */
            ArrayList files = new ArrayList();
            foreach (RioFile sourceFile in RioFiles.kFilesToCreate) { files.Add(sourceFile); }

            /* throw out whatever needs to be removed */
            if (bInstallAnimIntoRioWebServer == false)
            {
                /* loop thru all tentaive target filesand throw out js files */
                for (int i = 0; i < files.Count;)
                {
                    RioFile file = (RioFile)files[i];
                    if (file.Sourcename.Contains(".js"))
                    {
                        /* remove file, now i will point to next elem */
                        files.RemoveAt(i);
                    }
                    else
                    {
                        /* go to next elem */
                        ++i;
                    }
                }
            }

            /* start connection */
            if (error == 0)
            {
                Log("Connecting to roboRIO " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                ftpClient = new SftpClient(_host.GetHostName(), "admin", "");
                ftpClient.ConnectionInfo.Timeout = new TimeSpan(0, 0, 0, 0, 6000);
                try { ftpClient.Connect(); }
                catch (Exception) { error = -1; }
                if (error == 0)
                    Log("Connected sucessfully.");
                else
                    Log("Could not connect.");
            }

            /* stop diag server if running already */
            if (error == 0)
            {
                SendCommand("Attempting to close server", "/etc/init.d/Phoenix-diagnostics-server stop");
                /* even if this fails, keep going */
            }

            /* check file presence */
            if (error == 0)
            {
                foreach (RioFile file in files)
                {
                    if (file.CheckExistanceOnPC() == false)
                    {
                        /* could not open file? */
                        Log("  Could not open file:" + file.TargetPath);
                        error = -10;
                    }
                }
            }
            /* backup what needs to be backed up */
            if (error == 0)
            {
                foreach (string filepath in RioFiles.kFilesToBackup)
                {
                    string backUpPath = filepath + ".bak";
                    if (ftpClient.Exists(backUpPath))
                    {
                        /* already there, don't change anything */
                    }
                    else if (ftpClient.Exists(filepath) == false)
                    {
                        /* not there, don't bother */
                    }
                    else
                    {
                        /* copy command */
                        String command = "cp " + filepath + " " + backUpPath;
                        SendCommand(command);
                    }
                }
            }

            /* write new files  */
            if (error == 0)
            {
                Log("Writing files...");
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
                            /* if file exists, delete it */
                            if (ftpClient.Exists(file.TargetPath)) { ftpClient.DeleteFile(file.TargetPath); }
                        }
                        catch (Exception) { }
                        /* write it */
                        try
                        {
                            ftpClient.WriteAllBytes(file.TargetPath, content);
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
            if (ftpClient != null)
            {
                try { ftpClient.Disconnect(); }
                catch (Exception) { }
            }

            /* permissions */
            if (error == 0)
            {
                Log("Updating File Write Permissions");
                SendCommand("chmod 777 /etc/init.d/Phoenix-diagnostics-server");
                SendCommand("chmod 777 /usr/local/frc/bin/Phoenix-diagnostics-server");
                Thread.Sleep(1000); //Wait a bit to make sure any file changes actually took
            }
            /* sync */
            if (error == 0)
            {
                Log("Syncing RoboRIO to ensure files are on the flash");
                SendCommand("sync");
                Thread.Sleep(1000); //Wait a bit to make sure any file changes actually took
            }

            /* Create Symlink for Phoenix-diagnostics-server in etc/rc5.d/ */
            if (error == 0)
            {
                Log("Creating/Updating Symlink for startup");
                if (false == SendCommand("ln -sf /etc/init.d/Phoenix-diagnostics-server /etc/rc5.d/S25Phoenix-diagnostics-server"))
                {
                    error = -39;
                }
            }
            /* restart server */
            if (error == 0)
            {
                if (false == SendCommand("Starting Server", "/etc/init.d/Phoenix-diagnostics-server start"))
                {
                    error = -40;
                    Log("Server could not be started, you many need to restart the RoboRIO. ");
                }
                else { Log("Server has successfully started."); }
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

        public void RevertRobotController()
        {
            SftpClient ftpClient = null;
            int error = 0;

            /* start stop watch */
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            /* copy a mutable list of File objects to delete */
            ArrayList filesToDel = new ArrayList();
            foreach (string sourceFile in RioFiles.kFilesToDeleteOnRevert) { filesToDel.Add(sourceFile); }

            /* start connection */
            if (error == 0)
            {
                Log("Connecting to roboRIO " + DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                ftpClient = new SftpClient(_host.GetHostName(), "admin", "");
                ftpClient.ConnectionInfo.Timeout = new TimeSpan(0, 0, 0, 0, 6000);
                try { ftpClient.Connect(); }
                catch (Exception) { error = -1; }
                if (error == 0)
                    Log("Connected sucessfully.");
                else
                    Log("Could not connect.");
            }

            /* stop diag server if running already */
            if (error == 0)
            {
                SendCommand("Attempting to close server", "/etc/init.d/Phoenix-diagnostics-server stop");
                /* even if this fails, keep going */
            }

            /* delete them the diag related files */
            if (error == 0)
            {
                foreach (string filePathToDel in filesToDel)
                {
                    if (ftpClient.Exists(filePathToDel) == false)
                    {
                        /* do nothing */
                        Log("File does not exist for " + filePathToDel);
                    }
                    else
                    {
                        /* delete the target */
                        try { ftpClient.DeleteFile(filePathToDel); } catch (Exception) { }
                    }
                }
            }

            /* restore files that are were backed up via .bak extension*/
            if (error == 0)
            {
                foreach (string filepath in RioFiles.kFilesToBackup)
                {
                    string destPath = filepath;
                    string sourcePath = filepath + ".bak";

                    if (ftpClient.Exists(sourcePath) == false)
                    {
                        /* file has no backup, move on */
                    }
                    else
                    {
                        /* delete the newer file if it exists */
                        if (ftpClient.Exists(destPath)) { ftpClient.DeleteFile(destPath); }

                        /* copy bak file to original path */
                        String command = "cp " + sourcePath + " " + destPath;
                        SendCommand(command);
                    }
                }
            }

            /* always close the sftp */
            if (ftpClient != null)
            {
                try { ftpClient.Disconnect(); } catch (Exception) { }
            }
            /* restart server */
            if (error == 0) { Log("Binaries have successfully been removed from the RIO"); }
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
