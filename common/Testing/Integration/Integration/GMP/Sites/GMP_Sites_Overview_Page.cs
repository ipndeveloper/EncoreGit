using WatiN.Core;
using System.Text.RegularExpressions;
using System;

namespace NetSteps.Testing.Integration.GMP.Sites
{
    public class GMP_Sites_Overview_Page : GMP_Sites_WebsiteBase_Page
    {
        //private Div _module;

        //protected override void InitializeContents()
        //{
        //    base.InitializeContents();
        //    _module = Document.GetElement<Div>(Find.ByClass("LandingModules"));
        //    _module.CustomWaitUntilExist(5);
        //}
        
         public override bool IsPageRendered()
        {
            //return _module.Exists;
            return Document.GetElement<Div>(new Param("LandingModules", AttributeName.ID.ClassName)).Exists;
        }

         public TPage ClickLoadSiteInEditMode<TPage>(int? timeout = null) where TPage : NS_Page, new()
         {
             Link lnk = Document.GetElement<Link>(new Param("EditSite", AttributeName.ID.ClassName, RegexOptions.None));
             string[] separators = { "/Login" };
             string url = lnk.GetAttributeValue("href").Split(separators, 2, StringSplitOptions.None)[0];
             lnk.CustomClick(timeout);
             return Util.AttachBrowser<TPage>(url);
         }
    }
}
