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
            this.label5 = new System.Windows.Forms.Label();
            this.txtSelrand = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSelbest = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSeltourn = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtNewPop = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtMuta = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtMutx = new System.Windows.Forms.TextBox();
            this.txtError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(878, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 20);
            this.button1.TabIndex = 0;
            this.button1.Text = "Vypocitaj";
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(231, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "selrand";
            // 
            // txtSelrand
            // 
            this.txtSelrand.Location = new System.Drawing.Point(327, 35);
            this.txtSelrand.Name = "txtSelrand";
            this.txtSelrand.Size = new System.Drawing.Size(100, 20);
            this.txtSelrand.TabIndex = 12;
            this.txtSelrand.Text = "7";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(231, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "selbest";
            // 
            // txtSelbest
            // 
            this.txtSelbest.Location = new System.Drawing.Point(327, 9);
            this.txtSelbest.Name = "txtSelbest";
            this.txtSelbest.Size = new System.Drawing.Size(100, 20);
            this.txtSelbest.TabIndex = 10;
            this.txtSelbest.Text = "1, 1";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(449, 38);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "seltourn";
            // 
            // txtSeltourn
            // 
            this.txtSeltourn.Location = new System.Drawing.Point(545, 35);
            this.txtSeltourn.Name = "txtSeltourn";
            this.txtSeltourn.Size = new System.Drawing.Size(100, 20);
            this.txtSeltourn.TabIndex = 16;
            this.txtSeltourn.Text = "14";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(449, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(52, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "nova pop";
            // 
            // txtNewPop
            // 
            this.txtNewPop.Location = new System.Drawing.Point(545, 9);
            this.txtNewPop.Name = "txtNewPop";
            this.txtNewPop.Size = new System.Drawing.Size(100, 20);
            this.txtNewPop.TabIndex = 14;
            this.txtNewPop.Text = "7";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(665, 38);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(30, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "muta";
            // 
            // txtMuta
            // 
            this.txtMuta.Location = new System.Drawing.Point(761, 35);
            this.txtMuta.Name = "txtMuta";
            this.txtMuta.Size = new System.Drawing.Size(100, 20);
            this.txtMuta.TabIndex = 20;
            this.txtMuta.Text = "0.2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(665, 12);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "mutx";
            // 
            // txtMutx
            // 
            this.txtMutx.Location = new System.Drawing.Point(761, 9);
            this.txtMutx.Name = "txtMutx";
            this.txtMutx.Size = new System.Drawing.Size(100, 20);
            this.txtMutx.TabIndex = 18;
            this.txtMutx.Text = "0.25";
            // 
            // txtError
            // 
            this.txtError.AutoSize = true;
            this.txtError.ForeColor = System.Drawing.Color.Red;
            this.txtError.Location = new System.Drawing.Point(875, 38);
            this.txtError.Name = "txtError";
            this.txtError.Size = new System.Drawing.Size(0, 13);
            this.txtError.TabIndex = 22;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1379, 905);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.txtMuta);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtMutx);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSeltourn);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtNewPop);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSelrand);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtSelbest);
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSelrand;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSelbest;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSeltourn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtNewPop;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtMuta;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtMutx;
        private System.Windows.Forms.Label txtError;
    }
}

