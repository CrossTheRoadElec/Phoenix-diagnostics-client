using System;
using System.Windows.Forms;

namespace CTRE.Phoenix.dotNET.Form
{
    public class ToggleButton
    {
        Button _button;
        string _notPressedText;
        string _pressedText;
        bool _isPressed;

        public ToggleButton(Button button, bool IsPressed, string notPressedText, string pressedText)
        {
            _button = button;
            _notPressedText = notPressedText;
            _pressedText = pressedText;
            _isPressed = IsPressed;
            Render();
        }
        public void Toggle()
        {
            _isPressed = !_isPressed;
            Render();
        }
        public bool IsPressed
        {
            get
            {
                return _isPressed;
            }
            set
            {
                _isPressed = value;
                Render();
            }
        }
        private void Render()
        {
            if (_isPressed)
            {
                _button.Text = _pressedText;
            }
            else
            {
                _button.Text = _notPressedText;
            }
        }
    }
}
