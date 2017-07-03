
namespace nsDistributor.Models.Account
{
    public class AccountIndexViewModel
    {
        public bool HasAutoshipOrders { get; set; }
        public bool ShowUpgrade { get; set; }
        public string UpgradeUrl { get; set; }
        public string AccountType { get; set; }
		public bool DisplayUsernameField { get; set; }

        public virtual AccountIndexViewModel LoadResources(
            bool hasAutoshipOrders,
            bool showUpgrade,
            string upgradeUrl,
            string accountType,
			bool displayUsernameField)
        {
            HasAutoshipOrders = hasAutoshipOrders;
            ShowUpgrade = showUpgrade;
            UpgradeUrl = upgradeUrl;
            AccountType = accountType;
			DisplayUsernameField = displayUsernameField;
            return this;
        }
    }
}