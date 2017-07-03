using System;
using System.ComponentModel;
using System.Windows.Forms;

using NetSteps.ShippingDataImporter.Models;
using NetSteps.ShippingDataImporter.Services;

namespace NetSteps.ShippingDataImporter.UI
{


    public partial class Interface : Form
    {
        public ShippingService service;

        public Interface()
        {
            InitializeComponent();
            service = new ShippingService();
            service.UpdateProgress += this.UpdateUICrossThread;

            var ioService = new IOService();
            var model = ioService.LoadInterfaceSettings();

            this.PopulateControlsFromModel(model);
        }

		private void updateProgressBar(int currentFile, int totalFiles)
		{
			if (currentFile > totalFiles)
			{
				currentFile = totalFiles;
			}

			pgbProgress.Value = currentFile;
			pgbProgress.Maximum = totalFiles;
		}

		private void updateMessageBox(string message)
		{
			txtStatus.AppendText(message + "\n");
		}

        private void UpdateUICrossThread(object sender, ProgressUpdatedEventArgs args)
        {
            if (pgbProgress.InvokeRequired)
            {
                Invoke(new MethodInvoker(() => updateProgressBar(args.CurrentStep, args.TotalSteps)));
            }
            else
            {
                updateProgressBar(args.CurrentStep, args.TotalSteps);
            }

            if (lbStatus.InvokeRequired)
            {
                Invoke(new MethodInvoker(() => updateMessageBox(args.Message)));
            }
            else
            {
                updateMessageBox(args.Message);
            }
        }

    	private void btnBrowseFile_Click(object sender, EventArgs e)
        {
            var diag = new OpenFileDialog { Filter = "Excel Spreadsheet | *.xlsx" };
            var result = diag.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtFilePath.Text = diag.FileName;
            }
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            bgwExecute.WorkerReportsProgress = true;
            bgwExecute.RunWorkerAsync();
        }

        private void btnOutFolder_Click(object sender, EventArgs e)
        {
            var diag = new FolderBrowserDialog { SelectedPath =  txtOutputPath.Text };
            var result = diag.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtOutputPath.Text = diag.SelectedPath;
            }
        }

        private void bgwExecute_DoWork(object sender, DoWorkEventArgs e)
        {
            var model = this.LoadInterfaceModelFromControls();
            service.CreateSQL(model);
        }

        private void bgwExecute_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnExecute.Enabled = true;
            txtStatus.AppendText("\n\n");
        }

        private void Interface_FormClosing(object sender, FormClosingEventArgs e)
        {
            var ioService = new IOService();
            var model = LoadInterfaceModelFromControls();
            ioService.WriteInterfaceSettings(model);
        }

        private ServiceRunnerModel LoadInterfaceModelFromControls()
        {
            var model = new ServiceRunnerModel
            {
                ImportFilePath = txtFilePath.Text,
                OutputFolderPath = txtOutputPath.Text,
                RowsPerFile = Convert.ToInt32(txtRowsPerFile.Text)
            };

            return model;
        }

        private void PopulateControlsFromModel(ServiceRunnerModel model)
        {
            if (model != null)
            {
                txtFilePath.Text = model.ImportFilePath;
                txtOutputPath.Text = model.OutputFolderPath;
                txtRowsPerFile.Text = model.RowsPerFile.ToString();
            }
            else
            {
                var ioService = new IOService();
                txtOutputPath.Text = ioService.GetOutputDirectory();
            }
        }
    }
}
