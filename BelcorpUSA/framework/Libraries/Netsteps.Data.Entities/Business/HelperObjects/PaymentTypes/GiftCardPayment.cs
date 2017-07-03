using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes
{
    public class GiftCardPayment : IPaymentType
    {
        public bool IsMatch(int paymentType)
        {
            return paymentType == ConstantsGenerated.PaymentType.GiftCard.ToInt();
        }

        public IPayment PaymentMethod(PaymentTypeModel model)
        {
            return new NonAccountPaymentMethod()
            {
                DecryptedAccountNumber = model.AccountNumber ?? model.GiftCardCode,
                PaymentTypeID = model.PaymentType
            };
        }

    }
}