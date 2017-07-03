using System;
using System.Diagnostics.Contracts;
using NetSteps.Data.Common.Context;
using NetSteps.Enrollment.Common.Models.Context;

namespace NetSteps.Enrollment.Common
{
	[ContractClass(typeof(IEnrollmentServiceContracts))]
	public interface IEnrollmentService
	{
		/// <summary>
		/// Gets the AccountID of the Corporate account
		/// </summary>
		/// <returns>Int AccountID</returns>
		int GetCorporateSponsorID();

		/// <summary>
		/// Whether the account type can opt out
		/// </summary>
		/// <param name="accountTypeID">The AccountTypeID</param>
		/// <returns>True if the account type can opt out</returns>
		bool IsAccountTypeOptOutable(short accountTypeID);

		/// <summary>
		/// Whether the account type is enrollable
		/// </summary>
		/// <param name="accountTypeID">The AccountTypeID</param>
		/// <returns>True if the account type can enroll</returns>
		bool IsAccountTypeSignUpEnabled(short accountTypeID);

		/// <summary>
		/// Performs any actions that result from completing enrollment
		/// </summary>
		/// <param name="enrollmentCompleteUTC"></param>
		/// <param name="enrollingAccountID"></param>
		/// <param name="accountTypeID"></param>
		/// <param name="initialOrderID"></param>
		void OnEnrollmentComplete(DateTime enrollmentCompleteUTC, int enrollingAccountID, int accountTypeID, int? initialOrderID = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enrollmentContext"></param>
		void OptOut(IEnrollmentContext enrollmentContext);
	}

	[ContractClassFor(typeof(IEnrollmentService))]
	internal abstract class IEnrollmentServiceContracts : IEnrollmentService
	{
		int IEnrollmentService.GetCorporateSponsorID()
		{
			throw new NotImplementedException();
		}

		bool IEnrollmentService.IsAccountTypeOptOutable(short accountTypeID)
		{
			Contract.Requires<ArgumentException>(accountTypeID > 0);
			throw new NotImplementedException();
		}

		bool IEnrollmentService.IsAccountTypeSignUpEnabled(short accountTypeID)
		{
			Contract.Requires<ArgumentException>(accountTypeID > 0);
			throw new NotImplementedException();
		}

		void IEnrollmentService.OnEnrollmentComplete(DateTime enrollmentCompleteUTC, int enrollingAccountID, int accountTypeID, int? initialOrderID)
		{
			Contract.Requires<ArgumentException>(enrollingAccountID > 0);
			Contract.Requires<ArgumentException>(accountTypeID > 0);
			throw new NotImplementedException();
		}

		void IEnrollmentService.OptOut(IEnrollmentContext enrollmentContext)
		{
			Contract.Requires<ArgumentNullException>(enrollmentContext != null);
			throw new NotImplementedException();
		}
	}
}
