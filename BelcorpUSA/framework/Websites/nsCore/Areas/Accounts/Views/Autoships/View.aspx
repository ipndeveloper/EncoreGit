<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
	Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.AutoshipOrder>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>"><%= Html.Term("Accounts", "Accounts")%></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
		<%= ViewBag.CurrentAccount.FullName %></a> > <%= Html.Term("AutoshipOrderHistory", "Autoship Order History")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("AutoshipOrderHistoryFor", "Autoship Order History For")%>
			</h2>
	    <%if (Model.AutoshipSchedule.IsTemplateEditable)
	   { %>
            <a href="<%= ResolveUrl("~/Accounts/Autoships/Edit/" + Model.AutoshipOrderID) %>"><%= Html.Term("EditTemplate", "Edit Template")%></a> | 
		<%} %>
            <%= Html.Term("ViewOrders", "View Orders") %> | 
            <a href="<%= ResolveUrl("~/Accounts/Autoships/AuditHistory/" + Model.AutoshipOrderID) %>"><%= Html.Term("AuditHistory", "Audit History")%></a>
	</div>
	<% Html.PaginatedGrid("~/Accounts/Autoships/Get/" + Model.AutoshipOrderID)
			.AddColumn(Html.Term("ID"), "OrderNumber", true, true, NetSteps.Common.Constants.SortDirection.Ascending)
			.AddColumn(Html.Term("FirstName", "First Name"), "FirstName", true)
			.AddColumn(Html.Term("LastName", "Last Name"), "LastName", true)
			.AddColumn(Html.Term("Status", "Status"), "OrderStatus.TermName", true)
			.AddColumn(Html.Term("Type", "Type"), "OrderType.TermName", true)
			.AddColumn(Html.Term("CompletedOn", "Completed On"), "CompleteDateUTC", true)
			.AddColumn(Html.Term("ShippedOn", "Shipped On"), "DateShippedUTC", false)
			.AddColumn(Html.Term("Subtotal"), "Subtotal", true)
			.AddColumn(Html.Term("GrandTotal"), "GrandTotal", true)
			.AddColumn(Html.Term("CommissionDate", "Commission Date"), "CommissionDateUTC", true)
			.AddColumn(Html.Term("Sponsor"), "Sponsor", true)
			.AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", "1/1/2000", true)
			.AddInputFilter(Html.Term("To"), "endDate", DateTime.Now.ToShortDateString(), true)
			.Render(); %>

	<script type="text/javascript">
		$(function () {
			$('#paginatedGridOptions > *:not(.clearFiltersButton)').remove();
			$('#paginatedGridOptions .clearFiltersButton').unbind('click').click(function () {
				window.location.reload();
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>
