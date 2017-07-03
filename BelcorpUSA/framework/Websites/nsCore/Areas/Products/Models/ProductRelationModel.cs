using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Extensions;
using System;

namespace nsCore.Areas.Products.Models
{
    public class ProductRelationModel
    {
        public Product Product;
        public List<ProductRelationsType> RelationTypes { get; set; }
        public int CurrentLanguageID { get; set; }
        public List<Language> Languages { get; set; }
        public List<DynamicKitPricingType> PricingTypes { get; set; }
        public List<ProductRelationKitGroupModel> DynamicKitGroups { get; set; }
        public int DynamicKitPricingTypeID { get; set; }
        public int ProductID { get { return this.Product.ProductID; } }
        public decimal ParticipacionPorcentaje { get; set; }

        public ProductRelationModel LoadResources(Product product)
        {
            Product = product;
            RelationTypes = SmallCollectionCache.Instance.ProductRelationsTypes.ToList();
            CurrentLanguageID = CoreContext.CurrentLanguageID;
            Languages = TermTranslation.GetLanguages(CurrentLanguageID);
            PricingTypes = SmallCollectionCache.Instance.DynamicKitPricingTypes.ToList();
            string valorParticipacionPorcentaje= OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "PPR");
            if (valorParticipacionPorcentaje.Trim().Length > 0)
                ParticipacionPorcentaje = Convert.ToDecimal(valorParticipacionPorcentaje);
            else ParticipacionPorcentaje = 100;

            var returnModel = new List<ProductRelationKitGroupModel>();
            if (product.DynamicKits.Count > 0)
            {
                DynamicKitPricingTypeID = product.DynamicKits[0].DynamicKitPricingTypeID != null
                                              ? (int) product.DynamicKits[0].DynamicKitPricingTypeID
                                              : 0;

                if (product.DynamicKits[0].DynamicKitGroups.Count > 0)
                {
                    returnModel.AddRange(
                        product.DynamicKits[0].DynamicKitGroups.Select(
                            @group => new ProductRelationKitGroupModel().LoadResources(@group)));
                }
            }

            DynamicKitGroups = returnModel;

            return this;
        }
    }
}