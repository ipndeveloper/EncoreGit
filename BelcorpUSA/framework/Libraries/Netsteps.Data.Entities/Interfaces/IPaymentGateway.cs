using NetSteps.Common.Base;
using NetSteps.Data.Entities.Base;

namespace NetSteps.Data.Entities.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 4/13/2010
    /// </summary>
    public interface IPaymentGateway
    {
        PaymentAuthorizationResponse Charge(OrderPayment orderPayment);
        PaymentAuthorizationResponse Refund(OrderPayment orderPayment, decimal ammount);
        BasicResponse ValidateCharge(OrderPayment orderPayment, ref decimal currentBalance);
        BasicResponse ValidateRefund(OrderPayment orderPayment, ref decimal currentBalance);
    }
}
