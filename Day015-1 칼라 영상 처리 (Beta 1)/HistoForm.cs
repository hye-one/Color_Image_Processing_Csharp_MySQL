using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Day015_1_칼라_영상_처리__Beta_1_
{
    public partial class HistoForm : Form
    {
        public HistoForm(long[] rh, long[] gh, long[] bh)
        {
            InitializeComponent();
            rHisto = rh;
            gHisto = gh;
            bHisto = bh;
        }
        long[] rHisto, gHisto, bHisto;

        private void HistoForm_Load(object sender, EventArgs e)
        {
            chart1.Visible = true;
            chart1.Series[0].ChartType
                = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[1].ChartType
                = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[2].ChartType
                = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chart1.Series[0].Color = Color.Red;
            chart1.Series[1].Color = Color.Green;
            chart1.Series[2].Color = Color.Blue;
            for (int i=0; i<256; i++)
            {
                chart1.Series[0].Points.AddXY(i, rHisto[i]);
                chart1.Series[1].Points.AddXY(i, gHisto[i]);
                chart1.Series[2].Points.AddXY(i, bHisto[i]);
            }


        }
    }
}
