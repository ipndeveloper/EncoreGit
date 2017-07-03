using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Financials.ShippedRevenue;
using NetstepsDataAccess.DataEntities;

namespace NetSteps.Financials.Data
{
    public static class Repository
    {
        public static FinancialsGrossRevenue GetGrossRevenue(DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            FinancialsGrossRevenue rev = new FinancialsGrossRevenue();
            using (NetStepsEntities db = new NetStepsEntities())
            {
                System.Data.Objects.ObjectResult<uspIntegrationsGetGrossRevenueResult> result = db.uspIntegrationsGetGrossRevenue(fromDate, toDate);
                uspIntegrationsGetGrossRevenueResult res = new uspIntegrationsGetGrossRevenueResult();
                res = result.Single(); // database will always only return a single row
                rev.CreditCardRevenue = new Money { Currency = Currency.USD, Value = res.CreditCardRevenue };
                rev.CashRevenue = new Money { Currency = Currency.USD, Value = res.CashRevenue };
                rev.GiftCardRevenue = new Money { Currency = Currency.USD, Value = res.GiftCardRevenue };
                rev.ProductCreditRevenue = new Money { Currency = Currency.USD, Value = res.ProductCreditRevenue };
                rev.SalesTaxRevenue = new Money { Currency = Currency.USD, Value = res.SalesTaxRevenue };
                rev.ServiceIncomeRevenue = new Money { Currency = Currency.USD, Value = res.ServiceIncomeRevenue };
            }
            return rev;
        }

        public static FinancialsShippedRevenue GetShippedRevenue(DateTime fromDate, DateTime toDate, string CountryISOCode)
        {
            FinancialsShippedRevenue rev = new FinancialsShippedRevenue();
            List<ShippedRevenue.OrderItem> items = new List<ShippedRevenue.OrderItem>();
            System.Data.Objects.ObjectResult<uspIntegrationsGetShippedRevenueResult> result = null;
            using (NetStepsEntities db = new NetStepsEntities())
            {
                result = db.uspIntegrationsGetShippedRevenue();

                foreach (uspIntegrationsGetShippedRevenueResult res in result)
                {
                    ShippedRevenue.OrderItem item = new ShippedRevenue.OrderItem();
                    item.ActualPrice = new ShippedRevenue.Money { Currency = ShippedRevenue.Currency.USD, Value = Convert.ToDecimal(res.ActualPrice) };
                    item.QuantityShipped = res.QuantityShipped;
                    item.RetailPrice = new ShippedRevenue.Money { Currency = ShippedRevenue.Currency.USD, Value = res.RetailPrice };
                    item.ShippingCost = new ShippedRevenue.Money { Currency = ShippedRevenue.Currency.USD, Value = res.ShippingCost };
                    item.SKU = res.SKU;
                    item.WholesalePrice = new ShippedRevenue.Money { Currency = ShippedRevenue.Currency.USD, Value = res.WholesalePrice };
                    items.Add(item);
                }
            }
            rev.OrderItem = items;
            return rev;
        }
    }
}
