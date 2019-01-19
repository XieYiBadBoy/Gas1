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

namespace 天然气供应方案分析与决策软件
{
    public partial class CNGStandardStationSet : Form
    {
        public CNGStandardStationSet()
        {
            InitializeComponent();
        }

        private string XMLRead(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/CNGStandardStationRoughEstimate/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }
        private void CNG标准站设置_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

            this.dataGridView2.Rows[0].Cells[0].Value = XMLRead(xmlDoc, "StandardStationScale");
            this.dataGridView2.Rows[0].Cells[1].Value = XMLRead(xmlDoc, "StandardStationInvestment");
            this.dataGridView2.Rows[0].Cells[2].Value = XMLRead(xmlDoc, "StandardStationArea");
            this.dataGridView2.Rows[0].Cells[3].Value = XMLRead(xmlDoc, "SandardStationProjectTime");

            this.dataGridView2.Rows[0].HeaderCell.Value = "1";

            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.RowTemplate.Height = 40; //改变行的高度;
                                                   //设置标题高度;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView2.ColumnHeadersHeight = 40;
            //设置标题内容居中显示;
            dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dataGridView2.Columns[0].HeaderText = "母站规模\r\n" + "（万方 / 天）";
            this.dataGridView2.Columns[1].HeaderText = "投资\r\n" + "（万元）";
            this.dataGridView2.Columns[2].HeaderText = "占地面积\r\n" + "（万平方米）";
            this.dataGridView2.Columns[3].HeaderText = "工期\r\n" + "（月）";

        }
        private void SaveElement()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/CNGStandardStationRoughEstimate/StandardStationScale");
            xn0.InnerText = this.dataGridView2.Rows[0].Cells[0].Value.ToString();

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/CNGStandardStationRoughEstimate/StandardStationInvestment");
            xn1.InnerText = this.dataGridView2.Rows[0].Cells[1].Value.ToString();

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/CNGStandardStationRoughEstimate/StandardStationArea");
            xn2.InnerText = this.dataGridView2.Rows[0].Cells[2].Value.ToString();

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/CNGStandardStationRoughEstimate/SandardStationProjectTime");
            xn3.InnerText = this.dataGridView2.Rows[0].Cells[3].Value.ToString();

            xmlDoc.Save("XMLFile1.xml");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveElement();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SaveElement();
        }
    }
}
