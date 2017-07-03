using System.Collections.Generic;
using System.Data;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.WebService.Mobile.Models
{
    public class KPIRptWidget
    {
        public string LandingTitle { get; set; }

        public string LandingValue { get; set; }

        public List<KPIRptRow> Rows { get; set; }

        public static implicit operator KPIRptWidget(MobilePerformanceWidgetDataSet ds)
        {
            var model = new KPIRptWidget
            {
	            LandingTitle = ds.KPITitle,
	            LandingValue = ds.KPIValue,
	            Rows = new List<KPIRptRow>()
            };

	        // name: column name  value: IsOnSummary
            var columnNVs = new List<NameValue<string, bool>>();
            foreach (DataColumn item in ds.Tables[0].Columns)
            {
                columnNVs.Add(new NameValue<string, bool>(item.ColumnName, ds.SummaryViewColumns.Contains(item.ColumnName)));
            }

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
	            var kpiRow = new KPIRptRow {Cells = new List<KPIRptCell>()};
	            foreach (var col in columnNVs)
                {
                    kpiRow.Cells.Add(new KPIRptCell
                    {
                        Name = col.Name,
                        Value = dr[col.Name].ToString(),
                        IsOnSummary = col.Value
                    });
                }
                model.Rows.Add(kpiRow);
            }

            return model;
        }
    }

    public class KPIRptRow
    {
        public List<KPIRptCell> Cells { get; set; }
    }

    public class KPIRptCell
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public bool IsOnSummary { get; set; }
    }
}