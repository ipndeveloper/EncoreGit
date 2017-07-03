using System.ComponentModel.DataAnnotations;
using NetSteps.Common.Globalization;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Addresses.UI.Models
{
	[ContainerRegister(typeof(Common.Models.IAddressUIModel), RegistrationBehaviors.Default)]
	public class DefaultAddressUIModel : Common.Models.IAddressUIModel
	{
		[Display(Name = Localization.__Attention, ResourceType = typeof(Localization))]
        [StringLength(100)]
		public virtual string Attention { get; set; }

		[Display(Name = Localization.__AddressLine1, ResourceType = typeof(Localization))]
		[Required(ErrorMessageResourceName = Localization.__AddressLine1Required, ErrorMessageResourceType = typeof(Localization))]
        [StringLength(200)]
		public virtual string Address1 { get; set; }

		[Display(Name = Localization.__AddressLine2, ResourceType = typeof(Localization))]
		[StringLength(200)]
		public virtual string Address2 { get; set; }

		[Display(Name = Localization.__AddressLine3, ResourceType = typeof(Localization))]
        [StringLength(200)]
		public virtual string Address3 { get; set; }

		[Display(Name = Localization.__City, ResourceType = typeof(Localization))]
		[Required(ErrorMessageResourceName = Localization.__CityRequired, ErrorMessageResourceType = typeof(Localization))]
        [StringLength(200)]
		public virtual string City { get; set; }

		[Display(Name = Localization.__County, ResourceType = typeof(Localization))]
        [StringLength(100)]
		public virtual string County { get; set; }

		[Display(Name = Localization.__State, ResourceType = typeof(Localization))]
        [StringLength(100)]
		public virtual string StateProvince { get; set; }

		[Display(Name = Localization.__PostalCode, ResourceType = typeof(Localization))]
		[Required(ErrorMessageResourceName = Localization.__PostalCodeRequired, ErrorMessageResourceType = typeof(Localization))]
		[StringLength(20)]
		public virtual string PostalCode { get; set; }

        [Display(Name = Localization.__CountryCode, ResourceType = typeof(Localization))]
        [StringLength(25)]
		public virtual string CountryCode { get; set; }

        [Display(Name = Localization.__FirstName, ResourceType = typeof(Localization))]
        [StringLength(100)]
        public virtual string FirstName { get; set; }

        [Display(Name = Localization.__LastName, ResourceType = typeof(Localization))]
        [StringLength(100)]
        public virtual string LastName { get; set; }

        [Display(Name = Localization.__PhoneNumber, ResourceType = typeof(Localization))]
        [StringLength(50)]
        public virtual string PhoneNumber { get; set; }

        [Display(Name = Localization.__PhoneType, ResourceType = typeof(Localization))]
        public virtual int? PhoneTypeID { get; set; }

        [Display(Name = Localization.__State, ResourceType = typeof(Localization))]
        public virtual int? StateProvinceID { get; set; }

		public class Localization
		{
            internal const string __FirstName = "FirstName";
            public static string FirstName { get { return Translation.GetTerm(__FirstName, "First Name"); } }

            internal const string __LastName = "LastName";
            public static string LastName { get { return Translation.GetTerm(__LastName, "Last Name"); } }

            internal const string __PhoneNumber = "PhoneNumber";
            public static string PhoneNumber { get { return Translation.GetTerm(__PhoneNumber, "Phone Number"); } }

            internal const string __PhoneType = "PhoneType";
            public static string PhoneType { get { return Translation.GetTerm(__PhoneType, "Phone Type"); } }

            internal const string __Attention = "Attention";
			public static string Attention { get { return Translation.GetTerm(__Attention, "Attention (C/O)"); } }

			internal const string __AddressLine1 = "AddressLine1";
			public static string AddressLine1 { get { return Translation.GetTerm(__AddressLine1, "Address Line 1"); } }
			internal const string __AddressLine1Required = "AddressLine1Required";
			public static string AddressLine1Required { get { return Translation.GetTerm(__AddressLine1Required, "Address Line 1 is required."); } }

			internal const string __AddressLine2 = "AddressLine2";
			public static string AddressLine2 { get { return Translation.GetTerm(__AddressLine2, "Address Line 2"); } }

			internal const string __AddressLine3 = "AddressLine3";
			public static string AddressLine3 { get { return Translation.GetTerm(__AddressLine3, "Address Line 3"); } }

			internal const string __City = "City";
			public static string City { get { return Translation.GetTerm(__City, "City"); } }
			internal const string __CityRequired = "CityRequired";
			public static string CityRequired { get { return Translation.GetTerm(__CityRequired, "City is required."); } }

			internal const string __County = "County";
			public static string County { get { return Translation.GetTerm(__County, "County"); } }

			internal const string __State = "State";
			public static string State { get { return Translation.GetTerm(__State, "State"); } }

			internal const string __PostalCode = "PostalCode";
			public static string PostalCode { get { return Translation.GetTerm(__PostalCode, "PostalCode"); } }
			internal const string __PostalCodeRequired = "PostalCodeRequired";
			public static string PostalCodeRequired { get { return Translation.GetTerm(__PostalCodeRequired, "PostalCode is required."); } }

            internal const string __CountryCode = "CountryCode";
            public static string CountryCode { get { return Translation.GetTerm(__CountryCode, "CountryCode"); } }
            internal const string __CountryCodeRequired = "Country Required";
            public static string CountryCodeRequired { get { return Translation.GetTerm(__CountryCodeRequired, "Country is required."); } }
		}
    }
}
