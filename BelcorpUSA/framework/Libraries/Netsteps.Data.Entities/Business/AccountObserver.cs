using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Business
{
	public class AccountObserver : IAccountObserver
	{
		public void AccountActivating(Account acct)
		{
			FireOnActivatingAccount(acct);
		}

		public void AccountUpdating(Account acct)
		{
			FireOnUpdatingAccount(acct);
		}

		protected virtual void FireOnActivatingAccount(Account acct)
		{
			var root = Container.Root;
			if (root.Registry.IsTypeRegistered<IAccountActivatingObserver>())
			{
				var observer = Create.New<IAccountActivatingObserver>();
				observer.OnActivatingAccount(acct);
			}
		}

		protected virtual void FireOnUpdatingAccount(Account acct)
		{
			var root = Container.Root;
			if (root.Registry.IsTypeRegistered<IAccountActivatingObserver>())
			{
				var observer = Create.New<IAccountUpdatingObserver>();
				observer.OnUpdatingAccount(acct);
			}
		}
	}
}
