using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Review_Page : PWS_Base_Page
    {
        private Div _id;
        private Link _step5; // ItWorks only

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _id = _content.GetElement<Div>(new Param("FL splitCol", AttributeName.ID.ClassName));
            _step5 = _content.GetElement<Link>(new Param("Step5").And(new Param("current", AttributeName.ID.ClassName)));
        }

        public override bool IsPageRendered()
        {
            return (_id.Exists || _step5.Exists);
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _content.GetElement<Link>(new Param("uxBTContinue").Or(new Param("btnSubmit"))).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public string GetUserID()
        {
            // Not valid for itWorks
            string userName = _id.CustomGetText();
            int start = userName.IndexOf("ID#: ") + 5;
            if (start < 5)
                start = userName.IndexOf("Username: ") + 10;
            return userName.Substring(start).Trim();
        }
    }
}
