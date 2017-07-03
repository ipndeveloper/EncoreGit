using System;
using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Business.Logic
{
    class NegotiationOrderPaymentsBusinessLogic
    {
        public dynamic GetAllNegotiationOrderByOrderRuleId(int orderRuleId)
        {
            dynamic orderRuleTicketPayment = new OrderRuleNegotiationOrderPayments();
            var result = orderRuleTicketPayment.Find(OrderRuleID: orderRuleId);
            return result;
        }

        public bool Insert(dynamic model)
        {

            dynamic orderRuleNegotiation = new OrderRuleNegotiationOrderPayments();

            try
            {

                var orderRuleId = model.OrderRuleID;
                var negotiationLevelId = model.NegotiationLevelID;
                var quantityMax = model.QuantityMax;
                const int cumulative = 0;

                orderRuleNegotiation.Insert(new
                {
                    OrderRuleID = orderRuleId,
                    NegotiationLevelID = negotiationLevelId,
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

            var orderRuleTicketPayment = new OrderRuleNegotiationOrderPayments();

            try
            {

                var orderRuleId = model.OrderRuleID;
                var negotiationLevelId = model.NegotiationLevelID;
                var quantityMax = model.QuantityMax;
                const int cumulative = 0;

                orderRuleTicketPayment.Update(new
                {
                    OrderRuleID = orderRuleId,
                    NegotiationLevelID = negotiationLevelId,
                    QuantityMax = quantityMax,
                    Cumulative = cumulative
                },
                model.OrderRuleNegotiationOrderPaymentID);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ExistsOrderRuleByRuleIdAndStatusId(int orderRuleId, int paymentStatusId)
        {
            var table = new OrderRuleNegotiationOrderPayments();
            var result = table.All(where: "OrderRuleID=@0 AND NegotiationLevelID=@1",
                args: new object[] { orderRuleId, paymentStatusId });
            return result.Any();
        }

        public IEnumerable<dynamic> GetOrderRuleByRuleIdAndStatusId(int orderRuleId, int paymentStatusId)
        {
            var table = new OrderRuleNegotiationOrderPayments();
            var result = table.All(where: "OrderRuleID=@0 AND NegotiationLevelID=@1",
                args: new object[]{ orderRuleId, paymentStatusId });
            return result;
        }


        public void DeleteNegotiedPaymentRulesByRuleIdAndStatusId(int orderRuleId, int[] statusIds)
        {
            string strSql;

            if (statusIds.Length > 0)
            {
                var statusIdArray = statusIds.Aggregate<int, string>(String.Empty, (x, y) => (x.Length > 0 ? x + "," : x) + y.ToString(""));
                strSql = string.Format("DELETE FROM OrderRuleNegotiationOrderPayments WHERE OrderRuleID ={0} AND NegotiationLevelID NOT IN ({1})", orderRuleId, statusIdArray);
            }
            else
            {
                strSql = string.Format("DELETE FROM OrderRuleNegotiationOrderPayments WHERE OrderRuleID ={0}", orderRuleId);
            }

            var table = new OrderRuleNegotiationOrderPayments();
            table.Execute(strSql);
        }

    }

}
