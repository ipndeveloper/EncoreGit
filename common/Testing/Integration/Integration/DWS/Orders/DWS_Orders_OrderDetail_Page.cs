using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WatiN.Core;
using WatiN.Core.Extras;
using TestMasterHelpProvider;

namespace NetSteps.Testing.Integration.DWS.Orders
{
    public class DWS_Orders_OrderDetail_Page : DWS_Orders_Base_Page
    {
        private Div _divOrderNumber, _divShipAttn, _divShippingMethod;
        private Table _paymentDetails, _tableOrderHistory;
        private Link _lnkOrderHistory;

        protected override void InitializeContents()
        {
            base.InitializeContents();

            _divOrderNumber = _content.GetElement<Div>(new Param("PageTitle", AttributeName.ID.ClassName));
            _paymentDetails = _content.GetElement<Table>(new Param("DataGrid", AttributeName.ID.ClassName).And(new Param(1)));
            _lnkOrderHistory = _content.GetElement<Link>(new Param("Order History", AttributeName.ID.InnerText));
            _tableOrderHistory = _content.GetElement<Table>(new Param("paginatedGridOrders"));
            _divShipAttn = _content.GetElement<Div>(new Param("FL", AttributeName.ID.ClassName));
            _divShippingMethod = _content.GetElement<Div>(new Param("FL", AttributeName.ID.ClassName).And(new Param(1)));
        }

        #region Methods.

        public string GetOrderNumber()
        {
            return _divOrderNumber.GetElement<H1>().CustomGetText().Split('#')[1].Trim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">Zero based index</param>
        /// <returns></returns>
        public DWS_Orders_OrderDetails_Control GetOrderedProductDetails(int index)
        {
            return _content.GetElement<Table>(new Param("DataGrid", AttributeName.ID.ClassName, RegexOptions.None)).OwnTableRows[index + 1].As<DWS_Orders_OrderDetails_Control>();
        }

        /// <summary>
        /// Get Payment details details.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <returns>Payment details.</returns>
        public List<string> GetPaymentDetailsUnderOrderSummary(int row = 0)
        {
            return this._paymentDetails.GetTableRowsData(2, row);
        }

        /// <summary>
        /// Is orders page rendered.
        /// </summary>
        /// <returns>True if rendered, else false.</returns>
         public override bool IsPageRendered()
        {
            return Document.GetElement<Div>(new Param("Customers", AttributeName.ID.ClassName, RegexOptions.None)).Exists;
        }        

        #endregion

        #region validation Methods.

        /// <summary>
        /// Validate Shipping method details under DWS Order summary page.
        /// </summary>
        /// <param name="shippingMethod">Shipping method.</param>
        /// <returns>True if all validations are passed, else false.</returns>
        public bool ValidateShippingMethodDetails(string shippingMethod)
        {
            bool result = true;

            string shippingMethodDetails = this._divShippingMethod.CustomGetText();
            if (!Utils.AssertIsTrue(shippingMethodDetails.Contains(shippingMethod),
                string.Format("Shipping method '{0}' is properly shown under DWS order summary page.", shippingMethod))) result = false;

            return result;
        }

        /// <summary>
        /// Validate Payment details under DWS Order summary page.
        /// </summary>
        /// <param name="productPrice">Product price.</param>
        /// <param name="orderTotal">Order total price.</param>
        /// <returns>True if all validations are passed, else false.</returns>
        public bool ValidatePaymentDetails(string productPrice, string orderTotal)
        {
            bool result = true;

            List<string> paymentDetails = this.GetPaymentDetailsUnderOrderSummary();
            if (!Utils.AssertIsTrue(paymentDetails[5].Contains(productPrice),
                string.Format("Product price '{0}' is properly shown under Payment table of DWS order details page.", productPrice))) result = false;

            if (!Utils.AssertIsTrue(paymentDetails[9].Contains(orderTotal),
                string.Format("Product order total price captured from GMP Order details '{0}' is properly shown under Payment table of DWS order details page.", orderTotal))) result = false;

            double paymentTotal = Convert.ToDouble(paymentDetails[7].Trim().Remove(0, 1)) + Convert.ToDouble(paymentDetails[8].Trim().Remove(0, 1)) + Convert.ToDouble(productPrice);
            if (!Utils.AssertIsTrue(paymentDetails[9].Contains(paymentTotal.ToString()),
                string.Format("Product order total price (including taxes) '{0}' is properly shown under Payment table of DWS order details page.", paymentTotal.ToString()))) result = false;

            return result;
        }

        /// <summary>
        /// Validate Shipping details under DWS Order summary page.
        /// </summary>
        /// <param name="addressLineOne">Address line one.</param>
        /// <param name="addressLineTwo">Address line two.</param>
        /// <param name="postalCode">Postal code.</param>
        /// <returns>True if all validations are passed, else false.</returns>
        public bool ValidateShippingDetails(string addressLineOne, string addressLineTwo, string postalCode)
        {
            bool result = true;

            string shippingAddress = this._divShipAttn.CustomGetText();

            if (!Utils.AssertIsTrue(shippingAddress.Contains(addressLineOne),
                string.Format("Shipping address line one '{0}' is properly shown under DWS order summary page.", addressLineOne))) result = false;

            if (!Utils.AssertIsTrue(shippingAddress.Contains(addressLineTwo),
                string.Format("Shipping address line two '{0}' is properly shown under DWS order summary page.", addressLineTwo))) result = false;

            if (!Utils.AssertIsTrue(shippingAddress.Contains(postalCode),
                string.Format("Shipping address postal code '{0}' is properly shown under DWS order summary page.", postalCode))) result = false;

            return result;
        }

        #endregion
    }
}
