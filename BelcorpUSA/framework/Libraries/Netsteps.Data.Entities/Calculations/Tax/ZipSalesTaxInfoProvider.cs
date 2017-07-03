using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Interfaces;

namespace NetSteps.Data.Entities.TaxInfoProviders
{
    public class ZipSalesTaxRateProvider : ITaxRateProvider
    {
        public DataTable GetAddressInfo(string postalCode)
        {
            return ZipSalesCache.GetValidationListsForPostalCode(postalCode);
            //DataTable dataTable = new DataTable();
            //CreateZSColumns(dataTable);
            //ZipSalesCacheCollection zipSalesCollection = ZipSalesCache.LoadByPostalCode(postalCode);
            //SetZSColumnContents(dataTable, zipSalesCollection);
            //dataTable.AcceptChanges();
            //return dataTable;
        }
        
        public DataTable GetAddressInfo(string state, string county, string city, string postalCode)
        {
            DataTable dataTable = new DataTable();
            CreateZSColumns( dataTable);
            ZipSalesCacheCollection zipSalesCollection = ZipSalesCache.LoadByAddress( state, county, city, postalCode);
            SetZSColumnContents(dataTable, zipSalesCollection);
            dataTable.AcceptChanges();
            return dataTable;
        }
        
        public void SetTaxAmounts(OrderCustomer orderCustomer)
        {
            OrderShipment orderShipment = orderCustomer.Order.GetDefaultShipment();

            //bool foundZip = false;
            ZipSalesCacheCollection zipSalesCollection = ZipSalesCache.LoadByAddress( orderShipment.State,
                orderShipment.County, orderShipment.City, orderShipment.PostalCode);

            if (zipSalesCollection.Count == 0)  // Didn't find any sales tax records -- fall back to just postal code
                zipSalesCollection = ZipSalesCache.LoadByPostalCode(orderShipment.PostalCode);

            foreach (OrderItem item in orderCustomer.OrderItems)
            {
                CalculateItemSalesTax(item, orderCustomer, zipSalesCollection);
            }

            // Tax percentages don't apply to orders, just to order items
            orderCustomer.TaxPercent = 0;
            orderCustomer.TaxPercentCity = 0;
            orderCustomer.TaxPercentCounty = 0;
            orderCustomer.TaxPercentDistrict = 0;
            orderCustomer.TaxPercentState = 0;

            // Calculate the total tax amount for the order customer
            orderCustomer.TaxAmount = 0;
            orderCustomer.TaxAmountCity = 0;
            orderCustomer.TaxAmountCounty = 0;
            orderCustomer.TaxAmountDistrict = 0;
            orderCustomer.TaxAmountState = 0;
            foreach (OrderItem item in orderCustomer.OrderItems)
            {
                orderCustomer.TaxAmount += Functions.GetRoundedNumber(item.ItemPrice * item.CombinedSalesTax);
                orderCustomer.TaxAmountCity += Functions.GetRoundedNumber(item.ItemPrice * item.CitySalesTax);
                orderCustomer.TaxAmountCounty += Functions.GetRoundedNumber(item.ItemPrice * item.CountySalesTax);
                orderCustomer.TaxAmountState += Functions.GetRoundedNumber(item.ItemPrice * item.StateSalesTax);
                orderCustomer.TaxAmountDistrict += Functions.GetRoundedNumber(item.ItemPrice * item.CountyLocalSalesTax);
                orderCustomer.TaxAmountDistrict += Functions.GetRoundedNumber(item.ItemPrice * item.CityLocalSalesTax);
            }
        }

        private void CreateZSColumns(DataTable dataTable)
        {
            List<DataColumn> columns = new List<DataColumn>();
            columns.Add(new DataColumn("city", typeof(string)));
            columns.Add(new DataColumn("county", typeof(string)));
            columns.Add(new DataColumn("state", typeof(string)));
            columns.Add(new DataColumn("ID", typeof(string)));
            columns.Add(new DataColumn("PostalCode", typeof(string)));
            columns.Add(new DataColumn("StateName", typeof(string)));
            columns.Add(new DataColumn("StateTax", typeof(string)));
            columns.Add(new DataColumn("CitySales", typeof(string)));
            columns.Add(new DataColumn("CountyProvince", typeof(string)));
            columns.Add(new DataColumn("CountySales", typeof(string)));
            columns.Add(new DataColumn("DateCreated", typeof(string)));
            columns.Add(new DataColumn("DefaultCounty", typeof(string)));
            columns.Add(new DataColumn("DefaultGeneral", typeof(string)));
            columns.Add(new DataColumn("CountyFIPS", typeof(string)));
            columns.Add(new DataColumn("CountyLocalSales", typeof(string)));
            columns.Add(new DataColumn("CombinedSalesTax", typeof(string)));
            columns.Add(new DataColumn("StateUseTax", typeof(string)));
            columns.Add(new DataColumn("CountyUseTax", typeof(string)));
            columns.Add(new DataColumn("CityUseTax", typeof(string)));
            columns.Add(new DataColumn("CountyLocalUse", typeof(string)));
            columns.Add(new DataColumn("CityLocalUse", typeof(string)));
            columns.Add(new DataColumn("CombinedUseTax", typeof(string)));
            columns.Add(new DataColumn("EffectiveDate", typeof(string)));
            columns.Add(new DataColumn("Geocode", typeof(string)));
            columns.Add(new DataColumn("InCityLimits", typeof(string)));
            columns.Add(new DataColumn("StateAbbr", typeof(string)));
            columns.Add(new DataColumn("CountryID", typeof(string)));
            columns.Add(new DataColumn("CountryCode", typeof(string)));
            columns.Add(new DataColumn("CountryName", typeof(string)));
            columns.Add(new DataColumn("CurrencyID", typeof(string)));
            columns.Add(new DataColumn("CurrencyCode", typeof(string)));
            columns.Add(new DataColumn("CurrencyName", typeof(string)));
            columns.Add(new DataColumn("TaxCategoryID", typeof(string)));
            columns.Add(new DataColumn("DataSource", typeof(string)));
            foreach (DataColumn column in columns)
            {
                dataTable.Columns.Add(column);
            }
        }
        
        private void SetZSColumnContents(DataTable dataTable, ZipSalesCacheCollection zipSalesCollection)
        {
            foreach (ZipSalesCache zipSales in zipSalesCollection)
            {
                DataRow row = dataTable.NewRow();
                row["city"] = zipSales.City;
                row["county"] = zipSales.CountyProvince;
                row["state"] = zipSales.StateName;
                row["ID"] = zipSales.ID;
                row["PostalCode"] = zipSales.PostalCode;
                row["StateName"] = zipSales.StateName;
                row["StateTax"] = zipSales.StateTax;
                row["CitySales"] = zipSales.CitySales;
                row["CountyProvince"] = zipSales.CountyProvince;
                row["CountySales"] = zipSales.CountySales;
                row["DateCreated"] = zipSales.DateCreated;
                row["DefaultCounty"] = zipSales.DefaultCounty;
                row["DefaultGeneral"] = zipSales.DefaultGeneral;
                row["CountyFIPS"] = zipSales.CountyFIPS;
                row["CountyLocalSales"] = zipSales.CountyLocalSales;
                row["CombinedSalesTax"] = zipSales.CombinedSalesTax;
                row["StateUseTax"] = zipSales.StateUseTax;
                row["CountyUseTax"] = zipSales.CountyUseTax;
                row["CityUseTax"] = zipSales.CityUseTax;
                row["CountyLocalUse"] = zipSales.CountyLocalUse;
                row["CityLocalUse"] = zipSales.CityLocalUse;
                row["CombinedUseTax"] = zipSales.CombinedUseTax;
                row["EffectiveDate"] = zipSales.EffectiveDate;
                row["Geocode"] = zipSales.Geocode;
                row["InCityLimits"] = zipSales.InCityLimits;
                row["StateAbbr"] = zipSales.StateAbbr;
                row["CountryID"] = zipSales.CountryID;
                row["CountryCode"] = zipSales.CountryCode;
                row["CountryName"] = zipSales.CountryName;
                row["CurrencyID"] = zipSales.CurrencyID;
                row["CurrencyCode"] = zipSales.CurrencyCode;
                row["CurrencyName"] = zipSales.CurrencyName;
                row["TaxCategoryID"] = zipSales.TaxCategoryID;
                row["DataSource"] = zipSales.DataSource;
                dataTable.Rows.Add(row);
            }
        }
        
        private void CalculateItemSalesTax(OrderItem item, OrderCustomer orderCustomer, ZipSalesCacheCollection zipSalesCollection)
        {
            //bool isWholesale = false;
            //bool isKit = false;
            //bool isSupplement = false;

            if (orderCustomer.IsTaxExempt || item.ChargeTax == false)
            {
                //This account is tax exempt or there is no tax on the item
                item.CombinedSalesTax = 0;
                item.StateSalesTax = 0;
                item.CitySalesTax = 0;
                item.CountySalesTax = 0;
                item.CountyLocalSalesTax = 0;
                item.CityLocalSalesTax = 0;
                return;
            }
            //else if (orderCustomer.AccountTypeId == (int)Constants.AccountTypeEnum.RetailCustomer)
            //{
            //    // Tax at the retail price
            //    isWholesale = false;
            //}
            //else
            //{
            //    // Tax at the wholesale price
            //    isWholesale = true;
            //}

            if (item.ChildOrderItems != null &&
                item.ChildOrderItems.Count > 0 &&
                item.Product.Relations.ContainsKey((int)Constants.ProductRelationType.Kit))
            {
                // This is a kit with items--calculate the tax from the kit items.
                foreach (OrderItem childItem in item.ChildOrderItems)
                {
                    SetItemTaxes(childItem, zipSalesCollection);
                }
                // Update the parent item with the values from its children
                SetItemTaxesFromChildren(item, zipSalesCollection);
            }
            else
            {
                SetItemTaxes(item, zipSalesCollection);
            }
        }
        
        private void SetItemTaxes(OrderItem item, ZipSalesCacheCollection zipSalesCollection)
        {
            ZipSalesCache foundZip = null;

            foreach (ZipSalesCache zip in zipSalesCollection)
            {
                if (zip.TaxCategoryID == item.Product.TaxCategoryID)
                {
                    foundZip = zip;
                    break;
                }
            }
            if (foundZip == null)   // Didn't find a record that matches the source.  Take any possible record
            {
                if (zipSalesCollection.Count == 0)
                {
                    throw new Exception("No tax records were found for the requested location and category.");
                }
                foundZip = zipSalesCollection[0];
            }
            item.CombinedSalesTax = foundZip.CombinedSalesTax;
            item.StateSalesTax = foundZip.StateTax;
            item.CountySalesTax = foundZip.CountySales;
            item.CitySalesTax = foundZip.CitySales;
            item.CountyLocalSalesTax = foundZip.CountyLocalSales;
            item.CityLocalSalesTax = foundZip.CityLocalSales;
        }

        private void SetItemTaxesFromChildren(OrderItem item, ZipSalesCacheCollection zipSalesCollection)
        {
            item.CombinedSalesTax = 0;
            item.StateSalesTax = 0;
            item.CitySalesTax = 0;
            item.CountySalesTax = 0;
            item.CountyLocalSalesTax = 0;
            item.CityLocalSalesTax = 0;
            foreach (OrderItem childItem in item.ChildOrderItems)
            {
                SetItemTaxes(childItem, zipSalesCollection);
                item.CombinedSalesTax += childItem.CombinedSalesTax;
                item.StateSalesTax += childItem.StateSalesTax;
                item.CitySalesTax += childItem.CitySalesTax;
                item.CountySalesTax += childItem.CountySalesTax;
                item.CountyLocalSalesTax += childItem.CountyLocalSalesTax;
                item.CityLocalSalesTax += childItem.CityLocalSalesTax;
            }
        }
    }
}
