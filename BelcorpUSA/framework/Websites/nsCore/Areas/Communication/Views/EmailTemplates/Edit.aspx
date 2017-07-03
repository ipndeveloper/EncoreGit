<%@ Page Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Communication.Models.EmailTemplateModel>" %>

<%--  Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.EmailTemplate>" --%>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/fileuploader.css") %>" />
<%--<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/adapters/jquery.js"></script>--%>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/adapters/jquery.js"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/fileuploader.js") %>"></script>
	<script type="text/javascript">
		$(function () {



			$('.btnEditTemplate').click(function () {

				var id = $(this).attr("id");
				var campaignActionIDValue = 1;
				if ($('#campaignActionID').length > 0) {
					campaignActionIDValue = $('#campaignActionID').val();
				}
				$.getJSON('<%= ResolveUrl("~/Communication/EmailTemplates/GetEmailTemplateTranslation") %>',
						{ id: $(this).attr("id"), campaignActionID: $('#campaignActionID').val() },
						function (data) {
							$('#body').val(data.body);
							$('#subject').val(data.subject);
							$('#fromAddressEmail').val(data.from);
							$('#campaignActionPlaceholder').val(data.distributorContent);
							if (data.containAttachPath) {
								$('#thumbnailUpload').hide();
								$('#thumbnailPreview').show();
								$('#thumbnail').attr('src', data.attachPath);
							} else {
								$('#thumbnailUpload').show();
								$('#thumbnailPreview').hide();
								$('#thumbnail').attr('src', '');
							}

							/* Display image for Newsletter template type, if any */
							if (data.containImage) {
								$('#imageUpload').hide();
								$('#imagePreview').show();
								$('#imageThumbnail').attr('src', data.distributorImage);
							} else {
								$('#imageUpload').show();
								$('#imagePreview').hide();
								$('#imageThumbnail').attr('src', '');
							}
						});


				$('#emailTemplateTranslationID').val(id);

				$.getJSON('<%= ResolveUrl("~/Communication/EmailTemplates/AvailableLanguages") %>',
					{ id: $('#emailTemplateId').val(), emailTemplateTranslationID: $(this).attr("id") },
					function (data) {
						$('#LanguageID >option').remove();
						$(data.languages).each(function () {
							$("<option>").val(this.ID)
										 .text(this.Name)
										 .appendTo('#LanguageID');

							$('#LanguageID').val(data.selected);

						});
						$('#emailTemplateForm').show();
					});
			});

			$('#btnCancel').click(function () {
				$('#emailTemplateForm').hide();
			});

			$('#btnAdd').click(function () {
				var id = $('#emailTemplateId').val();
				var langCount = $('#languageCount').val();
				if (langCount > 0) {
					if (parseInt(id)) {

						$('#body').val('');
						$('#subject').val('');
						$('#fromAddressEmail').val('');
						$('#emailTemplateTranslationID').val('');

						$.getJSON('<%= ResolveUrl("~/Communication/EmailTemplates/AllLanguages") %>',
							{ id: $('#emailTemplateId').val() },
							function (data) {
								$('#LanguageID >option').remove();
								$(data).each(function () {
									$("<option>").val(this.ID)
													.text(this.Name)
													.appendTo('#LanguageID');
								});
								$('#emailTemplateForm').show();
							});
					} else {
						alert('<%= Html.Term("SaveEmailTemplateFirst", "You must save a new email template first.") %>');
					}
				} else {
					/* No more languages */
					alert('<%= Html.Term("AllLanguagesAreTaken", "All of the available languages have been selected.") %>');
				}

				/* Reset the image upload sections */
				$('#imageUpload').show();
				$('#imagePreview').hide();
				$('#imageThumbnail').attr('src', '');
				$('#thumbnailUpload').show();
				$('#thumbnailPreview').hide();
				$('#thumbnail').attr('src', '');
				$('#campaignActionPlaceholder').val('');
			});

			$('#btnCancelTemplate').click(function () {
				window.location = '<%=Model.CancelURL %>';
			});

			$('#btnSaveTemplate').click(function () {

				var isComplete = true;

				if (!$('#templateForm').checkRequiredFields()) {
					isComplete = false;
				}

				if (isComplete) {
				
					var t = $(this);
					showLoading(t);

					$.post('<%= ResolveUrl("~/Communication/EmailTemplates/Save") %>',
					{
						emailTemplateID: $('#emailTemplateId').val(),
						Name: $('#name').val(),
						EmailTemplateTypeID: $('#type').val(),
						Active: $('#active').prop('checked'),
						campaignID: $('#campaignID').val(),
						campaignActionID: $('#campaignActionID').val(),
						campaignTypeID: $('#campaignTypeID').val()
					},
					function (response) {
						hideLoading(t);
						showMessage(response.message || '<%= Html.Term("SavedSuccessfully", "Saved successfully!") %>', !response.result);
						if (response.result) {
							$('#emailTemplateId').val(response.emailTemplateID);

							if (response.redirectUrl)
								window.location = response.redirectUrl;

							// Get the Token values for the selected email template
							$.getJSON('<%= ResolveUrl("~/Communication/EmailTemplates/GetTokens") %>',
								{ id: response.emailTemplateTypeID },
								function (data) {
									$('#tokenList >option').remove();
									$(data).each(function () {
										$("<option>").val(this.ID)
														.text(this.Name)
														.appendTo('#tokenList');

									});
								});
						}
					});

				}
				else {
					return isComplete;
				}
			});

			$('input.numeric').numeric();
			$('#btnSave').click(function () {
				if (!$('#emailTemplateForm').checkRequiredFields()) {
					return false;
				}
				
				var regexEmailValidaitonString = <%= Html.GetEmailValidaionRegex() %>;

				/** 6/8/2011: Tenzin - Email validation: DWS - Areas/Account/Views/Edit/Index.cshtml **/
				var isValid = true;
				if (!regexEmailValidaitonString.test($('#fromAddressEmail').val())) {
					$('#fromAddressEmail').showError('<%= Html.JavascriptTerm("InvalidEmail", "Please enter a valid email address.") %>');
					isValid = false;
				}
				else {
					$('#fromAddressEmail').clearError();
				}

				if (isValid) {

					var t = $(this);
					showLoading(t);
					$.post('<%= ResolveUrl("~/Communication/EmailTemplates/SaveEmailTemplateTranslation") %>', {
						emailTemplateTranslationID: $('#emailTemplateTranslationID').val(),
						emailTemplateId: $('#emailTemplateId').val(),
						languageID: $('#LanguageID').val(),
						subject: $('#subject').val(),
						body: $('#bodyHtml').is(':visible') ? $('#bodyHtml').val() : $('#body').val(),
						fromAddress: $('#fromAddressEmail').val(),
						attachmentPath: $('#thumbnail').attr('src'),
						campaignActionPlaceholder: $('#campaignActionPlaceholder').val(),
						imagePath: $('#imageThumbnail').attr('src'),
						campaignActionID: $('#campaignActionID').val()
					}, function (response) {
						hideLoading(t);
						showMessage(response.message, !response.result);
						if (response.result) {
							$('#emailTemplateId').val(response.emailTemplateId);
							$('#campaignActionID').val(response.campaignActionID);
							$('#emailTemplateForm').hide();
							if (response.templateOnly) {
								window.location = "/Communication/EmailTemplates/Edit/" + response.emailTemplateID;
							} else {
								window.location = "/Communication/EmailTemplates/Edit/" + response.emailTemplateID +
																			"?campaignID=" + $('#campaignID').val() +
																			 "&campaignActionID=" + response.campaignActionID +
																			 "&campaignTypeID=" + $('#campaignTypeID').val();
							}
						}
					});
				}
			});

			$('#btnAddToken').click(function () {
				CKEDITOR.instances.body.insertHtml($('#tokenList').val());
			});

			var uploader = new qq.FileUploader({
				element: document.getElementById('thumbnailUpload'),
				action: '<%= ResolveUrl("~/Communication/EmailTemplates/UploadThumbnail") %>',
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
					$('#thumbnailUpload').hide();
					$('#thumbnailPreview').show();
					$('#thumbnail').attr('src', response.filePath);

				}
			});

			$('#btnDeleteThumbnail').click(function () {
				$('#thumbnailUpload').show();
				$('#thumbnailPreview').hide();
				$('#thumbnail').attr('src', '');
			});

			$('.Remove').click(function () {
				return confirm('<%: Html.Term("AreYouSureYouWishToDeleteThisItem", "Are you sure you wish to delete this item?") %>');
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<%  
		if (!Model.CampaignID.HasValue || Model.CampaignID == 0)
		{
	%>
	<a href="<%= ResolveUrl("~/Communication/EmailTemplates") %>">
		<%= Html.Term("EmailTemplates", "Email Templates") %></a> >
	<%= Model.EmailTemplate.EmailTemplateID == 0 ? Html.Term("NewEmailTemplate", "New Email Template") : Html.Term("EditEmailTemplate", "Edit Email Template") %>
	<%
		}
	%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<%=Html.Hidden("campaignID", Model.CampaignID) %>
	<%=Html.Hidden("campaignActionID", Model.CampaignActionID) %>
	<%=Html.Hidden("campaignTypeID", Model.CampaignTypeID) %>
	<div class="SectionHeader">
		<h2>
			<%= Model.EmailTemplate.EmailTemplateID == 0 ? Html.Term("NewEmailTemplate", "New Email Template") : Html.Term("EditEmailTemplate", "Edit EmailTemplate") %></h2>
	</div>
	<!-- EmailTemplate: Name, Language, Type, Active section -->
	<div id="templateForm" class="splitCol formStyle1">
		<div class="FRow mb10">
			<label class="bold">
				<%= Html.Term("Name")%>:</label>
			<div class="FInput">
				<input type="hidden" id="emailTemplateId" value="<%= Model.EmailTemplate.EmailTemplateID == 0 ? "" : Model.EmailTemplate.EmailTemplateID.ToString() %>" />
				<input id="name" type="text" class="required fullWidth pad5" name="Email name is required." value="<%= Model.EmailTemplate.Name %>" />
			</div>
		</div>
		<div class="FRow mb10">
			<label class="bold">
				<%= Html.Term("Type")%>:</label>
			<div class="FInput">
				<select id="type" <%= Model.IsNewEmailTemplate ? "" : "disabled=\"disabled\"" %>>
					<% 
						var unsupportedEmailTemplateType = new List<short>();

						if (Model.IsNewEmailTemplate)
						{
							unsupportedEmailTemplateType.Add((short)Constants.EmailTemplateType.Autoresponder);
							unsupportedEmailTemplateType.Add((short)Constants.EmailTemplateType.Campaign);
							unsupportedEmailTemplateType.Add((short)Constants.EmailTemplateType.Newsletter);    
							//TODO: Remove the following 2 for Phase2 release.
							unsupportedEmailTemplateType.Add((short)Constants.EmailTemplateType.SupportTicketInfoRequestAutoresponder);
							unsupportedEmailTemplateType.Add((short)Constants.EmailTemplateType.AutoshipCreditCardFailed);
						}

						foreach (var emailTemplateType in SmallCollectionCache.Instance.EmailTemplateTypes.Where(ett => !unsupportedEmailTemplateType.Contains(ett.EmailTemplateTypeID)))
						{ %>
					<option value="<%= emailTemplateType.EmailTemplateTypeID %>" <%= emailTemplateType.EmailTemplateTypeID == Model.EmailTemplate.EmailTemplateTypeID ? "selected=\"selected\"" : "" %>>
						<%= emailTemplateType.GetTerm() %></option>
					<%} %>
				</select>
			</div>
		</div>
		<div class="FRow mb10">
			<label class="bold" for="active">
				<%= Html.Term("Active")%></label>
			<input id="active" type="checkbox" <%= (Model.EmailTemplate.Active != null && Model.EmailTemplate.Active.Value) ? "checked=\"checked\"" : "" %> />
		</div>
		<div class="FRow">
			<p>
				<a id="btnSaveTemplate" href="javascript:void(0);" class="Button BigBlue"><span>
					<%= Html.Term("Save")%></span></a>
				<%
					if (Model.CampaignID.HasValue && Model.CampaignID > 0)
					{ 
				%>
				<a id="btnCancelTemplate" href="javascript:void(0);" class="Button"><span>
					<%= Html.Term("Cancel")%></span></a>
				<%
				}
				%>
			</p>
		</div>
	</div>
	<hr />
	<!-- Displays the Email Templates if any -->
	<div class="FLcolWrapper mt10">
		<div class="FL splitCol30">
			<h3 class="mb10">
				<%= Html.Term("EmailTemplateTranslations", "Email Template Translations")%>
				<a style="cursor: pointer" id="btnAdd" class="FR DTL Add">
					<%= Html.Term("Add", "Add")%></a></h3>
			<div class="emailTranslationsWrapper">
				<% 
					foreach (var emailTemplate in Model.EmailTemplate.EmailTemplateTranslations)
					{ 
				%>
				<div class="UI-lightBg brdrAll mb10 emailTranslation">
					<div class="inner pad5">
						<div class="brdrAll pad5 UI-secBg">
						   <%: Html.ActionLink("Delete", "DeleteTemplateTranslation", new { id = emailTemplate.EmailTemplateTranslationID, 
																campaignActionID = Model.CampaignActionID,
																campaignID = Model.CampaignID,
																campaignTypeID = Model.CampaignTypeID}, new { @class = "DTL FL Remove" })%>
							<a title="<%= Html.Term("Edit") %> <%= Language.Load(emailTemplate.LanguageID).TermName %>" class="FR DTL EditSite mr10 btnEditTemplate" id="<%: emailTemplate.EmailTemplateTranslationID %>" href="javascript:void(0);">Edit</a>
							<span class="clr"></span>
						</div>
						<ul class="infoList">
							<li>
							<span class="label"><%= Html.Term("Language") %>: </span>
							<span class="data"><%:Language.Load(emailTemplate.LanguageID).TermName%></span>
							<span class="clr"></span>
							</li>
							<li> 
							<span class="label">
							<%: Html.Hidden("currentLanguageID", emailTemplate.LanguageID) %>
							<%= Html.Term("Subject", "Subject") %>:
							</span>
							<span class="data">
							<label id="templateSubject<%: emailTemplate.EmailTemplateTranslationID %>">
							<%: string.IsNullOrEmpty(emailTemplate.Subject) ? Html.Term("Unnamed") : emailTemplate.Subject%></label>
							</span>
							<span class="clr"></span>
						   </li>
						   <li>
						   <span class="label">
						   <%= Html.Term("From") %>:
						   </span>
						   <span class="data">
						   <label id="fromAddress<%: emailTemplate.EmailTemplateTranslationID %>">
						   <%: Html.Raw(emailTemplate.FromAddress ?? string.Empty )%></label>
						   </span>
						   <span class="clr"></span>
						   </li> 
						</ul>
					</div>
				</div>
				<%
						}      
				%>
			</div>
		</div>
		<div class="FR splitCol70">
			<% Html.RenderPartial("_EmailTemplateTranslation", Model); %>
		</div>
		<span class="clr"></span>
	</div>
</asp:Content>
