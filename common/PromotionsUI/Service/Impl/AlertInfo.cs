using NetSteps.Promotions.UI.Service.Impl;
using NetSteps.Promotions.UI.Common.Interfaces;

namespace NetSteps.Promotions.UI.Service.Impl
{
    public class AlertInfo : DisplayInfo, IAlertInfo
    {
        public AlertInfo()
        {
            PartialName = "";
            PartialTitle = "";
        }

        public int AccountAlertId { get; set; }
        public string PartialName { get; set; }
        public string PartialTitle { get; set; }
    }
}