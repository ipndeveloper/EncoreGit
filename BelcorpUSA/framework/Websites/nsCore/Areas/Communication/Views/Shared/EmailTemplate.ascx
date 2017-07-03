<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.EmailTemplate>" %>

<script type="text/javascript">
    $(function () {
        var uploader = new qq.FileUploader({
            element: document.getElementById('attachmentUpload'),
            action: '<%= ResolveUrl("~/Communication") + "/" + ViewContext.RouteData.Values["controller"] + "/UploadAttachment" %>',
            messages: {
                typeError: '<%= Html.Term("InvalidExtension", "{0} has invalid extension. Only {1} are allowed.", "{file}", "{extensions}") %>',
                sizeError: '<%= Html.Term("FileTooLarge", "{0} is too large, maximum file size is {1}.", "{file}", "{sizeLimit}") %>',
                minSizeError: '<%= Html.Term("FileTooSmall", "{0} is too small, minimum file size is {1}.", "{file}", "{minSizeLimit}") %>',
                emptyError: '<%= Html.Term("EmptyFile", "{0} is empty, please select files again without it.", "{file}") %>',
                onLeave: '<%= Html.Term("FilesStillUploading", "The files are being uploaded, if you leave now the upload will be canceled.") %>'
            },
            template: '<div class="qq-uploader">' +
					'<div class="qq-upload-drop-area"><span><%= Html.Term("DropFilesHereToUpload", "Drop files here to upload") %></span></div>' +
					'<div class="qq-upload-button"><%= Html.Term("UploadAFile", "Upload a file") %></div>' +
					'<ul class="qq-upload-list"></ul>' +
				 '</div>',
            fileTemplate: '<li>' +
					'<span class="qq-upload-file"></span>' +
					'<span class="qq-upload-spinner"></span>' +
					'<span class="qq-upload-size"></span>' +
					'<a class="qq-upload-cancel" href="javascript:void(0);"><%= Html.Term("Cancel") %></a>' +
					'<span class="qq-upload-failed-text"><%= Html.Term("Failed") %></span>' +
				'</li>',
            onComplete: function (id, fileName, response) {
                $('#attachmentPathContainer').show();
                $('#attachmentPath').text(fileName);
                $('#attachmentUploadContainer').hide();
            }
        });
    });
</script>

<table width="100%" class="FormTable">
    <tr>
        <td class="FLabel">
            <%= Html.Term("EmailName", "Email Name") %>:
        </td>
        <td>
            <input type="hidden" name="emailTemplateId" value="<%= Model.EmailTemplateID == 0 ? "" : Model.EmailTemplateID.ToString() %>" />
            <input id="emailName" type="text" class="required" name="Email name is required." value="<%= Model.Name %>" style="width:300px;" />
        </td>
    </tr>
    <%--<tr>
        <td class="FLabel">
            <%= Html.Term("Active") %>:
        </td>
        <td>
            <input id="active" type="checkbox" <%= Model.Active ? "checked=\"checked\"" : "" %> />
        </td>
    </tr>--%>
    <tr>
        <td class="FLabel">
            <%= Html.Term("FromAddress", "From Address") %>:
        </td>
        <td>
            <input id="fromAddress" type="text" class="required" name="From address is required." value="<%= Model.FromAddress %>" style="width: 300px;" />
        </td>
    </tr>
    <tr>
        <td class="FLabel">
            <%= Html.Term("EmailSubject", "Email Subject") %>:
        </td>
        <td>
            <input id="subject" name="subject" type="text" value="<%= Model.Subject %>" style="width: 100%;" />
        </td>
    </tr>
    <tr>
        <td class="FLabel">
            <%= Html.Term("Attachment") %>:
        </td>
        <td>
            <div id="attachmentPathContainer" <%= string.IsNullOrEmpty(Model.AttachmentPath) ? "style=\"display:none;\"" : "" %>>
                <span id="attachmentPath">
                    <%= !string.IsNullOrEmpty(Model.AttachmentPath) ? System.IO.Path.GetFileName(Model.AttachmentPath.ReplaceFileUploadPathToken().WebUploadPathToAbsoluteUploadPath()) : "" %></span>
                <a href="javascript:void(0);" id="btnDeleteAttachment" class="IconLink Delete" style="display:inline;">&nbsp;&nbsp;&nbsp;</a>
            </div>
            <div id="attachmentUploadContainer" <%= string.IsNullOrEmpty(Model.AttachmentPath) ? "" : "style=\"display:none;\"" %>>
                <div id="attachmentUpload">
                </div>
                <span class="LawyerText">max
                    <%= NetSteps.Common.Configuration.ConfigurationManager.MaxFileSize %></span>
            </div>
        </td>
    </tr>
    <tr>
        <td class="FLabel">
            <%= Html.Term("Template") %>:
        </td>
        <td>
            <textarea id="body" name="body" rows="10" cols="30" style="width: 100%; height: 400px;"><%= Model.Body %></textarea>
            <% Html.RenderPartial("TokenList"); %>
        </td>
    </tr>
</table>
