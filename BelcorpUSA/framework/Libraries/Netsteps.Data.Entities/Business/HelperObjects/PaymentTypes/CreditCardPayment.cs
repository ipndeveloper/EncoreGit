using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes
{
    public class CreditCardPayment : IPaymentType
    {

        public bool IsMatch(int paymentType)
        {
            return paymentType == ConstantsGenerated.PaymentType.CreditCard.ToInt();
        }

        public IPayment PaymentMethod(PaymentTypeModel model)
        {
            IPayment payment = new Payment()
                                   {
                                       DecryptedAccountNumber = model.AccountNumber.RemoveNonNumericCharacters(),
                                       CVV = model.Cvv,
                                       ExpirationDate = model.ExpirationDate.Value.LastDayOfMonth(),
                                       NameOnCard = model.NameOnCard
                                   };

            if (payment.BillingAddress == null)
                payment.BillingAddress = new Address();

            return payment;
        }
    }
}