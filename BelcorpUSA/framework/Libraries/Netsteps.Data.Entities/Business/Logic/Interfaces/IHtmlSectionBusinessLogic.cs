
namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IHtmlSectionBusinessLogic
    {
        HtmlSection CloneHtmlSection(int htmlSectionIdToClone, int? siteID = null);
        HtmlSection CloneHtmlSection(HtmlSection htmlSectionToClone, int? existingSiteID = null, int? siteID = null);
    }
}
