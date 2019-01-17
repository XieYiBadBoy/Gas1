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
    public partial class OptionSet : Form
    {
        public OptionSet()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(numericUpDown1.Value);
            Properties.Settings.Default.historMaxFiles = count;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void OptionSet_Load(object sender, EventArgs e)
        {
            int count = Properties.Settings.Default.historMaxFiles;
            numericUpDown1.Value = count;
        }
        private void button6_Click(object sender, EventArgs e)
        {
            int count = Convert.ToInt32(numericUpDown1.Value);
            Properties.Settings.Default.historMaxFiles = count;
            Properties.Settings.Default.Save();
        }
    }
}
