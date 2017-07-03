using System;
using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Web.Mvc;
using System.Xml;

namespace NetSteps.Web.Mvc.ActionResults
{
    public class RssResult<T> : FileResult
    {
        private Uri currentUrl;

        public List<T> Items { get; set; }
        public string Title { get; set; }
        public Func<T, SyndicationItem> ConvertToSyndicationItem { get; set; }        

        public RssResult() : base("application/rss+xml") { }

        public RssResult(List<T> items, string title, Func<T, SyndicationItem> convertToSyndicationItem)
            : this()
        {
            Items = items;
            Title = title;
            ConvertToSyndicationItem = convertToSyndicationItem;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            currentUrl = context.RequestContext.HttpContext.Request.Url;
            base.ExecuteResult(context);
        }

        protected override void WriteFile(System.Web.HttpResponseBase response)
        {
            throw new NotImplementedException();
        }
    }
}
