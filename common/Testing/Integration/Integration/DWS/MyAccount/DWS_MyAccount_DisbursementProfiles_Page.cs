using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.MyAccount
{
    public class DWS_MyAccount_DisbursementProfiles_Page : DWS_MyAccount_Base_Page
    {
        private Link _save;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            if (Document.GetElement<Link>(new Param("btnSaveDisbursement", AttributeName.ID.ClassName, RegexOptions.None)).Exists)
                _save = Document.GetElement<Link>(new Param("btnSaveDisbursement", AttributeName.ID.ClassName, RegexOptions.None));
            else // custom code for ItWorks
                _save = Document.GetElement<Link>(new Param("Click here to sign up", AttributeName.ID.InnerText));
        }

         public override bool IsPageRendered()
        {
            return _save.Exists;
        }
    }
}
