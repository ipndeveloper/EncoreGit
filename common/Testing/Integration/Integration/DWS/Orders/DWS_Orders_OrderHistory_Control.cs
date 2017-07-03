using System;
using System.Threading;
using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public class DWS_Orders_OrderHistory_Control : Control<Div>
    {
        private SelectList _orderStatus, _orderType;
        private TextField _startDate, _endDate;
        private Link _applyFilter;
        private Pagination_Control _pagination;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _orderStatus = Element.GetElement<SelectList>(new Param("statusSelectFilter"));
            _orderType = Element.GetElement<SelectList>(new Param("typeSelectFilter"));
            _startDate = Element.GetElement<TextField>(new Param("startDateInputFilter"));
            _endDate = Element.GetElement<TextField>(new Param("endDateInputFilter"));
            _applyFilter = Element.GetElement<Link>(new Param("Button ModSearch filterButton", AttributeName.ID.ClassName));
            _pagination = Element.GetElement<Div>(new Param("paginatedGridPaginationOrders")).As<Pagination_Control>();
        }

        public Pagination_Control Pagination
        {
            get { return _pagination; }
        }

        public DWS_Orders_OrderHistory_Control SelectStatus(OrderStatus.ID orderStatusID, int? timeout = null, bool pageRequired = true)
        {
            timeout = _orderStatus.CustomSelectDropdownItem(orderStatusID.ToPattern(), timeout);
            return this;
        }

        public DWS_Orders_OrderHistory_Control SelectOrderType(OrderType.ID orderType)
        {
            _orderType.CustomSelectDropdownItem(orderType.ToPattern());
            return this;
        }

        public DWS_Orders_OrderHistory_Control EnterStartDate(DateTime start)
        {
            _startDate.CustomSetTextQuicklyHelper(start.ToShortDateString());
            return this;
        }

        public DWS_Orders_OrderHistory_Control EnterEndDate(DateTime end)
        {
            _endDate.CustomSetTextQuicklyHelper(end.ToShortDateString());
            return this;
        }

        public DWS_Orders_OrderHistory_Control ClickApplyFilter(int? timeout = null, int? delay = 2)
        {
            _applyFilter.CustomClick(null);
            Element.CustomWaitForSpinners(timeout, delay);
            return this;
        }

        public ControlCollection<DWS_Orders_Order_Control> GetOrders()
        {
            return GetOrderTable().As<DWS_Orders_Order_Control>();
        }

        public DWS_Orders_Order_Control GetOrder(int?index = null)
        {
            ElementCollection<TableRow> rows = GetOrderTable();
            if (!index.HasValue)
                index = Util.GetRandom(0, rows.Count - 1);
            return rows[(int)index].As<DWS_Orders_Order_Control>();
        }

        public DWS_Orders_Order_Control GetOrder(string orderId)
        {
            bool result = false;
            DWS_Orders_Order_Control order = null;
            ElementCollection<TableRow> rows = GetOrderTable();

            foreach (TableRow row in rows)
            {
                order = row.As<DWS_Orders_Order_Control>();
                if (order.Number == orderId)
                {
                    result = true;
                    break;
                }
            }
            if (!result)
                throw new ArgumentException(string.Format("Order {0} not found", orderId));
            return order;
        }

        private ElementCollection<TableRow> GetOrderTable(int? timeout = null, int? delay = 1)
        {
            Element.CustomWaitForSpinners(timeout, delay);
            Thread.Sleep(1000);
            return Element.GetElement<Table>(new Param("paginatedGridOrders")).GetElement<TableBody>().GetElements<TableRow>();
        }
    }
}
