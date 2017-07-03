using NetSteps.Data.Entities.Business.Interfaces;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces
{
    public interface IPaymentTypeProvider
    {
        IPayment GetPaymentMethod(PaymentTypeModel paymentTypeModel);
    }
}
