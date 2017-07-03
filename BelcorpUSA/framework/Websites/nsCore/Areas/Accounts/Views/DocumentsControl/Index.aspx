<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
 Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.DocumentsControl.DocumentsControlModel>" %>

<asp:Content   ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">

    var Requirements = JSON.parse('<%= ViewBag.Requirements %>');

    $(function () {

        $('#btnSaveCustomerTypes').click(function () {

            var data = {};
            var cantReq = $('#hdnCantReq').val();

            for (var i = 0; i < cantReq; i++) {
                data['creditRequirements[' + i + '].CreditRequirementID'] = $('#hdnCreditID' + i + '_' + $('#hdnReqID' + i).val()).val();
                data['creditRequirements[' + i + '].RequirementTypeID'] = $('#hdnReqID' + i).val();
                data['creditRequirements[' + i + '].RequirementStatusID'] = $('#s' + i).val();
                data['creditRequirements[' + i + '].Observations'] = $('#txtObs' + i).val();

                data['creditRequirements[' + i + '].LastModifiedDate'] = $('#txtDate' + i).val();
                data['creditRequirements[' + i + '].LastUserModifiedName'] = $('#txtUser' + i).val();

                var isModified = false;
                if ($('#s' + i).val() != Requirements[i].RequirementStatusID || $('#txtObs' + i).val() != Requirements[i].Observations)
                    isModified = true;

                data['creditRequirements[' + i + '].IsModified'] = isModified;
            }

            $.post('<%=ResolveUrl("~/Accounts/DocumentsControl/Save")%>', data, function (response) {
                if (response.result) {
                    showMessage('Credit Requirement saved!', false);
                    location.reload();
//                    for (var i = 0; i < response.creditRequirements.length; i++) {
//                        $('#hdnCreditID' + i + '_' + response.creditRequirements[i].requirementTypeID).val(response.creditRequirements[i].creditRequirementID);

//                        $('#txtDate' + i).val(response.creditRequirements[i].lastModifiedDate);

//                        $('#txtUser' + i).val(response.creditRequirements[i].lastUserModifiedName);
//                    }

                } else {
                    showMessage(response.message, true);
                }
            });
        });

    });
         </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">

<a href="<%= ResolveUrl("~/Accounts") %>">Accounts</a> > <a href="<%= ResolveUrl("~/Accounts/DocumentsControl") %>">
		<%= CoreContext.CurrentAccount.FullName %></a> > Documents Control
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="hdnCantReq" value="<%= Model.cantRequirement %>" />

    <table id="creditItems" width="100%" class="DataGrid">
    <thead>
        <tr class="GridColHead">
            <th>
                <%= Html.Term("Requeriment88", "Requeriment")%>
            </th>
            <th>
                <%= Html.Term("Status", "Status")%>
            </th>
            <th>
                <%= Html.Term("Observations", "Observations")%>
            </th>
            <th>
                <%= Html.Term("Last Date Modified", "Last Date Modified")%>
            </th>
             <th>
                <%= Html.Term("User", "User")%>
            </th>
        </tr>
         </thead>
         <tbody>
           <%
               int @index = 0;
           foreach (var item in Model.listCreditRequirement )
           { %>
        <tr id="trID<%= @index %>" >
            <td>
            <input type="hidden" id="hdnCreditID<%= @index %>_<%= item.RequirementTypeID %>"
                     value="<%= item.CreditRequirementID %>" />

            <input type="hidden" id="hdnReqID<%= @index %>" value="<%= item.RequirementTypeID %>" />
                <%= item.nameRequirementType%>
                
            </td>
            <td>    
            <select id="s<%= @index %>"  class="accountType">
                <% foreach (var itemE in Model.lisRequirementStatuses)
                    { 
                        if(itemE.Active){       
                %>
                   
                <option value="<%= itemE.RequirementStatusID %>" <%= itemE.RequirementStatusID == item.RequirementStatusID ? "selected=\"selected\"" : "" %>>
                    <%= itemE.Name%>
                    </option>

                <%
                    }
                    } %>
            </select>
            </td>
            <td>
                <input id="txtObs<%= @index %>" type="text" value="<%= item.Observations%>"  size="40"/>            
            </td>
            <td>
              
               <input id="txtDate" type="text" value="<%=  string.IsNullOrEmpty(item.LastModifiedDate) ? "" : Convert.ToDateTime(item.LastModifiedDate , CoreContext.CurrentCultureInfo).ToShortDateString() %>" disabled="disabled" />
             <%--<input id="txtDate<%= @index %>" type="text" value="<%= item.LastModifiedDate%>" disabled="disabled" />--%>

            </td>
            <td>
            <input id="txtUser<%= @index %>" type="text" value="<%= item.Username%>" disabled="disabled" />
            </td>
        </tr>
        <% 
               @index++;
            } %>
            </tbody>
    </table>
    <p>
        <a href="javascript:void(0);" id="btnSaveCustomerTypes" class="Button BigBlue">
            <%= Html.Term("Save", "Save") %>
            <%= Html.Term("Information", "Information")%></a>
    </p>



</asp:Content>