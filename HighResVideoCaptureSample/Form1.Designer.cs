namespace HighResVideoCaptureSample
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.SuspendLayout();
            this.dxCameraControl1 = new TwCameraLib.DxCameraControl();
            this.gameConPollingTimer = new System.Windows.Forms.Timer(this.components);
            // 
            // dxCameraControl1
            // 
            this.dxCameraControl1.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.dxCameraControl1.Location = new System.Drawing.Point(2, 2);
            this.dxCameraControl1.Name = "dxCameraControl1";
            this.dxCameraControl1.Size = new System.Drawing.Size(1280, 720);
            this.dxCameraControl1.TabIndex = 0;
            this.dxCameraControl1.Text = "dxCameraControl1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(1285, 725);
            this.Controls.Add(this.dxCameraControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "High Res Video Code Sample";
            this.MaximizeBox = true;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.ResumeLayout(false);
            // 
            // GameCon Polling Timer
            // 
            this.gameConPollingTimer.Interval = 5;
            this.gameConPollingTimer.Tick += new System.EventHandler(this.gameConPollingTimer_Tick);
        }

        #endregion
        private TwCameraLib.DxCameraControl dxCameraControl1;
        private System.Windows.Forms.Timer gameConPollingTimer;
    }
}

