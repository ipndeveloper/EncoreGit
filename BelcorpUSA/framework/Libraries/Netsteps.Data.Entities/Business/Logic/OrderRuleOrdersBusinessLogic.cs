using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class OrderRuleOrdersBusinessLogic
    {

        public dynamic GetAllOrderRulesOrdersByOrderRuleId(int orderRuleId)
        {
            dynamic table = new OrderRuleOrders();
            var result = table.Find(OrderRuleID: orderRuleId);
            return result;
        }

        public dynamic ExistsOrderRuleByRuleIdAndStatusId(int orderRuleId, int orderStatusId)
        {
            var table = new OrderRuleOrders();
            var result = table.All(where: "OrderRuleID = @0 AND OrderStatusID = @1", args: new object[] { orderRuleId, orderStatusId });
            return result.Any();
        }

        public IEnumerable<dynamic> GetOrderRuleByRuleIdAndStatusId(int orderRuleId, int orderStatusId)
        {
            var table = new OrderRuleOrders();
            var result = table.All(where: "OrderRuleID = @0 AND OrderStatusID = @1", args: new object[] { orderRuleId, orderStatusId });
            return result;
        }

        public void DeleteOrderRulesByRuleIdAndStatusId(int orderRuleId, int[] statusIds)
        {
            string strSql;

            if (statusIds.Length > 0)
            {
                var statusIdArray = statusIds.Aggregate<int, string>(String.Empty, (x, y) => (x.Length > 0 ? x + "," : x) + y.ToString(""));
                strSql = string.Format("DELETE FROM OrderRuleOrders WHERE OrderRuleID = {0} AND OrderStatusID NOT IN ({1})", orderRuleId, statusIdArray);
            }
            else
            {
                strSql = string.Format("DELETE FROM OrderRuleOrders WHERE OrderRuleID = {0}", orderRuleId);
            }

            var table = new OrderRuleOrders();
            table.Execute(strSql);
        }

        public bool Insert(dynamic model)
        {
            var table = new OrderRuleOrders();

            try
            {
                table.Insert(new
                {
                    OrderRuleID = model.OrderRuleID,
                    OrderStatusID = model.OrderStatusID,
                    QuantityMax = model.QuantityMax,
                    Cumulative = model.Cumulative
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(dynamic model)
        {
            var table = new OrderRuleOrders();

            try
            {
                table.Update(new
                {
                    OrderRuleID = model.OrderRuleID,
                    OrderStatusID = model.OrderStatusID,
                    QuantityMax = model.QuantityMax,
                    Cumulative = model.Cumulative
                },
                model.OrderRuleOrdersID
                );

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}