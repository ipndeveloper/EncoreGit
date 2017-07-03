using System;
using System.Collections.Generic;
using NetSteps.Data.Entities;

namespace nsCore.Models
{
	[Serializable]
	public class CurrentUserMarketsModel
	{
		public int CorporateUserID { get; set; }
		public List<Market> Markets { get; set; }
	}
}