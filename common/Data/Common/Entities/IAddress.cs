namespace NetSteps.Data.Common.Entities
{
    public interface IAddress
    {
        string Address1 { get; set; }
        string Address2 { get; set; }
        string Address3 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string PostalCode { get; set; }
        string Country { get; }
		int CountryID { get; set; }
		string PhoneNumber { get; set; }
		bool IsDefault { get; set; }
		int ProfileID { get; set; }
		string ProfileName { get; set; }
		bool IsWillCall { get; set; }
		double? Latitude { get; set; }
		double? Longitude { get; set; }
		int? StateProvinceID { get; set; }
		string County { get; set; }
		int AddressID { get; }
		short AddressTypeID { get; set; }
		string FirstName { get; set; }
		string LastName { get; set; }
		string Attention { get; set; }
		string Name { get; set; }
	}
}
