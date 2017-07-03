namespace SQLTransmogrifierFileGenerator
{
    partial class GenerateScript
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
            this.lblFileName = new System.Windows.Forms.Label();
            this.lblDatabase = new System.Windows.Forms.Label();
            this.lblBranchCode = new System.Windows.Forms.Label();
            this.cbDatabaseName = new System.Windows.Forms.ComboBox();
            this.tbDescription = new System.Windows.Forms.TextBox();
            this.tbBranchCode = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGenerateScript = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnChooseOutputDirectory = new System.Windows.Forms.Button();
            this.tbOutputDirectory = new System.Windows.Forms.TextBox();
            this.lblOutputDirectory = new System.Windows.Forms.Label();
            this.lblScriptCode = new System.Windows.Forms.Label();
            this.tbScriptCode = new System.Windows.Forms.TextBox();
            this.btnNextCode = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.lblProposedFilename = new System.Windows.Forms.Label();
            this.lblGenerateFilename = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(16, 59);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(63, 13);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "Description:";
            // 
            // lblDatabase
            // 
            this.lblDatabase.AutoSize = true;
            this.lblDatabase.Location = new System.Drawing.Point(16, 85);
            this.lblDatabase.Name = "lblDatabase";
            this.lblDatabase.Size = new System.Drawing.Size(56, 13);
            this.lblDatabase.TabIndex = 1;
            this.lblDatabase.Text = "Database:";
            // 
            // lblBranchCode
            // 
            this.lblBranchCode.AutoSize = true;
            this.lblBranchCode.Location = new System.Drawing.Point(16, 112);
            this.lblBranchCode.Name = "lblBranchCode";
            this.lblBranchCode.Size = new System.Drawing.Size(72, 13);
            this.lblBranchCode.TabIndex = 2;
            this.lblBranchCode.Text = "Branch Code:";
            // 
            // cbDatabaseName
            // 
            this.cbDatabaseName.FormattingEnabled = true;
            this.cbDatabaseName.Items.AddRange(new object[] {
            "COMMISSIONS",
            "CORE",
            "MAIL"});
            this.cbDatabaseName.Location = new System.Drawing.Point(124, 82);
            this.cbDatabaseName.Name = "cbDatabaseName";
            this.cbDatabaseName.Size = new System.Drawing.Size(313, 21);
            this.cbDatabaseName.TabIndex = 1;
            this.cbDatabaseName.Text = "CORE";
            this.cbDatabaseName.SelectedIndexChanged += new System.EventHandler(this.cbDatabaseName_SelectedIndexChanged);
            // 
            // tbDescription
            // 
            this.tbDescription.Location = new System.Drawing.Point(124, 56);
            this.tbDescription.Name = "tbDescription";
            this.tbDescription.Size = new System.Drawing.Size(313, 20);
            this.tbDescription.TabIndex = 0;
            this.tbDescription.TextChanged += new System.EventHandler(this.tbDescription_TextChanged);
            // 
            // tbBranchCode
            // 
            this.tbBranchCode.Location = new System.Drawing.Point(124, 109);
            this.tbBranchCode.Name = "tbBranchCode";
            this.tbBranchCode.Size = new System.Drawing.Size(313, 20);
            this.tbBranchCode.TabIndex = 2;
            this.tbBranchCode.Text = "TRUNK";
            this.tbBranchCode.TextChanged += new System.EventHandler(this.tbBranchCode_TextChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(362, 164);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGenerateScript
            // 
            this.btnGenerateScript.Location = new System.Drawing.Point(187, 164);
            this.btnGenerateScript.Name = "btnGenerateScript";
            this.btnGenerateScript.Size = new System.Drawing.Size(169, 23);
            this.btnGenerateScript.TabIndex = 6;
            this.btnGenerateScript.Text = "Generate Script And Close";
            this.btnGenerateScript.UseVisualStyleBackColor = true;
            this.btnGenerateScript.Click += new System.EventHandler(this.btnGenerateScript_ClickClose);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.HelpRequest += new System.EventHandler(this.folderBrowserDialog1_HelpRequest);
            // 
            // btnChooseOutputDirectory
            // 
            this.btnChooseOutputDirectory.Location = new System.Drawing.Point(409, 135);
            this.btnChooseOutputDirectory.Name = "btnChooseOutputDirectory";
            this.btnChooseOutputDirectory.Size = new System.Drawing.Size(28, 23);
            this.btnChooseOutputDirectory.TabIndex = 4;
            this.btnChooseOutputDirectory.Text = "...";
            this.btnChooseOutputDirectory.UseVisualStyleBackColor = true;
            this.btnChooseOutputDirectory.Click += new System.EventHandler(this.btnChooseOutputDirectory_Click);
            // 
            // tbOutputDirectory
            // 
            this.tbOutputDirectory.Location = new System.Drawing.Point(124, 138);
            this.tbOutputDirectory.Name = "tbOutputDirectory";
            this.tbOutputDirectory.Size = new System.Drawing.Size(279, 20);
            this.tbOutputDirectory.TabIndex = 3;
            this.tbOutputDirectory.TextChanged += new System.EventHandler(this.tbOutputDirectory_TextChanged);
            // 
            // lblOutputDirectory
            // 
            this.lblOutputDirectory.AutoSize = true;
            this.lblOutputDirectory.Location = new System.Drawing.Point(16, 141);
            this.lblOutputDirectory.Name = "lblOutputDirectory";
            this.lblOutputDirectory.Size = new System.Drawing.Size(87, 13);
            this.lblOutputDirectory.TabIndex = 2;
            this.lblOutputDirectory.Text = "Output Directory:";
            // 
            // lblScriptCode
            // 
            this.lblScriptCode.AutoSize = true;
            this.lblScriptCode.Location = new System.Drawing.Point(16, 33);
            this.lblScriptCode.Name = "lblScriptCode";
            this.lblScriptCode.Size = new System.Drawing.Size(65, 13);
            this.lblScriptCode.TabIndex = 0;
            this.lblScriptCode.Text = "Script Code:";
            // 
            // tbScriptCode
            // 
            this.tbScriptCode.Location = new System.Drawing.Point(124, 30);
            this.tbScriptCode.Name = "tbScriptCode";
            this.tbScriptCode.Size = new System.Drawing.Size(232, 20);
            this.tbScriptCode.TabIndex = 8;
            this.tbScriptCode.TextChanged += new System.EventHandler(this.tbScriptCode_TextChanged);
            // 
            // btnNextCode
            // 
            this.btnNextCode.Location = new System.Drawing.Point(362, 28);
            this.btnNextCode.Name = "btnNextCode";
            this.btnNextCode.Size = new System.Drawing.Size(75, 23);
            this.btnNextCode.TabIndex = 9;
            this.btnNextCode.Text = "Next Code";
            this.btnNextCode.UseVisualStyleBackColor = true;
            this.btnNextCode.Click += new System.EventHandler(this.btnNextCode_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 164);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(169, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Generate Script";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnGenerateScript_Click);
            // 
            // lblProposedFilename
            // 
            this.lblProposedFilename.AutoSize = true;
            this.lblProposedFilename.Location = new System.Drawing.Point(16, 9);
            this.lblProposedFilename.Name = "lblProposedFilename";
            this.lblProposedFilename.Size = new System.Drawing.Size(105, 13);
            this.lblProposedFilename.TabIndex = 0;
            this.lblProposedFilename.Text = "Proposed File Name:";
            // 
            // lblGenerateFilename
            // 
            this.lblGenerateFilename.AutoSize = true;
            this.lblGenerateFilename.Location = new System.Drawing.Point(121, 9);
            this.lblGenerateFilename.Name = "lblGenerateFilename";
            this.lblGenerateFilename.Size = new System.Drawing.Size(22, 13);
            this.lblGenerateFilename.TabIndex = 0;
            this.lblGenerateFilename.Text = "NA";
            // 
            // GenerateScript
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 196);
            this.Controls.Add(this.btnChooseOutputDirectory);
            this.Controls.Add(this.btnNextCode);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnGenerateScript);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.tbBranchCode);
            this.Controls.Add(this.tbOutputDirectory);
            this.Controls.Add(this.tbScriptCode);
            this.Controls.Add(this.tbDescription);
            this.Controls.Add(this.cbDatabaseName);
            this.Controls.Add(this.lblOutputDirectory);
            this.Controls.Add(this.lblBranchCode);
            this.Controls.Add(this.lblGenerateFilename);
            this.Controls.Add(this.lblProposedFilename);
            this.Controls.Add(this.lblScriptCode);
            this.Controls.Add(this.lblDatabase);
            this.Controls.Add(this.lblFileName);
            this.MaximumSize = new System.Drawing.Size(459, 234);
            this.MinimumSize = new System.Drawing.Size(459, 234);
            this.Name = "GenerateScript";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SQL Transmogrifier Script Generator";
            this.Load += new System.EventHandler(this.GenerateScript_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblDatabase;
        private System.Windows.Forms.Label lblBranchCode;
        private System.Windows.Forms.ComboBox cbDatabaseName;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbBranchCode;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnGenerateScript;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnChooseOutputDirectory;
        private System.Windows.Forms.TextBox tbOutputDirectory;
        private System.Windows.Forms.Label lblOutputDirectory;
        private System.Windows.Forms.Label lblScriptCode;
        private System.Windows.Forms.TextBox tbScriptCode;
        private System.Windows.Forms.Button btnNextCode;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblProposedFilename;
        private System.Windows.Forms.Label lblGenerateFilename;
    }
}

