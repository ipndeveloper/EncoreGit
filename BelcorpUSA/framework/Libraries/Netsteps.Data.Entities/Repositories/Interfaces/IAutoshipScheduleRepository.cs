using System.Collections.Generic;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IAutoshipScheduleRepository
	{
		List<AutoshipSchedule> LoadFullByAccountTypeID(int accountTypeID);
		List<AutoshipSchedule> LoadByAccountTypeID(int accountTypeID);
	}
}
