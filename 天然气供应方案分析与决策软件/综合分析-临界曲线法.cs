using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace 天然气供应方案分析与决策软件
{
    public partial class ComprehensiveAnalysCriticalCurveMethod : Form
    {
        public ComprehensiveAnalysCriticalCurveMethod()
        {
            InitializeComponent();
        }

        private void 综合分析_临界曲线法_Load(object sender, EventArgs e)
        {
            InitChart();
        }

        public static void ShowCurByClick(int ringNum, Chart chart)
        {
            //设置游标位置
            chart.ChartAreas[0].CursorX.Position = ringNum;
            //设置视图缩放
            chart.ChartAreas[0].AxisX.ScaleView.Zoom(ringNum - 1, ringNum + 2);
            //改变曲线线宽
            chart.Series[0].BorderWidth = 3;
            //改变X轴刻度间隔
            chart.ChartAreas[0].AxisX.Interval = 1;
        }
        private void InitChart()
        {
            Series serie1 = chart1.Series[0];
            Series serie2 = chart1.Series[1];
            Series serie3 = chart1.Series[2];
            Series serie4 = chart1.Series[3];
            serie1.Points.Clear();
            serie2.Points.Clear();
            serie3.Points.Clear();
            serie4.Points.Clear();
            serie1.Name = "1";
            serie3.Name = "2";
            double y = Convert.ToDouble(txtInput1.Text);
            double x= Convert.ToDouble(txtInput2.Text);
            serie4.Points.AddXY(x, y);

            //设置是否显示坐标标注
            chart1.Series[0].IsValueShownAsLabel = false;

            //设置游标
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            //设置X轴是否可以缩放
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            //将滚动条放到图表外
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            // 设置滚动条的大小
            chart1.ChartAreas[0].AxisX.ScrollBar.Size = 15;
            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chart1.ChartAreas[0].AxisX.ScrollBar.ButtonColor = Color.SkyBlue;
            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisX.ScaleView.SmallScrollMinSize = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;

            //设置游标
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorY.AutoScroll = true;
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;
            //设置X轴是否可以缩放
            chart1.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
            //将滚动条放到图表外
            chart1.ChartAreas[0].AxisY.ScrollBar.IsPositionedInside = false;
            // 设置滚动条的大小
            chart1.ChartAreas[0].AxisY.ScrollBar.Size = 15;
            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chart1.ChartAreas[0].AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chart1.ChartAreas[0].AxisY.ScrollBar.ButtonColor = Color.SkyBlue;
            // 设置自动放大与缩小的最小量
            chart1.ChartAreas[0].AxisY.ScaleView.SmallScrollSize = double.NaN;
            chart1.ChartAreas[0].AxisY.ScaleView.SmallScrollMinSize = 1;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            double q1;
            double q2;
            double q3;
            for (int i = 100; i < 700; i=i+5)
            {
                q3 = 0.284 / (2.48 *Math.Pow(10,-3) * i - 0.566);
                serie1.Points.AddXY(i,q3);
                q2=(0.0734*i - 0.888)/ (Math.Pow(10, -4)*i + 0.927);
                serie2.Points.AddXY(i, q2);
                q1 = (0.0734 * i - 0.604) / (2.93 * Math.Pow(10, -3) * i + 0.361);
                serie3.Points.AddXY(i, q1);
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {
            HitTestResult myTestResult = chart1.HitTest(e.X, e.Y);
            if (myTestResult.Series != null)
            {
                try
                {
                    label13.Text = myTestResult.Series.Points[myTestResult.PointIndex].XValue.ToString("0.000");
                    label14.Text = myTestResult.Series.Points[myTestResult.PointIndex].YValues[0].ToString("0.000");

                }
                catch (Exception)
                {
                }
            }
            else
            {
                label13.Text = "";
                label14.Text = "";
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //chart1.ChartAreas.Clear();
            InitChart();
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
          
        }
    }
}
