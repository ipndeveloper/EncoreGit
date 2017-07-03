<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/HolidayManagement.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("GeneralLegder","General Ledger") %></a> >
	<%= Html.Term("HolidayBrowse", "Browse Holidays") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("BrowseHolidays", "Browse Holidays") %></h2>
	</div>
	<% Html.RenderPartial("HolidayBrowse"); %>
	<script type="text/javascript">
	    $(function () {
	        $('#paginatedGridOptions .deleteButton').unbind('click').click(function () {
	            var selected = $('#paginatedGrid input[type="checkbox"]:checked:not(.checkAll)'), data = {};
	            if (selected.length && confirm('<%: Html.Term("AreYouSureYouWishToDeleteTheseItems", "Are you sure you wish to delete these items?") %>')) {
	                selected.each(function (i) {
	                    data['items[' + i + ']'] = $(this).val();
	                });
	                $.post('<%= ResolveUrl("~/GeneralLedger/Holiday/Delete") %>', data, function () {
	                    window.location.reload(true);
	                });
	            }
	        });
	    });
	</script>
</asp:Content>
