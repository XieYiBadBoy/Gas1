using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        private void CheckError(double Input)
        {

            if (Input<0)
            {
                MessageBox.Show("您的输入为负数，管子尺寸初步计算(经济及可行性)计算对象无效!","提示",MessageBoxButtons.OK,MessageBoxIcon.Error);
               
            }
        }
        private void Calculate()
        {
            try {
                double Supply = Convert.ToDouble(txtInput1.Text);
                //CheckError(Supply);
                if (Supply <0)
                {
                    throw new InvalidOperationException("您的输入为负数，管子尺寸初步计算(经济及可行性)计算对象无效!");
                }
                SubstationNumber = Convert.ToInt16(Math.Ceiling(Supply / 2));
                PermanentArea = 5000 * SubstationNumber / 10000;
                if (Supply == 0)
                {
                    ProjectTime = 0;
                    Investment = 0;
                }
                else
                {
                    ProjectTime = 5;
                    Investment = Supply * 1000;
                }


                txtOuput7.Text = SubstationNumber.ToString();
                txtOuput10.Text = SubstationNumber.ToString();
                txtOuput11.Text = ProjectTime.ToString();
                txtOuput13.Text = "0";
                txtOuput14.Text = Investment.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            CngStandardStationSet.Show();


        }
    }
}
