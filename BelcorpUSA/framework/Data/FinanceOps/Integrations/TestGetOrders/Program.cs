using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Integrations.Service;
using NetstepsDataAccess.DataEntities;
using NetSteps.Integrations.Service.OrderExport;

namespace NetSteps.Integrations.Service
{
    class Program
    {
        static void Main(string[] args)
        {
            IntegrationsService serv = new IntegrationsService();
            string s = GetOrders();  
        }

        public static string GetOrders()
        {
            List<uspIntegrationsOrderExportResult> list = null;
            using (NetStepsEntities db = new NetStepsEntities())
                list = db.uspIntegrationsOrderExport().ToList();

            if (list.Count > 0)
            {
                // apply group by clause
                var GroupedList = (from l in list
                                   group l by new { l.AccountID, l.DateCreatedUTC, l.OrderNumber }
                                       into grp
                                       select new { grp.Key.AccountID, grp.Key.DateCreatedUTC, grp.Key.OrderNumber }).ToList();

                // Create Order Collection XML
                OrderExportCollection col = new OrderExportCollection();
                // Create List of Orders for XML
                List<OrderExport.Order> orders = new List<OrderExport.Order>();

                foreach (var g in GroupedList)
                {
                    // create order items list
                    List<OrderExport.OrderItem> listOfItems = new List<OrderExport.OrderItem>();

                    // get items for this Order
                    var items = from i in list
                                where i.OrderNumber == g.OrderNumber
                                select i;

                    // add each item for this order to the list
                    foreach (var item in items)
                    {
                        OrderExport.OrderItem orderItem = new OrderExport.OrderItem()
                        {
                            OrderItemID = item.OrderItemID,
                            Quantity = item.Quantity,
                            SKU = item.SKU
                        };
                        listOfItems.Add(orderItem);
                    }

                    // create an Order and add the list of items.
                    OrderExport.Order o = new OrderExport.Order()
                    {
                        CustomerID = g.AccountID,
                        OrderDate = g.DateCreatedUTC,
                        OrderNumber = g.OrderNumber,
                        OrderItem = listOfItems
                    };
                    orders.Add(o);
                }

                // add Order to XML Order Collection and return Serialized XML
                col.Order = orders;
                return col.Serialize();
            }
            return null;
        }
    }
}
