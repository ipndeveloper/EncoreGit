using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AccountEmailLog.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountEmailLogContracts))]
	public interface IAccountEmailLog
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountEmailLogID for this AccountEmailLog.
		/// </summary>
		int AccountEmailLogID { get; set; }
	
		/// <summary>
		/// The AccountID for this AccountEmailLog.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The EmailTypeID for this AccountEmailLog.
		/// </summary>
		Nullable<short> EmailTypeID { get; set; }
	
		/// <summary>
		/// The EmailAddress for this AccountEmailLog.
		/// </summary>
		string EmailAddress { get; set; }
	
		/// <summary>
		/// The SentSuccessfully for this AccountEmailLog.
		/// </summary>
		Nullable<bool> SentSuccessfully { get; set; }
	
		/// <summary>
		/// The DateSentUTC for this AccountEmailLog.
		/// </summary>
		Nullable<System.DateTime> DateSentUTC { get; set; }
	
		/// <summary>
		/// The FailureMessage for this AccountEmailLog.
		/// </summary>
		string FailureMessage { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The EmailType for this AccountEmailLog.
		/// </summary>
	    IEmailType EmailType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountEmailLog))]
		internal abstract class AccountEmailLogContracts : IAccountEmailLog
		{
		    #region Primitive properties
		
			int IAccountEmailLog.AccountEmailLogID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAccountEmailLog.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IAccountEmailLog.EmailTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountEmailLog.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IAccountEmailLog.SentSuccessfully
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAccountEmailLog.DateSentUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAccountEmailLog.FailureMessage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IEmailType IAccountEmailLog.EmailType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
