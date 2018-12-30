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
    public partial class NatStationSetup : Form
    {
        public NatStationSetup()
        {
            InitializeComponent();
        }

        public string NatFactor
        {
            get { return this.txtInput1.Text; }
            set { this.txtInput1.Text = value; }
        }
        public string NatTime
        {
            get { return this.txtInput2.Text; }
            set { this.txtInput2.Text = value; }
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
