using System;
using System.Collections.Generic;
using NetSteps.Data.Common.Entities;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public class MockOrderCustomer : IOrderCustomer
    {
        public List<IOrderItem> _orderItems;
        public IList<IOrderItem> OrderItems
        {
            get
            {
                if (_orderItems == null)
                    _orderItems = new List<IOrderItem>();
                return _orderItems;
            }
        }


        public short AccountTypeID
        {
            get { throw new NotImplementedException(); }
        }


        public int AccountID { get; set; }

        public List<IOrderAdjustmentOrderModification> _orderModifications;
        public IList<IOrderAdjustmentOrderModification> OrderModifications
        {
            get
           { 
                if (_orderModifications == null)
                    _orderModifications = new List<IOrderAdjustmentOrderModification>();
                return _orderModifications;
            }
        }


        public void AddOrderModification(IOrderAdjustmentOrderModification modification)
        {
            OrderModifications.Add(modification);
        }


        public decimal AdjustedHandlingTotal
        {
            get { throw new NotImplementedException(); }
        }

        public decimal AdjustedShippingTotal
        {
            get { throw new NotImplementedException(); }
        }

        public decimal AdjustedTaxTotal
        {
            get { throw new NotImplementedException(); }
        }


		public decimal ShippingAdjustmentAmount
		{
			get { throw new NotImplementedException(); }
		}


		public decimal AdjustedSubTotal
		{
			get { throw new NotImplementedException(); }
		}

		public decimal ProductSubTotal
		{
			get { throw new NotImplementedException(); }
		}

		public IEnumerable<IOrderItem> AdjustableOrderItems
		{
			get { return OrderItems; }
		}


		public short OrderCustomerTypeID
		{
			get { throw new NotImplementedException(); }
		}
	}
}
