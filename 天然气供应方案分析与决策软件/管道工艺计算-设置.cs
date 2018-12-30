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
    public partial class PipeSetup1 : Form
    {
        public PipeSetup1()
        {
            InitializeComponent();
        }
        public string PipeRough
        {
            get { return this.txtInput1.Text; }
            set { this.txtInput1.Text = value; }
        }
        public string PipeGasWeight
        {
            get { return this.txtInput2.Text; }
            set { this.txtInput2.Text = value; }
        }
        public string PipeTep
        {
            get { return this.txtInput3.Text; }
            set { this.txtInput3.Text = value; }
        }
        private void Set_Load(object sender, EventArgs e)
        {

        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}
