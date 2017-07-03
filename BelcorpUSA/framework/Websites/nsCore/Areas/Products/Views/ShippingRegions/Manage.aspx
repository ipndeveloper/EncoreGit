<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Warehouses/Warehouses.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#sCountry').change(function () {
                getStates($('#sRegion').val());
            });

            $('#btnAddStates').click(function () {
                $('#sStates option:selected').each(function () {
                    $('#sRegionStates').append($(this));
                });
                $('#sStates option:selected').remove();
                sortSelectList('sRegionStates');
            });

            $('#btnRemoveStates').click(function () {
                $('#sRegionStates option:selected').each(function () {
                    $('#sStates').append($(this));
                });
                $('#sRegionStates option:selected').remove();
                sortSelectList('sStates');
            });

            $('#btnSaveShippingRegions').click(function () {
                var data = { regionId: $('#sRegion').val(), warehouseId: $('#sWarehouse').val(), countryId: $('#sCountry').val() };
                $('#sRegionStates option').each(function (i) {
                    data['states[' + i + ']'] = $(this).val();
                });
                $.post('<%= ResolveUrl("~/Products/ShippingRegions/SaveRegionManagementSettings") %>', data, function (response) {
                    if (response.result && response.message) {
                        getStates($('#sRegion').val());
                        showMessage(response.message, false);
                    }
                    else if (!response.result) {
                        showMessage(response.message, true);
                    }
                });
            });

            $('#sRegion').change(function () {
                var regionID = $(this).val();
                getWarehouses(regionID);
                getStates(regionID);
            });

            $('#sRegion').trigger('change');
            $('#sCountry').trigger('change');
        });

        function sortSelectList(id) {
            var my_options = $('#' + id + ' option');

            my_options.sort(function(a,b) {
                if (a.text.toUpperCase() > b.text.toUpperCase()) return 1;
                else if (a.text.toUpperCase() < b.text.toUpperCase()) return -1;
                else return 0
            })

            $('#' + id).empty().append( my_options );
        }

        function getStates(regionId) {
            $('#stateRegionHeader').text($('#sRegion option:selected').text());
            $('.stateRegion.selected').removeClass('selected');
            $('#stateRegion' + regionId).addClass('selected');
            $.get('<%= ResolveUrl("~/Products/ShippingRegions/GetStates") %>', { regionId: regionId, countryId: $('#sCountry').val() }, function (response) {
                if (response.result === undefined || response.result) {
                    $('#sRegionStates').html(response.regionStates);
                    $('#sStates').html(response.unusedStates);
                } else {
                    showMessage(response.message, true);
                }
            });
        }

        function getWarehouses(regionId) {
            $.get('<%= ResolveUrl("~/Products/ShippingRegions/GetWarehouse") %>', { regionId: regionId }, function (response) {
                if (response.result) {
                    $('#sWarehouse').val(response.WarehouseID);
                } else {
                    showMessage(response.message, true);
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/ShippingRegions") %>">
            <%= Html.Term("ShippingRegions", "Shipping Regions")%>
        </a>>
    <%= Html.Term("ManageShippingRegions", "Manage Shipping Regions") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ManageShippingRegions", "Manage Shipping Regions") %></h2>
        <a href="<%= ResolveUrl("~/Products/ShippingRegions") %>">
            <%= Html.Term("BrowseShippingRegions", "Browse Shipping Regions") %></a> |
        <%= Html.Term("ManageShippingRegions", "Manage Shipping Regions") %>
        <br />
        <%= Html.Term("Region") %>:&nbsp;
        <select id="sRegion">
        <% foreach (ShippingRegion region in SmallCollectionCache.Instance.ShippingRegions)
           { %>
        <option value="<%= region.ShippingRegionID %>"><%= region.GetTerm() %></option>
        <% } %>
        </select>
    </div>
    <!-- Data Grid -->
    <table class="DataGrid" cellspacing="0" width="100%">
        <tr>
            <td colspan="3" style="vertical-align: middle;">
                <%= Html.Term("Warehouse")%>:&nbsp;
                <select id="sWarehouse">
                <% foreach (var warehouse in SmallCollectionCache.Instance.Warehouses)
                   { %>
                    <option value="<%= warehouse.WarehouseID %>"><%= warehouse.Name %></option>
                <% } %>
                </select>
                <br />
                <br />
                <select id="sCountry">
                    <%foreach (Country country in SmallCollectionCache.Instance.Countries.Where(c=>c.Active).OrderBy(c => c.CountryID))
                        { %>
                    <option value="<%= country.CountryID %>">
                        <%= country.Name %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td style="width: 40%;">
                <h3><%= Html.Term("States/Provinces", "States/Provinces") %></h3>
                <select id="sStates" multiple="multiple" style="width: 100%; height: 16.667em;">
                </select>
            </td>
            <td style="width: 2.182em; vertical-align: middle;">
                <p>
                    <a id="btnAddStates" href="javascript:void(0);" title="Add to region" class="pad2">
                        <span class="icon-ArrowNext"></span></a></p>
                <p>
                    <a id="btnRemoveStates" href="javascript:void(0);" title="Remove from region" class="pad2">
                        <span class="icon-ArrowPrev-grey"></span></a></p>
            </td>
            <td style="width: 40%;">
                <h3 id="stateRegionHeader">
                    <%= Html.Term("Region", "Region") %></h3>
                <select id="sRegionStates" multiple="multiple" style="width: 100%; height: 16.667em;">
                </select>
            </td>
        </tr>
        <tr>
            <td colspan="2">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSaveShippingRegions" class="Button BigBlue">
                    <%= Html.Term("Save", "Save") %></a>
                </p>
            </td>
            <td>
            </td>
        </tr>
    </table>
</asp:Content>
