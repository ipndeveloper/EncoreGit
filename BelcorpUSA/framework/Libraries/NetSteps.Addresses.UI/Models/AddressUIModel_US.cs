using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;

namespace NetSteps.Addresses.UI.Models
{
	public class AddressUIModel_US : DefaultAddressUIModel
	{
		[RegularExpression("^(\\d{9}|\\d{5})", ErrorMessageResourceName = Localization_US.__InvalidZipCode, ErrorMessageResourceType = typeof(Localization_US))]
		public override string PostalCode { get; set; }

		[Required(ErrorMessageResourceName = Localization_US.__CountyRequired, ErrorMessageResourceType = typeof(Localization_US))]
		public override string County { get; set; }

		[Required(ErrorMessageResourceName = Localization_US.__StateRequired, ErrorMessageResourceType = typeof(Localization_US))]
		public override string StateProvince { get; set; }

        [Required(ErrorMessageResourceName = Localization_US.__StateRequired, ErrorMessageResourceType = typeof(Localization_US))]
        public override int? StateProvinceID { get; set; }

		public class Localization_US
		{
			internal const string __InvalidZipCode = "InvalidZipCode";
			public static string InvalidZipCode { get { return Translation.GetTerm(__InvalidZipCode, "Invalid Postal Code."); } }

			internal const string __CountyRequired = "CountyRequired";
			public static string CountyRequired { get { return Translation.GetTerm(__CountyRequired, "County is required."); } }

			internal const string __StateRequired = "StateRequired";
			public static string StateRequired { get { return Translation.GetTerm(__StateRequired, "State is required."); } }
		}
	}
}
