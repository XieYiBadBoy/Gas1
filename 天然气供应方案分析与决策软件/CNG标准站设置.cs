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
