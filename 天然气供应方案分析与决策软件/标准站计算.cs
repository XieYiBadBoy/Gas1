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
    public partial class Windows5 : Form
    {
        public Windows5()
        {
            InitializeComponent();
        }
        private MathOpt calculator3 = new MathOpt();
        StdSetup2 stdsetup2 = new StdSetup2();
     
        private void Add5()
        {
            try
            {
                double z = Convert.ToDouble(stdsetup2.StdFactor);         //压缩机因子
                double StdFlow3 = Convert.ToDouble(txtInput1.Text);       //标准体积流量       StdFlow
                double Time = Convert.ToDouble(stdsetup2.StdTime);        //子站加气机工作时间 Time  
                double ExitPre = Convert.ToDouble(txtInput4.Text);        //压缩机出口压力     ExitPre  
                double UpPre = Convert.ToDouble(txtInput5.Text);          //压缩机入口压力   UpPre       
                double LowProp = Convert.ToDouble(stdsetup2.StdLowProp);  //低压区容积比例
                double MidProp = Convert.ToDouble(stdsetup2.StdMidProp);  //中压区容积比例
                double TaxiNum = Convert.ToDouble(textBox2.Text);         //出租车数量
                double BusNum = Convert.ToDouble(textBox1.Text);          //公交车数量
                double CompTime = Convert.ToDouble(comInput1.Text);      //压缩机补气时间
                double PreRat = calculator3.CompressorPower(z, StdFlow3, Time, UpPre, ExitPre);
                double TotalArea = 10000 + 0.14 * StdFlow3;
                double CNGnum = Math.Ceiling(calculator3.CNGNum(TaxiNum, BusNum, Time));
                double Vg31 = calculator3.LowPer(LowProp, MidProp, StdFlow3, CompTime);
                double Vg21 = MidProp * Vg31;
                double Vg11 = LowProp * Vg31;
                double n3 = Vg31 / 2700;
                double n2 = Vg21 / 2700;
                double n1 = Vg11 / 2700;
                txtOutput3.Text = PreRat.ToString();
                txtOutput4.Text = TotalArea.ToString("0.0000");
                txtOutput5.Text = Vg31.ToString("0.0000");
                txtOutput6.Text = Vg21.ToString("0.0000");
                txtOutput7.Text = Vg11.ToString("0.0000");
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

        private void Clobutton3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Calbutton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Add5();
            }
            else
            {
                MessageBox.Show("请选择计算类型");
            }
        }
        private void Clear()
        {
            txtInput1.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            txtInput4.Text = "";
            txtInput5.Text = "";
            txtOutput3.Text = "";
            txtOutput4.Text = "";
            txtOutput5.Text = "";
            txtOutput6.Text = "";
            txtOutput7.Text = "";
            txtOutput8.Text = "";
            txtOutput9.Text = "";
            txtOutput10.Text = "";
            txtOutput11.Text = "";
            comInput1.Text = "";   
        }
        private void Clebutton2_Click(object sender, EventArgs e)
        {
            Clear();
        }
        public StdSetup2 stdset;
        private void button1_Click(object sender, EventArgs e)
        {
            stdset = new StdSetup2();
            stdset.ShowDialog();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Windows5_Load(object sender, EventArgs e)
        {

        }
    }
}
