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
    public partial class Windows7 : Form
    {
        public Windows7()
        {
            InitializeComponent();
        }
         LNGSetup2 LngSetup2=new LNGSetup2 ();

        private void Add1()
        {
            try
            {
                double GasQuality = Convert.ToDouble(txtInput1.Text);    //  气化站  日气化量
                double PeekCoefficient = Convert.ToDouble(txtInput2.Text);  //气化站月高峰系数
                double GasConsumption = Convert.ToDouble(txtInput3.Text);  //气化站年平均供气量
                double MiddleVariable1 =Math .Ceiling(GasQuality / (40 * 625));//日装车密度
                double StorageDay =Convert.ToDouble(LngSetup2.LngStroageDay);  //LNG存储天数
                double GasDensity = Convert.ToDouble(LngSetup2.LngGasDensity);  //LNG 天然气气态密度
                double LiquidDensity = Convert.ToDouble(LngSetup2.LngLiquidDensity);  //LNG 天然气液态密度
                double Volume = StorageDay * PeekCoefficient * GasConsumption * GasDensity / (0.95*LiquidDensity);  //LNG储罐区体积
                double N = Math.Ceiling(Volume / 100);        //LNG储罐的个数
                double TotalArea = 18000 + 2350 * N + 250 * MiddleVariable1;  //总的占地面积
                txtOutput1.Text = MiddleVariable1.ToString();
                txtOutput2.Text = Volume .ToString("0.0000");
                txtOutput3.Text = N.ToString();
                txtOutput4.Text = TotalArea.ToString(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearText()
        {
            txtInput1.Text = "";
            txtInput2.Text = "";
            txtInput3.Text = "";
            txtOutput1.Text = "";
            txtOutput2.Text = "";
            txtOutput3.Text = "";
            txtOutput4.Text = "";

        }

        private void Clobutton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Calbutton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Add1();
            }
            else
            {
                MessageBox.Show("请选择计算类型");
            }
        }

        private void Clebutton2_Click(object sender, EventArgs e)
        {
            ClearText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LngSetup2 = new LNGSetup2();
            LngSetup2.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
