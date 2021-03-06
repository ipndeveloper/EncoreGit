﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Warehouses/Warehouses.Master" 
Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.InventoryMovementsSearchParameters>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            // Look-up product ini
            $('#productIdInputFilter').change(function () {
                $('#productId').val("");
            });

            $('#productIdInputFilter').removeClass('Filter').after($('#productId')).css('width', '275px')
                .val('')
				.watermark('<%= Html.JavascriptTerm("ProductSearch", "Look up product by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Products/Promotions/Search") %>', { onSelect: function (item) {
				    $('#productId').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});
            // Look-up product finss    

            // Look-up material ini
            $('#materialIdInputFilter').change(function () {
                $('#materialId').val("");
            });

            $('#materialIdInputFilter').removeClass('Filter').after($('#materialId')).css('width', '275px')
                            .val('')
				            .watermark('<%= Html.JavascriptTerm("MaterialSearch", "Look up material by ID or name") %>')
				            .jsonSuggest('<%= ResolveUrl("~/Products/Materials/SearchFilter") %>', { onSelect: function (item) {
				                $('#materialId').val(item.id);
				            }, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				            });
            // Look-up material fin  

            //$('#endDateInputFilter').removeClass('Filter');
            //            $('#endDateInputFilter').off("change");
            //            $('#endDateInputFilter').change(function () {
            //                alert($('#endDateInputFilter').val());
            //                $('#endDateInputFilter').clearError();
            //                var startDate = $('#startDateInputFilter').val();
            //                var endDate = $('#endDateInputFilter').val();

            //                if (Date.parse(startDate) > Date.parse(endDate)) {
            //                    $('#endDateInputFilter').showError('<%= Html.JavascriptTerm("EndDateRangeValidation", "End date can not be less than start date") %>');
            //                    return false;
            //                }
            //            });

            //@01 A3
            $("#exportToExcel").click(function () {

                var mv = 0
                if ($('#movementTypeId').val() != null)
                    mv = $('#movementTypeId').val();

                var warehouseId = $('#warehouseIdSelectFilter').val();

                if (warehouseId != '') {

                    var url = '<%= ResolveUrl("~/Products/InventoryMovements/ExportInventoryMovements") %>';



                    var parameters = {
                        warehouseId: warehouseId,
                        productId: $('#productId').val(),
                        materialId: $('#materialId').val(),
                        startDate: $('#startDateInputFilter').val(),
                        endDate: $('#endDateInputFilter').val(),
                        movementTypeId: mv
                    };

                    url = url + '?' + $.param(parameters).toString();
        

                    $("#frmExportar").attr("src", url);


                }

            });
            //@01 A3

        });
    </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
	<%= Html.Term("InventoryMovements", "Inventory Movements")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="SectionHeader">
		<h2>
			<%= Html.Term("InventoryMovements", "Inventory Movements")%>
        </h2>
	</div>

    <input type="hidden" value="" id="productId" class="Filter"/>
    <input type="hidden" value="" id="materialId" class="Filter"/>

    <% Html.PaginatedGrid<WarehouseInventoryMovementsSearchData>("~/Products/InventoryMovements/Get")
        .AutoGenerateColumns()
        .AddSelectFilter(Html.Term("Warehouse"), "warehouseId",
                        new Dictionary<string, string>() { 
                            { "", Html.Term("SelectaWarehouse", "Select a Warehouse") } }
                            .AddRange(SmallCollectionCache.Instance.Warehouses.ToDictionary(x => x.WarehouseID.ToString(), 
                                                                                            y => y.GetTerm().ToString())),
                        startingValue: Model.WarehouseID)
        .AddInputFilter(Html.Term("Product", "Product"), "productId", startingValue: "")
        .AddInputFilter(Html.Term("Material", "Material"), "materialId", startingValue: "")
        .AddSelectFilter(Html.Term("MovementType"), "movementTypeId",
                        new Dictionary<string, string>() { 
                            { "0", Html.Term("SelectaType", "Select a Type") } }
                            .AddRange(TempData["MovementTypes"] as Dictionary<string, string>), 
                        startingValue: Model.InventoryMovementTypeID)
        .AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", Html.Term("StartDate", "Start Date"), true)
        .AddInputFilter(Html.Term("To", "To"), "endDate", Html.Term("EndDate", "End Date"), true, true)
        .ClickEntireRow()
        .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel")) 
		.Render(); %>   
        <iframe   name ="frmExportar" id="frmExportar" style="display:none" src=""></iframe> <%--01 A2--%>
</asp:Content>
