using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes
{
    public class EFTPayment : IPaymentType
    {
        short BankAccountTypeID;
        public bool IsMatch(int paymentType)
        {
            return paymentType == ConstantsGenerated.PaymentType.EFT.ToInt();
        }

        public IPayment PaymentMethod(PaymentTypeModel model)
        {
            IPayment payment = new Payment()
            {
                DecryptedAccountNumber = model.BankAccountNumber.RemoveNonNumericCharacters(),
                NameOnCard = model.NameOnAccount,
                RoutingNumber = model.RoutingNumber.RemoveNonNumericCharacters(),
                BankName = model.BankName,
                BankAccountTypeID = model.BankAccountTypeID,
                PaymentType = ConstantsGenerated.PaymentType.EFT
            };

            BankAccountTypeID = model.BankAccountTypeID;

            if (payment.BillingAddress == null)
                payment.BillingAddress = new Address();

            return payment;
        }
    }
}