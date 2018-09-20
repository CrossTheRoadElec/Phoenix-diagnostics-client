using System;
using System.Text;
using System.Net;
using System.IO;
using CTRE.Phoenix.dotNET;
using CTRE.Phoenix.Diagnostics.BackEnd;

namespace CTRE.Phoenix.Diagnostics.HTTP
{

    /// <summary>
    /// Implems all the HTTP exchanges.  The Phoenix-diagnostic-server receives HTTP commands and responds with a JSON payload.
    /// </summary>
    public class WebExchange
    {
        private WebRequest _client = null;

        // ------------------ Public functional interface ----------------------------- //
        public Status HttpGet(HostNameAndPort hostName, string model, byte deviceID, ActionType action, out string response, string extraOptions = "", int timeout = 1000)
        {
            string ip = hostName.ToString();
            Status retval = Status.Ok;

            /* init outputs */
            response = string.Empty;

            /* build URI to send */
            string builtUrl = BuildURI(ip, model, deviceID, action, extraOptions);

            /* attempt sending */
            try
            {
                response = SendHttpGet(builtUrl, timeout);
            }
            catch (WebException we)
            {
                Debug.Print("HTTP", we.ToString());
                retval = Status.TimedOut;
            }
            catch (Exception e)
            {
                Debug.Print("HTTP", e.ToString());
                retval = Status.BadParam;
            }

            /* telemtry */
            Telemetry.GetInstance().Push(new Telemetry.ExchangeLog("GET", builtUrl, response, action));

            /* give caller the response */
            return retval;
        }
        public Status HttpPost(HostNameAndPort hostName, string model, byte deviceID, ActionType action, byte [] file, out string response, int timeout = 2000)
        {
            Status retval = Status.Ok;

            string ip = hostName.ToString();

            /* init outputs */
            response = string.Empty;

            /* build URI to send */
            string extraOptions = string.Empty;
            string builtUrl = BuildURI(ip, model, deviceID, action, extraOptions);

            /* attempt sending */
            try { response = SendHttpPost(builtUrl, file, timeout); }
            catch (WebException we)
            {
                Debug.Print("HTTP", we.ToString());
                retval = Status.TimedOut;
            }
            catch (Exception e)
            {
                Debug.Print("HTTP", e.ToString());
                retval = Status.BadParam;
            }

            /* telemtry */
            Telemetry.GetInstance().Push(new Telemetry.ExchangeLog("POST", builtUrl, response, action));

            /* give caller the response */
            return retval;
        }

        /* ---------------------- Private routines ----------------- */
        private string SendHttpGet(string builtUrl, int timeout = 500)
        {
            string retval = "";

            //Request a GET at specified address
            WebRequest request = WebRequest.Create(builtUrl);
            request.Timeout = timeout;

            /* perform the exchagne */
            using (WebResponse response = request.GetResponse())
            {
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                //Got the string resposne
                retval = reader.ReadToEnd();

                //Clean up afterwards
                reader.Close();
                dataStream.Close();
                response.Close();
            }
      
            return retval;
        }


        private string SendHttpPost(string ip, byte [] file, int timeout = 2000)
        {
            string boundary = "||-----------------||";
            WebRequest client = WebRequest.Create(ip);
            client.Method = "POST";
            client.ContentLength = file.Length + boundary.Length + 2 + boundary.Length + 4;
            client.ContentType = "multipart/form-data; boundary=\"" + boundary + "\"\r\n\r\n";
            client.Timeout = timeout;

            /* save in case Abort gets called */
            _client = client;

            string retval;
        
            using (var mpStream = (client.GetRequestStream()))
            {
                /* Follow convention of form-data with boundaries */
                mpStream.Write(Encoding.UTF8.GetBytes("--"), 0, 2);
                mpStream.Write(Encoding.UTF8.GetBytes(boundary), 0, boundary.Length);
                mpStream.Write(file, 0, file.Length);
                mpStream.Write(Encoding.UTF8.GetBytes("--"), 0, 2);
                mpStream.Write(Encoding.UTF8.GetBytes(boundary), 0, boundary.Length);
                mpStream.Write(Encoding.UTF8.GetBytes("--"), 0, 2);

                mpStream.Close();
            }

            using (WebResponse response = client.GetResponse())
            {

                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                //Got the string resposne
                retval = reader.ReadToEnd();

                //Clean up afterwards
                reader.Close();
                dataStream.Close();
                response.Close();
            }

            /* null client */
            _client = null;

            return retval;
        }

        private string BuildURI(string baseIP, string model, byte deviceID, ActionType action, string extraOptions = "")
        {
            string address = baseIP;
            address += "/?";
            if (model == "")
            {
                // Do nothing
            }
            else
            {
                address += "model=" + model;
            }
            address += "&";

            address += "id=" + deviceID;

            address += "&";

            switch (action)
            {
                case ActionType.None:
                    break; //Just calling the address for a ping
                default:
                    address += URI.ActionMap[action] + extraOptions;
                    break;
            }
            address = Uri.EscapeUriString(address);
            return address;
        }

        public void Abort()
        {
            if (_client != null)
            {
                try { _client.Abort(); }
                catch (Exception) { }
            }
        }
    }
}
