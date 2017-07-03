<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.CorporateUser>>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Admin") %>">
		<%= Html.Term("Admin", "Admin") %></a> > <%= Html.Term("Users") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Users")%>
		</h2>
		<a href="<%= ResolveUrl("~/Admin/Users/Edit") %>"><%= Html.Term("AddNewUser", "Add new user") %></a>
	</div>
	<%if (TempData["SavedUser"] != null)
   { %>
	<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous; -moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0; border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold; margin-bottom: 10px; padding: 7px;">
		<div style="color: #385E0F; display: block;">
			<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("UserSaved", "User saved successfully!") %></div>
	</div>
	<%} %>
	<% Html.PaginatedGrid("~/Admin/Users/Get")
			.AddColumn(Html.Term("Name"), "Name", true, true, NetSteps.Common.Constants.SortDirection.Ascending)
			.AddColumn(Html.Term("Username"), "User.Username", true)
			.AddColumn(Html.Term("Email"), "Email", true)
			.AddColumn(Html.Term("Role"), "Role", false)
			.AddColumn(Html.Term("Status"), "User.UserStatus.TermName", true)
			.AddColumn(Html.Term("LastLogin", "Last Login"), "User.LastLoginUTC", true)
			.AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(SmallCollectionCache.Instance.UserStatuses.ToDictionary(us => us.UserStatusID.ToString(), us => us.GetTerm())))
			.AddSelectFilter(Html.Term("Role"), "role", new Dictionary<string, string>() { { "", Html.Term("SelectaRole", "Select a Role...") } }.AddRange(SmallCollectionCache.Instance.Roles.ToDictionary(r => r.RoleID.ToString(), r => r.GetTerm())))
			.AddInputFilter(Html.Term("Username"), "username")
			.CanChangeStatus(true, true, "~/Admin/Users/ChangeStatus")
			.Render(); %>
</asp:Content>