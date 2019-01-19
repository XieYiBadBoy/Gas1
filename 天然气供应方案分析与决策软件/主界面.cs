using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        //public AboutBox1 W1;  // 母站计算
        public Windows6 w6;    //LNG液化站计算
        public Windows7 w7;    //LNG液化站计算
        public CompressorCalculate compressorcalculate;
        public StellConsumptionCalaulate ConsumptionStell;
        public CNGWindowsProject CNGProjectRoughEstimate;
        public CNGStanardStationProjectAndInvestment CngStandardStationProjectAndInvestment;
        public ComprehensiveAnalysCriticalCurveMethod CriticalCurveMethod;
        public LNGProjectAndInvestment LngProjectAndInvestment;
        public OptionSet Optionset;

        private  string skinPath = @"Resources\";

        private void 工艺计算ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w1 = new Windows1();
            w1.MdiParent = this;
            w1.Show();

        }
        #region  这部分没解决  复制、粘贴 
        private void 粘贴ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^{V}");
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^{C}");
        }

        #endregion
        private void 母站ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            w2 = new Windows2();
            w2.MdiParent = this;
            w2.Show();
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.MdiChildren != null)
            {
                if (DialogResult.Yes == MessageBox.Show("是否关闭当前窗口？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    //TODO:save fiel or parameters
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
            string SourcePath = "C:\\天然气供应方案与决策软件";
            string destPath = null;
            if (Directory.Exists(SourcePath) == false)//如果不存在就创建file文件夹
            {
                Directory.CreateDirectory(SourcePath);
            }
            string strAttURL = "OpenNewFile.xml";
            if (File.Exists(strAttURL) == true)//如果不存在就创建file文件夹
            {
                destPath = Path.Combine(SourcePath, Path.GetFileName(strAttURL));
                System.IO.File.Copy(@strAttURL, destPath, true);
                OpenProject(destPath);
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
            }
        }

        private void OpenProject(string selectedFile)
        {
            Common.path = selectedFile;
            WriteRecentDocumentsToIniFile(selectedFile);    //将打开的文件存入历史文件

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
            //toolStripLabel1.Visible = true;
            //toolStripLabel2.Visible = true;
            //toolStripLabel3.Visible = true;
            toolStripLabel5.Visible = true;
            toolStripLabel6.Visible = true;
            toolStripLabel7.Visible = true;
            toolStripLabel8.Visible = true;
            toolStripLabel10.Visible = true;
            //toolStripSeparator1.Visible = true;
            toolStripSeparator2.Visible = true;
            rtbInf.Visible = true;
            toolStripMenuItem1.Visible = true;
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
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "保存文件";
            save.Filter = "所有文件|*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                //TODO:保存文件代码
            }

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
            this.toolStripStatusLabel3.Text = "系统当前时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
            AddInf(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " 打开软件");
            Reset();

            try
            {
                this.BackgroundImage = Image.FromFile( skinPath + "BackgroundImage.jpg");
            }
            catch (Exception)
            {
            }
            this.timer1.Interval = 1000;

            this.timer1.Start();
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
            //toolStripLabel1.Visible = false;
            //toolStripLabel2.Visible = false;
            //toolStripLabel3.Visible = false;
            toolStripLabel5.Visible = false;
            toolStripLabel6.Visible = false;
            toolStripLabel7.Visible = false;
            toolStripLabel8.Visible = false;
            //toolStripLabel10.Visible = false;
            //toolStripSeparator1.Visible = false;
            toolStripSeparator2.Visible = false;
            rtbInf.Visible = false;
            toolStripMenuItem1.Visible = false;
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
            this.toolStripStatusLabel3.Text = "系统当前时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }

        private void 剪切ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendKeys.Send("^{X}");
        }

        private void 清空粘贴板ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
        }

        private void 清空当前文本ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Control c in this.Controls)//遍历控件
            {
                if (c.Focused)//判断是否有焦点
                {
                    c.Text = "";
                    //return;
                }
            }
        }

        private void 选项ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Optionset = new OptionSet();
            //Optionset.MdiParent = this;
            Optionset.ShowDialog();
            SendKeys.Send("^{F}");
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
            string path = @"C:\Users\Administrator\Desktop\EXCEL工具\01测算月不均匀系数.xlsx"; //由月均测算不平均系数
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
            string path = @"EXCEL工具\Help.docx"; //打开帮助文档
            System.Diagnostics.Process.Start(path); //打开此文件
        }
            

        private void 归一化月不平均系数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = @"C:\Users\Administrator\Desktop\EXCEL工具\02归一化月不均匀系数.xlsx"; //打开归一化月不均匀系数表
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
    }
}
