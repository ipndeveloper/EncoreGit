using NetSteps.OrderAdjustments.Common;

namespace NetSteps.NonCommissionablePaymentTypeProvider.Common
{
	public interface INonCommissionablePaymentOrderAdjustmentProvider : IOrderAdjustmentProvider
	{
	}

	public sealed class NonCommissionablePaymentOrderAdjustmentProviderInfo
	{
		public const string OrderAdjustmentProviderKey = "78D19036-05C3-4B34-B7A0-64A975346C01";
	}
}
