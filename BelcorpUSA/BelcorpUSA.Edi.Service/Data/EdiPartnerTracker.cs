using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelcorpUSA.Edi.Service.Data
{
	public class EdiPartnerTracker
	{
		public int EdiPartnerTrackerId { get; set; }
		public int EdiInterchangeTrackerId { get; set; }
		public string PartnerName { get; set; }
		public string FilePath { get; set; }
		public bool Confirmed { get; set; }
	}
}
