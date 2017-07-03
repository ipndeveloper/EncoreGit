
namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IAccountAlertBusinessLogic
    {
        void Dismiss(Repositories.IAccountAlertRepository repository, AccountAlert accountAlertID);
        AccountAlert LoadByTemplateIDAndAccountID(Repositories.IAccountAlertRepository repository, int templateID, int accountID);
    }
}
