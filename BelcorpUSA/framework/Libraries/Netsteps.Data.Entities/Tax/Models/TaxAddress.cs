
namespace NetSteps.Taxes.Common.Models
{
    public static class TaxAddressProperty
    {
        public static ITaxAddressProperty Create(AddressPropertyKind kind, string name)
        {
            return Create(kind, name, null);
        }
        public static ITaxAddressProperty Create(AddressPropertyKind kind, string name, string code)
        {
            var prop = NetSteps.Encore.Core.IoC.Create.New<ITaxAddressProperty>();
            prop.Kind = kind;
            prop.Name = name;
            prop.Code = code;
            return prop;
        }
        public static ITaxAddressProperty FromCity(string city)
        {
            return Create(AddressPropertyKind.City, city);
        }
        public static ITaxAddressProperty FromCounty(string county)
        {
            return Create(AddressPropertyKind.SubDivision, county);
        }
        public static ITaxAddressProperty FromCounty(string county, string code)
        {
            return Create(AddressPropertyKind.SubDivision, county, code);
        }
        public static ITaxAddressProperty FromStateCode(string stateCode)
        {
            return Create(AddressPropertyKind.MainDivision, null, stateCode);
        }
        public static ITaxAddressProperty FromState(string state)
        {
            return Create(AddressPropertyKind.MainDivision, state, null);
        }
        public static ITaxAddressProperty FromState(string state, string code)
        {
            return Create(AddressPropertyKind.MainDivision, state, code);
        }
        public static ITaxAddressProperty FromCountry(string country)
        {
            return Create(AddressPropertyKind.Country, country);
        }
        public static ITaxAddressProperty FromCountry(string country, string code)
        {
            return Create(AddressPropertyKind.Country, country, code);
        }
        public static ITaxAddressProperty FromPostalCode(string postalCode)
        {
            return Create(AddressPropertyKind.PostalCode, postalCode, postalCode);
        }
    }   
}
