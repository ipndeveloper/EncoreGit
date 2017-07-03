using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Modules.Enrollments.Common
{
	/// <summary>
	/// Enrollment Repository Adapter
	/// </summary>
	public interface IEnrollmentRepositoryAdapter
	{
		/// <summary>
		/// Create a new Account
		/// </summary>
		/// <param name="account"></param>
		/// <returns></returns>
		IEnrollmentAccountResult CreateAccount(IEnrollingAccount account);
		/// <summary>
		/// Create a new User
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="password"></param>
		/// <param name="languageID"></param>
		/// <returns></returns>
		IEnrollingUserResult CreateUser(int accountID, string password, int languageID);
		/// <summary>
		/// Check for prospect account to upgrade
		/// </summary>
		/// <param name="email"></param>
		/// <param name="sponsorId"></param>
		/// <returns></returns>
		IEnrollingAccount GetProspectAccountForUpgradeIfExists(string email, int sponsorId);
		/// <summary>
		/// Check availability of an Email Address
		/// </summary>
		/// <param name="email"></param>
		/// <returns></returns>
		bool ValidateEmailAddressAvailibility(string email);
		/// <summary>
		/// Check the availability of a Tax Number
		/// </summary>
		/// <param name="taxNumber"></param>
		/// <param name="accountID"></param>
		/// <returns></returns>
		bool IsTaxNumberAvailable(string taxNumber, int accountID);
		/// <summary>
		/// Create an internal mail account for Distributor account types
		/// </summary>
		/// <param name="accountType"></param>
		/// <param name="accountID"></param>
		void CreateMailAccountForDistributors(int accountType, int accountID);
		/// <summary>
		/// Get a term translation
		/// </summary>
		/// <param name="termName"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		string GetTerm(string termName, string defaultValue);
		/// <summary>
		/// Create a new PWS Site
		/// </summary>
		/// <param name="accountID"></param>
		/// <param name="autoshipOrderID"></param>
		/// <param name="url"></param>
		/// <param name="marketID"></param>
		/// <param name="siteName"></param>
		/// <param name="displayName"></param>
		/// <returns></returns>
		IEnrollmentOrderResult CreateSite(int accountID, int autoshipOrderID, string url, int marketID, string siteName = "", string displayName = "");

		/// <summary>
		/// Activate an account
		/// </summary>
		/// <param name="accountID"></param>
		void ActivateAccount(int accountID);
	}
}
