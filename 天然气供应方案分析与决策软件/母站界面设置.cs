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
    public partial class NatStationSetup : Form
    {
        public NatStationSetup()
        {
            InitializeComponent();
        }

        //public string NatFactor
        //{
        //    get { return this.txtInput1.Text; }
        //    set { this.txtInput1.Text = value; }
        //}
        //public string NatTime
        //{
        //    get { return this.txtInput2.Text; }
        //    set { this.txtInput2.Text = value; }
        //}
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string str1 = label1.Text;
                string str2 = txtInput1.Text;
                string str3 = label2.Text;
                string str4 = txtInput2.Text;

                ParameterErrorDetectionInput1(str1, str2);
                ParameterErrorDetectionInput2(str3, str4);

                SaveParameter();
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }



        }
        private void ParameterErrorDetectionInput1(string str1, string str2)
        {
            //参数检测，判断txtInput2输入是否为空
            if (str2 == "")
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为空，请重新输入。");

            }
            //参数检测，判断txtInput2输入是否含有字符
            foreach (char c in str2)
            {
                if (char.IsLetter(c))
                {
                    throw new InvalidOperationException("输入参数{" + str1 + str2 + "}输入参数含有字符，请重新输入。");
                }
            }
            double targetValue1 = Convert.ToDouble(str2);
            //参数检测，判断输入是否为数字、零
            if (targetValue1 < 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为负数，请重新输入。");

            }
            if (targetValue1 == 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为零，请重新输入。");

            }
            //参数检测，判断输入是否在规定范围内 （0,1000000]
            if (targetValue1 > 1.5 || targetValue1 < 0.1)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围[0.1,1.5]，请重新输入。");
            }
        }
        private void ParameterErrorDetectionInput2(string str1, string str2)
        {
            //参数检测，判断txtInput2输入是否为空
            if (str2 == "")
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为空，请重新输入。");

            }
            //参数检测，判断txtInput2输入是否含有字符
            foreach (char c in str2)
            {
                if (char.IsLetter(c))
                {
                    throw new InvalidOperationException("输入参数{" + str1 + str2 + "}输入参数含有字符，请重新输入。");
                }
            }
            double targetValue1 = Convert.ToDouble(str2);
            //参数检测，判断输入是否为数字、零
            if (targetValue1 < 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为负数，请重新输入。");

            }
            if (targetValue1 == 0)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}为零，请重新输入。");

            }
            //参数检测，判断输入是否在规定范围内 （0,1000000]
            if (targetValue1 > 24)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围(0,24]，请重新输入。");
            }
        }

        private void SaveParameter()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/PrimaryStation/CompersiveFactor");
            xn0.InnerText = txtInput1.Text;

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/PrimaryStation/DailyWorkTime");
            xn1.InnerText = txtInput2.Text;

            xmlDoc.Save("XMLFile1.xml");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                string str1 = label1.Text;
                string str2 = txtInput1.Text;
                string str3 = label2.Text;
                string str4 = txtInput2.Text;

                ParameterErrorDetectionInput1(str1, str2);
                ParameterErrorDetectionInput2(str3, str4);
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

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("OpenNewFile.gsa"); //加载xml文件

            txtInput1.Text = ReadXml(xmlDoc, "CompersiveFactor");
            txtInput2.Text = ReadXml(xmlDoc, "DailyWorkTime");
            
        }
        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/PrimaryStation/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void NatStationSetup_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件
            txtInput1.Text = ReadXml(xmlDoc, "CompersiveFactor");
            txtInput2.Text = ReadXml(xmlDoc, "DailyWorkTime");

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void txtInput2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void txtInput1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label52_Click(object sender, EventArgs e)
        {

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
    }
}
