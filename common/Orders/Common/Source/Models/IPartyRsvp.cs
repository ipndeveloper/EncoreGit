using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for PartyRsvp.
	/// </summary>
	[ContractClass(typeof(Contracts.PartyRsvpContracts))]
	public interface IPartyRsvp
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PartyRsvpID for this PartyRsvp.
		/// </summary>
		int PartyRsvpID { get; set; }
	
		/// <summary>
		/// The PartyID for this PartyRsvp.
		/// </summary>
		int PartyID { get; set; }
	
		/// <summary>
		/// The IsComing for this PartyRsvp.
		/// </summary>
		bool IsComing { get; set; }
	
		/// <summary>
		/// The PartyGuestID for this PartyRsvp.
		/// </summary>
		int PartyGuestID { get; set; }
	
		/// <summary>
		/// The ResponseDateUTC for this PartyRsvp.
		/// </summary>
		System.DateTime ResponseDateUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Party for this PartyRsvp.
		/// </summary>
	    IParty Party { get; set; }
	
		/// <summary>
		/// The PartyGuest for this PartyRsvp.
		/// </summary>
	    IPartyGuest PartyGuest { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPartyRsvp))]
		internal abstract class PartyRsvpContracts : IPartyRsvp
		{
		    #region Primitive properties
		
			int IPartyRsvp.PartyRsvpID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPartyRsvp.PartyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IPartyRsvp.IsComing
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPartyRsvp.PartyGuestID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IPartyRsvp.ResponseDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IParty IPartyRsvp.Party
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IPartyGuest IPartyRsvp.PartyGuest
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
