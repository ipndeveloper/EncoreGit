using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Common.Entities;

namespace NetSteps.OrderAdjustments.Service.Test.Mocks
{
    public class MockOrderItem : IOrderItem
    {
        public MockOrderItem()
        {
            IsDeleted = false;
            OrderLineModifications = new List<IOrderAdjustmentOrderLineModification>();
        }

        public int? ProductID { get; set; }

        public int Quantity { get; set; }

        public bool IsDeleted { get; set; }

        public void MarkAsDeleted()
        {
            IsDeleted = true;
        }


        public decimal ItemPrice { get; set; }
        public IList<IOrderAdjustmentOrderLineModification> OrderLineModifications { get; private set; }

        public int OrderItemID { get; set; }

        public decimal AdjustedItemPrice
        {
            get { throw new NotImplementedException(); }
        }

        public decimal AdjustedOrderItemCommissionableVolume
        {
            get { throw new NotImplementedException(); }
        }

        public decimal AdjustedOrderItemPrice
        {
            get { throw new NotImplementedException(); }
        }

        public decimal AdjustedOrderItemQualifyingVolume
        {
            get { throw new NotImplementedException(); }
        }


        public IOrderItem ParentOrderItem
        {
            get { return null; }
        }

        public void AddOrderLineModification(IOrderAdjustmentOrderLineModification lineModification)
        {
            OrderLineModifications.Add(lineModification);
        }


		public decimal GetAdjustedPrice(int priceTypeID)
		{
			throw new NotImplementedException();
		}

		public void AddOrUpdateOrderItemProperty(string name, string value)
		{
			throw new NotImplementedException();
		}

		public IList<IOrderItemProperty> OrderItemProperties
		{
			get { throw new NotImplementedException(); }
		}

		public int ProductPriceTypeID { get; set; }
	}
}
