namespace FinalDiacritiser
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
            this.inputTextBox = new System.Windows.Forms.RichTextBox();
            this.intitialPOSBtn = new System.Windows.Forms.Button();
            this.POSNgramBtn = new System.Windows.Forms.Button();
            this.InitialDiacriticsBtn = new System.Windows.Forms.Button();
            this.diacriticsNgramBtn = new System.Windows.Forms.Button();
            this.outputTextBox = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.progressLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.POSNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.DiacNumericUpDown = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.POSNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiacNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // inputTextBox
            // 
            this.inputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.inputTextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.inputTextBox.Location = new System.Drawing.Point(3, 3);
            this.inputTextBox.Name = "inputTextBox";
            this.inputTextBox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.inputTextBox.Size = new System.Drawing.Size(444, 169);
            this.inputTextBox.TabIndex = 0;
            this.inputTextBox.Text = "كتب الطفل الوظيفة";
            // 
            // intitialPOSBtn
            // 
            this.intitialPOSBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.intitialPOSBtn.Location = new System.Drawing.Point(468, 12);
            this.intitialPOSBtn.Name = "intitialPOSBtn";
            this.intitialPOSBtn.Size = new System.Drawing.Size(165, 23);
            this.intitialPOSBtn.TabIndex = 1;
            this.intitialPOSBtn.Text = "Initial POS";
            this.intitialPOSBtn.UseVisualStyleBackColor = true;
            this.intitialPOSBtn.Click += new System.EventHandler(this.intitialPOSBtn_Click);
            // 
            // POSNgramBtn
            // 
            this.POSNgramBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.POSNgramBtn.Enabled = false;
            this.POSNgramBtn.Location = new System.Drawing.Point(468, 41);
            this.POSNgramBtn.Name = "POSNgramBtn";
            this.POSNgramBtn.Size = new System.Drawing.Size(103, 23);
            this.POSNgramBtn.TabIndex = 2;
            this.POSNgramBtn.Text = "POS N-gram";
            this.POSNgramBtn.UseVisualStyleBackColor = true;
            this.POSNgramBtn.Click += new System.EventHandler(this.POSNgramBtn_Click);
            // 
            // InitialDiacriticsBtn
            // 
            this.InitialDiacriticsBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.InitialDiacriticsBtn.Enabled = false;
            this.InitialDiacriticsBtn.Location = new System.Drawing.Point(468, 70);
            this.InitialDiacriticsBtn.Name = "InitialDiacriticsBtn";
            this.InitialDiacriticsBtn.Size = new System.Drawing.Size(165, 23);
            this.InitialDiacriticsBtn.TabIndex = 3;
            this.InitialDiacriticsBtn.Text = "Initial Diacritics";
            this.InitialDiacriticsBtn.UseVisualStyleBackColor = true;
            this.InitialDiacriticsBtn.Click += new System.EventHandler(this.InitialDiacriticsBtn_Click);
            // 
            // diacriticsNgramBtn
            // 
            this.diacriticsNgramBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.diacriticsNgramBtn.Enabled = false;
            this.diacriticsNgramBtn.Location = new System.Drawing.Point(468, 99);
            this.diacriticsNgramBtn.Name = "diacriticsNgramBtn";
            this.diacriticsNgramBtn.Size = new System.Drawing.Size(103, 23);
            this.diacriticsNgramBtn.TabIndex = 4;
            this.diacriticsNgramBtn.Text = "Diacritics N-gram";
            this.diacriticsNgramBtn.UseVisualStyleBackColor = true;
            this.diacriticsNgramBtn.Click += new System.EventHandler(this.diacriticsNgramBtn_Click);
            // 
            // outputTextBox
            // 
            this.outputTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.outputTextBox.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.outputTextBox.Location = new System.Drawing.Point(5, 3);
            this.outputTextBox.Name = "outputTextBox";
            this.outputTextBox.ReadOnly = true;
            this.outputTextBox.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.outputTextBox.Size = new System.Drawing.Size(442, 109);
            this.outputTextBox.TabIndex = 5;
            this.outputTextBox.Text = "";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 12);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.inputTextBox);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.outputTextBox);
            this.splitContainer1.Size = new System.Drawing.Size(450, 294);
            this.splitContainer1.SplitterDistance = 175;
            this.splitContainer1.TabIndex = 6;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(468, 283);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(165, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // progressLabel
            // 
            this.progressLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.progressLabel.AutoSize = true;
            this.progressLabel.Location = new System.Drawing.Point(468, 267);
            this.progressLabel.Name = "progressLabel";
            this.progressLabel.Size = new System.Drawing.Size(108, 13);
            this.progressLabel.TabIndex = 8;
            this.progressLabel.Text = "Waiting for Command";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(571, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Order";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(571, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(33, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Order";
            // 
            // POSNumericUpDown
            // 
            this.POSNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.POSNumericUpDown.Location = new System.Drawing.Point(603, 44);
            this.POSNumericUpDown.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.POSNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.POSNumericUpDown.Name = "POSNumericUpDown";
            this.POSNumericUpDown.Size = new System.Drawing.Size(30, 20);
            this.POSNumericUpDown.TabIndex = 11;
            this.POSNumericUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // DiacNumericUpDown
            // 
            this.DiacNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.DiacNumericUpDown.Location = new System.Drawing.Point(603, 102);
            this.DiacNumericUpDown.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.DiacNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.DiacNumericUpDown.Name = "DiacNumericUpDown";
            this.DiacNumericUpDown.Size = new System.Drawing.Size(30, 20);
            this.DiacNumericUpDown.TabIndex = 12;
            this.DiacNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(645, 318);
            this.Controls.Add(this.DiacNumericUpDown);
            this.Controls.Add(this.POSNumericUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.diacriticsNgramBtn);
            this.Controls.Add(this.InitialDiacriticsBtn);
            this.Controls.Add(this.POSNgramBtn);
            this.Controls.Add(this.intitialPOSBtn);
            this.MinimumSize = new System.Drawing.Size(489, 356);
            this.Name = "Form1";
            this.Text = "Automatic Diacritisation Demo";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.POSNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DiacNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox inputTextBox;
        private System.Windows.Forms.Button intitialPOSBtn;
        private System.Windows.Forms.Button POSNgramBtn;
        private System.Windows.Forms.Button InitialDiacriticsBtn;
        private System.Windows.Forms.Button diacriticsNgramBtn;
        private System.Windows.Forms.RichTextBox outputTextBox;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label progressLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown POSNumericUpDown;
        private System.Windows.Forms.NumericUpDown DiacNumericUpDown;
    }
}

