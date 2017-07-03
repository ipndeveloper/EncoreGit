using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for PartyGuest.
	/// </summary>
	[ContractClass(typeof(Contracts.PartyGuestContracts))]
	public interface IPartyGuest
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PartyGuestID for this PartyGuest.
		/// </summary>
		int PartyGuestID { get; set; }
	
		/// <summary>
		/// The PartyID for this PartyGuest.
		/// </summary>
		int PartyID { get; set; }
	
		/// <summary>
		/// The AccountID for this PartyGuest.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The FirstName for this PartyGuest.
		/// </summary>
		string FirstName { get; set; }
	
		/// <summary>
		/// The LastName for this PartyGuest.
		/// </summary>
		string LastName { get; set; }
	
		/// <summary>
		/// The EmailAddress for this PartyGuest.
		/// </summary>
		string EmailAddress { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Party for this PartyGuest.
		/// </summary>
	    IParty Party { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The PartyRsvps for this PartyGuest.
		/// </summary>
		IEnumerable<IPartyRsvp> PartyRsvps { get; }
	
		/// <summary>
		/// Adds an <see cref="IPartyRsvp"/> to the PartyRsvps collection.
		/// </summary>
		/// <param name="item">The <see cref="IPartyRsvp"/> to add.</param>
		void AddPartyRsvp(IPartyRsvp item);
	
		/// <summary>
		/// Removes an <see cref="IPartyRsvp"/> from the PartyRsvps collection.
		/// </summary>
		/// <param name="item">The <see cref="IPartyRsvp"/> to remove.</param>
		void RemovePartyRsvp(IPartyRsvp item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPartyGuest))]
		internal abstract class PartyGuestContracts : IPartyGuest
		{
		    #region Primitive properties
		
			int IPartyGuest.PartyGuestID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPartyGuest.PartyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IPartyGuest.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPartyGuest.FirstName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPartyGuest.LastName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IPartyGuest.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IParty IPartyGuest.Party
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IPartyRsvp> IPartyGuest.PartyRsvps
			{
				get { throw new NotImplementedException(); }
			}
		
			void IPartyGuest.AddPartyRsvp(IPartyRsvp item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IPartyGuest.RemovePartyRsvp(IPartyRsvp item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
