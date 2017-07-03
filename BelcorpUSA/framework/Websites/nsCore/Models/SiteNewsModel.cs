using System.Collections.Generic;
using NetSteps.Data.Entities;

namespace nsCore.Models
{
	public class SiteNewsModel
	{
		public int SiteID { get; set; }
		public List<News> News { get; set; }
	}
}