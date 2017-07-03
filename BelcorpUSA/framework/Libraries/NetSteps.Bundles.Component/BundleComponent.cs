using System.Collections.Generic;
using System.Linq;
using NetSteps.Bundles.Common;
using NetSteps.Bundles.Common.Models;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Bundles.Component
{
	[ContainerRegister(typeof(IBundleComponent), RegistrationBehaviors.DefaultOrOverrideDefault)]
	public class BundleComponent : IBundleComponent
	{
		private IBundleRepository repository;

		public BundleComponent()
			: this(null)
		{
		}

		public BundleComponent(IBundleRepository repo)
		{
			repository = repo ?? Create.New<IBundleRepository>();
		}

		public bool IsValidKit(List<IBundleItem> itemsOnKit, int kitProductID)
		{
			var kit = repository.GetDynamicKitProductByID(kitProductID);
			var allGroups = kit.DynamicKits.SelectMany(dk => dk.DynamicKitGroups);
			var clonedList = CloneList(itemsOnKit);


			foreach (IDynamicKitGroup group in allGroups.OrderBy(ag => ag.DynamicKitGroupRules.Any(dkgr => !dkgr.Required)))
			{
				if (!ValidateGroup(group, clonedList))
				{
					return false;
				}
			}

			return true;
		}

		public bool ValidateGroup(IDynamicKitGroup group, List<IBundleItem> workingList)
		{
			int quantityOnGroup = 0;
			foreach (IBundleItem item in workingList.Where(cl => CanBeAddedToDynamicKitGroup(cl, group)))
			{
				if (quantityOnGroup >= group.MinimumProductCount)
				{
					break;
				}

				quantityOnGroup += ApplyItemsToGroup(group, item, quantityOnGroup);
			}

			if (quantityOnGroup < group.MinimumProductCount)
			{
				return false;
			}

			return true;
		}

		public int ApplyItemsToGroup(IDynamicKitGroup group, IBundleItem item, int quantityAlreadyOnGroup)
		{
			int remainingItems = group.MinimumProductCount - quantityAlreadyOnGroup;
			var clonedItem = CloneBundleItem(item);

			if (item.Quantity > remainingItems)
			{
				item.Quantity -= remainingItems;
				clonedItem.Quantity = remainingItems;
				return remainingItems;
			}

			item.Quantity = 0;
			return clonedItem.Quantity;

		}

		public virtual bool CanBeAddedToDynamicKitGroup(IBundleItem item, IDynamicKitGroup group)
		{
			if (item.IsDynamicKit || item.IsStaticKit || item.IsParentStaticKit)
			{
				return false;
			}

			if (!group.DynamicKitGroupRules.Any(r => (r.ProductTypeID == item.ProductTypeID || r.ProductID == item.ProductID) && r.Include))
			{
				return false;
			}

			if (group.DynamicKitGroupRules.Any(r => (r.ProductID == item.ProductID) && !r.Include))
			{
				return false;
			}

			return true;
		}




		#region Most Certainly Not Used

		public IEnumerable<ICustomerKitResult> AutoBundleItems(IBundleOrder order, int storeFrontID)
		{
			// All dynamic kits for the given store front ID
			List<IKit> sortedDynamicKitProducts = repository.GetDynamicKitProducts(storeFrontID, true, true);
			var returnValue = new List<ICustomerKitResult>();

			foreach (var customer in order.OrderCustomers)
			{
				var kitsCustomerCanQualifyFor = new List<IDynamicKit>();
				foreach (var kitProduct in sortedDynamicKitProducts)
				{
					// Get each kit that the customer can qualify for
					kitProduct.OrderItems.Clear();
					foreach (IDynamicKit dynamicKit in kitProduct.DynamicKits)
					{
						if (kitsCustomerCanQualifyFor.All(kccqf => kccqf.DynamicKitID == dynamicKit.DynamicKitID)
							&& canCustomerQualifyForKit(customer.OrderItems, dynamicKit, 0))
						{
							kitsCustomerCanQualifyFor.Add(dynamicKit);
						}
					}
				}


				var result = DivvyItemsToKits(kitsCustomerCanQualifyFor, customer);
				result.AccountID = customer.AccountID;
				returnValue.Add(result);
			}

			return returnValue;
		}

		private ICustomerKitResult DivvyItemsToKits(List<IDynamicKit> kitsCustomerCanQualifyFor, IBundleCustomer customer)
		{
			var returnValue = Create.New<ICustomerKitResult>();

			returnValue.KitlessOrderItems = new List<IBundleItem>();
			returnValue.Kits = new List<IKit>();

			var workingList = CloneList(customer.OrderItems);


			// First pass is to get as many high-value kits as possible
			foreach (IDynamicKit dynamicKit in kitsCustomerCanQualifyFor.OrderByDescending(ki => ki.DynamicKitGroups.Sum(dy => dy.MinimumProductCount)))
			{
				if (!CanCoverKitWithRemainingItems(workingList, dynamicKit))
				{
					continue;
				}

				foreach (IDynamicKitGroup group in dynamicKit.DynamicKitGroups)
				{
					int quantityApplied = 0;
					foreach (IBundleItem item in workingList.Where(oi => oi.Quantity > 0 && CanBeAddedToDynamicKitGroup(oi, group)))
					{
						if (quantityApplied >= group.MaximumProductCount)
						{
							continue;
						}

						int quantityToAddToGroup = DetermineQuantityToAddToGroup(item, group);

						var currentKit = GetOrCreateKitFromCustomerKitContents(dynamicKit, returnValue);

						var clonedBundleItem = CloneBundleItem(item);

						clonedBundleItem.Quantity = quantityToAddToGroup;
						item.Quantity -= quantityToAddToGroup;

						currentKit.OrderItems.Add(clonedBundleItem);

						quantityApplied += quantityToAddToGroup;

					}
				}
			}

			foreach (IBundleItem item in workingList.Where(oi => oi.Quantity > 0))
			{
				returnValue.KitlessOrderItems.Add(CloneBundleItem(item));
			}

			return returnValue;
		}

		private bool CanCoverKitWithRemainingItems(IEnumerable<IBundleItem> workingList, IDynamicKit dynamicKit)
		{
			var tempCopy = CloneList(workingList.ToList());
			foreach (IDynamicKitGroup group in dynamicKit.DynamicKitGroups)
			{
				int quantityApplied = 0;
				foreach (IBundleItem item in tempCopy.Where(oi => oi.Quantity > 0 && CanBeAddedToDynamicKitGroup(oi, group)))
				{
					if (quantityApplied >= group.MaximumProductCount)
					{
						continue;
					}

					int quantityToAddToGroup = DetermineQuantityToAddToGroup(item, group);

					item.Quantity -= quantityToAddToGroup;
					quantityApplied += quantityToAddToGroup;

				}

				if (quantityApplied < group.MinimumProductCount)
				{
					return false;
				}
			}

			return true;
		}

		private IKit GetOrCreateKitFromCustomerKitContents(IDynamicKit dynamicKit, ICustomerKitResult returnValue)
		{
			var currentKit = returnValue.Kits.FirstOrDefault(ki => ki.DynamicKits.First().DynamicKitID == dynamicKit.DynamicKitID);
			if (currentKit == null)
			{
				currentKit = Create.New<IKit>();
				currentKit.DynamicKits = new List<IDynamicKit> { dynamicKit };
				currentKit.OrderItems = new List<IBundleItem>();
				returnValue.Kits.Add(currentKit);
			}
			return currentKit;
		}

		private int DetermineQuantityToAddToGroup(IBundleItem item, IDynamicKitGroup group)
		{
			if (item.Quantity >= group.MaximumProductCount)
			{
				return group.MaximumProductCount;
			}
			if (item.Quantity < group.MaximumProductCount)
			{
				return item.Quantity;
			}

			return 0;
		}

		private List<IBundleItem> CloneList(List<IBundleItem> toClone)
		{
			var returnValue = new List<IBundleItem>();
			foreach (var item in toClone)
			{
				var toAdd = CloneBundleItem(item);

				returnValue.Add(toAdd);
			}
			return returnValue;
		}

		private IBundleItem CloneBundleItem(IBundleItem toClone)
		{
			var returnValue = Create.New<IBundleItem>();

			returnValue.HasAdjustmentOrderLineModifications = toClone.HasAdjustmentOrderLineModifications;
			returnValue.IsDynamicKit = toClone.IsDynamicKit;
			returnValue.IsHostReward = toClone.IsHostReward;
			returnValue.IsStaticKit = toClone.IsStaticKit;
			returnValue.ProductID = toClone.ProductID;
			returnValue.ProductTypeID = toClone.ProductTypeID;
			returnValue.Quantity = toClone.Quantity;

			return returnValue;
		}

		private bool canCustomerQualifyForKit(List<IBundleItem> bundleItems, IDynamicKit kit, int numberOfProductsAway)
		{
			var groups = kit.DynamicKitGroups;
			int requiredNumberOfProducts = groups.Sum(g => g.MinimumProductCount);

			List<IBundleItem> elligibleBundleItems = bundleItems.Where(bi => !bi.HasAdjustmentOrderLineModifications && !bi.IsHostReward).ToList();
			if (elligibleBundleItems.Sum(bi => bi.Quantity) < requiredNumberOfProducts)
				return false;

			var usedItemProductIDsAndQuantities = elligibleBundleItems.Select(el => new ProductQuantity { OrderItem = el, QuantityUsed = 0 });

			foreach (IDynamicKitGroup kitGroup in groups)
			{
				int quantityUsedOnKitGroup = 0;
				foreach (var item in usedItemProductIDsAndQuantities)
				{
					if (quantityUsedOnKitGroup >= kitGroup.MinimumProductCount)
						continue;

					//ProductQuantity currentProperty = usedItemProductIDsAndQuantities.FirstOrDefault(used => used.OrderItem.ProductID == toClone.OrderItem.ProductID);
					int unusedQuantity = item.OrderItem.Quantity - item.QuantityUsed;
					if (unusedQuantity <= 0)
						continue;

					if (CanBeAddedToDynamicKitGroup(item.OrderItem, kitGroup))
					{
						int quantityToUse = unusedQuantity < kitGroup.MinimumProductCount - quantityUsedOnKitGroup ? unusedQuantity : kitGroup.MinimumProductCount - quantityUsedOnKitGroup;
						quantityUsedOnKitGroup += quantityToUse;
						item.QuantityUsed += quantityToUse;
					}
				}

				if (quantityUsedOnKitGroup < kitGroup.MinimumProductCount)
				{
					return false;
				}
			}

			return true;
		}




		protected class ProductQuantity
		{
			public IBundleItem OrderItem { get; set; }
			public int QuantityUsed { get; set; }
		}

		#endregion
	}


}
