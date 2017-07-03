using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Common.Models;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for SupportTicket.
	/// </summary>
	[ContractClass(typeof(Contracts.SupportTicketContracts))]
	public interface ISupportTicket
	{
	    #region Primitive properties
	
		/// <summary>
		/// The SupportTicketID for this SupportTicket.
		/// </summary>
		int SupportTicketID { get; set; }
	
		/// <summary>
		/// The SupportTicketNumber for this SupportTicket.
		/// </summary>
		string SupportTicketNumber { get; set; }
	
		/// <summary>
		/// The AccountID for this SupportTicket.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The AssignedUserID for this SupportTicket.
		/// </summary>
		Nullable<int> AssignedUserID { get; set; }
	
		/// <summary>
		/// The Title for this SupportTicket.
		/// </summary>
		string Title { get; set; }
	
		/// <summary>
		/// The Description for this SupportTicket.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The SupportTicketCategoryID for this SupportTicket.
		/// </summary>
		Nullable<short> SupportTicketCategoryID { get; set; }
	
		/// <summary>
		/// The SupportTicketPriorityID for this SupportTicket.
		/// </summary>
		short SupportTicketPriorityID { get; set; }
	
		/// <summary>
		/// The SupportTicketStatusID for this SupportTicket.
		/// </summary>
		short SupportTicketStatusID { get; set; }
	
		/// <summary>
		/// The IsVisibleToOwner for this SupportTicket.
		/// </summary>
		bool IsVisibleToOwner { get; set; }
	
		/// <summary>
		/// The CreatedByUserID for this SupportTicket.
		/// </summary>
		Nullable<int> CreatedByUserID { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this SupportTicket.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this SupportTicket.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The DateLastModifiedUTC for this SupportTicket.
		/// </summary>
		System.DateTime DateLastModifiedUTC { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Notes for this SupportTicket.
		/// </summary>
		IEnumerable<INote> Notes { get; }
	
		/// <summary>
		/// Adds an <see cref="INote"/> to the Notes collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to add.</param>
		void AddNote(INote item);
	
		/// <summary>
		/// Removes an <see cref="INote"/> from the Notes collection.
		/// </summary>
		/// <param name="item">The <see cref="INote"/> to remove.</param>
		void RemoveNote(INote item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISupportTicket))]
		internal abstract class SupportTicketContracts : ISupportTicket
		{
		    #region Primitive properties
		
			int ISupportTicket.SupportTicketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicket.SupportTicketNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISupportTicket.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISupportTicket.AssignedUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicket.Title
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ISupportTicket.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ISupportTicket.SupportTicketCategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ISupportTicket.SupportTicketPriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ISupportTicket.SupportTicketStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ISupportTicket.IsVisibleToOwner
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISupportTicket.CreatedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ISupportTicket.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ISupportTicket.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ISupportTicket.DateLastModifiedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<INote> ISupportTicket.Notes
			{
				get { throw new NotImplementedException(); }
			}
		
			void ISupportTicket.AddNote(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ISupportTicket.RemoveNote(INote item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
