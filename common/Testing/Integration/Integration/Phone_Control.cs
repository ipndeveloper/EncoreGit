using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration
{
    public class Phone_Control : Control<Div>
    {
        private TextField _areaCode, _prefix, _number, _phoneNumber;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _areaCode = Element.GetElement<TextField>(new Param("AreaCode", AttributeName.ID.Id, RegexOptions.None).Or(new Param("MainPhone_Substrings_0__Text")));
            _prefix = Element.GetElement<TextField>(new Param("FirstThree", AttributeName.ID.Id, RegexOptions.None).Or(new Param("MainPhone_Substrings_1__Text")));
            _number = Element.GetElement<TextField>(new Param("LastFour", AttributeName.ID.Id, RegexOptions.None).Or(new Param("MainPhone_Substrings_2__Text")));
            _phoneNumber = Element.GetElement<TextField>(new Param("phoneInput", AttributeName.ID.Id, RegexOptions.None).Or(new Param("MainPhone_Substrings_0__Text")));
        }

        public Country.ID Country
        { get; set; }

        public PhoneType.ID PhoneID
        { get; set; }

        public string PhoneNumber
        {
            get
            {
                string number;
                if (Country == NetSteps.Testing.Integration.Country.ID.UnitedStates)
                {
                    number = _areaCode.CustomGetText() + _prefix.CustomGetText() + _number.CustomGetText();
                }
                else
                {
                    number = _phoneNumber.CustomGetText();
                }
                return number;
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
                {
                    _phoneNumber.CustomSetTextQuicklyHelper(value);
                }
            }
        }

        public void EnterPhone(Phone phone)
        {
            Country = phone.Country;
            PhoneID = phone.PhoneID;
            PhoneNumber = phone.PhoneNumber;
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Country, PhoneNumber, PhoneID);
        }

        public bool ValidatePhone(Phone phone)
        {
            Country = phone.Country;
            return Compare.CustomCompare<string>(this.ToString(), CompareID.Equal, phone.ToString(), "Phone");
        }
    }
}
