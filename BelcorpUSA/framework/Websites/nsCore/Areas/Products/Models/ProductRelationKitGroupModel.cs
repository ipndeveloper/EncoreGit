using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.Products.Models
{
    public class ProductRelationKitGroupModel
    {
        public int? GroupID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinimumProductCount { get; set; }
        public int MaximumProductCount { get; set; }
        //public int PricingTypeID { get; set; }
        //public virtual List<GroupPricingTypeModel> PricingTypes  { get; set; }
        public virtual List<ProductTypeModel> ProductTypeList { get; set; }
        public virtual List<ProductRelationKitGroupRuleModel> IncludedRules { get; set; }
        public virtual List<ProductRelationKitGroupRuleModel> ExcludedRules { get; set; }
        public virtual List<ProductRelationKitGroupRuleModel> DefaultRules { get; set; }        
        public virtual string ProductRuleSelectionType { get; set; }
        public virtual int? SelectedProductTypeIDValue { get; set; }
        public virtual int? SelectedProductIDValue { get; set; }
        public virtual bool SelectedExclude { get; set; }
        public virtual bool SelectedDefault { get; set; }
        public virtual int SelectedLanguageID { get; set; }

        public ProductRelationKitGroupModel LoadResources(DynamicKitGroup group)
        {
            GroupID = group.DynamicKitGroupID;
            Name = group.Translations.Name();

            Description = group.Translations.ShortDescription();
            MinimumProductCount = group.MinimumProductCount;
            MaximumProductCount = group.MaximumProductCount;

            var productTypeEntities = SmallCollectionCache.Instance.ProductTypes.Where(
                pt =>
                !group.DynamicKitGroupRules.Any(
                    dkgr => dkgr.ProductTypeID.HasValue && dkgr.ProductTypeID.Value == pt.ProductTypeID));

            ProductTypeList = productTypeEntities.Select(
                            x => new ProductTypeModel().LoadResources(x)).ToList();
            var kitGroupRules =
                    group.DynamicKitGroupRules.Select(rule => new ProductRelationKitGroupRuleModel().LoadResources(rule))
                        .ToList();
            IncludedRules = kitGroupRules.Where(x => x.Include).ToList();
            ExcludedRules = kitGroupRules.Where(x => !x.Include).ToList();
            DefaultRules = kitGroupRules.Where(x => x.Include && x.Default).ToList();
            ProductRuleSelectionType = "ProductType";
            return this;
        }

        public bool IsMinMoreThanMax()
        {
            return (MinimumProductCount > MaximumProductCount);
        }
    }
}