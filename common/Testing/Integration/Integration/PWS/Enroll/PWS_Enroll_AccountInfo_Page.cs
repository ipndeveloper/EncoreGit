using System;
using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_AccountInfo_Page : PWS_Base_Page
    {
        private TextField _firstName, _lastName, _ssn1, _ssn2, _ssn3, _ein1, _ein2, _businessName, _dobMonth, _dobDay, _dobYear, _email,
            _password, _passwordConfirm;
        private RadioButton _individual, _business, _male, _female, _gender;
        private Country.ID _countryId;
        private Address_Control _address;
        private Phone_Control _phone;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _firstName = _content.GetElement<TextField>(new Param("FirstName"));
            _lastName = _content.GetElement<TextField>(new Param("LastName"));
            _individual = _content.GetElement<RadioButton>(new Param("btnIsEntityFalse"));
            _business = _content.GetElement<RadioButton>(new Param("btnIsEntityTrue"));
            _ssn1 = _content.GetElement<TextField>(new Param("SSN_Substrings_0__Text"));
            _ssn2 = _content.GetElement<TextField>(new Param("SSN_Substrings_1__Text"));
            _ssn3 = _content.GetElement<TextField>(new Param("SSN_Substrings_2__Text"));
            _ein1 = _content.GetElement<TextField>(new Param("EIN_Substrings_0__Text"));
            _ein2 = _content.GetElement<TextField>(new Param("EIN_Substrings_1__Text"));
            _businessName = _content.GetElement<TextField>(new Param("EntityName"));
            _male = _content.GetElement<RadioButton>(new Param("btnGenderMale"));
            _female = _content.GetElement<RadioButton>(new Param("btnGenderFemale"));
            _gender = _content.GetElement<RadioButton>(new Param("btnGenderNotSet"));
            _dobMonth = _content.GetElement<TextField>(new Param("Birthday_Month"));
            _dobDay = _content.GetElement<TextField>(new Param("Birthday_Day"));
            _dobYear = _content.GetElement<TextField>(new Param("Birthday_Year"));
            _phone = _content.GetElement<TextField>(new Param("MainPhone_CountryID")).Parent.As<Phone_Control>();
            _email = _content.GetElement<TextField>(new Param("Email"));
            _password = _content.GetElement<TextField>(new Param("Password"));
            _passwordConfirm = _content.GetElement<TextField>(new Param("ConfirmPassword"));
            _address = _content.GetElement<Div>(new Param("MainAddress")).As<Address_Control>();
        }

        public Address_Control Address
        {
            get { return _address; }
        }

         public override bool IsPageRendered()
        {
            return IsCurrentDocument;
        }

        public string FirstName
        {
            get { return _firstName.CustomGetText(); }
            set { _firstName.CustomSetTextQuicklyHelper(value); }
        }

        public string LastName
        {
            get { return _lastName.CustomGetText(); }
            set { _lastName.CustomSetTextQuicklyHelper(value); }
        }

        public bool BusinessEntity
        {
            get { return _business.Checked; }
            set
            {
                if (value)
                    _business.CustomSelectRadioButton();
                else
                    _individual.CustomSelectRadioButton();
            }
        }

        public string BusinessName
        {
            get { return _businessName.CustomGetText(); }
            set { _businessName.CustomSetTextQuicklyHelper(value); }
        }

        public string TaxID
        {
            get
            {
                string taxId;
                if (BusinessEntity)
                {
                    if (_countryId == Country.ID.UnitedStates)
                        taxId = _ein1.CustomGetText() + _ein2.CustomGetText();
                    else
                        taxId = _ein1.CustomGetText();
                }
                else
                {
                    if (_countryId == Country.ID.UnitedStates)
                        taxId = _ssn1.CustomGetText() + _ssn2.CustomGetText() + _ssn3.CustomGetText();
                    else
                        taxId = _ssn1.CustomGetText();
                }
                return taxId;
            }
            set
            {
                if (BusinessEntity)
                {
                    if (_countryId == Country.ID.UnitedStates)
                    {
                        _ein1.CustomSetTextQuicklyHelper(value.Substring(0, 2));
                        _ein2.CustomSetTextQuicklyHelper(value.Substring(2));
                    }
                    else
                        _ein1.CustomSetTextQuicklyHelper(value);
                }
                else
                {
                    if (_countryId == Country.ID.UnitedStates)
                    {
                        _ssn1.CustomSetTextQuicklyHelper(value.Substring(0, 3));
                        _ssn2.CustomSetTextQuicklyHelper(value.Substring(3, 2));
                        _ssn3.CustomSetTextQuicklyHelper(value.Substring(5));
                    }
                    else
                        _ssn1.CustomSetTextQuicklyHelper(value);
                }

            }

        }

        public Gender.ID Gender
        {
            get
            {
                Integration.Gender.ID gender;

                if (_male.Checked == true)
                    gender = Integration.Gender.ID.Male;
                else if (_female.Checked == true)
                    gender = Integration.Gender.ID.Female;
                else
                    gender = Integration.Gender.ID.PreferNotToSay;
                return gender;
            }
            set
            {
                if (value == Integration.Gender.ID.Male)
                    _male.CustomSelectRadioButton();
                else if (value == Integration.Gender.ID.Female)
                    _female.CustomSelectRadioButton();
                else
                    _gender.CustomSelectRadioButton();
            }
        }

        public DateTime BirthDate
        {
            get { return new DateTime(int.Parse(_dobYear.CustomGetText()), int.Parse(_dobMonth.CustomGetText()), int.Parse(_dobDay.CustomGetText())); }
            set
            {
                _dobMonth.CustomSetTextQuicklyHelper(value.Month.ToString());
                _dobDay.CustomSetTextQuicklyHelper(value.Day.ToString());
                _dobYear.CustomSetTextQuicklyHelper(value.Year.ToString());
            }
        }

        public Phone_Control Phone
        {
            get { return _phone; }
        }

        public string Email
        {
            get { return _email.CustomGetText(); }
            set { _email.CustomSetTextQuicklyHelper(value); }
        }

        public string Password
        {
            set { _password.CustomSetTextQuicklyHelper(value); }
        }

        public string PasswordConfirm
        {
            set { _passwordConfirm.CustomSetTextQuicklyHelper(value); }
        }


        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _content.GetElement<Link>(new Param("btnBasicInfoSubmit").Or(new Param("Button", AttributeName.ID.ClassName))).CustomClick(timeout);
            var addressValidation = Util.GetPage<UnverifiedAddress_Page>(2, false);
            if(addressValidation.IsPageRendered())
                addressValidation.ClickContinueWithThisAddress();
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public PWS_Enroll_AccountInfo_Page EnterDistributorInfo(Distributor customer)
        {
            EnterPreferredInfo(customer);
            if (!string.IsNullOrEmpty(customer.BusinessName))
            {
                BusinessEntity = true;
                BusinessName = customer.BusinessName;
            }
            if (!string.IsNullOrEmpty(customer.TaxID))
                TaxID = customer.TaxID;
            return this;
        }

        public PWS_Enroll_AccountInfo_Page EnterPreferredInfo(PreferredCustomer preferredCustomer)
        {
            // Set Country as some field depend on this knowledge
            _countryId = preferredCustomer.MainAddress.Country;
            // Enter the fields
            FirstName = preferredCustomer.FirstName;
            LastName = preferredCustomer.LastName;
            this.Gender = preferredCustomer.Gender;
            BirthDate = preferredCustomer.BirthDate;
            this.Phone.EnterPhone(preferredCustomer.Phones.GetPhone(0));
            Email = preferredCustomer.Email;
            Password = preferredCustomer.Password;
            PasswordConfirm = preferredCustomer.Password;
            return this;
        }

        public bool ValidatePreferredCustomer(PreferredCustomer customer)
        {
            bool valid = ValidateRetailCustomer(customer);
            if (!Compare.CustomCompare<Gender.ID>(Gender, CompareID.Equal, customer.Gender, "Gender"))
                valid = false;
            if (!Compare.CustomCompare<string>(BirthDate.ToShortDateString(), CompareID.Equal, customer.BirthDate.ToShortDateString(), "Birthdate"))
                valid = false;
            if (!Phone.ValidatePhone(customer.Phones.GetPhone(0)))
                valid = false;
            return valid;
        }

        public bool ValidateRetailCustomer(RetailCustomer customer)
        {
            bool valid = true;
            if (!Compare.CustomCompare<string>(FirstName, CompareID.Equal, customer.FirstName, "First Name"))
                valid = false;
            if (!Compare.CustomCompare<string>(LastName, CompareID.Equal, customer.LastName, "Last Name"))
                valid = false;
            if (!Compare.CustomCompare<string>(Email, CompareID.Equal, customer.Email, "Email"))
                valid = false;
            if (!Compare.CustomCompare<string>(_password.CustomGetText(), CompareID.Equal, "~.FAKE.~", "Password"))
                valid = false;
            if (!Compare.CustomCompare<string>(_passwordConfirm.CustomGetText(), CompareID.Equal, "~.FAKE.~", "Password Confirm"))
                valid = false;
            return valid;
        }
    }
}
