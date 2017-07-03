using WatiN.Core;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public class Address_Control : Control<Div>
    {
        //private Div _parent;
        bool _countrySelector, _profileLine, _attentionLine, _thirdAddressLine;
        private SelectList _country, _citySelectList, _county, _stateOrProvince;
        private TextField _firstName, _lastName, _phone;
        private TextField _profile, _attention, _address1, _address2, _address3, _postalCode, _zip, _zipExtension, _cityTextField;
        private Country.ID _localCountry;

        protected override void  InitializeContents()
        {
            _countrySelector = false;
            _profileLine = false;
            _attentionLine = false;
            _thirdAddressLine = false;
            _country = Element.GetElement<SelectList>(new Param("Country", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _profile = Element.GetElement<TextField>(new Param("ProfileName", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _firstName = Element.GetElement<TextField>(new Param("FirstName", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _lastName = Element.GetElement<TextField>(new Param("LastName", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _phone = Element.GetElement<TextField>(new Param("PhoneNumber", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _attention = Element.GetElement<TextField>(new Param("Attention", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _address1 = Element.GetElement<TextField>(new Param("Address(Line)?1(Account.)?$", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _address2 = Element.GetElement<TextField>(new Param("Address(Line)?2(Account.)?$", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _address3 = Element.GetElement<TextField>(new Param("Address(Line)?3(Account.)?$", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _zip = Element.GetElement<TextField>(new Param("Zip", AttributeName.ID.Id, RegexOptions.IgnoreCase).Or(new Param("PostalCode", AttributeName.ID.Id, RegexOptions.None)));
            _zipExtension = Element.GetElement<TextField>(new Param("ZipPlusFour", AttributeName.ID.Id, RegexOptions.IgnoreCase).Or(new Param("PostalCode2", AttributeName.ID.Id, RegexOptions.None)));
            _citySelectList = Element.GetElement<SelectList>(new Param("City", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _cityTextField = Element.GetElement<TextField>(new Param("City", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _county = Element.GetElement<SelectList>(new Param("County", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _stateOrProvince = Element.GetElement<SelectList>(new Param("State", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _postalCode = Element.GetElement<TextField>(new Param("PostalCode", AttributeName.ID.Id, RegexOptions.IgnoreCase));
        }

        public Country.ID Country
        {
            get
            {
                Integration.Country.ID country;
                if (_countrySelector)
                    country = Integration.Country.Parse(_country.CustomGetSelectedItem());
                else
                    country = _localCountry;
                return country;
            }
            set
            {
                if (_countrySelector)
                {
                    _country.CustomSelectDropdownItem(value.ToPattern(), 1);
                    Thread.Sleep(2000);
                }
                else
                    _localCountry = value;
            }
        }

        public string Profile
        {
            get
            {
                if (!_profileLine)
                    throw new ArgumentException();
                return _profile.CustomGetText();
            }
            set
            {
                if (!_profileLine)
                    throw new ArgumentException();
                _profile.CustomSetTextQuicklyHelper(value);
            }
        }

        public string FirstName
        {
            get { return _firstName.CustomGetText(); }
            set
            {
                _firstName.CustomSetTextQuicklyHelper(value);
                _firstName.CustomRunScript(Util.strChange);
            }
        }

        public string LastName
        {
            get { return _lastName.CustomGetText(); }
            set
            {
                _lastName.CustomSetTextQuicklyHelper(value);
                _lastName.CustomRunScript(Util.strChange);
            }
        }

        public string Phone
        {
            get { return _phone.CustomGetText(); }
            set
            {
                _phone.CustomSetTextQuicklyHelper(value);
                _phone.CustomRunScript(Util.strChange);
            }
        }

        public string Attention
        {
            get
            {
                if (!_attentionLine)
                    throw new ArgumentException();
                return _attention.CustomGetText();
            }
            set
            {
                if (!_attentionLine)
                    throw new ArgumentException();
                _attention.CustomSetTextQuicklyHelper(value);
            }
        }

        public string Address1
        {
            get { return _address1.CustomGetText(); }
            set
            {
                _address1.CustomSetTextQuicklyHelper(value);
                _address1.CustomRunScript(Util.strChange);
            }
        }

        public string Address2
        {
            get { return _address2.CustomGetText(); }
            set
            {
                _address2.CustomSetTextQuicklyHelper(value);
                _address2.CustomRunScript(Util.strChange);
            }
        }

        public string Address3
        {
            get
            {
                if (!_thirdAddressLine)
                    throw new ArgumentException();
                return _address3.CustomGetText();
            }
            set
            {
                if (!_thirdAddressLine)
                    throw new ArgumentException();
                _address3.CustomSetTextQuicklyHelper(value);
            }
        }

        public string PostalCode
        {
            get
            {
                string postalCode;
                if (Country == NetSteps.Testing.Integration.Country.ID.UnitedStates)
                {
                    postalCode = _zip.CustomGetText();
                }
                else
                {
                    postalCode = _postalCode.CustomGetText();
                }
                return postalCode;
            }
            set
            {
                if (Country == NetSteps.Testing.Integration.Country.ID.UnitedStates)
                {
                    _zip.CustomSetTextQuicklyHelper(value);
                    _zip.CustomRunScript(Util.strChange);
                    _zip.CustomRunScript(Util.strKeyUp);
                    Element.CustomWaitForSpinners();
                    Util.Browser.CustomWaitForComplete();
                }
                else
                {
                    _postalCode.CustomSetTextQuicklyHelper(value);
                    _postalCode.CustomRunScript(Util.strChange);
                    Util.Browser.CustomWaitForComplete();
                }
            }
        }

        public string PostalCodeExtension
        {
            get
            {
                return _zipExtension.CustomGetText();
            }
            set
            {
                _zipExtension.CustomSetTextQuicklyHelper(value);
                _zip.CustomRunScript(Util.strChange);
                _zipExtension.CustomRunScript(Util.strKeyUp);
                Element.CustomWaitForSpinners();
            }
        }

        public string City
        {
            get
            {
                string city;
                if (Country == NetSteps.Testing.Integration.Country.ID.UnitedStates)
                {
                    city = _citySelectList.CustomGetSelectedItem();
                }
                else
                {
                    city = _cityTextField.CustomGetText();
                }
                return city;
            }
            set
            {
                if (Country == NetSteps.Testing.Integration.Country.ID.UnitedStates)
                {
                    _citySelectList.CustomSelectDropdownItem(value);
                }
                else
                {
                    _cityTextField.CustomSetTextQuicklyHelper(value);
                    _cityTextField.CustomRunScript(Util.strChange);
                }
            }
        }

        public string County
        {
            get
            {
                return _county.CustomGetSelectedItem();
            }
            set { _county.CustomSelectDropdownItem(value); }
        }

        public string StateOrProvince
        {
            get
            {
                return _stateOrProvince.CustomGetSelectedItem(10);
            }
            set { _stateOrProvince.CustomSelectDropdownItem(value, 10); }
        }

        public Address_Control ConfigureAddressControl(bool countrySelector = false, bool profileLine = false, bool attentionLine = false, bool thirdAddressLine = false)
        {
            _countrySelector = countrySelector;
            _profileLine = profileLine;
            _attentionLine = attentionLine;
            _thirdAddressLine = thirdAddressLine;
            return this;
        }

        public Address_Control EnterCustomer(RetailCustomer customer)
        {
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            if (customer.Phones.Count > 0)
                Phone = customer.Phones.GetPhone(0).PhoneNumber;
            return this;
        }

        public void EnterAddress(Address address)
        {
            Country = address.Country;
            if (address.Country.ToExpandedString() != "United States")
            {
                _county.CustomWaitForExist(null, false);
            }
            if (_profileLine && !string.IsNullOrEmpty(address.Profile))
                Profile = address.Profile;
            if (_attentionLine && !string.IsNullOrEmpty(address.Attention))
                Attention = address.Attention;
            Address1 = address.Address1;
            if (!string.IsNullOrEmpty(address.Address2))
                Address2 = address.Address2;
            if (_thirdAddressLine && !string.IsNullOrEmpty(address.Address3))
                Address3 = address.Address3;
            if (!string.IsNullOrEmpty(address.PostalCode))
                PostalCode = address.PostalCode;
            if (!string.IsNullOrEmpty(address.PostalCodeExtension))
                PostalCodeExtension = address.PostalCodeExtension;
            if (!string.IsNullOrEmpty(address.City))
                City = address.City;
            if (!string.IsNullOrEmpty(address.County))
                County = address.County;
            if (!string.IsNullOrEmpty(address.State))
                StateOrProvince = address.State;
        }

        public bool ValidateAddress(Address address)
        {
            bool valid;

            if(_countrySelector)                
                valid = Compare.CustomCompare<Country.ID>(Country, CompareID.Equal, address.Country, "Country");
            else
            {
                _localCountry = address.Country;
                valid = true;
            }

            if (!Compare.CustomCompare<string>(Address1, CompareID.Equal, address.Address1, "Address1"))
                valid = false;
            if (_attentionLine && !string.IsNullOrEmpty(address.Attention))
            {
                if (!Compare.CustomCompare<string>(Attention, CompareID.Equal, address.Attention, "Attention"))
                    valid = false;
            }
            if (!string.IsNullOrEmpty(address.Address2))
            {
                if (!Compare.CustomCompare<string>(Address2, CompareID.Equal, address.Address2, "Address2"))
                    valid = false;
            }
            if (_thirdAddressLine && !string.IsNullOrEmpty(address.Address3))
            {
                if (!Compare.CustomCompare<string>(Address3, CompareID.Equal, address.Address3, "Address3"))
                    valid = false;
            }
            if (!Compare.CustomCompare<string>(PostalCode, CompareID.Equal, address.PostalCode, "Postal Code"))
                valid = false;
            if (Country == Integration.Country.ID.UnitedStates && !string.IsNullOrEmpty(address.PostalCodeExtension))
            {
                if (!Compare.CustomCompare<string>(PostalCodeExtension, CompareID.Equal, address.PostalCodeExtension, "Postal Code Extension"))
                    valid = false;
            }
            if (!string.IsNullOrEmpty(address.County))
            {
                if (!Compare.CustomCompare<string>(County, CompareID.Equal, address.County, "County"))
                    valid = false;
            }
            if (!Compare.CustomCompare<string>(City, CompareID.Equal, address.City, "City"))
                valid = false;
            if (!Compare.CustomCompare<string>(StateOrProvince, CompareID.Equal, address.State, "State/Provence"))
                valid = false;
            return valid;
        }
    }
}
