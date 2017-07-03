using System.Collections.Generic;

namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class NewsBusinessLogic
    {

        public override List<string> ValidatedChildPropertiesSetByParent(Repositories.INewsRepository repository)
        {
            return new List<string>() { "HtmlSectionID", "HtmlSectionContentID", "HtmlSectionChoiceID", "HtmlContentID" };
        }

    }
}
