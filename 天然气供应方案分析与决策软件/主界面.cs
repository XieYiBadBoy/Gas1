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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            MdiClient m = new MdiClient();
            this.Controls.Add(m);
            m.Dock = DockStyle.Fill;
            m.BackgroundImage = Properties.Resources.BackgroundImage;
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
            if (DialogResult.Yes == MessageBox.Show("是否关闭本系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
               this.Close();
            }
            else
            {
            
            }
      
        }

        private void 关闭所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("是否关闭本系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                System.Environment.Exit(0);
            }
            else
            {

            }
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //1.创建一个文件打开对话框
            OpenFileDialog ofd = new OpenFileDialog();
            //设置对话框属性:允许选择多个文件
            //ofd.Multiselect = true;
            //2.打开对话框
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //3.成功选择，获取被选择的文件名
                string selectedFile = ofd.FileName;        //Multiselect 为 false时;
                string[] selectedFiles = ofd.FileNames;  //Multiselect 为 true时;
                                                         //TODO...
            }

        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Title = "保存文件";
            save.Filter = "所有文件|*";
            if (save.ShowDialog() == DialogResult.OK)
            {
                //保存文件代码
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
            if (DialogResult.Yes == MessageBox.Show("是否关闭本系统？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Stop))
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
            this.toolStripStatusLabel3.Text = "系统当前时间：" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

            this.timer1.Interval = 1000;

            this.timer1.Start();
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
    }
}
