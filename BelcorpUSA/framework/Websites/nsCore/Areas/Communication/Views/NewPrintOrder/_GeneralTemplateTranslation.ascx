<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Communication.Models.GeneralTemplateModel>" %>

<link rel="stylesheet" type="text/css" href="<%= Url.Content("~/Resource/Content/CSS/EditMode.css") %>" />

<script type="text/javascript">

</script>

<div id="generalTemplateForm" style="display: none;">
	<h3>
		Edit General Template</h3>
	<input type="hidden" id="generalTemplateTranslationID" value="" />
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
				<%= Html.Term("Active") %>
			</td>
			<td>
				<input id="activeGTT" type="checkbox"/>
			</td>
		</tr>
		
		<tr>
			<td class="FLabel">
				<%= Html.Term("Template")%>:
			</td>
			<td>
				<% 
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
                    //textEditorModel.ContentBody = Model.NewPrintOrderSearchData.objGeneralTemplateTranslations.Body;

					Html.RenderPartial("TextEditor", textEditorModel);       

				%>
			</td>
		</tr>
	</table>
	<%-- 6/1/2011: End --%>
	<table class="FormTable" width="100%">
		<tr>
			<td class="FLabel">
				&nbsp;
			</td>
			<td>
				<div class="mt10">
					<a id="btnSave" href="javascript:void(0);" class="Button BigBlue"><span>
						<%= Html.Term("SaveEmailTemplate","Save Template")%></span>
                    </a> 
					<a href="javascript:void(0);" class="Button jqmClose" id="btnCancel">
						<%= Html.Term("Cancel")%>
                    </a>
				</div>
			</td>
		</tr>
	</table>
</div>
