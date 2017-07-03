using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MessageGroupStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.MessageGroupStatusContracts))]
	public interface IMessageGroupStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MessageGroupStatusID for this MessageGroupStatus.
		/// </summary>
		short MessageGroupStatusID { get; set; }
	
		/// <summary>
		/// The Description for this MessageGroupStatus.
		/// </summary>
		string Description { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessageGroupStatusAudits for this MessageGroupStatus.
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
		[ContractClassFor(typeof(IMessageGroupStatus))]
		internal abstract class MessageGroupStatusContracts : IMessageGroupStatus
		{
		    #region Primitive properties
		
			short IMessageGroupStatus.MessageGroupStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMessageGroupStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessageGroupStatusAudit> IMessageGroupStatus.MailMessageGroupStatusAudits
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMessageGroupStatus.AddMailMessageGroupStatusAudit(IMailMessageGroupStatusAudit item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMessageGroupStatus.RemoveMailMessageGroupStatusAudit(IMailMessageGroupStatusAudit item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
