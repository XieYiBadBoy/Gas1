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
    public partial class StdSetup2 : Form
    {
        public StdSetup2()
        {
            InitializeComponent();
        }
     
        public string StdFactor
        {
            get { return this.txtInput1.Text; }
            set { this.txtInput1.Text = value; }
        }
        public string StdLowProp
        {
            get { return this.txtInput2.Text; }
            set { this.txtInput2.Text = value; }
        }
        public string StdMidProp
        {
            get { return this.txtInput3.Text; }
            set { this.txtInput3.Text = value; }
        }
     
        public string StdTime
        {
            get { return this.txtInput6.Text; }
            set { this.txtInput6.Text = value; }
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void StdSetup2_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件
            txtInput1.Text = ReadXml(xmlDoc, "CompressureFator");
            txtInput6.Text = ReadXml(xmlDoc, "DailyWorkTime");
            txtInput2.Text = ReadXml(xmlDoc, "LowPressureVolumeRatio");
            txtInput3.Text = ReadXml(xmlDoc, "MiddlePressureVolumeRatio");
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("OpenNewFile.gsa"); //加载xml文件

            txtInput1.Text = ReadXml(xmlDoc, "CompressureFator");
            txtInput6.Text = ReadXml(xmlDoc, "DailyWorkTime");
            txtInput2.Text = ReadXml(xmlDoc, "LowPressureVolumeRatio");
            txtInput3.Text = ReadXml(xmlDoc, "MiddlePressureVolumeRatio");
        }

        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/CNGStandard/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string str1 = label1.Text;
                string str2 = txtInput1.Text;
                string str3 = lblInput3.Text;
                string str4 = txtInput6.Text;
                string str5 = lblInput6.Text;
                string str6 = txtInput2.Text;
                string str7 = lblInput7.Text;
                string str8 = txtInput3.Text;

                Common.ParameterErrorDetectionCompressureFacator(str1, str2);
                Common.ParameterErrorDetectionDailyWorkTime(str3, str4);
                Common.ParameterErrorDetectionTaxiCount(str5, str6);
                Common.ParameterErrorDetectionBusCount(str7, str8);
                SaveParameter();
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void SaveParameter()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/CNGStandard/CompressureFator");
            xn0.InnerText = txtInput1.Text;

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/CNGStandard/DailyWorkTime");
            xn1.InnerText = txtInput6.Text;

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/CNGStandard/LowPressureVolumeRatio");
            xn2.InnerText = txtInput2.Text;

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/CNGStandard/MiddlePressureVolumeRatio");
            xn3.InnerText = txtInput3.Text;

            xmlDoc.Save("XMLFile1.xml");

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string str1 = label1.Text;
                string str2 = txtInput1.Text;
                string str3 = lblInput3.Text;
                string str4 = txtInput6.Text;
                string str5 = lblInput6.Text;
                string str6 = txtInput2.Text;
                string str7 = lblInput7.Text;
                string str8 = txtInput3.Text;

                Common.ParameterErrorDetectionCompressureFacator(str1, str2);
                Common.ParameterErrorDetectionDailyWorkTime(str3, str4);
                Common.ParameterErrorDetectionTaxiCount(str5, str6);
                Common.ParameterErrorDetectionBusCount(str7, str8);
                SaveParameter();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
