<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% var sites = ViewData["Sites"] as List<Site>;
   var currentSite = nsCore.Areas.Sites.Controllers.BaseSitesController.GetCurrentSiteFromCache();
       var corporateSites = sites.Where(s => s.SiteTypeID == Constants.SiteType.Corporate.ToInt());
       if (corporateSites.Count() > 0)
       {
           List<NavigationItem> utilities = new List<NavigationItem>();
           if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CorporateMasterSiteId"]))
           {
               utilities.Add(new NavigationItem()
               {
                   Url = "~/Sites/Edit?isBase=true&siteTypeId=" + (int)Constants.SiteType.Corporate,
                   LinkText = "+ <span>" + Html.Term("CreateNewBaseSite", "Create new base site") + "</span>",
                   Function = "Sites-Create and Edit Base Site"
               });
           }
           Response.Write(Html.DropDownNavigation(Html.Term("GlobalSite", "Global Site"), "", corporateSites.Select(s => new NavigationItem()
           {
               Url = "~/Sites/Overview/Index/" + s.SiteID,
               LinkText = s.Name,
               RequestParamKey = "id",
               RequestParamValue = s.SiteID
           }), utilities, currentSite != null && currentSite.SiteTypeID == (int)Constants.SiteType.Corporate && !ViewContext.RouteData.Values["controller"].Equals("TermTranslations")));
       }
       var backOfficeSites = sites.Where(s => s.SiteTypeID == Constants.SiteType.BackOffice.ToInt());
       if (backOfficeSites.Count() > 0)
       {
           List<NavigationItem> utilities = new List<NavigationItem>();
           if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["BackOfficeMasterSiteId"]))
           {
               utilities.Add(new NavigationItem()
               {
                   Url = "~/Sites/Edit?isBase=true&siteTypeId=" + (int)Constants.SiteType.BackOffice,
                   LinkText = "+ <span>" + Html.Term("CreateNewBaseSite", "Create new base site") + "</span>",
                   Function = "Sites-Create and Edit Base Site"
               });
           }
           Response.Write(Html.DropDownNavigation(Html.Term("DistributorWorkstation", "Distributor Workstation"), "", backOfficeSites.Select(s => new NavigationItem()
           {
               Url = "~/Sites/Overview/Index/" + s.SiteID,
               LinkText = s.Name,
               RequestParamKey = "id",
               RequestParamValue = s.SiteID
           }), utilities, currentSite != null && currentSite.SiteTypeID == (int)Constants.SiteType.BackOffice && !ViewContext.RouteData.Values["controller"].Equals("TermTranslations")));
       }
       var replicatedSites = sites.Where(s => s.SiteTypeID == Constants.SiteType.Replicated.ToInt());
       if (replicatedSites.Count() > 0)
       {
           List<NavigationItem> utilities = new List<NavigationItem>();
           if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReplicatedMasterSiteId"]))
           {
               utilities.Add(new NavigationItem()
               {
                   Url = "~/Sites/Edit?isBase=true&siteTypeId=" + (int)Constants.SiteType.Replicated,
                   LinkText = "+ <span>" + Html.Term("CreateNewBaseSite", "Create new base site") + "</span>",
                   Function = "Sites-Create and Edit Base Site"
               });
           }
           Response.Write(Html.DropDownNavigation(Html.Term("PersonalSites", "Personal Sites"), "", replicatedSites.Select(s => new NavigationItem()
           {
               Url = "~/Sites/Overview/Index/" + s.SiteID,
               LinkText = s.Name,
               RequestParamKey = "id",
               RequestParamValue = s.SiteID
           }), utilities, currentSite != null && currentSite.SiteTypeID == (int)Constants.SiteType.Replicated && !ViewContext.RouteData.Values["controller"].Equals("TermTranslations")));
       } %>
    <%= Html.SubTab("~/Sites/TermTranslations", Html.Term("TermTranslations", "Term Translations"), function: "Sites-Term Translations")%>
    <%= Html.SubTab("~/Sites/Copy", Html.Term("CopySite", "Copy Site"), function: "Sites-Copy")%>