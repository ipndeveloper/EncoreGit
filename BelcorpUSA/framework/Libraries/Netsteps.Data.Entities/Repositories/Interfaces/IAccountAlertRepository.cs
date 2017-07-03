
namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IAccountAlertRepository
    {
        void Dismiss(AccountAlert accountAlert);
        AccountAlert LoadByTemplateIDAndAccountID(int templateID, int accountID);
    }
}
