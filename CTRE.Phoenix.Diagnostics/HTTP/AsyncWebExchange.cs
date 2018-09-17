using System;
using System.Text;
using System.Net;
using System.IO;
using CTRE.Phoenix.dotNET;
using CTRE.Phoenix.Diagnostics.BackEnd;
using System.Threading;

namespace CTRE.Phoenix.Diagnostics.HTTP
{

    /// <summary>
    /// Implems all the HTTP exchanges.  The Phoenix-diagnostic-server receives HTTP commands and responds with a JSON payload.
    /// </summary>
    public class AsyncWebExchange
    {
        private class ThreadParams
        {
            public HostNameAndPort hostName;
            public Model device;
            public byte deviceID;
            public ActionType action;
            public byte [] file;
            public int timeout;

            public string outResponse;
            public Status outStatus;
        }

        private dotNET.Thread _thread;

        private WebExchange _webExchange = new WebExchange();
        private ManualResetEvent _isDone = new ManualResetEvent(false);
        private ThreadParams _params = new ThreadParams();
        
        public Status StartHttpPost(HostNameAndPort hostName, Model device, byte deviceID, ActionType action, byte [] file, int timeout = 2000)
        {
            if (_thread != null)
                return Status.Busy;

            _params.hostName = hostName;
            _params.device = device;
            _params.deviceID = deviceID;
            _params.action = action;
            _params.file = file;
            _params.timeout = timeout;

            _isDone.Reset();

            _thread = new dotNET.Thread(ExecuteTask);
            _thread.Start();

            return Status.Ok;
        }
        public Status PollResponse(int timeoutMs, out string response)
        {
            if (_isDone.WaitOne(timeoutMs) == true)
            {
                Dispose(timeoutMs);

                response = _params.outResponse;
                return _params.outStatus;
            }
            else
            {
                /* timed out waiting for resp */
                response = string.Empty;
                return Status.AsyncWebReqNotDone;
            }
        }

        public void Dispose(int timeoutMs = 500)
        {
            _webExchange.Abort();
            if (_thread != null)
            {
                _thread.Shutdown(timeoutMs);
            }
            _thread = null;
        }


        private void ExecuteTask()
        {
            ThreadParams p = (ThreadParams)_params;

            p.outStatus = _webExchange.HttpPost(p.hostName, p.device, p.deviceID, p.action, p.file, out p.outResponse, p.timeout);

            _isDone.Set();
        }
    }
}
