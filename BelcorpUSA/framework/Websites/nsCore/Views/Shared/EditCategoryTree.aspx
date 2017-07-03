<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Models.EditCategoryTreeModel>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<%--<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/adapters/jquery.js"></script>--%>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/adapters/jquery.js"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/") %>Scripts/ajaxupload.js"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/") %>Scripts/tree.min.js"></script>
	<script type="text/javascript">
		$(function () {
			var firstPass = true, categoryStatus = {}, lastLeafId = "";

			function resetCategoryDetails(parentId) {
				$('#categoryId,#txtName,#txtContent').val('');
				$('#imgPreview').attr('src', '');
				$('#parentId').val(parentId || '<%= Model.CategoryTree.CategoryID %>');
				$('#sortIndex').val('0');
			}

			function getCategory() {
				if ($('#categoryId').val()) {
					var data = { categoryId: $('#categoryId').val(), languageId: $('#sLanguage').val() };
					var parent = $('#category' + data.categoryId).closest('li:not(#category' + data.categoryId + ')'); //.parents('li:first');
					$.getJSON('<%= ResolveUrl(Model.GetCategoryURL) %>', data, function (response) {
						if (response.result) {
						    $('#categoryMessageCenter').messageCenter('clearAllMessages').messageCenter('addMessage', '<%= Html.Term("youareediting", "You are editing")%>'+ ' ' + response.name); 

							$('#btnDelete').show();
							// populate the controls
							$('#txtName').val(response.name);
							$('#txtContent').val(response.content);
							if (response.image) {
								$('#imgPreview').attr('src', response.image).show();
								$('#noImage').hide();
								$('#btnDeleteImage').show();
							} else {
								$('#imgPreview').attr('src', '').hide();
								$('#noImage').show();
								$('#btnDeleteImage').hide();
							}
							$('#parentId').val(parent && parent.length ? parent.attr('id').replace(/\D/g, '') : '<%= Model.CategoryTree.CategoryID %>');
							$('#sortIndex').val(response.sortIndex);
						} else
							showMessage(response.message, true);
					});
				}
			}

			function createTree() {

				if ($('#categoryTree ul').length) {
					$('#categoryTree').addClass('ltr').tree({
						rules: {
							multitree: true,
							draggable: 'all',
							createat: 'bottom',
							//Entity framework can only handle 8 levels
							max_depth: 7
						},
						//					ui: {
						//						dots: false
						//					},
						callback: {
							onmove: function (node, refNode, type) {
								var parent = $(node).parents('li').length > 0 ? $(node).parents('li')[0] : null,
							data = { parentId: parent ? parent.id.replace(/\D/g, '') : '<%= Model.CategoryTree.CategoryID %>' },
							nodes = $(node).parent().children('li');
								for (var i = 0; i < nodes.length; i++) {
									data['categoryIds[' + i + ']'] = nodes[i].id.replace(/\D/g, '');
								}
								$.post('<%= ResolveUrl(Model.MoveURL) %>', data, function (response) {
									if (!response.result)
										showMessage(response.message, true);
								});
							},
							onselect: function (node, tree) {
								var categoryId;
								if ($(node).parent().hasClass('AddCat')) {
									categoryId = $(node).parent().parent().attr('id').replace(/\D/g, '');
									$('#categoryMessageCenter').messageCenter('clearAllMessages').messageCenter('addMessage', String.format('<%= Html.Term("AddingChildCategory", "You are adding a child to {0}.") %>', $('#category' + categoryId + ' > a.category').text()));
									resetCategoryDetails(categoryId);
								} else {
									categoryId = $(node).parent().attr('id').replace(/\D/g, '');
									$('#categoryId').val(categoryId);
									getCategory();
								}

								if ($("#category" + categoryId).hasClass("last") || $("#category" + categoryId).hasClass("leaf")) {
									lastLeafId = "#category" + categoryId;
								}
							}
						}
					});
					if (firstPass) {
						$('#categoryTree').find('ul.ltr').removeClass('ltr').find('li.closed').removeClass('closed').addClass('open'); // expand every folder element in the tree
						firstPass = false;
					} else {
						$('#categoryTree').find('ul.ltr').removeClass('ltr');
						$('#categoryTree li').each(function () {
							$(this).attr('class', categoryStatus[$(this).attr('id')]);
						});

						if ($(lastLeafId).children("ul").length > 0) {
							$(lastLeafId).removeClass("closed").addClass("open");
							$(lastLeafId).removeClass("leaf").addClass("open");
						}
						var count = $('#categoryTree ul ul li').length;
						if (count == 1) {
							window.location.reload();
						}
					}
				}
}

			$('#categoryMessageCenter').messageCenter({ color: '#458416', background: '#e6fad9', iconPath: 'UI-icon icon-check' }).messageCenter('addMessage', '<%= Html.Term("YouAreAddingaRootCategory", "You are adding a root category.")%>');
            resetCategoryDetails('<%= Model.CategoryTree.CategoryID %>');
            
			if ($('#categoryId').val('<%= Model.EditingCategoryID.ToString() %>').val())
				getCategory();

			$('#txtContent').ckeditor({ toolbar:
			[
				['Source', '-', ],
				['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Scayt'],
				['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
				['Table', 'HorizontalRule', 'SpecialChar', 'PageBreak'],
				'/',
				['Styles', 'Format'],
				['Bold', 'Italic', 'Strike'],
				['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
				['Link', 'Unlink', 'Anchor'],
				['Maximize', '-', ]
			]
			});
			new AjaxUpload('btnUploadImage', {
				action: '<%= ResolveUrl(Model.SaveImageURL) %>',
				name: 'categoryImage',
				//data: { folder: 'Categories' },
				responseType: 'json',
				autoSubmit: true,
				onComplete: function (file, response) {
					if (response.result) {
						$('#imgPreview').show().attr('src', response.imagePath);
						$('#noImage').hide();
					} else {
						showMessage(response.message, true);
					}
				}
			});

            $('#addRoot').click(function () {
                $('#categoryMessageCenter').messageCenter('clearAllMessages').messageCenter('addMessage', '<%= Html.Term("YouAreAddingaRootCategory", "You are adding a root category.")%>');

				resetCategoryDetails('<%= Model.CategoryTree.CategoryID %>');
			});
			$('#languageFilter').val(1);

			createTree(); // rebinds events and formats the tree by adding css classes

			$('#btnSave').click(function () {
				if (!$('#categoryDetails').checkRequiredFields()) {
					return false;
				}
				var t = $(this);
				showLoading(t);
				$.post('<%= ResolveUrl(Model.SaveURL) %>',
					{
						categoryId: $('#categoryId').val(),
						name: $('#txtName').val(),
						content: $('#txtContent').val(),
						image: $('#imgPreview').attr('src'),
						parentId: $('#parentId').val(),
						sortIndex: $('#sortIndex').val(),
						languageId: $('#sLanguage').val()
					},
					function (response) {
						hideLoading(t);
						if (response.result) {
							$('#categoryTree li').each(function () {
								if (!($(this).hasClass("last") && $(this).hasClass("leaf"))) {
									categoryStatus[$(this).attr('id')] = $(this).attr('class');
								}
							});
							$("#categoryTree").replaceWith("<div id='categoryTree' class='CategoryTree'></div>");
							$('#categoryTree').html(response.categories);
							createTree(); // rebinds events and formats the tree by adding css classes
							$('#categoryId').val(response.categoryId);
							$('#categoryMessageCenter').messageCenter('clearAllMessages').messageCenter('addMessage', 'You are editing ' + $('#category' + response.categoryId + ' > a.category').text() + '.');
							showMessage('Category saved!', false);
						} else {
							showMessage(response.message, true);
						}
					});
			});

			$('#btnDeleteCategory').click(function () {
				if ($('#categoryId').val() && confirm('Are you sure you want to delete this category and all child categories?')) {
					$.post('<%= ResolveUrl(Model.DeleteURL) %>', { categoryId: $('#categoryId').val() }, function (response) {
						if (response.result) {
							$('#category' + $('#categoryId').val()).remove();
							$('#addRoot').click();
						} else {
							showMessage(response.message, true);
						}
					});
				}
			});

			$('#btnDeleteImage').click(function () {
				$.post('<%= ResolveUrl(Model.DeleteImageURL) %>',
					{
						categoryId: $('#categoryId').val(),
						image: $('#imgPreview').attr('src'),
						languageId: $('#sLanguage').val()
					}, function (response) {
						if (response.result) {
							getCategory();
							showMessage('Image Deleted Successfully', false);
						} else {
							showMessage(response.message, true);
						}
					});
			});

			$('#rootCategoryLink').click(function () {
				var rootCat = $('#rootCategoryId').val();
				$('#categoryId').val(rootCat);
				getCategory();
			});

			$('#sLanguage').change(function () {
				// reload the content based on the language
				getCategory();
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<% Constants.CategoryType categoryType = Model.CategoryType; %>
		<h2>
			<%= categoryType == ConstantsGenerated.CategoryType.Archive ? Html.Term("Document") : Html.Term("Product") %>
			<%= Html.Term("Categories")%></h2>
		<%if (categoryType == ConstantsGenerated.CategoryType.Product)
	{ %>
		<%}
	else
	{ %>
		<a href="<%= ResolveUrl("~/Sites/Documents") %>">
			<%= Html.Term("DocumentLibrary", "Document Library") %></a> | <a href="<%= Page.ResolveUrl("~/Sites/Documents/Edit") %>">
				<%= Html.Term("AddDocument", "Add Document") %></a> |
		<%= Html.Term("ManageDocumentCategories", "Manage Document Categories") %>
		<%} %>
	</div>
	<table width="100%" class="SectionTable">
		<tr>
			<td width="40%">
				<h3>
					<a href="javascript:void(0);" id="rootCategoryLink">
						<%= Html.Term("TopLevelArchiveCategory", "Top Level Archive Category")%></a>
                        <!--Model.CategoryTree.Translations.Name()</a>-->
				</h3>
				<div class="UI-lightBg brdrAll pad10 mb10">
					<span><a id="addRoot" href="javascript:void(0);">
						<%= Html.Term("AddaRootCategory", "Add a Root Category")%></a><span class="pipe" style="float: none;">&nbsp;</span> <a id="btnExpandAll" href="javascript:void(0);" onclick="$('#categoryTree').find('li.closed').removeClass('closed').addClass('open');">
							<%= Html.Term("ExpandAll", "Expand All")%></a><span class="pipe" style="float: none;">&nbsp;</span> <a id="btnCollapseAll" href="javascript:void(0);" onclick="$('#categoryTree').find('li.open').removeClass('open').addClass('closed');">
								<%= Html.Term("CollapseAll", "Collapse All")%></a>
						<%--| <a href="javascript:void(0);">Edit Tree Name</a>--%></span>
				</div>
				<div id="categoryTree" class="CategoryTree">
					<%= Model.Categories %>
				</div>
			</td>
			<td width="60%">
				<h3>
					<%= Html.Term("CategoryDetails", "Category Details")%>
					<span><a id="btnDeleteCategory" href="javascript:void(0);">
						<%= Html.Term("RemoveCategory", "Remove Category")%></a></span></h3>
				<table id="categoryDetails" class="DataGrid" width="100%">
					<tr>
						<td colspan="2" style="padding: 0;">
							<div id="categoryMessageCenter" class="UI-lightBg brdrAll pad5 mb10">
							</div>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("Language", "Language")%>:
						</td>
						<td>
							<select id="sLanguage" class="pad5">
								<% foreach (Language language in Language.LoadAll())
		   { %>
								<option value="<%= language.LanguageID %>" <%= language.LanguageID == 1 ? "selected=\"selected\"" : "" %>>
									<%= Html.Term(language.Name, language.Name)%></option>
								<%} %>
							</select>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("Name", "Name")%>:
						</td>
						<td>
							<input id="txtName" type="text" class="required fullWidth pad5" name="Name is required." maxlength="100" />
						</td>
					</tr>
					<%--<tr>
						<td class="FLabel">
							Short Description:
						</td>
						<td>
							<textarea id="txtShortDescription" style="width: 500px; height: 50px;"></textarea>
						</td>
					</tr>--%>
					<tr>
						<td class="FLabel" style="vertical-align: top;">
							<%= Html.Term("Page Content", "Page Content")%>:
						</td>
						<td>
							<textarea id="txtContent" style="width: 45.455em; height: 27.273em;">
							</textarea>
						</td>
					</tr>
					<tr>
						<td colspan="2" style="padding: 0;">
							<p class="UI-secBg pad5 brdrAll description">
								<%= Html.Term("Category_ImageOptions", "Image Options")%>:</p>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("Image", "Image")%>:
							<p class="InputTools">
								<input type="hidden" id="rootCategoryId" value="<%=ViewData["RootCategoryId"] %>" />
								<input type="hidden" id="categoryId" />
								<input type="hidden" id="parentId" value="<%= Model.CategoryTree.CategoryID %>" />
								<input type="hidden" id="sortIndex" value="0" />
								<a id="btnUploadImage" href="javascript:void(0);">
									<%= Html.Term("UploadaNewImage", "Upload a New Image")%></a><br />
								<a id="btnDeleteImage" href="javascript:void(0);" style="display: none;">
									<%= Html.Term("DeleteImage", "Delete Image")%></a>
							</p>
						</td>
						<td>
							<img id="imgPreview" alt="" src="" width="400" style="display: none;" />
							<span id="noImage" class="LawyerText">
								<%= Html.Term("NoImageUploaded", "No Image Uploaded")%></span>
						</td>
					</tr>
					<tr>
						<td>
						</td>
						<td>
							<br />
							<p>
								<a id="btnSave" href="javascript:void(0);" class="Button BigBlue">
									<%= Html.Term("Save Category", "Save Category")%></a>
							</p>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
