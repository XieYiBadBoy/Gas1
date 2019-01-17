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
    public partial class CriticalCurveMethodSetting : Form
    {
        public CriticalCurveMethodSetting()
        {
            InitializeComponent();
        }

        private void CriticalCurveMethodSetting_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

            textBox2.Text = ReadXml(xmlDoc, "PNGCoefficientA");
            textBox3.Text = ReadXml(xmlDoc, "PNGCoefficientB");
            textBox4.Text = ReadXml(xmlDoc, "PNGCoefficientC");
            textBox5.Text = ReadXml(xmlDoc, "PNGCoefficientD");

            textBox8.Text = ReadXml(xmlDoc, "LNGCoefficientA");
            textBox9.Text = ReadXml(xmlDoc, "LNGCoefficientB");
            textBox10.Text = ReadXml(xmlDoc,"LNGCoefficientC");
            textBox11.Text = ReadXml(xmlDoc, "LNGCoefficientD");

            textBox14.Text = ReadXml(xmlDoc, "CNGCoefficientA");
            textBox15.Text = ReadXml(xmlDoc, "CNGCoefficientB");
            textBox16.Text = ReadXml(xmlDoc, "CNGCoefficientC");
            textBox17.Text = ReadXml(xmlDoc, "CNGCoefficientD");


        }

        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/ComprehensiveAnalysis/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }
        private void Savement()
        {
            try
            {


           
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件


            if (textBox2.Text==""||textBox3.Text==""|| textBox4.Text == "" || textBox5.Text == "")
            {
                throw new InvalidOperationException("您的设置输入为空，计算对象无效!");
            }


            if (textBox8.Text == "" || textBox9.Text == "" || textBox10.Text == "" || textBox11.Text == "")
            {
                throw new InvalidOperationException("您的设置输入为空，计算对象无效!");
            }


            if (textBox14.Text == "" || textBox15.Text == "" || textBox16.Text == "" || textBox17.Text == "")
            {
                throw new InvalidOperationException("您的设置输入为空，计算对象无效!");
            }

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/PNGCoefficientA");
            xn0.InnerText = textBox2.Text;

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/PNGCoefficientB");
            xn1.InnerText = textBox3.Text;

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/PNGCoefficientC");
            xn3.InnerText = textBox4.Text;

            XmlNode xn4 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/PNGCoefficientD");
            xn4.InnerText = textBox5.Text;

            XmlNode xn5 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/LNGCoefficientA");
            xn5.InnerText = textBox8.Text;

            XmlNode xn6 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/LNGCoefficientB");
            xn6.InnerText = textBox9.Text;

            XmlNode xn7 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/LNGCoefficientC");
            xn7.InnerText = textBox10.Text;

            XmlNode xn8 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/LNGCoefficientD");
            xn8.InnerText = textBox11.Text;

            XmlNode xn9 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/CNGCoefficientA");
            xn9.InnerText = textBox14.Text;

            XmlNode xn10 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/CNGCoefficientB");
            xn10.InnerText = textBox15.Text;

            XmlNode xn11 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/CNGCoefficientC");
            xn11.InnerText = textBox16.Text;

            XmlNode xn12 = xmlDoc.SelectSingleNode("configuration/ComprehensiveAnalysis/CNGCoefficientD");
            xn12.InnerText = textBox17.Text;

          xmlDoc.Save("XMLFile1.xml");

                if (Convert.ToDouble(textBox3.Text) < 0)
                {
                    if (Convert.ToDouble(textBox5.Text) < 0)
                    {
                        textBox6.Text = "q=(" + textBox2.Text + "L" + textBox3.Text + ")/(" + textBox4.Text + "L" + textBox5.Text + ")";
                    }
                    else
                    {
                        textBox6.Text = "q=(" + textBox2.Text + "L" + textBox3.Text + ")/(" + textBox4.Text + "L" + "+" + textBox5.Text + ")";
                    }
                }
                else
                {
                    if (Convert.ToDouble(textBox5.Text) < 0)
                    {
                        textBox6.Text = "q=(" + textBox2.Text + "L" + "+" + textBox3.Text + ")/(" + textBox4.Text + "L" + textBox5.Text + ")";
                    }
                    else
                    {
                        textBox6.Text = "q=(" + textBox2.Text + "L" + "+" + textBox3.Text + ")/(" + textBox4.Text + "L" + "+" + textBox5.Text + ")";
                    }
                }


                if (Convert.ToDouble(textBox9.Text) < 0)
                {
                    if (Convert.ToDouble(textBox11.Text) < 0)
                    {
                        textBox7.Text = "q=(" + textBox8.Text + "L" + textBox9.Text + ")/(" + textBox10.Text + "L" + textBox11.Text + ")";
                    }
                    else
                    {
                        textBox7.Text = "q=(" + textBox8.Text + "L" + textBox9.Text + ")/(" + textBox10.Text + "L" + "+" + textBox11.Text + ")";
                    }
                }
                else
                {
                    if (Convert.ToDouble(textBox11.Text) < 0)
                    {
                        textBox7.Text = "q=(" + textBox8.Text + "L" + "+" + textBox9.Text + ")/(" + textBox10.Text + "L" + textBox11.Text + ")";
                    }
                    else
                    {
                        textBox7.Text = "q=(" + textBox8.Text + "L" + "+" + textBox9.Text + ")/(" + textBox10.Text + "L" + "+" + textBox11.Text + ")";
                    }
                }


                if (Convert.ToDouble(textBox15.Text) < 0)
                {
                    if (Convert.ToDouble(textBox17.Text) < 0)
                    {
                        textBox13.Text = "q=(" + textBox14.Text + "L" + textBox15.Text + ")/(" + textBox16.Text + "L" + textBox17.Text + ")";
                    }
                    else
                    {
                        textBox13.Text = "q=(" + textBox14.Text + "L" + textBox15.Text + ")/(" + textBox16.Text + "L" + "+" + textBox17.Text + ")";
                    }
                }
                else
                {
                    if (Convert.ToDouble(textBox17.Text) < 0)
                    {
                        textBox13.Text = "q=(" + textBox14.Text + "L" + "+" + textBox15.Text + ")/(" + textBox16.Text + "L" + textBox17.Text + ")";
                    }
                    else
                    {
                        textBox13.Text = "q=(" + textBox14.Text + "L" + "+" + textBox15.Text + ")/(" + textBox16.Text + "L" + "+" + textBox17.Text + ")";
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      

        private void button2_Click(object sender, EventArgs e)
        {


            Savement();

            this.Close();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Savement();
        }
    }
}
