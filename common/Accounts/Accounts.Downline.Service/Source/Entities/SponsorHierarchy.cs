using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using NetSteps.Accounts.Downline.Service.Hierarchy;

namespace NetSteps.Accounts.Downline.Service.Entities
{
	[Table("SponsorHierarchy", Schema = "Accounts")]
	public class SponsorHierarchy : INestedSet
	{
		public int AccountId { get; set; }
		
		public int? SponsorId { get; set; }
		
		public int TreeLevel { get; set; }
		
		[Key, Column(Order = 3)]
		public int LeftAnchor { get; set; }

		[Key, Column(Order = 4)]
		public int RightAnchor { get; set; }
		
		public int NodeNumber { get; set; }
		
		public int NodeCount { get; set; }
		
		public byte[] Upline { get; set; }
	}
}
