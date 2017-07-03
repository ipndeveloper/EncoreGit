<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Warehouses/Warehouses.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.ShippingRegion>>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

	<script type="text/javascript">
		$(function () {
			$('#btnAdd').click(function () {
				$('#shippingRegions').append('<li><input type="text" name="value0" class="shippingRegion" /><a href="javascript:void(0);" class="delete listValue"><span class="UI-icon icon-x" title="Delete"></span></a></li>');
			});
			$('.delete').live('click', function () {
				if ($(this).parent().find('input').attr('name').replace(/\D/g, '') == 0 || confirm('Are you sure you want to delete this shipping region?  Please note: regions that are in use cannot be deleted.')) {
					var shippingRegion = $(this).parent().find('input'), shippingRegionId = shippingRegion.attr('name').replace(/\D/g, '');
					if (shippingRegionId > 0) {
						$.post('<%= ResolveUrl("~/Products/ShippingRegions/Delete") %>', { shippingRegionId: shippingRegionId });
					}
					shippingRegion.parent().fadeOut('normal', function () {
						$(this).remove();
					});
				}
			});
			$('#btnSave').click(function () {
				var data = {};
				$('#shippingRegions .shippingRegion').each(function (i) {
					data['shippingRegions[' + i + '].ShippingRegionID'] = $(this).attr('name').replace(/\D/g, '');
					data['shippingRegions[' + i + '].Name'] = $(this).val();
				});

				$.post('<%= ResolveUrl("~/Products/ShippingRegions/Save") %>', data, function (response) {
					if (response.result) {
						//window.location = '<%= ResolveUrl("~/Admin/ListTypes") %>';
						showMessage('Shipping Regions saved!', false);
						$('#shippingRegions').html(response.shippingRegions);
					}
				});
			});
		});
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
	<%= Html.Term("ShippingRegions", "Shipping Regions")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">


	<div class="SectionHeader">
		<h2>
			<%= Html.Term("ShippingRegions", "Shipping Regions") %>
		</h2>
		<%= Html.Term("BrowseShippingRegions", "Browse Shipping Regions") %>
		| <a href="<%= ResolveUrl("~/Products/ShippingRegions/Manage") %>">
			<%= Html.Term("ManageShippingRegions", "Manage Shipping Regions") %></a><br />
		<a href="javascript:void(0);" id="btnAdd" class="DTL Add">
			<%= Html.Term("AddNewShippingRegion", "Add new shipping region") %></a>
	</div>
	<ul id="shippingRegions" class="listValues">
		<%foreach (ShippingRegion shippingRegion in Model.OrderBy(r => r.Editable))
	{ %>
		<li>
			<%if (shippingRegion.Editable)
	 { %>
			<input type="text" name="value<%= shippingRegion.ShippingRegionID %>" value="<%= shippingRegion.GetTerm() %>" class="shippingRegion" maxlength="50" />
			<a href="javascript:void(0);" class="delete listValue">
				<span class="UI-icon icon-x" title="<%= Html.Term("Delete", "Delete") %>" /></a>
			<%}
	 else
	 { %>
			<input type="text" value="<%= shippingRegion.GetTerm() %>" disabled="disabled" />
            <span class="UI-icon icon-lock" title="locked"></span>
			<%} %>
		</li>
		<%} %>
	</ul>
	<span class="ClearAll"></span>
	<p>
		<a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
			<%= Html.Term("Save", "Save") %></a>
	</p>
</asp:Content>
