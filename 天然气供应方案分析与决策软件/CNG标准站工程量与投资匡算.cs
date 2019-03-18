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
    public partial class CNGStanardStationProjectAndInvestment : Form
    {
        public CNGStanardStationProjectAndInvestment()
        {
            InitializeComponent();
        }

        public CNGStandardStationSet CngStandardStationSet;

        int SubstationNumber;//子站数
        double PermanentArea;//永久占地面积
        double ProjectTime;//工期
        double Investment;//投资
        double StandardStationArea;
        private string XMLRead(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/CNGStandardStationRoughEstimate/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }
        private void Calculate()
        {
            try
            {
                ParameterErrorDetection();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("XMLFile1.xml"); //加载xml文件

                double Supply = Convert.ToDouble(txtInput1.Text);

                textBox14.Text = XMLRead(xmlDoc, "StandardStationScale");
                radioButton1.Text = textBox14.Text + "万方/座";

                double StandardStationScale = Convert.ToDouble(XMLRead(xmlDoc, "StandardStationScale"));
                StandardStationArea = Convert.ToDouble(XMLRead(xmlDoc, "StandardStationArea"));
                double SandardStationProjectTime = Convert.ToDouble(XMLRead(xmlDoc, "SandardStationProjectTime"));
                double StandardStationInvestment = Convert.ToDouble(XMLRead(xmlDoc, "StandardStationInvestment"));

                SubstationNumber = Convert.ToInt32(Math.Ceiling(Supply / StandardStationScale));
                PermanentArea = SubstationNumber * StandardStationArea;
                ProjectTime = SandardStationProjectTime;
                Investment = SubstationNumber * StandardStationInvestment;


                txtOuput7.Text = SubstationNumber.ToString();
                txtOuput10.Text = PermanentArea.ToString();
                txtOuput11.Text = ProjectTime.ToString();
                txtOuput13.Text = "0";
                txtOuput14.Text = Investment.ToString();
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
                    //有字母
                    throw new InvalidOperationException("输入参数{" + label1.Text + txtInput1.Text + "}输入参数含有字符，请重新输入。");
                }
            }

            double Supply = Convert.ToDouble(txtInput1.Text);
            //参数检测，判断输入是否为数字、零
            if (Supply < 0)
            {
                throw new InvalidOperationException("输入参数{" + label1.Text + txtInput1.Text + "}为负数，请重新输入。");

            }
            if (Supply == 0)
            {
                throw new InvalidOperationException("输入参数{" + label1.Text + txtInput1.Text + "}为零，请重新输入。");

            }
            //参数检测，判断输入是否在规定范围内 （0,1000000]
            if (Supply > 1000000)
            {
                throw new InvalidOperationException("输入参数{" + label1.Text + txtInput1.Text + "}超过输入参数范围，请重新输入(0,10000000]。");
            }
        }

        private void ClearString()
        {
            txtInput1.Text = "";
            txtOuput7.Text = "";
            txtOuput10.Text = "";
            txtOuput11.Text = "";
            txtOuput13.Text = "";
            txtOuput14.Text = "";

        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Calculate();
            }
            else
            {
                MessageBox.Show("请选择计算类型");

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ClearString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CNGStanardStationProjectAndInvestment_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CngStandardStationSet = new CNGStandardStationSet();
            CngStandardStationSet.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void txtInput1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
