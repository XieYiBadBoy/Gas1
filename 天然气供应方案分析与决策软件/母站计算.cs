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
    public partial class Windows2 : Form
    {
        public Windows2()
        {
            InitializeComponent();
        }
        private MathOpt calculator1 = new MathOpt();
        NatStationSetup natstationsetup = new NatStationSetup();
        private void Add2()
        {
            try
            {
                string str1 = lblInput2.Text;
                string str2 = txtInput2.Text;
                string str3 = lblInput4.Text;
                string str4 = txtInput4.Text;
                string str5 = lblInput5.Text;
                string str6 = txtInput5.Text;
                ParameterErrorDetectionInput1(str3, str4);
                ParameterErrorDetectionInput2(str5, str6);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("XMLFile1.xml"); //加载xml文件


                double Z = Convert.ToDouble(ReadXml(xmlDoc, "CompersiveFactor"));
                double StdFlow2 = Convert.ToDouble(txtInput2.Text);   //标准体积流量       StdFlow
                                                                      //单位转换，万方/日转换成立方米/日
                double StdFlow = StdFlow2 * 10000;
                double UpPre = Convert.ToDouble(txtInput4.Text);     //压缩机进口前压力   UpPre
                double ExitPre = Convert.ToDouble(txtInput5.Text);   //压缩机出口压力     ExitPre  
                double t = Convert.ToDouble(ReadXml(xmlDoc, "DailyWorkTime"));       //工作时长
                double N = calculator1.CompressorPower(Z, StdFlow, t, UpPre, ExitPre);
                double TotalArea = 9500 + 0.04 * StdFlow;
                txtOutput1.Text = N.ToString("");
                txtOutput2.Text = TotalArea.ToString("0.00");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            if (targetValue1 > 100)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围，请重新输入。");
            }
        }

        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/PrimaryStation/" + s;

            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }
        private void lblInput2_Click(object sender, EventArgs e)
        {

        }

        private void Calbutton1_Click(object sender, EventArgs e)
        {

            if (radioButton1.Checked == true)
            {
                Add2();
            }
            else
            {
                MessageBox.Show("请选择计算类型");
            }
        }

        private void Clobutton3_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否关闭当前窗口？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                this.Close();
            }
            //Close();
        }
        protected void Clear()
        {
            txtInput2.Text = "";
            txtInput4.Text = "";
            txtInput5.Text = "";
            txtOutput1.Text = "";
            txtOutput2.Text = "";

        }
        private void Clebutton2_Click(object sender, EventArgs e)
        {
            Clear();
        }
        //public NatStationSetup 
        private void Setbutton1_Click(object sender, EventArgs e)
        {
            natstationsetup.ShowDialog();
        }

        private void Windows2_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            natstationsetup = new NatStationSetup();

            natstationsetup.ShowDialog();
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
            if (targetValue1 > 1000000)
            {
                throw new InvalidOperationException("输入参数{" + str1 + str2 + "}超过输入参数范围，请重新输入。");
            }
        }
        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = null;
            saveFile.Filter = "gsa(*.gsa)|*.gsa";//设置文件类型
            saveFile.FileName = "GAS";//设置默认文件名
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                path = saveFile.FileName;
                SaveCurrentParameters("OpenNewFile.gsa", path);
            }
        }

        public void SaveCurrentParameters(string sourcePath, string targetPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(sourcePath); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/PrimaryStation/StdFlow");
            xn0.InnerText = txtInput2.Text;

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/PrimaryStation/UpPre");
            xn1.InnerText = txtInput4.Text;

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/PrimaryStation/ExitPre");
            xn2.InnerText = txtInput5.Text;

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/PrimaryStation/CompresiveNumber");
            xn3.InnerText = txtOutput1.Text;

            XmlNode xn4 = xmlDoc.SelectSingleNode("configuration/PrimaryStation/CNGPrimaryStationArea");
            xn4.InnerText = txtOutput2.Text;

            xmlDoc.Save(targetPath);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string path = null;
            loadFile.Multiselect = false;
            loadFile.Filter = "gsa(*.gsa)|*.gsa";
            if (loadFile.ShowDialog() == DialogResult.OK)
            {
                path = loadFile.FileName;
                LoadCurrentParameters(path);
            }
        }

        private void LoadCurrentParameters(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path); //加载xml文件
            txtInput2.Text = ReadXml(xmlDoc, "StdFlow");
            txtInput4.Text = ReadXml(xmlDoc, "UpPre");
            txtInput5.Text = ReadXml(xmlDoc, "ExitPre");
            txtOutput1.Text= ReadXml(xmlDoc, "CompresiveNumber");
            txtOutput2.Text= ReadXml(xmlDoc, "CNGPrimaryStationArea");
        }
    }
}
