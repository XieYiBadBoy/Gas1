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
    public partial class SetStationSetup : Form
    {
        public SetStationSetup()
        {
            InitializeComponent();
        }
        public string SecStationFactor
        {
            get { return this.txtInput1.Text; }
            set { this.txtInput1.Text = value; }
        }
        public string SecStationTaxiNum
        {
            get { return this.txtInput4.Text; }
            set { this.txtInput4.Text = value; }
        }
        public string SecStationBusNum
        {
            get { return this.txtInput5.Text; }
            set { this.txtInput5.Text = value; }
        }

        public string SecStationTime
        {
            get { return this.txtInput2.Text; }
            set { this.txtInput2.Text = value; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //try
            //{
                string str1 = label1.Text;
                string str2 = txtInput1.Text;
                string str3 = label2.Text;
                string str4 = txtInput2.Text;
                string str5 = lblInput12.Text;
                string str6 = txtInput4.Text;
                string str7 = lblInput13.Text;
                string str8 = txtInput5.Text;

                Common.ParameterErrorDetectionCompressureFacator(str1, str2);
                Common.ParameterErrorDetectionDailyWorkTime(str3, str4);
                Common.ParameterErrorDetectionTaxiCount(str5, str6);
                Common.ParameterErrorDetectionBusCount(str7, str8);
                SaveParameter();
                this.Close();
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}

        }

        private void SecStatSetup_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件
            txtInput1.Text = ReadXml(xmlDoc, "CompressureFator");
            txtInput2.Text = ReadXml(xmlDoc, "DailyWorkTime");
            txtInput4.Text = ReadXml(xmlDoc, "TaxiCount");
            txtInput5.Text = ReadXml(xmlDoc, "BusCount");
        }

        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/CNGStandardStation/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            //try
            //{
                string str1 = label1.Text;
                string str2 = txtInput1.Text;
                string str3 = label2.Text;
                string str4 = txtInput2.Text;
                string str5 = lblInput12.Text;
                string str6 = txtInput4.Text;
                string str7 = lblInput13.Text;
                string str8 = txtInput5.Text;

                Common.ParameterErrorDetectionCompressureFacator(str1,str2);
                Common.ParameterErrorDetectionDailyWorkTime(str3,str4);
                Common.ParameterErrorDetectionTaxiCount(str5 ,str6);
                Common.ParameterErrorDetectionBusCount(str7,str8);
                SaveParameter();
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
        }

        private void SaveParameter()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/CompressureFator");
            xn0.InnerText = txtInput1.Text;

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/DailyWorkTime");
            xn1.InnerText = txtInput2.Text;

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/TaxiCount");
            xn2.InnerText = txtInput4.Text;

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/BusCount");
            xn3.InnerText = txtInput5.Text;

            xmlDoc.Save("XMLFile1.xml");
        }
    }
}
