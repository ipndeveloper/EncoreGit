

namespace NetSteps.Testing.Integration
{
    public class Address
    {
        public Address(Country.ID country)
        {
            Country = country;
        }

        public Address(Country.ID country, string postalCode)
            : this(country)
        {
            PostalCode = postalCode;
        }

        //public Address(Country.ID country, string postalCode, string address1)
        //    : this(country, postalCode)
        //{
        //    Address1 = address1;
        //}

        //public Address(Country.ID country, string postalCode, string address1, string city)
        //    : this(country, postalCode, address1)
        //{
        //    City = city;
        //}

        public Address(Country.ID country, string postalCode, string address1, string city = null, string state = null, string county = null, string postalCodeExtension = null, string address2 = null, string address3 = null)
            : this(country, postalCode)
        {
            Address1 = address1;
            PostalCodeExtension = postalCodeExtension;
            City = city;
            County = county;
            State = state;
            Address2 = address2;
            Address3 = address3;
        }

        public Address(string profile, Country.ID country, string postalCode, string address1, string city = null, string state = null, string county = null, string postalCodeExtension = null, string address2 = null, string address3 = null)
            : this(country, postalCode, address1, city, state, county, postalCodeExtension, address2, address3)
        {
            Profile = profile;
        }

        public Country.ID Country
        { get; set; }

        public string Profile
        { get; set; }

        public string Attention
        { get; set; }

        public string Address1
        { get; set; }

        public string Address2
        { get; set; }

        public string Address3
        { get; set; }

        public string PostalCode
        { get; set; }

        public string PostalCodeExtension
        { get; set; }

        public string City
        { get; set; }

        public string County
        { get; set; }

        public string State
        { get; set; }
    }
}
