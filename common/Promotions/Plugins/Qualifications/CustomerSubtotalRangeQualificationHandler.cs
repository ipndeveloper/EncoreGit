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
using System.Data;
using System.Data.SqlClient;
using NetSteps.Common.Configuration;
using System.Configuration;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using NetSteps.Promotions.Plugins.EntityModel;

namespace NetSteps.Promotions.Plugins.Qualifications
{
    public class CustomerSubtotalRangeQualificationHandler : BasePromotionQualificationHandler<ICustomerSubtotalRangeQualificationExtension, ICustomerSubtotalRangeQualificationRepository, IEncorePromotionsPluginsUnitOfWork>, ICustomerSubtotalRangeQualificationHandler
    {
        public CustomerSubtotalRangeQualificationHandler()
        {
        }

        public override string GetProviderKey()
        {
            return QualificationExtensionProviderKeys.CustomerSubtotalRangeProviderKey;
        }

        public override PromotionQualificationResult Matches(IPromotion promotion, IPromotionQualificationExtension promotionQualification, IOrderContext orderContext)
        {
            Contract.Assert(promotionQualification != null);
            Contract.Assert(orderContext != null);
            Contract.Assert(typeof(ICustomerSubtotalRangeQualificationExtension).IsAssignableFrom(promotionQualification.GetType()));
            var qualification = (ICustomerSubtotalRangeQualificationExtension)promotionQualification; 
            
            if (!qualification.CustomerSubtotalRangesByCurrencyID.ContainsKey(orderContext.Order.CurrencyID))
            {
                return PromotionQualificationResult.NoMatch;
            }

            PromotionTypeConfigurationPerPromotionRepository promo = new PromotionTypeConfigurationPerPromotionRepository();
            decimal acumuladoAnterior = 0;
            if (promo.ExistePromotionType(promotion.PromotionID))
            {
                int ProductPriceTypeID = 1;// {ProductPriceTypes:Name:Retail  | ProductPriceTypeID:1}
                acumuladoAnterior = promo.ObtenerValorAcumuladoAccountKPIs(ProductPriceTypeID, orderContext.Order.OrderCustomers.ElementAt(0).AccountID);
            }

            var subtotalRange = qualification.CustomerSubtotalRangesByCurrencyID[orderContext.Order.CurrencyID];
            var customerIds = orderContext.Order.OrderCustomers.Where(x => x.AdjustedSubTotal + acumuladoAnterior >= subtotalRange.Minimum
                && (subtotalRange.Maximum == null || x.AdjustedSubTotal + acumuladoAnterior < subtotalRange.Maximum)).Select(x => x.AccountID);

            return customerIds.Any() ? PromotionQualificationResult.MatchForSelectCustomers(customerIds) : PromotionQualificationResult.NoMatch;
        }

        public override bool AreEqual(IPromotionQualificationExtension promotionQualification1, IPromotionQualificationExtension promotionQualification2)
        {
            Contract.Assert(promotionQualification1 != null);
            Contract.Assert(promotionQualification2 != null);
            Contract.Assert(promotionQualification1 is ICustomerSubtotalRangeQualificationExtension);
            Contract.Assert(promotionQualification2 is ICustomerSubtotalRangeQualificationExtension);

            var extension1 = promotionQualification1 as ICustomerSubtotalRangeQualificationExtension;
            var extension2 = promotionQualification2 as ICustomerSubtotalRangeQualificationExtension;

            if (extension1.CustomerSubtotalRangesByCurrencyID.Keys.Except(extension2.CustomerSubtotalRangesByCurrencyID.Keys).Any())
                return false;
			if (extension2.CustomerSubtotalRangesByCurrencyID.Keys.Except(extension1.CustomerSubtotalRangesByCurrencyID.Keys).Any())
                return false;

			return extension1.CustomerSubtotalRangesByCurrencyID.Keys.All(key => extension1.CustomerSubtotalRangesByCurrencyID[key].IsEqualTo(extension2.CustomerSubtotalRangesByCurrencyID[key]));
        }

        public override bool ValidFor<TProperty>(IPromotionQualificationExtension qualification, string propertyName, TProperty value)
        {
            Contract.Assert(qualification != null);

            return qualification.ValidFor(propertyName, value);
        }

		public override void CheckValidity(string qualificationKey, ICustomerSubtotalRangeQualificationExtension qualification, IPromotionState state)
		{
			if (qualification.CustomerSubtotalRangesByCurrencyID == null || !qualification.CustomerSubtotalRangesByCurrencyID.Any())
			{
				state.AddConstructionError
					(
						String.Format("Promotion Qualification {0}", qualificationKey),
						"Customer subtotal range dictionary is null or empty."
					);
			}
		}
	}
}
