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
    public partial class Windows3 : Form
    {
        public Windows3()
        {
            InitializeComponent();
        }
        private MathOpt calculator2 = new MathOpt();
        SetStationSetup SecSetup = new SetStationSetup();
        private void Change1()
        {
            
            lblInput2.Text = "设计体积流量：";
          
            lblInput4.Text = "压缩机出口压力：";
            txtInput4.Visible = true ;
            txtInput5.Visible = true ;
            txtInput6.Visible = true ;
            txtInput7.Visible = true ;
           
        
            comInput1.Visible = true;
            lblInput5.Text = "压缩机入口压力：";
            lblInput6.Text = "低压区容积：";
            lblInput7.Text = "中压区容积：";
            lblInput8.Text = "高压区容积=";
            lblInput9.Text = "：";
            lblInput10.Text = "：1";
            lblInput11.Text = "压缩机补气时间：";
        
         
        
            label9.Text = "万方/日";
           
            label11.Text = "兆帕";
            label12.Text = "兆帕";
            label13.Text = "小时/日";
          
            label16.Text = "台";
            label17.Text = "立方米";
            label18.Text = "立方米";
            label19.Text = "立方米";
            label20.Text = "个";
            label21.Text = "个";
            label22.Text = "个";
            label23.Text = "台";
            label24.Text = "立方米";
            lblOutput1.Text = "压缩机的台数：";
            lblOutput2.Text = "低压区储气容积：";
            lblOutput3.Text = "中压区储气容积：";
            lblOutput4.Text = "高压区储气容积：";
            lblOutput5.Text = "低压区井管数量：";
            lblOutput6.Text = "中压区井管数量：";
            lblOutput7.Text = "高压区井管数量：";
            lblOutput8.Text = "子站加气机数量：";
            lblOutput9.Text = "CNG子站总面积：";
            txtOutput4.Visible = true ;
            txtOutput6.Visible = true ;
            txtOutput7.Visible = true ;
            txtOutput8.Visible = true ;
            txtOutput9.Visible = true ;
            txtOutput10.Visible = true ;
            txtOutput11.Visible = true ;
        }
        private void Change2()
        {
          
            lblInput2.Text = "设计体积流量：";
           
            lblInput5.Text = "每日工作时长：";
            txtInput4.Visible = false;
            txtInput6.Visible = false;
            txtInput7.Visible = false;
            
          
            comInput1.Visible = false;
            lblInput4.Text = "";
            lblInput6.Text = "";
            lblInput7.Text = "";
            lblInput8.Text = "";
            lblInput9.Text = "";
            lblInput10.Text = "";
            lblInput11.Text = "";

            label9.Text = "万方/日";
          
            label12.Text = "小时/日";
            label11.Text = "";
            label13.Text = "";
         
            label16.Text = "台";
            label17.Text = "立方米";
            label18.Text = "";
            label19.Text = "";
            label20.Text = "";
            label21.Text = "";
            label22.Text = "";
            label23.Text = "";
            label24.Text = "";
            lblOutput1.Text = "子站加气机数量：";
            lblOutput2.Text = "CNG子站总面积：";
            lblOutput3.Text = "";
            lblOutput4.Text = "";
            lblOutput5.Text = "";
            lblOutput6.Text = "";
            lblOutput7.Text = "";
            lblOutput8.Text = "";
            lblOutput9.Text = "";
            txtOutput4.Visible = false;
            txtOutput6.Visible = false;
            txtOutput7.Visible = false;
            txtOutput8.Visible = false;
            txtOutput9.Visible = false;
            txtOutput10.Visible = false;
            txtOutput11.Visible = false;
        }
        private void Add3()
        {
            try
            {  
                double z = Convert.ToDouble(SecSetup .SecStationFactor);    //压缩机因子
                double StdFlow3 = Convert.ToDouble(txtInput2.Text);   //标准体积流量       StdFlow
                double Time = Convert.ToDouble(SecSetup.SecStationTime);    //子站加气机工作时间 Time  
                double ExitPre = Convert.ToDouble(txtInput4.Text);   //压缩机出口压力     ExitPre  
                double UpPre = Convert.ToDouble(txtInput5.Text);     //压缩机入口压力   UpPre       
                double LowProp = Convert.ToDouble(txtInput6.Text);  //低压区容积比例
                double MidProp = Convert.ToDouble(txtInput7.Text);   //中压区容积比例
                double TaxiNum = Convert.ToDouble(SecSetup.SecStationTaxiNum);   //出租车数量
                double BusNum = Convert.ToDouble(SecSetup.SecStationBusNum);    //公交车数量
                double CompTime = Convert.ToDouble(comInput1.Text);  //压缩机补气时间
                double PreRat = calculator2.CompressorPower(z,StdFlow3,Time,UpPre,ExitPre);
                double TotalArea = 1550 + 0.1 * StdFlow3;
                double CNGnum = Math.Ceiling(calculator2.CNGNum(TaxiNum, BusNum, Time));
                double Vg31 = calculator2.LowPer(LowProp, MidProp, StdFlow3, CompTime);
                double Vg21 = MidProp * Vg31;
                double Vg11 = LowProp * Vg31;
                double n3 = Vg31 / 2700;
                double n2 = Vg21 / 2700;
                double n1 = Vg11 / 2700;
                txtOutput3.Text = PreRat.ToString("");
                txtOutput4.Text = TotalArea.ToString("0.00");
                txtOutput5.Text = Vg31.ToString("0.00");
                txtOutput6.Text = Vg21.ToString("0.00");
                txtOutput7.Text = Vg11.ToString("0.00");
                txtOutput8.Text = Math.Ceiling(n3).ToString();
                txtOutput9.Text = Math.Ceiling(n2).ToString();
                txtOutput10.Text = Math.Ceiling(n1).ToString();
                txtOutput11.Text = CNGnum.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Add4()
        {
            try
            {

                double StdFlow4 = Convert.ToDouble(txtInput2.Text);   //标准体积流量       StdFlow
                double Time = Convert.ToDouble(txtInput5.Text);      //每日工作时长       Time
                double TaxiNum = Convert.ToDouble(SecSetup.SecStationTaxiNum);   //出租车数量    TaxiNum  
                double BarNum = Convert.ToDouble(SecSetup.SecStationBusNum);       //公交车数量     BarNum
                double Num11 = calculator2.CNGNum(TaxiNum, BarNum, Time);
                double TotalArea = 1000 + 0.1 * StdFlow4;
                txtOutput3.Text = Math.Ceiling (Num11).ToString("");
                txtOutput5.Text = TotalArea.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void Clobutton3_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void Calbutton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Add3();
            }
            else if (radioButton2.Checked == true)
            {
                Add4();
            }
            else
            {
                MessageBox.Show("请选择计算类型");
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblInput2_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Change2();
            Clear2();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Change1();
            Clear1();
        }
        private void Clear()
        {
        }
        private void Clear1()
        {
            txtInput2.Text = "";
            txtInput7.Text = "";
            txtInput4.Text = "";
            txtInput5.Text = "";
            txtInput6.Text = "";
            txtInput2.Text = "";
            txtOutput3.Text = "";
            txtOutput4.Text = "";
            txtOutput5.Text = "";
            txtOutput6.Text = "";
            txtOutput7.Text = "";
            txtOutput9.Text = "";
            txtOutput8.Text = "";
            txtOutput10.Text = "";
            txtOutput11.Text = ""; 
        }
        private void Clear2()
        {
            txtInput2.Text = "";
            txtInput4.Text = "";
            txtOutput3.Text = "";
            txtOutput5.Text = "";
        }
        private void Clebutton2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Clear1();
            }
            else if (radioButton2.Checked == true)
            {
                Clear2();
            }
            else
            {
                Clear1();
            }
        }

        private void lblOutput4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SecSetup.ShowDialog();

        }
    }
}
