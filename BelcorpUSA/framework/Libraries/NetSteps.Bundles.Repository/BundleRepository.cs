using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Bundles.Repository
{
	using NetSteps.Bundles.Common;
	using NetSteps.Bundles.Common.Models;
	using NetSteps.Data.Entities;
	using NetSteps.Data.Entities.Cache;
	using NetSteps.Encore.Core.IoC;

	[ContainerRegister(typeof(IBundleRepository), RegistrationBehaviors.DefaultOrOverrideDefault, ScopeBehavior = ScopeBehavior.Singleton)]
	public class BundleRepository : IBundleRepository
	{
		protected InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }

		public IKit GetDynamicKitProductByID(int dynamicKitProductID)
		{
			var dynamicKitProduct = Inventory.GetProduct(dynamicKitProductID);
			if (dynamicKitProduct == null)
			{
				return null;
			}

			var returnValue = Create.New<IKit>();
			returnValue.ProductID = dynamicKitProduct.ProductID;
			returnValue.OrderItems = GetIBundleItems(dynamicKitProduct);
			returnValue.DynamicKits = GetIDynamicKits(dynamicKitProduct);

			return returnValue;
		}

		public List<IKit> GetDynamicKitProducts(int storeFrontID, bool sort = false, bool sortDescending = false)
		{
			var dynamicKitProducts = Inventory.GetDynamicKitProducts(storeFrontID, sort, sortDescending);
			var returnValue = new List<IKit>();

			foreach (Product product in dynamicKitProducts)
			{
				var toAdd = Create.New<IKit>();
				toAdd.ProductID = product.ProductID;
				toAdd.OrderItems = GetIBundleItems(product);
				toAdd.DynamicKits = GetIDynamicKits(product);

				returnValue.Add(toAdd);
			}

			return returnValue;
		}

		private List<IBundleItem> GetIBundleItems(Product product)
		{
			var returnValue = new List<IBundleItem>();
			foreach (OrderItem item in product.OrderItems)
			{
				var toAdd = Create.New<IBundleItem>();
				toAdd.HasAdjustmentOrderLineModifications = item.OrderAdjustmentOrderLineModifications.Any();
				toAdd.IsDynamicKit = product.IsDynamicKit();
				toAdd.IsStaticKit = product.IsStaticKit();
				toAdd.ProductTypeID = product.ProductBase.ProductTypeID;
				toAdd.IsHostReward = item.IsHostReward;
				toAdd.ProductID = item.ProductID ?? 0;
				toAdd.Quantity = item.Quantity;
				toAdd.IsParentStaticKit = false;

				returnValue.Add(toAdd);
			}
			return returnValue;
		}

		private List<IDynamicKit> GetIDynamicKits(Product product)
		{
			var returnValue = new List<IDynamicKit>();

			foreach (DynamicKit kt in product.DynamicKits)
			{

				var toAdd = Create.New<IDynamicKit>();
				toAdd.DynamicKitID = kt.DynamicKitID;
				toAdd.DynamicKitGroups = GetIDynamicKitGroups(kt);
				toAdd.DynamicKitProductID = kt.ProductID;
				returnValue.Add(toAdd);
			}

			return returnValue;
		}

		private List<IDynamicKitGroup> GetIDynamicKitGroups(DynamicKit kit)
		{
			var returnValue = new List<IDynamicKitGroup>();

			foreach (DynamicKitGroup dynamicKitGroup in kit.DynamicKitGroups)
			{
				var toAdd = Create.New<IDynamicKitGroup>();

				toAdd.DynamicKitGroupID = dynamicKitGroup.DynamicKitGroupID;
				toAdd.MinimumProductCount = dynamicKitGroup.MinimumProductCount;
				toAdd.DynamicKitGroupRules = GetIDynamicKitGroupRules(dynamicKitGroup);
				toAdd.MaximumProductCount = dynamicKitGroup.MaximumProductCount;

				returnValue.Add(toAdd);
			}

			return returnValue;
		}

		private List<IDynamicKitGroupRule> GetIDynamicKitGroupRules(DynamicKitGroup kitGroup)
		{
			var returnValue = new List<IDynamicKitGroupRule>();
			foreach (DynamicKitGroupRule rl in kitGroup.DynamicKitGroupRules)
			{
				var toAdd = Create.New<IDynamicKitGroupRule>();

				toAdd.ProductID = rl.ProductID ?? 0;
				toAdd.ProductTypeID = rl.ProductTypeID ?? 0;
				toAdd.DynamicKitGroupRuleID = rl.DynamicKitGroupRuleID;
				toAdd.Default = rl.Default;
				toAdd.Include = rl.Include;
				toAdd.Required = rl.Required;
				toAdd.SortOrder = rl.SortOrder;

				returnValue.Add(toAdd);
			}
			return returnValue;
		}
	}
}
