<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Sites.Models.EditDocumentModel>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/MediaLibrary.css") %>" />
    <script type="text/javascript">
    	$(function () {
    		var target;

    		$('#mediaLibrary').jqm({
    			modal: false,
    			trigger: 'a.MediaLibrary',
    			onShow: function () { $('#mediaLibrary').fadeIn('fast').trigger('resizeHeaders'); }
    		});

    		$('#resourceProperties').setupRequiredFields();

    		$('#btnBrowseServer').click(function () {
    			target = $('#filePath');
    			$('#folderBrowser li:gt(0)').show();
    		});
    		$('#btnBrowseServerThumbnails').click(function () {
    			target = $('#thumbnail');
    			$('#folderBrowser li:first').click();
    			$('#folderBrowser li:gt(0)').hide();
    		});

    		$('#mediaLibrary a.SelectFile').live('click', function () {
    			target.val($(this).closest('.NS-fileInfo').find('.emFileURL').val());
    			target.keyup(); // Fire event to initiate error checking
    			$('#mediaLibrary').jqmHide();
    		});

    		$('#btnSave').click(function () {
    			if (!$('#resourceProperties').checkRequiredFields()) {
    				return false;
    			}

    			var data = {
    				archiveId: $('#archiveId').val(),
    				languageId: $('#languageId').val(),
    				name: $('#name').val(),
    				description: $('#description').val(),
    				filePath: $('#filePath').val(),
    				thumbnail: $('#thumbnail').val(),
    				startDate: $('#startDate').val(),
    				endDate: $('#endDate').val(),
    				active: $('#active').prop('checked'),
    				isDownloadable: $('#isDownloadable').prop('checked'),
    				canBeEmailed: $('#canBeEmailed').prop('checked'),
    				isFeatured: $('#isFeatured').prop('checked')
    			};

    			$('#categories .category:checked').each(function (i) {
    				data['categories[' + i + ']'] = $(this).val();
    			});

    			$.post('<%= ResolveUrl("~/Sites/Documents/Save") %>', data, function (response) {
    				if (response.result) {
    					showMessage(response.message || '<%= Html.Term("DocumentSavedSuccessfully", "Document saved successfully!") %>', !response.result);
    					$('#archiveId').val(response.archiveId);
    				} else {
    					showMessage(response.message, true);
    				}
    			});
    		});

    		$('#languageId').change(function () {
    			$.get('<%= ResolveUrl("~/Sites/Documents/GetTranslation") %>', { archiveId: $('#archiveId').val(), languageId: $(this).val() }, function (response) {
    				if (response.result) {
    					$('#name').val(response.name);
    					$('#description').val(response.description);
    				} else {
    					showMessage(response.message, true);
    				}
    			});
    		});
    	});
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Sites") %>">
        <%= Html.Term("Sites") %></a> > <a href="<%= ResolveUrl("~/Sites/Documents") %>">
            <%= Html.Term("DocumentLibrary", "Document Library") %></a> >
    <%= Html.Term("Add/EditDocument", "Add/Edit Document") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% Archive archive = Model.Archive; %> F
    <form action="<%= ResolveUrl("~/Sites/Documents/Save") %>" method="post">
    <div class="SectionHeader">
        <h2>
            <%= archive.ArchiveID == 0 ? Html.Term("NewDocument", "New Document") : Html.Term("EditDocument", "Edit Document")%></h2>
        <a href="<%= ResolveUrl("~/Sites/Documents") %>">
            <%= Html.Term("DocumentLibrary", "Document Library") %></a> |
        <%if (archive.ArchiveID > 0)
          { %><a href="<%= ResolveUrl("~/Sites/Documents/Edit") %>"><%} %><%= Html.Term("AddDocument", "Add Document") %><%if (archive.ArchiveID > 0)
                                                                                                                           { %></a><%} %>
        | <a href="<%= ResolveUrl("~/Sites/Documents/Categories") %>">
            <%= Html.Term("ManageDocumentCategories", "Manage Document Categories")%></a>
        <br />
        <%= Html.Term("Language") %>:
        <%= Html.DropDownLanguages(htmlAttributes: new { id = "languageId" }, selectedLanguageID: (archive.Translations.Count > 0 ? archive.Translations.FirstOrDefault().LanguageID : CoreContext.CurrentLanguageID))%>
    </div>
    <%if (TempData["Error"] != null)
      { %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
        -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
        border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
        margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;">
            <img alt="" src="<%= ResolveUrl("~/Content/Images/exclamation.png") %>" />&nbsp;<%= TempData["Error"] %></div>
    </div>
    <%} %>
    <input type="hidden" id="archiveId" value="<%= archive.ArchiveID == 0 ? "" : archive.ArchiveID.ToString() %>" />
    <table id="resourceProperties" width="100%" cellspacing="0" class="DataGrid">
        <tr>
            <td style="width: 13.636em; vertical-align: top;">
                <%= Html.Term("Name") %>
            </td>
            <td>
                <input id="name" name="Name is required." type="text" class="required" value="<%= archive.Translations.Name() %>"
                    style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top;">
                <%= Html.Term("Description") %>
            </td>
            <td>
                <textarea id="description" cols="30" rows="5"><%= archive.Translations.ShortDescription()%></textarea>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top;">
                <%= Html.Term("Path") %>
            </td>
            <td>
                <input id="filePath" type="text" class="required" name="A file is required." value="<%= string.IsNullOrEmpty(archive.ArchivePath) ? "" : archive.ArchivePath.ReplaceFileUploadPathToken() %>"
                    style="width: 41.667em;margin-bottom:.667em;" />
                <a href="javascript:void(0);" id="btnBrowseServer" class="Button MediaLibrary"><%= Html.Term("BrowseServer", "Browse Server") %></a>
            </td>
        </tr>
        <tr>
            <td style="vertical-align: top;">
                <%= Html.Term("Thumbnail") %>
            </td>
            <td>
                <input id="thumbnail" type="text" value="<%= string.IsNullOrEmpty(archive.ArchiveImage) ? "" : archive.ArchiveImage.ReplaceFileUploadPathToken() %>"
                    style="width: 41.667em;margin-bottom:.667em" />
                <a href="javascript:void(0);" id="btnBrowseServerThumbnails" class="Button MediaLibrary"><%= Html.Term("BrowseServer", "Browse Server") %></a>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Categories") %>
            </td>
            <td id="categories" class="CategoryTree ltr tree tree-default" style="max-width:400px;">
                <% Html.RenderPartial("DocumentCategories", Model.DocumentCategories); %>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("StartDate", "Start Date") %>
            </td>
            <td>
                <input id="startDate" type="text" name="startDate" value="<%= archive.StartDate.ToShortDateString() %>"
                    class="DatePicker" style="width: 13.636em;" />
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("EndDate", "End Date") %>
            </td>
            <td>
                <input id="endDate" type="text" name="endDate" value="<%= archive.EndDate.HasValue ? archive.EndDate.ToShortDateString() : Html.Term("EndDate", "End Date") %>"
                    class="DatePicker" style="width: 13.636em;" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="active">
                    <%= Html.Term("Active") %></label>
            </td>
            <td>
                <input id="active" type="checkbox" <%= archive.Active ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td>
                <label for="isDownloadable">
                    <%= Html.Term("AllowDownloads", "Allow Downloads")%></label>
            </td>
            <td>
                <input id="isDownloadable" type="checkbox" <%= archive.IsDownloadable ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td>
                <label for="canBeEmailed">
                    <%= Html.Term("CanBeEmailed", "Can Be Emailed")%></label>
            </td>
            <td>
                <input id="canBeEmailed" type="checkbox" <%= archive.IsEmailable ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td>
                <label for="isFeatured">
                    <%= Html.Term("IsFeatured", "Is Featured")%></label>
            </td>
            <td>
                <input id="isFeatured" type="checkbox" <%= archive.IsFeatured ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
    </table>
    <p>
        <a href="javascript:void(0);" id="btnSave" class="Button BigBlue">
            <%= Html.Term("Save") %></a>
        <a href="<%= Request.UrlReferrer != null ? Request.UrlReferrer.AbsolutePath : ResolveUrl("~/Sites/DocumentLibrary") %>"
            class="Button">
            <%= Html.Term("Cancel") %></a>
    </p>
    </form>
    <% Html.RenderPartial("MediaLibrary", new MediaLibraryModel()
       {
           AllowImageInsert = false,
           GenerateSelectButtons = true,
           SystemBaseUrl = NetSteps.Common.Configuration.ConfigurationManager.GetAbsoluteFolder("DocumentLibrary"),
           WebBaseUrl = NetSteps.Common.Configuration.ConfigurationManager.GetWebFolder("DocumentLibrary"),
           UploadUrl = "~/Sites/Documents/UploadFile"
       }); %>
</asp:Content>
