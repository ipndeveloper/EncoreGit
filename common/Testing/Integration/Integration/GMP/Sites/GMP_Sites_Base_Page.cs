using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public abstract class GMP_Sites_Base_Page : GMP_Base_Page
    {
        private Div _breadcrumb;
        private Div _sitesSectionHeader;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _breadcrumb = Document.GetElement<Div>(new Param("BreadCrumb", AttributeName.ID.ClassName));
            _sitesSectionHeader = Document.GetElement<Div>(new Param("SectionHeader", AttributeName.ID.ClassName));
        }

        public GMP_Sites_SubNav_Control SubNav
        {
            get { return _subNav.As<GMP_Sites_SubNav_Control>(); }
        }

        public string Breadcrumb
        {
            get { return _breadcrumb.CustomGetText(); }
        }

        public string SectionHeader
        {
            get { return _sitesSectionHeader.CustomGetText(); }
        }
    }
}
