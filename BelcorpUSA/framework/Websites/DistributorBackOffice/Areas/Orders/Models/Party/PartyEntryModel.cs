using DistributorBackOffice.Models;
using NetSteps.Addresses.Common.Models;
using DataEntities = NetSteps.Data.Entities;

namespace DistributorBackOffice.Areas.Orders.Models.Party
{
	public class PartyEntryModel
	{
		public DataEntities.Account HostAccount { get; set; }
		public DataEntities.Party Party { get; set; }
		public DataEntities.OrderCustomer Host { get; set; }
		public PartyLocation? PartyLocation { get; set; }
		public PartyShipTo? ShipTo { get; set; }
		public IAddress ConsultantShippingAddress { get; set; }
		public IAddress HostAddress { get; set; }
		public IAddress ShippingAddress { get; set; }
		public bool HasHostInviteContent { get; set; }
		public bool HostEmailExists { get; set; }
		public bool HostHasAddress { get; set; }
	}
}