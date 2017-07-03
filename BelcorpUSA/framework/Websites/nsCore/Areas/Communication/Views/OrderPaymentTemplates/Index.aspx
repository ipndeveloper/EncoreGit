<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<%--<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Communication") %>">
		<%= Html.Term("Communication") %></a> >
	<%= Html.Term("ScheduleCollectionManagement", "Schedule Collection Management")%>
</asp:Content>
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("OrderPaymentTemplates", "Order Payment Overdue")%>
        </h2>
        <%= Html.Term("BrowsePaymentOverdue", "Browse Payment Overdue")%> | <a href="<%= ResolveUrl("~/Communication/OrderPaymentTemplates/Edit") %>"><%= Html.Term("CreateaNewPaymentOverdue", "Create a New Payment Overdue")%></a>
	</div>

    <% Html.PaginatedGrid<OrderPaymentTemplatesSearchData>("~/Communication/OrderPaymentTemplates/Get")
		.AutoGenerateColumns()
        .CanDelete("~/Communication/OrderPaymentTemplates/Delete")
        //.CanChangeStatus(true, true, "~/Communication/OrderPaymentTemplates/ChangeStatus")
        //.ClickEntireRow()
		.Render(); %>
</asp:Content>