<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderRules.Service.DTO.RulesDTO>" %>

<% Html.RenderPartial("_EditScriptOrderRulesValidation"); %>
<div id="PromotionForm" class="splitCol mt10 mb10">
	<h3 class="UI-lightBg pad10">
		<%= Html.Term("Promotion_OptionsHeading", "Options:")%>
		<a style="" id="reviewOptions" class="FR viewHideOptions" href="#">
			<%= Html.Term("PromotionOptions_MinimizeOptions", "Minimize Options")%></a>
		<span class="clr"></span>
	</h3>
	<div id="PromoOptions">
		<% Html.RenderPartial("OrderRulesPlugins/_OptionMinimumAmount", Model.RuleValidationsDTO.FirstOrDefault()); %>
		<% Html.RenderPartial("OrderRulesPlugins/_OptionMinimumVolume", Model.RuleValidationsDTO.FirstOrDefault()); %>
		<% Html.RenderPartial("OrderRulesPlugins/_OptionActiveImmediately"); %>
		<% Html.RenderPartial("OrderRulesPlugins/_OptionRestrictAccounts", Model.RuleValidationsDTO.FirstOrDefault()); %>
		<% Html.RenderPartial("OrderRulesPlugins/_OptionRestrictOrderTypes", Model.RuleValidationsDTO.FirstOrDefault()); %>
		<% Html.RenderPartial("OrderRulesPlugins/_OptionRestrictStoreFronts", Model.RuleValidationsDTO.FirstOrDefault()); %>
        <% Html.RenderPartial("OrderRulesPlugins/_AccountIDLoad", Model.RuleValidationsDTO.FirstOrDefault()); %>
        <% Html.RenderPartial("OrderRulesPlugins/_OptionRestrictProductTypes", Model.RuleValidationsDTO.FirstOrDefault()); %>
      <div class="UI-lightBg pad2 bold overflow">
			<a style="" id="reviewOptionsBottom" class="FR mr10 viewHideOptions" href="#">
				<%= Html.Term("PromotionOptions_MinimizeOptions", "Minimize Options")%></a>
		</div>
    </div>
</div>

	
<% Html.RenderPartial("OrderRulesPlugins/_ValidationProductOrderRules", Model.RuleValidationsDTO.FirstOrDefault()); %>

<div class="mt10" id="SaveRewards">
	<a class="Button BigBlue" id="btnSave" href="javascript:void(0);">
		<%= Html.Term("Rules_SaveRule", "Save Rule")%></a>
</div>
