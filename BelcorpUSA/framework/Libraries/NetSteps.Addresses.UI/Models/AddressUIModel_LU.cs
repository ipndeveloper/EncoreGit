using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;

namespace NetSteps.Addresses.UI.Models
{
	public class AddressUIModel_LU : DefaultAddressUIModel
	{
		[RegularExpression("^(\\d{4})", ErrorMessageResourceName = Localization_LU.__InvalidPostalCode, ErrorMessageResourceType = typeof(Localization_LU))]
		public override string PostalCode { get; set; }

		public class Localization_LU
		{
			internal const string __InvalidPostalCode = "InvalidPostalCode";
			public static string InvalidPostalCode { get { return Translation.GetTerm(__InvalidPostalCode, "Invalid Postal Code."); } }
		}
	}
}
