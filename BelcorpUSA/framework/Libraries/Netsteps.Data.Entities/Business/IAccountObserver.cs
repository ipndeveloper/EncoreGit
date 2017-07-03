
namespace NetSteps.Data.Entities.Business
{
	public interface IAccountObserver
	{
		void AccountActivating( Account acct );
		void AccountUpdating( Account acct );
	}
}
