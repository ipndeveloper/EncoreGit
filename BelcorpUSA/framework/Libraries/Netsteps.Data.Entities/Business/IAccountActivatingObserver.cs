
namespace NetSteps.Data.Entities.Business
{
	public interface IAccountActivatingObserver
	{
		void OnActivatingAccount( Account acct );
	}
}
