﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace 天然气供应方案分析与决策软件
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MdiClient m = new MdiClient();
            this.Controls.Add(m);
            m.Dock = DockStyle.Fill;
           // m.BackgroundImage = Properties.Resources.BackgroundImage;
        }


        public Windows1 w1;   // 管道工艺计算 
        public Windows2 w2;   // 母站计算
        public Windows3 w3;   // 子母站计算
        public Windows4 w4;   // 槽车数计算
        public Windows5 w5;   // 标准站计算
        private  AboutBox1 w11;
        public Windows6 w6;    //LNG液化站计算
        public Windows7 w7;    //LNG液化站计算
        public CompressorCalculate compressorcalculate;
        public StellConsumptionCalaulate ConsumptionStell;
        public CNGWindowsProject CNGProjectRoughEstimate;
        public CNGStanardStationProjectAndInvestment CngStandardStationProjectAndInvestment;
        public ComprehensiveAnalysCriticalCurveMethod CriticalCurveMethod;
        public LNGProjectAndInvestment LngProjectAndInvestment;
        public OptionSet Optionset;
        public LowPressureAnalysis LowpressureAnalysis;
        public OutputAmountAnalysis Outputamount;
        public PlumberDiameterAnalysis Plumberdiameteranalysis;
        private  string skinPath = @"Resources\";


        [DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);


        public static bool CapsLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x14] == 1);
            }
        }
        public static bool NumLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x90] == 1);
            }
        }


        public static bool InsertStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x2D] == 1);
            }
        }

        private void 工艺计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w1 = new Windows1();
            w1.MdiParent = this;
            w1.Show();

        }

        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^{V}");
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^{C}");
        }

     
        private void 母站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w2 = new Windows2();
            w2.MdiParent = this;
            w2.Show();
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren.Length>0)
            {
                if (DialogResult.Yes == MessageBox.Show("是否关闭当前窗口？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    this.ActiveMdiChild.Close();
                }
            }
            
        }

        private void 关闭所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否关闭当前所有窗口？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                foreach (Form frm in this.MdiChildren)
                {
                    frm.Close();
                }
                Reset();
            }
        }

        private void 新建ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string SourcePath = "GasSupplyProgramAndDecisionSoftware";
            string destPath = null;
            if (Directory.Exists(SourcePath))//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(SourcePath);
            }
            string strAttURL = "OpenNewFile.gsa";
            if (File.Exists(strAttURL))//如果不存在就创建file文件夹
            {
                destPath = Path.Combine(SourcePath, Path.GetFileName(strAttURL));
                System.IO.File.Copy(@strAttURL, destPath, true);
                OpenProject(destPath);
                XMLOP.filePath = destPath;
                XMLOP.xmlDoc = XDocument.Load(destPath);
            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //1.创建一个文件打开对话框
            OpenFileDialog ofd = new OpenFileDialog();
            //设置对话框属性:允许选择多个文件
            ofd.Multiselect = false;
            ofd.Filter = "gsa(*.gsa)|*.gsa";
            //2.打开对话框
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //3.成功选择，获取被选择的文件名
                string selectedFile = ofd.FileName;        //Multiselect 为 false时;
                //string[] selectedFiles = ofd.FileNames;  //Multiselect 为 true时;
                
                OpenProject(selectedFile);
                XMLOP.filePath = selectedFile;
                XMLOP.xmlDoc = XDocument.Load(selectedFile);
            }
        }

        private void RefreshRecentDocList()
        {
            //TODO:刷新最近文件列表
        }

        private void OpenProject(string selectedFile)
        {
            Common.path = selectedFile;
            this.Text = "中国石油规划总院--" + selectedFile;
            WriteRecentDocumentsToIniFile(selectedFile);    //将打开的文件存入历史文件
            RefreshRecentDocList(); //刷新最近文件列表

            AddInf(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " 打开文件：" + selectedFile);
            bianjiToolStripMenuItem.Visible = true;

            gunadaofenxiToolStripMenuItem.Visible = true;
            CNGfenxiToolStripMenuItem.Visible = true;
            CNGfenxiToolStripMenuItem.Visible = true;
            LNGFENXIToolStripMenuItem.Visible = true;
            PILAINGFENXIToolStripMenuItem.Visible = true;
            ZOHEFENXIToolStripMenuItem.Visible = true;
            TONGJUToolStripMenuItem.Visible = true;
            CHANGKOUToolStripMenuItem.Visible = true;
            GAUNBIToolStripMenuItem.Visible = true;
            GUANBISUOYOUToolStripMenuItem.Visible = true;
            BAOCUNToolStripMenuItem.Visible = true;
            LINGCUNWEIToolStripMenuItem.Visible = true;
            toolStripLabel5.Visible = true;
            toolStripLabel6.Visible = true;
            toolStripLabel7.Visible = true;
            toolStripLabel8.Visible = true;
            toolStripLabel10.Visible = true;
            toolStripSeparator2.Visible = true;
            rtbInf.Visible = true;
            toolStripMenuItem1.Visible = true;
            ToolStripMenuItem42.Checked = true;
            GAUNBIToolStripMenuItem.Enabled = false;

        }

        private void WriteRecentDocumentsToIniFile(string v)
        {
            FileStream fs1 = new FileStream("RecentDocuments.ini", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(fs1, Encoding.Default);
            string txt = sr.ReadToEnd();
            sr.Close();
            fs1.Close();

            FileStream fs2 = new FileStream("RecentDocuments.ini", FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs2,Encoding.Default);
            sw.WriteLine(v);
            sw.Write(txt);
            sw.Flush();
            sw.Close();
            fs2.Close();
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Common.path;
            //SaveFileDialog save = new SaveFileDialog();
            //save.Title = "保存文件";
            //save.Filter = "所有文件|*";
            foreach (Form item in this.MdiChildren)
            {
                switch (item.Text)
                {
                    case "CNG子母站站工程量与投资匡算":
                        //CNGWindowsProject frm = new CNGWindowsProject(this);
                        //frm.SaveCurrentParameters(path,path);
                        break;
                    default:
                        break;
                }
            }
            //TODO
            MessageBox.Show("ok");
            //if (save.ShowDialog() == DialogResult.OK)
            //{
            //    //TODO:保存文件代码
            //}

        }

        private void 子站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w3 = new Windows3();
            w3.MdiParent = this;
            w3.Show();
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否关闭本系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                System.Environment.Exit(0);
            }
            else
            {

            }
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w11 = new AboutBox1();
            w11.Show();
        }

        private void cNG槽车ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w4 = new Windows4();
            w4.MdiParent = this;
            w4.Show();
        }

        private void 标准站计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w5 = new Windows5();
            w5.MdiParent = this;
            w5.Show();
        }

        private void 层叠窗口ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);//层叠
        }

        private void 垂直平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileVertical);  //垂直
        }

        private void 水平平铺ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.TileHorizontal); //水平
        }

        private void 工具ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int count = Properties.Settings.Default.historMaxFiles;
            SetImg();   //为菜单子选项设置图标
            ReadRecentDocumentsInIniFile(count);    //读取历史文件
            this.toolStripStatusLabel3.Text = "系统当前时间：" + DateTime.Now.ToString("yyyy-MM-dd  hh:mm:ss");
            this.toolStripStatusLabel7.Text = "当前操作：     ";
            AddInf(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " 打开软件");
            Reset();
         

            try
            {
                this.BackgroundImage = Image.FromFile( skinPath + "BackgroundImage.jpg");
                CapsLockStatusShow();
                NumLockStatusShow();
                InsertStatusShow();

            }
            catch (Exception)
            {
            }
            this.timer1.Interval = 50;

            this.timer1.Start();
        }

        private void CapsLockStatusShow()
        {
            if (CapsLockStatus == true)
                toolStripStatusLabel2.Text = "大小键状态：  " + "大写锁定";
            else
                toolStripStatusLabel2.Text = "大小键状态：  " +  "小写锁定";
        }

        private void NumLockStatusShow()
        {
            if (NumLockStatus == true)
                toolStripStatusLabel4.Text = "数字键状态：  " + "开启";
            else
                toolStripStatusLabel4.Text = "数字键状态：  " + "关闭";
        }

        private void InsertStatusShow()
        {
            if (InsertStatus == true)
                toolStripStatusLabel5.Text = "插入键状态：  " +  "开启";
            else
                toolStripStatusLabel5.Text = "插入键状态：  " + "关闭";
        }

        private void ReadRecentDocumentsInIniFile(int c)
        {
            if (File.Exists("RecentDocuments.ini"))
            {
                FileStream fs = new FileStream("RecentDocuments.ini", FileMode.Open, FileAccess.Read);
                StreamReader sr = new StreamReader(fs);
                int count = 文件ToolStripMenuItem.DropDownItems.Count;
                int i = count - 2;
                while (sr.Peek() >= 0)
                {
                    string file = sr.ReadLine();
                    ToolStripMenuItem item = new ToolStripMenuItem(file);
                    this.文件ToolStripMenuItem.DropDownItems.Insert(i, item);
                    if (File.Exists(file))
                    {
                        item.Click += new EventHandler(MenuItem_Click);
                    }
                    else
                    {
                        item.Enabled = false;
                    }
                    i++;
                    if (i >= count + (c-2)) 
                    {
                        sr.Close();
                        fs.Close();
                        return;
                    }
                }
            }
        }

        private void MenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = sender as ToolStripMenuItem;
            OpenProject(menu.Text);
        }

        private void SetImg()
        {
            //文件菜单的图标设置
            新建ToolStripMenuItem.Image = Image.FromFile(skinPath + "New.png");
            打开ToolStripMenuItem.Image = Image.FromFile(skinPath + "Open.png");
            GAUNBIToolStripMenuItem.Image = Image.FromFile(skinPath + "Close.png");
            GUANBISUOYOUToolStripMenuItem.Image = Image.FromFile(skinPath + "CloseAll.png");
            BAOCUNToolStripMenuItem.Image = Image.FromFile(skinPath + "Save.png");
            LINGCUNWEIToolStripMenuItem.Image = Image.FromFile(skinPath + "SaveAs.png");
            最近文件ToolStripMenuItem.Image = Image.FromFile(skinPath + "RecentDocuments.png");
            退出ToolStripMenuItem.Image = Image.FromFile(skinPath + "Exit.png");

            toolStripLabel1.Image = Image.FromFile(skinPath + "New.png");
            toolStripLabel2.Image = Image.FromFile(skinPath + "Open.png");
            toolStripLabel3.Image = Image.FromFile(skinPath + "Save.png");
            toolStripLabel5.Image = Image.FromFile(skinPath + "Cut.png");
            toolStripLabel6.Image = Image.FromFile(skinPath + "Copy.png");
            toolStripLabel7.Image = Image.FromFile(skinPath + "Paste.png");
            toolStripLabel8.Image = Image.FromFile(skinPath + "Search.png");
            toolStripLabel10.Image = Image.FromFile(skinPath + "Help.png");

            foreach (ToolStripItem vItem in toolStrip1.Items)
            {
                if (vItem is ToolStripLabel)
                {
                    vItem.Text = "";
                    vItem.AutoSize = false;
                    vItem.Width = 40;
                }
            }
            
        }

        private void Reset()
        {
            bianjiToolStripMenuItem.Visible = false;

            gunadaofenxiToolStripMenuItem.Visible = false;
            CNGfenxiToolStripMenuItem.Visible = false;
            CNGfenxiToolStripMenuItem.Visible = false;
            LNGFENXIToolStripMenuItem.Visible = false;
            PILAINGFENXIToolStripMenuItem.Visible = false;
            ZOHEFENXIToolStripMenuItem.Visible = false;
            TONGJUToolStripMenuItem.Visible = false;
            CHANGKOUToolStripMenuItem.Visible = false;
            GAUNBIToolStripMenuItem.Visible = false;
            GUANBISUOYOUToolStripMenuItem.Visible = false;
            BAOCUNToolStripMenuItem.Visible = false;
            LINGCUNWEIToolStripMenuItem.Visible = false;
            toolStripLabel5.Visible = false;
            toolStripLabel6.Visible = false;
            toolStripLabel7.Visible = false;
            toolStripLabel8.Visible = false;
            toolStripSeparator2.Visible = false;
            rtbInf.Visible = false;
            toolStripMenuItem1.Visible = false;
           

            this.Text = "中国石油规划总院";
        }
        private void 工具栏ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem31.Checked = !ToolStripMenuItem31.Checked;
            toolStrip1.Visible =!toolStrip1.Visible;
           
        }

        private void ToolStripMenuItem32_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem32.Checked = !ToolStripMenuItem32.Checked;
            statusStrip1.Visible = !statusStrip1.Visible ;
        }

        private void lNG液化工厂分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w6 = new Windows6();
            w6.MdiParent = this;
            w6.Show();
        }

        private void lNGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w7 = new Windows7();
            w7.MdiParent = this;
            w7.Show();
        }

        private void 压气站布置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            compressorcalculate = new CompressorCalculate();
            compressorcalculate.MdiParent = this;
            compressorcalculate.Show();
        }

        private void 耗钢量计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
                ConsumptionStell = new StellConsumptionCalaulate();
            ConsumptionStell.MdiParent = this;
            ConsumptionStell.Show();
        }

        private void 工程量匡算ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabel3.Text = "系统当前时间：" + DateTime.Now.ToString("yyyy-MM-dd  hh:mm:ss");
            CapsLockStatusShow();
            NumLockStatusShow();
            InsertStatusShow();
            Form frm = this.ActiveMdiChild;
            if (frm!=null)
            {
                GAUNBIToolStripMenuItem.Enabled = true;
                toolStripStatusLabel7.Text = "当前操作：   " + frm.Text;
            }
            else
            {
                GAUNBIToolStripMenuItem.Enabled = false;
                toolStripStatusLabel7.Text = "当前操作：   " + "无";
            }
                
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^{X}");
        }

        private void 清空粘贴板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
        }

        /// <summary>
        /// 清空当前文本s the tool strip menu item_ click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs" /> instance containing the event data.</param>
        private void 清空当前文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form currentForm = this.ActiveMdiChild;
            if (currentForm!=null)
            {
                foreach (Control c in currentForm.Controls)//遍历控件
                {
                    if (c is TextBox)
                    {
                        //清掉含有TexBox控件上的内容
                        c.Text = "";
                    }
                    if (c is GroupBox)
                    {
                        foreach (Control cc in c.Controls)
                        {
                            if (cc is TextBox)
                            {
                                //清掉含有TexBox控件上的内容
                                cc.Text = "";
                            }
                        }
                    }
                }
            }
        }

        private void 选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Optionset = new OptionSet();
            //Optionset.MdiParent = this;
            Optionset.ShowDialog();
          
        }

        private void 子母站匡算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CNGProjectRoughEstimate = new CNGWindowsProject();
            CNGProjectRoughEstimate.MdiParent = this;
            CNGProjectRoughEstimate.Show();
        }

        private void 标准站匡算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CngStandardStationProjectAndInvestment = new CNGStanardStationProjectAndInvestment();
            CngStandardStationProjectAndInvestment.MdiParent = this;
            CngStandardStationProjectAndInvestment.Show();
        }

        private void 临界曲线法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CriticalCurveMethod = new ComprehensiveAnalysCriticalCurveMethod();
            CriticalCurveMethod.MdiParent = this;
            CriticalCurveMethod.Show();
        }

        private void 工程量匡算ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            LngProjectAndInvestment = new LNGProjectAndInvestment();
            LngProjectAndInvestment.MdiParent = this;
            LngProjectAndInvestment.Show();
        }

        public void AddInf(string v)
        {
            rtbInf.AppendText(v +"\r\n");
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

   

        private void 有月量测算不平均系数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = @"EXCEL工具\01测算月不均匀系数.xlsx"; //由月均测算不平均系数
            System.Diagnostics.Process.Start(path); //打开此文件。
        }

        private void 中间过程ToolStripMenuItem_Click(object sender, EventArgs e)
        {  
            ToolStripMenuItem42.Checked = !ToolStripMenuItem42.Checked;
            rtbInf.Visible = !rtbInf.Visible;
        }

        private void 工子母站程量与投资匡算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CNGProjectRoughEstimate = new CNGWindowsProject();
            CNGProjectRoughEstimate.MdiParent = this;
            CNGProjectRoughEstimate.Show();
        }

        private void 工标准站程量与投资匡算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CngStandardStationProjectAndInvestment = new CNGStanardStationProjectAndInvestment();
            CngStandardStationProjectAndInvestment.MdiParent = this;
            CngStandardStationProjectAndInvestment.Show();
        }

        private void 内容ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = @"帮助\Help.docx"; //打开帮助文档
            System.Diagnostics.Process.Start(path); //打开此文件
        }
            

        private void 归一化月不平均系数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = @"EXCEL工具\02归一化月不均匀系数.xlsx"; //打开归一化月不均匀系数表
            System.Diagnostics.Process.Start(path); //打开此文件
        }

        private void 由年量测算月量ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = @"EXCEL工具\03由年量测算月量.xlsx"; //打开由年量测算月量表
            System.Diagnostics.Process.Start(path); //打开此文件
        }
        
        private void toolStripLabel1_Click(object sender, EventArgs e)
        {
            新建ToolStripMenuItem.PerformClick();
        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {
            打开ToolStripMenuItem.PerformClick();
        }

        private void toolStripLabel3_Click(object sender, EventArgs e)
        {
            BAOCUNToolStripMenuItem.PerformClick();
        }

        private void ZOHEFENXIToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel5_Click(object sender, EventArgs e)
        {
            剪切ToolStripMenuItem.PerformClick();
        }

        private void toolStripLabel6_Click(object sender, EventArgs e)
        {
            复制ToolStripMenuItem.PerformClick();
        }

        private void toolStripLabel7_Click(object sender, EventArgs e)
        {
            粘贴ToolStripMenuItem.PerformClick();
        }

        private void 查找ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^{F}");
        }

        private void toolStripLabel8_Click(object sender, EventArgs e)
        {
            查找ToolStripMenuItem.PerformClick();
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripLabel10_Click(object sender, EventArgs e)
        {
            内容ToolStripMenuItem.PerformClick();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {

        }

        private void LINGCUNWEIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string projectPath = Common.path;
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "另存为";
            save.Filter = "gsa(*.gsa)|*.gsa";//设置文件类型
            
            if (save.ShowDialog() == DialogResult.OK)
            {
                string path = save.FileName;
                System.IO.File.Copy(projectPath, path, true);
                OpenProject(path);
            }

        }

        private void 文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
             //ReadRecentDocumentsInIniFile(count);    //读取历史文件
        }

        private void 下端压力分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LowpressureAnalysis = new LowPressureAnalysis();
            LowpressureAnalysis.MdiParent = this;
            LowpressureAnalysis.Show();
        }

        private void PILAINGFENXIToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void 输量分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            Outputamount = new OutputAmountAnalysis();
            Outputamount.MdiParent = this;
            Outputamount.Show();
        }

        private void 管径分析ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Plumberdiameteranalysis = new PlumberDiameterAnalysis();
            Plumberdiameteranalysis.MdiParent = this;
            Plumberdiameteranalysis.Show();
        }

        private void rtbInf_TextChanged(object sender, EventArgs e)
        {

        }

        private void 层次分析法ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ComprisiveAnalysisAHP AnalyticHierarchyProcess = new ComprisiveAnalysisAHP();
            AnalyticHierarchyProcess.MdiParent = this;
            AnalyticHierarchyProcess.Show();
        }
    }
}
