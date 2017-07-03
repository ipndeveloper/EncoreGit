using NetSteps.Data.Entities.Repositories;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
    public partial class HtmlSectionContent
    {
        #region Methods

        public static HtmlSectionContent LoadByHtmlSectionIdAndHtmlContentId(int htmlSectionId, int htmlContentId)
        {
            var repository = Create.New<IHtmlSectionContentRepository>();
            return repository.LoadByHtmlSectionIdAndHtmlContentId(htmlSectionId, htmlContentId);
        }

        #endregion
    }
}
