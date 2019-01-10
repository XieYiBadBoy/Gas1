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
    public partial class CNGSubstationSet : Form
    {
        public CNGSubstationSet()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CNG子母站设置_Load(object sender, EventArgs e)
        {
            textBox1.Text = Properties.Settings.Default.a;
            textBox2.Text = Properties.Settings.Default.b;
            textBox3.Text = Properties.Settings.Default.c;
            int index = this.dataGridView1.Rows.Add(5);
          

            this.dataGridView1.Rows[0].Cells[0].Value = "10";
            this.dataGridView1.Rows[0].Cells[1].Value = "2000";
            this.dataGridView1.Rows[0].Cells[2].Value = "1";
            this.dataGridView1.Rows[0].Cells[3].Value = "5";
            

            this.dataGridView1.Rows[1].Cells[0].Value = "15";
            this.dataGridView1.Rows[1].Cells[1].Value = "2500";
            this.dataGridView1.Rows[1].Cells[2].Value = "1.3";
            this.dataGridView1.Rows[1].Cells[3].Value = "5";

            this.dataGridView1.Rows[2].Cells[0].Value = "20";
            this.dataGridView1.Rows[2].Cells[1].Value = "2800";
            this.dataGridView1.Rows[2].Cells[2].Value = "1.5";
            this.dataGridView1.Rows[2].Cells[3].Value = "5";

            this.dataGridView1.Rows[3].Cells[0].Value = "25";
            this.dataGridView1.Rows[3].Cells[1].Value = "3100";
            this.dataGridView1.Rows[3].Cells[2].Value = "1.7";
            this.dataGridView1.Rows[3].Cells[3].Value = "6";

            this.dataGridView1.Rows[4].Cells[0].Value = "30";
            this.dataGridView1.Rows[4].Cells[1].Value = "3500";
            this.dataGridView1.Rows[4].Cells[2].Value = "2";
            this.dataGridView1.Rows[4].Cells[3].Value = "7";

            this.dataGridView1.Rows[5].Cells[0].Value = "50";
            this.dataGridView1.Rows[5].Cells[1].Value = "4500";
            this.dataGridView1.Rows[5].Cells[2].Value = "2.5";
            this.dataGridView1.Rows[5].Cells[3].Value = "8";


            this.dataGridView1.Rows[0].HeaderCell.Value = "1";
            this.dataGridView1.Rows[1].HeaderCell.Value = "2";
            this.dataGridView1.Rows[2].HeaderCell.Value = "3";
            this.dataGridView1.Rows[3].HeaderCell.Value = "4";
            this.dataGridView1.Rows[4].HeaderCell.Value = "5";
            this.dataGridView1.Rows[5].HeaderCell.Value = "6";

            int index1 = this.dataGridView2.Rows.Add(1);

            this.dataGridView2.Rows[0].Cells[0].Value = "1.5";
            this.dataGridView2.Rows[0].Cells[1].Value = "750";
            this.dataGridView2.Rows[0].Cells[2].Value = "0.3";
            this.dataGridView2.Rows[0].Cells[3].Value = "4";


            this.dataGridView2.Rows[1].Cells[0].Value = "2";
            this.dataGridView2.Rows[1].Cells[1].Value = "900";
            this.dataGridView2.Rows[1].Cells[2].Value = "0.4";
            this.dataGridView2.Rows[1].Cells[3].Value = "5";


            this.dataGridView2.Rows[0].HeaderCell.Value = "1";
            this.dataGridView2.Rows[1].HeaderCell.Value = "2";

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.a = textBox1.Text;
            Properties.Settings.Default.b = textBox2.Text;
            Properties.Settings.Default.c = textBox3.Text;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.a = textBox1.Text;
            Properties.Settings.Default.b = textBox2.Text;
            Properties.Settings.Default.c = textBox3.Text;
            Properties.Settings.Default.Save();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.Reset();
            textBox1.Text = Properties.Settings.Default.a;
            textBox2.Text = Properties.Settings.Default.b;
            textBox3.Text = Properties.Settings.Default.c;
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }
}
