
namespace DistributorBackOffice.Areas.Account.Models.BillingShippingProfiles
{
    public class AddressViewModel
    {
        public int? AddressId { get; set; }
        public string ProfileName { get; set; }
        public string Attention { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public int CountryId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
    }
}