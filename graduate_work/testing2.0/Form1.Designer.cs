namespace testing2._0
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.ib1 = new Emgu.CV.UI.ImageBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.trackBar2 = new System.Windows.Forms.TrackBar();
            this.trackBar3 = new System.Windows.Forms.TrackBar();
            this.trackBar4 = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.ib1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).BeginInit();
            this.SuspendLayout();
            // 
            // ib1
            // 
            this.ib1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ib1.Enabled = false;
            this.ib1.Location = new System.Drawing.Point(12, 12);
            this.ib1.Name = "ib1";
            this.ib1.Size = new System.Drawing.Size(400, 300);
            this.ib1.TabIndex = 2;
            this.ib1.TabStop = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(871, 23);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(259, 282);
            this.textBox1.TabIndex = 3;
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(900, 321);
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(172, 45);
            this.trackBar1.TabIndex = 4;
            this.trackBar1.Value = 1;
            // 
            // trackBar2
            // 
            this.trackBar2.Location = new System.Drawing.Point(900, 372);
            this.trackBar2.Minimum = 1;
            this.trackBar2.Name = "trackBar2";
            this.trackBar2.Size = new System.Drawing.Size(172, 45);
            this.trackBar2.TabIndex = 5;
            this.trackBar2.Value = 1;
            // 
            // trackBar3
            // 
            this.trackBar3.Location = new System.Drawing.Point(900, 423);
            this.trackBar3.Maximum = 300;
            this.trackBar3.Minimum = -300;
            this.trackBar3.Name = "trackBar3";
            this.trackBar3.Size = new System.Drawing.Size(172, 45);
            this.trackBar3.TabIndex = 6;
            this.trackBar3.Value = 1;
            // 
            // trackBar4
            // 
            this.trackBar4.Location = new System.Drawing.Point(900, 474);
            this.trackBar4.Maximum = 300;
            this.trackBar4.Minimum = -300;
            this.trackBar4.Name = "trackBar4";
            this.trackBar4.Size = new System.Drawing.Size(172, 45);
            this.trackBar4.TabIndex = 7;
            this.trackBar4.Value = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 723);
            this.Controls.Add(this.trackBar4);
            this.Controls.Add(this.trackBar3);
            this.Controls.Add(this.trackBar2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ib1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ib1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar4)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Emgu.CV.UI.ImageBox ib1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TrackBar trackBar2;
        private System.Windows.Forms.TrackBar trackBar3;
        private System.Windows.Forms.TrackBar trackBar4;
    }
}

