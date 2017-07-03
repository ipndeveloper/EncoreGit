using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class OrderStatusTokenValueProvider : ITokenValueProvider
    {
        private readonly Order _order;

        private const string ORDER_NUMBER = "OrderNumber";
        private const string ORDER_STATUS = "OrderStatus";
        private const string ORDER_PURCHASE_DATE = "OrderPurchaseDate";
        private const string SHIPPING_ADDRESS = "ShippingAddress";
        private const string SHIPPING_METHOD = "ShippingMethod";
        private const string BILLING_ADDRESS = "BillingAddress";
        private const string ORDER_ITEMS = "OrderItems";
        private const string ORDER_TOTALS = "OrderTotals";
        private const string DISTRIBUTOR_FULL_NAME = "DistributorFullName";
        private const string ORDER_COMPLETE_DATE = "OrderDatePlaced";
        private const string ORDER_SHIPPING_ADDRESS = "OrderShippingAddress";
        private const string ORDER_SHIPPING_METHOD = "OrderShippingMethod";
        private const string ORDER_BILLING_ADDRESS = "OrderBillingAddress";
        private const string DISTRIBUTOR_PWSURL = "DistributorPWSUrl";



        private const string ORDER_ITEMS_TABLE_HEADER_HTML = @"<style>table.order-items td &#123;padding-left:5px;&#125; 
table.order-items th &#123;padding-left:10px;&#125;
.quantity, td.extended-price &#123;text-align:right;&#125;</style>
<table class=""order-items"">
	<tr>
		<th class=""sku"">{0}</th>
		<th class=""product"">{1}</th>
		<th class=""unit-price"">{2}</th>
		<th class=""quantity"">{3}</th>
		<th class=""extended-price"">{4}</th>
	</tr>";

        private const string ORDER_ITEMS_TABLE_ROW_HTML = @"	<tr>
		<td class=""sku"">{0}</td>
		<td class=""product"">{1}</td>
		<td class=""unit-price"">{2}</td>
		<td class=""quantity"">{3}</td>
		<td class=""extended-price"">{4}</td>
	</tr>";

        private const string ORDER_ITEMS_TABLE_FOOTER_HTML = @"</table>";

        private const string ORDER_TOTALS_HTML = @"<style>table.order-total &#123;width: 100%&#125; 
td.currency &#123;text-align:right;&#125;</style>
<table class=""order-total"">
<tr><td>{0}</td><td class=""currency"">{1}</td></tr>
<tr><td>{2}</td><td class=""currency"">{3}</td></tr>
<tr><td>{4}</td><td class=""currency"">{5}</td></tr>
<tr><td>{6}</td><td class=""currency"">{7}</td></tr>
</table>";

        public OrderStatusTokenValueProvider(Order order)
        {
            _order = order;
            if (_order.Consultant == null)
                _order.Consultant = Account.LoadFull(_order.ConsultantID);
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
                       {
                           ORDER_NUMBER,
                           ORDER_STATUS,
                           ORDER_PURCHASE_DATE,
                           SHIPPING_ADDRESS,
                           SHIPPING_METHOD,
                           BILLING_ADDRESS,
                           ORDER_ITEMS,
                           DISTRIBUTOR_FULL_NAME, 
                           ORDER_COMPLETE_DATE,
                           ORDER_SHIPPING_ADDRESS,
                           ORDER_SHIPPING_METHOD, 
                           ORDER_BILLING_ADDRESS, 
                           ORDER_TOTALS,
                           DISTRIBUTOR_PWSURL
                       };
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case ORDER_NUMBER:
                    return _order.OrderNumber;

                case ORDER_STATUS:
                    int OrderValue = _order.OrderStatusID;
                    Constants.OrderStatus EnumOrderValue = (Constants.OrderStatus)OrderValue;
                    return EnumOrderValue.ToString();
                    
                case ORDER_PURCHASE_DATE:
                    return _order.CompleteDate.ToShortDateString();

                case ORDER_SHIPPING_ADDRESS:
                case SHIPPING_ADDRESS:
                    var address = _order.GetDefaultShipment() as IAddress;
                    return address != null ? address.ToDisplay(IAddressExtensions.AddressDisplayTypes.Web) : null;

                case ORDER_SHIPPING_METHOD:
                case SHIPPING_METHOD:
                    var shipment = _order.GetDefaultShipment();
                    return shipment != null ? shipment.ShippingMethodName : null;

                case ORDER_BILLING_ADDRESS:
                case BILLING_ADDRESS:
                    var orderCustomer = _order.OrderCustomers != null ? _order.OrderCustomers.FirstOrDefault() : null;
                    if (orderCustomer != null)
                    {
                        var payment = orderCustomer.OrderPayments != null ? orderCustomer.OrderPayments.FirstOrDefault() as IPayment : null;
                        return payment != null ? payment.ToDisplay() : null;
                    }
                    return null;

                case ORDER_ITEMS:
                    return GetOrderItemsHtml();
                case ORDER_TOTALS:
                    return GetOrderTotalsHtml();
                case DISTRIBUTOR_FULL_NAME:
                    return _order.Consultant != null ? _order.Consultant.FullName : string.Empty;
                case ORDER_COMPLETE_DATE:
                    if (_order.CompleteDate != null && _order.Consultant != null)
                        return _order.CompleteDate.ToShortDateStringDisplay(_order.Consultant.AccountCultureInfo);
                    return string.Empty;
                case DISTRIBUTOR_PWSURL:
                    if (_order.Consultant != null)
                    {
                        var sites = Site.LoadByAccountID(_order.ConsultantID);
                        Site site = sites.Count > 1 ? sites.FirstOrDefault(s => s.SiteStatusID == Constants.SiteStatus.Active.ToShort() && s.PrimaryUrl != null && !s.PrimaryUrl.Url.IsNullOrEmpty()) : sites.FirstOrDefault();
                        return site == null ? null : site.PrimaryUrl == null ? null : site.PrimaryUrl.Url;
                    }
                    return string.Empty;
                default:
                    return null;
            }
        }

        public string GetOrderItemsHtml()
        {
            StringBuilder itemsTableHtml = new StringBuilder();
            itemsTableHtml.AppendFormat(ORDER_ITEMS_TABLE_HEADER_HTML, Translation.GetTerm("SKU"), Translation.GetTerm("Product"),
                Translation.GetTerm("Price"), Translation.GetTerm("Quantity"), Translation.GetTerm("Total"));

            if (_order.OrderCustomers != null && _order.OrderCustomers.Count > 0 && _order.OrderCustomers[0].OrderItems != null)
            {
                foreach (var item in _order.OrderCustomers[0].OrderItems)
                {
                    itemsTableHtml.AppendFormat(ORDER_ITEMS_TABLE_ROW_HTML, item.SKU, item.ProductName,
                                                item.ItemPrice.ToString(_order.CurrencyID), item.Quantity,
                                                ((item.Quantity * item.ItemPrice).ToString(_order.CurrencyID)));
                }
            }

            itemsTableHtml.Append(ORDER_ITEMS_TABLE_FOOTER_HTML);

            return itemsTableHtml.ToString();
        }

        public string GetOrderTotalsHtml()
        {
            return String.Format(ORDER_TOTALS_HTML, Translation.GetTerm("Subtotal"), _order.Subtotal.ToString(_order.CurrencyID),
                                 Translation.GetTerm("Tax"), _order.TaxAmountTotal.ToString(_order.CurrencyID),
                                 Translation.GetTerm("ShippingAndHandling", "Shipping & Handling"), (_order.ShippingTotal + _order.HandlingTotal).ToString(_order.CurrencyID),
                                 Translation.GetTerm("Total"), _order.GrandTotal.ToString(_order.CurrencyID));
        }
    }
}
