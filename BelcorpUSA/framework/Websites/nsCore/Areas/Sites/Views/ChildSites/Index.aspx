<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master"
	Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.SiteSearchData>>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Sites") %>">
		<%= Html.Term("Sites") %></a> >
	<%= Html.Term("ChildSites", "Child Sites")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("ChildSites", "Child Sites") %></h2>
		<%--<a href="<%= ResolveUrl("~/Sites/Edit") %>?isBase=false">Create a New Replicated Site</a>--%>
	</div>
	<%if (TempData["SavedSite"] != null)
   { %>
	<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
		-moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0;
		border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold;
		margin-bottom: 10px; padding: 7px;">
		<div style="color: #385E0F; display: block;">
			<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("SiteSaved", "Site saved successfully!") %></div>
	</div>
	<%} %>
	<% Html.PaginatedGrid<SiteSearchData>("~/Sites/ChildSites/Get")
		.AutoGenerateColumns()
		.AddData("baseSiteId", ViewData["BaseSiteId"])
		.AddSelectFilter(Html.Term("Status"), "active", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") }, { "true", Html.Term("Active") }, { "false", Html.Term("Inactive") } })
		.AddInputFilter(Html.Term("SiteName", "Site Name"), "siteName")
		.AddInputFilter(Html.Term("URL"), "url")
		.Render(); %>
</asp:Content>
