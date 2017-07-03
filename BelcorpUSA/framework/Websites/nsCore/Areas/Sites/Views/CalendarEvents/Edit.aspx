<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master"
	Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.CalendarEvent>" ValidateRequest="false" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
	<script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
	<%--<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/adapters/jquery.js"></script>--%>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/adapters/jquery.js"></script>
	<script type="text/javascript">
		$(function () {

			function allDayEventChanged() {
				$('#allDay').val($('#chkAllDay').prop('checked'));
				if ($('#chkAllDay').prop('checked')) {
					$('#startTime,#endTime').attr('disabled', 'disabled');
					$('#endDate').data('oldDate', $('#endDate').val()).val($('#startDate').val()).attr('readonly', 'readonly').datepicker('destroy');
					$('#startTime').data('oldTime', $('#startTime').val()).val('');
					$('#endTime').data('oldTime', $('#endTime').val()).val('');
				} else {
					$('#startTime,#endTime').removeAttr('disabled');
					if ($('#endDate').data('oldDate')) {
						$('#endDate').val($('#endDate').data('oldDate'));
					}
					$('#endDate').removeAttr('readonly').datepicker('destroy').datepicker({
						changeMonth: true,
						changeYear: true,
						minDate: new Date(1900, 0, 1),
						yearRange: '-100:+100'
					});
					$('#startTime').val($('#startTime').data('oldTime'));
					$('#endTime').val($('#endTime').data('oldTime'));
				}
			}

			//			var ckEditor = CKEDITOR.replace('body');
			$('#body').ckeditor();
			$('#eventProperties').setupRequiredFields();
			$('#startTime,#endTime').timepickr({ convention: 12 });
			$('#chkAllDay').click(allDayEventChanged);
			$('#startDate').datepicker('destroy').datepicker({
				changeMonth: true,
				changeYear: true,
				minDate: new Date(1900, 0, 1),
				yearRange: '-100:+100',
				onSelect: function () {
					if ($('#chkAllDay').prop('checked')) {
						$('#endDate').val($('#startDate').val());
					}
				}
			});
			allDayEventChanged();

			$('#btnSave').click(function () {
				var t = $(this);
				showLoading(t);
				if ($('#eventProperties').checkRequiredFields()) {
					$.post('<%= ResolveUrl("~/Sites/CalendarEvents/Save") %>', {
						eventId: $('#eventId').val(),
						languageId: $('#languageId').val(),
						subject: $('#subject').val(),
						type: $('#type').val(),
						allDay: $('#chkAllDay').prop('checked'),
						startDate: $('#startDate').val(),
						startTime: $('#startTime').val(),
						endDate: $('#endDate').val(),
						endTime: $('#endTime').val(),
						priority: $('#priority').val(),
						isPublic: $('#isPublic').prop('checked'),
						state: $('#state').val(),
						caption: $('#caption').val(),
						body: $('#body').val()
					}, function (response) {
						hideLoading(t);
						showMessage(response.message || 'Event saved successfully!', !response.result);
						if (response.result)
							$('#eventId').val(response.eventId);
					});
				}
			});

			$('#languageId').change(function () {
				$.get('<%= ResolveUrl("~/Sites/CalendarEvents/GetTranslation") %>', { eventId: $('#eventId').val(), languageId: $(this).val() }, function (response) {
					if (response.result) {
						$('#subject').val(response.subject);
						$('#caption').val(response.caption);
						$('#body').val(response.body);
					} else {
						showMessage(response.message, true);
					}
				});
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Sites") %>">
		<%= Html.Term("Sites") %></a> > <a href="<%= ResolveUrl("~/Sites/CalendarEvents") %>">
			<%= Html.Term("CalendarEvents", "Calendar Events") %></a> >
	<%= Model == null || Model.CalendarEventID == 0 ? Html.Term("AddEvent", "Add Event") : Html.Term("EditEvent", "Edit Event") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<form action="<%= ResolveUrl("~/Sites/CalendarEvents/Save") %>" method="post">
	<div class="SectionHeader">
		<h2>
			<%= Model.CalendarEventID == 0 ? Html.Term("AddEvent", "Add Event") : Html.Term("EditEvent", "Edit Event") %></h2>
		<%= Html.Term("Language")%>:
		<%= Html.DropDownLanguages(errorMessage: "languageId", htmlAttributes: new { id = "languageId" }, selectedLanguageID: Constants.Language.English.ToInt())%>
	</div>
	<input type="hidden" id="eventId" value="<%= Model.CalendarEventID == 0 ? "" : Model.CalendarEventID.ToString() %>" />
	<table id="eventProperties" width="100%" cellspacing="0" class="DataGrid">
		<tr>
			<td style="width: 150px;">
				<%= Html.Term("Subject") %>:
			</td>
			<td>
				<input type="text" id="subject" name="<%= Html.Term("SubjectRequired", "Subject is required.") %>"
					class="required" value="<%= Model.HtmlSection == null ? "" : Model.HtmlSection.ProductionContentForDisplay(ViewData["CurrentSite"] as Site).FirstOrEmptyElement(Constants.HtmlElementType.Title).Contents %>" />
			</td>
		</tr>
		<tr>
			<td style="width: 150px;">
				<%= Html.Term("Type") %>:
			</td>
			<td>
				<select id="type" name="type">
					<% foreach (AccountListValue type in (ViewData["CalendarEventTypes"] as List<AccountListValue>))
					   { %>
					<option value="<%= type.AccountListValueID %>" <%= Model.CalendarEventTypeID == type.AccountListValueID ? "selected=\"selected\"" : "" %>>
						<%= type.Value %></option>
					<%} %>
				</select>
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("StartDate", "Start Date") %>:
			</td>
			<td>
				<input type="text" id="startDate" name="<%= Html.Term("StartDateRequired", "Start date is required.") %>"
					class="DatePicker required" value="<%= Model.StartDate == DateTime.MinValue ? DateTime.Today.ToShortDateString() : Model.StartDate.ToShortDateString() %>"
					style="width: 100px;" />
				<input type="text" id="startTime" name="startTime" value="<%= Model.StartDate.ToShortTimeString() %>" />
				<input type="checkbox" id="chkAllDay" <%= Model.IsAllDayEvent ? "checked=\"checked\"" : "" %> /><label
					for="chkAllDay"><%= Html.Term("AllDayEvent", "All day event")%></label>
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("EndDate", "End Date") %>:
			</td>
			<td>
				<input type="text" id="endDate" name="<%= Html.Term("EndDateRequired", "End date is required.") %>"
					class="DatePicker required" value="<%= !Model.EndDate.HasValue || Model.EndDate == DateTime.MinValue ? DateTime.Today.ToShortDateString() : Model.EndDate.ToShortDateString() %>"
					style="width: 100px;" />
				<input type="text" id="endTime" name="endTime" value="<%= Model.EndDate.ToDateTime().ToShortTimeString() %>" />
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("Priority")  %>:
			</td>
			<td>
				<select name="priority" id="priority">
					<% foreach (AccountListValue type in (ViewData["CalendarPriority"] as List<AccountListValue>))
					   {  %>
					<option value="<%= type.AccountListValueID %>" <%= Model.CalendarPriorityID == type.AccountListValueID ? "selected=\"selected\"" : "" %>>
						<%= type.Value %></option>
					<%} %>
				</select>
			</td>
		</tr>
		<tr>
			<td>
				<label for="isPublic">
					<%= Html.Term("IsPublic", "Is Public") %>:</label>
			</td>
			<td>
				<input id="isPublic" type="checkbox" <%= Model.IsPublic ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
		<%var activeStateProvinces = SmallCollectionCache.Instance.StateProvinces.Where(sp => SmallCollectionCache.Instance.Countries.Where(c => c.Active).Select(c => c.CountryID).Contains(sp.CountryID));
		  if (activeStateProvinces.Count() > 0)
		   { %>
		<tr>
			<td>
				<%= Html.Term("State", "State") %>:
			</td>
			<td>
				<select id="state" name="state">
					<option value="" <%= Model.Address == null || Model.Address.StateProvinceID == 0 ? "selected=\"selected\"" : "" %>>
						<%= Html.Term("SelectAState", "Select a state...") %></option>
					<% foreach (StateProvince state in activeStateProvinces)
					   { %>
					<option value="<%= state.StateProvinceID %>" <%= Model.Address != null && Model.Address.StateProvinceID == state.StateProvinceID ? "selected=\"selected\"" : "" %>>
						<%= state.StateAbbreviation %>
						-
						<%= SmallCollectionCache.Instance.Countries.First(c => c.CountryID == state.CountryID).CountryCode %></option>
					<%} %>
				</select>
			</td>
		</tr>
		<%} %>
		<tr>
			<td>
				<%= Html.Term("Caption") %>:
			</td>
			<td>
				<textarea id="caption" name="caption" rows="3" cols="30"><%= Model.HtmlSection == null ? "" : Model.HtmlSection.ProductionContentForDisplay(ViewData["CurrentSite"] as Site).FirstOrEmptyElement(Constants.HtmlElementType.Caption).Contents %></textarea>
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("Body") %>:
			</td>
			<td>
				<textarea id="body" name="body" rows="5" cols="20"><%= Model.HtmlSection == null ? "" : Model.HtmlSection.ProductionContentForDisplay(ViewData["CurrentSite"] as Site).FirstOrEmptyElement(Constants.HtmlElementType.Body).Contents%></textarea>
			</td>
		</tr>
	</table>
	<p>
		<a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
			<%= Html.Term("Save") %></a>
		<a href="<%= ResolveUrl("~/Sites/CalendarEvents") %>" class="Button"><span>
			<%= Html.Term("Cancel")%></span></a>
	</p>
	</form>
</asp:Content>
