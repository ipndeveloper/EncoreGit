<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Interfaces.IStandardQualificationPromotionModel>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.HasOrderTypeIDs ? "#restrictOrderTypeY" : "#restrictOrderTypeN";  
	%>
	$(function () { 
		$('<%= radioID %>').attr('checked', 'checked').trigger('change'); 

		$('#OrderTypes a.checkAllOptions').toggle(function () {
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
			<%=Html.Term("PromotionOptions_RestrictOrderTypesOption", "Restrict to Order Types?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="orderTypes">
		<span>
			<input type="radio" value="no" name="ordersRestrict" id="restrictOrderTypeN" />
			<label for="restrictOrderTypeN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="ordersRestrict" id="restrictOrderTypeY" />
			<label for="restrictOrderTypeY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<div <%= Model.HasOrderTypeIDs?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="OrderTypes">
			<span class="lawyer">
				<%=Html.Term("PromotionOptions_RestrictOrderTypesTip", "Only checked order types will contain the promotion.")%>
			</span><a class="FR checkAllOptions" href="javascript:void();">
				<%=Html.Term("PromotionOptions_CheckAllLink", "check all")%></a> <span class="clr">
			</span>
			<ul class="flatList listNav">
				<%foreach (OrderType orderType in SmallCollectionCache.Instance.OrderTypes)
	  { %>
				<li>
					<input type="checkbox" class="orderType" value="<%= orderType.OrderTypeID %>" <%= Model.OrderTypeIDs.Contains(orderType.OrderTypeID) ? "checked='checked'" : "" %> />
					<label>
						<%= orderType.GetTerm()%></label>
				</li>
				<%} %>
			</ul>
		</div>
	</div>
</div>
