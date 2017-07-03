<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Admin.Models.UsersEditModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<!--[if IE]>
	<style type="text/css">
		input[type="password"]
		{
			width:12.417em;
		}
	</style>
	<![endif]-->
	<script type="text/javascript">
		$(function () {

			$('#userProperties').setupRequiredFields();
			$('#btnSave').click(function () {

				var txtPassword = $('#password');
				var txtPasswordConfirm = $('#confirmPassword');

				if (!$('#userProperties').checkRequiredFields()) {
					return false;
				}
				if (txtPassword.val() != txtPasswordConfirm.val()) {
					txtPasswordConfirm.showError('<%= Html.JavascriptTerm("PasswordsMustMatch", "The passwords must match.") %>');
					return false;
				}

				// Clear any lingering error messages
				txtPassword.clearError();
				txtPasswordConfirm.clearError();


				var data = {
					userId: $('#userId').val(),
					firstName: $('#firstName').val(),
					lastName: $('#lastName').val(),
					username: $('#username').val(),
					password: txtPassword.val(),
					confirmPassword: txtPasswordConfirm.val(),
					passwordQuestion: $('#passwordQuestion').val(),
					passwordAnswer: $('#passwordAnswer').val(),
					hasAccessToAllSites: $('#hasAccessToAllSites').prop('checked'),
					userChangingPassword: $('#newPassword').is(':visible'),
					email: $('#email').val(),
					statusId: $('#statusId').val(),
					defaultLanguageId: $('#defaultLanguageId').val(),
					createShoppingAccount: $('#createEmployeeAccount').prop('checked')
				};

				$('#roles .role:checked').each(function (i) {
					data['roles[' + i + ']'] = $(this).val();
				});

				$('#sites .site:checked').each(function (i) {
					data['sites[' + i + ']'] = $(this).val();
				});

				var t = $(this);
				showLoading(t);
				$.post('<%= ResolveUrl("~/Admin/Users/Save") %>', data, function (response) {
					showMessage(response.message || 'User saved successfully!', !response.result);
					if (response.result) {
						$('#userId').val(response.userId);
						if (response.accountId && response.accountId != '') {
							$('#createEmployeeAccount').attr('disabled', 'disabled').attr('checked', true);
							$('#lblCreateEmployeeAccount').html('<%= Html.Term("EmployeeAccountExists", "Employee account created")%> <a href="<%= ResolveUrl("~/Accounts/Overview/Index/") %>' + response.accountId + '"><%= Html.Term("View", "view")%></a>');
						}
                    }
//                    else {
//                        showMessage(response.message, true);
//                    }
				})
				.fail(function () {
					showMessage('@Html.Term("ErrorSavingUser", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
				})
				.always(function () {
					hideLoading(t);
				});
			});

			var checkAllSites = function () {
				if ($('#hasAccessToAllSites').prop('checked')) {
					$('#sites .site').attr('disabled', 'disabled').attr('checked', true);
				}
				else {
					$('#sites .site').removeAttr('disabled').attr('checked', false);
				}
			};

			var allSitesSelected = function () {
				if ($('#hasAccessToAllSites').prop('checked')) {
					$('#sites .site').attr('disabled', 'disabled').attr('checked', true);
				}
			}

			$('#btnChangePassword,#btnCancelChangePassword').click(function () {
				$('#password, #confirmPassword').val('').clearError();
				$('#encrypedPassword,#newPassword,#newPasswordConfirm').toggle();
			});

			$('#hasAccessToAllSites').click(checkAllSites);

			//checkAllSites();
			allSitesSelected();
		});
		
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Admin") %>">
		<%= Html.Term("Admin", "Admin") %></a> > <a href="<%= ResolveUrl("~/Admin/Users") %>">
			<%= Html.Term("Users", "Users") %></a> >
	<%= Model.CorporateUser.UserID == 0 ? Html.Term("AddUser", "Add User") : Html.Term("EditUser", "Edit User")  %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Model.CorporateUser.CorporateUserID == 0 ? Html.Term("AddUser", "Add User") : Html.Term("EditUser", "Edit User")%>
		</h2>
		<%if (Model.CorporateUser.CorporateUserID > 0)
	{ %><%= Html.Term("EditUser", "Edit User")%>
		| <a href="<%= ResolveUrl("~/Admin/Users/AuditHistory") %>/<%= Model.CorporateUser.CorporateUserID %>">
			<%= Html.Term("AuditHistory", "Audit History")%></a>
		<%} %>
	</div>
	<input type="hidden" id="userId" value="<%= Model.CorporateUser.CorporateUserID == 0 ? "" : Model.CorporateUser.CorporateUserID.ToString() %>" />
	<table id="userProperties" width="100%" cellspacing="0" class="DataGrid">
		<tr>
			<td style="width: 13.636em;">
				<%= Html.Term("FirstName", "First Name") %>:
			</td>
			<td>
				<input type="text" id="firstName" class="required" name="<%= Html.Term("FirstNameRequired", "First Name is required.") %>" value="<%= Model.CorporateUser.FirstName %>" />
			</td>
		</tr>
		<tr>
			<td style="width: 13.636em;">
				<%= Html.Term("LastName", "Last Name") %>:
			</td>
			<td>
				<input type="text" id="lastName" class="required" name="<%= Html.Term("LastNameRequired", "Last Name is required.") %>" value="<%= Model.CorporateUser.LastName %>" />
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("Username") %>:
			</td>
			<td>
				<input type="text" id="username" class="required" name="<%= Html.Term("UsernameRequired", "Username is required.") %>" value="<%= Model.CorporateUser.User.Username %>" />
			</td>
		</tr>
		<tr id="encrypedPassword" <%= Model.CorporateUser.CorporateUserID == 0 ? "style=\"display:none;\"" : "" %>>
			<td>
				<%= Html.Term("Password") %>:
			</td>
			<td>
				(<%= Html.Term("Encrypted", "Encrypted") %>) <a id="btnChangePassword" href="javascript:void(0);">
					<%= Html.Term("ChangePassword", "Change Password") %></a>
			</td>
		</tr>
		<tr id="newPassword" <%= Model.CorporateUser.CorporateUserID == 0 ? "" : "style=\"display:none;\"" %>>
			<td>
				<%= Html.Term("Password") %>:
			</td>
			<td>
				<input type="password" id="password" class="required" name="<%= Html.Term("PasswordRequired", "Password is required.") %>" value="" />
				<%if (Model.CorporateUser.CorporateUserID > 0)
	  { %>
				<a id="btnCancelChangePassword" href="javascript:void(0);">
					<%= Html.Term("CancelChangePassword", "Cancel Change Password") %></a>
				<%} %>
			</td>
		</tr>
		<tr id="newPasswordConfirm" <%= Model.CorporateUser.CorporateUserID == 0 ? "" : "style=\"display:none;\"" %>>
			<td>
				<%= Html.Term("ConfirmPassword", "Confirm Password") %>:
			</td>
			<td>
				<input type="password" id="confirmPassword" class="required" name="<%= Html.Term("PasswordConfirmRequired", "Password confirmation is required.") %>" value="" />
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("PasswordQuestion", "Password Question")%>:
			</td>
			<td>
				<input type="text" id="passwordQuestion" value="<%= Model.CorporateUser.User.PasswordQuestion %>" style="width: 20.833em;" />
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("PasswordAnswer", "Password Answer")%>:
			</td>
			<td>
				<input type="text" id="passwordAnswer" value="<%= Model.CorporateUser.User.PasswordAnswer %>" />
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("EmailAddress", "Email Address") %>:
			</td>
			<td>
				<input type="text" id="email" value="<%= Model.CorporateUser.Email %>" style="width: 20.833em;" />
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("Language", "Language") %>:
			</td>
			<td>
				<%= Html.DropDownLanguages(htmlAttributes: new { id = "defaultLanguageId" }, selectedLanguageID: Model.CorporateUser.User.DefaultLanguageID > 0 ? Model.CorporateUser.User.DefaultLanguageID : 1)%>
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("Status", "Status") %>:
			</td>
			<td>
				<select id="statusId">
					<%foreach (var status in SmallCollectionCache.Instance.UserStatuses)
	   { %>
					<option value="<%= status.UserStatusID %>" <%= status.UserStatusID == Model.CorporateUser.User.UserStatusID ? "selected=\"selected\"" : "" %>>
						<%= status.GetTerm() %></option>
					<%} %>
				</select>
			</td>
		</tr>
        <tr>
		<td>
			<%= Html.Term("EmployeeAccount", "Employee Account")%>:
		</td>
		<td>
			<input type="checkbox" id="createEmployeeAccount" <%= Model.Account != null ? "checked=\"checked\" disabled=\"disabled\"" : "" %> />
			<label id="lblCreateEmployeeAccount" for="createEmployeeAccount">
				<%if (Model.Account != null)
	  { %><%= Html.Term("EmployeeAccountExists", "Employee account created")%>
				| <a href="<%= ResolveUrl("~/Accounts/Overview/Index/" + Model.Account.AccountID) %>">&nbsp
					<%= Html.Term("View", "view")%></a>
				<%} %>
				<%  else
	  { %>
				<%= Html.Term("CreateEmployeeAccount", "Create an Employee Account / Enable shopping") %>
				<%  } %>
			</label>
		</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("Role", "Role") %>:
			</td>
			<td id="roles">
				<% 
					foreach (var role in SmallCollectionCache.Instance.Roles.OrderBy(r => r.Name))
					{
				%>
				<input type="checkbox" class="role" id="roleCheckBox<%= role.RoleID %>" value="<%= role.RoleID %>" <%= Model.CorporateUser.User.Roles.ContainsRole(role.RoleID) ? "checked=\"checked\"" : "" %> />
				<label for="roleCheckBox<%= role.RoleID %>">
					<%= role.GetTerm() %></label><br />
				<%} %>
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("SiteAccess", "Site Access") %>:
			</td>
			<td id="sites">
				<input type="checkbox" id="hasAccessToAllSites" <%= Model.CorporateUser.HasAccessToAllSites ? "checked=\"checked\"" : "" %> />
				<label for="hasAccessToAllSites">
					<b>
						<%= Html.Term("AllSites", "All Sites")%></b></label>
				<br />
				<% var markets = SmallCollectionCache.Instance.Markets; // Market.GetAll(0, 0)
	   foreach (var baseSite in NetSteps.Data.Entities.Site.LoadBaseSites().OrderBy(s => s.Name))
	   { 
				%>
				<input type="checkbox" class="site" id="siteAccessCheckBox<%= baseSite.SiteID %>" value="<%= baseSite.SiteID %>" <%= Model.CorporateUser.HasAccessToSite(baseSite.SiteID) ? "checked=\"checked\"" : "" %> />
				<label for="siteAccessCheckBox<%= baseSite.SiteID %>">
					<%= baseSite.Name %>
					-
					<%= markets.First(m => m.MarketID == baseSite.MarketID).GetTerm() %></label><br />
				<%} %>
			</td>
		</tr>
	</table>
	<p>
		<a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
			<%= Html.Term("Save") %></a> <a href="<%= ResolveUrl("~/Admin/Users") %>" class="Button"><span>
				<%= Html.Term("Cancel") %></span></a>
	</p>
</asp:Content>
