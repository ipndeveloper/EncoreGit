<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.CampaignAction>" %>
<div id="newsletterForm" style="background-color: White; padding: 10px;">
    <input type="hidden" id="newsletterCampaignID" value="<%= Model.CampaignID%>" />
    <input type="hidden" id="newsletterCampaignActionID" value="<%= Model.CampaignActionID%>" />
    <div class="SectionHeader">
		<% if (Model.CampaignActionTypeID == (short)Constants.CampaignActionType.Email)
	 { %>
        <%= Html.Term("NewsletterEmailCampaignAction", "Scheduled Email")%>
		<% }
	 else
	 {%>
		<%= Html.Term("NewsletterAlertCampaignAction", "Scheduled Alert Campaign")%>
		<%} %>
    </div>
    <table id="campaignAction" class="FormTable" width="100%">
        <tr>
            <td class="FLabel">
			<% if (Model.CampaignActionTypeID == (short)Constants.CampaignActionType.Email)
	  {%>
                <%= Html.Term("NewsletterName", "Newsletter Name")%>
			<%}
	  else
	  { %>
			 <%= Html.Term("AlertCampaignName", "Alert Campaign Name")%>
			 <%} %>
            </td>
            <td>
                <input id="name" type="text" class="required" name="<%= Html.Term("Nameisrequired", "Name is required")%>"
                    value="<%= Model.Name %>" style="width: 41.667em" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%=Html.Term("ScheduledSendDate", "Scheduled Send Date")%>
            </td>
            <td>
                <%--<%=Model.NextRunDate.HasValue ? Model.NextRunDate.Value.ToString(CoreContext.CurrentCultureInfo) : Html.Term("NotScheduled", "Not Scheduled")%>--%>
                <input id="runDate" name="<%=Html.Term("StartDateRequired", "Date is required") %>" class="TextInput DatePicker StartDate required" type="text" value="<%: Model.NextRunDate.HasValue ? Model.NextRunDate.Value.ToShortDateString() : "" %>" />
                <input id="runTime" name="<%=Html.Term("StartTimeRequired", "Time is requried") %>" class="TimePicker StartTime required" type="text" value="<%: Model.NextRunDate.HasValue ? Model.NextRunDate.Value.ToShortTimeString() : "" %>" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%=Html.Term("NewsletterConsultantEditableDate", "Consultant Editable Date") %>
            </td>
            <td>
                <%var emailCampaignAction = Model.EmailCampaignActions.FirstOrDefault();%>
                <%DateTime? distributorEditableDate = emailCampaignAction != null && emailCampaignAction.DistributorEditableDate.HasValue ? emailCampaignAction.DistributorEditableDate : null;%>
                <input id="distributorEditableDate" name="<%=Html.Term("StartDateRequired") %>" class="TextInput DatePicker required" type="text" value="<%: distributorEditableDate.HasValue ? distributorEditableDate.ToShortDateString() : ""%>" />
                <input id="distributorEditableTime" name="<%=Html.Term("StartTimeRequired") %>" class="TimePicker required" type="text" value="<%: distributorEditableDate.HasValue ? distributorEditableDate.ToShortTimeString() : ""%>" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Active")%>
            </td>
            <td>
                <input id="active" type="checkbox" <%= Model.Active ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <%--<%var emailCampaignAction = Model.EmailCampaignActions.FirstOrDefault(); %>
		<%if (emailCampaignAction != null && emailCampaignAction.EmailTemplateID > 0)%>
		<% { %>
		<tr>
			<td class="FLabel">
				<%= Html.Term("EmailTemplate", "Email Template")%>
			</td>
			<td>
				<a href="">
					<%= Html.Term("Edit EmailTemplate", "Edit Email Template")%></a>
			</td>
		</tr>
		<% } %>--%>
        <tr>
            <td colspan="2">
            <hr />
            <p class="pad10">
                <% if (Model.CampaignActionID > 0)
                   {
                %>
                <a id="btnSaveCampaignAction" class="Button BigBlue" href="javascript:void(0);">
                    <%=Html.Term("SaveandClose", "Save and Close")%>
                </a>
                <% } %>
                <a id="btnSaveCampaignActionAndTemplate" class="Button" href="javascript:void(0);">
                    <%=Html.Term("SaveandEditTemplate", "Save and Edit Template")%></a> <a href="javascript:void(0);"
                        class="Button jqmClose">
                        <%=Html.Term("Cancel", "Cancel")%></a>
            </p>    
            </td>
        </tr>
    </table>
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
    <script type="text/javascript">
        $(function () {


          
            $('.TimePicker').blur(function () { $(this).clearError(); }).timepickr({ convention: 12 });
            $('.DatePicker').datepicker({ changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100', onSelect: function () { $(this).clearError(); } });
            // set distributor max editable date to the selected run date
            $('#distributorEditableDate').datepicker('option', 'maxDate', $('#runDate').datepicker('getDate'));
            $('#runDate').datepicker('option', 'onSelect',
                function () {
                    var runDate = $('#runDate').datepicker('getDate');
                    $('#distributorEditableDate').datepicker('option', 'maxDate', runDate);
                    $(this).clearError();
                }
            );

            function parseDate(text) {
                var time = text.match(/(\d+):(\d{2})\s(\w{2})/);
                var h = time[1];
                if (time[3].toLowerCase() == 'pm') {
                    h += 12;
                }
                var m = time[2];
                var d = new Date();
                d.setHours(h);
                d.setMinutes(m);
                return d;
            }

            var dateRegex = /\d+\/\d+\/\d+/i, timeRegex = /\d+\:\d+\s(am|pm)/i;
            function save(editTemplate) {
                if ($('#campaignAction').checkRequiredFields()) {
                    var runDate = $('#newsletterForm #runDate').val();
                    var runTime = $('#newsletterForm #runTime').val();
                    var distributorEditableDate = $('#newsletterForm #distributorEditableDate').val();
                    var distributorEditableTime = $('#newsletterForm #distributorEditableTime').val();

                    //if they are the same date, and the editable time is after the run time, display error
                    if (($('#distributorEditableDate').datepicker('getDate') || new Date()).getTime() == $('#runDate').datepicker('getDate').getTime()
                        && parseDate(distributorEditableTime) > parseDate(runTime)) {
                        $('#distributorEditableTime').showError('<%= Html.JavascriptTerm("EditableTimeInvalid", "The editable time must be before the send time.") %>');
                        return false;
                    }


                    var data = {
                        newsletterCampaignID: $('#newsletterForm #newsletterCampaignID').val(),
                        emailTemplateID: $('#newsletterForm #emailTemplateID').val(),
                        scheduledRunDate: ((dateRegex.test(runDate) && timeRegex.test(runTime)) ? (runDate + ' ' + runTime) : ''),
                        distributorEditableDate: dateRegex.test(distributorEditableDate) ? (distributorEditableDate + ' ' + (timeRegex.test(distributorEditableTime) ? distributorEditableTime : '')) : '',
                        newsletterCampaignActionID: $('#newsletterForm #newsletterCampaignActionID').val(),
                        name: $('#newsletterForm #name').val(),
                        active: $('#newsletterForm #active').prop('checked')
                    };
                    $.post('<%= ResolveUrl("~/Communication/NewsletterCampaigns/SaveCampaignAction") %>', data, function (response) {
                        if (response.result) {
                            if (editTemplate && response.editTemplateUrl) {
                                window.location = response.editTemplateUrl;
                            }
                            else {
                                window.location.reload(true);
                            }
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            }
            $('#btnSaveCampaignAction').click(function () { save(false); return false; });
            $('#btnSaveCampaignActionAndTemplate').click(function () { save(true); return false; });
        });
    </script>
</div>
