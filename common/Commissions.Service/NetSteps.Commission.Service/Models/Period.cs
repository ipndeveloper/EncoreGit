using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [Serializable]
	public class Period : IPeriod
	{
		public string Description { get; set; }

		public DisbursementFrequencyKind DisbursementFrequency { get; set; }

		public DateTime EndDateUTC { get; set; }

		public bool IsOpen { get; set; }

		public int PeriodId { get; set; }

		public DateTime StartDateUTC { get; set; }

		public DateTime? ClosedDateUTC { get; set; }
	}
}
