<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/TicketManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%
        Dictionary<int, string> dcNegotiationLevels = (ViewBag.dcNegotiationLevels as Dictionary<int, string>) ?? new Dictionary<int, string>();
        Dictionary<int, string> dcExpirationStatuses = (ViewBag.dcExpirationStatuses as Dictionary<int, string>) ?? new Dictionary<int, string>(); ;
        Dictionary<int, string> dcBank = (ViewBag.dcBank as Dictionary<int, string>) ?? new Dictionary<int, string>();
        Dictionary<int, string> dcCountry = (ViewBag.dcCountry as Dictionary<int, string>) ?? new Dictionary<int, string>();
     
    %>
      <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/json2.js") %>"></script>
   
    <script  type="text/javascript" >
        $(function () {
            var NameInputFilter = $('#NameInputFilter');
            NameInputFilter.attr("disabled", true);
            $('#AccountIDInputFilter')
                .removeClass('Filter')
                .watermark('<%= Html.JavascriptTerm("CitySearch", "Look up city") %>')
                .jsonSuggest('<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/AccountSearch") %>',
                    {
                        onSelect: function (item) {
                            $('#AccountIDInputFilter').val(item.id);
                            NameInputFilter.val(item.text);

                        },
                        minCharacters: 3,
                        source: NameInputFilter,
                        ajaxResults: true,
                        maxResults: 50,
                        showMore: true
                    }).blur(function () {
                        if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                            NameInputFilter.val('');
                        }
                    });

            $(".deactivateButton").hide()

        });
    </script>
<% Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.PaymetTycketsReportSearchData>("~/GeneralLedger/PaymentTicketsReport/GetPaymentTicketsReport")
.AutoGenerateColumns()
.AddInputFilter(Html.Term("Account", "Account"), "AccountID", startingValue:Convert.ToInt32(Request.QueryString["AccountId"]))
.AddInputFilter(Html.Term("Name", "Name"), "Name", startingValue: Request.QueryString["AccountName"])
.AddSelectFilter(Html.Term("NegociationLevel", "Negociation Level"), "LevelId", dcNegotiationLevels, startingValue: Convert.ToInt32(Request.QueryString["NegotiationLevelsId"]))

.AddInputFilter(Html.Term("ExpirationDate", "Expiration Date"), "startDateExpirationDate", (Request.QueryString["ExpirationDateDesde"]!=null)?Request.QueryString["ExpirationDateDesde"]:Html.Term("StarDate", "Star Date"), true)
.AddInputFilter(Html.Term("To", "To"), "endDateExpirationDate", (Request.QueryString["ExpirationDateHasta"] != null) ? Request.QueryString["ExpirationDateHasta"] : Html.Term("EndDate", "End Date")/*DateTime.Now.ToShortDateString()*/, true, true)

.AddInputFilter(Html.Term("LiquidationDate", "Liquidation Date"), "startDateLiquidationDate", (Request.QueryString["LiquidationDateInicio"] != null) ? Request.QueryString["LiquidationDateInicio"] : Html.Term("StarDate", "Star Date")/*new DateTime(1900, 1, 1).ToShortDateString()*/, true)
.AddInputFilter(Html.Term("To", "To"), "endDateLiquidationDate", (Request.QueryString["LiquidationDateFin"] != null) ? Request.QueryString["LiquidationDateFin"] : Html.Term("EndDate", "End Date")/*DateTime.Now.ToShortDateString()*/, true, true)

.AddInputFilter(Html.Term("Order", "Order"), "OrderId", (Request.QueryString["OrderId"] == "0") ? "" : Request.QueryString["OrderId"])
.AddInputFilter(Html.Term("TicketNumberFiltro", "Ticket Number"), "TicketNumber", (Request.QueryString["TicketNumber"] == "0") ? "" : Request.QueryString["TicketNumber"])
.AddInputFilter(Html.Term("FiscalNote", "Fiscal Note"), "FiscalNote", Request.QueryString["FiscalNote"])
.AddSelectFilter(Html.Term("ExpirationSituation", "Expiration Situation"), "ExpirationSituationId", dcExpirationStatuses, startingValue: Convert.ToInt32(Request.QueryString["ExpirationStatusesId"]))

//.AddInputFilter(Html.Term("LiquidationDate", "Liquidation Date"), "startOrderDate", (Request.QueryString["OrderDateInicio"] != null) ? Request.QueryString["OrderDateInicio"] : Html.Term("StarDate", "Star Date")/*new DateTime(1900, 1, 1).ToShortDateString()*/, true)
//.AddInputFilter(Html.Term("To", "To"), "endOrderDate", (Request.QueryString["OrderDateFin"] != null) ? Request.QueryString["OrderDateFin"] : Html.Term("EndDate", "End Date")/*DateTime.Now.ToShortDateString()*/, true, true)

.AddSelectFilter(Html.Term("Country", "Country"), "CountryId",dcCountry, startingValue: Convert.ToInt32(Request.QueryString["CountryId"]))
.AddSelectFilter(Html.Term("Bank", "Bank"), "BankId", dcBank, startingValue: Convert.ToInt32(Request.QueryString["BankId"]))


.ClickEntireRow()
.AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))
 
.Render(); %>
    <script type="text/javascript">

        $(function () {

            $('#noteModal').jqm({
                modal: false
            });
//            $("#ViewTicket").click(function () {
//            cargarBillingInformation();
//               
//            });
        });


        function verViewTicket() {
            $('#noteModal').jqmShow();
        }
        function CancelarViewTicket() {
            $('#noteModal').jqmHide();
        }

        
        //Obtener informacion del viewTicket y la exportacion a pdf de las filas seleccionadas
        function loadBillingInformation(OrderPaymentID) {
            var url = '<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/ExportarBoletaRepositorio") %>';
            // var data = ObtenerSeleccionados();
            var OrderPaymentIDs = new Array();
            OrderPaymentIDs.push(OrderPaymentID);

            var odata = JSON.stringify({ OrderPaymentID: OrderPaymentID });

            $.ajax({
                data: odata,
                url: url,
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                success: function (response) {
                    var response = response;
                    var html = response.html;
                    var BankCode = response.BankCode;

                    if (response.result) {                       
                        showMessage(response.message, false);
                    } else {                        
                        showMessage(response.message, true);
                    }
                },
               

                error: function (error) {
                }
            });
        }

        //Obtener informacion del viewTicket y la exportacion a pdf de las filas seleccionadas
        function cargarBillingInformation(OrderPaymentID) {
            var url = '<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/VerificarTipoCuenta") %>';
            // var data = ObtenerSeleccionados();
            var OrderPaymentIDs = new Array();
            OrderPaymentIDs.push(OrderPaymentID);

            var odata = JSON.stringify({ OrderPaymentID: OrderPaymentID });

            $.ajax({
                data: odata,
                url: url,
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                success: function (response) {
                    var response = response;
                    var html = response.html;
                    var BankName = 'nothing';// response.BankName;
                    var BankCode = response.BankCode;

                    if (response.IsCreditCard) {
                        $("#tblinformacion").html(html)
                        verViewTicket();
                    }
                    if (BankName != "") {
                        var url = '<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/ExportarBoleta") %>?';
                        url = url + "OrderPaymentID=" + OrderPaymentID + "&BankName=" + BankName + "&BankCode=" + BankCode;
                        $("#frmExportar").attr("src", url);
                    }
                },
                error: function (error) {
                }
            });
        }

        function ObtenerSeleccionados() {
            var listaSeleccionados = new Array();
            $("#paginatedGrid tbody tr").each(function () {
                var checkBox = $(this).find("td:eq(0) input:checkbox")
                if (checkBox.is(":checked")) {
                    var value = $(checkBox).val();
                    listaSeleccionados.push(value)
                }

            });
         return listaSeleccionados;
        }

        function Exportar()
        {
                var sortableColumns = $('#paginatedGrid th.sort');
                var currentSort = sortableColumns.filter('.currentSort');
                orderBy = currentSort.attr('id');
                orderByDirection = currentSort.attr('class').split(' ')[2];

            var data =
            {
                    page:0,
                    LevelId:(isNaN($("#LevelIdSelectFilter").val()) ||$.trim($("#LevelIdSelectFilter").val())=="")?"0":$("#LevelIdSelectFilter").val(),
                    pageSize:0,
                    startDateExpirationDate:$("#startDateExpirationDateInputFilter").val(),
                    endDateExpirationDate:$("#endDateExpirationDateInputFilter").val(),
                    startDateLiquidationDate:$("#startDateLiquidationDateInputFilter").val(),
                    endDateLiquidationDate:$("#endDateLiquidationDateInputFilter").val(),
                    OrderId: $("#OrderIdInputFilter").val(),
                    FiscalNote: $("#FiscalNoteInputFilter").val(),
                    ExpirationSituationId:$("#ExpirationSituationIdSelectFilter").val(),
                    startOrderDate:$("#startOrderDateInputFilter").val(),
                    endOrderDate:$("#endOrderDateInputFilter").val(),
                    CountryId:$("#CountryIdSelectFilter").val(),
                    BankId:$("#BankIdSelectFilter").val(),
                    orderBy: orderBy,
                    orderByDirection:orderByDirection
                };
                return data;
        }
        $(function () {
            $("#AccountIDInputFilter").addClass("Filter");

            var exportToExcel = $("#exportToExcel");
                exportToExcel.attr("target", "frmExportar");

            exportToExcel.click(
                 function () {
                     var url = '<%= ResolveUrl("~/GeneralLedger/PaymentTicketsReport/ExportarExcelGrilla") %>?';
                     var data = Exportar();
                     var query = $.param(data);
                     url = url + query;
                     $("#frmExportar").attr("src", url);
                 }
             );
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <div id="noteModal" class="jqmWindow LModal">
        <div class="mContent">
            <h2>
                <%= Html.Term("BillingInformation","billing information")%>
            </h2>
            <table class="FormTable Section" width="100%">
               <tbody id="tblinformacion">

                  <%--<tr>
                    <td class="FLabel">
                        <%= Html.Term("AccountNumber", "Account Number") %>
                    </td>
                    <td id="txtAccountNumber">
                    </td>
                </tr>
                  <tr>
                    <td class="FLabel">
                        <%= Html.Term("ExpirationDate", "Expiration Date") %>
                    </td>
                    <td id="txtExpirationDate">
                    </td>
                </tr>
                  <tr>
                    <td class="FLabel">
                        <%= Html.Term("AccountName", "Account Name") %>
                    </td>
                    <td id="txtAccountName">
                    </td>
                </tr>
                  <tr>
                    <td class="FLabel">
                        <%= Html.Term("Address", "Address") %>
                    </td>
                    <td id="txtAddress">
                    </td>
                </tr>
                  <tr>
                    <td class="FLabel">
                        <%= Html.Term("Country", "Country") %>
                    </td>
                    <td id="txtCountry">
                    </td>
                </tr>
                  <tr>
                    <td class="FLabel">
                        <%= Html.Term("PaymentStatus", "Payment Status") %>
                    </td>
                    <td id="txtPaymentStatus">
                    </td>
                </tr>
                  <tr>
                    <td class="FLabel">
                        <%= Html.Term("TransactionId", "Transaction ID") %>
                    </td>
                    <td id="txtTransactionId">
                    </td>
                </tr>--%>
               </tbody>
            </table>
            <p>
                <a href="javascript:void(0);" class="Button LinkCancel jqmClose" onclick="CancelarViewTicket()"
                    id="btnCancelObservacion">
                    <%= Html.Term("Close","Close")%></a>
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>
        <iframe   name ="frmExportar" id="frmExportar" style="display:none" src="">
        </iframe>
      
</asp:Content>
