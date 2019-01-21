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
using System.Xml;

namespace 天然气供应方案分析与决策软件
{
    public partial class ComprehensiveAnalysCriticalCurveMethod : Form
    {
        public ComprehensiveAnalysCriticalCurveMethod()
        {
            InitializeComponent();
        }

          

        public CriticalCurveMethodSetting CriticalCurveMethodSet;
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

        private void Calculate()
        {
            try
            {
            double InputValue1 = Convert.ToDouble(txtInput1.Text);
            double InputValue2 = Convert.ToDouble(txtInput2.Text);
            if (txtInput1.Text == "" || txtInput2.Text == "")
            {
                throw new InvalidOperationException("您的输入为空，管子尺寸初步计算(经济及可行性)计算对象无效!");
            }

            if (InputValue1 < 0 || InputValue2 < 0)
            {
                throw new InvalidOperationException("您的输入为负，管子尺寸初步计算(经济及可行性)计算对象无效!");
            }
            if (InputValue1 == 0 || InputValue2 == 0)
            {
                throw new InvalidOperationException("您的输入为零，管子尺寸初步计算(经济及可行性)计算对象无效!");
            }

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("XMLFile1.xml"); //加载xml文件

                Double PngAndCngCriticalFlow = (Convert.ToDouble(ReadXml(xmlDoc, "PNGCoefficientA" )) * InputValue2 + Convert.ToDouble(ReadXml(xmlDoc, "PNGCoefficientB")))/ (Convert.ToDouble(ReadXml(xmlDoc, "PNGCoefficientC")) * InputValue2 + Convert.ToDouble(ReadXml(xmlDoc, "PNGCoefficientD")));
                Double PngAndLngCriticalFlow = (Convert.ToDouble(ReadXml(xmlDoc, "LNGCoefficientA")) * InputValue2 + Convert.ToDouble(ReadXml(xmlDoc, "LNGCoefficientB"))) / (Convert.ToDouble(ReadXml(xmlDoc, "LNGCoefficientC")) * InputValue2 + Convert.ToDouble(ReadXml(xmlDoc, "LNGCoefficientD")));
                Double CngAndLngCriticalFlow = (Convert.ToDouble(ReadXml(xmlDoc, "CNGCoefficientA")) * InputValue2 + Convert.ToDouble(ReadXml(xmlDoc, "CNGCoefficientB"))) / (Convert.ToDouble(ReadXml(xmlDoc, "CNGCoefficientC")) * InputValue2 + Convert.ToDouble(ReadXml(xmlDoc, "CNGCoefficientD")));

                txtOnput1.Text = PngAndCngCriticalFlow.ToString("0.00");
                txtOnput2.Text = PngAndLngCriticalFlow.ToString("0.00");
                txtOnput3.Text = CngAndLngCriticalFlow.ToString("0.00");

                if (PngAndCngCriticalFlow== PngAndLngCriticalFlow)
                {
                    if (CngAndLngCriticalFlow<=PngAndCngCriticalFlow)
                    {
                        if (InputValue1 < CngAndLngCriticalFlow)
                        {
                            txtOnput4.Text = "LNG";
                        }
                        else if (PngAndCngCriticalFlow <= InputValue1)
                        {
                            txtOnput4.Text = "管道";
                        }
                        else if (CngAndLngCriticalFlow <= InputValue1 && InputValue1 < PngAndCngCriticalFlow)
                        {
                            txtOnput4.Text = "CNG";
                        }
                        else
                        {
                            MessageBox.Show("该情况理论上不存在，请检查输入是否正确");
                        }
                    }
                    else
                    {
                        if (InputValue1 <= PngAndCngCriticalFlow)
                        {
                            txtOnput4.Text = "LNG";
                        }
                        else
                        {
                            txtOnput4.Text = "管道";
                        }

                    }

                }
                else if (PngAndCngCriticalFlow < PngAndLngCriticalFlow)
                {
                    if (CngAndLngCriticalFlow < PngAndCngCriticalFlow)
                    {
                        if (InputValue1 < CngAndLngCriticalFlow)
                        {
                            txtOnput4.Text = "LNG";
                        }
                        else if (CngAndLngCriticalFlow <= InputValue1 && InputValue1 < PngAndCngCriticalFlow)
                        {
                            txtOnput4.Text = "CNG";
                        }
                        else
                        {
                            txtOnput4.Text = "管道";
                        }
                    }
                    else if (PngAndCngCriticalFlow <= CngAndLngCriticalFlow && CngAndLngCriticalFlow  < PngAndLngCriticalFlow)
                    {

                        if (InputValue1< PngAndCngCriticalFlow)
                        {
                            txtOnput4.Text = "LNG";
                        }
                        else
                        {
                            txtOnput4.Text = "管道";
                        }
                    }
                    else
                    {
                        if (InputValue1 < PngAndCngCriticalFlow)
                        {
                            txtOnput4.Text = "LNG";
                        }
                        else
                        {
                            txtOnput4.Text = "管道";
                        }

                    }
                }
                else
                {
                    if (CngAndLngCriticalFlow< PngAndLngCriticalFlow)
                    {
                        if (InputValue1 < CngAndLngCriticalFlow)
                        {
                            txtOnput4.Text = "LNG";
                        }
                        else if (CngAndLngCriticalFlow <= InputValue1 && InputValue1 < PngAndLngCriticalFlow)
                        {
                            txtOnput4.Text = "CNG";
                        }
                        else
                        {
                            txtOnput4.Text = "管道";
                        }
                    }
                    else if (PngAndLngCriticalFlow <= CngAndLngCriticalFlow && CngAndLngCriticalFlow < PngAndCngCriticalFlow)
                    {
                        if (InputValue1 < PngAndLngCriticalFlow)
                        {
                            txtOnput4.Text = "LNG";
                        }
                        else
                        {
                            txtOnput4.Text = "管道";
                        }
                    }
                    else
                    {
                        if (InputValue1 < PngAndLngCriticalFlow)
                        {
                            txtOnput4.Text = "LNG";
                        }
                        else
                        {
                            txtOnput4.Text = "管道";
                        }

                    }

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }

        }

        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/ComprehensiveAnalysis/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }

        private void InitChart()
        {
         try
         {
                double InputValue1 = Convert.ToDouble(txtInput1.Text);
                double InputValue2 = Convert.ToDouble(txtInput2.Text);
                if (txtInput1.Text==""||txtInput2.Text=="")
                {
                    throw new InvalidOperationException("您的输入为空，管子尺寸初步计算(经济及可行性)计算对象无效!");
                }

                if (InputValue1<0||InputValue2<0)
                {
                    throw new InvalidOperationException("您的输入为负，管子尺寸初步计算(经济及可行性)计算对象无效!");
                }
                if (InputValue1 == 0 || InputValue2 == 0)
                {
                    throw new InvalidOperationException("您的输入为零，管子尺寸初步计算(经济及可行性)计算对象无效!");
                }

            Series serie1 = chart1.Series[0];
            Series serie2 = chart1.Series[1];
            Series serie3 = chart1.Series[2];
            Series serie4 = chart1.Series[3];
            serie1.Points.Clear();
            serie2.Points.Clear();
            serie3.Points.Clear();
            serie4.Points.Clear();

          
  

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("XMLFile1.xml"); //加载xml文件

             string Str1 = ReadXml(xmlDoc, "PNGCoefficientA");
             string Str2 = ReadXml(xmlDoc, "PNGCoefficientB");
             string Str3 = ReadXml(xmlDoc, "PNGCoefficientC");
             string Str4 = ReadXml(xmlDoc, "PNGCoefficientD");

             string Str5 = ReadXml(xmlDoc, "LNGCoefficientA");
             string Str6 = ReadXml(xmlDoc, "LNGCoefficientB");
             string Str7 = ReadXml(xmlDoc, "LNGCoefficientC");
             string Str8 = ReadXml(xmlDoc, "LNGCoefficientD");

             string Str9 = ReadXml(xmlDoc, "CNGCoefficientA");
             string Str10 = ReadXml(xmlDoc, "CNGCoefficientB");
             string Str11 = ReadXml(xmlDoc, "CNGCoefficientC");
             string Str12 = ReadXml(xmlDoc, "CNGCoefficientD");

             string Str13;
             string Str14;
             string Str15;

                if (Convert.ToDouble(Str2) < 0)
                {
                    if (Convert.ToDouble(Str4) < 0)
                    {
                        Str13 = "q=(" + Str1 + "L" + Str2 + ")/(" + Str3 + "L" + Str4 + ")";
                    }
                    else
                    {
                        Str13 = "q=(" + Str1 + "L" + Str2 + ")/(" + Str3 + "L" + "+" + Str4 + ")";
                    }
                }
                else
                {
                    if (Convert.ToDouble(Str4) < 0)
                    {
                        Str13 = "q=(" + Str1 + "L" + "+" + Str2 + ")/(" + Str3 + "L" + Str4 + ")";
                    }
                    else
                    {
                        Str13 = "q=(" + Str1 + "L" + "+" + Str2 + ")/(" + Str3 + "L" + "+" + Str4 + ")";
                    }
                }


                if (Convert.ToDouble(Str6) < 0)
                {
                    if (Convert.ToDouble(Str8) < 0)
                    {
                        Str14 = "q=(" + Str5 + "L" + Str6 + ")/(" + Str7 + "L" + Str8 + ")";
                    }
                    else
                    {
                        Str14 = "q=(" + Str5 + "L" + Str6 + ")/(" + Str7 + "L" + "+" + Str8 + ")";
                    }
                }
                else
                {
                    if (Convert.ToDouble(Str8) < 0)
                    {
                        Str14 = "q=(" + Str5 + "L" + "+" + Str6 + ")/(" + Str7 + "L" + Str8 + ")";
                    }
                    else
                    {
                        Str14 = "q=(" + Str5 + "L" + "+" + Str6 + ")/(" + Str7 + "L" + "+" + Str8 + ")";
                    }
                }


                if (Convert.ToDouble(Str10) < 0)
                {
                    if (Convert.ToDouble(Str12) < 0)
                    {
                        Str15 = "q=(" + Str9 + "L" + Str10 + ")/(" + Str11 + "L" + Str12 + ")";
                    }
                    else
                    {
                        Str15 = "q=(" + Str9 + "L" + Str10 + ")/(" + Str11 + "L" + "+" + Str12 + ")";
                    }
                }
                else
                {
                    if (Convert.ToDouble(Str12) < 0)
                    {
                        Str15 = "q=(" + Str9 + "L" + "+" + Str10 + ")/(" + Str11 + "L" + Str12 + ")";
                    }
                    else
                    {
                        Str15 = "q=(" + Str9 + "L" + "+" + Str10 + ")/(" + Str11 + "L" + "+" + Str12 + ")";
                    }
                }









            serie1.Name = Str13;
            serie2.Name = Str14;
            serie3.Name = Str15;
            serie4.Name = "输入值";
            serie4.Points.AddXY(InputValue2, InputValue1);

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
            for (Double i = Convert.ToDouble(ReadXml(xmlDoc, "ChartInitialPoint")); i < Convert.ToDouble(ReadXml(xmlDoc, "ChartTerminalPoint")); i=i+Convert.ToDouble(ReadXml(xmlDoc, "ChartInterval")))
            {

                q1= (Convert.ToDouble(ReadXml(xmlDoc, "PNGCoefficientA")) * i + Convert.ToDouble(ReadXml(xmlDoc, "PNGCoefficientB"))) / (Convert.ToDouble(ReadXml(xmlDoc, "PNGCoefficientC")) * i + Convert.ToDouble(ReadXml(xmlDoc, "PNGCoefficientD")));
                serie1.Points.AddXY(i, q1);
                q2= (Convert.ToDouble(ReadXml(xmlDoc, "LNGCoefficientA")) * i + Convert.ToDouble(ReadXml(xmlDoc, "LNGCoefficientB"))) / (Convert.ToDouble(ReadXml(xmlDoc, "LNGCoefficientC")) * i + Convert.ToDouble(ReadXml(xmlDoc, "LNGCoefficientD")));
                serie2.Points.AddXY(i, q2);
                q3= (Convert.ToDouble(ReadXml(xmlDoc, "CNGCoefficientA")) * i + Convert.ToDouble(ReadXml(xmlDoc, "CNGCoefficientB"))) / (Convert.ToDouble(ReadXml(xmlDoc, "CNGCoefficientC")) * i + Convert.ToDouble(ReadXml(xmlDoc, "CNGCoefficientD")));
                serie3.Points.AddXY(i, q3);


            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

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
            Calculate();
            //chart1.ChartAreas.Clear();
            InitChart();
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
          
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CriticalCurveMethodSet = new CriticalCurveMethodSetting();
            CriticalCurveMethodSet.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Clear()
        {
            txtInput1.Text = "";
            txtInput2.Text = "";
            txtOnput1.Text = "";
            txtOnput2.Text = "";
            txtOnput3.Text = "";
            txtOnput4.Text = "";

        }
        private void button5_Click(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
