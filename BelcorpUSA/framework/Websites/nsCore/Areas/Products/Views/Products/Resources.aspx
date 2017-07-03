<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master"
	Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ajaxupload.js") %>"></script>
	<script type="text/javascript">
		$(function () {
			var upload = new AjaxUpload('btnUploadResource', {
			    action: '<%= ResolveUrl(string.Format("~/Products/Products/UploadResource/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>',
				responseType: 'json',
				onSubmit: function (file, extension) {
					upload.setData({ resourceType: $('#resourceType').val() });
				},
				onComplete: function (file, response) {
					if (response.result) {
						$('#resources').append(response.resource);
					} else {
						showMessage(response.message, true);
					}
				}
			});

			$('#resources .delete').live('click', function () {
				var t = $(this);
				showLoading(t);
				$.post('<%= ResolveUrl(string.Format("~/Products/Products/DeleteResource/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', { resourceId: t.parent().find('.resourceId').val() }, function (response) {
					hideLoading(t);
					if (response.result) {
						t.parent().fadeOut('fast', function () {
							$(this).remove();
						});
					} else
						showMessage(response.message, true);
				});
			});

			$('#btnSave').click(function () {
				var data = {};

				$('#resources li').each(function (i) {
					data['resources[' + i + '].Key'] = $('.resourceId', this).val();
					data['resources[' + i + '].Value'] = $('.resourceType', this).val();
				});

            	$.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveResources/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data, function (response) {
					showMessage(response.message || 'Resources saved successfully!', !response.result);
				});
			});
		});
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
			<%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>">
				<%= Model.Translations.Name() %></a> >
	<%= Html.Term("Resources") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Resources", "Resources") %></h2>
		<a id="btnUploadResource" href="javascript:void(0);">Upload a new resource</a><select
			id="resourceType">
			<%foreach (var resourceType in SmallCollectionCache.Instance.ProductFileTypes.Where(pft => pft.ProductFileTypeID != (int)Constants.ProductFileType.Image && pft.Active))
	 { %>
			<option value="<%= resourceType.ProductFileTypeID %>">
				<%= resourceType.GetTerm() %></option>
			<%} %>
		</select>
		| <a href="<%= ResolveUrl("~/Products/ResourceTypes") %>">Resource Types Management</a>
	</div>
	<ul id="resources" style="line-height: 28px;">
		<%foreach (var resource in Model.Files.Where(pf => pf.ProductFileTypeID != (int)Constants.ProductFileType.Image))
	{
		string icon, fileName = resource.FilePath.Substring(resource.FilePath.LastIndexOf('/') + 1);
		var type = fileName.GetFileType();
		switch (type)
		{
			case NetSteps.Common.Constants.FileType.PDF:
				icon = "acrobatICO.gif";
				break;
			case NetSteps.Common.Constants.FileType.Audio:
				icon = "audioICO.gif";
				break;
			case NetSteps.Common.Constants.FileType.Flash:
				icon = "flashICO.gif";
				break;
			case NetSteps.Common.Constants.FileType.Image:
				icon = "jpegICO.gif";
				break;
			case NetSteps.Common.Constants.FileType.Video:
				icon = "movieICO.gif";
				break;
			case NetSteps.Common.Constants.FileType.Powerpoint:
				icon = "powerpointICO.gif";
				break;
			default:
				icon = "genericdocICO.gif";
				break;
		}%>
		<li>
			<img src="<%= ResolveUrl("~/Content/Images/Icons/DocumentTypes/") + icon %>" alt="<%= type.ToString() %>"
				title="<%= type.ToString() %>" />
			<%= fileName %>
			<input type="hidden" class="resourceId" value="<%= resource.ProductFileID %>" />
			<select class="resourceType">
				<%foreach (var resourceType in SmallCollectionCache.Instance.ProductFileTypes.Where(pft => pft.ProductFileTypeID != (int)Constants.ProductFileType.Image && pft.Active))
	 { %>
				<option value="<%= resourceType.ProductFileTypeID %>" <%= resourceType.ProductFileTypeID == resource.ProductFileTypeID ? "selected=\"selected\"" : "" %>>
					<%= resourceType.GetTerm() %></option>
				<%} %>
			</select>
			<a href="javascript:void(0);" class="delete listValue">
				<span class="UI-icon icon-x" title="<%= Html.Term("Delete", "Delete") %>"></span></a></li>
		<%} %>
	</ul>
	<span class="ClearAll"></span>
	<p>
		<a href="javascript:void(0);" class="Button BigBlue" id="btnSave"><%= Html.Term("Save", "Save") %></a>
	</p>
</asp:Content>
