<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderRules.Service.DTO.RulesDTO>" %>
<link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
<script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
<script type="text/javascript">
<% 
	string radioID = Model.HasDates ? "#activeN" : "#activeY";
%>
    $(function () {
    	$('.TimePicker').timepickr();
		$('.DatePicker').datepicker();
		$('<%= radioID %>').attr('checked', 'checked').trigger('change');
        $('#ActiveImmediatelyHelp').click(function() {
            $('#activeHelp').fadeIn();
        });
        $('.hideDesc').click(function () {
			$(this).closest('div.desc').fadeOut('fast');
		});
    });
</script>
<div class="pad5 promotionOption activeImmediately">
    <div class="FL optionHelpIcons">
        <a class="UI-icon-wrapper" id="ActiveImmediatelyHelp">
            <span class="UI-icon icon-help"></span></a>
        <div class="FL ml10 adjustmentDesc activeImmediatelyDesc">
            <div class="UI-mainBg desc" id="activeHelp" style="display: none;">
                <%= Html.Term("Promotions_ActiveImmediatelyHelp", "By selecting \"Yes\", this promotion will continue indefinitely. If you would like to set an end date, select \"No\" to set an active date range.") %><hr />
                <a class="hideDesc" href="javascript:void(0);">Hide</a>
            </div>
        </div>
    </div>
	<div class="FLabel">
		<label for="timedActivity">
			<%=Html.Term("PromotionOptions_ActiveImmediatelyOption", "Active Immediately?")%></label>
	</div>
	<div rel="isNo" class="hasPanel" id="Activate">
		<span>
			<input type="radio" rel="TimedActivation" value="no" name="timedActivity" id="activeN" />
			<label for="activeN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="timedActivity" id="activeY" />
			<label for="activeY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<div <%= Model.HasDates?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="TimedActivation">
			<div class="FL mr10">
				<%=Html.Term("PromotionOptions_StartsOn", "Starts On:")%>
				<p>
					<input type="text" class="DatePicker" id="txtStartDate" value="<%= Model.StartDate.HasValue ? Model.StartDate.ToShortDateStringDisplay() : "" %>" />
					<input type="text" class="TimePicker" name="startTime" id="txtStartTime" value="<%= Model.StartDate.HasValue ? Model.StartDate.ToShortTimeStringDisplay() : "" %>" />
				</p>
			</div>
			<div class="FL">
				<%=Html.Term("PromotionOptions_ExpiresOn", "Expires On:")%>
				<p>
					<input type="text" class="DatePicker" id="txtEndDate" value="<%= Model.EndDate.HasValue ? Model.EndDate.ToShortDateStringDisplay() : "" %>" />
					<input type="text" class="TimePicker" name="endTime" id="txtEndTime" value="<%= Model.EndDate.HasValue ? Model.EndDate.ToShortTimeStringDisplay() : "" %>" />
				</p>
			</div>
		</div>
	</div>
</div>
