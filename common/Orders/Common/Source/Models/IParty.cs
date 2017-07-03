using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for Party.
	/// </summary>
	[ContractClass(typeof(Contracts.PartyContracts))]
	public interface IParty
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PartyID for this Party.
		/// </summary>
		int PartyID { get; set; }
	
		/// <summary>
		/// The OrderID for this Party.
		/// </summary>
		int OrderID { get; set; }
	
		/// <summary>
		/// The AddressID for this Party.
		/// </summary>
		Nullable<int> AddressID { get; set; }
	
		/// <summary>
		/// The EmailTemplateID for this Party.
		/// </summary>
		Nullable<int> EmailTemplateID { get; set; }
	
		/// <summary>
		/// The Name for this Party.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The EviteOrganizerEmail for this Party.
		/// </summary>
		string EviteOrganizerEmail { get; set; }
	
		/// <summary>
		/// The UseEvites for this Party.
		/// </summary>
		bool UseEvites { get; set; }
	
		/// <summary>
		/// The ShowOnPWS for this Party.
		/// </summary>
		bool ShowOnPWS { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this Party.
		/// </summary>
		System.DateTime StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this Party.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The DataVersion for this Party.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this Party.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The ParentPartyID for this Party.
		/// </summary>
		Nullable<int> ParentPartyID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Address for this Party.
		/// </summary>
	    IAddress Address { get; set; }
	
		/// <summary>
		/// The Order for this Party.
		/// </summary>
	    IOrder Order { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The PartyGuests for this Party.
		/// </summary>
		IEnumerable<IPartyGuest> PartyGuests { get; }
	
		/// <summary>
		/// Adds an <see cref="IPartyGuest"/> to the PartyGuests collection.
		/// </summary>
		/// <param name="item">The <see cref="IPartyGuest"/> to add.</param>
		void AddPartyGuest(IPartyGuest item);
	
		/// <summary>
		/// Removes an <see cref="IPartyGuest"/> from the PartyGuests collection.
		/// </summary>
		/// <param name="item">The <see cref="IPartyGuest"/> to remove.</param>
		void RemovePartyGuest(IPartyGuest item);
	
		/// <summary>
		/// The PartyRsvps for this Party.
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
		[ContractClassFor(typeof(IParty))]
		internal abstract class PartyContracts : IParty
		{
		    #region Primitive properties
		
			int IParty.PartyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IParty.OrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IParty.AddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IParty.EmailTemplateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IParty.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IParty.EviteOrganizerEmail
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IParty.UseEvites
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IParty.ShowOnPWS
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IParty.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IParty.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IParty.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IParty.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IParty.ParentPartyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAddress IParty.Address
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IOrder IParty.Order
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IPartyGuest> IParty.PartyGuests
			{
				get { throw new NotImplementedException(); }
			}
		
			void IParty.AddPartyGuest(IPartyGuest item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IParty.RemovePartyGuest(IPartyGuest item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IPartyRsvp> IParty.PartyRsvps
			{
				get { throw new NotImplementedException(); }
			}
		
			void IParty.AddPartyRsvp(IPartyRsvp item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IParty.RemovePartyRsvp(IPartyRsvp item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
