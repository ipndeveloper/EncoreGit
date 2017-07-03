<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.ICartRewardsPromotionModel>" %>





<script type="text/javascript">
	$(function () {
    var url1 = $("#HF_URL_1").val();
        var data1 = null;
        var response1 = null;

        var promise1 = $.get(url1, data1);

        promise1.done(function () {
            response1 = JSON.parse(promise1.responseText);
        });
        promise1.fail(function (XMLHttpRequest, textStatus, errorThrown) {
            alert('<%=Html.Term("AnErrorHasOccurredToLoadTheProductPriceTypes", "An error has occurred to load the product price types") %>');
        });

        //Reward
        <% int defaultRewardTypePrice = ViewData["rewardProductPriceTypeId"]!=null? (int)ViewData["rewardProductPriceTypeId"]:0; %>
        $.when(promise1).done(function (promise1) {
            var ddlProductPriceTypes = document.getElementById("ddl2_ProductPriceTypes");

            $.each(response1, function (i, item) {
                ddlProductPriceTypes.options[i] = new Option(item.TermName, item.ProductPriceTypeID);
                
                if(item.ProductPriceTypeID == <%= defaultRewardTypePrice %>){
                    ddlProductPriceTypes.selectedIndex = i;
                }

                i += 1;
            });
        });

		$('#rewardsAutoAddGrid .BtnDelete').live('click', function () {
			$(this).closest('tr').remove();
			$('#rewardsAutoAddGrid').refreshTable();
			setAutoAddGridVisibility();
		});

		$('#rewardsPickFromListGrid').refreshTable();
		setAutoAddGridVisibility();

		$('#rewardsCheckAll').click(function () { $('#rewardsPickFromListGrid .selectRow').attr('checked', $(this).prop('checked')); });

		$('#paginatedGridPickFromList .deleteButton').click(function () {
			$('#rewardsPickFromListGrid .selectRow:checked').each(function () {
				$(this).closest('tr').remove();
			});
			$('#rewardsPickFromListGrid').refreshTable();
		});

		$('#rewardsAutoAddProductAdd').click(function () {
			var productId = $('#rewardsAutoAddProductId').val();
			if (productId) {
				if (isProductInGrid($('#rewardsPickFromListGrid'), productId)) {
					showMessage('<%= Html.Term("ProductAlreadyAddedToPromotion", "Product already added to promotion") %>', true)
					return;
				}
				getProductInfo(productId, $(this), function (result) {
					$('#rewardsAutoAddGrid').prepend('<tr>'
						+ '<td><a class="BtnDelete" href="javascript:void(0);"><span class="icon-x"></span></a><input type="hidden" id="productId" class="productId" value="' + productId + '" /></td>'
						+ '<td>' + result.product.SKU + '</td>'
						+ '<td>' + result.product.Name + '</td>'
						+ '<td><input type="text" id="qty" class="qty" value="' + $('#rewardsAutoAddProductQty').val() + '" /></td>'
						+ '</tr>').refreshTable();
					$('#txtRewardsAutoAddProduct, #rewardsAutoAddProductId').val('');
					$('#rewardsAutoAddProductQty').val('1');
					setAutoAddGridVisibility();
				});
			}
		});

		$('#rewardsPickFromListProductAdd').click(function () {
			var productId = $('#rewardsPickFromListProductId').val();
			if (productId) {
				if (isProductInGrid($('#rewardsPickFromListGrid'), productId)) {
					showMessage('<%= Html.Term("ProductAlreadyAddedToPromotion", "Product already added to promotion") %>', true)
					return;
				}
				getProductInfo(productId, $(this), function (result) {
					$('#rewardsPickFromListGrid').prepend('<tr>'
						+ '<td><input type="checkbox" class="selectRow" /><input type="hidden" class="productId" value=' + productId + ' /></td>'
						+ '<td>' + result.product.SKU + '</td>'
						+ '<td>' + result.product.Name + '</td>'
						+ '</tr>').refreshTable();
					$('#txtRewardsPickFromList, #rewardsPickFromListProductId').val('');
					$('.qty').numeric();
				});
			}
		});

		$('div.cartDiscountReward').click(function () {
			if ($('div.optionContent', this).find('input').is(':focus') === false) {
				var checkbox = $('input', this),
					selected = $(this).hasClass('UI-lightBg'),
					isCartDiscount = $(this).hasClass('cartDiscountReward');

				if (selected) {
					checkbox.removeAttr('checked');
					$(this).removeClass('UI-lightBg');
					if(isCartDiscount) {
						$('div.optionContent', this).find('input').attr({ 'disabled': true });
					}
				}
				else {
					checkbox.attr({ 'checked': 'checked' });
					$(this).addClass('UI-lightBg');
					if(isCartDiscount) {
						$('div.optionContent', this).find('input').removeAttr('disabled').focus();
					}
				}
			}
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
            $('#rewardsPickFromListGrid').append(rows).refreshTable();
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
                            alert(rows);
                            $('#rewardsPickFromListGrid').append(rows).refreshTable();
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
                var adjustedPrice_culture = new Intl.NumberFormat(cultura, { maximumFractionDigits: 2 , minimumFractionDigits: 2 }).format(adjustedPrice);
                discountLbl.text(adjustedPrice_culture);
//                discountLbl.text(adjustedPrice.toFixed(2));
            });
        }).change();

        function alertIfAlreadyInGrid(productId) {
            var ret = $('#rewardsPickFromListGrid td input:hidden[value="' + productId + '"]').length != 0;
            if (ret) {
                showMessage('<%= Html.Term("ProductAlreadyAddedToPromotion", "Product already added to promotion") %>', true);
            }
            return ret;
        }

        function buildBulkAddModalProductRow(productInfo) {
                       
                        var cultura = '<%= CoreContext.CurrentCultureInfo.Name%>';
                        var RetailPrice = new Intl.NumberFormat(cultura ,{ maximumFractionDigits: 2 , minimumFractionDigits: 2}).format(productInfo.RetailPrice);
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

        function buildProductRow(productInfo) {

         
            var append = '';
            if ($('#percentForm').is(':visible')) {
                append = '%';
            }
            var html = '<tr>'
				+ '<td><input type="checkbox" class="selectRow" /><input type="hidden" class="productId" value="' + productInfo.ProductID + '" /></td>'
				+ '<td>' + productInfo.SKU + '</td>'
				+ '<td>' + productInfo.Name + '</td>'
				+ '</tr>';
            return html;
        }

		function setAutoAddGridVisibility() {
			if ($('#rewardsAutoAddGrid tbody tr').length) {
				$('#rewardsAutoAddGrid').show();
			}
			else {
				$('#rewardsAutoAddGrid').hide();
			}
		}

		function isProductInGrid(table, productId) {
			return $(table).find('td input:hidden[value="' + productId + '"]').length != 0;
		}

		$().ready(function() {
		<%
			if (Model.CartReward is nsCore.Areas.Products.Models.Promotions.IAddProductsToCartReward)
			{
				%>
				$("#RewardsTabber #tabSingle").trigger('click');
				<%
			}
			if (Model.CartReward is nsCore.Areas.Products.Models.Promotions.IPickFromListOfProductsCartRewardModel)
			{
				%>
				$("#RewardsTabber #tabSelect").trigger('click');
				<%
                var reward= (nsCore.Areas.Products.Models.Promotions.IPickFromListOfProductsCartRewardModel)(Model.CartReward);
                if(reward.IsEspecialPromotion!=null && reward.IsEspecialPromotion==true){
                    %>
                    $('#chkAmount').prop('checked')=true;
                   <%
                }
			}
			if (Model.CartReward is nsCore.Areas.Products.Models.Promotions.ICartDiscountCartReward)
			{
				%>
				$("#RewardsTabber #tabDiscount").trigger('click');
				<%
			}

        
		 %>
		 });

         
	});
    $(document).ready(function(){
     <%
         if (Model.CartReward is nsCore.Areas.Products.Models.Promotions.IPickFromListOfProductsCartRewardModel)
         {
             nsCore.Areas.Products.Models.Promotions.IPickFromListOfProductsCartRewardModel cartreward = (nsCore.Areas.Products.Models.Promotions.IPickFromListOfProductsCartRewardModel)Model.CartReward;
             if (cartreward.IsEspecialPromotion.ToBool())
             {
      %>
      $("#chkAmount").prop('checked', true);
      <%}
         }%>
    });
</script>
<h3 class="UI-lightBg mt10 pad10">
	<%= Html.Term("Promotions_DefineRewardsHeading", "Define the Rewards") %>:</h3>
     
<div id="DefineRewards">
	<div id="RewardsTabber" class="Tabber">
		<ul class="inlineNav">
			<li rel="rwSingle" class="current" id="tabSingle"><a class="tabLabel pad5" href="#">
				<label for="rwSingle">
					<%= Html.Term("Promotions_DefineRewardsFreeProductOption", "Automatically put product(s) in shopper's cart") %></label></a></li>
			<li rel="rwList"><a class="tabLabel pad5" href="#">
				<label for="rwList" id="tabSelect">
					<%= Html.Term("Promotions_DefineRewardsSelectItemOption", "Shopper can pick from a list of free products") %></label></a></li>
			<li rel="rwDiscount" id="tabDiscount"><a class="tabLabel pad5" href="#">
				<label for="rwDiscount">
					<%= Html.Term("Promotions_DefineRewardsDiscountAndFreeShippingOption", "Include a discount and/or free shipping")%></label></a></li>
		</ul>
	</div>
	<div id="RewardsContents">
		<!-- particular item -->
		<div id="rwSingle" class="TabContent">
			<table width="100%" class="FormTable">
				<tbody>
					<tr>
						<td>
							<!--If one Sku may be added only for this option, then the Quick Add will only appear when user is actually able to add item
							(Markup is same as in _CartConditions, but I have them set to show the two scenarios)-->
							<div style="display: block;">
								<%= Html.Term("Promotions_ProductLookUpLabel", "Product look-up")%>:<br />
								<input type="text" value="" size="30" class="pad5 mr10 txtQuickAdd" id="txtRewardsAutoAddProduct"
									hiddenid="rewardsAutoAddProductId" />
								<input type="text" value="1" class="pad5 qty center" id="rewardsAutoAddProductQty" />
								<input type="hidden" value="" id="rewardsAutoAddProductId" />
								<a class="DTL Add" href="javascript:void(0);" id="rewardsAutoAddProductAdd">
									<%= Html.Term("Promotions_QuickAdd", "Add")%></a>
							</div>
							<table width="100%" style="display: none;" class="DataGrid" id="rewardsAutoAddGrid">
								<thead>
									<tr class="GridColHead">
										<th class="GridCheckBox">
										</th>
										<th>
											<%=Html.Term("SKU")%>
										</th>
										<th>
											<%=Html.Term("Product")%>
										</th>
										<th>
											<%=Html.Term("Quantity") %>
										</th>
									</tr>
								</thead>
								<tbody>
									<%if (Model.CartReward is nsCore.Areas.Products.Models.Promotions.IAddProductsToCartReward)
									  {
										  var reward = (nsCore.Areas.Products.Models.Promotions.IAddProductsToCartReward)Model.CartReward;
										  foreach (var item in reward.ProductIDQuantities)
										  {
											  int productID = item.Key;
											  int quantity = item.Value;
											  var productInfo = new nsCore.Areas.Products.Models.Promotions.PromotionProductModel().LoadResources(productID);
											%>
											<tr>
												<td>
													<a class="BtnDelete" href="javascript:void(0);"><span class="icon-x"></span></a>
													<input type="hidden" id="productId" class="productID" value="<%= productID %>" />
												</td>
												<td>
													<%=productInfo.SKU %>
												</td>
												<td>
													<%=productInfo.Name%>
												</td>
												<td>
													<%= quantity %>
													<input type="hidden" id="qty" class="qty" value="<%= quantity %>" />
												</td>
											</tr>
											<%
										}
									}
									%>
								</tbody>
							</table>
						</td>
					</tr>
				</tbody>
			</table>
		</div>
		<!-- combo of items -->
		<div style="display: none;" id="rwList" class="TabContent">
			<table width="100%" class="FormTable">
				<tbody>
					<tr id="productPanel">
						<td style="vertical-align: top;" class="FLabel">
					    <div class="mr10">
						<ul class="InputTools flatList listNav">
							<li><a class="BulkAdd" href="javascript:void(0);" id="btnOpenProductBulkAdd">
								<%= Html.Term("Promotions_OpenProductBulkAdd", "Open Product Bulk Add") %></a></li>
							</ul>
					        </div>
				       </td>
                        <td>
							<div class="UI-secBg pad10 brdrYYNN">
								<div class="FL mr10">
									<%= Html.Term("Promotions_QuickProductAdd", "Quick Product Add")%>:<br />
									<input type="text" value="" size="30" class="pad5 txtQuickAdd" id="txtRewardsPickFromList"
										hiddenid="rewardsPickFromListProductId" />
									<input type="hidden" value="" id="rewardsPickFromListProductId" />
									<a class="DTL Add" href="javascript:void(0);" id="rewardsPickFromListProductAdd">
										<%= Html.Term("Promotions_QuickAdd", "Add")%></a>
								</div>
								<div class="FL ml10">
								<%= Html.Term("Promotions_MaxQuantity", "Max Quantity") %>: <br />
								<% var pickFromListReward = Model.CartReward as nsCore.Areas.Products.Models.Promotions.IPickFromListOfProductsCartRewardModel; %>
                              		<input type="text" class="pad5 qty numeric" id="maxQuantity" value='<%=pickFromListReward == null ? 1 : pickFromListReward.MaxQuantity %>' />
						        </div>
                                <div class="FL ml10">
                                 <%= Html.Term("Promotions_AmountCondition", "Amount Condition") %>: <br />
								 <input type="checkbox"  name="chkAmount" id="chkAmount" />
                                </div>
								<span class="clr"></span>
							</div>
							<div class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility" id="paginatedGridPickFromList">
								<a class="deleteButton UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-deleteSelected">
								</span><span>
									<%=Html.Term("DeleteSelected", "Delete Selected") %></span></a>
							</div>
							<table width="100%" class="DataGrid" id="rewardsPickFromListGrid">
								<thead>
									<tr class="GridColHead">
										<th class="GridCheckBox">
											<input type="checkbox" id="rewardsCheckAll" />
										</th>
										<th>
											<%=Html.Term("SKU")%>
										</th>
										<th>
											<%=Html.Term("Product")%>
										</th>
									</tr>
								</thead>
								<tbody>
									<% if (Model.CartReward is nsCore.Areas.Products.Models.Promotions.IPickFromListOfProductsCartRewardModel)
									{
										var reward = (nsCore.Areas.Products.Models.Promotions.IPickFromListOfProductsCartRewardModel)Model.CartReward;
										var productIDs = (Model.CartReward as nsCore.Areas.Products.Models.Promotions.IPickFromListOfProductsCartRewardModel).ProductIDs;
										foreach (var productID in productIDs)
										{
											var productInfo = new nsCore.Areas.Products.Models.Promotions.PromotionProductModel().LoadResources(productID); %>
											<tr>
												<td>
													<input type="checkbox" class="selectRow" /><input type="hidden" class="productId" value="<%= productInfo.ProductID %>" />
												</td>
												<td>
													<%= productInfo.SKU%>
												</td>
												<td>
													<%= productInfo.Name%>
												</td>
											</tr>
										<% }
									}%>
								</tbody>
							</table>
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
						</td>
					</tr>
				</tbody>
			</table>
		</div>
		<!-- cart has => subtotal -->
		<div style="display: none;" id="rwDiscount" class="TabContent">
            <%
                bool includeDiscount = false;
                decimal discount = 0M;
                bool freeShipping = false;
              if (Model.CartReward is nsCore.Areas.Products.Models.Promotions.ICartDiscountCartReward)
			  {
                  var reward = (nsCore.Areas.Products.Models.Promotions.ICartDiscountCartReward)Model.CartReward;
                  includeDiscount = reward.DiscountPercent.HasValue && reward.DiscountPercent > 0;
                  if (includeDiscount)
                    discount = reward.DiscountPercent.Value;
                  freeShipping = reward.FreeShipping;
              }
            %>
			<div class="FL pad10 mr10 brdr1 brdrAll cartDiscountReward entireCart">
				<input type="checkbox" name="ShippingRewardOption" <%= includeDiscount?"checked=\"checked\"":"" %>" />
				<label class="mr10 bold">
					<%= Html.Term("Promotions_DefineRewardsDiscountEntireCartLabel", "Include a Discount on Entire Cart")%></label>
				<div class="mt10 ml10 center optionContent">
					<%--<input type="text" class="pad5 qty" id="entireCartDiscount" <%= !includeDiscount?"disabled=\"disabled\"":"" %>" value="<%= discount.ToString("0.00").Replace(",", ".") %>" />%--%>
                    <input type="text" class="pad5 qty" id="entireCartDiscount" <%= !includeDiscount?"disabled=\"disabled\"":"" %>" value="<%= discount.ToString("N", CoreContext.CurrentCultureInfo) %>" monedaidioma='CultureIPN'/>%
					<span class="lawyer block">
						<%= Html.Term("Promotions_DefineRewardsDoesNotIncludeText", "does not consider taxes &amp; shipping")%></span>
				</div>
			</div>
			<div class="FL pad10 brdr1 brdrAll cartDiscountReward cartShipping">
				<input type="checkbox" name="ShippingRewardOption" <%= freeShipping?"checked=\"checked\"":"" %> />
				<label class="mr10 bold">
					<%= Html.Term("Promotions_CartRewardsFreeShippingLabel", "Include Free Shipping")%></label>
				<div class="mt10 ml10 center optionContent">
					<span class="lawyer block"><%= Html.Term("Promotions_DefineRewardsAppliesToDefaultShipping", "applies to default shipping method")%></span>
				</div>
			</div>
            
			<span class="ClearAll clr"></span>
		</div>        
	</div>
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
