<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            var t = $(this);
            showLoading(t);
            $.get('<%= ResolveUrl(string.Format("~/Products/Products/GetPricesCatalogs/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>',
                        { productId: $("#hidProductId").val(), currencyId: $("#sCurrency").val(), catalogID: $("#sCatalogPricing").val() }, function (response) {
                            if (response.result) {
                                hideLoading(t);
                                $('.currencySymbol').text(response.currencySymbol);
                                $('.price').val('');
                                if (response.prices) {
                                    for (var i = 0; i < response.prices.length; i++) {
                                        $('#priceType' + response.prices[i].priceTypeId).val(response.prices[i].price.toFixed(2));
                                    }
                                }
                            } else {
                                showMessage(response.message, true);
                            }
                        });
        });

        $(function () {
            $('.numeric').numeric();
            $('#sCurrency').change(function () {
                var t = $(this);
                showLoading(t);
                $.get('<%= ResolveUrl(string.Format("~/Products/Products/GetPricesCatalogs/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>',
                        { productId: $("#hidProductId").val(), currencyId: $(this).val(), catalogID: $("#sCatalogPricing").val() }, function (response) {
                            if (response.result) {
                                hideLoading(t);
                                $('.currencySymbol').text(response.currencySymbol);
                                $('.price').val('');
                                if (response.prices) {
                                    for (var i = 0; i < response.prices.length; i++) {
                                        $('#priceType' + response.prices[i].priceTypeId).val(response.prices[i].price.toFixed(2));
                                    }
                                }
                            } else {
                                showMessage(response.message, true);
                            }
                        });
            });
            /*CATALGOS*/
            $('#sCatalogPricing').change(function () {

                var t = $(this);
                showLoading(t);
                $.get('<%= ResolveUrl(string.Format("~/Products/Products/GetPricesCatalogs/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>',
                { productId: $("#hidProductId").val(), currencyId: $("#sCurrency").val(), catalogID: $(this).val() }, function (response) {
                    if (response.result) {
                        hideLoading(t);
                        $('.currencySymbol').text(response.currencySymbol);
                        $('.price').val('');
                        if (response.prices) {
                            for (var i = 0; i < response.prices.length; i++) {
                                $('#priceType' + response.prices[i].priceTypeId).val(response.prices[i].price.toFixed(2));
                            }
                        }
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });
            /**/
            $('#sProduct').change(function () {
                var pId = $(this).val();
                var data = {
                    productId: pId,
                    currencyId: $('#sCurrency').val(),
                    catalogID: $("#sCatalogPricing").val()
                };
                var t = $(this);
                $('#hidProductId').val(pId);
                showLoading(t);
                $.post('<%= ResolveUrl(string.Format("~/Products/Products/GetPricesCatalogs/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data, function (response) {
                    hideLoading(t);
                    if (response.result) {
                        $('.currencySymbol').text(response.currencySymbol);
                        $('.price').val('');
                        if (response.prices) {
                            for (var i = 0; i < response.prices.length; i++) {
                                $('#priceType' + response.prices[i].priceTypeId).val(response.prices[i].price.toFixed(2));
                            }
                        }
                        if (response.isVariant) {
                            if (response.useMasterPricing) {
                                $('#chkUseMasterPricing').prop('checked');
                            }
                            else {
                                $('#chkUseMasterPricing').removeAttr('checked');
                            }
                            $('#divUseMasterPricing').show();
                            showPriceWarning();
                            showHideTable();
                        }
                        else {
                            $('#divUseMasterPricing').hide();
                            $('#chkUseMasterPricing').removeAttr('checked');
                            showHideTable();
                        }
                    }
                    else {
                        showMessage(response.message);
                    }
                });

            });

            $('#chkUseMasterPricing').change(function () {
                var data = {
                    productId: $('#hidProductId').val(),
                    useMasterPricing: $(this).prop('checked')
                };
                $.post('<%= ResolveUrl(string.Format("~/Products/Products/ChangeUseMasterPricingInfo/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data, function (response) {
                    if (response.result) {
                        showHideTable();
                        showMessage('<%= Html.Term("SavedSuccessfully") %>');
                    }
                    else {
                        showMessage(response.message);
                    }
                });

                showPriceWarning();
            });

            $('#btnSavePrices').click(function () {

          
                if ($('#prices').checkRequiredFields()) {

                    var data = { productId: $('#hidProductId').val(), currencyId: $('#sCurrency').val(), catalogID: $('#sCatalogPricing').val() }, t = $(this);

                    $('#prices tr:gt(0) input[value!=""]').each(function (i) {
                        var priceInput = $(this);
                        data['prices[' + i + '].Key'] = priceInput.attr('id').replace(/\D/g, '');
                        data['prices[' + i + '].Value'] = priceInput.val();
                    });


                    var options = {
                        url: '<%= ResolveUrl(string.Format("~/Products/Products/SavePrices/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>',
                        showLoading: t,
                        data: data,
                        success: function (response) {
                            showMessage(response.message || '<%= Html.Term("PricesSaved", "Prices saved") %>!', !response.result);
                        }
                    };
                    NS.post(options);
                }
            });

        });

        function showHideTable() {
            if ($('#chkUseMasterPricing').prop('checked')) {
                $('#prices').hide();
                $('#btnSavePrices').hide();
            }
            else {
                $('#prices').show();
                $('#btnSavePrices').show();
            }
        }

        function showPriceWarning() {
            if ($('#chkUseMasterPricing').prop('checked')) {
                $('#AdjustPriceWarning').hide();
            }
            else {
                $('#AdjustPriceWarning').show();
            }

        }
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
            <%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>">
                <%= Model.Translations.Name() %></a> >
    <%= Html.Term("Pricing") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
    <input type="hidden" id="hidProductId" value="<%=Model.ProductID %>" />
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ProductPricing", "Product Pricing") %></h2>
        <%= Html.Term("Currency", "Currency") %>:
        <select id="sCurrency">
            <%  var activeCurrencies = (from country in SmallCollectionCache.Instance.Countries
                                        join currency in SmallCollectionCache.Instance.Currencies on country.CurrencyID equals currency.CurrencyID
                                        where country.Active
                                        select currency).Distinct().ToList();
                Currency firstCurrency = activeCurrencies.FirstOrDefault();
                foreach (Currency currency in activeCurrencies)
                { %>
            <option value="<%= currency.CurrencyID %>" <%= firstCurrency != null && firstCurrency.CurrencyID == currency.CurrencyID ? "selected=\"selected\"" : "" %>>
                <%= currency.Name %></option>
            <%} %>
        </select>
        <!-- KLC -CSTI -->
        <%= Html.Term("Catalog", "Catalog")%>:
        <select id="sCatalogPricing">
            <% foreach (var items in ViewData["CatalogsID"] as List<PricesPerCatalogsData>)
                   {
                %>
                    <option value="<%=items.CatalogID %>"><%=items.Name%></option>
                <%                                       
                  }                   
            %>
        </select>
        <!--end -->
        
        <%
              Response.Write(Html.Term("Product") + ":");
              Response.Write(Html.DropDownVariantProducts(htmlAttributes: new { id = "sProduct" }, selectedProductID: (Model.ProductID), productBase: (Model.ProductBase), includeVariantTemplate: (true)));
           
          %>
    </div>
    <div id="divUseMasterPricing" style="display: none">
        <input type="checkbox" id="chkUseMasterPricing" <%= (Model.ProductVariantInfo != null && Model.ProductVariantInfo.UseMasterPricing) ? "checked=\"checked\"" : "" %> />
        <%=Html.Term("UseMasterPricing", "Use Master Pricing") %><span id="AdjustPriceWarning">(<%=Html.Term("WillUpdatePricingAutomatically", "Checking This will update the pricing to match the master product automatically") %>)</span>
    </div>
    <table width="100%" class="SectionTable">
        <tr>
            <td style="width: 40%;">
                <%--<h3>
					Price Types <span class="FR"></span></h3>--%>
                <table id="prices" width="100%" class="DataGrid">
                    <tr class="GridColHead">
                        <th style="width: 11.364em;min-width: 11.364em;">
                            <%= Html.Term("PriceType", "Price Type") %>
                        </th>
                        <th style="min-width: 11.364em;">
                            <%= Html.Term("Price", "Price") %>
                        </th>
                    </tr>
                    <%foreach (ProductPriceType priceType in SmallCollectionCache.Instance.ProductPriceTypes)
                      {
                          bool required = NetSteps.Data.Entities.Business.Logic.ProductPriceTypeExtensionBusinessLogic.Instance.GetMandatory(priceType.ProductPriceTypeID);
                    %>
                    <tr>
                        <td>
                            <%= priceType.Name %>
                        </td>
                        <td>
                            <% ProductPrice price = Model.Prices.FirstOrDefault(pp => pp.CurrencyID == firstCurrency.CurrencyID && pp.ProductPriceTypeID == priceType.ProductPriceTypeID); %>
                            <span class="currencySymbol">
                                <%= firstCurrency == null ? "" : firstCurrency.CurrencySymbol %></span>
                                <input type="text" id="priceType<%= priceType.ProductPriceTypeID %>" class="numeric price<%= required == true ? " required" : "" %>" name="<%= required ? priceType.Name + "is required." : "" %>" style="width: 6.25em;" value="<%= price == null || price.ProductPriceID == 0 ? "" : price.Price.ToString("F2") %>" />
                        </td>
                    </tr>
                    <%} %>
                </table>
                <p>
                    <a href="javascript:void(0);" id="btnSavePrices" class="Button BigBlue">
                        <%= Html.Term("SavePrices", "Save Prices") %></a>
                </p>
            </td>
            <td style="width: 60%;">
            </td>
        </tr>
    </table>
</asp:Content>
