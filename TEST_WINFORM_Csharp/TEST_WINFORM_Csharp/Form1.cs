using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TEST_WINFORM_Csharp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                userControl11.Value = int.Parse(textBox1.Text);
            } catch { }
        }

        private void userControl11_ValueChangeEvent(object sender , EventArgs e)
        {
            textBox1.Text = userControl11.Value.ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            userControl11.Value = 50;
        }
    }
}
