using SlimDX.DirectInput;
using System;
using System.Windows.Forms;

namespace XOutput
{
    public partial class XOut : Form
    {

        public XOut()
        {
            InitializeComponent();
            /*
            this.enabledOne.CheckedChanged += (sender, e) => enabledChanged(0);
            this.enabledTwo.CheckedChanged += (sender, e) => enabledChanged(1);
            this.enabledThree.CheckedChanged += (sender, e) => enabledChanged(2);
            this.enabledFour.CheckedChanged += (sender, e) => enabledChanged(3);
            */
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
        }

        private void StartStopBtn_Click(object sender, EventArgs e)
        {
            if (StartStopBtn.Text == "Start")
            {
                if (optionsWindow != null)
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    optionsWindow.Focus();
                    return;
                }
                if (controllerManager.Start())
                {
                    StartStopBtn.Text = "Stop";
                    for (int i = 0; i < 4; i++)
                    {
                        //checks[i].Enabled = false;
                        controllerList.Enabled = false;
                        isExclusive.Enabled = false;
                        /*
                        foreach (Control con in boxes[i].Controls)
                        {
                            con.Enabled = false;
                        }
                        */
                    }
                }
            }
            else
            {
                if (controllerManager.Stop())
                {
                    StartStopBtn.Text = "Start";
                    for (int i = 0; i < 4; i++)
                    {
                        //checks[i].Enabled = true;
                        controllerList.Enabled = true;
                        isExclusive.Enabled = true;
                        /*
                        foreach (Control con in boxes[i].Controls)
                        {
                            con.Enabled = true;
                        }
                        */
                    }
                }
            }
        }

        private void UpdateInfo(ControllerDevice[] dev)
        {
            for (int i = 0; i < 4; i++)
            {
                if (dev[i] != null)
                {
                    /*
                    boxes[i].Visible = true;
                    boxes[i].Text = (i + 1).ToString() + ": " + dev[i].name;
                    checks[i].Visible = true;
                    */
                    if (controllerList.Items.Count > i)
                    {
                        controllerList.Items.RemoveAt(i);
                    }
                    controllerList.Items.Insert(i, (i + 1).ToString() + ": " + dev[i].name);
                    controllerList.SetItemChecked(i, dev[i].enabled);
                    Console.WriteLine("Index {0} hinzugefügt", i);
                }
                else
                {
                    /*
                    boxes[i].Visible = false;
                    checks[i].Visible = false;
                    */
                    if (controllerList.Items.Count > i)
                    {
                        controllerList.Items.RemoveAt(i);
                        Console.WriteLine("Index {0} entfernt", i);
                    }
                }
            }
        }

        private void Swap(int i, int p)
        {
            bool s = checks[i - 1].Checked;
            checks[i- 1].Checked = checks[p - 1].Checked;
            checks[p - 1].Checked = s;
            controllerManager.Swap(i, p);
            
            UpdateInfo(controllerManager.detectControllers());
        }



        private void enabledChanged(int i, CheckState st)
        {
            /*
            boxes[i].Enabled = checks[i].Checked;
            controllerManager.setControllerEnable(i, checks[i].Checked);
            */
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
                if (optionsWindow == null)
                {
                    optionsWindow = new ControllerOptions(controllerManager.getController(i));
                    optionsWindow.Show();
                    optionsWindow.Activate();
                    optionsWindow.FormClosed += (sender, e) => { optionsWindow = null; };
                }
                else
                {
                    System.Media.SystemSounds.Asterisk.Play();
                    optionsWindow.Focus();
                }
            }
        }

        protected override void WndProc(ref Message m)
        {
            try
            {
                if (m.Msg == 0x0219)
                {
                    lock (this)
                    {
                        UpdateInfo(controllerManager.detectControllers());
                    }
                }
            }
            catch { }

            base.WndProc(ref m);
        }

        private void isExclusive_CheckedChanged(object sender, EventArgs e)
        {
            controllerManager.changeExclusive(!controllerManager.isExclusive);
        }

        private void controllerList_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void controllerList_MouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Right:
                    int index = this.controllerList.IndexFromPoint(e.Location);
                    controllerList.SetSelected(index, true);
                    openOptions(index);
                    break;
            }
        }
    }
}
