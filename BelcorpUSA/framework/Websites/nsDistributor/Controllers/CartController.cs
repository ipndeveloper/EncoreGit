using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Dynamic;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Plugins.Common.Helpers;
using NetSteps.Promotions.Plugins.Common.Rewards.Effects.Components;
using NetSteps.Promotions.UI.Common.Interfaces;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Controls.Models;
using nsDistributor.Models.Cart;
using NetSteps.Data.Entities.Business.Common;
using NetSteps.Data.Entities.Repositories;
using OrderRules.Core.Model;
using OrderRules.Service.Interface;
using OrderRules.Service.DTO;
using OrderRules.Service.DTO.Converters;

namespace nsDistributor.Controllers
{
	public class CartController : BaseShoppingController
	{
		private string GetProductImageByProduct(Product product)
		{
			string previewImage = "../../Content/Images/Shopping/no-image.jpg".ResolveUrl();
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

		[NonAction]
		public virtual HtmlString GetDynamicBundleHtml(string parentGuid)
		{
			if (OrderContext.Order == null)
				return new HtmlString("");

			StringBuilder builder = new StringBuilder();
			int counter = 0;
			var orderItem = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.GetByGuid(parentGuid);
			var product = Inventory.GetProduct(orderItem.ProductID.ToInt());
			var dynamicKit = product.DynamicKits[0];

			foreach (var group in dynamicKit.DynamicKitGroups)
			{
				TagBuilder h5 = new TagBuilder("h5");
				h5.InnerHtml = string.Format("{0}: <span class=\"currentNumOfGroup18 currentGroupTotal\"></span>", group.Translations.Name());
				builder.Append(h5.ToString());
				var existingItemsInGroup = orderItem.ChildOrderItems.Where(index => index.DynamicKitGroupID == group.DynamicKitGroupID);
				int existingGroupCount = existingItemsInGroup.Count();
				int existingGroupExpandedCount = existingItemsInGroup.Sum(index => index.Quantity);
				foreach (var item in existingItemsInGroup)
				{
					Product childProduct = Inventory.GetProduct(item.ProductID.Value);
					for (int q = 0; q < item.Quantity; q++)
					{
						TagBuilder div = new TagBuilder("div");
						div.AddCssClass(string.Format("group{0}", counter));
						div.AddCssClass("ThumbWrapper");
						div.Attributes["rel"] = group.DynamicKitGroupID.ToString();

						div.InnerHtml = new StringBuilder()
							.Append(string.Format("<input type=\"hidden\" value=\"{0}_{1}\" class=\"sku\" />", item.Guid, childProduct.SKU))
							.Append(string.Format("<input type=\"hidden\" value=\"{0}\" class=\"orderItemId\" />", item.OrderItemID))
							.Append(string.Format("<input type=\"hidden\" value=\"{0}\" class=\"orderItemGuid\" />", item.Guid.ToString("N")))
							.Append("<a title=\"Remove\" class=\"RemoveItem\" href=\"javascript:void(0);\"><img alt=\"Remove\" src=\"/Content/Images/Shopping/button-delete.gif\" /></a>")
							.Append(string.Format("<img height=\"45\" width=\"45\" src=\"{0}\" alt=\"\" />", childProduct.MainImage == null ? Url.Content("../../Content/Images/Shopping/no-image.jpg") : childProduct.MainImage.FilePath.ReplaceFileUploadPathToken())).ToString();

						builder.Append(div.ToString());
					}

				}
				var remainingGroupPositionCount = GetRemainingProductCount(group, existingGroupExpandedCount);
				for (int i = 0; i < remainingGroupPositionCount; i++)
				{
					TagBuilder div = new TagBuilder("div");
					div.AddCssClass(string.Format("group{0}", counter));
					div.AddCssClass("ThumbWrapper");
					div.Attributes["rel"] = group.DynamicKitGroupID.ToString();
					builder.Append(div.ToString());
				}
				builder.Append("<span class=\"clrall\"></span>");
				counter++;
			}
			var results = builder.ToString();

			return new HtmlString(string.IsNullOrEmpty(results) ? "" : results);
		}

		protected virtual decimal GetDynamicBundlePrice(string parentGuid)
		{
			return 0.0m;
		}

		protected virtual int GetRemainingProductCount(DynamicKitGroup group, int existingGroupExpandedCount)
		{
			return group.MaximumProductCount - existingGroupExpandedCount;
		}

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult Index(int? orderID)
		{
			if (orderID.HasValue)
			{
				OrderContext.Order = Order.LoadFull(orderID.Value);
			}

			OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;

			if (ApplicationContext.Instance.UseDefaultBundling)
			{
				ViewBag.DynamicKitUpSaleHTML = GetDynamicBundleUpSale();
			}

			ViewBag.CartModel = GetCartModelData(OrderContext.Order.AsOrder());

			return View(OrderContext.Order as Order);
		}

		public virtual void SetOrderGiven(int? orderID)
		{

		}

		[FunctionFilter("Orders", "~/")]
		public ActionResult RetrieveDynamicBundleUpSaleHTML()
		{
			try
			{
				return Json(new
				{
					result = true,
					DynamicBundleUpSaleHTML = GetDynamicBundleUpSale()
				});
			}
			catch (Exception ex)
			{
				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				else
					exception = ex as NetStepsException;

				return Json(new
				{
					result = false
				});
			}
		}

		[NonAction]
		public virtual string GetDynamicBundleUpSale()
		{
			var customer = OrderContext.Order.AsOrder().OrderCustomers[0];
			if (customer.IsTooBigForBundling())
			{
				return string.Empty;
			}

			var possibleDynamicKitProducts = OrderContext.Order.AsOrder().GetPotentialDynamicKitUpSaleProducts(customer, OrderContext.SortedDynamicKitProducts);
			var sb = new StringBuilder();
			foreach (var product in possibleDynamicKitProducts)
			{
				sb.Append("<div><p>").Append(product.Translations.Name()).Append("<input type=\"hidden\" class=\"dynamicKitProductSuggestion\" value=\"").Append(product.ProductID).Append("\" /></p>")
					.Append("<a href=\"javascript:void(0);\" class=\"CreateBundle Button FL mr10\"><span>Yes, I'd like to create a bundle</span></a>")
					.Append("<span class=\"clrall\"></span></div><br/>");
			}

			return sb.ToString();
		}

		[NonAction]
		private string ConvertToDynamicKit(Product kitProduct)
		{
			if (OrderContext.Order == null || kitProduct == null)
			{
				return string.Empty;
			}

			var customer = OrderContext.Order.AsOrder().OrderCustomers[0];
			var parentGuid = OrderContext.Order.AsOrder().ConvertToDynamicKit(customer, kitProduct);
			OrderService.UpdateOrder(OrderContext);

			return parentGuid;
		}

		protected virtual string GetProductShortDescriptionDisplay(Product product)
		{
			Contract.Requires<ArgumentNullException>(product != null);
			string shortDesc = product.Translations.ShortDescription();
			if (shortDesc.IsNullOrWhiteSpace())
			{
				shortDesc = product.ProductBase.Translations.ShortDescription();
			}

			return shortDesc ?? string.Empty;
		}

		[FunctionFilter("Orders", "~/")]
		public ActionResult CreateDynamicBundleUpSale(int productId)
		{
			var originalOrder = OrderContext.Order.AsOrder().Clone();
			var customer = OrderContext.Order.AsOrder().OrderCustomers[0];

			try
			{
				var product = Inventory.GetProduct(productId);
				var kitGuid = ConvertToDynamicKit(product);

				if (kitGuid.IsNullOrEmpty())
				{
					return Json(new
					{
						result = false,
						guid = kitGuid,
						message = Translation.GetTerm("BundleCouldNotBeCreated", "The bundle could not be created.  Please try again.")
					});
				}

				return Json(new
				{
					result = true,
					guid = kitGuid
				});
			}
			catch (Exception ex)
			{
				//something went wrong in conversion to kit so revert
				OrderContext.Order = originalOrder; // Revert to original party object (before these modifications) to avoid a possibly corrupted object graph - JHE

				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				else
					exception = ex as NetStepsException;

				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

        // CSTI(mescobar)-28-01-2015-Inicio
		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult Submit()
		{
			if (OrderContext.Order == null)
				return RedirectToAction("Index");

            // Developed by BAL - CSTI - AINI
            string strSKUs = "";
            foreach (var item in OrderContext.Order.OrderCustomers[0].OrderItems)
            {
                if (ProductQuotasRepository.ProductIsRestricted(item.ProductID.Value, item.Quantity, Account.AccountID, Account.AccountTypeID))
                    strSKUs += " *" + Inventory.GetProduct(item.ProductID.Value).SKU;
            }

            if (!strSKUs.IsEmpty())
                return Json(new
                {
                    result = false,
                    restricted = true,
                    message = Translation.GetTerm("ProductCantPurchased", "The added product can not be purchased"),
                    products = strSKUs
                });
            // Developed by BAL - CSTI - AFIN

            foreach (OrderCustomer customer in OrderContext.Order.OrderCustomers)
            {
                foreach (var orderItem in customer.OrderItems)
                {
                    var product = Inventory.GetProduct(orderItem.ProductID.Value);
                    if (!Order.IsDynamicKitValid(orderItem))
                    {
                        return Json(new
                        {
                            result = false,
                            validrule = false, // CGI(CMR)-06/05/2015-Se agregó validrule
                            message = Translation.GetTerm("TheBundleYouTriedToOrderIsNotComplete", "The bundle you tried to order ({0}) is not complete.", product.Translations.Name())
                        });
                    }

                    if (!Order.IsStaticKitValid(orderItem))
                    {
                        return Json(new
                        {
                            result = false,
                            validrule = false, // CGI(CMR)-06/05/2015-Se agregó validrule
                            message = Translation.GetTerm("TheKitYouTriedToOrderIsNotComplete", "The kit you tried to order ({0}) is not complete.", product.Translations.Name())
                        });
                    }
                }
            }

            // CGI(JCT)-2490-13/05/2015-Inicio
            OrderCustomer orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
            var addedItemOperationID = 1;
            var nonPromotionalItems = orderCustomer.ParentOrderItems.Where(x => !x.OrderAdjustmentOrderLineModifications.Any(y => y.ModificationOperationID == addedItemOperationID));

            /*Reglas de Salida*/
            decimal qvRule = 0;
            decimal retailRule = 0;
            decimal subtotalRule = (decimal)OrderContext.Order.Subtotal;
            /*Reglas de Entrada*/
            List<int> productsRule = new List<int>();
            List<int> productTypesRule = new List<int>();
            int constQV = (int)NetSteps.Data.Entities.Constants.ProductPriceType.QV;
            int constRetail = (int)NetSteps.Data.Entities.Constants.ProductPriceType.Retail;
            foreach (OrderItem orderItem in nonPromotionalItems)
            {
                var productInfo = Product.Load((int)orderItem.ProductID);
                productsRule.Add(productInfo.ProductID);
                productTypesRule.Add(ProductBase.Load(productInfo.ProductBaseID).ProductTypeID);
                qvRule += (orderItem.Quantity * (decimal)orderItem.OrderItemPrices
                                .Where(x => x.ProductPriceTypeID == constQV)
                                    .Select(y => y.OriginalUnitPrice).FirstOrDefault());
                retailRule += (orderItem.Quantity * (decimal)orderItem.OrderItemPrices
                                .Where(x => x.ProductPriceTypeID == constRetail)
                                    .Select(y => y.OriginalUnitPrice).FirstOrDefault());
            }
            int storeFrontRule = ApplicationContext.Instance.StoreFrontID;
            short orderTypeRule = OrderContext.Order.OrderTypeID;
            int accountRule = orderCustomer.AccountID;
            short accountTypeRule = orderCustomer.AccountTypeID;
           
            var ruleOrder = Create.New<IOrderRulesService>().GetRules().ToList();

            var ruleBasicFilter = Create.New<IOrderRulesService>().GetRules().Where(x => x.RuleStatus == (int)RuleStatus.Active &&
                                                                  (x.StartDate.IsNullOrEmpty() ? true : x.StartDate <= DateTime.Now ? true : false) &&
                                                                  (x.EndDate.IsNullOrEmpty() ? true : x.EndDate >= DateTime.Now ? true : false)).ToList();


            List<RulesDTO> dtoRuleComparer = new List<RulesDTO>();
            var ordeRuleConverter = Create.New<OrderRuleConverter<Rules, RulesDTO>>();
            foreach (var item in ruleBasicFilter)
            {
                dtoRuleComparer.Add(ordeRuleConverter.Convert(item));
            }


            /*Filtrar Reglas a las que aplica la order*/
            var appliedRules = dtoRuleComparer.Where(x => (x.RuleValidationsDTO.Where(y => (y.AccountIDs.Count == 0 ? true : y.AccountIDs.Contains(accountRule) ? true : false)
                                                                                && (y.AccountTypeIDs.Count == 0 ? true : y.AccountTypeIDs.Contains(accountTypeRule) ? true : false)
                                                                                && (y.OrderTypeIDs.Count == 0 ? true : y.OrderTypeIDs.Contains(orderTypeRule) ? true : false)
                                                                                && (y.StoreFrontIDs.Count == 0 ? true : y.StoreFrontIDs.Contains(storeFrontRule) ? true : false)
                                                                                && (y.ProductIDs.Count == 0 ? true : productsRule.Distinct().Intersect(y.ProductIDs).Any() ? true : false)
                                                                                && (y.ProductTypeIDs.Count == 0 ? true : productTypesRule.Distinct().Intersect(y.ProductTypeIDs).Any() ? true : false)).Any())).ToList();

            /*Filtrar Reglas no cumplidas*/
            var unfulfilledRules = appliedRules.Where(x => (x.RuleValidationsDTO.Where(y => (y.CustomerPriceSubTotalDTO.Count == 0 ? false : y.CustomerPriceSubTotalDTO.FirstOrDefault().MinimumAmount > subtotalRule ? true : false)
                                                                                        || (y.CustomerPriceTotalDTO.Count == 0 ? false : (y.CustomerPriceTotalDTO
                                                                    .Where(z => z.ProductPriceTypeID == constQV && z.MinimumAmount > qvRule).Any() || y.CustomerPriceTotalDTO
                                                                    .Where(z => z.ProductPriceTypeID == constRetail && z.MinimumAmount > retailRule).Any()))).Any())).ToList();

            /*Concatenar mensajes*/
            var messageRule = string.Empty;
            foreach (var faildRule in unfulfilledRules)
            {
                TermTranslation translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == faildRule.TermName && tt.LanguageID == CurrentLanguageID);
                if (translation == default(TermTranslation))
                {
                    translation = TermTranslation.Repository.FirstOrDefault(tt => tt.TermName == faildRule.TermName);
                    messageRule += translation.Term;
                }
                else
                {
                    messageRule += translation.Term;
                }
            }
            ruleBasicFilter = null; 

            if (!messageRule.IsEmpty())
            {
                return Json(new { result = false, validrule = true, message = messageRule });
            }
            // CGI(JCT)-2490-13/05/2015-Fin
			return Json(new { result = true });
		}
        // CSTI(mescobar)-28-01-2015-Fin

		protected virtual IEnumerable<IOrderItem> AddOrUpdateOrderItems(IEnumerable<IOrderItemQuantityModification> changes)
		{
			IEnumerable<IOrderItem> results = null;

			try
			{
				results = OrderService.UpdateOrderItemQuantities(OrderContext, changes);
			}
			catch (Exception ex)
			{

				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (OrderContext.Order != null) ? OrderContext.Order.OrderID.ToIntNullable() : null, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
				else
					exception = ex as NetStepsException;
				throw exception;
			}

			return results;
		}

		#region free gift modal actions
		public virtual ActionResult GetGiftStepInfo(string stepId)
		{
			try
			{
				var allSteps = OrderContext.InjectedOrderSteps.Union(OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
				var step = (IUserProductSelectionOrderStep)allSteps.Single(os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
				var inventoryService = Create.New<IInventoryService>();
				var options = step.AvailableOptions.Select(o =>
				{
					var product = Inventory.GetProduct(o.ProductID);
					var inventoryResult = inventoryService.GetProductAvailabilityForOrder(OrderContext, product.ProductID, 1);
					int currencyID = OrderContext.Order.CurrencyID;
					return new GiftModel
					{
						Name = product.Name,
						Image = GetProductImageByProduct(product),
						Description = GetProductShortDescriptionDisplay(product),
						ProductID = product.ProductID,
						Value = product.GetPriceByPriceType((int)Constants.ProductPriceType.Retail, currencyID).ToString(currencyID),
						IsOutOfStock = inventoryResult.CanAddBackorder == 0 && inventoryResult.CanAddNormally == 0
					};
				});

				var giftSelectionModel = new GiftSelectionModel(Url.Action("GetGiftStepInfo"), Url.Action("AddGifts"), callbackFunctionName: "updateCartDisplay");
				giftSelectionModel.StepID = stepId;
				giftSelectionModel.MaxQuantity = step.MaximumOptionSelectionCount;

				var selections = step.Response == null ? Enumerable.Empty<IProductOption>() : ((IUserProductSelectionOrderStepResponse)step.Response).SelectedOptions;
				giftSelectionModel.SelectedOptions = selections.Select(p => options.First(o => o.ProductID == p.ProductID)).ToList();
				giftSelectionModel.AvailableOptions = options.ToList();

				return Json(new { result = true, GiftSelectionModel = giftSelectionModel });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult AddGifts(List<int> productIds, string stepId)
		{
			try
			{
				var response = Create.New<IUserProductSelectionOrderStepResponse>();
				if (productIds != null)
				{
					foreach (var productId in productIds)
					{
						var option = Create.New<IProductOption>();
						option.ProductID = productId;
						option.Quantity = 1;
						response.SelectedOptions.Add(option);
					}
				}
				var allSteps = OrderContext.InjectedOrderSteps.Union(OrderContext.Order.OrderAdjustments.SelectMany(oa => oa.InjectedOrderSteps));
				var step = (IUserProductSelectionOrderStep)allSteps.Single(os => os is IUserProductSelectionOrderStep && os.OrderStepReferenceID.ToString() == stepId);
				step.Response = response;
				OrderService.UpdateOrder(OrderContext);

				return Json(new
				{
					result = true,
					//orderItems = GetOrderItemsHtml(CoreContext.CurrentOrder),
					//totals = Totals,
					CartModel = GetCartModelData(OrderContext.Order.AsOrder()),
					//promotions = GetApplicablePromotions(CoreContext.CurrentOrder),
					//shippingMethods = ShippingMethods,
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		[FunctionFilter("Orders", "~/", NetSteps.Data.Entities.Constants.SiteType.Corporate)]
		public virtual ActionResult Add(string sku, int quantity, string parentGuid = null, int? dynamicKitGroupId = null, IDictionary<string, string> itemProperties = null)
		{
			try
			{
				if (OrderContext.Order.AsOrder() == null || OrderContext.Order.AsOrder().OrderStatusID != (int)Constants.OrderStatus.Pending)
				{
					OrderContext.Order = CreateNewOrder();
				}

				OrderContext.Order.AsOrder().OrderPendingState = Constants.OrderPendingStates.Open;

				var product = Inventory.GetProduct(sku);
				if (product == null)
				{
					return Json(new { result = false, message = string.Format("Could not find a product with SKU '{0}'", sku) });
				}

				var allow = true;
				var showOutOfStockMessage = false;

                if (Product.CheckStock(product.ProductID, CoreContext.CurrentOrder.GetDefaultShipment()).IsOutOfStock)
                {
                    switch (product.ProductBackOrderBehaviorID)
                    {
                        case (int)ConstantsGenerated.ProductBackOrderBehavior.AllowBackorder:
                            break;
                        case (int)ConstantsGenerated.ProductBackOrderBehavior.AllowBackorderInformCustomer:
                            allow = true;
                            showOutOfStockMessage = true;
                            break;
                        case (int)ConstantsGenerated.ProductBackOrderBehavior.ShowOutOfStockMessage:
                            allow = false;
                            showOutOfStockMessage = true;
                            break;
                        case (int)ConstantsGenerated.ProductBackOrderBehavior.Hide:
                            allow = false;
                            break;
                    }
                }

                 string messageValidKit = "";
                 //Convert.ToInt32(Session["WareHouseId"])
                 int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;
                 foreach (var item in Order.GenerateAllocation(product.ProductID, 
                                                               quantity, 
                                                               OrderContext.Order.AsOrder().OrderID,
                                                               Convert.ToInt32(Session["WareHouseId"]), 
                                                               EntitiesEnums.MaintenanceMode.Add,
                                                               Convert.ToInt32(Session["PreorderID"]) ,
                                                               CoreContext.CurrentAccount.AccountTypeID, false))
                {
                    if (!item.Estado)
                    {
                        return Json(new { result = false, restricted = true, message = item.Message });
                    }
                    else if (item.EstatusNewQuantity)
                    {
                        messageValidKit = item.Message;
                        quantity = item.NewQuantity;
                    }
                    else if (item.Estado && item.Message != "" && item.NewQuantity > 0)
                    {
                        messageValidKit = item.Message;
                        quantity = item.NewQuantity;
                    }
                }


				var isDynamicKit = product.IsDynamicKit();
				var orderCustomer = OrderContext.Order.AsOrder().OrderCustomers[0];
				bool isBundleGroupComplete = false;
				OrderItem bundleItem = null;
				Product bundleProduct = null;
				DynamicKitGroup group = null;

				if (!parentGuid.IsNullOrEmpty() && dynamicKitGroupId.HasValue)
				{
					bundleItem = orderCustomer.OrderItems.FirstOrDefault(oc => oc.Guid.ToString("N") == parentGuid);
					bundleProduct = Inventory.GetProduct(bundleItem.ProductID.Value);
					group = bundleProduct.DynamicKits[0].DynamicKitGroups.FirstOrDefault(g => g.DynamicKitGroupID == dynamicKitGroupId);
					int currentCount = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
					if ((currentCount + quantity) > group.MinimumProductCount)
					{
						return Json(new
						{
							result = false,
							message = string.Format(Translation.GetTerm("TheItemCouldNotBeAddedInvalidQuantityOf", "The product could not be added. Invalid quantity of {0}."), quantity)
						});
					}
				}


               

				IEnumerable<IOrderItem> addedItems = null;

				if (allow)
				{
					var mod = Create.New<IOrderItemQuantityModification>();
					mod.ProductID = product.ProductID;
					mod.Quantity = quantity;
					mod.ModificationKind = OrderItemQuantityModificationKind.Add;
					mod.Customer = OrderContext.Order.OrderCustomers[0];

					if (bundleItem != null && bundleProduct != null && group != null)
					{
						OrderService.AddOrderItemsToOrderBundle(OrderContext, bundleItem, new IOrderItemQuantityModification[] { mod }, dynamicKitGroupId.Value);
						int currentCount = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == dynamicKitGroupId).Sum(oi => oi.Quantity);
						isBundleGroupComplete = currentCount == group.MinimumProductCount;
					}
					else
					{
						addedItems = AddOrUpdateOrderItems(new IOrderItemQuantityModification[] { mod });
					}

					if (itemProperties != null && itemProperties.Any() && addedItems != null && addedItems.Any())
					{
						var modProps = Create.New<IOrderItemPropertyModification>();
						modProps.Customer = orderCustomer;
						modProps.ExistingItem = addedItems.First();
						foreach (var kvp in itemProperties)
						{
							modProps.Properties.Add(kvp.Key, kvp.Value);
						}
						OrderService.UpdateOrderItemProperties(OrderContext, new IOrderItemPropertyModification[] { modProps });
					}

					OrderService.UpdateOrder(OrderContext);
				}

				//Due to split payments, we can only reasonably add to a payment if there is only 1 active payment.
				var orderPayments = orderCustomer.OrderPayments.Where(p => p.OrderPaymentStatusID != Constants.OrderPaymentStatus.Completed.ToShort() && p.OrderPaymentStatusID != Constants.OrderPaymentStatus.Declined.ToShort() && p.OrderPaymentStatusID != Constants.OrderPaymentStatus.Cancelled.ToShort()).ToList();
				if (orderPayments.Count == 1 && orderPayments[0].Amount != OrderContext.Order.AsOrder().GrandTotal.ToDecimal())
				{
					orderPayments[0].Amount = OrderContext.Order.AsOrder().GrandTotal.ToDecimal();
					OrderService.UpdateOrder(OrderContext);
				}
                 
				bool giftsAvailable = OrderContext.Order.OrderAdjustments.SelectMany(o => o.InjectedOrderSteps).Any(s => StepHasAnItemInStockToBeChosen(s.OrderStepReferenceID));
				//available promotions will have no steps (i.e. discount) or they will not contain unavailable items.
				var unavailablePromotions = OrderContext.Order.OrderAdjustments.Where(oa => oa.OrderModifications.Any(om => om.ModificationDescription.Contains("Unable")));
				return Json(new
				{
					result = true,
					availablePromotions = OrderContext.Order.OrderAdjustments.Except(unavailablePromotions).Select(oa => oa.Description),
					unavailablePromotions = unavailablePromotions.Select(oa => oa.Description),
					hasFreeGiftSteps = OrderContext.Order.OrderAdjustments.Any(oa => oa.InjectedOrderSteps.Any()),
					giftsAvailable = giftsAvailable,
					itemsInCart = orderCustomer.ParentOrderItems.Sum(poi => poi.Quantity),
					discountTotal = OrderContext.Order.AsOrder().Subtotal.ToString(OrderContext.Order.AsOrder().CurrencyID),
					total = OrderContext.Order.AsOrder().SubtotalAdjusted.ToString(OrderContext.Order.AsOrder().CurrencyID),
					currencyID = OrderContext.Order.AsOrder().CurrencyID,
					allow,
					showOutOfStockMessage,
					productName = product.Translations.Name(),
					image = GetProductImageByProduct(product),
					//orderItems = GetFormattedCartPreview().ToString(),
					isBundle = isDynamicKit,
					bundleGuid = isDynamicKit ? OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.FirstOrDefault(i => i.ProductID == product.ProductID && !i.HasChildOrderItems).Guid.ToString("N") : string.Empty,
					productId = product.ProductID,
					bundleItemsHtml = parentGuid == null ? "" : GetDynamicBundleHtml(parentGuid).ToString(),
					bundlePricing = parentGuid == null ? "" : GetDynamicBundlePrice(parentGuid).ToString(OrderContext.Order.CurrencyID),
					childItemCount = parentGuid == null ? 0 : orderCustomer.OrderItems.GetByGuid(parentGuid).ChildOrderItems.Sum(c => c.Quantity),
					CartModel = GetCartModelData(OrderContext.Order.AsOrder()),
                    message = messageValidKit
				});
			}
			catch (Exception ex)
			{
				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				else
					exception = ex as NetStepsException;
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult AddBundle(int productId, string bundleGuid)
		{
			try
			{
				var kitProduct = Inventory.GetProduct(productId);
				if (kitProduct == null)
					return Json(new { result = false, message = string.Format("Could not find a product with SKU '{0}'", kitProduct.SKU) });

				var orderItem = OrderContext.Order.AsOrder().OrderCustomers[0].OrderItems.GetByGuid(bundleGuid);
				if (!Order.IsDynamicKitValid(orderItem))
					return Json(new { result = false, message = Translation.GetTerm("TheBundleYouTriedToOrderIsNotComplete", "The bundle you tried to order ({0}) is not complete.", kitProduct.Translations.Name()) });

				return Json(new
				{
					result = true,
					itemsInCart = OrderContext.Order.OrderCustomers[0].OrderItems.Count,
					total = OrderContext.Order.Subtotal.ToString(OrderContext.Order.CurrencyID),
					productName = kitProduct.Translations.Name(),
					image = kitProduct.MainImage == null ? "" : kitProduct.MainImage.FilePath.ReplaceFileUploadPathToken(),
					//orderItems = GetFormattedCartPreview().ToString()
				});
			}
			catch (Exception ex)
			{
				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				else
					exception = ex as NetStepsException;
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult Update(List<ProductQuantityContainer> products)
		{
			try
			{
				if (OrderContext.Order == null)
					return Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Your session has timed out.") });

				if (products == null || products.Count == 0)
					return Json(new { result = false, message = Translation.GetTerm("ErrorUpdatingCartNoItems", "Error updating cart. No items specified.") });

				var orderCustomer = (OrderCustomer)OrderContext.Order.OrderCustomers[0];
				var changes = products
					.Select(item =>
					{
						var mod = Create.New<IOrderItemQuantityModification>();
						mod.ProductID = item.ProductID;
						mod.Quantity = item.Quantity;
						mod.ModificationKind = OrderItemQuantityModificationKind.SetToQuantity;
						mod.Customer = orderCustomer;
						return mod;
					});
                string messageValidKit = "";
                int productID = 0;
                int quantity = 0;

                int ppt = OrderContext.Order.AsOrder().OrderCustomers[0].ProductPriceTypeID;
                foreach (var p in products)
                {
                    foreach (var item in Order.GenerateAllocation(p.ProductID, 
                                                                  p.Quantity, 
                                                                  OrderContext.Order.AsOrder().OrderID, 
                                                                  Convert.ToInt32(Session["WareHouseId"]),
                                                                  EntitiesEnums.MaintenanceMode.Update,
                                                                  Convert.ToInt32(Session["PreorderID"]),
                                                                  CoreContext.CurrentAccount.AccountTypeID, false))
                    {
                        if (!item.Estado)
                        {
                            return Json(new { result = false, restricted = true, message = item.Message });
                        }
                        else if (item.EstatusNewQuantity)
                        {
                            messageValidKit = item.Message;
                            quantity = item.NewQuantity;
                        }
                        else if (item.Estado && item.Message != "" && item.NewQuantity > 0)
                        {
                            messageValidKit = item.Message;
                            p.Quantity = item.NewQuantity;
                        }
                    }
                }
				OrderService.UpdateOrderItemQuantities(OrderContext, changes);
				OrderService.UpdateOrder(OrderContext);

				var outOfStockProducts = GetOutOfStockProducts(orderCustomer);
				ViewBag.OutOfStockProducts = outOfStockProducts;

				return Json(new
				{
					result = true,
					subtotal = orderCustomer.Subtotal.ToString(OrderContext.Order.CurrencyID),
					adjustedSubtotal = orderCustomer.AdjustedSubTotal.ToString(OrderContext.Order.CurrencyID),
					outOfStockProducts = outOfStockProducts,
					CartModel = GetCartModelData(OrderContext.Order.AsOrder()),
				});
			}
			catch (Exception ex)
			{
				NetStepsException exception;
				if (!(ex is NetStepsBusinessException))
					exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				else
					exception = ex as NetStepsException;
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult UpdateOrderItemByGuid(Dictionary<string, int> updates)
		{
			try
			{
				if (OrderContext.Order == null)
				{
					return Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Your session has timed out.") });
				}

				var customer = (OrderCustomer)OrderContext.Order.OrderCustomers.FirstOrDefault();
				foreach (KeyValuePair<string, int> update in updates)
				{
					var orderItemGuid = update.Key;
					var quantity = update.Value;
					var item = customer.OrderItems.FirstOrDefault(oi => oi.Guid.ToString("N") == orderItemGuid);
					if (item == null)
						return Json(new
						{
							result = false,
							message = string.Format("Could not find item '{0}'", orderItemGuid)
						});

					OrderContext.Order.AsOrder().UpdateItem(customer, item, quantity);
				}
				OrderService.UpdateOrder(OrderContext);

				StringBuilder builder = new StringBuilder();
				int count = 0;
				foreach (var orderItem in OrderContext.Order.AsOrder().OrderCustomers[0].ParentOrderItems)
				{
					var product = Inventory.GetProduct(orderItem.ProductID.ToInt());

					TagBuilder tr = new TagBuilder("tr");
					if (count % 2 == 0)
						tr.AddCssClass("AltRow");

					count++;

					if (product.IsDynamicKit())
						tr.AddCssClass("DynamicKit");

					var rowBuilder = new StringBuilder()
						.AppendCell(string.Format("<a id=\"remove{0}\" href=\"javascript:void(0);\" class=\"UI-icon-container icon-24 Delete\" title=\"{1}\"><span class=\"UI-icon icon-x\"></span></a><img class=\"loading\" src=\"{2}\" alt=\"\" height=\"15\" width=\"15\" style=\"display:none;margin:3px 0 0 10px;padding: 1px;\" />", orderItem.Guid.ToString("N"), Translation.GetTerm("RemoveFromCart", "Remove from cart"), "~/Resource/Content/Images/loading.gif".ResolveUrl()), "center noText")
						//.AppendCell(string.Format("<a id=\"remove{0}\" href=\"javascript:void(0);\" class=\"UI-icon-container icon-24 Delete\" title=\"{1}\"><span class=\"UI-icon icon-x\"></span></a><img class=\"loading\" src=\"{2}\" alt=\"\" height=\"15\" width=\"15\" style=\"display:none;margin:3px 0 0 10px;padding: 1px;\" />", orderItem.OrderItemID, Translation.GetTerm("RemoveFromCart", "Remove from cart"), "~/Resource/Content/Images/loading.gif".ResolveUrl()), "center noText")
						.AppendCell(string.Format("<input type=\"hidden\" class=\"productId\" value=\"{0}\" /><input type=\"hidden\" class=\"guid\" value=\"{1}\" />{2}", product.ProductID, orderItem.Guid.ToString("N"), product.SKU))
						.AppendCell(string.Format("<img src=\"{0}\" alt=\"\" width=\"38\" />", product.MainImage == null ? "../../Content/Images/Shopping/no-image.jpg".ResolveUrl() : product.MainImage.FilePath.ReplaceFileUploadPathToken()), "center CartThumb");

					var productNameColumn = new StringBuilder(product.Translations.Name());
					if (product.IsStaticKit() || product.IsDynamicKit())
					{
						productNameColumn.Append("<span class=\"ClearAll\"></span>");
						if (product.IsDynamicKit())
						{
							//productNameColumn.Append("<a class=\"EditKit TextLink\" href=\"javascript:void(0);\">")
							//    .Append(Translation.GetTerm("EditBundle", "Edit Bundle")).Append("</a><br />");
						}

						productNameColumn.Append("<a class=\"ViewKitContents TextLink Add\" href=\"javascript:void(0);\">")
						.Append(Translation.GetTerm("ViewKitContents", "View Kit Contents")).Append("</a><div class=\"KitContents\" style=\"display: none;\">")
						.Append("<table width=\"100%\" cellspacing=\"0\"><tbody><tr><th>&nbsp; </th><th>")
						.Append(Translation.GetTerm("SKU")).Append("</th><th>").Append(Translation.GetTerm("Product"))
						.Append("</th><th>").Append(Translation.GetTerm("Quantity"))
						.Append("</th></tr>");
						foreach (var childItem in orderItem.ChildOrderItems)
						{
							var childProduct = Inventory.GetProduct(childItem.ProductID.Value);
							productNameColumn.Append("<tr>")
								.AppendCell(string.Format("<img width=\"25\" src=\"{0}\" alt=\"\" />", childProduct.MainImage == null ? "../../Content/Images/Shopping/no-image.jpg".ResolveUrl() : childProduct.MainImage.FilePath.ReplaceFileUploadPathToken()))
								.AppendCell(childProduct.SKU, "KitSKU")
								.AppendCell(childProduct.Translations.Name())
								.AppendCell(string.Format("{0}", childItem.Quantity))
								.Append("</tr>");
						}
						productNameColumn.Append("</tbody></table></div>");
					}
					string strQuantityCellContents = string.Empty;
					string cssClasses = string.Empty;
					if (product.IsDynamicKit() && !ApplicationContext.Instance.AllowBundleQuantityUpdate)
					{
						strQuantityCellContents = string.Format("<input type=\"hidden\" class=\"quantity\" value=\"{0}\" />{0}", orderItem.Quantity);
						cssClasses = "center";
					}
					else
					{
						strQuantityCellContents = string.Format("<input type=\"text\" class=\"TextInput quantity\" value=\"{0}\" />", orderItem.Quantity);
						cssClasses = "center cartQty";
					}
					rowBuilder.AppendCell(productNameColumn.ToString())
						//.AppendCell("-$10.00") // TODO: Change this to not be hardcoded later - JHE
						 .AppendCell(orderItem.ItemPrice.ToString(OrderContext.Order.CurrencyID))
						 .AppendCell(strQuantityCellContents, cssClasses)
						 .AppendCell(string.Format("<p>{0}</p>", (orderItem.GetAdjustedPrice() * orderItem.Quantity).ToString(OrderContext.Order.CurrencyID)), "right")
						//.Append("<td><span class=\"block strikethrough price OriginalPrice\">")
						//.Append(orderItem.AdjustedPrice.ToString(Order.CurrencyID))
						//.Append("</span> ")
						//.Append(((orderItem.Quantity * orderItem.ItemPrice) - (orderItem.Quantity * 10)).ToString(Order.CurrencyID))
						//.Append(" <span class=\"block price PromotionalPrice\">(")
						//.Append((orderItem.ItemPrice - 10).ToString(Order.CurrencyID))
						//.Append(" each)</span></td>")
						 .ToString();

					tr.InnerHtml = rowBuilder.ToString();

					builder.Append(tr.ToString());
				}

				return Json(new { result = true, orderItems = builder.ToString(), subtotal = OrderContext.Order.Subtotal.ToString(OrderContext.Order.CurrencyID) });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult Remove(string orderItemGuid, string parentGuid = null, int? quantity = null)
		{
			try
			{
				if (OrderContext.Order == null)
				{
					return Json(new { result = false, message = Translation.GetTerm("SessionTimedOut", "Your session has timed out.") });
				}

				var customer = OrderContext.Order.AsOrder().OrderCustomers.First();
				var item = customer.OrderItems.FirstOrDefault(oi => oi.Guid.ToString("N") == orderItemGuid);
				if (item == null)
					return Json(new { result = false, message = string.Format("Could not find item '{0}'", orderItemGuid) });

				var removeModification = Create.New<IOrderItemQuantityModification>();
				removeModification.Customer = customer;
				removeModification.ModificationKind = OrderItemQuantityModificationKind.Delete;
				removeModification.ProductID = item.ProductID.Value;
				removeModification.Quantity = 0;
				removeModification.ExistingItem = item;

				var changes = new IOrderItemQuantityModification[] { removeModification };
				OrderService.UpdateOrderItemQuantities(OrderContext, changes);
				OrderService.UpdateOrder(OrderContext);

				var outOfStockProducts = GetOutOfStockProducts(customer);
				ViewBag.OutOfStockProducts = outOfStockProducts;

				return Json(new
				{
					result = true,
					promotions = GetApplicablePromotions(OrderContext.Order.AsOrder()),
					remainingProductDiscounts = RemainingProductDiscounts,
					itemsInCart = customer.ParentOrderItems.Sum(poi => poi.Quantity),
					adjustedSubtotal = customer.AdjustedSubTotal.ToString(OrderContext.Order.CurrencyID),
					subtotal = customer.Subtotal.ToString(OrderContext.Order.CurrencyID),
					total = OrderContext.Order.Subtotal.ToString(OrderContext.Order.CurrencyID),
					//message = Translation.GetTerm("ItemSuccessfullyRemovedFromCart","Item successfully removed from the cart"),
					bundlePricing = parentGuid == null ? "" : GetDynamicBundlePrice(parentGuid).ToString(OrderContext.Order.CurrencyID),
					totals = Totals,
					outOfStockProducts = outOfStockProducts,
					CartModel = GetCartModelData(OrderContext.Order.AsOrder())
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult GetLastPendingOrderByAccount()
		{
			return new EmptyResult();
		}

		#region Rewards

		public virtual ActionResult FinalizeHostessRewards()
		{
			try
			{
				BasicResponse response = OrderContext.Order.AsOrder().GetHostess().ValidateHostessRewards();

				return Json(new { result = response.Success, message = response.Message });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult Rewards()
		{
			if (OrderContext.Order == null)
				return RedirectToAction("Index");

			var party = OrderContext.Order.ParentOrderID == null ? null : Party.LoadFullByOrderID(OrderContext.Order.ParentOrderID.Value);
			var productDiscounts = OrderContext.Order.AsOrder().GetApplicableHostessRewardRules(CurrentSite.MarketID).Where(r => r.Products.HasValue && r.Products.Value > 0).ToList();
			var itemDiscounts = OrderContext.Order.AsOrder().GetApplicableHostessRewardRules(CurrentSite.MarketID).Where(r => r.HostessRewardTypeID == Constants.HostessRewardType.ItemDiscount.ToInt() && r.ProductDiscount.HasValue && r.ProductDiscount.Value > 0).ToList();
			var hostCredit = OrderContext.Order.AsOrder().GetApplicableHostessRewardRules(CurrentSite.MarketID).Where(r => r.HostessRewardTypeID == Constants.HostessRewardType.HostCredit.ToInt() && r.Reward.HasValue && r.Reward.Value > 0).FirstOrDefault();
			var host = (party == null || party.Order == null) ? null : party.Order.GetHostess();
			var accountId = host == null ? 0 : host.AccountID;

			//There are no rewards to redeem or it's attached to a party or the client has chosen not to have host rewards for PWS orders - DES
			// Also there should be 1 party and the host account should be the same as the user currently logged in.
			if ((OrderContext.Order.AsOrder().HostessRewardsEarned == 0 && productDiscounts.Count() == 0) || OrderContext.Order.ParentOrderID.HasValue || !ConfigurationManager.GetAppSetting<bool>("AllowHostRewardsForPWSOrder", true) || party == null || accountId != Account.AccountID)
				return RedirectToAction("Shipping", "Checkout");

			ViewBag.ProductDiscounts = productDiscounts;
			ViewBag.ItemDiscounts = itemDiscounts;
			ViewData["RemainingProductDiscounts"] = RemainingProductDiscounts;
			ViewData["HostCreditRewardRuleID"] = hostCredit != null ? hostCredit.HostessRewardRuleID : 0;

			return View(OrderContext.Order);
		}

		protected virtual IEnumerable<object> GetRewardsOrderItemsHtml(Order order, OrderCustomer orderCustomer)
		{
			return orderCustomer.OrderItems.OrderBy(oi => oi.OrderItemTypeID).Select(oi => new
			{
				orderItemId = oi.Guid.ToString("N"),
				orderItem = GetRewardsOrderItemHtml(order, oi)
			});
		}

		protected virtual object GetRewardsOrderItemHtml(Order order, OrderItem orderItem)
		{
			decimal commissionableTotal = (orderItem.CommissionableTotalOverride.HasValue) ? orderItem.CommissionableTotalOverride.ToDecimal() : orderItem.CommissionableTotal.ToDecimal();
			var product = Inventory.GetProduct(orderItem.ProductID.ToInt());
			var builder = new StringBuilder().Append("<tr id=\"oi").Append(orderItem.Guid.ToString("N")).Append("\">");
			if (orderItem.IsHostReward)
			{
				builder.AppendCell("<a href=\"javascript:void(0);\" title=\"Remove\" class=\"UI-icon icon-x RemoveOrderItem\"><input type=\"hidden\" class=\"orderItemId\" value=\"" + orderItem.Guid.ToString("N") + "\" /></a><input type=\"hidden\" class=\"productId\" value=\"" + orderItem.ProductID + "\" />");
			}
			else
			{
				builder.AppendCell("");
			}
			builder.AppendCell(product.SKU)
				.AppendCell(product.Translations.Name())
				.AppendCell(orderItem.ItemPrice.ToString(OrderContext.Order.CurrencyID));
			if (orderItem.IsHostReward)
			{
				builder.AppendCell("<input type=\"text\" class=\"quantity\" value=\"" + orderItem.Quantity + "\" style=\"width:50px;\" />");
				if (orderItem.Discount.HasValue)
				{
					builder.AppendCell(orderItem.Discount.Value.ToString(OrderContext.Order.CurrencyID));
				}
				else if (orderItem.DiscountPercent.HasValue)
				{
					builder.AppendCell((((orderItem.ItemPrice * orderItem.Quantity) * orderItem.DiscountPercent).ToString(OrderContext.Order.CurrencyID)) + " " + Translation.GetTerm("XPercentOff", "({0} off)", orderItem.DiscountPercent.ToDecimal().ToString("P")));
				}
				else
				{
					builder.AppendCell(0M.ToString(OrderContext.Order.CurrencyID));
				}
			}
			else
			{
				builder.AppendCell(orderItem.Quantity.ToString())
					.AppendCell(0M.ToString(OrderContext.Order.CurrencyID));
			}

			return builder.AppendCell((orderItem.GetAdjustedPrice() * orderItem.Quantity).ToString(OrderContext.Order.CurrencyID))
			.Append("</tr>").ToString();
		}

		protected virtual Dictionary<string, int> RemainingProductDiscounts
		{
			get
			{
				var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
				var applicableHostRewards = OrderContext.Order.AsOrder().GetApplicableHostessRewardRules().Where(r => r.Products.HasValue && r.Products.Value > 0);

				return applicableHostRewards.ToDictionary(r => r.HostessRewardRuleID.ToString(), r => r.Products.ToInt() - customer.OrderItems.Where(oi => oi.OrderItemTypeID == (int)Constants.OrderItemType.PercentOff && oi.DiscountPercent == r.ProductDiscount).Sum(oi => oi.Quantity));
			}
		}

		protected virtual object Totals
		{
			get
			{
				var customer = OrderContext.Order.OrderCustomers.FirstOrDefault();

				return new
				{
					subtotal = OrderContext.Order.Subtotal.ToDecimal().ToString(OrderContext.Order.CurrencyID),
					taxTotal = OrderContext.Order.AsOrder().TaxAmountTotal.ToString(OrderContext.Order.CurrencyID),
					shippingAndHandlingTotal = (OrderContext.Order.ShippingTotal + OrderContext.Order.AsOrder().HandlingTotal).ToString(OrderContext.Order.CurrencyID),
					grandTotal = OrderContext.Order.GrandTotal.ToDecimal().ToString(OrderContext.Order.CurrencyID),
					hostOverage = OrderContext.Order.AsOrder().HostessOverage.ToString(OrderContext.Order.CurrencyID),
					hostCreditRemaining = OrderContext.Order.AsOrder().GetRemainingHostessRewards().ToString(OrderContext.Order.CurrencyID)
				};
			}
		}

		public virtual ActionResult RedeemHostCredit(int productId, int quantity, int hostRewardRuleId)
		{
			try
			{
				var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
				OrderContext.Order.AsOrder().AddItem(customer, Inventory.GetProduct(productId), quantity, Constants.OrderItemType.HostCredit, -1, -1, false, hostRewardRuleId: hostRewardRuleId);

				return Json(new
				{
					result = true,
					orderItems = GetRewardsOrderItemsHtml(OrderContext.Order.AsOrder(), customer),
					totals = Totals
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult RedeemItemDiscount(int productId, int quantity, int hostRewardRuleId)
		{
			try
			{
				var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
				OrderContext.Order.AsOrder().AddItem(customer, Inventory.GetProduct(productId), quantity, Constants.OrderItemType.ItemDiscount, -1, -1, false, hostRewardRuleId: hostRewardRuleId);

				return Json(new
				{
					result = true,
					orderItems = GetRewardsOrderItemsHtml(OrderContext.Order.AsOrder(), customer),
					totals = Totals
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult RedeemPercentOff(int productId, int quantity, int hostRewardRuleId)
		{
			try
			{
				var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
				string errorMessage = string.Empty;
				BasicResponse response = customer.ValidateHostessRewardItem(quantity, hostRewardRuleId, OrderContext.Order.AsOrder());

				if (response.Success)
				{
					OrderContext.Order.AsOrder().AddItem(customer, Inventory.GetProduct(productId), quantity, Constants.OrderItemType.PercentOff, -1, -1, false, hostRewardRuleId: hostRewardRuleId);

					return Json(new
					{
						result = true,
						remainingProductDiscounts = RemainingProductDiscounts,
						orderItems = GetRewardsOrderItemsHtml(OrderContext.Order.AsOrder(), customer),
						totals = Totals
					});
				}

				return Json(new { result = false, message = errorMessage });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual ActionResult UpdateRewardQuantities(Dictionary<string, int> orderItems)
		{
			try
			{
				var customer = OrderContext.Order.AsOrder().OrderCustomers.FirstOrDefault();
				string errorMessage = string.Empty;
				bool hasInvalidItems = false;
				int invalidItemsCount = 0;
				string invalidItemsErrorMessage = string.Empty;

				if (orderItems != null)
				{
					foreach (var orderItem in orderItems)
					{
						var originalOrderItem = customer.OrderItems.FirstOrDefault(oi => oi.Guid.ToString("N") == orderItem.Key);
						var updateQuantity = orderItem.Value;
						if (originalOrderItem != null)
						{
							int quantityDiff = updateQuantity - originalOrderItem.Quantity;
							errorMessage = string.Empty;

							BasicResponse response = customer.ValidateHostessRewardItem(quantityDiff, originalOrderItem.HostessRewardRuleID, OrderContext.Order.AsOrder());

							//Only update if we are actually changing the quantity to avoid unnecessary calculations - DES
							if (response.Success && quantityDiff != 0)
							{
								OrderContext.Order.AsOrder().UpdateItem(customer, originalOrderItem, updateQuantity);

								if (originalOrderItem.OrderItemTypeID == (int)Constants.OrderItemType.HostCredit)
								{
									foreach (var hostCreditItem in customer.OrderItems.Where(oi => oi.OrderItemTypeID == (int)Constants.OrderItemType.HostCredit && oi.Guid.ToString("N") != originalOrderItem.Guid.ToString("N")))
									{
										//This is to force re-calculation for host rewards - DES
										OrderContext.Order.AsOrder().UpdateItem(customer, hostCreditItem, hostCreditItem.Quantity);
									}
								}
							}
							else if (!response.Success)
							{
								hasInvalidItems = true;
								invalidItemsCount++;
								if (invalidItemsCount > 1)
								{
									invalidItemsErrorMessage = invalidItemsErrorMessage + "<br />" + errorMessage.Clone();
								}
								else
								{
									invalidItemsErrorMessage = invalidItemsErrorMessage + errorMessage.Clone();
								}
							}
						}
					}
				}

				if (hasInvalidItems)
				{
					return Json(new
					{
						result = false,
						message = invalidItemsErrorMessage,
						remainingProductDiscounts = RemainingProductDiscounts,
						orderItems = GetRewardsOrderItemsHtml(OrderContext.Order.AsOrder(), customer),
						totals = Totals
					});
				}

				return Json(new
				{
					result = true,
					remainingProductDiscounts = RemainingProductDiscounts,
					orderItems = GetRewardsOrderItemsHtml(OrderContext.Order.AsOrder(), customer),
					totals = Totals
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult SearchRewardProducts(string query)
		{
			try
			{
				var types = OrderContext.Order.AsOrder().GetApplicableHostessRewardRules(CurrentSite.MarketID).Select(r => r.HostessRewardTypeID).Distinct();
				var catalogs = HostessRewardType.GetAvailableCatalogs(types);

				OrderShipment orderShipment = null;
				if (OrderContext.Order != null)
				{
					orderShipment = OrderContext.Order.AsOrder().OrderShipments.FirstOrDefault();
				}
				var outOfStockProducts = Product.GetOutOfStockProductIDs(orderShipment);
				return Json(Inventory.SearchProducts(NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID, query, Account.AccountTypeID, catalogsToSearch: catalogs)
					.Where(p => !outOfStockProducts.Contains(p.ProductID) || p.ProductBackOrderBehaviorID != (int)Constants.ProductBackOrderBehavior.Hide)
					.Select(p => new
					{
						id = p.ProductID,
						text = p.SKU + " - " + p.Translations.Name()
					}));
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (CoreContext.CurrentOrder != null) ? CoreContext.CurrentOrder.OrderID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
		#endregion

		[FunctionFilter("Orders", "~/", Constants.SiteType.Replicated)]
		public virtual ActionResult ApplyPromotionCode(string promotionCode)
		{
			try
			{
				if (string.IsNullOrWhiteSpace(promotionCode))
				{
					return JsonError(_errorInvalidPromotionCode(promotionCode));
				}
				var accountID = OrderContext.Order.OrderCustomers[0].AccountID;

				if (!OrderService.GetActivePromotionCodes(accountID).Contains(promotionCode))
				{
					return JsonError(Translation.GetTerm("Promotions_PromotionCodeNotFound", "Promotion Code Not Found or Already Used"));
				}

				if (OrderContext.CouponCodes.Any(existing => existing.AccountID == accountID && existing.CouponCode.Equals(promotionCode, StringComparison.InvariantCultureIgnoreCase)))
				{
					return JsonError(Translation.GetTerm("Promotions_PromotionCodeAlreadyApplied", "Promotion Code Already Applied"));
				}

				var newCode = Create.New<ICouponCode>();
				newCode.CouponCode = promotionCode;
				newCode.AccountID = accountID;
				OrderContext.CouponCodes.Add(newCode);
				OrderService.UpdateOrder(OrderContext);

				return Json(new
				{
					result = true,
					CartModel = GetCartModelData(OrderContext.Order.AsOrder())
				});
			}
			catch (Exception ex)
			{
				var message = ex.Log(orderID: CoreContext.CurrentOrder.OrderID).PublicMessage;
				return JsonError(message);
			}
		}

        #region PDF

        //[OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Orders", "~/", Constants.SiteType.Replicated)]
        public virtual ActionResult GenerarReportePDF(string orderNumber)
        {
            List<string> OrderList = new List<string>();

            OrderList.Add(orderNumber);
            return new NetSteps.Web.Mvc.ActionResults.PdfResult(string.Format("Order{0}.pdf", orderNumber), Pdf.GeneratePDFMemoryStream(OrderList));

        }

        #endregion

        protected virtual string GetPromotionCodesForUnconsumedPromotions(Order order)
		{
			return string.Empty;
		}

		protected virtual string GetPromotionsHtml(Order order)
		{
			List<IDisplayInfo> promoDisplayInfos = GetPromotionsDisplayInfo();
			string promotionsHtml = RenderRazorPartialViewToString("~/Views/Promotions/_AppliedPromotions.cshtml", promoDisplayInfos);
			return promotionsHtml;
		}

		protected virtual List<IDisplayInfo> GetPromotionsDisplayInfo()
		{
			return new List<IDisplayInfo>();
		}

		protected virtual IEnumerable<string> GetOutOfStockItemsString(Order order)
		{
			return order.OrderCustomers.SelectMany(oi => oi.ParentOrderItems)
                .Where(oi => Inventory.IsOutOfStock(Inventory.GetProduct(oi.ProductID.Value))/* && !Inventory.IsAvailable(oi.ProductID ?? 0)*/&& !oi.HasChildOrderItems)
				.Select(oi => oi.SKU + " - " + oi.ProductName);
		}

		/// <summary>
		/// Returns the data to send to the client-side viewmodel during AJAX calls.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		protected virtual IDictionary<string, object> GetOrderEntryModelData(Order order)
		{
			Contract.Requires<ArgumentNullException>(order != null);

			return LoadOrderEntryModelData(new DynamicDictionary(), order).AsDictionary();
		}

		protected virtual string _errorInvalidPromotionCode(string promotionCode)
		{
			return Translation.GetTerm("ErrorInvalidPromotionCode", "The promotion could not be applied. Invalid promotion code: '{0}'.", promotionCode);
		}

		/// <summary>
		/// Loads the data bag for the client-side viewmodel.
		/// For consistency, this method should be used for
		/// both the initial page load and for AJAX calls.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="order"></param>
		protected virtual dynamic LoadOrderEntryModelData(dynamic data, Order order)
		{
			Contract.Requires<ArgumentNullException>(order != null);
			// Code contracts rewriter doesn't work with dynamics
			if (data == null)
			{
				throw new ArgumentNullException("options");
			}

			data.Subtotal = GetSubtotal(order);
			data.OrderItemModels = GetOrderItemModels(order);

			return data;
		}

		/// <summary>
		/// Returns subtotal, formatted for the client-side viewmodel.
		/// </summary>
		/// <param name="order"></param>
		/// <returns></returns>
		protected virtual string GetSubtotal(Order order)
		{
			Contract.Requires<ArgumentNullException>(order != null);

			return OrderContext.Order.Subtotal.ToString(OrderContext.Order.CurrencyID);
		}
	}
}
