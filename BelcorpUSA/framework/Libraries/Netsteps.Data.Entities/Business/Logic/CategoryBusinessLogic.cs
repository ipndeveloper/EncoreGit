using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class CategoryBusinessLogic
	{
		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.ICategoryRepository repository)
		{
			List<string> list = new List<string>() { "CategoryID", "HtmlContentID" };
			return list;
		}
	}
}
