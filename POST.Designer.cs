namespace SpacemeshHelper
{
    partial class POST
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            panel3 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBox1 = new PictureBox();
            button2 = new Button();
            label7 = new Label();
            label4 = new Label();
            button1 = new Button();
            label2 = new Label();
            label3 = new Label();
            label1 = new Label();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel3
            // 
            panel3.Controls.Add(pictureBox2);
            panel3.Controls.Add(pictureBox1);
            panel3.Controls.Add(button2);
            panel3.Controls.Add(label7);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(button1);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label1);
            panel3.Location = new Point(1, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(744, 32);
            panel3.TabIndex = 1;
            // 
            // pictureBox2
            // 
            pictureBox2.InitialImage = null;
            pictureBox2.Location = new Point(437, 5);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(36, 21);
            pictureBox2.TabIndex = 7;
            pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.InitialImage = null;
            pictureBox1.Location = new Point(491, 5);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(36, 21);
            pictureBox1.TabIndex = 7;
            pictureBox1.TabStop = false;
            // 
            // button2
            // 
            button2.Location = new Point(588, 0);
            button2.Name = "button2";
            button2.Size = new Size(75, 32);
            button2.TabIndex = 1;
            button2.Text = "Restart";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(387, 8);
            label7.Name = "label7";
            label7.Size = new Size(32, 17);
            label7.TabIndex = 0;
            label7.Text = "0GB";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(534, 8);
            label4.Name = "label4";
            label4.Size = new Size(36, 17);
            label4.TabIndex = 0;
            label4.Text = "3000";
            // 
            // button1
            // 
            button1.Location = new Point(666, 0);
            button1.Name = "button1";
            button1.Size = new Size(75, 32);
            button1.TabIndex = 1;
            button1.Text = "display";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(615, 8);
            label2.Name = "label2";
            label2.Size = new Size(36, 17);
            label2.TabIndex = 0;
            label2.Text = "3000";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(35, 8);
            label3.Name = "label3";
            label3.Size = new Size(20, 17);
            label3.TabIndex = 0;
            label3.Text = "w:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(3, 8);
            label1.Name = "label1";
            label1.Size = new Size(15, 17);
            label1.TabIndex = 0;
            label1.Text = "1";
            // 
            // POST
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel3);
            Name = "POST";
            Size = new Size(746, 32);
            Load += POST_Load;
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel3;
        private Button button1;
        private Label label1;
        private Label label2;
        private Label label3;
        private Button button2;
        private Label label4;
        private Label label7;
        private PictureBox pictureBox2;
        private PictureBox pictureBox1;
    }
}
