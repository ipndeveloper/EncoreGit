using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
	public partial class CalendarEventBusinessLogic
	{
		public override List<string> ValidatedChildPropertiesSetByParent(Repositories.ICalendarEventRepository repository)
		{
			return new List<string>() { "HtmlSectionID", "HtmlContentID" };
		}
	}
}
