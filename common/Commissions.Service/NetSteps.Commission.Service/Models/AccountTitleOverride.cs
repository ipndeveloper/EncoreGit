using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using System;

namespace NetSteps.Commissions.Service.Models
{
    [ContainerRegister(typeof(IAccountTitleOverride), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest), Serializable]
	public class AccountTitleOverride : IAccountTitleOverride
	{
		public int AccountTitleOverrideId { get; set; }

		public ITitle OverrideTitle { get; set; }

        public ITitleKind OverrideTitleKind { get; set; }

		public IPeriod Period { get; set; }

		public ITitle Title { get; set; }

		public ITitleKind TitleKind { get; set; }

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

		public IOverrideReason OverrideReason { get; set; }

		public IOverrideKind OverrideKind { get; set; }

		public DateTime UpdatedDateUTC { get; set; }

		public int UserId { get; set; }

		#region Internal Properties

		internal int OverrideReasonId { get; set; }

		internal int OverrideKindId { get; set; }

		internal int OverrideTitleId { get; set; }

		internal int PeriodId { get; set; }

		internal int TitleId { get; set; }

		internal int TitleKindId { get; set; }

		#endregion
	}
}
