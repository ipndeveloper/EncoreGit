<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderRules.Service.DTO.RuleValidationsDTO>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.ProductTypeIDs.Count > 0 ? "#restrictProductTypeY" : "#restrictProductTypeN";  
	%>
	$(function () { 
		$('<%= radioID %>').attr('checked', 'checked').trigger('change'); 

		$('#ProductTypes a.checkAllOptions').toggle(function () {
			$(this).text('<%=Html.Term("PromotionOptions_UncheckAllLink", "uncheck all")%>');
			$(this).closest('div').find(':checkbox').attr('checked', 'checked');
		}, function () {
			$(this).text('<%=Html.Term("PromotionOptions_CheckAllLink", "check all")%>');
			$(this).closest('div').find(':checkbox').removeAttr('checked');
		});
	});
</script>
<div class="pad5 promotionOption restrictOrderTypes">
	<div class="FL optionHelpIcons">
    </div>
	<div class="FLabel">
		<label for="ordersRestrict">
			<%=Html.Term("RulesOptions_RestrictProductTypesOption", "Restrict to Product Types?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="productTypes">
		<span>
			<input type="radio" value="no" name="productsRestrict" id="restrictProductTypeN" />
			<label for="restrictProductTypeN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="productsRestrict" id="restrictProductTypeY" />
			<label for="restrictProductTypeY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<div <%= Model.ProductTypeIDs.Count > 0 ?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="ProductTypes">
			<span class="lawyer">
				<%=Html.Term("RulesOptions_RestrictToProductTypesTip", "Only checked product types will contain the rule.")%>
			</span><a class="FR checkAllOptions" href="javascript:void();">
				<%=Html.Term("PromotionOptions_CheckAllLink", "check all")%></a> <span class="clr">
			</span>
			<ul class="flatList listNav">
				<%foreach (ProductType productType in SmallCollectionCache.Instance.ProductTypes)
                { %>
				<li>
					<input type="checkbox" class="productType" value="<%= productType.ProductTypeID %>" <%=Model.ProductTypeIDs.Contains(productType.ProductTypeID) ? "checked='checked'" : "" %>" />
					<label>
						<%= productType.GetTerm()%></label>
				</li>
				<%} %>
			</ul>
		</div>
	</div>
</div>
