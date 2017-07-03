<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Schedules/Schedules.Master" 
Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.PeriodSearchParameters>" %>

<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
	<%= Html.Term("ScheduleManagement", "Schedule Management")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("Periods", "Periods") %>
        </h2>
        <%= Html.Term("BrowsePeriods", "Browse Periods") %> | <a href="<%= ResolveUrl("~/Products/Periods/Edit") %>"><%= Html.Term("CreateaNewPeriod", "Create a New Period") %></a>
	</div>

    <% Html.PaginatedGrid<PeriodSearchData>("~/Products/Periods/GetPeriods")
        .AutoGenerateColumns()
        .AddSelectFilter(Html.Term("Plans"), "planId", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(TempData["Plans"] as Dictionary<string, string>), startingValue: Model.PlanID.ToString())
        .AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", Html.Term("StartDate", "Start Date"), true)
        .AddInputFilter(Html.Term("To", "To"), "endDate", Html.Term("EndDate", "End Date"), true, true)
        .ClickEntireRow()
		.Render(); %>
</asp:Content>
