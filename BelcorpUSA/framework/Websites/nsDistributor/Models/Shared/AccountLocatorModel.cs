using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using NetSteps.AccountLocatorService;
using NetSteps.AccountLocatorService.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;

namespace nsDistributor.Models.Shared
{
	public class AccountLocatorModel : IValidatableObject
	{
		#region Values
		public virtual AccountLocatorSearchType SearchType { get; set; }

		[NSDisplayName("DistributorID", "Distributor ID")]
		public virtual string AccountNumber { get; set; }

		[NSDisplayName("FirstName", "First Name")]
		public virtual string FirstName { get; set; }

		[NSDisplayName("LastName", "Last Name")]
		public virtual string LastName { get; set; }

		[NSDisplayName("Street")]
		public virtual string Address1 { get; set; }

		[NSDisplayName("City")]
		public virtual string City { get; set; }

        [NSDisplayName("State")]
        public virtual string State { get; set; }

        [NSDisplayName("Country")]
        public virtual string Country { get; set; }

        [NSDisplayName("Lograduro")]
        public virtual string Lograduro { get; set; }


		[NSDisplayName("ZipCode", "Zip Code")]
        //[NSRequired(Condition = "EnableLocationValidation")]
		public virtual string PostalCode { get; set; }

		[NSDisplayName("AccountLocatorMaximumDistanceLabel", "Distance in Miles")]
		[NSRequired(Condition = "EnableLocationValidation")]
		[NSRange(5, 100, Condition = "EnableLocationValidation")]
		public virtual int? MaximumDistance { get; set; }

		[NSRequired(TermName = "ErrorGettingLocationGeoCode", ErrorMessage = "Unable to lookup your location, please try again.", Condition = "EnableLocationValidation")]
		public virtual double? Latitude { get; set; }

		[NSRequired(TermName = "ErrorGettingLocationGeoCode", ErrorMessage = "Unable to lookup your location, please try again.", Condition = "EnableLocationValidation")]
		public virtual double? Longitude { get; set; }

		public virtual int PageIndex { get; set; }
		#endregion

		#region Resources
		public virtual bool ShowMoreButton { get; set; }
		#endregion
        public int? BusquedaAutomaticaSponsorBrasil { get; set; }
		#region Models
		public virtual IEnumerable<AccountLocatorResultModel> SearchResults { get; set; }
        public virtual List<ParameterCountryModel> ParameterCountries { get; set; } 
		

        #endregion

		#region Infrastructure
		public enum AccountLocatorSearchType
		{
			AccountInfo,
			Location,
            Other
		}

		public AccountLocatorModel()
		{
			this.SearchResults = Enumerable.Empty<AccountLocatorResultModel>();
            this.ParameterCountries = new List<ParameterCountryModel>();
		}

		public virtual AccountLocatorModel ApplyTo(AccountLocatorServiceSearchParameters searchParameters)
		{
			switch (SearchType)
			{
				case AccountLocatorSearchType.AccountInfo:
					if (!string.IsNullOrWhiteSpace(AccountNumber))
					{
						searchParameters.AccountNumber = this.AccountNumber;
						searchParameters.FirstName = string.Empty;
						searchParameters.LastName = string.Empty;
					}
					else
					{
						searchParameters.AccountNumber = string.Empty;
						searchParameters.FirstName = FirstName;
						searchParameters.LastName = LastName;
					}
					searchParameters.Latitude = null;
					searchParameters.Longitude = null;
					searchParameters.MaximumDistance = null;
					searchParameters.OrderBy = "FirstName, LastName";
					break;
				case AccountLocatorSearchType.Location:
					searchParameters.AccountNumber = string.Empty;
					searchParameters.FirstName = string.Empty;
					searchParameters.LastName = string.Empty;
					searchParameters.Latitude = this.Latitude;
					searchParameters.Longitude = this.Longitude;
					searchParameters.MaximumDistance = this.MaximumDistance;
					searchParameters.OrderBy = "Distance";
					break;
			}

			return this;
		}

		[Obsolete("This is the old method.  Use the new method that utilizes the new AccountLocatorService object.")]
		public virtual AccountLocatorModel ApplyTo(AccountLocatorSearchParameters searchParameters)
		{
			switch (SearchType)
			{
				case AccountLocatorSearchType.AccountInfo:
					if (!string.IsNullOrWhiteSpace(AccountNumber))
					{
						searchParameters.AccountNumber = this.AccountNumber;
						searchParameters.FirstName = string.Empty;
						searchParameters.LastName = string.Empty;
					}
					else
					{
						searchParameters.AccountNumber = string.Empty;
						searchParameters.FirstName = FirstName;
						searchParameters.LastName = LastName;
					}
					searchParameters.Latitude = null;
					searchParameters.Longitude = null;
					searchParameters.MaximumDistance = null;
					searchParameters.OrderBy = "FirstName, LastName";
					break;
				case AccountLocatorSearchType.Location:
					searchParameters.AccountNumber = string.Empty;
					searchParameters.FirstName = string.Empty;
					searchParameters.LastName = string.Empty;
					searchParameters.Latitude = this.Latitude;
					searchParameters.Longitude = this.Longitude;
					searchParameters.MaximumDistance = this.MaximumDistance;
					searchParameters.OrderBy = "Distance";
					break;
			}

			return this;
		}

		public virtual AccountLocatorModel LoadResources(
			IPaginatedList<IAccountLocatorServiceResult> searchResults,
			Func<IAccountLocatorServiceResult, string> selectUrlExpression)
		{
			SearchResults = searchResults.Select(x => new AccountLocatorResultModel
			{
				AccountID = x.AccountId,
				FullName = string.Format("{0} {1}", x.FirstName, x.LastName),
				Location = string.Format("{0}, {1}", x.City, x.State),
				Distance = (decimal?)x.Distance,
				PwsUrl = x.PwsUrl ?? "",
				PhotoHtml = x.IsPhotoContentHtmlEncoded || string.IsNullOrWhiteSpace(x.PhotoContent) ? x.PhotoContent : HttpUtility.HtmlEncode(x.PhotoContent),
				SelectUrl = selectUrlExpression(x),
                PhoneNumber=x.PhoneNumber,
                EmailAddress = x.EmailAddress
                
			});

			PageIndex = searchResults.PageIndex;
			ShowMoreButton = searchResults.HasNextPage;

			return this;
		}

		[Obsolete("This is the old method.  Use the new method that utilizes the new AccountLocatorService object.")]
		public virtual AccountLocatorModel LoadResources(IPaginatedList<IAccountLocatorSearchData> searchData,
			Func<IAccountLocatorSearchData, string> selectUrlExpression)
		{
			SearchResults = searchData.Select(x => new AccountLocatorResultModel
			{
				AccountID = x.AccountID,
				FullName = string.Format("{0} {1}", x.FirstName, x.LastName),
				Location = string.Format("{0}, {1}", x.City, x.State),
				Distance = (decimal?)x.Distance,
				PwsUrl = x.PwsUrl ?? "",
				PhotoHtml = x.PhotoContent != null ? x.PhotoContent.ToString() : String.Empty,
				SelectUrl = selectUrlExpression(x),
                PhoneNumber = x.PhoneNumber,
                EmailAddress = x.EmailAddress
			});

			PageIndex = searchData.PageIndex;
			ShowMoreButton = searchData.HasNextPage;

			return this;
		}

		public static readonly Predicate<AccountLocatorModel>
			EnableAccountInfoValidation = m => m.SearchType == AccountLocatorSearchType.AccountInfo,
			EnableLocationValidation = m => m.SearchType == AccountLocatorSearchType.Location; 

		public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
		{
			if(EnableAccountInfoValidation(this))
			{
				string[] accountInfoStrings = {
					this.AccountNumber,
					this.FirstName,
					this.LastName,
                    this.Address1,
                    this.City,
                    this.PostalCode
				};
				if (accountInfoStrings.All(x => string.IsNullOrWhiteSpace(x)))
				{
					yield return new ValidationResult(_errorAtLeastOneFieldIsRequired);
				}
			}
		}
		#endregion

		#region Strings
		protected virtual string _errorAtLeastOneFieldIsRequired { get { return Translation.GetTerm("ErrorAtLeastOneFieldIsRequired", "At least one field is required."); } }
		protected virtual string _errorInvalidLocationString { get { return Translation.GetTerm("ErrorGettingLocationGeoCode", "Unable to lookup your location, please try again."); } }
		#endregion
	}

	public class AccountLocatorResultModel
	{
		public virtual int AccountID { get; set; }
		public virtual string FullName { get; set; }
		public virtual string Location { get; set; }
		public virtual decimal? Distance { get; set; }
		public virtual string PwsUrl { get; set; }
		public virtual string PhotoHtml { get; set; }
		public virtual string SelectUrl { get; set; }
        public virtual string EmailAddress { get; set; }
        public virtual string PhoneNumber { get; set; }
        public virtual List<ParameterCountryModel> ParameterCountries { get; set; } 
	}
}