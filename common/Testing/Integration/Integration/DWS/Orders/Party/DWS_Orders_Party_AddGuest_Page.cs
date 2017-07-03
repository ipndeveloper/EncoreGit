using WatiN.Core;
using System;
using System.Text.RegularExpressions;
using NetSteps.Testing.Integration;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{    
    public class DWS_Orders_Party_AddGuest_Page : DWS_Base_Page
    {
        private Link _addGuest;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _addGuest = Document.GetElement<Link>(new Param("btnAddGuest"));
        }

        public override bool IsPageRendered()
        {
            return _addGuest.Exists;
        }

        private ElementCollection<Div> GetGuestsDivs()
        {
            return Document.GetElements<Div>(new Param("GuestForm", AttributeName.ID.ClassName, RegexOptions.None));
        }

        public DWS_Orders_Party_Guest_Control GetGuest(int index, int timeout = 10)
        {
            return GetGuestsDivs()[index].As<DWS_Orders_Party_Guest_Control>();
        }

        public DWS_Orders_Party_Guest_Control ClickAddGuest(int? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = Settings.WaitUntilExistsTimeOut;
            int index = GetGuestsDivs().Count;
            timeout = _addGuest.CustomClick(timeout);
            ElementCollection<Div> guests;
            do
            {
                guests = GetGuestsDivs();
                if (guests.Count == index + 1)
                    break;
                Thread.Sleep(1000);
            } while (--timeout >= 0);
            if (timeout < 0)
                throw new TimeoutException();
            return guests[index].As<DWS_Orders_Party_Guest_Control>();
        }

        public DWS_Orders_Party_Cart_Page ClickSave(int? timeout = null, bool pageRequired = true)
        {
            Link lnk = Document.GetElement<Link>(new Param("btnSave"));
            timeout = lnk.CustomClick(timeout);
            DWS_Orders_Party_Cart_Page page = Util.GetPage<DWS_Orders_Party_Cart_Page>(timeout, pageRequired);
            page.Document.CustomWaitForSpinners();
            return page;
        }
    }
}
