<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Common.Base.PaginatedList<NetSteps.Data.Entities.Business.OrderSearchData>>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>"><%= Html.Term("Accounts", "Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
		<%= CoreContext.CurrentAccount.FullName %></a> > <%= Html.Term("FullOrderHistory", "Full Order History") %>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("OrderHistory", "Order History") %></h2>
		<a href="<%= ResolveUrl("~/Orders/OrderEntry?accountId=") + CoreContext.CurrentAccount.AccountID %>"><%= Html.Term("PlaceNewOrder", "Place New Order") %></a>
	</div>
	<% Html.PaginatedGrid("~/Accounts/OrderHistory/Get")
			.AddColumn(Html.Term("ID"), "OrderNumber", true, true, NetSteps.Common.Constants.SortDirection.Descending)
			.AddColumn(Html.Term("FirstName", "First Name"), "FirstName", true)
			.AddColumn(Html.Term("LastName", "Last Name"), "LastName", true)
			.AddColumn(Html.Term("Status"), "OrderStatus.TermName", true)
			.AddColumn(Html.Term("Type"), "OrderType.TermName", true)
			.AddColumn(Html.Term("CompletedOn", "Completed On"), "CompleteDateUTC", true)
			.AddColumn(Html.Term("ShippedOn", "Shipped On"), "DateShippedUTC", false)
			.AddColumn(Html.Term("Subtotal"), "Subtotal", true)
			.AddColumn(Html.Term("GrandTotal", "Grand Total"), "GrandTotal", true)
			.AddColumn(Html.Term("CommissionDate", "Commission Date"), "CommissionDateUTC", true)
			.AddColumn(Html.Term("Sponsor"), "Sponsor", true)
			.AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(SmallCollectionCache.Instance.OrderStatuses.ToDictionary(os => os.OrderStatusID.ToString(), os => os.GetTerm())))
			.AddSelectFilter(Html.Term("Type"), "type", new Dictionary<string, string>() { { "", Html.Term("SelectaType", "Select a Type...") } }.AddRange(SmallCollectionCache.Instance.OrderTypes.Where(ot => !ot.IsTemplate).ToDictionary(ot => ot.OrderTypeID.ToString(), ot => ot.GetTerm())))
			.AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", "1/1/2000", true)
            .AddInputFilter(Html.Term("To", "To"), "endDate", DateTime.Now.ToShortDateString(), true)
            .AddInputFilter(Html.Term("AccountID"), "AccountID", CoreContext.CurrentAccount.AccountID, true, isHidden: true)
			.Render(); %>
</asp:Content>
