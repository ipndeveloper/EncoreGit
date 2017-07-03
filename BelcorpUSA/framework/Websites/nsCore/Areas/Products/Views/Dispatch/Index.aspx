<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Dispatch/Dispatch.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Dispatch") %>">
		<%= Html.Term("Dispatch") %></a> >
			<%= Html.Term("Dispatch") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2><%= Html.Term("Dispatch") %></h2>
        <%= Html.Term("BrowseDispatch", "Browse Dispatch")%> | <a href="<%= ResolveUrl("~/Products/Dispatch/Create") %>"><%= Html.Term("CreateaNewDispatch", "Create a New Dispatch")%></a>
	</div>
	<div>
		<%if (TempData["SavedDispatch"] != null)
	{ %>
		<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
			-moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0;
			border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold;
			margin-bottom: 10px; padding: 7px;">
			<div style="color: #385E0F; display: block;">
				<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("DispatchSaved", "Dispatch saved successfully!") %></div>
		</div>
		<%} %>

        <%
            Dictionary<string, string> dcStartPeriod = (ViewBag.dcStartPeriod as Dictionary<string, string>) ?? new Dictionary<string, string>();
            Dictionary<string, string> dcCampaniaHasta = (ViewBag.dcCampaniaHasta as Dictionary<string, string>) ?? new Dictionary<string, string>();
        %>

		<% Html.PaginatedGrid("~/Products/Dispatch/Get")
         .AddInputFilter(Html.Term("Description", "Description"), "description")
         .AddSelectFilter(Html.Term("PeriodStart", "Period Start"), "periodStart", new Dictionary<string, string>() { { "190001", Html.Term("SelectPeriod", "Select Period") } }.AddRange(dcStartPeriod))
         .AddSelectFilter(Html.Term("PeriodEnd", "Period End"), "periodEnd", new Dictionary<string, string>() { { "210012", Html.Term("SelectPeriod", "Select Period") } }.AddRange(dcCampaniaHasta))
         .AddInputFilter(Html.Term("SKU", "SKU"), "sku")
            
         .AddColumn(Html.Term("Description"), "Description", true, true, NetSteps.Common.Constants.SortDirection.Ascending)
         .AddColumn(Html.Term("PeriodStart", "Period Start"), "PeriodStart", true)
         .AddColumn(Html.Term("PeriodEnd", "Period End"), "PeriodEnd", true)
         .AddColumn(Html.Term("DateStart", "Start Date"), "DateStart", true)
         .AddColumn(Html.Term("DateEnd", "End Date"), "DateEnd", true)
         .AddColumn(Html.Term("Status", "Status"), "Status", true)
         .AddColumn(Html.Term("ListName", "Dispatch List"), "ListName", true)
		 .CanDelete("~/Products/Dispatch/DeleteDispatch")
         .CanChangeStatus(true, true, "~/Products/Dispatch/ChangeDispatchStatus")
         .ClickEntireRow()
		 .Render(); %>
	</div>
</asp:Content>
