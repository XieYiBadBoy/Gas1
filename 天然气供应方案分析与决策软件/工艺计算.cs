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

        #region 计算下游绝对压力   ()
        private void Add1()
        {
            try
            {
                if (true)
                {
                    if (txtInput1.Text == "")
                    {
                        throw new InvalidOperationException("输入参数{" + lblInput1.Text + txtInput1.Text + "}为空，请重新输入。");

                    }
                }


                double StdFlow = Convert.ToDouble(txtInput1.Text);   //标准体积流量   StdFlow
                double UpPre = Convert.ToDouble(txtInput2.Text);     //管线上游表压   UpPre
                double Dia = Convert.ToDouble(txtInput3.Text);        //管线内径       Dia
                double AbsRough = Convert.ToDouble(pipesetup.PipeRough);   //绝对粗糙度     AbsRough
                double Length = Convert.ToDouble(txtInput4.Text);     //管线长度       Length
                double GasWeight = Convert.ToDouble(pipesetup.PipeGasWeight );  //气体比重       GasWeight
                double Tep = Convert.ToDouble(pipesetup.PipeTep);        //气体平均温度 
                double PP1 = UpPre;
                double z1 = CompressibilityFactor(UpPre, GasWeight, Tep);
                double RR1 = DaXiXiShu(Tep, GasWeight, Dia, StdFlow, AbsRough);  //计算达西摩阻系数
                double PP2 = PipelinePressure(UpPre, StdFlow, RR1, z1, GasWeight, Tep, Length, Dia);
                int k = 0;

                if (true)
                {
                    if (StdFlow < 0)
                    {
                        throw new InvalidOperationException("输入参数{" + lblInput1.Text + txtInput1.Text + "}为负数，请重新输入。");

                    }
                    if (StdFlow == 0)
                    {
                        throw new InvalidOperationException("输入参数{" + lblInput1.Text + txtInput1.Text + "}为零，请重新输入。");

                    }
                }

                while (Math.Abs(PP2 - PP1) > 0.00001)
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


        private double CompressibilityFactor(double P1, double Q3, double T5)
        {
            double T6 = T5 + 273;
            double PK1 = P1;                                     //选取表压初值
            double BL4 = P1 + PK1 * PK1 / (PK1 + P1);
            double PJ1 = 2 / 3 * BL4;   //计算管线平均表压
            double BL5 = 507200 * PJ1 * Math.Pow(10, 1.785 * Q3); //BL5,BL6 均为中间变量，计算方便，无实义  
            double BL6 = BL5 / (Math.Pow(T6, 3.825));
            double Z1 = 1 / (1 + BL6);    // Calculate Compressibility Fcator 
            return Z1;
            //现在开始进入迭代过程
        }

        private double DaXiXiShu(double T1, double Q1, double D1, double F1, double K1)
        {
            double T2 = (T1 + 273) / 191.16;                             //注意温度单位是℉而不是摄氏度，需要单位换算
            double N1 = 1.0009 * Math.Pow(10, -5) * Math.Pow(Q1, 0.5); //N1表示临界温度T下的动力粘度
            double N2;                                                 //N2 表示为实际温度下T下的动力粘度；
            //double R1=0.039685;
            double R2;
            if (T2 <= 1)         //注意是T2不是T1!!
            {
                N2 = N1 * Math.Pow(T2, 0.005);
            }
            else
            {
                N2 = N1 * Math.Pow(T2, 0.71 + 0.29 * 191.16 / (T1 + 273));
            }
            double Re1 = 0.558 * Q1 * F1 / (D1 * N2 * Math.PI);
            double BL1;   //BL1,BL2 均为中间变量，方便计算
            double BL2;
            if (Re1 <= 4000)
            {
                if (Re1 <= 2000)
                {
                    R2 = 64 / Re1;
                }
                else
                {
                    R2 = 0.0025 * Math.Pow(Re1, 1 / 3.0);
                }
            }
            else
            {
                BL1 = K1 / (3.71 * D1) + 5.7385 / (Math.Pow(Re1, 0.9));
                BL2 = Math.Log(BL1);
                R2 = 1.33036 / Math.Pow(BL2, 2);   //  由于求解的达西系数为隐函数，不方面计算，这里参考文献 <<似牛顿浆体几个摩阻系数计算公式的比较>> 求解其系数
            }
            return R2;
        }


        private double PipelinePressure(double P2, double F3, double R2, double Z2,
                               double Q4, double T7, double L1, double D3)
        {
            double T8 = T7 + 273;
            double PP1;                                           //PP1 为管线下游表压
            double C = 0.332 * Math.Pow(10, -6);                   //C为常数
            double BL7 = F3 * F3 / (C * C);                       //BL7,BL8,BL9 BL10 均为中间变量，计算方便，无实义
            double BL8 = R2 * Z2 * Q4 * T8 * L1;            //注意温度单位是℉而不是摄氏度，需要单位换算
            double BL9 = BL7 * BL8 / (Math.Pow(D3, 5));
            double BL10 = P2 * P2 - BL9;
            PP1 = Math.Pow(BL10, 0.5);
            return PP1;
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

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
