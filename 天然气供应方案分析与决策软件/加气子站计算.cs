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
    public partial class Windows3 : Form
    {
        public Windows3()
        {
            InitializeComponent();
        }
        private MathOpt calculator2 = new MathOpt();
        SetStationSetup SecSetup = new SetStationSetup();
        private void Change1()
        {
            
            lblInput2.Text = "设计体积流量：";
          
            lblInput4.Text = "压缩机出口压力：";
            txtInput4.Visible = true ;
            txtInput5.Visible = true ;
            txtInput6.Visible = true ;
            txtInput7.Visible = true ;
           
        
            comInput1.Visible = true;
            lblInput5.Text = "压缩机入口压力：";
            lblInput6.Text = "低压区容积：";
            lblInput7.Text = "中压区容积：";
            lblInput8.Text = "高压区容积=";
            lblInput9.Text = "：";
            lblInput10.Text = "：1";
            lblInput11.Text = "压缩机补气时间：";
        
         
        
            label9.Text = "万方/日";
           
            label11.Text = "兆帕";
            label12.Text = "兆帕";
            label13.Text = "小时/日";
          
            label16.Text = "台";
            label17.Text = "立方米";
            label18.Text = "立方米";
            label19.Text = "立方米";
            label20.Text = "个";
            label21.Text = "个";
            label22.Text = "个";
            label23.Text = "台";
            label24.Text = "立方米";
            lblOutput1.Text = "压缩机的台数：";
            lblOutput2.Text = "低压区储气容积：";
            lblOutput3.Text = "中压区储气容积：";
            lblOutput4.Text = "高压区储气容积：";
            lblOutput5.Text = "低压区井管数量：";
            lblOutput6.Text = "中压区井管数量：";
            lblOutput7.Text = "高压区井管数量：";
            lblOutput8.Text = "子站加气机数量：";
            lblOutput9.Text = "CNG子站总面积：";
            txtOutput4.Visible = true ;
            txtOutput6.Visible = true ;
            txtOutput7.Visible = true ;
            txtOutput8.Visible = true ;
            txtOutput9.Visible = true ;
            txtOutput10.Visible = true ;
            txtOutput11.Visible = true ;
        }
        private void Change2()
        {
          
            lblInput2.Text = "设计体积流量：";
           
            lblInput5.Text = "每日工作时长：";
            txtInput4.Visible = false;
            txtInput6.Visible = false;
            txtInput7.Visible = false;
            
          
            comInput1.Visible = false;
            lblInput4.Text = "";
            lblInput6.Text = "";
            lblInput7.Text = "";
            lblInput8.Text = "";
            lblInput9.Text = "";
            lblInput10.Text = "";
            lblInput11.Text = "";

            label9.Text = "万方/日";
          
            label12.Text = "小时/日";
            label11.Text = "";
            label13.Text = "";
         
            label16.Text = "台";
            label17.Text = "立方米";
            label18.Text = "";
            label19.Text = "";
            label20.Text = "";
            label21.Text = "";
            label22.Text = "";
            label23.Text = "";
            label24.Text = "";
            lblOutput1.Text = "子站加气机数量：";
            lblOutput2.Text = "CNG子站总面积：";
            lblOutput3.Text = "";
            lblOutput4.Text = "";
            lblOutput5.Text = "";
            lblOutput6.Text = "";
            lblOutput7.Text = "";
            lblOutput8.Text = "";
            lblOutput9.Text = "";
            txtOutput4.Visible = false;
            txtOutput6.Visible = false;
            txtOutput7.Visible = false;
            txtOutput8.Visible = false;
            txtOutput9.Visible = false;
            txtOutput10.Visible = false;
            txtOutput11.Visible = false;
        }
        private void Add3()
        {
            try
            {
                string str1 = lblInput2.Text;
                string str2 = txtInput2.Text;
                string str3 = lblInput4.Text;
                string str4 = txtInput4.Text;
                string str5 = lblInput5.Text;
                string str6 = txtInput5.Text;
                Common.ParameterErrorDetectionFlow(str1, str2);
                Common.ParameterErrorDetectioPressure(str1, str2);
                Common.ParameterErrorDetectioPressure(str1, str2);


                double z = Convert.ToDouble(SecSetup .SecStationFactor);    //压缩机因子
                double StdFlow3 = Convert.ToDouble(txtInput2.Text);   //标准体积流量       StdFlow
                double Time = Convert.ToDouble(SecSetup.SecStationTime);    //子站加气机工作时间 Time  
                double ExitPre = Convert.ToDouble(txtInput4.Text);   //压缩机出口压力     ExitPre  
                double UpPre = Convert.ToDouble(txtInput5.Text);     //压缩机入口压力   UpPre       
                double LowProp = Convert.ToDouble(txtInput6.Text);  //低压区容积比例
                double MidProp = Convert.ToDouble(txtInput7.Text);   //中压区容积比例
                double TaxiNum = Convert.ToDouble(SecSetup.SecStationTaxiNum);   //出租车数量
                double BusNum = Convert.ToDouble(SecSetup.SecStationBusNum);    //公交车数量
                double CompTime = Convert.ToDouble(comInput1.Text);  //压缩机补气时间
                double PreRat = calculator2.CompressorPower(z,StdFlow3,Time,UpPre,ExitPre);
                double TotalArea = 1550 + 0.1 * StdFlow3;
                double CNGnum = Math.Ceiling(calculator2.CNGNum(TaxiNum, BusNum, Time));
                double Vg31 = calculator2.LowPer(LowProp, MidProp, StdFlow3, CompTime);
                double Vg21 = MidProp * Vg31;
                double Vg11 = LowProp * Vg31;
                double n3 = Vg31 / 2700;
                double n2 = Vg21 / 2700;
                double n1 = Vg11 / 2700;
                txtOutput3.Text = PreRat.ToString("");
                txtOutput4.Text = TotalArea.ToString("0.00");
                txtOutput5.Text = Vg31.ToString("0.00");
                txtOutput6.Text = Vg21.ToString("0.00");
                txtOutput7.Text = Vg11.ToString("0.00");
                txtOutput8.Text = Math.Ceiling(n3).ToString();
                txtOutput9.Text = Math.Ceiling(n2).ToString();
                txtOutput10.Text = Math.Ceiling(n1).ToString();
                txtOutput11.Text = CNGnum.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Add4()
        {
            try
            {


                double StdFlow4 = Convert.ToDouble(txtInput2.Text);   //标准体积流量       StdFlow
                double Time = Convert.ToDouble(txtInput5.Text);      //每日工作时长       Time
                double TaxiNum = Convert.ToDouble(SecSetup.SecStationTaxiNum);   //出租车数量    TaxiNum  
                double BarNum = Convert.ToDouble(SecSetup.SecStationBusNum);       //公交车数量     BarNum
                double Num11 = calculator2.CNGNum(TaxiNum, BarNum, Time);
                double TotalArea = 1000 + 0.1 * StdFlow4;
                txtOutput3.Text = Math.Ceiling (Num11).ToString("");
                txtOutput5.Text = TotalArea.ToString("0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void Clobutton3_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void Calbutton1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Add3();
            }
            else if (radioButton2.Checked == true)
            {
                Add4();
            }
            else
            {
                MessageBox.Show("请选择计算类型");
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblInput2_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Change2();
            Clear2();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Change1();
            Clear1();
        }
        private void Clear()
        {
        }
        private void Clear1()
        {
            txtInput2.Text = "";
            txtInput7.Text = "";
            txtInput4.Text = "";
            txtInput5.Text = "";
            txtInput6.Text = "";
            txtInput2.Text = "";
            txtOutput3.Text = "";
            txtOutput4.Text = "";
            txtOutput5.Text = "";
            txtOutput6.Text = "";
            txtOutput7.Text = "";
            txtOutput9.Text = "";
            txtOutput8.Text = "";
            txtOutput10.Text = "";
            txtOutput11.Text = ""; 
        }
        private void Clear2()
        {
            txtInput2.Text = "";
            txtInput5.Text = "";
            txtOutput3.Text = "";
            txtOutput5.Text = "";
        }
        private void Clebutton2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Clear1();
            }
            else if (radioButton2.Checked == true)
            {
                Clear2();
            }
            else
            {
                Clear1();
            }
        }

        private void lblOutput4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SecSetup.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string path = null;
            saveFile.Filter = "gsa(*.gsa)|*.gsa";//设置文件类型
            saveFile.FileName = "GAS";//设置默认文件名
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                path = saveFile.FileName;
                SaveCurrentParameters1("OpenNewFile.gsa", path);
            }
        }

        private void SaveCurrentParameters1(string sourcePath, string targetPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(sourcePath); //加载xml文件

            XmlNode xn0 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/StdFlow");
            xn0.InnerText = txtInput2.Text;

            XmlNode xn1 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/ExitPre");
            xn1.InnerText = txtInput4.Text;

            XmlNode xn2 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/UpPre");
            xn2.InnerText = txtInput5.Text;

            XmlNode xn3 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/LowPressureVolumeRatio");
            xn3.InnerText = txtInput6.Text;

            XmlNode xn4 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/MiddlePressureVolumeRatio");
            xn4.InnerText = txtInput7.Text;

            XmlNode xn6 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/CompressorFillingTime");
            xn6.InnerText = comInput1.Text;

            XmlNode xn7 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/CompressorCount");
            xn7.InnerText = txtOutput3.Text;

            XmlNode xn8 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/CngTotalArea");
            xn8.InnerText = txtOutput4.Text;

            XmlNode xn9= xmlDoc.SelectSingleNode("configuration/CNGStandardStation/LowPressureVolume");
            xn9.InnerText = txtOutput5.Text;

            XmlNode xn10= xmlDoc.SelectSingleNode("configuration/CNGStandardStation/MIddlePressureVolume");
            xn10.InnerText = txtOutput6.Text;

            XmlNode xn11 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/HighPressureVolume");
            xn8.InnerText = txtOutput7.Text;

            XmlNode xn12 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/LowPressureCount");
            xn12.InnerText = txtOutput8.Text;

            XmlNode xn13 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/MiddlePresureCount");
            xn13.InnerText = txtOutput9.Text;

            XmlNode xn14 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/HighPressureCount");
            xn14.InnerText = txtOutput10.Text;

            XmlNode xn15 = xmlDoc.SelectSingleNode("configuration/CNGStandardStation/CngStationaAddGasCount");
            xn15.InnerText = txtOutput11.Text;

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
                LoadCurrentParameters1(path);
            }
        }

        private void LoadCurrentParameters1(string path)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path); //加载xml文件
            txtInput2.Text = ReadXml(xmlDoc, "StdFlow");
            txtInput4.Text = ReadXml(xmlDoc, "ExitPre");
            txtInput5.Text = ReadXml(xmlDoc, "UpPre");
            txtInput6.Text = ReadXml(xmlDoc, "LowPressureVolumeRatio");
            txtInput7.Text = ReadXml(xmlDoc, "MiddlePressureVolumeRatio");
            comInput1.Text = ReadXml(xmlDoc, "CompressorFillingTime");
            txtOutput3.Text = ReadXml(xmlDoc, "CompressorCount");
            txtOutput4.Text = ReadXml(xmlDoc, "CngTotalArea");
            txtOutput5.Text = ReadXml(xmlDoc, "LowPressureVolume");
            txtOutput6.Text = ReadXml(xmlDoc, "MIddlePressureVolume");
            txtOutput7.Text = ReadXml(xmlDoc, "HighPressureVolume");
            txtOutput8.Text = ReadXml(xmlDoc, "LowPressureCount");
            txtOutput9.Text = ReadXml(xmlDoc, "MiddlePresureCount");
            txtOutput10.Text = ReadXml(xmlDoc, "HighPressureCount");
            txtOutput11.Text = ReadXml(xmlDoc, "CngStationaAddGasCount");
        }

        private string ReadXml(XmlDocument xmlDoc, string s)
        {
            string Str = "configuration/CNGStandardStation/" + s;
            XmlNode xn0 = xmlDoc.SelectSingleNode(Str);
            return xn0.InnerText;
        }

        private void Windows3_Load(object sender, EventArgs e)
        {

        }
    }
}
