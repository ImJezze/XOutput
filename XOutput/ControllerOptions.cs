using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using System.Windows;

namespace XOutput
{
    public partial class ControllerOptions : Form
    {
        ControllerDevice dev;
        public ControllerOptions(ControllerDevice device)
        {
            InitializeComponent();
            dev = device;
            this.Text = (dev.name + " (" + dev.joystick.Information.InstanceGuid + ")");
            hintLabel.Text = "";
            int ind = 0;

            foreach (GroupBox g in this.Controls.OfType<GroupBox>().OrderBy(g => g.TabIndex))
            {
                foreach (MultiLevelComboBox m in g.Controls.OfType<MultiLevelComboBox>().OrderBy(m => m.TabIndex))
                {
                    //Tag structure: [Type, Number, Index]
                    m.Items[0] = getBindingText(ind); //Change combobox text according to saved binding
                    m.Tag = ind;
                    m.addOption("Disabled",
                        tag: new byte[] { 255, 0, (byte)ind });
                    m.addOption("Detect",
                        tag: new byte[] { 254, 0, (byte)ind });
                    ToolStripMenuItem axes = m.addMenu("Axes");
                    ToolStripMenuItem buttons = m.addMenu("Buttons");
                    ToolStripMenuItem dpads = m.addMenu("D-Pads");
                    ToolStripMenuItem incaxes = m.addMenu("+ Axes", axes);
                    ToolStripMenuItem decaxes = m.addMenu("- Axes", axes);
                    for (int i = 1; i <= dev.joystick.Capabilities.ButtonCount; i++)
                    {
                        m.addOption("Button " + i.ToString(), buttons,
                            new byte[] { 0, (byte)(i - 1), (byte)ind });            //example: type 0 (button), subtype i - 1 (number of button), ind index
                    }                                                               //since the types start with powers of two, the four leftmost bytes contain the main type
                    for (int i = 1; i <= dev.joystick.Capabilities.PovCount; i++)   //the four rightmost types then contain the subtype which is (type - maintype)
                    {
                        m.addOption("D-Pad " + i.ToString() + " Up", dpads,
                            new byte[] { 32, (byte)(i - 1), (byte)ind });
                        m.addOption("D-Pad " + i.ToString() + " Down", dpads,
                            new byte[] { 33, (byte)(i - 1), (byte)ind });
                        m.addOption("D-Pad " + i.ToString() + " Left", dpads,
                            new byte[] { 34, (byte)(i - 1), (byte)ind });
                        m.addOption("D-Pad " + i.ToString() + " Right", dpads,
                            new byte[] { 35, (byte)(i - 1), (byte)ind });
                    }
                    for (int i = 1; i <= dev.analogs.Length; i++)
                    {
                        if (dev.analogs[i - 1] != 0)
                        {
                            m.addOption("+ Axis " + i.ToString(), incaxes,
                                new byte[] { 16, (byte)(i - 1), (byte)ind });
                            m.addOption("- Axis " + i.ToString(), decaxes,
                                new byte[] { 17, (byte)(i - 1), (byte)ind });
                        }
                    }
                    for (int i = 1; i <= dev.sliders.Length; i++)   //placeholder
                    {
                        if (dev.sliders[i - 1] != 0)
                        {
                            m.addOption("+ Slider " + i.ToString(), incaxes,
                                new byte[] { 48, (byte)(i - 1), (byte)ind });
                            m.addOption("- Slider " + i.ToString(), decaxes,
                                new byte[] { 49, (byte)(i - 1), (byte)ind });
                        }
                    }
                    m.SelectionChangeCommitted += (sender, e) => SelectionChanged(sender, e, m);
                    m.KeyPress += (sender, e) => comboBoxKeyPress(sender, e);
                    m.MouseUp += (sender, e) => comboBox_MouseUp(sender, e);
                    m.MouseEnter += (sender, e) => showHint(sender, e);
                    m.MouseLeave += (sender, e) => hideHint(sender, e);
                    ind++;
                }
            }
        }

        private string getBindingText(int i)
        {
            if (dev.mapping[i * 2] == 255) {
                return "Disabled";
            }
            byte subType = (byte)(dev.mapping[i * 2] & 0x0F);   //0F are the four rightmost bits
            byte type = (byte)((dev.mapping[i * 2] & 0xF0) >> 4);   //F0 are the four leftmost bits (this line takes the four leftmost bits and assigns them to the four rightmost bits)
            byte num = (byte)(dev.mapping[(i * 2) + 1] + 1);
            string[] typeString = new string[] { "Button {0}", "{1}Axis {0}", "D-Pad {0} {2}", "{1}Slider {0}" };
            string[] axesString = new string[] { "+ ", "- ", "", "" };
            string[] dpadString = new string[] { "Up", "Down", "Left", "Right" };
            return string.Format(typeString[type], num, axesString[subType], dpadString[subType]);
        }

        private byte[] detectInput(byte index) {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SlimDX.DirectInput.JoystickState jState = dev.joystick.GetCurrentState();
            bool input = false;
            byte[] b = new byte[] { (byte)0, (byte)0, (byte)0 };

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
                    if (axis[i] > axisNeutral[i] + 15000)   //adding a deadzone, just to make sure user really presses the right direction
                    {
                        input = true;
                        b[0] = 16;
                        b[1] = (byte)i;
                        b[2] = index;
                        Console.WriteLine("Binding + axis {0} to index {1}", i + 1, b[2]);
                        return b;
                    } else if (axis[i] < axisNeutral[i] - 15000) //adding a deadzone, just to make sure user really presses the right direction
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
                    b[0] = 127; //this type defines an unsuccessful detection
                    b[2] = index;
                    break;
                }
            }
            return b;
        }

        private void SelectionChanged(object sender, EventArgs e, MultiLevelComboBox m) {
            ToolStripMenuItem i = (ToolStripMenuItem)sender;
            byte[] b = (byte[])i.Tag;   //store selection tag in array (b[0] = type, b[1] = number, b[2] = index
            if (b[0] == 254)
            {
                assignDetectedInput(b[2], m);
            } else
            {
                dev.mapping[b[2] * 2] = b[0];           //type and subtype are stored at every even number
                dev.mapping[(b[2] * 2) + 1] = b[1];     //number of the control is stored at every uneven number above the corresponding even number
                dev.Save();
            }
        }

        private void comboBox_MouseUp(object sender, MouseEventArgs e)
        {
            MultiLevelComboBox m = (MultiLevelComboBox)sender;
            if (e.Button == MouseButtons.Right)
            {
                byte ind = (byte)(int)m.Tag;
                assignDetectedInput(ind, m);
            }
        }

        private async void assignDetectedInput(byte i, MultiLevelComboBox m)    //really neccessary? might remove
        {
            this.Enabled = false;
            m.Items[0] = "Input...";
            byte[] b = await Task.Run( () => detectInput(i) );  //run actual detection asynchronous
            if (b[0] != 127)    //detectInput returns b[0] = 127 if no input was made
            {
                dev.mapping[b[2] * 2] = b[0];
                dev.mapping[(b[2] * 2) + 1] = b[1];
                dev.Save();
            }
            m.Items[0] = getBindingText(b[2]);
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
                    m.Items[0] = getBindingText(b[2]);
                }
            }
        }

        private void comboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;   //prevents the dropdown lists from being edited
        }

        private void showHint(object sender, EventArgs e)
        {
            hintLabel.Text = "Right-click to autodetect input.";
        }

        private void hideHint(object sender, EventArgs e)
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
    }
}
