<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/Catalogs.Master"
	Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
	<%= Html.Term("CategoryTrees", "Category Trees")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("CategoryTrees", "Category Trees") %></h2>
		<%= Html.Term("BrowseCategoryTrees", "Browse Category Trees") %>
		| <a href="<%= ResolveUrl("~/Products/Categories/NewTree") %>">
			<%= Html.Term("CreateaNewCategoryTree", "Create a New Category Tree") %></a>
	</div>
	<%Html.PaginatedGrid("~/Products/Categories/GetTrees")
	   .AddColumn(Html.Term("TreeName", "Tree Name"), "Name", true)
	   .AddColumn(Html.Term("UsedBy(StoreFront/Catalog)", "Used By (Store Front/Catalog)"), "", false)
	   .AddColumn(Html.Term("Products", "Products"), "ProductCount", false)
	   .CanDelete("~/Products/Categories/DeleteTrees")
       .ClickEntireRow()
	   .Render(); %>
	<script type="text/javascript">
		$(function () {
			$('#paginatedGridOptions .deleteButton').unbind('click').click(function () {
				var selected = $('#paginatedGrid input[type="checkbox"]:checked:not(.checkAll)'), data = {};
				if (selected.length && confirm('Are you sure you want to delete these category trees?')) {
					selected.each(function (i) {
						data['items[' + i + ']'] = $(this).val();
					});
					$.post('<%= ResolveUrl("~/Products/Categories/DeleteTrees") %>', data, function () {
						window.location.reload(true);
					});
				}
			});
		});
	</script>
	<%Html.RenderPartial("MessageCenter"); %>
</asp:Content>
