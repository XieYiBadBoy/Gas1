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
    public partial class StdSetup2 : Form
    {
        public StdSetup2()
        {
            InitializeComponent();
        }
     
        public string StdFactor
        {
            get { return this.txtInput1.Text; }
            set { this.txtInput1.Text = value; }
        }
        public string StdTaxiNum
        {
            get { return this.txtInput4.Text; }
            set { this.txtInput4.Text = value; }
        }
        public string StdBusNum
        {
            get { return this.txtInput5.Text; }
            set { this.txtInput5.Text = value; }
        }
     
        public string StdTime
        {
            get { return this.txtInput6.Text; }
            set { this.txtInput6.Text = value; }
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void StdSetup2_Load(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}
