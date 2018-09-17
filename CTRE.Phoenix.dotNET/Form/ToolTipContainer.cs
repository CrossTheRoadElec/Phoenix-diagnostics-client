using System.Windows.Forms;

namespace CTRE.Phoenix.dotNET.Form
{
    /// <summary>
    /// General toop tip handler that will lazyily update the popup contents
    /// when the user overs over the control (specified in the c'tor).
    /// </summary>
    public class ToolTipContainer
    {
        private System.Windows.Forms.ToolTip _toolTip = new System.Windows.Forms.ToolTip();

        private string _toolTipText = null;

        private Control _control = null;

        public ToolTipContainer(Control control)
        {
            _control = control;
        }
        private bool IsDiff(string s1, string s2)
        {
            if (s1 == null && s2 != null)
                return true;
            if (s1 != null && s2 == null)
                return true;
            if (s1 == s2) /* same string ref or both null */
                return false;
            return s1.CompareTo(s2) != 0;
        }
        public void SetText(string text)
        {
            if (IsDiff(_toolTipText, text))
            {
                _toolTipText = text;
                if (_toolTipText == null)
                    _toolTip.RemoveAll();
                else if (_toolTipText.Length == 0)
                    _toolTip.RemoveAll();
                else
                {
                    /* update the hover text and rearm the delay so we 
                     * hover as long as the API will allow */
                    _toolTip.SetToolTip(_control, _toolTipText);
                    _toolTip.AutoPopDelay = 32767;
                }
            }
            else
            {
                /* nothing to do */
            }
        }
    }
}
