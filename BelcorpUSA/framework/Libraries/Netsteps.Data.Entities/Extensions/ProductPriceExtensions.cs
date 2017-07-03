namespace NetSteps.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Common.Extensions;
    using NetSteps.Data.Entities.Business.Logic.Interfaces;
    using NetSteps.Data.Entities.Cache;
    using NetSteps.Encore.Core.IoC;
    using System.Data.SqlClient;
    using System.Data;
    using NetSteps.Data.Entities.Generated;

	/// <summary>
	/// Author: John Egbert
	/// Description: ProductPrices Extensions
	/// Created: 04-14-2010
	/// </summary>
	public static class ProductPricesExtensions
	{

		public static decimal? GetPriceByPriceType(this IEnumerable<ProductPrice> prices, Constants.ProductPriceType productPriceType, int currencyID)
		{
            return GetPriceByPriceType(prices, productPriceType.ToInt(), currencyID);
		}

        /*CS.24MAY2016.Inicio*/
        public static IDictionary<int, decimal> GetPriceByPriceTypes(int currencyID, int ProductID)
        {
            int StoreFrontID = NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID;
            Dictionary<int, decimal> result = new Dictionary<int, decimal>();
            IDataReader dr = DataAccess.ExecuteReader(DataAccess.GetCommand("GetProductPrices", new Dictionary<string, object>() 
            {   { "ProductID", ProductID },
                { "CurrencyID", currencyID },
                { "StoreFrontID", StoreFrontID } 
            }, ConnectionStrings.BelcorpCore));

            while (dr.Read())
                result.Add(Convert.ToInt32(dr["ProductPriceTypeID"]), Convert.ToDecimal(dr["Precio"]));
            dr.Close();
            return result;
        }
        /*CS.24MAY2016.Fin*/

		public static decimal? GetPriceByPriceType(this IEnumerable<ProductPrice> prices, int productPriceTypeId, int currencyID)
		{
            //var price = prices.FirstOrDefault(p => p.ProductPriceTypeID == productPriceTypeId && p.CurrencyID == currencyID);
            //return price == null ? (decimal?)null : price.Price;

            var ProductPrice = prices.FirstOrDefault(p => p.ProductPriceTypeID == productPriceTypeId && p.CurrencyID == currencyID);
            int StoreFrontID = NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID;
            int ProductID = ProductPrice == null ? 0 : ProductPrice.ProductID;
            decimal Price = DataAccess.ExecWithStoreProcedureDecimal("Core", "GetProductPrice",
                                                             new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
                                                             new SqlParameter("ProductPriceTypeID", SqlDbType.Int) { Value = productPriceTypeId },
                                                             new SqlParameter("CurrencyID", SqlDbType.Int) { Value = currencyID },
                                                             new SqlParameter("StoreFrontID", SqlDbType.Int) { Value = StoreFrontID }
                                                             );
            return ProductPrice == null && Price == 0 ? (decimal?)null : Price;
		}

        public static decimal GetPriceByPriceType(int ProductID, int productPriceTypeId, int currencyID)
        {
            int StoreFrontID = NetSteps.Data.Entities.ApplicationContext.Instance.StoreFrontID;
            decimal Price = DataAccess.ExecWithStoreProcedureDecimal("Core", "GetProductPrice",
                                                             new SqlParameter("ProductID", SqlDbType.Int) { Value = ProductID },
                                                             new SqlParameter("ProductPriceTypeID", SqlDbType.Int) { Value = productPriceTypeId },
                                                             new SqlParameter("CurrencyID", SqlDbType.Int) { Value = currencyID },
                                                             new SqlParameter("StoreFrontID", SqlDbType.Int) { Value = StoreFrontID }
                                                             );

            return Price;
        }

		public static decimal? GetPriceByAccountTypeAndRelationship(this IEnumerable<ProductPrice> prices, short accountTypeID, Constants.PriceRelationshipType priceRelationshipType, int currencyID)
		{
			var accountPriceTypeBusinessLogic = Create.New<IAccountPriceTypeBusinessLogic>();
			var productPriceType = accountPriceTypeBusinessLogic.GetPriceType(accountTypeID, priceRelationshipType, null);
			if (productPriceType == null)
			{
				throw new Exception(
					string.Format("No price type found for: Account Type: {0}, Price Relationship Type: {1}, Store Front: {2}",
						SmallCollectionCache.Instance.AccountTypes.GetById(accountTypeID).Name,
						SmallCollectionCache.Instance.PriceRelationshipTypes.GetById(priceRelationshipType.ToInt()).Name,
					 SmallCollectionCache.Instance.StoreFronts.GetById(ApplicationContext.Instance.StoreFrontID).Name));
			}

			return GetPriceByPriceType(prices, (Constants.ProductPriceType)productPriceType.ProductPriceTypeID, currencyID);
		}
	}
}
