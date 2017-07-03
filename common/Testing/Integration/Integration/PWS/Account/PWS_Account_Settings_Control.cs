using System;
using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Account
{
    public class PWS_Account_Settings_Control : Control<Div>
    {
        public void ClickCancel()
        {
            Element.GetElement<Link>(new Param("jqmClose", AttributeName.ID.ClassName)).CustomClick();
        }
    }
}
