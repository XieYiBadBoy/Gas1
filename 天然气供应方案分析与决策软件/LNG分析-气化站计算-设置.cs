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
    public partial class LNGSetup2 : Form
    {
        public LNGSetup2()
        {
            InitializeComponent();
        }

   
        public string LngStroageDay
        {
            get { return this.txtInput3 .Text ; }
            set { this.txtInput3.Text = value; }
        }
        public string LngGasDensity
        {
            get { return this.txtInput4.Text; }
            set { this.txtInput4.Text = value; }
        }
        public string LngLiquidDensity
        {
            get { return this.txtInput5.Text; }
            set { this.txtInput5.Text = value; }
        }
        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        public  void txtInput3_TextChanged(object sender, EventArgs e)
        {
            LngStroageDay = txtInput3.Text;
        }

        private void groupBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public  void txtInput4_TextChanged(object sender, EventArgs e)
        {
            LngGasDensity = txtInput4.Text;
        }

       public  void txtInput5_TextChanged(object sender, EventArgs e)
        {
            LngLiquidDensity = txtInput5.Text;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void LNGSetup2_Load(object sender, EventArgs e)
        {


        }
    }
}
