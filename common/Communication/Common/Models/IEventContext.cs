using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EventContext.
	/// </summary>
	[ContractClass(typeof(Contracts.EventContextContracts))]
	public interface IEventContext
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EventContextID for this EventContext.
		/// </summary>
		int EventContextID { get; set; }
	
		/// <summary>
		/// The OrderID for this EventContext.
		/// </summary>
		Nullable<int> OrderID { get; set; }
	
		/// <summary>
		/// The AccountID for this EventContext.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The PartyID for this EventContext.
		/// </summary>
		Nullable<int> PartyID { get; set; }
	
		/// <summary>
		/// The SupportTicketID for this EventContext.
		/// </summary>
		Nullable<int> SupportTicketID { get; set; }
	
		/// <summary>
		/// The NewsID for this EventContext.
		/// </summary>
		Nullable<int> NewsID { get; set; }
	
		/// <summary>
		/// The IncentiveID for this EventContext.
		/// </summary>
		Nullable<int> IncentiveID { get; set; }
	
		/// <summary>
		/// The ReferenceAccountID for this EventContext.
		/// </summary>
		Nullable<int> ReferenceAccountID { get; set; }
	
		/// <summary>
		/// The PromotionID for this EventContext.
		/// </summary>
		Nullable<int> PromotionID { get; set; }
	
		/// <summary>
		/// The TitleID for this EventContext.
		/// </summary>
		Nullable<int> TitleID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEventContext))]
		internal abstract class EventContextContracts : IEventContext
		{
		    #region Primitive properties
		
			int IEventContext.EventContextID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEventContext.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEventContext.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEventContext.PartyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEventContext.SupportTicketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEventContext.NewsID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEventContext.IncentiveID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEventContext.ReferenceAccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEventContext.PromotionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEventContext.TitleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
