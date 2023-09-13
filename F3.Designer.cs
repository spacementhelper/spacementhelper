namespace SpacemeshHelper
{
    partial class F3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            outputTextBox = new RichTextBox();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(outputTextBox);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1092, 436);
            panel1.TabIndex = 0;
            // 
            // outputTextBox
            // 
            outputTextBox.BackColor = Color.Black;
            outputTextBox.Dock = DockStyle.Fill;
            outputTextBox.ForeColor = Color.White;
            outputTextBox.Location = new Point(0, 0);
            outputTextBox.Name = "outputTextBox";
            outputTextBox.Size = new Size(1092, 436);
            outputTextBox.TabIndex = 0;
            outputTextBox.Text = "";
            // 
            // F3
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1092, 436);
            Controls.Add(panel1);
            Name = "F3";
            Text = "F3";
            FormClosing += F3_FormClosing;
            Load += F3_Load;
            VisibleChanged += F3_VisibleChanged;
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private RichTextBox outputTextBox;
    }
}