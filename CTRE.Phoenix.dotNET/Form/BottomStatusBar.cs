using System;
using System.Drawing;
using CTRE.Phoenix.dotNET.Form;

namespace CTRE.Phoenix.dotNET.Form
{
    /// <summary>
    /// Wrapper for bottom status bar with four text panels.
    /// </summary>
    public class BottomStatusBar
    {
        System.Windows.Forms.ToolStripStatusLabel[] _labels = new System.Windows.Forms.ToolStripStatusLabel[4];

        ToolTipContainer _tooltip;

        public BottomStatusBar(System.Windows.Forms.StatusStrip statusStrip,
                                System.Windows.Forms.ToolStripStatusLabel bottomLeft,
                                System.Windows.Forms.ToolStripStatusLabel bottomMiddleLeft,
                                System.Windows.Forms.ToolStripStatusLabel bottomMiddleRight, 
                                System.Windows.Forms.ToolStripStatusLabel bottomRight)
        {
            _labels[0] = bottomLeft;
            _labels[1] = bottomMiddleLeft;
            _labels[2] = bottomMiddleRight;
            _labels[3] = bottomRight;
            _labels[3].Font = new Font(_labels[3].Font, FontStyle.Regular);

            foreach (var label in _labels)
            {
                if (label != null)
                {
                    label.Text = String.Empty;
                }
            }

            _tooltip = new CTRE.Phoenix.dotNET.Form.ToolTipContainer(statusStrip);
        }
        public void SetHoverString(String hoverMsg)
        {
            _tooltip.SetText(hoverMsg);
        }
        public void PrintLeft(Color col, String text)
        {
            _labels[0].ForeColor = col;
            _labels[0].Text = text;
        }
        public void PrintMiddleLeft(Color col, String text)
        {
            _labels[1].ForeColor = col;
            _labels[1].Text = text;
        }
        public void PrintMiddleRight(Color col, String text)
        {
            _labels[2].ForeColor = col;
            _labels[2].Text = text;
        }
        public void PrintRight(Color col, String text)
        {
            _labels[3].ForeColor = col;
            _labels[3].Text = text;
        }
        public string GetMiddleRight()
        {
            return _labels[2].Text;
        }
    }
}
