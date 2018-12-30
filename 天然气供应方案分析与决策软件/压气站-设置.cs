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
    public partial class CompressorStation : Form
    {
        public CompressorStation()
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
    }
}
