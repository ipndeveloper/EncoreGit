using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Common.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public class MockOrder : IOrder
    {
        public MockOrder(int accountID)
        {
            _previousOrderAdjustmentIDs = new List<int>();
            OrderCustomers.Add(new MockOrderCustomer() { AccountID = accountID });
        }
        public short OrderStatusID { get; set; }

        public decimal? ShippingTotal { get; set; }

        private List<IOrderCustomer> _orderCustomers;
        public IList<IOrderCustomer> OrderCustomers
        {
            get
            {
                if (_orderCustomers == null)
                    _orderCustomers = new List<IOrderCustomer>();
                return _orderCustomers;
            }
        }

        private List<IOrderAdjustment> _orderAdjustments;
        public IList<IOrderAdjustment> OrderAdjustments
        {
            get
            {
                if (_orderAdjustments == null)
                    _orderAdjustments = new List<IOrderAdjustment>();
                return _orderAdjustments;
            }
        }

        public bool isSaved = false;

        public void Save()
        {
            this.RecursiveExecuteForChildren<IExtensibleDataObject>((x) =>
            {
                IDataObjectExtensionProvider provider = Create.New<IDataObjectExtensionProviderRegistry>().RetrieveExtensionProvider(x.ExtensionProviderKey);
                provider.UpdateDataObjectExtension(x);
                provider.SaveDataObjectExtension(x);
            });
            isSaved = true;
        }

        public IOrderItem AddItem(int productId, int quantity)
        {
            if (OrderCustomers.Count == 0)
                OrderCustomers.Add(new MockOrderCustomer());
            IOrderItem item = new MockOrderItem() { ProductID = productId, Quantity = quantity };
            OrderCustomers[0].OrderItems.Add(item);
            return item;
        }

        public IOrderItem AddItem(IOrderCustomer customer, int productId, int quantity, bool calculateTotals = false, string parentGuid = null, int? dynamicKitGroupId = null, short? orderItemParentTypeID = null, bool disableDuplicateChecking = true)
        {
            if (OrderCustomers.Count == 0)
                OrderCustomers.Add(new MockOrderCustomer());
            IOrderItem item = new MockOrderItem() { ProductID = productId, Quantity = quantity };
            OrderCustomers[0].OrderItems.Add(item);
            return item;
        }


        public int OrderID { get; set; }


        public bool CalculationsDirty { get; set; }

        public void CalculateTotals()
        {

        }

        public decimal? GrandTotal { get; set; }

        public short OrderTypeID { get; set; }

        public int GetShippingMarketID()
        {
            return 1;
        }


        public decimal? Subtotal { get; set; }


        public void AddOrderAdjustment(IOrderAdjustment adjustment)
        {
            this.OrderAdjustments.Add(adjustment);
        }


        public bool RemoveItem(int orderItemId)
        {
            foreach (IOrderCustomer customer in OrderCustomers)
            {
                if (customer.OrderItems.Any(x => { return x.OrderItemID == orderItemId; }))
                {
                    List<IOrderItem> items = customer.OrderItems.Where(x => x.OrderItemID == orderItemId).ToList();
                    foreach (IOrderItem item in items)
                    {
                        customer.OrderItems.Remove(item);
                    }
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItem(IOrderItem orderItem)
        {
            foreach (IOrderCustomer customer in OrderCustomers)
            {
                if (customer.OrderItems.Any(x => { return x == orderItem; }))
                {
                    customer.OrderItems.Remove(orderItem);
                    return true;
                }
            }
            return false;
        }

        public int CurrencyID { get; set; }


        public List<int> _previousOrderAdjustmentIDs { get; private set; }
        public IEnumerable<int> GetExistingOrderAdjustmentIDsForAccount(int accountID)
        {
            return _previousOrderAdjustmentIDs;
        }


        public IList<IAutoshipOrder> AutoshipOrders
        {
            get { throw new System.NotImplementedException(); }
        }

        public int? ParentOrderID
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
                throw new System.NotImplementedException();
            }
        }


        public bool ClearAdjustments()
        {
            _orderAdjustments.Clear();
            return true;
        }


		public decimal GetDefaultShippingTotal()
		{
			throw new System.NotImplementedException();
		}


        public ICollection<IOrder> ChildOrders
        {
            get { throw new System.NotImplementedException(); }
        }

		public IOrderItem AddItem(IOrderCustomer customer, int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null, short? orderItemParentTypeID = null, bool disableDuplicateChecking = true)
		{
			throw new System.NotImplementedException();
		}

		public int ConsultantID { get; set; }
	}
}
