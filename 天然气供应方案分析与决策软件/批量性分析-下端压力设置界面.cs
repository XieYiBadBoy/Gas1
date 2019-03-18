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
    public partial class BatchAnalysisLowPressureSet : Form
    {
        public BatchAnalysisLowPressureSet()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string str1 = label1.Text;
                string str2 = txtInput1.Text;
                string str3 = label2.Text;
                string str4 = txtInput2.Text;
                string str5 = lblInput3.Text;
                string str6 = txtInput3.Text;
                string str7 = lblInput4.Text;
                string str8 = txtInput4.Text;
                string str9 = lblInput5.Text;
                string str10 = txtInput5.Text;
             

                Common.ParameterErrorDetectionFlow(str1, str2);
                Common.ParameterErrorDetectionIncrementInterval(str3,str4);
                Common.ParameterErrorDetectionRough(str5, str6);
                Common.ParameterErrorDetectionGasWeighRatio(str7, str8);
                Common.ParameterErrorDetectionAverageTemperature(str9, str10);
               

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

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/BatchAnalysis/InitialStandardFlow");
            xn0.InnerText = txtInput1.Text;

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/BatchAnalysis/Interval");
            xn1.InnerText = txtInput2.Text;

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/BatchAnalysis/DiaRough");
            xn2.InnerText = txtInput3.Text;

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/BatchAnalysis/GasWeight");
            xn3.InnerText = txtInput4.Text;
            XmlNode xn4 = xmlDoc.SelectSingleNode("configuration/BatchAnalysis/GasTep");
            xn4.InnerText = txtInput5.Text;

            xmlDoc.Save("XMLFile1.xml");
        }

        private void txtInput1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //输入0-9和Backspace del 有效
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 46)                       //小数点      
            {
                if (txtInput1.Text.Length <= 0)
                    e.Handled = true;           //小数点不能在第一位      
                else
                {
                    float f;
                    if (float.TryParse(txtInput1.Text + e.KeyChar.ToString(), out f))
                    {
                        e.Handled = false;
                    }
                }
            }
        }

        private void txtInput2_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //输入0-9和Backspace del 有效
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 46)                       //小数点      
            {
                if (txtInput2.Text.Length <= 0)
                    e.Handled = true;           //小数点不能在第一位      
                else
                {
                    float f;
                    if (float.TryParse(txtInput2.Text + e.KeyChar.ToString(), out f))
                    {
                        e.Handled = false;
                    }
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string str1 = label1.Text;
                string str2 = txtInput1.Text;
                string str3 = label2.Text;
                string str4 = txtInput2.Text;
                string str5 = lblInput3.Text;
                string str6 = txtInput3.Text;
                string str7 = lblInput4.Text;
                string str8 = txtInput4.Text;
                string str9 = lblInput5.Text;
                string str10 = txtInput5.Text;


                Common.ParameterErrorDetectionFlow(str1, str2);
                Common.ParameterErrorDetectionIncrementInterval(str3, str4);
                Common.ParameterErrorDetectionRough(str5, str6);
                Common.ParameterErrorDetectionGasWeighRatio(str7, str8);
                Common.ParameterErrorDetectionAverageTemperature(str9, str10);

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

        private void 批量性分析_下端压力设置界面_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

            txtInput1.Text = ReadXml(xmlDoc, "InitialStandardFlow");
            txtInput2.Text = ReadXml(xmlDoc, "Interval");
            txtInput3.Text = ReadXml(xmlDoc, "DiaRough");
            txtInput4.Text = ReadXml(xmlDoc, "GasWeight");
            txtInput5.Text = ReadXml(xmlDoc, "GasTep");
        }

        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/BatchAnalysis/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("OpenNewFile.gsa"); //加载xml文件

            txtInput1.Text = ReadXml(xmlDoc, "InitialStandardFlow");
            txtInput2.Text = ReadXml(xmlDoc, "Interval");
            txtInput3.Text = ReadXml(xmlDoc, "DiaRough");
            txtInput4.Text = ReadXml(xmlDoc, "GasWeight");
            txtInput5.Text = ReadXml(xmlDoc, "GasTep");
         
        }

        private void txtInput3_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //输入0-9和Backspace del 有效
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 46)                       //小数点      
            {
                if (txtInput3.Text.Length <= 0)
                    e.Handled = true;           //小数点不能在第一位      
                else
                {
                    float f;
                    if (float.TryParse(txtInput3.Text + e.KeyChar.ToString(), out f))
                    {
                        e.Handled = false;
                    }
                }
            }
        }

        private void txtInput4_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //输入0-9和Backspace del 有效
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 46)                       //小数点      
            {
                if (txtInput4.Text.Length <= 0)
                    e.Handled = true;           //小数点不能在第一位      
                else
                {
                    float f;
                    if (float.TryParse(txtInput4.Text + e.KeyChar.ToString(), out f))
                    {
                        e.Handled = false;
                    }
                }
            }
        }

        private void txtInput5_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //输入0-9和Backspace del 有效
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
            }
            if (e.KeyChar == 46)                       //小数点      
            {
                if (txtInput5.Text.Length <= 0)
                    e.Handled = true;           //小数点不能在第一位      
                else
                {
                    float f;
                    if (float.TryParse(txtInput5.Text + e.KeyChar.ToString(), out f))
                    {
                        e.Handled = false;
                    }
                }
            }
        }
    }
}
