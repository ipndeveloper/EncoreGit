using System;
using System.Linq;
using System.Diagnostics.Contracts;
using NetSteps.Data.Common.Context;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Base;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.Plugins.Common;
using NetSteps.Promotions.Common.ModelConcrete;
using NetSteps.Promotions.Plugins.Common.Qualifications.Concrete;
using NetSteps.Data.Common.Entities;
using System.Collections.Generic;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class CustomerPriceTypeTotalRangeQualificationHandler : BasePromotionQualificationHandler<ICustomerPriceTypeTotalRangeQualificationExtension, ICustomerPriceTypeTotalRangeQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, ICustomerPriceTypeTotalRangeQualificationHandler
    {
        public CustomerPriceTypeTotalRangeQualificationHandler()
        {
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.CustomerPriceTypeTotalRangeProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(typeof(ICustomerPriceTypeTotalRangeQualificationExtension).IsAssignableFrom(promotionQualification.GetType()));
            var qualification = (ICustomerPriceTypeTotalRangeQualificationExtension)promotionQualification;
            PromotionTypeConfigurationPerPromotionRepository promo = new PromotionTypeConfigurationPerPromotionRepository();

            //**********************************************************************
            //var first = promo.ExistsAndConditionQVTotal(promotion.PromotionID).First();
            //bool AndConditionQvTotal = first.Key;
            //decimal QvMin = first.Value.First().Key;
            //decimal QvMax = first.Value.First().Value;

            //**********************************************************************

            if (!qualification.CustomerPriceTypeTotalRangesByCurrencyID.ContainsKey(orderContext.Order.CurrencyID))
            {
                return PromotionQualificationResult.NoMatch;
            }
            Decimal acumuladoAnterior = 0;

            var PriceTypeTotalRange = qualification.CustomerPriceTypeTotalRangesByCurrencyID[orderContext.Order.CurrencyID];
            var customerIds = new List<int>();
            foreach (var orderCustomer in orderContext.Order.OrderCustomers)
            {
                //var priceTypeTotal = AndConditionQvTotal ? 
                //                     promo.GetVolumeByProductListPriceType(orderContext.Order.OrderID, promotion.PromotionID, qualification.ProductPriceTypeID) : 
                //                     GetPriceTypeTotal(orderCustomer, qualification.ProductPriceTypeID);
                var priceTypeTotal = GetPriceTypeTotal(orderCustomer, qualification.ProductPriceTypeID);

                if (promo.ExistePromotionType(promotion.PromotionID))
                {
                    acumuladoAnterior = promo.ObtenerValorAcumuladoAccountKPIs(qualification.ProductPriceTypeID, orderContext.Order.OrderCustomers.ElementAt(0).AccountID);
                    priceTypeTotal += acumuladoAnterior;
                }

                if (PriceTypeTotalRange.IsInRange(priceTypeTotal))
                {
                    customerIds.Add(orderCustomer.AccountID);
                }
            }

            return customerIds.Any() ? PromotionQualificationResult.MatchForSelectCustomers(customerIds) : PromotionQualificationResult.NoMatch;
        }

        private decimal GetPriceTypeTotal(IOrderCustomer orderCustomer, int priceType)
        {
            return orderCustomer.OrderItems.Sum(orderItem => orderItem.GetAdjustedPrice(priceType) * orderItem.Quantity);
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is ICustomerPriceTypeTotalRangeQualificationExtension);
            Contract.Assert(promotionQualification2 is ICustomerPriceTypeTotalRangeQualificationExtension);

            var extension1 = promotionQualification1 as ICustomerPriceTypeTotalRangeQualificationExtension;
            var extension2 = promotionQualification2 as ICustomerPriceTypeTotalRangeQualificationExtension;

            if (extension1.CustomerPriceTypeTotalRangesByCurrencyID.Keys.Except(extension2.CustomerPriceTypeTotalRangesByCurrencyID.Keys).Any())
                return false;
            if (extension2.CustomerPriceTypeTotalRangesByCurrencyID.Keys.Except(extension1.CustomerPriceTypeTotalRangesByCurrencyID.Keys).Any())
                return false;

            return extension1.CustomerPriceTypeTotalRangesByCurrencyID.Keys.All(key => extension1.CustomerPriceTypeTotalRangesByCurrencyID[key].IsEqualTo(extension2.CustomerPriceTypeTotalRangesByCurrencyID[key]));
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }

        public override void CheckValidity(string qualificationKey, ICustomerPriceTypeTotalRangeQualificationExtension qualification, IPromotionState state)
        {
            if (qualification.CustomerPriceTypeTotalRangesByCurrencyID == null || !qualification.CustomerPriceTypeTotalRangesByCurrencyID.Any())
            {
                state.AddConstructionError
                    (
                        String.Format("Promotion Qualification {0}", qualificationKey),
                        "Minimum PriceTypeTotals dictionary is null or empty."
                    );
            }
        }
    }
}
