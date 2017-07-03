using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Business;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
    public class PricesPerCatalogDataAcces
    {
        //Obtencion de datos de Precios Por Catalogo
        //Developed by Kelvin Lopez C. - CSTI
        public static List<PricesPerCatalogsData> SearchCatalogs(int ProductID, int languageID)
        {
            List<PricesPerCatalogsData> result = new List<PricesPerCatalogsData>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductID", ProductID }, { "@languageID", languageID } };

                SqlDataReader reader = DataAccess.GetDataReader("uspGetCatalogs", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<PricesPerCatalogsData>();
                    while (reader.Read())
                    {
                        PricesPerCatalogsData PricesPerCatalag = new PricesPerCatalogsData();
                        PricesPerCatalag.CatalogID = Convert.ToInt32(reader["CatalogID"]);
                        PricesPerCatalag.Name = Convert.ToString(reader["Name"]);

                        result.Add(PricesPerCatalag);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
        //INSERT PRODUCT PRICE
        public static int InsertProductPrices(ProductPricesCatalog prices)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() {  { "@ProductPriceTypeID", prices.ProductPriceTypeID },
                                                                                            { "@ProductID", prices.productId }, 
                                                                                            { "@CurrencyID", prices.currencyId },
                                                                                            { "@CatalogID", prices.catalogID },
                                                                                            { "@Price", prices.Price}};

                SqlCommand cmd = DataAccess.GetCommand("spInsertProductPrices", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int DeleteProductPrices(ProductPricesCatalog prices)
        {
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductID", prices.productId }, 
                                                                                           { "@CatalogID", prices.catalogID }
                                                                                        };
                SqlCommand cmd = DataAccess.GetCommand("spDeleteProductPrices", parameters, "Core") as SqlCommand;
                cmd.Connection.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        //CONSULTAR PRODUCTPRICES
        public static List<ProductPrice> SearchProductPrices(int ProductID, int CurrencyID, int CatalogID)
        {
            List<ProductPrice> result = new List<ProductPrice>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@ProductID", ProductID }, 
                                                                                           { "@CurrencyID", CurrencyID },
                                                                                           { "@CatalogID", CatalogID }
                                                                                           };

                SqlDataReader reader = DataAccess.GetDataReader("spGetProductPrices", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<ProductPrice>();
                    while (reader.Read())
                    {
                        ProductPrice PricesPerCatalags = new ProductPrice();
                        PricesPerCatalags.ProductPriceTypeID = Convert.ToInt32(reader["ProductPriceTypeID"]);
                        PricesPerCatalags.Price = Convert.ToDecimal(reader["Price"]);

                        result.Add(PricesPerCatalags);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }


        public static List<ProductPrice> AllProductPrices(int CurrencyID)
        {
            List<ProductPrice> result = new List<ProductPrice>();
            try
            {
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@CurrencyID", CurrencyID }};

                SqlDataReader reader = DataAccess.GetDataReader("spGetAllProductPrices", parameters, "Core");

                if (reader.HasRows)
                {
                    result = new List<ProductPrice>();
                    while (reader.Read())
                    {
                        ProductPrice PricesPerCatalags = new ProductPrice();
                        PricesPerCatalags.ProductPriceTypeID = Convert.ToInt32(reader["ProductPriceTypeID"]);
                        PricesPerCatalags.Price = Convert.ToDecimal(reader["Price"]);

                        result.Add(PricesPerCatalags);
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            return result;
        }
    }
}
