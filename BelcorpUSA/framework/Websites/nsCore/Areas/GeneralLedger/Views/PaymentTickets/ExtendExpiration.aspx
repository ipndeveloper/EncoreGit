<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/PaymentTicketsSearch.Master" 
Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.GLCalculateUpdateBalanceSearchData>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		    <h2>
			    <%=  Html.Term("ExtentExpiration", "Extent Expiration")%>
            </h2>
    </div>
    <table id="periodForm" class="FormTable" width="70%">      
        <tr>
            <td class="FLabel">
                <%= Html.Term("AccountNumber", "Account Number")%>:
            </td>
            <td>
                <input id="txtAccountNumber" type="text" disabled="disabled" value="<%=Model[0].AccountID %>"/><br />
            </td>
            <td class="FLabel">
                <%= Html.Term("Name", "Name")%>:
            </td>
            <td>
                <input id="txtName" type="text" disabled="disabled" value="<%=Model[0].Name %>" /><br />
            </td>
        </tr>        
         <tr>
            
            <td class="FLabel">
                <%= Html.Term("Ticket ", "Ticket")%>:
            </td>
            <td>
                <input id="txtTicketNumber" type="text" disabled="disabled" value="<%=Model[0].TicketNumber %>"/><br />
            </td>
            <td class="FLabel">
                <%= Html.Term("Amount", "Amount")%>:
            </td>
            <td>
                <input id="txtAmount" type="text" disabled="disabled" value="<%=Model[0].TotalAmount %>"/><br />
            </td>
          
          </tr>         
          
          <tr>
            <td class="FLabel">
                <%= Html.Term("ActualDate", "Actual Date")%>:
            </td>
            <td>
               <input type="text" id="txtActualDate" disabled="disabled" value="<%=Model[0].CurrentExpirationDateUTC.ToString("dd/MM/yyyy")  %>" />
            </td>
            <td class="FLabel">
                <%= Html.Term("NewDate", "New Date")%>:
            </td>
            <td>
              <input type="text" id="txtNewDate" class="DatePicker" value="" placeholder="<%=Html.Term("ChoseaDate","Chose a Date") %>" />
            </td>
          </tr>               
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSaveNewDate" style="display:inline-block;" class="Button BigBlue" >
                        <%= Html.Term("SaveNewDate", "Save New Date")%></a>
                </p>
            </td><td></td><td>
           <input type="hidden" id="hdnPaymentStatus" value="<%=  ViewBag.Status %>" />
             <input type="hidden" id="hdnPaymentExpiration" value="<%=  Model[0].ExpirationStatusID %>" />
            </td>
        </tr>
        
        </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
    $(function () {

        $.datepicker.regional['es'] = {
            closeText: 'Cerrar',
            prevText: '<Ant',
            nextText: 'Sig>',
            currentText: 'Hoy',
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
            weekHeader: 'Sm',
            dateFormat: 'dd/mm/yy',
            firstDay: 1,
            isRTL: false,
            showMonthAfterYear: false,
            yearSuffix: ''
        };

        $.datepicker.setDefaults($.datepicker.regional['es']);

        $("ul.SectionLinks li:eq(2)").click(function (e) {

            if ($('#hdnPaymentStatus').val() == 1)
                return true;
            else {
                showMessage('<%=@Html.Term("Payment_ValidExtendExpiration", "For the payment status is not possible process")%>', true);
                return false;
            }
        });

        $('#btnSaveNewDate').click(function (e) {


            var element = $("#txtActualDate").val().split(' ');

            var array_fecha = $("#txtActualDate").val().split("/")
            var dia = array_fecha[0]
            var mes = (array_fecha[1])
            var ano = (array_fecha[2])
            var fechaactual = new Date(ano, mes, dia)


            var d = new Date();

            var fechaSystem = new Date(d.getFullYear(), d.getMonth() + 1, d.getDate());

            var newDate02 = new Date();
            var newDate = "";
            if ($("#txtNewDate").val() == '') {
                newDate02 = new Date("2000/01/01");
                newDate = "2000/01/01";
            } else {
                array_fecha = $("#txtNewDate").val().split("/")
                dia = array_fecha[0]
                mes = (array_fecha[1])
                ano = (array_fecha[2])

                newDate02 = new Date(ano, mes, dia)
                newDate = $("#txtNewDate").val();
            }
           
            if ($('#hdnPaymentExpiration').val() == 1) {
                showMessage('<%=@Html.Term("Payment_ValidExpirationDate", "Title expired. No process is possible")%>', true);
            } else if ($('#hdnPaymentStatus').val() == 0) {
                showMessage('<%=@Html.Term("Payment_ValidExtendExpiration", "For the payment status is not possible process")%>', true);
            } else if (newDate02 < fechaactual) {
                showMessage('<%=@Html.Term("AlterDueDateFalse", "The New Date cant Be Less Than the Current Date")%>', true);
            } else if (newDate02 <= fechaSystem) {

                showMessage('<%=@Html.Term("AlterDueDateSystemFalse", "The New Date cant Be Less Than the System Date")%>', true);
            }
            else {
                var data = {
                    NewDate: newDate,
                    TicketNumber: $('#txtTicketNumber').val()
                };

                $.post('<%= ResolveUrl("~/GeneralLedger/PaymentTickets/AlterDueDate") %>', data, function (response) {
                    if (response.result) {
                        showMessage('<%=@Html.Term("AlterDueDateTrue", "Alter Due Date Save.")%>', false);
                        $("#txtActualDate").val($("#txtNewDate").val());
                    } else {
                        showMessage(response.message, true);
                    }
                });
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
				<%= Html.SelectedLink("~/GeneralLedger/PaymentTickets/PayTicket/" + Model[0].TicketNumber, Html.Term("PaymentPayTicket", "Pay Ticket"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
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
            <%--<a href="<%= ResolveUrl("~/GeneralLedger/PaymentTickets/BrowseTickets") %>">--%>
            <a href="<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/BrowseTickets") %>">
		    <%= Html.Term("PaymentTickets", "Payment Tickets")%></a> >
	    <%= Html.Term("ExtentExpiration", "Extent Expiration")%>
</asp:Content>
