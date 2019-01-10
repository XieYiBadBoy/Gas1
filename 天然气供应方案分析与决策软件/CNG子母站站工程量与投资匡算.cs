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
        int MiddleVariable1;
        int MiddleVariable2;
        int MiddleVariable3;
        int MiddleVariable4;
        int MiddleVariable5;
        int MiddleVariable6;
        int MiddleVariable7;
        int MiddleVariable8;

        public CNGSubstationSet CngSubstationSet=new CNGSubstationSet();
  

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
            CngSubstationSet.Show();
        }

        private void ScaleLargeToSmall(int  Variable)
        {
            int v = Variable;
            int[] s = new int[6] { 0,0,0,0,0,0};
            int[] a = new int[6] { 50, 30, 25, 20, 15, 10 };

            for (int i = 0; i < 6; i++)
            {
                s[i] = v / a[i];
                if (s[i] ==0)
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
            if (Variable%5!=0)
            {
                v = Variable + 5 - (Variable % 5);
            }
            else
            {
                v = Variable;
            }
            
            int[] s = new int[6];
            int[] a = new int[6] { 50, 30, 25, 20, 15, 10 };
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
                double targetValue1 = Convert.ToDouble(txtInput1.Text);

                if (targetValue1 < 0)
                {
                    throw new InvalidOperationException("您的输入为负数，管子尺寸初步计算(经济及可行性)计算对象无效!");

                }
                if (targetValue1 ==0)
                {
                    throw new InvalidOperationException("您的输入为零，管子尺寸初步计算(经济及可行性)计算对象无效!");

                }
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
                        txtOuput7.Text = Math.Ceiling(targetValue1 / 2).ToString();
                        txtOuput8.Text = "0";
                    }
                    else
                    {
                        txtOuput8.Text = Math.Ceiling(targetValue1 / 1.5).ToString();
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
                        txtOuput7.Text = Math.Ceiling(targetValue1 / 2).ToString();


                    }
                    else
                    {
                        txtOuput7.Text = "0";
                        txtOuput8.Text = Math.Ceiling(targetValue1 / 1.5).ToString();
                    }
                }
                MiddleVariable7 = Convert.ToInt32(txtOuput7.Text);
                MiddleVariable8 = Convert.ToInt32(txtOuput8.Text);
                double BundleInvestment = Convert.ToDouble(Properties.Settings.Default.b);
                double TractorInvestment = Convert.ToDouble(Properties.Settings.Default.a);
                double BundleStorageGasScale = Convert.ToDouble(Properties.Settings.Default.c);

                double Bundle = Math.Ceiling((MiddleVariable7 * 2 / BundleStorageGasScale + MiddleVariable8 * 1.5 / BundleStorageGasScale) / 2);//管束车
                double PermanentFloorArea = MiddleVariable1 * 2.5 + MiddleVariable2 * 2 + MiddleVariable3 * 1.7 + MiddleVariable4 * 1.5 + MiddleVariable5 * 1.3 + MiddleVariable6 * 1 + MiddleVariable7 * 0.4 + MiddleVariable8 * 0.3;
                double PrimaryStationInvestment = MiddleVariable1 * 4500 + MiddleVariable2 * 3500 + MiddleVariable3 * 3100 + MiddleVariable4 * 2800 + MiddleVariable5 * 2500 + MiddleVariable6 * 2000;
                double SubstationInvestment = MiddleVariable7 * 900 + MiddleVariable8 * 750;
                double BundleAndTractorInvestment = TractorInvestment * (Bundle / 2) + BundleInvestment * Bundle;
                double Investment = PrimaryStationInvestment + SubstationInvestment + BundleAndTractorInvestment;
                double ProjectTime = 0;
                if (MiddleVariable1 != 0)
                {
                    ProjectTime = 8;
                }
                else if (MiddleVariable2 != 0)
                {
                    ProjectTime = 7;
                }
                else if (MiddleVariable3 != 0)
                {
                    ProjectTime = 6;
                }
                else if (MiddleVariable4 != 0 || MiddleVariable5 != 0 || MiddleVariable6 != 0 || MiddleVariable7 != 0)
                {
                    ProjectTime = 5;
                }
                else if (MiddleVariable8 != 0)
                {
                    ProjectTime = 4;
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
            }
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
        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string  Str = "configuration/CNGPrimaryStationRoughEstimate/" + s;
            
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
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("template.gsa"); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/Supply");
            xn0.InnerText= txtInput1.Text ;

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale1");
            xn1.InnerText = textBox2.Text ;

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale2");
            xn2.InnerText= textBox3.Text  ;

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale3");
            xn3.InnerText = textBox4.Text ;

            XmlNode xn4 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale4");
            xn4.InnerText= textBox5.Text ;

            XmlNode xn5 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale5");
            xn5.InnerText= textBox6.Text ;

            XmlNode xn6 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationScale6");
            xn6.InnerText= textBox7.Text ;

            XmlNode xn7 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount1");
            txtOuput1.Text = xn7.InnerText;

            XmlNode xn8 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount2");
            xn8.InnerText = txtOuput2.Text ;

            XmlNode xn9 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount3");
            xn9.InnerText= txtOuput3.Text ;

            XmlNode xn10 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount4");
            xn10.InnerText= txtOuput4.Text ;

            XmlNode xn11 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount5");
            xn11.InnerText= txtOuput5.Text ;

            XmlNode xn12 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PrimaryStationCount6");
            xn12.InnerText= txtOuput6.Text ;

            XmlNode xn13 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale1");
            xn13.InnerText = textBox14.Text ;

            XmlNode xn14 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationScale2");
            xn14.InnerText = textBox15.Text;

            XmlNode xn15 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationCount1");
            xn15.InnerText= txtOuput7.Text ;

            XmlNode xn16 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/SubstationCount2");
            xn16.InnerText= txtOuput8.Text ;

            XmlNode xn17 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/BundleCount");
            xn17.InnerText= txtOuput9.Text ;

            XmlNode xn18 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/TractorCount");
            xn18.InnerText= txtOuput12.Text ;

            XmlNode xn19 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/PermanentFloorArea");
            xn19.InnerText= txtOuput10.Text ;

            XmlNode xn20 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/TemporaryFloorArea");
            xn20.InnerText = txtOuput13.Text ;

            XmlNode xn21 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/ProjectTime");
            xn21.InnerText = txtOuput11.Text  ;

            XmlNode xn22 = xmlDoc.SelectSingleNode("configuration/CNGPrimaryStationRoughEstimate/Investment");
            xn22.InnerText = txtOuput14.Text  ;

            xmlDoc.Save(path);
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
    }
}
