<%@ Page Title="" Language="C#"  MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" 
 Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Admin.Models.ReEntryRules.ReEntryRulesModel>" %>
 
 
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


    <link href="../../../../Content/CSS/Validation.css" rel="stylesheet" type="text/css" />

    <script src="../../../../Scripts/jquery.number.min.js" type="text/javascript"></script>

    <script src="../../../../Scripts/jquery.numeric.js" type="text/javascript"></script>
    <script src="../../../../Scripts/Validaciones.js" type="text/javascript"></script>
    
     <script type="text/javascript">
         $(function () {

             $('#btnSave').click(function () {

                 var data = {};
                 var cantReq = $('#hdnCantReEntryRules').val();

                 for (var i = 0; i < cantReq; i++) {
                     data['listReEntryRules[' + i + '].ReEntryRuleID'] = $('#hdnReEntryRuleID' + i).val();
                     data['listReEntryRules[' + i + '].ReEntryRuleTypeID'] = $('#hdnReEntryRuleTypeID' + i).val();
                     data['listReEntryRules[' + i + '].ReEntryRuleValueID'] = $('#ddlTypes' + i).val();                    
                     data['listReEntryRules[' + i + '].Active'] = true;                    
                 }

                 $.post('<%=ResolveUrl("~/Admin/ReEntryRules/Save")%>', data, function (response) {
                     if (response.result) {
                         showMessage('Re Entry Rules saved!', false);
                         alert(response.reEntryRules.length)
                         for (var i = 0; i < response.reEntryRules.length; i++) {
                             $('#hdnReEntryRuleID' + i).val(response.reEntryRules[i].ReEntryRuleID);
                             $('#hdnReEntryRuleValueID' + i).val(response.reEntryRules[i].ReEntryRuleValueID);
                         }

                     } else {
                         showMessage(response.message, true);
                     }
                 });
             });

         });
         </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Admin") %>">
		<%= Html.Term("Admin", "Admin") %></a> > <%= Html.Term("Users") %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="hdnCantReEntryRules" value="<%= Model.cantReEntryRules %>" />

    <table id="creditItems" width="100%" class="DataGrid">
    <thead>
        <tr class="GridColHead">
            <th>
                <%= Html.Term("ReEntryType", "Re Entry Rule Type")%>
            </th>
            <th>
                <%= Html.Term("Value", "Value")%>
            </th>            
        </tr>
         </thead>
         <tbody>
           <%
               int @index = 0;
               string _reentryRuleTypeID = "";
               int _reEntryRuleValueID = 0;
               string _id = "";
           foreach (var item in Model.listReEntryRules )
           {
               _reentryRuleTypeID = item.ReEntryRuleTypeID.ToString() ;
               _reEntryRuleValueID = item.ReEntryRuleValueID;
               _id = "ddlTypes" + @index;
               %>
        <tr id="trID<%= @index %>" >
            <td>
            <input type="hidden" id="hdnReEntryRuleID<%= @index %>" value="<%= item.ReEntryRuleID %>" />
            <input type="hidden" id="hdnReEntryRuleTypeID<%= @index %>" value="<%= item.ReEntryRuleTypeID %>" />
            <input type="hidden" id="hdnReEntryRuleValueID<%= @index %>" value="<%= item.ReEntryRuleValueID %>" />                
             <%= item.nameType%>                
            </td>           
            <td>

            <%--<%=  int _reentryRuleTypeID= item.ReEntryRuleTypeID %>--%>
                <%= @Html.DropDownReentryRulesValuesByType(ReentryRuleTypeID: _reentryRuleTypeID, selectedReentryRuleValueID: _reEntryRuleValueID, htmlAttributes: new { id = _id })%>             
            </td>
            
        </tr>
        <% 
               @index++;
            } %>
            </tbody>
    </table>
    <p>
        <a href="javascript:void(0);" id="btnSave" class="Button BigBlue">
            <%= Html.Term("Save", "Save") %>
            <%= Html.Term("Information", "Information")%></a>
    </p>



</asp:Content>
