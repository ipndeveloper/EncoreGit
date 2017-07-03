using System.Collections.Generic;
using Microsoft.Office.Interop.Excel;
using NetSteps.ShippingDataImporter.Helpers;

namespace NetSteps.ShippingDataImporter
{
    public class OrderType : Base
    {
        public struct ObjectStruct
        {
            public string Name { get; set; }
        }

        public OrderType(Worksheet worksheet)
            : base(worksheet)
        {

        }

        public override void SetStructureList()
        {
            var ranges = GetRangeByName("OrderTypes");

            foreach (var item in ranges)
            {
                ObjectStruct addItem = new ObjectStruct();

                Range range = (Range)item;

                if (range != null && !string.IsNullOrWhiteSpace(range.Text) && ((string)range.Text).ToLower().Trim() != "all")
                {
                    addItem.Name = range.Text;
                    StaticCrap.OrderTypes.Add(addItem);
                }
            }
        }

        public override void BuildSQLStatement()
        {
            AppendCommentLine(SB);
            SB.AppendLine("-- Start OrderTypes");
            AppendCommentLine(SB);

            int count = 0;

            foreach (var item in StaticCrap.OrderTypes)
            {
                count++;

                SB.AppendLine("");

                AppendCommentLine(SB);
                SB.AppendLine(string.Format("-- OrderType - {0}", item.Name));
                AppendCommentLine(SB);

                string template = GetTemplate("OrderTypeTemplate");

                ReplaceToken(ref template, "WorksheetID", WorksheetIdentifier);
                ReplaceToken(ref template, "Count", count.ToString());
                ReplaceToken(ref template, "Name", item.Name);

                SB.AppendLine(template);

                SB.AppendLine("");
            }
        }
    }
}
