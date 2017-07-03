using WatiN.Core;
using System;
using System.Threading;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Upgrade_Kits_Page : PWS_Base_Page
    {
        private ElementCollection<Div> _kits;
        private Link _next;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            Thread.Sleep(1000);
            _kits = _content.GetElements<Div>(new Param("FL check", AttributeName.ID.ClassName));
            _next = _content.GetElement<Link>(new Param("btnSubmit"));
        }
        public override bool IsPageRendered()
        {
            if (_kits.Count > 1)
                return true;
            else
                return false;
        }

        //public PWS_Upgrade_Kits_Page SelectKit(int index)
        //{
        //    _kits[index].CustomSetCheckBox(true);
        //    return this;
        //}

        //public PWS_Upgrade_Kits_Page SelectAllKits()
        //{
        //    foreach (CheckBox kit in _kits)
        //        kit.CustomSetCheckBox(true);
        //    return this;
        //}

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

    }
}
