using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;

namespace NetSteps.Addresses.UI.Models
{
	public class AddressUIModel_BE : DefaultAddressUIModel
	{
		[RegularExpression("^(\\d{4})", ErrorMessageResourceName = Localization_BE.__InvalidPostalCode, ErrorMessageResourceType = typeof(Localization_BE))]
		public override string PostalCode { get; set; }

		public class Localization_BE
		{
			internal const string __InvalidPostalCode = "InvalidPostalCode";
			public static string InvalidPostalCode { get { return Translation.GetTerm(__InvalidPostalCode, "Invalid Postal Code."); } }
		}
	}
}
