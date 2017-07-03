<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Interfaces.IStandardQualificationPromotionModel>" %>
<script type="text/javascript">
	$(function () {
		<% string radioID = Model.HasContinuity ? "#chkContinuityY" : "#chkContinuityN"; %>
		$('<%= radioID %>').attr('checked', 'checked');
	});
</script>
<div class="pad5 continuity percentOff">
	<div class="FL optionHelpIcons">
    </div>
	<div class="FLabel">
		<label for="chkHasContinuity">
			<%=Html.Term("PromotionOptions_ShouldHaveContinuityOption", "Should have continuity?")%></label>
	</div>
	<div>
		<span>
			<input type="radio" value="no" name="chkHasContinuity" id="chkContinuityN" />
			<label for="chkContinuityN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="chkHasContinuity" id="chkContinuityY" />
			<label for="chkContinuityY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
	</div>
</div>