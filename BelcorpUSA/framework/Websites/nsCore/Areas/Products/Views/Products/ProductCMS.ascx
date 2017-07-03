<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.ProductCMSModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="NetSteps.Common.Globalization" %>
<%
%>
<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ajaxupload.js") %>"></script>
<script type="text/javascript">
    $(function () {

        new AjaxUpload('btnUpload', {
            action: '<%= ResolveUrl(string.Format("~/Products/Products/SaveImage/{0}/{1}", Model.ProductBase.ProductBaseID, Model.ProductID)) %>',
            data: { folder: 'Products', createProductFile: true, productId: $('#hidProductId').val() },
            responseType: 'json',
            onComplete: function (file, response) {
                if (response.result) {
                    var imageCount = $('#imagesContainer img').length;
                    $('#imagesContainer').append('<p id="productFile' + response.fileId + '" class="ImageThumbWrap"><span class="ImgTools"><a href="javascript:void(0);" class="moveUp"' + (imageCount > 0 ? '' : 'style="display:none;"') + '><span class="UI-icon icon-ArrowUp"></span></a><br><a href="javascript:void(0);" class="moveDown" style="display:none;"><span class="UI-icon icon-ArrowDown"></span></a><br></span><img width="100" src="' + response.imagePath + '" alt=""><input type="checkbox" class="checkbox" name=""></p>');
                    $('#btnSaveImages').show();
                    reorderImages();
                } else {
                    showMessage(response.message, true);
                }
            }
        });
    });
</script>
<input type="hidden" id="hidIsProductVariant" value="<%=Model.IsVariant %>" />
<% if (Model.IsVariant)
   {%>
<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
    -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
    border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
    margin-bottom: 10px; padding: 7px;">
    <div style="color: #FF0000; display: block;" class="UI-icon icon-exclamation">
        <%=Html.Term("VariantProductMessage", "You are editing a variant product.") %></div>
</div>
<%} %>
<table width="100%" class="SectionTable">
    <tr>
        <td width="70%" id="descriptionTD">
            <h3>
                <%= Html.Term("Descriptions", "Descriptions") %>
                <span class="childLock" style="display: <%=Model.IsVariant ? "block" : "none" %>;"><a
                    href="javascript:void(0);" class="locked">
                    <%//= Html.Term("Locked")%></a></span> <span class="childUnLock" style="display: <%=Model.IsVariant ? "none" : "block" %>;">
                        <a href="javascript:void(0);" class="locked unlocked">
                            <%//= Html.Term("Unlocked")%></a></span>
            </h3>
            <table class="DataGrid">
                <tr>
                    <td class="FLabel">
                        <%= Html.Term("DisplayName", "Display Name") %>:
                    </td>
                    <td>
                        <input id="txtName" type="text" value="<%= Model.Name %>" class="fullWidth" />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                        <%= Html.Term("ShortDescription", "Short Description") %>:
                    </td>
                    <td>
                        <textarea id="txtShortDescription" style="height: 50px;" class="fullWidth"><%= Model.ShortDescription %></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="FLabel" style="vertical-align: top;">
                        <%= Html.Term("LongDescription", "Long Description") %>:
                    </td>
                    <td>
                        <textarea id="txtLongDescription" style="width: 500px; height: 300px;"><%= Model.LongDescription %></textarea>
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                    </td>
                    <td>
                        <p class="FormButtons">
                            <a id="btnSaveDescriptions" href="javascript:void(0);" class="Button BigBlue">
                                <%= Html.Term("SaveDescriptions", "Save Descriptions") %></a>
                        </p>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width: 30%;" id="imageTD">
            <h3>
                <%= Html.Term("Images") %><span class="childLock" style="display: <%=Model.IsVariant ? "block" : "none" %>;"><a
                    href="javascript:void(0);" class="locked"><%//= Html.Term("Locked")%></a></span>
                <span>
                    <a href="javascript:void(0);" class="locked unlocked">
                        <%//= Html.Term("Unlocked")%></a> <a href="javascript:void(0);" id="btnUpload">
                            <%= Html.Term("Upload", "Upload") %></a> | <a href="javascript:void(0);" id="btnDeleteImages">
                                <%= Html.Term("DeleteSelected", "Delete Selected") %></a></span>
            </h3>
            <div id="imagesContainer">
                <% if (Model.Files.ContainsProductFileTypeID(Constants.ProductFileType.Image.ToInt()))
                   {
                       foreach (ProductFile image in Model.Files.GetByProductFileTypeID(Constants.ProductFileType.Image.ToInt()).OrderBy(pf => pf.SortIndex))
                       { %>
                <p id="productFile<%= image.ProductFileID %>" class="ImageThumbWrap">
                    <span class="ImgTools"><a href="javascript:void(0);" class="moveUp" <%= image.SortIndex == 0 ? "style=\"display:none;\"" : "" %>>
                        <span class="icon-ArrowUp"></span></a><br />
                        <a href="javascript:void(0);" class="moveDown" <%= image.SortIndex == Model.Files.GetByProductFileTypeID(Constants.ProductFileType.Image.ToInt()).Count - 1 ? "style=\"display:none;\"" : "" %>>
                            <span class="UI-icon icon-ArrowDown"></span></a><br />
                    </span>
                    <img width="100" src="<%= image.FilePath.ReplaceFileUploadPathToken() %>" alt="" />
                    <input type="checkbox" class="checkbox" name="" />
                    <span class="ClearAll"></span>
                </p>
                <%}
                   } %>
            </div>
            <p class="FormButtons">
                <a href="javascript:void(0);" class="Button BigBlue" id="btnSaveImages" <%=Model.IsVariant ? "disabled='disabled'" : "" %>
                    <%= Model.Files.ContainsProductFileTypeID(Constants.ProductFileType.Image.ToInt()) ? "" : "style=\"display:none;\"" %>>
                    <%= Html.Term("SaveImages", "Save Images") %></a>
            </p>
        </td>
    </tr>
</table>
