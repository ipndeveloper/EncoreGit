using System.Collections.Generic;

namespace DistributorBackOffice.Models.Performance
{
    public class TitleProgressionWidgetAdvancedViewModel : TitleProgressionWidgetViewModel
    {
        public IEnumerable<TitleProgressionAdvancedWidgetRow> Rows { get; private set; }
        public TitleProgressionWidgetAdvancedViewModel(IEnumerable<TitleProgressionAdvancedWidgetRow> rows)
        {
            Rows = rows;
        }
    }
}