
namespace NetSteps.Data.Entities
{
    public partial class AccountAlert
    {
        public static void Dismiss(AccountAlert accountAlert)
        {
            BusinessLogic.Dismiss(Repository, accountAlert);
        }
        public static AccountAlert LoadByTemplateIDAndAccountID(int templateID, int accountID)
        {
            return BusinessLogic.LoadByTemplateIDAndAccountID(Repository, templateID, accountID);
        }
    }
}
