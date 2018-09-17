using System;
using Renci.SshNet;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTRE.Phoenix.Diagnostics
{
    public class CtrSshClient
    {
        SshClient _client;
        RioUpdater _rioReference;
        public CtrSshClient(HostNameAndPort host, RioUpdater rioReference)
        {
            _rioReference = rioReference;
            _client = new SshClient(host.GetHostName(), "admin", "");
            _client.Connect();
        }

        public void Close()
        {
            if (_client != null)
                _client.Disconnect();
        }
        public bool SendCommand(string command)
        {
            string response;
            return SendCommand(command, out response);
        }
        public bool SendCommand(string command, out string response)
        {
            /* init outputs */
            response = string.Empty;

            var cmd = _client.CreateCommand(command);   //  very long list
            cmd.CommandTimeout = TimeSpan.FromSeconds(15);
            bool gotResponse = false;
            try
            {
                response = cmd.Execute();
                gotResponse = true;
            }
            catch (Renci.SshNet.Common.SshOperationTimeoutException)
            {
                gotResponse = false;
            }

            if (gotResponse)
            {
                /* pass response to caller for processing */
                /* throw out newlines */
                var toPrint = TrimNewlines(response);
                _rioReference.Log(" " + toPrint); /* indent for clarity */
            }
            else
            {
                _rioReference.Log(" Command timed out, moving on...."); /* indent for clarity */
                return false;
            }
            return true;
        }

        string TrimNewlines(string term)
        {
            /* throw out newlines */
            return term.Replace('\n', ' ').Replace('\r', ' ');
        }
    }
}
