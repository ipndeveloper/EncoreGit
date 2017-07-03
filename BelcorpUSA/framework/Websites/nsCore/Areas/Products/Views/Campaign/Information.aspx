<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> >
    <%= Html.Term("CampaignInformation", "Campaign Information")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            exportE();
        });

        function exportE() {
            $('#exportToExcel').click(function () {
                
                var query = $("#queryInputFilter").val();
                var sapSku = $("#sapSkuInputFilter").val();
                var type = $("#typeSelectFilter").val();
                var periodID = $("#periodIDSelectFilter").val();
                var parameters = '?parameters=' + query + "*" + sapSku + "*" + type + "*" + periodID;
                window.location = '<%=ResolveUrl("~/Products/Products/CampaignInformationExport")%>' + parameters;
            });
        }

    </script>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("CampaignInformation", "Campaign Information")%>
        </h2>
    </div>
    <% 
        Html.PaginatedGrid<ProductSearchData>("~/Products/Products/GetCampaignInformation")
        .AddSelectFilter(Html.Term("Period"), "periodID", TempData["Periods"] as Dictionary<string, string>)
        .AddSelectFilter(Html.Term("Type"), "type", new Dictionary<string, string>() { { "0", Html.Term("SelectaType", "Select a Type...") } }.AddRange(SmallCollectionCache.Instance.ProductTypes.ToDictionary(pt => pt.ProductTypeID.ToString(), pt => pt.GetTerm())))
        .AddInputFilter(Html.Term("SKUorName", "SKU or Name"), "query", ViewData["Query"])
        .AddInputFilter(Html.Term("SAPSKU", "SAP SKU"), "sapSku", ViewData["SAPSKU"])
        .AddColumn(Html.Term("Period"), "PeriodID", true)
        .AddColumn(Html.Term("CUV"), "SKU", true)
        .AddColumn(Html.Term("ExternalCode"), "External Code", true)
        .AddColumn(Html.Term("Product"), "Name", true)
        .AddColumn(Html.Term("Material"), "SAPSKU", true)
        .AddColumn(Html.Term("Kit"), "IsKit", true)
        .AddColumn(Html.Term("OfferType"), "OfferType", true)
        .AddColumn(Html.Term("Retail"), "PrecioTabela", true)
        .AddColumn(Html.Term("CommissionablePrice"), "PrecioComision", true)
        .AddColumn(Html.Term("QV"), "Puntos", true)
        .AddColumn(Html.Term("Wholesale"), "PrecioPracticado", true)
        .AddColumn(Html.Term("Type"), "Tipo", true)
        .AddColumn(Html.Term("Categories"), "Categorias", true)
        .AddColumn(Html.Term("Catalogs"), "Catalogos", true)
        .AddColumn(Html.Term("Status"), "Situacion", true)
        .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))
        .ClickEntireRow()
        .Render();
    %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
