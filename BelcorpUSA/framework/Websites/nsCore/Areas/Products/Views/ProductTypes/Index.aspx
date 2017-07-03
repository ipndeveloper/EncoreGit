<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.IEnumerable<NetSteps.Data.Entities.ProductType>>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

	<script type="text/javascript">
		$(function () {
			$('#productTypes .delete').live('click', function () {
				var productType = $(this), productTypeId = productType.attr('id').replace(/\D/g, '');
				if (productTypeId == 0 || confirm('Are you sure you want to delete this product type?  Please note: any type in use cannot be deleted.')) {
					if (productTypeId > 0) {
						$.post('<%= ResolveUrl("~/Products/ProductTypes/Delete") %>', { productTypeId: productTypeId });
					}
					productType.parent().fadeOut('normal', function () {
						$(this).remove();
					});
				}
			});
		});
	</script>

</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > 
			<%= Html.Term("ProductTypes", "Product Types")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("ProductTypes","Product Types") %>
		</h2>
		<a href="<%= ResolveUrl("~/Products/ProductTypes/Edit") %>" id="btnAdd">
			<%= Html.Term("AddNewProductType","Add new product type") %></a>
	</div>
    <div class="splitCol">
	<ul id="productTypes" class="flastList listNav listValues">
		<%foreach (ProductType productType in Model.OrderBy(pt => pt.Editable))
	{ %>
		<li>
		<%if (productType.Editable)
	{ %>
		<a href="<%= ResolveUrl("~/Products/ProductTypes/Edit/") + productType.ProductTypeID %>" class="FL">
			<%= productType.GetTerm()%></a>
       <a id="productType<%= productType.ProductTypeID %>" href="javascript:void(0);" class="FR delete listValue" title="Delete">
       <span class="UI-icon icon-x" title="<%= Html.Term("Delete", "Delete") %>"></span></a>
			<%}
	 else
	 { %>
			<span class="UI-icon icon-lock" title="locked"></span>
            <input type="text" value="<%= productType.GetTerm() %>" disabled="disabled" />
			<%} %>
            <span class="clr"></span>
            </li>
		<%} %>
	</ul>
    </div>
	<%--<span class="ClearAll"></span>
	<p style="margin-top: 5px;">
		<a href="javascript:void(0);" class="Button BigBlue" id="btnSave">Save</a>
	</p>--%>
</asp:Content>
