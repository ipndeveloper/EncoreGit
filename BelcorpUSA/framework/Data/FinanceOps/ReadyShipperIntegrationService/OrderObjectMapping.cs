using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Linq;

namespace ReadyShipperIntegrationService
{
    internal static class OrderObjectMapping
    {
        internal static ordercollection MapOrderCollectionFromView(NetstepsDataContext dc)
        {
            ordercollection col = new ordercollection();
            List<VwLogisticsProviderImport> query = dc.UspSelectVwLogisticsProviderOrderHeader().ToList();
            int count = query.Count();
            order[] orderArrary = new order[count];
            col.order = orderArrary;
            int counter = 0;
            foreach (VwLogisticsProviderImport v in query)
            {
                order o = new order();
                // assign ID
                id ID = new id();
                ID.primary_id = v.PrimaryID.ToString();
                ID.numerical_id = v.PrimaryID.ToString();
                ID.trueid = v.PrimaryID.ToString();
                o.id = ID;
                o.message = v.MESSAGE;
                time t = new time() { order_time = v.DateofSale.ToLongDateString() };
                money mm = new money() { currency = currency.USD, Value = Convert.ToDecimal(v.BilledShipping) };
                o.time = t;
                o.actual_shipcost = mm;
                o.currency = currency.USD;
                o.customer_number = v.CustomerNumber.ToString();

                // assign ship_data
                o.ship_data = CreateShipData(v, dc, v.PrimaryID);


                // assign billing
                o.billing = CreateBilling(v, o.ship_data.box[0].item);



                col.order[counter] = o;
                counter++;
            }
            return col;
        }

        private static ship_data CreateShipData(VwLogisticsProviderImport v, NetstepsDataContext dc, int orderID)
        {
            ship_data data = new ship_data();
            residential_indicator res = new residential_indicator() { enabled = v.Residential == "Y" ? true : false };
            address a = new address()
            {
                address1 = v.ShipAddress1,
                address2 = v.ShipAddress2,
                city = v.ShipCity,
                company = v.ShipCompany,
                country = v.ShipCountry,
                email = v.ShipEmail,
                name_first = v.ShipFirst,
                name_last = v.ShipLast,
                phone = v.ShipPhone,
                residential_indicator = res,
                state = v.ShipState,
                zip = v.ShipZip
            };
            data.address = a;
            data.box = CreateBox(v, dc, orderID);
            data.residential_indicator = res;
            data.ship_type = v.ShipType;
            data.ship_via = v.ShipVia;
            return data;
        }

        private static box[] CreateBox(VwLogisticsProviderImport v, NetstepsDataContext dc, int orderID)
        {
            box b = new box();
            money shipcost = new money() { currency = currency.USD, Value = Convert.ToDecimal(v.BilledShipping) };
            b.actual_shipcost = shipcost;
            money declare = new money() { currency = currency.USD, Value = Convert.ToDecimal(v.DeclaredValue) };
            b.declared_value = declare;
            List<VwLogisticsProviderImport> query = dc.UspSelectVwLogisticsProviderOrderItemsByOrderID(orderID).ToList();
            b.item = CreateItems(query);

            box[] collection = new box[1];
            collection[0] = b;
            return collection;
        }

        private static item[] CreateItems(List<VwLogisticsProviderImport> vs)
        {
            item[] items =new item[vs.Count()];
            int counter = 0;
            foreach (VwLogisticsProviderImport v in vs)
            {
                money item_price = new money() { currency = currency.USD, Value = v.Price };
                weight item_weight = new weight() { units = weight_system.lbs, Value = v.Weight.ToString() };
                item Item = new item() { code = v.Product, commodity_code = v.CommodityCode, description = v.MerchandiseDescription,
                    id = v.Product, origin_country_code = v.CountryOfOrigin, quantity = v.Quantity.ToString(), unit_price = item_price,
                    unit_weight = item_weight, url = v.URL };
                items[counter] = Item;
                counter++;
            }
            return items;
        }

        private static billing CreateBilling(VwLogisticsProviderImport v, item[] orderItems)
        {
            money base_price_money = new money() { currency = currency.USD, Value = 0 };

            foreach (item i in orderItems)
            {
                base_price_money.Value += Int32.Parse(i.quantity) * i.unit_price.Value;
            }
            residential_indicator res = new residential_indicator() { enabled = v.Residential == "Y" ? true : false };
            address a = new address()
            {
                address1 = v.BillAddress1,
                address2 = v.BillAddress2,
                city = v.BillCity,
                company = v.BillCompany,
                country = v.Billcountry,
                email = v.BillEmail,
                name_first = v.BillFirst,
                name_last = v.BillLast,
                phone = v.BillPhone,
                residential_indicator = res,
                state = v.BillState,
                zip = v.BillZip
            };
            
            money shipping_money = new money() { currency = currency.USD, Value = Convert.ToDecimal(v.BilledShipping) };
            money tax_money = new money() { currency = currency.USD, Value = Convert.ToDecimal(v.Tax) };
            money total_money = new money() { currency = currency.USD, Value = Convert.ToDecimal(v.TotalSale) };
            billing b = new billing() { address = a, base_price = base_price_money, shipping = shipping_money, tax = tax_money, total = total_money };
            return b;
        }
    }
}
