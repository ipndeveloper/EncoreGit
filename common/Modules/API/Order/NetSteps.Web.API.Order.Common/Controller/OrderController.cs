using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Order.Common;
using NetSteps.Web.API.Base.Common;
using NetSteps.Web.API.Order.Common.Models;
using NetSteps.Encore.Core;
using NetSteps.Modules.Order.Common.Results;

namespace NetSteps.Web.API.Order.Common
{
	/// <summary>
	/// functions for Orders
	/// </summary>
    public class OrderController : BaseController
    {

        #region Declarations

        private ISiteOrder orderModule;

        private ILogResolver logResolver;

        private ITermResolver termResolver;

		private readonly static ICopier<LoadOrderModel, ILoadOrderModel> _loadOrderCopier = Create.New<ICopier<LoadOrderModel, ILoadOrderModel>>();

        private readonly static ICopier<OrderViewModel, IOrderCreate> _orderModelCopier = Create.New<ICopier<OrderViewModel, IOrderCreate>>();

        private readonly static ICopier<ProductViewModel, IProduct> _productModelCopier = Create.New<ICopier<ProductViewModel, IProduct>>();

        #endregion

        #region Constructor(s)

		/// <summary>
		/// Create an instance
		/// </summary>
        public OrderController() : this(Create.New<ISiteOrder>(), Create.New<ILogResolver>(), Create.New<ITermResolver>())
        {
            Contract.Ensures(orderModule != null);
            Contract.Ensures(termResolver != null);
            Contract.Ensures(logResolver != null);
        }
		/// <summary>
		/// Create an instance
		/// </summary>
		/// <param name="ordModule">Order Module</param>
		/// <param name="lResolver">Log Resolver</param>
		/// <param name="tResolver">Term Resolver</param>
        public OrderController(ISiteOrder ordModule, ILogResolver lResolver, ITermResolver tResolver)
        {
            this.orderModule = ordModule;
            this.termResolver = tResolver;
            this.logResolver = lResolver;
        }

        #endregion

        #region Helper Methods

        private bool ValidateNumberOfRecords(int numberOfRecords, ref string message)
        {
            bool isValid = true;

            if (numberOfRecords == 0)
            {
                isValid = false;
                string term = termResolver.Term("Order_Invalid_NumberOfRecords", "Invalid Number Of Records:");
                message = string.IsNullOrEmpty(message)
                    ? string.Format("{0} {1}", term, numberOfRecords)
                    : string.Format("{0} {1} {2}", message, term, numberOfRecords);
            }

            return isValid;
        }

        private bool ValidateOrderID(int orderID, ref string message)
        {
            bool isValid = true;

            if (orderID == 0)
            {
                isValid = false;
                string term = termResolver.Term("Order_Invalid_OrderID", "Invalid OrderID:");
                message = string.IsNullOrEmpty(message)
                    ? string.Format("{0} {1}", term, orderID)
                    : string.Format("{0} {1} {2}", message, term, orderID);
            }

            return isValid;
        }

        private bool ValidateAccountID(int userID, LoadOrderModel model, ref string message)
        {
            bool isValid = true;

            if (userID == 0)
            {
                isValid = false;
                string term = termResolver.Term("Order_Invalid_UserID", "Invalid UserID:");
                message = string.Format("{0} {1}", term, userID);
            }
            if (userID != model.AccountID)
            {
                isValid = false;
                string term = termResolver.Term("Order_UserID_Mismatch", "AccountID in the route does not match the AccountID in the model");
                message = string.Format("{0} {1}", term, userID);
            }

            return isValid;
        }

        private IOrderCreateResult ValidateOrderViewModel(OrderViewModel model)
        {
            IOrderCreateResult result = Create.New<IOrderCreateResult>();
            result.ErrorMessages = new List<string>();
            result.Success = true;

            string term = string.Empty;

			if (!model.GetQuote && (!model.ShippingMethodID.HasValue || model.ShippingMethodID.Value == 0))
			{
				result.ErrorMessages.Add(termResolver.Term("Order_Invalid_Shipping_Method", "Invalid Shipping Method."));
				result.Success = false;
			}
            if (model.AccountID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Order_Invalid_AccountID", "Invalid AccountID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, model.AccountID));
            }

            if (model.CurrencyID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Order_Invalid_CurrencyID", "Invalid CurrencyID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, model.CurrencyID));
            }

            if (model.AccountTypeID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Order_Invalid_AccountTypeID", "Invalid AccountTypeID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, model.AccountTypeID));
            }

            if (model.OrderTypeID == 0)
            {
                result.Success = false;
                term = termResolver.Term("Order_Invalid_OrderTypeID", "Invalid OrderTypeID:");
                result.ErrorMessages.Add(string.Format("{0} {1}", term, model.OrderTypeID));
            }

            if (model.Products.Count == 0)
            {
                result.Success = false;
                term = termResolver.Term("Order_No_Products", "There are no products on this order.");
                result.ErrorMessages.Add(term);
            }
            else
            {
                result = ValidateProductViewModels(model.Products, result);
            }

            return result;
        }

        private IOrderCreateResult ValidateProductViewModels(IList<ProductViewModel> products, IOrderCreateResult result)
        {
            string term = string.Empty;

            foreach (ProductViewModel product in products)
            {
                if (product.ProductID <= 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Order_Invalid_ProductID", "Invalid ProductID:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, product.ProductID));
                }

                if (product.Quantity <= 0)
                {
                    result.Success = false;
                    term = termResolver.Term("Order_Invalid_Quantity", "Invalid Quantity:");
                    result.ErrorMessages.Add(string.Format("{0} {1}", term, product.Quantity));
                }
            }

            return result;
        }

        private IOrderCreate CopyOrderViewModelToDTO(OrderViewModel order)
        {
            var dto = Create.New<IOrderCreate>();
            _orderModelCopier.CopyTo(dto, order, CopyKind.Loose, Container.Current);
            
            var products = new List<IProduct>();

            if (order.Products != null)
            {
                foreach (var product in order.Products)
                {
                    var productDTO = Create.New<IProduct>();
                    _productModelCopier.CopyTo(productDTO, product, CopyKind.Loose, Container.Current);

                    products.Add(productDTO);
                }
            }

            dto.Products = products;

            return dto;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Create an account, a user, an enrollment order, 
        /// a product subscription (optional), a site subscription (optional)
        /// 
        /// /// eg. http://yourdomain.com/orders/
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiAccessKeyFilter]
        public ActionResult CreateOrder(OrderViewModel model)
        {
            try
            {
                string message = string.Empty;

                var orderResult = ValidateOrderViewModel(model);

                if (orderResult.Success)
                {
                    var dto = CopyOrderViewModelToDTO(model);
                    
                    IOrderCreateResult result = orderModule.CreateOrder(dto);

                    return this.Result_200_OK(result);
                }

                return this.Result_400_BadRequest(orderResult);

            }            
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                return this.Result_400_BadRequest(ex.Message);
            }
        }

		/// <summary>
		/// Load orders from an account
		/// 
		/// eg. http://yourdomain.com/account/{accountID}/orders/{numberOfRecords}
		/// </summary>
		/// <param name="accountID">account whose orders you want to load</param>
		/// <param name="model">Load Order Model</param>
		/// <returns>ActionResult</returns>
		/// <seealso cref="ActionResult"/>
        [HttpGet]
        [ApiAccessKeyFilter]
        public ActionResult LoadOrders(int accountID, LoadOrderModel model)
        {
            try
            {
                string message = string.Empty;

				bool isValidUserID = ValidateAccountID(accountID, model, ref message);

				bool isValidNumberOfRecords = model.NumberOfRecords.HasValue ? ValidateNumberOfRecords(model.NumberOfRecords.Value, ref message) : true;

				if (isValidUserID && isValidNumberOfRecords)
                {
					var dto = Create.New<ILoadOrderModel>();
					_loadOrderCopier.CopyTo(dto, model, CopyKind.Loose, Container.Current);

					IEnumerable<IOrderSearchResult> result = orderModule.LoadOrders(dto);

                    return this.Result_200_OK(result);
                }

                return this.Result_400_BadRequest(message);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                return this.Result_400_BadRequest(ex.Message);
            }
        }
        
        /// <summary>
        /// Move an Order to a new account
		/// 
		/// eg. http://yourdomain.com/order/{orderID}/{accountID}
        /// </summary>
        /// <param name="orderID">Order to move</param>
        /// <param name="accountID">Account to move the order to</param>
		/// <returns>ActionResult</returns>
		/// <seealso cref="ActionResult"/>
        [HttpPut]
        [ApiAccessKeyFilter]
		public ActionResult MoveOrder(int orderID, int accountID)
        {
            try
            {
                string message = string.Empty;

				bool isValidUserID = ValidateAccountID(accountID, new LoadOrderModel() { AccountID = accountID }, ref message);

				bool isValidOrderID = ValidateOrderID(accountID, ref message);

                if (isValidUserID && isValidOrderID)
                {
					IOrderMoveResult result = orderModule.MoveOrder(orderID, accountID);

                    return this.Result_200_OK(result);
                }

                return this.Result_400_BadRequest(message);
            }
            catch (Exception ex)
            {
                logResolver.LogException(ex, true);

                return this.Result_400_BadRequest(ex.Message);
            }
        }

        #endregion

    }
}
