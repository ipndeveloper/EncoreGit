<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Site>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Sites") %>">
		<%= Html.Term("Sites") %></a> >
	<%= Html.Term("Overview") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
		<%if (TempData["SavedSite"] != null)
	{ %>
		<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous; -moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0; border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold; margin-bottom: 10px; padding: 7px;">
			<div style="color: #385E0F; display: block;">
				<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("SiteSaved", "Site saved successfully!") %></div>
		</div>
		<%} %>
	<div class="LandingModules" style="margin: 0px;">
    gfyuyt
		<table class="ModuleHolder" width="100%" cellspacing="0">
			<tr>
				<td>
					<!-- Overview Module -->    
					<div class="Module">
						<p class="ModTitle">
							<%= Html.Term("News&Announcements", "News &amp; Announcements")%>
						</p>
						<div class="ModUtility">
							<b class="FL"><%= Html.Term("NewsInTheLastCurrentDays", "News in the last") + " " + ViewData["Days"] +" "+ Html.Term("Days","Days")%></b> <a href="<%= ResolveUrl("~/Sites/News/Edit") %>" class="FR"><%= Html.Term("AddNews", "Add News")%></a>
                            <span class="ClearAll"></span>
						</div>
						<div class="ModContent">
							<table class="ModGrid" width="100%" cellspacing="0">
								<tr class="GridColHead">
									<th><%= Html.Term("NewsTitle", "News Title")%></th>
									<th><%= Html.Term("Type", "Type")%></th>
									<th><%= Html.Term("Date", "Date")%></th>
								</tr>
								<% TimeSpan days = new TimeSpan(int.Parse(ConfigurationManager.AppSettings["OverviewDays"]), 0, 0, 0);
									foreach (News news in Model.News.Where(n => n.StartDate >= DateTime.Now.Subtract(days).StartOfDay() && n.StartDate <= DateTime.Now.EndOfDay()).OrderByDescending(n => n.StartDate))
    { %>        
								<tr>
									<td width="50%">
										<a href="<%= ResolveUrl("~/Sites/News/Edit/") + news.NewsID %>">
											<%= news.HtmlSection.ProductionContentForDisplay(ViewData["CurrentSite"] as NetSteps.Data.Entities.Site).FirstOrEmptyElement(ConstantsGenerated.HtmlElementType.Title).Contents%></a>
									</td>
									<td width="50%">
										<%= SmallCollectionCache.Instance.NewsTypes.GetById(news.NewsTypeID).GetTerm() %>
									</td>
									<td>

                                    
										<%= news.StartDate.ToShortDateString() %>
									</td>
								</tr>
								<%} %>
							</table>
						</div>
					</div>
				</td>
				<td>
					<!-- Overview Module -->
					<div class="Module">
						<p class="ModTitle">
							<%= Html.Term("DocumentLibrary", "Document Library")%>
						</p>
						<div class="ModUtility">
							<b class="FL"><%=  Html.Term("DocumentsUploadedInTheLast", "Documents uploaded in the last") + " " + ViewData["Days"] + " " + Html.Term("Days", "Days")%></b> <a href="<%= ResolveUrl("~/Sites/Documents/Edit") %>" class="FR"><%= Html.Term("UploadDocument", "Upload Document")%></a>
                            <span class="ClearAll"></span>
						</div>
						<div class="ModContent">
							<table class="ModGrid" width="100%" cellspacing="0">
								<tr class="GridColHead">
									<th><%= Html.Term("Title", "Title")%></th>
									<th><%= Html.Term("Type", "Type")%></th>
									<th><%= Html.Term("Uploaded On", "Uploaded On")%></th>
								</tr>
								<%foreach (Archive archive in Model.Archives.Where(a => a.ArchiveDate > DateTime.Now.Subtract(days).StartOfDay() && a.ArchiveDate <= DateTime.Now.EndOfDay()).OrderByDescending(a => a.ArchiveDate))
    { %>
								<tr>
									<td width="50%">
									<a href="<%= ResolveUrl("~/Sites/Documents/Edit/") + archive.ArchiveID %>">
										<%= archive.Translations.Name() %>
										</a>
									</td>
									<td width="50%">
										<%= SmallCollectionCache.Instance.ArchiveTypes.GetById(archive.ArchiveTypeID).GetTerm() %>
									</td>
									<td>
										<%= archive.ArchiveDate.ToString() %>
									</td>
								</tr>
								<%} %>
							</table>
						</div>
					</div>
				</td>
			</tr>
		</table>
		<span class="Clear"></span>
	</div>
</asp:Content>
