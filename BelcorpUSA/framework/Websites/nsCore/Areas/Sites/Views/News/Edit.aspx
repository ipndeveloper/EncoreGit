<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.News>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<%--<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/adapters/jquery.js"></script>--%>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/adapters/jquery.js"></script>
	<script type="text/javascript">
		$(function () {
			$('#body').ckeditor();
			$('#newsProperties').setupRequiredFields();

			$('#btnSave').click(function () {
				if ($('#newsProperties').checkRequiredFields()) {

					$.post('<%= ResolveUrl("~/Sites/News/Save") %>', {
						newsId: $('#newsId').val(),
						languageId: $('#languageId').val(),
						title: $('#title').val(),
						type: $('#type').val(),
						startDate: $('#startDate').val(),
						startTime: $('#startTime').val(),
						endDate: $('#endDate').val(),
						endTime: $('#endTime').val(),
						active: $('#active').prop('checked'),
						isPublic: $('#isPublic').prop('checked'),
						isFeatured: $('#isFeatured').prop('checked'),
						isMobile: $('#isMobile').prop('checked'),
						caption: $('#caption').val(),
						body: $('#body').val()
					}, function (response) {
						if (response.result) {
							showMessage(response.message || 'News saved successfully!', !response.result);
							$('#newsId').val(response.newsId);
							$('#btnPushNotifications').show();
						} else {
							showMessage(response.message, true);
						}
					});
				}
			});
			$('#title').keypress(function() {
					var length = this.value.length;
					if(length <= $(this).attr('maxlength')) {
						 $("#title").removeAttr("disabled"); 
					 }
			});
			$('#btnPushNotifications.Button').click(function () {
				if(confirm('<%= Html.Term("Are you sure you want to push this to devices? This cannot be undone and will be pushed for all languages with content.") %>')){
					$('#btnPushNotifications').toggleClass('Button ButtonOff');
					$.post('<%= ResolveUrl("~/Sites/News/PushNotifications")%>', {
						newsId: $('#newsId').val()
					}, function (response) {
						if (response.result) {
							showMessage(response.message, !response.result);
						} else {
							showMessage(response.message, true);
						}
					});
				}
			});

			$('.btnDelete').live('click', function () {
				var alertTemplateTranslationID = $(this).closest('div#IdHolder').attr('data-id');

				if (confirm('<%: Html.Term("AreYouSureYouWishToDeleteThisItem", "Are you sure you wish to delete this item?") %>')) {

					var t = $(this);

					var data = {};

					data['items[0]'] = <%= Model.NewsID %>;
					
					showLoading(t);

					$.post('<%= ResolveUrl("~/Sites/News/Delete") %>', data,
					function (response) {
						hideLoading(t);

						if (response.result) {
							showMessage(response.message || 'News deleted successfully!', !response.result);
							window.location = "/Sites/News/";
						} else {
							showMessage(response.message, true);
						}
					});
				}
			});

			$('#languageId').change(function () {
				$.get('<%= ResolveUrl("~/Sites/News/GetTranslation") %>', { newsId: $('#newsId').val(), languageId: $(this).val() }, function (response) {
					if (response.result) {
						$('#title').val(response.title);
						$('#caption').val(response.caption);
						$('#body').val(response.body);
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
		<%= Html.Term("Sites") %></a> > <a href="<%= ResolveUrl("~/Sites/News") %>">
			<%= Html.Term("NewsAndAnnounements", "News & Announcements") %></a> > 
	<%= Model == null || Model.NewsID == 0 ? Html.Term("AddNews", "Add News") : Html.Term("EditNews", "Edit News") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<form action="<%= ResolveUrl("~/Sites/News/Save") %>" method="post">
	<div class="SectionHeader">
    rererer

		<h2>
			<%= Model == null || Model.NewsID == 0 ? Html.Term("AddNews", "Add News") : Html.Term("EditNews", "Edit News")%></h2>
		<a href="<%= ResolveUrl("~/Sites/News/Edit") %>" class="mr10 DTL Add">
			<%= Html.Term("AddNewNews", "Add a new news item") %></a>
		<% if (Model != null && Model.NewsID > 0)
	 {
		%>
		<a class="btnDelete DTL Remove" href="javascript:void(0);"><span>
			<%: Html.Term("Delete") %></span></a> |
		<%
		   } %>
		<%= Html.Term("Language")%>:
		<% var content = Model.NewsID > 0 ? Model.HtmlSection.ProductionContentForDisplay(ViewData["CurrentSite"] as Site, useCache: false) : null; %>
		<%= Html.DropDownLanguages(errorMessage: "languageId", htmlAttributes: new { id = "languageId" }, selectedLanguageID: (Model.NewsID > 0 && content != null ? content.LanguageID : CoreContext.CurrentLanguageID))%>
		<% if (CoreContext.CurrentUser.HasFunction("Sites-News-PushNotifications"))
	 { %>
		<a href="javascript:void(0);" class="mr10 Button<%= Model.EventContexts.Count > 0 ? "Off" : string.Empty %> BigBlue" id="btnPushNotifications" style="<%=Model.NewsID > 0 ? string.Empty: "display: none;"%>" title="<%= ViewData["DevicePushMessage"] != null ? ViewData["DevicePushMessage"].ToString() : string.Empty %>">
			<%= Html.Term("Push To Devices") %>
		</a>
		<% } %>
	</div>
	<%if (TempData["Error"] != null)
   { %>
	<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous; -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0; border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold; margin-bottom: 10px; padding: 7px;">
		<div style="color: #FF0000; display: block;" class="UI-icon icon-exclamation">
			<%= TempData["Error"] %></div>
	</div>
	<%} %>
	<input type="hidden" id="newsId" name="newsId" value="<%= Model == null ? "" : Model.NewsID.ToString() %>" />
	<table id="newsProperties" width="100%" cellspacing="0" class="DataGrid">
		<tr>
			<td style="width: 150px;">
				<%= Html.Term("Title") %>:
			</td>
			<td>
				<input type="text" id="title" name="<%= Html.Term("TitleRequired", "Title is required.") %>" class="required fullWidth pad5 newsTitleInput" maxlength="50" value="<%= content == null ? "" : content.FirstOrEmptyElement(Constants.HtmlElementType.Title).Contents %>" />
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("Type") %>:
			</td>
			<td>
				<select id="type" name="type">
					<% foreach (var type in (SmallCollectionCache.Instance.NewsTypes))
		{ %>
					<option value="<%= type.NewsTypeID %>" <%= Model.NewsTypeID == type.NewsTypeID ? "selected=\"selected\"" : "" %>>
						<%= type.GetTerm() %></option>
					<%} %>
				</select>
				<% if (CoreContext.CurrentUser.HasFunction("Admin-Create and Edit List Value"))
	   { %>
				&nbsp;<a href="<%= ResolveUrl("~/Admin/ListTypes/Values/") + Constants.EditableListTypes.NewsType.ToString() %>" target="_blank">Edit News Types</a>
				<% } %>
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("StartDate", "Start Date") %>:
			</td>
			<td>
                 <%-- Codigo Original seteo de fecha actual  %>
				<%--<input type="text" id="startDate" name="<%= Html.Term("DateRequired", "Date is required.") %>" class="DatePicker required" value="<%= Model.StartDate == DateTime.MinValue ?    DateTime.Today.ToShortDateString() : Model.StartDate.ToShortDateString() %>" style="width: 100px;" />--%>
                 <%--codigo modificado por IPN para generalizar el formato de fecha por tipo de lenguaje--%>
                <input type="text" id="Text1" name="<%= Html.Term("DateRequired", "Date is required.") %>" class="DatePicker required" value="<%= Model.StartDate == DateTime.MinValue ?    DateTime.Today.ToString(new System.Globalization.CultureInfo(CoreContext.CurrentCultureInfo.ToString()).DateTimeFormat.ShortDatePattern) : Model.StartDate.ToShortDateString() %>" style="width: 100px;" />
				<input type="text" id="startTime" name="startTime" value="<%= Model.StartDate.ToShortTimeString() %>" />
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("EndDate", "End Date") %>:
			</td>
			<td>
				<input type="text" id="endDate" class="DatePicker" value="<%= !Model.EndDate.HasValue ? Html.Term("EndDate", "End Date") : Model.EndDate.ToDateTime().ToShortDateString() %>" style="width: 100px;" />
				<input type="text" id="endTime" name="endTime" value="<%= Model.EndDate.HasValue ? Model.EndDate.ToDateTime().ToShortTimeString() : Html.Term("EndTime", "End Time") %>" />
			</td>
		</tr>
		<tr>
			<td>
				<label for="active">
					<%= Html.Term("Active") %>:</label>
			</td>
			<td>
				<input type="checkbox" id="active" <%= Model.Active ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
		<tr>
			<td>
				<label for="isPublic">
					<%= Html.Term("IsPublic", "Is Public") %>:</label>
			</td>
			<td>
				<input type="checkbox" id="isPublic" <%= Model.IsPublic ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
		<tr>
			<td>
				<label for="isFeatured">
					<%= Html.Term("IsFeatured", "Is Featured")%>:</label>
			</td>
			<td>
				<input type="checkbox" id="isFeatured" <%= Model.IsFeatured ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
		<tr>
			<td>
				<label for="isMobile">
					<%= Html.Term("IsMobile", "Is Mobile")%>:</label>
			</td>
			<td>
				<input type="checkbox" id="isMobile" <%= Model.IsMobile ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("Caption") %>:
			</td>
			<td>
				<textarea id="caption" name="caption" class="fullWidth pad3 newsCaption" style="height: 4.167em;"><%= content == null ? "" : content.FirstOrEmptyElement(Constants.HtmlElementType.Caption).Contents%></textarea>
			</td>
		</tr>
		<tr>
			<td>
				<%= Html.Term("Body") %>:
			</td>
			<td>
				<textarea id="body" name="body" rows="5" cols="20"><%= content == null ? "" : content.FirstOrEmptyElement(Constants.HtmlElementType.Body).Contents%></textarea>
			</td>
		</tr>
	</table>
	<div class="mt10">
		<p>
			<a href="javascript:void(0);" class="FL Button BigBlue" id="btnSave">
				<%= Html.Term("Save") %>
			</a><a href="<%= ResolveUrl("~/Sites/News") %>" class="FL Button"><span>
				<%= Html.Term("Cancel") %>
			</span></a>
			<img class="Loading FL ml10" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." style="display: none;" height="26" />
			<span class="clr"></span>
		</p>
	</div>
	</form>
</asp:Content>
