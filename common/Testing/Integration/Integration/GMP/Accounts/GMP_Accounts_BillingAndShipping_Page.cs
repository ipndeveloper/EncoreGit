using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_BillingAndShipping_Page : GMP_Accounts_Section_Page
    {
        #region Controls.

        private Link lnkAddShippingAddress;
        private TextField txtShippingProfileName;
        private TextField txtShippingAddressLineOne;
        private TextField txtShippingAddressLineTwo;
        private TextField txtShippingAddressPostalCode;
        private Link lnkSaveShippingAddress;
        private TextField txtNameOnCard;
        private TextField txtCreditCardNumber;
        private SelectList selectCCExpiryMonth;
        private SelectList selectCCExpiryYear;
        private SelectList selectDefaultBilling;
        private TextField txtBillingProfileName;
        private TextField txtBillingAddressLineOne;
        private TextField txtBillingAddressLineTwo;
        private TextField txtBillingAddressPostalCode;
        private Link lnkBillingSavePaymentMethod;
        private Link lnkAddBillingDetails;
        private Link lnkAddShippingDetails;
        private DivCollection billings;
        private DivCollection shippings;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            this.lnkAddShippingAddress = Document.GetElement<Link>(new Param("Account", AttributeName.ID.InnerText));
            this.lnkSaveShippingAddress = Document.GetElement<Link>(new Param("btnSaveAddress"));
            this.txtShippingProfileName = Document.GetElement<TextField>(new Param("shippingAddressProfileName"));
            this.txtShippingAddressLineOne = Document.GetElement<TextField>(new Param("shippingAddressAddress1"));
            this.txtShippingAddressLineTwo = Document.GetElement<TextField>(new Param("shippingAddressAddress2"));
            this.txtShippingAddressPostalCode = Document.GetElement<TextField>(new Param("shippingAddressZip"));
            this.txtNameOnCard = Document.GetElement<TextField>(new Param("nameOnCard"));
            this.txtCreditCardNumber = Document.GetElement<TextField>(new Param("accountNumber"));
            this.selectCCExpiryMonth = Document.GetElement<SelectList>(new Param("expMonth"));
            this.selectCCExpiryYear = Document.GetElement<SelectList>(new Param("expYear"));
            this.selectDefaultBilling = Document.GetElement<SelectList>(new Param("existingAddress"));
            this.txtBillingProfileName = Document.GetElement<TextField>(new Param("paymentMethodAddressProfileName"));
            this.txtBillingAddressLineOne = Document.GetElement<TextField>(new Param("paymentMethodAddressAddress1"));
            this.txtBillingAddressLineTwo = Document.GetElement<TextField>(new Param("paymentMethodAddressAddress2"));
            this.txtBillingAddressPostalCode = Document.GetElement<TextField>(new Param("paymentMethodAddressZip"));
            this.lnkBillingSavePaymentMethod = Document.GetElement<Link>(new Param("btnSavePaymentMethod"));
            this.lnkAddBillingDetails = Document.GetElement<Link>(new Param("btnAddBillingAddress"));
            this.lnkAddShippingDetails = Document.GetElement<Link>(new Param("btnAddShippingAddress"));
            this.billings = Document.GetElement<Div>(new Param("paymentMethods")).Divs.Filter(Find.ByClass("Profile", false));
            this.shippings = Document.GetElement<Div>(new Param("addresses")).Divs.Filter(Find.ByClass("Profile", false));
        }

         public override bool IsPageRendered()
        {
            return Title.Contains("Billing/Shipping Profiles");
        }

        //public GMP_Accounts_Profile_Control GetBilling(int index)
        //{
        //    return billings[index].As<GMP_Accounts_Profile_Control>();
        //}

        //public GMP_Accounts_Profile_Control GetShipping(int index)
        //{
        //    return shippings[index].As<GMP_Accounts_Profile_Control>();
        //}

        #endregion

        #region Methods

        /// <summary>
        /// Set Billing address details and save billing profile.
        /// </summary>
        /// <param name="nameOnCard">Name on the credit card.</param>
        /// <param name="CCNumber">Credit card number.</param>
        /// <param name="expiration">Credit card expiration</param>
        /// <param name="profileName">Billing profile name.</param>
        /// <param name="addresslineOne">Address line one.</param>
        /// <param name="addressLineTwo">Address line two.</param>
        /// <param name="postalCode">Address postal code.</param>
         public void EnterBillingDetailsAndSaveProfile(string nameOnCard, string CCNumber, DateTime expiration, string profileName, string addresslineOne, string addressLineTwo, string postalCode, int? timeout = null)
        {
            // Click Add billing details.
            timeout = this.lnkAddBillingDetails.CustomClick(timeout);

            // Set Billing fields.
            this.txtNameOnCard.CustomSetTextHelper(nameOnCard);
            this.txtCreditCardNumber.CustomSetTextHelper(CCNumber);
            this.selectCCExpiryMonth.CustomSelectDropdownItem(expiration.Month);
            this.selectCCExpiryYear.CustomSelectDropdownItem(expiration.Year.ToString());
            this.txtBillingProfileName.CustomSetTextHelper(profileName);
            this.txtBillingAddressLineOne.CustomSetTextHelper(addresslineOne);
            this.txtBillingAddressLineTwo.CustomSetTextHelper(addressLineTwo);
            this.txtBillingAddressPostalCode.CustomSetTextHelper(postalCode);
            this.txtBillingAddressPostalCode.CustomRunScript(Util.strKeyUp);

            // Click Save billing details.
            this.lnkBillingSavePaymentMethod.CustomClick(timeout);
        }

        /// <summary>
        /// Set Shipping address details and save shipping profile.
        /// </summary>             
        /// <param name="profileName">Shipping profile name.</param>
        /// <param name="addresslineOne">Shipping address line one.</param>
        /// <param name="addressLineTwo">Shipping address line two.</param>
        /// <param name="postalCode">Shipping address postal code.</param>
         public void EnterShippingDetailsAndSaveProfile(string profileName, string addresslineOne, string addressLineTwo, string postalCode, int? timeout = null)
        {
            // Click Add shipping details.
            timeout = this.lnkAddShippingDetails.CustomClick(timeout);

            // Set Shipping fields.
            this.txtShippingProfileName.CustomSetTextHelper(profileName);
            this.txtShippingAddressLineOne.CustomSetTextHelper(addresslineOne);
            this.txtShippingAddressLineTwo.CustomSetTextHelper(addressLineTwo);
            this.txtShippingAddressPostalCode.CustomSetTextHelper(postalCode);
            this.txtShippingAddressPostalCode.CustomRunScript(Util.strKeyUp);

            // Click Save shipping details.
            this.lnkSaveShippingAddress.CustomClick(timeout);
        }

        #endregion

        #region Validation Methods.

        public bool ValidateBillingProfile(int index, BillingProfile profile)
        {
            bool valid = true;

            if (index < 0 && billings.Count != 0) //"No Billing section is available."
                valid = false;
            if (index >= 0 && billings.Count - 1 < index) //"Billing section is not available which should be."
                valid = false;
            if(valid)
            {
                var billing = billings[index];
                if (!Compare.CustomCompare<string>(billing.GetElement<Div>(new Param("PaymentMethod", AttributeName.ID.Id, RegexOptions.None)).CustomGetText(), CompareID.Contains, profile.Account.Substring(profile.Account.Length - 4, 4), "Account Number"))
                    valid = false;
                if (!Compare.CustomCompare<string>(billing.GetElement<Div>(new Param("PaymentMethod", AttributeName.ID.Id, RegexOptions.None)).CustomGetText(), CompareID.Contains, String.Format("{0}/{1}", profile.Expiration.Month, profile.Expiration.Year), "Expiration"))
                    valid = false;
            }
            return valid;
        }

        public bool ValidateShippingProfile(int index, Address shipping)
        {
            bool valid = true;
            if (index < 0 && shippings.Count != 0)
               valid = false;
            if (index >= 0 && shippings.Count - 1 < index)
                valid = false;
            if(valid)
            {

                string shippingText = shippings[index].CustomGetText();

                if (shipping.Attention != null)
                    if (!Compare.CustomCompare<string>(shippingText, CompareID.Contains, shipping.Attention, "Attention"))
                        valid = false;
                if (!Compare.CustomCompare<string>(shippingText, CompareID.Contains, shipping.Address1, "Address1"))
                    valid = false;
                if (shipping.Address2 != null)
                    if (!Compare.CustomCompare<string>(shippingText, CompareID.Contains, shipping.Address2, "Address2"))
                        valid = false;
                if (shipping.Address3 != null)
                    if (!Compare.CustomCompare<string>(shippingText, CompareID.Contains, shipping.Address3,"Address3"))
                        valid = false;
                if (!Compare.CustomCompare<string>(shippingText, CompareID.Contains, shipping.City, "City"))
                        valid = false;
                if (!Compare.CustomCompare<string>(shippingText, CompareID.Contains, shipping.State, "State"))
                        valid = false;
                if (!Compare.CustomCompare<string>(shippingText, CompareID.Contains, shipping.PostalCode, "Postal Code"))
                        valid = false;
                if (shipping.PostalCodeExtension != null)
                    if (!Compare.CustomCompare<string>(shippingText, CompareID.Contains, shipping.PostalCodeExtension, "Postal Code Extension"))
                        valid = false;
            }
            return valid;
        }

        #endregion
    }
}
