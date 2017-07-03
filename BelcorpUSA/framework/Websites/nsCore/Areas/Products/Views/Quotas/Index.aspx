<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master" 
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
//        $(function () {
            // Product ini

//            var productSelected = false;
//            var productoId = $('<input type="hidden" id="productoIdFilter" class="Filter" />');
//            $('#productInputFilter').removeClass('Filter').css('width', '275px').watermark('<%=Html.JavascriptTerm("ProductSearch","Look up product by ID or name") %>').jsonSuggest('<%=ResolveUrl("~/Products/Promotions/Search") %>',
//                        { onSelect: function (item) {
//                            productoId.val(item.id);
//                            productSelected = false;
//                        }, minCharacter: 3,
//                            source: $('#productInputFilter'),
//                            ajaxResults: true,
//                            maxResult: 50,
//                            showMore: true
//                        }).blur(function () {
//                            if (!$(this).val() || !$(this).val() == $(this).data('watermark')) {
//                                productoId.val('');
//                            }
//                            productSelected = false;
//                        }).after(productoId);

            // Product fin
//        });
	</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
	<%= Html.Term("ProductQuota", "Product Quota")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("ProductQuota", "Product Quota")%>
        </h2>
        <a href="<%= ResolveUrl("~/Products/Quotas/Create") %>"><%= Html.Term("CreateaNewQuota", "Create a New Quota") %></a>
	</div>
    <% Html.PaginatedGrid<ProductQuotaSearchData>("~/Products/Quotas/GetQuotas")
        //.AutoGenerateColumns()
        .AddSelectFilter(Html.Term("Active"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaActive", "Select a Active...") }, { "true", Html.Term("True") }, { "false", Html.Term("False") } })
        .AddInputFilter(Html.Term("SKUorName", "SKU or Name"), "SKUorName")
        //.AddInputFilter(Html.Term("Product"), "product") //autocomplete
        
        .AddColumn(Html.Term("Name"), "Name", true)
        .AddColumn(Html.Term("CUV"), "SKU", true)
        .AddColumn(Html.Term("Product"), "ProductName", true)
        .AddColumn(Html.Term("Status"), "Active", true)
        
        .CanDelete("~/Products/Quotas/Delete")
        .CanChangeStatus(true, true, "~/Products/Quotas/ChangeQuotaStatus")
        .ClickEntireRow()
        .Render(); %>
</asp:Content>