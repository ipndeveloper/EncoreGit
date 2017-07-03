using WatiN.Core;
using NetSteps.Testing.Integration.DWS;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_Page : GMP_Sites_Base_Page
    {
        private GMP_Sites_SiteNavigation_Control _cws;
        private GMP_Sites_SiteNavigation_Control _dws;
        private GMP_Sites_SiteNavigation_Control _pws;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _cws = Document.GetElement<Div>(new Param("sitesLandingMenu", AttributeName.ID.ClassName, RegexOptions.None)).As<GMP_Sites_SiteNavigation_Control>();
            _dws = Document.GetElement<Div>(new Param("sitesLandingMenu", AttributeName.ID.ClassName, RegexOptions.None).And(new Param(1))).As<GMP_Sites_SiteNavigation_Control>();
            _pws = Document.GetElement<Div>(new Param("sitesLandingMenu", AttributeName.ID.ClassName, RegexOptions.None).And(new Param(2))).As<GMP_Sites_SiteNavigation_Control>();
        }

         public override bool IsPageRendered()
        {
            return Document.GetElement<Link>(new Param("/Sites/Overview/Index/", AttributeName.ID.Href, RegexOptions.None)).Exists;
        }

        public GMP_Sites_SiteNavigation_Control DWSSiteNavigation
        {
            get { return _dws; }
        }

        public GMP_Sites_SiteNavigation_Control CWSSiteNavigation
        {
            get { return _cws; }
        }

        public GMP_Sites_SiteNavigation_Control PWSSiteNavigation
        {
            get { return _pws; }
        }
    }
}
