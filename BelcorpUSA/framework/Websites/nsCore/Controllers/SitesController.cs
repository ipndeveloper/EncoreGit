using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using LumenWorks.Framework.IO.Csv;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.ActionResults;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Models;
using nsCore5.Models;

namespace nsCore.Controllers
{
    public class SitesController : BaseController
    {
        public Site CurrentSite
        {
            get
            {
                if (Request.Params.AllKeys.Contains("SiteId") && Request.Params.AllKeys.Contains("SiteToEdit"))
                {
                    Session["SiteToEdit"] = Site.Load(int.Parse(Request.Params["SiteToEdit"]));
                }
                return Session["SiteToEdit"] as Site;
            }
            set { Session["SiteToEdit"] = value; }
        }

        /// <summary>
        /// Make sure we always have all of the base sites in the sub navigation
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewData["Sites"] = Site.LoadBaseSites(CoreContext.CurrentMarketId, 0);
            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// Lists out the base sites and the actions on each
        /// </summary>
        /// <returns></returns>
        [FunctionFilter("Sites", "~/Accounts")]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Shows an overview of the selected base site
        /// </summary>
        /// <param name="id">The id of the site</param>
        /// <returns></returns>
        [FunctionFilter("Sites", "~/Accounts")]
        public ActionResult Overview(int? id)
        {
            if (id.HasValue && id.Value > 0)
                CurrentSite = Site.Load(id.Value);
            if (CurrentSite == null)
                return RedirectToAction("Index");

            //Load up all of the types and events to show
            ViewData["NewsTypes"] = SmallCollectionCache.Instance.NewsTypes.ToIListValueList(); // ListValue.GetValues(ListTypes.NewsType, CurrentSite.DefaultLanguageID, CoreContext.CurrentMarketId);
            ViewData["Events"] = CalendarEvent.LoadByDateRange(DateTime.Now, DateTime.Now.AddDays(int.Parse(ConfigurationManager.AppSettings["OverviewDays"])), 0, CurrentSite.DefaultLanguageID.ToInt(), CoreContext.CurrentMarketId, true);
            ViewData["CalendarEventTypes"] = AccountListValue.LoadCorporateListValuesByType(NetSteps.Data.Entities.Constants.ListValueType.CalendarEventType.ToInt());
            ViewData["ArchiveTypes"] = SmallCollectionCache.Instance.ArchiveTypes; // ListValue.GetValues(ListTypes.ArchiveType, CurrentSite.DefaultLanguageID, CoreContext.CurrentMarketId);
            ViewData["Days"] = ConfigurationManager.AppSettings["OverviewDays"];
            return View(CurrentSite);
        }

        #region PWS Content Review
        [FunctionFilter("Sites-PWS Content Review", "~/Sites/Overview")]
        public ActionResult ReviewContent(int? id)
        {
            if (id.HasValue && id.Value > 0)
                CurrentSite = Site.Load(id.Value);
            if (CurrentSite == null)
                return RedirectToAction("Index");

            List<Content> results = new List<Content>();
            var contentAndDistributorNameByStatus = HtmlContent.GetContentAndDistributorNameByStatus(NetSteps.Data.Entities.Constants.HtmlContentStatus.Submitted.ToInt());
            var allContents = HtmlContent.LoadBatch(contentAndDistributorNameByStatus.ToList(c => c.ID));
            foreach (var row in contentAndDistributorNameByStatus)
            {
                //HtmlContent content = HtmlContent.Load(row.ID);
                HtmlContent content = allContents.FirstOrDefault(c => c.HtmlContentID == row.ID);

                if (content != null)
                {
                    HtmlSection section = HtmlSection.LoadByContentID(content.HtmlContentID, Convert.ToInt32(row.SiteID));

                    if (section != null)
                    {
                        NetSteps.Web.Mvc.Content contentControl = new NetSteps.Web.Mvc.Content()
                        {
                            ContentToRender = content,
                            HtmlSection = section,
                            UseNewEditWrapper = true,
                            ControllerName = "Master",
                            UseSurroundImagesWithLinks = false,
                            PageMode = NetSteps.Common.Constants.ViewingMode.Staging,
                            SiteID = Convert.ToInt32(row.SiteID)
                        };

                        results.Add(new Content()
                        {
                            ConsultantName = row.Consultant,
                            UploadedOn = Convert.ToDateTime(row.Uploaded),
                            InnerContent = contentControl
                        });
                    }
                }
            }
            return View(results);
        }

        [FunctionFilter("Sites-PWS Content Review", "~/Sites/Overview")]
        public ActionResult ApproveContent(int contentId, int siteId)
        {
            try
            {
                HtmlContent content = HtmlContent.Load(contentId);
                if (content != null)
                {
                    HtmlSection section = HtmlSection.LoadByContentID(content.HtmlContentID, siteId);
                    if (section != null)
                    {
                        HtmlSection.SelectChoice(siteId, section.HtmlSectionID, content.HtmlContentID);

                        HtmlContentHistory history = new HtmlContentHistory()
                        {
                            HtmlContentID = content.HtmlContentID,
                            //HtmlContentStatusID = this.HtmlContentStatusID, // Should we set this? - JHE
                            UserID = CoreContext.CurrentUser.UserID,
                            HistoryDate = DateTime.Now,
                            Comments = "Content Changes Approved"
                        };
                        history.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

            return Json(new { result = true });
        }

        [FunctionFilter("Sites-PWS Content Review", "~/Sites/Overview")]
        public ActionResult DenyContent(int contentId, string reason)
        {
            try
            {
                HtmlContent content = HtmlContent.Load(contentId);
                content.Disapprove(reason);

                HtmlContentHistory history = new HtmlContentHistory();
                history.HtmlContentID = content.HtmlContentID;
                history.UserID = CoreContext.CurrentUser.UserID;
                history.Comments = reason;
                history.Save();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

            return Json(new { result = true });
        }
        #endregion

        #region Site Management
        /// <summary>
        /// Show the child sites of this base site
        /// </summary>
        /// <param name="id">The id of the base site</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Replicated Sites", "~/Sites/Overview")]
        public ActionResult ReplicatedSites(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                CurrentSite = Site.Load(id.Value);
            }
            if (CurrentSite != null)
            {
                ViewData["BaseSiteId"] = CurrentSite.SiteID;
                var sites = Site.GetPagedChildSiteDetails(CurrentSite.SiteID, null, null, new PaginatedListParameters());
                ViewData["ResultCount"] = sites.TotalCount;
                return View("SiteManagement", sites);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// Checks if the url is available
        /// </summary>
        /// <param name="url">The url to check for</param>
        /// <returns></returns>
        public ActionResult CheckIfUrlAvailable(string url)
        {
            return Content(SiteUrl.IsAvailable(url).ToString());
        }

        /// <summary>
        /// Get the sites with pagination
        /// </summary>
        /// <param name="baseSiteId">The id of the base site</param>
        /// <param name="page">The current page</param>
        /// <param name="pageSize">The size of the current page</param>
        /// <param name="status">A possible status to check for</param>
        /// <param name="siteName">A possible site name to check for</param>
        /// <param name="orderBy">The column to order by</param>
        /// <param name="orderByDirection">The direction of the sort</param>
        /// <returns></returns>
        public ActionResult GetSites(int baseSiteId, int page, int pageSize, int? status, string siteName, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            PaginatedListParameters paginatedListParameters = new PaginatedListParameters()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection
            };
            var sites = Site.GetPagedChildSiteDetails(baseSiteId, status, siteName, paginatedListParameters);
            StringBuilder siteBuilder = new StringBuilder();
            int count = 0;
            foreach (SiteSearchData site in sites)
            {
                siteBuilder.Append("<tr class=\"GridRow").Append(count % 2 == 0 ? "" : " Alt").Append("\"><td><a id=\"site").Append(site.SiteID)
                    .Append("\" href=\"").Append("~/Sites/EditSite/".ResolveUrl()).Append(site.SiteID).Append("\" class=\"editSite\">")
                    .Append(site.SiteName).Append("</a></td><td>").Append(site.Url).Append("</td><td>").Append(site.Enrolled).Append("</td><td>")
                    .Append(site.SiteStatusID == 0 ? "Active" : "Cancelled").Append("</td></tr>");
                ++count;
            }
            return Json(new { totalPages = sites.TotalPages, page = siteBuilder.ToString() });
        }

        /// <summary>
        /// Show a site to be able to edit it
        /// </summary>
        /// <param name="id">The id of the site to edit</param>
        /// <param name="baseSiteId">The base site of a new site</param>
        /// <param name="isBase">Whether the new site is a base site or not</param>
        /// <returns></returns>
        public ActionResult EditSite(int? id, int? baseSiteId, bool? isBase)
        {
            string domains = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.Domains);
            string masterSiteId = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.MasterSiteId);

            // Set the possible domains to use
            if (!string.IsNullOrEmpty(masterSiteId))
            {
                Site masterSite = Site.Load(int.Parse(masterSiteId));
                try
                {
                    ViewData["Domains"] = masterSite != null && masterSite.Settings["Domains"] != null ? masterSite.Settings["Domains"].Value.ToString().Split(';') : domains.Split(';');
                }
                catch
                {
                    ViewData["Domains"] = new string[] { "netsteps.com" };
                }
            }
            else if (!string.IsNullOrEmpty(domains))
            {
                ViewData["Domains"] = domains.Split(';');
            }

            // Get the base site id for this site
            if (baseSiteId.HasValue)
                ViewData["BaseSiteId"] = baseSiteId.Value;
            else
            {
                if (CurrentSite == null && id.HasValue)
                    CurrentSite = Site.Load(id.Value);
                if (CurrentSite != null)
                    ViewData["BaseSiteId"] = CurrentSite.SiteID;
            }
            if (CurrentSite == null)
                return RedirectToAction("Index");

            // Load the site
            Site site;
            if (id.HasValue && id.Value > 0)
            {
                site = Site.Load(id.Value);
            }
            else if (TempData["Site"] != null)
            {
                site = TempData["Site"] as Site;
            }
            else if (isBase.HasValue && isBase.Value)
            {
                site = new Site()
                {
                    IsBase = true
                };
            }
            else
            {
                site = new Site();
            }
            if (site.IsBase.ToBool())
            {
                if (!CoreContext.CurrentUser.HasFunction("Sites-Create and Edit Base Site"))
                    return RedirectToAction("Overview");
            }
            else
            {
                if (!CoreContext.CurrentUser.HasFunction("Sites-Replicated Sites"))
                    return RedirectToAction("Overview");
            }
            return View(site);
        }

        /// <summary>
        /// Save a site
        /// </summary>
        /// <param name="isBase">Whether the site is a base site or not</param>
        /// <param name="baseSiteId">The id of the base site</param>
        /// <param name="siteId">The id of this site (0 for new site)</param>
        /// <param name="siteName">The name of the site</param>
        /// <param name="description">The description of the site</param>
        /// <param name="active">Whether the site is active or not</param>
        /// <param name="url">The subdomain of the site</param>
        /// <param name="domain">The tld of the site</param>
        /// <param name="accountId">The id of the account to attach to the site</param>
        /// <param name="firstName">The first name of the account attached to the site</param>
        /// <param name="middleInitial">The middle initial of the account attached to the site</param>
        /// <param name="lastName">The last name of the account attached to the site</param>
        /// <param name="email">The email of the account attached to the site</param>
        /// <param name="username">The username of the account attached to the site</param>
        /// <param name="password">The password of the account attached to the site</param>
        /// <param name="marketId">The market of the site</param>
        /// <param name="languageId">The language of the site</param>
        /// <param name="returnUrl">The url to go to after the site is saved</param>
        /// <returns></returns>
        public ActionResult SaveSite(bool isBase, int baseSiteId, int siteId, string siteName, string description, int statusId, string url, string domain, int? accountId, string firstName, string middleInitial, string lastName, string email, string username, string password, int marketId, int languageId, string returnUrl)
        {
            if (isBase)
            {
                if (!CoreContext.CurrentUser.HasFunction("Sites-Create and Edit Base Site"))
                    return RedirectToAction("Overview");
            }
            else
            {
                if (!CoreContext.CurrentUser.HasFunction("Sites-Replicated Sites"))
                    return RedirectToAction("Overview");
            }
            Site site = null;
            try
            {
                if (siteId == 0)
                {
                    if (isBase)
                        site = new Site();
                    else
                    {
                        siteId = Site.Load(CurrentSite.SiteID).DuplicateSite(siteName, marketId);
                        site = Site.Load(siteId);
                        site.DateSignedUp = DateTime.Now;
                    }
                }
                else
                {
                    site = Site.Load(siteId);
                }
                site.Name = siteName;
                site.Description = description;
                site.SiteStatusID = statusId.ToShort();
                if (!isBase)
                {
                    if (accountId.HasValue && accountId > 0)
                    {
                        site.AccountID = accountId.Value;
                    }
                    else
                    {
                        AccountType consultantType = SmallCollectionCache.Instance.AccountTypes.FirstOrDefault(at => at.Name.ToLower() == "consultant");
                        if (consultantType == null)
                            consultantType = SmallCollectionCache.Instance.AccountTypes.First();
                        Account newAccount = new Account()
                        {
                            AccountTypeID = (int)consultantType.AccountTypeID,
                            AccountStatusID = (int)NetSteps.Data.Entities.Constants.AccountStatus.Active,
                            //ModifiedByApplicationID = 1, // nsCore
                            FirstName = firstName,
                            MiddleName = middleInitial,
                            LastName = lastName,
                            EmailAddress = email,
                            DateCreated = DateTime.Now
                        };
                        newAccount.User = new NetSteps.Data.Entities.User()
                        {
                            Username = username,
                            Password = password
                        };
                        newAccount.Save();
                        site.AccountID = newAccount.AccountID;
                    }
                }
                site.MarketID = marketId;
                site.DefaultLanguageID = languageId;
                site.SiteStatusID = NetSteps.Data.Entities.Constants.SiteStatus.Active.ToShort();
                site.IsBase = isBase;
                site.Save();
                (CoreContext.CurrentUser as CorporateUser).GrantSiteAccess(site.SiteID);

                SiteUrl siteUrl;
                if (site.PrimaryUrl == null)
                {
                    siteUrl = new SiteUrl();
                    site.SiteUrls.Add(siteUrl);
                }
                else
                {
                    siteUrl = site.PrimaryUrl;
                }
                siteUrl.IsPrimaryUrl = true;
                siteUrl.Url = "http://" + url + "." + domain;
                //siteUrl.CultureInfo = "en-US";
                siteUrl.ExpirationDate = new DateTime(1900, 1, 1);
                site.Save();

                // If it's a base site, copy all of the default stuff from the master site
                if (siteId == 0 && site.IsBase.ToBool())
                {
                    int masterSiteId = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.MasterSiteId);
                    Site masterSite = Site.Load(masterSiteId);
                    int homePageId = 0;
                    foreach (Page page in masterSite.Pages)
                    {
                        Page newPage = new Page();
                        newPage.IsStartPage = page.IsStartPage;
                        newPage.Name = page.Name;
                        masterSite.Pages.Add(newPage);
                        Copy(page, ref newPage);
                        masterSite.Save();
                        if (newPage.Name.ToLower() == "Home")
                        {
                            homePageId = newPage.PageID;
                        }


                        throw EntityExceptionHelper.GetAndLogNetStepsException("Finish porting this code - JHE");
                        //foreach (HtmlSection section in page.HtmlSections)
                        //{
                        //    HtmlSection newSection = newPage.HtmlSections.AddNew();
                        //    Copy(section, ref newSection);
                        //    newSection.Save();
                        //    HtmlContent defaultContent = new HtmlContent();
                        //    Copy(section.ProductionContent, ref defaultContent);
                        //    defaultContent.HtmlSectionId = newSection.Id;
                        //    defaultContent.Save();
                        //}
                    }



                    // 'Clone' Navigation for new site. - JHE
                    foreach (var navigationType in NavigationType.LoadBySiteTypeID(NetSteps.Data.Entities.Constants.SiteType.Replicated.ToInt()))
                    {
                        //foreach (var nav in masterSite.SiteNavigations.Where(s => s.NavigationTypeID == navigationType.NavigationTypeID))
                        //{

                        // TODO: Finish porting this. - JHE

                        //// TODO: Test this new code - JHE
                        //Navigation newNavigation = new Navigation();
                        //List<string> excludedProperties = new List<string>() { "ChangeTracker", "NavigationID", "BaseNavigationID" };
                        //Reflection.CopyProperties(nav.Navigation, newNavigation, null, excludedProperties);

                        //SiteNavigation siteNavigation = new SiteNavigation();
                        //siteNavigation.NavigationTypeID = navigationType.NavigationTypeID;
                        //siteNavigation.Navigation = newNavigation;
                        //siteNavigation.PageID = nav.PageID;
                        //siteNavigation.IsPrimary = nav.IsPrimary;
                        //site.SiteNavigations.Add(siteNavigation);
                        //site.Save();





                        //Navigation newNavigation = site.Navigation[navigationType.NavigationTypeID].AddNew();

                        //Copy(nav, ref newNavigation);
                        ////if (newNav.PageName.ToLower() == "Home")
                        ////{
                        ////    newNav.PageID = homePageId;
                        ////}
                        //newNavigation.Save();
                        //}
                    }
                    //site.Save();


                    // NEW Code - JHE
                    ////Site baseSite = Site.LoadFull(masterSite.BaseSiteID.ToInt());
                    //foreach (SiteSetting setting in baseSite.SiteSettings)
                    //{
                    //    SiteSetting newSetting = new SiteSetting(site.Id, setting.Title);
                    //    newSetting.Save();
                    //}

                    //foreach (var settingValue in masterSite.Settings)
                    //{
                    //    SiteSettingValue newSettingValue = new SiteSettingValue(settingValue.Name, settingValue.Value, site.SiteID);
                    //    newSettingValue.Save();
                    //}
                }
                TempData["SavedSite"] = true;

                if (CurrentSite.SiteID == site.SiteID)
                    CurrentSite = site;

                if (!string.IsNullOrEmpty(returnUrl))
                    return Redirect(returnUrl);
                return RedirectToAction("ReplicatedSites");
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                TempData["Site"] = site;
                TempData["Error"] = exception.PublicMessage;
                return RedirectToAction("EditSite");
            }
        }

        public ActionResult GetChildSites(int baseSiteId)
        {
            return Json(Site.LoadByBaseSiteID(baseSiteId).Select(s => new { siteId = s.SiteID, siteName = s.Name, description = s.Description, url = s.PrimaryUrl == null ? "" : s.PrimaryUrl.Url }));
        }

        /// <summary>
        /// Make a copy of one object into another
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj1">The object to copy from</param>
        /// <param name="obj2">The object to copy to</param>
        private void Copy<T>(T obj1, ref T obj2) where T : class
        {
            foreach (PropertyInfo property in obj1.GetType().GetProperties())
            {
                if (property.GetIndexParameters().Length == 0 && property.CanWrite && property.CanRead && property.Name.ToLower() != "id" && property.Name.ToLower() != "siteid" && property.Name.ToLower() != "ownerid")
                {
                    try
                    {
                        property.SetValue(obj2, property.GetValue(obj1, null), null);
                    }
                    catch { }
                }
            }
        }
        #endregion

        #region Navigation
        public int NavigationTypeId
        {
            get
            {
                if (Session["NavigationTypeId"] == null)
                {
                    var navigationTypes = NavigationType.LoadBySiteTypeID(NetSteps.Data.Entities.Constants.SiteType.Replicated.ToInt());
                    //Dictionary<int, string> navigationTypes = Navigation.GetNavigationTypes(Constants.SiteType.Replicated);
                    Session["NavigationTypeId"] = navigationTypes.OrderBy(t => t.NavigationTypeID).FirstOrDefault().NavigationTypeID;
                }
                return (int)Session["NavigationTypeId"];
            }
            set { Session["NavigationTypeId"] = value; }
        }

        /// <summary>
        /// Show a site map of all of the navigation
        /// </summary>
        /// <param name="id">The id of the current site</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Site Map", "~/Sites/Overview")]
        public ActionResult SiteMap(int? id)
        {
            if (id.HasValue && id.Value > 0)
                CurrentSite = Site.Load(id.Value);
            if (CurrentSite == null)
                return RedirectToAction("Index");
            if (!string.IsNullOrEmpty(Request.Params["navigationType"]))
                NavigationTypeId = int.Parse(Request.Params["navigationType"]);

            // Reload the navigation and pages to get the most current stuff
            //CurrentSite.ResetNavigation();
            //CurrentSite.ResetPages();
            ViewData["Pages"] = CurrentSite.Pages.Where(p => p.ParentID > 0);
            StringBuilder builder = new StringBuilder();
            BuildNavigationTreeRecursively(null, builder);
            ViewData["Links"] = builder.ToString();
            ViewData["NavigationTypes"] = NavigationType.LoadBySiteTypeID(NetSteps.Data.Entities.Constants.SiteType.Replicated.ToInt());
            return View();
        }

        /// <summary>
        /// Build out the navigation tree as a ul > li
        /// </summary>
        /// <param name="parent">The parent link</param>
        /// <param name="builder">The builder to append to</param>
        private void BuildNavigationTreeRecursively(Navigation parent, StringBuilder builder)
        {
            // TODO: Finish porting this. - JHE

            //builder.Append("<ul>");
            //foreach (Navigation link in (parent == null ? CurrentSite.SiteNavigations.GetByNavigationTypeID(NavigationTypeId) : parent.ChildNavigations.ToList()).OrderBy(n => n.SortIndex))
            //{
            //    int currentEditingLanguageID = CurrentSite.GetDefaultEditingLanguageID();
            //    builder.Append("<li id=\"").Append(link.NavigationID).Append("\"><a class=\"NavNode\" href=\"javascript:void(0);\">").Append(link.GetLinkText(currentEditingLanguageID)).Append("</a>&nbsp;<a href=\"javascript:void(0);\" class=\"AddChild\" title=\"Add a child link\">+</a>");
            //    if (link.ChildNavigations.Count > 0)
            //    {
            //        BuildNavigationTreeRecursively(link, builder);
            //    }
            //    builder.Append("</li>");
            //}
            //builder.Append("</ul>");
        }

        /// <summary>
        /// Reorder navigation under a parent so that they are in order
        /// </summary>
        /// <param name="parentId">The id of the parent link</param>
        /// <param name="navigationIds">All of the link ids on that level in order</param>
        [FunctionFilter("Sites-Site Map", "~/Sites/Overview")]
        public void MoveNavigation(int parentId, List<int> navigationIds)
        {
            throw EntityExceptionHelper.GetAndLogNetStepsException("Finish porting this code - JHE");
            //for (short i = 0; i < navigationIds.Count; i++)
            //{
            //    bool changed = false;
            //    Navigation link = CurrentSite.SiteNavigations[NavigationTypeId][navigationIds[i]];
            //    if (link.SortIndex != i)
            //    {
            //        link.SortIndex = i;
            //        changed = true;
            //    }
            //    if (link.ParentID != parentId)
            //    {
            //        link.ParentID = parentId;
            //        changed = true;
            //    }
            //    if (changed)
            //    {
            //        link.Save();
            //    }
            //}
        }

        /// <summary>
        /// Get a single navigation element to display
        /// </summary>
        /// <param name="navigationId">The id of the navigation to display</param>
        /// <returns></returns>
        public ActionResult GetNavigation(int navigationId)
        {
            throw EntityExceptionHelper.GetAndLogNetStepsException("Finish porting this code - JHE");
            //Navigation link = CurrentSite.Navigation[NavigationTypeId][navigationId];
            //return Json(new
            //{
            //    isInternal = link.PageID != 0,
            //    url = link.LinkUrl,
            //    linkText = link.LinkText,
            //    pageId = link.PageID,
            //    active = link.Active,
            //    parentId = link.ParentID
            //});
        }

        public enum LinkDestination
        {
            ExistingPage = 1,
            ExternalURL = 2
        }

        /// <summary>
        /// Save a navigation link
        /// </summary>
        /// <param name="navigationId">The id of the link</param>
        /// <param name="destination">Whether the link goes to an existing page or to an external url</param>
        /// <param name="linkText">The text of the link</param>
        /// <param name="url">The url of the link</param>
        /// <param name="pageId">The id of the page the link points to</param>
        /// <param name="parentId">The id of the parent of this link</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Site Map", "~/Sites/Overview")]
        public int SaveNavigation(int navigationId, LinkDestination destination, string linkText, string url, int pageId, int parentId, int languageId)
        {
            Navigation link;
            if (navigationId == 0)
            {
                // Create a link as the last link in the list
                throw EntityExceptionHelper.GetAndLogNetStepsException("Finish porting this code - JHE");
                //link = CurrentSite.SiteNavigations[NavigationTypeId].AddNew();
                //link.Active = false;
                //List<Navigation> siblings = Navigation.LoadSingleLevelNav(CurrentSite.SiteID, NavigationTypeId, parentId);
                //link.SortIndex = (short)(siblings.Count > 0 ? siblings.Max(nl => nl.SortIndex) + 1 : 0);
            }
            else
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException("Finish porting this code - JHE");
                //link = CurrentSite.SiteNavigations[NavigationTypeId][navigationId];
            }

            var description = link.Translations.GetByLanguageIdOrDefaultInList(languageId);
            description.LanguageID = languageId;
            description.LinkText = linkText;

            link.LinkUrl = url;
            link.PageID = destination == LinkDestination.ExistingPage ? pageId : 0;
            link.ParentID = parentId;
            link.Save();
            return link.NavigationID;
        }

        /// <summary>
        /// Change the status of the navigation link
        /// </summary>
        /// <param name="navigationId">The id of the link to change</param>
        /// <param name="isActive">Whether the link is active or not</param>
        [FunctionFilter("Sites-Site Map", "~/Sites/Overview")]
        public void ChangeNavigationActiveStatus(int navigationId, bool isActive)
        {
            throw EntityExceptionHelper.GetAndLogNetStepsException("Finish porting this code - JHE");
            //Navigation link = CurrentSite.Navigation[NavigationTypeId][navigationId];
            //link.Active = isActive;
            //link.Save();
        }
        #endregion

        #region Pages
        /// <summary>
        /// Show all of the pages for this site
        /// </summary>
        /// <param name="id">The id of the current site</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Site Pages", "~/Sites/Overview")]
        public ActionResult SitePages(int? id)
        {
            if (id.HasValue && id.Value > 0)
                CurrentSite = Site.Load(id.Value);
            if (CurrentSite == null)
                return RedirectToAction("Index");
            ViewData["ExistingUrls"] = CurrentSite.Pages.Where(p => !string.IsNullOrEmpty(p.Url)).Select(p => p.Url).Distinct().ToJSON();
            return View(CurrentSite.Pages);
        }

        /// <summary>
        /// Gets pages for this site by page
        /// </summary>
        /// <param name="page">The current page</param>
        /// <param name="pageSize">The size of the current page</param>
        /// <returns></returns>
        public ActionResult GetPages(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, bool? active, string name, string url)
        {
            StringBuilder pageBuilder = new StringBuilder();
            int count = 0;
            IEnumerable<Page> pages = CurrentSite.Pages.Where(p => p.ParentID > 0);
            if (active.HasValue)
                pages = pages.Where(p => p.Active == active.Value);
            if (!string.IsNullOrEmpty(name))
                pages = pages.Where(p => p.Name.ContainsIgnoreCase(name));
            if (!string.IsNullOrEmpty(url))
                pages = pages.Where(p => p.Url.ContainsIgnoreCase(url));
            foreach (Page sitePage in pages.Skip(page * pageSize).Take(pageSize))
            {
                int currentEditingLanguageID = CurrentSite.GetDefaultEditingLanguageID();
                pageBuilder.Append("<tr id=\"").Append(sitePage.PageID).Append("\"").Append(count % 2 == 0 ? "" : " class=\"Alt\"").Append("><td><a href=\"")
                    .Append("~/Sites/EditPage/".ResolveUrl()).Append(sitePage.PageID).Append("\" class=\"Edit\">").Append(sitePage.Name).Append("</a></td><td>")
                    .Append(sitePage.GetTitle(currentEditingLanguageID)).Append("</td><td>").Append(sitePage.GetDescription(currentEditingLanguageID)).Append("</td><td>").Append(sitePage.Url)
                    .Append("</td><td>").Append(sitePage.Active ? "Active" : "Inactive")
                    .Append("</td><td>").Append(CurrentSite.Pages.First(template => template.PageID == sitePage.ParentID).Name)
                    .Append("</td></tr>");
            }
            return Json(new { totalPages = Math.Ceiling(CurrentSite.Pages.Where(p => p.ParentID > 0).Count() / (double)pageSize), page = pageBuilder.ToString() });
        }

        public ActionResult ChangePageStatus(List<int> items, bool active)
        {
            try
            {
                foreach (int pageId in items)
                {
                    Page page = CurrentSite.Pages.First(p => p.PageID == pageId);
                    if (page.Active != active)
                    {
                        page.Active = active;
                        page.Save();
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Save a page
        /// </summary>
        /// <param name="pageId">The id of the page</param>
        /// <param name="pageName">The name of the page</param>
        /// <param name="pageTitle">The title of the page</param>
        /// <param name="pageDesc">The description of the page</param>
        /// <param name="pageUrl">The url of the page</param>
        /// <param name="active">Whether the page is active or not</param>
        /// <param name="keywords">The keywords of the page</param>
        /// <param name="parentId">The id of the parent of the page</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Site Pages", "~/Sites/Overview")]
        public ActionResult SavePage(int pageId, int languageId, string pageName, string pageTitle, string pageDesc, string pageUrl, bool active, string keywords, int parentId)
        {
            Page page;
            if (pageId > 0)
                page = CurrentSite.Pages[pageId];
            else
            {
                page = new Page();
                page.IsStartPage = false;
                page.Name = pageName;
                CurrentSite.BaseSite.Pages.Add(page);
            }
            page.Name = pageName;

            var description = page.Translations.GetByLanguageIdOrDefaultInList(languageId);
            description.LanguageID = languageId;
            description.Title = pageTitle;
            description.Description = pageDesc;
            description.Keywords = keywords;

            // Make sure we have 1 and only 1 slash at the beginning
            if (!pageUrl.StartsWith("/"))
                pageUrl = "/" + pageUrl;
            page.Url = pageUrl.Replace("//", "/");
            page.Active = active;
            //page.Keywords = keywords;
            page.ParentID = parentId;
            try
            {
                page.Save();
                TempData["SavedPage"] = true;
                return RedirectToAction("SitePages");
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                TempData["Page"] = page;
                TempData["Error"] = exception.PublicMessage;
                return RedirectToAction("EditPage");
            }
        }

        /// <summary>
        /// Show a page to edit
        /// </summary>
        /// <param name="id">The id of the page</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Site Pages", "~/Sites/Overview")]
        public ActionResult EditPage(int? id)
        {
            if (CurrentSite == null)
                return RedirectToAction("Index");
            ViewData["Templates"] = CurrentSite.Pages.Where(p => p.ParentID == 0 && !p.IsStartPage/* Temporary fix for nsHealth to make it so that home page doesn't show */);
            if (id.HasValue && id.Value > 0)
            {
                Page page = CurrentSite.Pages.FirstOrDefault(p => p.PageID == id.Value);
                if (page != null)
                    return View(page);
            }
            else if (TempData["Page"] != null)
                return View(TempData["Page"]);

            return View(new Page());
        }

        public ActionResult GetPageDescription(int? pageId, int languageId)
        {
            if (!pageId.HasValue)
                return Json(new { result = true, name = "", shortDescription = "", longDescription = "" });

            Page page = CurrentSite.Pages.FirstOrDefault(p => p.PageID == pageId.Value);
            var description = page.Translations.GetByLanguageIdOrDefault(languageId);

            return Json(new { result = true, title = description.Title, description = description.Description, keywords = description.Keywords });
        }
        #endregion

        #region News and Announements
        public SiteNewsModel CurrentNews
        {
            get
            {
                if (Session["CurrentNews"] == null || ((Session["CurrentNews"] as SiteNewsModel).News.Count > 0 && (Session["CurrentNews"] as SiteNewsModel).SiteID != CurrentSite.SiteID))
                {
                    Session["CurrentNews"] = new SiteNewsModel()
                    {
                        News = NetSteps.Data.Entities.News.LoadByDateRange(CurrentSite.SiteID),
                        SiteID = CurrentSite.SiteID
                    };
                }
                return Session["CurrentNews"] as SiteNewsModel;
            }
            set { Session["CurrentNews"] = value; }
        }

        /// <summary>
        /// Show the news for the current site
        /// </summary>
        /// <param name="id">The id of the current site</param>
        /// <returns></returns>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        public ActionResult News(int? id)
        {
            if (id.HasValue && id.Value > 0)
                CurrentSite = Site.Load(id.Value);
            if (CurrentSite == null)
                return RedirectToAction("Index");
            ViewData["NewsTypes"] = SmallCollectionCache.Instance.NewsTypes.ToIListValueList(); // ListValue.GetValues(ListTypes.NewsType, 1, CoreContext.CurrentMarketId);
            return View(CurrentNews.News);
        }

        /// <summary>
        /// Batch operation to change the active status of a list of news
        /// </summary>
        /// <param name="items">A list of ids of all the news to change</param>
        /// <param name="active">Whether the news is active or not</param>
        /// <returns></returns>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        public ActionResult ChangeNewsStatus(List<int> items, bool active)
        {
            try
            {
                foreach (int newsId in items)
                {
                    News news = NetSteps.Data.Entities.News.Load(newsId);
                    if (news.Active != active)
                    {
                        news.Active = active;
                        news.Save();
                    }
                }
                CurrentNews = null;
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Batch operation to delete news
        /// </summary>
        /// <param name="newsToDelete">A list of ids of news to delete</param>
        /// <returns></returns>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        public ActionResult DeleteNews(List<int> newsToDelete)
        {
            try
            {
                foreach (int newsId in newsToDelete)
                {
                    NetSteps.Data.Entities.News.Delete(newsId);
                    CurrentNews = null;
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        /// Save news
        /// </summary>
        /// <param name="newsId">The id of the news</param>
        /// <param name="title">The title of the news</param>
        /// <param name="type">The type of news</param>
        /// <param name="date">The date the news was posted</param>
        /// <param name="isActive">Whether the news is active or not</param>
        /// <param name="isPublic">Whether the news is public or not</param>
        /// <param name="caption">The caption to put in the content of the news</param>
        /// <param name="body">The body of the content of the news</param>
        /// <returns></returns>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        [ValidateInput(false)]
        public ActionResult SaveNews(int? newsId, string title, int type, DateTime date, bool isActive, bool isPublic, string caption, string body)
        {
            News news;
            if (newsId.HasValue && newsId.Value > 0)
            {
                news = CurrentSite.News.FirstOrDefault(n => n.NewsID == newsId.Value);
            }
            else
            {
                news = new NetSteps.Data.Entities.News();
                CurrentSite.News.Add(news);
            }
            throw new NotImplementedException("Finish porting this functionality if needed. - JHE");
            //news.Title = title;
            news.NewsTypeID = type.ToShort();
            news.StartDate = date;
            news.Active = isActive;
            news.IsPublic = isPublic;

            //Create content if there is a caption or body
            if (!string.IsNullOrEmpty(caption) || !string.IsNullOrEmpty(body))
            {
                throw new NotImplementedException("Finish porting this functionality if needed. - JHE");
                //HtmlContent newsContent = news.HtmlContent ?? new HtmlContent();
                //HtmlBuilder builder = new HtmlBuilder("");
                //if (!string.IsNullOrEmpty(caption))
                //    builder.AppendCaption(caption);
                //if (!string.IsNullOrEmpty(body))
                //    builder.AppendBody(body);


                ////newsContent.Html = builder.ToString();

                //if (news.HtmlContent == null)
                //{
                //    news.HtmlContent = newsContent;
                //}
            }
            try
            {
                news.Save();
                TempData["SavedNews"] = true;

                CurrentNews = null;

                return RedirectToAction("News");
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                TempData["Error"] = exception.PublicMessage;
                TempData["News"] = news;
                return RedirectToAction("EditNews");
            }
        }

        /// <summary>
        /// Get news by page
        /// </summary>
        /// <param name="page">The current page</param>
        /// <param name="pageSize">The size of the current page</param>
        /// <param name="orderBy">The column to order by</param>
        /// <param name="orderByDirection">The direction of the sort</param>
        /// <returns></returns>
        public ActionResult GetNews(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, bool? active, int? type, string title)
        {
            StringBuilder builder = new StringBuilder();
            string cssClass = "GridRow";
            IEnumerable<IListValue> newsTypes = SmallCollectionCache.Instance.NewsTypes.ToIListValueList(); // ListValue.GetValues(ListTypes.NewsType, 1, CoreContext.CurrentMarketId);
            IEnumerable<News> filteredNews = CurrentNews.News;
            if (active.HasValue)
            {
                filteredNews = filteredNews.Where(n => n.Active == active.Value);
            }
            if (type.HasValue)
            {
                filteredNews = filteredNews.Where(n => n.NewsTypeID == type.Value);
            }
            if (!string.IsNullOrEmpty(title))
            {
                throw new NotImplementedException("Finish porting this functionality if needed. - JHE");
                //filteredNews = filteredNews.Where(n => n.Title.Contains(title));
            }
            filteredNews = orderByDirection == NetSteps.Common.Constants.SortDirection.Ascending ? CurrentNews.News.OrderBy(orderBy) : CurrentNews.News.OrderByDescending(orderBy);
            foreach (News news in filteredNews.Skip(page * pageSize).Take(pageSize))
            {
                throw new NotImplementedException("Finish porting this functionality if needed. - JHE");
                //builder.Append("<tr id=\"news").Append(news.NewsID).Append("\" class=\"").Append(cssClass).Append("\"><td><input type=\"checkbox\" class=\"newsSelector\" /></td><td><a href=\"").Append("~/Sites/EditNews/".ResolveUrl()).Append(news.NewsID).Append("\" class=\"dLink EditRow\">").Append(news.Title).Append("</a></td><td>");
                //IListValue newsType = newsTypes.FirstOrDefault(nt => nt.ID == news.NewsTypeID);
                //if (newsType != default(IListValue))
                //{
                //    builder.Append(newsType.Title);
                //}
                //builder.Append("</td><td>").Append(news.StartDate.ToString("g")).Append("</td><td>").Append(news.EndDate.HasValue ? news.EndDate.Value.ToString("g") : "N/A").Append("</td><td>").Append(news.Active ? "Active" : "Inactive").Append("</td></tr>");
                //cssClass = cssClass == "GridRow" ? "GridRowAlt" : "GridRow";
            }
            return Json(new { totalPages = Math.Ceiling(CurrentNews.News.Count / (double)pageSize), page = builder.ToString() });
        }

        /// <summary>
        /// Show a news item to edit
        /// </summary>
        /// <param name="id">The id of the news to edit</param>
        /// <returns></returns>
        [FunctionFilter("Sites-News", "~/Sites/Overview")]
        public ActionResult EditNews(int? id)
        {
            if (CurrentSite == null)
                return RedirectToAction("Index");
            ViewData["NewsTypes"] = SmallCollectionCache.Instance.NewsTypes.ToIListValueList(); // ListValue.GetValues(ListTypes.NewsType, 1, CoreContext.CurrentMarketId);
            News news = null;
            if (id.HasValue)
            {
                if (id.Value > 0)
                    news = NetSteps.Data.Entities.News.LoadFull(id.Value);
                else
                    news = new News();
            }
            else if (TempData["News"] != null)
                news = TempData["News"] as News;
            if (news == null)
                news = new News();
            return View(news);
        }
        #endregion

        #region Calendar Events
        /// <summary>
        /// Show the events for the current site
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [FunctionFilter("Sites-Calendar Events", "~/Sites/Overview")]
        public ActionResult CalendarEvents(int? id)
        {
            if (id.HasValue && id.Value > 0)
                CurrentSite = Site.Load(id.Value);
            if (CurrentSite == null)
                return RedirectToAction("Index");
            return View();
        }

        /// <summary>
        /// Get events between 2 dates
        /// </summary>
        /// <param name="start">The unix timestamp to start from</param>
        /// <param name="end">The unix timestamp to end at</param>
        /// <returns></returns>
        public ActionResult GetEvents(ulong start, ulong end)
        {
            throw new NotImplementedException("Finish porting this functionality if needed. - JHE");
            ////Convert the timestamps to datetimes
            //DateTime startDate = new DateTime(1970, 1, 1).AddSeconds(start);
            //DateTime endDate = new DateTime(1970, 1, 1).AddSeconds(end);
            //var events = CalendarEvent.LoadByDateRange(startDate, endDate, 0, CurrentSite.DefaultLanguageID.ToInt(), CoreContext.CurrentMarketId, true);
            //return Json(events.Select(e => new { id = e.CalendarEventID, title = e.Subject, allDayEvent = e.IsAllDayEvent, start = (e.StartDate.LocalToUTC() - new DateTime(1970, 1, 1)).TotalSeconds, end = (e.EndDate.LocalToUTC() - new DateTime(1970, 1, 1)).TotalSeconds, url = ("~/Sites/EditEvent/" + e.CalendarEventID).ResolveUrl() }));
        }

        /// <summary>
        /// Show an event to edit
        /// </summary>
        /// <param name="id">The id of the event to edit</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Calendar Events", "~/Sites/Overview")]
        public ActionResult EditEvent(int? id)
        {
            if (CurrentSite == null)
                return RedirectToAction("Index");
            ViewData["CalendarEventTypes"] = AccountListValue.LoadCorporateListValuesByType(NetSteps.Data.Entities.Constants.ListValueType.CalendarEventType.ToInt());
            ViewData["CalendarPriority"] = AccountListValue.LoadCorporateListValuesByType(NetSteps.Data.Entities.Constants.ListValueType.CalendarPriority.ToInt());
            ViewData["States"] = SmallCollectionCache.Instance.StateProvinces.GetByMarketID(CoreContext.CurrentMarketId);
            if (id.HasValue)
            {
                if (id.Value > 0)
                    return View(CalendarEvent.LoadFull(id.Value));
                else if (id.Value == 0)
                    return View(new CalendarEvent() { Address = new Address() });
            }
            else if (TempData["Event"] != null)
                return View(TempData["Event"]);

            return View(new CalendarEvent() { Address = new Address() });
        }

        /// <summary>
        /// Save an event
        /// </summary>
        /// <param name="eventId">The id of the event</param>
        /// <param name="subject">The subject (title) of the event</param>
        /// <param name="type">The type of event</param>
        /// <param name="startDate">The date component of the starting date of the event</param>
        /// <param name="startTime">The time component of the starting date of the event</param>
        /// <param name="endDate">The date component of the ending date of the event</param>
        /// <param name="endTime">The time component of the ending date of the event</param>
        /// <param name="priority">The priority of the event</param>
        /// <param name="isPublic">Whether the event is public or not</param>
        /// <param name="state">The state to show this news for</param>
        /// <param name="caption">The caption of the content of the event</param>
        /// <param name="body">The body of the content of the event</param>
        /// <returns></returns>
        [ValidateInput(false)]
        [FunctionFilter("Sites-Calendar Events", "~/Sites/Overview")]
        public ActionResult SaveEvent(int? eventId, string subject, int type, DateTime startDate, DateTime? startTime, DateTime endDate, DateTime? endTime, CalendarEvent.Priority priority, bool isPublic, int? state, string caption, string body)
        {
            CalendarEvent calendarEvent;
            if (eventId.HasValue && eventId.Value > 0)
            {
                calendarEvent = CalendarEvent.Load(eventId.Value);
            }
            else
            {
                calendarEvent = new CalendarEvent();
                CurrentSite.CalendarEvents.Add(calendarEvent);
            }

            throw new NotImplementedException("Finish porting this functionality if needed. - JHE");
            //calendarEvent.Subject = subject;
            calendarEvent.CalendarEventTypeID = type;
            // Build the full datetimes from the individual components
            if (startTime.HasValue)
                calendarEvent.StartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, startTime.Value.Hour, startTime.Value.Minute, startTime.Value.Second);
            else
                calendarEvent.StartDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 1);
            if (endTime.HasValue)
                calendarEvent.EndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, endTime.Value.Hour, endTime.Value.Minute, endTime.Value.Second);
            else
                calendarEvent.EndDate = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
            calendarEvent.StartDate = calendarEvent.StartDate.LocalToUTC();
            calendarEvent.EndDate = calendarEvent.EndDate.LocalToUTC();
            calendarEvent.ReminderDate = new DateTime(1900, 1, 1);
            //calendarEvent.Description = "";
            calendarEvent.CalendarPriorityID = priority.ToInt();
            calendarEvent.IsPublic = isPublic;
            calendarEvent.Address.StateProvinceID = state.HasValue ? state.Value : 0;
            calendarEvent.AccountID = 0;

            // Create an HTMLContent if there is a caption or body
            if (!string.IsNullOrEmpty(caption) || !string.IsNullOrEmpty(body))
            {
                throw new NotImplementedException("Finish porting this functionality if needed. - JHE");
                //HtmlContent content = calendarEvent.HtmlContent ?? new HtmlContent();
                //HtmlBuilder builder = new HtmlBuilder("");
                //if (!string.IsNullOrEmpty(caption))
                //    builder.AppendCaption(caption);
                //if (!string.IsNullOrEmpty(body))
                //    builder.AppendBody(body);


                ////content.Html = builder.ToString();

                //if (calendarEvent.HtmlContent == null)
                //{
                //    calendarEvent.HtmlContent = content;
                //}
            }

            try
            {
                if (eventId.HasValue && eventId.Value > 0)
                    calendarEvent.Save();
                else
                    CurrentSite.Save();
                TempData["SavedEvent"] = true;
                return RedirectToAction("CalendarEvents");
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                TempData["Error"] = exception.PublicMessage;
                TempData["Event"] = calendarEvent;
                return RedirectToAction("EditEvent");
            }
        }
        #endregion

        #region Term Translations
        /// <summary>
        /// Show the terms to translate
        /// </summary>
        /// <returns></returns>
        [FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
        public ActionResult TermTranslations()
        {
            ViewData["Languages"] = TermTranslation.GetLanguages(NetSteps.Data.Entities.Constants.Language.English.ToInt());
            return View();
        }

        public enum TermType
        {
            OutOfDate,
            Untranslated
        }

        /// <summary>
        /// Get terms by page and language
        /// </summary>
        /// <param name="page">The current page</param>
        /// <param name="pageSize">The size of the current page</param>
        /// <param name="languageId">The id of the language to show terms for</param>
        /// <param name="type">If specified, determine whether to show out of date terms or untranslated terms</param>
        /// <param name="term">Possibly a part of the term name, English term, or local term to search for</param>
        /// <returns></returns>
        public ActionResult GetTerms(int? page, int? pageSize, int languageId, TermType? type, string term)
        {
            // Get all of the English terms and their local translation
            var terms = CurrentApplicationCache.TermTranslations.EnglishTerms.Distinct((t1, t2) => t1.TermName == t2.TermName).Select(et =>
            {
                var localTerm = CurrentApplicationCache.TermTranslations.AllTerms.FirstOrDefault(t => t.LanguageID == languageId && t.TermName == et.TermName);
                return new
                {
                    TermName = et.TermName,
                    EnglishTerm = et.Term,
                    LocalTerm = localTerm == default(TermTranslation) ? null : localTerm.Term,
                    IsOutOfDate = localTerm == default(TermTranslation) ? false : et.LastUpdated > localTerm.LastUpdated
                };
            });

            // Filter out all of the terms that don't match the term we're searching for
            if (!string.IsNullOrWhiteSpace(term))
                terms = terms.Where(t => t.TermName.ContainsIgnoreCase(term) || t.EnglishTerm.ContainsIgnoreCase(term) || t.LocalTerm.ContainsIgnoreCase(term)).ToList();

            // Only show untranslated or out of date terms if specified
            if (type.HasValue)
            {
                switch (type.Value)
                {
                    case TermType.OutOfDate: terms = terms.Where(t => t.IsOutOfDate); break;
                    case TermType.Untranslated: terms = terms.Where(t => string.IsNullOrEmpty(t.LocalTerm)); break;
                }
            }
            int maxPage = pageSize.HasValue ? (terms.Count() / pageSize.Value) - 1 : 0;
            if (page.HasValue && pageSize.HasValue)
                terms = terms.Skip(page.Value * pageSize.Value).Take(pageSize.Value);

            // Return the html of all of the terms
            if (terms.Count() > 0)
            {
                return Json(new
                {
                    maxPage = maxPage,
                    terms = terms.ToString((t, i) =>
                        {
                            StringBuilder builder = new StringBuilder("<tr class=\"GridRow");
                            if (i % 2 == 1)
                                builder.Append(" Alt");
                            builder.Append("\"><td>");
                            if (t.IsOutOfDate)
                                builder.Append("<img src=\"").Append("~/Content/Images/Icons/expired.gif".ResolveUrl()).Append("\" alt=\"Out of date\" title=\"This term is out of date.\" />");
                            if (string.IsNullOrEmpty(t.LocalTerm))
                                builder.Append("<img src=\"").Append("~/Content/Images/Icons/notranslation.gif".ResolveUrl()).Append("\" alt=\"Untranslated\" title=\"This term has not been translated into this language yet.\" />");
                            return builder.Append("</td><td class=\"termName\" style=\"width: 175px;\">").Append(t.TermName).Append("</td><td style=\"width: 175px;\">").Append(t.EnglishTerm).Append("</td><td><textarea class=\"localTerm\" style=\"width: 100%\" rows=\"2\">").Append(t.LocalTerm).Append("</textarea></td></tr>").ToString();
                        })
                });
            }
            return Json(new { maxPage = 0, terms = "<tr><td colspan=\"3\">There are no terms that match that criteria</td></tr>" });
        }

        /// <summary>
        /// Save all of the modified terms from the UI
        /// </summary>
        /// <param name="languageId">The id of the language we are editing</param>
        /// <param name="terms">The term name and the local term</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
        public ActionResult SaveTerms(int languageId, Dictionary<string, string> terms)
        {
            try
            {
                foreach (KeyValuePair<string, string> term in terms)
                {
                    // Look for the translated term
                    TermTranslation translation = CurrentApplicationCache.TermTranslations.AllTerms.FirstOrDefault(t => t.LanguageID == languageId && t.TermName == term.Key);
                    // If the term doesn't exist, create it
                    if (translation == default(TermTranslation))
                    {
                        translation = new TermTranslation()
                        {
                            LanguageID = languageId,
                            TermName = term.Key,
                            Term = term.Value,
                            Active = true,
                            LastUpdated = DateTime.Now
                        };
                        translation.Save();
                    }
                    // Make sure we modified the term before saving it
                    else if (translation.Term != term.Value)
                    {
                        translation.Term = term.Value;
                        translation.LastUpdated = DateTime.Now;
                        translation.Save();
                    }
                }

                //Refresh the terms to make sure we have the newest
                CurrentApplicationCache.TermTranslations.ExpireEnglishTermsDictionary();
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        /// <summary>
        ///	Export the terms to Excel via a .csv file
        /// </summary>
        /// <param name="languageId">The id of the language to export</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
        public ActionResult ExportTerms(int languageId)
        {
            // Get all of the English terms and their local translation
            var terms = CurrentApplicationCache.TermTranslations.EnglishTerms.Distinct((t1, t2) => t1.TermName == t2.TermName).Select(et =>
            {
                var localTerm = CurrentApplicationCache.TermTranslations.AllTerms.FirstOrDefault(t => t.LanguageID == languageId && t.TermName == et.TermName);
                return new Term
                {
                    TermName = et.TermName,
                    EnglishTerm = et.Term,
                    LocalTerm = localTerm == default(TermTranslation) ? null : localTerm.Term,
                    LanguageId = languageId,
                    LastUpdated = localTerm == default(TermTranslation) ? (DateTime?)null : localTerm.LastUpdated
                };
            });

            //List<Language> languages = new Language(1).Languages;
            List<Language> languages = TermTranslation.GetLanguages(NetSteps.Data.Entities.Constants.Language.English.ToInt());

            // Build out the CSV
            return new CSVResult<Term>("Terms-" + languages.First(l => l.LanguageID == languageId).Name + ".csv", terms);

            //return typeof(CSVResult<>).MakeGenericType(terms.FirstOrDefault().GetType()).GetConstructor(new Type[] { typeof(string), typeof(IEnumerable<>).MakeGenericType(terms.FirstOrDefault().GetType()) }).Invoke(new object[] { "Terms-" + languages.First(l => l.LanguageId == languageId).EnglishName + ".csv", terms }) as ActionResult;
            //return new ExcelResult<Term>("Terms-" + languages.First(l => l.Id == languageId).EnglishName + ".xls", terms);
        }

        /// <summary>
        /// Import terms from a .csv file
        /// </summary>
        /// <returns></returns>
        [FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
        public ActionResult ImportTerms()
        {
            try
            {
                // Make sure there is a file and that it's a CSV
                if (Request.Files.Count == 0)
                    throw EntityExceptionHelper.GetAndLogNetStepsException("No file uploaded");
                HttpPostedFileBase termFile = Request.Files[0];
                if (!termFile.FileName.EndsWith(".csv"))
                    throw EntityExceptionHelper.GetAndLogNetStepsException("The file must be a CSV");

                Dictionary<Term, TermTranslation> outOfDateTerms = new Dictionary<Term, TermTranslation>();
                using (CsvReader reader = new CsvReader(new StreamReader(termFile.InputStream), true))
                {
                    while (reader.ReadNextRecord())
                    {
                        DateTime lastUpdated;
                        bool validDate = DateTime.TryParse(reader[4], out lastUpdated);

                        Term importedTerm = new Term
                        {
                            TermName = reader[0],
                            EnglishTerm = reader[1],
                            LocalTerm = reader[2],
                            LanguageId = int.Parse(reader[3]),
                            LastUpdated = validDate ? lastUpdated : (DateTime?)null
                        };

                        // Get the term from the db
                        TermTranslation translation = CurrentApplicationCache.TermTranslations.AllTerms.FirstOrDefault(t => t.TermName == importedTerm.TermName && t.LanguageID == importedTerm.LanguageId);

                        // If the term exists, make sure the term is different from the file, and then check if it is out of date with the database (whether the term has been modified since the file was exported)
                        if (translation != default(TermTranslation))
                        {
                            if (importedTerm.LocalTerm != translation.Term)
                            {
                                if (importedTerm.LastUpdated != translation.LastUpdated)
                                {
                                    outOfDateTerms.Add(importedTerm, translation);
                                }
                                else
                                {
                                    translation.Term = importedTerm.LocalTerm;
                                    translation.LastUpdated = DateTime.Now;
                                    translation.Save();
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(importedTerm.LocalTerm))
                            {
                                // Insert the translation
                                translation = new TermTranslation()
                                {
                                    Active = true,
                                    LanguageID = importedTerm.LanguageId,
                                    LastUpdated = DateTime.Now,
                                    Term = importedTerm.LocalTerm,
                                    TermName = importedTerm.TermName
                                };
                                translation.Save();
                            }
                        }
                    }
                    TempData["OutOfDateTerms"] = outOfDateTerms;
                }
                return Content(new { result = true, anyOutOfDateTerms = outOfDateTerms.Count > 0 }.ToJSON());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Content(new { result = false, message = exception.PublicMessage }.ToJSON());
            }
        }

        /// <summary>
        /// Show the merge resolution tool for out of date terms
        /// </summary>
        /// <returns></returns>
        [FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
        public ActionResult MergeTerms()
        {
            return PartialView(TempData["OutOfDateTerms"] as Dictionary<Term, TermTranslation>);
        }

        /// <summary>
        /// Choose which terms to use: the one from the file, or the one from the db
        /// </summary>
        /// <param name="languageId">The id of the language to resolve terms for</param>
        /// <param name="terms">The term name and the term that the user chose</param>
        [FunctionFilter("Sites-Term Translations", "~/Sites/Overview")]
        public void ResolveTerms(int languageId, Dictionary<string, string> terms)
        {
            foreach (KeyValuePair<string, string> term in terms)
            {
                TermTranslation translation = CurrentApplicationCache.TermTranslations.AllTerms.First(t => t.LanguageID == languageId && t.TermName == term.Key);
                translation.Term = term.Value;
                translation.LastUpdated = DateTime.Now;
                translation.Save();
            }
        }
        #endregion

        #region Resource Library

        /// <summary>
        /// Displays the ResourceLibrary view
        /// </summary>
        /// <param name="id">A site id</param>
        /// <returns></returns>
        [FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
        public ActionResult ResourceLibrary(int? id)
        {
            if (id.HasValue && id.Value > 0)
                CurrentSite = Site.Load(id.Value);
            if (CurrentSite == null)
                return RedirectToAction("Index");

            ViewData["ArchiveCategories"] = Category.LoadFullByCategoryTypeId(Constants.CategoryType.Archive.ToInt());
            return View(CurrentSite.Archives);
        }

        /// <summary>
        /// Populates the grid on the ResourceLibrary view
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="category"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="searchText"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDirection"></param>
        /// <returns></returns>
        public ActionResult GetResources(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, int? category, bool? active, string name)
        {
            PaginatedListParameters pagedListParameters = new PaginatedListParameters()
            {
                PageIndex = page,
                PageSize = pageSize,
                OrderBy = orderBy,
                OrderByDirection = orderByDirection
            };
            var archiveItems = Archive.SearchArchives(category, active, null, null, name, pagedListParameters);
            StringBuilder builder = new StringBuilder();
            string cssClass = "GridRow";
            foreach (ArchiveSearchData item in archiveItems)
            {
                builder.Append("<tr id=\"archive").Append(item.ArchiveID).Append("\" class=\"").Append(cssClass)
                    .Append("\"><td><input type=\"checkbox\" class=\"archiveSelector\" /></td><td><a href=\"").Append("~/Sites/EditResource/".ResolveUrl()).Append(item.ArchiveID)
                    .Append("\">").Append(item.Name).Append("</a></td><td>").Append(item.StartDate == DateTime.MaxValue ? "N/A" : item.StartDate.ToShortDateString())
                    .Append("</td><td>").Append(item.EndDate == DateTime.MaxValue ? "N/A" : item.EndDate.ToShortDateString()).Append("</td><td>")
                    .Append("</td><td>")
                    .Append(item.Active ? "Active" : "Inactive").Append("</td></tr>");
                cssClass = cssClass == "GridRow" ? "GridRowAlt" : "GridRow";
            }
            return Json(new { totalPages = Math.Ceiling(archiveItems.Count / (double)pageSize), page = builder.ToString() });
        }

        /// <summary>
        /// Changes the status on a list of resources, called from the ResourceLibrary View
        /// </summary>
        /// <param name="items"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        [FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
        public ActionResult ChangeResourceStatus(List<int> items, bool active)
        {
            try
            {
                Archive item;
                foreach (int itemID in items)
                {
                    item = Archive.Load(itemID);
                    if (item.Active != active)
                    {
                        item.Active = active;
                        item.Save();
                    }
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }

        /// <summary>
        /// Changes the category on a list of resources, called from the ResourceLibrary View
        /// </summary>
        /// <param name="items"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        [FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
        public ActionResult ChangeResourceCategory(List<int> items, int categoryId)
        {
            try
            {
                Archive item;
                foreach (int itemID in items)
                {
                    item = Archive.Load(itemID);
                    item.Save();
                }
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }

        }

        /// <summary>
        /// Displays the EditResource view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
        public ActionResult EditResource(int? id)
        {
            string imagesWebPath = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ImagesWebPath);
            if (imagesWebPath.Substring(imagesWebPath.Length - 1) == "/")
                ViewData["ArchiveWebPath"] = imagesWebPath.Substring(0, imagesWebPath.Length - 1);
            else
                ViewData["ArchiveWebPath"] = imagesWebPath;
            Archive item;
            if (id == null)
            {
                if (TempData["Resource"] != null)
                    item = TempData["Resource"] as Archive;
                else
                    item = new Archive()
                    {
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today.AddYears(60),
                        ArchiveDate = DateTime.Today,
                        //SiteId = CurrentSite.SiteID,
                        Active = true,
                        ArchivePath = ViewData["ArchiveWebPath"].ToString()
                    };
            }
            else
                item = Archive.Load(id.ToInt());
            ViewData["ArchiveRelativePath"] = ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.ImagesRelativePath);
            return View(item);
        }

        /// <summary>
        /// Saves changes to a resource, called from the EditResource view
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
        public ActionResult SaveResource(int? archiveId, string name, string description, string filePath, int category, DateTime startDate, DateTime endDate, bool active, bool isDownloadable, bool canBeEmailed)
        {
            Archive archive;
            if (archiveId.HasValue && archiveId.Value > 0)
            {
                archive = Archive.Load(archiveId.Value);
            }
            else
            {
                archive = new Archive();
                CurrentSite.Archives.Add(archive);
            }
            archive.Name = name;
            archive.Description = description;
            archive.ArchivePath = filePath;
            archive.StartDate = startDate;
            archive.EndDate = endDate;
            archive.Active = active;
            archive.IsDownloadable = isDownloadable;
            archive.IsEmailable = canBeEmailed;
            archive.ArchiveDate = DateTime.Now;
            try
            {
                CurrentSite.Save();
                TempData["SavedResource"] = true;
                return RedirectToAction("ResourceLibrary");
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                TempData["Error"] = exception.PublicMessage;
                TempData["Resource"] = archive;
                return RedirectToAction("EditResource");
            }
        }

        /// <summary>
        /// Displays the EditCategory view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
        public ActionResult EditResourceCategory(int? id)
        {
            throw new Exception("TODO: Finish implementing this or remove it in vue of the new Category Tree for Archives - JHE");
            //ArchiveCategory category;
            //if (!id.HasValue || id.Value == 0)
            //    category = new ArchiveCategory();
            //else
            //    category = SmallCollectionCache.Instance.ArchiveCategories.GetById(id.ToShort());
            //TempData["urlReferrer"] = Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : "/Sites/ResourceLibrary";
            //return View(category);
        }

        /// <summary>
        /// Saves changes to a category, called from the EditCategory view
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
        public ActionResult SaveResourceCategory(int id, string title)
        {
            throw new Exception("TODO: Finish implementing this or remove it in vue of the new Category Tree for Archives - JHE");
            //ArchiveCategory category;
            //if (id == 0)
            //    category = new ArchiveCategory();
            //else
            //    category = ArchiveCategory.Load(id.ToShort());
            //category.Name = title;
            //category.Active = true;
            //category.Editable = true;

            //try
            //{
            //    category.Save();
            //    TempData["SavedCategory"] = true;
            //    //return RedirectToAction("ResourceLibrary");
            //    return RedirectToAction("ResourceCategories");
            //}
            //catch (Exception ex)
            //{
            //    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            //    TempData["Error"] = exception.PublicMessage;
            //    TempData["Category"] = category;
            //    return View(category);
            //}
        }

        /// <summary>
        /// Display the resource categories view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
        public ActionResult ResourceCategories(int? id)
        {
            throw new Exception("TODO: Finish implementing this or remove it in vue of the new Category Tree for Archives - JHE");
            //if (id.HasValue && id.Value > 0)
            //    CurrentSite = Site.Load(id.Value);
            //if (CurrentSite == null)
            //    return RedirectToAction("Index");
            //return View(SmallCollectionCache.Instance.ArchiveCategories.ToIListValueList()); //.ListValue.GetValues(ListTypes.ArchiveType, CurrentSite.DefaultLanguageID, CoreContext.CurrentMarketId));
        }

        /// <summary>
        /// Get a list of resource categories, called from the resource categories view
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchText"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderByDirection"></param>
        /// <returns></returns>
        public ActionResult GetResourceCategories(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string name)
        {
            throw new Exception("TODO: Finish implementing this or remove it in vue of the new Category Tree for Archives - JHE");
            //IList<IListValue> categories = SmallCollectionCache.Instance.ArchiveCategories.ToIListValueList(); // ListValue.GetValues(ListTypes.ArchiveType, CurrentSite.DefaultLanguageID, CoreContext.CurrentMarketId);

            //if (!string.IsNullOrEmpty(name))
            //    categories = categories.Where(c => c.Title.Contains(name)).ToList();

            //int resultCount = categories.Count;

            //StringBuilder builder = new StringBuilder();
            //string cssClass = "GridRow";
            //foreach (IListValue category in categories.OrderBy(c => c.Title).Skip(page * pageSize).Take(pageSize))
            //{
            //    builder.Append("<tr id=\"category").Append(category.ID).Append("\" class=\"").Append(cssClass).Append("\">")
            //        .Append("<td><input type=\"checkbox\" class=\"categorySelector\" /></td>")
            //        .Append("<td><a href=\"").Append("~/Sites/EditResourceCategory/".ResolveUrl()).Append(category.ID).Append("\">").Append(category.Title).Append("</a></td>")
            //        .Append("</tr>");
            //    cssClass = cssClass == "GridRow" ? "GridRowAlt" : "GridRow";
            //}

            //return Json(new { totalPages = Math.Ceiling(categories.Count / (double)pageSize), page = builder.ToString() });
        }

        /// <summary>
        /// Deletes categories, called from the resource categories view
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        [FunctionFilter("Sites-Resource Library", "~/Sites/Overview")]
        public ActionResult DeleteCategories(List<int> items)
        {
            throw new Exception("TODO: Finish implementing this or remove it in vue of the new Category Tree for Archives - JHE");
            //bool result = true;
            //string message = string.Empty;

            //try
            //{
            //    foreach (int itemID in items)
            //    {
            //        ArchiveCategory.Delete(itemID.ToShort());
            //    }
            //}
            //catch (Exception ex)
            //{
            //    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            //    result = false;
            //    message = exception.PublicMessage;
            //}

            //return Json(new { result = result, message = message });
        }

        #endregion Resource Library
    }
}