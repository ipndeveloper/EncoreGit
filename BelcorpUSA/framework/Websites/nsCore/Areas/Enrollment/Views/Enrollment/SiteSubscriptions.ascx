<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%
    var enrollmentContext = ViewBag.EnrollmentContext as NetSteps.Web.Mvc.Controls.Models.Enrollment.EnrollmentContext;
%>
<script type="text/javascript">
    $(function () {
        //Add the following 4 lines to the top of any addon - DES
        window.enrollmentAccountNumber = '<%= enrollmentContext.EnrollingAccount.AccountNumber %>';
        if (parseBool('<%= ViewData["IsSkippable"] %>')) {
            $('#btnSkip').show();
        }

        var checkAvailability = function () {
            var t = $(this);
            t.parent().find('.availability').empty();
            if (t.val() != t.data('originalValue')) {
                t.parent().find('.checkAvailability').show();
            } else {
                t.parent().find('.checkAvailability').hide();
            }
        }, domains = $('.domain');

        $('.subdomain').keyup(checkAvailability);

        if (domains.length && domains[0].tagName.toLowerCase() == 'select') {
            domains.change(checkAvailability);
        }


        $('.checkAvailability').click(function () {
            var urlLink = $(this);
            $.getJSON('<%= ResolveUrl("~/Accounts/SiteSubscriptions/CheckIfAvailableUrl") %>', { url: 'http://' + urlLink.parent().find('.subdomain').val() + '.' + urlLink.parent().find('.domain').val() }, function (results) {
                var img = results.available ? '<span class="UI-icon icon-check"></span>' : '<span class="UI-icon icon-exclamation"></span>';
                urlLink.hide().parent().find('.availability').html(img);
            });
        });

        $('.toggleCustomUrl').live('click', function () {
            var p = $(this).parent(), f = p.find('.fixedDomain'), c = p.find('.customUrl');
            $(this).prop('checked') && f.fadeOut('fast', function () { c.fadeIn('fast'); }) || c.fadeOut('fast', function () { f.fadeIn('fast'); });
        });

        $('#btnAddUrl').click(function () {
            var domains = '<%= (ViewData["Domains"] as string[]).Join(",") %>'.split(','), builder = new StringBuilder(), i;
            builder.append('<div class="urlContainer"><p><span class="fixedDomain">http://<input class="subdomain required" type="text" value="" style="width: 8.333em;" name="<%= Html.Term("URLRequired", "URL is required") %>" />.');

            if (domains.Length == 1) {
                builder.append(String.format('<span>{0}</span><input type="hidden" class="domain" value="{0}" />', domains[0]));
            }
            else {
                builder.append('<select class="domain">');
                for (i = 0; i < domains.length; i++) {
                    builder.append(String.format('<option value="{0}">{0}</option>', domains[i]));
                }
                builder.append('</select>');
            }
            builder.append('</span><span class="customUrl" style="display: none;">http://<input class="url required" type="text" value="" style="width: 25em;" name="<%= Html.Term("URLRequired", "URL is required") %>" /></span>')
			.append('<input type="checkbox" class="toggleCustomUrl" />Use a custom url <a href="javascript:void(0);" class="DeleteUrl DTL Remove"></a><a href="javascript:void(0);" class="checkAvailability" style="display: none; margin-left: .909em;">Check availability</a><span class="availability"></span></p></div>');

            $('#container').append(builder.toString());
        });

        $('.DeleteUrl').live('click', function () {
            $(this).parent().remove();
        });

        $('#btnNext').click(function () {
            if (!$('.FormTable').checkRequiredFields()) {
                return false;
            }

            var data = getData();
            if (data === false)
                return false;

            window.letUnload = false;
            enrollmentMaster.postStepAction({
                step: "SiteSubscriptions",
                stepAction: "SubmitStep",
                data: data,
                showLoadingElement: $(this).parent(),
                load: true
            });
        });

        $('#btnSkip').click(function () {
            window.letUnload = false;
            enrollmentMaster.postStepAction({
                step: "SiteSubscriptions",
                stepAction: "SkipStep",
                load: true
            });
        });
    });

	function getData() {
		if ($('.checkAvailability:visible').length) {
			showMessage('Please check the availability of all urls before continuining.', true);
			return false;
		}

		var data = { autoshipScheduleId: $('#autoshipScheduleId').val() };
		$('.urlContainer').each(function (i) {
		    var t = $(this);
		    data['urls[' + i + ']'] = t.find('.toggleCustomUrl').prop('checked') ? 'http://' + t.find('.url').val() : 'http://' + t.find('.subdomain').val() + '.' + t.find('.domain').val();
		});
		return data;
	}
</script>
<div class="StepGutter">
	<h3>
		<b>
			<%= Html.Term("EnrollmentStep", "Step {0}", ViewData["StepCounter"]) %></b>
		<%= Html.Term("SetUpYourPWS", "Set up your PWS") %></h3>
</div>
<div class="StepBody">
	<table class="FormTable" cellspacing="0" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("WebsiteURLs", "Website URL(s)")%>
				<p class="InputTools">
					<a id="btnAddUrl" href="javascript:void(0);" class="">
						<%= Html.Term("AddAnotherUrl", "Add another url") %></a>
				</p>
			</td>
			<td id="container">
				<% var schedules = ViewData["Schedules"] as IEnumerable<AutoshipSchedule>; %>
				<select id="autoshipScheduleId" <%= schedules.Count() == 1 ? "style=\"display:none;\"" : "" %>>
					<% foreach(AutoshipSchedule schedule in schedules) { %>
					<option value="<%= schedule.AutoshipScheduleID %>">
						<%= SmallCollectionCache.Instance.IntervalTypes.GetById(schedule.IntervalTypeID).GetTerm() %></option>
					<% } %>
				</select>
				<div class="urlContainer">
					<p>
						<span class="fixedDomain">http://<input class="subdomain required" type="text" value=""
							style="width: 8.333em;" name="<%= Html.Term("URLRequired", "URL is required") %>" />.
							<%
							string[] domains = ViewData["Domains"] as string[];
	   if (domains.Length == 1)
	   { 
							%>
							<span>
								<%= domains[0] %></span>
							<input type="hidden" class="domain" value="<%= domains[0] %>" />
							<% } else { %>
							<select class="domain">
								<% foreach (string domain in domains)
		{ %>
								<option value="<%= domain %>">
									<%= domain %></option>
								<% } %>
							</select>
							<% } %>
						</span><span class="customUrl" style="display: none;">http://<input class="url required"
							type="text" value="" style="width: 25em;" name="<%= Html.Term("URLRequired", "URL is required") %>" />
						</span>
						<input type="checkbox" class="toggleCustomUrl" /><%= Html.Term("UseaCustomUrl", "Use a custom url") %>
						<a href="javascript:void(0);" class="checkAvailability" style="display: none; margin-left: .909em;">
							<%= Html.Term("CheckAvailability", "Check availability") %></a><span class="availability"></span>
					</p>
				</div>
			</td>
		</tr>
	</table>
	<span class="ClearAll"></span>
</div>
<span class="ClearAll"></span>
<p class="Enrollment SubmitPage">
    <a id="btnNext" href="javascript:void(0);" class="Button BigBlue"><%= Html.Term("Next") %>&gt;&gt;</a>
    <a id="btnSkip" href="javascript:void(0);" class="Button" style="display: none;"><%= Html.Term("Skip") %>&gt;&gt;</a>
</p>
