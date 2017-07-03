using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using NetSteps.Web.API.Base.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Content.Common;
using NetSteps.Modules.Autoship.Common;
using System.Diagnostics.Contracts;
using System.Web.Mvc;
using NetSteps.Diagnostics.Logging.Common;

namespace NetSteps.Web.API.Autoship.Common
{
	/// <summary>
	/// Controler for Autoships
	/// </summary>
	public class AutoshipController : BaseController
	{
		private IAutoship autoship;
		private ITermResolver termResolver;
		private ILogResolver logResolver;

		/// <summary>
		/// Create a new instance
		/// </summary>
		public AutoshipController()
			: this(Create.New<IAutoship>(), Create.New<ITermResolver>(), Create.New<ILogResolver>())
		{
			Contract.Ensures(autoship != null);
			Contract.Ensures(termResolver != null);
			Contract.Ensures(logResolver != null);
		}

		/// <summary>
		/// Create a new instance
		/// </summary>
		/// <param name="iAutoship">Autoship Module</param>
		/// <param name="tResolver">Term Resolver</param>
		/// <param name="lResolver">Log Resolver</param>
		public AutoshipController(IAutoship iAutoship, ITermResolver tResolver, ILogResolver lResolver)
		{
			this.autoship = iAutoship;
			this.termResolver = tResolver;
			this.logResolver = lResolver;
		}

		private bool ValidateAutoshipScheduleID(int autoshipScheduleID,  ref string message)
		{
			bool isValid = true;
			if (autoshipScheduleID == 0)
			{
				isValid = false;
				string term = termResolver.Term("Autoship_Invalid_AutoshipScheduleID", "Invlaid AutoshipScheduleID:");
				message = string.Format("{0} {1}", term, autoshipScheduleID);
			}
			return isValid;
		}

		private bool ValidateAutoshipID(int autoshipID, ref string message)
		{
			bool isValid = true;
			if (autoshipID == 0)
			{
				isValid = false;
				string term = termResolver.Term("Autoship_Invalid_AutoshipID", "Invlaid AutoshipID:");
				message = string.IsNullOrEmpty(message)
					? string.Format("{0} {1}", term, autoshipID)
					: string.Format("{0} {1} {2}", message, term, autoshipID);
			}
			return isValid;
		}

		private bool ValidateAccountID(int accountID, ref string message)
		{
			bool isValid = true;

			if (accountID == 0)
			{
				isValid = false;
				string term = termResolver.Term("Account_Invalid_AccountID", "Invalid AccountID:");
				message = string.Format("{0} {1}", term, accountID);
			}
			return isValid;
		}

		/// <summary>
		/// Search for an accounts autoships of a specific schedule
		/// 
		/// eg. http://yourdomain.com/account/{accountID}/autoships/{autoshipScheduleID}
		/// </summary>
		/// <param name="accountID">Account to search</param>
		/// <param name="autoshipScheduleID">Schedule to search</param>
		/// <param name="ActiveAutoshipsOnly">Only search active autoships</param>
		/// <returns>ActionResult</returns>
		/// <see cref="ActionResult"/>
		[HttpGet]
		[ApiAccessKeyFilter]
		public ActionResult Search(int accountID, int? autoshipScheduleID, bool ActiveAutoshipsOnly = true)
		{
			try
			{
				string message = string.Empty;
				bool isValidAccountID = ValidateAccountID(accountID, ref message);
				bool isValidAutoshipScheduleID = autoshipScheduleID.HasValue ? ValidateAutoshipScheduleID(autoshipScheduleID.Value, ref message) : true;

				if (isValidAccountID && isValidAutoshipScheduleID)
				{
					var result = autoship.Search(accountID, autoshipScheduleID, ActiveAutoshipsOnly);
					return this.Result_200_OK(result);
				}
				return this.Result_400_BadRequest(message);
			}
			catch (Exception ex)
			{
				logResolver.LogException(ex, true);
				return this.Result_400_BadRequest(ex.Message);			
			}
		}

		/// <summary>
		/// Cancel a specific autoship.
		/// 
		/// eg. http://yourdomain.com/account/{accountID}/autoship/{autoshipID}
		/// </summary>
		/// <param name="accountID">the account the autoship belongs too.</param>
		/// <param name="autoshipID">the autoship you wish to cancel</param>
		/// <returns>ActionResult</returns>
		/// <see cref="ActionResult"/>
		[HttpPut]
		[ApiAccessKeyFilter]
		public ActionResult Cancel(int accountID, int autoshipID)
		{
			try
			{
				string message = string.Empty;
				bool isValidAccountID = ValidateAccountID(accountID, ref message);
				bool isValidAutoshipID = ValidateAutoshipID(autoshipID, ref message);

				if (isValidAccountID && isValidAutoshipID)
				{
					var result = autoship.Cancel(accountID, autoshipID);
					return this.Result_200_OK(result);
				}

				return this.Result_400_BadRequest(message);
			}
			catch (Exception ex)
			{
				logResolver.LogException(ex, true);

                return this.Result_400_BadRequest(ex.Message);
			}
		}
	}
}
