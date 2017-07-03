using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;
using NetSteps.ShippingDataImporter.Helpers;
using NetSteps.ShippingDataImporter.Models;

namespace NetSteps.ShippingDataImporter.Services
{
    public class ShippingService
    {
        private Application ObjExcel = new Application();
        private int _fileSequence;
        private int _currentStep;
        private int _totalSteps;
        private ServiceRunnerModel _model;
        private string filePrefix = "ShippingData";

        public event EventHandler<ProgressUpdatedEventArgs> UpdateProgress;

        public void CreateSQL(string filePath, string generatedFilePath)
        {
            var model = new ServiceRunnerModel { ImportFilePath = filePath, OutputFolderPath = generatedFilePath };
            CreateSQL(model);
        }
        
        public void CreateSQL(ServiceRunnerModel model)
        {
            _model = model;
            _fileSequence = 0;
            _currentStep = 0;

			StaticCrap.StateRegions = new List<StateRegion.ObjectStruct>();
			StaticCrap.OrderTypes = new List<OrderType.ObjectStruct>();

            var ObjWorkBook = GetWorkbook(model.ImportFilePath);
            var worksheets = GetWorkSheets(ObjWorkBook);
            var ws = worksheets.FirstOrDefault(x => x.Name.StartsWith("6"));

            // 1. clear tables
            // 2. create warehouse
            // 3. create order types
            // 4. create shipping methods
            // 5. create shipping regions
            // 6. create state/provinces
            // 7. create shipping order types/shipping rate groups
            var staticSteps = 7;
            var rateRows = ws.UsedRange.Rows.Count - 3; // subtract the header rows
            var rateSteps = Math.Ceiling((double)rateRows / _model.RowsPerFile);
            _totalSteps = staticSteps + (int)rateSteps;
            
            try
            {
                GenerateSQL(ObjWorkBook);
            }
            finally
            {
                Exit(ObjWorkBook);
            }
        }

        private void GenerateSQL(Workbook ObjWorkBook)
        {
            var orderTypes = new List<OrderType.ObjectStruct>();
            var worksheets = GetWorkSheets(ObjWorkBook);

            OnUpdateProgress("Starting Master Data generation...", false);
            GenerateMasterData(worksheets);

            OnUpdateProgress("Starting Shipping Order Types generation...", false);
            GenerateShippingOrderTypeRateGroupsSQL(worksheets);

            OnUpdateProgress("Starting Shipping Rates generation...", false);
            BuildShippingRatesSQL(worksheets);

            OnUpdateProgress("Process complete!", false);
        }

        private void OnUpdateProgress(string message, bool increment)
        {
            if (UpdateProgress != null)
            {
                UpdateProgress(this, new ProgressUpdatedEventArgs { CurrentStep = _currentStep, TotalSteps = _totalSteps, Message = message });
                
                if (increment)
                {
                    _currentStep++;
                }
            }
        }

        #region SQLBuilderMethods

        private void GenerateMasterData(List<Worksheet> worksheets)
        {
            var masterSQL = new StringBuilder();
            masterSQL.AppendLine(GenerateTableCleanup());
            this.OnUpdateProgress("Table cleanup script created.", true);

            masterSQL.AppendLine(GenerateWarehouseSQL(worksheets));
            this.OnUpdateProgress("Warehouse script created.", true);

            masterSQL.AppendLine(GenerateOrderTypeSQL(worksheets));
            this.OnUpdateProgress("Order Types script created.", true);

            masterSQL.AppendLine(GenerateShippingMethodSQL(worksheets));
            this.OnUpdateProgress("Shipping Methods script created.", true);

            masterSQL.AppendLine(GenerateShippingRegionSQL(worksheets));
            this.OnUpdateProgress("Shipping Regions script created.", true);

            masterSQL.AppendLine(GenerateStateRegionSQL(worksheets));
            this.OnUpdateProgress("State/Province script created.", true);

            GenerateFileAndSave(filePrefix, masterSQL.ToString(), "MainData", _fileSequence);
            _fileSequence++;
            this.OnUpdateProgress("\nMaster Data file generated!", false);
        }

        public void BuildShippingRatesSQL(List<Worksheet> worksheets)
        {
            var ws = worksheets.FirstOrDefault(x => x.Name.StartsWith("6"));
            var rate = new Rate(ws);
            var sqlStringBuilder = new StringBuilder();
            sqlStringBuilder.AppendLine("-- Start ShippingService Rates");

            var shippingRates = rate.GetShippingRates();
            var recordCount = 0;
            var totalCount = 0;

            foreach (var item in shippingRates)
            {
                recordCount++;
                totalCount++;

                sqlStringBuilder.AppendLine(string.Empty);
                sqlStringBuilder.AppendLine(string.Format("-- ShippingService Rate - Record Number {0}", totalCount));

                var template = Base.GetTemplate("ShippingRateTemplate");

                Base.ReplaceToken(ref template, "WorksheetID", rate.WorksheetIdentifier);
                Base.ReplaceToken(ref template, "Count", totalCount.ToString());
                Base.ReplaceToken(ref template, "ShippingRateGroupName", item.ShippingRateGroupName);
                Base.ReplaceToken(ref template, "CurrencyName", item.CurrencyName);
                Base.ReplaceToken(ref template, "ValueName", item.ValueName);
                Base.ReplaceToken(ref template, "ValueFrom", item.ValueFrom);
                Base.ReplaceToken(ref template, "ValueTo", item.ValueTo);
                Base.ReplaceToken(ref template, "ShippingAmount", item.ShippingAmount);
                Base.ReplaceToken(ref template, "DirectShipmentFee", item.DirectShipmentFee);
                Base.ReplaceToken(ref template, "HandlingFee", item.HandlingFee);
                Base.ReplaceToken(ref template, "IncrementalAmount", item.IncrementalAmount);
                Base.ReplaceToken(ref template, "IncrementalFee", item.IncrementalFee);
                Base.ReplaceToken(ref template, "ShippingPercentage", item.ShippingPercentage);
                Base.ReplaceToken(ref template, "ShippingRateTypeID", item.ShippingRateTypeID);
                Base.ReplaceToken(ref template, "MinimumAmount", item.MinimumAmount);

                sqlStringBuilder.AppendLine(template);
                sqlStringBuilder.AppendLine(string.Empty);

                if (recordCount >= _model.RowsPerFile || recordCount >= shippingRates.Count)
                {
                    WriteShippingRateFile(sqlStringBuilder.ToString());
                    recordCount = 0;
                    sqlStringBuilder = new StringBuilder();
                }
            }

            if (recordCount > 0)
            {
                this.WriteShippingRateFile(sqlStringBuilder.ToString());
            }
        }

        private string GenerateTableCleanup()
        {
            var sqlStringBuilder = new StringBuilder();

            sqlStringBuilder.AppendLine(string.Empty);
            sqlStringBuilder.AppendLine("-- Delete ShippingOrderTypes Table");
            
            sqlStringBuilder.AppendLine(string.Empty);
            sqlStringBuilder.AppendLine("PRINT 'Delete ShippingOrderTypes'");
            sqlStringBuilder.AppendLine("DELETE FROM ShippingOrderTypes");

            sqlStringBuilder.AppendLine(string.Empty);
            sqlStringBuilder.AppendLine("-- Delete ShippingRates Table");
            
            sqlStringBuilder.AppendLine(string.Empty);
            sqlStringBuilder.AppendLine("PRINT 'Delete ShippingRates'");
            sqlStringBuilder.AppendLine("DELETE FROM ShippingRates");

            sqlStringBuilder.AppendLine(string.Empty);
            sqlStringBuilder.AppendLine("-- Delete ShippingRateGroups Table");
            
            sqlStringBuilder.AppendLine(string.Empty);
            sqlStringBuilder.AppendLine("PRINT 'Delete ShippingRateGroups'");
            sqlStringBuilder.AppendLine("DELETE FROM ShippingRateGroups");

            sqlStringBuilder.AppendLine(string.Empty);
            sqlStringBuilder.AppendLine("PRINT 'Delete ShippingRegions'");
            sqlStringBuilder.AppendLine("UPDATE StateProvinces SET ShippingRegionID = NULL");
            sqlStringBuilder.AppendLine("DELETE FROM ShippingRegions");

            return sqlStringBuilder.ToString();
        }

        private string GenerateWarehouseSQL(List<Worksheet> worksheets)
        {
            StringBuilder sb = new StringBuilder();

            Worksheet ws = worksheets.FirstOrDefault(x => x.Name.StartsWith("1"));

            if (ws != null)
            {
                Warehouse shipping = new Warehouse(ws);

                sb = shipping.GenerateSQL();
            }
            else
            {
                sb.AppendLine("--Error sheet 1 not found.");
            }

            return sb.ToString();
        }

        private string GenerateOrderTypeSQL(List<Worksheet> worksheets)
        {
            StringBuilder sb = new StringBuilder();

            Worksheet ws = worksheets.FirstOrDefault(x => x.Name.StartsWith("2"));

            if (ws != null)
            {
                OrderType shipping = new OrderType(ws);

                sb = shipping.GenerateSQL();
            }
            else
            {
                sb.AppendLine("--Error sheet 2 not found.");
            }

            return sb.ToString();
        }

        private string GenerateShippingMethodSQL(List<Worksheet> worksheets)
        {
            StringBuilder sb = new StringBuilder();

            Worksheet ws = worksheets.FirstOrDefault(x => x.Name.StartsWith("3"));

            if (ws != null)
            {
                ShippingMethod shipping = new ShippingMethod(ws);
                sb = shipping.GenerateSQL();
            }
            else
            {
                sb.AppendLine("--Error sheet 3 not found.");
            }

            return sb.ToString();
        }

        private string GenerateShippingRegionSQL(List<Worksheet> worksheets)
        {
            var sb = new StringBuilder();

            var ws = worksheets.FirstOrDefault(x => x.Name.StartsWith("4"));

            if (ws != null)
            {
                var shipping = new ShippingRegion(ws);

                sb = shipping.GenerateSQL();
            }
            else
            {
                sb.AppendLine("--Error sheet 4 not found.");
            }

            return sb.ToString();
        }

        private string GenerateStateRegionSQL(List<Worksheet> worksheets)
        {
            var sb = new StringBuilder();
            var ws = worksheets.FirstOrDefault(x => x.Name.StartsWith("5"));
            if (ws != null)
            {
                var shipping = new StateRegion(ws);
                sb = shipping.GenerateSQL();
			}
            else
            {
                sb.AppendLine("--Error sheet 5 not found.");
            }

            return sb.ToString();
        }

        private void GenerateShippingOrderTypeRateGroupsSQL(List<Worksheet> worksheets)
        {
            var count = 0;
            var ws = worksheets.FirstOrDefault(x => x.Name.StartsWith("6"));
            var shipping = new Rate(ws);
            var rateSupportingSql = new StringBuilder();

            rateSupportingSql.AppendLine(shipping.BuildShippingRateGroupsSQL(ref count));

            this.OnUpdateProgress(string.Format("Generating Shipping Order Types and Rate Groups file {0}...", _fileSequence), false);
            rateSupportingSql.AppendLine(shipping.BuildShippingOrderTypesSQL(ref count));
            this.OnUpdateProgress(string.Format("Shipping Order Types and Rate Groups File {0} created!", _fileSequence), true);

            GenerateFileAndSave(filePrefix, rateSupportingSql.ToString(), "ShippingOrderTypesRateGroups", _fileSequence);
            _fileSequence++;
        }

       #endregion

        #region FileIO

        private void GenerateFileAndSave(string fileNamePrefix, string SQLString, string fileTypeSuffix, int sequence)
        {
            var stringToWrite = SQLString; // _masterSQL.ToString();
            const string sequenceFormat = "000";

            var transmorgrifiedFilePath = string.Format(
                "{0}_{1}_{2} - {3}{4}{5} {6}_{7}_{8}.sql",
                fileNamePrefix,
                sequence.ToString(sequenceFormat),
                fileTypeSuffix,
                DateTime.Now.Year,
                DateTime.Now.Month.ToString().PadLeft(2, '0'),
                DateTime.Now.Day.ToString().PadLeft(2, '0'),
                DateTime.Now.Hour.ToString().PadLeft(2, '0'),
                DateTime.Now.Minute.ToString().PadLeft(2, '0'),
                DateTime.Now.Second.ToString().PadLeft(2, '0'));

            var fs = File.Create(Path.Combine(_model.OutputFolderPath, transmorgrifiedFilePath));
            var sw = new StreamWriter(fs, Encoding.UTF8);

            sw.Write(stringToWrite);
            sw.Close();
        }

        private void WriteShippingRateFile(string outputString)
        {
            this.OnUpdateProgress(string.Format("Generating Shipping Rates File {0}...", _fileSequence), false);
            GenerateFileAndSave(filePrefix, outputString, "ShippingRates", _fileSequence);
            this.OnUpdateProgress(string.Format("Shipping Rates File {0} created!", _fileSequence), true);
            _fileSequence++;
        }

        #endregion

        #region ExcelSpreadsheetHandlers

        public static List<Worksheet> GetWorkSheets(Workbook ObjWorkBook)
        {
            List<Worksheet> result = new List<Worksheet>();

            foreach (var item in ObjWorkBook.Worksheets)
            {
                result.Add((Worksheet)item);
            }

            result = result.OrderBy(w => w.Name).ToList();

            return result;
        }

        public void Exit(Workbook ObjWorkBook)
        {
            ObjWorkBook.Close(XlSaveAction.xlSaveChanges, Type.Missing, Type.Missing);
            ObjExcel.Quit();
        }

        public Workbook GetWorkbook(string fileName)
        {
            return ObjExcel.Workbooks.Open(fileName, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        #endregion
    }
}