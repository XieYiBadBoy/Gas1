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

            try
            {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件


            textBox1.Text = ReadXml(xmlDoc, "TractorInvestment"); ;
            textBox2.Text = ReadXml(xmlDoc, "BundleInvestment");
            textBox3.Text = ReadXml(xmlDoc, "BundleScale");


            int index = this.dataGridView1.Rows.Add(5);
          

            this.dataGridView1.Rows[0].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale6");
            this.dataGridView1.Rows[0].Cells[1].Value = ReadXml(xmlDoc,"PrimaryStationScale6Investment");
            this.dataGridView1.Rows[0].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale6Area");
            this.dataGridView1.Rows[0].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale6ProjectTime");


            this.dataGridView1.Rows[1].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale5");
            this.dataGridView1.Rows[1].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale5Investment");
            this.dataGridView1.Rows[1].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale5Area");
            this.dataGridView1.Rows[1].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale5ProjectTime");

            this.dataGridView1.Rows[2].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale4");
            this.dataGridView1.Rows[2].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale4Investment");
            this.dataGridView1.Rows[2].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale4Area");
            this.dataGridView1.Rows[2].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale4ProjectTime");

            this.dataGridView1.Rows[3].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale3");
            this.dataGridView1.Rows[3].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale3Investment");
            this.dataGridView1.Rows[3].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale3Area");
            this.dataGridView1.Rows[3].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale3ProjectTime");

            this.dataGridView1.Rows[4].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale2");
            this.dataGridView1.Rows[4].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale2Investment");
            this.dataGridView1.Rows[4].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale2Area");
            this.dataGridView1.Rows[4].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale2ProjectTime");

            this.dataGridView1.Rows[5].Cells[0].Value = ReadXml(xmlDoc, "PrimaryStationScale1");
            this.dataGridView1.Rows[5].Cells[1].Value = ReadXml(xmlDoc, "PrimaryStationScale1Investment");
            this.dataGridView1.Rows[5].Cells[2].Value = ReadXml(xmlDoc, "PrimaryStationScale1Area");
            this.dataGridView1.Rows[5].Cells[3].Value = ReadXml(xmlDoc, "PrimaryStationScale1ProjectTime");


            this.dataGridView1.Rows[0].HeaderCell.Value = "1";
            this.dataGridView1.Rows[1].HeaderCell.Value = "2";
            this.dataGridView1.Rows[2].HeaderCell.Value = "3";
            this.dataGridView1.Rows[3].HeaderCell.Value = "4";
            this.dataGridView1.Rows[4].HeaderCell.Value = "5";
            this.dataGridView1.Rows[5].HeaderCell.Value = "6";

            int index1 = this.dataGridView2.Rows.Add(1);

            this.dataGridView2.Rows[0].Cells[0].Value = ReadXml(xmlDoc, "SubstationScale2");
            this.dataGridView2.Rows[0].Cells[1].Value = ReadXml(xmlDoc, "SubstationScale2Investment");
            this.dataGridView2.Rows[0].Cells[2].Value = ReadXml(xmlDoc, "SubstationScale2Area");
            this.dataGridView2.Rows[0].Cells[3].Value = ReadXml(xmlDoc, "SubstationScale2ProjectTime");


            this.dataGridView2.Rows[1].Cells[0].Value = ReadXml(xmlDoc, "SubstationScale1");
            this.dataGridView2.Rows[1].Cells[1].Value = ReadXml(xmlDoc, "SubstationScale1Investment");
            this.dataGridView2.Rows[1].Cells[2].Value = ReadXml(xmlDoc, "SubstationScale1Area");
            this.dataGridView2.Rows[1].Cells[3].Value = ReadXml(xmlDoc, "SubstationScale1ProjectTime");


            this.dataGridView2.Rows[0].HeaderCell.Value = "1";
            this.dataGridView2.Rows[1].HeaderCell.Value = "2";
            }
            catch (Exception)
            {

            }

        }
        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/CNGPrimaryStationRoughEstimate/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }
        private void SaveElement()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale6");
            xn0.InnerText = this.dataGridView1.Rows[0].Cells[0].Value.ToString();

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale5");
            xn1.InnerText = this.dataGridView1.Rows[1].Cells[0].Value.ToString();

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale4");
            xn2.InnerText = this.dataGridView1.Rows[2].Cells[0].Value.ToString();

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale3");
            xn3.InnerText = this.dataGridView1.Rows[3].Cells[0].Value.ToString();

            XmlNode xn4 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale2");
            xn4.InnerText = this.dataGridView1.Rows[4].Cells[0].Value.ToString();

            XmlNode xn5 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale1");
            xn5.InnerText = this.dataGridView1.Rows[5].Cells[0].Value.ToString();


            XmlNode xn6 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale6Investment");
            xn6.InnerText = this.dataGridView1.Rows[0].Cells[1].Value.ToString();

            XmlNode xn7 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale5Investment");
            xn7.InnerText = this.dataGridView1.Rows[1].Cells[1].Value.ToString();

            XmlNode xn8 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale4Investment");
            xn8.InnerText = this.dataGridView1.Rows[2].Cells[1].Value.ToString();

            XmlNode xn9 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale3Investment");
            xn9.InnerText = this.dataGridView1.Rows[3].Cells[1].Value.ToString();

            XmlNode xn10 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale2Investment");
            xn10.InnerText = this.dataGridView1.Rows[4].Cells[1].Value.ToString();

            XmlNode xn11 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale1Investment");
            xn11.InnerText = this.dataGridView1.Rows[5].Cells[1].Value.ToString();


            XmlNode xn12 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale6Area");
            xn12.InnerText = this.dataGridView1.Rows[0].Cells[2].Value.ToString();

            XmlNode xn13 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale5Area");
            xn13.InnerText = this.dataGridView1.Rows[1].Cells[2].Value.ToString();

            XmlNode xn14 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale4Area");
            xn14.InnerText = this.dataGridView1.Rows[2].Cells[2].Value.ToString();

            XmlNode xn15 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale3Area");
            xn15.InnerText = this.dataGridView1.Rows[3].Cells[2].Value.ToString();

            XmlNode xn16 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale2Area");
            xn16.InnerText = this.dataGridView1.Rows[4].Cells[2].Value.ToString();

            XmlNode xn17 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale1Area");
            xn17.InnerText = this.dataGridView1.Rows[5].Cells[2].Value.ToString();


            XmlNode xn18 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale6ProjectTime");
            xn18.InnerText = this.dataGridView1.Rows[0].Cells[3].Value.ToString();

            XmlNode xn19 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale5ProjectTime");
            xn19.InnerText = this.dataGridView1.Rows[1].Cells[3].Value.ToString();

            XmlNode xn20 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale4ProjectTime");
            xn20.InnerText = this.dataGridView1.Rows[2].Cells[3].Value.ToString();

            XmlNode xn21 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale3ProjectTime");
            xn21.InnerText = this.dataGridView1.Rows[3].Cells[3].Value.ToString();

            XmlNode xn22 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale2ProjectTime");
            xn22.InnerText = this.dataGridView1.Rows[4].Cells[3].Value.ToString();

            XmlNode xn23 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale1ProjectTime");
            xn23.InnerText = this.dataGridView1.Rows[5].Cells[3].Value.ToString();



            XmlNode xn24 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale2");
            xn24.InnerText = this.dataGridView2.Rows[0].Cells[0].Value.ToString();

            XmlNode xn25 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale1");
            xn25.InnerText = this.dataGridView2.Rows[1].Cells[0].Value.ToString();

            XmlNode xn26 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale2Investment");
            xn26.InnerText = this.dataGridView2.Rows[0].Cells[1].Value.ToString();

            XmlNode xn27 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale1Investment");
            xn27.InnerText = this.dataGridView2.Rows[1].Cells[1].Value.ToString();

            XmlNode xn28 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale2Area");
            xn28.InnerText = this.dataGridView2.Rows[0].Cells[2].Value.ToString();

            XmlNode xn29 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale1Area");
            xn29.InnerText = this.dataGridView2.Rows[1].Cells[2].Value.ToString();

            XmlNode xn30 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale2ProjectTime");
            xn30.InnerText = this.dataGridView2.Rows[0].Cells[3].Value.ToString();

            XmlNode xn31 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale1ProjectTime");
            xn31.InnerText = this.dataGridView2.Rows[1].Cells[3].Value.ToString();


            XmlNode xn32 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/TractorInvestment");
            xn32.InnerText = textBox1.Text;

            XmlNode xn33 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/BundleInvestment");
            xn33.InnerText = textBox2.Text;

            XmlNode xn34 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/BundleScale");
            xn34.InnerText = textBox3.Text;

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

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }
    }
}
