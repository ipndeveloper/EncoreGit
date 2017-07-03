namespace NetSteps.NonCommissionablePaymentTypeProvider.Service
{
	using System.Collections.Generic;

	using NetSteps.NonCommissionablePaymentTypeProvider.Common;
	using NetSteps.OrderAdjustments.Common.Model;

	public class NonCommissionablePaymentOrderAdjustmentProfile : INonCommissionablePaymentOrderAdjustmentProfile
	{
		public NonCommissionablePaymentOrderAdjustmentProfile()
		{
			this.OrderModifications = new List<IOrderAdjustmentProfileOrderModification>();
			this.OrderLineModificationTargets = new List<IOrderAdjustmentProfileOrderItemTarget>();
			this.AddedOrderSteps = new List<IOrderAdjustmentOrderStep>();
			this.AffectedAccountIDs = new List<int>();
		}

		public ICollection<int> AffectedAccountIDs { get; private set; }

		public IList<IOrderAdjustmentProfileOrderModification> OrderModifications { get; private set; }

		public IList<IOrderAdjustmentProfileOrderItemTarget> OrderLineModificationTargets { get; private set; }

		public IList<IOrderAdjustmentOrderStep> AddedOrderSteps { get; private set; }

		public string ExtensionProviderKey { get; set; }

		public string Description { get; set; }
	}
}
