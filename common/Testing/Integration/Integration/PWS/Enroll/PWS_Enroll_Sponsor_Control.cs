using WatiN.Core;
using NetSteps.Testing.Integration;
using System;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Sponsor_Control : Control<Div>
    {
        private Span _website;
        protected override void InitializeContents()
        {
            base.InitializeContents();
            _website = Element.GetElement<Span>(new Param("html: PwsUrl", "data-bind"));
        }

        public string Website
        {
            get { return _website.CustomGetText(); }
        }

        [Obsolete("Use SelectSponsor<PWS_Enroll_Sponsor_Page>()")]
        public PWS_Enroll_Sponsor_Page SelectSponsor(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>().CustomClick(timeout);
            return Util.GetPage<PWS_Enroll_Sponsor_Page>(timeout, pageRequired);
        }

        public TPage SelectSponsor<TPage>(int? timeout = null, bool pageRequired = true) where TPage: PWS_Base_Page, new()
        {
            timeout = Element.GetElement<Link>().CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
