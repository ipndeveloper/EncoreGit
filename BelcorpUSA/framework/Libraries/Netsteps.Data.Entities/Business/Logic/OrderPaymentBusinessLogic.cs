using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Validation.NetTiers;
using NetSteps.Data.Entities.Repositories;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class OrderPaymentBusinessLogic
    {
        public override void DefaultValues(IOrderPaymentRepository repository, OrderPayment entity)
        {
            entity.OrderPaymentStatusID = NetSteps.Data.Entities.Constants.OrderPaymentStatus.Pending.ToShort();
            entity.Guid = Guid.NewGuid();
        }

        public override void CleanDataBeforeSave(IOrderPaymentRepository repository, OrderPayment entity)
        {
            if (entity.BillingPostalCode == null)
            {
                entity.BillingPostalCode = string.Empty;
            }

            if (entity.BillingCountryID == 0)
            {
                entity.BillingCountryID = null;
            }

            if (entity.BillingStateProvinceID == 0)
            {
                entity.BillingStateProvinceID = null;
            }

            base.CleanDataBeforeSave(repository, entity);
        }

        #region IOrderPaymentBusinessLogic Members

        public virtual string GetDisplayName(OrderPayment orderPayment)
        {
            string result = string.Empty;

            switch ((Constants.PaymentType)orderPayment.PaymentTypeID)
            {
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.PaymentType.GiftCard:
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.PaymentType.EFT:
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.PaymentType.CreditCard:
                    result = orderPayment.DecryptedAccountNumber.MaskString(4);
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.PaymentType.Check:
                    result = Translation.GetTerm("Check", "Check");
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.PaymentType.Cash:
                    result = Translation.GetTerm("Cash", "Cash");
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.PaymentType.ProductCredit:
                    result = Translation.GetTerm("ProductCredit", "Product Credit");
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.PaymentType.EnrollmentCredit:
                    result = Translation.GetTerm("EnrollmentCredit", "Enrollment Credit");
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.PaymentType.InstantCommission:
                    result = Translation.GetTerm("InstantCommission", "Instant Commission");
                    break;
                default:
                    result = orderPayment.AccountNumber;
                    break;
            }

            return result;
        }

        #endregion

        public virtual BasicResponse IsPaymentValidForAuthorization(IOrderPaymentRepository repository, OrderPayment entity)
        {
            BasicResponse basicResponse = new BasicResponse()
            {
                Success = true
            };
            return basicResponse;
        }

        public IEnumerable<IOrderPayment> FilterByNachaClassType(IOrderPaymentRepository repository, string nachaClassType)
        {
            return repository.GetUnSubmittedOrderPaymentsByClassType(nachaClassType);
        }

        public IEnumerable<OrderPayment> FilterByNachaClassTypeAndCountryID(IOrderPaymentRepository repository, string nachaClassType, int countryID)
        {
            return repository.GetUnSubmittedOrderPaymentsByClassTypeAndCountryID(nachaClassType, countryID);
        }

        public IEnumerable<IOrderPayment> FilterByDateAndNachaClassType(IOrderPaymentRepository repository, DateTime startDate, DateTime endDate, string nachaClassType)
        {
            var result = FilterByNachaClassType(repository, nachaClassType);
            result = result.Where(a => a.ProcessOnDateUTC > startDate.Date && a.ProcessOnDateUTC < endDate.Date);
            return result;
        }

        public IOrderPayment LoadOrderPaymentByPaymentId(IOrderPaymentRepository repository, int orderPaymentId)
        {
            var result = repository.LoadOrderPaymentByOrderPaymentId(orderPaymentId);
            return result;
        }

        public IOrderPayment LoadOrderPaymentByOrderId(IOrderPaymentRepository repository, int orderId)
        {
            var result = repository.LoadOrderPaymentByOrderPaymentId(orderId);
            return result;
        }

        public override void AddValidationRules(OrderPayment entity)
        {
            entity.ValidationRules.AddRule(Address.IsCountryIdOnAddressValid, new ValidationRuleArgs("OrderPaymentID", "Invalid CountryID"));
        }
    }
}
