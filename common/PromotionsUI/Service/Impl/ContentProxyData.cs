using System.Collections.Generic;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.UI.Common.Interfaces;

namespace NetSteps.Promotions.UI.Service.Impl
{
    ///// <summary>
    ///// This is meant to be a temporary container for the content proxy 
    ///// for now it'll just wrap the current functionality of the TranslationProvider.
    ///// </summary>
    //public class ContentProxyData : IContentProxyData
    //{
    //    private ITermLocalizedProxy proxy;

    //    public ContentProxyData()
    //    {
    //        proxy = Create.New<ITermLocalizedProxy>();
    //    }

    //    public virtual IEnumerable<string> GetFilePathsRelatedTo()
    //    {
    //        yield break;
    //    }

    //    public ITermLocalizedProxy AllTerms
    //    {
    //        get { return proxy; }
    //    }
    //}

    //public class FakeProxyData : ContentProxyData
    //{
    //    public override IEnumerable<string> GetFilePathsRelatedTo()
    //    {
    //        yield return "http://placekitten.com/200/200";
    //        yield return "http://placekitten.com/100/100";
    //        yield return "http://placekitten.com/100/200";
    //    }
    //}
}
