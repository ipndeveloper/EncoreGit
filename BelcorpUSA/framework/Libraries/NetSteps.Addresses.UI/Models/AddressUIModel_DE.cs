using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;

namespace NetSteps.Addresses.UI.Models
{
	public class AddressUIModel_DE : DefaultAddressUIModel
	{
		[RegularExpression("^(\\d{5})", ErrorMessageResourceName = Localization_DE.__InvalidPostalCode, ErrorMessageResourceType = typeof(Localization_DE))]
		public override string PostalCode { get; set; }

		public class Localization_DE
		{
			internal const string __InvalidPostalCode = "InvalidPostalCode";
			public static string InvalidPostalCode { get { return Translation.GetTerm(__InvalidPostalCode, "Invalid Postal Code."); } }
		}
	}
}
