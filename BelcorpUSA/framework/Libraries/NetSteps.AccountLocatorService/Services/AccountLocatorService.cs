using NetSteps.AccountLocatorService.Common;
using NetSteps.Common.Base;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.AccountLocatorService
{
	[ContainerRegister(typeof(IAccountLocatorService), RegistrationBehaviors.Default)]
	public class AccountLocatorService : IAccountLocatorService
	{
		public IPaginatedList<IAccountLocatorServiceResult> Search(IAccountLocatorServiceSearchParameters searchParameters)
		{
			var adapter = Create.New<IAccountLocatorServiceAdapter>();

			return adapter.Search(searchParameters);
		}
	}
}
