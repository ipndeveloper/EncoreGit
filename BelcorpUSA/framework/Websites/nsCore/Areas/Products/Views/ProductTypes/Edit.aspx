<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.ProductType>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

	<script type="text/javascript">
		$(function () {
			$('#btnSave').click(function () {
				if ($('#productType').checkRequiredFields()) {
					var data = { productTypeId: $('#productTypeId').val(), name: $('#name').val(), active: $('#active').prop('checked') };

					$('#productType .property:checked').each(function (i) {
						data['properties[' + i + ']'] = $(this).val();
					});

					$.post('<%= ResolveUrl("~/Products/ProductTypes/Save") %>', data, function (response) {
						showMessage(response.message || '<%= Html.Term("ProductTypeSaved", "Product Type saved successfully!") %>', !response.result);
						if (response.result)
							$('#productTypeId').val(response.productTypeId);
					});
				}
			});
		});
	</script>

</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/ProductTypes") %>">
			<%= Html.Term("ProductTypes", "Product Types")%></a> >
	<%= Model.ProductTypeID > 0 ? Html.Term("EditProductType", "Edit Product Type") : Html.Term("NewProductType", "New Product Type") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Model.ProductTypeID > 0 ? Html.Term("EditProductType", "Edit Product Type") : Html.Term("NewProductType", "New Product Type") %></h2>
	</div>
	<table id="productType" class="FormTable" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("Name", "Name") %>:
			</td>
			<td>
				<input id="productTypeId" type="hidden" value="<%= Model.ProductTypeID == 0 ? "" : Model.ProductTypeID.ToString() %>" />
				<input id="name" type="text" class="required" name="<%= Html.Term("NameRequired", "Name is required.") %>" value="<%= Model.GetTerm() %>" maxlength="25" />
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("Active", "Active") %>:
			</td>
			<td>
				<input id="active" type="checkbox" <%= Model.Active ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("Properties","Properties") %>:
			</td>
			<td>
				<% foreach (ProductPropertyType property in ProductPropertyType.LoadAll())
	   { %>
				<input type="checkbox" class="property" value="<%= property.ProductPropertyTypeID %>" <%= Model.ProductPropertyTypes.Select(p => p.ProductPropertyTypeID).Contains(property.ProductPropertyTypeID) ? "checked=\"checked\"" : "" %> /><%= property.GetTerm() %><br />
				<%} %>
			</td>
		</tr>
	</table>
	<p>
        <a id="btnSave" href="javascript:void(0);" class="Button BigBlue">
			<%= Html.Term("Save","Save") %></a>
		<a href="<%= ResolveUrl("~/Products/ProductTypes") %>" class="Button">
			<%= Html.Term("Cancel","Cancel") %></a>
	</p>
</asp:Content>
