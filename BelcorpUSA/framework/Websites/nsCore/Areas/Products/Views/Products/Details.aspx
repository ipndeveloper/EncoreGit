<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Products.Models.ProductDetailsModel>" %>

<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
            <%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>">
                <%= Model.Translations.Name() %></a> >
    <%= Html.Term("Details") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#txtWeight').numeric();
            $('#btnSave').click(function () {
                if ($('#details').checkRequiredFields()) {
                    var data = {
                        productTypeId: $('#sProductType').val(),
                        taxCategoryId: $('#sTaxCategory').val(),
                        backOrderBehaviorId: $('#backOrderBehavior').val(),
                        weight: $('#txtWeight').val(),
                        chargeShipping: $('#chkChargeShipping').prop('checked'),
                        chargeTax: $('#chkChargeTax').prop('checked'),
                        chargeTaxOnShipping: $('#chkChargeTaxOnShipping').prop('checked'),
                        isShippable: $('#chkIsShippable').prop('checked'),
                        updateInventoryOnBase: $('#chkUpdateInventoryOnBase').prop('checked'),
                        showKitContents: $('#chkShowKitContents').prop('checked')
                    };
                    var i = 0;
                    $('input[name="excludedShippingMethod"]').each(function () {
                        if ($(this).prop('checked')) {
                          data['excludedShippingMethodIds[' + i + ']'] = $(this).val();
                            ++i;
                        }
                    });

                    var p = $('#btnSave').parent();
                    showLoading(p);
                    $.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveDetails/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', data, function (response) {
                        hideLoading(p);
                        showMessage(response.message || '<%= Html.Term("DetailsSaved", "Details saved!") %>', !response.result);
                    });
                }
            });
        });
    </script>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Details", "Details") %></h2>
    </div>
    <table id="details" class="FormTable" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("SKU", "SKU") %>:
            </td>
            <td>
                <input id="txtSKU" type="text" class="required" name="<%= Html.Term("SKUisRequired", "SKU is required.") %>" value="<%= Model.Sku %>" readonly />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 200px;">
                <%= Html.Term("ProductType", "Product Type") %>:
            </td>
            <td>
                <select id="sProductType">
                    <%foreach (var productType in SmallCollectionCache.Instance.ProductTypes.Where(pt => pt.Active))
                      { %>
                    <option value="<%= productType.ProductTypeID %>" <%= productType.ProductTypeID == Model.ProductTypeId ? "selected=\"selected\"" : "" %>>
                        <%= productType.Name %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 200px;">
                <%= Html.Term("TaxCategory", "Tax Category") %>:
            </td>
            <td>
                <select id="sTaxCategory">
                    <option value="">
                        <%= Html.Term("None", "None") %></option>
                    <%foreach (var taxCategory in SmallCollectionCache.Instance.TaxCategories)
                      { %>
                    <option value="<%= taxCategory.TaxCategoryID %>" <%= Model.TaxCategoryId == taxCategory.TaxCategoryID ? "selected=\"selected\"" : "" %>>
                        <%= taxCategory.GetTerm() %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("BackOrderBehavior", "Back Order Behavior") %>:
            </td>
            <td>
                <select id="backOrderBehavior">
                    <%foreach (var behavior in SmallCollectionCache.Instance.ProductBackOrderBehaviors)
                      { %>
                    <option value="<%= behavior.ProductBackOrderBehaviorID %>" <%= behavior.ProductBackOrderBehaviorID == Model.BackOrderBehaviorId ? "selected=\"selected\"" : "" %>>
                        <%= behavior.GetTerm() %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Weight", "Weight") %>:
            </td>
            <td>
                <input id="txtWeight" type="text" value="<%= Model.Weight %>" />
            </td>
        </tr>
		  <tr>
            <td class="FLabel">
                <%= Html.Term("ChargeShipping", "Charge Shipping") %>:
            </td>
            <td>
                <input id="chkChargeShipping" type="checkbox" <%= Model.ChargeShipping ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
		  <tr>
            <td class="FLabel">
                <%= Html.Term("ChargeTax", "Charge Tax") %>:
            </td>
            <td>
                <input id="chkChargeTax" type="checkbox" <%= Model.ChargeTax ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr <%if (!Model.DisplayChargeTaxOnShipping)
				  { %> style="display:none" <% } %>>
            <td class="FLabel">
                <%= Html.Term("ChargeTaxOnShipping", "Charge Tax On Shipping") %>:
            </td>
            <td>
                <input id="chkChargeTaxOnShipping" type="checkbox" <%= Model.ChargeTaxOnShipping ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("IsShippable", "Is Shippable") %>:
            </td>
            <td>
                <input id="chkIsShippable" type="checkbox" <%= Model.IsShippable ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <% if (!Model.Product.IsVariant()) { %>
            <tr>
                 <td class="FLabel">
                        <%= Html.Term("RestrictedShippingMethods", "Restricted Shipping Methods")%>:
                 </td>
                 <td>
                 <% foreach (var shippingMethod in Model.AvailableShippingMethods)
		            { %>
					    <input type="checkbox" name="excludedShippingMethod" id="excludedShippingMethodCheckBox<%= shippingMethod.ShippingMethodID %>" value="<%=  shippingMethod.ShippingMethodID %>" <%= Model.ExcludedShippingMethodIds != null && Model.ExcludedShippingMethodIds.Contains(shippingMethod.ShippingMethodID) ? "checked=\"checked\"" : "" %> />
					    <label for="excludedShippingMethodCheckBox<%= shippingMethod.ShippingMethodID %>">
					    <%= shippingMethod.Name %></label><br />
				    <%}%>
                 </td>
            </tr>
        <% } %>
        <tr>
            <td class="FLabel">
                <%= Html.Term("UpdateInventoryOnBase", "Update Inventory On Base") %>:
            </td>
            <td>
                <input id="chkUpdateInventoryOnBase" type="checkbox" <%= Model.UpdateInventoryOnBase ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <% if (Model.ChildProductRelations != null && Model.ChildProductRelations.Count(r => r.ProductRelationsTypeID == (int)Constants.ProductRelationsType.Kit) > 0)
           { %>
        <tr>
            <td class="FLabel">
                <%= Html.Term("ShowKitContents", "Show Kit Contents")%>:
            </td>
            <td>
                <input id="chkShowKitContents" type="checkbox" <%= (Model.ShowKitContents.HasValue && Model.ShowKitContents.Value) ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <%} %>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" class="Button BigBlue">
                        <%= Html.Term("Save", "Save") %></a>
                </p>
            </td>
        </tr>
    </table>
    <%Html.RenderPartial("MessageCenter"); %>
</asp:Content>
