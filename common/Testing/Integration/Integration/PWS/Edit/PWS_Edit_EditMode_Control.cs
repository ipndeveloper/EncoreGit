using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration.PWS.Edit
{
    public class PWS_Edit_EditMode_Control : Control<UnorderedList>
    {
        private Link _headContent;
        private Link _liveMode;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            Thread.Sleep(1000);
            _headContent = Element.GetElement<ListItem>(new Param(0)).GetElement<Link>();
            _liveMode = Element.GetElement<ListItem>(new Param(2)).GetElement<Link>();
        }

        public bool IsControlRendered
        {
            get { return _headContent.Exists && _liveMode.Exists; }
        }

        public TPage ClickSwitchToLiveMode<TPage>(int? timeout = null, bool pageRequired = true) where TPage : PWS_Base_Page, new()
        {
            timeout = _liveMode.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
