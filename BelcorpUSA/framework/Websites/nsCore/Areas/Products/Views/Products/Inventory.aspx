<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript">
		$(function () {
			$('.warehouseEnabler').click(function () {
				var enabled = $(this).prop('checked');
				$(this).parent().find('input[type="text"]').each(function () {
					if (enabled) {
						$(this).removeAttr('disabled').css({ 'color': '#758D91' });
					} else {
						$(this).attr('disabled', 'disabled').css({ 'color': '#cfcfcf' });
					}
				});
				if (enabled) {
					$(this).parent().find('img').hide();
				} else {
					$(this).parent().find('img').css('display', 'inline-block');
				}
			});
			$('#btnSave').click(function () {
				var t = $(this);
				showLoading(t);
				var data = {};
				$('.warehouseProduct').filter(function () { return $(this).find('.warehouseEnabler').prop('checked') == true; }).each(function (i) {

					data['warehouseProducts[' + i + '].WarehouseProductID'] = $('.warehouseProductId', this).val();
					data['warehouseProducts[' + i + '].WarehouseID'] = $('.warehouseId', this).val();
					data['warehouseProducts[' + i + '].ProductID'] = $('.productId', this).val();
					data['warehouseProducts[' + i + '].IsAvailable'] = $('.IsAvailable', this).prop('checked');
					data['warehouseProducts[' + i + '].QuantityOnHand'] = $('.QuantityOnHand', this).val();
					data['warehouseProducts[' + i + '].QuantityBuffer'] = $('.QuantityBuffer', this).val();
					data['warehouseProducts[' + i + '].ReorderLevel'] = $('.ReorderLevel', this).val();
				});


				$.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveInventory/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data, function (response) {
					if (response.result) {
						showMessage('<%= Html.JavascriptTerm("InventorySaved", "Inventory saved!") %>', false);
					} else {
						showMessage(response.message, true);
					}
					hideLoading(t);

				});
			});
		});
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
			<%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>">
				<%= Model.Translations.Name() %></a> >
	<%= Html.Term("Inventory") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<%
	var ActiveWarehouses = SmallCollectionCache.Instance.Warehouses.Where(w => w.Active);
%>
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("ProductInventory", "Product Inventory") %></h2>
	</div>
	<table width="100%" class="DataGrid">
		<thead>
			<tr class="GridColHead">
				<th>
					<%= Html.Term("SKU") %>
				</th>
				<th>
					<%= Html.Term("ProductName", "Product Name") %>
				</th>
				<%
					foreach (var warehouse in ActiveWarehouses)
					{ 
				%>
				<th>
					<%= warehouse.Name %>
				</th>
				<%
					} 
				%>
			</tr>
			<tr class="">
				<th>
					<%=Model.IsVariantTemplate ? Model.SKU : "" %>
				</th>
				<th>
					<%=Model.IsVariantTemplate ? Model.Translations.Name() : "" %>
				</th>
				<%
					foreach (var warehouse in ActiveWarehouses)
					{ 
				%>
				<th>
					<div class="warehouse<%= warehouse.WarehouseID %>">
						<span style="margin-left: 25px;" class="SubGridHead">
							<%= Html.Term("QuantityOnHand", "Quantity on Hand")%></span> <span class="SubGridHead">
								<%= Html.Term("Buffer")%></span> <span class="SubGridHead">
									<%= Html.Term("ReorderLevel", "Reorder Level") %></span> <span class="SubGridHead">
										<%= Html.Term("Allocated") %></span>
					</div>
				</th>
				<%
					}
				%>
			</tr>
		</thead>
		<tbody>
			<% 
				if (Model.ProductBase.IsShippable)
				{
					int count = 0;
					var applicableProducts = Model.ProductBase.Products.Where(p => !p.IsVariantTemplate);
					foreach (Product product in applicableProducts)
					{
			%>
			<tr class="GridRow<%= count % 2 == 1 ? "" : " Alt" %>">
				<td>
					<%= product.SKU%>
				</td>
				<td>
					<%= product.Translations.Name()%>
				</td>
				<%
						foreach (Warehouse warehouse in ActiveWarehouses)
						{
							WarehouseProduct wp = product.WarehouseProducts.FirstOrDefault(p => p.WarehouseID == warehouse.WarehouseID);
							bool productExists = wp != default(WarehouseProduct) && wp.IsAvailable; 
				%>
				<td class="warehouseProduct">
					<div class="warehouse<%= warehouse.WarehouseID %>">
						<input type="hidden" class="changed" value="false" />
						<input type="hidden" class="warehouseProductId" value="<%= wp == null ? 0 : wp.WarehouseProductID %>" />
						<input type="hidden" class="warehouseId" value="<%= warehouse.WarehouseID %>" />
						<input type="hidden" class="productId" value="<%= product.ProductID %>" />
						<input type="checkbox" <%= productExists ? "checked=\"checked\"" : "" %> class="IsAvailable warehouseEnabler" />
						<input type="text" class="QuantityOnHand numeric" value="<%= wp != null ? wp.QuantityOnHand : 0 %>" <%= productExists ? "" : " style=\"color: #cfcfcf;\"" %><%= productExists ? "" : " disabled=\"disabled\"" %> />
						<input type="text" class="QuantityBuffer numeric" value="<%= wp != null ? wp.QuantityBuffer : 0 %>" <%= productExists ? "" : " style=\"color: #cfcfcf;\"" %><%= productExists ? "" : " disabled=\"disabled\"" %> />
						<input type="text" class="ReorderLevel numeric" value="<%= wp != null ? wp.ReorderLevel : 0 %>" <%= productExists ? "" : " style=\"color: #cfcfcf;\"" %><%= productExists ? "" : " disabled=\"disabled\"" %> />
						<span class="QuantityAllocated">
							<%= wp == null ? 0 : wp.QuantityAllocated %></span> <span title="Not shipping" style="display: <%= productExists ? "none" : "inline-block" %>;" class="icon-cancelled NoShip<%= product.ProductID %>"></span>
					</div>
				</td>
				<%
						}
				%>
			</tr>
			<%
						count++;
					}
				}
				else
				{ 
			%>
			<tr>
				<td colspan="3">
					<%= Html.Term("WarehouseInventoryManagementNotAvailableForNonShippableItems", "Warehouse Inventory Management Not Available For Non-Shippable Items")%>
				</td>
			</tr>
			<%
				} 
			%>
		</tbody>
	</table>
	<br />
	<p>
		<a id="btnSave" href="javascript:void(0);" class="Button BigBlue"><span>
			<%= Html.Term("SaveInventory", "Save Inventory") %></span></a>
	</p>
</asp:Content>
