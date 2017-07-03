using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Base;

namespace NetSteps.Web.Mvc.Extensions
{
    public static class MessageExtensions
    {
        public static ControllerBase AddMessageToTempData(this ControllerBase controller, BasicResponse basicResponse)
        {
            controller.TempData.AddMessage(basicResponse);

            return controller;
        }

        public static ControllerBase AddMessageToViewData(this ControllerBase controller, BasicResponse basicResponse)
        {
            controller.ViewData.AddMessage(basicResponse);

            return controller;
        }

        public static IEnumerable<BasicResponse> GetMessages(this ControllerBase controller)
        {
            if (controller == null)
            {
                return Enumerable.Empty<BasicResponse>();
            }

            return controller.TempData.GetMessages()
                .Concat(controller.ViewData.GetMessages());
        }

        public static MvcHtmlString Messages(this HtmlHelper helper)
        {
            var messages = helper.ViewContext.TempData.GetMessages()
                .Concat(helper.ViewData.GetMessages());

            if (!messages.Any())
            {
                return null;
            }

            string divClass = "validation-summary-errors";

            var divBuilder = new TagBuilder("div");
            divBuilder.MergeAttribute("class", divClass);
            var ulBuilder = new TagBuilder("ul");
            var listBuilder = new StringBuilder();

            foreach (var message in messages)
            {
                var liBuilder = new TagBuilder("li");
                liBuilder.InnerHtml = message.Message;
                listBuilder.Append(liBuilder.ToString());
            }

            ulBuilder.InnerHtml = listBuilder.ToString();
            divBuilder.InnerHtml = ulBuilder.ToString();
            var divHtml = divBuilder.ToString().ToMvcHtmlString();

            return divHtml;
        }

        private static IDictionary<string, object> AddMessage(this IDictionary<string, object> dictionary, BasicResponse basicResponse)
        {
            if (dictionary == null)
            {
                return null;
            }

            if (!dictionary.ContainsKey("_messages"))
            {
                dictionary["_messages"] = new List<BasicResponse>();
            }

            var messages = dictionary["_messages"] as List<BasicResponse>;

            if (messages == null)
            {
                return dictionary;
            }

            messages.Add(basicResponse);

            return dictionary;
        }

        private static IEnumerable<BasicResponse> GetMessages(this IDictionary<string, object> dictionary)
        {
            if (dictionary == null
                || !dictionary.ContainsKey("_messages")
                || !(dictionary["_messages"] is IEnumerable<BasicResponse>))
            {
                return Enumerable.Empty<BasicResponse>();
            }

            return dictionary["_messages"] as IEnumerable<BasicResponse>;
        }
    }
}