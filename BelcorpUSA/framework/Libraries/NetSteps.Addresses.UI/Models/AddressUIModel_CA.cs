using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;

namespace NetSteps.Addresses.UI.Models
{
	public class AddressUIModel_CA : DefaultAddressUIModel
	{
		[Display(Name = Localization_CA.__Province, ResourceType = typeof(Localization_CA))]
		[Required(ErrorMessageResourceName = Localization_CA.__ProvinceRequired, ErrorMessageResourceType = typeof(Localization_CA))]
		public override string StateProvince { get; set; }

		[RegularExpression("^([^\\d\\s]\\d[^\\d\\s]\\s?\\d[^\\d\\s]\\d)", ErrorMessageResourceName = Localization_CA.__InvalidPostalCode, ErrorMessageResourceType = typeof(Localization_CA))]
		public override string PostalCode { get; set; }

		public class Localization_CA
		{
			internal const string __Province = "Province";
			public static string Province { get { return Translation.GetTerm(__Province, "Province"); } }
			internal const string __ProvinceRequired = "ProvinceRequired";
			public static string ProvinceRequired { get { return Translation.GetTerm(__ProvinceRequired, "Province is required."); } }

			internal const string __InvalidPostalCode = "InvalidPostalCode";
			public static string InvalidPostalCode { get { return Translation.GetTerm(__InvalidPostalCode, "Invalid Postal Code."); } }
		}
	}
}
