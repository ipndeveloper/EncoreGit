using System.Collections.Generic;

namespace NetSteps.Addresses.UI.Mvc.Models
{
    public class AddressEntryCountrySpecificOverrideSettings
    {
        #region Constructor(s)
        
        public AddressEntryCountrySpecificOverrideSettings(string countryCode)
        {
            CountryCode = countryCode;
            FieldOverrides = new List<AddressEntryFieldOverrideSettings>();
        }

        #endregion

        #region Properties

        public string CountryCode { get; private set; }

        public BoolOverrideSetting AllowZipPostalLookup { get; set; }

        public AddressCleansingOverrideMode CleansingMode { get; set; }

        public IList<AddressEntryFieldOverrideSettings> FieldOverrides { get; set; }

        #endregion
    }
}
