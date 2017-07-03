<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.ProductPropertyValue>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%
    var type = Model.ProductPropertyType;
%>
    <li>
        

        <%if (type == null || type.Editable)
      {
          string filePath = Model.FilePath;
          %>
        <p class="FL mr10 AttributeName">
            <label><%=Html.Term("AttributeName", "Attribute Name")%>:</label>
            <input type="text" class="pad2 variantAttributeInput" name="value<%= Model.ProductPropertyValueID %>" value="<%= Model.Value %>" />
            <input type="hidden" class="valueId" value="<%=Model.ProductPropertyValueID %>" />
        </p>
        <p class="FL mr10 Thumbnail">
            <label><%=Html.Term("Thumbnail") %>:</label>
            <a href="javascript:void(0);" class="MediaLibrary btnBrowseServer" id="btnBrowseServer<%=Model.ProductPropertyValueID %>" style="display:<%=filePath.IsNullOrEmpty() ? "inline" : "none" %>;">
                <span><%=Html.Term("Upload") %></span>
            </a>
            <span class="thumbnailPreview" style="display:<%=filePath.IsNullOrEmpty() ? "none" : "inline" %>;" title="<%=Html.Term("DeleteThumbnail","Delete Thumbnail") %>">
                <%--<img class="imageThumbnail" alt="<%= Html.Term("DeleteThumbnail", "Delete Thumbnail") %>" src="<%=filePath.IsNullOrEmpty() ? "" : filePath.ReplaceFileUploadPathToken() %>" />--%>
                <a href="javascript:void(0);" class="btnDeleteImage">
                    <span class="UI-icon icon-deleteItem"></span>
                </a>
            </span>
        </p>
        <p class="FR RemoveRow">
            <a href="javascript:void(0);" class="DTL Remove" title="<%=Html.Term("DeleteAttribute","Delete attribute") %>"><span></span></a>
        </p>
 
        <%--<a href="javascript:void(0);" class="delete listValue"><span class="UI-icon icon-x" title="<%=Html.Term("DeleteValue", "Delete Value") %>"></span></a>--%>

        <script type="text/javascript">
            $(function () {
                createUploadButton("btnBrowseServer<%=Model.ProductPropertyValueID %>", '<%=type.ProductPropertyTypeID %>', '<%=Model.ProductPropertyValueID %>');
            });
        </script>
        
    <%} else { %>
        <span class="UI-icon icon-lock" title="locked"></span>
            <input type="text" value="<%= Model.Value %>" disabled="disabled" />
    <%} %>

    </li>