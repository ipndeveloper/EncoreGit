using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IDisbursementHold), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
    public class DisbursementHold : IDisbursementHold
	{
		public int AccountId { get; set; }

		public int? ApplicationSourceId { get; set; }

        public int DisbursementHoldId { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime? HoldUntil { get; set; }

		public string Notes { get; set; }

		public IOverrideReason Reason { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime UpdatedDate { get; set; }

		public int UserId { get; set; }
    }
}
