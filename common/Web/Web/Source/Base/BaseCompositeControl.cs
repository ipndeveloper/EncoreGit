using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetSteps.Objects.Business;

namespace NetSteps.Web.Base
{
    public class BaseCompositeControl : CompositeControl
    {
        protected override void Render(HtmlTextWriter writer)
        {
            this.RenderContents(writer);
        }

        #region Parent Properties
        protected NetSteps.Common.Constants.ViewingMode PageMode
        {
            get
            {
                return ((BaseMasterPage)Page.Master).PageMode;
            }
        }
        public NetSteps.Objects.Business.Site CurrentSite
        {
            get
            {
                return ((BasePage)Page).CurrentSite;
            }
        }

        public SitePage CurrentPage
        {
            get
            {
                return ((BasePage)Page).CurrentPage;
            }
        }

        public void FillContent(ContentControl control, HtmlSection section)
        {
            ((BasePage)Page).FillContent(control, section);
        }

        public Account CurrentAccount
        {
            get
            {
                return ((BasePage)Page).CurrentAccount;
            }
        }

        public ShoppingCart CurrentCart
        {
            get
            {
                return ((BasePage)Page).CurrentCart;
            }
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                return ((BasePage)Page).CurrentCulture;
            }
        }
        #endregion
    }
}
