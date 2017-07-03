<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/RoutesManagement.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.RouterParametros>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="SectionHeader">
		<h2>
			<%= Html.Term("Logistics", "Logistics")%>
        </h2>
        <%= Html.Term("BrowseRoute", "Browse Route")%> | <a href="<%= ResolveUrl("~/Logistics/Routes/NewRoute") %>"><%= Html.Term("AddaNewRoute", "Add a New Route")%></a>
	</div>
    <% Html.PaginatedGrid<RoutesData>("~/Logistics/Routes/GetRoutes")
        .AutoGenerateColumns()
        .HideClientSpecificColumns_()
        .AddInputFilter(Html.Term("Route", "Route"), "route")
        .CanChangeStatus(true, false, "~/Logistics/Routes/ChangeStatus")
        .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))      
        .ClickEntireRow()
		.Render(); %>
         <iframe   name ="frmExportar" id="frmExportar" style="display:none" src=""></iframe>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
    $(function () {
        var RouteID = $('<input type="hidden" id="RouteID" class="Filter" />').val('');
        $('#routeInputFilter').change(function () {
            RouteID.val('');
        });
        $('#routeInputFilter').removeClass('Filter').after(RouteID).css('width', '275px')
				.val('')
				.watermark('<%= Html.JavascriptTerm("routeSearch", "Look up route by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Routes/searchRoutes") %>', { onSelect: function (item) {
				    RouteID.val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});

        $('#exportToExcel').click(function () {
            var routeId = $('#RouteID').val();

            var url = '<%= ResolveUrl("~/Logistics/Routes/ExportRoutes") %>';

                var parameters = {
                    routeId: routeId
                };

                url = url + '?' + $.param(parameters).toString();

                $("#frmExportar").attr("src", url);
            
        });
    });
	</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
 <a href="<%= ResolveUrl("~/Products") %>">
		    <%= Html.Term("Logistics") %></a> >
	    <%= Html.Term("RoutesManagement", "Routes Management")%>
</asp:Content>
