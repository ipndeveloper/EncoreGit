<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
	Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.DisbursementHolds.DisbursementHoldsEditModel>" %>

<%@ Import Namespace="nsCore.Extensions" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>">
		<%= Html.Term("Accounts", "Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
			<%= CoreContext.CurrentAccount.FullName %></a> >
	<%: Html.ActionLink(Html.Term("DisbursementHolds", "Disbursement Holds"), "Index")%>
	>
	<%= Html.Term("DisbursementHold", "Disbursement Holds")%>
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

		    

//		    var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
//		    $.datepicker.setDefaults($.datepicker.regional[cultureInfo]);

		


		    $('#btnSave').click(function () {

		        if (validateForm()) {

		            var data = {
		                overrideReasonID: $('#overrideReasonID').val(),
//		                holdUntil: GetFormatedDate($('#holdUntil').val()),
		                //		                startDate: GetFormatedDate($('#startDate').val()),
		                holdUntil: $('#holdUntil').val(),
		                startDate: $('#startDate').val(),
		                note: $('#note').val(),
		                disbursementHoldId: $('#disbursementHoldId').val()
		            };

		            $.post('<%= ResolveUrl("~/Accounts/DisbursementHolds/Save") %>', data, function (response) {
		                if (response.result) {
		                    showMessage('<%= Html.Term("DisbursementHoldSaved", "Disbursement Hold saved successfully") %>', false);
		                    setTimeout('window.location=\'<%: ResolveUrl("~/Accounts/DisbursementHolds") %>\'', 1000);
		                }
		                else {
		                    showMessage('<%= Html.Term("DisbursementHoldFailed", "Disbursement Hold failed to save")%>: ' + response.message, true);
		                    return false;
		                }
		            });
		        }
		    });
		});

    function GetFormatedDate(date) {
        var result = '';

        if (date != '' && date.length == 10) {
            var month = date.substr(0, 2);
            var day = date.substr(3, 2);
            var year = date.substr(6, 4);

            result = day + '/' + month + '/' + year;
        }

        return result;
    }
        		
	</script>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("DisbursementHold", "Disbursement Hold")%></h2>
	</div>
	<form id="DisbursementHoldForm">
	<table width="100%" class="DataGrid" cellspacing="0">
		<%: Html.Hidden("disbursementHoldId", Model.DisbursementHold.DisbursementHoldId)%>
		<tbody>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("StartDate", "Start Date")%>:
				</td>
				<td>
					<input id="startDate" name="startDate" class="TextInput DatePicker" type="text" value="<%= Model.DisbursementHold.StartDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo) %>" />
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("HoldUntilDate", "Hold Until Date")%>:
				</td>
				<td>
					<input id="holdUntil" name="holdUntil" class="TextInput DatePicker" type="text" value="<%= Model.DisbursementHold.HoldUntil.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo) %>" />
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em;">
					<%= Html.Term("DisbursementHoldReason", "Disbursement Hold Reason")%>:
				</td>
				<td>
					<%= Html.DropDownList(name: "overrideReasonID",
                            selectList: new SelectList(new Dictionary<string, string>().AddRange(Model.OverrideReasons), 
							dataValueField: "Key", 
							dataTextField: "Value",
							selectedValue: Model.DisbursementHold.Reason != null ? Model.DisbursementHold.Reason.OverrideReasonId : 0))%>
				</td>
			</tr>
			<tr>
				<td style="width: 13.636em; vertical-align: top;">
					<%= Html.Term("Note", "Note")%>:
				</td>
				<td>
					<%= Html.TextArea(name: "note", value: Model.DisbursementHold.Notes, rows: 5, columns: 30,
							htmlAttributes: new { 
								@class = "required",
								onFocus = "this.value = this.value;"
							})  %>
				</td>
			</tr>
		</tbody>
	</table>
	<p>
		<a href="javascript:void(0);" id="btnSave" class="Button BigBlue">Save</a>
		<%: Html.ActionLink("Cancel", "Index", null, new { @class = "Button"}) %>
	</p>
	</form>
	<div>
	</div>
	<% Html.RenderPartial("MessageCenter"); %>
</asp:Content>
