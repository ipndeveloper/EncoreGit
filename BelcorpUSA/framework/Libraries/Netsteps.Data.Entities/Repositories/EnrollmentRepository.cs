using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;
using NetSteps.Enrollment.Common;

namespace NetSteps.Data.Entities.Repositories
{
	[ContainerRegister(typeof(IEnrollmentRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class EnrollmentRepository : IEnrollmentRepository
	{
		public void AddDistributorJoinsDownlineEvent(int enrollingAccountID, int initialOrderID)
		{
			DomainEventQueueItem.AddDistributorJoinsDownlineEventToQueue(initialOrderID, enrollingAccountID);
		}

		public void AddEnrollmentCompleteEvent(int enrollingAccountID, int accountTypeID, int? initialOrderID)
		{
			if (!initialOrderID.HasValue)
			{
				DomainEventQueueItem.AddEnrollmentCompletedEventToQueue(enrollingAccountID, accountTypeID);
			}
			else
			{
				// Send Email to the new account holder with order start kit info.
				// Note the annoying order of the parameters.
				DomainEventQueueItem.AddEnrollmentCompletedEventToQueue(initialOrderID.Value, enrollingAccountID);
			}
		}

		public IEnumerable<int> GetEnabledAccountTypes()
		{
			return SmallCollectionCache.Instance.AccountTypes
												.Where(at => at.Active)
												.Select(at => (int)at.AccountTypeID);
		}

		public int DistributorAccountID
		{
			get { return (int)Constants.AccountType.Distributor; }
		}

		public void SaveOptOut(string emailAddress)
		{
			var optedOut = Create.New<OptOut>();
			optedOut.EmailAddress = emailAddress;
			optedOut.OptOutTypeID = (short)Constants.OptOutType.EndUser;
			optedOut.Save();
		}
	}
}
