<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/PaymentTicketsSearch.Master" 
Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.GeneralLedgerNegotiationData>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<div id="divCreditCard" >
<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("TicketInformation", "Ticket Information")%></h2>
	</div>
    <table id="newMaterial" class="FormTable Section" width="75%">
    <tr>
        <td colspan="3">  
        <div id="divTitulo" style="display:block;"> 
        <h3 width="75%" align="left">The Renegotation of this ticket is not posible. The Ticket information is:</h3>
        </div>
        </td>
    </tr>
    <tr>
        <td style="font-weight:bold;width:120px"><%= Html.Term("TicketNumber", "Ticket Number")%>: </td>
        <td style="text-align:left;width:70px"><span id="spTicketNumber" style="color: #8DC0DB;"></span></td>
        <td style="font-weight:bold;width:120px"><%= Html.Term("PaymentMethod", "Payment Method")%></td>
        <td  style="text-align:left;width:80px"> <span id="spPaymentMethod" style="color: #8DC0DB;"></span></td>
        <td style="font-weight:bold;;width:130px"><%= Html.Term("Negotiation", "Negotiation")%>: </td>
        <td style="text-align:left;width:70px"><span id="spNegotiationLevel" style="color: #8DC0DB;"></span></td>
    </tr>
    <tr>
        <td style="font-weight:bold;"><%= Html.Term("OrdenNumber", "Orden Number")%></td><td><span id="spOrdenNumber" style="color: #8DC0DB;"></span></td>
        <td style="font-weight:bold;"><%= Html.Term("TicketStatus", "Ticket Status")%></td><td><span id="spTicketStatus" style="color: #8DC0DB;"></span></td>
        <td style="font-weight:bold;"><%= Html.Term("AuthorizationNumber", "Authorization Number")%> </td><td><span id="spAuthorizationNumber" style="color: #8DC0DB;"></span></td>
    </tr>
    <tr>
    <td style="font-weight:bold;"><%= Html.Term("OriginalAmount", "Original Amount")%>: </td><td><span id="spInitialAmount" style="color: #8DC0DB;"></span></td>
        <td style="font-weight:bold;"><%= Html.Term("FinancialAmount", "Financial Amount")%>: </td><td><span id="spFinancialAmount" style="color: #8DC0DB;"></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DisCountedAmount", "DisCounted Amount")%>: </td><td ><span id="spDiscountedAmount" style="color: #8DC0DB;"></span></td>        
        <td style="font-weight:bold;width:80px;"><%= Html.Term("TotalAmount", "Total Amount")%>: </td><td style="text-align:left;width:40px"><span id="spTotalAmount" style="color: #8DC0DB;"></span></td>
    </tr>
     <tr>
        <td style="font-weight:bold;"><%= Html.Term("DataExpiration", "Data Expiration")%>: </td><td><span id="spCurrentExpirationDateUTC" style="color: #8DC0DB;"></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DayValidate", "Day Validate")%>: </td><td><span id="spDayValidate" style="color: #8DC0DB;"></span></td>
        <td style="font-weight:bold;"><%= Html.Term("DayExpiration", "Day Expiration")%>: </td><td><span id="spDayExpiration" style="color: #8DC0DB;"></span></td>
        
    </tr>
    </table>
</div>
<br />
<input type="hidden" id="hdnPaymentStatus" value="<%=  ViewBag.Status %>" />
<input type="hidden" id="hdnOrderPaymentID" />
<input type="hidden" id="hdnDayValidate" />
<input type="hidden" id="hdnRenegotiationConfigurationID" />
<input type="hidden" id="hdnLastDateExpirated" />
<input type="hidden" id="hdnNumberCuotas" />

<div id="divRenegotiation" style="display:none;">
<table   style="width:100%; text-align:center">
 <tr>
        <td colspan="5">   
        <h3 width="75%" align="left"></h3>
        </td>
    </tr>
<tr>
<td style="width:56%">
<%--// aca se veran los metodos de pago--%>

<table class="DataGrid" width="100%">
<thead >
        <tr class="GridColHead" style="width:100%;  " >                           
            <th >
            </th>                                                   
            <th>
                <%= Html.Term("Plano", "Plano")%>
            </th>    
             <th>
                <%= Html.Term("Shares", "Shares")%>
            </th>         
            <th>
                <%= Html.Term("Juros_Dias", "Juros/Dias")%>
            </th>                           
            <th>
                <%= Html.Term("Taxa", "Taxa R$")%>
            </th>          
            <th>
                <%= Html.Term("DetDiscount", "Descuento")%>
            </th>                                       
        </tr>
    </thead>
<tbody id="tblMethods">
</tbody>
</table>

</td>
<td style="width:7%"></td>

<td style="width:37%">
<div id="divShareds" style="display:none;">

<%--// aca se veran los valors a  ingresar o llenar--%>

<table id="tablaShareds" class="DataGrid" width="100%">
<thead>
        <tr class="GridColHead" style="width:100%; text-align:center">                           
                                                          
            <th>
                <%= Html.Term("Parcela", "Parcela")%>
            </th>                   
            <th>
                <%= Html.Term("ValShared", "Valor")%>
            </th>                           
            <th>
                <%= Html.Term("ExpirationDate", "Data de Vencimiento")%>
            </th>                                       
        </tr>
    </thead>
<tbody id="tblShareds">
</tbody>
</table>
<br /><br />
<table>
<tr>
<td  >
<input id="Text3" disabled="disabled" type="text" style="background:white;border-width: 0px; display: block;width:70px;"
 value="<%= Html.Term("DetTotal", "Total")%>:"/>
            <%--    <%= Html.Term("DetTotal", "Total")%>:--%>
</td>
<td  style="text-align:left">
    <%--<span id="spDetTotalAmount"  style="text-align:left"></span>--%>
    <input id="spDetTotalAmount" disabled="disabled" type="text" 
    style="background:white;border-width: 0px; display: block;width:60px;"/>
</td>
<td style="width:80px" >
               
</td>
</tr>
<tr  >
<td  >
<input id="lblDetDiscount" disabled="disabled" style="background:white;border-width: 0px; display: block;width:70px;"
 type="text" value="<%= Html.Term("DetDiscount", "Descuento")%>:"/>
               <%-- <%= Html.Term("DetDiscount", "Descuento")%>:--%>
</td>
<td  style="text-align:left">
    <%--<span id="spDetDiscount"  style="text-align:left"></span>--%>
    <input id="spDetDiscount"  type="text" disabled="disabled"
    style="background:white;border-width: 0px; display: block;width:60px;"/>
</td>
<td  >
               
</td>
</tr>
<tr>
<td colspan="3" >
   <br />         
</td>
</tr>
<tr>
<td  >
 <input id="Text1" disabled="disabled" type="text" style="background:white;border-width: 0px; display: block; width:70px;"
  value="<%= Html.Term("DetAPagar", "A Pagar")%>:"/>
                
</td>
<td  style="text-align:left">
    <%--<span id="spDetAPagar"  style="text-align:left; font-size:x-large; color:Blue"></span>--%>
    <input id="spDetAPagar" disabled="disabled" type="text"
    style="background:white;border-width: 0px; display: block; font-size:large; color:Blue;width:60px;" />
    </td>
    <td  >
    <p>
    <a id="btnRenegotiate" href="javascript:void(0);" class="Button BigBlue">
           <%=Html.Term("Renegotiate","Renegociar") %> </a></p>           



</td>
</tr>
</table>
</div>
</td>
</tr>
</table>
</div>

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

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<script type="text/javascript">
    $(function () {
        InicializarCampos();
        function InicializarCampos() {
            var PaymentMethod = '<%= Model[0].PaymentTypeID %>';
            var ExpirationStatusID = '<%= Model[0].ExpirationStatusID %>';
            var IsDeferred = '<%= Model[0].IsDeferred %>';
            var MaximumAmountOfPayments = '<%= Model[0].MaximumAmountOfPayments %>';

            $("#spTicketNumber").text('<%= Model[0].TicketNumber %>');
            $("#spPaymentMethod").text('<%= Model[0].PaymentMethod %>');
            $("#spTotalAmount").text('<%= Model[0].TotalAmount %>');
            $("#spOrdenNumber").text('<%= Model[0].OrderNumber %>');
            $("#spTicketStatus").text('<%= Model[0].TicketStatus %>');
            $("#spAuthorizationNumber").text('<%= Model[0].AuthorizationNumber %>');

            $("#spNegotiationLevel").text('<%= Model[0].NegotiationLevel %>');
            $("#spInitialAmount").text('<%= Model[0].InitialAmount %>');
            $("#spFinancialAmount").text('<%= Model[0].FinancialAmount %>');
            $("#spDiscountedAmount").text('<%= Model[0].DiscountedAmount %>');

            $("#spCurrentExpirationDateUTC").text('<%= Model[0].CurrentExpirationDateUTC %>');
            $("#spDayExpiration").text('<%= Model[0].DayExpiration %>');
            $("#spDayValidate").text('<%= Model[0].DayValidate %>');

            var ViewMethodsRenegotiation = '<%= Model[0].ViewMethodsRenegotiation %>';
            
            if (ViewMethodsRenegotiation==1) {
               
                var url = '<%= ResolveUrl("~/GeneralLedger/PaymentTickets/GeListRenegotiationMethodsByOrder") %>';
                var odata = JSON.stringify({ TicketNumber: '<%= Model[0].TicketNumber %>' });

                $.ajax({
                    data: odata,
                    url: url,
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json",
                    success: function (response) {
                        var html = response.Items;

                        $("#tblMethods").html(html)
                        $("#divRenegotiation").css("display", "block");
                        $("#divTitulo").css("display", "none");
                        

                    },
                    error: function (error) {
                    }
                });

            }
        }


        //Guardar
        $('#btnRenegotiate').click(function () {

            var isComplit = ValidarShareds();


            if (isComplit) {

                var data = {
                    OrderPaymentID: $('#hdnOrderPaymentID').val(),
                    DayValidate: $('#hdnDayValidate').val(),
                    RenegotiationConfigurationID: $('#hdnRenegotiationConfigurationID').val(),
                    DescuentoGlobal: $('#lblDetDiscount').val(),
                    NumberCuotas: $('#hdnNumberCuotas').val()
                }, t = $(this);

                $('#tablaShareds tbody:first tr').each(function (i) {

                    data['ListSharedDet[' + i + '].ValShared'] = $("#txtValShared" + i).val();
                    data['ListSharedDet[' + i + '].ExpirationDate'] = $("#txtExpirationDate" + i).val();

                });


                showLoading(t);

                $.post('<%= ResolveUrl("~/GeneralLedger/PaymentTickets/RegisterOrderPayments") %>', data, function (response) {
                    if (response.result) {
                        showMessage("Renegotiation was saved!", false);
                        hideLoading(t);
                        $('btnRenegotiate').attr('disabled', true);
                        $("#divRenegotiation").css("display", "none");
                        $("#divTitulo").css("display", "none");
                    } else {
                        showMessage(response.message, true);
                    }
                });
            }
        });

    });

    function ConvertirFecha(fecha) {
        var array_fecha = fecha.split("/")
        var dia = array_fecha[0]
        var mes = (array_fecha[1])
        var ano = (array_fecha[2])
        var fechaactual = new Date(ano, mes-1, dia)
        var fechaUltimaCalulada = new Date(fechaactual);
        return fechaUltimaCalulada;
    }

    function ValidarShareds() {
        var isComplete = true;
        var total = 0;
        var cuota = 0;
        var fechaactual = Date.now;
        var fecha = '';
        var fechaAnterior = '';
        var fechaUltimaIngresada = '';
        var fechaUltimaCalulada = '';
        var idfechaUltimaIngresada = '';
       
        fechaUltimaCalulada = ConvertirFecha($("#hdnLastDateExpirated").val());


        $('#tablaShareds tbody:first tr').each(function (i) {
            $("#txtExpirationDate" + i).clearError();
            $("#txtValShared" + i).clearError();
            fechaUltimaIngresada = ConvertirFecha($("#txtExpirationDate" + i).val());           
            idfechaUltimaIngresada = "#txtExpirationDate" + i;
        });


        $('#tablaShareds tbody:first tr').each(function (i) {

            cuota = parseFloat($("#txtValShared" + i).val());
            fecha = $("#txtExpirationDate" + i).val();

            if (cuota == '' || cuota <= 0) {
                $("#txtValShared" + i).showError('<%= Html.JavascriptTerm("InvalidRenegotiationShared","Please enter the shared") %>');
                isComplete = false;
            
            }

            if (fecha == '') {
                $("#txtExpirationDate" + i).showError('<%= Html.JavascriptTerm("InvalidRenegotiationExpirationDate","Please enter the Expiration Date") %>');
                isComplete = false;
             
            }



            if (fecha != '') {

                var array_fecha = fecha.split("/")
                var dia = array_fecha[0]
                var mes = (array_fecha[1])
                var ano = (array_fecha[2])
                fechaactual = new Date(ano, mes - 1, dia)
                d = new Date(fechaactual);

                if (!d || d.getFullYear() == ano && d.getMonth() == mes - 1 && d.getDate() == dia) {
                    var ok = '';
                } else {
                    $("#txtExpirationDate" + i).showError('<%= Html.JavascriptTerm("InvalidRenegotiationDateFormat","The date format is incorrect should be dd/mm/yyyy") %>');
                    isComplete = false;
                      
                }

                   if (fechaUltimaCalulada <= d && idfechaUltimaIngresada != "#txtExpirationDate" + i) 
                   {
                    $("#txtExpirationDate" + i).showError('<%= Html.JavascriptTerm("InvalidRenegotiationDateLessExpiration","The date must be less than the final expiration date calculated") %>');
                    isComplete = false;
                 
                } //&& idfechaUltimaIngresada != "#txtExpirationDate" + i

                if (fechaAnterior != '') {
                    if (fechaAnterior >= d) {
                        $("#txtExpirationDate" + i).showError('<%= Html.JavascriptTerm("InvalidRenegotiationDateOrder","The order of the dates is incorrect") %>');
                        isComplete = false;
                      
                    }
                }
                fechaAnterior = d;
            }

            total = total + cuota;


        });


        if ($('#spDetAPagar').val() != parseFloat(total).toFixed(2)) {
            //            $('.classShared').showError('<%= Html.JavascriptTerm("InvalidRenegotiationTotal", "The sum of the shares must be equal to total pay") %>');
            showMessage('<%= Html.Term("InvalidRenegotiationTotal", "The sum of the shares must be equal to total pay") %>', true);
            $('.classShared').showError('');
            isComplete = false;
         
        }

        if ($(idfechaUltimaIngresada).val() == $("#hdnLastDateExpirated").val()) {
            $(idfechaUltimaIngresada).clearError();
          
        }


        if (ConvertirFecha($(idfechaUltimaIngresada).val()) > fechaUltimaCalulada) {
            $(idfechaUltimaIngresada).showError('<%= Html.JavascriptTerm("InvalidRenegotiationDateExceed","The Date should not exceed the expiration date calculated") %>');
            isComplete = false;
           
        }

        return isComplete;
    }

    function ViewShareds(orderPaymentID, renegotiationConfigurationID, negotiationLevelID) {
        $('#hdnOrderPaymentID').val(orderPaymentID);
        $('#hdnRenegotiationConfigurationID').val(renegotiationConfigurationID);

        var url = '<%= ResolveUrl("~/GeneralLedger/PaymentTickets/GeListRenegotiationShares") %>';
        var odata = JSON.stringify({ TicketNumber: orderPaymentID ,
        RenegotiationConfigurationID:renegotiationConfigurationID,
        NegotiationLevelID:negotiationLevelID});

        $.ajax({
            data: odata,
            url: url,
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            success: function (response) {
                var html = response.Items;
                $('#spDetTotalAmount').val(response.TotalAmount);
                $('#spDetDiscount').val(response.Discount);
                //                alert(response.Discount);
                if (parseFloat(response.Discount) == 0) {
//                    $('#trDiscount').style.visibility = "hidden";
                    $('#spDetDiscount').css("display", "none");
                    $('#lblDetDiscount').css("display", "none");
                }
                else {
                    //$('#trDiscount').style.visibility = "visible";

                    $('#spDetDiscount').css("display", "block");
                    $('#lblDetDiscount').css("display", "block");
                }


                $('#spDetAPagar').val(response.TotalPay);
                $('#hdnDayValidate').val(response.DayValidate);
                $('#hdnLastDateExpirated').val(response.LastDateExpirated);

                $('#hdnNumberCuotas').val(response.NumShared);
                $("#tblShareds").html(html)
                $("#divShareds").css("display", "block");

            },
            error: function (error) {
            }
        });

    }

      </script>
        <a href="<%= ResolveUrl("~/GeneralLedger") %>">

        <%= Html.Term("GeneralLedger")%></a> > 
       <%-- <a href="<%= ResolveUrl("~/GeneralLedger/PaymentTickets") %>">--%>
       <a href="<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/BrowseTickets") %>">
        <%= Html.Term("PaymentTickets")%></a> > 
        <a href="<%= ResolveUrl("~/GeneralLedger/PaymentTickets/Renegotiation/" + Model[0].TicketNumber) %>">
        <%= Model[0].TicketNumber%></a> >
        <% if (Model[0].PaymentTypeID == 1)
           { %>         
               <%=Html.Term("TicketInformation", "Ticket Information")%>
         <% } else { %>         
               <%=Html.Term("Renegotation", "Renegotation")%> 
         <% } %>
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
