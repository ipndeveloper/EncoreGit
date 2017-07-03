using WatiN.Core;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_Edit_Page : GMP_Accounts_Section_Page
    {
        private SelectList _selectAccountType;
        private SelectList _selectDefaultlanguage;
        private Span _spanSponsor;
        private Span _spanEnroller;
        private CheckBox _chkbxApplicationOnFile;
        private CheckBox _chkbxIsEntity;
        private TextField _txtUsername;
        private SelectList _selectAccountStatus;
        private TextField _txtFirstName;
        private TextField _txtLastName;
        private TextField _txtEmail;
        private CheckBox _chkbxTaxExempt;
        private TextField _txtSSN0;
        private TextField _txtSSN1;
        private TextField _txtSSN2;
        private TextField _txtEIN0;
        private TextField _txtEIN1;
        private TextField _txtDOBMonth;
        private TextField _txtDOBDay;
        private TextField _txtDOBYear;
        private SelectList _selectGender;
        private ParaCollection _phones;
        private TextField _txtHostedMail;
        private TextField txtEntityName;
        private SelectList selectSiteAccess;
        private SelectList selectIsTaxExemptVerified;
        private Link lnkChangePassword;
        private TextField txtPassword;
        private TextField txtConfirmPassword;
        private Link lnkCancel;
        private Link lnkSaveInformation;
        private Address_Control _address;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _selectAccountType = _content.GetElement<SelectList>(new Param("sAccountType"));
            _selectDefaultlanguage = _content.GetElement<SelectList>(new Param("defaultLanguageId"));
            _spanSponsor = _content.GetElement<Span>(new Param("sponsor"));
            _spanEnroller = _content.GetElement<Span>(new Param("enroller"));
            _chkbxApplicationOnFile = _content.GetElement<CheckBox>(new Param("chkApplicationOnFile"));
            _chkbxIsEntity = _content.GetElement<CheckBox>(new Param("chkIsEntity"));
            _txtUsername = _content.GetElement<TextField>(new Param("txtUsername"));
            _selectAccountStatus = _content.GetElement<SelectList>(new Param("AccountStatusId", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _txtFirstName = _content.GetElement<TextField>(new Param("txtFirstName"));
            _txtLastName = _content.GetElement<TextField>(new Param("txtLastName"));
            _txtEmail = _content.GetElement<TextField>(new Param("txtEmail"));
            _chkbxTaxExempt = _content.GetElement<CheckBox>(new Param("isTaxExempt"));
            _txtEIN0 = _content.GetElement<TextField>(new Param("txtEINPart1"));
            _txtEIN1 = _content.GetElement<TextField>(new Param("txtEINPart2"));
            _txtSSN0 = _content.GetElement<TextField>(new Param("txtSSNPart1"));
            _txtSSN1 = _content.GetElement<TextField>(new Param("txtSSNPart2"));
            _txtSSN2 = _content.GetElement<TextField>(new Param("txtSSNPart3"));
            _txtDOBMonth = _content.GetElement<TextField>(new Param("txtDOBMonth"));
            _txtDOBDay = _content.GetElement<TextField>(new Param("txtDOBDay"));
            _txtDOBYear = _content.GetElement<TextField>(new Param("txtDOBYear"));
            _selectGender = _content.GetElement<SelectList>(new Param("gender"));
            _phones = _content.Paras.Filter(Find.ByClass("phoneContainer"));
            _txtHostedMail = _content.GetElement<TextField>(new Param("txtHostedMailAccount"));
            lnkSaveInformation = _content.GetElement<Link>(new Param("btnSaveAccount"));
            lnkChangePassword = _content.GetElement<Link>(new Param("btnChangePassword"));
            txtPassword = _content.GetElement<TextField>(new Param("txtPassword"));
            txtConfirmPassword = _content.GetElement<TextField>(new Param("txtConfirmPassword"));
            txtEntityName = _content.GetElement<TextField>(new Param("entityName"));
            selectSiteAccess = _content.GetElement<SelectList>(new Param("statusId"));
            selectIsTaxExemptVerified = _content.GetElement<SelectList>(new Param("isTaxExemptVerified"));
            lnkSaveInformation = _content.GetElement<Link>(new Param("btnSaveAccount"));
            lnkCancel = _content.GetElement<Link>(new Param("btnCancel"));
            _address = _content.GetElement<Div>(new Param("FormContainer", AttributeName.ID.ClassName)).As<Address_Control>();
        }

         public override bool IsPageRendered()
        {
            return _content.GetElement<Link>(new Param("btnSaveAccount")).Exists; //Title.Contains("btnSaveAccount"Edit Account");
        }

        public AccountStatus.ID AccountStatus
        {
            get { return Integration.AccountStatus.Parse(_selectAccountStatus.CustomGetSelectedItem()); }
            set { _selectAccountStatus.CustomSelectDropdownItem(value.ToPattern()); }
        }

        public string AccountType
        {
            get { return _selectAccountType.CustomGetSelectedItem(); }
        }

        public string DefaultLanguage
        {
            get { return _selectDefaultlanguage.CustomGetSelectedItem(); }
            set { _selectDefaultlanguage.CustomSelectDropdownItem(value); }
        }

        public string Enroller
        {
            get { return _spanEnroller.CustomGetText(); }
        }

        public string Placement
        {
            get { return _spanSponsor.CustomGetText(); }
        }

        public bool ApplicationOnFile
        {
            get { return _chkbxApplicationOnFile.CustomChecked(); }
            set { _chkbxApplicationOnFile.CustomSetCheckBox(value); }
        }

        public bool BusinessEntity
        {
            get { return _chkbxIsEntity.CustomChecked(); }
            set { _chkbxIsEntity.CustomSetCheckBox(value); }
        }

        public string BusinessName
        {
            get { return txtEntityName.CustomGetText(); }
            set { txtEntityName.CustomSetTextQuicklyHelper(value); }
        }

        public string UserName
        {
            get { return _txtUsername.CustomGetText(); }
            set { _txtUsername.CustomSetTextQuicklyHelper(value); }
        }

        public string HostedEmail
        {
            get { return _txtHostedMail.CustomGetText(); }
        }

        public string FirstName
        {
            get { return _txtFirstName.CustomGetText(); }
            set { _txtFirstName.CustomSetTextQuicklyHelper(value); }
        }

        public string LastName
        {
            get { return _txtLastName.CustomGetText(); }
            set { _txtLastName.CustomSetTextQuicklyHelper(value); }
        }

        public string Email
        {
            get { return _txtEmail.CustomGetText(); }
            set { _txtEmail.CustomSetTextQuicklyHelper(value); }
        }

        public bool TaxExempt
        {
            get { return _chkbxTaxExempt.CustomChecked(); }
            set { _chkbxTaxExempt.CustomSetCheckBox(value); }
        }

        public string TaxID
        {
            get
            {
                string taxNumber;
                if (Address.Country == Country.ID.UnitedStates)
                {
                    if (BusinessEntity)
                    {
                        if (_txtEIN0.Exists)
                            taxNumber = _txtEIN0.CustomGetText() + _txtEIN1.CustomGetText();
                        else
                        {
                            Util.LogFail("Ein fields do not exist");
                            taxNumber = null;
                        }
                    }
                    else
                    {
                        if (_txtSSN0.Exists)
                            taxNumber = _txtSSN0.CustomGetText() + _txtSSN1.CustomGetText() + _txtSSN2.CustomGetText();
                        else
                        {
                            Util.LogFail("SSN fields do not exist");
                            taxNumber = null;
                        }
                    }

                }
                else
                    taxNumber = _txtSSN0.CustomGetText();
                return taxNumber;
            }
            set
            {
                if (Address.Country == Country.ID.UnitedStates)
                {
                    if (BusinessEntity)
                    {
                        _txtSSN0.CustomSetTextQuicklyHelper(value.Substring(0, 2));
                        _txtSSN1.CustomSetTextQuicklyHelper(value.Substring(2));
                    }
                    else
                    {
                        _txtSSN0.CustomSetTextQuicklyHelper(value.Substring(0, 3));
                        _txtSSN1.CustomSetTextQuicklyHelper(value.Substring(3, 2));
                        _txtSSN2.CustomSetTextQuicklyHelper(value.Substring(5));
                    }
                }
                else
                    _txtSSN0.CustomSetTextQuicklyHelper(value);
            }
        }

        public DateTime BirthDate
        {
            get
            {
                string year, month, day;
                DateTime date;
                year = _txtDOBYear.CustomGetText();
                month = _txtDOBMonth.CustomGetText();
                day = _txtDOBDay.CustomGetText();
                if (string.IsNullOrEmpty(year) || string.IsNullOrEmpty(month) || string.IsNullOrEmpty(day))
                    date = new DateTime();
                else
                    date = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day));

                return date;
            }
            set
            {
                _txtDOBYear.CustomSetTextQuicklyHelper(value.Year.ToString());
                _txtDOBMonth.CustomSetTextQuicklyHelper(value.Month.ToString());
                _txtDOBDay.CustomSetTextQuicklyHelper(value.Day.ToString());
            }
        }

        public Gender.ID Gender
        {
            get { return NetSteps.Testing.Integration.Gender.Parse(_selectGender.CustomGetSelectedItem()); }
            set { _selectGender.CustomSelectDropdownItem(value.ToPattern()); }
        }

        public Address_Control Address
        {
            get { return _address; }
        }

        public Phone GetPhone(int index)
        {
            GMP_Accounts_Phone_Control phone = _phones[index].As<GMP_Accounts_Phone_Control>();
            string number;
            Country.ID country = Address.Country;
            if (country == Country.ID.UnitedStates)
                number = phone.AreaCode + phone.Prefix + phone.Number;
            else
                number = phone.PhoneNumber;
            return new Phone(country, number, phone.PhoneID);
        }

        public bool ChangePassword(string password)
        {
            _content.GetElement<Link>(new Param("btnChangePassword")).CustomClick();
            _content.GetElement<TextField>(new Param("txtPassword")).CustomSetTextQuicklyHelper(password);
            _content.GetElement<TextField>(new Param("txtConfirmPassword")).CustomSetTextQuicklyHelper(password);
            string city = Address.City;
            // Treat the next steps as atomic
            ClickSave();
            _content.CustomWaitForSpinners();
            string txt = Document.GetElement<Div>(new Param("messageCenterMessage", AttributeName.ID.ClassName, RegexOptions.None)).CustomGetText();
            return txt.Contains("Account saved");
        }

        public void ClickSave()
        {
            _content.GetElement<Link>(new Param("btnSaveAccount")).CustomClick();
        }

        public bool ValidateAccount(Distributor customer, CustomerType.ID customerType, string accountType)
        {
            bool valid = Compare.CustomCompare<Integration.AccountStatus.ID>(AccountStatus, CompareID.Equal, customer.AccountStatus, "Account Status");
            if (!Compare.CustomCompare<string>(AccountType, CompareID.Equal, accountType, "Account Type"))
                valid = false;
            if (!Compare.CustomCompare<string>(DefaultLanguage, CompareID.Equal, customer.Language.ToExpandedString(), "Default Language"))
                valid = false;
            if (!Compare.CustomCompare<string>(Enroller, CompareID.Contains, String.Format("{0} - {1} {2}", customer.Sponsor.ID, customer.Sponsor.FirstName, customer.Sponsor.LastName), "Sponsor"))
                valid = false;
            if (!Compare.CustomCompare<string>(Placement, CompareID.Contains, String.Format("{0} - {1} {2}", customer.Placement.ID, customer.Placement.FirstName, customer.Placement.LastName), "Enroller"))
                valid = false;
            if (!Compare.CustomCompare<bool>(ApplicationOnFile, CompareID.Equal, customer.ApplicationOnFile, "Application on File"))
                valid = false;
            if (!Compare.CustomCompare<bool>(BusinessEntity, CompareID.Equal, customer.BusinessName != null, "Business Entity"))
                valid = false;
            if (!string.IsNullOrEmpty(customer.UserName) && !Compare.CustomCompare<string>(UserName, CompareID.Equal, customer.UserName, "Username"))
                valid = false;
            if (!Compare.CustomCompare<string>(FirstName, CompareID.Equal, customer.FirstName, "First Name"))
                valid = false;
            if (!Compare.CustomCompare<string>(LastName, CompareID.Equal, customer.LastName, "Last Name"))
                valid = false;
            if (!Compare.CustomCompare<string>(Email, CompareID.Equal, customer.Email, "Email"))
                valid = false;
            if (!Compare.CustomCompare<bool>(TaxExempt, CompareID.Equal, customer.TaxExempt, "Tax Exempt"))
                valid = false;
            if ((int)customerType > (int)CustomerType.ID.Preferred)
            {
                if (!Compare.CustomCompare<string>(customer.TaxID.Substring(customer.TaxID.Length - 4), CompareID.Equal, TaxID.Substring(TaxID.Length - 4), "TaxID"))
                    valid = false;
            }
            if ((int)customerType > (int)CustomerType.ID.Retail)
            {
                // Preferred & Distributor
                if (!Compare.CustomCompare<string>(BirthDate.ToString(), CompareID.Equal, customer.BirthDate.ToString(), "Birth Date"))
                    valid = false;
                if (!ValidatePhones(customer.Phones.GetAllPhones()))
                    valid = false;
            }
            if ((int)customerType > (int)CustomerType.ID.Distributor)
            {
                // Business entity
                //temporary fix until Entity bug:82288 is fixed
                if (txtEntityName.IsVisible())
                {
                    if (!Compare.CustomCompare<string>(BusinessName, CompareID.Equal, customer.BusinessName, "Business Name"))
                        valid = false;
                }
                else
                    valid = false;
                if (!Compare.CustomCompare<bool>(!string.IsNullOrEmpty(HostedEmail), CompareID.Equal, true, "Hosted Email"))
                    valid = false;
            }
            else
            {
                // all but Business
                if (!Compare.CustomCompare<Gender.ID>(Gender, CompareID.Equal, customer.Gender, "Gender"))
                    valid = false;
            }
            return valid;
        }

        public bool ValidatePhones(List<Phone> phones)
        {
            bool valid = true;

            for (int i = 0; i < phones.Count; i++)
            {
                if (!Compare.CustomCompare<string>(phones[i].ToString(), CompareID.Equal, GetPhone(i).ToString(), string.Format("Phone {0}", i)))
                {
                    valid = false;
                    break;
                }
            }
            return valid;
        }

        public void SetPassword(string password)
        {
        }
    }
}
