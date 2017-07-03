using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelcorpUSA.Edi.Service.Data
{
	public class EdiCanceledOrderTracker
	{
		public int EdiCanceledOrderTrackerId { get; set; }
		public int OrderId { get; set; }
		public int? EdiInterchangeTrackerId { get; set; }
	}
}
