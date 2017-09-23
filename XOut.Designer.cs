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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(XOut));
            this.StartStopBtn = new System.Windows.Forms.Button();
            this.isExclusive = new System.Windows.Forms.CheckBox();
            this.controllerList = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // StartStopBtn
            // 
            this.StartStopBtn.Location = new System.Drawing.Point(397, 316);
            this.StartStopBtn.Name = "StartStopBtn";
            this.StartStopBtn.Size = new System.Drawing.Size(75, 23);
            this.StartStopBtn.TabIndex = 0;
            this.StartStopBtn.Text = "Start";
            this.StartStopBtn.UseVisualStyleBackColor = true;
            this.StartStopBtn.Click += new System.EventHandler(this.StartStopBtn_Click);
            // 
            // isExclusive
            // 
            this.isExclusive.AutoSize = true;
            this.isExclusive.Location = new System.Drawing.Point(12, 320);
            this.isExclusive.Name = "isExclusive";
            this.isExclusive.Size = new System.Drawing.Size(75, 17);
            this.isExclusive.TabIndex = 9;
            this.isExclusive.Text = "Exclusivity";
            this.isExclusive.UseVisualStyleBackColor = true;
            this.isExclusive.CheckedChanged += new System.EventHandler(this.isExclusive_CheckedChanged);
            // 
            // controllerList
            // 
            this.controllerList.CheckOnClick = true;
            this.controllerList.FormattingEnabled = true;
            this.controllerList.Location = new System.Drawing.Point(12, 12);
            this.controllerList.Name = "controllerList";
            this.controllerList.Size = new System.Drawing.Size(460, 289);
            this.controllerList.TabIndex = 10;
            this.controllerList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.controllerList_MouseUp);
            // 
            // XOut
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.controllerList);
            this.Controls.Add(this.isExclusive);
            this.Controls.Add(this.StartStopBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "XOut";
            this.Text = "XOutput";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.XOut_Closing);
            this.Load += new System.EventHandler(this.XOut_Load);
            this.Shown += new System.EventHandler(this.XOut_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartStopBtn;

        private System.Windows.Forms.GroupBox[] boxes;
        private System.Windows.Forms.CheckBox[] checks;
        private System.Windows.Forms.Button[] options;
        private ControllerOptions optionsWindow;
        private ControllerManager controllerManager;
        private System.Windows.Forms.CheckBox isExclusive;
        private System.Windows.Forms.CheckedListBox controllerList;
    }
}