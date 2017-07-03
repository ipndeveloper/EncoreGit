<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Interfaces.IStandardQualificationPromotionModel>" %>
<script type="text/javascript">
	$(function () {
		<% string radioID = Model.OneTimeUse ? "#chkOneTimeUseY" : "#chkOneTimeUseN"; %>
		$('<%= radioID %>').attr('checked', 'checked');
	});
</script>
<div class="pad5 oneTimeUse percentOff">
	<div class="FL optionHelpIcons">
    </div>
	<div class="FLabel">
		<label for="chkOneTimeUse">
			<%=Html.Term("PromotionOptions_OneTimeUseOption", "One Time Use?")%></label>
	</div>
	<div>
		<span>
			<input type="radio" value="no" name="chkOneTimeUse" id="chkOneTimeUseN" />
			<label for="chkOneTimeUseN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="chkOneTimeUse" id="chkOneTimeUseY" />
			<label for="chkOneTimeUseY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
	</div>
</div>
