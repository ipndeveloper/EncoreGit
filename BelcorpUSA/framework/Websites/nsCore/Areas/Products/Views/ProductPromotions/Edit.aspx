<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Promotions/Promotions.Master"
	Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Products.Models.Promotions.Base.PriceAdjustmentPromotionModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript">
		$(function () {
			<% if(Model.PromotionID > 0 && ViewBag.AdjustmentTypeID != null) { %>
			$('#sType').val('<%= ViewBag.AdjustmentTypeID %>');
			<% } %>
			$('#sType').change(function () {
				$('#formPartial').html('');
				var options = {
					url: '<%= ResolveUrl("~/Products/ProductPromotions/GetPartialForAdjustmentType") %>',
					showLoading: $('#formPartial'),
					data: {
						adjustmentType: $('option:selected', this).val(),
						promotionId: $('#promotionId').val() 
					},
					success: function (data) {
						$('#formPartial').html(data);
						wireupOptionsPanel();
					}
				};
				NS.post(options);
			}).trigger('change');
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Promotions") %>">
			<%= Html.Term("Promotions") %></a> > <a href="<%= ResolveUrl("~/Products/Promotions") %>">
				<%= Html.Term("ProductPromotion", "Product Promotion")%></a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Promotions_CreateProductPromotionHeader", "Create a Product Promotion") %></h2>
	</div>
	<div id="NameType">
		<table width="100%" class="FormTable">
			<tbody>
				<tr>
					<td class="FLabel">
						<%= Html.Term("Promotions_PromotionNameLabel", "Promotion Name") %>:
					</td>
					<td>
						<input type="hidden" value="<%= Model.PromotionID %>" id="promotionId" />
						<input type="text" value="<%= Model.Name %>" name="Name is required" class="required pad5 fullWidth"
							id="txtName" />
					</td>
				</tr>
				<tr>
					<td class="FLabel">
						<%= Html.Term("Promotions_AdjustmentTypeLabel", "Adjustment Type") %>:
					</td>
					<td>
						<div class="FL adjustmentSelector">
                            <select class="pad2" id="sType"  <%= Model.PromotionID > 0 ? "disabled=\"disabled\"" : "" %>>
								<option rel="adj1" value="<%= (int)nsCore.Areas.Products.Controllers.AdjustmentType.PercentOff %>">
									<%= Html.Term("Promotions_PercentageOffOption", "Percentage Off")%></option>
								<option rel="adj2" value="<%= (int)nsCore.Areas.Products.Controllers.AdjustmentType.FlatDiscount %>">
									<%= Html.Term("Promotions_FixedPriceDiscountOption", "Fixed Price Discount")%></option>
							</select>
                        </div>
						<a id="AdjustemntDescHelp" class="UI-icon-wrapper ml10"><span class="UI-icon icon-help">
						</span></a>
						<div class="FL ml10 adjustmentDesc">
							<div class="UI-mainBg desc" id="adj1" style="display: none;">
								<%= Html.Term("Promotions_DefinePercentOffPriceTip", "Define a % off the original price of inidividual item, or an entire group of items.") %><hr />
								<a class="hideDesc" href="javascript:void(0);">Hide</a>
							</div>
							<div class="UI-mainBg desc" id="adj2" style="display: none;">
								<%= Html.Term("Promotions_DiscountPriceExactAmountTip", "Discount the price of items by an exact amount.") %><hr />
								<a class="hideDesc" href="javascript:void(0);">Hide</a>
							</div>
							<div class="UI-mainBg desc" id="adjAll" style="display: none;">
								<span class="bold">
									<%= Html.Term("Promotions_PercentageOffOption", "Percentage Off") %>:</span>
								<%= Html.Term("Promotions_DefinePercentOffPriceTip") %><hr />
								<span class="bold">
									<%= Html.Term("Promotions_FixedPriceDiscountOption", "Fixed Price Discount") %>:</span>
								<%= Html.Term("Promotions_DiscountPriceExactAmountTip") %><hr />
								<a class="hideDesc" href="javascript:void(0);">Hide</a>
							</div>
						</div>
					</td>
				</tr>
			</tbody>
		</table>
	</div>
	<div id="formPartial">
	</div>
	<% Html.RenderPartial("MessageCenter"); %>
</asp:Content>
