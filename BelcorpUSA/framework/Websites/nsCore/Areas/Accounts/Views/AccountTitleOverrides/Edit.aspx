<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.AccountTitleOverrides.AccountTitleOverrideEditModel>" %>

<%@ Import Namespace="nsCore.Extensions" %>
<%@ Import Namespace="NetSteps.Commissions.Common" %>
<%@ Import Namespace="NetSteps.Commissions.Common.Models" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>">
		<%= Html.Term("Accounts", "Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
			<%= CoreContext.CurrentAccount.FullName %></a> >
	<%: Html.ActionLink(Html.Term("TitleOverrides", "Title Overrides"), "Index") %>
	>
	<%= Html.Term("TitleOverride", "Title Override") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">

		function validateForm() {
			isComplete = $('table.DataGrid').checkRequiredFields();
			if (!isComplete) {
				showMessage('<%= Html.Term("ErrorsBelow", "There are some errors below, please correct them before continuing.") %>', true);
				$(window).scrollTop($('#errorCenter').offset().top - 20);
				return false;
			}
			return true;
		}

		$(document).ready(function () {

			$('#btnSave').click(function () {

				if (validateForm()) {
					var data = {
						titleID: $('#titleID').val(),
						titleTypeID: $('#titleTypeID').val(),
						notes: $('#notes').val(),
						overrideReasonID: $('#overrideReasonID').val(),
						accountTitleOverrideID: $('#accountTitleOverrideID').val(),
						periodID: $('#periodID').val()
					};

					$.post('<%= ResolveUrl("~/Accounts/AccountTitleOverrides/Save") %>', data, function (response) {
						if (response.result) {
							showMessage('<%= Html.Term("TitleOverrideSaved", "Title Override saved successfully") %>', false);
							setTimeout('window.location=\'<%: ResolveUrl("~/Accounts/AccountTitleOverrides") %>\'', 1000);
						}
						else {
							showMessage('<%= Html.Term("TitleOverrideFailed", "Title Override failed to save")%>: ' + response.message, true);
							return false;
						}
					});
				}
			});
		});


		
	</script>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("TitleOverride", "Title Override")%></h2>
	</div>
	<form id="TitleOverrideForm">
	<table width="100%" class="DataGrid" cellspacing="0">
		<%: Html.Hidden("accountTitleOverrideID", Model.AccountTitleOverride.AccountTitleOverrideId)%>
		<tbody>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("Period", "Period")%>:
				</td>
				<td>
					<%
						if (Model.AccountTitleOverride != null && Model.AccountTitleOverride.Period != null)
						{
							if (Model.AccountTitleOverride.Period == null || Model.AccountTitleOverride.Period.PeriodId == 0)
							{ 
					%>
					<select id="periodID">
						<%
								foreach (var item in Model.CurrentPeriods)
								{                                 
						%>
						<option value="<%= item.PeriodId %>" <%= (Model.AccountTitleOverride.Period != null && item.PeriodId == Model.AccountTitleOverride.Period.PeriodId) ? "selected=\"selected\"" : "" %>>
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
					<%: Html.Hidden("periodID", Model.AccountTitleOverride.Period.PeriodId)%>
					<%= String.Format("{0}-{1}", Model.AccountTitleOverride.Period.PeriodId.ToString().Substring(0, 4), Model.AccountTitleOverride.Period.PeriodId.ToString().Substring(4, 2))%>
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
					<%= Html.Term("Title", "Title")%>:
				</td>
				<td>
					<select id="titleID">
						<%
							var commissionsService = NetSteps.Encore.Core.IoC.Create.New<ICommissionsService>();
							var titles = commissionsService.GetTitles();
							foreach (ITitle item in titles)
							{ 
						%>
						<option value="<%= item.TitleId %>" <%= (Model.AccountTitleOverride.OverrideTitle != null && item.TitleId == Model.AccountTitleOverride.OverrideTitle.TitleId) ? "selected=\"selected\"" : "" %>>
							<%= Model.GetTerm(item)%>
						</option>
						<%
							} 
						%>
					</select>
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("TitleType", "Title Type")%>:
				</td>
				<td>
					<select id="titleTypeID">
						<%
							foreach (var item in Model.TitleTypes)
							{ 
						%>
						<option value="<%= item.TitleKindId %>" <%= (Model.AccountTitleOverride.OverrideTitleKind != null && item.TitleKindId == Model.AccountTitleOverride.OverrideTitleKind.TitleKindId) ? "selected=\"selected\"" : "" %>>
							<%= Model.GetTerm(item)%>
						</option>
						<%
							} 
						%>
					</select>
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("OverrideReason", "Override Reason")%>:
				</td>
				<td>
					<select id="overrideReasonID">
						<%
							foreach (var item in Model.OverrideReasons)
							{
						%>
						<option value="<%= item.OverrideReasonId %>" <%= (Model.AccountTitleOverride.OverrideReason != null && item.OverrideReasonId == Model.AccountTitleOverride.OverrideReason.OverrideReasonId) ? "selected=\"selected\"" : "" %>>
							<%= Model.GetTerm(item) %>
						</option>
						<%
							} 
						%>
					</select>
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em; vertical-align: top;">
					<%= Html.Term("Note", "Note")%>:
				</td>
				<td>
					<%= Html.TextArea(name: "notes", value: Model.AccountTitleOverride.Notes, rows: 5, columns: 30,
							htmlAttributes: new { 
								@class = "required",
								onFocus = "this.value = this.value;"
							}) %>
				</td>
			</tr>
		</tbody>
	</table>
	<p>
		<% 
			if (Model.AccountTitleOverride.IsEditable)
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
	<% Html.RenderPartial("MessageCenter"); %>
	<script type="text/javascript">
		$(function () {
			if (parseBool('<%= !Model.AccountTitleOverride.IsEditable %>')) {
				$('table.DataGrid :input').attr('disabled', true);
				showMessage('<%: Html.Term("CommissionPeriodPassedNotEditable", "Because this commission period is closed the item is no longer editable.") %>', true);
			}
		});
	</script>
</asp:Content>
