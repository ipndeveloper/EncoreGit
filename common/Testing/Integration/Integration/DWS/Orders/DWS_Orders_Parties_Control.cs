using System;
using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public class DWS_Orders_Parties_Control : Control<Div>
    {
        private Pagination_Control _pagination;

        protected override void InitializeContents()
        {
            _pagination = Element.GetElement<Div>(new Param("paginatedGridPaginationOpenParties")).As<Pagination_Control>();
        }

        public Pagination_Control Pagination
        {
            get { return _pagination; }
        }

        public ControlCollection<DWS_Orders_Order_Control> GetPartyOrders()
        {
            ControlCollection<DWS_Orders_Order_Control> orders = GetPartyOrderTable().As<DWS_Orders_Order_Control>();
            foreach(DWS_Orders_Order_Control order in orders)
            {
                order.Configure(1);
            }
            return orders;
        }

        public DWS_Orders_Order_Control GetPartyOrder(int? index)
        {
            ElementCollection<TableRow> rows = GetPartyOrderTable();
            if (!index.HasValue)
            {
                index = Util.GetRandom(0, rows.Count - 1);
            }
            return rows[(int)index].As<DWS_Orders_Order_Control>().Configure(1);
        }

        public DWS_Orders_Order_Control GetPartyOrder(string orderId)
        {
            DWS_Orders_Order_Control order = null;
            ElementCollection<TableRow> rows = GetPartyOrderTable();

            foreach (TableRow row in rows)
            {
                order = row.As<DWS_Orders_Order_Control>().Configure(1);
                if (order.Number == orderId)
                {
                    break;
                }
            }
            if (order.Number != orderId)
                throw new ArgumentException(string.Format("Order {0} not found", orderId));
            return order;
        }

        private ElementCollection<TableRow> GetPartyOrderTable(int? timeout = null, int? delay = 2)
        {
            Element.CustomWaitForSpinners(timeout, delay);
            return Element.GetElement<Table>(new Param("paginatedGridOpenParties")).GetElement<TableBody>().GetElements<TableRow>();
        }
    }
}
