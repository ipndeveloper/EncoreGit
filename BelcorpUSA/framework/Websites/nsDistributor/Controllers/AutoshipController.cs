using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;
using nsDistributor.Models.Autoship;
using nsDistributor.Models.Shared;
using NetSteps.Promotions.Service;
using System.Diagnostics.Contracts;
using NetSteps.OrderAdjustments.Common.Model;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;

namespace nsDistributor.Controllers
{
	public class AutoshipController : BaseController
	{
		#region Properties

		protected virtual string _errorLoadingAutoship { get { return Translation.GetTerm("ErrorLoadingAutoship", "There was an error loading your autoship."); } }
		protected virtual string _errorUpdatingAutoship { get { return Translation.GetTerm("ErrorUpdatingAutoship", "There was an error updating your autoship."); } }
		protected virtual string _errorAutoshipDoesNotMeetMinimumVolume { get { return Translation.GetTerm("ErrorAutoshipDoesNotMeetMinimumVolume", "Your autoship does not meet the minimum volume requirement."); } }
		protected virtual string _errorAutoshipMustContainAtLeastOneItem { get { return Translation.GetTerm("ErrorAutoshipMustContainAtLeastOneItem", "Your autoship must contain at least one item."); } }
		protected virtual string _errorOrderMissingInfoString { get { return Translation.GetTerm("ErrorOrderMissingInfo", "The order is missing required information."); } }

		private readonly Lazy<IOrderService> _orderServiceFactory = new Lazy<IOrderService>(Create.New<IOrderService>);
		protected IOrderService OrderService { get { return _orderServiceFactory.Value; } }

		public InventoryBaseRepository Inventory { get { return Create.New<InventoryBaseRepository>(); } }
		public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }

		#endregion
		
		public virtual ActionResult Edit()
		{
			var model = new EditModel();

			Edit_LoadResources(model, _autoshipOrder);

			return View(model);
		}

		[HttpPost, ValidateAntiForgeryToken]
		public virtual ActionResult Edit(EditModel model)
		{
			Edit_ValidateOrderItems(_autoshipOrder);
			Edit_ValidateVolume(_autoshipOrder);

			if (!ModelState.IsValid)
			{
				Edit_LoadResources(model, _autoshipOrder);
				return View(model);
			}

			try
			{
				Edit_UpdateAutoshipOrder(model, _autoshipOrder);
				_autoshipOrder.Save();
				SubmitOrder(_autoshipOrder.Order, CoreContext.CurrentAccount);
			}
			catch (Exception ex)
			{
				AddErrorToViewData(ex.Log().PublicMessage);
				Edit_LoadResources(model, _autoshipOrder);
				return View(model);
			}

			return _defaultCompletedResult;
		}

		[HttpPost]
		public virtual ActionResult GetAutoshipProducts(MiniShopCategoryModel model)
		{
			try
			{
				var productModels = GetProductModels(model.CategoryID, _autoshipOrder.Order);

				var modelData = Edit_GetModelData(products: productModels);

				return Json(modelData.AsDictionary());
			}
			catch (Exception ex)
			{
				return JsonError(ex.Log().PublicMessage);
			}
		}

		[HttpPost]
		public virtual ActionResult AddAutoshipOrderItem(MiniShopProductModel model)
		{
			try
			{
				var orderItem = _autoshipOrder.Order.OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID == model.ProductID);
				var shippingMethodList = ShippingCalculator.GetShippingMethodsWithRates(_autoshipOrder.Order.OrderCustomers[0], _autoshipOrder.Order.OrderShipments[0]).OrderBy(sm => sm.ShippingAmount);
				var shippingMethodModel = new MiniShopShippingMethodModel(shippingMethodList, _autoshipOrder.Order.CurrencyID);

				// Increment quantity by one unless this is the first instance of this product in the order.
				if (orderItem != null)
				{
					_autoshipOrder.Order.UpdateItem(
							_autoshipOrder.Order.OrderCustomers[0],
							orderItem,
							orderItem.Quantity + 1
					);
				}
				else
				{
					_autoshipOrder.Order.AddItem(model.ProductID, 1);
				}

				_autoshipOrder.Order = UpdateOrderUsingOrderService(_autoshipOrder.Order);

				var modelData = Edit_GetModelData(
						autoshipOrder: _autoshipOrder,
						orderItems: GetOrderItemModels(_autoshipOrder.Order),
						shippingMethods: shippingMethodModel,
						selectedShippingMethodID: _autoshipOrder.Order.OrderShipments[0].ShippingMethodID
				);

				return Json(modelData.AsDictionary());
			}
			catch (Exception ex)
			{
				return JsonError(ex.Log().PublicMessage);
			}
		}

		[HttpPost]
		public virtual ActionResult RemoveAutoshipOrderItem(MiniShopOrderItemModel model)
		{
			try
			{
				var orderItem = _autoshipOrder.Order.OrderCustomers[0].OrderItems.FirstOrDefault(x => x.ProductID == model.ProductID);
				var shippingMethodList = ShippingCalculator.GetShippingMethodsWithRates(_autoshipOrder.Order.OrderCustomers[0], _autoshipOrder.Order.OrderShipments[0]).OrderBy(sm => sm.ShippingAmount);
				var shippingMethodModel = new MiniShopShippingMethodModel(shippingMethodList, _autoshipOrder.Order.CurrencyID);

				if (orderItem == null)
				{
					return JsonError(_errorUpdatingAutoship);
				}

				// Decrement quantity by one unless this is the last instance of this product in the order.
				if (model.Quantity > 1)
				{
					_autoshipOrder.Order.UpdateItem(
							_autoshipOrder.Order.OrderCustomers[0],
							orderItem,
							model.Quantity - 1
					);
				}
				else
				{
					_autoshipOrder.Order.RemoveItem(orderItem);
				}

				_autoshipOrder.Order = UpdateOrderUsingOrderService(_autoshipOrder.Order);

				var modelData = Edit_GetModelData(
						autoshipOrder: _autoshipOrder,
						orderItems: GetOrderItemModels(_autoshipOrder.Order),
						shippingMethods: shippingMethodModel,
						selectedShippingMethodID: _autoshipOrder.Order.OrderShipments[0].ShippingMethodID

				);

				return Json(modelData.AsDictionary());
			}
			catch (Exception ex)
			{
				return JsonError(ex.Log().PublicMessage);
			}
		}

		#region Helpers

		protected AutoshipOrder _autoshipOrder;
		protected int _storeFrontID;

		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			// TODO: Make an Authorize attribute for this
			if (!IsLoggedIn)
			{
				filterContext.Result = Request.IsAjaxRequest()
						? JsonError(_errorSessionTimedOut)
						: _defaultCompletedResult;

				return;
			}

			// Load autoship
			_autoshipOrder = CoreContext.CurrentAutoship;
			if (_autoshipOrder == null
				// Validate they are the autoship owner
					|| _autoshipOrder.AccountID != CoreContext.CurrentAccount.AccountID
				// Always reload on initial page load
					|| (filterContext.ActionDescriptor.ActionName.EqualsIgnoreCase("Edit") && filterContext.HttpContext.Request.HttpMethod.EqualsIgnoreCase("GET")))
			{
				_autoshipOrder = LoadAutoshipOrder();
				CoreContext.CurrentAutoship = _autoshipOrder;
			}

			// Validate autoship is fully loaded
			if (_autoshipOrder == null
					|| _autoshipOrder.Order == null
					|| !_autoshipOrder.Order.OrderCustomers.Any())
			{
				filterContext.Result = Request.IsAjaxRequest()
						? JsonError(_errorLoadingAutoship)
						: _defaultCompletedResult;

				return;
			}

			_storeFrontID = GetStoreFrontID(CoreContext.CurrentAccount);

			base.OnActionExecuting(filterContext);
		}

		protected virtual AutoshipOrder LoadAutoshipOrder()
		{
			var autoshipOrder = AutoshipOrder
					.LoadAllFullByAccountID(CoreContext.CurrentAccount.AccountID)
					.FirstOrDefault(a =>
							a.Order.OrderStatusID == (int)Constants.OrderStatus.Paid
							&& SmallCollectionCache.Instance.AutoshipSchedules.GetById(a.AutoshipScheduleID).AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.Normal);

			if(autoshipOrder != null && autoshipOrder.Order != null)
			{
				autoshipOrder.Order = UpdateOrderUsingOrderService(autoshipOrder.Order);
			}

			return autoshipOrder;
		}

		protected virtual int GetStoreFrontID(Account account)
		{
			// Try to get the account's storefront
			var shippingAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Shipping);
			if (shippingAddress != null)
			{
				var marketStoreFront = SmallCollectionCache.Instance.MarketStoreFronts.FirstOrDefault(x =>
						x.SiteTypeID == ApplicationContext.Instance.SiteTypeID
						&& x.MarketID == SmallCollectionCache.Instance.Countries.GetById(shippingAddress.CountryID).MarketID);
				if (marketStoreFront != null)
				{
					return marketStoreFront.StoreFrontID;
				}
			}

			// Else default to site's storefront
			return ApplicationContext.Instance.StoreFrontID;
		}

		protected virtual ActionResult _defaultCompletedResult
		{
			get
			{
				return Redirect("~/Account");
			}
		}

		protected virtual void Edit_LoadResources(EditModel model, AutoshipOrder autoshipOrder)
		{
			var categoryModels = GetCategoryModels();
			var productModels = categoryModels.Any() ? GetProductModels(categoryModels.First().CategoryID, autoshipOrder.Order) : null;
			var orderItemModels = GetOrderItemModels(autoshipOrder.Order);
			var shippingMethodList = ShippingCalculator.GetShippingMethodsWithRates(autoshipOrder.Order.OrderCustomers[0], autoshipOrder.Order.OrderShipments[0]).OrderBy(sm => sm.ShippingAmount);
			var shippingMethodModel = new MiniShopShippingMethodModel(shippingMethodList, autoshipOrder.Order.CurrencyID);

			model.LoadResources(
					Edit_GetModelData(
							autoshipOrder: autoshipOrder,
							categories: categoryModels,
							products: productModels,
							orderItems: orderItemModels,
							shippingMethods: shippingMethodModel,
							selectedShippingMethodID: autoshipOrder.Order.OrderShipments[0].ShippingMethodID
					)
			);
		}

		protected virtual void Edit_ValidateOrderItems(AutoshipOrder autoshipOrder)
		{
			if (!autoshipOrder.Order.OrderCustomers[0].OrderItems.Any())
			{
				ModelState.AddModelError(string.Empty, _errorAutoshipMustContainAtLeastOneItem);
			}
		}

		protected virtual void Edit_ValidateVolume(AutoshipOrder autoshipOrder)
		{
			decimal? minimumVolume = null;

			var autoshipSchedule = GetAutoshipSchedule(autoshipOrder);
			if (autoshipSchedule != null)
			{
				minimumVolume = autoshipSchedule.MinimumCommissionableTotal;
			}

			if ((autoshipOrder.Order.CommissionableTotal ?? 0) < (minimumVolume ?? 0))
			{
				ModelState.AddModelError(string.Empty, _errorAutoshipDoesNotMeetMinimumVolume);
			}
		}

		protected virtual AutoshipSchedule GetAutoshipSchedule(AutoshipOrder autoshipOrder)
		{
			return SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipOrder.AutoshipScheduleID);
		}

		protected virtual dynamic Edit_GetModelData(
				AutoshipOrder autoshipOrder = null,
				IEnumerable<MiniShopCategoryModel> categories = null,
				IEnumerable<MiniShopProductModel> products = null,
				//IEnumerable<MiniShopOrderItemModel> orderItems = null,
                IList<IOrderItemModel> orderItems = null,
				MiniShopShippingMethodModel shippingMethods = null,
				int? selectedShippingMethodID = null)
		{
			decimal? minimumVolume = null;
			if (autoshipOrder != null)
			{
				var autoshipSchedule = GetAutoshipSchedule(autoshipOrder);
				if (autoshipSchedule != null)
				{
					minimumVolume = autoshipSchedule.MinimumCommissionableTotal;
				}
			}

			var modelData = MiniShopModel.GetModelData(
					order: autoshipOrder != null ? autoshipOrder.Order : null,
					minimumVolume: minimumVolume,
					categories: categories,
					products: products,
					orderItems: orderItems,
					shippingMethods: shippingMethods,
					selectedShippingMethodID: selectedShippingMethodID
			);

			modelData.GetProductsUrl = Url.Action("GetAutoshipProducts");
			modelData.AddOrderItemUrl = Url.Action("AddAutoshipOrderItem");
			modelData.RemoveOrderItemUrl = Url.Action("RemoveAutoshipOrderItem");
			modelData.AddOrderItemLabel = Translation.GetTerm("AddToAutoship", "Add to Autoship");

			return modelData;
		}

		protected virtual void Edit_UpdateAutoshipOrder(EditModel model, AutoshipOrder autoshipOrder)
		{
			if (!autoshipOrder.Order.OrderCustomers.Any())
			{
				return;
			}
			if (model.MiniShop.SelectedShippingMethodID != autoshipOrder.Order.OrderShipments[0].ShippingMethodID)
			{
				autoshipOrder.Order.SetShippingMethod(model.MiniShop.SelectedShippingMethodID);
				autoshipOrder.Order = UpdateOrderUsingOrderService(autoshipOrder.Order);
			}
		}

		/// <summary>
		/// Returns a list of first-tier categories from the corresponding catalog.
		/// </summary>
		protected virtual IEnumerable<MiniShopCategoryModel> GetCategoryModels()
		{
			var inventory = Create.New<InventoryBaseRepository>();

			Catalog catalog = inventory
					.GetActiveCatalogs(_storeFrontID)
					.FirstOrDefault(x => x.CatalogTypeID == (short)Constants.CatalogType.AutoshipItems);

			if (catalog != null)
			{
				// Get all "active" categories - meaning they have products.
				var activeCategories = inventory.GetActiveCategories(_storeFrontID, CoreContext.CurrentAccount.AccountTypeID);
				if (activeCategories != null)
				{
					// Filter the "active" categories based on the catalog's category tree.
					return activeCategories
							.Where(x => x.ParentCategoryID == catalog.CategoryID)
							.OrderBy(x => x.SortIndex).ThenBy(x => x.CategoryID)
							.Select(x => new MiniShopCategoryModel().LoadResources(x, activeCategories, 1));
				}
			}

			return Enumerable.Empty<MiniShopCategoryModel>();
		}

        //protected virtual IEnumerable<MiniShopOrderItemModel> GetOrderItemModels(Order order)
        //{
        //    if (!order.OrderCustomers.Any())
        //    {
        //        return Enumerable.Empty<MiniShopOrderItemModel>();
        //    }

        //    return order.OrderCustomers[0].OrderItems
        //            .Select(x => new MiniShopOrderItemModel().LoadResources(x));
        //}

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

		protected virtual IEnumerable<MiniShopProductModel> GetProductModels(int categoryID, Order order)
		{
			if (order == null)
			{
				throw new ArgumentNullException("order");
			}

			var allProducts = Inventory.GetActiveProductsForCategory(
					_storeFrontID,
					categoryID,
					CoreContext.CurrentAccount.AccountTypeID
			);

			var validProducts = Inventory.ExcludeInvalidProducts(
					allProducts,
					CoreContext.CurrentAccount.AccountTypeID,
					order.CurrencyID
			);

			var productModels = validProducts
					.OrderByDescending(x => x.CatalogItems.Max(ci => ci.StartDateUTC))
					.Select(x => new MiniShopProductModel().LoadResources(
							x,
							CoreContext.CurrentAccount.AccountTypeID,
							order.CurrencyID,
							order.OrderTypeID
					));

			return productModels;
		}

		protected virtual void SubmitOrder(Order order, Account account)
		{
			if (order == null || !order.OrderCustomers.Any() || !order.OrderCustomers[0].OrderItems.Any())
			{
				throw new Exception(_errorOrderMissingInfoString);
			}

			order = UpdateOrderUsingOrderService(order);
			if (order.GrandTotal == null)
			{
				throw new Exception(_errorOrderMissingInfoString);
			}

			var accountPaymentMethod = account.AccountPaymentMethods
					.OrderByDescending(x => x.IsDefault)
					.FirstOrDefault();
			if (accountPaymentMethod == null)
			{
				throw new Exception(_errorOrderMissingInfoString);
			}

			order.OrderCustomers[0].RemoveAllPayments();
            var applyPaymentResponse = order.ApplyPaymentToCustomer(accountPaymentMethod.PaymentTypeID, order.GrandTotal.Value, accountPaymentMethod.NameOnCard, accountPaymentMethod);

			order = UpdateOrderUsingOrderService(order);

			if (!applyPaymentResponse.Success)
			{
				throw new Exception(applyPaymentResponse.Message);
			}

			order.SetConsultantID(account);
			var submitOrderResponse = this.SubmitOrderUsingOrderService(order);

			if (!submitOrderResponse.Success)
			{
				throw new Exception(submitOrderResponse.Message);
			}

			RefreshCoreContextAccount(account.AccountID);
		}

		private void RefreshCoreContextAccount(int accountID)
		{
			CoreContext.CurrentAccount = Account.LoadForSession(accountID);
		}

		#endregion

		#region Private Methods

		private Order UpdateOrderUsingOrderService(Order order)
		{
			var orderContext = Create.New<IOrderContext>();
			orderContext.Order = order;
			OrderService.UpdateOrder(orderContext);

			return orderContext.Order.AsOrder();
		}

		private BasicResponse SubmitOrderUsingOrderService(Order order)
		{
			var orderContext = Create.New<IOrderContext>();
			orderContext.Order = order;
			var response = OrderService.SubmitOrder(orderContext);

			return response;
		}

		#endregion
	}
}