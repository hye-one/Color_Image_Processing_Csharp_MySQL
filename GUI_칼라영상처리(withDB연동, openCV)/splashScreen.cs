using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Day015_01_컬러영상처리_Beta1_
{
    public partial class splashScreen : Form
    {
        public splashScreen()
        {
            InitializeComponent();
        }

        private void splashScreen_Load(object sender, EventArgs e)
        {
            Base_panel.BackColor = Color.FromArgb(108, 114, 182);
            Time_panel.BackColor = Color.FromArgb(204, 142, 137);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Time_panel.Width += 3; //길어지는 panel 
            if (Time_panel.Width >= 690)
            {
                timer1.Stop();
                this.Size = new Size(300, 450);
                Base_panel.Visible = false; //라인 panel
                Time_panel.Visible = false; // 길어지는 panel

                this.Close();
                MainForm frm = new MainForm();
                frm.Show();
            }
        }
    }
}
