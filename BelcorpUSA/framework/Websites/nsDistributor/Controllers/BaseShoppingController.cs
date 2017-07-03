using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components;
using NetSteps.Promotions.Service;
using nsDistributor.Models.Shared;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Common.Globalization;

namespace nsDistributor.Controllers
{

	public abstract class BaseShoppingController : BaseOrderContextController
	{
		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }


		public static new Account Account
		{
			get
			{
				return IsLoggedIn ? CoreContext.CurrentAccount : new Account()
				{
					AccountTypeID = (int)NetSteps.Data.Entities.Constants.AccountType.RetailCustomer,
					SponsorID = 0
				};
			}
		}

		protected override void SetViewData()
		{
			var baseSite = CurrentSite.IsBase ? CurrentSite : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.ToInt());
			var shoppingNav = baseSite.Navigations.FirstOrDefault(n => n.LinkUrl.Equals("/Shop", StringComparison.InvariantCultureIgnoreCase));
			ViewBag.SelectedNavigationId = shoppingNav == default(Navigation) ? (int?)null : shoppingNav.NavigationID;

			//ViewBag.OrderItemCount = Order == null ? 0 : Order.OrderCustomers[0].OrderItems.Count;
			ViewBag.OrderItemCount = OrderContext.Order == null ? 0 : ((Order)OrderContext.Order).OrderCustomers[0].ParentOrderItems.Count;

			if (this.GetType() != typeof(CheckoutController) && SiteOwner != null)
			{
				List<int> orderStatuses = new List<int>();
				orderStatuses.Add((int)Constants.OrderStatus.Pending);
				orderStatuses.Add((int)Constants.OrderStatus.PartiallyPaid);
				orderStatuses.Add((int)Constants.OrderStatus.CreditCardDeclined);
				ViewBag.OpenParties = Party.GetOpenParties(SiteOwner.AccountID, orderStatuses);
			}


			if (OrderContext.Order != null && OrderContext.Order.ParentOrderID.HasValue && OrderContext.Order.ParentOrderID > 0)
			{
				ViewBag.Party = Party.LoadFullByOrderID(OrderContext.Order.ParentOrderID.Value);
				if (this.GetType() != typeof(CheckoutController))
					ViewBag.AllowPartyDetach = true;
			}
			//ViewBag.FormattedCartPreview = GetFormattedCartPreview();
			ViewBag.CartModel = GetCartModelData((Order)OrderContext.Order);

			base.SetViewData();
		}

		protected virtual string GetProductImagePath(OrderItem orderItem)
		{
			string previewImage = "../../Content/Images/Shopping/no-image.jpg".ResolveUrl();
			var product = Inventory.GetProduct(orderItem.ProductID.ToInt());
			if (product != null && product.MainImage != null)
			{
				previewImage = product.MainImage.FilePath.ReplaceFileUploadPathToken();
			}
			if (product != null && product.IsVariant())
			{
				var productBase = ProductBase.LoadFull(product.ProductBaseID);
				if (productBase != null)
				{
					var nonVariantProduct = productBase.Products.FirstOrDefault(p => !p.IsVariant());
					if (nonVariantProduct != null && nonVariantProduct.MainImage != null)
					{
						previewImage = nonVariantProduct.MainImage.FilePath.ReplaceFileUploadPathToken();
					}
				}
			}
			return previewImage;
		}

		public bool StepHasAnItemInStockToBeChosen(string stepId)
		{
			try
			{
				var allSteps = CoreContext.CurrentOrderContext.InjectedOrderSteps.Union(CoreContext.CurrentOrder.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
				var step = (IUserProductSelectionOrderStep)allSteps.Single(os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
				var inventoryService = Create.New<IInventoryService>();
				var options = step.AvailableOptions.Select(o =>
				{
					var product = Inventory.GetProduct(o.ProductID);
					return inventoryService.GetProductAvailabilityForOrder(CoreContext.CurrentOrderContext, o.ProductID, 1);
				});
				foreach (var option in options)
				{
					if (option.CanAddBackorder > 0 || option.CanAddNormally > 0)
					{
						return true;
					}
				}
				return false;
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return false;
			}
		}

		protected virtual List<int> GetOutOfStockProducts(OrderCustomer customer)
		{
			var outOfStockProducts = new List<int>();
			if (customer != null)
			{
				foreach (OrderItem oi in customer.OrderItems)
				{
					Product p = Inventory.GetProduct(oi.ProductID.Value);
					if (p != null && Product.CheckStock(p.ProductID).IsOutOfStock && p.ProductBackOrderBehaviorID == (int)Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer)
					{
						outOfStockProducts.Add(p.ProductID);
					}
				}
			}

			return outOfStockProducts;
		}

		public virtual ICartModel GetCartModelData(Order order)
		{
			var customer = order.OrderCustomers[0];
			var model = Create.New<ICartModel>();

			model.ApplicablePromotions = GetApplicablePromotions(order);
			model.OrderItems = GetOrderItemModels(order);
			model.PromotionallyAddedItems = GetPromotionallyAddedItemModels(order);
			model.Subtotal = customer.Subtotal ?? 0;
			model.AdjustedSubtotal = customer.AdjustedSubTotal;
			model.Tax = customer.TaxAmountTotal;
			model.ShippingHandling = customer.ShippingTotal + customer.HandlingTotal;
			model.AdjustedShippingHandling = customer.AdjustedShippingTotal + customer.AdjustedHandlingTotal;
			model.GrandTotal = order.GrandTotal;
			model.CurrencySymbol = SmallCollectionCache.Instance.Currencies.GetById(order.CurrencyID).CurrencySymbol;
			var outofstockProds = GetOutOfStockProducts(customer);
			if (outofstockProds.Any())
			{
				model.OutOfStockProducts = customer.OrderItems.Where(oi => oi.ProductID.HasValue && outofstockProds.Contains(oi.ProductID.Value)).Select(oi => new { oi.SKU, oi.ProductName });
			}
			else
			{
				model.OutOfStockProducts = Enumerable.Empty<object>();
			}

			return model;
		}

		protected virtual IList<IPromotionInfoModel> GetApplicablePromotions(Order order)
		{
			var promotionAdjustments = order.OrderAdjustments.Where(adjustment => adjustment.ExtensionProviderKey == PromotionProvider.ProviderKey);
			var adjustments = promotionAdjustments.Where(adjustment => adjustment.OrderAdjustmentOrderLineModifications.Any() || adjustment.OrderAdjustmentOrderModifications.Any() || adjustment.InjectedOrderSteps.Any());
			return adjustments.Select(adj =>
			{
				bool isAvailable = false;
				if (adj.InjectedOrderSteps.Count() > 0)
				{
					foreach (var step in adj.InjectedOrderSteps)
					{
						isAvailable = StepHasAnItemInStockToBeChosen(step.OrderStepReferenceID);
						if (isAvailable)
							break;
					}
				}
				else
				{
					isAvailable = !adj.OrderModifications.Any(om => om.ModificationDescription.Contains("Unable"));
				}
				var giftStep = adj.InjectedOrderSteps.FirstOrDefault(os => os is IUserProductSelectionOrderStep &&
					(os.Response == null || (os.Response is IUserProductSelectionOrderStepResponse && (os.Response as IUserProductSelectionOrderStepResponse).SelectedOptions.Count == 0)));
				var promoInfo = Create.New<IPromotionInfoModel>();
				promoInfo.Description = adj.Description;
				promoInfo.StepID = giftStep == null ? null : giftStep.OrderStepReferenceID;
				promoInfo.Available = isAvailable;
				return promoInfo;
			}).ToList();
		}

		/// <summary>
		/// Returns order items, formatted for the client-side viewmodel.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
        protected virtual IList<IOrderItemModel> GetOrderItemModels(Order order)
        {
            Contract.Requires<ArgumentNullException>(order != null);
            Contract.Requires<ArgumentException>(order.OrderCustomers != null);
            Contract.Requires<ArgumentException>(order.OrderCustomers.Count > 0);

            return order.OrderCustomers[0].ParentOrderItems
                .Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem))
                .Select(GetOrderItemModel)
                .ToList();
        }
        //protected virtual IList<IOrderItemModel> GetOrderItemModels(Order order)
        //{
        //    Contract.Requires<ArgumentNullException>(order != null);
        //    Contract.Requires<ArgumentException>(order.OrderCustomers != null);
        //    Contract.Requires<ArgumentException>(order.OrderCustomers.Count > 0);

        //    return (order.OrderCustomers[0]).ParentOrderItems
        //        .Where(oi => !oi.OrderAdjustmentOrderLineModifications.Any(mod => mod.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem))
        //        .Select(orderItem =>
        //        {
        //            var orderItemModel = Create.New<IOrderItemModel>();
        //            var orderItemProduct = Inventory.GetProduct(orderItem.ProductID.Value);
        //            var preAdjustmentUnitPrice = orderItem.GetPreAdjustmentPrice(orderItem.ProductPriceTypeID.Value);
        //            var finalUnitPrice = orderItem.GetAdjustedPrice();
        //            orderItemModel.Guid = orderItem.Guid.ToString("N");
        //            orderItemModel.ProductID = orderItem.ProductID ?? 0;
        //            orderItemModel.SKU = orderItemProduct.SKU;
        //            orderItemModel.ProductName = orderItemProduct.Translations.Name();
        //            orderItemModel.ImageUrl = GetProductImagePath(orderItem);

        //            orderItemModel.OriginalUnitPrice = preAdjustmentUnitPrice;
        //            orderItemModel.AdjustedUnitPrice = finalUnitPrice;

        //            orderItemModel.Quantity = orderItem.Quantity;

        //            orderItemModel.OriginalCommissionableTotal = orderItem.GetPreAdjustmentPrice(orderItem.OrderCustomer.CommissionablePriceTypeID) * orderItem.Quantity;
        //            orderItemModel.AdjustedCommissionableTotal = orderItem.GetAdjustedPrice(orderItem.OrderCustomer.CommissionablePriceTypeID) * orderItem.Quantity;

        //            orderItemModel.AdjustedTotal = finalUnitPrice * orderItem.Quantity;
        //            orderItemModel.OriginalTotal = preAdjustmentUnitPrice * orderItem.Quantity;

        //            orderItemModel.ModificationReason = orderItem.OrderAdjustmentOrderLineModifications.Any() ? orderItem.OrderAdjustmentOrderLineModifications.First().OrderAdjustment.Description : string.Empty;

        //            orderItemModel.CurrencySymbol = SmallCollectionCache.Instance.Currencies.GetById(order.CurrencyID).CurrencySymbol.Trim();
        //            orderItemModel.GiftCardCodes = orderItem.GiftCards != null && orderItem.GiftCards.Any()
        //                                              ? orderItem.GiftCards.Select(x => x.Code).ToList()
        //                                              : null;

        //            // Hostess rewards show the discount amount next total.
        //            if (orderItem.IsHostReward)
        //            {
        //                // when hostess rewards are an order adjustment type we can refactor this.
        //                //orderItemModel.Total = string.Format("{0} {1}",
        //                //    orderItemModel.Total,
        //                //    Translation.GetTerm("HostRewardDiscount", "(discounted {0})",
        //                //        (x.Discount ?? (x.DiscountPercent.HasValue ? (x.ItemPrice * x.Quantity) * x.DiscountPercent.Value : 0)).ToString(order.CurrencyID)
        //                //    )
        //                //);
        //            }

        //            orderItemModel.IsStaticKit = orderItemProduct.IsStaticKit();
        //            orderItemModel.IsDynamicKit = orderItemProduct.IsDynamicKit();
        //            if (orderItemModel.IsDynamicKit)
        //            {
        //                orderItemModel.IsDynamicKitFull = orderItem.ChildOrderItems.Sum(oi => oi.Quantity) >= orderItemProduct.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount);
        //            }
        //            orderItemModel.IsHostReward = orderItem.IsHostReward;

        //            if (Url != null)
        //            {
        //                orderItemModel.BundlePackItemsUrl = Url.Action("BundlePackItems", new { productId = orderItem.ProductID, bundleGuid = orderItem.Guid.ToString("N"), orderCustomerId = orderItem.OrderCustomer.Guid.ToString("N") });
        //            }
        //            orderItemModel.KitItemsModel = Create.New<IKitItemsModel>();
        //            if (orderItemProduct.IsStaticKit() || orderItemProduct.IsDynamicKit())
        //            {
        //                orderItemModel.KitItemsModel.KitItemModels = orderItem.ChildOrderItems
        //                    .Select(k =>
        //                    {
        //                        var kitItemModel = Create.New<IKitItemModel>();
        //                        var kitItemProduct = Inventory.GetProduct(k.ProductID.Value);
        //                        kitItemModel.ProductName = kitItemProduct.Translations.Name();
        //                        kitItemModel.Quantity = k.Quantity;
        //                        kitItemModel.SKU = kitItemProduct.SKU;
        //                        kitItemModel.ImageUrl = GetProductImagePath(k);
        //                        return kitItemModel;
        //                    })
        //                    .ToList();
        //            }
        //            else
        //            {
        //                // Non-kits still need an empty list.
        //                orderItemModel.KitItemsModel.KitItemModels = new List<IKitItemModel>();
        //            }

        //            var product = Inventory.GetProduct(orderItem.ProductID.Value);

        //            if (product != null)
        //            {
        //                orderItemModel.CustomizationType = product.CustomizationType();
        //                orderItemModel.CustomValue = orderItem.GetOrderItemProperty(orderItemModel.CustomizationType);
        //                orderItemModel.HasCustomization = orderItemModel.CustomizationType != "";
        //            }

        //            return orderItemModel;
        //        })
        //        .ToList();
        //}

        protected virtual IOrderItemModel GetOrderItemModel(OrderItem orderItem)
        {
            Contract.Requires<ArgumentNullException>(orderItem != null);
            Contract.Requires<ArgumentNullException>(orderItem.OrderCustomer != null);
            Contract.Requires<ArgumentNullException>(orderItem.OrderCustomer.Order != null);

            var orderCustomer = orderItem.OrderCustomer;
            var order = orderCustomer.Order;
            var currencyId = order.CurrencyID;
            var orderItemModel = Create.New<IOrderItemModel>();

            var orderItemProduct = Inventory.GetProduct(orderItem.ProductID.Value);
            var preAdjustmentUnitPrice = orderItem.GetPreAdjustmentPrice();
            var finalUnitPrice = orderItem.ItemPriceActual ?? orderItem.GetAdjustedPrice();

            orderItemModel.Guid = orderItem.Guid.ToString("N");
            orderItemModel.ProductID = orderItem.ProductID ?? 0;
            orderItemModel.SKU = orderItem.SKU;
            orderItemModel.ProductName = orderItemProduct.Translations.Name();
            orderItemModel.retailPricePerItem = NetSteps.Data.Entities.ProductPricesExtensions.GetPriceByPriceType(orderItem.ProductID.Value, 1, currencyId).GetRoundedNumber(2).ToString(currencyId);
            orderItemModel.AdjustedUnitPrice = finalUnitPrice.ToString(currencyId);
            orderItemModel.OriginalUnitPrice = preAdjustmentUnitPrice.ToString(currencyId);

            orderItemModel.AdjustedTotal = (finalUnitPrice * orderItem.Quantity).ToString(currencyId);
            orderItemModel.OriginalTotal = (preAdjustmentUnitPrice * orderItem.Quantity).ToString(currencyId);

            orderItemModel.Quantity = orderItem.Quantity;

            orderItemModel.OriginalCommissionableTotal = (orderItem.GetPreAdjustmentPrice(orderCustomer.CommissionablePriceTypeID) * orderItem.Quantity).ToString(currencyId);
            orderItemModel.AdjustedCommissionableTotal = (orderItem.GetAdjustedPrice(orderCustomer.CommissionablePriceTypeID) * orderItem.Quantity).ToString(currencyId);

            foreach (var message in orderItem.OrderItemMessages
                .Select(m => m.Message))
            {
                orderItemModel.Messages.Add(message);
            }

            // Hostess rewards show the discount amount next total.
            if (orderItem.IsHostReward)
            {
                // when hostess rewards are an order adjustment type we can refactor this.
                orderItemModel.AdjustedTotal = string.Format("{0} {1}",
                    orderItemModel.AdjustedTotal,
                    Translation.GetTerm("HostRewardDiscount", "(discounted {0})",
                        (orderItem.Discount ?? (orderItem.DiscountPercent.HasValue ? (orderItem.ItemPrice * orderItem.Quantity) * orderItem.DiscountPercent.Value : 0)).ToString(currencyId)
                    )
                );
            }

            orderItemModel.IsStaticKit = orderItemProduct.IsStaticKit();
            orderItemModel.IsDynamicKit = orderItemProduct.IsDynamicKit();
            if (orderItemModel.IsDynamicKit)
            {
                orderItemModel.IsDynamicKitFull = orderItem.ChildOrderItems.Sum(oi => oi.Quantity) >= orderItemProduct.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount);
            }
            orderItemModel.IsHostReward = orderItem.IsHostReward;
            orderItemModel.BundlePackItemsUrl = Url.Action("BundlePackItems", new { productId = orderItem.ProductID, bundleGuid = orderItem.Guid.ToString("N"), orderCustomerId = orderItem.OrderCustomer.Guid.ToString("N") });

            orderItemModel.KitItemsModel = Create.New<IKitItemsModel>();
            if (orderItemProduct.IsStaticKit())
            {
                //orderItemModel.KitItemsModel.KitItemModels obj = new  
                List<MaterialName> objList = new List<MaterialName>();
                foreach (var objE in Order.GetMaterialWithMaterialID(Convert.ToInt32(Session["WareHouseId"]), Convert.ToInt32(Session["PreOrder"])).Where(x => x.ProductId == orderItem.ProductID))
                {
                    objList.Add(objE);
                }
                //foreach (var item in Order.productRelations.Where(x => x.ProductID == orderItem.ProductID).ToList())
                //{
                //    foreach (var objM in Order.GetMaterialWithMaterialID(item.MaterialId))
                //    {
                //        MaterialName objE = new MaterialName();
                //        objE.Name = objM.Name;
                //        objE.Quantity = item.Quantity;
                //        objE.SKU = objM.SKU;
                //        objList.Add(objE);
                //    }
                //} 
                //Is Kit
                orderItemModel.KitItemsModel.KitItemModels = objList
                    .Select(k =>
                    {
                        var kitItemModel = Create.New<IKitItemModel>();

                        kitItemModel.ProductName = k.Name;
                        kitItemModel.Quantity = k.Quantity;
                        kitItemModel.SKU = k.SKU;
                        return kitItemModel;
                    }).ToList();

            }
            //if (orderItemProduct.IsStaticKit() || orderItemProduct.IsDynamicKit())
            //{
            //    orderItemModel.KitItemsModel.KitItemModels = orderItem.ChildOrderItems
            //        .Select(k =>
            //        {
            //            var kitItemModel = Create.New<IKitItemModel>();
            //            var kitItemProduct = Inventory.GetProduct(k.ProductID.Value);
            //            kitItemModel.ProductName = kitItemProduct.Translations.Name();
            //            kitItemModel.Quantity = k.Quantity;
            //            kitItemModel.SKU = kitItemProduct.SKU;
            //            return kitItemModel;
            //        })
            //        .ToList();
            //}
            else
            {
                // Non-kits still need an empty list.
                orderItemModel.KitItemsModel.KitItemModels = new List<IKitItemModel>();
            }

            //CGI(CMR)-29/10/2014-Inicio
            decimal totalQV = 0;
            totalQV = orderItem.OrderItemPrices
                                    .Where(ip => ip.ProductPriceTypeID == (int)NetSteps.Data.Entities.Constants.ProductPriceType.QV)
                                    .Sum(ip => ((ip.UnitPrice == null ? 0 : ip.UnitPrice) * (orderItem.Quantity == null ? 0 : (decimal)orderItem.Quantity)));

            orderItemModel.TotalQV = totalQV;
            orderItemModel.TotalQV_Currency = totalQV.ToString(currencyId);
            //CGI(CMR)-29/10/2014-Inicio

            return orderItemModel;
        }


		/// <summary>
		/// Gets the OrderItem's adustment description for displaying
		/// </summary>
		/// <param name="orderItem">The OrderItem</param>
		/// <returns>A modification description for displaying to the user</returns>
		protected virtual string GetOrderItemModificationReason(OrderItem orderItem)
		{
			// return the description of the first adjustment, if any
			var mods = orderItem.OrderAdjustmentOrderLineModifications;
			return mods.Any() ? mods.First().OrderAdjustment.Description : string.Empty;
		}


		protected virtual IList<IPromotionallyAddedItemModel> GetPromotionallyAddedItemModels(Order order)
		{
			OrderCustomer orderCustomer = order.OrderCustomers[0];
			int addedItemOperationID = (int)OrderAdjustmentOrderLineOperationKind.AddedItem;
			var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));
			var promotionalItems = orderCustomer.ParentOrderItems.Except(nonPromotionalItems).ToList();
			var adjustments = promotionalItems.GroupBy(x => x.OrderAdjustmentOrderLineModifications.Single(y => y.ModificationOperationID == addedItemOperationID).OrderAdjustment);

			return adjustments.Select(grp =>
			{
				var model = Create.New<IPromotionallyAddedItemModel>();
				model.Description = grp.Key.Description;
				model.StepID = grp.Key.InjectedOrderSteps.Any() ? grp.Key.InjectedOrderSteps.First().OrderStepReferenceID : null;
				model.Selections = grp.Select(i =>
				{
					var item = Create.New<IOrderItemModel>();
					item.SKU = i.SKU;
					item.ProductName = i.ProductName;
					item.Quantity = i.Quantity;
					item.ImageUrl = GetProductImagePath(i);
					return item;
				}).ToList();

				return model;
			}).ToList();
		}

	}
}
