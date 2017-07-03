<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
	Inherits="System.Web.Mvc.ViewPage<List<AccountPolicy>>" %>

<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>">
		<%= Html.Term("Accounts", "Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
			<%= CoreContext.CurrentAccount.FullName %></a> >
	<%= Html.Term("PoliciesChangeHistory", "Policies Change History")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("PoliciesChangeHistory", "Policies Change History") %></h2>
		<a href="<%= ResolveUrl("~/Accounts/Overview") %>"><%= Html.Term("Overview") %></a> | <a href="<%= ResolveUrl("~/Orders/OrderEntry?accountId=") + CoreContext.CurrentAccount.AccountID %>">
			<%= Html.Term("PlaceNewOrder", "Place New Order") %></a> | <%= Html.Term("PoliciesChangeHistory", "Policies Change History") %> | <a href="<%= ResolveUrl("~/Accounts/Overview/AuditHistory") %>">
				<%= Html.Term("AuditHistory", "Audit History") %></a>
	</div>
	<table cellspacing="0" cellpadding="0" width="100%;" class="DataGrid">
		<tr class="GridColHead">
			<th>
				<%= Html.Term("Name", "Name")%>
			</th>
			<th>
				<%= Html.Term("Version", "Version") %>
			</th>
			<th>
				<%= Html.Term("DateReleased", "Date Released") %>
			</th>
			<th>
				<%= Html.Term("DateAccepted", "Date Accepted") %>
			</th>
		</tr>
		<%int count = 0;
          foreach (AccountPolicy row in Model)
          { %>
		<tr <%= count % 2 == 1 ? "class=\"Alt\"" : "" %>>
			<td>
				<%= row.Policy.Name%>
			</td>
			<td>
				<%= row.Policy.VersionNumber%>
			</td>
			<td>
				<%= row.Policy.DateReleased%>
			</td>
			<td>
				<%= row.DateAccepted%>
			</td>
		</tr>
		<%++count;
       } %>
	</table>
</asp:Content>
