<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/CTE.Master" 
Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.GLPaymeTycketsSearchData>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <%-- <div class="SectionHeader">
		<h2>
			<%=  Html.Term("TicketDetails", "Ticket Details")%>
        </h2>
	</div>--%>
     <input type="hidden" id="hdnPaymentStatus" value="<%=  ViewBag.Status %>" />
    <table id="detalleid" class="FormTable Section" width="95%">
    <tr>
        <td colspan="3">  
        <div id="divTitulo" style="display:block;"> 
        <h3 width="75%" align="left"><%=  Html.Term("TicketDetails", "Ticket Details")%></h3>
        </div>
        </td>
    </tr>
    <tr>
        <td style="font-weight:bold;width:120px"><%= Html.Term("TicketNumber", "Boleto")%>: </td>
        <td style="text-align:left;width:100px"><span id="spTicketNumber" style="color: #8DC0DB;"><%=Model[0].TicketNumber%></span></td>
        <td style="font-weight:bold;width:120px"><%= Html.Term("Consultora", "Consultora")%>: </td>
        <td  style="text-align:left;width:100px"> <span id="spPaymentMethod" style="color: #8DC0DB;"><%=Model[0].OrderCustomer%></span></td>
        <td style="font-weight:bold;;width:130px"><%= Html.Term("Orders", "Pedido")%>: </td>
        <td style="text-align:left;width:70px"><span id="spNegotiationLevel" style="color: #8DC0DB;"><%=Model[0].Orders%></span></td>

        <td style="font-weight:bold;width:120px"> </td>
          <td style="text-align:left;width:70px"><span id="Span13" style="color: #8DC0DB;"></span></td>
    </tr>
    <tr>
        <td style="font-weight:bold;"><%= Html.Term("StatusExpiration", "Status Expiration")%></td><td><span id="spOrdenNumber" style="color: #8DC0DB;"><%=nsCore.Areas.CTE.Controllers.PaymentTicketsController.FormatDatetimeByLenguage(Model[0].StatusExpiration , CoreContext.CurrentCultureInfo)%></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketDataExpiration", "Fecha Vencimiento")%></td><td><span id="spTicketStatus" style="color: #8DC0DB;"><%=nsCore.Areas.CTE.Controllers.PaymentTicketsController.FormatDatetimeByLenguage(Model[0].DataExpiration,CoreContext.CurrentCultureInfo)  %></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketDateValidity", "Fecha de Validez")%> </td><td><span id="spAuthorizationNumber" style="color: #8DC0DB;"><%=nsCore.Areas.CTE.Controllers.PaymentTicketsController.FormatDatetimeByLenguage(Model[0].DateValidity,CoreContext.CurrentCultureInfo)%></span></td>
    </tr>
    <tr>
    <td style="font-weight:bold;"><%= Html.Term("PaymentMethod", "Payment Method")%>: </td><td><span id="spInitialAmount" style="color: #8DC0DB;"><%=Model[0].PaymentMetod%></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketPaymentType", "Tipo de Pago-Boleto o tarjeta")%>: </td><td><span id="spFinancialAmount" style="color: #8DC0DB;"><%=Model[0].PaymentType%></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketDeferredAmount", "Numero de Cuotas")%>: </td><td ><span id="spDiscountedAmount" style="color: #8DC0DB;"><%=Model[0].DeferredAmount%></span></td>        
        
    </tr>
     <tr>
       <td style="font-weight:bold;"><%= Html.Term("DetTiketDateCreated", "Fecha Creacion")%>: </td><td><span id="Span1" style="color: #8DC0DB;"><%=nsCore.Areas.CTE.Controllers.PaymentTicketsController.FormatDatetimeByLenguage(Model[0].DateCreated, CoreContext.CurrentCultureInfo)%></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketOriginalExpiration", "Vencimiento Titulo Original")%>: </td><td><span id="Span2" style="color: #8DC0DB;"><%=Model[0].OriginalExpiration%></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketStatusPayment", "Estado del Boleto")%>: </td><td ><span id="Span3" style="color: #8DC0DB;"><%=Model[0].StatusPayment%></span></td>        
         <td style="font-weight:bold;"> <%= Html.Term("NegotiationLevel", "Negotiation Level")%>: </td><td ><span id="Span11" style="color: #8DC0DB;"><%=Model[0].NegotiationLevel%></span></td>        
    </tr>
    <tr>
       <td style="font-weight:bold;"><%= Html.Term("DetTiketInitialAmount", "Monto Inicial")%>: </td><td><span id="Span4" style="color: #8DC0DB;"><%=Model[0].InitialAmount.ToString("N", CoreContext.CurrentCultureInfo)%></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketFinancialAmount", "Financial Amount")%>: </td><td><span id="Span5" style="color: #8DC0DB;"  ><%=Model[0].FinancialAmount.ToString("N",CoreContext.CurrentCultureInfo)  %></span><input id="hdnFinancial" value="<%=Model[0].FinancialAmount%>" type="hidden" /><input id="hdnTicketNumber" value="<%=Model[0].TicketNumber%>" type="hidden" /></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketDiscountedAmount", "Descuentos")%>: </td><td ><span id="Span6" style="color: #8DC0DB;"><%=Model[0].DiscountedAmount.ToString("N",CoreContext.CurrentCultureInfo)%></span></td>        
        <td style="font-weight:bold;"><%= Html.Term("TotalAmount", "Total Amount")%>: </td><td ><span id="Span12" style="color: #8DC0DB;"><%=Model[0].TotalAmount.ToString("N",CoreContext.CurrentCultureInfo)   %></span></td>
    </tr>
     <tr>
       <td style="font-weight:bold;"><%= Html.Term("DetTiketDateLastTotalAmount", "Fecha Actualización Montos")%>: </td><td><span id="Span7" style="color: #8DC0DB;"><%=nsCore.Areas.CTE.Controllers.PaymentTicketsController.FormatDatetimeByLenguage(Model[0].DateLastTotalAmount,CoreContext.CurrentCultureInfo)%></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketAccepted", "Indicador pago aceptado del banco")%>: </td><td><span id="Span8" style="color: #8DC0DB;"><%=Model[0].Accepted%></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DetTiketForefit", "Enviado a Pedida")%>: </td><td ><span id="Span9" style="color: #8DC0DB;"><%=Model[0].Forefit%></span></td>        
        
    </tr>
     <tr>
       <td style="font-weight:bold;"><%= Html.Term("DetTiketDateLastModified", "Ultima actualización")%>: </td><td><span id="Span10" style="color: #8DC0DB;"><%=nsCore.Areas.CTE.Controllers.PaymentTicketsController.FormatDatetimeByLenguage(Model[0].DateLastModified,CoreContext.CurrentCultureInfo)%></span></td>
       </tr>
       
    </table>
     <br />
    <br />
    <%  
        if (Model[0].FinancialAmount > 0 && ViewBag.Status == 1)
        {
        %>
    <table  >
    <tr>
   
            <td class="FLabel">
                <%= Html.Term("IngDesctoAplicate", "Ingrese Descuento Aplicar")%>:
            </td>
            <td>
                <input id="txtDesctoAplicate" type="text" class="clear required justNumbers"  value=""/><br />
                
            </td>
            <td class="FLabel">
              <p>
                    <a href="javascript:void(0);" id="btnDesctoAplicate"  class="Button BigBlue" >
                        <%= Html.Term("DesctoAplicate", "Descuento Aplicar")%></a>
                </p>
            </td>
            
            </tr>
    </table>

    <% }%>

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


         $('#btnDesctoAplicate').click(function (e) {
            
             var DesctoAplicate = 0;
             DesctoAplicate = $("#txtDesctoAplicate").val();
             var FinancialAmount = 0;
             FinancialAmount = parseFloat($("#hdnFinancial").val());

             if (FinancialAmount < DesctoAplicate || DesctoAplicate < 0) {
                 showMessage('<%=@Html.Term("Payment_ValidDesctoAplicate", "El valor ingresado no es correcto")%>', true);
             } else {

                 var warningMessage = '<%= Html.Term("AreYouSureDesctoProces", "Confirme Proceso de descuento.")%>';
                 if (confirm(warningMessage)) {
                     var data = {
                         DesctoAplicate: $("#txtDesctoAplicate").val(),
                         TicketNumber: $('#hdnTicketNumber').val()
                     };

                     $.post('<%= ResolveUrl("~/CTE/PaymentTickets/ApplicateDescto") %>', data, function (response) {
                         if (response.result) {
                             showMessage('<%=@Html.Term("DesctoAplicateSuccesfull", "Descuento Aplicado.")%>', false);
                             $("#txtDesctoAplicate").val('');
                             window.location = '<%= ResolveUrl("~/CTE/PaymentTickets/TicketDetails/' + $('#hdnTicketNumber').val() + '") %>';
                         } else {
                             showMessage(response.message, true);
                         }
                     });
                 }
             }


         });

     });
</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
<div class="SectionNav">			
            <ul class="SectionLinks">
				<%= Html.SelectedLink("~/CTE/PaymentTickets/TicketDetails/" + Model[0].TicketNumber, Html.Term("PaymentTicketDetails", "Ticket Details"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
                <%= Html.SelectedLink("~/CTE/PaymentTickets/Renegotiation/" + Model[0].TicketNumber, Html.Term("PaymentRenegotiation", "Renegotiation"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
				<%= Html.SelectedLink("~/CTE/PaymentTickets/PayTicket/" + Model[0].TicketNumber, Html.Term("PaymentPayTicket", "Pay Ticket"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
                <%= Html.SelectedLink("~/CTE/PaymentTickets/ExtendExpiration/" + Model[0].TicketNumber, Html.Term("PaymendExtentExpiration", "Extend Expiration"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
                <%= Html.SelectedLink("~/CTE/PaymentTickets/CalculeUpdateBalance/" + Model[0].TicketNumber, Html.Term("PaymentCalculeUpdateBalance", "Calcule Update Balance"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>
                <%= Html.SelectedLink("~/CTE/PaymentTickets/ControlLog/" + Model[0].TicketNumber, Html.Term("PaymentControlLog", "Control Log"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "Products")%>

			</ul>
		</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<%--<a href="<%= ResolveUrl("~/CTE/PaymentTicketsReport/BrowseTickets") %>">--%>
            <a href="<%= ResolveUrl("~/CTE") %>">
		    <%= Html.Term("GMP-Nav-General-Cte", "CTE")%></a> >
            <a href="<%= ResolveUrl("~/CTE/PaymentTicketsReport/BrowseTickets") %>">
		    <%= Html.Term("PaymentTickets", "Payment Tickets")%></a> >
	            <%= Html.Term("TicketDetails", "Ticket Details")%>
</asp:Content>
