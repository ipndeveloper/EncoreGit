<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.CalculationOverrides.CalculationOverridesEditModel>" %>

<%@ Import Namespace="nsCore.Extensions" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>">
		<%= Html.Term("Accounts", "Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
			<%= CoreContext.CurrentAccount.FullName %></a> >
	<%: Html.ActionLink(Html.Term("CalculationOverrides", "Calculation Overrides"), "Index") %>
	>
	<%= Html.Term("CalculationOverride", "Calculation Override") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">

		function ajaxSuccess(json) {
			if (json.result == 'success') {
				showMessage('<%= Html.Term("CalculationOverrideSaved", "Calculation Override saved successfully") %>', false);
				setTimeout('window.location=\'<%: ResolveUrl("~/Accounts/CalculationOverrides") %>\'', 1000);

			} else {
				ajaxError();
			}
			$(window).scrollTop($('#errorCenter').offset().top - 20);
		}
		function ajaxError() {
			showMessage('<%= Html.Term("CalculationOverrideFailed", "Calculation Override failed to save.") %>', true);
		}

		function validateForm() {
			isComplete = $('table.DataGrid').checkRequiredFields();
			if (!isComplete) {
				showMessage('<%= Html.Term("ErrorsBelow", "There are some errors below, please correct them before continuing.") %>', true);
				$(window).scrollTop($('#errorCenter').offset().top - 20);
				return false;
			}
			return true;
		}

		$(function () {

			$('#btnSave').click(function () {

				if (validateForm()) {

					var data = {
						calculationTypeID: $('#calculationTypeID').val(),
						overrideTypeID: $('#overrideTypeID').val(),
						calculationValue: $('#calculationValue').val(),
						overrideReasonID: $('#overrideReasonID').val(),
						note: $('#note').val(),
						periodID: $('#periodID').val(),
						calculationOverrideID: $('#calculationOverrideID').val()
					};

					$.post('<%= ResolveUrl("~/Accounts/CalculationOverrides/Save") %>', data, function (response) {
						if (response.result) {
							showMessage('<%= Html.Term("CalculationOverrideSaved", "Calculation Override saved successfully") %>', false);
							setTimeout('window.location=\'<%: ResolveUrl("~/Accounts/CalculationOverrides") %>\'', 1000);
						}
						else {
							showMessage('<%= Html.Term("CalculationOverrideFailed", "Calculation Override failed to save")%>: ' + response.message, true);
							return false;
						}
					});

				}

			});
		});
	</script>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("CalculationOverride", "Calculation Override")%></h2>
	</div>
	<form id="calculationOverrideForm">
	<table width="100%" class="DataGrid" cellspacing="0">
		<%: Html.Hidden("calculationOverrideID", Model.CalculationOverride.CalculationOverrideId) %>
		<tbody>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("Period", "Period")%>:
				</td>
				<td>
					<%
						if (Model.CalculationOverride != null && Model.CalculationOverride.Period != null)
						{
							if (Model.CalculationOverride.Period.PeriodId == 0)
							{
					%>
					<select id="periodID">
						<%
								foreach (var item in Model.CurrentPeriods)
								{                                 
						%>
						<option value="<%= item.PeriodId %>" <%= item.PeriodId == Model.CalculationOverride.Period.PeriodId ? "selected=\"selected\"" : "" %>>
							<%= item.StartDateUTC.ToString("yyyy-MM")%>
						</option>
						<%
								} 
						%>
					</select>
					<% 
							}
							else
							{ 
					%>
					<%: Html.Hidden("periodID", Model.CalculationOverride.Period.PeriodId)%>
					<%= String.Format("{0}-{1}", Model.CalculationOverride.Period.PeriodId.ToString().Substring(0, 4), Model.CalculationOverride.Period.PeriodId.ToString().Substring(4, 2))%>
					<% 
							}
						}
						else
						{
					%>
						<%= Html.Term("Commissions_NoData_OrServiceUnavailable", "No data was returned, either because none exists or the service is unavailable.") %>
					<%
						}
					%>
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("Calculation", "Calculation")%>:
				</td>
				<td>
					<%= Html.DropDownList(name: "calculationTypeID",
								selectList: new SelectList(new Dictionary<string, string>().AddRange(Model.CalculationTypes),
								dataValueField: "Key", dataTextField: "Value",
														selectedValue: Model.CalculationOverride.CalculationKind != null ? Model.CalculationOverride.CalculationKind.CalculationKindId : 0))%>
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("OverrideType", "Override Type")%>:
				</td>
				<td>
					<%= Html.DropDownList(name: "overrideTypeID", 
							selectList: new SelectList(new Dictionary<string, string>().AddRange(Model.OverrideTypes),
							dataValueField: "Key", dataTextField: "Value",
																			selectedValue: Model.CalculationOverride.OverrideKind != null ? Model.CalculationOverride.OverrideKind.OverrideKindId : 0))%>
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("CalculationValue", "Calculation Value")%>:
				</td>
				<td>
					<%= Html.TextBox(name: "calculationValue",
							value: (Model.CalculationOverride.NewValue == 0) ? "" : Model.CalculationOverride.NewValue.ToString(), 
							htmlAttributes: new { 
								@class = "required",
                                monedaidioma="CultureIPN",
								onFocus = "this.select();"
							})  %>
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("OverrideReason", "Override Reason")%>:
				</td>
				<td>
					<%= Html.DropDownList(name: "overrideReasonID",
							selectList: new SelectList(new Dictionary<string, string>().AddRange(Model.OverrideReasons), 
							dataValueField: "Key", dataTextField: "Value",
							selectedValue: Model.CalculationOverride.OverrideReason != null ? Model.CalculationOverride.OverrideReason.OverrideReasonId : 0))%>
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em; vertical-align: top;">
					<%= Html.Term("Note", "Note")%>:
				</td>
				<td>
					<%= Html.TextArea(name: "note", value: Model.CalculationOverride.Notes, rows: 5, columns: 30,
							htmlAttributes: new { 
								@class = "required",
								onFocus = "this.value = this.value;"
							})  %>
				</td>
			</tr>
		</tbody>
	</table>
	<p>
		<%
			if (Model.CalculationOverride.IsEditable)
			{ 
		%>
		<a href="javascript:void(0);" id="btnSave" class="Button BigBlue">Save</a>
		<%
			}
		%>
		<%: Html.ActionLink("Cancel", "Index", null, new { @class = "Button"}) %>
	</p>
	</form>
	<div>
	</div>
	<% 
		Html.RenderPartial("MessageCenter"); 
	%>
	<script type="text/javascript">
		$(function () {
			if (parseBool('<%= !Model.CalculationOverride.IsEditable %>')) {
				$('table.DataGrid :input').attr('disabled', true);
				showMessage('<%: Html.Term("CommissionPeriodPassedNotEditable", "Because this commission period is closed the item is no longer editable.") %>', true);
			}
		});
	</script>
</asp:Content>
