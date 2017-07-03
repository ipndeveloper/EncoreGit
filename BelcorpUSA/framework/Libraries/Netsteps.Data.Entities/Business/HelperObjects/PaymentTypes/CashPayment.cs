using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes
{
    public class CashPayment : IPaymentType
    {
        public bool IsMatch(int paymentType)
        {
            return paymentType == ConstantsGenerated.PaymentType.Cash.ToInt();
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