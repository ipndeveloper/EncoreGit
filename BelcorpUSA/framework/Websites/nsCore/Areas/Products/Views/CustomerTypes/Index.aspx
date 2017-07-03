<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#sStoreFront').change(function () {
                $('#accountTypes .accountType').each(function () { $('option:first', this).attr('selected', 'selected'); $(this).next().val('0'); });
                $.get('<%= ResolveUrl("~/Products/CustomerTypes/Get") %>', { storeFrontId: $(this).val() }, function (response) {
                    if (response.result === undefined || response.result) {
                        for (var i = 0; i < response.length; i++) {
                            $('#account' + response[i].accountType + 'RelationshipType' + response[i].relationshipType).val(response[i].priceType);
                            $('#account' + response[i].accountType + 'RelationshipType' + response[i].relationshipType + 'ID').val(response[i].accountPriceType);
                        }
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });
            $('#btnSaveCustomerTypes').click(function () {
                var data = {}, storeFrontId = $('#sStoreFront').val();
                $('#accountTypes .accountType').each(function (i) {
                    var accountTypeId = $(this).attr('id').replace(/^account(\d+).*/, '$1');
                    data['accountTypes[' + i + '].ProductPriceTypeID'] = $(this).val();
                    data['accountTypes[' + i + '].AccountTypeID'] = accountTypeId;
                    data['accountTypes[' + i + '].PriceRelationshipTypeID'] = $(this).attr('id').replace(/.*RelationshipType(\d+)$/, '$1');
                    data['accountTypes[' + i + '].StoreFrontID'] = storeFrontId;
                    data['accountTypes[' + i + '].AccountPriceTypeID'] = $(this).next().val();
                });

                $.post('<%= ResolveUrl("~/Products/CustomerTypes/Save") %>', data, function (response) {
                    if (response.result) {
                        showMessage('Customer Types saved!', false);
                        for (var i = 0; i < response.accountTypes.length; i++) {
                            $('#account' + response.accountTypes[i].accountType + 'RelationshipType' + response.accountTypes[i].relationshipType + 'ID').val(response.accountTypes[i].accountPriceType);
                        }
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> >
    <%= Html.Term("CustomerTypes", "Customer Types") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("CustomerTypes", "Customer Types") %>
        </h2>
        <%= Html.Term("StoreFront", "Store Front") %>:
        <select id="sStoreFront">
            <% StoreFront firstStoreFront = SmallCollectionCache.Instance.StoreFronts.FirstOrDefault();
               foreach (StoreFront storeFront in SmallCollectionCache.Instance.StoreFronts)
               { %>
            <option value="<%= storeFront.StoreFrontID %>" <%= firstStoreFront != null && storeFront.StoreFrontID == firstStoreFront.StoreFrontID ? "selected=\"selected\"" : "" %>>
                <%= storeFront.GetTerm() %></option>
            <%} %>
        </select>
    </div>
    <table id="accountTypes" width="100%" class="DataGrid">
        <tr class="GridColHead">
            <th>
                <%= Html.Term("Account Type", "Account Type") %>
            </th>
            <th>
                <%= Html.Term("Products", "Products") %>
            </th>
            <th>
                <%= Html.Term("Taxes", "Taxes") %>
            </th>
            <th>
                <%= Html.Term("Commissionable", "Commissionable") %>
            </th>
        </tr>
        <% var accountPriceTypes = firstStoreFront == null ? new List<AccountPriceType>() : AccountPriceType.LoadAllByStoreFront(firstStoreFront.StoreFrontID);
           foreach (AccountType accountType in SmallCollectionCache.Instance.AccountTypes)
           { %>
        <tr>
            <td>
                <%= accountType.GetTerm() %>
            </td>
            <td>
                <select id="account<%= accountType.AccountTypeID %>RelationshipType1" class="accountType">
                    <% AccountPriceType pt = accountPriceTypes.FirstOrDefault(apt => apt.AccountTypeID == accountType.AccountTypeID && apt.PriceRelationshipTypeID == (int)Constants.PriceRelationshipType.Products);
                       foreach (ProductPriceType priceType in SmallCollectionCache.Instance.ProductPriceTypes)
                       { %>
                    <option value="<%= priceType.ProductPriceTypeID %>" <%= pt != null && priceType.ProductPriceTypeID == pt.ProductPriceTypeID ? "selected=\"selected\"" : "" %>>
                        <%= priceType.GetTerm() %></option>
                    <%} %>
                </select>
                <input type="hidden" id="account<%= accountType.AccountTypeID %>RelationshipType1ID"
                    value="<%= pt != null ? pt.AccountPriceTypeID : 0 %>" />
            </td>
            <td>
                <select id="account<%= accountType.AccountTypeID %>RelationshipType2" class="accountType">
                    <% pt = accountPriceTypes.FirstOrDefault(apt => apt.AccountTypeID == accountType.AccountTypeID && apt.PriceRelationshipTypeID == (int)Constants.PriceRelationshipType.Taxes);
                       foreach (ProductPriceType priceType in SmallCollectionCache.Instance.ProductPriceTypes)
                       { %>
                    <option value="<%= priceType.ProductPriceTypeID %>" <%= pt != null && priceType.ProductPriceTypeID == pt.ProductPriceTypeID ? "selected=\"selected\"" : "" %>>
                        <%= priceType.GetTerm()%></option>
                    <%} %>
                </select>
                <input type="hidden" id="account<%= accountType.AccountTypeID %>RelationshipType2ID"
                    value="<%= pt != null ? pt.AccountPriceTypeID : 0 %>" />
            </td>
            <td>
                <select id="account<%= accountType.AccountTypeID %>RelationshipType3" class="accountType">
                    <% pt = accountPriceTypes.FirstOrDefault(apt => apt.AccountTypeID == accountType.AccountTypeID && apt.PriceRelationshipTypeID == (int)Constants.PriceRelationshipType.Commissions);
                       foreach (ProductPriceType priceType in SmallCollectionCache.Instance.ProductPriceTypes)
                       { %>
                    <option value="<%= priceType.ProductPriceTypeID %>" <%= pt != null && priceType.ProductPriceTypeID == pt.ProductPriceTypeID ? "selected=\"selected\"" : "" %>>
                        <%= priceType.GetTerm()%></option>
                    <%} %>
                </select>
                <input type="hidden" id="account<%= accountType.AccountTypeID %>RelationshipType3ID"
                    value="<%= pt != null ? pt.AccountPriceTypeID : 0 %>" />
            </td>
        </tr>
        <%} %>
    </table>
    <p>
        <a href="javascript:void(0);" id="btnSaveCustomerTypes" class="Button BigBlue">
            <%= Html.Term("Save", "Save") %>
            <%= Html.Term("Customer Types", "Customer Types") %></a>
    </p>
</asp:Content>
