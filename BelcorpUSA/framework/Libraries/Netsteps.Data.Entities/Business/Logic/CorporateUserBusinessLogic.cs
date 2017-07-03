using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class CorporateUserBusinessLogic
	{
		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.ICorporateUserRepository repository)
		{
			return new List<string>() { "UserID" };
		}
	}
}
