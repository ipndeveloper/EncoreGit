

namespace NetSteps.Testing.Integration
{
    public class Phone
    {
        string _number;

        public Phone(Country.ID country, string number, PhoneType.ID phoneID)
        {
            Country = country;
            _number = number;
            PhoneID = phoneID;
        }

        public Country.ID Country
        { get; set; }

        public PhoneType.ID PhoneID
        { get; set; }

        public string PhoneNumber
        {
            get { return _number; }
            set { _number = value; }
        }

        public string AreaCode
        {
            get { return _number.Substring(0, 3); }
        }

        public string Prefix
        {
            get { return _number.Substring(3, 3); }
        }

        public string Number
        {
            get { return _number.Substring(6); }
        }

        public override string ToString()
        {
            return string.Format("{0} {1} {2}", Country, PhoneNumber, PhoneID);
        }
    }
}
