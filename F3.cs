using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static SpacemeshHelper.F2;

namespace SpacemeshHelper
{
    public partial class F3 : Form
    {
        string info;
        public F3(string info)
        {
            this.info = info;
            InitializeComponent();
        }
        private int linesSinceLastUpdate = 0;
        private const int updateFrequency = 5;

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



                if (outputTextBox.Lines.Length > maxLines)
                {

                    outputTextBox.Select(0, outputTextBox.GetFirstCharIndexFromLine(1));
                    outputTextBox.SelectedText = "";
                }
            }
        }






        private void F3_Load(object sender, EventArgs e)
        {



            var conf = $"{info.Split(',')[0]} - {info.Split(',')[1]} - {info.Split(',')[2]} - {info.Split(',')[3]} - {info.Split(',')[4]}";
            this.Text = conf;
        }

        private void F3_FormClosing(object sender, FormClosingEventArgs e)
        {
           
            e.Cancel = true; 
            this.Hide();
        }

        private void F3_VisibleChanged(object sender, EventArgs e)
        {
            if (!this.Visible)
            {
                if (outputTextBox.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate
                    {
                        outputTextBox.Clear(); 
                    });
                }
                else
                {
                    outputTextBox.Clear(); 
                    
                }
            }
        }
    }
}
