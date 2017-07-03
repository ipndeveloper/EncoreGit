using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_News_Page : GMP_Sites_WebsiteBase_Page
    {
        private Link _addNews;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _addNews = Document.GetElement<Link>(new Param("/Sites/News/Edit", AttributeName.ID.Href, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _addNews.Exists;
        }

         public GMP_Sites_NewsEdit_Page AddNews(int? timeout = null, bool pageRequired = true)
        {
            timeout = _addNews.CustomClick(timeout);
            return Util.GetPage<GMP_Sites_NewsEdit_Page>(timeout, pageRequired);
        }
    }
}
