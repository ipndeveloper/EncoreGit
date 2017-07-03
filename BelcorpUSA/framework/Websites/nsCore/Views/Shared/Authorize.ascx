<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<script type="text/javascript">
	var userIsAuthorized = false;
	$(function () {
	    $('#authorizationErrors').messageCenter();
	    $('#btnAuthorize').click(function (e) {
	        $('#authorizationErrors').messageCenter('clearAllMessages');
	        if (!$("#uxAuthorizeOverride").checkRequiredFields()) {
	            return false;
	        }

	        var t = $(this);
	        showLoading(t, { float: 'right' });
	        $.getJSON('<%= ResolveUrl("~/Security/Authorize") %>', {
	            username: $('#txtAdminUsername').val(),
	            password: $('#txtAdminPassword').val(),
	            'function': '<%= ViewData["Function"] %>'
	        }, function (response) {
	            if (response.result) {
	                $('#authorization').hide('fast');
	                var form = $(response.form);
	                $('#authorization').after(form);
	                form.show('fast');
	                if ($('#orderOverrides').length > 0) {
	                    getOverrides();
	                }
	                hideLoading(t);
	                //$(response.form).appendTo($('#authorization').parent()).show('fast');
	            } else {
	                $('#authorizationErrors').messageCenter('addMessage', response.message);
	                hideLoading(t);
	            }
	        });
	    });
	    $('#txtAdminPassword').keyup(function (e) {
	        if (e.keyCode == 13)
	            $('#btnAuthorizeOverride').click();
	    });
	    $.getJSON('<%= ResolveUrl("~/Security/Authorize") %>', { 'function': '<%= ViewData["Function"] %>' }, function (response) {
	        userIsAuthorized = response.result;
	        if (response.result) {
	            $('#authorization').after(response.form).hide();
	        }
	    });
	});
</script>

<div id="authorization">CXVG
	<div id="authorizationErrors">
	</div>
	<p>
		<%= Html.Term("EnterAdminUsername", "Enter Admin Username")%>:
		<input type="text" id="txtAdminUsername" class="TextInput Required" /></p>
	<p>
		<%= Html.Term("EnterAdminPassword", "Enter Admin Password")%>:
		<input type="password" id="txtAdminPassword" class="TextInput Required" /></p>
	<br />
    <p>
		<a href="javascript:void(0);" id="btnAuthorize" class="Button BigBlue"><%= Html.Term("Authorize", "Authorize")%></a>
        <a href="javascript:void(0);" class="Button jqmClose"><%= Html.Term("Cancel", "Cancel")%></a>
	</p>
	<span class="ClearAll"></span>
</div>
