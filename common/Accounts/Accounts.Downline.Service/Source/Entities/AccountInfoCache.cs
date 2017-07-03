using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Downline.Service.Entities
{
	[Table("AccountInfoCache", Schema = "Accounts")]
	public class AccountInfoCache
	{
		[Key]
		public int AccountID { get; set; }
		public string AccountNumberSortable { get; set; }
		public string SponsorAccountNumber { get; set; }
		public string SponsorFirstName { get; set; }
		public string SponsorLastName { get; set; }
		public string EnrollerAccountNumber { get; set; }
		public string EnrollerFirstName { get; set; }
		public string EnrollerLastName { get; set; }
		public string Address1 { get; set; }
		public string City { get; set; }
		public int? StateProvinceID { get; set; }
		public string StateAbbreviation { get; set; }
		public string PostalCode { get; set; }
		public int? CountryID { get; set; }
		public double? Latitude { get; set; }
		public double? Longitude { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime? NextAutoshipRunDate { get; set; }
		public string PwsUrl { get; set; }
		public DateTime? LastOrderCommissionDateUTC { get; set; }
		public byte[] RowHash { get; set; }
	}
}
