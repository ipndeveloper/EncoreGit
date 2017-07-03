
using NetSteps.AccountLeadService.Common;
using NetSteps.AccountLeadService.Common.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.AccountLeadService.Services
{
	[ContainerRegister(typeof(IAccountLeadService), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class AccountLeadService : IAccountLeadService
	{
		protected IAccountLeadRepository AccountLeadRepository;

		public AccountLeadService() : this(null)
		{
		}

		public AccountLeadService(IAccountLeadRepository accountLeadRepository)
		{
			AccountLeadRepository = accountLeadRepository ?? Create.New<IAccountLeadRepository>();
		}

		#region Implementation of IAccountLeadService

		public virtual int GetLeadCount(int accountId)
		{
			int leadCount = AccountLeadRepository.GetLeadCount(accountId) ?? 0;

			return leadCount;
		}

		public virtual void SetLeadCount(int accountId, int amount)
		{
			AccountLeadRepository.SetLeadCount(accountId, amount);
		}

		public virtual void IncrementLeadCount(int accountId, int amount = 1)
		{
			int leadCount = this.GetLeadCount(accountId);

			AccountLeadRepository.SetLeadCount(accountId, leadCount + amount);
		}

		#endregion
	}
}
