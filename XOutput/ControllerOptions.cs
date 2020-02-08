using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace XOutput
{
    public partial class ControllerOptions : Form
    {
        private enum ControllerInputs
        {
            ButtonA = 0,
            ButtonB = 1,
            ButtonX = 2,
            ButtonY = 3,
            ButtonHome = 4,
            ButtonStart = 5,
            ButtonBack = 6,
            DigitalUp = 7,
            DigitalDown = 8,
            DigitalLeft = 9,
            DigitalRight = 10,
            BumperLeft = 11,
            BumperRight = 12,
            TriggerLeft = 13,
            TriggerRight = 14,
            AnalogLeftButton = 15,
            AnalogLeftYPositive = 16,
            AnalogLeftYNegative = 17,
            AnalogLeftXPositive = 18,
            AnalogLeftXNegative = 19,
            AnalogRightButton = 20,
            AnalogRightYPositive = 21,
            AnalogRightYNegative = 22,
            AnalogRightXPositive = 23,
            AnalogRightXNegative = 24,
        }

        ControllerDevice dev;

        public ControllerOptions(ControllerDevice device)
        {
            InitializeComponent();

            this.Icon = Properties.Resources.AppIcon;
            
            dev = device;
            this.Text = $"{dev.name} ({dev.joystick.Information.InstanceGuid})";

            this.dropButtonA.Tag = ControllerInputs.ButtonA;
            this.dropButtonB.Tag = ControllerInputs.ButtonB;
            this.dropButtonX.Tag = ControllerInputs.ButtonX;
            this.dropButtonY.Tag = ControllerInputs.ButtonY;

            this.dropButtonHome.Tag = ControllerInputs.ButtonHome;
            this.dropButtonStart.Tag = ControllerInputs.ButtonStart;
            this.dropButtonBack.Tag = ControllerInputs.ButtonBack;

            this.dropDigitalUp.Tag = ControllerInputs.DigitalUp;
            this.dropDigitalDown.Tag = ControllerInputs.DigitalDown;
            this.dropDigitalLeft.Tag = ControllerInputs.DigitalLeft;
            this.dropDigitalRight.Tag = ControllerInputs.DigitalRight;

            this.dropBumperLeft.Tag = ControllerInputs.BumperLeft;
            this.dropBumperRight.Tag = ControllerInputs.BumperRight;
            this.dropTriggerLeft.Tag = ControllerInputs.TriggerLeft;
            this.dropTriggerRight.Tag = ControllerInputs.TriggerRight;

            this.dropAnalogLeftButton.Tag = ControllerInputs.AnalogLeftButton;
            this.dropAnalogLeftYPositive.Tag = ControllerInputs.AnalogLeftYPositive;
            this.dropAnalogLeftYNegative.Tag = ControllerInputs.AnalogLeftYNegative;
            this.dropAnalogLeftXPositive.Tag = ControllerInputs.AnalogLeftXPositive;
            this.dropAnalogLeftXNegative.Tag = ControllerInputs.AnalogLeftXNegative;

            this.dropAnalogRightButton.Tag = ControllerInputs.AnalogRightButton;
            this.dropAnalogRightYPositive.Tag = ControllerInputs.AnalogRightYPositive;
            this.dropAnalogRightYNegative.Tag = ControllerInputs.AnalogRightYNegative;
            this.dropAnalogRightXPositive.Tag = ControllerInputs.AnalogRightXPositive;
            this.dropAnalogRightXNegative.Tag = ControllerInputs.AnalogRightXNegative;

            foreach (GroupBox g in this.Controls.OfType<GroupBox>())
            {
                foreach (MultiLevelComboBox m in g.Controls.OfType<MultiLevelComboBox>())
                {
                    //Tag structure: [Type, Number, Index]
                    byte index = (byte)(ControllerInputs)m.Tag;
                    m.Items[0] = GetBindingText(index); //Change combobox text according to saved binding
                    m.addOption("Disabled",
                        tag: new byte[] { 255, 0, index });
                    m.addOption("Detect",
                        tag: new byte[] { 254, 0, index });
                    ToolStripMenuItem axes = m.addMenu("Axes");
                    ToolStripMenuItem buttons = m.addMenu("Buttons");
                    ToolStripMenuItem dpads = m.addMenu("D-Pads");
                    ToolStripMenuItem incaxes = m.addMenu("+ Axes", axes);
                    ToolStripMenuItem decaxes = m.addMenu("- Axes", axes);
                    for (int i = 1; i <= dev.joystick.Capabilities.ButtonCount; i++)
                    {
                        m.addOption("Button " + i.ToString(), buttons,
                            new byte[] { 0, (byte)(i - 1), index });            //example: type 0 (button), subtype i - 1 (number of button), ind index
                    }                                                               //since the types start with powers of two, the four leftmost bytes contain the main type
                    for (int i = 1; i <= dev.joystick.Capabilities.PovCount; i++)   //the four rightmost types then contain the subtype which is (type - maintype)
                    {
                        m.addOption("D-Pad " + i.ToString() + " Up", dpads,
                            new byte[] { 32, (byte)(i - 1), index });
                        m.addOption("D-Pad " + i.ToString() + " Down", dpads,
                            new byte[] { 33, (byte)(i - 1), index });
                        m.addOption("D-Pad " + i.ToString() + " Left", dpads,
                            new byte[] { 34, (byte)(i - 1), index });
                        m.addOption("D-Pad " + i.ToString() + " Right", dpads,
                            new byte[] { 35, (byte)(i - 1), index });
                    }
                    for (int i = 1; i <= dev.analogs.Length; i++)
                    {
                        if (dev.analogs[i - 1] != 0)
                        {
                            m.addOption("+ Axis " + i.ToString(), incaxes,
                                new byte[] { 16, (byte)(i - 1), index });
                            m.addOption("- Axis " + i.ToString(), decaxes,
                                new byte[] { 17, (byte)(i - 1), index });
                        }
                    }
                    for (int i = 1; i <= dev.sliders.Length; i++)   //placeholder
                    {
                        if (dev.sliders[i - 1] != 0)
                        {
                            m.addOption("+ Slider " + i.ToString(), incaxes,
                                new byte[] { 48, (byte)(i - 1), index });
                            m.addOption("- Slider " + i.ToString(), decaxes,
                                new byte[] { 49, (byte)(i - 1), index });
                        }
                    }
                    m.SelectionChangeCommitted += (sender, e) => ComboBox_SelectionChangeCommitted(sender, e, m);
                    m.KeyPress += (sender, e) => ComboBox_KeyPress(sender, e);
                    m.MouseUp += (sender, e) => ComboBox_MouseUp(sender, e);
                    m.MouseEnter += (sender, e) => ComboBox_MouseEnter(sender, e);
                    m.MouseLeave += (sender, e) => ComboBox_MouseLeave(sender, e);
                }
            }
        }

        private string GetBindingText(byte index)
        {
            if (dev.mapping[index * 2] == 255)
            {
                return "Disabled";
            }

            byte subType = (byte)(dev.mapping[index * 2] & 0x0F);   //0F are the four rightmost bits
            byte type = (byte)((dev.mapping[index * 2] & 0xF0) >> 4);   //F0 are the four leftmost bits (this line takes the four leftmost bits and assigns them to the four rightmost bits)
            byte num = (byte)(dev.mapping[(index * 2) + 1] + 1);
            string[] typeString = new string[] { "Button {0}", "{1}Axis {0}", "D-Pad {0} {2}", "{1}Slider {0}" };
            string[] axesString = new string[] { "+ ", "- ", "", "" };
            string[] dpadString = new string[] { "Up", "Down", "Left", "Right" };

            return string.Format(typeString[type], num, axesString[subType], dpadString[subType]);
        }

        private byte[] DetectInput(byte index)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            SlimDX.DirectInput.JoystickState jState = dev.joystick.GetCurrentState();
            bool input = false;
            byte[] b = new byte[] { 0, 0, 0 };

            int[] axisNeutral = new int[] { jState.X, jState.Y, jState.Z, jState.RotationX, jState.RotationY, jState.RotationZ }; //gets the current axis positions and assumes them as neutral
            int[] slidersNeutral = new int[] { jState.GetSliders()[0], jState.GetSliders()[1] };

            while (!input)
            {
                jState = dev.joystick.GetCurrentState();
                bool[] buttons = jState.GetButtons(); //return bool for every button as array
                int[] pov = jState.GetPointOfViewControllers();  //returns int for every pov position as array
                int[] axis = new int[] { jState.X, jState.Y, jState.Z, jState.RotationX, jState.RotationY, jState.RotationZ }; //returns int for every slider position as array
                int[] sliders = new int[] { jState.GetSliders()[0], jState.GetSliders()[1] };

                int i;
                i = 0;
                while (i < buttons.Length && !input)
                {
                    if (buttons[i])
                    {
                        input = true;
                        b[0] = 0;
                        b[1] = (byte)i;
                        b[2] = index;
                        Console.WriteLine("Binding button {0} to index {1}", i + 1, b[2]);
                        return b;
                    }
                    i++;
                }

                i = 0;
                while (i < axis.Length && !input)
                {
                    // adding a deadzone, just to make sure user really presses the right direction
                    if (axis[i] > axisNeutral[i] + 15000)
                    {
                        input = true;
                        b[0] = 16;
                        b[1] = (byte)i;
                        b[2] = index;
                        Console.WriteLine("Binding + axis {0} to index {1}", i + 1, b[2]);
                        return b;
                    }
                    // adding a deadzone, just to make sure user really presses the right direction
                    else if (axis[i] < axisNeutral[i] - 15000)
                    {   
                        input = true;
                        b[0] = 17;
                        b[1] = (byte)i;
                        b[2] = index;
                        Console.WriteLine("Binding - axis {0} to index {1}", i + 1, b[2]);
                        return b;
                    }
                    i++;
                }

                i = 0;
                while (i < sliders.Length && !input)
                {
                    if (sliders[i] > slidersNeutral[i] + 15000)
                    {
                        input = true;
                        b[0] = 48;
                        b[1] = (byte)i;
                        b[2] = index;
                        Console.WriteLine("Binding + slider {0} to index {1}", i + 1, b[2]);
                        return b;
                    }
                    else if (sliders[i] < slidersNeutral[i] - 15000)
                    {
                        input = true;
                        b[0] = 49;
                        b[1] = (byte)i;
                        b[2] = index;
                        Console.WriteLine("Binding - slider {0} to index {1}", i + 1, b[2]);
                        return b;
                    }
                    i++;
                }

                i = 0;
                while (i < pov.Length && !input)
                {
                    {
                        switch(pov[i])
                        {
                            case 0:
                                input = true;
                                b[0] = 32;
                                b[1] = (byte)i;
                                b[2] = index;
                                Console.WriteLine("Binding DPad Up {0} to index {1}", i + 1, b[2]);
                                return b;
                            case 9000:
                                input = true;
                                b[0] = 35;
                                b[1] = (byte)i;
                                b[2] = index;
                                Console.WriteLine("Binding DPad Right {0} to index {1}", i + 1, b[2]);
                                return b;
                            case 18000:
                                input = true;
                                b[0] = 33;
                                b[1] = (byte)i;
                                b[2] = index;
                                Console.WriteLine("Binding DPad Down {0} to index {1}", i + 1, b[2]);
                                return b;
                            case 27000:
                                input = true;
                                b[0] = 34;
                                b[1] = (byte)i;
                                b[2] = index;
                                Console.WriteLine("Binding DPad Left {0} to index {1}", i + 1, b[2]);
                                return b;
                        }
                    }
                    i++;
                }

                if (sw.ElapsedMilliseconds > 2000 )
                {
                    b[0] = 127; // this type defines an unsuccessful detection
                    b[2] = index;
                    break;
                }
            }
            return b;
        }

        private void ComboBox_SelectionChangeCommitted(object sender, EventArgs e, MultiLevelComboBox m)
        {
            ToolStripMenuItem i = (ToolStripMenuItem)sender;

            byte[] b = (byte[])i.Tag; // store selection tag in array (b[0] = type, b[1] = number, b[2] = index
            if (b[0] == 254)
            {
                assignDetectedInput(b[2], m);
            }
            else
            {
                // type and subtype are stored at every even number
                dev.mapping[b[2] * 2] = b[0];
                // number of the control is stored at every uneven number above the corresponding even number
                dev.mapping[(b[2] * 2) + 1] = b[1];
                dev.Save();
            }
        }

        private void ComboBox_MouseUp(object sender, MouseEventArgs e)
        {
            MultiLevelComboBox m = (MultiLevelComboBox)sender;
            if (e.Button == MouseButtons.Right)
            {
                byte ind = (byte)(int)m.Tag;
                assignDetectedInput(ind, m);
            }
        }

        private async void assignDetectedInput(byte index, MultiLevelComboBox m)
        {
            this.Enabled = false;
            m.Items[0] = "Input...";
            byte[] b = await Task.Run(() => DetectInput(index));  //run actual detection asynchronous
            //detectInput returns b[0] = 127 if no input was made
            if (b[0] != 127)
            {
                dev.mapping[b[2] * 2] = b[0];
                dev.mapping[(b[2] * 2) + 1] = b[1];
                dev.Save();
            }
            m.Items[0] = GetBindingText(b[2]);
            this.Enabled = true;
        }

        private void resetProfile()
        {
            foreach (GroupBox g in this.Controls.OfType<GroupBox>().OrderBy(g => g.TabIndex))
            {
                foreach (MultiLevelComboBox m in g.Controls.OfType<MultiLevelComboBox>())
                {
                    byte[] b = { 255, 0, (byte)(int)m.Tag };
                    dev.mapping[b[2] * 2] = b[0];
                    dev.mapping[(b[2] * 2) + 1] = b[1];
                    dev.Save();
                    m.Items[0] = GetBindingText(b[2]);
                }
            }
        }

        private void ComboBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;   //prevents the dropdown lists from being edited
        }

        private void ComboBox_MouseEnter(object sender, EventArgs e)
        {
            hintLabel.Text = "Right-click to autodetect input.";
        }

        private void ComboBox_MouseLeave(object sender, EventArgs e)
        {
            hintLabel.Text = "";
        }

        private void onClose(object sender, EventArgs e) {
            dev.Save();
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("ALL settings in this profile will be lost. Are you sure?", "Reset Profile", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                resetProfile();
            }
        }

        private void ControllerOptions_Load(object sender, EventArgs e)
        {

        }

        private void labelLeft_Click(object sender, EventArgs e)
        {

        }
    }
}
