using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;

namespace NetSteps.Addresses.UI.Models
{
	public class AddressUIModel_NL : DefaultAddressUIModel
	{
		[RegularExpression("^(\\d{4}\\s?[^\\d\\s]{2})", ErrorMessageResourceName = Localization_NL.__InvalidPostalCode, ErrorMessageResourceType = typeof(Localization_NL))]
		public override string PostalCode { get; set; }

		public class Localization_NL
		{
			internal const string __InvalidPostalCode = "InvalidPostalCode";
			public static string InvalidPostalCode { get { return Translation.GetTerm(__InvalidPostalCode, "Invalid Postal Code."); } }
		}
	}
}
