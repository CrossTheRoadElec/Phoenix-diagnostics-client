using System;

namespace CTRE.Phoenix.Diagnostics
{
    public class HostNameAndPort
    {
        private string _name = string.Empty;
        private string _port = string.Empty;

        private string _newName = string.Empty;
        private string _newPort = string.Empty;

        private bool _changed = false;

        public bool HasChanged()
        {
            if (_changed)
            {
                _changed = false;

                /* move new name into name */
                _name = _newName;
                _port = _newPort;

                return true;
            }

            return false;
        }

        public void Set(string name, string port)
        {
            /* remove everything after # */
            name = name.Split('#')[0];
            name = name.Trim();

            port = port.Split('#')[0];
            port = port.Trim();

            if (_newName.CompareTo(name) != 0)
            {
                /* save it, thread loop will pick it up */
                _newName = name;
                /* signal main loop */
                _changed = true;
            }
            if (_newPort.CompareTo(port) != 0)
            {
                /* save it, thread loop will pick it up */
                _newPort = port;
                /* signal main loop */
                _changed = true;
            }
        }

        /// <returns>Get just the hostname/IP (without port or http prefix)</returns>
        public string GetHostName()
        {
            return _name;
        }
        /// <returns>Full URI with port number and http prefix </returns>
        public string GetFullURI()
        {
            string retval = _name;

            /* if http prefix is missing, add it */
            if (retval.Length == 0)
            {
                /* leave blank */
            }
            else if (retval.StartsWith("http")) // also covers https
            {
                /* already there */
            }
            else
            {
                /* insert http */
                retval = "http://" + retval;
            }

            /* append : */
            retval += ":";

            /* append port number */
            retval += _port;

            return retval;
        }

        public override string ToString()
        {
            return GetFullURI();
        }
    }
}
