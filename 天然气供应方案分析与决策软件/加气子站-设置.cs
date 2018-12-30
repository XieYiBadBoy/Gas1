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
    public partial class SecStatSetup : Form
    {
        public SecStatSetup()
        {
            InitializeComponent();
        }
        public string SecStationFactor
        {
            get { return this.txtInput1.Text; }
            set { this.txtInput1.Text = value; }
        }
        public string SecStationTaxiNum
        {
            get { return this.txtInput4.Text; }
            set { this.txtInput4.Text = value; }
        }
        public string SecStationBusNum
        {
            get { return this.txtInput5.Text; }
            set { this.txtInput5.Text = value; }
        }

        public string SecStationTime
        {
            get { return this.txtInput6.Text; }
            set { this.txtInput6.Text = value; }
        }
    }
}
