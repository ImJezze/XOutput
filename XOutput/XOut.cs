using SlimDX.DirectInput;
using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace XOutput
{
    public partial class XOut : Form
    {

        public XOut()
        {
            InitializeComponent();
            tipLabel.Text = "";
            this.controllerList.ItemCheck += (sender, e) => enabledChanged(e.Index, e.NewValue);
        }

        private void XOut_Load(object sender, EventArgs e)
        {
            controllerManager = new ControllerManager(this);
        }

        private void XOut_Shown(object sender, EventArgs e)
        {
            UpdateInfo(controllerManager.detectControllers());
        }

        private void XOut_Closing(object sender, FormClosingEventArgs e)
        {
            if (controllerManager.IsActive)
            {
                controllerManager.Stop();
            }
            notifyIcon.Visible = false;
        }

        private void StartStopBtn_Click(object sender, EventArgs e)
        {
            if (StartStopBtn.Text == "Start")
            {
                if (controllerManager.Start())
                {
                    StartStopBtn.Text = "Stop";
                    controllerList.Enabled = false;
                    isExclusive.Enabled = false;
                    notifyIcon.Text = ("XOutput\nEmulating " + controllerManager.pluggedDevices + " device(s).");
                    notifyIcon.Icon = Properties.Resources.AppIcon;
                }
            }
            else
            {
                if (controllerManager.Stop())
                {
                    StartStopBtn.Text = "Start";
                    controllerList.Enabled = true;
                    isExclusive.Enabled = true;
                    notifyIcon.Text = ("XOutput");
                    notifyIcon.Icon = Properties.Resources.AppIconInactive;
                }
            }
        }

        private void UpdateInfo(List<ControllerDevice> dev)
        {
            if (controllerList.Items.Count > dev.Count)
            {
                for (int i = 0; i < (controllerList.Items.Count - dev.Count); i++) controllerList.Items.RemoveAt(controllerList.Items.Count - 1 + i);
            }

            for (int i = 0; i < dev.Count; i++)
            {
                if (dev[i] != null)
                {
                    if (controllerList.Items.Count > i)
                    {
                        controllerList.Items.RemoveAt(i);
                    }
                    controllerList.Items.Insert(i, (i + 1).ToString() + ": " + dev[i].name + " (" + dev[i].joystick.Information.InstanceGuid + ")");
                    controllerList.SetItemChecked(i, dev[i].enabled);
                }
                else
                {
                    if (controllerList.Items.Count > i)
                    {
                        controllerList.Items.RemoveAt(i);
                    }
                }
            }
        }

        private void enabledChanged(int i, CheckState st)
        {
            bool enable = true;
     
            switch (st)
            {
                case CheckState.Checked:
                    enable = true;
                    break;
                case CheckState.Unchecked:
                    enable = false;
                    break;
                default:
                    break;
            }
            controllerManager.setControllerEnable(i, enable);

            Console.WriteLine("Controller {0} enabled: {1}", i, enable);
        }

        private void openOptions(int i)
        {
            if (i >= 0)
            {
                optionsWindow = new ControllerOptions(controllerManager.getController(i));
                optionsWindow.ShowDialog();
                optionsWindow.Activate();
                optionsWindow.FormClosed += (sender, e) => { optionsWindow = null; };
            }
        }

        private void XOut_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon.Visible = true;
                this.Hide();
            }
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon.Visible = false;
        }

        protected override void WndProc(ref Message m)    //update controllers on device change
        {
            try
            {
                if (m.Msg == 0x0219)
                {
                    lock (this)
                    {
                        Console.WriteLine("Device change detected. Updating devices...");
                        UpdateInfo(controllerManager.detectControllers());
                    }
                }
            }
            catch { }

            base.WndProc(ref m);
        }

        private void isExclusive_CheckedChanged(object sender, EventArgs e)
        {
            controllerManager.isExclusive = !controllerManager.isExclusive;
        }

        private void controllerList_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && this.controllerList.IndexFromPoint(e.Location) >= 0)
            {
                int index = this.controllerList.IndexFromPoint(e.Location);
                controllerList.SetSelected(index, true);
                openOptions(index);
            }
        }

        private void settingsLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("control", "joy.cpl");
        }

        private void controllerList_MouseEnter(object sender, EventArgs e)
        {
            tipLabel.Text = "Right-click for options.";
        }

        private void controllerList_MouseLeave(object sender, EventArgs e)
        {
            tipLabel.Text = "";
        }
    }
}
