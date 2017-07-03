using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_Pages_Page : GMP_Sites_WebsiteBase_Page
    {
        private Link _addPage;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _addPage = Document.GetElement<Link>(new Param("/Sites/Pages/Edit", AttributeName.ID.Href, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _addPage.Exists;
        }

         public GMP_Sites_PagesEdit_Page AddPage(int? timeout = null, bool pageRequired = true)
        {
            timeout = _addPage.CustomClick(timeout);
            return Util.GetPage<GMP_Sites_PagesEdit_Page>(timeout, pageRequired);
        }
    }
}
