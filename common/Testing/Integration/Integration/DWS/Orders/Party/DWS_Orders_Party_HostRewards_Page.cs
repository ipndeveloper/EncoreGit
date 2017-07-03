using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_HostRewards_Page : DWS_Base_Page
    {
        private Link _continue;
        private Table _guests;
        private DWS_Orders_Party_Summary_Control _summary;
        private DWS_Orders_CartTools_Control _hostGift, _hostCredit, _hostDiscount;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
             _continue = _content.GetElement<Link>(new Param("btnNext"));
             _guests = _content.GetElement<Table>(new Param("DataGrid bold OverviewTable invitedGuestList", AttributeName.ID.ClassName));
             _summary = _content.GetElement<Div>(new Param("SideModule")).As<DWS_Orders_Party_Summary_Control>();
             _hostGift = _content.GetElement<Div>(new Param("rewardsHostessExclusiveProducts ", AttributeName.ID.ClassName, RegexOptions.None)).As<DWS_Orders_CartTools_Control>();
             _hostCredit = _content.GetElement<Div>(new Param("rewardsHostCredit", AttributeName.ID.ClassName, RegexOptions.None)).As<DWS_Orders_CartTools_Control>();
             _hostDiscount = _content.GetElement<Div>(new Param("rewardsDiscounts", AttributeName.ID.ClassName, RegexOptions.None)).As<DWS_Orders_CartTools_Control>();
        }

        public DWS_Orders_CartTools_Control HostGift
        {
            get { return _hostGift; }
        }

        public DWS_Orders_CartTools_Control HostCredit
        {
            get { return _hostCredit; }
        }

        public DWS_Orders_CartTools_Control HostDiscount
        {
            get { return _hostDiscount; }
        }

        public DWS_Orders_Party_Summary_Control Summary
        {
            get { return _summary; }
        }

         public override bool IsPageRendered()
        {
            return _continue.Exists;
        }

         public TPage ClickContinue<TPage>(int? timeout = null, bool pageRequired = true) where TPage : DWS_Base_Page, new()
        {
            timeout = _continue.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
