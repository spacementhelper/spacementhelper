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
    public partial class UC2 : UserControl
    {
        public UC2()
        {
            InitializeComponent();
        }
        public   void DisplayOutput(string output)
        {
            if (outputTextBox.InvokeRequired)
            {
              
                Invoke(new Action(() => outputTextBox.AppendText(output + Environment.NewLine)));
            }
            else
            {
                outputTextBox.AppendText(output + Environment.NewLine);
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
