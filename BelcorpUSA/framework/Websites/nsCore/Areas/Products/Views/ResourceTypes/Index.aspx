<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.ProductFileType>>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#btnAdd').click(function () {
                $('#resourceTypes').append('<li><input type="checkbox" name="check0" class="activeStatus" ><span>&nbsp;&nbsp;&nbsp;&nbsp;</span><input type="text" name="value0" class="resourceType"  /><a href="javascript:void(0);" class="delete listValue"><span class="UI-icon icon-x" title="Delete"></span></a></li>');
            });
            $('.delete').live('click', function () {
                if ($(this).parent().find('input').attr('name').replace(/\D/g, '') == 0 || confirm('Are you sure you want to delete this resource type?')) {
                    var resourceType = $(this).parent().find('input'), resourceTypeId = resourceType.attr('name').replace(/\D/g, '');
                    if (resourceTypeId > 0) {
                        $.post('<%= ResolveUrl("~/Products/ResourceTypes/Delete") %>', { resourceTypeId: resourceTypeId });
                    }
                    resourceType.parent().fadeOut('normal', function () {
                        $(this).remove();
                    });
                }
            });
            $('#btnSave').click(function () {
                var data = {};
                $('#resourceTypes .resourceType').each(function (i) {
                    data['resourceTypes[' + i + '].ProductFileTypeID'] = $(this).attr('name').replace(/\D/g, '');
                    data['resourceTypes[' + i + '].Name'] = $(this).val();
                });

                $('#resourceTypes .activeStatus').each(function (aCounter) {
                    data['resourceTypes[' + aCounter + '].Active'] = $(this).prop('checked'); 
                });


                $.post('<%= ResolveUrl("~/Products/ResourceTypes/Save") %>', data, function (response) {
                    showMessage(response.message || 'Resource Types saved!', !response.result);
                    if (response.result)
                        $('#resourceTypes').html(response.resourceTypes);
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> >
    <%= Html.Term("ResourceTypes", "Resource Types")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ResourceTypes", "Resource Types") %>
        </h2>
        <a href="javascript:void(0);" id="btnAdd">
            <%= Html.Term("AddNewResourceType", "Add new resource type") %></a>
    </div>
    <ul id="resourceTypes" class="listValues">
      <%= Html.Term("Active", "Active")%>
        <%foreach (ProductFileType resourceType in Model.OrderBy(r => r.Editable))
          { %>
        <li>
            <% bool activeVal;
               activeVal= false ;

               activeVal = resourceType.Active  == true ? true : false;   
                if (resourceType.Editable)
              { %>
            <input type="checkbox" name="check<%= resourceType.ProductFileTypeID %>"  <%=activeVal == true ? "checked=\"checked\"" : ""%>  class="activeStatus"  />
            <span>&nbsp;&nbsp;&nbsp;&nbsp;</span><input type="text" name="value<%= resourceType.ProductFileTypeID %>"
                value="<%= resourceType.GetTerm() %>" class="resourceType" maxlength="100" />
            <a href="javascript:void(0);" class="delete listValue">
                <span class="UI-icon icon-x" title="<%= Html.Term("Delete", "Delete") %>" />
            </a>
            <%}
              else
              { %>
            <input type="checkbox" name="check<%= resourceType.ProductFileTypeID %>" <%=activeVal == true ? "checked=\"checked\"" : ""%> disabled="disabled" /><span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
            <input type="text" value="<%= resourceType.GetTerm() %>" disabled="disabled" />
            <span class="UI-icon icon-lock" title="locked"></span>
            <%} %></li>
        <%} %>
    </ul>
    <span class="ClearAll"></span>
    <p>
        <a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
            <%= Html.Term("Save", "Save") %></a>
    </p>
</asp:Content>
