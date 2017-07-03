<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Base.PriceAdjustmentPromotionModel>" %>
<script type="text/javascript">
	$(function () {
		$('#conditionSingleGrid .BtnDelete').live('click', function () {
			$(this).closest('tr').remove();
			$('#singleItemQuickAdd').show();
			$('#conditionSingleGrid').hide();
		});

		$('#conditionComboDelete').click(function () {
			$('#conditionComboGrid .checkRow:checked').closest('tr').remove();
			$('#conditionComboGrid').refreshTable();
		});

		$('#comboSelectAll').click(function () {
			$('#conditionComboGrid .checkRow').attr('checked', $(this).prop('checked'));
		});

		if ($('#crSKU table tbody tr').length) {
			$('#singleItemQuickAdd').hide();
			$('#conditionSingleGrid').show();
		}

		$('#conditionComboProductAdd').click(function () {
			var productId = $('#conditionComboProductID').val();
			if (productId) {
				if ($('#conditionComboGrid td input:hidden[value="' + productId + '"]').length != 0) {
					showMessage('<%= Html.Term("ProductAlreadyAddedToPromotion", "Product already added to promotion") %>', true)
					return;
				}
				getProductInfo(productId, $(this), function (result) {
					$('#conditionComboGrid tbody').prepend('<tr>'
						+ '<td><input type="checkbox" class="checkRow" /><input type="hidden" class="productId" value="' + productId + '" /></td>'
						+ '<td>' + result.product.SKU + '</td>'
						+ '<td>' + result.product.Name + '</td>'
						+ '</tr>');
					$('#conditionComboGrid').refreshTable();
					$('#conditionComboProductID, #txtConditionCombo').val('');
				});
			}
		});

		$('#conditionSingleProductAdd').click(function () {
			var productId = $('#conditionSingleProductID').val();
			if (productId) {
				getProductInfo(productId, $(this), function (result) {
					$('#conditionSingleGrid tbody').prepend('<tr>'
						+ '<td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a><input type="hidden" id="productId" class="productId" value="' + productId + '" /></td>'
						+ '<td>' + result.product.SKU + '</td>'
						+ '<td>' + result.product.Name + '</td>'
						+ '<td><input type="text" id="qty" class="qty" value="' + $('#conditionSingleProductQty').val() + '" /></td>'
						+ '</tr>');
					$('#txtConditionSingle, #conditionSingleProductID').val('');
					$('#conditionSingleProductQty').val('1');
					$('.qty').numeric({ negative: false });
					$('#singleItemQuickAdd').hide();
					$('#conditionSingleGrid').show();
				});
			}
		});

		$().ready(function() {

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





		<%
			if (Model.CartCondition is nsCore.Areas.Products.Models.Promotions.ISingleProductCartCondition)
			{
				%>
				    $("#CriteriaTabber #tabSKU").trigger('click');
				    $('#singleItemQuickAdd').hide();
				    $('#conditionSingleGrid').show();
				    $('#txtConditionSingle, #conditionSingleProductID').val('');
				    $('#conditionSingleProductQty').val('1');
				<%
			}
			
            if (Model.CartCondition is nsCore.Areas.Products.Models.Promotions.ICombinationOfProductsCartCondition)
			{
                if ((TempData["AndConditionQvTotalFlatDiscount"] != null && Convert.ToBoolean(TempData["AndConditionQvTotalFlatDiscount"])) 
                || (TempData["AndConditionQvTotalPercentOff"] != null && Convert.ToBoolean(TempData["AndConditionQvTotalPercentOff"])))
                {
				%>
                    $("#CriteriaTabber #tabQVSubtotal").trigger('click');
				<%
                }
                else
                {
                %>
                    $("#CriteriaTabber #tabCombo").trigger('click');
                <%
                }
			}
			
			if (Model.CartCondition is nsCore.Areas.Products.Models.Promotions.ICustomerSubtotalRangeCartCondition)
			{
				%>
				$("#CriteriaTabber #tabSubtotal").trigger('click');
				<%
			}

            if (Model.CartCondition is nsCore.Areas.Products.Models.Promotions.ICustomerQVRangeCartCondition)
			{
				%>
				$("#CriteriaTabber #tabQVSubtotal").trigger('click');
				<%
			}
		 %>
		 });
	});
</script>
<h3 class="UI-lightBg pad10">
    <%= Html.Term("Promotions_ConditionsCartMustMeetHeading2", "Conditions the Promotion Must Meet:")%></h3>
<div id="CartConditions">
    <input type="hidden" value="singleSKU" id="criteria" name="criteria" />
    <div id="CriteriaTabber" class="Tabber">
        <ul class="inlineNav">
            <li rel="crSKU" id="tabSKU" class="current"><a class="tabLabel pad5" href="#">
                <label for="singleSKU">
                    <%= Html.Term("Promotions_ConditionsSpecificItemOption", "Has a single specific item")%></label></a></li>
            <li rel="crCombo" id="tabCombo"><a class="tabLabel pad5" href="#">
                <label for="manySKU">
                    <%= Html.Term("Promotions_ConditionsCombinationOfItemsOption", "Has a combination of items")%></label></a></li>
            <li rel="crSub" id="tabSubtotal"><a class="tabLabel pad5" href="#">
                <label for="subTotal">
                    <%= Html.Term("Promotions_ConditionsDefinedSubTotalOption", "Has a defined subtotal")%></label></a></li>
            <li rel="crQVSub" id="tabQVSubtotal"><a class="tabLabel pad5" href="#">
                <label for="qvSubTotal">
                    <%= Html.Term("Promotions_ConditionsDefinedQVSubTotalOption", "Has a defined QV total")%></label></a></li>
        </ul>
    </div>
    <!-- tab contents -->
    <div id="CriteriaContents">
        <!-- particular item -->
        <div id="crSKU" class="TabContent">
            <!--If one Sku may be added only for this option, then the Quick Add will only appear when user is actually able to add item
			(Markup is same as in _DefineRewards, but I have them set to show the two scenarios)-->
            <div id="singleItemQuickAdd">
                <%= Html.Term("Promotions_ProductLookUpLabel", "Product look-up")%>:<br />
                <input type="text" value="" size="30" class="pad5 mr10 txtQuickAdd" hiddenid='conditionSingleProductID'
                    id="txtConditionSingle" />
                <input type="text" value="1" class="pad5 qty center" id="conditionSingleProductQty" />
                <input type="hidden" value="" id="conditionSingleProductID" />
                <a class="DTL Add" href="javascript:void(0);" id="conditionSingleProductAdd">
                    <%= Html.Term("Promotions_QuickAdd", "Add")%></a>
            </div>
            <!--If one Sku may be added only for this option, then the table will not exist until item has been added.-->
            <table width="100%" style="display: none;" class="DataGrid" id="conditionSingleGrid">
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
                            <%=Html.Term("Quantity")%>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <%if (Model.CartCondition is nsCore.Areas.Products.Models.Promotions.ISingleProductCartCondition)
                      {
                          int productID = ((nsCore.Areas.Products.Models.Promotions.ISingleProductCartCondition)Model.CartCondition).ProductID;
                          int quantity = ((nsCore.Areas.Products.Models.Promotions.ISingleProductCartCondition)Model.CartCondition).Quantity;
                          var productInfo = new nsCore.Areas.Products.Models.Promotions.PromotionProductModel().LoadResources(productID);
                    %>
                    <tr>
                        <td>
                            <a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span>
                            </a>
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
                    %>
                </tbody>
            </table>
        </div>
        <!-- combo of items -->
        <div id="crCombo" class="TabContent" style="display: none;">
            <table width="100%" class="FormTable">
                <tbody>
                    <tr>
                        <td>
                            <div class="UI-secBg pad10 brdrYYNN">
                                <div class="FL mr10">
                                    <table>
                                        <tr>
                                            <td>
                                                <%= Html.Term("Promotions_QuickProductAdd", "Quick Product Add")%>:<br />
                                                <input type="text" value="" size="30" class="pad5 txtQuickAdd txtQuickAdd" hiddenid="conditionComboProductID"
                                                    id="txtConditionCombo" />
                                                <input type="hidden" value="" id="conditionComboProductID" />
                                                <a class="DTL Add" href="javascript:void(0);" id="conditionComboProductAdd">
                                                    <%= Html.Term("Promotions_QuickAdd", "Add")%></a>
                                            </td>
                                            <td style="vertical-align: bottom; padding-bottom: 10px">
                                                |
                                            </td>
                                            <td style="vertical-align: bottom; padding-bottom: 6px">
                                                <input type="checkbox" id="chkOrCondition" />
                                                <label for="chkOrCondition">
                                                    <%= Html.Term("OrCondition", "'Or' condition")%></label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <span class="clr"></span>
                            </div>
                            <div class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility" id="paginatedGridOptionsreward">
                                <a class="deleteButton UI-icon-container" href="javascript:void(0);" id="conditionComboDelete">
                                    <span class="UI-icon icon-deleteSelected"></span><span>
                                        <%=Html.Term("DeleteSelected", "Delete Selected") %></span></a>
                            </div>
                            <table width="100%" class="DataGrid" id="conditionComboGrid">
                                <thead>
                                    <tr class="GridColHead">
                                        <th class="GridCheckBox">
                                            <input type="checkbox" id="comboSelectAll" />
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
                                    <% if (Model.CartCondition is nsCore.Areas.Products.Models.Promotions.ICombinationOfProductsCartCondition)
                                       {
                                           var productIDs = (Model.CartCondition as nsCore.Areas.Products.Models.Promotions.ICombinationOfProductsCartCondition).RequiredProductIDs;
                                           foreach (var productID in productIDs)
                                           {
                                               var productInfo = new nsCore.Areas.Products.Models.Promotions.PromotionProductModel().LoadResources(productID); %>
                                    <tr>
                                        <td>
                                            <input type="checkbox" class="checkRow" /><input type="hidden" class="productId"
                                                value="<%= productInfo.ProductID %>" />
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
        <% 
            Html.RenderPartial("_CartConditionPriceAdjustmentSubtotals", Model); 
        %>
        <!-- qv cart subtotal -->
        <% Html.RenderPartial("_CartConditionQVSubtotals", Model); %>
    </div>
</div>
