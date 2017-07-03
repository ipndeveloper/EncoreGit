using WatiN.Core;
using System;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Orders
{
    public class GMP_Orders_AutoshipRunOverview_Page : GMP_Orders_Base_Page
    {
         public override bool IsPageRendered()
        {
            return Document.GetElement<Div>(new Param("Autoship Run Overview", AttributeName.ID.InnerText)).Exists;
        }
    }
}
