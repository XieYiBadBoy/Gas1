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
    public partial class Windows2 : Form
    {
        public Windows2()
        {
            InitializeComponent();
        }
        private MathOpt calculator1 = new MathOpt();
        NatStationSetup natstationsetup = new NatStationSetup();
        private void Add2() 
        {
            try
            {
                double Z = Convert.ToDouble(natstationsetup .NatFactor);
                double StdFlow2 = Convert.ToDouble(txtInput2.Text);   //标准体积流量       StdFlow
                double UpPre = Convert.ToDouble(txtInput4.Text);     //压缩机进口前压力   UpPre
                double ExitPre = Convert.ToDouble(txtInput5.Text);   //压缩机出口压力     ExitPre  
                double t = Convert.ToDouble(natstationsetup .NatTime);       //工作时长
                double N = calculator1.CompressorPower(Z,StdFlow2,t,UpPre ,ExitPre);           
                double TotalArea = 9500 + 0.04 * StdFlow2;
                txtOutput1.Text = N.ToString("0.0000");
                txtOutput2.Text = TotalArea.ToString("0.0000");
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void lblInput2_Click(object sender, EventArgs e)
        {

        }

        private void Calbutton1_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked == true)
            {
                Add2();
            }
            else
            {
                MessageBox.Show("请选择计算类型");
            }
        }

        private void Clobutton3_Click(object sender, EventArgs e)
        {
            Close();
        }
        protected void Clear()
        {
            txtInput2.Text = "";
            txtInput4.Text = "";
            txtInput5.Text = "";
            txtOutput1.Text = "";
            txtOutput2.Text = "";

        }
        private void Clebutton2_Click(object sender, EventArgs e)
        {
            Clear();
        }
        //public NatStationSetup 
        private void Setbutton1_Click(object sender, EventArgs e)
        {
            natstationsetup.ShowDialog();
        }
    }
}
