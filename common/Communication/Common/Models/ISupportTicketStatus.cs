using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for SupportTicketStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.SupportTicketStatusContracts))]
	public interface ISupportTicketStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SupportTicketStatusID for this SupportTicketStatus.
		/// </summary>
		short SupportTicketStatusID { get; set; }
	
		/// <summary>
		/// The Name for this SupportTicketStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this SupportTicketStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this SupportTicketStatus.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this SupportTicketStatus.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The EmailTemplateID for this SupportTicketStatus.
		/// </summary>
		Nullable<int> EmailTemplateID { get; set; }
	
		/// <summary>
		/// The SortIndex for this SupportTicketStatus.
		/// </summary>
		short SortIndex { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The SupportTickets for this SupportTicketStatus.
		/// </summary>
		IEnumerable<ISupportTicket> SupportTickets { get; }
	
		/// <summary>
		/// Adds an <see cref="ISupportTicket"/> to the SupportTickets collection.
		/// </summary>
		/// <param name="item">The <see cref="ISupportTicket"/> to add.</param>
		void AddSupportTicket(ISupportTicket item);
	
		/// <summary>
		/// Removes an <see cref="ISupportTicket"/> from the SupportTickets collection.
		/// </summary>
		/// <param name="item">The <see cref="ISupportTicket"/> to remove.</param>
		void RemoveSupportTicket(ISupportTicket item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISupportTicketStatus))]
		internal abstract class SupportTicketStatusContracts : ISupportTicketStatus
		{
		    #region Primitive properties
		
			short ISupportTicketStatus.SupportTicketStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicketStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicketStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicketStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISupportTicketStatus.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISupportTicketStatus.EmailTemplateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ISupportTicketStatus.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ISupportTicket> ISupportTicketStatus.SupportTickets
			{
				get { throw new NotImplementedException(); }
			}
		
			void ISupportTicketStatus.AddSupportTicket(ISupportTicket item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ISupportTicketStatus.RemoveSupportTicket(ISupportTicket item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
