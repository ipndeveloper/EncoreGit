
namespace NetSteps.Addresses.UI.Common.Models
{
	public interface IAddressUIModel 
	{
        string Attention { get; set; }
		string Address1 { get; set; }
		string Address2 { get; set; }
        string Address3 { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string PhoneNumber { get; set; }
        int? PhoneTypeID { get; set; }
		string City { get; set; }
		string County { get; set; }
		string StateProvince { get; set; }
        int? StateProvinceID { get; set; }
		string PostalCode { get; set; }
        string CountryCode { get; set; }
	}
}