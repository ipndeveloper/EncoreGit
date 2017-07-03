using System;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Product Extensions
	/// Created: 06-22-2010
	/// </summary>
	public static class ProductExtensions
	{
		public static string GetShortDescriptionDisplay(this Product product)
		{
			Contract.Requires<ArgumentNullException>(product != null);
			string shortDesc = product.Translations.ShortDescription();
			if (string.IsNullOrWhiteSpace(shortDesc))
			{
				shortDesc = product.ProductBase.Translations.ShortDescription();
			}
			return shortDesc ?? string.Empty;
		}

		public static bool ContainsPrice(this Product product, short accountTypeID, int currencyID, int? orderTypeID = null)
		{
			return product.ContainsPrice(accountTypeID, Constants.PriceRelationshipType.Products, currencyID, orderTypeID);
		}

		public static bool ContainsPrice(this Product product, short accountTypeID, ConstantsGenerated.PriceRelationshipType priceRelationshipType, int currencyID, int? orderTypeID = null)
		{
			var productPriceType = AccountPriceType.GetPriceType(accountTypeID, priceRelationshipType, orderTypeID);
			if (productPriceType == null)
				throw new NetStepsBusinessException(string.Format("No price type found for account type '{0}'; PriceRelationshipType: {1}", SmallCollectionCache.Instance.AccountTypes.GetById(accountTypeID).Name, SmallCollectionCache.Instance.PriceRelationshipTypes.GetById(priceRelationshipType.ToInt()).Name))
				{
					PublicMessage = Translation.GetTerm("NoPriceTypeFoundforAccountTypePriceRelationshipType", "No price type found for account type '{0}' and price relationship type '{1}'", SmallCollectionCache.Instance.AccountTypes.GetById(accountTypeID).GetTerm(), SmallCollectionCache.Instance.PriceRelationshipTypes.GetById(priceRelationshipType.ToInt()).Name)
				};

            var hasProductPrice =
                product.Prices.GetPriceByPriceType(
                    (ConstantsGenerated.ProductPriceType)productPriceType.ProductPriceTypeID, currencyID).HasValue;

			//If price doesnt exist in memory check db
            if (!hasProductPrice)
			{
				var productPrices = Product.Repository.GetProductPrices(product.ProductID, currencyID);
                if (productPrices.GetPriceByPriceType((ConstantsGenerated.ProductPriceType)productPriceType.ProductPriceTypeID, currencyID).HasValue)
				{
					product.Prices.Clear();
					product.Prices.AddRange(productPrices);
					return true;
				}
				return false;
			}
			return true;
		}

		public static bool RequiresCustomization(this Product product)
		{
			string tmp;
			return product.RequiresCustomization(out tmp);
		}

		public static bool RequiresCustomization(this Product product, out string customizationType)
		{
			customizationType = product.CustomizationType();
			return !string.IsNullOrEmpty(customizationType);
		}

		public static string CustomizationType(this Product product)
		{
			string cType = "";
			var propertyType = SmallCollectionCache.Instance.ProductPropertyTypes.FirstOrDefault(x => x.Name == "RequiresCustomization");
			if (product != null && propertyType != null)
			{
				var requiresCustomizationProperty = product.Properties.FirstOrDefault(p => p.ProductPropertyTypeID == propertyType.ProductPropertyTypeID);
				if (requiresCustomizationProperty != null)
				{
					cType = requiresCustomizationProperty.ProductPropertyValue.Value;
				}
			}
			return cType;
		}

		//call this to get original default shop term for add button after item is customized
		public static string GetCustomizedShopTerm(this Product product, bool isCurrentlyABundle = false)
		{
			return (isCurrentlyABundle) ? "Add to Pack" : product.IsDynamicKit() ? "Create Bundle" : "Add to Cart";
		}

		public static bool DoesChargeTax(this Product product)
		{
			var productBase = product.ProductBase;
			if (productBase == null)
				productBase = ProductBase.Load(product.ProductBaseID);

			return productBase.ChargeTax;
		}

		public static bool AssertCustomerCanOrderProduct(this Product product, OrderCustomer customer, out IEnumerable<string> excluded)
		{
			bool result = true;
			excluded = Enumerable.Empty<string>();
			var excludedSPs = product.ProductBase.ExcludedStateProvinces.Select(sp => sp.StateProvinceID).ToArray();
			if (excludedSPs.Any())
			{
				var ocShipments = customer.OrderShipments;
				if (ocShipments.Any())
				{
					var shipmentSPs = ocShipments.Where(os => os.StateProvinceID.HasValue).Select(os => os.StateProvinceID.Value).ToArray();
					if (shipmentSPs.Any(s => excludedSPs.Contains(s)))
					{
						excluded = product.ProductBase.ExcludedStateProvinces.Where(esp => shipmentSPs.Contains(esp.StateProvinceID)).Select(esp => esp.StateAbbreviation).ToArray();
						result = false;
					}
				}
				else
				{
					var oshipments = customer.Order.OrderShipments;
					if (oshipments.Any())
					{
						var shipmentSPs = oshipments.Where(os => os.StateProvinceID.HasValue).Select(os => os.StateProvinceID.Value).ToArray();
						if (shipmentSPs.Any(s => excludedSPs.Contains(s)))
						{
							excluded = product.ProductBase.ExcludedStateProvinces.Where(esp => shipmentSPs.Contains(esp.StateProvinceID)).Select(esp => esp.StateAbbreviation).ToArray();
							result = false;
						}
					}
				}
			}
			return result;
		}

        public static void InsertVariants(int ProductBaseID, int MaterialID, int OfertType, int ExternalCode,int ProductID)
        {
            DataAccess.ExecWithStoreProcedureSave("Core", "uspInsertVariantCUV",
                    new SqlParameter("ProductBaseID", SqlDbType.Int) { Value = ProductBaseID },
                    new SqlParameter("MaterialID", SqlDbType.Int) { Value = MaterialID },
                    new SqlParameter("OfertType", SqlDbType.Int) { Value = OfertType },
                    new SqlParameter("ExternalCode", SqlDbType.Int) { Value = ExternalCode },
                      new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID }
                );
        }


        public static PaginatedList<listdispatchGet> listDispatchGen(PaginationParameters param) // wv:20160616 Lista todos los Dispatch Generados
        {
            object RowsCount;
            var resultDispatch = DataAccess.ExecWithStoreProcedureListParam<listdispatchGet>("Core", "listDispatchGen", out RowsCount,
                new SqlParameter("PageSize", SqlDbType.Int) { Value = param.PageSize },
                new SqlParameter("PageNumber", SqlDbType.Int) { Value = param.PageIndex },
                new SqlParameter("Colum", SqlDbType.VarChar) { Value = param.OrderBy },
                new SqlParameter("Order", SqlDbType.VarChar) { Value = param.Order },

                new SqlParameter("Description", SqlDbType.VarChar) { Value = param.Description },
                new SqlParameter("PeriodStart", SqlDbType.VarChar) { Value = param.PeriodStart },
                new SqlParameter("PeriodEnd", SqlDbType.VarChar) { Value = param.PeriodEnd },
                new SqlParameter("SKU", SqlDbType.VarChar) { Value = param.SKU },

                new SqlParameter("RowsCount", SqlDbType.Int) { Value = 0, Direction = ParameterDirection.Output }
                ).ToList();

            IQueryable<listdispatchGet> matchingItems = resultDispatch.AsQueryable<listdispatchGet>();

            var resultTotalCount = (int)RowsCount;// matchingItems.Count();
            //ya no pues el SP pagina
            //matchingItems = matchingItems.ApplyPagination(searchParameter);

            return matchingItems.ToPaginatedList<listdispatchGet>(param, resultTotalCount);
             
        }

        public static void DispatchListDelById(int DispatchListID)
        {
            DataAccess.ExecWithStoreProcedureSave("Core", "updDispatchListDelById",
                    new SqlParameter("DispatchListID", SqlDbType.Int) { Value = DispatchListID } 
                );
        } 
      
        public static VariantsCUVsSearchData GetVariantCUVByProduct(int ProductID)
        {
            VariantsCUVsSearchData variantsCUVsSearchData = new VariantsCUVsSearchData();

            var reg= DataAccess.ExecWithStoreProcedureListParam<VariantsCUVsSearchData>("Core", "uspGetVariantCUVByProduct",
                     new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID }
                 ).FirstOrDefault();

            if (reg == null)
                return variantsCUVsSearchData;
            else
                return reg;

        }
	}
}
