<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Role>" %>
    <%@ Import namespace="NetSteps.Authorization.Common" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#btnSave').click(function () {
                if (!$('#roleProperties').checkRequiredFields()) {
                    return false;
                }

                var data = { roleId: $('#roleId').val(), name: $('#name').val() }, i = 0;
                $('input[name="functions"]').each(function () {
                    if ($(this).prop('checked')) {
                        data['functionIds[' + i + ']'] = $(this).val();
                        ++i;
                    }
                });
                $.post('<%= ResolveUrl("~/Admin/Roles/Save") %>', data, function (response) {
                    showMessage(response.message || 'Role saved successfully!', !response.result);
                    if (response.result)
                        $('#roleId').val(response.roleId);
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Admin") %>">
        <%= Html.Term("Admin", "Admin") %></a> > <a href="<%= ResolveUrl("~/Admin/Roles") %>">
            <%= Html.Term("Roles") %></a> >
    <%= Model.RoleID == 0 ? Html.Term("AddRole", "Add Role") : Html.Term("EditRole", "Edit Role") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%
        var authorizationService = NetSteps.Encore.Core.IoC.Create.New<NetSteps.Authorization.Common.IAuthorizationService>();
     %>
    <div class="SectionHeader">
        <h2>
            <%= Model.RoleID == 0 ? Html.Term("AddRole", "Add Role") : Html.Term("EditRole", "Edit Role") %>
        </h2>
    </div>
    <input type="hidden" id="roleId" name="roleId" value="<%= Model.RoleID == 0 ? "" : Model.RoleID.ToString() %>" />
    <table id="roleProperties" width="100%" cellspacing="0" class="DataGrid">
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Name") %>:
            </td>
            <td>
                <input type="text" id="name" class="required" name="<%= Html.Term("NameRequired", "Name is required.") %>"
                    value="<%= Model.Name %>" <% if(!Model.Editable) { %> disabled <% } %> />
                <%if (!Model.Editable)
                  {
                %>
                <span class="UI-icon icon-lock" title="locked"></span>
                <% } %>
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Functions") %>:
            </td>
            <td>
                <% var nsCoreTabs = new string[] { "Sites", "Accounts", "Orders", "Products", "Admin", "Communication", "Commissions", "Support", "Reports" };
                   foreach (string nsCoreTab in nsCoreTabs)
                   {
                %>
                <a href="javascript:void(0);" onclick="if(!$('#functions<%= nsCoreTab %>').is(':visible')){$('#functions<%= nsCoreTab %>').fadeIn('slow');}else{$('#functions<%= nsCoreTab %>').fadeOut('slow');}">
                    <%= nsCoreTab %>
                    <%= Html.Term("Functions", "Functions") %></a>
                <fieldset id="functions<%= nsCoreTab %>" style="display: none; margin-left: .909em;">
                    <legend>
                        <%= nsCoreTab%></legend>
                    <% foreach (var function in SmallCollectionCache.Instance.Functions.Where(f => f.Name.StartsWith(nsCoreTab)))
                       {
                           if (authorizationService.FilterAuthorizationFunctions(new string[] { function.Name }).Count() > 0)
                           { 
                    %>
                    <input type="checkbox" name="functions" id="functionCheckBox<%= function.FunctionID %>"
                        value="<%= function.FunctionID %>" <%= Model.Functions.ContainsFunction(function.FunctionID) ? "checked=\"checked\"" : "" %> />
                    <label for="functionCheckBox<%= function.FunctionID %>">
                        <%= string.IsNullOrEmpty(function.Name.Replace(nsCoreTab, "")) ? "General" : function.Name.Replace(nsCoreTab + "-", "")%></label><br />
                    <%}
                       }%>
                </fieldset>
                <br />
                <%
        
       } %>
                <a href="javascript:void(0);" onclick="if(!$('#otherFunctions').is(':visible')){$('#otherFunctions').fadeIn('slow');}else{$('#otherFunctions').fadeOut('slow');}">
                    <%= Html.Term("OtherFunctions", "Other Functions") %></a>
                <fieldset id="otherFunctions" style="display: none; margin-left: .909em;">
                    <legend>
                        <%= Html.Term("OtherFunctions", "Other Functions") %></legend>
                    <%
                        foreach (Function function in SmallCollectionCache.Instance.Functions.Where(f => !f.Name.StartsWithAny(nsCoreTabs)))
                      { 
                      if (authorizationService.FilterAuthorizationFunctions(new string[] { function.Name }).Count() > 0)
                           {
                           %>
                    <input type="checkbox" name="functions" id="otherFunctionsCheckBox<%= function.FunctionID %>"
                        value="<%= function.FunctionID %>" <%= Model.Functions.ContainsFunction(function.FunctionID) ? "checked=\"checked\"" : "" %> />
                    <label for="otherFunctionsCheckBox<%= function.FunctionID %>">
                        <%= function.Name%></label><br />
                    <%      } 
                      }%>
                </fieldset>
            </td>
        </tr>
    </table>
    <p>
        <a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
            <%= Html.Term("Save") %></a> <a href="<%= ResolveUrl("~/Admin/Roles") %>" class="Button">
                <span>
                    <%= Html.Term("Cancel") %></span></a>
    </p>
</asp:Content>
