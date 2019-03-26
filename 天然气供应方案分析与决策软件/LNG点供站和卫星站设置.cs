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
    public partial class LNGProjectAndInvestmentSet : Form
    {
        public LNGProjectAndInvestmentSet()
        {
            InitializeComponent();
        }

        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/LNGGasificattionStationRoughEstimate/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }
        private void LNG点供站和卫星站设置_Load(object sender, EventArgs e)
        {
           
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("XMLFile1.xml"); //加载xml文件


                textBox1.Text = ReadXml(xmlDoc, "PrimaryStationGasificattionRate"); 
                textBox2.Text = ReadXml(xmlDoc, "PrimaryStationColdInsulation");


                int index = this.dataGridView1.Rows.Add(5);


                this.dataGridView1.Rows[0].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale6");
                this.dataGridView1.Rows[0].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale6Investment");
                this.dataGridView1.Rows[0].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale6Area");
                this.dataGridView1.Rows[0].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale6ProjectTime");
                this.dataGridView1.Rows[0].Cells[4].Value = ReadXml(xmlDoc, "PrimaryStationScale6TurnoverRate");
                this.dataGridView1.Rows[0].Cells[5].Value = ReadXml(xmlDoc, "PrimaryStationScale6OperationCost");


                this.dataGridView1.Rows[1].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale5");
                this.dataGridView1.Rows[1].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale5Investment");
                this.dataGridView1.Rows[1].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale5Area");
                this.dataGridView1.Rows[1].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale5ProjectTime");
                this.dataGridView1.Rows[1].Cells[4].Value = ReadXml(xmlDoc, "PrimaryStationScale5TurnoverRate");
                this.dataGridView1.Rows[1].Cells[5].Value = ReadXml(xmlDoc, "PrimaryStationScale5OperationCost");

                this.dataGridView1.Rows[2].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale4");
                this.dataGridView1.Rows[2].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale4Investment");
                this.dataGridView1.Rows[2].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale4Area");
                this.dataGridView1.Rows[2].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale4ProjectTime");
                this.dataGridView1.Rows[2].Cells[4].Value = ReadXml(xmlDoc, "PrimaryStationScale4TurnoverRate");
                this.dataGridView1.Rows[2].Cells[5].Value = ReadXml(xmlDoc, "PrimaryStationScale4OperationCost");

                      this.dataGridView1.Rows[3].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale3");
                this.dataGridView1.Rows[3].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale3Investment");
                this.dataGridView1.Rows[3].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale3Area");
                this.dataGridView1.Rows[3].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale3ProjectTime");
                this.dataGridView1.Rows[3].Cells[4].Value = ReadXml(xmlDoc, "PrimaryStationScale3TurnoverRate");
                this.dataGridView1.Rows[3].Cells[5].Value = ReadXml(xmlDoc, "PrimaryStationScale3OperationCost");

                this.dataGridView1.Rows[4].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale2");
                this.dataGridView1.Rows[4].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale2Investment");
                this.dataGridView1.Rows[4].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale2Area");
                this.dataGridView1.Rows[4].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale2ProjectTime");
                this.dataGridView1.Rows[4].Cells[4].Value = ReadXml(xmlDoc, "PrimaryStationScale2TurnoverRate");
                this.dataGridView1.Rows[4].Cells[5].Value = ReadXml(xmlDoc, "PrimaryStationScale2OperationCost");

                this.dataGridView1.Rows[5].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale1");
                this.dataGridView1.Rows[5].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale1Investment");
                this.dataGridView1.Rows[5].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale1Area");
                this.dataGridView1.Rows[5].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale1ProjectTime");
                this.dataGridView1.Rows[5].Cells[4].Value = ReadXml(xmlDoc, "PrimaryStationScale1TurnoverRate");
                this.dataGridView1.Rows[5].Cells[5].Value = ReadXml(xmlDoc, "PrimaryStationScale1OperationCost");


                this.dataGridView1.Rows[0].HeaderCell.Value = "1";
                this.dataGridView1.Rows[1].HeaderCell.Value = "2";
                this.dataGridView1.Rows[2].HeaderCell.Value = "3";
                this.dataGridView1.Rows[3].HeaderCell.Value = "4";
                this.dataGridView1.Rows[4].HeaderCell.Value = "5";
                this.dataGridView1.Rows[5].HeaderCell.Value = "6";

               dataGridView1.EnableHeadersVisualStyles = false;


            //设置标题高度;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView1.ColumnHeadersHeight = 40;
            //设置标题内容居中显示;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dataGridView1.Columns[0].HeaderText = "母站规模\r\n" + "（万方 / 天）";
            this.dataGridView1.Columns[1].HeaderText = "投资\r\n" + "（万元）";
            this.dataGridView1.Columns[2].HeaderText = "占地面积\r\n" + "（万平方米）";
            this.dataGridView1.Columns[3].HeaderText = "工期\r\n" + "（月）";
            this.dataGridView1.Columns[4].HeaderText = "周转率\r\n" + "（天/次）";
            this.dataGridView1.Columns[5].HeaderText = "气化运行成本\r\n" + "（元/方）";


        }
        private void SaveElement()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale6");
            xn0.InnerText = this.dataGridView1.Rows[0].Cells[0].Value.ToString();

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale5");
            xn1.InnerText = this.dataGridView1.Rows[1].Cells[0].Value.ToString();

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale4");
            xn2.InnerText = this.dataGridView1.Rows[2].Cells[0].Value.ToString();

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale3");
            xn3.InnerText = this.dataGridView1.Rows[3].Cells[0].Value.ToString();

            XmlNode xn4 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale2");
            xn4.InnerText = this.dataGridView1.Rows[4].Cells[0].Value.ToString();

            XmlNode xn5 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale1");
            xn5.InnerText = this.dataGridView1.Rows[5].Cells[0].Value.ToString();


            XmlNode xn6 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale6Investment");
            xn6.InnerText = this.dataGridView1.Rows[0].Cells[1].Value.ToString();

            XmlNode xn7 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale5Investment");
            xn7.InnerText = this.dataGridView1.Rows[1].Cells[1].Value.ToString();

            XmlNode xn8 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale4Investment");
            xn8.InnerText = this.dataGridView1.Rows[2].Cells[1].Value.ToString();

            XmlNode xn9 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale3Investment");
            xn9.InnerText = this.dataGridView1.Rows[3].Cells[1].Value.ToString();

            XmlNode xn10 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale2Investment");
            xn10.InnerText = this.dataGridView1.Rows[4].Cells[1].Value.ToString();

            XmlNode xn11 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale1Investment");
            xn11.InnerText = this.dataGridView1.Rows[5].Cells[1].Value.ToString();


            XmlNode xn12 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale6Area");
            xn12.InnerText = this.dataGridView1.Rows[0].Cells[2].Value.ToString();

            XmlNode xn13 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale5Area");
            xn13.InnerText = this.dataGridView1.Rows[1].Cells[2].Value.ToString();

            XmlNode xn14 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale4Area");
            xn14.InnerText = this.dataGridView1.Rows[2].Cells[2].Value.ToString();

            XmlNode xn15 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale3Area");
            xn15.InnerText = this.dataGridView1.Rows[3].Cells[2].Value.ToString();

            XmlNode xn16 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale2Area");
            xn16.InnerText = this.dataGridView1.Rows[4].Cells[2].Value.ToString();

            XmlNode xn17 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale1Area");
            xn17.InnerText = this.dataGridView1.Rows[5].Cells[2].Value.ToString();


            XmlNode xn18 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale6ProjectTime");
            xn18.InnerText = this.dataGridView1.Rows[0].Cells[3].Value.ToString();

            XmlNode xn19 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale5ProjectTime");
            xn19.InnerText = this.dataGridView1.Rows[1].Cells[3].Value.ToString();

            XmlNode xn20 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale4ProjectTime");
            xn20.InnerText = this.dataGridView1.Rows[2].Cells[3].Value.ToString();

            XmlNode xn21 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale3ProjectTime");
            xn21.InnerText = this.dataGridView1.Rows[3].Cells[3].Value.ToString();

            XmlNode xn22 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale2ProjectTime");
            xn22.InnerText = this.dataGridView1.Rows[4].Cells[3].Value.ToString();

            XmlNode xn23 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale1ProjectTime");
            xn23.InnerText = this.dataGridView1.Rows[5].Cells[3].Value.ToString();

            XmlNode xn24 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationGasificattionRate");
            xn24.InnerText = textBox1.Text;

            XmlNode xn25 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationColdInsulation");
            xn25.InnerText = textBox2.Text;

            XmlNode xn26 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale6TurnoverRate");
            xn26.InnerText = this.dataGridView1.Rows[0].Cells[4].Value.ToString();

            XmlNode xn27 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale5TurnoverRate");
            xn27.InnerText = this.dataGridView1.Rows[1].Cells[4].Value.ToString();

            XmlNode xn28 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale4TurnoverRate");
            xn28.InnerText = this.dataGridView1.Rows[2].Cells[4].Value.ToString();

            XmlNode xn29 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale3TurnoverRate");
            xn29.InnerText = this.dataGridView1.Rows[3].Cells[4].Value.ToString();

            XmlNode xn30 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale2TurnoverRate");
            xn30.InnerText = this.dataGridView1.Rows[4].Cells[4].Value.ToString();

            XmlNode xn31= xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale1TurnoverRate");
            xn31.InnerText = this.dataGridView1.Rows[5].Cells[4].Value.ToString();


            XmlNode xn32 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale6OperationCost");
            xn32.InnerText = this.dataGridView1.Rows[0].Cells[5].Value.ToString();

            XmlNode xn33 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale5OperationCost");
            xn33.InnerText = this.dataGridView1.Rows[1].Cells[5].Value.ToString();

            XmlNode xn34 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale4OperationCost");
            xn34.InnerText = this.dataGridView1.Rows[2].Cells[5].Value.ToString();

            XmlNode xn35 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale3OperationCost");
            xn35.InnerText = this.dataGridView1.Rows[3].Cells[5].Value.ToString();

            XmlNode xn36= xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale2OperationCost");
            xn36.InnerText = this.dataGridView1.Rows[4].Cells[5].Value.ToString();

            XmlNode xn37 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale1OperationCost");
            xn37.InnerText = this.dataGridView1.Rows[5].Cells[5].Value.ToString();




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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string[] data = textBox1.Text.Split(',');
            int i = 0;
            foreach(var str in data)
            {
                if (i == 6) break;
                dataGridView1.Rows[i].Cells[0].Value = str;
                i++;
            }
        }
    }
}
