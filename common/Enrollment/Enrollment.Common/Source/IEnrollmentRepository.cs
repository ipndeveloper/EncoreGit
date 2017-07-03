using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace NetSteps.Enrollment.Common
{
	[ContractClass(typeof(IEnrollmentRepositoryContracts))]
	public interface IEnrollmentRepository
	{
		/// <summary>
		/// Queues a domain event for Distributor Joins Downline
		/// </summary>
		/// <param name="enrollingAccountID"></param>
		/// <param name="initialOrderID"></param>
		void AddDistributorJoinsDownlineEvent(int enrollingAccountID, int initialOrderID);

		/// <summary>
		/// Queues a domain event for Enrollment Complete
		/// </summary>
		/// <param name="enrollingAccountID"></param>
		/// <param name="initialOrderID"></param>
		void AddEnrollmentCompleteEvent(int enrollingAccountID, int accountTypeID, int? initialOrderID);

		/// <summary>
		/// Returns a list of active account types
		/// </summary>
		/// <returns></returns>
		IEnumerable<int> GetEnabledAccountTypes();

		/// <summary>
		/// This is the Distributor account type
		/// </summary>
		int DistributorAccountID { get; }

		/// <summary>
		/// Saves an OptOut entity to the database
		/// </summary>
		/// <param name="emailAddress">Email address to create OptOut for</param>
		void SaveOptOut(string emailAddress);
	}

	public static class IEnrollmentRepositoryExtensions
	{
		public static void AddEnrollmentCompleteEvent(this IEnrollmentRepository repository, int enrollingAccountID, int accountTypeID)
		{
			repository.AddEnrollmentCompleteEvent(enrollingAccountID, accountTypeID, null);
		}
	}

	[ContractClassFor(typeof(IEnrollmentRepository))]
	internal abstract class IEnrollmentRepositoryContracts : IEnrollmentRepository
	{
		void IEnrollmentRepository.AddDistributorJoinsDownlineEvent(int enrollingAccountID, int initialOrderID)
		{
			throw new System.NotImplementedException();
		}

		void IEnrollmentRepository.AddEnrollmentCompleteEvent(int enrollingAccountID, int accountTypeID, int? initialOrderID)
		{
			throw new System.NotImplementedException();
		}

		IEnumerable<int> IEnrollmentRepository.GetEnabledAccountTypes()
		{
			throw new System.NotImplementedException();
		}

		int IEnrollmentRepository.DistributorAccountID
		{
			get { throw new System.NotImplementedException(); }
		}

		void IEnrollmentRepository.SaveOptOut(string emailAddress)
		{
			Contract.Requires<ArgumentNullException>(emailAddress != null);
			Contract.Requires<ArgumentException>(emailAddress == string.Empty);
			throw new System.NotImplementedException();
		}
	}
}
