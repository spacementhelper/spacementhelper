using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpacemeshHelper
{
    public partial class F2 : Form
    {
        //  public Action<string> DisplayOutputAction { get; set; }

        public delegate void DisplayOutputWithMaxLinesDelegate(string output, int maxLines);

        public F2(string info)
        {
            this.info = info;
            InitializeComponent();
        }
        string info = string.Empty;
        private void F2_Load(object sender, EventArgs e)
        {
            var cardID = info.Split('|')[0];
            var fromfile = info.Split('|')[1];
            var tofile = info.Split('|')[2];
            this.Text = $"  [{cardID}]  ---  FromFile:{fromfile} -- ToFile:{tofile}";
        }
        public void DisplayOutputWithMaxLines(string output, int maxLines)
        {
            if (outputTextBox.InvokeRequired)
            {
                Invoke(new DisplayOutputWithMaxLinesDelegate(DisplayOutputWithMaxLines), output, maxLines);
            }
            else
            {
                outputTextBox.AppendText(output + Environment.NewLine);
                outputTextBox.ScrollToCaret();


                // 检查当前行数是否超过最大行数
                if (outputTextBox.Lines.Length > maxLines)
                {
                    // 移除最旧的行
                    outputTextBox.Select(0, outputTextBox.GetFirstCharIndexFromLine(1));
                    outputTextBox.SelectedText = "";
                }
            }
        }


        private void F2_FormClosed(object sender, FormClosedEventArgs e)
        {
            // UC.display = false;
        }

        private void F2_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 阻止窗口关闭
            e.Cancel = true;

            // 隐藏窗口而不是关闭
            this.Hide();
        }
    }
}
