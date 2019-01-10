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
    public partial class CNGStandardStationSet : Form
    {
        public CNGStandardStationSet()
        {
            InitializeComponent();
        }

        private void CNG标准站设置_Load(object sender, EventArgs e)
        {

            this.dataGridView2.Rows[0].Cells[0].Value = "1.5";
            this.dataGridView2.Rows[0].Cells[1].Value = "750";
            this.dataGridView2.Rows[0].Cells[2].Value = "0.3";
            this.dataGridView2.Rows[0].Cells[3].Value = "4";


       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
