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
    public partial class OutputAmountAnalysis : Form
    {
        public OutputAmountAnalysis()
        {
            InitializeComponent();
        }

        private void 批量分析__管道适应性分析__输量分析_Load(object sender, EventArgs e)
        {
            this.dataGridView2.Rows[0].HeaderCell.Value = "1";

            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.RowTemplate.Height = 40; //改变行的高度;
                                                   //设置标题高度;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView2.ColumnHeadersHeight = 40;
            //设置标题内容居中显示;
            dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dataGridView2.Columns[0].HeaderText = "输量\r\n" + "（立方米/天）";
            this.dataGridView2.Columns[1].HeaderText = "流速\r\n" + "（米/秒）";
            this.dataGridView2.Columns[2].HeaderText = "雷诺数\r\n" + "（无单位）";
            this.dataGridView2.Columns[3].HeaderText = "达西摩阻系数\r\n" + "（无单位）";


            this.dataGridView1.Rows[0].HeaderCell.Value = "1";

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.RowTemplate.Height = 40; //改变行的高度;
                                                   //设置标题高度;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ColumnHeadersHeight = 40;
            //设置标题内容居中显示;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
