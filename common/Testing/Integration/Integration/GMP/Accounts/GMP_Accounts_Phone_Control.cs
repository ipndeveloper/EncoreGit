using WatiN.Core;
using WatiN.Core.Extras;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_Phone_Control : Control<Para>
    {
        private TextField _areaCode, _prefix, _number, _phoneNumber;
        private SelectList _phoneID;
        private Link deletePhone; // this control does not appear until a phone number is added

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _areaCode = Element.GetElement<TextField>(new Param("AreaCode", AttributeName.ID.Id, RegexOptions.None));
            _prefix = Element.GetElement<TextField>(new Param("FirstThree", AttributeName.ID.Id, RegexOptions.None));
            _number = Element.GetElement<TextField>(new Param("LastFour", AttributeName.ID.Id, RegexOptions.None));
            _phoneNumber = Element.GetElement<TextField>(new Param("phoneInput", AttributeName.ID.ClassName)); // based on Jewel Kade
            _phoneID = Element.GetElement<SelectList>(new Param("phoneType", AttributeName.ID.ClassName));
        }

        public void EnterPhone(Phone phone)
        {
            Country = phone.Country;
            PhoneNumber = phone.PhoneNumber;
            PhoneID = phone.PhoneID;
            deletePhone = Element.GetElement<Link>(new Param("DeletePhone DTL Remove", AttributeName.ID.ClassName));
        }

        public Country.ID Country
        { get; set; }

        public string PhoneNumber
        {
            get
            {
                string phone;
                if (Country == NetSteps.Testing.Integration.Country.ID.UnitedStates)
                    phone = _areaCode.CustomGetText() + _prefix.CustomGetText() + _number.CustomGetText();
                else
                    phone = _phoneNumber.CustomGetText();
                return phone;
            }
            set
            {
                if (Country == NetSteps.Testing.Integration.Country.ID.UnitedStates)
                {
                    _areaCode.CustomSetTextQuicklyHelper(value.Substring(0, 3));
                    _prefix.CustomSetTextQuicklyHelper(value.Substring(3, 3));
                    _number.CustomSetTextQuicklyHelper(value.Substring(6));
                }
                else
                    _phoneNumber.CustomSetTextQuicklyHelper(value);
            }
        }

        public string AreaCode
        {
            get { return _areaCode.CustomGetText(); }
            set 
            {                
                _areaCode.CustomSetTextQuicklyHelper(value);
            }
        }

        public string Prefix
        {
            get { return _prefix.CustomGetText(); }
            set { _prefix.CustomSetTextQuicklyHelper(value); }
        }

        public string Number
        {
            get { return _number.CustomGetText(); }
            set { _number.CustomSetTextQuicklyHelper(value); }
        }

        public PhoneType.ID PhoneID
        {
            get { return PhoneType.Parse(_phoneID.CustomGetSelectedItem()); }
            set { _phoneID.CustomSelectDropdownItem(value.ToString()); }
        }

        public void DeletePhone(int? timeout = null)
        {
            deletePhone.CustomClick(timeout);
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Country.ToPattern(), PhoneNumber, PhoneID);
        }
    }
}
