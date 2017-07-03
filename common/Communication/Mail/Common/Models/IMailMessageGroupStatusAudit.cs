using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailMessageGroupStatusAudit.
	/// </summary>
	[ContractClass(typeof(Contracts.MailMessageGroupStatusAuditContracts))]
	public interface IMailMessageGroupStatusAudit
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailMessageGroupStatusAuditID for this MailMessageGroupStatusAudit.
		/// </summary>
		int MailMessageGroupStatusAuditID { get; set; }
	
		/// <summary>
		/// The MailMessageGroupID for this MailMessageGroupStatusAudit.
		/// </summary>
		int MailMessageGroupID { get; set; }
	
		/// <summary>
		/// The MessageGroupStatusID for this MailMessageGroupStatusAudit.
		/// </summary>
		Nullable<short> MessageGroupStatusID { get; set; }
	
		/// <summary>
		/// The AttemptCount for this MailMessageGroupStatusAudit.
		/// </summary>
		int AttemptCount { get; set; }
	
		/// <summary>
		/// The RetryTimeUTC for this MailMessageGroupStatusAudit.
		/// </summary>
		Nullable<System.DateTime> RetryTimeUTC { get; set; }
	
		/// <summary>
		/// The DateAddedUTC for this MailMessageGroupStatusAudit.
		/// </summary>
		System.DateTime DateAddedUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The MailMessageGroup for this MailMessageGroupStatusAudit.
		/// </summary>
	    IMailMessageGroup MailMessageGroup { get; set; }
	
		/// <summary>
		/// The MessageGroupStatus for this MailMessageGroupStatusAudit.
		/// </summary>
	    IMessageGroupStatus MessageGroupStatus { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailMessageGroupStatusAudit))]
		internal abstract class MailMessageGroupStatusAuditContracts : IMailMessageGroupStatusAudit
		{
		    #region Primitive properties
		
			int IMailMessageGroupStatusAudit.MailMessageGroupStatusAuditID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailMessageGroupStatusAudit.MailMessageGroupID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IMailMessageGroupStatusAudit.MessageGroupStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailMessageGroupStatusAudit.AttemptCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IMailMessageGroupStatusAudit.RetryTimeUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IMailMessageGroupStatusAudit.DateAddedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IMailMessageGroup IMailMessageGroupStatusAudit.MailMessageGroup
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IMessageGroupStatus IMailMessageGroupStatusAudit.MessageGroupStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
