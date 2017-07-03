<%@ Page Title="" Language="C#"MasterPageFile="~/Areas/CTE/Views/Shared/CTE.Master"
 
 %>

<%--//@01 20150817 BR-CC-002 G&S LIB: Se crea la pantalla--%>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
//            $.datepicker.regional['es'] = {
//                closeText: 'Cerrar',
//                prevText: '<Ant',
//                nextText: 'Sig>',
//                currentText: 'Hoy',
//                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
//                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
//                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
//                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
//                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
//                weekHeader: 'Sm',
//                dateFormat: 'dd/mm/yy',
//                firstDay: 1,
//                isRTL: false,
//                showMonthAfterYear: false,
//                yearSuffix: ''
//            };

//            $.datepicker.setDefaults($.datepicker.regional['es']);





        });
    </script>

    <link href="../../../../Content/CSS/Validation.css" rel="stylesheet" type="text/css" />
    <script src="../../../../Scripts/jquery.number.min.js" type="text/javascript"></script>
    <script src="../../../../Scripts/jquery.numeric.js" type="text/javascript"></script>
    <script src="../../../../Scripts/Validaciones.js" type="text/javascript"></script>
    <script src="../../../../Scripts/jquery.filestyle.js" type="text/javascript"></script>
  
    <script type="text/javascript">
        $(function () {



            $('#ledgerContainer').ajaxStop(function () {
                $('#ledgerContainer .Negative').css('color', 'Red');
            });




            $("#txtTicketNumer").keyup(function (event) {

                if (event.keyCode == 13) {

                    var _ticketNumber = $("#txtTicketNumer").val();
                    var _logErrorBankPaymentID = $('#hdnlogErrorBankPaymentID').val();
                   
                    var url = '<%= ResolveUrl("~/CTE/PaymentApplication/ApplyManualPayment") %>';

                    var odata = JSON.stringify({ ticketNumber: _ticketNumber,
                        logErrorBankPaymentID: _logErrorBankPaymentID
                    });

                    $.ajax({
                        data: odata,
                        url: url,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        success: function (response) {
                            if (response.result) {
                                $("#noteModal").css("display", "none");
                                showMessage(response.message, false);
                                window.location = '<%= ResolveUrl("~/CTE/PaymentApplication/Index") %>'  
                            } else {
                                $("#noteModal").css("display", "none");
                                showMessage(response.message, true);
                            }
                        },
                        error: function (error) {

                        }
                    });

                }
            });



            $('#btnAceptar').click(function () {

              

                    var _ticketNumber = $("#txtTicketNumer").val();
                    var _logErrorBankPaymentID = $('#hdnlogErrorBankPaymentID').val();
                 
                    var url = '<%= ResolveUrl("~/CTE/PaymentApplication/ApplyManualPayment") %>';

                    var odata = JSON.stringify({ ticketNumber: _ticketNumber,
                        logErrorBankPaymentID: _logErrorBankPaymentID
                    });

                    $.ajax({
                        data: odata,
                        url: url,
                        dataType: "json",
                        type: "POST",
                        contentType: "application/json",
                        success: function (response) {
                            if (response.result) {
                                $("#noteModal").css("display", "none");
                                showMessage(response.message, false);

                                window.location = '<%= ResolveUrl("~/CTE/PaymentApplication/Index") %>'                             

                            } else {
                                $("#noteModal").css("display", "none");
                                showMessage(response.message, true);
                            }
                        },
                        error: function (error) {

                        }
                    });

                
            });


        });
	</script>
    <style type="text/css">
        .file-fake
        {
            padding: 6px 2px 7px;
            border: 1px solid #ddd;
        }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">



<div id="noteModal" class="jqmWindow LModal" style="left: 700px; z-index: 800; top: 100px;" >

        <div class="mContent">       
         <table id="campaignAction" class="FormTable" width="430px">
            <tr id="valuePanel">          
            <td id="taskAdd"> 

            <table>
            <tr>
            	<td class="FLabel">
					<%= Html.Term("TicketNumber", "TicketNumber")%>:
				</td>
               <td>     
               <input type="hidden"  id="hdnlogErrorBankPaymentID"  />
			     <input id="txtTicketNumer" type="text" class="clear required" size="60px"  maxlength="50"  />
			   </td>
            </tr>
             
            </table>
            </td>
            </tr>
            </table>
            <p>

              <a href="javascript:void(0);" class="Button LinkCancel jqmClose" 
                    id="A1" onclick="Ocultar()">
                    <%= Html.Term("Close","Close")%></a>

               
                <a  id="btnAceptar" href="javascript:void(0);" class="Button BigBlue">
                    <%= Html.Term("Aceptar", "Aceptar") %></a>
            
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>

  


	
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% if (ViewData["Success"] != null) { %>
    <div id="successMessage">
        <%: ViewData["Success"] %>
    </div>
<% } %>

 <%       
        Dictionary<int, string> dcBank = (ViewBag.dcBank as Dictionary<int, string>) ?? new Dictionary<int, string>();
        Dictionary<int, string> dcStatusLog = (ViewBag.StatusLog as Dictionary<int, string>) ?? new Dictionary<int, string>();
        
        
    %>

  
 
      <div class="SectionHeader">
		<h2>
			<%=  Html.Term("BrowseBankConsolidateApplication", "Browse Bank Consolidate  Application")%>
        </h2>
        
        <%--<%= Html.Term("BrowsePaymentRulesConfiguration", "Browse Payment Rules Configuration") %> | <a href="<%= ResolveUrl("~/CTE/InterestRules/Index") %>"><%= Html.Term("GenerateRules", "Generate Rules")%></a>--%>
	</div>
    <div id="ledgerContainer">
     

    <% Html.PaginatedGrid<LogErrorBankPaymentsSearchData>("~/CTE/PaymentApplication/GetPaymentApplication")
            .AutoGenerateColumns()
            .AddSelectFilter(Html.Term("Bank", "Bank"), "BankId", dcBank, startingValue: Convert.ToInt32(Request.QueryString["BankId"]))
         
            
            .AddInputFilter(Html.Term("DateBankLog", "DateBankLog"), "DateBankLog", isDateTime: true)
            .AddInputFilter(Html.Term("DateProcess", "DateProcess"), "DateProcess", isDateTime: true)
            .AddInputFilter(Html.Term("FileSequenceLog", "FileSequenceLog"), "FileSequenceLog")
            .AddSelectFilter(Html.Term("StatusLog", "StatusLog"), "StatusLog", dcStatusLog, startingValue: Convert.ToInt32(Request.QueryString["StatusLog"]))   
            //.AddOption("ProcesarPago", Html.Term("ProcesarPago", "Procesar Pago") )
         
              
           .ClickEntireRow()
         
		    .Render(); 
    %>
     
    </div>
      <div id="newsletterModal" class="jqmWindow LModal">
        <div id="newsletterModalSub"  >
          
        </div>
          <p >
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Close", "Close") %></a>
            </p>

              
    </div>

   

    <script type="text/javascript">

        $(document).ready(function () {
            var ApplyFilter = "<a id='ApplyFilter'  class='Button ModSearch filterButton' href='javascript:void(0);'>Apply Filter</a>";
            
            var botonProcesarPago = "                                  <a href='javascript:void(0);' id='ProcesarPago' class='Button BigBlue'>Procesar Pago</a>";
            $('.RunFilter').append(botonProcesarPago);


            $(".RunFilter span").each(function (index) {
                $(this).removeClass();
                $(this).remove();
            }) 
        });


        $(function () {
            $('#noteModal').jqm({
                modal: false
            });

            




            $('#ProcesarPago').click(function () {

                if (confirm('¿Estas seguro de realizar el proceso?')) {

                    var oid = 0, t = $(this);
                    var j = 0;
                    $('#paginatedGrid tbody:first tr').each(function (i) {
                        if ($(this).find('td:first input[type=checkbox ]').is(':checked')) {
                            //odata['items[' + j + ']'] = $(this).find('td:first input[type=checkbox]').val();
                            oid = $(this).find('td:first input[type=checkbox]').val();
                            j++;
                        }
                    });

                    $('#hdnlogErrorBankPaymentID').val(oid);

                    if (j == 1) {

                        var url = '<%= ResolveUrl("~/CTE/PaymentApplication/validarPayment") %>';

                        var odata = JSON.stringify({ id: oid });

                        $.ajax({
                            data: odata,
                            url: url,
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json",
                            success: function (response) {
                                if (response.result) {
                                    //                                    var html = response.TicketNumber;
                                    $('#txtTicketNumer').val(response.TicketNumber);
                                    //  $("#tblinformacion").html(html)
                                    // $('#noteModal').jqmShow();
                                    $("#noteModal").css("display", "block");
                                } else {
                                    showMessage(response.message, true);
                                }
                            },
                            error: function (error) {

                            }
                        });
                    } else {
                        showMessage('<%=Html.JavascriptTerm("SelecPaymentOnlyOne", "Select only one Payment to Process")%>', true);
                    }

                }
            });

        });

        function Ocultar() {
            $("#noteModal").css("display", "none");
        };

     

    </script>
</asp:Content>


