using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using WatiN.Core;
using WatiN.Core.Extras;
using ListItems = WatiN.Core.ListItem;
using System.Threading;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    public class PWS_Shop_ConfirmOrder_Page : PWS_Base_Page
    {
        private Link _submit;
        protected override void InitializeContents()
        {
            base.InitializeContents();
            _submit = _content.GetElement<Link>(new Param("btnSubmitOrder", AttributeName.ID.ClassName, RegexOptions.None));
            _submit.CustomWaitForExist();
        }

        public override bool IsPageRendered()
        {
            return _submit.Exists;
        }

        public PWS_Shop_Receipt_Page ClickSubmitOrder(int? timeout = null, bool pageRequired = true)
        {
            timeout = _submit.CustomClick(timeout);
            return Util.GetPage<PWS_Shop_Receipt_Page>(timeout, pageRequired);
        }
    }
}