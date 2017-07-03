using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WatiN.Core;
using WatiN.Core.Extras;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public class DWS_Orders_OrderEntry_Page : DWS_Orders_Base_Page
    {
        private TextField _quantity, _itemQuantity, _amountToApply;
        private Link _updateCart, _addItem, _removeItem, _bulkOrderAdd;
        private SearchSuggestionBox_Control _sku;
        private TableRowCollection _orders;
        private Table _orderTable;
        private Div _personOrderDetails, _bulkModal;
        private Link _submitOrder, _applyPayment;
        private OrderTotal_Control _orderTotals;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _sku = Control.CreateControl<SearchSuggestionBox_Control>(Document.GetElement<TextField>(new Param("txtQuickAddSearch")));
            _quantity = Document.GetElement<TextField>(new Param("txtQuickAddQuantity"));
            _updateCart = Document.GetElement<Link>(new Param("btnUpdateCart"));
            _addItem = Document.GetElement<Link>(new Param("btnQuickAdd"));
            _removeItem = Document.GetElement<Link>(new Param("RemoveOrderItem", AttributeName.ID.ClassName, RegexOptions.None));
            _itemQuantity = Document.GetElement<TextField>(new Param("quantity", AttributeName.ID.ClassName));
            _orderTable = Document.GetElement<Table>(new Param("products"));
            if (Document.GetElement<Div>(new Param("orderEntryArea")).Exists)
                _personOrderDetails = Document.GetElement<Div>(new Param("orderEntryArea"));
            else if (Document.GetElement<Div>(new Param("PersonalOrderDetails", AttributeName.ID.ClassName, RegexOptions.None)).Exists)
                _personOrderDetails = Document.GetElement<Div>(new Param("PersonalOrderDetails", AttributeName.ID.ClassName, RegexOptions.None));
            else
                _personOrderDetails = Document.GetElement<Div>(new Param("ContentContainer"));
            _bulkOrderAdd = Document.GetElement<Link>(new Param("btnOpenBulkAdd"));
            _applyPayment = Document.GetElement<Link>(new Param("btnApplyPayment"));
            _amountToApply = Document.GetElement<TextField>(new Param("txtPaymentAmount"));
            _submitOrder = Document.GetElement<Link>(new Param("btnSubmitOrder"));
            _orderTotals = _content.GetElement<TableRow>(new Param("totalBar")).As<OrderTotal_Control>();
        }

        public override bool IsPageRendered()
        {
            return _sku.Exists;
        }

        public bool BulkModalVisible
        {
            get { return Document.GetElement<Div>(new Param("bulkAddModal")).IsVisible(); }
        }

        public OrderTotal_Control OrderTotals
        {
            get { return _orderTotals; }
        }

        public void AddItem(OrderItem item, int? timeout = null)
        {
            _sku.Select(item.Product.SKU);
            timeout = _addItem.CustomClick(timeout);
            _personOrderDetails.CustomWaitForSpinners(timeout);
        }

        public void AddItem(string item, string match = null, bool exactMatch = false, int? timeout = null)
        {
            _sku.Select(item, match, exactMatch, timeout);
            timeout = _addItem.CustomClick(timeout);
            _personOrderDetails.CustomWaitForSpinners(timeout);
        }

        public DWS_Orders_BulkAdd_Control OpenBulkAdd()
        {
            _bulkOrderAdd.CustomClick();
            _bulkModal = _content.GetElement<Div>(new Param("bulkAddModal"));
            _bulkModal.CustomWaitForVisibility();
            _bulkModal.CustomWaitForSpinner();
            return _bulkModal.As<DWS_Orders_BulkAdd_Control>();
        }

        public ControlCollection<OrderItem_Control> GetOrderItems(int skuIndex = 0)
        {
            RefreshCart(_orderTable);
            ControlCollection<OrderItem_Control> orderItems = _orders.As<OrderItem_Control>();
            foreach (OrderItem_Control orderItem in orderItems)
            {
                orderItem.Configure(skuIndex);
            }
            return orderItems;
        }

        public OrderItem_Control GetOrderItem(int? index = null, int skuIndex = 0)
        {
            RefreshCart(_orderTable);
            if (!index.HasValue)
                index = Util.GetRandom(0, _orderTable.TableRows.Count - 1);
            return _orders[(int)index].As<OrderItem_Control>().Configure(skuIndex);
        }

        public OrderItem_Control GetOrderItem(string sku, int skuIndex = 0)
        {
            OrderItem_Control order = null;
            bool found = false;
            RefreshCart(_orderTable);

            foreach (TableRow row in _orders)
            {
                order = row.As<OrderItem_Control>().Configure(skuIndex);
                if (order.Sku == sku)
                {
                    found = true;
                    break;
                }
            }
            if (found == false)
                throw new ArgumentException(string.Format("Order item {0} not found", sku));
            return order;
        }

        public void UpdateOrder(int? timeout = null)
        {
            timeout = _updateCart.CustomClick(timeout);
            _content.GetElement<Div>(new Param("GridFilters", AttributeName.ID.ClassName, RegexOptions.None)).CustomWaitForSpinners(timeout);
        }

        public void ApplyPayment()
        {
            _applyPayment.CustomClick();
            _personOrderDetails.CustomWaitForSpinners();
        }

        public DWS_Orders_OrderDetail_Page SubmitOrder()
        {
            _submitOrder.CustomClick();
            Document.CustomWaitForSpinners();
            return Util.GetPage<DWS_Orders_OrderDetail_Page>();
        }

        private void RefreshCart(Table table, int? timeout = null, int? delay = 3)
        {
            _personOrderDetails.CustomWaitForSpinners(timeout, delay);
            _orders = null;
            _orders = _orderTable.TableBody(Find.Any).TableRows;
        }

    }
}
