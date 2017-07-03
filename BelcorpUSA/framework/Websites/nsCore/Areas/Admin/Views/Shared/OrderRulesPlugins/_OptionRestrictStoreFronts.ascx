<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderRules.Service.DTO.RuleValidationsDTO>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.StoreFrontIDs.Count > 0 ? "#restrictMarketsY" : "#restrictMarketsN";  
	%>
	$(function () { 
		$('<%= radioID %>').attr('checked', 'checked').trigger('change'); 

		$('#RestrictMarkets a.checkAllOptions').toggle(function () {
			$(this).text('<%=Html.Term("PromotionOptions_UncheckAllLink", "uncheck all")%>');
			$(this).closest('div').find(':checkbox').attr('checked', 'checked');
		}, function () {
			$(this).text('<%=Html.Term("PromotionOptions_CheckAllLink", "check all")%>');
			$(this).closest('div').find(':checkbox').removeAttr('checked');
		});
	});
</script>
<div class="pad5 promotionOption restrictMarkets">
	<div class="FL optionHelpIcons">
	</div>
	<div class="FLabel">
		<label for="marketRestrict">
			<%=Html.Term("RulesOptions_RestrictStoreFrontsOption", "Restrict to Store Fronts?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="markets">
		<span>
			<input type="radio" value="no" name="marketRestrict" id="restrictMarketsN" />
			<label for="restrictMarketsN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="marketRestrict" id="restrictMarketsY" />
			<label for="restrictMarketsY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<div <%= Model.StoreFrontIDs.Count > 0 ?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="RestrictMarkets">
			<span class="lawyer FL">
				<%=Html.Term("RulesOptions_RestrictToStoreFrontsTip", "Only checked store fronts will contain the rule.")%>
			</span><a class="FR checkAllOptions" href="javascript:void();">
				<%=Html.Term("PromotionOptions_CheckAllLink", "check all")%></a> <span class="clr">
			</span>
			<ul class="clr flatList listNav">
				<% foreach (StoreFront storeFront in SmallCollectionCache.Instance.StoreFronts)
	            { %>
				<li>
					<input type="checkbox" class="market" value="<%= storeFront.StoreFrontID %>" <%= Model.StoreFrontIDs.Contains(storeFront.StoreFrontID) ? "checked='checked'" : "" %> />
					<label>
						<%= storeFront.GetTerm()%></label>
				</li>
				<%} %>
			</ul>
		</div>
	</div>
</div>
