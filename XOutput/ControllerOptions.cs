using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace XOutput
{
    public partial class ControllerOptions : Form
    {
        ControllerDevice dev;
        public ControllerOptions(ControllerDevice device)
        {
            InitializeComponent();
            dev = device;
            this.Text = (dev.name + " (" + dev.joystick.Information.ProductGuid + ")");
            int ind = 0;

            foreach (MultiLevelComboBox m in this.Controls.OfType<MultiLevelComboBox>()) {
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
                ToolStripMenuItem iaxes = m.addMenu("Inverted Axes", axes);
                ToolStripMenuItem haxes = m.addMenu("Half Axes", axes);
                ToolStripMenuItem ihaxes = m.addMenu("Inverted Half Axes", axes);
                for (int i = 1; i <= dev.joystick.Capabilities.ButtonCount; i++)
                {
                    m.addOption("Button " + i.ToString(), buttons,
                        new byte[] { 0, (byte)(i - 1), (byte)ind });    //example: type 0 (button), number i - 1 (number of button), ind index
                }
                for (int i = 1; i <= dev.joystick.Capabilities.PovCount; i++)
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
                for (int i = 1; i <= dev.joystick.Capabilities.AxesCount; i++)
                {
                    m.addOption("Axis " + i.ToString(), axes,
                        new byte[] { 16, (byte)(i - 1), (byte)ind });
                    m.addOption("IAxis " + i.ToString(), iaxes,
                        new byte[] { 17, (byte)(i - 1), (byte)ind });
                    m.addOption("HAxis" + i.ToString(), haxes,
                        new byte[] { 18, (byte)(i - 1), (byte)ind });
                    m.addOption("IHAxis" + i.ToString(), ihaxes,
                        new byte[] { 19, (byte)(i - 1), (byte)ind });
                }
                //m.SelectionChangeCommitted += new System.EventHandler(SelectionChanged);
                m.SelectionChangeCommitted += (sender, e) => SelectionChanged(sender, e, m);
                m.KeyPress += (sender, e) => comboBoxKeyPress(sender, e);
                m.MouseUp += (sender, e) => comboBox_MouseUp(sender, e); 
                ind++;
            }
        }

        private string getBindingText(int i)
        {
            if (dev.mapping[i * 2] == 255) {
                return "Disabled";
            }
            byte subType = (byte)(dev.mapping[i * 2] & 0x0F);
            byte type = (byte)((dev.mapping[i * 2] & 0xF0) >> 4);
            byte num = (byte)(dev.mapping[(i * 2) + 1] + 1);
            string[] typeString = new string[] { "Button {0}", "{1}Axis {0}", "D-Pad {0} {2}" };
            string[] axesString = new string[] { "", "I", "H", "IH" };
            string[] dpadString = new string[] { "Up", "Down", "Left", "Right" };
            return string.Format(typeString[type], num, axesString[subType], dpadString[subType]);
        }

        private byte[] detectInput(byte index) {
            SlimDX.DirectInput.JoystickState devState = dev.joystick.GetCurrentState();
            bool input = false;
            byte[] inputByte = new byte[] { (byte)0, (byte)0, (byte)0 };

            int[] axisNeutral = new int[] { devState.X, devState.Y, devState.Z, devState.RotationX, devState.RotationY, devState.RotationZ }; //gets the current axis positions and assumes them as neutral
            while (!input)
            {
                devState = dev.joystick.GetCurrentState();
                bool[] buttons = devState.GetButtons(); //return bool for every button as array
                int[] pov = devState.GetPointOfViewControllers();  //returns int for every pov position as array
                int[] axis = new int[] { devState.X, devState.Y, devState.Z, devState.RotationX, devState.RotationY, devState.RotationZ }; //returns int for every slider position as array

                int n = 0;
                while (n < buttons.Length && !input)
                {
                    if (buttons[n])
                    {
                        input = true;
                        inputByte[0] = 0;
                        inputByte[1] = (byte)n;
                        inputByte[2] = index;
                        Console.WriteLine("Binding button {0} to index {1}", n + 1, inputByte[2]);
                    }
                    n++;
                }

                int j = 0;
                while (j < axis.Length && !input)
                {
                    if (axis[j] > axisNeutral[j] + 15000 || axis[j] < axisNeutral[j] - 15000)   //adding a deadzone, just to make sure user really presses the right direction
                    {
                        input = true;
                        inputByte[0] = 16;
                        inputByte[1] = (byte)j;
                        inputByte[2] = index;
                        Console.WriteLine("Binding axis {0} to index {1}", j + 1, inputByte[2]);
                    }
                    j++;
                }

                int k = 0;
                while (k < pov.Length && !input)
                {
                    {
                        switch(pov[k])
                        {
                            case 0:
                                input = true;
                                inputByte[0] = 32;
                                inputByte[1] = (byte)k;
                                inputByte[2] = index;
                                Console.WriteLine("Binding DPad Up {0} to index {1}", k + 1, inputByte[2]);
                                break;
                            case 9000:
                                input = true;
                                inputByte[0] = 35;
                                inputByte[1] = (byte)k;
                                inputByte[2] = index;
                                Console.WriteLine("Binding DPad Right {0} to index {1}", k + 1, inputByte[2]);
                                break;
                            case 18000:
                                input = true;
                                inputByte[0] = 33;
                                inputByte[1] = (byte)k;
                                inputByte[2] = index;
                                Console.WriteLine("Binding DPad Down {0} to index {1}", k + 1, inputByte[2]);
                                break;
                            case 27000:
                                input = true;
                                inputByte[0] = 34;
                                inputByte[1] = (byte)k;
                                inputByte[2] = index;
                                Console.WriteLine("Binding DPad Left {0} to index {1}", k + 1, inputByte[2]);
                                break;
                        }
                    }
                    k++;
                }
            }
            dev.mapping[inputByte[2] * 2] = inputByte[0];
            dev.mapping[(inputByte[2] * 2) + 1] = inputByte[1];
            dev.Save();
            return inputByte;
        }

        private void SelectionChanged(object sender, EventArgs e, MultiLevelComboBox m) {
            ToolStripMenuItem i = (ToolStripMenuItem)sender;
            byte[] b = (byte[])i.Tag;   //store selection tag in array (b[0] = type, b[1] = number, b[2] = index
            if (b[0] == 254)
            {
                m.Items[0] = "Input...";
                BeginInvoke(new Action(() => assignDetectedInput(b[2], m)));
            } else
            {
                dev.mapping[b[2] * 2] = b[0];
                dev.mapping[(b[2] * 2) + 1] = b[1];
                dev.Save();
            }
        }

        private void comboBox_MouseUp(object sender, MouseEventArgs e)
        {
            MultiLevelComboBox m = (MultiLevelComboBox)sender;
            if (e.Button == MouseButtons.Right)
            {
                byte ind = (byte)(int)m.Tag;
                //byte[] b = new byte[3];
                //byte[] b = detectInput(ind);
                m.Items[0] = "Input...";
                BeginInvoke(new Action(() => assignDetectedInput(ind, m)));
                //m.Items[0] = getBindingText(b[2]);
            }
        }

        private void assignDetectedInput(byte i, MultiLevelComboBox m)
        {
            byte[] b = detectInput(i);
            m.Items[0] = getBindingText(b[2]);
        }

        private void comboBoxKeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;   //prevents the dropdown lists from being edited
        }

        private void onClose(object sender, EventArgs e) {
            dev.Save();
        }

    }
}
