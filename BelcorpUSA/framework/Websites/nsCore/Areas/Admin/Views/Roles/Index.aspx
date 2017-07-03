<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<List<int>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Admin") %>">
        <%= Html.Term("Admin", "Admin") %></a> >
    <%= Html.Term("Roles") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">

        $('.delete').die().live('click', function () {
            if (confirm('Would you like to delete this role value?')) {
                var listValue = $(this).parent().find('input'), id = listValue.attr('name').replace(/\D/g, '');
                if (id > 0) {
                    var data = { id: id };
                    $.post('<%= ResolveUrl("~/Admin/Roles/Delete") %>', data, function (response) {
                        if (response.result) {
                            listValue.parent().fadeOut('normal', function () {
                                $(this).remove();
                            });
                        }
                        else {
                            showMessage(response.message, true);
                            return false;
                        }
                    });
                }
            }
        });
    
    </script>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Roles") %>
        </h2>
        <a href="<%= ResolveUrl("~/Admin/Roles/Edit") %>">
            <%= Html.Term("AddNewRole", "Add new role") %></a>
    </div>
    <ul class="listValues roles">
        <%
            var roleList = SmallCollectionCache.Instance.Roles.OrderByDescending(r => Model.Contains(r.RoleID)).ThenBy(r => r.GetTerm()).ToList();
            foreach (var role in roleList)
            {
                if (Model.Contains(role.RoleID) && roleList.IndexOf(role) == 0)
                { %>
        <li><h3><%: Html.Term("AccountTypeRoles","Account Type Roles") %></h3></li>
             <% }
                else if (!Model.Contains(role.RoleID) && roleList.IndexOf(role) != 0 && Model.Contains(roleList[(roleList.IndexOf(role) - 1)].RoleID))
                { %>
        <li><br /><h3><%: Html.Term("UserRoles", "User Roles")%></h3></li>
             <% }  %>
        <li><a href="<%= ResolveUrl("~/Admin/Roles/Edit/") + role.RoleID %>">
            <%= role.GetTerm() %></a>
            <input type="hidden" name="value<%= role.RoleID %>" value="<%= role.Name %>" class="listValue" />
            <%if (role.Editable && !Model.Contains(role.RoleID))
              {
            %>
            <a href="javascript:void(0);" class="delete listValue">
                <span class="UI-icon icon-x" title="<%= Html.Term("Delete", "Delete") %>"></span></a>
            <% }
              else
              { 
            %>
            <span class="UI-icon icon-lock" title="locked"></span>
            <% } %>
        </li>
         <% } %>
    </ul>
</asp:Content>
