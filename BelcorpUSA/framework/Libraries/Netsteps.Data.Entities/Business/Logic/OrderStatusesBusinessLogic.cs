using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class OrderStatusesBusinessLogic
    {

        public IEnumerable<dynamic> GetAllOrderStatuses()
        {
            var table = new OrderStatuses();
            return table.All();
        }

        public IEnumerable<dynamic> GetOrderStatusesByOrderStatusIdArray(string orderStatusIdArray)
        {
            var table = new OrderStatuses();

            var orderStatusList = table.Query("EXEC UPS_GET_ORDERSTATUSES_BY_ORDERSTATUSID_ARRAY @0", orderStatusIdArray);

            return orderStatusList;
        }

    }
}