<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Schedules/Schedules.Master" 
Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.PlanSearchData>>" %>

<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
	<%= Html.Term("ScheduleManagement", "Schedule Management")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("Plans", "Plans") %>
        </h2>
        <%= Html.Term("BrowsePlans", "Browse Plans") %> | <a href="<%= ResolveUrl("~/Products/Plans/Edit") %>"><%= Html.Term("CreateaNewPlan", "Create a New Plan") %></a>
	</div>

    <% Html.PaginatedGrid<PlanSearchData>("~/Products/Plans/Get")
		.AutoGenerateColumns()
        .AddInputFilter(Html.Term("Name", "Name"), "Name")
        //.ClickEntireRow()
        .CanChangeStatus(true, true, "~/Products/Plans/ChangeStatus")
        //.AddSelectFilter(Html.Term("Status"), "active", new Dictionary<string, string>() { { "", Html.Term("All") }, { "true", Html.Term("Active") }, { "false", Html.Term("Inactive") } })
		.Render(); %>
</asp:Content>