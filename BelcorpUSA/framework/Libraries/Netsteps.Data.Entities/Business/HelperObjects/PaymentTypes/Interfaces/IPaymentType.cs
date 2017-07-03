using NetSteps.Data.Entities.Business.Interfaces;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces
{
    public interface IPaymentType
    {
        IPayment PaymentMethod(PaymentTypeModel model);

        bool IsMatch(int paymentType);
    }
}
