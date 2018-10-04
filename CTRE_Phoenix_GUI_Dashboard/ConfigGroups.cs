using System.Windows.Forms;
using Newtonsoft.Json;

namespace CTRE_Phoenix_GUI_Dashboard
{
    interface IControlGroup
    {
        Panel CreateLayout();
        void SetFromValues(object values, int ordinal);
        IControlGroup GetFromValues(GroupTabPage tab);
        void UpdateFromValues(GroupTabPage tab);
    }

    class GroupTabPage : TabPage
    {
        public IControlGroup group;
        public GroupTabPage(IControlGroup motorGroup) : base()
        {
            group = motorGroup;
        }
    }
    class MotorOutputGroup : IControlGroup
    {
        public enum eNeutralMode
        {
            Coast = 0,
            Brake = 1
        }

        public eNeutralMode? NeutralMode;

        public IControlGroup GetFromValues(GroupTabPage tab)
        {
            var allControls = ((TableLayoutPanel)((Panel)tab.Controls[0]).Controls[0]).Controls;
            foreach (Control c in allControls)
            {
                if (c is ComboBox)
                {
                    NeutralMode = (eNeutralMode)((ComboBox)c).SelectedItem;
                }
            }
            return this;
        }

        public void UpdateFromValues(GroupTabPage tab)
        {
            var allControls = ((TableLayoutPanel)((Panel)tab.Controls[0]).Controls[0]).Controls;
            foreach (Control c in allControls)
            {
                if (c is ComboBox)
                {
                    ((ComboBox)c).SelectedItem = NeutralMode;
                }
            }
        }

        public void SetFromValues(object values, int ordinal)
        {
            MotorOutputGroup newGroup = JsonConvert.DeserializeObject<MotorOutputGroup>(JsonConvert.SerializeObject(values));
            NeutralMode = newGroup.NeutralMode;
        }

        public Panel CreateLayout()
        {
            Label label = new Label();
            label.Text = "NeutralMode";
            label.Dock = DockStyle.Fill;

            ComboBox combo = new ComboBox();
            combo.Dock = DockStyle.Fill;
            combo.Items.Add(eNeutralMode.Coast);
            combo.Items.Add(eNeutralMode.Brake);
            combo.SelectedItem = NeutralMode;
            combo.DropDownStyle = ComboBoxStyle.DropDownList;

            TableLayoutPanel grid = new TableLayoutPanel();
            grid.RowCount = 1;
            grid.ColumnCount = 2;
            grid.Dock = DockStyle.Fill;
            grid.Controls.Add(label);
            grid.Controls.Add(combo);

            Panel ret = new Panel();
            ret.Dock = DockStyle.Fill;
            ret.Controls.Add(grid);
            return ret;
        }
    }

    class HardLimitSwitchGroup : IControlGroup
    {
        public enum eModeOfOperation
        {
            NormallyOpen = 0,
            NormallyClosed = 1,
            Disabled = 2
        }

        public eModeOfOperation? LimitSwitchForward;
        public eModeOfOperation? LimitSwitchReverse;

        public IControlGroup GetFromValues(GroupTabPage tab)
        {
            var allControls = ((TableLayoutPanel)((Panel)tab.Controls[0]).Controls[0]).Controls;
            int state = 0;
            foreach (Control c in allControls)
            {
                if (c is ComboBox)
                {
                    ComboBox combo = (ComboBox)c;
                    switch (state)
                    {
                        case 0:
                            //Forward limit switch
                            LimitSwitchForward = (eModeOfOperation)combo.SelectedItem;
                            state = 1;
                            break;
                        case 1:
                            LimitSwitchReverse = (eModeOfOperation)combo.SelectedItem;
                            break;
                    }
                }
            }
            return this;
        }

        public void UpdateFromValues(GroupTabPage tab)
        {
            var allControls = ((TableLayoutPanel)((Panel)tab.Controls[0]).Controls[0]).Controls;
            int state = 0;
            foreach (Control c in allControls)
            {
                if (c is ComboBox)
                {
                    ComboBox combo = (ComboBox)c;
                    switch (state)
                    {
                        case 0:
                            //Forward limit switch
                            combo.SelectedItem = LimitSwitchForward;
                            state = 1;
                            break;
                        case 1:
                            combo.SelectedItem = LimitSwitchReverse;
                            break;
                    }
                }
            }
        }

        public void SetFromValues(object values, int ordinal)
        {
            HardLimitSwitchGroup newGroup = JsonConvert.DeserializeObject<HardLimitSwitchGroup>(JsonConvert.SerializeObject(values));
            LimitSwitchForward = newGroup.LimitSwitchForward;
            LimitSwitchReverse = newGroup.LimitSwitchReverse;
        }

        public Panel CreateLayout()
        {
            Label forwardLabel = new Label();
            forwardLabel.Text = "Limit Switch Forward";
            forwardLabel.Dock = DockStyle.Fill;

            Label reverseLabel = new Label();
            reverseLabel.Text = "Limit Switch Reverse";
            reverseLabel.Dock = DockStyle.Fill;

            ComboBox forwardCombo = new ComboBox();
            forwardCombo.Dock = DockStyle.Fill;
            forwardCombo.Items.Add(eModeOfOperation.Disabled);
            forwardCombo.Items.Add(eModeOfOperation.NormallyOpen);
            forwardCombo.Items.Add(eModeOfOperation.NormallyClosed);
            forwardCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            forwardCombo.SelectedItem = LimitSwitchForward;

            ComboBox reverseCombo = new ComboBox();
            reverseCombo.Dock = DockStyle.Fill;
            reverseCombo.Items.Add(eModeOfOperation.Disabled);
            reverseCombo.Items.Add(eModeOfOperation.NormallyOpen);
            reverseCombo.Items.Add(eModeOfOperation.NormallyClosed);
            reverseCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            reverseCombo.SelectedItem = LimitSwitchReverse;

            TableLayoutPanel grid = new TableLayoutPanel();
            grid.RowCount = 2;
            grid.ColumnCount = 2;
            grid.Dock = DockStyle.Fill;
            grid.Controls.Add(forwardLabel);
            grid.Controls.Add(forwardCombo);
            grid.Controls.Add(reverseLabel);
            grid.Controls.Add(reverseCombo);

            Panel ret = new Panel();
            ret.Dock = DockStyle.Fill;
            ret.Controls.Add(grid);
            return ret;
        }
    }

    class SoftLimitSwitchGroup : IControlGroup
    {
        public bool? ForwardSoftLimitEnable;
        public bool? ReverseSoftLimitEnable;
        public float? SoftLimitForwardValue;
        public float? SoftLimitReverseValue;

        public IControlGroup GetFromValues(GroupTabPage tab)
        {
            var allControls = ((TableLayoutPanel)((Panel)tab.Controls[0]).Controls[0]).Controls;
            int checkState = 0;
            int numericState = 0;
            foreach (Control c in allControls)
            {
                if (c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    switch (checkState)
                    {
                        case 0:
                            //Forward limit switch
                            ForwardSoftLimitEnable = check.Checked;
                            checkState = 1;
                            break;
                        case 1:
                            ReverseSoftLimitEnable = check.Checked;
                            break;
                    }
                }
                if (c is NumericUpDown)
                {
                    NumericUpDown numeric = (NumericUpDown)c;
                    switch (numericState)
                    {
                        case 0:
                            //Forward limit switch
                            SoftLimitForwardValue = (float)numeric.Value;
                            numericState = 1;
                            break;
                        case 1:
                            SoftLimitReverseValue = (float)numeric.Value;
                            break;
                    }
                }
            }
            return this;
        }

        public void UpdateFromValues(GroupTabPage tab)
        {
            var allControls = ((TableLayoutPanel)((Panel)tab.Controls[0]).Controls[0]).Controls;
            int checkState = 0;
            int numericState = 0;
            foreach (Control c in allControls)
            {
                if (c is CheckBox)
                {
                    CheckBox check = (CheckBox)c;
                    switch (checkState)
                    {
                        case 0:
                            //Forward limit switch
                            check.Checked = ForwardSoftLimitEnable ?? false;
                            checkState = 1;
                            break;
                        case 1:
                            check.Checked = ReverseSoftLimitEnable ?? false;
                            break;
                    }
                }
                if (c is NumericUpDown)
                {
                    NumericUpDown numeric = (NumericUpDown)c;
                    switch (numericState)
                    {
                        case 0:
                            //Forward limit switch
                            numeric.Value = (decimal)(SoftLimitForwardValue ?? 0);
                            numericState = 1;
                            break;
                        case 1:
                            numeric.Value = (decimal)(SoftLimitReverseValue ?? 0);
                            break;
                    }
                }
            }
        }

        public void SetFromValues(object values, int ordinal)
        {
            SoftLimitSwitchGroup newGroup = JsonConvert.DeserializeObject<SoftLimitSwitchGroup>(JsonConvert.SerializeObject(values));
            ForwardSoftLimitEnable = newGroup.ForwardSoftLimitEnable;
            ReverseSoftLimitEnable = newGroup.ReverseSoftLimitEnable;
            SoftLimitForwardValue = newGroup.SoftLimitForwardValue;
            SoftLimitReverseValue = newGroup.SoftLimitReverseValue;
        }

        public Panel CreateLayout()
        {
            Label forwardEnableLabel = new Label();
            forwardEnableLabel.Text = "Forward Soft Limit Switch Enable";
            forwardEnableLabel.Dock = DockStyle.Fill;

            Label reverseEnableLabel = new Label();
            reverseEnableLabel.Text = "Reverse Soft Limit Switch Enable";
            reverseEnableLabel.Dock = DockStyle.Fill;

            Label forwardValueLabel = new Label();
            forwardValueLabel.Text = "Forward Soft Limit Switch Value";
            forwardValueLabel.Dock = DockStyle.Fill;

            Label reverseValueLabel = new Label();
            reverseValueLabel.Text = "Reverse Soft Limit Switch Value";
            reverseValueLabel.Dock = DockStyle.Fill;

            CheckBox forwardEnable = new CheckBox();
            forwardEnable.Dock = DockStyle.Fill;
            forwardEnable.Checked = ForwardSoftLimitEnable == true;

            CheckBox reverseEnable = new CheckBox();
            reverseEnable.Dock = DockStyle.Fill;
            reverseEnable.Checked = ReverseSoftLimitEnable == true;

            NumericUpDown forwardValue = new NumericUpDown();
            forwardValue.Minimum = decimal.MinValue;
            forwardValue.Maximum = decimal.MaxValue;
            forwardValue.DecimalPlaces = 5;
            forwardValue.Dock = DockStyle.Fill;
            forwardValue.Value = (decimal)SoftLimitForwardValue;

            NumericUpDown reverseValue = new NumericUpDown();
            reverseValue.Minimum = decimal.MinValue;
            reverseValue.Maximum = decimal.MaxValue;
            reverseValue.DecimalPlaces = 5;
            reverseValue.Dock = DockStyle.Fill;
            reverseValue.Value = (decimal)SoftLimitReverseValue;

            TableLayoutPanel grid = new TableLayoutPanel();
            grid.RowCount = 4;
            grid.ColumnCount = 2;
            grid.Dock = DockStyle.Fill;
            grid.Controls.Add(forwardEnableLabel);
            grid.Controls.Add(forwardEnable);
            grid.Controls.Add(reverseEnableLabel);
            grid.Controls.Add(reverseEnable);
            grid.Controls.Add(forwardValueLabel);
            grid.Controls.Add(forwardValue);
            grid.Controls.Add(reverseValueLabel);
            grid.Controls.Add(reverseValue);

            Panel ret = new Panel();
            ret.Dock = DockStyle.Fill;
            ret.Controls.Add(grid);
            return ret;
        }

    }

    class SlotGroup : IControlGroup
    {
        public int? SlotNumber;
        public float? pGain;
        public float? iGain;
        public float? dGain;
        public float? fGain;
        public float? iZone;
        public float? clRampRate;

        public IControlGroup GetFromValues(GroupTabPage tab)
        {
            var allControls = ((TableLayoutPanel)((Panel)tab.Controls[0]).Controls[0]).Controls;
            int state = 0;
            switch(tab.Text)
            {
                case "Slot 0":
                    SlotNumber = 0;
                    break;
                case "Slot 1":
                    SlotNumber = 1;
                    break;
                default:
                    SlotNumber = -1;
                    break;
            }
            foreach (Control c in allControls)
            {
                if (c is NumericUpDown)
                {
                    NumericUpDown numeric = (NumericUpDown)c;
                    switch (state)
                    {
                        case 0:
                            //Forward limit switch
                            pGain = (float)numeric.Value;
                            state = 1;
                            break;
                        case 1:
                            //Forward limit switch
                            iGain = (float)numeric.Value;
                            state = 2;
                            break;
                        case 2:
                            //Forward limit switch
                            dGain = (float)numeric.Value;
                            state = 3;
                            break;
                        case 3:
                            //Forward limit switch
                            fGain = (float)numeric.Value;
                            state = 4;
                            break;
                        case 4:
                            //Forward limit switch
                            iZone = (float)numeric.Value;
                            state = 5;
                            break;
                        case 5:
                            //Forward limit switch
                            clRampRate = (float)numeric.Value;
                            break;
                    }
                }
            }
            return this;
        }

        public void UpdateFromValues(GroupTabPage tab)
        {
            var allControls = ((TableLayoutPanel)((Panel)tab.Controls[0]).Controls[0]).Controls;
            int state = 0;
            foreach (Control c in allControls)
            {
                if (c is NumericUpDown)
                {
                    NumericUpDown numeric = (NumericUpDown)c;
                    switch (state)
                    {
                        case 0:
                            //Forward limit switch
                            numeric.Value = (decimal)(pGain ?? 0);
                            state = 1;
                            break;
                        case 1:
                            //Forward limit switch
                            numeric.Value = (decimal)(iGain ?? 0);
                            state = 2;
                            break;
                        case 2:
                            //Forward limit switch
                            numeric.Value = (decimal)(dGain ?? 0);
                            state = 3;
                            break;
                        case 3:
                            //Forward limit switch
                            numeric.Value = (decimal)(fGain ?? 0);
                            state = 4;
                            break;
                        case 4:
                            //Forward limit switch
                            numeric.Value = (decimal)(iZone ?? 0);
                            state = 5;
                            break;
                        case 5:
                            //Forward limit switch
                            numeric.Value = (decimal)(clRampRate ?? 0);
                            break;
                    }
                }
            }
        }

        public void SetFromValues(object values, int ordinal)
        {
            SlotGroup newGroup = JsonConvert.DeserializeObject<SlotGroup>(JsonConvert.SerializeObject(values));
            SlotNumber = newGroup.SlotNumber;
            pGain = newGroup.pGain;
            iGain = newGroup.iGain;
            dGain = newGroup.dGain;
            fGain = newGroup.fGain;
            iZone = newGroup.iZone;
            clRampRate = newGroup.clRampRate;
        }

        public Panel CreateLayout()
        {
            Label pLabel = new Label();
            pLabel.Text = "P Gain";
            pLabel.Dock = DockStyle.Fill;

            Label ilabel = new Label();
            ilabel.Text = "I Gain";
            ilabel.Dock = DockStyle.Fill;

            Label dLabel = new Label();
            dLabel.Text = "D Gain";
            dLabel.Dock = DockStyle.Fill;

            Label fLabel = new Label();
            fLabel.Text = "F Gain";
            fLabel.Dock = DockStyle.Fill;

            Label iZoneLabel = new Label();
            iZoneLabel.Text = "I Zone";
            iZoneLabel.Dock = DockStyle.Fill;

            Label clRampLabel = new Label();
            clRampLabel.Text = "Closed Loop Ramp Rate";
            clRampLabel.Dock = DockStyle.Fill;

            NumericUpDown p = new NumericUpDown();
            p.Minimum = decimal.MinValue;
            p.Maximum = decimal.MaxValue;
            p.DecimalPlaces = 5;
            p.Dock = DockStyle.Fill;
            p.Value = (decimal)pGain;

            NumericUpDown i = new NumericUpDown();
            i.Minimum = decimal.MinValue;
            i.Maximum = decimal.MaxValue;
            i.DecimalPlaces = 5;
            i.Dock = DockStyle.Fill;
            i.Value = (decimal)iGain;

            NumericUpDown d = new NumericUpDown();
            d.Minimum = decimal.MinValue;
            d.Maximum = decimal.MaxValue;
            d.DecimalPlaces = 5;
            d.Dock = DockStyle.Fill;
            d.Value = (decimal)dGain;

            NumericUpDown f = new NumericUpDown();
            f.Minimum = decimal.MinValue;
            f.Maximum = decimal.MaxValue;
            f.DecimalPlaces = 5;
            f.Dock = DockStyle.Fill;
            f.Value = (decimal)fGain;

            NumericUpDown iZ = new NumericUpDown();
            iZ.Minimum = decimal.MinValue;
            iZ.Maximum = decimal.MaxValue;
            iZ.Dock = DockStyle.Fill;
            iZ.Value = (decimal)iZone;

            NumericUpDown clRamp = new NumericUpDown();
            clRamp.Minimum = decimal.MinValue;
            clRamp.Maximum = decimal.MaxValue;
            p.DecimalPlaces = 5;
            clRamp.Dock = DockStyle.Fill;
            clRamp.Value = (decimal)clRampRate;


            TableLayoutPanel grid = new TableLayoutPanel();
            grid.RowCount = 6;
            grid.ColumnCount = 2;
            grid.Dock = DockStyle.Fill;
            grid.Controls.Add(pLabel);
            grid.Controls.Add(p);
            grid.Controls.Add(ilabel);
            grid.Controls.Add(i);
            grid.Controls.Add(dLabel);
            grid.Controls.Add(d);
            grid.Controls.Add(fLabel);
            grid.Controls.Add(f);
            grid.Controls.Add(iZoneLabel);
            grid.Controls.Add(iZ);
            grid.Controls.Add(clRampLabel);
            grid.Controls.Add(clRamp);

            Panel ret = new Panel();
            ret.Dock = DockStyle.Fill;
            ret.Controls.Add(grid);
            return ret;
        }
    }

}
