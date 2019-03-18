﻿using System;
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
    public partial class LowPressureAnalysis : Form
    {
        public LowPressureAnalysis()
        {
            InitializeComponent();
        }
        private Common CommonCalcuiate = new Common();
        private void Calculate()
        {
            try
            {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load("XMLFile1.xml"); //加载xml文件
                string str1 = lblInput2.Text;
                string str2 = txtInput2.Text;
                string str3 = lblInput3.Text;
                string str4 = txtInput3.Text;
                string str5 = lblInput4.Text;
                string str6 = txtInput4.Text;
                Common.ParameterErrorDetectioPressure(str1, str2);
                Common.ParameterErrorDetectioLength(str3, str4);
                Common.ParameterErrorDetectioDiameter(str5, str6);

                int Rownumber = Convert.ToInt32(txtInput1.Text);
                double UpPressure = Convert.ToDouble(txtInput2.Text);
                double PipeLength = Convert.ToDouble(txtInput3.Text);
                double PipeDiameter = Convert.ToDouble(txtInput4.Text);

                double InitialStandardFlow = Convert.ToDouble(ReadXml(xmlDoc, "InitialStandardFlow"));
                double DiaRough = Convert.ToDouble(ReadXml(xmlDoc, "DiaRough"));
                double GasWeigh = Convert.ToDouble(ReadXml(xmlDoc, "GasWeight"));
                double AverageTep = Convert.ToDouble(ReadXml(xmlDoc, "GasTep"));
                double Interval = Convert.ToDouble(ReadXml(xmlDoc, "Interval"));
                MessageBox.Show(Convert.ToString(InitialStandardFlow));
                MessageBox.Show(Convert.ToString(Interval));
                int index = this.dataGridView1.Rows.Add(Rownumber);
                int index1 = this.dataGridView2.Rows.Add(Rownumber);
                int k = 1;
                for (int i = 0; i < Rownumber; i++)
                {
                    this.dataGridView1.Rows[i].HeaderCell.Value = Convert.ToString(k);
                    this.dataGridView2.Rows[i].HeaderCell.Value = Convert.ToString(k);
                    this.dataGridView1.Rows[i].Cells[0].Value = Convert.ToString(InitialStandardFlow);
                    this.dataGridView2.Rows[i].Cells[0].Value = CommonCalcuiate.LowPressureAnalysis(InitialStandardFlow, UpPressure, PipeDiameter, DiaRough, PipeLength, GasWeigh, AverageTep).ToString("0.0000");
                    this.dataGridView2.Rows[i].Cells[1].Value = CommonCalcuiate.FlowSpeed(InitialStandardFlow, UpPressure, PipeDiameter, DiaRough, PipeLength, GasWeigh, AverageTep).ToString("0.0000");
                    this.dataGridView2.Rows[i].Cells[2].Value =CommonCalcuiate.LeiNuoXiShuVar(InitialStandardFlow, UpPressure, PipeDiameter, DiaRough, PipeLength, GasWeigh, AverageTep).ToString("0.0000");
                    this.dataGridView2.Rows[i].Cells[3].Value = CommonCalcuiate.DaXiXiShuVar(InitialStandardFlow, UpPressure, PipeDiameter, DiaRough, PipeLength, GasWeigh, AverageTep).ToString("0.000000");
                    InitialStandardFlow = Interval + InitialStandardFlow;
                    k++;
                }
                Properties.Settings.Default.rowcount= dataGridView1.Rows.Count;
                Properties.Settings.Default.Save();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private object ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/BatchAnalysis/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }

        private void ClearRowCount()
        {
            int rowcount = dataGridView1.Rows.Count;
            for (int i = 1; i <rowcount; i++)
            {

                MessageBox.Show(Convert.ToString (dataGridView1.Rows.Count));
                MessageBox.Show(Convert.ToString(i));
                dataGridView1.Rows.RemoveAt(i);
             
            }
        }
        private void removeRows(DataGridView dgv)
        {

            foreach (DataGridViewRow row in dgv.Rows)
            {
                // if some condition holds
                dgv.Rows.Remove(row);
            }
            dgv.Refresh();

        }
        private void LowPressureAnalysis_Load(object sender, EventArgs e)
        {
            this.dataGridView1.TopLeftHeaderCell.Value = "序号";
            this.dataGridView2.TopLeftHeaderCell.Value = "序号";
            int index = this.dataGridView1.Rows.Add(1);
            int k = 0;
            //MessageBox.Show(Convert.ToString(index));
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                this.dataGridView1.Rows[i].HeaderCell.Value = Convert.ToString(k);
                k++;
            }
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView2.EnableHeadersVisualStyles = false;

            ////设置标题高度;
            //dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            //dataGridView1.ColumnHeadersHeight = 40;
            //设置标题内容居中显示;
            dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dataGridView1.Columns[0].HeaderText = "供应量\r\n" + "（万方 / 天）";


            //设置标题高度;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridView2.ColumnHeadersHeight = 40;
            //设置标题内容居中显示;
            dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            this.dataGridView2.Columns[0].HeaderText = "下端压力\r\n" + "（兆帕）";
            this.dataGridView2.Columns[1].HeaderText = "流速\r\n" + "（米/秒）";
            this.dataGridView2.Columns[2].HeaderText = "雷诺数\r\n" + "（无单位）";
            this.dataGridView2.Columns[3].HeaderText = "达西摩阻系数\r\n" + "（无单位）";

            Properties.Settings.Default.rowcount = dataGridView1.Rows.Count;
            Properties.Settings.Default.Save();                                                                           


        }

        private void button6_Click(object sender, EventArgs e)
        {

            //ClearRowCount();
            Calculate();
         
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

        private void txtInput1_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
            //输入0-9和Backspace del 有效
            if ((e.KeyChar >= 47 && e.KeyChar <= 58) || e.KeyChar == 8)
            {
                e.Handled = false;
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

        private void button1_Click(object sender, EventArgs e)
        {
            BatchAnalysisLowPressureSet LowpressureAnalysis = new BatchAnalysisLowPressureSet();
            LowpressureAnalysis.ShowDialog();
        }

        private void txtInput3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
