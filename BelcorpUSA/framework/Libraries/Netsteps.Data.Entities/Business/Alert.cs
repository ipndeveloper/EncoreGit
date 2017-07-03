
namespace NetSteps.Data.Entities.Business
{
    public class Alert
    {
        public int AccountAlertID { get; set; }
        public int AlertTemplateID { get; set; }
        public string Message { get; set; }
        public Constants.AlertPriority Priority { get; set; }
        public string ActionLinkUrl { get; set; }
        public bool OpenActionLinkInNewWindow { get; set; }
        public bool CanBeDismissed { get; set; }
    }
}
