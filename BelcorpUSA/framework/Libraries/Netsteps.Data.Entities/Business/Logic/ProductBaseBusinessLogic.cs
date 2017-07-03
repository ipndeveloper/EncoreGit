using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class ProductBaseBusinessLogic
	{
		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IProductBaseRepository repository)
		{
			return new List<string>() { "ProductBaseID", "ProductID" };
		}
	}
}
