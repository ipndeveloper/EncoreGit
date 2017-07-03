<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Orders.Models.Shipping.IndexModel>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        var viewModelOptions = {
            pagesFormat: '<%: Html.Term("XOfXPages", "{0} of {1}") %>'
        };

        $(function () {
            $('.checkAll').click(function () {
                $('input:checkbox').attr('checked', this.checked);
            });
        });
    </script>
    <script type="text/javascript" src="<%= Url.Content("~/Areas/Orders/Scripts/Shipping/Index.js") %>"></script>
    <style type="text/css">
        .modified { background-color: yellow; }
    </style>
</asp:Content>

<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%: ResolveUrl("~/Orders") %>"><%: Html.Term("Orders") %></a>
    >
    <%: Html.Term("ShipOrders", "Ship Orders") %>
</asp:Content>

<asp:Content ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2 class="FL">
            <%: Html.Term("ShipOrders", "Ship Orders") %>
        </h2>
        <span class="FR" style="height: 30px">
            <span id="ShowLoading"></span>
        </span>
        <span class="clr"></span>
        <div class="validation-summary-errors" data-bind="visible: showErrors">
            <ul data-bind="foreach: Errors">
                <li data-bind="html: $data"></li>
            </ul>
        </div>
    </div>
    
    <div class="UI-lightBg brdrAll GridFilters">
        <div class="FL FilterSet">
            <div class="FL">
                <label><%: Html.Term("Type") %>:</label>
                <%: Html.DropDownList("OrderTypeID", Model.OrderTypes, Html.Term("SelectaType", "Select a Type..."), new { @class = "Filter", data_bind = "value: SearchParams.OrderTypeID" })%>
            </div>
            <div class="FL">
                <label><%: Html.Term("OrderStatus", "Order Status") %>:</label>
                <%: Html.DropDownList("OrderStatusID", Model.OrderStatuses, Html.Term("SelectanOrderStatus", "Select an Order Status..."), new { @class = "Filter", data_bind = "value: SearchParams.OrderStatusID" })%>
            </div>
            <div class="FL">
                <label><%: Html.Term("OrderShipmentStatus", "Order Shipment Status") %>:</label>
                <%: Html.DropDownList("OrderShipmentStatusID", Model.OrderShipmentStatuses, Html.Term("SelectaShipmentStatus", "Select a Shipment Status..."), new { @class = "Filter", data_bind = "value: SearchParams.OrderShipmentStatusID" })%>
            </div>
            <span class="ClearAll"></span>
            <div class="FL">
                <label><%: Html.Term("OrderNumber", "Order Number") %>:</label>
                <input type="text" class="Filter TextInput" data-bind="value: SearchParams.OrderNumber" />
            </div>
            <div class="FL">
                <label><%: Html.Term("CompleteDate", "Complete Date") %>:</label>
                <input type="text" class="Filter TextInput DatePicker Manual" placeholder="<%: Html.Term("StartDate","Start Date") %>" data-bind="datepicker: SearchParams.StartDate, datepickerOptions: { changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100' }" />
            </div>
            <div class="FL">
                <label><%: Html.Term("To") %>:</label>
                <input type="text" class="Filter TextInput DatePicker Manual" placeholder="<%: Html.Term("EndDate","End Date") %>" data-bind="datepicker: SearchParams.EndDate, datepickerOptions: { changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100' }" />
            </div>
            <div class="FL RunFilter">
                <a class="ModSearch Button" href="javascript:void(0);" data-bind="click: applyFilters">
                    <span><%: Html.Term("ApplyFilter")%></span>
                </a>
                <span class="ClearAll"></span>
            </div>
        </div>
        <span class="ClearAll"></span>
    </div>
    
    <div class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility">
        <a class="clearFiltersButton UI-icon-container" href="javascript:void(0);" data-bind="click: resetAll">
            <span class="UI-icon icon-refresh"></span>
            <span><%: Html.Term("ClearFilters", "Clear Filters") %></span>
        </a>
        <a class="UI-icon-container" href="javascript:void(0);" data-bind="click: save">
            <span class="UI-icon icon-save"></span>
            <span><%: Html.Term("Save") %></span>
        </a>
        <span class="ClearAll"></span>
    </div>

    <table class="DataGrid ClickableDataGrid" cellspacing="0" width="100%">
        <thead>
            <tr class="GridColHead UI-bg UI-header">
                <%-- Disabled until we have a use for checkboxes (i.e. print packing slip).
                <th class="GridCheckBox">
                    <input type="checkbox" class="checkAll" />
                </th>--%>
                <% foreach (var gridColumn in Model.GridColumns) { %>
                    <% if (string.IsNullOrWhiteSpace(gridColumn.OrderBy)) { %>
                        <th>
                            <%: Html.Term(gridColumn.TermName, gridColumn.Name) %>
                        </th>
                    <% } else { %>
                        <th class="sort">
                            <a class="FL" href="javascript:void(0);" data-bind="click: function() { setOrderBy('<%: gridColumn.OrderBy %>'); }">
                                <%: Html.Term(gridColumn.TermName, gridColumn.Name) %>
                            </a>
                            <span class="FR IconLink SortColumn" data-bind="visible: SearchParams.OrderBy() === '<%: gridColumn.OrderBy %>', css: { SortDescend: SearchParams.OrderByDirection() === 'Descending' }" />
                        </th>
                    <% } %>
                <% } %>
            </tr>
        </thead>
        <tbody data-bind="template: { foreach: Packages, name: getTemplateName }">
        </tbody>
    </table>
    <div class="UI-mainBg Pagination">
        <div class="PaginationContainer">
            <div class="Bar">
                <a href="javascript:void(0);" class="previousPage" data-bind="css: { disabled: !enablePreviousPage() }, click: previousPage">
                    <span>&lt;&lt; <%: Html.Term("Previous") %></span>
                </a>
                <span class="pages" data-bind="text: pagesText"></span>
                <a href="javascript:void(0);" class="nextPage" data-bind="css: { disabled: !enableNextPage() }, click: nextPage">
                    <span><%: Html.Term("Next") %> &gt;&gt;</span>
                </a>
                <span class="ClearAll clr"></span>                        
            </div>
            <div class="PageSize">
            </div>
            <span class="ClearAll clr"></span>
        </div>
    </div>

    <script type="text/html" id="package-template">
        <tr>
            <%--<td><input type="checkbox" data-bind="checked: IsSelected" /></td>--%>
            <td>
                <input type="text" data-bind="value: TrackingNumber, attr: { tabindex: TabIndex }, css: { modified: IsModified }" />
                <%-- Disabled until we have time to build the split UI.
                <a href="javascript:void(0);"><%: Html.Term("Split") %></a>--%>
            </td>  
            <td>
                <input type="text" data-bind="value: DateShipped, attr: { tabindex: TabIndex }, css: { modified: IsModified }" />
            </td>               
            <td><a data-bind="attr: { href: OrderUrl }, text: OrderNumber"></a></td>
            <td><span data-bind="text: PackageNumber"></span></td>
            <td><span data-bind="text: FirstName"></span></td>
            <td><span data-bind="text: LastName"></span></td>
            <td><span data-bind="text: OrderType"></span></td>
            <td><span data-bind="text: OrderStatus"></span></td>
            <td><span data-bind="text: OrderShipmentStatus"></span></td>
            <td><span data-bind="text: CompleteDate"></span></td>
            <td><a data-bind="attr: { href: ConsultantUrl }, text: ConsultantFullName"></a></td>
        </tr>
    </script>

    <script type="text/html" id="nopackage-template">
        <tr>
            <%--<td><input type="checkbox" data-bind="checked: IsSelected" /></td>--%>
            <td><%: Html.Term("NoShippingInfo", "No Shipping Info") %></td>
            <td></td>
            <td><a data-bind="attr: { href: OrderUrl }, text: OrderNumber"></a></td>
            <td></td>
            <td></td>
            <td></td>
            <td><span data-bind="text: OrderType"></span></td>
            <td><span data-bind="text: OrderStatus"></span></td>
            <td></td>
            <td><span data-bind="text: CompleteDate"></span></td>
            <td><a data-bind="attr: { href: ConsultantUrl }, text: ConsultantFullName"></a></td>
        </tr>
    </script>
</asp:Content>
