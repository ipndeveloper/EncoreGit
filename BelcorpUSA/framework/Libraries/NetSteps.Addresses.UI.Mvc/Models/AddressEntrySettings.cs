using System.Collections.Generic;
using NetSteps.Addresses.UI.Common.Models;

namespace NetSteps.Addresses.UI.Mvc.Models
{
    public class AddressEntrySettings
    {
        #region Fields

        private Data.Entities.Address _addressEntity;
        private IAddressUIModel _addressModel;

        #endregion

        #region Constructor(s)
        
        public AddressEntrySettings()
        {
            CommonCountryOverrideSettings = new AddressEntryCountrySpecificOverrideSettings(string.Empty)
            {
                AllowZipPostalLookup = BoolOverrideSetting.Inherit,
                CleansingMode = AddressCleansingOverrideMode.Inherit,
            };
            CountryOverrideSettings = new Dictionary<string, AddressEntryCountrySpecificOverrideSettings>();
        }

        #endregion

        #region Properties

        public int MarketID { get; set; }

        public Data.Entities.Address AddressEntity
        {
            get
            {
                return _addressEntity;
            }
            set
            {
                if (_addressEntity == value)
                    return;
                _addressEntity = value;
                _addressModel = null;
            }
        }

        public IAddressUIModel AddressModel
        {
            get
            {
                return _addressModel;
            }
            set
            {
                if (_addressModel == value)
                    return;
                _addressModel = value;
                _addressEntity = null;
            }
        }

        public string ClientHtmlID { get; set; }

        public string ClientJsObjID { get; set; }

        public AddressEntryCountryListModes CountryListMode { get; set; }

        public AddressEntryCountrySpecificOverrideSettings CommonCountryOverrideSettings { get; private set; }

        public Dictionary<string, AddressEntryCountrySpecificOverrideSettings> CountryOverrideSettings { get; private set; }

        #endregion
    }
}
