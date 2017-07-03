
namespace DistributorBackOffice.Areas.Orders.Models.Party
{
	public class PartyConfirmationModel
	{
		public int PartyId { get; set; }
		public int HostAccountId { get; set; }
		public string PartyName { get; set; }
		public string HostName { get; set; }
		public string PartyTime { get; set; }
		public string PartyAddress { get; set; }
		public bool IsNewParty { get; set; }
	}
}