namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
	public partial interface IAccountTypeBusinessLogic
	{
		bool CanSendEnrollmentCompletedEmail(int accountTypeID);
	}
}
