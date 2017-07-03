<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
	<%= Html.Term("BrowseProducts", "Browse Products") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("BrowseProducts", "Browse Products") %></h2>
	</div>
	<% Html.RenderPartial("ProductBrowse"); %>
	<script type="text/javascript">
		$(function () {
			$('#paginatedGridOptions .deleteButton').unbind('click').click(function () {
				var selected = $('#paginatedGrid input[type="checkbox"]:checked:not(.checkAll)'), data = {};
				if (selected.length && confirm('<%: Html.Term("AreYouSureYouWishToDeleteTheseItems", "Are you sure you wish to delete these items?") %>')) {
					selected.each(function (i) {
						data['items[' + i + ']'] = $(this).val();
					});
					$.post('<%= ResolveUrl("~/Products/Products/Delete") %>', data, function () {
						window.location.reload(true);
					});
				}
			});
		});
	</script>
</asp:Content>
