using System.Collections.Generic;

namespace NetSteps.WebService.Mobile.Models
{
	public class PartyDetailModel
	{
		public string address1;
		public string address2;
		public string postalCode;
		public string city;
		public string state;
		public string date;
		public string time;
		public int inviteCount;
		public int guestsAttending;
		public int guestsNotAttending;
		public string onlinePurchases;
		public List<ContactModel> guests;
	}
}