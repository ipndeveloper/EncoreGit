using System;
using System.Collections.Concurrent;
using System.Linq;
using NetSteps.Common.Configuration;
using NetSteps.Common.Expressions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.PaymentGateways
{
    public class Payments
    {
        #region Members
        private static IPaymentGateway _fakePaymentGateway;
        public static IPaymentGateway FakePaymentGateway
        {
            get
            {
                if (_fakePaymentGateway == null)
                {
                    PaymentGatewaySection _paymentGatewaySection = new PaymentGatewaySection();

                    _fakePaymentGateway = new FakePaymentGateway(_paymentGatewaySection);
                }

                return _fakePaymentGateway;
            }
            set { _fakePaymentGateway = value; }
        }

        private static readonly ConcurrentDictionary<string, ExpressionHelper.ObjectActivator<IPaymentGateway>> _paymentGateways = new ConcurrentDictionary<string, ExpressionHelper.ObjectActivator<IPaymentGateway>>();
        #endregion

        #region Properties
        public static bool UseFakePaymentGateway { get; set; }
        #endregion

        #region Methods
        private static IPaymentGateway GetPaymentGateway(string gatewayType, PaymentGatewaySection paymentGatewaySection)
        {
            // the lambda below is vital to keep this performant - trotter
            var objectActivator = _paymentGateways.GetOrAdd(gatewayType, key => ExpressionHelper.GetActivator<IPaymentGateway>(Type.GetType(key, true).GetConstructors().First()));
            return objectActivator(paymentGatewaySection);

            #region OLD VERSION
            // old version - very dangerous as the same instances were being shared for 
            // multiple concurrent order submissions, causing payment into to be shared between 
            // accounts (the only record of which would be on their next bank statement) - trotter

            //if (!_paymentGateways.ContainsKey(gatewayType))
            //{
            //    Type type = Type.GetType(gatewayType, true);
            //    IPaymentGateway gateway = Activator.CreateInstance(type) as IPaymentGateway;
            //    _paymentGateways.Add(gatewayType, gateway);
            //}

            //return _paymentGateways[gatewayType];
            #endregion
        }

        private static PaymentGateway GetGatewayType(Order order, OrderPayment orderPayment)
        {
            try
            {
                PaymentGateway paymentGateway = null;

                OrderShipment shipment = order.GetDefaultShipment();

                if (order.OrderTypeID == (int)Constants.OrderType.ReturnOrder)
                {
                    if (orderPayment.PaymentGatewayID.HasValue)
                    {
                        paymentGateway = (from g in SmallCollectionCache.Instance.PaymentGateways
                                          where g.PaymentGatewayID == orderPayment.PaymentGatewayID
                                          select g).FirstOrDefault();
                    }
                    else
                    {
                        paymentGateway = GetDefaultPaymentGateway(shipment, order, orderPayment);
                    }
                }
                else
                {
                    paymentGateway = GetDefaultPaymentGateway(shipment, order, orderPayment);
                }

                return paymentGateway;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsException);
            }
        }

        private static PaymentGateway GetDefaultPaymentGateway(OrderShipment shipment, Order order, OrderPayment orderPayment)
        {
            var gateway = (from t in SmallCollectionCache.Instance.PaymentOrderTypes
                           join g in SmallCollectionCache.Instance.PaymentGateways on t.PaymentGatewayID equals g.PaymentGatewayID
                           where t.OrderTypeID == order.OrderTypeID && t.CountryID == shipment.CountryID && t.PaymentTypeID == orderPayment.PaymentTypeID
                           select g).FirstOrDefault();

            return gateway;
        }

        public static IPaymentGateway GetPaymentGateway(Order order, OrderPayment orderPayment)
        {
            if (UseFakePaymentGateway)
                return Payments.FakePaymentGateway;
            else
            {
                var paymentGateway = GetGatewayType(order, orderPayment);

                if (paymentGateway == null)
                {
                    throw new Exception("Payment Gateway not found.");
                }

                var paymentGatewaySection = PaymentGatewaySections.Instance.FirstOrDefault(p => p.PaymentGatewayID == paymentGateway.PaymentGatewayID);

                if (paymentGatewaySection == null)
                {
                    paymentGatewaySection = PaymentGatewaySections.Instance.FirstOrDefault(p => p.Namespace == paymentGateway.Namespace);
                    if (paymentGatewaySection != null)
                    {
                        paymentGatewaySection.PaymentGatewayID = paymentGateway.PaymentGatewayID;
                    }
                }

                if (paymentGatewaySection == null)
                {
                    paymentGatewaySection = new PaymentGatewaySection()
                    {
                        PaymentGatewayID = paymentGateway.PaymentGatewayID,
                        Namespace = paymentGateway.Namespace
                    };
                }

                return GetPaymentGateway(paymentGateway.Namespace, paymentGatewaySection);
            }
        }
        #endregion
    }
}