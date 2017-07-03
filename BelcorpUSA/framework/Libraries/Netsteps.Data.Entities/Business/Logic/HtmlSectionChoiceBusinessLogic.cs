using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class HtmlSectionChoiceBusinessLogic
	{
		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.IHtmlSectionChoiceRepository repository)
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
