namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: AutoshipOrder Extensions
    /// Created: 03-04-2011
    /// </summary>
    public static class AutoshipOrderExtensions
    {
        public static void UpdateAutoshipAccount(this AutoshipOrder autoshipOrder, Account account)
        {
            if (autoshipOrder != null)
            {
                autoshipOrder.StartEntityTracking();
                autoshipOrder.Account = account;
                autoshipOrder.AccountID = account.AccountID;

                autoshipOrder.StopEntityTracking();
                account.AutoshipOrders.Clear();
                autoshipOrder.StartEntityTracking();

                autoshipOrder.Order.UpdateOrderCustomerAccount(account);
            }
        }
    }
}
