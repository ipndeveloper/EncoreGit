using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Edit
{
    /// <summary>
    /// Control based upon common parent of 
    /// </summary>
    public class DWS_Edit_EditMode_Control : Control<UnorderedList>
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

        public DWS_Edit_Choices_Page ClickHeadContent(int? timeout = null, bool pageRequired = true)
        {
            timeout = this._liveMode.CustomClick(timeout);
            return Util.GetPage<DWS_Edit_Choices_Page>(timeout, pageRequired);
        }

        public TPage ClickSwitchToLiveMode<TPage>(int? timeout = null, bool pageRequired = true) where TPage : DWS_Base_Page, new()
        {
            timeout = _liveMode.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
