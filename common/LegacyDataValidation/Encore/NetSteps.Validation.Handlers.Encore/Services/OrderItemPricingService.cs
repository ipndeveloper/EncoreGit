using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Foundation.Common;
using NetSteps.Validation.Handlers.Encore.Common.Services;
using NetSteps.Validation.Handlers.Services.Containers.OrderItemPricingService;
using NetSteps.Validation.Common;

namespace NetSteps.Validation.Handlers.Common.Services
{
    public class OrderItemPricingService : IOrderItemPricingService
    {
        private const string _name = "Order Item Pricing Service";

        public OrderItemPricingService()
        {
            Manager = new OrderItemPriceManager();
            useCachedData = false;
        }

        public virtual bool GetHistoricalPrice(int productId, DateTime orderDate, int priceTypeId, int currencyID, out decimal price)
        {
            if (useCachedData)
            {
                List<ProductPrice> priceCollection = Manager.GetPrices(productId);
                var found = priceCollection
                    .Where(
                        x =>
                        x.CurrencyID == currencyID && x.EffectiveDate <= orderDate &&
                        x.ProductPriceTypeID == priceTypeId)
                    .OrderBy(x => x.EffectiveDate)
                    .LastOrDefault();
                if (found != null)
                {
                    price = found.Price;
                    return true;
                }
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core].ConnectionString))
                {
                    SqlCommand command =
                        new SqlCommand(
                            String.Format("SELECT TOP 1 Price FROM dbo.Commissions_ProductPrices WHERE ProductID={0} AND ProductPriceTypeID = {1} AND CurrencyID={2} AND EffectiveDate <= '{3}' ORDER BY EffectiveDate DESC", productId, priceTypeId, currencyID, orderDate),
                            connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        price = (decimal) reader["Price"];
                        return true;
                    }
                }
            }
            price = 0M;
            return false;
        }

        protected readonly OrderItemPriceManager Manager;

        private bool useCachedData;

        protected virtual void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings[ConnectionStringNames.Core].ConnectionString))
            {
                SqlCommand command = new SqlCommand("SELECT ProductID, ProductPriceTypeID, Price, CurrencyID, EffectiveDate FROM dbo.Commissions_ProductPrices", connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    Manager.AddPrice(
                                        (int)reader["ProductID"],
                                        (int)reader["ProductPriceTypeID"],
                                        (decimal)reader["Price"],
                                        (int)reader["CurrencyID"],
                                        (DateTime)reader["EffectiveDate"]
                                    );
                }
            }
        }

        public void Initialize()
        {
            LoadData();
            useCachedData = true;
        }

        public IRecordQuery QueryBase { get; set; }


        public virtual bool ShouldMultiplyOrderItemPricesByQuantity
        {
            get { return true; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}
