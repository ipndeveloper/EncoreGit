
namespace NetSteps.Data.Entities.Business.Logic
{
    public partial class HtmlContentBusinessLogic
    {
        public HtmlContent CloneHtmlContent(HtmlContent existingHtmlContent)
        {
            HtmlContent newHtmlContent = new HtmlContent();

            newHtmlContent.HtmlContentStatusID = existingHtmlContent.HtmlContentStatusID;
            newHtmlContent.LanguageID = existingHtmlContent.LanguageID;
            newHtmlContent.Name = existingHtmlContent.Name;
            newHtmlContent.PublishDateUTC = existingHtmlContent.PublishDateUTC;
            newHtmlContent.SortIndex = existingHtmlContent.SortIndex;
            newHtmlContent.CreatedByUserID = existingHtmlContent.CreatedByUserID;

            foreach (var htmlElement in existingHtmlContent.HtmlElements)
            {
                HtmlElement newHtmlElement = new HtmlElement();

                newHtmlElement.HtmlElementTypeID = htmlElement.HtmlElementTypeID;
                newHtmlElement.HtmlContentID = htmlElement.HtmlContentID;
                newHtmlElement.Contents = htmlElement.Contents;
                newHtmlElement.SortIndex = htmlElement.SortIndex;
                newHtmlElement.Active = htmlElement.Active;

                newHtmlContent.HtmlElements.Add(newHtmlElement);
            }

            return newHtmlContent;
        }

    }
}
