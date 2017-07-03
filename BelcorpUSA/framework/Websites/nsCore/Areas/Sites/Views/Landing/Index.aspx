<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="NetSteps.Common.EldResolver" %>
<asp:Content ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var isEditor = CoreContext.CurrentUser.HasFunction("CMS-Content Editing");
        var isApprover = CoreContext.CurrentUser.HasFunction("CMS-Content Approving");
        var isPusher = CoreContext.CurrentUser.HasFunction("CMS-Content Pushing");
        var canEdit = isEditor || isApprover || isPusher;
    %>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Sites") %></h2>
    </div>
    <div>
        <% var sites = (ViewData["Sites"] as List<Site>).Where(s => (CoreContext.CurrentUser as CorporateUser).HasAccessToSite(s.SiteID));
           var corporateSites = sites.Where(s => s.SiteTypeID == Constants.SiteType.Corporate.ToInt());
           if (corporateSites.Count() > 0)
           { %>
        <div class="nsCorporate FL sitesLandingMenu">
            <h3>
                <%= Html.Term("nsCorporate") %></h3>
            <div>
                <ul>
                    <% foreach (Site site in corporateSites)
                       { %>
                    <li>
                        <div class="UI-lightBg pad10 brdrAll">
                            <a href="<%= ResolveUrl("~/Sites/Overview/Index/") + site.SiteID %>" class="FL">
                                <b>
                                    <%= site.Name%></b></a>
                            <% string url = null;
                               if (Request.Url.Authority.Contains("localhost") && site.SiteUrls.Any(u => u.Url.Contains("localhost")))
                                   url = site.SiteUrls.First(u => u.Url.Contains("localhost")).Url;
                               if (string.IsNullOrEmpty(url) && site.PrimaryUrl != null)
                                   url = site.PrimaryUrl.Url;
                               if (!string.IsNullOrEmpty(url))
                               {
                                   url = url.EldEncode();

                                   if (CoreContext.CurrentUser != null && CoreContext.CurrentUser is CorporateUser && canEdit)
                                   {  %>
                            <a target="_blank" rel="external" href="<%= url.AppendForwardSlash() %>Login?sso=<%= Server.UrlEncode(CorporateUser.GetSingleSignOnToken((CoreContext.CurrentUser as CorporateUser).CorporateUserID)) %>"
                                class="FR DTL EditSite">
                                <%= Html.Term("LoadSiteInEditMode", "Load Site in Edit Mode")%></a>
                            <%}
              else
              {%>
                            <a target="_blank" rel="external" href="<%= url %>" class="FR DTL EditSite">
                                <%= Html.Term("LoadSite", "Load Site")%></a>
                            <%}
          }%>
                            <span class="clr"></span>
                        </div>
                        <div class="pad5">
                            <ul class="flatList listNav">
                                <%= Html.SelectedLink("~/Sites/Edit/Index/" + site.SiteID, Html.Term("EditSiteDetails", "Edit Site Details"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Create and Edit Base Site")%>
                                <%--<%= Html.SelectedLink("~/Sites/ChildSites/Index/" + site.SiteID, Html.Term("ChildSites", "Child Sites"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Replicated Sites")%>--%>
                                <%= Html.SelectedLink("~/Sites/Pages/Index/" + site.SiteID, Html.Term("SitePages", "Site Pages"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Site Pages")%>
                                <%= Html.SelectedLink("~/Sites/SiteMap/Index/" + site.SiteID, Html.Term("SiteMap", "Site Map"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Site Map")%>
                                <%= Html.SelectedLink("~/Sites/News/Index/" + site.SiteID, Html.Term("NewsAndAnnouncements", "News & Announcements"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-News")%>
                                <%--<%= Html.SelectedLink("~/Sites/CalendarEvents/Index/" + site.SiteID, Html.Term("CalendarEvents", "Calendar Events"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Calendar Events")%>--%>
                                <%= Html.SelectedLink("~/Sites/Documents/Index/" + site.SiteID, Html.Term("DocumentLibrary", "Document Library"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Resource Library")%>
                                <%--<%= Html.SelectedLink("~/Sites/ReviewContent/" + site.SiteID, Html.Term("PWSContentReview", "PWS Content Review"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-PWS Content Review")%>--%>
                            </ul>
                        </div>
                    </li>
                    <%} %>
                </ul>
                <span class="clr"></span>
            </div>
        </div>
        <%}
     var backOfficeSites = sites.Where(s => s.SiteTypeID == Constants.SiteType.BackOffice.ToInt());
     if (backOfficeSites.Count() > 0)
     { %>
        <div class="nsBackOffice FL sitesLandingMenu">
            <h3>
                <%= Html.Term("DistributorWorkstation", "Distributor Workstation") %></h3>
            <div>
                <ul>
                    <% foreach (Site site in backOfficeSites)
                       { %>
                    <li>
                        <div class="UI-lightBg pad10 brdrAll">
                            <a href="<%= ResolveUrl("~/Sites/Overview/Index/") + site.SiteID %>" title="<%= Html.Term("SiteOverview","Site Overview") %>"
                                class="FL"><b>
                                    <%= site.Name%></b></a>
                            <% string url = null;
                               if (Request.Url.Authority.Contains("localhost") && site.SiteUrls.Any(u => u.Url.Contains("localhost")))
                                   url = site.SiteUrls.First(u => u.Url.Contains("localhost")).Url;
                               if (string.IsNullOrEmpty(url) && site.PrimaryUrl != null)
                                   url = site.PrimaryUrl.Url;
                               if (!string.IsNullOrEmpty(url) && Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute))
                               {
                                   if (!url.Contains("localhost"))
                                       url = url.EldEncode();
                                   if (CoreContext.CurrentUser != null && CoreContext.CurrentUser is CorporateUser && canEdit)
                                   {  %>
                            <a target="_blank" rel="external" href="<%= url.AppendForwardSlash() %>Login?sso=<%= Server.UrlEncode(CorporateUser.GetSingleSignOnToken((CoreContext.CurrentUser as CorporateUser).CorporateUserID)) %>"
                                class="FR DTL EditSite">
                                <%= Html.Term("LoadSiteInEditMode", "Load Site in Edit Mode")%></a>
                            <%}
              else
              {%>
                            <a target="_blank" rel="external" href="<%= url %>" class="FR DTL EditSite">
                                <%= Html.Term("LoadSite", "Load Site")%></a>
                            <%}
          }%>
                            <span class="clr"></span>
                        </div>
                        <div class="pad5">
                            <ul class="flatList listNav">
                                <%= Html.SelectedLink("~/Sites/Edit/Index/" + site.SiteID, Html.Term("EditSiteDetails", "Edit Site Details"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Create and Edit Base Site")%>
                                <%--<%= Html.SelectedLink("~/Sites/ChildSites/Index/" + site.SiteID, Html.Term("ChildSites", "Child Sites"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Replicated Sites")%>--%>
                                <%--<%= Html.SelectedLink("~/Sites/Pages/Index/" + site.SiteID, Html.Term("SitePages", "Site Pages"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Site Pages")%>
                                <%= Html.SelectedLink("~/Sites/SiteMap/Index/" + site.SiteID, Html.Term("SiteMap", "Site Map"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Site Map")%>--%>
                                <%= Html.SelectedLink("~/Sites/News/Index/" + site.SiteID, Html.Term("NewsAndAnnouncements", "News & Announcements"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-News")%>
                                <%--<%= Html.SelectedLink("~/Sites/CalendarEvents/Index/" + site.SiteID, Html.Term("CalendarEvents", "Calendar Events"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Calendar Events")%>--%>
                                <%= Html.SelectedLink("~/Sites/Documents/Index/" + site.SiteID, Html.Term("DocumentLibrary", "Document Library"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Resource Library")%>
                                <%--<%= Html.SelectedLink("~/Sites/ReviewContent/" + site.SiteID, Html.Term("PWSContentReview", "PWS Content Review"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-PWS Content Review")%>--%>
                            </ul>
                        </div>
                    </li>
                    <%} %>
                </ul>
                <span class="clr"></span>
            </div>
        </div>
        <%}
     var replicatedSites = sites.Where(s => s.SiteTypeID == Constants.SiteType.Replicated.ToInt());
     if (replicatedSites.Count() > 0)
     { %>
        <div class="nsDistributor FL sitesLandingMenu">
            <h3>
                <%= Html.Term("DistributorPersonalSite", "Distributor Personal Site") %></h3>
            <div>
                <ul>
                    <% foreach (Site site in replicatedSites)
                       { %>
                    <li>
                        <div class="UI-lightBg pad10 brdrAll">
                            <a href="<%= ResolveUrl("~/Sites/Overview/Index/") + site.SiteID %>" class="FL"
                                title="<%= Html.Term("SiteOverview","Site Overview") %>"><b>
                                    <%= site.Name %></b></a>
                            <% string url = null;
                               if (Request.Url.Authority.Contains("localhost") && site.SiteUrls.Any(u => u.Url.Contains("localhost")))
                                   url = site.SiteUrls.First(u => u.Url.Contains("localhost")).Url;
                               if (string.IsNullOrEmpty(url) && site.PrimaryUrl != null)
                                   url = site.PrimaryUrl.Url;
                               if (!string.IsNullOrEmpty(url))
                               {
                                   if (!url.Contains("localhost"))
                                       url = url.EldEncode();
                                   if (CoreContext.CurrentUser != null && CoreContext.CurrentUser is CorporateUser && canEdit)
                                   {  %>
                            <a target="_blank" rel="external" href="<%= url.AppendForwardSlash() %>Login?sso=<%= Server.UrlEncode(CorporateUser.GetSingleSignOnToken((CoreContext.CurrentUser as CorporateUser).CorporateUserID)) %>"
                                class="DTL EditSite FR">
                                <%= Html.Term("LoadSiteInEditMode", "Load Site in Edit Mode")%></a>
                            <%}
              else
              {%>
                            <a target="_blank" rel="external" href="<%= url %>" class="DTL EditSite FR">
                                <%= Html.Term("LoadSite", "Load Site")%></a>
                            <%}
          }%>
                            <span class="clr"></span>
                        </div>
                        <div class="pad5">
                            <ul class="flatList listNav">
                                <%= Html.SelectedLink("~/Sites/Edit/Index/" + site.SiteID, Html.Term("EditSiteDetails", "Edit Site Details"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Create and Edit Base Site")%>
                                <%--<%= Html.SelectedLink("~/Sites/ChildSites/Index/" + site.SiteID, Html.Term("ChildSites", "Child Sites"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Replicated Sites")%>--%>
                                <%= Html.SelectedLink("~/Sites/Pages/Index/" + site.SiteID, Html.Term("SitePages", "Site Pages"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Site Pages")%>
                                <%= Html.SelectedLink("~/Sites/SiteMap/Index/" + site.SiteID, Html.Term("SiteMap", "Site Map"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Site Map")%>
                                <%= Html.SelectedLink("~/Sites/News/Index/" + site.SiteID, Html.Term("NewsAndAnnouncements", "News & Announcements"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-News")%>
                                <%--<%= Html.SelectedLink("~/Sites/CalendarEvents/Index/" + site.SiteID, Html.Term("CalendarEvents", "Calendar Events"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Calendar Events")%>--%>
                                <%= Html.SelectedLink("~/Sites/Documents/Index/" + site.SiteID, Html.Term("DocumentLibrary", "Document Library"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-Resource Library")%>
                                <%--<%= Html.SelectedLink("~/Sites/ReviewContent/" + site.SiteID, Html.Term("PWSContentReview", "PWS Content Review"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Sites-PWS Content Review")%>--%>
                            </ul>
                        </div>
                    </li>
                    <% } %>
                </ul>
                <%if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReplicatedMasterSiteId"]))
                  { %>
                <div class="DropDownUtility">
                    <ul>
                        <li>
                            <%--TODO: get the master site for this type and add it to the query string--%>
                            <a href="<%= ResolveUrl("~/Sites/Edit") %>?isBase=true&baseSiteId=">+ <span>
                                <%= Html.Term("CreateNewBaseSite", "Create new base site")%></span></a></li>
                    </ul>
                </div>
                <%} %>
                <span class="clr"></span>
            </div>
        </div>
        <%} %>
    </div>
</asp:Content>
