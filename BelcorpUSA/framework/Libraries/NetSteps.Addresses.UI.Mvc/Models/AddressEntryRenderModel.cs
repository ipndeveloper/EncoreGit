using System.Collections.Generic;

namespace NetSteps.Addresses.UI.Mvc.Models
{
    public class AddressEntryRenderModel
    {
        public AddressEntrySettings Settings { get; set; }
        public List<AddressEntryCountryListItem> Countries { get; set; }
        public List<AddressEntryCityListItem> Cities { get; set; }
        public List<AddressEntryStateProvinceListItem> States { get; set; }
    }
}