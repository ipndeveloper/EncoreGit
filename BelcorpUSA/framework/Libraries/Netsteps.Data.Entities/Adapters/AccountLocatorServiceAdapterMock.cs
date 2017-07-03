using NetSteps.AccountLocatorService.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Adapters
{
	public class AccountLocatorServiceAdapterMock : IAccountLocatorServiceAdapter
	{
		public IPaginatedList<IAccountLocatorServiceResult> Search(IAccountLocatorServiceSearchParameters searchParameters)
		{
			var results = new PaginatedList<IAccountLocatorServiceResult> { CreateTestResult() };

			return results;
		}

		private static IAccountLocatorServiceResult CreateTestResult()
		{
			var result = Create.New<IAccountLocatorServiceResult>();

			result.AccountId = 0;
			result.City = "Lehi";
			result.CountryId = 1;
			result.Distance = 1.0;
			result.DistanceType = GeoLocation.DistanceType.Kilometers;
			result.FirstName = "Test";
			result.IsPhotoContentHtmlEncoded = false;
			result.LastName = "Test";
			result.PhotoContent = string.Empty;
			result.PwsUrl = string.Empty;
			result.State = "UT";

			return result;
		}
	}
}
