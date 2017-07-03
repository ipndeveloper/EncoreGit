
namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class AccountAlertBusinessLogic
    {
        public virtual void Dismiss(Repositories.IAccountAlertRepository repository, AccountAlert alert)
        {
            repository.Dismiss(alert);
        }
        public AccountAlert LoadByTemplateIDAndAccountID(Repositories.IAccountAlertRepository repository, int templateID, int accountID)
        {
            return repository.LoadByTemplateIDAndAccountID(templateID, accountID);
        }

    }
}
