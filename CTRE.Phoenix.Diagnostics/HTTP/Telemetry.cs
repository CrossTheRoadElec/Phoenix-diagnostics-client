using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using CTRE.Phoenix.dotNET;
using CTRE.Phoenix.Diagnostics.BackEnd;

namespace CTRE.Phoenix.Diagnostics.HTTP
{
    public class Telemetry
    {
        public class ExchangeLog
        {
            public int lineNumber;
            public String type;
            public String request;
            public String response;
            public ActionType actionType;

            private static int counter = 0;

            public ExchangeLog(String type, String request, String response, ActionType actionType)
            {
                this.lineNumber = ++counter;
                this.type = type;
                this.request = request;
                this.response = response;
                this.actionType = actionType;
            }
            public const int kStringArrayIndex_Num = 0;
            public const int kStringArrayIndex_Req = 1;
            public const int kStringArrayIndex_Resp = 2;
            public const int kStringArrayIndex_Type = 3;

            public string [] ToStringArray()
            {
                var retval = new string[4];

                retval[kStringArrayIndex_Num] = lineNumber.ToString();
                retval[kStringArrayIndex_Req] = request;
                retval[kStringArrayIndex_Resp] = response;
                retval[kStringArrayIndex_Type] = type;

                return retval;
            }
        }
        private List<ExchangeLog> _exchanges = new List<ExchangeLog>();
        private object _lock = new object();

        public void Push(ExchangeLog exchange)
        {
            lock (_lock)
            {
                _exchanges.Add(exchange);
            }
        }
        public List<ExchangeLog> Pop()
        {
            if (_exchanges.Count == 0)
                return null;

            lock (_lock)
            {
                List<ExchangeLog> retval = _exchanges;
                _exchanges = new List<ExchangeLog>();
                return retval;
            }
        }
        /* ------------- singletone pattern --------------------- */
        private static Telemetry _instance = new Telemetry();
        private Telemetry()
        {

        }
        public static Telemetry GetInstance()
        {
            return _instance;
        }
    }
}
