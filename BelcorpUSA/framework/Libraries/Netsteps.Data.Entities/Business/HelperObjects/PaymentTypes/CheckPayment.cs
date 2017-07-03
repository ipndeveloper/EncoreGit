using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Generated;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes
{
    public class CheckPayment : IPaymentType
    {
        public bool IsMatch(int paymentType)
        {
            return paymentType == ConstantsGenerated.PaymentType.Check.ToInt();
        }

        public IPayment PaymentMethod(PaymentTypeModel model)
        {
            return new Payment()
            {
                DecryptedAccountNumber = model.AccountNumber.RemoveNonNumericCharacters(),
                PaymentType = (Constants.PaymentType)model.PaymentType
            };
        }


    }
}
