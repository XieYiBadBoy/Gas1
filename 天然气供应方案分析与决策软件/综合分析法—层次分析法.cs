using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace 天然气供应方案分析与决策软件
{
    public partial class ComprisiveAnalysisAHP : Form
    {
        public ComprisiveAnalysisAHP()
        {
            InitializeComponent();
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
        

        private void ComprisiveAnalysisAHP_Load(object sender, EventArgs e)
        {
            int index1 = this.dataGridView1.Rows.Add(4);
            //dataGridView1.ColumnHeadersHeight =60;
            dataGridView1.EnableHeadersVisualStyles = false;
            this.dataGridView1.RowHeadersWidth = 200;//DisableResizing
            dataGridView1.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            //dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Rows[0].HeaderCell.Value = "建设费用（万元）";
            this.dataGridView1.Rows[1].HeaderCell.Value = "年均运行维护费用（万元/年）";
            this.dataGridView1.Rows[2].HeaderCell.Value = "投资回收期(年)";
            this.dataGridView1.Rows[3].HeaderCell.Value = "建设周期（年）";
            this.dataGridView1.Rows[4].HeaderCell.Value = "生命周期（年）";
            this.dataGridView1.TopLeftHeaderCell.Value = "名称";
          
        }
    }
}
