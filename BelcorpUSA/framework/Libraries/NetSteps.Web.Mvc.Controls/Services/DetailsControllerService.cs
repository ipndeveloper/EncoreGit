using System;
using System.Collections.Generic;

using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.HelperObjects.OrderPackages;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Controls.Services.Interfaces;

namespace NetSteps.Web.Mvc.Controls.Services
{
    [ContainerRegister(typeof(IDetailsControllerService), RegistrationBehaviors.Default)]
    public class DetailsControllerService : IDetailsControllerService
    {

        public virtual IPaymentMethodModel GetPaymentMethodModel(OrderPayment payment)
        {
            if (payment.PaymentTypeID == (short)Constants.PaymentType.EFT)
                return GetEftMethodModel(payment);

            return GetDefaultMethodModel(payment);
        }


        public virtual DefaultPaymentMethodModalModel GetDefaultMethodModel(OrderPayment payment)
        {
            var model = new DefaultPaymentMethodModalModel
            {
                DecryptedAccountNumber = payment.DecryptedAccountNumber,
                ExpirationDate = payment.ExpirationDate,
                BillingName = payment.NameOnCard,
                BillingAddress1 = payment.BillingAddress1,
                BillingCity = payment.BillingCity,
                BillingState = payment.BillingState,
                BillingPostalCode = payment.BillingPostalCode,
                BillingCountryId = payment.BillingCountryID,
                TransactionId = payment.TransactionID
            };
            return model;
        }

        public virtual EFTPaymentMethodModalModel GetEftMethodModel(OrderPayment payment)
        {
            var model = new EFTPaymentMethodModalModel
            {
                DecryptedAccountNumber = payment.DecryptedAccountNumber,
                BankName = payment.BankName,
                RoutingNumber = payment.RoutingNumber,
                AccountType = payment.BankAccountTypeID > 0 ? BankAccountType.Load(payment.BankAccountTypeID.ToShort()).Name : "",
                BillingName = payment.NameOnCard,
                BillingAddress1 = payment.BillingAddress1,
                BillingCity = payment.BillingCity,
                BillingState = payment.BillingState,
                BillingPostalCode = payment.BillingPostalCode,
                BillingCountryId = payment.BillingCountryID
            };
            return model;
        }

        protected IEnumerable<OrderPackageInfoModel> PackageInfoModels(OrderShipment shipment)
        {
            var packageInfoModels = new List<OrderPackageInfoModel>();

            if (shipment.OrderShipmentPackages.Count == 0)
            {
                packageInfoModels.Add(new OrderPackageInfoModel
                {
                    ShipMethodName = shipment.ShippingMethodName,
                    BaseTrackUrl = shipment.TrackingURL,
                    TrackingNumber = shipment.TrackingNumber,
                    ShipDate = shipment.DateShipped ?? System.DateTime.MinValue
                });
            }
            else
            {
                AddAllPackagesToTheList(shipment, packageInfoModels);
            }

            return packageInfoModels;
        }

        public virtual BasicResponse DisallowAutoshipTemplateEdits(Order order)
        {
            BasicResponse response = new BasicResponse() { Success = true };

            if (order.OrderTypeID == (int)Constants.OrderType.AutoshipTemplate)
            {
                response.Success = false;
                response.Message = Translation.GetTerm("CannotEditAutoshipTemplate", "Cannot edit Autoship template");
            }

            return response;
        }

        protected void AddAllPackagesToTheList(OrderShipment shipment, IList<OrderPackageInfoModel> packageInfoModels)
        {
            foreach (var package in shipment.OrderShipmentPackages)
            {
                var packageItem = GetOrderPackageInformation(package);
                packageInfoModels.Add(packageItem);
            }
        }

        protected virtual OrderPackageInfoModel GetOrderPackageInformation(OrderShipmentPackage package)
        {
            var model = new OrderPackageInfoModel();
            model.ShipMethodName = package.ShippingMethodName;
            model.ShipDate = package.DateShipped;
            model.TrackingNumber = package.TrackingNumber;
            if (package.ShippingMethodID != null)
                model.BaseTrackUrl = OrderShipment.GetBaseTrackUrl(package.ShippingMethodID.Value, package.TrackingNumber);

            return model;
        }

        public Order LoadOrder(string id)
        {
            return Order.LoadByOrderNumberFull(id);
        }

        public bool IsOrderNotComplete(Order order)
        {
            return (order.OrderStatusID == (short)Constants.OrderStatus.CreditCardDeclined || order.OrderStatusID == (short)Constants.OrderStatus.PartiallyPaid);
        }

        public bool IsPartyOrder(Order order)
        {
            return Party.IsParty(order.OrderID);
        }

        public OrderPayment LoadPayment(int paymentId)
        {
            return OrderPayment.LoadFull(paymentId);
        }

        public Order ChangeCommissionConsultant(Order order, int commissionConsultantID)
        {
            order.ConsultantID = commissionConsultantID;
            return order;
        }

        public Order ChangeAttachedParty(Order order, int newPartyOrderID)
        {
            order.ParentOrderID = newPartyOrderID;
            return order;
        }

        public Order ChangeCommissionDate(Order order, DateTime commissionDate)
        {
            order.CommissionDate = commissionDate;
            return order;
        }
    }
}