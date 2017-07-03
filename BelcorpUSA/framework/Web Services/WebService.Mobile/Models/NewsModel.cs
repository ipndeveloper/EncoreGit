using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using NetSteps.Common.Base;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.WebService.Mobile.Models
{
    public class NewsModel
    {
        public int id;
        public string date;
        public string title;
        public string text;
        public string thumb;
        public bool hasValidThumb;
        public int type;
        public string typename;
        public string summary;
        public int sort;
        public bool isAlert;
        public bool isFeatured;

        public NewsModel()
        {
        }

        public NewsModel(News news, SqlUpdatableList<NewsType> newsTypes)
        {
            var imageUrl = "";
            var caption = string.Empty;
            var content = string.Empty;

            var prodContent = news.HtmlSection.ProductionContent(MobileService.CurrentSite, MobileService.CurrentLanguage.LanguageID);

            if (prodContent == null)
                return;

            caption = prodContent.Caption() ?? string.Empty;
            content = prodContent.Body() ?? string.Empty;

            var imageEnd = 0;
            var imageStart = content.IndexOf("<img ");
            if (imageStart > -1)
            {
                imageEnd = content.Substring(imageStart).IndexOf(" />") + imageStart + 3;
                var imageElement = content.Substring(imageStart, imageEnd - imageStart);
                imageUrl = prodContent.ParseAttribute(imageElement, "src");
                //content = content.Replace(imageElement, string.Empty);
            }

            hasValidThumb = !string.IsNullOrWhiteSpace(imageUrl) && DoesImageExistRemotely(imageUrl);

            this.id = news.NewsID;
            this.date = news.StartDate.ToString("dd-MM-yyyy");
            this.title = prodContent.Title();
            this.text = content;
            this.thumb = imageUrl;
            this.type = news.NewsTypeID;
            this.summary = caption;
            this.isFeatured = news.IsFeatured;
            this.isAlert = false;

            var newsType = newsTypes.FirstOrDefault(nt => nt.NewsTypeID == this.type);
            if (newsType != null)
            {
                this.typename = CachedData.Translation.GetTerm(MobileService.CurrentLanguage.LanguageID, newsType.TermName ?? ("NewsType_" + newsType.Name), newsType.Name);
                // set the sort by news type for now, though a secondary sort by date will be added later
                this.sort = newsType.GetSortIndexByLanguage(MobileService.CurrentLanguage.LanguageID, SmallCollectionCache.Instance.NewsTypes);
            }
        }

        /*public static implicit operator NewsModel(MailMessage message)
        {
            var model = new NewsModel
            {
                id = message.MailMessageID,
                //date = message.Date.ToString("MM-dd-yyyy"),
                thumb = "lib/resources/themes/images/encore/alert.png",
                isAlert = true,
                isFeatured = false
            };

            //if (message.Body.IsNullOrEmpty())
            //    message = message.Load();

            //var chunks = message.Body.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            //int chunksCount = chunks.Count();
            //var firstName = string.Empty;
            //var lastName = string.Empty;
            //var comments = string.Empty;
            //var trailingComment = string.Empty;

            //if (message.Subject == "Contact request" && chunksCount >= 6)
            //{
            //    firstName = chunks[0].Replace("First Name: ", string.Empty);
            //    lastName = chunks[1].Replace("Last Name: ", string.Empty);
				
            //    var email = chunks[2].Replace("Email: ", string.Empty);
            //    var phone = chunks[3].Replace("Phone: ", string.Empty);
				
            //    comments = chunks[4].Replace("Comments: ", string.Empty);
            //    if (chunksCount > 6)
            //        for (int i = 5; i < chunksCount - 1; i++)
            //            comments += "<br />" + chunks[i];

            //    if (!email.IsNullOrEmpty() || !phone.IsNullOrEmpty())
            //    {
            //        comments += "<br /><br />Contact Information: ";
            //        if (!email.IsNullOrEmpty())
            //            comments += GetFormattedEmailRow(email);
            //        if (!phone.IsNullOrEmpty())
            //            comments += GetFormattedPhoneRow(phone);
            //    }
            //}
            //else if (message.Subject == "Host request" && chunksCount >= 13)
            //{
            //    firstName = chunks[0].Replace("First name: ", string.Empty);
            //    lastName = chunks[1].Replace("Last name: ", string.Empty);

            //    var email = chunks[2].Replace("Email: ", string.Empty);
            //    var phone = chunks[3].Replace("Phone: ", string.Empty);
            //    var address1 = chunks[4].Replace("Address: ", string.Empty);
            //    var address2 = chunks[5].Replace("Line 2", string.Empty);
            //    var city = chunks[6].Replace("City: ", string.Empty);
            //    var state = chunks[7].Replace("State: ", string.Empty);
            //    var zip = chunks[8].Replace("Zip: ", string.Empty);
            //    var country = chunks[9].Replace("Country: ", string.Empty);
            //    var preferredMethod = chunks[10].Replace("Preferred contact method: ", string.Empty);
            //    var date = chunks[chunksCount - 1].Replace("Date for proposed party: ", string.Empty);
				
            //    comments = chunks[11].Replace("Comments: ", string.Empty);
            //    if (chunksCount > 13)
            //        for (int i = 12; i < chunksCount - 1; i++)
            //            comments += "<br />" + chunks[i];

            //    comments += "<br /><br />Contact Information: ";
                
            //    if (!email.IsNullOrEmpty())
            //        comments += GetFormattedEmailRow(email);
            //    if (!phone.IsNullOrEmpty())
            //        comments += GetFormattedPhoneRow(phone);

            //    comments += "<br />Address: " + address1 + "  " + address2 + " " + city + ", " + state + " " + zip + ", " + country;
            //    comments += "<br />Preferred Contact Method: " + preferredMethod;
            //    comments += "<br />Proposed Date: " + date;

            //    trailingComment = "<br /><br />Visit your home workstation for more detailed information.";
            //}
            //else
            //{
            //    model.title = message.Subject;
            //    model.text = message.Body;
            //}

            //if (!firstName.IsNullOrEmpty() && !lastName.IsNullOrEmpty())
            //{
            //    model.title = string.Format("{0} from {1} {2}", message.Subject, firstName, lastName);
            //    model.text = comments;

            //    var breakPoint = comments.IndexOfAny(new char[] { '.', '?', '!', ';' });
            //    if (breakPoint > -1 && breakPoint <= 110)
            //        model.summary = comments.Substring(0, breakPoint + 1);
            //    else
            //    {
            //        breakPoint = Math.Max(comments.Left(Math.Min(comments.Length, 110)).LastIndexOf(' '), comments.Length);
            //        model.summary = comments.Substring(0, breakPoint) + "...";
            //    }
            //}

            //model.text += trailingComment;

            return model;
        }*/

        private static string GetFormattedEmailRow(string email)
        {
            return string.Format("<br />Email: <a href=\"mailto:{0}\">{0}</a>", email);
        }

        private static string GetFormattedPhoneRow(string phone)
        {
            return string.Format("<br />Phone: <a href=\"tel:{0}\">{1}</a>", Regex.Replace(phone, "[() -]", ""), phone);
        }

        private bool DoesImageExistRemotely(string uriToImage)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uriToImage);
                request.Method = "HEAD";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch { return false; }
        }
    }
}