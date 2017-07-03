using System.Collections.Generic;
using NetSteps.Enrollment.Common;

namespace NetSteps.Enrollment.Service.Tests.Mocks
{
	public class MockEnrollmentRepository : IEnrollmentRepository
	{
		public void AddDistributorJoinsDownlineEvent(int enrollingAccountID, int initialOrderID)
		{
		}

		public void AddEnrollmentCompleteEvent(int enrollingAccountID, int? initialOrderID)
		{
		}

		public IEnumerable<int> GetEnabledAccountTypes()
		{
			return new[] { 1, 2, 3 };
		}

		public int DistributorAccountID
		{
			get { return 1; }
		}

		public void SaveOptOut(string emailAddress)
		{
		}


		public void AddEnrollmentCompleteEvent(int enrollingAccountID, int accountTypeID, int? initialOrderID)
		{
			throw new System.NotImplementedException();
		}
	}
}
