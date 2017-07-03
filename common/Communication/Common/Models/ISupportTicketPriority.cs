using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for SupportTicketPriority.
	/// </summary>
	[ContractClass(typeof(Contracts.SupportTicketPriorityContracts))]
	public interface ISupportTicketPriority
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SupportTicketPriorityID for this SupportTicketPriority.
		/// </summary>
		short SupportTicketPriorityID { get; set; }
	
		/// <summary>
		/// The Name for this SupportTicketPriority.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this SupportTicketPriority.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this SupportTicketPriority.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The SortIndex for this SupportTicketPriority.
		/// </summary>
		short SortIndex { get; set; }
	
		/// <summary>
		/// The Active for this SupportTicketPriority.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The SupportTickets for this SupportTicketPriority.
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
		[ContractClassFor(typeof(ISupportTicketPriority))]
		internal abstract class SupportTicketPriorityContracts : ISupportTicketPriority
		{
		    #region Primitive properties
		
			short ISupportTicketPriority.SupportTicketPriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicketPriority.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicketPriority.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicketPriority.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ISupportTicketPriority.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISupportTicketPriority.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ISupportTicket> ISupportTicketPriority.SupportTickets
			{
				get { throw new NotImplementedException(); }
			}
		
			void ISupportTicketPriority.AddSupportTicket(ISupportTicket item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ISupportTicketPriority.RemoveSupportTicket(ISupportTicket item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
