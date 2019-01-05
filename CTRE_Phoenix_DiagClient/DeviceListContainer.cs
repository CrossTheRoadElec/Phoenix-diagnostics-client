using CTRE.Phoenix.Diagnostics;
using CTRE.Phoenix.Diagnostics.BackEnd;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CTRE_Phoenix_DiagClient
{
    /// <summary>
    /// Container class that will take ownership of a device tree view.  Form will pass relevent data from backend to this object,
    /// will will rerender the graphical list in a lazy/fast fashion.  On click events, device selectection/de-selection are 
    /// also handled here.
    /// 
    /// The main form should not interact with lstDevices directly, but rather uses the member functions of this class.
    /// </summary>
    public class DeviceListContainer
    {
        /* Device timeout before graying-out, Original value of 5 */
        private double kDeviceTimeout = 2.5;    //Seconds

        private Dictionary<ListViewItem, DeviceDescrip> _mapDescriptors = new Dictionary<ListViewItem, DeviceDescrip>();

        private System.Windows.Forms.ListView lstDevices;

        public delegate void SelectionChangedEventHandler(object sender, EventArgs e, DeviceDescrip dd);

        public event SelectionChangedEventHandler SelectionChanged;

        public DeviceListContainer(System.Windows.Forms.ListView listView)
        {
            /* save the gui object we will manipulate */
            lstDevices = listView;
            /* clear the list view */
            lstDevices.Items.Clear();
        }
        public bool GetDescriptor(ListViewItem item, out DeviceDescrip descriptor)
        {
            if (item != null)
            {
                if (_mapDescriptors.ContainsKey(item))
                {
                    descriptor = _mapDescriptors[item];
                    return true;
                }
            }
            descriptor = null;
            return false;
        }
        bool GetListViewItem(DeviceDescrip descriptor, out ListViewItem item)
        {
            foreach (KeyValuePair<ListViewItem, DeviceDescrip> entry in _mapDescriptors)
            {
                if (entry.Value.deviceID == descriptor.deviceID && entry.Value.model == descriptor.model)
                {
                    item = entry.Key;
                    return true;
                }
            }
            item = null;
            return false;
        }
        public void SelectDevice(ListViewItem item)
        {
            if (item == null)
            {
                if (lstDevices.Items.Count > 0)
                {   /* none were selected before, or it fell out, so select the first one */
                    lstDevices.Items[0].Selected = true;
                }
            }
            else if (item.Selected == false)
            {
                /* only set the property if it's falseto prevent redundant number entry updates */
                item.Selected = true;
            }
        }
        private DeviceDescrip Lookup(ListViewItem item)
        {
            if (item == null)
                return null;

            DeviceDescrip descriptor;
            if (GetDescriptor(item, out descriptor))
            {
            }
            return descriptor;
        }
        bool AssignIfDiff(ListViewItem.ListViewSubItem lhs, string rhs)
        {
            if (lhs.Text.Equals(rhs))
            {
                return false;
            }
            else
            {
                lhs.Text = rhs;
                return true;
            }
        }
        /**
         * Create a row in the device tree 
         */
        ListViewItem CreateListViewItem(DeviceDescrip descriptor)
        {
            ListViewItem item = new ListViewItem();
            item.SubItems.Clear();
            item.Text= descriptor.jsonStrings.Name;                         // Name
            item.SubItems.Add((descriptor.jsonStrings.SoftStatus));         // Status
            item.SubItems.Add((descriptor.jsonStrings.Model));              // Model
            item.SubItems.Add((descriptor.deviceID.ToString()));            // devID
            item.SubItems.Add((descriptor.jsonStrings.CurrentVers));        // Firm
            item.SubItems.Add((descriptor.jsonStrings.ManDate));            // Man Date
            item.SubItems.Add((descriptor.jsonStrings.BootloaderRev));      // Btld
            item.SubItems.Add(descriptor.jsonStrings.HardwareRev);          // Hard Rev
            item.SubItems.Add(descriptor.jsonStrings.Vendor);               // Vendor
            item.ImageIndex = ModelToInt(descriptor.model);    /* icon index, same as model */

            return item;
        }
        bool FillListViewItem(ListViewItem item, DeviceDescrip descriptor)
        {
            bool ValueChanged = false; /* this can be used to count the tree changes */
            int i = 0;
            ValueChanged |= AssignIfDiff(item.SubItems[i++], descriptor.jsonStrings.Name);              // Name
            ValueChanged |= AssignIfDiff(item.SubItems[i++], descriptor.jsonStrings.SoftStatus);        // Status
            ValueChanged |= AssignIfDiff(item.SubItems[i++], descriptor.jsonStrings.Model);             // Model
            ValueChanged |= AssignIfDiff(item.SubItems[i++], descriptor.deviceID.ToString());           // devID
            ValueChanged |= AssignIfDiff(item.SubItems[i++], descriptor.jsonStrings.CurrentVers);       // Firm
            ValueChanged |= AssignIfDiff(item.SubItems[i++], descriptor.jsonStrings.ManDate);           // Man Date
            ValueChanged |= AssignIfDiff(item.SubItems[i++], descriptor.jsonStrings.BootloaderRev);     // Btld
            ValueChanged |= AssignIfDiff(item.SubItems[i++], descriptor.jsonStrings.HardwareRev);       // Hard Rev
            ValueChanged |= AssignIfDiff(item.SubItems[i++], descriptor.jsonStrings.Vendor);            // Vendor

            /* icon index, same as model */
            int imageIndex = ModelToInt(descriptor.model);   
            if (item.ImageIndex != imageIndex)
            {
                item.ImageIndex = imageIndex;
                ValueChanged = true;
            }

            /* brk pt here to catch changes in tree*/
            if (ValueChanged)
                return true; 
            return false;
        }

        private int ModelToInt(string model)
        {
            switch (model.ToLower())
            {
                case "pcm":
                    return 1;
                case "pdp":
                    return 2;
                case "talon srx":
                    return 3;
                case "victor spx":
                    return 4;
                case "pigeon":
                    return 5;
                case "pigeon over ribbon":
                    return 6;
                case "canifier":
                    return 7;
                default:
                    return 0;
            }
        }

        public Status RemoveSelectedItem()
        {
            return _mapDescriptors.Remove(GetSelectedDevice()) ? Status.Ok : Status.DeviceNotFound;
        }
        /// <summary>
        /// Update list of devices.
        /// </summary>
        /// 
        /// BackEnd will give us periodic updates to the list of devices on the bus.
        /// So we want to ...
        /// - remove devices that dissapear, 
        /// - add missing devices yet to be added, 
        /// - update any properties that have changed, 
        /// - and do all this in a minimal fashion so user does not see flickering
        /// 
        /// <param name="newDDs"></param>
        public void RefreshDeviceTree(IEnumerable<DeviceDescrip> newDDs)
        {
            /* get selected device */
            ListViewItem oldSelection = GetSelectedDevice();
            DateTime currentTimeStamp = BackEnd.Instance.GetLastPoll();

            /* first remove anything we have now that isn't in the new list */
            for (int i = 0; i < lstDevices.Items.Count; /*no step*/ )
            {
                /* for each listitem in treeview*/
                ListViewItem oldListViewItem = lstDevices.Items[i];

                /* find the current DD for this list item, and find the new one. If there is no new one this device is gone */
                DeviceDescrip oldDD;
                if (GetDescriptor(oldListViewItem, out oldDD))
                {
                    /* go to next one */
                    ++i;

                    if (currentTimeStamp - oldDD.updateTimestamp > TimeSpan.FromSeconds(kDeviceTimeout))
                    {
                        // Old, gray it out
                        CTRE.Phoenix.dotNET.Form.FastUpdate.AssignIfDiff(oldListViewItem, System.Drawing.Color.Gray, System.Drawing.Color.LightGray);
                    }
                    else
                    {
                        // New
                        CTRE.Phoenix.dotNET.Form.FastUpdate.AssignIfDiff(oldListViewItem, System.Drawing.Color.Black, System.Drawing.Color.White);
                    }
                }
                else
                {
                    _mapDescriptors.Remove(oldListViewItem);
                    lstDevices.Items.RemoveAt(i);
                    /* i now points to next one */
                }
            }

            /* and finally look for anything new and add it */
            foreach (var dd in newDDs)
            {
                ListViewItem item;
                if (GetListViewItem(dd, out item))
                {
                    /* we already have a listitem for this dd */
                    FillListViewItem(item, dd);
                    _mapDescriptors[item] = dd;
                }
                else
                {
                    /* we don't have a listitem for this dd, add it */
                    item = lstDevices.Items.Add(CreateListViewItem(dd));
                    _mapDescriptors[item] = dd;
                }
            }

            /* re-select the selected one if available */
            if (oldSelection == null)
            {
                //SelectDevice(null); /* call selectdevice with null if you want to auto select first device */
            }
            else
            {
                SelectDevice(oldSelection);
            }
        }

        bool GetSelectedDescriptor(out DeviceDescrip descriptor)
        {
            return GetDescriptor(GetSelectedDevice(), out descriptor);
        }
        private ListViewItem GetSelectedDevice()
        {
            var list = lstDevices.SelectedItems;
            if (list.Count == 0) return null;
            return list[0];
        }
        public void ForcedRefresh()
        {
            if (lstDevices.SelectedItems.Count == 0)
            {
                /* no device is selected */
            }
            else
            {
                /* get the selected device */
                ListViewItem sel = lstDevices.SelectedItems[0];
                /* clear it */
                sel.Selected = false;
                /* reset it to force a full GUI refresh */
                sel.Selected = true;
            }
        }
        public void SelectedIndexChanged(object sender, EventArgs e)
        {
            DeviceDescrip dd = null;

            if (lstDevices.SelectedItems.Count > 0)
            {
                ListViewItem sel = lstDevices.SelectedItems[0];
                /* update any gui elements that depend on selection */
                dd = Lookup(sel);
            }


            if (SelectionChanged != null)
            {
                /* update config panels */
                SelectionChanged(this, e, dd);
            }
        }

        public DeviceDescrip SelectedDeviceDescriptor
        {
            get
            {
                DeviceDescrip retval = null;
                if (lstDevices.SelectedItems.Count > 0)
                {
                    if (GetDescriptor(lstDevices.SelectedItems[0], out retval))
                    {
                        /* do nothing, just return null */
                    }
                }
                return retval;
            }
        }
    }
}
