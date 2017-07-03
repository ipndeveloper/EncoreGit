using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Claims.Common
{
	public struct ClaimSet
	{
		public int Id { get; set; }
		public IEnumerable<Claim> Claims { get; set; }
		public string Name { get; set; }
	}
}
