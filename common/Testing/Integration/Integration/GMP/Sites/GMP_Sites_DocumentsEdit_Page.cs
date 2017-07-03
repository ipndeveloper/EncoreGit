using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_DocumentsEdit_Page : GMP_Sites_WebsiteBase_Page
    {
        private TextField _thumbnail;
        private Link _manageCategories;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _thumbnail = Document.GetElement<TextField>(new Param("thumbnail"));
            _manageCategories = Document.GetElement<Link>(new Param("/Sites/Documents/Categories", AttributeName.ID.Href, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _thumbnail.Exists;
        }

         public GMP_Sites_DocumentCategories_Page ClickManageCategories(int? timeout = null, bool pageRequired = true)
        {
            timeout = _manageCategories.CustomClick(timeout);
            return Util.GetPage<GMP_Sites_DocumentCategories_Page>(timeout, pageRequired);
        }
    }
}
