using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CTRE.Phoenix.Diagnostics
{
    public class DeviceDescriptors
    {
        // TODO replace this with a set
        private  Dictionary<uint, DeviceDescrip> _map = new Dictionary<uint, DeviceDescrip>();

        public void Insert(DeviceDescrip dd)
        {
            DeviceDescrip ddRef;
            if (_map.TryGetValue(dd.ToKeyValue(), out ddRef))
            {
                /* already there - update strings */
                ddRef.jsonStrings = dd.jsonStrings;
                ddRef.updateTimestamp = DateTime.UtcNow;
                /* do not update configs, this comes from a seperate command */

                _map[ddRef.ToKeyValue()] = ddRef;
            }
            else
            {
                /* first insert */
                dd.updateTimestamp = DateTime.UtcNow;
                _map.Add(dd.ToKeyValue(), dd);
            }

        }

        public bool Get(Model model, byte deviceID, out DeviceDescrip ddRef)
        {
            uint keyValue = DeviceDescrip.KeyValue.ToKeyValue(model, deviceID);

            return _map.TryGetValue(keyValue, out ddRef);
        }
        public uint Count
        {
            get
            {
                return (uint)_map.Count;
            }
        }
        public bool Remove(Model model, byte deviceID)
        {
            uint keyValue = DeviceDescrip.KeyValue.ToKeyValue(model, deviceID);

            bool existed = _map.Remove(keyValue);

            return existed;
        }
        public bool Remove(DeviceDescrip dd)
        {
            bool existed = _map.Remove(dd.ToKeyValue());

            return existed;
        }
        /// <summary>
        /// Creates a flat array with every element at the moment of the call.  This is useful
        /// for looping through each discovered ECU while removing stale ECUs in the inner loop.
        /// Inner lopo should call Get() to confirm element still exists.
        /// </summary>
        /// <returns> Flat array of key value pairs</returns>
        public DeviceDescrip[] ToArray()
        {
            DeviceDescrip[] retval = new DeviceDescrip[_map.Count()];

            int i = 0;
            foreach(var elem in _map)
            {
                retval[i++] = elem.Value;
            }

            return retval;
        }

        public bool ChangeID(DeviceDescrip dd, byte newID)
        {
            DeviceDescrip ddRef;
            if (_map.TryGetValue(dd.ToKeyValue(), out ddRef))
            {
                /* remove the original */
                _map.Remove(ddRef.ToKeyValue());

                /* modify the ID manually, this should match the next device poll */
                ddRef.deviceID = (byte)newID;

                /* re-insert */
                Insert(ddRef);
                return true;
            }
            return false;
        }
        public List<DeviceDescrip> ToList()
        {
            List<DeviceDescrip> retval = new List<DeviceDescrip>();

            try
            {
                foreach (var elem in _map)
                {
                    retval.Add(elem.Value);
                }
            }
            catch (Exception) { }

            return retval;
        }

    }
}
