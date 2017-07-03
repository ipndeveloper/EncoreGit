using WatiN.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_Enroll_Page : GMP_Accounts_Base_Page
    {
        // Common Components
        private CustomerType.ID _customerType;
        private SearchSuggestionBox_Control _sponsor;
        private TextField _txtFirstName, _txtMiddileName, _txtLastName, _txtEmail, _txtuserName, _txtPassword, _txtConfirmPassword;
        private SelectList _language;
        private CheckBox _taxExempt, _chkbxGenUserName, _chkbxGenPassword;
        private Address_Control _mainAddress;
        private Link _lnkNext;
        private Para _submit;
        private TableCell _taxID;
        private Param _param0 = new Param(0);
        private Param _param1 = new Param(1);
        private Param _param2 = new Param(2);

        // Retail Customer fields
        private SelectList _gender;

        // Preferred Customer fields
        private GMP_Accounts_Birthdate_Control _birthdate;
        private List<GMP_Accounts_Phone_Control> _phones;
        private CheckBox _shippingCheckbox, _billingCheckbox;
        private Address_Control _shipping;
        protected Billing_Control _billing;

        // Distributor fields
        private SearchSuggestionBox_Control _placement;
        private CheckBox _applicationOnFile;
        private SelectList _taxExemptReason;

        // Business Entity fields
        private TextField _txtBusEntityName;
        //private TextField _txtEINFieldOne, _txtEINFieldTwo;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _sponsor = Control.CreateControl<SearchSuggestionBox_Control>(Document.GetElement<TextField>(new Param("sponsor")));
            _txtFirstName = _content.GetElement<TextField>(new Param("firstName"));
            _txtMiddileName = _content.GetElement<TextField>(new Param("middleName"));
            _txtLastName = _content.GetElement<TextField>(new Param("lastName"));
            _txtEmail = _content.GetElement<TextField>(new Param("email"));
            _language = _content.GetElement<SelectList>(new Param("languageId"));
            _taxExempt = _content.GetElement<CheckBox>(new Param("taxExempt"));
            _chkbxGenUserName = _content.GetElement<CheckBox>(new Param("generateUsername"));
            _txtuserName = _content.GetElement<TextField>(new Param("username"));
            _chkbxGenPassword = _content.GetElement<CheckBox>(new Param("generatePassword"));
            _txtPassword = _content.GetElement<TextField>(new Param("password"));
            _txtConfirmPassword = _content.GetElement<TextField>(new Param("passwordConfirm"));
            _mainAddress = _content.GetElement<Div>(new Param("FormContainer", AttributeName.ID.ClassName)).As<Address_Control>();
            _lnkNext = _content.GetElement<Link>(new Param("btnNext"));
            _submit = _content.GetElement<Para>(new Param("Enrollment SubmitPage", AttributeName.ID.ClassName));
            _taxID = _content.GetElement<TableCell>(new Param("ssn"));
            ConfigurePage();
        }

        public GMP_Accounts_Enroll_Page ConfigurePage(CustomerType.ID customerType = CustomerType.ID.Retail)
        {
            _customerType = customerType;

            if(_customerType == CustomerType.ID.Preferred)
            {
                _shipping = _content.GetElement<Div>(new Param("FauxTable", AttributeName.ID.ClassName, RegexOptions.None).And(new Param(1))).As<Address_Control>();
                _billing = _content.GetElement<Div>(new Param("FauxTable", AttributeName.ID.ClassName, RegexOptions.None).And(new Param(2))).As<Billing_Control>();
            }
            else if ((int)_customerType > (int)CustomerType.ID.Preferred)
            {
                _placement = _content.GetElement<TextField>(new Param("placement")).As<SearchSuggestionBox_Control>();
                _applicationOnFile = _content.CheckBox("applicationOnFile");
                _taxExemptReason = _content.GetElement<SelectList>(new Param("PropertyType", AttributeName.ID.Id, RegexOptions.None));
                _shipping = _content.GetElement<Div>(new Param("FauxTable", AttributeName.ID.ClassName, RegexOptions.None).And(new Param(0))).As<Address_Control>();
                _billing = _content.GetElement<Div>(new Param("FauxTable", AttributeName.ID.ClassName, RegexOptions.None).And(new Param(1))).As<Billing_Control>();
            }

            if (_customerType == CustomerType.ID.Business)
            {
                _txtBusEntityName = _content.GetElement<TextField>(new Param("entityName"));
            }
            else
            {
                _gender = Document.GetElement<SelectList>(new Param("gender"));
            }

            if(_customerType != CustomerType.ID.Retail)
            {
                _birthdate = _content.GetElement<TableCell>(new Param("dateOfBirth")).As<GMP_Accounts_Birthdate_Control>();
                _phones = new List<GMP_Accounts_Phone_Control>();
                _shippingCheckbox = _content.GetElement<CheckBox>(new Param("chkUseMainForShipping"));
                _billingCheckbox = _content.GetElement<CheckBox>(new Param("chkUseMainForBilling"));
            }
            return this;
        }

        public string Sponsor
        {
            get { return _sponsor.Text; }
        }

        public SearchSuggestionBox_Control Placement
        {
            get { return _placement; }
        }

        public bool ApplicatonOnFile
        {
            get { return _applicationOnFile.CustomChecked(); }
            set { _applicationOnFile.CustomSetCheckBox(value); }
        }

        public string FirstName
        {
            get { return _txtFirstName.CustomGetText(); }
            set { _txtFirstName.CustomSetTextQuicklyHelper(value); }
        }

        public string MiddleName
        {
            get { return _txtMiddileName.CustomGetText(); }
            set { _txtMiddileName.CustomSetTextQuicklyHelper(value); }
        }

        public string LastName
        {
            get { return _txtLastName.CustomGetText(); }
            set { _txtLastName.CustomSetTextQuicklyHelper(value); }
        }

        public string BusinessName
        {
            get { return _txtBusEntityName.CustomGetText(); }
            set { _txtBusEntityName.CustomSetTextQuicklyHelper(value); }
        }

        public string Email
        {
            get { return _txtEmail.CustomGetText(); }
            set { _txtEmail.CustomSetTextQuicklyHelper(value); }
        }

        public Language.ID Language
        {
            get { return NetSteps.Testing.Integration.Language.Parse(_language.CustomGetSelectedItem()); }
            set { _language.CustomSelectDropdownItem(value.ToPattern()); }
        }

        public bool TaxExempt
        {
            get { return _taxExempt.CustomChecked(); }
            set { _taxExempt.CustomSetCheckBox(value); }
        }

        public string TaxID
        {            
            get
            {
                string taxID;

                if (MainAddress.Country == Country.ID.UnitedStates)
                {
                    taxID = _taxID.GetElement<TextField>(_param0).CustomGetText() + _taxID.GetElement<TextField>(_param1).CustomGetText();
                    if (_customerType != CustomerType.ID.Business)
                    {
                        taxID = taxID + _taxID.GetElement<TextField>(_param2).CustomGetText();
                    }
                }
                else
                {
                    taxID = _taxID.GetElement<TextField>(_param0).CustomGetText();
                }
                return taxID;
            }
            set
            {
                if (MainAddress.Country == Country.ID.UnitedStates)
                {
                    if (_customerType == CustomerType.ID.Business)
                    {
                        _taxID.GetElement<TextField>(_param0).CustomSetTextQuicklyHelper(value.Substring(0, 2));
                        _taxID.GetElement<TextField>(_param1).CustomSetTextQuicklyHelper(value.Substring(2));
                    }
                    else
                    {
                        _taxID.GetElement<TextField>(_param0).CustomSetTextQuicklyHelper(value.Substring(0, 3));
                        _taxID.GetElement<TextField>(_param1).CustomSetTextQuicklyHelper(value.Substring(3, 2));
                        _taxID.GetElement<TextField>(_param2).CustomSetTextQuicklyHelper(value.Substring(5));
                    }
                }
                else
                {
                    _taxID.GetElement<TextField>(_param0).CustomSetTextQuicklyHelper(value);
                }
            }
        }

        public Gender.ID Gender
        {
            get { return NetSteps.Testing.Integration.Gender.Parse(_gender.SelectedItem); }
            set { _gender.CustomSelectDropdownItem(value.ToPattern()); }
        }

        public bool GenerateUserName
        {
            get { return _chkbxGenUserName.CustomChecked(); }
            set
            {
                _chkbxGenUserName.CustomSetCheckBox(false);
                _chkbxGenUserName.NextSibling.CustomRunScript(Util.strShow);
            }
        }

        public string UserName
        {
            get { return _txtuserName.CustomGetText(); }
            set { _txtuserName.CustomSetTextQuicklyHelper(value); }
        }

        public bool GeneratePassword
        {
            get { return _chkbxGenPassword.CustomChecked(); }
            set
            {
                _chkbxGenPassword.CustomSetCheckBox(value);
                _chkbxGenPassword.CustomRunScript(Util.strShow);
            }
        }

        public string Password
        {
            set
            {
                this._txtPassword.CustomSetTextQuicklyHelper(value);
                this._txtConfirmPassword.CustomSetTextQuicklyHelper(value);
            }
        }

        public GMP_Accounts_Birthdate_Control Birthdate
        {
            get { return _birthdate; }
        }

        public Address_Control MainAddress
        {
            get { return _mainAddress; }
        }

        public Address_Control ShippingAddress
        {
            get { return _shipping; }
        }

        public Billing_Control Billing
        {
            get { return _billing; }
        }

        public override bool IsPageRendered()
        {
            return _content.GetElement<Div>(new Param("addresses")).Exists;
        }

        public bool ShippingCheckbox
        {
            set {_shippingCheckbox.CustomSetCheckBox(value); }
        }

        public bool BillingCheckbox
        {
            set { _billingCheckbox.CustomSetCheckBox(value); }
        }

        public void AddPhone(Phone phone, int? timeout = null)
        {
            Document.GetElement<Link>(new Param("btnAddPhone")).CustomClick(timeout);
            GMP_Accounts_Phone_Control phoneCtl = Control.CreateControl<GMP_Accounts_Phone_Control>(Document.Paras.Filter(Find.ByClass("phoneContainer"))[_phones.Count]);
            phoneCtl.EnterPhone(phone);
            _phones.Add(phoneCtl);
        }

        public bool EnrollRetailAccount(RetailCustomer customer, CustomerType.ID customerType = CustomerType.ID.Retail)
        {
            bool success = _sponsor.Select
                (
                string.Format("{0} {1}", customer.Sponsor.FirstName, customer.Sponsor.LastName),
                string.Format("{0} {1} (#{2})", customer.Sponsor.FirstName, customer.Sponsor.LastName, customer.Sponsor.ID)
                );
            if (!success)
            {
                // It Works
                success = _sponsor.Select
                    (
                    customer.Sponsor.LastName,
                    string.Format("{0} {1} (#{2})", customer.Sponsor.FirstName, customer.Sponsor.LastName, customer.Sponsor.ID)
                    );
            }
            MainAddress.ConfigureAddressControl(true, false, true, true).EnterAddress(customer.MainAddress);
            FirstName = customer.FirstName;
            if (!string.IsNullOrEmpty(customer.MiddleName))
                MiddleName = customer.MiddleName;
            LastName = customer.LastName;
            Email = customer.Email;
            Language = customer.Language;
            TaxExempt = customer.TaxExempt;
            if (customer.TaxExempt)
            {
                TaxID = customer.TaxID;
            }
            if (_customerType != CustomerType.ID.Business)
            {
                Gender = customer.Gender;
            }
            if (!string.IsNullOrEmpty(customer.UserName))
            {
                GenerateUserName = false;
                UserName = customer.UserName;
            }
            if (!string.IsNullOrEmpty(customer.Password))
            {
                GeneratePassword = false;
                Password = customer.Password;
            }
            return success;
        }

        public bool EnrollPreferredAccount(PreferredCustomer customer, CustomerType.ID customerType = CustomerType.ID.Preferred)
        {
            bool success = EnrollRetailAccount(customer, customerType);
            if (customer.Phones != null)
            {
                // Prefered, Distributor, Business
                foreach (Phone phone in customer.Phones.GetAllPhones())
                {
                    AddPhone(phone);
                }
            }
            Birthdate.EnterBirthDate(customer.BirthDate);
            if (customer.Shipping != null)
            {
                ShippingCheckbox = false;
                ShippingAddress.ConfigureAddressControl(true, false, true,true).EnterAddress(customer.Shipping.GetProfile(0).Address);
            }
            var billing = customer.Billing.GetProfile(0);
            if (billing.Address != null && billing.Address.Address1 != null)
            {
                BillingCheckbox = false;
                Billing.ConfigureBillingControl(false, false, true, true, false, true, true);
            }
            Billing.EnterBilling(billing);

            return success;
        }

        public bool EnrollDistributorAccount(Distributor customer, CustomerType.ID customerType = CustomerType.ID.Distributor)
        {
            bool success = EnrollPreferredAccount(customer, customerType);
            if (!_placement.Select
                (
                string.Format("{0} {1}", customer.Placement.FirstName, customer.Placement.LastName),
                string.Format("{0} {1} (#{2})", customer.Placement.FirstName, customer.Placement.LastName, customer.Placement.ID)
                ))
            {
                // It Works
                if (!_placement.Select
                    (
                    customer.Placement.LastName,
                    string.Format("{0} {1} (#{2})", customer.Placement.FirstName, customer.Placement.LastName, customer.Placement.ID)
                    )
                )
                    success = false;
            }
            if (customer.TaxExempt)
            {
                _taxExemptReason.CustomSelectDropdownItem(customer.TaxExemptReason);
            }
            else
            {
                TaxID = customer.TaxID;
            }
            if ((int)customerType > (int)CustomerType.ID.Distributor)
            {
                // Business
                BusinessName = customer.BusinessName;
            }
            ApplicatonOnFile = customer.ApplicationOnFile;
            return success;
        }

        [System.Obsolete("Use 'EnrollRetailAccount' or 'EnrollPreferredAccount' or 'EnrollDistributorAccount'", true)]
        public bool EnterAccount(Distributor customer, CustomerType.ID customerType)
        {
            return false;
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : GMP_Accounts_Base_Page, new()
        {
            _lnkNext.CustomRunScript("focus");
            timeout = _lnkNext.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
