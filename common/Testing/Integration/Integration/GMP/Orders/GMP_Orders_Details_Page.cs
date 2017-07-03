using WatiN.Core;
using System.Collections.Generic;
using System;
using WatiN.Core.Extras;
using TestMasterHelpProvider;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class GMP_Orders_Details_Page : GMP_Orders_Base_Page
    {
        #region Controls

        private Div divOrderSummary;
        private Div divOrderDetails;
        private Table tableOrderDetails;
        private Table tableSelectedProducts;
        private Table tableShipToDetails;
        private Table tableShipMethodDetails;
        private Table tablePaymentDetails;
        private Div lnkCustomer;
        private Label lblOrderStatus;
        private Span spanDistributorName;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            this.divOrderSummary = Document.GetElement<Div>(new Param("TagInfo", AttributeName.ID.ClassName));
            this.divOrderDetails = Document.GetElement<Div>(new Param("SectionHeader", AttributeName.ID.ClassName));
            this.tableOrderDetails = Document.GetElement<Table>(new Param("FormTable Section", AttributeName.ID.ClassName));
            this.tableSelectedProducts = Document.GetElement<Table>(new Param("DataGrid", AttributeName.ID.ClassName));
            this.tableShipToDetails = Document.GetElement<Table>(new Param("FormTable Section", AttributeName.ID.ClassName).And(new Param(1)));
            this.tableShipMethodDetails = Document.GetElement<Table>(new Param("FormTable Section", AttributeName.ID.ClassName).And(new Param(2)));
            this.tablePaymentDetails = Document.GetElement<Table>(new Param("DataGrid", AttributeName.ID.ClassName).And(new Param(1)));
            this.lnkCustomer = Document.GetElement<Div>(new Param("TagInfo", AttributeName.ID.ClassName));
            this.lblOrderStatus = Document.GetElement<Label>(new Param("spnOrderStatus"));
            this.spanDistributorName = Document.GetElement<Span>(new Param("consultant", AttributeName.ID.ClassName));
        }

        #endregion

        #region Methods

         public override bool IsPageRendered()
        {
            return divOrderDetails.Exists;
        }

        public GMP_Orders_Details_Page AddDetails(GMP_Orders_Order_Control order)
        {
            string orderText = Document.GetElement<TableCell>(new Param("CoreContent", AttributeName.ID.ClassName)).GetElement<Table>(new Param(0)).GetElement<TableRow>().GetElement<TableCell>(new Param(1)).GetElement<Div>().CustomGetText();
            int start = orderText.IndexOf("Created Date:") + 14;
            int count = orderText.IndexOf(' ', start) - start;
            order.CreateDate = DateTime.Parse(orderText.Substring(start, count));
            return this;
        }
        
        /// <summary>
        /// Get selected products details.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <returns>Selected product details.</returns>
        public List<string> GetSelectedProductDetails(int row = 0)
        {
            return this.tableSelectedProducts.GetTableRowsData(1, row);
        }

        /// <summary>
        /// Get Shipping address details from GMP order summary page.
        /// </summary>
        /// <returns>Shipping address details.</returns>
        public List<string> GetShippingAddressDetailsFromOrderSummary()
        {
            //this.tableShipToDetails.CustomWaitUntilExist();
            return this.tableShipToDetails.GetTableRowsData(0);
        }

        /// <summary>
        /// Get payment details under order details page.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <returns>Payment details.</returns>
        public List<string> GetPaymentDetailsUnderOrderDetails(int row = 0)
        {
            return this.tablePaymentDetails.GetTableRowsData(2, row);
        }

        /// <summary>
        /// Click on customer name.
        /// </summary>
        /// <param name="customerName">Customer name.</param>
        public void ClickOnCustomerName(string customerName, int? timeout = null)
        {
            Link customerLink = this.divOrderSummary.ElementsOfType<Link>()[0];
            customerLink.CustomClick(timeout);
        }

        /// <summary>
        /// Get Shipping method details.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <returns>Shipping method details.</returns>
        public List<string> GetShippingMethodDetails(int row = 0)
        {
            return this.tableShipMethodDetails.GetTableRowsData(row);
        }

        /// <summary>
        /// Get Order number from Url.
        /// </summary>
        /// <returns>Order number.</returns>
        public string GetOrderNumber()
        {
            string Url = Util.Browser.Url;
            string OrderNumber = Url.Substring(Url.LastIndexOf('/'), (Url.Length - Url.LastIndexOf('/'))).Remove(0, 1);
            return OrderNumber;
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validate customer order summary, product details and shipping address.
        /// </summary>
        /// <param name="customerName">Customer name.</param>
        /// <param name="accountID">Account id.</param>
        /// <param name="orderId">Order id.</param>
        /// <param name="orderStatus">Order status.</param>
        /// <param name="paymanetDues">Payment amount.</param>
        /// <param name="productId">Product id.0</param>
        /// <param name="productName">Product name.0</param>
        /// <param name="productQuantity">Product quantity.0</param>
        /// <param name="productPrice">Product price.0</param>
        /// <param name="mainAddressOne">Main address one.</param>
        /// <param name="mainAddressTwo">Main address two.</param>
        /// <param name="postalCode">Postal code.</param>
        /// <param name="product1Id">Product id.1</param>
        /// <param name="product1Name">Product name.1</param>
        /// <param name="product1Quantity">Product quantity.1</param>
        /// <param name="product1Price">Product price.1</param>
        /// <returns>True if all validations are passed.</returns>
        public bool ValidateCustomerOrderSummaryForTwoProducts(string customerName, string accountID, string orderId, string orderStatus, string paymanetDues, string productId, string productName, string productQuantity, string productPrice, string mainAddressOne, string mainAddressTwo, string postalCode, string product1Id = "", string product1Name = "", string product1Quantity = "", string product1Price = "")
        {
            bool result = true;

            // Validate customer order summary data.
            string orderSummary = this.divOrderSummary.CustomGetText();
            if (!Utils.AssertIsTrue(orderSummary.Contains(customerName), string.Format("Customer name '{0}' is shown properly under yellow widget.", customerName))) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(accountID), string.Format("Account ID '{0}' is shown properly under yellow widget.", accountID))) result = false;

            if (!Utils.AssertIsTrue(orderSummary.Contains(orderId), string.Format("Order ID '{0}' shown properly under yellow widget.", orderId))) result = false;

            string date = orderSummary.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[4].Trim();
            if (!Utils.AssertIsTrue((orderSummary.Contains(DateTime.UtcNow.AddHours(Util.UtcTimeDiff).ToString("M/d/yyyy"))) ||
               (orderSummary.Contains(DateTime.Now.ToString("M/d/yyyy"))), string.Format("Date of order '{0}' is properly shown under yellow widget.", date))) result = false;
            
            if (!Utils.AssertIsTrue(orderSummary.Contains(DateTime.UtcNow.AddHours(Util.UtcTimeDiff).ToShortDateString()), "Date of order is shown properly under yellow widget.")) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(orderStatus), string.Format("Order status '{0}' and shown properly under yellow widget.", orderStatus))) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(paymanetDues), string.Format("Order total '{0}' shown properly under yellow widget.", paymanetDues))) result = false;

            // Validate 1st product details from order details page.
            result = result && this.ValidateProductDetails(productId, productName, productPrice, productQuantity, productPrice);

            // Validate 2nd product details from order details page.
            result = result && this.ValidateProductDetails(product1Id, product1Name, product1Price, product1Quantity, product1Price, 1);

            // Validate default shipping address details.
            result = result && this.ValidateShippingAddress(mainAddressOne, mainAddressTwo, postalCode);

            return result;
        }

        /// <summary>
        /// Validate Shipping address.
        /// </summary>
        /// <param name="addressOne">Address line one.</param>
        /// <param name="addressTwo">Address line two.</param>
        /// <param name="postalCode">Address postal code.</param>
        /// <returns>True if all validations are passed.</returns>
        public bool ValidateShippingAddress(string addressOne, string addressTwo, string postalCode)
        {
            bool result = true;

            List<string> shippingDetails = this.GetShippingAddressDetailsFromOrderSummary();
            if (!Utils.AssertIsTrue(shippingDetails[1].Equals(addressOne), string.Format("Shipping address one '{0}' shown properly.", addressOne))) result = false;
            if (!Utils.AssertIsTrue(shippingDetails[2].Equals(addressTwo), string.Format("Shipping address two '{0}' shown properly.", addressTwo))) result = false;
            if (!Utils.AssertIsTrue(shippingDetails[3].Contains(postalCode), string.Format("Shipping address postal code '{0}' shown properly.", postalCode))) result = false;

            return result;
        }

        /// <summary>
        /// Validate product details.
        /// </summary>
        /// <param name="productSKU">Product SKU.</param>
        /// <param name="productName">Product name.</param>
        /// <param name="productPrice">Product price.</param>
        /// <param name="productQuantity">Product quantity.</param>
        /// <param name="totalPrice">Total price.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <returns>True if all validations are passed.</returns>
        public bool ValidateProductDetails(string productSKU, string productName, string productPrice, string productQuantity, string totalPrice, int rowNumber = 0)
        {
            bool result = true;

            List<string> intialOrders = this.GetSelectedProductDetails(rowNumber);
            if (!Utils.AssertIsTrue(intialOrders[0].Contains(productSKU), string.Format("Order product SKU '{0}' is proper under order summary page.", productSKU))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[1].Contains(productName), string.Format("Order product name '{0}' is proper under order summary page.", productName))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[2].Contains(productPrice), string.Format("Order product price '{0}' is proper under order summary page.", productPrice))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[3].Trim().Contains(productQuantity), string.Format("Order product quantity '{0}' is proper under order summary page.", productQuantity))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[5].Contains(totalPrice), string.Format("Order product total '{0}' is proper under order summary page.", totalPrice))) result = false;

            return result;
        }

        /// <summary>
        /// Validate product details.
        /// </summary>
        /// <param name="productSKU">Product SKU.</param>
        /// <param name="productName">Product name.</param>
        /// <param name="productPrice">Product price.</param>
        /// <param name="productQuantity">Product quantity.</param>
        /// <param name="totalPrice">Total price.</param>
        /// <param name="rowNumber">Row number.</param>
        /// <returns>True if all validations are passed.</returns>
        public bool ValidateSevenColumnProductDetails(string productSKU, string productName, string productPrice, string productQuantity, string totalPrice, int rowNumber = 0)
        {
            bool result = true;

            // Need to be updated as per update from Gabe.
            List<string> intialOrders = this.GetSelectedProductDetails(rowNumber);
            if (!Utils.AssertIsTrue(intialOrders[0].Contains(productSKU), string.Format("Order product SKU '{0}' is proper under order summary page.", productSKU))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[1].Contains(productName), string.Format("Order product name '{0}' is proper under order summary page.", productName))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[3].Contains(productPrice), string.Format("Order product price '{0}' is proper under order summary page.", productPrice))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[4].Trim().Contains(productQuantity), string.Format("Order product quantity '{0}' is proper under order summary page.", productQuantity))) result = false;
            if (!Utils.AssertIsTrue(intialOrders[6].Contains(totalPrice), string.Format("Order product total '{0}' is proper under order summary page.", totalPrice))) result = false;

            return result;
        }

        /// <summary>
        /// Validate customer order summary, product details and shipping address under GMP Order summary page.
        /// </summary>
        /// <param name="customerName">Customer name.</param>
        /// <param name="accountNumber">Account number.</param>
        /// <param name="securityNumber">Security number.</param>
        /// <param name="orderId">Order id.</param>
        /// <param name="orderStatus">Order status.</param>
        /// <param name="paymanetDues">Payment Dues.</param>
        /// <param name="productSKU">Product SKU.</param>
        /// <param name="productName">Product name.</param>
        /// <param name="productQuantity">Product quantity.</param>
        /// <param name="productPrice">Product price.</param>
        /// <param name="mainAddressOne">Main address one.</param>
        /// <param name="mainAddressTwo">Main address two.</param>
        /// <param name="postalCode">Postal code.</param>    
        /// <returns>True if all validations are passed.</returns>
        public bool ValidateCustomerOrderSummary(string customerName, string accountNumber, string securityNumber, string orderId, string orderStatus, string paymanetDues, string productSKU, string productName, string productQuantity, string productPrice, string mainAddressOne, string mainAddressTwo, string postalCode)
        {
            bool result = true;

            // Validate customer order summary data.
            string orderSummary = this.divOrderSummary.CustomGetText();
            if (!Utils.AssertIsTrue(orderSummary.Contains(customerName), string.Format("Customer name '{0}' is shown properly under yellow widget.", customerName))) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(accountNumber), string.Format("Account ID '{0}' is shown properly under yellow widget.", accountNumber))) result = false;

            if (!Utils.AssertIsTrue(orderSummary.Contains(orderId), string.Format("Order ID '{0}' shown properly under yellow widget.", orderId))) result = false;

            string date = orderSummary.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[4].Trim();
            if (!Utils.AssertIsTrue((orderSummary.Contains(DateTime.UtcNow.AddHours(Util.UtcTimeDiff).ToString("M/d/yyyy"))) ||
               (orderSummary.Contains(DateTime.Now.ToString("M/d/yyyy"))), string.Format("Date of order '{0}' is properly shown under yellow widget.", date))) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(orderStatus), string.Format("Order status '{0}' and shown properly under yellow widget.", orderStatus))) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(paymanetDues), string.Format("Order total '{0}' shown properly under yellow widget.", paymanetDues))) result = false;

            if (!string.IsNullOrEmpty(securityNumber))
            {
                if (!Utils.AssertIsTrue(orderSummary.Contains(securityNumber.Substring(5)), string.Format("Third SSN Field '{0}' is shown properly under yellow widget.", securityNumber.Substring(5)))) result = false;
            }

            // Validate product details from order details page.
            result = result && this.ValidateProductDetails(productSKU, productName, productPrice, productQuantity, productPrice);

            // Validate default shipping address details.
            result = result && this.ValidateShippingAddress(mainAddressOne, mainAddressTwo, postalCode);

            return result;
        }

        /// <summary>
        /// Validate Shipping address under GMP Order Summary page.
        /// </summary>
        /// <param name="addressOne">Address line one.</param>
        /// <param name="addressTwo">Address line two.</param>
        /// <param name="postalCode">Address postal code.</param>
        /// <returns>True if all validations are passed.</returns>
        public bool ValidateShippingAddressUnderGMPOrderSummary(string addressOne, string addressTwo, string postalCode)
        {
            bool result = true;

            List<string> shippingDetails = this.GetShippingAddressDetailsFromOrderSummary();
            if (!Utils.AssertIsTrue(shippingDetails[1].Equals(addressOne), string.Format("Shipping address one '{0}' shown properly.", addressOne))) result = false;
            if (!Utils.AssertIsTrue(shippingDetails[2].Equals(addressTwo), string.Format("Shipping address two '{0}' shown properly.", addressTwo))) result = false;
            if (!Utils.AssertIsTrue(shippingDetails[3].Contains(postalCode), string.Format("Shipping address postal code '{0}' shown properly.", postalCode))) result = false;

            return result;
        }

        /// <summary>
        /// Validate yellow widget details under Order summary page under GMP.
        /// </summary>
        /// <param name="customerName">Customer name.</param>
        /// <param name="accountNumber">Account number.</param>
        /// <param name="securityNumber">SSN/EIN number.</param>
        /// <param name="orderNumber">Order number.</param>
        /// <param name="orderStatus">Order status.</param>
        /// <param name="orderTotal">Order total.</param>
        /// <returns>True if all validations are passed.</returns>
        public bool ValidateYellowWidgetOrderSummaryDetails(string customerName, string accountNumber, string securityNumber, string orderNumber, string orderStatus, string orderTotal)
        {
            bool result = true;

            string orderSummary = this.divOrderSummary.CustomGetText();
            if (!Utils.AssertIsTrue(orderSummary.Contains(customerName), string.Format("Customer name '{0}' is shown properly under yellow widget.", customerName))) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(accountNumber), string.Format("Account number '{0}' is shown properly under yellow widget.", accountNumber))) result = false;

            if (!string.IsNullOrEmpty(securityNumber))
            {
                if (!Utils.AssertIsTrue(orderSummary.Contains(securityNumber), string.Format("SSN/EIN Field '{0}' is shown properly under yellow widget.", securityNumber))) result = false;
            }

            if (!Utils.AssertIsTrue(orderSummary.Contains(orderNumber), string.Format("Order number '{0}' shown properly under yellow widget.", orderNumber))) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(DateTime.UtcNow.AddHours(Util.UtcTimeDiff).ToShortDateString()), string.Format("Date of order '{0}' is shown properly under yellow widget.", DateTime.UtcNow.AddHours(Util.UtcTimeDiff).ToShortDateString()))) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(orderStatus), string.Format("Order status '{0}' is shown properly under yellow widget.", orderStatus))) result = false;
            if (!Utils.AssertIsTrue(orderSummary.Contains(orderTotal), string.Format("Order total '{0}' shown properly under yellow widget.", orderTotal))) result = false;

            return result;
        }

        /// <summary>
        /// Validate Shipping method details.
        /// </summary>
        /// <param name="shippingMethod">Shipping method.</param>
        /// <returns>True if all validations are passed.</returns>
        public bool ValidateShippingMethodDetails(string shippingMethod)
        {
            bool result = true;

            List<string> shippingMethodDetails = this.GetShippingMethodDetails();
            if (!Utils.AssertIsTrue(shippingMethodDetails[1].Contains(shippingMethod), string.Format("Shipping method '{0}' is properly shown under order summary page.", shippingMethod))) result = false;

            return result;
        }

        /// <summary>
        /// Validate Payment details.
        /// </summary>
        /// <param name="productPrice">Product price.</param>
        /// <param name="orderTotal">Order total price.</param>
        /// <returns>True if all validations are passed.</returns>
        public bool ValidatePaymentDetails(string productPrice, string orderTotal)
        {
            bool result = true;

            List<string> paymentDetails = this.GetPaymentDetailsUnderOrderDetails();
            if (!Utils.AssertIsTrue(paymentDetails[5].Contains(productPrice), string.Format("Product price '{0}' is properly shown under Payment table of order details page.", productPrice))) result = false;
            if (!Utils.AssertIsTrue(paymentDetails[9].Contains(orderTotal), string.Format("Product payment amount price '{0}' is properly shown under Payment table of order details page.", orderTotal))) result = false;

            double paymentTotal = Convert.ToDouble(paymentDetails[7].Trim().Remove(0, 1)) + Convert.ToDouble(paymentDetails[8].Trim().Remove(0, 1)) + Convert.ToDouble(productPrice);
            if (!Utils.AssertIsTrue(paymentDetails[9].Contains(paymentTotal.ToString()), string.Format("Product order total price (including taxes) '{0}' is properly shown under Payment table of order details page.", paymentTotal.ToString()))) result = false;

            return result;
        }

        #endregion
    }
}