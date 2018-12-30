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
    public partial class Windows4 : Form
    {
        public Windows4()
        {
            InitializeComponent();
        }

        private void Clobutton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Calbutton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {

            }
            else
            {
                MessageBox.Show("请选择计算类型");

            }
        }
        private void Clear1()
        {
            txtInput1.Text = "";
            txtInput2.Text = "";
            txtOutput1.Text = "";
        }

        private void Clebutton2_Click(object sender, EventArgs e)
        {
            Clear1();        }
    }
}
