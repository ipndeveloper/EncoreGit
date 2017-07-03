using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(ICalculationOverride), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class CalculationOverride : ICalculationOverride
	{

		public ICalculationKind CalculationKind { get; set; }

		public int CalculationOverrideId { get; set; }

		public decimal NewValue { get; set; }

		public IPeriod Period { get; set; }

		public int AccountId { get; set; }

		public int ApplicationSourceId { get; set; }

		public DateTime CreatedDateUTC { get; set; }

		public bool IsEditable
		{
			get
			{
				return (Period != null && Period.IsOpen);
			}
		}

		public string Notes { get; set; }

		public IOverrideKind OverrideKind { get; set; }

		public IOverrideReason OverrideReason { get; set; }

		public DateTime UpdatedDateUTC { get; set; }

		public int UserId { get; set; }

		public bool OverrideIfNull { get; set; }
	}
}
