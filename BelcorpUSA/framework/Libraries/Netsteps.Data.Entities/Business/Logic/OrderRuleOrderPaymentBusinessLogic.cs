using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class OrderRuleOrderPaymentBusinessLogic
    {

        public dynamic GetAllOrderRuleTicketPaymentByOrderRuleId(int orderRuleId)
        {
            dynamic orderRuleTicketPayment = new OrderRuleOrderPayments();
            var result = orderRuleTicketPayment.Find(OrderRuleID: orderRuleId);
            return result;
        }

        public bool Insert(dynamic model)
        {

            dynamic orderRuleTicketPayment = new OrderRuleOrderPayments();

            try
            {

                var orderRuleId = model.OrderRuleID;
                var expirationStatusId = model.ExpirationStatusID;
                var quantityMax = model.QuantityMax;
                const int cumulative = 0;

                orderRuleTicketPayment.Insert(new
                {
                    OrderRuleID = orderRuleId,
                    ExpirationStatusID = expirationStatusId,
                    QuantityMax = quantityMax,
                    Cumulative = cumulative
                });

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(dynamic model)
        {

            var orderRuleTicketPayment = new OrderRuleOrderPayments();

            try
            {

                var orderRuleId = model.OrderRuleID;
                var expirationStatusId = model.ExpirationStatusID;
                var quantityMax = model.QuantityMax;
                const int cumulative = 0;

                orderRuleTicketPayment.Update(new
                {
                    OrderRuleID = orderRuleId,
                    ExpirationStatusID = expirationStatusId,
                    QuantityMax = quantityMax,
                    Cumulative = cumulative
                },
                model.OrderRuleOrderPaymentID);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ExistsOrderRuleByRuleIdAndStatusId(int orderRuleId, int paymentStatusId)
        {
            var table = new OrderRuleOrderPayments();
            var result = table.All(where: "OrderRuleID=@0 AND ExpirationStatusID=@1",
                args: new object[] { orderRuleId, paymentStatusId });
            return result.Any();
        }

        public IEnumerable<dynamic> GetOrderRuleByRuleIdAndStatusId(int orderRuleId, int paymentStatusId)
        {
            var table = new OrderRuleOrderPayments();
            var result = table.All(where: "OrderRuleID=@0 AND ExpirationStatusID=@1",
                args: new object[] { orderRuleId, paymentStatusId });
            return result;
        }


        public void DeleteTicketPaymentRulesByRuleIdAndStatusId(int orderRuleId, int[] statusIds)
        {
            string strSql;

            if (statusIds.Length > 0)
            {
                var statusIdArray = statusIds.Aggregate<int, string>(String.Empty, (x, y) => (x.Length > 0 ? x + "," : x) + y.ToString(""));
                strSql = string.Format("DELETE FROM OrderRuleOrderPayments WHERE OrderRuleID ={0} AND ExpirationStatusID NOT IN ({1})", orderRuleId, statusIdArray);
            }
            else
            {
                strSql = string.Format("DELETE FROM OrderRuleOrderPayments WHERE OrderRuleID ={0}", orderRuleId);
            }

            var table = new OrderRuleOrderPayments();
            table.Execute(strSql);
        }

    }
}