using System;
using System.Collections.Generic;
using System.Threading;
using WatiN.Core;
using WatiN.Core.Extras;

namespace NetSteps.Testing.Integration.DWS
{
    /// <summary>
    /// Control and Ops that are common to all clients of DWS Home page.
    /// </summary>
    public class DWS_Home_Page : DWS_Base_Page
    {

         public override bool IsPageRendered()
        {
            Thread.Sleep(2000);
             return _content.GetElement<Div>(new Param("BreakingNews")).Exists;
        }
    }
}