using WatiN.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_Generic_Page : DWS_Base_Page
    {
        private DWS_Orders_Party_Summary_Control _summary;

        protected override void InitializeContents()
        {
            base.InitializeContents();
           _summary = Document.GetElement<Div>(new Param("SideModule")).As<DWS_Orders_Party_Summary_Control>();
        }

        public override bool IsPageRendered()
        {
            return _summary.Exists;
        }

        public DWS_Orders_Party_Summary_Control Summary
        {
            get { return _summary; }
        }
    }
}
