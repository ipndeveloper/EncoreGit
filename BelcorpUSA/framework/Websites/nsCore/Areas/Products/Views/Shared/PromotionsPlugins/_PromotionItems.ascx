<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<nsCore.Areas.Products.Models.Promotions.PromotionProductModel>>" %>
<script type="text/javascript">
    $(function () {

        $('input[monedaidioma=CultureIPN]').keyup(function (event) {

            var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
            // var value = parseFloat($(this).val());


            var formatDecimal = '$1.$2'; // valores por defaul 
            var formatMiles = ",";  // valores por defaul

            if (cultureInfo === 'en-US') {
                formatDecimal = '$1.$2';
                formatMiles = ",";
            }
            else if (cultureInfo === 'es-US') {
                formatDecimal = '$1,$2';
                formatMiles = ".";
            }
            else if (cultureInfo === 'pt-BR') {
                formatDecimal = '$1,$2';
                formatMiles = ".";
            }


            //            if (!isNaN(value)) {
            if (event.which >= 37 && event.which <= 40) {
                event.preventDefault();
            }

            $(this).val(function (index, value) {


                return value.replace(/\D/g, "")
                                 .replace(/([0-9])([0-9]{2})$/, formatDecimal)
                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, formatMiles);
            });

            //            }

        });




        $('.numeric').numeric({ negative: false });

        $.each([['#chkSelectAll', '#paginatedGrid .selectRow'],
			['#chkBulkAdd', '#bulkProductCatalog .bulkAdd']],
			function (i, item) {
			    $(item[0]).click(function () {
			        $(item[1]).attr('checked', $(this).prop('checked'));
			    });
			});

        $('.deleteButton').click(function () {
            $('#paginatedGrid .selectRow:checked').closest('tr').remove();
            $('#paginatedGrid').refreshTable();
            $('#chkSelectAll').attr('checked', false);
        });

        $('#catalogSelector, #bulkAddModal').jqm({ modal: false,
            onShow: function (h) {
                h.w.css({
                    top: $('#btnOpenProductBulkAdd').offset().top - $('#btnOpenProductBulkAdd').offset().bottom / 1.5 + 'px',
                    left: '0px',
                    'margin-left': '0px'
                }).fadeIn();
            },
            onHide: function (h) {
                h.w.fadeOut('slow');
            }, overlay: 0
        });

        $('#btnOpenProductBulkAdd').click(function () {
            var options = {
                url: '<%= ResolveUrl("~/Products/ProductPromotions/GetAvailableCatalogs") %>',
                showLoading: $(this),
                success: function (response) {
                    if (response.result) {
                        var rows = '';
                        for (catalogId in response.catalogs) {
                            rows += '<option value="' + catalogId + '">' + response.catalogs[catalogId] + '</option>';
                        }
                        $('#catalogFilter').empty().append(rows).change();
                        $('#bulkAddModal').jqmShow();
                    }
                    else {
                        showMessage(response.message, true);
                    }
                }
            };
            NS.get(options);
        });

        $('#btnBulkAdd').click(function () {
            var rows = '';
            $('#bulkProductCatalog .bulkAdd:checked').each(function () {
                var row = $(this).closest('tr');
                var productInfo = {
                    ProductID: row.find('.productId').val(),
                    SKU: row.find('.sku').text(),
                    Name: row.find('.name').text(),
                    RetailPrice: parseInt(row.find('.retailPrice').text()),
                    QVPrice: parseInt(row.find('.qvPrice').val()),
                    CVPrice: parseInt(row.find('.cvPrice').val()),
                    AdditionalPriceTypeValues: row.data('AdditionalPriceTypeValues')
                };
                if (!alertIfAlreadyInGrid(productInfo.ProductID)) {
                    rows += buildProductRow(productInfo);
                }
            });
            $('#paginatedGrid').append(rows).refreshTable();
        });

        $('#catalogFilter').change(function () {
            var options = {
                url: '<%= ResolveUrl("~/Products/ProductPromotions/GetProductInformationFromCatalog") %>',
                data: {
                    catalogId: $('#catalogFilter option:selected').val(),
                    marketId: $('.singleMarket').val()
                },
                showLoading: $('#bulkProductCatalog'),
                success: function (response) {
                    if (response.result) {
                        var rows = '';
                        response.products.each(function (product, index) { rows += buildBulkAddModalProductRow(product); });
                        $('#bulkProductCatalog').append(rows).refreshTable().find('tr').each(function (i) {
                            $(this).data('AdditionalPriceTypeValues', response.products[i].AdditionalPriceTypeValues);
                        });
                    }
                    else {
                        showMessage(response.message, true);
                    }
                }
            }
            $('#bulkProductCatalog').empty();
            NS.get(options);
        });

        $('#btnOpenPromotionProductCopier').click(function () {
            var options = {
                url: '<%= ResolveUrl("~/Products/ProductPromotions/GetAvailableCatalogs") %>',
                showLoading: $(this),
                success: function (response) {
                    if (response.result) {
                        var rows = '';
                        for (catalogId in response.catalogs) {
                            rows += '<input type="radio" name="radioCatalogIDs" value="' + catalogId + '"/>' + response.catalogs[catalogId] + '<br />';
                        }
                        $('#catalogSelector .catalogList').empty().append(rows);
                        $('#catalogSelector').jqmShow();
                    }
                    else {
                        showMessage(response.message, true);
                    }
                }
            };
            NS.get(options);
        });

        $('#btnCopySKUs').click(function () {
            var data = {
                catalogId: $('.catalogList input:checked').val()
            };
            if (data.catalogId) {
                var options = {
                    url: '<%= ResolveUrl("~/Products/ProductPromotions/GetProductInformationFromCatalog") %>',
                    data: data,
                    showLoading: $(this),
                    success: function (response) {
                        if (response.result) {
                            var rows = '';
                            response.products.each(function (productInfo, index) {
                                if (!alertIfAlreadyInGrid(productInfo.ProductID)) {
                                    rows += buildProductRow(productInfo);
                                }
                            });
                            $('#paginatedGrid').append(rows).refreshTable();
                            $('.numeric').numeric({ negative: false });
                        }
                        else {
                            showMessage(response.message, true);
                        }
                    }
                };
                NS.get(options);
            }
        });

        $('#productQuickAdd').jsonSuggest('<%= ResolveUrl("~/Products/Promotions/Search") %>', {
            minCharacters: 3, ajaxResults: true, onSelect: function (item) {
                $(this).text(item.text);
                $('#quickAddProductID').val(item.id);
            }
        });

        $('#btnProductQuickAdd').click(function () {
            var data = { productID: $('#quickAddProductID').val() };
            if (data.productID) {
                var options = {
                    url: '<%= ResolveUrl("~/Products/ProductPromotions/QuickAddProduct") %>',
                    showLoading: $('#btnProductQuickAdd'),
                    data: data,
                    success: function (response) {
                        if (response.result) {
                            if (!alertIfAlreadyInGrid(response.product.ProductID)) {
                                $('#paginatedGrid').append(buildProductRow(response.product)).refreshTable();
                                if (window['setPercentages']) { //copies values from percent entry to the new row
                                    window['setPercentages']($('#paginatedGrid .selectRow:last').closest('tr'));
                                    $('#pctRetailPrice, #pctCVPrice, #pctQVPrice').val('');
                                }
                            }
                            $('#quickAddProductID, #productQuickAdd').val('');
                            $('.numeric').numeric({ negative: false });
                        }
                        else {
                            showMessage(response.message, true);
                        }
                    }
                };
                NS.post(options);
            }
        });

        $('#PriceAdjustmentHelp').click(function () {
            $('#discountPriceHelp').fadeIn();
        });

        $('.hideDesc').click(function () {
            $(this).closest('div.desc').fadeOut('fast');
        });

        $('input.retailPrice').live('change', function () {

            
            var t = $(this),
				modifier = t.val(),
				isPercentOff = $('#percentForm').is(':visible');
            t.closest('tr').find('span.discountPrice').each(function () {
                
                var discountLbl = $(this);
                var originalPrice = discountLbl.attr('originalVal'),
					adjustedPrice;
                if (isPercentOff) {
                    adjustedPrice = originalPrice - (originalPrice * (modifier / 100));
                }
                else { //flat discount
                    adjustedPrice = originalPrice - modifier;
                }

                var cultura = '<%= CoreContext.CurrentCultureInfo.Name%>';
                var adjustedPrice_culture = new Intl.NumberFormat(cultura, { maximumFractionDigits: 2, minimumFractionDigits: 2 }).format(adjustedPrice);
                discountLbl.text(adjustedPrice_culture);
                //                discountLbl.text(adjustedPrice.toFixed(2));
            });
        }).change();

        function alertIfAlreadyInGrid(productId) {
            var ret = $('#paginatedGrid td input:hidden[value="' + productId + '"]').length != 0;
            if (ret) {
                showMessage('<%= Html.Term("ProductAlreadyAddedToPromotion", "Product already added to promotion") %>', true);
            }
            return ret;
        }

        function buildBulkAddModalProductRow(productInfo) {



            var cultura = '<%= CoreContext.CurrentCultureInfo.Name%>';
            var RetailPrice = new Intl.NumberFormat(cultura, { maximumFractionDigits: 2, minimumFractionDigits: 2 }).format(productInfo.RetailPrice);

            var html = '<tr>'
                + '<td style="width: 20px;"><input type="checkbox" class="bulkAdd" />'
					+ '<input type="hidden" class="productId" value="' + productInfo.ProductID + '"/>'
					+ '<input type="hidden" class="cvPrice" value="' + productInfo.CVPrice + '"/>'
					+ '<input type="hidden" class="qvPrice" value="' + productInfo.QVPrice + '"/>'
                + '</td>'
                + '<td style="width: 80px;" class="sku">' + productInfo.SKU + '</td>'
                + '<td style="width: 150px;" class="name">' + productInfo.Name + '</td>'
            //                + '<td style="width: 65px;" class="retailPrice">' + (productInfo.RetailPrice ? productInfo.RetailPrice.toFixed(2) : '<%=Html.Term("BulkAdd_NA", "N/A") %>') + '</td>'
                + '<td style="width: 65px;" class="retailPrice">' + (productInfo.RetailPrice ? RetailPrice : '<%=Html.Term("BulkAdd_NA", "N/A") %>') + '</td>'
                + '<td style="" class="active">' + (productInfo.Active ? '<%=Html.Term("Yes") %>' : '<%=Html.Term("No") %>') + '</td>'
                + '</tr>';
            return html;
        }
        $('#paginatedGrid').on('keyup', ".numeric", function () {


            var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
            // var value = parseFloat($(this).val());


            var formatDecimal = '$1.$2'; // valores por defaul 
            var formatMiles = ",";  // valores por defaul

            if (cultureInfo === 'en-US') {
                formatDecimal = '$1.$2';
                formatMiles = ",";
            }
            else if (cultureInfo === 'es-US') {
                formatDecimal = '$1,$2';
                formatMiles = ".";
            }
            else if (cultureInfo === 'pt-BR') {
                formatDecimal = '$1,$2';
                formatMiles = ".";
            }


            //            if (!isNaN(value)) {
            if (event.which >= 37 && event.which <= 40) {
                event.preventDefault();
            }

            $(this).val(function (index, value) {


                return value.replace(/\D/g, "")
                                 .replace(/([0-9])([0-9]{2})$/, formatDecimal)
                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, formatMiles);
            });
        });
        function buildProductRow(productInfo) {



            var cultura = '<%= CoreContext.CurrentCultureInfo.Name%>';
            var RetailPrice = new Intl.NumberFormat(cultura, { maximumFractionDigits: 2 , minimumFractionDigits: 2 }).format(productInfo.RetailPrice);
            var CVPrice = new Intl.NumberFormat(cultura, { maximumFractionDigits: 2 , minimumFractionDigits: 2 }).format(productInfo.CVPrice);
            var QVPrice = new Intl.NumberFormat(cultura, { maximumFractionDigits: 2 , minimumFractionDigits: 2 }).format(productInfo.QVPrice);

            var append = '';
            if ($('#percentForm').is(':visible')) {
                append = '%';
            }
            var html = '<tr>'
				+ '<td><input type="checkbox" class="selectRow" /><input type="hidden" class="productId" value="' + productInfo.ProductID + '" /></td>'
				+ '<td>' + productInfo.SKU + '</td>'
				+ '<td>' + productInfo.Name + '</td>'
            //				+ '<td class="originalPrice">' + productInfo.RetailPrice.toFixed(2) + '</td>'
                + '<td class="originalPrice">' + RetailPrice + '</td>'
                + '<td>- <input type="text"  monedaidioma="CultureIPN"  class="required retailPrice numeric" size="5"  />' + append + '</td>'
                + '<td>';
            $.each(productInfo.AdditionalPriceTypeValues, function (i) {

                

                var priceInfo = productInfo.AdditionalPriceTypeValues[i];

//                var priceInfo__ = new Intl.NumberFormat(cultura, { maximumFractionDigits: 2 }).format(productInfo.AdditionalPriceTypeValues[i].Value);
                if (priceInfo.Value) {
////                    html += '<span class="discountPrice adjVal" originalVal="' + priceInfo__ + '">' + priceInfo__ + '</span> - ' + priceInfo.PriceTypeName + '<br />';
                    html += '<span class="discountPrice adjVal" originalVal="' + priceInfo.Value.toFixed(2) + '">' + priceInfo.Value.toFixed(2) + '</span> - ' + priceInfo.PriceTypeName + '<br />';
                }
            });
            html += '</td>'
                + '<td>' + QVPrice + '</td>'
            //                + '<td>' + productInfo.CVPrice.toFixed(2) + '</td>'
                + '<td>- <input type="text" monedaidioma="CultureIPN"  class="cvPrice numeric" size="5"  />' + append + '</td>'
            //                + '<td>' + productInfo.QVPrice.toFixed(2) + '</td>'
                + '<td>' + QVPrice + '</td>'
                + '<td>- <input type="text" monedaidioma="CultureIPN"  class="qvPrice numeric" size="5"   />' + append + '</td>'
                + '</tr>';
            return html;
        }
    });
</script>
<div id="RewardsPanel">
    <h3 class="UI-lightBg pad10">
        <%= Html.Term("Promotions_PromotionItemsHeading", "Define the Promotion Items") %></h3>
    <table width="100%" class="FormTable">
        <tbody>
            <tr id="productPanel">
                <td style="vertical-align: top;" class="FLabel">
                    <div class="mr10">
                        <ul class="InputTools flatList listNav">
                            <li><a class="BulkAdd" href="javascript:void(0);" id="btnOpenProductBulkAdd">
                                <%= Html.Term("Promotions_OpenProductBulkAdd", "Open Product Bulk Add") %></a></li>
                            <li><a class="Copier" href="javascript:void(0);" id="btnOpenPromotionProductCopier">
                                <%= Html.Term("Promotions_ImportSkusFromExistingCatalog", "Import Skus From an Existing Catalog")%></a></li>
                        </ul>
                    </div>
                </td>
                <td>
                    <div class="UI-secBg pad10 brdrYYNN">
                        <div class="FL mr10">
                            <%= Html.Term("Promotions_QuickProductAdd", "Quick Product Add")%>:<br />
                            <input type="text" value="" size="30" class="pad5" id="productQuickAdd" />
                            <input type="hidden" value="" id="quickAddProductID" />
                            <input type="hidden" value="" id="quickAddProductRetailPrice" />
                            <input type="hidden" value="" id="quickAddProductQVPrice" />
                            <input type="hidden" value="" id="quickAddProductCVPrice" />
                            <a class="DTL Add" href="javascript:void(0);" id="btnProductQuickAdd">
                                <%= Html.Term("Promotions_QuickAdd", "Add")%></a>
                        </div>
                        <!-- hidden buttons for percentage off discount todo: refactor -->
                        <div style="display: none;" id="percentForm" class="FL ml10">
                            <%= Html.Term("Promotions_PercentageDiscount", "Percentage Discount")%>:<br />
                            <%= Html.Term("Promotions_PriceLabel", "Price") %>%<input type="text" class="pad5 qty numeric" monedaidioma='CultureIPN'
                                id="pctRetailPrice" />
                            <%= Html.Term("Promotions_CVLabel", "CV")%>%<input type="text" class="pad5 qty numeric" monedaidioma='CultureIPN'
                                id="pctCVPrice" />
                            <%= Html.Term("Promotions_QVLabel", "QV")%>%<input type="text" class="pad5 qty numeric" monedaidioma='CultureIPN'
                                id="pctQVPrice" />
                            <a id="percentOffApplyToSelected" class="DTL Add ml10">
                                <%= Html.Term("Promotions_ApplyToSelected", "Apply to Selected")%></a>
                        </div>
                        <!-- end hidden buttons -->
                        <span class="ClearAll clr"></span>
                    </div>
                    <div class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility" id="paginatedGridOptions">
                        <a class="deleteButton UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-deleteSelected">
                        </span><span>
                            <%=Html.Term("DeleteSelected", "Delete Selected") %></span></a> <span class="ClearAll clr">
                            </span>
                    </div>
                    <table width="100%" class="DataGrid" id="paginatedGrid">
                        <thead>
                            <tr class="GridColHead">
                                <th class="GridCheckBox">
                                    <input type="checkbox" id="chkSelectAll" />
                                </th>
                                <th>
                                    <%=Html.Term("SKU")%>
                                </th>
                                <th>
                                    <%=Html.Term("Product")%>
                                </th>
                                <th>
                                    <span class="FL mr5">
                                        <%=Html.Term("DefinePromotion_Price", "Price")%></span> <a id="PriceAdjustmentHelp"
                                            class="UI-icon-wrapper"><span class="UI-icon icon-help"></span></a>
                                    <div class="FL ml10 adjustmentDesc applyAllPricesHelp">
                                        <div class="UI-mainBg desc" id="discountPriceHelp" style="display: none;">
                                            <%= Html.Term("DefinePromotion_DiscountAppliedToAllPriceTypes", "The price adjustments you make below will apply to ALL price types. If needed, you may use the options above to create multiple promotions in order to achieve your desired results.")%>
                                            <hr />
                                            <a class="hideDesc" href="javascript:void(0);">Hide</a>
                                        </div>
                                    </div>
                                </th>
                                <th id="priceAdjLabel">
                                    <%=Html.Term("DefinePromotion_ReduceBy", "Reduce By")%>
                                </th>
                                <th>
                                    <%=Html.Term("DefinePromotion_NewPrices", "New Prices")%>
                                </th>
                                <th>
                                    <%=Html.Term("DefinePromotion_CV", "CV")%>
                                </th>
                                <th id="cvAdjLabel">
                                    <%=Html.Term("DefinePromotion_ReduceBy", "Reduce By")%>
                                </th>
                                <th>
                                    <%=Html.Term("DefinePromotion_QV", "QV")%>
                                </th>
                                <th id="qvAdjLabel">
                                    <%=Html.Term("DefinePromotion_ReduceBy", "Reduce By")%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <%
                                bool alt = true;
                                foreach (var productInfo in Model)
                                {
                                    alt = !alt;
                                    decimal? retailDiscount = null, qvDiscount = null, cvDiscount = null;
                                    if (productInfo is nsCore.Areas.Products.Models.Promotions.IFlatDiscountPromotionProductModel)
                                    {
                                        var flatDiscountInfo = productInfo as nsCore.Areas.Products.Models.Promotions.IFlatDiscountPromotionProductModel;
                                        retailDiscount = flatDiscountInfo.RetailDiscountPrice;
                                        cvDiscount = flatDiscountInfo.CVDiscountPrice;
                                        qvDiscount = flatDiscountInfo.QVDiscountPrice;
                                    }
                                    else if (productInfo is nsCore.Areas.Products.Models.Promotions.IPercentOffPromotionProductModel)
                                    {
                                        var percentOffDiscountInfo = productInfo as nsCore.Areas.Products.Models.Promotions.IPercentOffPromotionProductModel;
                                        retailDiscount = percentOffDiscountInfo.RetailDiscountPercent;
                                        cvDiscount = percentOffDiscountInfo.CVDiscountPercent;
                                        qvDiscount = percentOffDiscountInfo.QVDiscountPercent;
                                    }
                            %>
                            <tr <%= alt ? "class='Alt'" : "" %>>
                                <td>
                                    <input class="selectRow" type="checkbox" />
                                    <input type="hidden" class="productId" value="<%=productInfo.ProductID %>" />
                                </td>
                                <td>
                                    <%= productInfo.SKU %>
                                </td>
                                <td>
                                    <%= productInfo.Name %>
                                </td>
                                <td class="originalPrice">
                                    <%= productInfo.RetailPrice.Value.ToString("n2") %>
                                </td>
                                <td>
                                    <span class="helperSymbol">- </span>
                                    <input class="required retailPrice numeric" type="text" size="5" value="<%=retailDiscount.HasValue ? retailDiscount.Value.ToString("n2") : "" %>" />
                                </td>
                                <td>
                                    <%
                                    foreach (var priceInfo in productInfo.AdditionalPriceTypeValues)
                                    {
                                        decimal adjustedprice = 0;
                                        if (productInfo is nsCore.Areas.Products.Models.Promotions.IFlatDiscountPromotionProductModel)
                                        {
                                            adjustedprice = Convert.ToDecimal(priceInfo.Value) - Convert.ToDecimal(retailDiscount);
                                        }
                                        else if (productInfo is nsCore.Areas.Products.Models.Promotions.IPercentOffPromotionProductModel)
                                        {
                                            adjustedprice = Convert.ToDecimal(priceInfo.Value) * (1 - Convert.ToDecimal(retailDiscount / 100));
                                        }

                                        //decimal adjustedprice = Convert.ToDecimal(priceInfo.Value) * (1 - Convert.ToDecimal(retailDiscount / 100)); 
                                        if (adjustedprice > 0)
                                        {
                                    %>
                                    <%--<span class="discountPrice adjVal" originalVal="<%= priceInfo.Value %>"><%= priceInfo.Value%></span> - <%= priceInfo.PriceTypeName %> <br />--%>
                                    <span originalval="<%= priceInfo.Value %>" style="color: #d94125"><%= adjustedprice.GetRoundedNumber(2)%></span> - <%= priceInfo.PriceTypeName%>
                                    <br />
                                    <%}
                                    }%>
                                </td>
                                <td>
                                    <%= productInfo.CVPrice.Value.ToString("n2") %>
                                </td>
                                <td>
                                    <span class="helperSymbol">- </span>
                                    <input class="cvPrice numeric" type="text" size="5" value="<%=cvDiscount.HasValue ? cvDiscount.Value.ToString("n2") : "" %>" />
                                </td>
                                <td>
                                    <%= productInfo.QVPrice.Value.ToString("n2") %>
                                </td>
                                <td>
                                    <span class="helperSymbol">- </span>
                                    <input class="qvPrice numeric" type="text" size="5" value="<%=qvDiscount.HasValue ? qvDiscount.Value.ToString("n2") : "" %>" />
                                </td>
                            </tr>
                            <% } %>
                        </tbody>
                    </table>
                    <!--Pagination example still needs to be wired up-->
                    <div class="UI-mainBg Pagination" id="paginatedGridPagination">
                        <input type="hidden" id="paginatedGridRefresh" />
                        <div class="PaginationContainer">
                            <div class="Bar">
                                <a class="previousPage disabled" href="javascript:void(0);"><span>&lt;&lt; Previous</span></a>
                                <span class="pages">1 of 1</span><a class="nextPage disabled" href="javascript:void(0);"><span>
                                    Next &gt;&gt;</span></a> <span class="ClearAll clr"></span>
                            </div>
                            <div style="" class="PageSize">
                                Results Per Page:
                                <select class="pageSize">
                                    <option value="15">15</option>
                                    <option value="20">20</option>
                                    <option value="50">50</option>
                                    <option value="100">100</option>
                                </select>
                            </div>
                            <span class="ClearAll clr"></span>
                        </div>
                    </div>
                    <!--End pagination example-->
                </td>
            </tr>
        </tbody>
    </table>
</div>
<!-- Catalog modal -->
<div id="catalogSelector" class="jqmWindow LModal" style="position: fixed;">
    <div class="mContent">
        <h2>
            <%= Html.Term("SelectACatalog", "Select a Catalog") %></h2>
        <div style="overflow: scroll; max-height: 500px;" class="catalogList">
        </div>
        <br />
        <p class="FL">
            <a href="javascript:void(0);" class="jqmClose">
                <%= Html.Term("Close", "Close") %></a>
        </p>
        <p class="FR">
            <a id="btnCopySKUs" href="javascript:void(0);" class="Button BigBlue jqmClose">
                <%= Html.Term("ImportSKUs", "Import SKUs") %></a>
        </p>
        <span class="ClearAll"></span>
    </div>
</div>
<!-- Bulk Add modal -->
<div id="bulkAddModal" class="jqmWindow LModal ProductWin" style="margin-left: 0px;">
    <div class="mContent" style="width: 400px; height: 400px;">
        <h2>
            <%= Html.Term("BulkProductAdd", "Bulk Product Add")%></h2>
        <p>
            <%= Html.Term("FilterBy", "Filter by")%>:
            <select id="catalogFilter">
            </select>
        </p>
        <table cellspacing="0" cellpadding="0" width="100%;" class="DataGrid">
            <tr class="GridColHead">
                <th style="width: 20px;">
                    <input type="checkbox" id="chkBulkAdd" />
                </th>
                <th style="width: 80px;">
                    <%= Html.Term("SKU", "SKU") %>
                </th>
                <th style="width: 150px;">
                    <%= Html.Term("Name", "Name") %>
                </th>
                <th style="width: 65px;">
                    <%= Html.Term("RetailPrice", "Retail Price") %>
                </th>
                <th style="">
                    <%= Html.Term("Active", "Active") %>
                </th>
            </tr>
        </table>
        <div style="height: 250px; overflow: auto; border-bottom: 1px solid #efefef;">
            <table id="bulkProductCatalog" cellspacing="0" cellpadding="0" width="100%" class="DataGrid">
            </table>
        </div>
        <br />
        <p>
            <a id="btnBulkAdd" href="javascript:void(0)" class="Button BigBlue FR">
                <%= Html.Term("AddToPromotion", "Add to Promotion")%></a> <a href="javascript:void(0)"
                    class="jqmClose FL">
                    <%= Html.Term("Close", "Close") %></a>
        </p>
    </div>
</div>
