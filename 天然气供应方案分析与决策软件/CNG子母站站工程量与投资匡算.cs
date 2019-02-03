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
    public partial class CNGWindowsProject : Form
    {
        public CNGWindowsProject()
        {
            InitializeComponent();
        }

        int MiddleVariable1;    //母站1规模数量  最大规模的数量
        int MiddleVariable2;
        int MiddleVariable3;
        int MiddleVariable4;
        int MiddleVariable5;
        int MiddleVariable6;
        int MiddleVariable7;    //标准站1规模数量 最大规模的数量
        int MiddleVariable8;

        double PrimaryStationScale1Investment;//母站规模1投资  最大
        double PrimaryStationScale2Investment;
        double PrimaryStationScale3Investment;
        double PrimaryStationScale4Investment;
        double PrimaryStationScale5Investment;
        double PrimaryStationScale6Investment;

        double PrimaryStationScale1Area;      //占地面积  最大
        double PrimaryStationScale2Area;
        double PrimaryStationScale3Area;
        double PrimaryStationScale4Area;
        double PrimaryStationScale5Area;
        double PrimaryStationScale6Area;

        double PrimaryStationScale1ProjectTime;  //工期  最大
        double PrimaryStationScale2ProjectTime;
        double PrimaryStationScale3ProjectTime;
        double PrimaryStationScale4ProjectTime;
        double PrimaryStationScale5ProjectTime;
        double PrimaryStationScale6ProjectTime;

        double SubstationScale1Investment;      //子站规模1投资  最大
        double SubstationScale2Investment;

        double SubstationScale1Area;            //子站占地面积 最大
        double SubstationScale2Area;

        double SubstationScale1ProjectTime;   //子站工期 最大
        double SubstationScale2ProjectTime;

        public CNGSubstationSet CngSubstationSet = new CNGSubstationSet();


        private void ClearTextBox()
        {
            txtInput1.Text = "";
            txtOuput2.Text = "";
            txtOuput3.Text = "";
            txtOuput4.Text = "";
            txtOuput5.Text = "";
            txtOuput6.Text = "";
            txtOuput7.Text = "";
            txtOuput8.Text = "";
            txtOuput9.Text = "";
            txtOuput10.Text = "";
            txtOuput11.Text = "";
            txtOuput12.Text = "";
            txtOuput13.Text = "";
            txtOuput14.Text = "";

        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void button5_Click(object sender, EventArgs e)
        {
            ClearTextBox();
        }

        private void CNGWindowsProject_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CngSubstationSet = new CNGSubstationSet();
            //CngSubstationSet.MdiParent = this;
            CngSubstationSet.ShowDialog();
        }

        private void ScaleLargeToSmall(int Variable)
        {
            int v = Variable;
            int[] s = new int[6] { 0, 0, 0, 0, 0, 0 };
            int[] a = new int[6];
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("template.gsa"); //加载xml文件
            a[0] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale1"));
            a[1] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale2"));
            a[2] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale3"));
            a[3] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale4"));
            a[4] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale5"));
            a[5] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale6"));
            for (int i = 0; i < 6; i++)
            {
                s[i] = v / a[i];
                if (s[i] == 0)
                {
                    s[i] = 1;
                    break;
                }
                v = v % a[i];
            }
            txtOuput1.Text = s[0].ToString();
            txtOuput2.Text = s[1].ToString();
            txtOuput3.Text = s[2].ToString();
            txtOuput4.Text = s[3].ToString();
            txtOuput5.Text = s[4].ToString();
            txtOuput6.Text = s[5].ToString();
        }
        private void ScaleNearest(int Variable)
        {
            int v;
            if (Variable % 5 != 0)
            {
                v = Variable + 5 - (Variable % 5);
            }
            else
            {
                v = Variable;
            }

            int[] s = new int[6];
            int[] a = new int[6];
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件
            a[0] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale1"));
            a[1] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale2"));
            a[2] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale3"));
            a[3] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale4"));
            a[4] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale5"));
            a[5] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale6"));
            for (int i = 0; i < 6; i++)
            {
                s[i] = v / a[i];
                v = v % a[i];
            }
            if (v != 0)
            {
                s[5] += 1;
            }
            txtOuput1.Text = s[0].ToString();
            txtOuput2.Text = s[1].ToString();
            txtOuput3.Text = s[2].ToString();
            txtOuput4.Text = s[3].ToString();
            txtOuput5.Text = s[4].ToString();
            txtOuput6.Text = s[5].ToString();
        }
        private void Calculate()
        {
            try
            {
                ParameterErrorDetection();
                double targetValue1 = Convert.ToDouble(txtInput1.Text);

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("XMLFile1.xml"); //加载xml文件

                double SubstationScale1 = Convert.ToDouble(ReadXml(xmlDoc, "SubstationScale1"));
                double SubstationScale2 = Convert.ToDouble(ReadXml(xmlDoc, "SubstationScale2"));
                textBox2.Text = ReadXml(xmlDoc, "PrimaryStationScale1");
                textBox3.Text = ReadXml(xmlDoc, "PrimaryStationScale2");
                textBox4.Text = ReadXml(xmlDoc, "PrimaryStationScale3");
                textBox5.Text = ReadXml(xmlDoc, "PrimaryStationScale4");
                textBox6.Text = ReadXml(xmlDoc, "PrimaryStationScale5");
                textBox7.Text = ReadXml(xmlDoc, "PrimaryStationScale6");
                textBox14.Text = ReadXml(xmlDoc, "SubstationScale1");
                textBox15.Text = ReadXml(xmlDoc, "SubstationScale2");
                radioButton1.Text = textBox14.Text + "万方/座";
                radioButton2.Text = textBox15.Text + "万方/座";

                int targetValue = Convert.ToInt32(Math.Ceiling(targetValue1));
                if (radioButton3.Checked == true)
                {
                    ScaleLargeToSmall(targetValue);
                    MiddleVariable1 = Convert.ToInt32(txtOuput1.Text);
                    MiddleVariable2 = Convert.ToInt32(txtOuput2.Text);
                    MiddleVariable3 = Convert.ToInt32(txtOuput3.Text);
                    MiddleVariable4 = Convert.ToInt32(txtOuput4.Text);
                    MiddleVariable5 = Convert.ToInt32(txtOuput5.Text);
                    MiddleVariable6 = Convert.ToInt32(txtOuput6.Text);
                    if (radioButton1.Checked == true)
                    {
                        txtOuput7.Text = Math.Ceiling(targetValue1 / SubstationScale1).ToString();//2
                        txtOuput8.Text = "0";
                    }
                    else
                    {
                        txtOuput8.Text = Math.Ceiling(targetValue1 / SubstationScale2).ToString();//1.5
                        txtOuput7.Text = "0";
                    }

                }
                if (radioButton4.Checked == true)
                {
                    ScaleNearest(targetValue);
                    MiddleVariable1 = Convert.ToInt32(txtOuput1.Text);
                    MiddleVariable2 = Convert.ToInt32(txtOuput2.Text);
                    MiddleVariable3 = Convert.ToInt32(txtOuput3.Text);
                    MiddleVariable4 = Convert.ToInt32(txtOuput4.Text);
                    MiddleVariable5 = Convert.ToInt32(txtOuput5.Text);
                    MiddleVariable6 = Convert.ToInt32(txtOuput6.Text);
                    if (radioButton1.Checked == true)
                    {
                        txtOuput8.Text = "0";
                        txtOuput7.Text = Math.Ceiling(targetValue1 / SubstationScale1).ToString();//2


                    }
                    else
                    {
                        txtOuput7.Text = "0";
                        txtOuput8.Text = Math.Ceiling(targetValue1 / SubstationScale2).ToString();//1.5
                    }
                }
                MiddleVariable7 = Convert.ToInt32(txtOuput7.Text);
                MiddleVariable8 = Convert.ToInt32(txtOuput8.Text);
                double BundleInvestment = Convert.ToDouble(ReadXml(xmlDoc, "BundleInvestment"));
                double TractorInvestment = Convert.ToDouble(ReadXml(xmlDoc, "TractorInvestment"));
                double BundleStorageGasScale = Convert.ToDouble(ReadXml(xmlDoc, "BundleScale"));
                PrimaryStationScale1Investment = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale1Investment"));
                PrimaryStationScale2Investment = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale2Investment"));
                PrimaryStationScale3Investment = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale3Investment"));
                PrimaryStationScale4Investment = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale4Investment"));
                PrimaryStationScale5Investment = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale5Investment"));
                PrimaryStationScale6Investment = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale6Investment"));

                PrimaryStationScale1Area = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale1Area"));
                PrimaryStationScale2Area = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale2Area"));
                PrimaryStationScale3Area = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale3Area"));
                PrimaryStationScale4Area = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale4Area"));
                PrimaryStationScale5Area = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale5Area"));
                PrimaryStationScale6Area = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale6Area"));

                PrimaryStationScale1ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale1ProjectTime"));
                PrimaryStationScale2ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale2ProjectTime"));
                PrimaryStationScale3ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale3ProjectTime"));
                PrimaryStationScale4ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale4ProjectTime"));
                PrimaryStationScale5ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale5ProjectTime"));
                PrimaryStationScale6ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale6ProjectTime"));

                SubstationScale1Area = Convert.ToDouble(ReadXml(xmlDoc, "SubstationScale1Area"));
                SubstationScale2Area = Convert.ToDouble(ReadXml(xmlDoc, "SubstationScale2Area"));

                SubstationScale1Investment = Convert.ToDouble(ReadXml(xmlDoc, "SubstationScale1Investment"));
                SubstationScale2Investment = Convert.ToDouble(ReadXml(xmlDoc, "SubstationScale2Investment"));

                double Bundle = Math.Ceiling((MiddleVariable7 * SubstationScale1 / BundleStorageGasScale + MiddleVariable8 * SubstationScale2 / BundleStorageGasScale) / 2);//管束车
                double PermanentFloorArea = MiddleVariable1 * PrimaryStationScale1Area + MiddleVariable2 * PrimaryStationScale2Area + MiddleVariable3 * PrimaryStationScale3Area + MiddleVariable4 * PrimaryStationScale4Area + MiddleVariable5 * PrimaryStationScale5Area + MiddleVariable6 * PrimaryStationScale6Area + MiddleVariable7 * SubstationScale1Area + MiddleVariable8 * SubstationScale2Area;
                double PrimaryStationInvestment = MiddleVariable1 * PrimaryStationScale1Investment + MiddleVariable2 * PrimaryStationScale2Investment + MiddleVariable3 * PrimaryStationScale3Investment + MiddleVariable4 * PrimaryStationScale4Investment + MiddleVariable5 * PrimaryStationScale5Investment + MiddleVariable6 * PrimaryStationScale6Investment;
                double SubstationInvestment = MiddleVariable7 * SubstationScale1Investment + MiddleVariable8 * SubstationScale2Investment;
                double BundleAndTractorInvestment = TractorInvestment * (Bundle / 2) + BundleInvestment * Bundle;
                double Investment = PrimaryStationInvestment + SubstationInvestment + BundleAndTractorInvestment;
                double ProjectTime = 0;
                if (MiddleVariable1 != 0)
                {
                    ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale1ProjectTime"));
                }
                else if (MiddleVariable2 != 0)
                {
                    ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale2ProjectTime"));
                }
                else if (MiddleVariable3 != 0)
                {
                    ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale3ProjectTime"));
                }
                else if (MiddleVariable4 != 0)
                {
                    ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale4ProjectTime"));
                }
                else if (MiddleVariable5 != 0)
                {
                    ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale5ProjectTime"));
                }
                else if (MiddleVariable6 != 0)
                {
                    ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationScale6ProjectTime"));
                }
                else if (MiddleVariable7 != 0)
                {
                    ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "SubstationScale1ProjectTime"));
                }
                else if (MiddleVariable8 != 0)
                {
                    ProjectTime = Convert.ToDouble(ReadXml(xmlDoc, "SubstationScale2ProjectTime"));
                }

                txtOuput9.Text = Bundle.ToString();//管束车
                txtOuput10.Text = PermanentFloorArea.ToString("0.000");//永久占地面积
                txtOuput11.Text = ProjectTime.ToString();
                txtOuput12.Text = Math.Ceiling((Bundle / 2)).ToString();//牵引车
                txtOuput13.Text = "0.000";//临时占地
                txtOuput14.Text = Investment.ToString(); //总投资

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ParameterErrorDetection()
        {
            //参数检测，判断输入是否为空
            if (txtInput1.Text == "")
            {
                throw new InvalidOperationException("输入参数{" + label1.Text + txtInput1.Text + "}为空，请重新输入。");

            }
            //参数检测，判断输入是否含有字符
            foreach (char c in txtInput1.Text)
            {
                if (char.IsLetter(c))
                {
                    throw new InvalidOperationException("输入参数{" + label1.Text + txtInput1.Text + "}输入参数含有字符，请重新输入。");
                }
            }

            double targetValue1 = Convert.ToDouble(txtInput1.Text);
            //参数检测，判断输入是否为数字、零
            if (targetValue1 < 0)
            {
                throw new InvalidOperationException("输入参数{" + label1.Text + txtInput1.Text + "}为负数，请重新输入。");

            }
            if (targetValue1 == 0)
            {
                throw new InvalidOperationException("输入参数{" + label1.Text + txtInput1.Text + "}为零，请重新输入。");

            }
            //参数检测，判断输入是否在规定范围内 （0,1000000]
            if (targetValue1 > 1000000)
            {
                throw new InvalidOperationException("输入参数{" + label1.Text + txtInput1.Text + "}超过输入参数范围(0,1000000]，请重新输入。");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Calculate();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            string path = null;
            loadFile.Multiselect = false;
            loadFile.Filter = "gsa(*.gsa)|*.gsa";
            if (loadFile.ShowDialog() == DialogResult.OK)
            {
                path = loadFile.FileName;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(path); //加载xml文件

                txtInput1.Text = ReadXml(xmlDoc, "Supply");
                textBox2.Text = ReadXml(xmlDoc, "PrimaryStationScale1");
                textBox3.Text = ReadXml(xmlDoc, "PrimaryStationScale2");
                textBox4.Text = ReadXml(xmlDoc, "PrimaryStationScale3");
                textBox5.Text = ReadXml(xmlDoc, "PrimaryStationScale4");
                textBox6.Text = ReadXml(xmlDoc, "PrimaryStationScale5");
                textBox7.Text = ReadXml(xmlDoc, "PrimaryStationScale6");

                XmlNode xn7 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount1");
                txtOuput1.Text = xn7.InnerText;

                XmlNode xn8 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount2");
                txtOuput2.Text = xn8.InnerText;

                XmlNode xn9 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount3");
                txtOuput3.Text = xn9.InnerText;

                XmlNode xn10 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount4");
                txtOuput4.Text = xn10.InnerText;

                XmlNode xn11 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount5");
                txtOuput5.Text = xn11.InnerText;

                XmlNode xn12 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount6");
                txtOuput6.Text = xn12.InnerText;

                XmlNode xn13 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale1");
                textBox14.Text = xn13.InnerText;

                XmlNode xn14 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale2");
                textBox15.Text = xn14.InnerText;

                XmlNode xn15 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationCount1");
                txtOuput7.Text = xn15.InnerText;

                XmlNode xn16 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationCount2");
                txtOuput8.Text = xn16.InnerText;

                XmlNode xn17 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/BundleCount");
                txtOuput9.Text = xn17.InnerText;

                XmlNode xn18 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/TractorCount");
                txtOuput12.Text = xn18.InnerText;

                XmlNode xn19 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PermanentFloorArea");
                txtOuput10.Text = xn19.InnerText;

                XmlNode xn20 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/TemporaryFloorArea");
                txtOuput13.Text = xn20.InnerText;

                XmlNode xn21 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/ProjectTime");
                txtOuput11.Text = xn21.InnerText;

                XmlNode xn22 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/Investment");
                txtOuput14.Text = xn22.InnerText;
            }

        }
        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/CNGPrimaryStationRoughEstimate/" + s;

            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = null;
            saveFile.Filter = "gsa(*.gsa)|*.gsa";//设置文件类型
            saveFile.FileName = "GAS";//设置默认文件名
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                path = saveFile.FileName;
                SaveCurrentParameters("template.gsa", path);


            }
        }

        public void SaveCurrentParameters(string sourcePath, string targetPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(sourcePath); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/Supply");
            xn0.InnerText = txtInput1.Text;

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale1");
            xn1.InnerText = textBox2.Text;

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale2");
            xn2.InnerText = textBox3.Text;

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale3");
            xn3.InnerText = textBox4.Text;

            XmlNode xn4 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale4");
            xn4.InnerText = textBox5.Text;

            XmlNode xn5 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale5");
            xn5.InnerText = textBox6.Text;

            XmlNode xn6 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale6");
            xn6.InnerText = textBox7.Text;

            XmlNode xn7 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount1");
            txtOuput1.Text = xn7.InnerText;

            XmlNode xn8 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount2");
            xn8.InnerText = txtOuput2.Text;

            XmlNode xn9 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount3");
            xn9.InnerText = txtOuput3.Text;

            XmlNode xn10 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount4");
            xn10.InnerText = txtOuput4.Text;

            XmlNode xn11 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount5");
            xn11.InnerText = txtOuput5.Text;

            XmlNode xn12 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount6");
            xn12.InnerText = txtOuput6.Text;

            XmlNode xn13 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale1");
            xn13.InnerText = textBox14.Text;

            XmlNode xn14 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale2");
            xn14.InnerText = textBox15.Text;

            XmlNode xn15 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationCount1");
            xn15.InnerText = txtOuput7.Text;

            XmlNode xn16 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationCount2");
            xn16.InnerText = txtOuput8.Text;

            XmlNode xn17 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/BundleCount");
            xn17.InnerText = txtOuput9.Text;

            XmlNode xn18 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/TractorCount");
            xn18.InnerText = txtOuput12.Text;

            XmlNode xn19 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PermanentFloorArea");
            xn19.InnerText = txtOuput10.Text;

            XmlNode xn20 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/TemporaryFloorArea");
            xn20.InnerText = txtOuput13.Text;

            XmlNode xn21 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/ProjectTime");
            xn21.InnerText = txtOuput11.Text;

            XmlNode xn22 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/Investment");
            xn22.InnerText = txtOuput14.Text;

            xmlDoc.Save(targetPath);// 保存至目标文件
        }

        private void groupBox1_MouseHover(object sender, EventArgs e)
        {

        }

        private void groupBox1_Move(object sender, EventArgs e)
        {

        }

        private void CNGWindowsProject_Move(object sender, EventArgs e)
        {

        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }
    }
}
