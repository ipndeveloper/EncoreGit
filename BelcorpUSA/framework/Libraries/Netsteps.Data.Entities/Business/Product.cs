using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Common.Context;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using System.Data;
using NetSteps.Data.Entities.Business.Logic;

namespace NetSteps.Data.Entities
{
	public partial class Product : IProduct, IProductService
	{
		/// <summary>
		/// Related entities that can be included by the 'Load' methods (see <see cref="LoadRelationsExtensions"/>).
		/// WARNING: Changes to this list will affect various 'Load' methods, be careful.
		/// </summary>
		[Flags]
		public enum Relations
		{
			// These are bit flags so they must be numbered appropriately (eg. 0, 1, 2, 4, 8, 16)
			// Use bit-shifting for convenience (eg. 0, 1 << 0, 1 << 1, 1 << 2)
			None = 0
			,
			ProductBase = 1 << 0
				,
			Prices = 1 << 1
				,
			Files = 1 << 2
				,
			Properties = 1 << 3
				,
			CatalogItems = 1 << 4
				,
			WarehouseProducts = 1 << 5
				,
			ChildProductRelations = 1 << 6
				,
			ParentProductRelations = 1 << 7
				,
			DynamicKits = 1 << 8
				,
			Translations = 1 << 9
				,
			ProductVariantInfo = 1 << 10
				,
			ExcludedShippingMethods = 1 << 11

			 ,
			LoadFull = ProductBase
			 | Prices
			 | Files
			 | Properties
			 | CatalogItems
			 | WarehouseProducts
			 | ChildProductRelations
			 | ParentProductRelations
			 | DynamicKits
			 | Translations
			 | ProductVariantInfo
			 | ExcludedShippingMethods

				,
			LoadForBase = Prices
			 | Files
			 | Properties
			 | CatalogItems
			 | WarehouseProducts
			 | ChildProductRelations
			 | ParentProductRelations
			 | DynamicKits
			 | Translations
			 | ProductVariantInfo
			 | ExcludedShippingMethods
		};
		#region Properties
		/// <summary>
		/// Helper method to get the name of the product; first checking Product DescriptionTranslations for an overridden value to the BaseProduct. - JHE
		/// </summary>
        public virtual string Name
		{
			get
			{
				if (this.Translations != null && this.Translations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID) != null)
					return this.Translations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID).Name;
				else if (this.ProductBase != null && this.ProductBase.Translations != null && this.ProductBase.Translations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID) != null)
					return this.ProductBase.Translations.GetByLanguageID(ApplicationContext.Instance.CurrentLanguageID).Name;
				else if (this.Translations != null && this.Translations.GetByLanguageID(Constants.Language.English.ToInt()) != null)
					return this.Translations.GetByLanguageID(Constants.Language.English.ToInt()).Name;
				else if (this.ProductBase != null && this.ProductBase.Translations != null && this.ProductBase.Translations.GetByLanguageID(Constants.Language.English.ToInt()) != null)
					return this.ProductBase.Translations.GetByLanguageID(Constants.Language.English.ToInt()).Name;
				else
					return string.Empty;
			}
		}

		public ProductFile MainImage
		{
			get
			{
				ProductFile image = null;

				if (Files != null && Files.Count > 0 && Files.Any(f => f.ProductFileTypeID == (int)Constants.ProductFileType.Image))
				{
					image = Files.Where(f => f.ProductFileTypeID == (int)Constants.ProductFileType.Image).OrderBy(f => f.SortIndex).First();
				}

				return image;
			}
		}

        public double Participation { get; set; }

        //pretty much a hack to store in/out of stock, since many views are bound to this entity this is where its ending up for now. 
        public bool? IsOutOfStock { get; set; }
		#endregion

		#region Methods
		public static decimal GetProductPrice(int productID, int currencyID, int productPriceTypeID)
		{
			return Repository.GetProductPrice(productID, currencyID, productPriceTypeID);
		}

		public static void SaveBatch(IEnumerable<Product> products)
		{
			try
			{
				Repository.SaveBatch(products);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Product Load(string productNumber)
		{
			try
			{
				return Repository.Load(productNumber);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static Product LoadWithRelations(int productID)
		{
			try
			{
				return BusinessLogic.LoadWithRelations(productID, Repository);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Product> LoadAllFull()
		{
			try
			{
				return Repository.LoadAllFull();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Product> LoadAllFullByStorefront(int storefrontID)
		{
			try
			{
				var list = Repository.LoadAllFullByStorefront(storefrontID);
				list.Each(item =>
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
				});
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Product> LoadAllFullExcept(IEnumerable<int> productIDs)
		{
			try
			{
				var list = Repository.LoadAllFullExcept(productIDs);
				list.Each(item =>
				{
					item.StartTracking();
					item.IsLazyLoadingEnabled = true;
				});
				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<ProductSlimSearchData> SlimSearch(string query, int? pageSize = 1000)
		{
			try
			{
				return Repository.SlimSearch(query, pageSize);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<ProductSlimSearchData> LoadAllSlim(FilterPaginatedListParameters<Product> searchParams)
		{
			try
			{
				return Repository.LoadAllSlim(searchParams);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<int> GetOutOfStockProductIDs(NetSteps.Addresses.Common.Models.IAddress address = null)
		{
			try
			{
				return Repository.GetOutOfStockProductIDs(address);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		/// <summary>
		/// Checks to see if stock is available for a given product
		/// </summary>
		/// <param name="productId"></param>
		/// <param name="address"></param>
		/// <returns>Inventory level object.</returns>
		public static InventoryLevels CheckStock(int productId, NetSteps.Addresses.Common.Models.IAddress address = null)
		{
			try
			{
                List<InventoryCheck> InventoryCheck = PreOrderBusinessLogic.Instance.InventoryCheckResult(productId, 2);
				return Repository.CheckStock(productId, address);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public bool CanBeAddedToDynamicKitGroup(int kitProductId, int dynamicKitGroupId)
		{
			return BusinessLogic.CanBeAddedToDynamicKitGroup(this, kitProductId, dynamicKitGroupId);
		}

		public bool DisplayOutOfStockMessage()
		{
			return CheckStock(ProductID).IsOutOfStock && ProductBackOrderBehaviorID == Constants.ProductBackOrderBehavior.AllowBackorderInformCustomer.ToInt();
		}

		public bool IsDynamicKit()
		{
			return DynamicKits.Count > 0;
		}

		public bool IsDynamicallyPricedKit()
		{
			return DynamicKits.Count > 0 &&
					 DynamicKits[0].DynamicKitPricingTypeID == (int)ConstantsGenerated.DynamicKitPricingType.DynamicPricing;
		}

		public bool IsStaticKit()
		{
			return this.ChildProductRelations.Any(cpr => cpr.ProductRelationsTypeID == (int)Constants.ProductRelationsType.Kit);
		}

		public bool IsVariant()
		{
			return this.ProductBase.Products.Count > 1 && !this.IsVariantTemplate;
		}

		public bool HasOwnDescriptionInfo()
		{
			return !this.Translations.ShortDescription().IsNullOrEmpty() || !this.Translations.LongDescription().IsNullOrEmpty();
		}

		public bool HasOwnImages()
		{
			return this.Files.Count > 0;
		}

		public virtual void AddChildProductRelation(int relationTypeID, Product childProduct)
		{
			BusinessLogic.AddChildProductRelation(this, relationTypeID, childProduct);
		}

		public static PaginatedList<AuditLogRow> GetAuditLog(Product fullyLoadedProduct, int productID, AuditLogSearchParameters param)
		{
			try
			{
				if (fullyLoadedProduct != null)
					return BusinessLogic.GetAuditLog(Repository, fullyLoadedProduct, param);
				else
					return BusinessLogic.GetAuditLog(Repository, productID, param);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null);
			}
		}
		public static PaginatedList<AuditLogRow> GetAuditLog(Product fullyLoadedProduct, AuditLogSearchParameters param)
		{
			try
			{
				return BusinessLogic.GetAuditLog(Repository, fullyLoadedProduct, param);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, null, null);
			}
		}

		public decimal GetPrice(int accountTypeID, int currencyID, int? orderTypeID = null)
		{
			return BusinessLogic.GetPrice(this, accountTypeID, orderTypeID, currencyID);
		}

		public decimal GetPrice(int accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int currencyID, int? orderTypeID = null)
		{
			return BusinessLogic.GetPrice(this, accountTypeID, priceRelationshipType, orderTypeID, currencyID);
		}

		/* 2011-11-09, JWL, Added this new method to pass in client-specific product price types that don't exist in 
		 * the main Encore ConstantsGenerated enum.
		 * Had to rename the method to avoid conflicting with the GetPrice method above with a similar signature
		 */
        public virtual decimal GetPriceByPriceType(int productPriceType, int currencyID)
		{
			return Prices.GetPriceByPriceType(productPriceType, currencyID) ?? 0;
		}

		public decimal GetPrice(ConstantsGenerated.ProductPriceType productPriceType, int currencyID)
		{
			return Prices.GetPriceByPriceType(productPriceType, currencyID) ?? 0;
		}

		public decimal GetAdjustedPrice(int accountTypeID, int currencyID, int? orderTypeID = null)
		{
			return BusinessLogic.GetAdjustedPrice(this, accountTypeID, orderTypeID, currencyID);
		}

		public decimal GetAdjustedPrice(int accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int currencyID, int? orderTypeID)
		{
			return BusinessLogic.GetAdjustedPrice(this, accountTypeID, priceRelationshipType, orderTypeID, currencyID);
		}

		public decimal GetCurrentPromotionalPrice(IOrderContext orderContext, ConstantsGenerated.ProductPriceType priceType, int currencyID, int marketID)
		{
			return BusinessLogic.GetCurrentPromotionalPrice(orderContext, this, priceType, currencyID, marketID);
		}

		public decimal GetCurrentPromotionalPrice(IOrderContext orderContext, int priceTypeID, int currencyID, int marketID)
		{
			return BusinessLogic.GetCurrentPromotionalPrice(orderContext, this, priceTypeID, currencyID, marketID);
		}

		/// <summary>
		/// Makes a call to the respective BL (Encore or Overridden) and fetches the term.
		/// </summary>
		/// <param name="isCurrentlyABundle"></param>
		/// <returns></returns>
		public string GetShopTerm(bool isCurrentlyABundle = false)
		{
			return BusinessLogic.GetShopTerm(this, isCurrentlyABundle);
		}

		#endregion

		public static Product LoadFullBySKU(string sku)
		{
			return Repository.LoadFullBySKU(sku);
		}

		public static List<Product> Search(string query)
		{
			try
			{
				return Repository.Search(query);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public static List<Product> SearchProductForOrder(string query)
        {
            try
            {
                return Repository.SearchProductForOrder(query);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

		public static PaginatedList<Product> Search(FilterPaginatedListParameters<Product> searchParams)
		{
			try
			{
				return Repository.Search(searchParams);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static List<Product> GetVariants(int productID)
		{
			try
			{
				return Repository.GetVariants(productID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static int GetVariantsCount(int productID)
		{
			try
			{
				var variants = Repository.GetVariants(productID);
				return variants.CountSafe();
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void ChangeActiveStatus(int productID, bool active)
		{
			try
			{
				Repository.ChangeActiveStatus(productID, active);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static IList<int> GetExcludedShippingMethodIds(int productId)
		{
			try
			{
				return Repository.GetExcludedShippingMethodIds(productId);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		IProductBase IProduct.ProductBase
		{
			get { return ProductBase; }
		}

		IProduct IProductService.Load(int productID)
		{
			throw new NotImplementedException();
		}

		public override string ToString()
		{
			return string.Format("SKU: {0} ID: {1}", SKU, ProductID);
		}

        public static DataTable GetKitItemValuationsByParent(string parentSKU)
        {
            DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsGetKitItemValuationsByParent", new Dictionary<string, object>() { { "@PARENTSKU", parentSKU } }, "Core"));
            return ds.Tables[0];
        }
	}
}
