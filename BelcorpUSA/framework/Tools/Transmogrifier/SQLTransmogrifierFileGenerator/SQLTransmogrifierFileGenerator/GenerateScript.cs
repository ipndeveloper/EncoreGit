using System;
using System.Windows.Forms;
using SqlTransmogrifier;

namespace SQLTransmogrifierFileGenerator
{
    public partial class GenerateScript : Form
    {
        public string ScriptName { get; set; }

        public GenerateScript(String[] args)
        {
            InitializeComponent();
            SetDefaults(args);
        }

        private void SetDefaults(string[] args)
        {
            tbOutputDirectory.Text = Utilities.GetStringValueFromArguments(args, "-td");
            folderBrowserDialog1.SelectedPath = Utilities.GetStringValueFromArguments(args, "-td");
            ScriptName = Utilities.GetStringValueFromArguments(args, "-t");
            tbBranchCode.Text = Utilities.GetStringValueFromArguments(args, "-bc").ToUpper();
            tbScriptCode.Text = Utilities.GetNextCode(tbOutputDirectory.Text);

            this.AcceptButton = btnGenerateScript;

            if (string.IsNullOrEmpty(tbBranchCode.Text))
            {
                tbBranchCode.Text = "TRUNK";
            }

            GetFileName();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerateScript_Click(object sender, EventArgs e)
        {
            CreateScript(false);
        }

        private void btnGenerateScript_ClickClose(object sender, EventArgs e)
        {
            CreateScript(true);
        }

        private void CreateScript(bool closeOnSuccess)
        {
            string fileName;
            string outputDirectory = tbOutputDirectory.Text;
            string errorMessage = string.Empty;

            fileName = GetFileName();

            bool result = Transmogrfier.CreateNewScript(ScriptName, fileName, outputDirectory, ref errorMessage);

            if (result)
            {
                if (closeOnSuccess)
                {
                    this.Close();
                }
                else
                {
                    ResetForm();
                }
            }
            else
            {
                MessageBox.Show(string.Format("{0} - Was not Created. \r\nError:\r\n{1}", fileName, errorMessage));
            }
        }

        private void ResetForm()
        {
            GetNextCode();
            tbDescription.Text = string.Empty;
            tbDescription.Focus();

            GetFileName();

            this.BringToFront();
        }

        private string GetFileName()
        {
            string result = string.Empty;

            string scriptCode = tbScriptCode.Text;
            string description = tbDescription.Text;
            string databaseName = cbDatabaseName.Text;
            string branchCode = tbBranchCode.Text;

            result = Transmogrfier.GenerateFilename(scriptCode, description, databaseName, branchCode);

            lblGenerateFilename.Text = result;

            return result;
        }

        private void btnChooseOutputDirectory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                tbOutputDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btnNextCode_Click(object sender, EventArgs e)
        {
            GetNextCode();
            GetFileName();
        }

        private void GetNextCode()
        {
            tbScriptCode.Text = Utilities.GetNextCode(tbOutputDirectory.Text);
        }

        private void folderBrowserDialog1_HelpRequest(object sender, EventArgs e)
        {

        }

        private void GenerateScript_Load(object sender, EventArgs e)
        {

        }

        private void tbScriptCode_TextChanged(object sender, EventArgs e)
        {
            GetFileName();
        }

        private void tbDescription_TextChanged(object sender, EventArgs e)
        {
            GetFileName();
        }

        private void tbBranchCode_TextChanged(object sender, EventArgs e)
        {
            GetFileName();
        }

        private void tbOutputDirectory_TextChanged(object sender, EventArgs e)
        {
            GetFileName();
        }

        private void cbDatabaseName_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetFileName();
        }
    }
}
