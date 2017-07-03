namespace NetSteps.ShippingDataImporter.UI
{
    partial class Interface
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
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.txtFilePath = new System.Windows.Forms.TextBox();
			this.btnBrowseFile = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.txtRowsPerFile = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnExecute = new System.Windows.Forms.Button();
			this.btnOutFolder = new System.Windows.Forms.Button();
			this.txtOutputPath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.bgwExecute = new System.ComponentModel.BackgroundWorker();
			this.pgbProgress = new System.Windows.Forms.ProgressBar();
			this.lbStatus = new System.Windows.Forms.Label();
			this.txtStatus = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::NetSteps.ShippingDataImporter.UI.Properties.Resources.NetStepsstandard;
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(214, 50);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// txtFilePath
			// 
			this.txtFilePath.BackColor = System.Drawing.SystemColors.Control;
			this.txtFilePath.Location = new System.Drawing.Point(12, 94);
			this.txtFilePath.Name = "txtFilePath";
			this.txtFilePath.Size = new System.Drawing.Size(507, 20);
			this.txtFilePath.TabIndex = 1;
			this.txtFilePath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// btnBrowseFile
			// 
			this.btnBrowseFile.Location = new System.Drawing.Point(525, 91);
			this.btnBrowseFile.Name = "btnBrowseFile";
			this.btnBrowseFile.Size = new System.Drawing.Size(48, 23);
			this.btnBrowseFile.TabIndex = 2;
			this.btnBrowseFile.Text = "...";
			this.btnBrowseFile.UseVisualStyleBackColor = true;
			this.btnBrowseFile.Click += new System.EventHandler(this.btnBrowseFile_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 75);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(156, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "ShippingService Data Input File";
			// 
			// txtRowsPerFile
			// 
			this.txtRowsPerFile.Location = new System.Drawing.Point(12, 212);
			this.txtRowsPerFile.Name = "txtRowsPerFile";
			this.txtRowsPerFile.Size = new System.Drawing.Size(68, 20);
			this.txtRowsPerFile.TabIndex = 4;
			this.txtRowsPerFile.Text = "500";
			this.txtRowsPerFile.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 193);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(71, 13);
			this.label2.TabIndex = 5;
			this.label2.Text = "Rows per File";
			// 
			// btnExecute
			// 
			this.btnExecute.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnExecute.Location = new System.Drawing.Point(449, 197);
			this.btnExecute.Name = "btnExecute";
			this.btnExecute.Size = new System.Drawing.Size(124, 47);
			this.btnExecute.TabIndex = 6;
			this.btnExecute.Text = "Execute";
			this.btnExecute.UseVisualStyleBackColor = true;
			this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
			// 
			// btnOutFolder
			// 
			this.btnOutFolder.Location = new System.Drawing.Point(525, 141);
			this.btnOutFolder.Name = "btnOutFolder";
			this.btnOutFolder.Size = new System.Drawing.Size(48, 23);
			this.btnOutFolder.TabIndex = 8;
			this.btnOutFolder.Text = "...";
			this.btnOutFolder.UseVisualStyleBackColor = true;
			this.btnOutFolder.Click += new System.EventHandler(this.btnOutFolder_Click);
			// 
			// txtOutputPath
			// 
			this.txtOutputPath.BackColor = System.Drawing.SystemColors.Control;
			this.txtOutputPath.Cursor = System.Windows.Forms.Cursors.IBeam;
			this.txtOutputPath.Enabled = false;
			this.txtOutputPath.ForeColor = System.Drawing.Color.Black;
			this.txtOutputPath.Location = new System.Drawing.Point(12, 144);
			this.txtOutputPath.Name = "txtOutputPath";
			this.txtOutputPath.Size = new System.Drawing.Size(507, 20);
			this.txtOutputPath.TabIndex = 7;
			this.txtOutputPath.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 128);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(95, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "SQL Output Folder";
			// 
			// bgwExecute
			// 
			this.bgwExecute.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwExecute_DoWork);
			this.bgwExecute.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwExecute_RunWorkerCompleted);
			// 
			// pgbProgress
			// 
			this.pgbProgress.Location = new System.Drawing.Point(12, 449);
			this.pgbProgress.Name = "pgbProgress";
			this.pgbProgress.Size = new System.Drawing.Size(561, 23);
			this.pgbProgress.TabIndex = 10;
			// 
			// lbStatus
			// 
			this.lbStatus.AutoSize = true;
			this.lbStatus.Location = new System.Drawing.Point(12, 449);
			this.lbStatus.Name = "lbStatus";
			this.lbStatus.Size = new System.Drawing.Size(0, 13);
			this.lbStatus.TabIndex = 11;
			// 
			// txtStatus
			// 
			this.txtStatus.Location = new System.Drawing.Point(12, 259);
			this.txtStatus.Multiline = true;
			this.txtStatus.Name = "txtStatus";
			this.txtStatus.ReadOnly = true;
			this.txtStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtStatus.Size = new System.Drawing.Size(559, 184);
			this.txtStatus.TabIndex = 12;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(311, 22);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(260, 29);
			this.label4.TabIndex = 13;
			this.label4.Text = "Shipping Data Importer";
			// 
			// Interface
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(593, 484);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.txtStatus);
			this.Controls.Add(this.lbStatus);
			this.Controls.Add(this.pgbProgress);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.btnOutFolder);
			this.Controls.Add(this.txtOutputPath);
			this.Controls.Add(this.btnExecute);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.txtRowsPerFile);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.btnBrowseFile);
			this.Controls.Add(this.txtFilePath);
			this.Controls.Add(this.pictureBox1);
			this.Name = "Interface";
			this.Text = "Encore Shipping Data Importer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Interface_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnBrowseFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRowsPerFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnOutFolder;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker bgwExecute;
        private System.Windows.Forms.ProgressBar pgbProgress;
		private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TextBox txtStatus;
        private System.Windows.Forms.Label label4;
    }
}

