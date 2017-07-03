using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailMessageGroup.
	/// </summary>
	[ContractClass(typeof(Contracts.MailMessageGroupContracts))]
	public interface IMailMessageGroup
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailMessageGroupID for this MailMessageGroup.
		/// </summary>
		int MailMessageGroupID { get; set; }
	
		/// <summary>
		/// The MailMessageID for this MailMessageGroup.
		/// </summary>
		int MailMessageID { get; set; }
	
		/// <summary>
		/// The MessageGroupStatusID for this MailMessageGroup.
		/// </summary>
		Nullable<short> MessageGroupStatusID { get; set; }
	
		/// <summary>
		/// The AttemptCount for this MailMessageGroup.
		/// </summary>
		int AttemptCount { get; set; }
	
		/// <summary>
		/// The IsOriginal for this MailMessageGroup.
		/// </summary>
		Nullable<bool> IsOriginal { get; set; }
	
		/// <summary>
		/// The tmpAccountID for this MailMessageGroup.
		/// </summary>
		Nullable<int> tmpAccountID { get; set; }
	
		/// <summary>
		/// The RetryTimeUTC for this MailMessageGroup.
		/// </summary>
		Nullable<System.DateTime> RetryTimeUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The MailMessage for this MailMessageGroup.
		/// </summary>
	    IMailMessage MailMessage { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessageGroupAddresses for this MailMessageGroup.
		/// </summary>
		IEnumerable<IMailMessageGroupAddress> MailMessageGroupAddresses { get; }
	
		/// <summary>
		/// Adds an <see cref="IMailMessageGroupAddress"/> to the MailMessageGroupAddresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageGroupAddress"/> to add.</param>
		void AddMailMessageGroupAddress(IMailMessageGroupAddress item);
	
		/// <summary>
		/// Removes an <see cref="IMailMessageGroupAddress"/> from the MailMessageGroupAddresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageGroupAddress"/> to remove.</param>
		void RemoveMailMessageGroupAddress(IMailMessageGroupAddress item);
	
		/// <summary>
		/// The MailMessageGroupStatusAudits for this MailMessageGroup.
		/// </summary>
		IEnumerable<IMailMessageGroupStatusAudit> MailMessageGroupStatusAudits { get; }
	
		/// <summary>
		/// Adds an <see cref="IMailMessageGroupStatusAudit"/> to the MailMessageGroupStatusAudits collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageGroupStatusAudit"/> to add.</param>
		void AddMailMessageGroupStatusAudit(IMailMessageGroupStatusAudit item);
	
		/// <summary>
		/// Removes an <see cref="IMailMessageGroupStatusAudit"/> from the MailMessageGroupStatusAudits collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageGroupStatusAudit"/> to remove.</param>
		void RemoveMailMessageGroupStatusAudit(IMailMessageGroupStatusAudit item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailMessageGroup))]
		internal abstract class MailMessageGroupContracts : IMailMessageGroup
		{
		    #region Primitive properties
		
			int IMailMessageGroup.MailMessageGroupID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailMessageGroup.MailMessageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IMailMessageGroup.MessageGroupStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailMessageGroup.AttemptCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IMailMessageGroup.IsOriginal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IMailMessageGroup.tmpAccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IMailMessageGroup.RetryTimeUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IMailMessage IMailMessageGroup.MailMessage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessageGroupAddress> IMailMessageGroup.MailMessageGroupAddresses
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailMessageGroup.AddMailMessageGroupAddress(IMailMessageGroupAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailMessageGroup.RemoveMailMessageGroupAddress(IMailMessageGroupAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IMailMessageGroupStatusAudit> IMailMessageGroup.MailMessageGroupStatusAudits
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailMessageGroup.AddMailMessageGroupStatusAudit(IMailMessageGroupStatusAudit item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailMessageGroup.RemoveMailMessageGroupStatusAudit(IMailMessageGroupStatusAudit item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
