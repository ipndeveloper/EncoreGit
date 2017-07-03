using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;

namespace NetSteps.Addresses.UI.Models
{
	public class AddressUIModel_FR : DefaultAddressUIModel
	{
		[RegularExpression("^(\\d{5})", ErrorMessageResourceName = Localization_FR.__InvalidPostalCode, ErrorMessageResourceType = typeof(Localization_FR))]
		public override string PostalCode { get; set; }

		public class Localization_FR
		{
			internal const string __InvalidPostalCode = "InvalidPostalCode";
			public static string InvalidPostalCode { get { return Translation.GetTerm(__InvalidPostalCode, "Invalid Postal Code."); } }
		}
	}
}
