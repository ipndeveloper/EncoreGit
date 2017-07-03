using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace nsCore.Areas.Products.Models
{
	public class ProductRelationKitGroupRuleModel
	{
		public virtual int RuleID { get; set; }
		public virtual int GroupID { get; set; }
		public virtual int? RuleProductID { get; set; }
		public virtual int? ProductTypeID { get; set; }
		public virtual string Description { get; set; }
		public virtual bool Include { get; set; }
		public virtual bool Default { get; set; }
		public virtual int SortOrder { get; set; }
		public virtual bool Selected { get; set; }

		public virtual ProductRelationKitGroupRuleModel LoadResources(DynamicKitGroupRule Rule)
		{
			var inventory = Create.New<InventoryBaseRepository>();

			RuleID = Rule.DynamicKitGroupRuleID;
			GroupID = Rule.DynamicKitGroup.DynamicKitGroupID;
			RuleProductID = Rule.ProductID;
			ProductTypeID = Rule.ProductTypeID;
			Description = (Rule.ProductTypeID != null && Rule.ProductTypeID.HasValue)
							  ? SmallCollectionCache.Instance.ProductTypes.GetById(Rule.ProductTypeID.Value).GetTerm()
							  : (Rule.ProductID != null && Rule.ProductID.HasValue)
									? inventory.GetProduct(Rule.ProductID.Value).Translations.Name()
									: null;
			SortOrder = Rule.SortOrder;
			Include = Rule.Include;
			Default = Rule.Default;
			return this;
		}
	}
}