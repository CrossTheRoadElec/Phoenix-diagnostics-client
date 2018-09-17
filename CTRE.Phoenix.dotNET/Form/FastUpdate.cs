using System;
using System.Windows.Forms;

namespace CTRE.Phoenix.dotNET.Form
{
    public static class FastUpdate
    {
        //--------------- Data types with lazy updating, great for reducing excessive GUI updates ----------//

        public class Float : ChangingValue<float> { }
        public class Bool : ChangingValue<bool> { }
        public class TimeSpan : ChangingValue<TimeSpan> { }

        /**
		 * Helper classes to track when values actually change.
		 */
        public class ChangingValue<T>
        {
            T _value;
            bool _changed;

            public bool Changed
            {
                get
                {
                    return _changed;
                }
            }

            public T Value
            {
                get { return _value; }
                set
                {
                    if (!_value.Equals(value))
                        _changed = true;
                    else
                        _changed = false;
                    _value = value;
                }
            }
        }

        //--------------- Static routines for lazy updating ----------//
        public static void AssignIfDiff(ListViewItem.ListViewSubItem lhs, String rhs)
        {
            if (lhs.Text.Equals(rhs)) { }
            else { lhs.Text = rhs; }
        }

        public static void AssignIfDiff(System.Windows.Forms.Label lhs, String rhs)
        {
            if (lhs.Text.Equals(rhs)) { }
            else { lhs.Text = rhs; }
        }
        public static void AssignIfDiff(Panel lhs, System.Drawing.Color rhs)
        {
            if (lhs.BackColor.Equals(rhs)) { }
            else { lhs.BackColor = rhs; }
        }
        public static void AssignIfDiff(System.Windows.Forms.GroupBox lhs, String rhs)
        {
            if (lhs.Text.Equals(rhs)) { }
            else { lhs.Text = rhs; }
        }
        public static void AssignIfDiff(ListViewItem lhs, System.Drawing.Color fhs, System.Drawing.Color rhs)
        {
            if (lhs.BackColor.Equals(rhs) && lhs.ForeColor.Equals(fhs)) { }
            else { lhs.BackColor = rhs; lhs.ForeColor = fhs; }
        }
    }
}
