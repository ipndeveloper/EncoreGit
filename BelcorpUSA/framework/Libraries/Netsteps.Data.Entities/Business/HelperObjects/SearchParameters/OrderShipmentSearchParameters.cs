using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business
{
    public class OrderShipmentSearchParameters : FilterDateRangePaginatedListParameters<Order>
    {
        public short? OrderTypeID { get; set; }
        
        public short? OrderStatusID { get; set; }
        
        public short? OrderShipmentStatusID { get; set; }

        private string _orderNumber;
        public string OrderNumber
        {
            get
            {
                return _orderNumber;
            }
            set
            {
                _orderNumber = value != null ? value.Trim() : null;
            }
        }
        
        public IEnumerable<int> OrderIDs { get; set; }
    }
}
