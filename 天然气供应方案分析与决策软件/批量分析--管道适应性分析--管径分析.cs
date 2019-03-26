using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace 天然气供应方案分析与决策软件
{
    public partial class PlumberDiameterAnalysis : Form
    {
        public PlumberDiameterAnalysis()
        {
            InitializeComponent();
        }

        private Common CommonCalcuiate = new Common();
        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        private void Calculate()
        {
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("XMLFile1.xml"); //加载xml文件
                string str1 = lblInput2.Text;
                string str2 = txtInput2.Text;
                string str3 = lblInput3.Text;
                string str4 = txtInput3.Text;
                string str5 = lblInput4.Text;
                string str6 = txtInput4.Text;
                Common.ParameterErrorDetectioPressure(str1, str2);
                Common.ParameterErrorDetectioLength(str3, str4);
                Common.ParameterErrorDetectioDiameter(str5, str6);

                int Rownumber = Convert.ToInt32(txtInput1.Text);
                double UpPressure = Convert.ToDouble(txtInput2.Text);
                double PipeLength = Convert.ToDouble(txtInput3.Text);
                double PipeDiameter = Convert.ToDouble(txtInput4.Text);

                double InitialStandardFlow = Convert.ToDouble(ReadXml(xmlDoc, "InitialStandardFlow"));
                double DiaRough = Convert.ToDouble(ReadXml(xmlDoc, "DiaRough"));
                double GasWeigh = Convert.ToDouble(ReadXml(xmlDoc, "GasWeight"));
                double AverageTep = Convert.ToDouble(ReadXml(xmlDoc, "GasTep"));
                double Interval = Convert.ToDouble(ReadXml(xmlDoc, "Interval"));
                int k = 1;
                for (int i = 0; i < Rownumber; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = Convert.ToString(k);
                    this.dataGridView2.Rows[i].HeaderCell.Value = Convert.ToString(k);
                    InitialStandardFlow = Convert.ToDouble(this.dataGridView1.Rows[i].Cells[0].Value);
                    this.dataGridView2.Rows[i].Cells[0].Value = CommonCalcuiate.LowPressureAnalysis(InitialStandardFlow, UpPressure, PipeDiameter, DiaRough, PipeLength, GasWeigh, AverageTep).ToString("0.0000");
                    this.dataGridView2.Rows[i].Cells[1].Value = CommonCalcuiate.FlowSpeed(InitialStandardFlow, UpPressure, PipeDiameter, DiaRough, PipeLength, GasWeigh, AverageTep).ToString("0.0000");
                    this.dataGridView2.Rows[i].Cells[2].Value = CommonCalcuiate.LeiNuoXiShuVar(InitialStandardFlow, UpPressure, PipeDiameter, DiaRough, PipeLength, GasWeigh, AverageTep).ToString("0.0000");
                    this.dataGridView2.Rows[i].Cells[3].Value = CommonCalcuiate.DaXiXiShuVar(InitialStandardFlow, UpPressure, PipeDiameter, DiaRough, PipeLength, GasWeigh, AverageTep).ToString("0.000000");
                    k++;
                }
                Properties.Settings.Default.rowcount = dataGridView1.Rows.Count;
                Properties.Settings.Default.Save();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private object ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/BatchAnalysis/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }

        private void PlumberDiameterAnalysis_Load(object sender, EventArgs e)
        {
            dataGridView2.TopLeftHeaderCell.Value = "序号";

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


            dataGridView1.TopLeftHeaderCell.Value = "序号";

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

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
