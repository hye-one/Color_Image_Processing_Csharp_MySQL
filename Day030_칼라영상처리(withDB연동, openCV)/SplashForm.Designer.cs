
namespace Day015_01_컬러영상처리_Beta1_
{
    partial class SplashForm
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
            this.time = new System.Windows.Forms.Label();
            this.title = new System.Windows.Forms.Label();
            this.Base_panel = new System.Windows.Forms.Panel();
            this.Time_panel = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // time
            // 
            this.time.AutoSize = true;
            this.time.Location = new System.Drawing.Point(328, 115);
            this.time.Name = "time";
            this.time.Size = new System.Drawing.Size(45, 15);
            this.time.TabIndex = 1;
            this.time.Text = "label1";
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("맑은 고딕", 19.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(293, 91);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(409, 45);
            this.title.TabIndex = 2;
            this.title.Text = "C# IMAGE PROCESSING ";
            // 
            // Base_panel
            // 
            this.Base_panel.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.Base_panel.Location = new System.Drawing.Point(0, 291);
            this.Base_panel.Name = "Base_panel";
            this.Base_panel.Size = new System.Drawing.Size(726, 28);
            this.Base_panel.TabIndex = 3;
            // 
            // Time_panel
            // 
            this.Time_panel.BackColor = System.Drawing.Color.Thistle;
            this.Time_panel.Location = new System.Drawing.Point(0, 291);
            this.Time_panel.Name = "Time_panel";
            this.Time_panel.Size = new System.Drawing.Size(28, 28);
            this.Time_panel.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 15;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SplashForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 317);
            this.Controls.Add(this.Time_panel);
            this.Controls.Add(this.Base_panel);
            this.Controls.Add(this.title);
            this.Controls.Add(this.time);
            this.Name = "SplashForm";
            this.Text = "SplashForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label time;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Panel Base_panel;
        private System.Windows.Forms.Panel Time_panel;
        private System.Windows.Forms.Timer timer1;
    }
}