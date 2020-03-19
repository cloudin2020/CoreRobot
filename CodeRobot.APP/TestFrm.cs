using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeRobot.APP
{
    public partial class TestFrm : Form
    {
        public TestFrm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string strContent = textBox1.Text;
            //richTextBox1.Text = CodeRobot.Utility.CryptographyHelper.Encryption(strContent, CodeRobot.Utility.CommonHelper.cryptographyKey);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string strContent = richTextBox1.Text;
            //textBox2.Text = CodeRobot.Utility.CryptographyHelper.Decrypt(strContent, CodeRobot.Utility.CommonHelper.cryptographyKey);
        }
    }
}
