using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using NetSteps.ShippingDataImporter.Helpers;

namespace NetSteps.ShippingDataImporter
{
    public class StateRegion : Base
    {
        public struct ObjectStruct
        {
            public string StateCode { get; set; }
            public string ShippingRegionName { get; set; }
            public string CountryName { get; set; }
        }

        List<ObjectStruct> _structureList = null;

        public StateRegion(Worksheet worksheet)
            : base(worksheet)
        {

        }

        public override void SetStructureList()
        {
            var startRange = GetRangeByName("StateShippingRegionStart");

            List<ObjectStruct> collection = new List<ObjectStruct>();

            int row = startRange.Row;
            int column = startRange.Column;
            int endRow = 1000;

            while (row <= endRow)
            {
                row++;

                ObjectStruct addItem = new ObjectStruct();

                Range range = CurrentWorksheet.Cells[row, column];

                if (range != null && !string.IsNullOrWhiteSpace(range.Text) && ((string)range.Text).ToLower() != "State / Province".ToLower())
                {
                    addItem.StateCode = range.Text;
                    addItem.ShippingRegionName = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 1]).Text;
                    addItem.CountryName = ((Range)CurrentWorksheet.Cells[range.Row, range.Column - 1]).Text;
                    collection.Add(addItem);
                }
            }

            StaticCrap.StateRegions = collection;
            _structureList = collection;
        }

        public override void BuildSQLStatement()
        {
            base.AppendCommentLine(SB);
            SB.AppendLine("-- Start State Regions");
            base.AppendCommentLine(SB);

            int count = 0;

            foreach (var item in _structureList)
            {
                count++;

                SB.AppendLine("");

                AppendCommentLine(SB);
                SB.AppendLine(string.Format("-- State Region - {0}", item.StateCode));
                AppendCommentLine(SB);

                string template = GetTemplate("StateRegionTemplate");

                ReplaceToken(ref template, "WorksheetID", WorksheetIdentifier);
                ReplaceToken(ref template, "Count", count.ToString());
                ReplaceToken(ref template, "StateCode", item.StateCode);
                ReplaceToken(ref template, "ShippingRegionName", item.ShippingRegionName);
                ReplaceToken(ref template, "CountryName", item.CountryName);

                SB.AppendLine(template);

                SB.AppendLine("");
            }

        }
    }
}
