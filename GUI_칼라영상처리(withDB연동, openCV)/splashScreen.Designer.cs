
namespace Day015_01_컬러영상처리_Beta1_
{
    partial class splashScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(splashScreen));
            this.Time_panel = new System.Windows.Forms.Panel();
            this.Base_panel = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Base_panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Time_panel
            // 
            this.Time_panel.BackColor = System.Drawing.Color.Thistle;
            this.Time_panel.Location = new System.Drawing.Point(0, 0);
            this.Time_panel.Name = "Time_panel";
            this.Time_panel.Size = new System.Drawing.Size(28, 20);
            this.Time_panel.TabIndex = 6;
            // 
            // Base_panel
            // 
            this.Base_panel.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Base_panel.Controls.Add(this.Time_panel);
            this.Base_panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Base_panel.Location = new System.Drawing.Point(0, 348);
            this.Base_panel.Name = "Base_panel";
            this.Base_panel.Size = new System.Drawing.Size(774, 20);
            this.Base_panel.TabIndex = 8;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 3;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // splashScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(774, 368);
            this.Controls.Add(this.Base_panel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "splashScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "splashScreen";
            this.Load += new System.EventHandler(this.splashScreen_Load);
            this.Base_panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel Time_panel;
        private System.Windows.Forms.Panel Base_panel;
        private System.Windows.Forms.Timer timer1;
    }
}