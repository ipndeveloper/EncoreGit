<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/Catalogs.Master"
	Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/CategoryTrees") %>">
		<%= Html.Term("CategoryTrees", "Category Trees") %></a> >
	<%= Html.Term("NewCategoryTree", "New Category Tree") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		$(function () {
			$('#newCategoryTree').setupRequiredFields();
			$('#btnCreateTree').click(function () {
				if (!$('#newCategoryTree').checkRequiredFields()) {
					return false;
				}
				$('#newCategoryTreeForm').submit();
			});
		});
	</script>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("NewCategoryTree", "New Category Tree") %></h2>
		<a href="<%= ResolveUrl("~/Products/Categories") %>">
			<%= Html.Term("BrowseCategoryTrees", "Browse Category Trees") %></a> |
		<%= Html.Term("CreateaNewCategoryTree", "Create a New Category Tree") %>
	</div>
	<form id="newCategoryTreeForm" action="<%= ResolveUrl("~/Products/Categories/NewTree") %>"
	method="post">
	<table id="newCategoryTree" width="100%">
		<tbody>
			<tr>
				<td class="FLabel">
					<%= Html.Term("TreeName", "Tree Name") %>:
				</td>
				<td>
					<p>
						<input id="treeName" type="text" class="required" name="<%= Html.Term("TreeNameIsRequired", "Tree Name is required") %>" /></p>
				</td>
			</tr>
			<tr>
				<td>
					&nbsp;
				</td>
				<td>
					<p>
						<a id="btnCreateTree" class="Button BigBlue" href="javascript:void(0);"><span>
							<%= Html.Term("CreateTree", "Create Tree") %></span></a></p>
				</td>
			</tr>
		</tbody>
	</table>
	</form>
</asp:Content>
