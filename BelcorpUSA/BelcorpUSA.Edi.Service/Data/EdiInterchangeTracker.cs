using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BelcorpUSA.Edi.Service.Data
{
	public class EdiInterchangeTracker
	{
		public int EdiInterchangeTrackerId { get; set; }
		public DateTime DateCreatedUtc { get; set; }
		public string OrderType { get; set; }

		public virtual ICollection<EdiShipmentTracker> EdiShipmentTrackers { get; set; }
		public virtual ICollection<EdiPartnerTracker> EdiPartnerTrackers { get; set; }

		public static EdiInterchangeTracker GetNewInterchangeTracking(string orderType)
		{
			EdiInterchangeTracker result = null;
			using (EdiTrackingDbContext ctx = new EdiTrackingDbContext())
			{
				result = ctx.EdiInterchangeTrackers.Create();
				result.DateCreatedUtc = DateTime.UtcNow;
				result.OrderType = orderType;
				ctx.EdiInterchangeTrackers.Add(result);
				ctx.SaveChanges();
			}
			return result;
		}

		public static DateTime GetLatestTrackingDate(string orderType, int excludeId = 0)
		{
			using (EdiTrackingDbContext ctx = new EdiTrackingDbContext())
			{
				return (from i in ctx.EdiInterchangeTrackers
						where i.OrderType == orderType && i.EdiInterchangeTrackerId != excludeId
						orderby i.EdiInterchangeTrackerId descending
						select i.DateCreatedUtc).FirstOrDefault();
			}
		}
	}
}
