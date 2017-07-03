namespace NetSteps.ShippingDataImporter.Models
{
    public class ServiceRunnerModel
    {
        public string ImportFilePath { get; set; } 
        public int RowsPerFile { get; set; }
        public string OutputFolderPath { get; set; }
    }
}