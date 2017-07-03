
namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IAccountPaymentMethodRepository
	{
        bool IsUsedByAnyActiveOrderTemplates(int accountPaymentMethodID);
	}
}
