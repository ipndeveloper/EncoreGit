<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master" Inherits="System.Web.Mvc.ViewPage<List<NetSteps.Data.Entities.News>>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Sites") %>">
		<%= Html.Term("Sites") %></a> >
	<%= Html.Term("NewsAndAnnouncements", "News & Announcements") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("NewsAndAnnouncements", "News & Announcements") %></h2>
		<a href="<%= ResolveUrl("~/Sites/News/Edit") %>" class="DTL Add">
			<%= Html.Term("AddNewNews", "Add a new news item") %></a>
	</div>
	<%if (TempData["SavedNews"] != null)
   { %>
	<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous; -moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0; border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold; margin-bottom: 10px; padding: 7px;">
		<div style="color: #385E0F; display: block;">
			<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("NewsSaved", "News saved successfully!") %></div>
	</div>
	<%} %>
	<% Html.PaginatedGrid("~/Sites/News/Get")
		.AddColumn(Html.Term("Title"), "Title", true, true, NetSteps.Common.Constants.SortDirection.Ascending)
		.AddColumn(Html.Term("Type"), "NewsType.TermName", true)
		.AddColumn(Html.Term("StartDate", "Start Date"), "StartDateUTC", true)
		.AddColumn(Html.Term("EndDate", "End Date"), "EndDateUTC", true)
		.AddColumn(Html.Term("Status"), "Active", true)
		.CanChangeStatus(true, true, "~/Sites/News/ChangeStatus")
        .CanDelete("~/Sites/News/Delete")
		.AddSelectFilter(Html.Term("Status"), "active", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") }, { "true", Html.Term("Active") }, { "false", Html.Term("Inactive") } })
		.AddSelectFilter(Html.Term("Type"), "type", new Dictionary<string, string>() { { "", Html.Term("SelectaType", "Select a Type...") } }.AddRange(SmallCollectionCache.Instance.NewsTypes.ToDictionary(nt => nt.NewsTypeID.ToString(), nt => nt.GetTerm())))
		.AddInputFilter(Html.Term("Title"), "title")
		.Render(); %>
</asp:Content>
