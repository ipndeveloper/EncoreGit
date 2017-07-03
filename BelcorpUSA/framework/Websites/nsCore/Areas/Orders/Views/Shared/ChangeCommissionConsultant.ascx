<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Order>" %>
<div id="changeCommissionConsultant">

	<script type="text/javascript">
	    $(function () {
	        $("#btnChangeSubmit").click(function () {
	            if ($('#commissionConsultantId').val()) {
	                $.post('<%= ResolveUrl("~/Orders/Details/ChangeCommissionConsultant") %>', { orderNumber: $('#orderNumber').val(), commissionConsultantId: $('#commissionConsultantId').val() }, function (response) {
	                    if (response.result) {
	                        $('.consultant').html('<a href="<%= ResolveUrl("~/Accounts/Overview/Index/") %>' + response.accountNumber + '">' + response.fullName + '</a>');
	                        $('#changeCommissionConsultantModal').jqmHide();
	                        //							if (!userIsAuthorized) {
	                        //								$('#authorization').show();
	                        //								$('#changeCommissionConsultant').remove();
	                        //							}
	                        window.location.reload();
	                    } else {
	                        showMessage(response.message, true);
	                    }
	                });
	            } else {
	                $('#txtConsultantSearch').showError('Please use the autosuggest to choose a consultant.');
	            }
	        });

	        $('#txtConsultantSearch').jsonSuggest('<%= ResolveUrl("~/Accounts/SearchDistributors") %>', { onSelect: function (item) {
	            $('#commissionConsultantId').val(item.id);
	            $('#txtConsultantSearch').clearError();
	        },
	            minCharacters: 3, width: 250, ajaxResults: true, maxResults: 50, showMore: true
	        });
	    });
	</script>
	<%
		var order = Model;
	%>
	<h2 style="margin-bottom: 10px;">
		<%= Html.Term("ChangeCommissionConsultant", "Change Commission Consultant") %></h2>
		<div id="commissionConsultantErrors"></div>
	<span class="LawyerText">(<%= Html.Term("EnterAtLeast3Characters", "enter at least 3 characters") %>)</span>
	<br />
	<input type="text" id="txtConsultantSearch" style="width: 20.833em;" value="<%= order.ConsultantInfo == null ? "" : order.ConsultantInfo.FullName + " (#" + order.ConsultantInfo.AccountNumber + ")" %>" />
	<input type="hidden" id="commissionConsultantId" value="<%= order.ConsultantInfo == null ? "" : order.ConsultantInfo.AccountID.ToString() %>" />
	<br />
	<br />
	<p>
		<a href="javascript:void(0);" class="Button BigBlue" id="btnChangeSubmit"><%= Html.Term("Submit", "Submit") %></a>
		<a href="javascript:void(0);" id="btnChangeCancel" class="Button jqmClose"><%= Html.Term("Cancel", "Cancel") %></a>
	</p>
</div>
<span class="ClearAll"></span>