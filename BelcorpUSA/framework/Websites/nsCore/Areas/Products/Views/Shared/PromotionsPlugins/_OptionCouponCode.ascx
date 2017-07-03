<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Interfaces.IStandardQualificationPromotionModel>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.HasCouponCode ? "#couponYes" : "#couponNo";  
    %>
	$(function () { 
		$('<%= radioID %>').attr('checked', 'checked').trigger('change');
	});
</script>
<div class="pad5 promotionOption couponCode percentOff">
	<div class="FL optionHelpIcons">
    </div>
    <div class="FLabel">
		<label for="chkCouponCodeRequired">
			<%=Html.Term("PromotionOptions_RequireCouponCodeOption", "Require a Coupon Code?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="RequireCoupon">
		<span>
			<input type="radio" value="no" name="chkCouponCodeRequired" id="couponNo" />
			<label for="couponNo">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" name="chkCouponCodeRequired" value="yes" id="couponYes" />
			<label for="couponYes">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<div <%= Model.HasCouponCode?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10" id="CouponCode">
			<%=Html.Term("PromotionOptions_CreateCouponCode", "Create the Coupon Code:")%>
			<p>
				<input type="text" name="<%=Html.Term("PromotionOptions_CouponCodeRequired", "Coupon Code cannot be empty") %>" class="pad5 required fullWidth" value="<%= Model.CouponCode %>"
					id="txtCouponCode" />
			</p>
		</div>
	</div>
</div>
