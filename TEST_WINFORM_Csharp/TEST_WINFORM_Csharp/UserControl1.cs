using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST_WINFORM_Csharp
{
    public partial class UserControl1 : UserControl
    {
        public event EventHandler ValueChangeEvent;

        int max = 100; int val = 50;

        [Description("최대값"), Category()]
        public int MaxValue
        {
            get { return max; }
            set
            {
                if (max == 0)
                    max = 1;
                max = value; DrawControl();
            }
        }

        [Description("값"), Category()]
        public int Value
        {
            get { return val; }
            set
            {
                if (val == 0)
                    val = 1;
                val = value; DrawControl();
            }
        }

        public UserControl1()
        {
            Value = 50;
            InitializeComponent();
            DrawControl();
        }

        private void DrawControl()
        {
            try
            {
                if (!isDown)
                    DrawControl(wtf(val, max, this.Width));
            }
            catch { }
        }

        private void DrawControl(int ccircle)
        {
            try
            {
                this.Invalidate();
                this.Update();
                using (var grp = this.CreateGraphics())
                {
                    grp.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                    Pen pen = new Pen(Color.White, 2);
                    grp.DrawLine(pen, 0, 12, ccircle, 12);
                    Pen pen1 = new Pen(Color.Gray, 2);
                    grp.DrawLine(pen1, ccircle + 15, 12, this.Width, 12);

                    grp.DrawEllipse(new Pen(Color.White, 2), ccircle, 5, 15, 15);
                }
                this.Update();
            }
            catch
            {
            }
        }

        int circle = 1;
        bool isDown = false;

        private void PProgressBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (new Rectangle(circle, 5, 15, 15).Contains(e.Location))
                isDown = true;
        }

        private void PProgressBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDown)
                DrawControl(e.X);
        }

        private void PProgressBar_MouseUp(object sender, MouseEventArgs e)
        {
            isDown = false;
            MoveProgress(e.X);
        }

        private void MoveProgress(int X)
        {
            try
            {
                if (X <= 0)
                    X = 1;
                if (X >= this.Width)
                    X = this.Width - 1;

                Value = wtf(X, this.Width, MaxValue);
                ValueChangeEvent(Value, new EventArgs());
            }
            catch
            { }
        }

        private int wtf(int a1, int a2, int b)
        {
            //a1 : 원래 값
            //a2 : 원래 최대값
            //b : 변환 최대값
            //return : 변환 값
            return a1 * b / a2;
        }
    }
}
