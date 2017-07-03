using System.Collections.Generic;
using System.Linq;
using NetSteps.Addresses.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;

namespace NetSteps.Web.Mvc.Controls.Models
{
    public class AddressModel
    {
        public IAddress Address { get; set; }

        public int LanguageID { get; set; }

        private Country _country;
        public Country Country
        {
            get
            {
                if (_country == null && Address != null && Address.CountryID > 0)
                {
                    _country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == Address.CountryID);
                }
                return _country;
            }
            set
            {
                _country = value;
            }
        }

        public bool ShowCountrySelect { get; set; }

        public string ChangeCountryURL { get; set; }

        private bool _honorRequired = true;
        public bool HonorRequired
        {
            get { return _honorRequired; }
            set { _honorRequired = value; }
        }

        private IEnumerable<Country> _countries;
        public IEnumerable<Country> Countries
        {
            get
            {
                if (_countries == null)
                    _countries = SmallCollectionCache.Instance.Countries;
                return _countries;
            }
            set
            {
                _countries = value;
            }
        }

        public IEnumerable<string> ExcludeFields { get; set; }

        /// <summary>
        /// Allows for overriding a label per view, with the term name as the key and the label as the value
        /// </summary>
        public Dictionary<string, string> LabelOverrides { get; set; }

        public string Prefix { get; set; }

        private string _street;
        public string Street
        {
            get
            {
                return _street;
            }
            set
            {
                _street = value;
            }
        }
    }
}
