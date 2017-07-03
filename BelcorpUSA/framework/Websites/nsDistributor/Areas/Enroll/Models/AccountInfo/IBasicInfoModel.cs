namespace nsDistributor.Areas.Enroll.Models.AccountInfo
{
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.ComponentModel.DataAnnotations;

	using NetSteps.Addresses.Common.Models;
	using NetSteps.Data.Entities;

    using nsDistributor.Areas.Enroll.Models.Shared;

	public interface IBasicInfoModel
	{
		[NSRequired]
		[NSDisplayName("FirstName", "First Name")]
		string FirstName { get; set; }

		[NSRequired]
		[NSDisplayName("LastName", "Last Name")]
		string LastName { get; set; }

		[NSRequired]
		[NSEmail]
		[NSDisplayName("Email")]
		string Email { get; set; }

		[NSRequired]
		[NSDisplayName("Password")]
		string Password { get; set; }

		[NSRequired]
		[NSCompare("Password", TermName = "ErrorPasswordsDoNotMatch", ErrorMessage = "Passwords do not match.")]
		[NSDisplayName("ConfirmPassword", "Confirm Password")]
		string ConfirmPassword { get; set; }

		[NSDisplayName("EnrollAs", "Enroll as")]
		bool IsEntity { get; set; }

		[NSDisplayName("SSN")]
		[CustomValidation(typeof(BasicInfoModel), "ValidateSSN")]
		TaxNumberModel SSN { get; set; }

		[NSRequired(Condition = "EnableEntityValidation")]
		[NSDisplayName("EntityName", "Entity Name")]
		string EntityName { get; set; }

		[NSDisplayName("EIN")]
		[CustomValidation(typeof(BasicInfoModel), "ValidateEIN")]
		TaxNumberModel EIN { get; set; }

		[NSRequired]
		[NSDisplayName("Gender")]
		Constants.Gender Gender { get; set; }

		[NSDisplayName("DateOfBirth", "Date of Birth")]
		[CustomValidation(typeof(BasicInfoModel), "ValidateBirthday")]
		DateModel Birthday { get; set; }

		[NSDisplayName("PhoneNumber", "Phone Number")]
		[CustomValidation(typeof(BasicInfoModel), "ValidateMainPhone")]
		PhoneModel MainPhone { get; set; }

		bool ShowTaxNumber { get; set; }

		int CountryID { get; set; }

		string TaxNumber { get; }

		string MaskedTaxNumber { get; }

		BasicAddressModel MainAddress { get; set; }

		List<AccountPropertyModel> AccountProperties { get; set; }

		bool Active { get; set; }

		string Action { get; set; }

		string Title { get; set; }

		string PartialViewName { get; set; }

		bool Completed { get; set; }

		BasicInfoModel LoadValues(
			int countryID,
			Account account,
			IAddress mainAddress,
			bool forcePasswordChange,
			bool showTaxNumber);

		BasicInfoModel LoadResources();

		BasicInfoModel ApplyTo(Account account);

		BasicInfoModel ApplyTo(User user);

		BasicInfoModel ApplyTo(Address address);

		SectionModel LoadBaseResources(
			bool active,
			string action,
			string title,
			string partialViewName,
			bool completed);
	}
}