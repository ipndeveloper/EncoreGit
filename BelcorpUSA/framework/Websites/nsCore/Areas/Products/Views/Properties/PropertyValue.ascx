<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.ProductPropertyValue>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="NetSteps.Common.Configuration" %>
<%@ Import Namespace="NetSteps.Common.Extensions" %>
<%
    //var test = Model.HtmlInputTypeID;
 %>
<script type="text/javascript">
    $(function () {
        uploads['<%=Model.ProductPropertyValueID %>'] = new AjaxUpload('btnBrowseServer<%=Model.ProductPropertyValueID %>', {
            action: '<%= ResolveUrl("~/Products/Properties/UploadImage") %>',
            responseType: 'json',
            autoSubmit: false,
            onChange: function (file) {
                currentLi = $("#<%=Model.ProductPropertyValueID %>");
                uploads['<%=Model.ProductPropertyValueID %>'].submit();
            },
            onSubmit: function (file, extension) {
                uploads['<%=Model.ProductPropertyValueID %>'].setData({
                    productPropertyTypeId: <%=Model.ProductPropertyTypeID %>,
                    valueId: <%=Model.ProductPropertyValueID %>
                });
            },
            onComplete: function (file, response) {
                if (response.result) {
                    currentLi.find('.thumbnailPreview').show();
                    currentLi.find('.imageThumbnail').attr("src", response.imagePath);
                    currentLi.find('.btnBrowseServer').hide();
                } else {
                    showMessage(response.message, true);
                }
            }
        });

    });
</script>
<li id="<%=Model.ProductPropertyValueID %>">
    <%if (Model.ProductPropertyType == null || Model.ProductPropertyType.Editable)
      {
          string filePath = Model.FilePath;
          %>
        
            <input type="text" name="value<%= Model.ProductPropertyValueID %>" value="<%= Model.Value %>" class="value" />
        
            <a href="javascript:void(0);" id="btnBrowseServer<%=Model.ProductPropertyValueID %>"
                    class="Button MediaLibrary btnBrowseServer" style="display:<%=filePath.IsNullOrEmpty() ? "inline" : "none" %>;">
                    <%= Html.Term("Upload", "Upload") %></a>
        
            <span class="thumbnailPreview" style="display:<%=filePath.IsNullOrEmpty() ? "none" : "inline" %>;">
                <img class="imageThumbnail ClearAll" style="width:25px;max-width:25px;overflow:hidden;display:inherit;" alt="<%= Html.Term("DeleteThumbnail", "Delete Thumbnail") %>" 
                    src="<%=filePath.IsNullOrEmpty() ? "" : filePath.ReplaceFileUploadPathToken() %>" />
                <a href="javascript:void(0);" class="btnDeleteImage" style="margin-left: 3px;display:inherit;"><img
                    src="<%= ResolveUrl("~/") %>Content/Images/Icons/remove-trans.png" alt="<%=Html.Term("DeleteImage", "Delete Image") %>" /></a>
            </span>
              
       <a href="javascript:void(0);" class="delete" style="margin-left: 3px;"><img
            src="<%= ResolveUrl("~/") %>Content/Images/Icons/remove-trans.png" alt="<%=Html.Term("DeleteValue", "Delete Value") %>" /></a>
        
    <%} else { %>
        <img src="<%= ResolveUrl("~/Content/Images/Icons/16x16/lock_disabled-trans.png") %>"
            alt="locked" /><input type="text" value="<%= Model.Value %>" disabled="disabled" />
    <%} %>
</li>