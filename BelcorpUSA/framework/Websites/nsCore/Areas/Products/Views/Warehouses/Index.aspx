<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Warehouses/Warehouses.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.WarehouseProduct>>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        function LoadInitialMessage() {
            var cuv = $("#warehouseSelectFilter").val();
        }
        //csti - mescobar -08/03/2016 - EB-478 - inicio
        $(function () {
            $('#noteModal').jqm({
                modal: false
            });

        });
        //----
        function Ocultar() {
            $("#noteModal").css("display", "none");
        };
        //csti - mescobar -08/03/2016 - EB-478 - fin 

        function exportE() {
            $('#exportToExcel').click(function () {

                var wareHouse = $('#warehouseSelectFilter').val() == '' ? 0 : $('#warehouseSelectFilter').val();
                var productCod = $('#productoIdFilter').val() == '' ? 0 : $('#productoIdFilter').val();
                var materialCod = $('#materialIdFilter').val() == '' ? 0 : $('#materialIdFilter').val();

                var url = '<%=ResolveUrl("~/Warehouses/ItemsWareHouseInvetoryExport")%>' + '?';
                url += 'wareHouse=' + wareHouse;
                url += '&' + 'productCod=' + productCod;
                url += '&' + 'materialCod=' + materialCod;

                if (wareHouse == '') {
                    alert('PLEASE SELECT WAREHOUSE FOR SEARCHING');
                } else {
                    window.location = url;
                }
            });
        }
        //csti - mescobar -08/03/2016 - EB-478 - Inicio 
        function viewReemplacements(id) {

            var parentProductID = id.substring(0, id.indexOf('-'));
            var warehouseMaterialID = id.substring(id.indexOf('-') + 1);
            var url = '<%= ResolveUrl("~/Products/Warehouses/ListWareHouseInventoryReplacement") %>';
            var odata = JSON.stringify({ ParentProductID: parentProductID, WarehouseMaterialID: warehouseMaterialID });

            $.ajax({
                data: odata,
                url: url,
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                success: function (response) {
                    if (response.data.length > 0) {
                        var html = response.data;
                        $("#tblinformacion").html(html)
                        $('#noteModal').jqmShow();
                        $("#noteModal").css("display", "block");
                    } else {
                        $('#addMessage').jqmShow();
                        $("#addMessage").css("display", "block");
                    }
                },
                error: function (error) {
                }
            });
        }
        //csti - mescobar -08/03/2016 - EB-478 - Inicio

        $(function () {


            //StartQuitar 
            $('#txtQuantity').numeric();
            LoadInitialMessage();
            exportE();

            $("#btnClickPopup").click(function () {
                $("#divFormulario").load("/Accounts/LedgerSupport/", function (e) {
                    //Implementar la funcionalidad del jquery
                    $('#divFormulario').jqmShow();
                    //Validar informacion y la carga
                    $.ajax({
                        type: 'POST',
                        url: '/LedgerSupport/ValidResult',
                        data: ({ accountID: 1001,
                            orderID: 142
                        }),
                        asyn: false,
                        success: function (data) {
                            if (data.success == true) {
                                $('#txtOrder').val(data.orderId);
                                $('#txtSupportTicketNumber').val(data.supportTicketId);
                                $('#ddlEntryReason').val(data.entryReasonId);
                                $('#ddlEntryType').val(data.entryTypeId);
                                $('#ddlBonusType').val(data.BonusTypeId);
                            }
                        }
                    });
                });
            });

            //csti - mescobar - 08/03/2016 - EB-478 - inicio
            $('.btnViewReemplacements').live('click', function () {                
                viewReemplacements(this.id);
            });
            //-----
            $('#noteModal').jqm({ modal: false, onShow: function (h) {
                h.w.css({
                    top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });
            //-----
            $('#addMessage').jqm({ modal: false, onShow: function (h) {
                h.w.css({
                    top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            } 
            });
            //csti - mescobar - 08/03/2016 - EB-478 - Fin

            $('#divFormulario').jqm({ modal: false, onShow: function (h) {
                h.w.css({
                    top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });


            //EndQuitar

            //CSTI - FHP
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

            $('#productInputFilter').focus(function () {
                $('#productoIdFilter, #productInputFilter').val('');
            });
            //CSTI - FHP
            var materialSelected = false;
            var materialId = $('<input type="hidden" id="materialIdFilter" class="Filter" />');
            $('#materialInputFilter').removeClass('Filter').css('width', '275px').watermark('<%=Html.JavascriptTerm("MaterialtSearch","Look up material by ID or name") %>').jsonSuggest('<%=ResolveUrl("~/Products/Materials/SearchFilter") %>',
                        { onSelect: function (item) {
                            materialId.val(item.id);
                            materialSelected = false;
                        }, minCharacter: 3,
                            source: $('#materialInputFilter'),
                            ajaxResults: true,
                            maxResult: 50,
                            showMore: true
                        }).blur(function () {
                            if (!$(this).val() || !$(this).val() == $(this).data('watermark')) {
                                materialId.val('');
                            }
                            materialSelected = false;
                        }).after(materialId);

            $('#materialInputFilter').focus(function () {
                $('#materialIdFilter, #materialInputFilter').val('');
            });
            //CSTI - FHP
            $('.btnViewStats').live('click', function () {
                $('#addMovementModel').jqmShow();

                $.ajax({
                    type: 'POST',
                    url: '<%=ResolveUrl("~/Warehouses/DetailWareHouseMaterial") %>',
                    data: ({ Code: this.id }),
                    asyn: false,
                    success: function (data) {
                        if (data.success == true) {
                            $('#txtWareHouse').val(data.Result[0].WareHouseName);
                            $('#txtMaterialCode').val(data.Result[0].CodeMaterial);
                            $('#txtMaterialName').val(data.Result[0].MaterialName);
                            $('#txtWareHousesMaterialID').val(data.Result[0].WareHouseMaterialID);
                            $('#txtWareHouseID').val(data.Result[0].WareHouseId);
                            $('#txtMaterialID').val(data.Result[0].MaterialId);
                            $('#ddlMovementType').val(0)
                            $('#txtDescription').val('');
                            $('#txtQuantity').val('');
                        }
                    }
                });
            });

            $("#ddlMovementType").change(function () {
                if ($("#ddlMovementType").val() == 0) {
                    $('#ddlMovementType').showError('Select a Movement Type');
                } else {
                    $('#ddlMovementType').clearError();
                }
            });

            $('#txtQuantity').keyup(function () {
                if ($("#txtQuantity").val() == '') {
                    $('#txtQuantity').showError('Invalid number');
                } else {
                    $('#txtQuantity').clearError();
                }
            });

            //CSTI - FHP
            $('#btnSaveWareHouseMaterial').click(function (e) {
                //
                if ($("#ddlMovementType").val() == 0) {
                    $('#ddlMovementType').showError('Select a Movement Type');
                    return;
                } else if ($("#txtQuantity").val() == '') {
                    $('#txtQuantity').showError('Invalid number');
                }
                var estado = true;
                $.ajax({
                    type: 'POST',
                    url: '<%=ResolveUrl("~/Warehouses/SaveWareHouseMaterial")%>',
                    data: (
                        {
                            InventoryMovementType: $("#ddlMovementType").val(),
                            WareHouseMaterialId: $("#txtWareHousesMaterialID").val(),
                            QuantityField: $("#txtQuantity").val(),
                            Description: $("#txtDescription").val(),
                            WareHouseId: $('#txtWareHouseID').val(),
                            MaterialId: $('#txtMaterialID').val()
                        }),
                    asyn: false,
                    success: function (data) {
                        if (data.result == true) {
                            $(".filterButton").trigger("click");
                            estado = data.Estado;
                            $('#addMovementModel').jqmHide();
                            showMessage(data.menssage, estado);
                        } else {
                            $('#addMovementModel').jqmHide();
                            showMessage(data.menssage, estado);
                        }
                    }
                });
            });

            $('#addMovementModel').jqm({ modal: false, onShow: function (h) {
                h.w.css({
                    top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });

            $('.warehouseEnabler').live('click', function () {
                var enabled = $(this).prop('checked');
                $(this).parent().find('input[type="text"]').each(function () {
                    if (enabled) {
                        $(this).removeAttr('disabled').css({ 'color': '#758D91' });
                    } else {
                        $(this).attr('disabled', 'disabled').css({ 'color': '#cfcfcf' });
                    }
                });
                if (enabled) {
                    $(this).parent().find('img').hide();
                } else {
                    $(this).parent().find('img').css('display', 'inline-block');
                }
            });

            $('.Expand').live('click', function () {
                var row = $(this).closest('tr');
                row.nextAll().each(function () {
                    if ($(this).filter('.variantProduct').length == 0) {
                        return false;
                    }
                    $(this).toggle();
                });
            });

            $('#paginatedGrid').delegate('input[type="text"]', 'keyup', function () {
                $(this).parent().find('.changed').val(true);
            });

            $('#paginatedGrid').delegate('input[type="checkbox"]', 'click', function () {
                $(this).parent().find('.changed').val(true);
            });

            // Joey's UI prototyping stuff
            $('#paginatedGrid a.toggleWarehouse').live('click', function () {
                var targetWarehouse = $(this).attr('rel');
                $(this).parent().toggleClass('warehouseClosed');
                $('#paginatedGrid div.' + targetWarehouse).toggle();
            });
            $('#paginatedGrid th:eq(0)').css('width', '50px');
            $('#paginatedGrid th:eq(1)').css('width', '125px');

        });

    </script>
    <style type="text/css">
        th.warehouseClosed
        {
            width: 100px;
        }
        td.warehouseClosed div
        {
            display: none;
        }
    </style>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> >
    <%= Html.Term("WarehouseInventory", "Warehouse Inventory") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
    <div class="SectionHeader">
        <h2>
        <%= Html.Term("WarehouseInventory", "Warehouse Inventory")%></h2>
        <%= Html.Term("WarehouseInventory", "Warehouse Inventory")%>
       
    </div>
        <%
        Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.WareHouseMaterialSearchData>("~/Products/Warehouses/Get")
        .AutoGenerateColumns()
        //csti-mescobar-EB-478-23/02/2016-inicio
        .AddSelectFilter(Html.Term("WareHouse"), "warehouse", new Dictionary<string, string>() { { "0", Html.Term("All", "All") } }.AddRange(TempData["WareHouse"] as Dictionary<string, string>))
        //csti-mescobar-EB-478-23/02/2016-fin
        .AddInputFilter(Html.Term("Product"), "product")
        .AddInputFilter(Html.Term("Material"), "material")
        .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))
        .ClickEntireRow()
        .Render(); 
        %>
    <div id="addMovementModel" class="jqmWindow LModal Overrides">
        <div class="mContent">
            <input type="hidden" id="txtWareHousesMaterialID" />
            <input type="hidden" id="txtWareHouseID" />
            <input type="hidden" id="txtMaterialID" />
            <h2>
                <%=Html.Term("Add", "Add")%>
            </h2>
            <table id="newWareHouseMaterial" class="FormTable Section">
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("WareHouse", "Ware house")%>
                        :
                    </td>
                    <td>
                        <input type="text" id="txtWareHouse" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("MaterialCode", "Material Code")%>
                        :
                    </td>
                    <td>
                        <input type="text" id="txtMaterialCode" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("MaterialName", "MaterialName")%>
                        :
                    </td>
                    <td>
                        <input type="text" id="txtMaterialName" disabled="disabled" />
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("MovementType", "Movement Type")%>
                        :
                    </td>
                    <td>
                        <%= Html.DropDownList("ddlMovementType", (TempData["InventoryMovementTypes"] as IEnumerable<SelectListItem>))%>
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("Quantity", "Quantity")%>
                        :
                    </td>
                    <td>
                        <input type="text" id="txtQuantity" class="pad5 required" name="<%=Html.Term("Quantity","Quantity.") %>"
                            style="width: 4.167em" />
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("Description", "Description")%>
                        :
                    </td>
                    <td>
                        <textarea id="txtDescription"></textarea>
                    </td>
                </tr>
            </table>
            <br />
            <p>
                <a id="btnSaveWareHouseMaterial" class="Button BigBlue">
                    <%= Html.Term("Save", "Save")%></a> <a href="javascript:void(0);" class="Button jqmClose">
                        <%= Html.Term("Cancel")%></a>
            </p>
        </div>
    </div>
    <div id="divFormulario" class="jqmWindow LModal Overrides">
        <div class="mContent">
        </div>
    </div>

    <%--csti - mescobar - 08/03/2016 - EB-478 - Inicio--%>
    <div id="noteModal" class="jqmWindow LModal" style="left: 510px; z-index: 3000; top: 100px;">
        <div class="mContent">
            <h2>
                <%= Html.Term("Reemplacements", "Reemplacements")%>
            </h2>
            <table id="campaignAction" class="FormTable" width="1230px">
                <tr id="valuePanel">
                    <td id="taskAdd">
                        <table class="DataGrid" width="100%">
                            <thead>
                                <tr class="GridColHead">
                                    <th>
                                        <%= Html.Term("MaterialCode", "Material Code")%>
                                    </th>
                                    <th>
                                        <%= Html.Term("MaterialName", "Material Name")%>
                                    </th>
                                     <th>
                                        <%= Html.Term("AvgCost", "AvgCost")%>
                                    </th>
                                    <th>
                                        <%= Html.Term("QuantityOnHand", "Quantity on Hand")%>
                                    </th>
                                    <th>
                                        <%= Html.Term("ReorderLevel", "Reorder Level")%>
                                    </th>
                                    <th>
                                        <%= Html.Term("Allocated", "Allocated")%>
                                    </th>
                                </tr>
                            </thead>
                            <tbody id="tblinformacion">
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
            <p>
                <a href="javascript:void(0);" class="Button LinkCancel jqmClose" id="btnCancelObservacion"
                    onclick="Ocultar()">
                    <%= Html.Term("Close","Close")%></a>
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>
    <%--csti - mescobar - 08/03/2016 - EB-478 - Fin--%>

    <%--csti - mescobar - 08/03/2016 - EB-478 - Inicio--%>
    <div id="addMessage" class="jqmWindow LModal Overrides"> 
       <div class="mContent">
            <h2> <%=Html.Term("Message", "Message")%> </h2> 
                 <p>
                     <%=Html.Term("MessageReplacements", "There isn't replacements to this material.")%>
                 </p>
                 <p>
                    <a id="btnCancel"  href="javascript:void(0);" class="Button jqmClose"> <%= Html.Term("Close", "Close")%></a>
                </p>
        </div>
    </div>
    <%--csti - mescobar - 08/03/2016 - EB-478 - Inicio--%>
</asp:Content>
