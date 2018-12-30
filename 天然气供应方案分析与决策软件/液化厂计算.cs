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
    public partial class Windows6 : Form
    {
        public Windows6()
        {
            InitializeComponent();
        }
        private void Add1()
        {
            try
            {
                double StdFlow1 = Convert.ToDouble(txtInput1.Text);    //液化工厂设计规模
                double ProCap = Convert.ToDouble(txtInput2.Text);      //液化工厂生产能力      
                double StorageCap = 7 * StdFlow1 / 625;
                double TankCarNumber = Math.Ceiling (ProCap / 21.3 )+ 1; //液化厂槽车数
                txtOutput4.Text = TankCarNumber.ToString();
                if (StorageCap <= 2000)             //计算储罐设计规模及个数
                {
                    txtOutput1.Text = "1000";
                    txtOutput2.Text = "2";
                    txtOutput3.Text = "3";
                    double FloorSpace = 38000 + 3000*2 + 250 * TankCarNumber;
                    txtOutput5.Text = FloorSpace.ToString();
                }
                else if (StorageCap <= 10000 && StorageCap > 2000)
                {
                    double Variable1 = Math.Ceiling(StorageCap / 20) * 10;//储罐设计规模取整到10的中间变量
                    txtOutput1.Text = Variable1.ToString();
                    txtOutput2.Text = "2";
                    txtOutput3.Text = "3";
                    double FloorSpace = 38000 + 3000*2 + 250 * TankCarNumber;
                    txtOutput5.Text = FloorSpace.ToString();
                }
                else
                {
                    txtOutput1.Text = "5000";
                    double Variable2 =Math.Ceiling( (StorageCap - 10000) / 5000)+2;  //储罐个数
                    double Variable3 = Math.Ceiling((StorageCap - 10000) / 5000) + 3;//离心泵个数
                    txtOutput2.Text = Variable2.ToString();
                    txtOutput3.Text =Variable3.ToString();
                    double FloorSpace = 38000 +3000* Variable2 + 250 * TankCarNumber;
                    txtOutput5.Text = FloorSpace.ToString();
                }  
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
            txtOutput1.Text = "";
            txtOutput2.Text = "";
            txtOutput3.Text = "";
            txtOutput4.Text = "";
            txtOutput5.Text = "";

        }

        private void 液化厂计算_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
        
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

        private void Clobutton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Clebutton2_Click(object sender, EventArgs e)
        {
            ClearText();
        }
    }
}
