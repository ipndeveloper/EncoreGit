<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Communication.Models.EmailTemplateModel>" %>
<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Resource/Content/CSS/EditMode.css") %>" />
<script type="text/javascript">
	$(function () {
	
		$('#mediaLibrary').jqm({
			modal: false,
			trigger: '.MediaLibrary',
			onShow: function(){ $('#mediaLibrary').fadeIn('fast').trigger('resizeHeaders');}
		});

		$('#mediaLibrary .InsertFile').live('click', function () {
			
			var path = $(this).parent().parent().find('.absolutePath').val();
			CKEDITOR.instances.body.insertHtml('<img alt=\x22\x22 src=\x22' + path + '\x22 />');

		});

		if ($('#btnDeleteImage').length > 0 && $('#imageUpload').length > 0 && $('#imagePreview').length > 0 && $('#imageThumbnail').length > 0) {


			var uploader = new qq.FileUploader({
				element: document.getElementById('imageUpload'),
				action: '<%= ResolveUrl("~/Communication/EmailTemplates/UploadCampaignImage") %>',
				allowedExtensions: ['jpg', 'jpeg', 'png', 'gif', 'tif', 'tiff', 'bmp'],
				messages: {
					typeError: '<%= Html.Term("InvalidExtension", "{0} has invalid extension. Only {1} are allowed.", "{file}", "{extensions}")  %>',
					sizeError: '<%= Html.Term("FileTooLarge", "{0} is too large, maximum file size is {1}.", "{file}", "{sizeLimit}") %>',
					minSizeError: '<%= Html.Term("FileTooSmall", "{0} is too small, minimum file size is {1}.", "{file}", "{minSizeLimit}") %>',
					emptyError: '<%= Html.Term("EmptyFile", "{0} is empty, please select files again without it.", "{file}") %>',
					onLeave: '<%= Html.Term("FilesStillUploading", "The files are being uploaded, if you leave now the upload will be canceled.") %>'
				},
				template: '<div class="qq-uploader">' +
					'<div class="qq-upload-drop-area"><span><%= Html.Term("DropFilesHereToUpload", "Drop files here to upload") %></span></div>' +
					'<div class="qq-upload-button"><%= Html.Term("Upload") %></div>' +
					'<div class="qq-max-file-size"><%= Html.Term("MaxFileSize", "Max file size") %>: <%= NetSteps.Common.Configuration.ConfigurationManager.MaxFileSize %></div>' +
					'<ul id="qqUploadList" class="qq-upload-list"></ul>' +
				 '</div>',
				fileTemplate: '<li>' +
					'<span class="qq-upload-file"></span>' +
					'<span class="qq-upload-spinner"></span>' +
					'<span class="qq-upload-size"></span>' +
					'<a class="qq-upload-cancel" href="javascript:void(0);"><%= Html.Term("Cancel") %></a>' +
					'<span class="qq-upload-failed-text"><%= Html.Term("Failed") %></span>' +
				'</li>',
				onComplete: function (id, fileName, response) {
					if (!response.success) {
						showMessage(response.error, true);
						return;
					}
					$('#imageUpload').hide();
					$('#imagePreview').show();
					$('#imageThumbnail').attr('src', response.filePath);

				}
			});

			$('#btnDeleteImage').click(function () {
				$('#imageUpload').show();
				$('#imagePreview').hide();
				$('#imageThumbnail').attr('src', '');
			});
		}

		//btnCloseSendTestEmail

		$('#btnCloseSendTestEmail').click(function () {

			$('#divSendTestEmail').hide();
		});

		$('#btnSendTestEmail').click(function () {

			$('#divSendTestEmail').show();
		});

		$('#btnSendEmail').click(function () {
			var data, newWindow;
			var isComplete = true;

			if (!$('#divSendTestEmail').checkRequiredFields()) {
				isComplete = false;
			}

			if (!$('#templateForm').checkRequiredFields()) {
				isComplete = false;
			}

			var testEamilAddress = $('#testEmailAddress');
			var regexEmailValidaitonString = <%= Html.GetEmailValidaionRegex() %>;

			if (!regexEmailValidaitonString.test(testEamilAddress.val())) {
				testEamilAddress.showError('<%= Html.JavascriptTerm("InvalidEmail", "Please enter a valid email address.") %>');
				isComplete = false;
			}
			else {
				testEamilAddress.clearError();
			}

			var fromAddressEmail = $('#fromAddressEmail');

			if (!regexEmailValidaitonString.test(fromAddressEmail.val())) {
				fromAddressEmail.showError('<%= Html.JavascriptTerm("InvalidEmail", "Please enter a valid email address.") %>');
				isComplete = false;
			}
			else {
				fromAddressEmail.clearError();
			}

			if (isComplete) {

				//data to be passed to server
				data = {
					Subject: $('#subject').val(),
					Body: $('#bodyHtml').is(':visible') ? $('#bodyHtml').val() : $('#body').val(),
					CampaignActionID: '<%= Model.CampaignActionID %>',
					LanguageID: $('#LanguageID').val(),
					DefaultDistributorContent: $('#campaignActionPlaceholder').val(),
					DefaultDistributorImageUrl: $('#imageThumbnail').attr('src'),
					EmailTemplateTypeID: '<%= Model.EmailTemplate.EmailTemplateTypeID %>',
					To: $('#testEmailAddress').val()
				};

				$.post('/Communication/EmailTemplates/SendTestEmail', data, function (response) {
					if (response.success) {
						showMessage(response.message, !response.success);

					}
					else {
						showMessage(response.error, true);
					}
				});
			}
			else {
				return isComplete;
			}
		});

		$('#btnPreview, #bodyPreview').click(function () {
			var data, newWindow;

			//data to be passed to server
			data = {
				Subject: $('#subject').val(),
				Body: $('#bodyHtml').is(':visible') ? $('#bodyHtml').val() : $('#body').val(),
				CampaignActionID: '<%= Model.CampaignActionID %>',
				LanguageID: $('#LanguageID').val(),
				DefaultDistributorContent: $('#campaignActionPlaceholder').val(),
				DefaultDistributorImageUrl: $('#imageThumbnail').attr('src'),
				EmailTemplateTypeID: '<%= Model.EmailTemplate.EmailTemplateTypeID %>'
			};

			newWindow = window.open('', '');
			$.post('/Communication/EmailTemplates/SetupPreview', data, function (response) {
				if (response.success) {
					newWindow.location = '/Communication/EmailTemplates/Preview';
				}
				else {
					showMessage(response.error, true);
				}
			});
		});
	});

</script>
<!-- EmailTemplate save window -->
<div id="emailTemplateForm" style="display: none;">
	<h3>
		Edit Email Template</h3>
	<input type="hidden" id="emailTemplateTranslationID" value="" />
	<%: Html.Hidden("languageCount", Model.LanguageCount)%>
	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("Language") %>
			</td>
			<td>
				<%: Html.DropDownList("LanguageID", new SelectList(Model.Languages as System.Collections.IEnumerable, "Key", "Value", "")) %>
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("FromAddress", "From Address") %>
			</td>
			<td>
				<input id="fromAddressEmail" type="text" value="" class="required fullWidth pad5" />
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("TestEmailAddress", "Test Email Address")%>:
			</td>
			<td>
				<input id="testEmailAddress" type="text" value="" class="required fullWidth pad5" />
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("EmailSubject", "Email Subject")%>:
			</td>
			<td>
				<input id="subject" type="text" value="" class="required fullWidth pad5" />
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("Thumbnail")%>:
			</td>
			<td class="qq-upload-container">
				<div id="thumbnailPreview" style="display: none">
					<img id="thumbnail" class="ClearAll" alt="" src="" />
					<a href="javascript:void(0);" id="btnDeleteThumbnail" title="<%= Html.Term("DeleteThumbnail", "Delete Thumbnail") %>" class="DTL Delete"></a>
				</div>
				<div id="thumbnailUpload">
				</div>
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("Template")%>:
			</td>
			<td>
				<% 
					if (Model.Tokens != null && Model.Tokens.Count > 0)
					{ 
				%>
				<div class="TokenList">
					<div class="FL">
						<b>
							<%=Html.Term("Tokens")%>:</b>
						<select id="tokenList">
							<% 
						foreach (var token in Model.Tokens)
						{ 
							%>
							<option value="<%: NetSteps.Common.TokenReplacement.TokenReplacer.GetDelimitedTokenizedString(token.Placeholder) %>">
								<%: token.Name %>
							</option>
							<% 
						} 
							%>
						</select>
					</div>
					<a id="btnAddToken" href="javascript:void(0);" class="FL DTL Add"><span>
						<%= Html.Term("AddToken", "Add Token")%></span></a> <span class="clr"></span>
				</div>
				<% 
					}

					Html.RenderPartial("MediaLibrary", new MediaLibraryModel()
					{
						SystemBaseUrl = NetSteps.Common.Configuration.ConfigurationManager.GetAbsoluteFolder("CMS"),
						WebBaseUrl = NetSteps.Common.Configuration.ConfigurationManager.GetWebFolder("CMS"),
						AllowImageInsert = true
					});

					TextEditorModel textEditorModel = new TextEditorModel();

					textEditorModel.IsRichText = true;
					textEditorModel.InstanceName = "body";
					textEditorModel.ShowTabbedHeader = true;
					textEditorModel.ShowPreviewLink = true;
					textEditorModel.ShowMediaLibraryLink = true;
					textEditorModel.ContentBody = Model.EmailTemplate.Body;

					Html.RenderPartial("TextEditor", textEditorModel);       
		

				%>
			</td>
		</tr>
	</table>
	<%-- 6/1/2011: Tenzin  -- If email template type is of NewsLetter then display the following 2 rows --%>
	<% 
		if (Model.IsNewsLetterType)
		{
	%>
	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("DefaultDistributorImage", "Default Distributor Image")%>:
			</td>
			<td class="qq-upload-container">
				<div id="imagePreview" style="display: none">
					<img id="imageThumbnail" class="ClearAll" alt="" src="" style="border: 1px solid black; height: 100px;" />
					<a href="javascript:void(0);" id="btnDeleteImage">
						<%= Html.Term("DeleteThumbnail", "Delete Thumbnail") %></a>
				</div>
				<div id="imageUpload">
				</div>
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("DefaultDistributorContent", "Default Distributor Content")%>:
			</td>
			<td>
				<textarea id="campaignActionPlaceholder" class="required fullWidth emailDistributorContent"></textarea>
			</td>
		</tr>
	</table>
	<%
		} 
	%>
	<%-- 6/1/2011: End --%>
	<table class="FormTable" width="100%">
		<tr>
			<td class="FLabel">
				&nbsp;
			</td>
			<td>
				<div class="mt10">
					<a id="btnSave" href="javascript:void(0);" class="Button BigBlue"><span>
						<%= Html.Term("SaveEmailTemplate","Save Template")%></span></a> <a id="btnPreview" href="javascript:void(0);" class="Button"><span>
							<%= Html.Term("Preview")%></span></a> <a id="btnSendEmail" href="javascript:void(0);" class="Button"><span>
								<%= Html.Term("SendTestEmail", "Send a Test Email")%></span></a>
					<%--<a id="btnSendTestEmail" href="javascript:void(0);"
							class="Button BigBlue"><span>
								<%= Html.Term("SendTestEmail", "Send Test Email")%></span></a>  --%>
					<a href="javascript:void(0);" class="Button jqmClose" id="btnCancel">
						<%= Html.Term("Cancel")%></a>
				</div>
			</td>
		</tr>
	</table>
	<div id="divSendTestEmail" style="display: none;">
		<table>
			<tr>
				<td colspan="2">
					<a href="javascript:void(0);" class="Button jqmClose" id="btnCloseSendTestEmail">
						<%= Html.Term("Cancel")%></a>
				</td>
			</tr>
		</table>
	</div>
</div>
