using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Day015_01_컬러영상처리_Beta1_
{
    public partial class SplashForm : Form
    {
        private void timer1_Tick(object sender, EventArgs e)
        {
            Time_panel.Width += 3; //길어지는 panel 
            if (Time_panel.Width >= 300)
            {
                timer1.Stop();
                this.Size = new Size(300, 450);
                Base_panel.Visible = false; //라인 panel
                Time_panel.Visible = false; // 길어지는 panel
            }
        }
        //    delegate void TestDelegate(string msg);
        //    delegate void TestDelegate2();
        //    public SplashForm()
        //    {
        //        InitializeComponent();
        //        System.Threading.Thread thread = new System.Threading.Thread(Thread1);
        //        thread.Start();
        //    }
        //    private void showText(string msg)
        //    {
        //       time.Text = msg;
        //    }



        //    private void formClose()
        //    {
        //        this.Close();
        //    }

        //    private void Thread1()
        //    {
        //        for (int i = 0; i <= 1000; i++)
        //        {
        //            Invoke(new TestDelegate(showText), i.ToString());
        //            System.Threading.Thread.Sleep(100);
        //        }
        //        //System.Threading.Thread.Sleep(1000);
        //        Invoke(new TestDelegate2(formClose));
        //    }

        //    private void progressBar1_Click(object sender, EventArgs e)
        //    {

        //    }
    }
}
