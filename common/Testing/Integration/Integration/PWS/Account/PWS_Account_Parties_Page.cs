using System.Text.RegularExpressions;
using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Account
{
    public class PWS_Account_Parties_Page : PWS_Base_Page
    {
        protected override void InitializeContents()
        {
            base.InitializeContents();
        }

        public override bool IsPageRendered()
        {
            return _content.GetElement<Table>(new Param("partiesList", AttributeName.ID.ClassName, RegexOptions.None)).Exists;
        }
    }
}
