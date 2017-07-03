using WatiN.Core;
using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class GMP_Orders_Browse_Page : GMP_Orders_Base_Page
    {
        private SelectList _status;
        private Pagination_Control _pagination;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _status = Document.SelectList("statusSelectFilter");
            _pagination = Document.GetElement<Div>(new Param("paginatedGridPagination")).As<Pagination_Control>();
        }

        public Pagination_Control Pagination
        {
            get { return _pagination; }
        }
        
         public override bool IsPageRendered()
        {
            return _status.Exists;
        }

        public GMP_Orders_Browse_Page SelectStatus(OrderStatus.ID status, int? timeout = null)
        {
            _status.CustomSelectDropdownItem(status.ToPattern(), timeout);
            return this;
        }

        public GMP_Orders_Browse_Page SelectType(OrderType.ID type, int? timeout = null)
        {
            _content.SelectList("typeSelectFilter").CustomSelectDropdownItem(type.ToPattern(), timeout);
            return this;
        }

        public GMP_Orders_Browse_Page SelectMarket(int index, int? timeout = null)
        {
            _content.SelectList("marketSelectFilter").CustomSelectDropdownItem((int)index, timeout);
            return this;
        }

        public GMP_Orders_Browse_Page EnterStartDate(DateTime start)
        {
            _content.GetElement<TextField>(new Param("startDateInputFilter")).CustomSetTextQuicklyHelper(start.ToShortDateString());
            return this;
        }

        public GMP_Orders_Browse_Page EnterEndDate(DateTime end)
        {
            _content.GetElement<TextField>(new Param("endDateInputFilter")).CustomSetTextQuicklyHelper(end.ToShortDateString());
            return this;
        }

        public GMP_Orders_Browse_Page SelectAccount(string account)
        {
            _content.GetElement<TextField>(new Param("accountNumberOrNameInputFilter")).As<SearchSuggestionBox_Control>().Select(account);
            return this;
        }

        public GMP_Orders_Browse_Page ClickApplyFilter(int? timeout = null, int? delay = 2)
        {
            _content.GetElement<Link>(new Param("Button ModSearch filterButton", AttributeName.ID.ClassName)).CustomClick(timeout);
            Thread.Sleep(3000); // Wait for the table to display
            return this;
        }

        private TableRowCollection GetTable(int? timeout = null, int? delay = 2)
        {
            Table tbl = Document.GetElement<Table>(new Param("paginatedGrid"));
            timeout = tbl.CustomWaitForSpinner(timeout, delay);
            Thread.Sleep(1000);
            TableBody tblBody = tbl.TableBody(Find.Any);
            tblBody.CustomWaitForExist(timeout);
            Thread.Sleep(1000);
            return tblBody.OwnTableRows;
        }

        public ControlCollection<GMP_Orders_Order_Control> GetOrders()
        {
            return GetTable().As<GMP_Orders_Order_Control>();
        }

        public GMP_Orders_Order_Control GetOrder(int? index = null)
        {
            var orders = GetTable();
            if (!index.HasValue)
                index = Util.GetRandom(0, orders.Count - 1);
            return orders[(int)index].As<GMP_Orders_Order_Control>();
        }

        public GMP_Orders_Order_Control GetOrder(string orderID)
        {
            GMP_Orders_Order_Control order = null;

            foreach (TableRow row in GetTable())
            {
                if (row.GetElement<TableCell>(new Param(1)).CustomGetText() == orderID)
                {
                    order = row.As<GMP_Orders_Order_Control>();
                    break;
                }
            }
            return order;
        }
    }
}
