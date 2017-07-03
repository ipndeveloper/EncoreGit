using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;

namespace NetSteps.Addresses.UI.Models
{
	public class AddressUIModel_AU : DefaultAddressUIModel
	{
		[Display(Name = Localization_AU.__Province, ResourceType = typeof(Localization_AU))]
		[Required(ErrorMessageResourceName = Localization_AU.__ProvinceRequired, ErrorMessageResourceType = typeof(Localization_AU))]
		public override string StateProvince { get; set; }

		public class Localization_AU
		{
			internal const string __Province = "Province";
			public static string Province { get { return Translation.GetTerm(__Province, "Province"); } }
			internal const string __ProvinceRequired = "ProvinceRequired";
			public static string ProvinceRequired { get { return Translation.GetTerm(__ProvinceRequired, "Province is required."); } }
		}
	}
}
