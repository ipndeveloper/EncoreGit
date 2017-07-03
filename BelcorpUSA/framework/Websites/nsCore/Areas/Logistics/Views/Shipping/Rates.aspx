<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/Shipping.Master" 
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.ShippingRulesLogisticsSearchParameters>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            // Look-up shippingRule ini
            $('#shippingRuleIdInputFilter').change(function () {
                $('#shippingRuleId').val("0");
            });

            $('#shippingRuleIdInputFilter').removeClass('Filter').after($('#shippingRuleId')).css('width', '275px')
                .val('')
				.watermark('<%= Html.JavascriptTerm("ShippingRuleSearch", "Look up shipping rule by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Shipping/ShippingRulesLookUp") %>', { onSelect: function (item) {
				    $('#shippingRuleId').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});
            // Look-up shippingRule fin

            // Look-up warehouse ini
            $('#warehouseIdInputFilter').change(function () {
                $('#warehouseId').val("0");
            });

            $('#warehouseIdInputFilter').removeClass('Filter').after($('#warehouseId')).css('width', '275px')
                .val('')
				.watermark('<%= Html.JavascriptTerm("WarehouseSearch", "Look up warehouse by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Shipping/WarehouseLookUp") %>', { onSelect: function (item) {
				    $('#warehouseId').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});
            // Look-up warehouse fin

            // Look-up logisticProvider ini
            $('#logisticProviderIdInputFilter').change(function () {
                $('#logisticProviderId').val("0");
            });
            $('#logisticProviderIdInputFilter').removeClass('Filter').after($('#logisticProviderId')).css('width', '275px')
                .val('')
				.watermark('<%= Html.JavascriptTerm("LogisticProviderSearch", "Look up logistics provider rule by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Shipping/LogisticProviderLookUp") %>', { onSelect: function (item) {
				    $('#logisticProviderId').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});
            // Look-up logisticProvider fin

            $('#exportToExcel').click(function () {
                //Page
                var shippingRuleId = $('#shippingRuleId').val();
                var shippingMethodId = $('#shippingMethodIdSelectFilter').val();
                var statusId = $('#statusIdSelectFilter').val();
                var warehouseId = $('#warehouseId').val();
                var logisticProviderId = $('#logisticProviderId').val();
                var pagecount = "0";

                $.get('<%= ResolveUrl(string.Format("~/Logistics/Shipping/getCountRowExportRates/")) %>', { shippingRuleId: shippingRuleId,
                    shippingMethodId: shippingMethodId,
                    statusId: statusId,
                    warehouseId: warehouseId,
                    logisticProviderId: logisticProviderId
                }, function (response) {
                    if (response.result) {
                        pagecount = response.RowCountTotal;

                        var url = '<%= ResolveUrl("~/Logistics/Shipping/ExportShippingRates") %>';

                        var parameters = {
                            page: 0,
                            pageSize: parseInt(pagecount.toString()),
                            orderBy: "ShippingRateGroupID",
                            orderByDirection: "ASC",
                            shippingRuleId: shippingRuleId,
                            shippingMethodId: shippingMethodId,
                            statusId: statusId,
                            warehouseId: warehouseId,
                            logisticProviderId: logisticProviderId
                        };
                        url = url + '?' + $.param(parameters).toString();
                        $("#frmExportar").attr("src", url);
                        //

                    } else {
                        showMessage(response.message, true);
                    }
                });
                //Fin

               
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" value="0" id="shippingRuleId" class="Filter" />
<input type="hidden" value="0" id="warehouseId" class="Filter" />
<input type="hidden" value="0" id="logisticProviderId" class="Filter" />

<div class="SectionHeader">
    <h2>
        <%= Html.Term("ShippingRates", "Shipping Rates")%>
    </h2>
        <%= Html.Term("BrowseShippingRates", "Browse Shipping Rates")%>&nbsp;|&nbsp;<a href="<%= ResolveUrl("~/Logistics/Shipping/RateDetail") %>">
    <%= Html.Term("NewRate", "New Rate")%></a>
</div>

 <% Html.PaginatedGrid<ShippingRateGroupBe>("~/Logistics/Shipping/GetRutes")
        //.AutoGenerateColumns()        
        .AddInputFilter(Html.Term("ShippingRule", "Shipping Rule"), "shippingRuleId", startingValue: 0)
        .AddSelectFilter(Html.Term("ShippingMethod"), "shippingMethodId"
                        , new Dictionary<int, string>() { { 0, Html.Term("SelectaShippingMethod", "Select a Shipping Method") } }
                        .AddRange(TempData["ShippingMethods"] as Dictionary<int, string>),
                        startingValue: Model.ShippingMethodID)
        .AddSelectFilter(Html.Term("Status"), "statusId"
                        , new Dictionary<int, string>() { { -1, Html.Term("SelectaStatus", "Select a Status") } }
                        .AddRange(TempData["ShippingStatuses"] as Dictionary<int, string>),
                        startingValue: Model.StatusID)
        .AddInputFilter(Html.Term("Warehouse", "Warehouse"), "warehouseId", startingValue: 0)
        .AddInputFilter(Html.Term("LogisticsProvider", "Logistics Provider"), "logisticProviderId", startingValue: 0)

        .AddColumn(Html.Term("ShippingRateCode", "Shipping Rate Code"), "ShippingRateGroupID", true)
        .AddColumn(Html.Term("Name", "Name"), "Name", false)
        .AddColumn(Html.Term("Description", "Description"), "Description", false)
        .AddColumn(Html.Term("ExternalCode", "External Code"), "GroupCode", false)
        .AddColumn(Html.Term("Status", "Status"), "Active", false)

        .CanChangeStatus(true, true, "~/Logistics/Shipping/ChangeStatusRate")
        .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))
        .ClickEntireRow()
        .Render(); 
        %>
         <iframe   name ="frmExportar" id="frmExportar" style="display:none" src="">
        </iframe>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<a href="<%= ResolveUrl("~/Logistics") %>">
<%= Html.Term("Logistics")%>
</a> > 
<a href="<%= ResolveUrl("~/Logistics/Shipping/Rutes") %>">
<%= Html.Term("ShippingRutes", "Shipping Rutes")%>
</a> >
<%= Html.Term("BrowseShippingRutes", "Browse Shipping Rutes")%>
</asp:Content>
