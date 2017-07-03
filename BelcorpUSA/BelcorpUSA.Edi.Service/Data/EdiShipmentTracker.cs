using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelcorpUSA.Edi.Service.Data
{
	public class EdiShipmentTracker
	{
		public int EdiShipmentTrackerId { get; set; }
		public int EdiInterchangeTrackerId { get; set; }
		public int OrderId { get; set; }
		public int OrderShipmentId { get; set; }
	}
}
