<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Web.Mvc.Controls.Models.AccountPropertiesModel>" %>
<script type="text/javascript">
    $(function () {
        //        $('.numeric').numeric();
    });
</script>
<% 
    //sort the AccountProperties using SortIndex column
    var propertyTypes = Model.AccountPropertyTypes.Where(p => p.Active).OrderBy("SortIndex").ToList();
   if (propertyTypes.Count > 0)
   { %>
<table id="accountProperties" width="100%" cellspacing="0" class="FormTable">
    <%  foreach (var propertyType in propertyTypes)
        {           
            var property = Model.AccountProperties.FirstOrDefault(p => p.AccountPropertyTypeID == propertyType.AccountPropertyTypeID);

            // check if the property is 'exempt reason' & if avatax is enabled
            if (propertyType.TermName == Model.AvataxPropertyTypeName && !Model.IsAvataxEnabled)
            {
                continue;
            }
            
     %>
     <%--Adding id for setting the visibility for the Account Property by <TR>--%>
    <tr class="property" id="PropertyTypeTR<%= propertyType.AccountPropertyTypeID %>">
        <td class="FLabel">
            <%if (propertyType.Required)
              { %>
            <span class="requiredMarker">*</span>
            <% } %>
            <label for="PropertyType<%= propertyType.AccountPropertyTypeID %>">
                <%=propertyType.GetTerm()%>:</label>
            <input type="hidden" class="accountPropertyId" value="<%= property == null ? "" : property.AccountPropertyID.ToString() %>" />
            <input type="hidden" class="accountPropertyTypeId" value="<%= propertyType.AccountPropertyTypeID %>" />
        </td>
        <td>
            <%if (propertyType.AccountPropertyValues.Count == 0)
              { %>
                   <% if (propertyType.DataType.ToLower() == "bit")
                      { %>
                            <input type="checkbox" class="propertyValue" id="PropertyType<%= propertyType.AccountPropertyTypeID %>" />
                   <% }
                      else
                      { %>
                        <input type="text" class="propertyValue<%= propertyType.DataType == "System.DateTime" ? " DatePicker" : "" %><%= propertyType.DataType == "System.Int32" ? " numeric" : "" %><%= propertyType.Required ? " required" : "" %>"
                        <%= propertyType.Required ? "name=\"" + propertyType.Name + " is required.\"" : "" %>
                            value="<%= property != null ? property.PropertyValue : "" %>" id="PropertyType<%= propertyType.AccountPropertyTypeID %>"
                            style="width: 300px;" />
                   <% }
              }
              else
              { %>
            <select class="propertyValue" id="PropertyType<%= propertyType.AccountPropertyTypeID %>">
                <%if (!propertyType.Required)
                  { %>
                <option value="" <%= property == null || !property.AccountPropertyValueID.HasValue ? "selected=\"selected\"" : "" %>>
                    <%= Html.Term("NoValue", "No Value") %></option>
                <% } foreach (AccountPropertyValue value in propertyType.AccountPropertyValues)
                  { %>
                <option value="<%= value.AccountPropertyValueID %>" 
                    <%= property != null && property.AccountPropertyValueID == value.AccountPropertyValueID ? "selected=\"selected\"" : ""  %>>
                    <%= value.Name%>
                </option>
                <% } %>
            </select>
            <% } %>
        </td>
    </tr>
    <% } %>
</table>
<% } %>
