<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Warehouses/Warehouses.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            function getAmountAvailable() {
                if ($('#productId').val() && $('#fromWarehouse').val()) {
                    $.get('<%= ResolveUrl("~/Products/Warehouses/GetAmountAvailable") %>', { productId: $('#productId').val(), fromWarehouseId: $('#fromWarehouse').val(), toWarehouseId: $('#toWarehouse').val() }, function (response) {
                        if (response.result === undefined || response.result) {
                            $('#amountAvailableFrom').text(response.result ? response.amountAvailableFrom : 0);
                            $('#amountAvailableTo').text(response.result ? response.amountAvailableTo : 0);
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            }

            //var unavailableToWarehouse;

            $('#productSearch').jsonSuggest('<%= ResolveUrl("~/Products/Products/Search") %>', { onSelect: function (item) {
                $('#productId').val(item.id);
                getAmountAvailable();
            }, minCharacters: 3, ajaxResults: true, maxResults: 10, showMore: true
            });

            $('#txtAmount').numeric({ allowNegative: false });

            $('#fromWarehouse').change(function () {
                $('#toWarehouse option[disabled]').removeAttr('disabled');
                $('#toWarehouse option[value=' + $(this).val() + ']').attr('disabled', 'disabled');
                if (!$('#toWarehouse').val() || ($('#toWarehouse').val().constructor === Array && !$('#toWarehouse').val().length))
                    $('#toWarehouse option:not(option[disabled]):first').attr('selected', 'selected');
                getAmountAvailable();
            }).triggerHandler('change');

            $('#toWarehouse').change(getAmountAvailable);

            $('#btnTransfer').click(function () {
                clearErrors();
                if (!$('#productId').val()) {
                    $('#productSearch').showError('<%= Html.JavascriptTerm("PleaseSelectAProduct", "Please select a product") %>');
                    return false;
                }
                if ($('#txtAmount').val() == '' || parseInt($('#txtAmount').val()) == 0) {
                    $('#txtAmount').showError('<%= Html.JavascriptTerm("AmountToTransferMustBeGreaterThanZero", "Amount to transfer must be greater than zero") %>');
                    return false;
                }
                if ((parseInt($('#txtAmount').val()) > parseInt($('#amountAvailableFrom').text())) || parseInt($('#amountAvailableFrom').text()) == 0) {
                    $('#txtAmount').showError('<%= Html.JavascriptTerm("NotEnoughAvailable", "There is not enough available to transfer.") %>');
                    return false;
                }
                if ($('#fromWarehouse').val() == $('#toWarehouse').val()) {
                    $('#toWarehouse').showError('<%= Html.JavascriptTerm("PleaseSelectADifferentWarehouse", "Please select a different warehouse") %>').change(function () {
                        if ($('#fromWarehouse').val() != $('#toWarehouse').val()) {
                            $('#toWarehouse').clearError().unbind('change');
                        }
                    });

                    return false;
                }

                $.post('<%= ResolveUrl("~/Products/Warehouses/PerformTransfer") %>', {
                    productId: $('#productId').val(),
                    fromWarehouse: $('#fromWarehouse').val(),
                    toWarehouse: $('#toWarehouse').val(),
                    amount: $('#txtAmount').val()
                }, function (response) {
                    showMessage(response.message || '<%= Html.Term("TransferCompleted", "Transfer completed successfully!") %>', !response.result);
                    if (response.result) {
                        getAmountAvailable();
                    }
                });
            });
            function clearErrors() {
                $('#txtAmount').clearError();
                $('#productSearch').clearError();
                $('#toWarehouse').clearError();
                $('#productSearch').clearError();
            }
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Warehouses") %>">
            <%= Html.Term("WarehouseInventory", "Warehouse Inventory") %></a> >
    <%= Html.Term("TransferWarehouseInventory", "TransferWarehouseInventory") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("WarehouseInventory", "Warehouse Inventory") %></h2>
        <a href="<%= ResolveUrl("~/Products/Warehouses") %>">
            <%= Html.Term("WarehouseInventory", "Warehouse Inventory")%></a> |
        <%= Html.Term("TransferWarehouseInventory", "Transfer Warehouse Inventory") %>
    </div>
    <table class="FormTable" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("TransferProduct", "Transfer Product") %>:<br />
                <span style="font-size: 9px;">
                    <%= Html.Term("EnterProductSKUOrName", "(Enter product SKU or name)") %></span>
            </td>
            <td>
                <input type="text" id="productSearch" />
                <input type="hidden" id="productId" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("From") %>:
            </td>
            <td>
                <select id="fromWarehouse">
                    <% foreach (var warehouse in SmallCollectionCache.Instance.Warehouses)
                       { %>
                    <option value="<%= warehouse.WarehouseID %>">
                        <%= warehouse.GetTerm() %></option>
                    <%} %>
                </select><br />
                <%= Html.Term("AmountAvailable", "Amount Available") %>: <span id="amountAvailableFrom">
                </span>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("To") %>:
            </td>
            <td>
                <select id="toWarehouse">
                    <% foreach (var warehouse in SmallCollectionCache.Instance.Warehouses)
                       { %>
                    <option value="<%= warehouse.WarehouseID %>">
                        <%= warehouse.GetTerm() %></option>
                    <%} %>
                </select><br />
                <%= Html.Term("AmountAvailable", "Amount Available") %>: <span id="amountAvailableTo">
                </span>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Amount") %>:
            </td>
            <td>
                <input type="text" id="txtAmount" />
            </td>
        </tr>
    </table>
    <p>
    </p>
    <table class="FormTable" width="100%">
        <tr>
            <td class="FLabel">
                &nbsp;
            </td>
            <td>
                <p>
                    <a id="btnTransfer" href="javascript:void(0);" class="Button BigBlue"><span>
                        <%= Html.Term("Transfer") %></span></a></p>
            </td>
        </tr>
    </table>
</asp:Content>
