namespace Test
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
            this.button1 = new System.Windows.Forms.Button();
            this.txtNumgen = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLpop = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblBestFit = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblBestPop = new System.Windows.Forms.Label();
            this.zg1 = new ZedGraph.ZedGraphControl();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(214, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtNumgen
            // 
            this.txtNumgen.Location = new System.Drawing.Point(108, 9);
            this.txtNumgen.Name = "txtNumgen";
            this.txtNumgen.Size = new System.Drawing.Size(100, 20);
            this.txtNumgen.TabIndex = 1;
            this.txtNumgen.Text = "1000";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "pocet generacii";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "velkost populacie";
            // 
            // txtLpop
            // 
            this.txtLpop.Location = new System.Drawing.Point(108, 35);
            this.txtLpop.Name = "txtLpop";
            this.txtLpop.Size = new System.Drawing.Size(100, 20);
            this.txtLpop.TabIndex = 3;
            this.txtLpop.Text = "30";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "best fit";
            // 
            // lblBestFit
            // 
            this.lblBestFit.AutoSize = true;
            this.lblBestFit.Location = new System.Drawing.Point(105, 58);
            this.lblBestFit.Name = "lblBestFit";
            this.lblBestFit.Size = new System.Drawing.Size(0, 13);
            this.lblBestFit.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 80);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "best pop";
            // 
            // lblBestPop
            // 
            this.lblBestPop.AutoSize = true;
            this.lblBestPop.Location = new System.Drawing.Point(105, 80);
            this.lblBestPop.Name = "lblBestPop";
            this.lblBestPop.Size = new System.Drawing.Size(0, 13);
            this.lblBestPop.TabIndex = 8;
            // 
            // zg1
            // 
            this.zg1.Location = new System.Drawing.Point(15, 96);
            this.zg1.Name = "zg1";
            this.zg1.ScrollGrace = 0D;
            this.zg1.ScrollMaxX = 0D;
            this.zg1.ScrollMaxY = 0D;
            this.zg1.ScrollMaxY2 = 0D;
            this.zg1.ScrollMinX = 0D;
            this.zg1.ScrollMinY = 0D;
            this.zg1.ScrollMinY2 = 0D;
            this.zg1.Size = new System.Drawing.Size(1352, 797);
            this.zg1.TabIndex = 9;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1379, 905);
            this.Controls.Add(this.zg1);
            this.Controls.Add(this.lblBestPop);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblBestFit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtLpop);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNumgen);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtNumgen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLpop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblBestFit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblBestPop;
        private ZedGraph.ZedGraphControl zg1;
    }
}

