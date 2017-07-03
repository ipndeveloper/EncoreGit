<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/PaymentTicketsSearch.Master" 
Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.GLPaymeTycketsSearchData>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="hdnPaymentStatus" value="<%=  ViewBag.Status %>" />
    <div class="SectionHeader">
		<h2>
			<%=  Html.Term("ControlLog", "Control Log")%>
        </h2>
	</div>
     <% Html.PaginatedGrid<GLControlLogSearchData>("~/GeneralLedger/PaymentTickets/GetControlLog/" + @Model.First().TicketNumber)
        .AutoGenerateColumns()
        .AddInputFilter(Html.Term("AccountNumber", "Account Number"), "accountNumberId", startingValue: "")
        .AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", Html.Term("StartDate", "Start Date"), true)
        .ClickEntireRow()
		.Render(); %> 
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(function () {
        $("ul.SectionLinks li:eq(2)").click(function (e) {
            
            if ($('#hdnPaymentStatus').val() == 1)
                return true;
            else {
                showMessage('<%=@Html.Term("Payment_ValidExtendExpiration", "For the payment status is not possible process")%>', true);
                return false;
            }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
<div class="SectionNav">			
            <ul class="SectionLinks">
				<%= Html.SelectedLink("~/GeneralLedger/PaymentTickets/TicketDetails/" + Model[0].TicketNumber, Html.Term("PaymentTicketDetails", "Ticket Details"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
                <%= Html.SelectedLink("~/GeneralLedger/PaymentTickets/Renegotiation/" + Model[0].TicketNumber, Html.Term("PaymentRenegotiation", "Renegotiation"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
				<%= Html.SelectedLink("~/GeneralLedger/PaymentTickets/PayTicket/" + Model[0].TicketNumber, Html.Term("PaymentPayTicket", "Pay Ticket"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
                <%= Html.SelectedLink("~/GeneralLedger/PaymentTickets/ExtendExpiration/" + Model[0].TicketNumber, Html.Term("PaymendExtentExpiration", "Extend Expiration"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
                <%= Html.SelectedLink("~/GeneralLedger/PaymentTickets/CalculeUpdateBalance/" + Model[0].TicketNumber, Html.Term("PaymentCalculeUpdateBalance", "Calcule Update Balance"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
                <%= Html.SelectedLink("~/GeneralLedger/PaymentTickets/ControlLog/" + Model[0].TicketNumber, Html.Term("PaymentControlLog", "Control Log"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>

			</ul>
		</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%--<a href="<%= ResolveUrl("~/GeneralLedger/PaymentTickets/BrowseTickets") %>">--%>
<a href="<%= ResolveUrl("~/GeneralLedger") %>">
		    <%= Html.Term("GMP-Nav-General-Ledger", "General Ledger")%></a> >
           <%-- <a href="<%= ResolveUrl("~/GeneralLedger/PaymentTickets/BrowseTickets") %>">--%>
           <a href="<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/BrowseTickets") %>">
		    <%= Html.Term("PaymentTickets", "Payment Tickets")%></a> >
	    <%= Html.Term("ControlLog", "Control Log")%>
</asp:Content>
