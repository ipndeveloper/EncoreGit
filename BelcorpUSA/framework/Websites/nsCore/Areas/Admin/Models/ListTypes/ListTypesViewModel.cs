using System.Collections.Generic;

namespace nsCore.Areas.Admin.Models.ListTypes
{
    public class ListTypesViewModel
    {
        public ListTypesViewModel(IEnumerable<string> listTypes)
        {
            ListTypes = listTypes;
        }
        public IEnumerable<string> ListTypes { get; set; }
    }
}