using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Attributes;

namespace nsDistributor.Controllers
{
	public class ShopController : BaseShoppingController
	{
		#region View Data for all pages
		protected override void SetViewData()
		{
			StringBuilder builder = new StringBuilder();
			var inventory = Create.New<InventoryBaseRepository>();

			var activeCategories = inventory.GetActiveCategories(ApplicationContext.Instance.StoreFrontID, Account.AccountTypeID);

			var trees = GetCatalogIDs(activeCategories, builder);
			ViewBag.ActiveCategories = activeCategories;
			ViewBag.Categories = builder.ToString();

			base.SetViewData();
		}

		/// <summary>
		/// Do not remove this method - it is overridden for clients
		/// </summary>
		protected virtual List<int> GetCatalogIDs(List<Category> activeCategories, StringBuilder builder)
		{
			var inventory = Create.New<InventoryBaseRepository>();

			var trees = new List<int>();
			foreach (Catalog catalog in inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID)
																	.Where(c => c.CatalogTypeID == (short)NetSteps.Data.Entities.Constants.CatalogType.Normal))
			{
				if (!trees.Contains(catalog.CategoryID))
				{
					Category root = inventory.GetCategoryTree(catalog.CategoryID);
					SetActiveCategories(root, activeCategories);
					builder.Append(BuildCategories(root));
					trees.Add(catalog.CategoryID);
				}
			}
			return trees;
		}

		protected virtual void SetActiveCategories(Category parent, List<Category> activeCategories)
		{
			if (activeCategories.Any(c => c.CategoryID == parent.CategoryID))
				parent.Display = true;

			if (parent.ChildCategories != null && parent.ChildCategories.Count > 0)
			{
				foreach (var child in parent.ChildCategories)
				{
					SetActiveCategories(child, activeCategories);
					if (child.Display)
						parent.Display = true;
				}
			}
		}

		protected virtual string BuildCategories(Category parent)
		{
			if (parent.ChildCategories != null && parent.ChildCategories.Count(c => c.Display) > 0)
			{
				TagBuilder childBuilder = new TagBuilder("ul");

				StringBuilder builder = new StringBuilder();
				foreach (var category in parent.ChildCategories.OrderBy(c => c.SortIndex).Where(c => c.Display))
				{
					TagBuilder liBuilder = new TagBuilder("li");
					liBuilder.AddCssClass("category");

					TagBuilder aBuilder = new TagBuilder("a");
					bool isSubdomain = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true);
					var localPath = System.Web.HttpContext.Current.Request.Url.LocalPath;
					var url = string.Format("~{0}/Shop/Category/{1}", isSubdomain ? "" : localPath.Substring(0, localPath.IndexOf('/', 1)), category.CategoryID).ResolveUrl();
					aBuilder.MergeAttribute("href", url);
					if (Request.Url.LocalPath == url)
						aBuilder.AddCssClass("selected");
					aBuilder.SetInnerText(category.Translations.GetByLanguageIdOrDefaultForDisplay().Name);

					liBuilder.InnerHtml = aBuilder.ToString() + BuildCategories(category);

					builder.Append(liBuilder.ToString());
				}

				childBuilder.InnerHtml = builder.ToString();

				return childBuilder.ToString();
			}

			return string.Empty;
		}
		#endregion

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult Index(int? partyId)
		{

			var featuredItemsCatalog = Inventory.GetCatalog("Featured Items");
			if (featuredItemsCatalog != null)
			{
				var categoryTree = Inventory.GetCategoryTree(featuredItemsCatalog.CategoryID);
				ViewBag.FeaturedItemCatalogId = featuredItemsCatalog.CatalogID;
				ViewBag.FeaturedItemCategoryTree = categoryTree;
			}

			try
			{
				if (partyId.HasValue)
				{
					var party = Party.Load(partyId.Value);
					if (party.IsOpen) OrderContext.Order.ParentOrderID = party.OrderID;
				}
			}
			catch (Exception ex)
			{
				var e = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsBusinessException);
				TempData["Error"] = e.PublicMessage;
			}
			//ViewBag.FormattedCartPreview = GetFormattedCartPreview();
			return View(CurrentSite.GetPageByUrl("/Shop"));
		}
        protected virtual Account GetCurrentAccount()
        {
            return CoreContext.CurrentAccount;
        }
        protected virtual bool IsCurrentAccountNull()
        {
            return CoreContext.CurrentAccount.IsNull();
        }

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult Category(int id = 0)
		{
            var account = GetCurrentAccount();

            // OnActionExecuting should prevent account from being null, but we should
            // check it anyway in case OnActionExecuting is ever changed / overridden.
            Session["CategoryId"] = 0;
            if (IsCurrentAccountNull())
            {
                return Redirect("~/Login?ReturnUrl=Account");
                Session["CategoryId"] = id;
            }
            if (account.Addresses.Count == 0)
            { 
                return Redirect("~/Account/EditShippingProfile/");
                Session["CategoryId"] = id;
            }
			if (id != 0)
			{
				bool hasCategoryInMarket = HasCategoryInMarket(id);

				if (hasCategoryInMarket)
				{
					ViewData["GetCategoryCount"] = GetCategoryCount();
					var category = NetSteps.Data.Entities.Category.LoadFull(id);
					return View(category);
				}
			}

			return RedirectToAction("Index");
		}

		protected virtual int GetCategoryCount()
		{
			var configCnt = ConfigurationManager.ShopCategoryCount;
			var categoryCnt = !string.IsNullOrEmpty(configCnt) ? int.Parse(configCnt) : 5;

			return categoryCnt;
		}

		protected virtual bool HasCategoryInMarket(int id)
		{
			List<Category> categoreis = Inventory.GetActiveCategories(ApplicationContext.Instance.StoreFrontID);
			bool hasProductInMarket = categoreis.Any(x => x.CategoryID == id);

			return hasProductInMarket;
		}

		protected virtual Product GetClosestMatch(IEnumerable<Product> productsWithValue, Product selectedProductVariant, int productPropertyTypeId, int? productPropertyValueId = null)
		{
			if (selectedProductVariant != null)
			{
				int greatestMatches = 0;
				Product bestMatchProduct = productsWithValue.FirstOrDefault();

				var applicableProperties = selectedProductVariant.Properties.Where(p => p.ProductPropertyTypeID != productPropertyTypeId);
				foreach (var p in productsWithValue)
				{
					int totalMatches = 0;
					foreach (var applicableProperty in applicableProperties)
					{
						if (p.Properties.Any(prop => prop.ProductPropertyTypeID == applicableProperty.ProductPropertyTypeID && prop.ProductPropertyValueID == applicableProperty.ProductPropertyValueID))
						{
							totalMatches++;
						}
					}
					if (totalMatches > greatestMatches)
					{
						bestMatchProduct = p;
						greatestMatches = totalMatches;
					}
				}
				return bestMatchProduct;

			}
			else
			{
				return productsWithValue.FirstOrDefault();
			}
		}

		protected virtual string CheckSelectionChange(Product selectedProductVariant, Product newProduct, int productPropertyTypeId)
		{
			string notification = "";
			if (selectedProductVariant != null)
			{
				var selectedProductVariantApplicableProperties = selectedProductVariant.Properties.Where(p => p.ProductPropertyTypeID != productPropertyTypeId);
				var closestMatchApplicableProperties = newProduct.Properties.Where(p => p.ProductPropertyTypeID != productPropertyTypeId);
				foreach (var prop in selectedProductVariantApplicableProperties)
				{
					if (!closestMatchApplicableProperties.Any(p => p.ProductPropertyTypeID == prop.ProductPropertyTypeID))
					{
						notification = Translation.GetTerm("MatchingProductNotFoundSelectionsChanged", "A matching product could not be found. Your selections have been changed.");
						break;
					}
					else
					{
						if (closestMatchApplicableProperties.Any(p => p.ProductPropertyTypeID == prop.ProductPropertyTypeID && p.ProductPropertyValueID != prop.ProductPropertyValueID))
						{
							notification = Translation.GetTerm("MatchingProductNotFoundSelectionsChanged", "A matching product could not be found. Your selections have been changed.");
							break;
						}
					}
				}

			}
			return notification;
		}

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult RenderProductVariants(int? productBaseId, int productId, int selectedProductVariantId, int productPropertyTypeId, int? productPropertyValueId = null)
		{
			try
			{
				Product product = Inventory.GetProduct(productId);
				ProductBase productBase = product.ProductBase;
				Product variantTemplateProduct = productBase.Products.FirstOrDefault(p => p.IsVariantTemplate);
				var allChildProducts = productBase.Products.Where(p => !p.IsVariantTemplate && p.Active);
				string notification = "";

				if (productPropertyValueId.HasValue)
				{
					var productsWithValue = allChildProducts
					.Where(p => p.Properties.Any(prop => prop.ProductPropertyTypeID == productPropertyTypeId && prop.ProductPropertyValueID == productPropertyValueId));

					if (productsWithValue.FirstOrDefault(p => p.ProductID == productId) == null)
					{
						if (productsWithValue.Count() > 0)
						{
							Product selectedProductVariant = selectedProductVariantId > 0 ? Inventory.GetProduct(selectedProductVariantId) : null;
							product = GetClosestMatch(productsWithValue, selectedProductVariant, productPropertyTypeId, productPropertyValueId);
							notification = CheckSelectionChange(selectedProductVariant, product, productPropertyTypeId);
						}
						else
						{
							return Json(new { result = false, message = Translation.GetTerm("MatchingProductNotFound", "A matching product could not be found.") });
						}
					}
					//otherwise original product is still valid
				}
				else
				{
					product = productBase.Products.FirstOrDefault(p => p.IsVariantTemplate);
				}
				bool isOrderable = false;
				int newSelectedProductVariantId = 0;
				string name = variantTemplateProduct.Translations.Name();
				string longDescription = variantTemplateProduct.Translations.LongDescription();
				string imagePath = variantTemplateProduct.MainImage == null ? Url.Content("../../Content/Images/Shopping/no-image.jpg") : variantTemplateProduct.MainImage.FilePath.ReplaceFileUploadPathToken();
				decimal productPrice = variantTemplateProduct.GetPrice(BaseShoppingController.Account.AccountTypeID,
				BaseController.DefaultCountry.CurrencyID, null);
				if (!product.IsVariantTemplate)
				{
					if (!product.Translations.Name().IsNullOrEmpty())
					{
						name = product.Translations.Name();
					}
					if (!product.Translations.LongDescription().IsNullOrEmpty())
					{
						longDescription = product.Translations.LongDescription();
					}
					if (product.MainImage != null)
					{
						imagePath = product.MainImage.FilePath.ReplaceFileUploadPathToken();
					}
					productPrice = product.GetPrice(BaseShoppingController.Account.AccountTypeID,
													SmallCollectionCache.Instance.Markets.GetById(CoreContext.CurrentMarketId).GetDefaultCurrencyID(), null);
					isOrderable = true;
					newSelectedProductVariantId = product.ProductID;
				}

				ViewDataDictionary viewData = new ViewDataDictionary();
				viewData.Add("TriggerPropertyId", productPropertyTypeId);
				return Json(new
				{
					result = true,
					message = notification,
					productVariantsHTML = RenderRazorPartialViewToString("_ProductVariants", product, viewData),
					triggerPropertyId = productPropertyTypeId,
					productTitle = name,
					productLongDescription = longDescription,
					productImagePath = imagePath,
					productSKU = product.SKU,
					productPrice = productPrice.ToString(SmallCollectionCache.Instance.Markets.GetById(CoreContext.CurrentMarketId).GetDefaultCurrencyID()),
					isOrderable = isOrderable,
					selectedProductVariantId = newSelectedProductVariantId
				});
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult Product(string id)
		{
			if (!string.IsNullOrEmpty(id))
			{
				int intID;
				if (int.TryParse(id, out intID))
				{
					bool hasProductInMarket = HasProductInMarket(intID);

					if (hasProductInMarket)
					{
						var product = Inventory.GetProduct(intID);
						return ReturnProductView(product);
					}
				}
				else
				{
					bool hasProductInMarket = HasProductInMarket(id);

					if (hasProductInMarket)
					{
						var product = Inventory.GetProduct(id);
						return ReturnProductView(product);
					}
				}
			}

			return RedirectToAction("Index");
		}

		protected virtual ActionResult ReturnProductView(Product product)
		{
			return View(product);
		}

		protected virtual bool HasProductInMarket(int? id)
		{
			return id.HasValue && Inventory.IsProductInStoreFront(id.Value, ApplicationContext.Instance.StoreFrontID);
		}

		protected virtual bool HasProductInMarket(string id)
		{
			bool hasProductInMarket = false;

			if (!string.IsNullOrEmpty(id))
			{
				List<Catalog> catalogs = Inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID);

				hasProductInMarket = Inventory.IsProductInStoreFront(id, ApplicationContext.Instance.StoreFrontID);
			}

			return hasProductInMarket;
		}

		protected virtual List<int> LastSearchResult
		{
			get { return Session["LastSearchResult"] as List<int>; }
			set { Session["LastSearchResult"] = value; }
		}

		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult Search(string q)
		{
			OrderShipment orderShipment = null;
			if (OrderContext.Order != null)
				orderShipment = ((Order)OrderContext.Order).OrderShipments.FirstOrDefault();
			var outOfStockProducts = NetSteps.Data.Entities.Product.GetOutOfStockProductIDs(orderShipment);
			var catalogTypesToSearch = new[] { (int)Constants.CatalogType.Normal, (int)Constants.CatalogType.FeaturedItems };


			var catalogIDsToSearch = Inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID)
				.Where(c => catalogTypesToSearch.Contains(c.CatalogTypeID))
				.Select(c => (int)c.CatalogID).ToList();

			var products = Inventory.SearchProducts(
						ApplicationContext.Instance.StoreFrontID,
						q,
						catalogsToSearch: catalogIDsToSearch)
					 .Where(p => !outOfStockProducts.Contains(p.ProductID) || p.ProductBackOrderBehaviorID != (int)NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide)
					 .Each(p => { p.IsOutOfStock = outOfStockProducts.Contains(p.ProductID); return p; })
					 .ToList();
			LastSearchResult = products.Select(x => x.ProductID).ToList();
			var categories = new List<Category>();
			foreach (var productBase in products.Select(p => p.ProductBase))
			{
				categories.AddRange(productBase.Categories.Where(c => !categories.Any(cat => cat.CategoryID == c.CategoryID)));
			}
			ViewBag.SearchCategories = categories;
			//ViewData["SearchCategories"] = categories;
			return View(products);
		}

		public virtual ActionResult GetSearchResults(int? category)
		{
			IEnumerable<Product> products = Inventory.GetProducts(LastSearchResult);
			if (category.HasValue)
				products = products.Where(p => p.ProductBase.Categories.Any(c => c.CategoryID == category.Value)).ToList();
			return PartialView("SearchResults", products);
		}

		[OutputCache(CacheProfile = "AutoCompleteData")]
		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult QuickSearch(string query, bool includeDynamicKits = true)
		{
			OrderShipment orderShipment = null;
			if (OrderContext.Order != null)
				orderShipment = ((Order)OrderContext.Order).OrderShipments.FirstOrDefault();
			var outOfStockProducts = NetSteps.Data.Entities.Product.GetOutOfStockProductIDs(orderShipment);

			var categoryGroups = FindProducts(query, includeDynamicKits, outOfStockProducts)
								.GroupBy(p => p.ProductBase.Categories == null || p.ProductBase.Categories.Count == 0 ? null : p.ProductBase.Categories.FirstOrDefault())
								.Select(g => new
								{
									id = g.Key == null ? 0 : g.Key.CategoryID,
									text = g.Key == null ? Translation.GetTerm("Uncategorized") : g.Key.Translations.Name(),
									items = g.Select(p => new
									{
										id = p.ProductID,
										text = p.SKU + " - " + p.Translations.Name(),
										sku = p.SKU,
										image = GetMainImage(p)
									})
								});
			return Json(categoryGroups);
		}

		private string GetMainImage(Product p)
		{
			if (p.MainImage == null)
			{
				var variantTemplateProduct = p.ProductBase.Products.Where(cp => cp.IsVariantTemplate).FirstOrDefault();
				if (variantTemplateProduct == null || variantTemplateProduct.MainImage == null)
				{
					return Url.Content("../../Content/Images/Shopping/no-image.jpg");
					//return "../../Content/Images/Shopping/no-image.jpg".ResolveUrl();
				}
				else
				{
					return variantTemplateProduct.MainImage.FilePath.ReplaceFileUploadPathToken();
				}
			}
			else
			{
				return p.MainImage.FilePath.ReplaceFileUploadPathToken();
			}
		}

		protected virtual IEnumerable<Product> FindProducts(string query, bool includeDynamicKits, List<int> outOfStockProducts = null)
		{
			if (outOfStockProducts == null)
				outOfStockProducts = new List<int>();

			var catalogTypesToSearch = new[] { (int)Constants.CatalogType.Normal, (int)Constants.CatalogType.FeaturedItems };

			var catalogIDsToSearch = Inventory.GetActiveCatalogs(ApplicationContext.Instance.StoreFrontID)
				.Where(c => catalogTypesToSearch.Contains(c.CatalogTypeID))
				.Select(c => (int)c.CatalogID).ToList();

			return Inventory.SearchProducts(
				ApplicationContext.Instance.StoreFrontID,
				query,
				includeDynamicKits: includeDynamicKits,
				catalogsToSearch: catalogIDsToSearch)
				.Where(p => !outOfStockProducts.Contains(p.ProductID) || p.ProductBackOrderBehaviorID != (int)NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide);
		}

		[OutputCache(CacheProfile = "DontCache")]
		public virtual ActionResult ReloadInventory()
		{
			try
			{
				Inventory.ExpireCache();
				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Orders-Party Order", "~/")]
		public virtual ActionResult AttachToParty(int partyId)
		{
			try
			{
				var party = Party.Load(partyId);
				if (party.IsOpen)
				{
					OrderContext.Order.ParentOrderID = party.OrderID;
					return Json(new { result = true, message = Translation.GetTerm("YourOrderHasBeenAttachedToAParty", "You order has been attached to a party") });
				}

				return Json(new { result = false });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Orders-Party Order", "~/")]
		public virtual ActionResult DetachFromParty()
		{
			try
			{
				OrderContext.Order.ParentOrderID = null;

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Orders-Party Order", "~/")]
		public virtual ActionResult PartyInfo(int partyId)
		{
			ViewBag.AllowPartyDetach = true;
			return PartialView(Party.LoadFull(partyId));
		}

		[FunctionFilter("Orders", "~/")]
		public ActionResult InPageSearch(string query = null, string startsWith = null, int? kitProductId = null, int? dynamicKitGroupId = null, string sku = null)
		{
			List<Product> products = new List<Product>();
			if (sku != null)
			{
				products.Add(Inventory.GetProduct(sku));
			}
			else
			{
				//filter other dynamic kits

				if (query == null && startsWith == null)
				{
					products = Inventory.GetNonDynamicKitProducts(ApplicationContext.Instance.StoreFrontID);
				}
				else
				{
					products = Inventory.SearchProducts(ApplicationContext.Instance.StoreFrontID, query: query, startsWith: startsWith, includeDynamicKits: false);
				}

				if (kitProductId != null && dynamicKitGroupId != null)
				{
					products = products.Where(p => p.CanBeAddedToDynamicKitGroup(kitProductId.Value, dynamicKitGroupId.Value)).ToList();
				}
			}
			OrderShipment orderShipment = null;
			if (OrderContext.Order != null)
				orderShipment = ((Order)OrderContext.Order).OrderShipments.FirstOrDefault();
			var outOfStockProducts = NetSteps.Data.Entities.Product.GetOutOfStockProductIDs(orderShipment);
			LastSearchResult = products.Where(p => !outOfStockProducts.Contains(p.ProductID) || p.ProductBackOrderBehaviorID != (int)NetSteps.Data.Entities.Constants.ProductBackOrderBehavior.Hide).Select(x => x.ProductID).ToList();

			ViewBag.IsBundle = true;
			return PartialView("SearchResults", products);
		}

		[OutputCache(CacheProfile = "DontCache")]
		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult BundlePackItems(int productId, string bundleGuid)
		{
			var product = Inventory.GetProduct(productId);
			//var product = Product.LoadFull(productId);
			var dynamicKit = new DynamicKit();
			var dynamicKitGroups = new TrackableCollection<DynamicKitGroup>();
			if (product.DynamicKits.Count != 0)
			{
				dynamicKit = product.DynamicKits[0];
				dynamicKitGroups = product.DynamicKits[0].DynamicKitGroups;
			}
			ViewBag.DynamicKit = dynamicKit;
			ViewBag.DynamicKitGroups = dynamicKitGroups;

			ViewBag.MinItemsInBundle = dynamicKitGroups.Sum(g => g.MinimumProductCount);
			ViewBag.MaxItemsInBundle = dynamicKitGroups.Sum(g => g.MaximumProductCount);
			ViewBag.ProductId = productId;
			ViewBag.BundleGuid = bundleGuid;
			var error = GetDefaultProductsForGroup(dynamicKitGroups, bundleGuid, dynamicKitGroups.Sum(g => g.MaximumProductCount));
			if (error != null)
			{
				return error;
			}

			var orderItem = ((OrderCustomer)OrderContext.Order.OrderCustomers[0]).OrderItems.GetByGuid(bundleGuid);
			ViewBag.OrderItem = orderItem;

			return View();
		}

		public JsonResult GetDefaultProductsForGroup(IEnumerable<DynamicKitGroup> groups, string bundleid, int maxItemsForBundle)
		{
			try
			{
				var orderCustomer = (OrderCustomer)OrderContext.Order.OrderCustomers[0];
				var bundleItem = orderCustomer.OrderItems.FirstOrDefault(oc => oc.Guid.ToString("N") == bundleid);
				if (bundleItem != null)
				{
					int currentCount = bundleItem.ChildOrderItems.Sum(oi => oi.Quantity);
					if (currentCount < maxItemsForBundle)
					{
						var defaults = groups.SelectMany(x => x.DynamicKitGroupRules.Where(y => y.Default)).OrderBy(x => x.SortOrder).ToList();
						foreach (var groupRule in defaults)
						{
							int countForGroup = groupRule.DynamicKitGroup.MaximumProductCount;
							int currentCountForGroup = bundleItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == groupRule.DynamicKitGroup.DynamicKitGroupID).Sum(oi => oi.Quantity);
							if (currentCountForGroup < countForGroup && groupRule.Product != null && !NetSteps.Data.Entities.Product.CheckStock(groupRule.Product.ProductID).IsOutOfStock)
							{
								OrderPayment payment = null;
								var orderPayments = orderCustomer.OrderPayments.Where(p => p.OrderPaymentStatusID != (short)Constants.OrderPaymentStatus.Completed).ToList();
								if (orderPayments.Count > 0)
								{
									payment = orderPayments[0];
								}

								OrderContext.Order.AsOrder().AddOrUpdateOrderItem(orderCustomer, new List<OrderItemUpdateInfo>() { new OrderItemUpdateInfo() { ProductID = groupRule.Product.ProductID, Quantity = 1 } }, false, bundleid, groupRule.DynamicKitGroupID);

								if (payment != null)
								{
									OrderService.UpdateOrder(OrderContext);
									if (payment.OrderPaymentStatusID != Constants.OrderPaymentStatus.Completed.ToShort())
										payment.Amount = OrderContext.Order.GrandTotal.ToDecimal();
									orderCustomer.OrderPayments.Add(payment);
								}
							}
						}
					}
				}
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

			return null;
		}

		// Method to auto complete a 'location' textbox for searching products - JHE
		[OutputCache(CacheProfile = "AutoCompleteData")]
		[FunctionFilter("Orders", "~/")]
		public virtual ActionResult SearchLocation(string query)
		{
			var results = TaxCache.Search(query);
			return Json(results.Select(d => new { id = d.PostalCode, text = string.Format("{0}, {1}", d.City, d.StateAbbreviation) }).DistinctBy(d => d.text));
		}
	}
}
