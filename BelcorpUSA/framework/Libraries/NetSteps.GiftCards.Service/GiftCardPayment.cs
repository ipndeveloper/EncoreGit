using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Generated;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes;
using NetSteps.Data.Entities.Business.Interfaces;

namespace NetSteps.GiftCards.Service
{
    public class GiftCardPayment
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