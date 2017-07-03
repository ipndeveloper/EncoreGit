<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/Shipping.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.ShippingRulesLogisticsSearchParameters>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#shippingRuleId').val("0");
            $('#warehouseId').val("0");
            $('#logisticProviderId').val("0");
            $("#shippingMethodIdSelectFilter").val("0");
            $("#statusIdSelectFilter").val("-1");
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

            //$(".filterButton").text($("#HF_ApplyFilter").val());

            $("#exportToExcel").click(function () {
                var url = '<%= ResolveUrl("~/Logistics/Shipping/ExportShippingRules") %>';

                var parameters = {
                shippingRuleId: $("#shippingRuleIdInputFilter").val(),
                shippingMethodId: $("#shippingMethodIdSelectFilter").val(),
                statusId: $("#statusIdSelectFilter").val(),
                warehouseId: $("#warehouseIdInputFilter").val(),
                logisticProviderId: $("#logisticProviderIdInputFilter").val()
                };
                url = url + '?' + $.param(parameters).toString();
                $("#frmExportar").attr("src", url);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Logistics") %>">
        <%= Html.Term("Logistics")%></a> > <a href="<%= ResolveUrl("~/Logistics/Shipping/Rules") %>">
            <%= Html.Term("ShippingRules", "Shipping Rules")%></a> >
    <%= Html.Term("BrowseShippingRules", "Browse Shipping Rules")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ShippingRules", "Shipping Rules")%>
        </h2>
        <%= Html.Term("BrowseShippingRules", "Browse Shipping Rules")%>
        | <a href="<%= ResolveUrl("~/Logistics/Shipping/RuleDetail") %>">
            <%= Html.Term("NewRule", "New Rule")%></a>
    </div>
    <input type="hidden" value="0" id="shippingRuleId" class="Filter" />
    <input type="hidden" value="0" id="warehouseId" class="Filter" />
    <input type="hidden" value="0" id="logisticProviderId" class="Filter" />
    

    <% Html.PaginatedGrid<ShippingRulesLogisticsSearchData>("~/Logistics/Shipping/GetRules")
        .AutoGenerateColumns()
        .AddInputFilter(Html.Term("ShippingRule", "Shipping Rule"), "shippingRuleId", startingValue: 0)
        .AddSelectFilter(Html.Term("ShippingMethod"), "shippingMethodId",
                        new Dictionary<int, string>() { 
                            { 0, Html.Term("SelectaShippingMethod", "Select a Shipping Method") } }
                            .AddRange(TempData["ShippingMethods"] as Dictionary<int, string>),
                        startingValue: Model.ShippingMethodID)
        .AddSelectFilter(Html.Term("Status"), "statusId",
                        new Dictionary<int, string>() { 
                            { -1, Html.Term("SelectaStatus", "Select a Status") } }
                            .AddRange(TempData["ShippingStatuses"] as Dictionary<int, string>),
                        startingValue: Model.StatusID)
        .AddInputFilter(Html.Term("Warehouse", "Warehouse"), "warehouseId", startingValue: 0)
        .AddInputFilter(Html.Term("LogisticsProvider", "Logistics Provider"), "logisticProviderId", startingValue: 0)
        .CanChangeStatus(true, true, "~/Logistics/Shipping/ChangeStatus")        
        .AddOption("exportToExcel", Html.Term("ExportExcel", "Export Excel"))
        .ClickEntireRow() 
        .Render(); %>
        <iframe   name ="frmExportar" id="frmExportar" style="display:none" src=""></iframe>
</asp:Content>
