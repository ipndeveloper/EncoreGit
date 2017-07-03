using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes
{
    public class ProductCreditPayment : IPaymentType
    {
        public bool IsMatch(int paymentType)
        {
            return paymentType == (int)ConstantsGenerated.PaymentType.ProductCredit;
        }

        public IPayment PaymentMethod(PaymentTypeModel model)
        {
            return new NonAccountPaymentMethod()
                       {
                           PaymentTypeID = model.PaymentType
                       };
        }

    }
}