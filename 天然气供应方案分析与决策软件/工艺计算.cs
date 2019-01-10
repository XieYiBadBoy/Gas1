using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 天然气供应方案分析与决策软件
{
    public partial class Windows1 : Form
    {
        public Windows1()
        {
            InitializeComponent();
        }
        private MathOpt calculator = new MathOpt();
        PipeSetup1 pipesetup = new PipeSetup1();

        #region 计算下游绝对压力   (已检查 2018-09-14)
        private void Add1()
        {
            try
            {

                double StdFlow = Convert.ToDouble(txtInput1.Text);   //标准体积流量   StdFlow
                double UpPre = Convert.ToDouble(txtInput2.Text);     //管线上游表压   UpPre
                double Dia = Convert.ToDouble(txtInput3.Text);        //管线内径       Dia
                double AbsRough = Convert.ToDouble(pipesetup.PipeRough);   //绝对粗糙度     AbsRough
                double Length = Convert.ToDouble(txtInput4.Text);     //管线长度       Length
                double GasWeight = Convert.ToDouble(pipesetup.PipeGasWeight );  //气体比重       GasWeight
                double Tep = Convert.ToDouble(pipesetup.PipeTep);        //气体平均温度 
                double PP1 = UpPre;
                double z1 = calculator.CompressibilityFactor(UpPre, GasWeight, Tep);
                double RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough);  //计算达西摩阻系数
                double PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
                while (Math.Abs(PP2 - PP1) > 0.01)
                {
                    PP1 = PP2;
                    z1 = calculator.CompressibilityFactor(PP1, GasWeight, Tep);
                    PP2 = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
                }
                txtOutput1.Text = calculator.PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia).ToString("0.000000");
                txtOutput2.Text = calculator.MeanVelocity(StdFlow, Dia).ToString("0.000000");
                txtOutput3.Text = calculator.LeiNuoXiShu(Tep, GasWeight, Dia, StdFlow).ToString("0.000000");
                txtOutput4.Text = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough).ToString("0.000000");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion
        #region  计算上游绝对压力   （已检查 2018-09-14）
        private void Add2()
        {
            try
            {

                double StdFlow = Convert.ToDouble(txtInput1.Text);      //标准体积流量 StdFlow
                double DownPre = Convert.ToDouble(txtInput2.Text);      //管线下游表压 DownPre
                double Dia = Convert.ToDouble(txtInput3.Text);          //管线内径    Dia
                double AbsRough = Convert.ToDouble(pipesetup .PipeRough );     //绝对粗糙度   AbsRough
                double Length = Convert.ToDouble(txtInput4.Text);       //管线长度     Length
                double GasWeight = Convert.ToDouble(pipesetup.PipeGasWeight);    //气体比重     GasWeight
                double Tep = Convert.ToDouble(pipesetup.PipeTep );          //气体平均温度 Tep
                double PP1 = DownPre;
                double z1 = calculator.CompressibilityFactor(DownPre, GasWeight, Tep);
                double RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough);  //计算达西摩阻系数
                double PP2 = calculator.PipelinePressure1(DownPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
                while (Math.Abs(PP2 - PP1) > 0.01)
                {
                    PP1 = PP2;
                    z1 = calculator.CompressibilityFactor(PP2, GasWeight, Tep);
                    PP2 = calculator.PipelinePressure1(DownPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
                }
                txtOutput1.Text = PP2.ToString("0.000000");
                txtOutput2.Text = calculator.MeanVelocity(StdFlow, Dia).ToString("0.000000");
                txtOutput3.Text = calculator.LeiNuoXiShu(Tep, GasWeight, Dia, StdFlow).ToString("0.000000");
                txtOutput4.Text = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough).ToString("0.000000");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        #endregion     
        #region   计算流体体积流量
        private void Add3()
        {
            try
            {

                double UpPre = Convert.ToDouble(txtInput1.Text);       //管线上游表压 UpPre
                double DownPre = Convert.ToDouble(txtInput2.Text);     //管线下游表压 DownPre
                double Dia = Convert.ToDouble(txtInput3.Text);         //管线内径     Dia
                double AbsRough = Convert.ToDouble(pipesetup.PipeRough );    //绝对粗糙度   AbsRough
                double Length = Convert.ToDouble(txtInput4.Text);      //管线长度     Length
                double GasWeight = Convert.ToDouble(pipesetup.PipeGasWeight);   //气体比重     GasWeight
                double Tep = Convert.ToDouble(pipesetup.PipeTep);         //气体平均温度 Tep
                double StdFlow1 = 1.01736 * Dia * Dia;
                double RE = calculator.LeiNuoXiShu(Tep, GasWeight, Dia, StdFlow1);          //计算雷诺数
                double z1 = calculator.CompressibilityFactor1(UpPre, DownPre, GasWeight, Tep);   //计算压缩因子系数
                double RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow1, AbsRough);   //计算达西摩阻系数
                double StdFlow2 = calculator.StandardVolumeFlow(UpPre, DownPre, RR1, z1, GasWeight, Tep, Length, Dia);
                while (Math.Abs(StdFlow2 - StdFlow1) > 0.01)
                {
                    StdFlow1 = StdFlow2;
                    RE = calculator.LeiNuoXiShu(Tep, GasWeight, Dia, StdFlow1);
                    RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow1, AbsRough);
                    StdFlow2 = calculator.StandardVolumeFlow(UpPre, DownPre, RR1, z1, GasWeight, Tep, Length, Dia);
                }
                txtOutput1.Text = StdFlow2.ToString("0.000000");
                txtOutput2.Text = calculator.MeanVelocity(StdFlow2, Dia).ToString("0.000000");
                txtOutput3.Text = RE.ToString("0.000000");
                txtOutput4.Text = RR1.ToString("0.000000");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion 
        #region    计算管长  (已经检查 2018-9-14)
        private void Add4()
        {
            try
            {

                double StdFlow = Convert.ToDouble(txtInput1.Text);     //标准体积流量 StdFlow
                double UpPre = Convert.ToDouble(txtInput2.Text);       //管线上游表压 UpPre
                double DownPre = Convert.ToDouble(txtInput3.Text);     //管线下游表压 DownPre
                double Dia = Convert.ToDouble(txtInput4.Text);         //管线内径    Dia
                double AbsRough = Convert.ToDouble(pipesetup.PipeRough );    //绝对粗糙度   K
                double GasWeight = Convert.ToDouble(pipesetup.PipeGasWeight);   //气体比重    GasWeight
                double Tep = Convert.ToDouble(pipesetup.PipeTep);         //气体平均温度 Tep
                double RE = calculator.LeiNuoXiShu(Tep, GasWeight, Dia, StdFlow);               //计算雷诺系数
                double Z1 = calculator.CompressibilityFactor1(UpPre, DownPre, GasWeight, Tep);   //计算压缩因子系数
                double RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough);         //计算达西摩阻系数
                txtOutput1.Text = calculator.PipelineLength(UpPre, DownPre, Dia, StdFlow, RR1, Z1, GasWeight, Tep).ToString("0.000000");
                txtOutput2.Text = calculator.MeanVelocity(StdFlow, Dia).ToString("0.000000");
                txtOutput3.Text = RE.ToString("0.000000");
                txtOutput4.Text = RR1.ToString("0.000000");
            }
            catch (Exception ex)
            {  
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region   计算管内径   (已经检查  2018-9-14)
        private void Add5()
        {
            try
            {

                double StdFlow = Convert.ToDouble(txtInput1.Text);     //标准体积流量 StdFlow
                double UpPre = Convert.ToDouble(txtInput2.Text);       //管线上游表压 UpPre
                double DownPre = Convert.ToDouble(txtInput3.Text);     //管线下游表压 DownPre
                double Length = Convert.ToDouble(txtInput4.Text);      //管道长度    Length
                double AbsRough = Convert.ToDouble(pipesetup.PipeRough);    //绝对粗糙度   AbsRough
                double GasWeight = Convert.ToDouble(pipesetup.PipeGasWeight);   //气体比重    GasWeight
                double Tep = Convert.ToDouble(pipesetup.PipeTep);         //气体平均温度 Tep
                double Z1 = calculator.CompressibilityFactor1(UpPre, DownPre, GasWeight, Tep);
                double Dia1 = Math.Pow(1000 * StdFlow / 9 / 36 / Math.PI, 0.5);
                double RE = calculator.LeiNuoXiShu(Tep, GasWeight, Dia1, StdFlow);
                double RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia1, StdFlow, AbsRough);
                double Dia2 = calculator.PipelineDiameter(StdFlow, RR1, Z1, GasWeight, Tep, Length, UpPre, DownPre);
                while (Math.Abs(Dia2 - Dia1) > 0.01)
                {
                    Dia1 = Dia2;
                    RE = calculator.LeiNuoXiShu(Tep, GasWeight, Dia1, StdFlow);
                    RR1 = calculator.DaXiXiShu(Tep, GasWeight, Dia1, StdFlow, AbsRough);
                    Dia2 = calculator.PipelineDiameter(StdFlow, RR1, Z1, GasWeight, Tep, Length, UpPre, DownPre);
                }
                txtOutput1.Text = Dia2.ToString("0.00000000");
                txtOutput2.Text = calculator.MeanVelocity(StdFlow, Dia2).ToString("0.000000");
                txtOutput3.Text = RE.ToString("0.000000");
                txtOutput4.Text = RR1.ToString("0.000000");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        private void LabelChange1()
        {
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            Close();
        }
        public PipeSetup1 set;
        private void button1_Click(object sender, EventArgs e)
        {
            set =  new PipeSetup1();
            set.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Add1();
            }
            else if (radioButton2.Checked == true)
            {
                Add2();
            }
            else if (radioButton3.Checked == true)
            {
                Add3();
            }
            else if (radioButton4.Checked == true)
            {
                Add4();
            }
            else if (radioButton5.Checked == true)
            {
                Add5();
            }
            else
            {
                MessageBox.Show("请选择计算类型");
            }
        }

        private void Clebutton2_Click(object sender, EventArgs e)
        {
            PipeClear();
        }

        #region   下游压力标签改变
        private void Changed1()
        {
            lblInput1.Text = "标准体积流量：";
            lblInput2.Text = "管线上游表压：";
            lblInput3.Text = "管线内径：";
            lblInput4.Text = "管线长度：";
            //lblInput5.Text = "管线长度：";
            lblOutput1.Text = "管线下游表压：";
            label8.Text = "m3/d";
            label9.Text = "Mpa";
            label10.Text = "mm";
            label11.Text = "mm";
            //label12.Text = "km";
            label19.Text = "Mpa";
        }
        #endregion
        #region   上游压力标签改变
        private void Changed2()
        {
            lblInput1.Text = "标准体积流量：";
            lblInput2.Text = "管线下游表压：";
            lblInput3.Text = "管线内径：";
            //lblInput4.Text = "绝对粗糙度：";
            lblInput4.Text = "管线长度：";
            lblOutput1.Text = "管线上游表压：";
            label8.Text = "m3/d";
            label9.Text = "Mpa";
            label10.Text = "mm";
            label11.Text = "km";
            //label12.Text = "km";
            label19.Text = "Mpa";
        }
        #endregion
        #region 流体体积流量标签改变
        private void Changed3()
        {
            lblInput1.Text = "管线上游表压：";
            lblInput2.Text = "管线下游表压：";
            lblInput3.Text = "管线内径：";
            //lblInput4.Text = "绝对粗糙度：";
            lblInput4.Text = "管线长度：";
            lblOutput1.Text = "标准体积流量：";
            label8.Text = "Mpa";
            label9.Text = "Mpa";
            label10.Text = "mm";
            label11.Text = "km";
            //label12.Text = "km";
            label19.Text = "m3/d";
        }
        #endregion
        #region 管长标签改变
        private void Changed4()
        {
            lblInput1.Text = "标准体积流量：";
            lblInput2.Text = "管线上游表压：";
            lblInput3.Text = "管线下游表压：";
            lblInput4.Text = "管线内径：";
            //lblInput5.Text = "绝对粗糙度：";
            lblOutput1.Text = "管线长度：";
            label8.Text = "m3/d";
            label9.Text = "Mpa";
            label10.Text = "Mpa";
            label11.Text = "mm";
            //label12.Text = "mm";
            label19.Text = "km";
        }
        #endregion
        #region 管内径标签改变
        private void Changed5()
        {
            lblInput1.Text = "标准体积流量：";
            lblInput2.Text = "管线上游表压：";
            lblInput3.Text = "管线下游表压：";
            lblInput4.Text = "管道长度：";
            //lblInput5.Text = "绝对粗糙度：";
            lblOutput1.Text = "管线内径：";
            label8.Text = "m3/d";
            label9.Text = "Mpa";
            label10.Text = "Mpa";
            label11.Text = "km";
            //label12.Text = "mm";
            label19.Text = "mm";
        }
        #endregion
        private void PipeClear()
        {
            txtInput1.Text = "";
            txtInput2.Text = "";
            txtInput3.Text = "";
            txtInput4.Text = "";
            txtOutput1.Text = "";
            txtOutput2.Text = "";
            txtOutput3.Text = "";
            txtOutput4.Text = "";
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            Changed2();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Changed1();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Changed3();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            Changed4();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            Changed5();
        }
    }
}
