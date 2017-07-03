using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;

namespace NetSteps.Data.Entities.Business.HelperObjects.PaymentTypes
{
    /// <summary>
    /// CLIENTS: Extent this class and override method 'ClientSpecificPaymentTypes' to inject
    /// additional client payment types on top of Encore supported ones.
    /// </summary>
    public class PaymentTypeProvider : IPaymentTypeProvider
    {
        private readonly List<IPaymentType> _registeredPaymentTypes;

        public PaymentTypeProvider()
        {
            _registeredPaymentTypes = RegisteredPaymentMethods();
        }

        public List<IPaymentType> RegisteredPaymentMethods()
        {
            var paymentTypes = GetBasePaymentTypes();

            var clientPaymentTypes = ClientSpecificPaymentTypes();

            if (clientPaymentTypes.Any())
                paymentTypes.AddRange(clientPaymentTypes);

            return paymentTypes;
        }

        private List<IPaymentType> GetBasePaymentTypes()
        {
            return (from type in Assembly.GetExecutingAssembly().GetTypes()
                    where type.GetInterface(typeof(IPaymentType).ToString()) != null
                    select CreateInstance(type)).ToList();
        }

        /// <summary>
        /// NOTE: Override this method to add-in client specific payment types.
        /// </summary>
        public virtual List<IPaymentType> ClientSpecificPaymentTypes()
        {
            return new List<IPaymentType>();
        }

        public IPaymentType CreateInstance(Type paymentName)
        {
            Type t = paymentName;

            return Activator.CreateInstance(t) as IPaymentType;
        }

        public IPayment GetPaymentMethod(PaymentTypeModel paymentTypeModel)
        {
            IPaymentType type = _registeredPaymentTypes.FirstOrDefault(p => p.IsMatch(paymentTypeModel.PaymentType));

            if (type == null)
                throw new Exception(Translation.GetTerm("InvalidPaymentType", "Invalid payment type"));

            return type.PaymentMethod(paymentTypeModel);
        }
    }
}
