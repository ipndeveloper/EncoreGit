using System.Linq;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class HtmlSectionContentRepository
    {
        public HtmlSectionContent LoadByHtmlSectionIdAndHtmlContentId(int htmlSectionId, int htmlContentId)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (NetStepsEntities context = CreateContext())
                {
                    var htmlSectionContent = (from s in context.HtmlSectionContents
                                               where s.HtmlSectionID == htmlSectionId
                                                  && s.HtmlContentID == htmlContentId
                                               select s).FirstOrDefault();
                    return htmlSectionContent;
                }
            });
        }
    }
}
