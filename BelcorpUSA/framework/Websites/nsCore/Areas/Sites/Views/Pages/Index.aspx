<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master"
	Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Sites") %>">
		<%= Html.Term("Sites") %></a> >
	<%= Html.Term("Pages") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("SitePages", "Site Pages") %></h2>
		<a href="<%= Page.ResolveUrl("~/Sites/Pages/Edit") %>" class="DTL Add">
			<%= Html.Term("AddaNewPage", "Add a new page") %></a>
	</div>
	<%if (TempData["SavedPage"] != null)
   { %>
	<div id="ReturnSuccessMessage">
		<div id="SuccessMessage">
			<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("PageSaved", "Page saved successfully!") %></div>
	</div>
	<%} %>
	<% Html.PaginatedGrid("~/Sites/Pages/Get")
			.AddColumn(Html.Term("Name"), "Name", true, true, NetSteps.Common.Constants.SortDirection.Ascending)
			.AddColumn(Html.Term("Title"), "Title", true)
			.AddColumn(Html.Term("Description"), "Description", true)
			.AddColumn(Html.Term("URL"), "Url", true)
			.AddColumn(Html.Term("Status"), "Active", true)
			.AddColumn(Html.Term("Template"), "Template", false)
			.AddSelectFilter(Html.Term("Status"), "active", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") }, { "true",  Html.Term("Active") }, { "false",  Html.Term("Inactive") } })
			.AddInputFilter(Html.Term("Name"), "name")
			.AddInputFilter(Html.Term("URL"), "url")
			.CanChangeStatus(true, true, "~/Sites/Pages/ChangeStatus")
			.Render(); %>
</asp:Content>
