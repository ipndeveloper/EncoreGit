using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_Documents_Page : GMP_Sites_WebsiteBase_Page
    {
        private Link _addDocument;
        private Link _manageCategories;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _addDocument = Document.GetElement<Link>(new Param("/Sites/Documents/Edit", AttributeName.ID.Href, RegexOptions.None));
            _manageCategories = Document.GetElement<Link>(new Param("/Sites/Documents/Categories", AttributeName.ID.Href, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _addDocument.Exists;
        }

         public GMP_Sites_DocumentsEdit_Page ClickAddDocument(int? timeout = null, bool pageRequired = true)
        {
            timeout = _addDocument.CustomClick(timeout);
            return Util.GetPage<GMP_Sites_DocumentsEdit_Page>(timeout, pageRequired);
        }

         public GMP_Sites_DocumentCategories_Page ClickManageCategories(int? timeout = null, bool pageRequired = true)
        {
            timeout = _manageCategories.CustomClick(timeout);
            return Util.GetPage<GMP_Sites_DocumentCategories_Page>(timeout, pageRequired);
        }
    }
}
