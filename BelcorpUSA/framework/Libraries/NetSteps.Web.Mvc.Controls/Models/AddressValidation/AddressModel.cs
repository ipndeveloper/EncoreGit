using NetSteps.Data.Common.Entities;

namespace NetSteps.Web.Mvc.Controls.Models.AddressValidation
{

    public class AddressModel : IAddress
    {
		public int AddressID { get; set; }
		public short AddressTypeID { get; set; }
		public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Attention { get; set; }
		public string Name { get; set; }
		public string County { get; set; }
		public int? StateProvinceID { get; set; }
		public int CountryID { get; set; }
		public string CountryCode { get; set; }
		public string StateProvinceAbbreviation { get; set; }
		public string PhoneNumber { get; set; }
		public bool IsDefault { get; set; }
		public int ProfileID { get; set; }
		public string ProfileName { get; set; }
		public bool IsWillCall { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
    }
}