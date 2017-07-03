<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/CTE.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.GLCalculateUpdateBalanceSearchData>>" %>
  
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%=  Html.Term("PayTicket", "Pay Ticket")%>
        </h2>
    </div>
    <table id="periodForm" class="FormTable" width="70%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("AccountNumber", "Account Number")%>:
            </td>
            <td>
                <input id="txtAccountNumber" type="text" value="<%=Model[0].AccountID %>" disabled="disabled" /><br />
            </td>
            <td class="FLabel">
                <%= Html.Term("Name", "Name")%>:
            </td>
            <td>
                <input id="txtName" type="text" value="<%=Model[0].Name %>" disabled="disabled" /><br />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Amount", "Amount")%>:
            </td>
            <td>
              <%--  comentado IPN --%>
                <%--<input id="txtAmount" type="text" value="<%=Model[0].TotalAmount %>" disabled="disabled"/><br />--%>

                 <input id="txtAmount" type="text" value="<%=Model[0].TotalAmount.ToString("C",CoreContext.CurrentCultureInfo) %>" disabled="disabled"/><br />
               <%-- <a id="btnCalculate" href="javascript:void(0);"><u>Calculate</u></a><br />--%>
            </td>
            <td class="FLabel">
                <%= Html.Term("Ticket", "Ticket")%>#:
            </td>
            <td>
                <input id="txtTicket" type="text" value="<%=Model[0].TicketNumber %>" disabled="disabled" /><br />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Entity", "Entity")%>:
            </td>
            <td>
                <select id="ddlEntity" class="required">
                    <option value="0">
                        <%= Html.Term("termSelectEntity", " Select Entity ")%></option>
                    <% foreach (var items in ViewData["getEntitys"] as List<GLDropdownlistUtilSearchData>)
                       {
                    %> 
                    <option value="<%=items.id %>">
                        <%=items.Name%></option>
                    <%                                       
                       }                    
                    %>
                </select>
            </td>
            <td class="FLabel">
                
                <%= Html.Term("ProcessDate", "Process Date")%>:
           
            </td>
            <td>
               <%-- <select id="ddlPaymentType">
                    <option value="">
                        <%= Html.Term("termPaymentType", " Select Payment Type ")%></option>
                </select>--%>

                  <input id="txtProcessOnDateUTC" type="text" class="DatePicker TextInput" 
                  value="<%= nsCore.Areas.CTE.Controllers.PaymentTicketsController.FormatDatetimeByLenguage(Model[0].ProcessOnDateUTC , CoreContext.CurrentCultureInfo) %>"  /><br />

                
            </td>
        </tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue">
                        <%= Html.Term("PaymentTicket", "Payment Ticket")%></a>
                    
                </p>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

  <script type="text/javascript">
      $(function () {
//          $.datepicker.regional['es'] = {
//              closeText: 'Cerrar',
//              prevText: '<Ant',
//              nextText: 'Sig>',
//              currentText: 'Hoy',
//              monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
//              monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
//              dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
//              dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
//              dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
//              weekHeader: 'Sm',
//              dateFormat: 'dd/mm/yy',
//              firstDay: 1,
//              isRTL: false,
//              showMonthAfterYear: false,
//              yearSuffix: ''
//          };

//          $.datepicker.setDefaults($.datepicker.regional['es']);





      });
    </script>

    <script type="text/javascript">
    $(function () {
         $('#txtAmount').keypress(function (e) {
            key = e.keyCode ? e.keyCode : e.which
              // backspace
              if (key == 8) return true
              //37 and 40 Teclas direccion
              if (key > 36 && key < 41) return true
              // 0-9
              if (key > 47 && key < 58) {
                if (field.value == "") return true
                regexp = /.[0-9]{2}$/
                return !(regexp.test(field.value))
              }
              // .
              if (key == 46) {
                if (field.value == "") return false
                regexp = /^[0-9]+$/
                return regexp.test(field.value)
              }              
            return false
         });

        $('#btnCalculate').click(function (e) {            
             $('#txtAmount').val(<%=Model[0].TotalAmount %>);
        });

        $('#ddlEntity').change(function () {
            $('#ddlPaymentType').prop('selectedIndex', 0);
            var t = $(this);
            var BanckID = $("#ddlEntity").val();

            showLoading(t);
            $.get('<%= ResolveUrl(string.Format("~/CTE/PaymentTickets/GetPaymentTypes/")) %>', { BanckID: BanckID }, function (response) {
                if (response.result) {
                    hideLoading(t);
                    $('#ddlPaymentType').children('option:not(:first)').remove();
                    if (response.PaymentTypes) {
                        for (var i = 0; i < response.PaymentTypes.length; i++) {
                            $('#ddlPaymentType').append('<option value="' + response.PaymentTypes[i].ID + '">' + response.PaymentTypes[i].Name + '</option>');
                        }
                    }
                } else {
                    showMessage(response.message, true);
                }
            });
        });

            $('#btnSave').click(function () {
               var TicketNumber = $("#txtTicket").val();
               var bankID = $("#ddlEntity").val();
               var processOnDateUTC= $('#txtProcessOnDateUTC').val();
              

         var fechaInvalida=false;

          if (processOnDateUTC != '')
          {
             var d = new Date();
            var fechaSystem = new Date(d.getFullYear(), d.getMonth() + 1, d.getDate());
            var array_fecha = processOnDateUTC.split("/")
            dia = array_fecha[0]
            mes = (array_fecha[1])
            ano = (array_fecha[2])

            newDate02 = new Date(ano, mes, dia)
            if (newDate02 > fechaSystem)
              {
                fechaInvalida=true;
                }
           }


            var t = $(this);

            if(processOnDateUTC ==''  || processOnDateUTC == null) {
                showMessage('<%=@Html.Term("PayticketValidateDateProcesses", "Select the Date ")%>', true);
             }
           else if (fechaInvalida)
            {
             showMessage('<%=@Html.Term("DateGreater", "The new date can not be greater than the system date")%>', true);
            }
            else if($('#ddlEntity').val()=='0')
            {
            showMessage('<%=@Html.Term("PayTicket_ValidEntity", "You must select Entity")%>', true);
            }else{
            showLoading(t);
                $.get('<%= ResolveUrl(string.Format("~/CTE/PaymentTickets/GetApplyManualPayment/")) %>', { TicketNumber: TicketNumber, BankID: bankID ,ProcessOnDateUTC: processOnDateUTC}, function (response) {
                    if (response.result) {
                        hideLoading(t);
                        showMessage(response.message, false);
                    } else {
                         hideLoading(t);
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
<a href="<%= ResolveUrl("~/CTE") %>">
    <%--<a href="<%= ResolveUrl("~/CTE/PaymentTickets/BrowseTickets") %>">--%>
        <%= Html.Term("GMP-Nav-General-Cte", "CTE")%></a> > 
       <%-- <a href="<%= ResolveUrl("~/CTE/PaymentTickets/BrowseTickets") %>">--%>
       <a href="<%= ResolveUrl("~/CTE/PaymentTicketsReport/BrowseTickets") %>">
            <%= Html.Term("PaymentTickets", "Payment Tickets")%></a> >
    <%= Html.Term("PayTicket", "Pay Ticket")%>
</asp:Content>
