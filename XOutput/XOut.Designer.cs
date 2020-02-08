namespace XOutput
{
    partial class XOut
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.startStopButton = new System.Windows.Forms.Button();
            this.exclusiveCheckBox = new System.Windows.Forms.CheckBox();
            this.controllerList = new System.Windows.Forms.CheckedListBox();
            this.settingsLabel = new System.Windows.Forms.LinkLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.hintLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startStopButton
            // 
            this.startStopButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startStopButton.Location = new System.Drawing.Point(409, 182);
            this.startStopButton.Name = "startStopButton";
            this.startStopButton.Size = new System.Drawing.Size(75, 23);
            this.startStopButton.TabIndex = 0;
            this.startStopButton.Text = "Start";
            this.startStopButton.UseVisualStyleBackColor = true;
            this.startStopButton.Click += new System.EventHandler(this.StartStopButton_Click);
            // 
            // exclusiveCheckBox
            // 
            this.exclusiveCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exclusiveCheckBox.Location = new System.Drawing.Point(12, 187);
            this.exclusiveCheckBox.Name = "exclusiveCheckBox";
            this.exclusiveCheckBox.Size = new System.Drawing.Size(75, 17);
            this.exclusiveCheckBox.TabIndex = 9;
            this.exclusiveCheckBox.Text = "Exclusivity";
            this.exclusiveCheckBox.UseVisualStyleBackColor = true;
            this.exclusiveCheckBox.CheckedChanged += new System.EventHandler(this.ExclusiveCheckBox_CheckedChanged);
            // 
            // controllerList
            // 
            this.controllerList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controllerList.CheckOnClick = true;
            this.controllerList.FormattingEnabled = true;
            this.controllerList.IntegralHeight = false;
            this.controllerList.Location = new System.Drawing.Point(12, 12);
            this.controllerList.Name = "controllerList";
            this.controllerList.Size = new System.Drawing.Size(472, 156);
            this.controllerList.TabIndex = 10;
            this.controllerList.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.ControllerList_ItemCheck);
            this.controllerList.MouseEnter += new System.EventHandler(this.ControllerList_MouseEnter);
            this.controllerList.MouseLeave += new System.EventHandler(this.ControllerList_MouseLeave);
            this.controllerList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ControllerList_MouseUp);
            // 
            // settingsLabel
            // 
            this.settingsLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.settingsLabel.Location = new System.Drawing.Point(93, 188);
            this.settingsLabel.Name = "settingsLabel";
            this.settingsLabel.Size = new System.Drawing.Size(73, 13);
            this.settingsLabel.TabIndex = 11;
            this.settingsLabel.TabStop = true;
            this.settingsLabel.Text = "Control Panel";
            this.settingsLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SettingsLabel_LinkClicked);
            // 
            // notifyIcon
            // 
            this.notifyIcon.Text = "XOutput";
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // hintLabel
            // 
            this.hintLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hintLabel.BackColor = System.Drawing.SystemColors.Control;
            this.hintLabel.ForeColor = System.Drawing.SystemColors.GrayText;
            this.hintLabel.Location = new System.Drawing.Point(172, 187);
            this.hintLabel.Name = "hintLabel";
            this.hintLabel.Size = new System.Drawing.Size(231, 13);
            this.hintLabel.TabIndex = 12;
            // 
            // XOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(496, 217);
            this.Controls.Add(this.hintLabel);
            this.Controls.Add(this.settingsLabel);
            this.Controls.Add(this.controllerList);
            this.Controls.Add(this.exclusiveCheckBox);
            this.Controls.Add(this.startStopButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "XOut";
            this.Text = "XOutput";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XOut_Closing);
            this.Load += new System.EventHandler(this.XOut_Load);
            this.Shown += new System.EventHandler(this.XOut_Shown);
            this.Resize += new System.EventHandler(this.XOut_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startStopButton;
        
        private ControllerOptions optionsWindow;
        private ControllerManager controllerManager;
        private System.Windows.Forms.CheckBox exclusiveCheckBox;
        private System.Windows.Forms.CheckedListBox controllerList;
        private System.Windows.Forms.LinkLabel settingsLabel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Label hintLabel;
    }
}