using System;
using System.Collections.Generic;
using NetSteps.Orders.Common.Models;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Events.Service.Tests.Mocks
{
	public class FakeParty : IParty
	{
		public void AddPartyGuest(IPartyGuest item)
		{
			throw new NotImplementedException();
		}

		public void RemovePartyGuest(IPartyGuest item)
		{
			throw new NotImplementedException();
		}

		public void AddPartyRsvp(IPartyRsvp item)
		{
			throw new NotImplementedException();
		}

		public void RemovePartyRsvp(IPartyRsvp item)
		{
			throw new NotImplementedException();
		}

		public int PartyID { get; set; }
		public int OrderID { get; set; }
		public int? AddressID { get; set; }
		public int? EmailTemplateID { get; set; }
		public string Name { get; set; }
		public string EviteOrganizerEmail { get; set; }
		public bool UseEvites { get; set; }
		public bool ShowOnPWS { get; set; }
		public DateTime StartDateUTC { get; set; }
		public DateTime? EndDateUTC { get; set; }
		public byte[] DataVersion { get; set; }
		public int? ModifiedByUserID { get; set; }
		public int? ParentPartyID { get; set; }

		public IAddress Address
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		private IOrder order;
		public IOrder Order
		{
			get
			{
				if(order == null)
				{
					order = new FakeOrder();
				}
				return order;
			}
			set { order = value; }
		}
		public IEnumerable<IPartyGuest> PartyGuests { get; set; }
		public IEnumerable<IPartyRsvp> PartyRsvps { get; set; }
	}
}
