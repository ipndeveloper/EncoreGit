using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class HtmlSectionContentBusinessLogic
	{
		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IHtmlSectionContentRepository repository)
		{
			return new List<string>()
			{
				"HtmlElementID",
				"HtmlContentID",
				"HtmlSectionContentID",
				"HtmlSectionID"
			};
		}
	}
}
