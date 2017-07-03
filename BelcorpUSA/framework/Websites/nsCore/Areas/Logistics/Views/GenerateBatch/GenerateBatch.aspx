<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/RoutesManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/json2.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.Util.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.blockUI.js") %>"></script>
    <style type="text/css">
        .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset .Cerrar
        {
            font-family: Helvetica;
            font-size: 11px;
        }
        
        img.logo
        {
            float: right;
            border: none;
            margin: 10px 10px 0 0;
        }
        #main
        {
            padding: 0;
        }
        #main h1
        {
            padding: 15px 10px 15px 0;
        }
        dl
        {
            margin-top: 0;
        }
        dt
        {
            color: #a00;
            font-weight: bold;
        }
        #api dt, #debugging dt
        {
            font-size: 120%;
            margin: 5px 0;
        }
        dd
        {
            margin: 0 0 20px 0;
            color: #555;
        }
        dl.options
        {
            margin: 10px 25px;
        }
        code.inline
        {
            background-color: #ffc;
        }
        .footer
        {
            padding-top: 20px;
            margin-top: 30px;
            border-top: 1px solid #ddd;
            color: #666;
        }
        #header
        {
            background-color: #eee;
            font-weight: normal;
            margin: 0;
            padding: 10px;
            font-size: small;
        }
        
        h1, h2 .anchors a, .sampleAnchors a
        {
            font-family: 'trebuchet ms' , verdana, arial;
        }
        
        .anchors
        {
            background-color: #eee;
            border-bottom: 1px solid #ccc;
            zoom: 1;
        }
        .anchors, .sampleAnchors
        {
            list-style: none;
            margin: 0;
            padding: 0 0 1px;
        }
        .anchors:after, .sampleAnchors:after
        {
            display: block;
            clear: both;
            content: " ";
        }
        .anchors li
        {
            float: left;
            margin: 0 1px 0 0;
        }
        .anchors a, .sampleAnchors a
        {
            display: block;
            position: relative;
            top: 1px;
            border: 1px solid #ccc;
            border-bottom: 0;
            z-index: 2;
            padding: 2px 9px 1px;
            color: #444;
            text-decoration: none;
        }
        .anchors .on a
        {
            padding-bottom: 2px;
            font-weight: bold;
        }
        .anchors a:focus, .anchors a:active, .sampleAnchors a:focus, .sampleAnchors a:active
        {
            outline: none;
        }
        .anchors .on a, .anchors a:hover, .anchors a:focus, .anchors a:active
        {
            background: #fff;
        }
        .anchors .on a:link, .anchors .on a:visited
        {
            cursor: text;
        }
        .anchors a:hover, .anchors a:focus, .anchors a:active
        {
            cursor: pointer;
        }
        .on
        {
            display: block;
        }
        .anchors a h2
        {
            padding: 0;
            margin: 2px;
        }
        .tabs-selected a
        {
            background-color: #fff;
            color: #000;
            font-weight: bold;
            border-bottom: 1px solid #fff;
            margin-bottom: -1px;
            overflow: visible;
        }
        
        .tabContent
        {
            padding: 10px 25px;
            clear: left;
            background-color: #fff;
            margin: 0 0 30px 0;
            zoom: 1;
        }
        .sampleTabContent
        {
            padding: 10px 25px;
            clear: left;
            background-color: #fff;
            margin: 0 0 50px 0;
            border: 1px solid #ddd;
            zoom: 1;
        }
        .tabs-hide
        {
            display: none;
            background-color: #fff;
        }
        
        .sampleAnchors a
        {
            display: block;
        }
        .sampleAnchors li
        {
            float: left;
            margin: 0 1px 0 0;
        }
        .sample-tab-selected a
        {
            background-color: #fff;
            color: #000;
            font-weight: bold;
            border-bottom: 1px solid #fff;
            margin-bottom: -1px;
            overflow: visible;
        }
        
        
        div.growlUI
        {
            background: url(check48.png) no-repeat 10px 10px;
        }
        div.growlUI h1, div.growlUI h2
        {
            color: white;
            padding: 5px 5px 5px 75px;
            text-align: left;
        }
        div.growlUI h2
        {
            font-size: medium;
        }
    </style>
    <script type="text/javascript">

        var ObjIds = {
            LogisticProviderID: 0
        };

        //        function LoadInitialMessage() {
        //            var WarehouseID = $("#WarehouseIDSelectFilter").val();
        //            if (WarehouseID == '0') {
        //                alert('PLEASE ENTER INFORMATION FOR SEARCHING');
        //            }
        //        }

        function IsValid() {
            $('div.GridFilters a.filterButton').click(function () {
                var WarehouseID = $("#WarehouseIDSelectFilter").val();

                if (WarehouseID == '0') {
                    alert('WAREHOUSE MUST BE INCLUDE FOR SEARCHING');
                    return false;
                }
            });
        }

        $(document).ready(function () {
            $('#chkboxSelectAll1').change(function () {
                var checkboxSelectAll = $(this);
                //                 if (checkboxSelectAll.is(':checked')) { alert('marcado') } else { alert('desmarcado') } return 
                $("#paginatedGrid tbody tr").each(function () {
                    var checkbox = $(this).find("td:eq(0) input:checkbox")

                    //                     if (checkbox.is(":checked")) {
                    //                         marcadosElimar.push(checkbox.val())
                    //                     }

                    if (checkboxSelectAll.is(':checked')) {
                        checkbox.prop('checked', true);
                    } else {
                        checkbox.prop('checked', false);
                    }

                });
            });
        });

        $(function () {
            $('#WarehouseIDSelectFilter').change(function () {
                var selectedItem = $(this).val();
                var ddlWarehousePrinters = $("#ddlWarehousePrinters");

                $.ajax({
                    cache: false,
                    type: "GET",
                    url: '<%= ResolveUrl("~/Logistics/GenerateBatch/ListWarehousePrinters") %>',
                    data: { "WarehouseID": selectedItem },
                    success: function (data) {
                        ddlWarehousePrinters.html('');
                        ddlWarehousePrinters.append($('<option></option>').val('0').html('<%=Html.Term("SelectItem", "Select Item")%>'));
                        $.each(data, function (a, item) {
                            ddlWarehousePrinters.append($('<option></option>').val(item.Value).html(item.Text));
                        });
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                           ddlWarehousePrinters.html('');
                    }
                });

            });
            IsValid();
            //            LoadInitialMessage();

            $("#idsInputFilter").val(JSON.stringify(ObjIds));

            $("#idsInputFilter").appendTo($("#paginatedGridFilters"));
            $("#strLstOrderCustomerIDsInputFilter").appendTo($("#paginatedGridFilters"));

            //var btnEliminar ="<div class='FL RunFilter'>"
            //    btnEliminar =btnEliminar+"<a class='Button ModSearch filterButton' href='javascript:void(0);'>Apply Filter</a> <span class='ClearAll'></span>"
            //    btnEliminar = btnEliminar + "</div>"

            $("#DeleteSelected").click(function () {
                eliminarSeleccionados();
            });

            $("#paginatedGrid thead tr:eq(0) th:eq(0)").empty();


            $("#StartDateInputFilter").datepicker({
                changeMonth: true, changeYear: true
            });
            $("#EndDateInputFilter").datepicker({
                changeMonth: true, changeYear: true
            });

            //Warehouse ini

            //            var WarehouseID = $('<input type="hidden" id="WarehouseID" class="Filter" />').val('');
            //            $('#WarehouseIDInputFilter').change(function () {
            //                WarehouseID.val('');
            //            });
            //            $('#WarehouseIDInputFilter').removeClass('Filter').after(WarehouseID).css('width', '200px').val('').watermark('<%= Html.JavascriptTerm("WarehouseSearch", "Look up Ware house by ID or Name") %>')
            //                .jsonSuggest('<%= ResolveUrl("~/Logistics/GenerateBatch/WarehouseSearch") %>',
            //                    {
            //                    onSelect: function (item) {
            //				    WarehouseID.val(item.id);
            //				    $('#WarehouseID').val(item.id);
            //                    },minCharacters: 3,ajaxResults: true,maxResults: 50,showMore: true
            //                   });

            //Warehouse fin

            //Material ini

            var MaterialID = $('<input type="hidden" id="MaterialID" class="Filter" />').val('');
            $('#MaterialIDInputFilter').change(function () {
                MaterialID.val('');
            });

            $('#MaterialIDInputFilter').removeClass('Filter').after(MaterialID).css('width', '275px').val('').watermark('<%= Html.JavascriptTerm("MaterialSearch", "Look up Material by ID or Name") %>')
                .jsonSuggest('<%= ResolveUrl("~/Materials/SearchFilter") %>',
                    {
                        onSelect: function (item) {
                            MaterialID.val(item.id);
                            $('#MaterialID').val(item.id);
                        }, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
                    });

            //Material fin

            //Account ini

            var accountId = $('<input type="hidden" id="accountIdFilter" class="Filter" />').val('');
            $('#accountNumberOrNameInputFilter').change(function () {
                accountId.val('');
            });
            $('#accountNumberOrNameInputFilter').removeClass('Filter').after(accountId).css('width', '172px')
				.watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
				    accountId.val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});

            //Account fin

            // Product ini

            var productSelected = false;
            var productoId = $('<input type="hidden" id="productoIdFilter" class="Filter" />');
            $('#productInputFilter').removeClass('Filter').css('width', '275px').watermark('<%=Html.JavascriptTerm("ProductSearch","Look up product by ID or name") %>').jsonSuggest('<%=ResolveUrl("~/Products/Promotions/Search") %>',
                        { onSelect: function (item) {
                            productoId.val(item.id);
                            productSelected = false;
                        }, minCharacter: 3,
                            source: $('#productInputFilter'),
                            ajaxResults: true,
                            maxResult: 50,
                            showMore: true
                        }).blur(function () {
                            if (!$(this).val() || !$(this).val() == $(this).data('watermark')) {
                                productoId.val('');
                            }
                            productSelected = false;
                        }).after(productoId);

            // Product fin

            // LogisticProvider ini

            var LogisticsProviderID = $('<input type="hidden" id="LogisticsProviderID" class="Filter" />').val('');

            $('#logisticsProviderInputFilter').change(function () {
                LogisticsProviderID.val('');
                SetParamSearchGridView("Name", $('#logisticsProviderInputFilter').val());
            });
            $('#logisticsProviderInputFilter').removeClass('Filter').after(LogisticsProviderID).css('width', '275px')
				.val('')
				.watermark('<%= Html.JavascriptTerm("logisticsProviderSearch", "Look up Prov. by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Logistics/searchProv") %>', { onSelect: function (item) {
				    LogisticsProviderID.val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});

            // LogisticProvider fin

            //Route ini

            //            var query = $('#RouteIDInputFilter').val();
            var RouteID = $('<input type="hidden" id="RouteID" class="Filter" />').val('');
            $('#RouteIDInputFilter').change(function () {
                RouteID.val('');
            });

            $('#RouteIDInputFilter').removeClass('Filter').after(RouteID).css('width', '190px').val('').watermark('<%= Html.JavascriptTerm("TransporterSearch", "Look up Rotation by ID") %>')
                .jsonSuggest(GetUrl(),
                    {
                        onSelect: function (item) {
                            //                            $('#RouteIDInputFilter').val(item.text);
                            //                            ObjIds["RouteID"] = item.id;
                            //                            $("#idsInputFilter").val(JSON.stringify(ObjIds));
                            RouteID.val(item.id);
                            $('#RouteID').val(item.id);
                        },
                        minCharacters: 3,
                        ajaxResults: true,
                        maxResults: 50,
                        showMore: true
                    });

            //Route fin


            $(".deactivateButton").hide()

        });
        function GetUrl() {

            //            var urlRouteXLogisticProviderSearch = '<%= ResolveUrl("~/Logistics/GenerateBatch/RouteXLogisticProviderSearch") %>'
            var urlRouteXLogisticProviderSearch = '<%= ResolveUrl("~/Logistics/Routes/searchRoutes") %>'
            return urlRouteXLogisticProviderSearch;
        }
        function retornaMarcados() {
            var marcadosElimar = [];
            $("#paginatedGrid tbody tr").each(function () {
                var checkbox = $(this).find("td:eq(0) input:checkbox")
                if (checkbox.is(":checked")) {
                    marcadosElimar.push(checkbox.val())
                }

            });
            return marcadosElimar;
        }

        function eliminarSeleccionados() {
            var CheckEliminar = retornaMarcados();
            var srtCheckEliminar = JSON.stringify(CheckEliminar)
            $("#strLstOrderCustomerIDsInputFilter").val(srtCheckEliminar);
            $(".filterButton").click()
            $("#strLstOrderCustomerIDsInputFilter").val("");
        }

        //        function PutSelectAll() {
        //            $('#chkboxSelectAll1').click(function () { alert('marco/desmarco') });
        //        }

        function generarProceso() {
            //            
            var url = '<%= ResolveUrl("~/Logistics/GenerateBatch/GenerarLY037") %>';
            var ArrInsertar = new Array();
            $("#paginatedGrid tbody tr").each(function () {
                var checkbox = $(this).find("td:eq(0) input:checkbox");
                if (checkbox.length > 0) {
                    if (checkbox.is(":checked")) {
                        if (ArrInsertar == undefined) {
                            ArrInsertar = new Array();
                        }
                        ArrInsertar.push(checkbox.val());
                    }
                }

            });

            if ((ArrInsertar || new Array()).length == 0) {
                showMessage('<%= Html.Term("SelectedRecord", "Select a Record") %>', true);
                return false;
            }

            if ($('#ddlWarehousePrinters').val() == '0' || $('#ddlWarehousePrinters').val() == '') {
                showMessage('<%= Html.Term("WarehousePrinterIsRequired", "Warehouse Printer Is Required") %>', true);
                return false;
            }

            if (ArrInsertar.length > 0) {

                $('.BreadCrumb').block({
                    message: '<h1> <img src="/Content/Images/Icons/loading-blue.gif" alt="Loading" /><%= Html.Term("ProcessingRequest", "Processing Request Please wait") %></h1>',
                    css: { border: '3px solid #0078ba' }
                });

            }
            var data = JSON.stringify({
                LstOrderIds: ArrInsertar,
                WareHouseID: $('#WarehouseIDSelectFilter').val()
            });

            $.ajax({
                data: data,
                url: url,
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                success: function (response) {


                    if (!response.Estado) {
                        showMessage(response.message, !response.Estado);
                    } else {
                        showMessage(response.message);
                        location.reload();

                        var data = response.ArchivoGenerados;
                        var ArchivosCreadosSap = response.ArchivosCreadosSap;
                        var ArchivosCreadosRapidao = response.ArchivosCreadosRapidao;

                        $('.BreadCrumb').unblock();
                        for (var index = 0; index < data.length; index++) {
                            //                              alert('aqui entro');
                            //                              window.open("/" + data[index]);

                        }
                        //                        showMessage('<%= Html.Term("BatchGeneratedOK", "Batch Successfully Generated") %>');
                    }

                    $(".filterButton").click();

                },
                error: function (error) {
                    $('.BreadCrumb').unblock();
                }
            })
        }
        function cargarLogisticProvider(LogisticProviderID) {
            var data = JSON.stringify({
                LogisticProviderID: LogisticProviderID
            });
            var url = '<%= ResolveUrl("~/Logistics/GenerateBatch/cargarLogisticsProvider") %>';
            $.ajax({
                async: false,
                data: data,
                url: url,
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                success: function (response) {

                },
                error: function (error) {

                }
            })
        }        
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <%
        Dictionary<int, string> dcWareHouse = (ViewBag.WareHouse as Dictionary<int, string>) ?? new Dictionary<int, string>();
        Dictionary<int, string> dcperiodsFrom = (ViewBag.periodsFrom as Dictionary<int, string>) ?? new Dictionary<int, string>(); ;
        Dictionary<int, string> dcperiodsTo = (ViewBag.periodsTo as Dictionary<int, string>) ?? new Dictionary<int, string>();
        Dictionary<int, string> dcShippingMethods = (ViewBag.ShippingMethods as Dictionary<int, string>) ?? new Dictionary<int, string>();

        Dictionary<string, string> dcWareHouses = (ViewBag.WareHouses as Dictionary<string, string>) ?? new Dictionary<string, string>();
        Dictionary<int, string> dcAccountTypes = (ViewBag.AccountTypes as Dictionary<int, string>) ?? new Dictionary<int, string>();
        Dictionary<int, string> dcOrderTypes = (ViewBag.OrderTypes as Dictionary<int, string>) ?? new Dictionary<int, string>();
        //Dictionary<int, string> dcListWarehousePrinters = (ViewBag.ListWarehousePrinters as Dictionary<int, string>) ?? new Dictionary<int, string>();
        
    %>
    <% Html.PaginatedGrid<OrderToBatch>("~/Logistics/GenerateBatch/ConsultaGenerateBatchSeparation")
        //   .AutoGenerateColumns()
        //.HideClientSpecificColumns_()
                .AddOption("DeleteSelected", Html.Term("DeleteSelected", "DeleteSelected"))
                .AddInputFilter(Html.Term("AccountNumberOrName", "Account Number or Name"), "accountNumberOrName")
                .AddInputFilter(Html.Term("Material", "Material"), "MaterialID")
                .AddInputFilter(Html.Term("Product"), "product")
                .AddInputFilter(Html.Term("logisticsProvider", "logisticsProvider"), "logisticsProvider")
                .AddInputFilter(Html.Term("Route", "Route"), "RouteID")
                .AddInputFilter(Html.Term("OrderDateRange", "Order Date Range"), "StartDate", startingValue: Html.Term("StarDate", "Star Date"))
                .AddInputFilter(Html.Term("To", "To"), "EndDate", startingValue: Html.Term("EndDate", "End Date"))
                .AddSelectFilter(Html.Term("WareHouse2", "* WareHouse"), "WarehouseID", new Dictionary<string, string>() { { "0", Html.Term("SelectItem", "Select Item") } }.AddRange(dcWareHouses))
                .AddSelectFilter(Html.Term("OrderPeriodRange", "Order Period Range"), "PeriodID", new Dictionary<int, string>() { { 0, Html.Term("SelectItem", "Select Item") } }.AddRange(dcperiodsFrom))
                .AddSelectFilter(Html.Term("To", "To"), "PeriodID2", new Dictionary<int, string>() { { 0, Html.Term("SelectItem", "Select Item") } }.AddRange(dcperiodsTo))
                .AddSelectFilter(Html.Term("ShipmentMethod", "ShipmentMethod"), "ShippingMethodID", new Dictionary<int, string>() { { 0, Html.Term("SelectItem", "Select Item") } }.AddRange(dcShippingMethods))
                .AddSelectFilter(Html.Term("AccountType", "Account Type"), "AccountTypeID", new Dictionary<int, string>() { { 0, Html.Term("SelectItem", "Select Item") } }.AddRange(dcAccountTypes))
                //.AddSelectFilter(Html.Term("OrderType", "Order Type"), "OrderTypeID", new Dictionary<int, string>() { { 0, Html.Term("SelectItem", "Select Item") } }.AddRange(dcOrderTypes))
           //.AddSelectFilter(Html.Term("WarehousePrinte", "Warehouse Printe"), "WarehousePrinterID", new Dictionary<int, string>() { { 0, Html.Term("SelectItem", "Select Item") } }.AddRange(dcListWarehousePrinters))
                .AddInputFilter(Html.Term("OrderNumber", "OrderNumber"), "OrderNumber")

                .AddColumn(Html.Term("Order", "Order#"), "OrderID")
                .AddColumn(Html.Term("Order", "Order#"), "OrderID")
                .AddColumn(Html.Term("Period", "Period"), "Period")
                .AddColumn(Html.Term("CompleteDate", "Complete Date"), "CompleteDate")
                .AddColumn(Html.Term("ItemsQty", "Items QTY"), "Quantity")
                .AddColumn(Html.Term("Amount", "Amount"), "Amount")
                .AddColumn(Html.Term("Consultant", "Consultant"), "Consultant")
                .AddColumn(Html.Term("Transporter", "Transporter"), "Transporter")
                .AddColumn(Html.Term("Route", "Route"), "Route")
                .AddColumn(Html.Term("CityState", "City/State"), "CityState")
                .AddColumn(Html.Term("ShipmentMethod", "Shipment Method"), "ShipmentMethod")

           //.AddSelectFilter(Html.Term("Plan"), "planId", new Dictionary<string, string>() { { "0", Html.Term("SelectaPlan", "Select a Plan...") } }.AddRange(SmallCollectionCache.Instance.Plans.ToDictionary(p => p.PlanId.ToString(), p => p.Name)), startingValue: 1)
           //.AddSelectFilter(Html.Term("BonusType"), "bonusTypeId", new Dictionary<string, string>() { { "0", Html.Term("SelectaCommissionRuleType", "Select a CommissionRuleType...") } }, startingValue: 1)
                .ClickEntireRow()
                .Render(); %>
    <input class="Filter" type="hidden" id="idsInputFilter" />
    <input class="Filter" type="hidden" id="strLstOrderCustomerIDsInputFilter" />
    <table style="width: 100%">
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top; width: 30%">
                <label for="chkboxSelectAll1">
                    <%=Html.Term("SelectAll", "SelectAll")%></label>
                <input id='chkboxSelectAll1' type='checkbox' class='checkAll' title='Select All...' />
            </td>
            <td style="padding-top: 5px; width: 40%; text-align: center">
                <%=Html.Term("WarehousePrinter", "Warehouse Printer")%>
                <select id="ddlWarehousePrinters">
                    <option value="0"><%=Html.Term("SelectItem", "Select Item")%></option>
                </select>
                <br />
                <hr />
                <input onclick="generarProceso()" id="btnProcesar" class="Button BigBlue" type="button"
                    value="<%=Html.Term("Generate", "Generate") %>" />
            </td>
            <td style="width: 30%">
            </td>
        </tr>
    </table>
    <div style="width: 100%; height: 100%; overflow: hidden" id="pnlXml">
    </div>
    <div id="pnlMensaje" style="display: none; height: 100%; width: 100%; position: fixed;
        left: 0px; top: 0px; z-index: 2999; opacity: 0.5; background-color: #efefef">
        <h1>
            <%=Html.Term("ProcessRequest", "Process Request")%>
        </h1>
    </div>
</asp:Content>
