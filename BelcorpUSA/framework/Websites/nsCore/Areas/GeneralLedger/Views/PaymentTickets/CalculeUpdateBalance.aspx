<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/PaymentTicketsSearch.Master" 
Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.GLCalculateUpdateBalanceSearchData>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <div class="SectionHeader">
		    <h2>
			    <%=  Html.Term("CalculeUpdateBalance", "Calcule Update Balance")%>
            </h2>
 </div>
 <table id="calculeUpdateBalanceTable" class="FormTable" width="70%">      
        <tr>
            <td class="FLabel">
                <%= Html.Term("AccountNumber", "Account Number")%>:
                <input type="hidden" id="hdnPaymentStatus" value="<%=  ViewBag.Status %>" />
                <input id="hdnPaymentTicketID" type="hidden" value="<%=Model[0].TicketNumber %>"/>
            </td>

            <td>
                <input id="txtAccountNumber" type="text" value="<%=Model[0].AccountID %>" /><br />
            </td>
            <td class="FLabel">
                <%= Html.Term("Name", "Name")%>:
            </td>
            <td>
                <input id="txtName" type="text" value="<%=Model[0].Name %>"/><br />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("OriginalAmount", "Original Amount")%>:
            </td>
            <td>
                <input id="txtOriginalAmount" type="text" value="<%=Model[0].InitialAmount %>" /><br />
            </td>
            <td class="FLabel">
                <%= Html.Term("CreateTicketDate", "Create Ticket Date")%>:
            </td>
            <td>
                <input id="txtCreateTicketDate" type="text" value="<%=Model[0].DateCreatedUTC %>"/><br />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                  <%= Html.Term("FinancialAmount", "Financial Amount")%>:
            </td>
            <td>
                <input type="text" id="txtFinancialAmount" value="<%=Model[0].FinancialAmount %>"/>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                  <%= Html.Term("DisCountedAmount", "DisCounted Amount")%>:
            </td>
            <td>
                <input type="text" id="txtDisCountedAmount" value="<%=Model[0].DisCountedAmount %>"/>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                  <%= Html.Term("TotalAmount", "Total Amount")%>:
            </td>
            <td>
                <input type="text" id="txtTotalAmount" value="<%=Model[0].TotalAmount %>" />
            </td>
        </tr>
        
      
</table>
<br />
 <a id="btnSave" href="javascript:void(0);" class="Button BigBlue">
                               <%= Html.Term("UpdateBalance", "Update Balance")%>
                        </a>
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

        $('#btnSave').click(function () {
  
            if ($('#hdnPaymentStatus').val() == 0) {
                showMessage('<%=@Html.Term("Payment_ValidExtendExpiration", "For the payment status is not possible process")%>', true);
            } else {

                var data = {
                    id: $('#hdnPaymentTicketID').val()
                };

                $.post('<%= ResolveUrl("~/GeneralLedger/PaymentTickets/CalculateNewAmount") %>', data, function (response) {
                    if (response.result) {
                        $("#txtFinancialAmount").val(response.FinancialAmount)
                        $("#txtTotalAmount").val(response.TotalAmount);
                        showMessage('Calculate Balance successful!', false);
                    } else {
                        showMessage(response.message, true);
                    }
                });
            }

        });
        //        $('#btnSave02').click(function () {

        //            var sParams = {
        //            id:$('#hdnPaymentTicketID').val()
        //            }
        //            var t2 = $(this);
        //            showLoading(t2);
        //            $.ajax({
        //                type: "POST",
        //                url: '/GeneralLedger/PaymentTickets/CalculateNewAmount',
        //                data: sParams,
        //                contentType: "application/json; charset=utf-8",
        //                dataType: "json",
        //                async: "false",
        //                success: function (response) {
        //                    hideLoading(t2);

        //                    if (response.result) {
        //                        $("#txtFinancialAmount").val(response.MultaCalulada+response.InteresCalculado)
        //                        var DisCountedAmount= $("#txtDisCountedAmount").val();
        //                        var InitialAmount= $("#txtOriginalAmount").val();
        //                        var FinancialAmount=response.MultaCalulada+response.InteresCalculado;
        //                        $("#txtTotalAmount").val(InitialAmount+FinancialAmount-DisCountedAmount);
        //                        showMessage('Calculate Balance successful!', false);

        //                    } else {
        //                        showMessage(response.message, true);
        //                    }

        //                },
        //                error: function (resultado) { }
        //            });
        //        });
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
	    <%= Html.Term("CalculeUpdateBalance", "Calcule Update Balance")%>
</asp:Content>
