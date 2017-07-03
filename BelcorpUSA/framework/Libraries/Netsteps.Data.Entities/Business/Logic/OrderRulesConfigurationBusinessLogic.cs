using System;
using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public class OrderRulesConfigurationBusinessLogic
    {
        
        public bool OrderRulesConfiguration(dynamic model)
        {

            var orderRules = new OrdersRulesBusinessLogic();
            var orderRulesOrders = new OrderRuleOrdersBusinessLogic();
            var ticketPaymentRules = new OrderRuleOrderPaymentBusinessLogic();
            var negotiedPaymentRules = new NegotiationOrderPaymentsBusinessLogic();

            dynamic orderPerday = model.orderPerDay;
            dynamic orderWithoutPayment = model.orderWithoutPayment;
            dynamic ticketPayment = model.ticketPayment;
            dynamic ticketNegotiation = model.ticketNegotiation;

            if (! orderRules.HasOrderRules())
            {
                int orderRuleId = orderRules.Insert("MaxOrderPerDay","MaxOrderPerDay");

                orderPerday.OrderRuleID = orderRuleId;

                foreach (var item in orderPerday.List)
                {
                    orderPerday.OrderStatusID = item;
                    orderPerday.QuantityMax = orderPerday.quantity;
                    orderPerday.Cumulative = 0;
                    orderRulesOrders.Insert(orderPerday);
                }

                orderRuleId = orderRules.Insert("MaxOrderWithoutPayment", "MaxOrderWithoutPayment");

                orderWithoutPayment.OrderRuleID = orderRuleId;

                foreach (var item in orderWithoutPayment.List)
                {
                    orderWithoutPayment.OrderStatusID = item;
                    orderWithoutPayment.QuantityMax = orderWithoutPayment.quantity;
                    orderWithoutPayment.Cumulative = 0;
                    orderRulesOrders.Insert(orderWithoutPayment);
                }

                orderRuleId = orderRules.Insert("MaxOfTicketsPayment", "MaxOfTicketsPayment");

                ticketPayment.OrderRuleID = orderRuleId;

                foreach (var item in ticketPayment.List)
                {
                    ticketPayment.ExpirationStatusID = item;
                    ticketPayment.QuantityMax = ticketPayment.quantity;
                    ticketPayment.Cumulative = 0;
                    ticketPaymentRules.Insert(ticketPayment);
                }

                orderRuleId = orderRules.Insert("MaxOfTicketsPaymentNegotied", "MaxOfTicketsPaymentNegotied");
                ticketNegotiation.OrderRuleID = orderRuleId;

                foreach (var item in ticketNegotiation.List)
                {
                    ticketNegotiation.NegotiationLevelID = item;
                    ticketNegotiation.QuantityMax = ticketNegotiation.quantity;
                    ticketNegotiation.Cumulative = 0;
                    negotiedPaymentRules.Insert(ticketNegotiation);
                }
            }
            else
            {
               var orderRulesTypeList = orderRules.GetAllOrdersRules();

               foreach (var item in orderRulesTypeList)
               {
                   string termName = item.TermName.ToString();
                   int orderRuleId = item.OrderRuleID;
                   List<int> statusIdList;
                           
                   switch (termName)
                   {
                       case "MaxOrderPerDay":
                           statusIdList = new List<int>();
                           foreach (var xItem in orderPerday.List)
                           {
                               statusIdList.Add(Convert.ToInt32(xItem));

                               var existRule = orderRulesOrders.ExistsOrderRuleByRuleIdAndStatusId(orderRuleId, Convert.ToInt32(xItem));

                               orderPerday.OrderRuleID = orderRuleId;
                               orderPerday.OrderStatusID = xItem;
                               orderPerday.QuantityMax = orderPerday.quantity;
                                   
                               if (! existRule)
                               {
                                   orderPerday.Cumulative = 0;
                                   orderRulesOrders.Insert(orderPerday);    
                               }
                               else
                               {
                                   var orderRule = orderRulesOrders.GetOrderRuleByRuleIdAndStatusId(orderRuleId, Convert.ToInt32(xItem));
                                   int orderRuleOrdersId = 0;
                                   bool cumulative = false;
                                   foreach (var xitem in orderRule)
                                   {
                                       orderRuleOrdersId = xitem.OrderRuleOrdersID;
                                       cumulative = xitem.Cumulative;
                                   }

                                   orderPerday.OrderRuleOrdersID = orderRuleOrdersId;
                                   orderPerday.Cumulative = cumulative;
                                   orderRulesOrders.Update(orderPerday);    
                               }
                           }

                           orderRulesOrders.DeleteOrderRulesByRuleIdAndStatusId(orderRuleId, statusIdList.ToArray());

                           break;
                       case "MaxOrderWithoutPayment":
                           statusIdList = new List<int>();
                           
                           foreach (var xItem in orderWithoutPayment.List)
                           {
                               statusIdList.Add(Convert.ToInt32(xItem));

                               var existRule = orderRulesOrders.ExistsOrderRuleByRuleIdAndStatusId(orderRuleId, Convert.ToInt32(xItem));

                               orderWithoutPayment.OrderRuleID = orderRuleId;
                               orderWithoutPayment.OrderStatusID = xItem;
                               orderWithoutPayment.QuantityMax = orderWithoutPayment.quantity;
                                   
                               if (! existRule)
                               {
                                   orderWithoutPayment.Cumulative = 0;
                                   orderRulesOrders.Insert(orderWithoutPayment);    
                               }
                               else
                               {
                                   var orderRule = orderRulesOrders.GetOrderRuleByRuleIdAndStatusId(orderRuleId, Convert.ToInt32(xItem));
                                   int orderRuleOrdersId = 0;
                                   bool cumulative = false;
                                   foreach (var xitem in orderRule)
                                   {
                                       orderRuleOrdersId = xitem.OrderRuleOrdersID;
                                       cumulative = xitem.Cumulative;
                                   }

                                   orderWithoutPayment.OrderRuleOrdersID = orderRuleOrdersId;
                                   orderWithoutPayment.Cumulative = cumulative;
                                   orderRulesOrders.Update(orderWithoutPayment);    
                               }
                           }

                           orderRulesOrders.DeleteOrderRulesByRuleIdAndStatusId(orderRuleId, statusIdList.ToArray());

                           break;
                       case "MaxOfTicketsPayment":

                           statusIdList = new List<int>();

                           foreach (var xItem in ticketPayment.List)
                           {
                               statusIdList.Add(Convert.ToInt32(xItem));

                               var existRule = ticketPaymentRules.ExistsOrderRuleByRuleIdAndStatusId(orderRuleId, Convert.ToInt32(xItem));
                               
                               ticketPayment.OrderRuleID = orderRuleId;
                               ticketPayment.ExpirationStatusID = xItem;
                               ticketPayment.QuantityMax = ticketPayment.quantity;
                                   
                               if (! existRule)
                               {
                                   ticketPayment.Cumulative = 0;
                                   ticketPaymentRules.Insert(ticketPayment);    
                               }
                               else
                               {
                                   var orderRule = ticketPaymentRules.GetOrderRuleByRuleIdAndStatusId(orderRuleId, Convert.ToInt32(xItem));
                                   bool cumulative = false;
                                   foreach (var xitem in orderRule)
                                   {
                                       cumulative = xitem.Cumulative;
                                       ticketPayment.OrderRuleOrderPaymentID = xitem.OrderRuleOrderPaymentID;
                                   }

                                   ticketPayment.Cumulative = cumulative;
                                   ticketPaymentRules.Update(ticketPayment);    
                               }
                           }

                           ticketPaymentRules.DeleteTicketPaymentRulesByRuleIdAndStatusId(orderRuleId, statusIdList.ToArray());
                           break;
                       case "MaxOfTicketsPaymentNegotied":
                           statusIdList = new List<int>();

                           foreach (var xItem in ticketNegotiation.List)
                           {
                               statusIdList.Add(Convert.ToInt32(xItem));

                               var existRule = negotiedPaymentRules.ExistsOrderRuleByRuleIdAndStatusId(orderRuleId, Convert.ToInt32(xItem));

                               ticketNegotiation.OrderRuleID = orderRuleId;
                               ticketNegotiation.NegotiationLevelID = xItem;
                               ticketNegotiation.QuantityMax = ticketNegotiation.quantity;
                                   
                               if (! existRule)
                               {
                                   ticketNegotiation.Cumulative = 0;
                                   negotiedPaymentRules.Insert(ticketNegotiation);    
                               }
                               else
                               {
                                   var orderRule = negotiedPaymentRules.GetOrderRuleByRuleIdAndStatusId(orderRuleId, Convert.ToInt32(xItem));
                                   bool cumulative = false;
                                   foreach (var xitem in orderRule)
                                   {
                                       cumulative = xitem.Cumulative;
                                       ticketNegotiation.OrderRuleNegotiationOrderPaymentID =
                                           xitem.OrderRuleNegotiationOrderPaymentID;
                                   }

                                   ticketNegotiation.Cumulative = cumulative;
                                   negotiedPaymentRules.Update(ticketNegotiation);    
                               }
                           }

                           negotiedPaymentRules.DeleteNegotiedPaymentRulesByRuleIdAndStatusId(orderRuleId, statusIdList.ToArray());
                           break;
                   }
               }      
            }

            return true;
        }


        public List<dynamic> GetOrderRulesConfiguration()
        {
            var orderRules = new OrdersRulesBusinessLogic();
            dynamic orderRulesList = orderRules.GetAllOrdersRules();

            var orderRulesConfig = new List<dynamic>();

            foreach (var item in orderRulesList)
            {
                if (item.TermName == "MaxOrderPerDay" || item.TermName == "MaxOrderWithoutPayment")
                {
                    var orderRulesOrderBl = new OrderRuleOrdersBusinessLogic();
                    var list = orderRulesOrderBl.GetAllOrderRulesOrdersByOrderRuleId(item.OrderRuleID);
                    orderRulesConfig.Add(new { TermName = item.TermName, List = list });
                }
                else if (item.TermName == "MaxOfTicketsPayment")
                {
                    var orderRuleTicketPaymentBl = new OrderRuleOrderPaymentBusinessLogic();
                    var list = orderRuleTicketPaymentBl.GetAllOrderRuleTicketPaymentByOrderRuleId(item.OrderRuleID);
                    orderRulesConfig.Add(new { TermName = item.TermName, List = list });
                }
                else if (item.TermName == "MaxOfTicketsPaymentNegotied")
                {

                    var orderRuleNegotiedPaymentBl = new NegotiationOrderPaymentsBusinessLogic();
                    var list = orderRuleNegotiedPaymentBl.GetAllNegotiationOrderByOrderRuleId(item.OrderRuleID);
                    orderRulesConfig.Add(new { TermName = item.TermName, List = list });
                }             
            }

            return orderRulesConfig;
        }
    }
}