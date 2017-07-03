using WatiN.Core;
using WatiN.Core.Extras;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    /// <summary>
    /// Class related to Controls and methods of PWS Cart page.
    /// </summary>
    public class PWS_Shop_Cart_Page : PWS_Base_Page
    {
        #region Controls.

        private TextField _txtProdQuantity;
        private Link _lnkCheckoutNow, _continueShopping;
        private HeaderLevel1 _hOneCartPagetitle;


        protected override void InitializeContents()
        {
            base.InitializeContents();
            this._txtProdQuantity = Document.GetElement<TextField>(new Param("TextInput quantity", AttributeName.ID.ClassName));
            this._lnkCheckoutNow = Document.GetElement<Link>(new Param("Button btnContinue btnStartCheckOut", AttributeName.ID.ClassName));
            this._hOneCartPagetitle = Document.GetElement<HeaderLevel1>(new Param("Your Cart", AttributeName.ID.InnerText));
            _continueShopping = _content.GetElement<Link>(new Param("ContinueShopping", AttributeName.ID.ClassName, RegexOptions.None));
        }

        #endregion Controls.

        #region Methods

        public override bool IsPageRendered()
        {
            _lnkCheckoutNow.CustomWaitForVisibility();
            return true;
        }

        public PWS_Shop_CartProduct_Control GetProduct(int? index = null)
        {
            Thread.Sleep(2000);
            TableRowCollection rows = Document.TableBody("CartItems").OwnTableRows;
            if (!index.HasValue)
            {
                index = Util.GetRandom(0, rows.Count - 1);
            }
            return rows[(int)index].As<PWS_Shop_CartProduct_Control>();
        }

        public TPage ClickCheckoutNow<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = this._lnkCheckoutNow.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public PWS_Shop_Page ClickContinueShopping(int? timeout = null)
        {
            timeout = _continueShopping.CustomClick(timeout);
            return Util.GetPage<PWS_Shop_Page>(timeout);
        }

        #endregion
    }
}