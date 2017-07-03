using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;
using NetSteps.Content.Common;
using NetSteps.Modules.Order.Common.Results;

namespace NetSteps.Modules.Order.Common
{
	/// <summary>
	/// Default Implementation of ISiteOrder
	/// </summary>
	[ContainerRegister(typeof(ISiteOrder), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class DefaultOrder : ISiteOrder
    {

        #region Declarations

        private readonly IOrderRepositoryAdapter _orderRepository;

        private readonly ITermResolver _termTranslation;

        #endregion

        #region Constructors

        /// <summary>
		/// Default Constructor
		/// </summary>
        public DefaultOrder() : this(Create.New<IOrderRepositoryAdapter>(), Create.New<ITermResolver>()) { }

		/// <summary>
        /// Constructor with Dependency Injection
		/// </summary>
		/// <param name="orderRepository"></param>
		/// <param name="termTranslation"></param>
        public DefaultOrder(IOrderRepositoryAdapter orderRepository, ITermResolver termTranslation)
		{
            _orderRepository = orderRepository ?? Create.New<IOrderRepositoryAdapter>();
            _termTranslation = termTranslation ?? Create.New<ITermResolver>();
        }

        #endregion

        #region Helper Methods

        private IEnumerable<IOrderSearchResult> GetOrderResults(IEnumerable<IOrder> orders)
		{
			IList<IOrderSearchResult> results = new List<IOrderSearchResult>();

			foreach (var order in orders)
			{
				IOrderSearchResult result = Create.New<IOrderSearchResult>();
				result.Date = order.Date;
				result.OrderID = order.OrderID;
				result.OwnerID = order.OwnerID;
				result.Status = order.Status;
				result.Total = order.Total;
				result.Volume = order.Volume;
				result.OrderTypeID = order.OrderTypeID;

				results.Add(result);
			}

			return results.AsEnumerable();
		}

        private IOrderCreateResult ValidateOrder(IOrderCreate order, IOrderCreateResult result)
        {
			if (!order.GetQuote && (!order.ShippingMethodID.HasValue || order.ShippingMethodID.Value == 0))
			{
				result.ErrorMessages.Add(_termTranslation.Term("CreateOrder_Invalid_Shipping_Method", "Invalid Shipping Method."));
				result.Success = false;
			}
            if (order.AccountID == 0)
            {
                result.ErrorMessages.Add(_termTranslation.Term("CreateOrder_Invalid_Account_Number", "Invalid Account Number."));
                result.Success = false;
            }

            if (order.AccountTypeID == 0)
            {
                result.ErrorMessages.Add(_termTranslation.Term("CreateOrder_Invalid_Account_Type", "Invalid Account Type."));
                result.Success = false;
            }

            if (order.CurrencyID == 0)
            {
                result.ErrorMessages.Add(_termTranslation.Term("CreateOrder_Invalid_Currency", "Invalid Currency."));
                result.Success = false;
            }

            if (order.OrderTypeID == 0)
            {
                result.ErrorMessages.Add(_termTranslation.Term("CreateOrder_Invalid_Order_Type", "Invalid Order Type."));
                result.Success = false;
            }

            if (order.SiteID == 0)
            {
                result.ErrorMessages.Add(_termTranslation.Term("CreateOrder_Invalid_SiteID", "Invalid SiteID."));
                result.Success = false;
            }

            if (order.Products.Count == 0)
            {
                result.Success = false;
                result.ErrorMessages.Add(_termTranslation.Term("CreateOrder_Missing_Products", "Missing Products."));
            }
            else
            {
                foreach (IProduct product in order.Products)
                {
                    if (product.ProductID == 0)
                    {
                        result.Success = false;
                        result.ErrorMessages.Add(_termTranslation.Term("CreateOrder_Missing_ProductID", "Missing ProductID."));
                    }

                    if (product.Quantity == 0)
                    {
                        result.Success = false;
                        result.ErrorMessages.Add(_termTranslation.Term("CreateOrder_Missing_Quantity", "Missing Quantity."));
                    }
                }
            }

            return result;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create a new Order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public IOrderCreateResult CreateOrder(IOrderCreate order)
        {        
            var result = Create.New<IOrderCreateResult>();
            result.ErrorMessages = new List<string>();
            result.Success = true;

            var validation = ValidateOrder(order, result);

            if (validation.Success)
            {
                var createOrderResult = _orderRepository.CreateOrder(order);
                result.ErrorMessages.AddRange(createOrderResult.ErrorMessages);
                result.OrderID = createOrderResult.OrderID;
                result.Success = createOrderResult.Success;
				result.GrandTotal = createOrderResult.GrandTotal;
				result.OrderItems = createOrderResult.OrderItems;
				result.Shipping = createOrderResult.Shipping;
				result.ShippingMethod = createOrderResult.ShippingMethod;
				result.ShippingMethods = createOrderResult.ShippingMethods;
				result.Subtotal = createOrderResult.Subtotal;
				result.Tax = createOrderResult.Tax;
            }

            return result;
        }        

		/// <summary>
		/// Load orders for a given account
		/// </summary>
		/// <param name="model">Load Orders Model</param>
		/// <returns></returns>
		public IEnumerable<IOrderSearchResult> LoadOrders(ILoadOrderModel model)
		{
            var orders = _orderRepository.LoadOrders(model);

			IEnumerable<IOrderSearchResult> results = GetOrderResults(orders);

			return results;
		}

		/// <summary>
		/// Move an order to a new Account
		/// </summary>
		/// <param name="orderID"></param>
		/// <param name="userID">AccountID</param>
		/// <returns></returns>
		public IOrderMoveResult MoveOrder(int orderID, int userID)
		{
            var order = _orderRepository.MoveOrder(orderID, userID);

			IOrderMoveResult result = Create.New<IOrderMoveResult>();
			result.AccountID = order.AccountID;
			result.Success = order.Success;

			return result;
        }

		#endregion
	}
}
