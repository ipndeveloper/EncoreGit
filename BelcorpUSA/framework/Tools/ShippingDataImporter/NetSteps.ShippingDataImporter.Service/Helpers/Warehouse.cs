using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using NetSteps.ShippingDataImporter.Helpers;

namespace NetSteps.ShippingDataImporter
{
    public class Warehouse : Base
    {
        public struct ObjectStruct
        {
            public string Name { get; set; }
            public string Address1 { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            public string Country { get; set; }
            public string PostalCode { get; set; }
        }

        List<ObjectStruct> _structureList = null;

        public Warehouse(Worksheet worksheet)
            : base(worksheet)
        {

        }

        public override void SetStructureList()
        {
            var ranges = GetRangeByName("Warehouses");

            List<ObjectStruct> collection = new List<ObjectStruct>();

            foreach (var item in ranges)
            {
                ObjectStruct addItem = new ObjectStruct();

                Range range = (Range)item;

                if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                {
                    addItem.Name = range.Text;
                    addItem.Address1 = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 1]).Text;
                    addItem.City = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 2]).Text;
                    addItem.Country = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 3]).Text;
                    addItem.State = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 4]).Text;
                    addItem.PostalCode = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 5]).Text;
                    collection.Add(addItem);
                }
            }

            _structureList = collection;
        }

        public override void BuildSQLStatement()
        {
            AppendCommentLine(SB);
            SB.AppendLine("-- Start Warehouses");
            AppendCommentLine(SB);

            int count = 0;

            foreach (var item in _structureList)
            {
                count++;

                SB.AppendLine("");

                AppendCommentLine(SB);
                SB.AppendLine(string.Format("-- Warehouses - {0}", item.Name));
                AppendCommentLine(SB);

                string template = GetTemplate("WarehouseTemplate");

                ReplaceToken(ref template, "WorksheetID", WorksheetIdentifier);
                ReplaceToken(ref template, "Count", count.ToString());
                ReplaceToken(ref template, "Name", item.Name);
                ReplaceToken(ref template, "Address1", item.Address1);
                ReplaceToken(ref template, "City", item.City);
                ReplaceToken(ref template, "Country", item.Country);
                ReplaceToken(ref template, "State", item.State);
                ReplaceToken(ref template, "PostalCode", item.PostalCode);

                SB.AppendLine(template);

                SB.AppendLine("");
            }
        }
    }
}
