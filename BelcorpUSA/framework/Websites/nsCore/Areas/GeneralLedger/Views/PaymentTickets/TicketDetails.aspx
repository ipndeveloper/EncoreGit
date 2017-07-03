<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/PaymentTicketsSearch.Master" 
Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.GLPaymeTycketsSearchData>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%=  Html.Term("TicketDetails", "Ticket Details")%>
        </h2>
	</div>
    <table id="periodForm" class="FormTable" width="100%">      
        
        <tr>
            <td class="FLabel">
                <%= Html.Term("Ticket", "Ticket")%>:
                <input type="hidden" id="hdnPaymentStatus" value="<%=  ViewBag.Status %>" />
            </td>
            <td>
                <input id="txtTicket" type="text" disabled="disabled" value="<%=Model[0].TicketNumber %>" /><br />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Order", "Order")%>:
            </td>
            <td>
                <input id="txtOrder" type="text" disabled="disabled" value="<%=Model[0].Orders %>"/><br />
            </td>
         </tr>
         <tr>
            <td class="FLabel">
                <%= Html.Term("Period", "Period")%>:
            </td>
            <td>
                <input id="txtPeriod" type="text" disabled="disabled" value="<%=Model[0].period %>"/><br />
            </td>
          </tr>
          <tr>  
            <td class="FLabel">
                <%= Html.Term("DataExpiration", "Data Expiration")%>:
            </td>
            <td>
                <input id="txtDataExpiration" type="text" disabled="disabled" value="<%=Model[0].DataExpiration %>"/><br />
            </td>
          </tr>
          <tr>
            <td class="FLabel">
                <%= Html.Term("NegotiationLevel", "Negotiation Level")%>:
            </td>
            <td>
                <input id="txtNegotiationLevel" type="text" disabled="disabled"  value="<%=Model[0].NegotiationLevel %>"/><br />
            </td>
          </tr>
          <tr>
            <td class="FLabel">
                <%= Html.Term("TotalAmount", "Total Amount")%>:
            </td>
            <td>
                <input id="txtTotalAmount" type="text" disabled="disabled" value="<%=Model[0].TotalAmount %>"/><br />
            </td>
          </tr>
          
          <tr>
            <td class="FLabel">
                <%= Html.Term("StatusPayment", "Status Payment")%>:
            </td>
            <td>
                <input id="Text1" type="text" disabled="disabled" value="<%=Model[0].StatusPayment %>"/><br />
            </td>
        </tr>     
          <tr>
            <td class="FLabel">
                <%= Html.Term("StatusExpiration", "Status Expiration")%>:
            </td>
            <td>
                <input id="txtStatusPayment" type="text" disabled="disabled" value="<%=Model[0].StatusExpiration %>"/><br />
            </td>
        </tr>       
    </table>
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
<%--<a href="<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/BrowseTickets") %>">--%>
            <a href="<%= ResolveUrl("~/GeneralLedger") %>">
		    <%= Html.Term("GMP-Nav-General-Ledger", "General Ledger")%></a> >
            <a href="<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/BrowseTickets") %>">
		    <%= Html.Term("PaymentTickets", "Payment Tickets")%></a> >
	            <%= Html.Term("TicketDetails", "Ticket Details")%>
</asp:Content>
