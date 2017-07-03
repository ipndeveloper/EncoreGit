using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for SupportTicketCategory.
	/// </summary>
	[ContractClass(typeof(Contracts.SupportTicketCategoryContracts))]
	public interface ISupportTicketCategory
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SupportTicketCategoryID for this SupportTicketCategory.
		/// </summary>
		short SupportTicketCategoryID { get; set; }
	
		/// <summary>
		/// The Name for this SupportTicketCategory.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this SupportTicketCategory.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this SupportTicketCategory.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this SupportTicketCategory.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The SortIndex for this SupportTicketCategory.
		/// </summary>
		short SortIndex { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The SupportTickets for this SupportTicketCategory.
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
		[ContractClassFor(typeof(ISupportTicketCategory))]
		internal abstract class SupportTicketCategoryContracts : ISupportTicketCategory
		{
		    #region Primitive properties
		
			short ISupportTicketCategory.SupportTicketCategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicketCategory.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicketCategory.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicketCategory.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISupportTicketCategory.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ISupportTicketCategory.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ISupportTicket> ISupportTicketCategory.SupportTickets
			{
				get { throw new NotImplementedException(); }
			}
		
			void ISupportTicketCategory.AddSupportTicket(ISupportTicket item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ISupportTicketCategory.RemoveSupportTicket(ISupportTicket item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
