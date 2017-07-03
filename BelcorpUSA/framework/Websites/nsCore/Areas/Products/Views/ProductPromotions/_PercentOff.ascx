<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.PercentOffPromotionModel>" %>

<% Html.RenderPartial("_EditScriptPriceAdjustmentPromotion"); %>
<div id="PromotionForm" class="splitCol mt10 mb10">
	<h3 class="UI-lightBg pad10">
		<%= Html.Term("Promotion_OptionsHeading", "Options:")%>
		<a style="" id="reviewOptions" class="FR viewHideOptions" href="#">
			<%= Html.Term("PromotionOptions_MinimizeOptions", "Minimize Options")%></a>
		<span class="clr"></span>
	</h3>
	<div id="PromoOptions">
		<% Html.RenderPartial("PromotionsPlugins/_OptionCouponCode"); %>
		<% Html.RenderPartial("PromotionsPlugins/_OptionOneTime"); %>
        <% Html.RenderPartial("PromotionsPlugins/_OptionContinuity"); %>
		<% Html.RenderPartial("PromotionsPlugins/_OptionActiveImmediately"); %>
		<% Html.RenderPartial("PromotionsPlugins/_OptionRestrictAccounts"); %>
        <% Html.RenderPartial("PromotionsPlugins/_OptionRestrictNewBAs"); %>
        <% Html.RenderPartial("PromotionsPlugins/_OptionRestrictActivityStatuses"); %>
		<% Html.RenderPartial("PromotionsPlugins/_OptionRestrictOrderTypes"); %>
		<% Html.RenderPartial("PromotionsPlugins/_OptionRestrictMarkets"); %>
        <% Html.RenderPartial("PromotionsPlugins/_AccountIDLoad"); %>
      <div class="UI-lightBg pad2 bold overflow">
			<a style="" id="reviewOptionsBottom" class="FR mr10 viewHideOptions" href="#">
				<%= Html.Term("PromotionOptions_MinimizeOptions", "Minimize Options")%></a>
		</div>
    </div>
</div>

 <% Html.RenderPartial("PromotionsPlugins/_PriceAdjustmentPanel"); %>
	
<% Html.RenderPartial("PromotionsPlugins/_PercentOffPromotionItems", Model.PromotionProducts); %>

<div class="mt10" id="SaveRewards">
	<a class="Button BigBlue" id="btnSave" href="javascript:void(0);">
		<%= Html.Term("Promotions_SavePromotion", "Save Promotion")%></a>
</div>
