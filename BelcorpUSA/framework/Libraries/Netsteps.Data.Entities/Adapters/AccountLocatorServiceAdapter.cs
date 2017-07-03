using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Web;

using NetSteps.AccountLocatorService.Common;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Adapters
{
	[ContainerRegister(typeof(IAccountLocatorServiceAdapter), RegistrationBehaviors.Default)]
	public class AccountLocatorServiceAdapter : IAccountLocatorServiceAdapter
	{
		#region IAccountLocatorServiceAdapter Implementations

		public virtual IPaginatedList<IAccountLocatorServiceResult> Search(IAccountLocatorServiceSearchParameters searchParameters)
		{
			var mappedParameters = MapNew(searchParameters);

			return Search(mappedParameters);
		}

		#endregion

		#region Public Methods

		public virtual IPaginatedList<IAccountLocatorServiceResult> Search(IAccountLocatorSearchParameters searchParameters)
		{
			var results = Account.AccountLocatorSearch(searchParameters);

			return MapNew(results);
		}

		#endregion

		#region Map Data.Entities models to Account Locator Service models

		public static IPaginatedList<IAccountLocatorServiceResult> MapNew(IPaginatedList<IAccountLocatorSearchData> from)
		{
            var to = new PaginatedList<IAccountLocatorServiceResult>() { TotalCount = from.TotalCount, PageSize = from.PageSize, PageIndex = from.PageIndex };
			Map(from, to);

			return to;
		}

		public static IPaginatedList<IAccountLocatorServiceResult> Map(IPaginatedList<IAccountLocatorSearchData> from, PaginatedList<IAccountLocatorServiceResult> to)
		{
			Contract.Requires<ArgumentNullException>(to != default(PaginatedList<IAccountLocatorServiceResult>));

			if (from == default(IPaginatedList<IAccountLocatorSearchData>))
			{
				return default(IPaginatedList<IAccountLocatorServiceResult>);
			}

			to.AddRange(@from.Select(MapNew));

			return to;
		}

		public static IAccountLocatorServiceResult MapNew(IAccountLocatorSearchData from)
		{
			var to = Create.New<IAccountLocatorServiceResult>();
			Map(from, to);

			return to;
		}

		public static IAccountLocatorServiceResult Map(IAccountLocatorSearchData from, IAccountLocatorServiceResult to)
		{
			Contract.Requires<ArgumentNullException>(to != default(IAccountLocatorServiceResult));

			if (from == default(IAccountLocatorSearchData))
			{
				return default(IAccountLocatorServiceResult);
			}

			to.AccountId = from.AccountID;
			to.City = from.City;
			to.CountryId = from.CountryID;
			to.Distance = from.Distance;
			to.DistanceType = from.DistanceType;
			to.FirstName = from.FirstName;
			to.IsPhotoContentHtmlEncoded = true;
			to.LastName = from.LastName;
			to.PhotoContent = from.PhotoContent == default(IHtmlString) ? string.Empty : from.PhotoContent.ToHtmlString();
			to.PwsUrl = from.PwsUrl;
			to.State = from.State;
            to.EmailAddress=from.EmailAddress;
            to.PhoneNumber = from.PhoneNumber;
			return to;
		}

		#endregion

		#region Map Account Locator Service models to Data.Entities models

		public static IAccountLocatorSearchParameters MapNew(IAccountLocatorServiceSearchParameters from)
		{
			var to = new AccountLocatorSearchParameters();
			Map(from, to);

			return to;
		}

		public static IAccountLocatorSearchParameters Map(IAccountLocatorServiceSearchParameters from, AccountLocatorSearchParameters to)
		{
			Contract.Requires<ArgumentNullException>(to != default(AccountLocatorSearchParameters));

			if (from == default(IAccountLocatorServiceSearchParameters))
			{
				return default(IAccountLocatorSearchParameters);
			}

			to.AccountNumber = from.AccountNumber;
			to.AccountTypeIDs = from.AccountTypeIDs;
			to.DistanceType = from.DistanceType;
			to.FirstName = from.FirstName;
			to.LanguageID = from.LanguageID;
			to.LastName = from.LastName;
			to.Latitude = from.Latitude;
			to.Longitude = from.Longitude;
			to.MaximumDistance = from.MaximumDistance;
			to.OrderBy = from.OrderBy;
			to.OrderByDirection = from.OrderByDirection;
			to.PageIndex = from.PageIndex;
			to.PageSize = from.PageSize;
			to.RequirePwsUrl = from.RequirePwsUrl;

			return to;
		}

		#endregion
	}
}
