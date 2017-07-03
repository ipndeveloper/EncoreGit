<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/Shipping.Master" 
Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.ShippingRulesLogisticsSearchData>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            // Look-up warehouse ini
            $('#warehouseIdInputFilter').change(function () {
                $('#warehouseId').val("0");
            });

            $('#warehouseIdInputFilter').removeClass('Filter').after($('#warehouseId')).css('width', '275px')
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
                .watermark('<%= Html.JavascriptTerm("LogisticProviderSearch", "Look up logistics provider rule by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Shipping/LogisticProviderLookUp") %>', { onSelect: function (item) {
				    $('#logisticProviderId').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});
            // Look-up logisticProvider fin

            $('#btnToggleStatus').click(function () {
                var t = $(this);
                showLoading(t);
                $.post('<%= ResolveUrl(string.Format("~/Logistics/Shipping/ToggleStatus/{0}", Model.ShippingOrderTypeID)) %>', {}, function (response) {
                    hideLoading(t);
                    if (response.result) {
                        t.toggleClass('ToggleInactive');
                    } else {
                        showMessage(response.message);
                    }
                })
                .fail(function () {
                    hideLoading(t);
                    showMessage('@Html.Term("ErrorProcessingRequest", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
                });
            });

            $('#btnSave').click(function () {
                if ($('#txtDaysForDelivey').val().length != 0 && isNaN($('#txtDaysForDelivey').val())) {
                    return false;
                }

                if ($('#ruleDetailForm').checkRequiredFields()) {
                    var data = {
                        shippingRuleId: $('#shippingRuleId').val(),
                        name: $('#txtName').val(),
                        countryId: $('#ddlCountry').val(),
                        orderTypeId: $('#ddlOrderType').val(),
                        shippingMethodId: $('#ddlShippingMethod').val(),
                        warehouseId: $('#warehouseId').val(),
                        logisticProviderId: $('#logisticProviderId').val(),
                        daysForDelivery: $('#txtDaysForDelivey').val().length == 0 ? 0 : parseInt($('#txtDaysForDelivey').val()),
                        shippingRateGroupId: $("#ddlShippingRateGroup").val(),
                        isDefaultShippingMethod: $('#chkIsDefaultShippingMethod').prop('checked')
                    };

                    $.post('<%= ResolveUrl("~/Logistics/Shipping/SaveRule") %>', data, function (response) {
                        if (response.result) {
                            showMessage('<%=@Html.Term("RuleSavedSuccessfully", "Rule Saved Successfully")%>', false);
                            
                            if (!eval($('#shippingRuleId').val())) { // Create case
                                $('#shippingRuleId').val(response.shippingRuleId);
                                // Reload Edit Mode 
                                window.location.replace('<%= ResolveUrl("~/Logistics/Shipping/RuleDetail") %>' + "/" + response.shippingRuleId);
                            }
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Logistics") %>">
        <%= Html.Term("Logistics")%></a> > <a href="<%= ResolveUrl("~/Logistics/Shipping/Rules") %>">
            <%= Html.Term("ShippingRules", "Shipping Rules")%></a> >
    <%= Html.Term("ShippingRuleDetail", "Rule Detail")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ShippingRuleDetail", "Rule Detail")%>
        </h2>
   </div>

    <input type="hidden" value="<%= Model.ShippingOrderTypeID %>" id="shippingRuleId" class="Filter" />
    <input type="hidden" value="<%= Model.WarehouseID %>" id="warehouseId" class="Filter" />
    <input type="hidden" value="<%= Model.LogisticProviderID %>" id="logisticProviderId" class="Filter" />

    <table id="ruleDetailForm" class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <span class="requiredMarker">*</span>
                <%: Html.Term("Name", "Name")%>:
            </td>
            <td>
                <input id="txtName" type="text" value="<%= Model.ShippingRuleName %>" maxlength="200"
                    class="required" name="<%= Html.Term("NameIsRequired", "Name is required") %>" style="width: 20.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Country", "Country")%>:
            </td>
            <td>
                <%: Html.DropDownCountries(htmlAttributes: new { id="ddlCountry"}, selectedCountryID: Model.CountryID) %>
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("OrderType", "Order Type") %>:
            </td>
            <td>
                <%: Html.DropDownList("ddlOrderType", TempData["OrderTypes"] as IEnumerable<SelectListItem>)%>
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("ShippingMethod", "Shipping Method")%>:
            </td>
            <td>
                <%: Html.DropDownList("ddlShippingMethod", TempData["ShippingMethods"] as IEnumerable<SelectListItem>)%>
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Warehouse", "Warehouse")%>:
            </td>
            <td>
                <input id="warehouseIdInputFilter" type="text" value="<%= Model.Warehouse %>" maxlength="100" 
                    class="required distributorSearch" name="<%= Html.Term("WarehouseIsRequired", "Warehouse is required") %>" style="width: 20.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("LogisticProvider", "Logistic Provider")%>:
            </td>
            <td>
                <input id="logisticProviderIdInputFilter" type="text" value="<%= Model.LogisticProvider %>" maxlength="100" 
                    class="required" name="<%= Html.Term("LogisticProviderIsRequired", "Logistic Provider is required") %>" style="width: 20.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("DaysForDelivery", "Days for Delivery")%>:
            </td>
            <td>
                <input id="txtDaysForDelivey" type="text" value="<%= Model.DaysForDelivey %>" maxlength="200" style="width: 20.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("ShippingRateGroup", "Shipping Rate Group")%>:
            </td>
            <td>
                <%: Html.DropDownList("ddlShippingRateGroup", TempData["ShippingRateGroups"] as IEnumerable<SelectListItem>)%>
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("IsDefaultShippingMethod", "Is Default Shipping Method")%>:
            </td>
            <td>
                <input type="checkbox" id="chkIsDefaultShippingMethod" value="<%= Model.IsDefaultShippingMethod %>" />
            </td>
        </tr>

        <tr>
            <td class="FLabel"></td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue" >
                        <%= Html.Term("SaveRule", "Save Rule")%></a>
                </p>
            </td>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="YellowWidget" runat="server">
    <div class="TagInfo" <%= Model.ShippingOrderTypeID == 0 ? "style='display:none'" : "" %>>
        <div class="Content">
            <div class="SubTab">
            </div>
            <table class="DetailsTag Section" width="100%">
                <tr>
                    <td class="Label">
                        <%= Html.Term("Name") %>:
                    </td>
                    <td>
                        <label><%= Model.ShippingRuleName %></label>
                    </td>
                </tr>
                <tr>
                    <td class="Label">
                        <%= Html.Term("Code") %>:
                    </td>
                    <td>
                        <label><%= Model.LogisticProviderID %></label>
                    </td>
                </tr>
                <tr>
                    <td class="Label">
                        <%= Html.Term("Status", "Status") %>:
                    </td>
                    <td>
                        <a id="btnToggleStatus" href="javascript:void(0);" class="Toggle <%= Model.Status ?  "ToggleActive" : "ToggleInactive" %>"></a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
    <div class="SectionNav">
			<ul class="SectionLinks">
                <%= Html.SelectedLink("~/Logistics/Shipping/RuleDetail/", Html.Term("ShippingRuleDetail", "Rule Detail"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
                <%= Html.SelectedLink("~/Logistics/Shipping/DeliveryZone/" + (Model.ShippingOrderTypeID == 0 ? "" : Model.ShippingOrderTypeID.ToString()), Html.Term("ShippingRuleDetailDeliveryZones", "Delivery Zones"), LinkSelectionType.ActualPage, CoreContext.CurrentUser, "")%>
			</ul>
		</div>
</asp:Content>