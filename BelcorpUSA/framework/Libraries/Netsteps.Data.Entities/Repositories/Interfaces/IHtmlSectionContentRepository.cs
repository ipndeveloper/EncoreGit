namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IHtmlSectionContentRepository
    {
        HtmlSectionContent LoadByHtmlSectionIdAndHtmlContentId(int htmlSectionId, int htmlContentId);
    }
}
