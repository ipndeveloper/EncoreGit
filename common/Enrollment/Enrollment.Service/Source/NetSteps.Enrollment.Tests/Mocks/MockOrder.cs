using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Entities;

namespace NetSteps.Enrollment.Service.Tests.Mocks
{
	public class MockOrder : IOrder
	{
		public void AddOrderAdjustment(IOrderAdjustment adjustment)
		{
			throw new NotImplementedException();
		}

		public void Save()
		{
			throw new NotImplementedException();
		}

		public IOrderItem AddItem(int productId, int quantity)
		{
			throw new NotImplementedException();
		}

		public IOrderItem AddItem(IOrderCustomer customer, int productId, int quantity, bool calculateTotals = false, string parentGuid = null, int? dynamicKitGroupId = new int?(), short? orderItemParentTypeID = new short?(), bool disableDuplicateChecking = true)
		{
			throw new NotImplementedException();
		}

		public void CalculateTotals()
		{
			throw new NotImplementedException();
		}

		public int GetShippingMarketID()
		{
			throw new NotImplementedException();
		}

		public bool RemoveItem(int orderItemId)
		{
			throw new NotImplementedException();
		}

		public bool RemoveItem(IOrderItem orderItem)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<int> GetExistingOrderAdjustmentIDsForAccount(int accountID)
		{
			throw new NotImplementedException();
		}

		public bool ClearAdjustments()
		{
			throw new NotImplementedException();
		}

		public decimal GetDefaultShippingTotal()
		{
			throw new NotImplementedException();
		}

		public int OrderID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public short OrderStatusID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public decimal? ShippingTotal
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public IList<IOrderCustomer> OrderCustomers
		{
			get { throw new NotImplementedException(); }
		}

		public IList<IOrderAdjustment> OrderAdjustments
		{
			get { throw new NotImplementedException(); }
		}

		public bool CalculationsDirty
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public decimal? GrandTotal
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public short OrderTypeID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public decimal? Subtotal
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int CurrencyID
		{
			get { throw new NotImplementedException(); }
			set { throw new NotImplementedException(); }
		}

		public int? ParentOrderID { get; set; }

		public IList<IAutoshipOrder> AutoshipOrders
		{
			get { throw new NotImplementedException(); }
		}

		public int ConsultantID { get; set; }

		public IOrderItem AddItem(IOrderCustomer customer, int productId, int quantity, string parentGuid = null, int? dynamicKitGroupId = null, short? orderItemParentTypeID = null, bool disableDuplicateChecking = true)
		{
			throw new NotImplementedException();
		}

		public ICollection<IOrder> ChildOrders
		{
			get { throw new NotImplementedException(); }
		}
	}
}
