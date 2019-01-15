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
    public partial class LNGProjectAndInvestment : Form
    {
        public LNGProjectAndInvestment()
        {
            InitializeComponent();
        }

        public LNGProjectAndInvestmentSet LngProjectAndInvestmentSet;
        int MiddleVariable1;    //母站1规模数量  最大规模的数量
        int MiddleVariable2;
        int MiddleVariable3;
        int MiddleVariable4;
        int MiddleVariable5;
        int MiddleVariable6;

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

        double TurnoverRate1;   //周转率 最大
        double TurnoverRate2;
        double TurnoverRate3;
        double TurnoverRate4;
        double TurnoverRate5;
        double TurnoverRate6;

        double GasificattionOperationCost1;  //气化运行成本 最大
        double GasificattionOperationCost2;
        double GasificattionOperationCost3;
        double GasificattionOperationCost4;
        double GasificattionOperationCost5;
        double GasificattionOperationCost6;


        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void Calculate()
        {
       //try {

            double targetValue1 = Convert.ToDouble(txtInput1.Text);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件

         
            textBox2.Text = ReadXml(xmlDoc, "PrimaryStationScale1");
            textBox3.Text = ReadXml(xmlDoc, "PrimaryStationScale2");
            textBox4.Text = ReadXml(xmlDoc, "PrimaryStationScale3");
            textBox5.Text = ReadXml(xmlDoc, "PrimaryStationScale4");
            textBox6.Text = ReadXml(xmlDoc, "PrimaryStationScale5");
            textBox7.Text = ReadXml(xmlDoc, "PrimaryStationScale6");

            if (targetValue1 < 0)
            {
                throw new InvalidOperationException("您的输入为负数，管子尺寸初步计算(经济及可行性)计算对象无效!");

            }
            if (targetValue1 == 0)
            {
                throw new InvalidOperationException("您的输入为零，管子尺寸初步计算(经济及可行性)计算对象无效!");

            }
            int targetValue = Convert.ToInt32(Math.Ceiling(targetValue1));
            if (radioButton3.Checked == true)
            {
                ScaleLargeToSmall(targetValue);
            }
            else 
            {
                ScaleNearest(targetValue);
            }

            MiddleVariable1 = Convert.ToInt32(txtOuput1.Text);
            MiddleVariable2 = Convert.ToInt32(txtOuput2.Text);
            MiddleVariable3 = Convert.ToInt32(txtOuput3.Text);
            MiddleVariable4 = Convert.ToInt32(txtOuput4.Text);
            MiddleVariable5 = Convert.ToInt32(txtOuput5.Text);
            MiddleVariable6 = Convert.ToInt32(txtOuput6.Text);

                
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


           
            double PermanentFloorArea =MiddleVariable1 * PrimaryStationScale1Area + MiddleVariable2 * PrimaryStationScale2Area + MiddleVariable3 * PrimaryStationScale3Area + MiddleVariable4 * PrimaryStationScale4Area + MiddleVariable5 * PrimaryStationScale5Area + MiddleVariable6 * PrimaryStationScale6Area ;
            double PrimaryStationInvestment = MiddleVariable1 * PrimaryStationScale1Investment + MiddleVariable2 * PrimaryStationScale2Investment + MiddleVariable3 * PrimaryStationScale3Investment + MiddleVariable4 * PrimaryStationScale4Investment + MiddleVariable5 * PrimaryStationScale5Investment + MiddleVariable6 * PrimaryStationScale6Investment;         
            double Investment = PrimaryStationInvestment + 0;
            double ProjectTime = 0;
            if (MiddleVariable1 != 0)
            {
                ProjectTime = PrimaryStationScale1ProjectTime;
            }
            else if (MiddleVariable2 != 0)
            {
                ProjectTime = PrimaryStationScale2ProjectTime;
            }
            else if (MiddleVariable3 != 0)
            {
                ProjectTime = PrimaryStationScale2ProjectTime;
            }
            else if (MiddleVariable4!= 0)
            {
                ProjectTime = PrimaryStationScale2ProjectTime;
            }
            else if (MiddleVariable5 != 0)
            {
                ProjectTime = PrimaryStationScale2ProjectTime;
            }

            else if (MiddleVariable6 != 0)
            {
                ProjectTime = PrimaryStationScale2ProjectTime;
            }

       
            txtOuput10.Text = PermanentFloorArea.ToString("0.000");//永久占地面积
            txtOuput11.Text = ProjectTime.ToString();
            txtOuput13.Text = "0.000";//临时占地
            txtOuput14.Text = Investment.ToString(); //总投资

        //}
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
}

        private void Clear()
        {
            txtInput1.Text = "";
            txtOuput1.Text = "";
            txtOuput2.Text = "";
            txtOuput2.Text = "";
            txtOuput3.Text = "";
            txtOuput4.Text = "";
            txtOuput5.Text = "";
            txtOuput6.Text = "";      
            txtOuput10.Text = "";
            txtOuput11.Text = "";
            txtOuput13.Text = "";
            txtOuput14.Text = "";
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Clear();
        }
        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/LNGGasificattionStationRoughEstimate/"+ s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }
        private void ScaleLargeToSmall(int   Variable)
        {
      
            double  v = Variable;
            int[] s = new int[6] { 0, 0, 0, 0, 0, 0 };
            double [] a = new double [6];
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件
            a[0] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale1"))/ Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale1TurnoverRate"))* Convert.ToDouble (ReadXml(xmlDoc, "PrimaryStationGasificattionRate"))*(1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation")))/10000;
            a[1] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale2"))/ Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale2TurnoverRate"))* Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate"))*(1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation")))/ 10000;
            a[2] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale3"))/ Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale3TurnoverRate"))* Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate"))*(1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation")))/ 10000;
            a[3] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale4")) / Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale4TurnoverRate")) * Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate")) * (1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation"))) / 10000;
            a[4] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale5")) / Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale5TurnoverRate")) * Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate")) * (1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation"))) / 10000;
            a[5] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale6")) / Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale6TurnoverRate")) * Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate")) * (1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation"))) / 10000;
            for (int i = 0; i < 6; i++)
            {
                s[i] = Convert.ToInt32(Math.Floor((v / a[i])));
                if (s[i] == 0)
                {
                    s[i] = 1;
                    break;
                }
                v = v -s[i]*a[i];
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
            double  v;
            if (Variable % 5 != 0)
            {
                v = Variable + 5 - (Variable % 5);
            }
            else
            {
                v = Variable;
            }

            int[] s = new int[6];
            double [] a = new double[6];
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("XMLFile1.xml"); //加载xml文件
            a[0] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale1")) / Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale1TurnoverRate")) * Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate")) * (1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation"))) / 10000;
            a[1] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale2")) / Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale2TurnoverRate")) * Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate")) * (1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation"))) / 10000;
            a[2] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale3")) / Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale3TurnoverRate")) * Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate")) * (1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation"))) / 10000;
            a[3] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale4")) / Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale4TurnoverRate")) * Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate")) * (1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation"))) / 10000;
            a[4] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale5")) / Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale5TurnoverRate")) * Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate")) * (1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation"))) / 10000;
            a[5] = Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale6")) / Convert.ToInt32(ReadXml(xmlDoc, "PrimaryStationScale6TurnoverRate")) * Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationGasificattionRate")) * (1 - Convert.ToDouble(ReadXml(xmlDoc, "PrimaryStationColdInsulation"))) / 10000;
            for (int i = 0; i < 6; i++)
            {
                s[i] = Convert.ToInt32(Math.Floor(v / a[i]));
                v = v - s[i] * a[i];
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
        private void button6_Click(object sender, EventArgs e)  
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)   //计算
        {
            Calculate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LngProjectAndInvestmentSet = new LNGProjectAndInvestmentSet();
            LngProjectAndInvestmentSet.ShowDialog();
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

                XmlNode xn7 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount1");
                txtOuput1.Text = xn7.InnerText;

                XmlNode xn8 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount2");
                txtOuput2.Text = xn8.InnerText;

                XmlNode xn9 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount3");
                txtOuput3.Text = xn9.InnerText;

                XmlNode xn10 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount4");
                txtOuput4.Text = xn10.InnerText;

                XmlNode xn11 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount5");
                txtOuput5.Text = xn11.InnerText;

                XmlNode xn12 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount6");
                txtOuput6.Text = xn12.InnerText;

                XmlNode xn19 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PermanentFloorArea");
                txtOuput10.Text = xn19.InnerText;

                XmlNode xn20 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/TemporaryFloorArea");
                txtOuput13.Text = xn20.InnerText;

                XmlNode xn21 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/ProjectTime");
                txtOuput11.Text = xn21.InnerText;

                XmlNode xn22 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/Investment");
                txtOuput14.Text = xn22.InnerText;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = null;
            saveFile.Filter = "gsa(*.gsa)|*.gsa";//设置文件类型
            saveFile.FileName = "GAS";//设置默认文件名
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                path = saveFile.FileName;

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("template.gsa"); //加载xml文件

                XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/Supply");


                xn0.InnerText = txtInput1.Text;

                XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale1");
                xn1.InnerText = textBox2.Text;

                XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale2");
                xn2.InnerText = textBox3.Text;

                XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale3");
                xn3.InnerText = textBox4.Text;

                XmlNode xn4 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale4");
                xn4.InnerText = textBox5.Text;

                XmlNode xn5 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale5");
                xn5.InnerText = textBox6.Text;

                XmlNode xn6 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationScale6");
                xn6.InnerText = textBox7.Text;

                XmlNode xn7 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount1");
                txtOuput1.Text = xn7.InnerText;

                XmlNode xn8 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount2");
                xn8.InnerText = txtOuput2.Text;

                XmlNode xn9 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount3");
                xn9.InnerText = txtOuput3.Text;

                XmlNode xn10 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount4");
                xn10.InnerText = txtOuput4.Text;

                XmlNode xn11 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount5");
                xn11.InnerText = txtOuput5.Text;

                XmlNode xn12 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PrimaryStationCount6");
                xn12.InnerText = txtOuput6.Text;

                XmlNode xn19 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/PermanentFloorArea");
                xn19.InnerText = txtOuput10.Text;

                XmlNode xn20 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/TemporaryFloorArea");
                xn20.InnerText = txtOuput13.Text;

                XmlNode xn21 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/ProjectTime");
                xn21.InnerText = txtOuput11.Text;

                XmlNode xn22 = xmlDoc.SelectSingleNode("configuration/LNGGasificattionStationRoughEstimate/Investment");
                xn22.InnerText = txtOuput14.Text;

                xmlDoc.Save(path);
            }
        }
    }
}
