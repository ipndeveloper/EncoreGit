using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using NetSteps.ShippingDataImporter.Helpers;

namespace NetSteps.ShippingDataImporter
{
    public class ShippingMethod : Base
    {
        public struct ObjectStruct
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string ShortName { get; set; }
            public string Priority { get; set; }
            public string AcceptPOBoxes { get; set; }
            public string IsWillCall { get; set; }
            public string TrackingURL { get; set; }
        }

        List<ObjectStruct> _structureList = null;

        public ShippingMethod(Worksheet worksheet)
            : base(worksheet)
        {

        }

        public override void SetStructureList()
        {
            var ranges = GetRangeByName("ShippingMethods");

            List<ObjectStruct> collection = new List<ObjectStruct>();

            foreach (var item in ranges)
            {
                ObjectStruct addItem = new ObjectStruct();

                Range range = (Range)item;

                if (range != null && !string.IsNullOrWhiteSpace(range.Text))
                {
                    addItem.Name = range.Value2;
                    addItem.Description = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 1]).Text;
                    addItem.ShortName = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 2]).Text;
                    addItem.Priority = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 3]).Text;
                    addItem.AcceptPOBoxes = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 4]).Text;
                    addItem.IsWillCall = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 5]).Text;
                    addItem.TrackingURL = ((Range)CurrentWorksheet.Cells[range.Row, range.Column + 6]).Text;
                    collection.Add(addItem);
                }
            }


            _structureList = collection;
        }

        public override void BuildSQLStatement()
        {
            base.AppendCommentLine(SB);
            SB.AppendLine("-- Start ShippingService Methods");
            base.AppendCommentLine(SB);

            int count = 0;

            foreach (var item in _structureList)
            {
                count++;

                SB.AppendLine("");

                AppendCommentLine(SB);
                SB.AppendLine(string.Format("-- ShippingService Method - {0}", item.Name));
                AppendCommentLine(SB);

                string template = GetTemplate("ShippingMethodTemplate");

                ReplaceToken(ref template, "WorksheetID", WorksheetIdentifier);
                ReplaceToken(ref template, "Count", count.ToString());
                ReplaceToken(ref template, "Name", item.Name);
                ReplaceToken(ref template, "Description", item.Description);
                ReplaceToken(ref template, "ShortName", item.ShortName);
                ReplaceToken(ref template, "Priority", string.IsNullOrWhiteSpace(item.Priority) ? "0" : item.Priority);
                ReplaceToken(ref template, "AcceptPOBoxes", item.AcceptPOBoxes.ToLower() == "Yes" ? "1" : "0");
                ReplaceToken(ref template, "IsWillCall", item.IsWillCall.ToLower() == "Yes" ? "1" : "0");
                ReplaceToken(ref template, "TrackingURL", item.TrackingURL);

                SB.AppendLine(template);

                SB.AppendLine("");
            }
        }
    }
}