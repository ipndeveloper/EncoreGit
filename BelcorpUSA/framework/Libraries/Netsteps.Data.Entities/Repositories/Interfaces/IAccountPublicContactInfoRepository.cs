
namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IAccountPublicContactInfoRepository
	{
		AccountPublicContactInfo LoadByAccountID(int accountID);
	}
}
