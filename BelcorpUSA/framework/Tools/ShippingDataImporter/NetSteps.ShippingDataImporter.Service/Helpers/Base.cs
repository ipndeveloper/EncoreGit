namespace NetSteps.ShippingDataImporter.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.Office.Interop.Excel;
    using SqlTransmogrifier.Common;

    public abstract class Base
    {
        public Worksheet CurrentWorksheet { get; set; }
        public StringBuilder SB { get; set; }
        public string WorksheetIdentifier { get; set; }

        public Base() { }

        public Base(Worksheet worksheet)
        {
            this.CurrentWorksheet = worksheet;
            this.WorksheetIdentifier = worksheet.Name.Substring(0, 1);

            this.SB = new StringBuilder();
        }

        public StringBuilder GenerateSQL()
        {
            this.Execute();

            return this.SB;
        }

        public void Execute()
        {
            this.SetStructureList();
            this.BuildSQLStatement();
        }

        public abstract void SetStructureList();

        public abstract void BuildSQLStatement();

        public static string GetTemplate(string templateName)
        {
            return Utilities.GetTemplate("NetSteps.ShippingDataImporter", "NetSteps.ShippingDataImporter.Templates", templateName, false);
        }

        public void AppendCommentLine(StringBuilder SB)
        {
            SB.AppendLine("--************************************************************");
        }

        public Range GetRangeByName(string regionName)
        {
            return (Range)this.CurrentWorksheet.Range[regionName];
        }

        public static void ReplaceToken(ref string template, string placeholder, string value)
        {
            template = template.Replace("{" + placeholder.ToUpper() + "}", value);
        }
    }
}
