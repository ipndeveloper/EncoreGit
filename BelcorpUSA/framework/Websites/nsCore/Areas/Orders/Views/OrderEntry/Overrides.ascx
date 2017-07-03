<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="orderOverrides">
	<script type="text/javascript">

	    $(function () {

	        $('#btnUpdateOverrides').click(function () {
	            var runningTotal = 0;
	            $('#overrideProducts tbody:first tr').each(function (index, row) {
	                var pricePerItem = $('#overridePrices' + row.id).val();
	                var quantity = $('#overridePrices' + row.id).closest('td').next().text();
	                runningTotal += (quantity * pricePerItem);
	                $('#cvAmount' + row.id).closest('td').next().text('$' + (quantity * pricePerItem).toFixed(2));
	            });
	            $('#overrideProducts .subtotal').html('$' + runningTotal.toFixed(2))
	        });

	        $('#btnSaveOverride').click(function () {
	            $('#overrideErrors').messageCenter('clearAllMessages');
	            var data = {};
	            $('#overrideProducts tbody:first tr').each(function (index, row) {
	                data['items[' + index + '].orderItemGuid'] = row.id;
	                data['items[' + index + '].pricePerItem'] = $('#overridePrices' + row.id).val();
	                data['items[' + index + '].commissionableValue'] = $('#cvAmount' + row.id).val();
	            });
	            data['taxAmount'] = $('#txtOverrideTax').val();
	            data['shippingAmount'] = $('#txtOverrideShipping').val();
	            $.post('<%= ResolveUrl("~/Orders/OrderEntry/PerformOverrides") %>', data, function (results) {
	                if (results.result) {
	                    // close the modal and update the html with the changes
	                    $('#overridesModal').jqmHide();
	                    // disable the page
	                    $('.OverrideDisable').attr('disabled', 'disabled');
	                    // refresh the totals
	                    updateCartAndTotals(results);
	                    // remove payments (the payments were already removed from the object, now we need to update the html)
	                    $('#payments .paymentItem').remove();
	                    if (!userIsAuthorized) {
	                        $('#authorization').show();
	                        $('#orderOverrides').remove();
	                    }
	                }
	                else {
	                    // leave the modal open and show a message explaining why the override did not happen
	                    $('#overrideErrors').messageCenter('addMessage', results.message);
	                }
	            }, 'json');
	        });
	        $('#btnCancelOverride').click(function () {
	            $('#btnPerformOverrides').removeClass('cancelOverrides').html('<span>Perform Overrides</span>');
	            $('#overridesModal').jqmHide();
	        });
	    });
	</script>
	<table style="height: 200px;">
		<tr>
			<td>
				<table width="100%" class="DataGrid" id="overrideProducts">
					<thead class="GridColHead">
						<tr>
							<th>
								<%= Html.Term("SKU", "SKU")%>
							</th>
							<th>
								<%= Html.Term("Product", "Product")%>
							</th>
							<th>
								<%= Html.Term("PricePerItem", "Price Per Item")%>
							</th>
							<th style="width: 75px;">
								<%= Html.Term("Quantity", "Quantity")%>
							</th>

                            <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%string valorSCV = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "SCV");
                                if (valorSCV == "S")
                                { %>
							<th>
								<%= Html.Term("CommissionableValue", "Commissionable Value")%>
							</th>
                             <%}%>
                            <%--CS.03MAY2016.Fin.Muestra CV--%>


							<th>
								<%= Html.Term("Price", "Price")%>
							</th>
						</tr>
					</thead>
					<tbody>
					</tbody>
					<tbody>
						<tr id="productTotalBar" class="GridTotalBar">
							<td>
							</td>
							<td>
							</td>
							<td>
							</td>
							<td>
							</td>
							<td>
								<a id="btnUpdateOverrides" href="javascript:void(0);" class="DTL Update OverrideDisable">
									<%= Html.Term("Update", "Update")%></a>
							</td>
							<td>
								<b><span class="subtotal">
									<%= CoreContext.CurrentOrder.Subtotal.ToDecimal().ToString("C") %></span> (Sub Total)</b>
							</td>
						</tr>
					</tbody>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<table width="100%">
					<tr>
						<td style="text-align: right;">
							<%= Html.Term("Tax", "Tax")%>:<br />
							<%= Html.Raw(Html.Term("S&H", "S&amp;H"))%>:<br />
						</td>
						<td>
							&nbsp;<%= (CoreContext.CurrentOrder.Currency ?? SmallCollectionCache.Instance.Currencies.First()).CurrencySymbol%><input
								type="text" id="txtOverrideTax" value="<%= CoreContext.CurrentOrder.TaxAmountTotal.ToDecimal().ToString("C") %>"
								class="TextInput" /><br />
							&nbsp;<%= (CoreContext.CurrentOrder.Currency ?? SmallCollectionCache.Instance.Currencies.First()).CurrencySymbol%><input
								type="text" id="txtOverrideShipping" value="<%= CoreContext.CurrentOrder.ShippingTotal.ToDecimal().ToString("C") %>"
								class="TextInput" />
						</td>
					</tr>
				</table>
			</td>
		</tr>
		<tr>
			<td>
				<p>
                    <%--<img id="overridesLoading" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." />--%>
                    <a href="javascript:void(0);" id="btnSaveOverride" class="Button BigBlue"><%= Html.Term("Save", "Save")%></a>
                    <a href="javascript:void(0);" id="btnCancelOverride" class="Button jqmClose"><%= Html.Term("Cancel", "Cancel")%></a>
                </p>
			</td>
		</tr>
	</table>
</div>
