using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using NetSteps.ShippingDataImporter.Helpers;

namespace NetSteps.ShippingDataImporter
{
    public class ShippingRegion : Base
    {
        public struct ObjectStruct
        {
            public string Name { get; set; }
            public string WarehouseName { get; set; }
        }

        List<ObjectStruct> _structureList = null;

        public ShippingRegion(Worksheet worksheet)
            : base(worksheet)
        {

        }

        public override void SetStructureList()
        {
            var ranges = GetRangeByName("ShippingRegions");

            List<ObjectStruct> collection = new List<ObjectStruct>();

            foreach (var item in ranges)
            {
                ObjectStruct addItem = new ObjectStruct();

                Range range = (Range)item;

                if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                {
                    addItem.Name = range.Text;
                    addItem.WarehouseName = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 1]).Text;
                    collection.Add(addItem);
                }
            }

            _structureList = collection;
        }

        public override void BuildSQLStatement()
        {
            base.AppendCommentLine(SB);
            SB.AppendLine("-- Start ShippingService Regions");
            base.AppendCommentLine(SB);

            int count = 0;

            foreach (var item in _structureList)
            {
                count++;

                SB.AppendLine("");

                AppendCommentLine(SB);
                SB.AppendLine(string.Format("-- ShippingService Region - {0}", item.Name));
                AppendCommentLine(SB);

                string template = GetTemplate("ShippingRegionTemplate");

                ReplaceToken(ref template, "WorksheetID", WorksheetIdentifier);
                ReplaceToken(ref template, "Count", count.ToString());
                ReplaceToken(ref template, "Name", item.Name);
                ReplaceToken(ref template, "WarehouseName", item.WarehouseName);
                SB.AppendLine(template);

                SB.AppendLine("");
            }
        }
    }
}
