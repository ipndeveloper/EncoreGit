using System;
using System.Text.RegularExpressions;
using WatiN.Core;
using NetSteps.Testing.Integration;
using NetSteps.Testing.Integration.DWS;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_SubNav_Control : Control<UnorderedList>
    {
        private GMP_Sites_GlobalSiteSubNav_Control _globalSite;
        private GMP_Sites_DistributorWorkstationSubNav_Control _distributorWorkstation;
        private GMP_Sites_PersonalWorkstationSubNav_Control _personalWorkstation;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _globalSite = Element.GetElement<Div>(new Param("DropDown", AttributeName.ID.ClassName).And(new Param(0))).As<GMP_Sites_GlobalSiteSubNav_Control>();
            _distributorWorkstation = Element.GetElement<Div>(new Param("DropDown", AttributeName.ID.ClassName).And(new Param(1))).As<GMP_Sites_DistributorWorkstationSubNav_Control>();
            _personalWorkstation = Element.GetElement<Div>(new Param("DropDown", AttributeName.ID.ClassName).And(new Param(2))).As<GMP_Sites_PersonalWorkstationSubNav_Control>();
        }

        public GMP_Sites_GlobalSiteSubNav_Control GlobalSite
        {
            get { return _globalSite; }
        }

        public GMP_Sites_DistributorWorkstationSubNav_Control DistributorWorkstation
        {
            get { return _distributorWorkstation; }
        }

        public GMP_Sites_PersonalWorkstationSubNav_Control PersonalWorkstation
        {
            get { return _personalWorkstation; }
        }

        public GMP_Sites_TermTranslation_Page ClickTermTranslation(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites/TermTranslations", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_TermTranslation_Page>(timeout, pageRequired);
        }

        public GMP_Sites_Copy_Page ClickCopySite(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Sites/Copy", AttributeName.ID.Href, RegexOptions.IgnoreCase)).CustomClick(timeout);
            return Util.GetPage<GMP_Sites_Copy_Page>(timeout, pageRequired);
        }
    }
}
