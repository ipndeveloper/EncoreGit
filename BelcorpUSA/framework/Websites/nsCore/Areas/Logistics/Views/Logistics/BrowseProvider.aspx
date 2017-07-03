<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/LogiticsProvManagement.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="SectionHeader">
		<h2>
			<%= Html.Term("Logistics", "Logistics")%>
        </h2>
        <%= Html.Term("BrowseLogisticProvider", "Browse Logistic Provider")%> | <a href="<%= ResolveUrl("~/Logistics/Logistics/ProviderDetails/") %>"><%= Html.Term("AddNewLogisticProvider", "Add New Logistic Provider")%></a>
	</div>

    <% Html.PaginatedGrid<LogisticsProviderSearData>("~/Logistics/Logistics/GetLogisticsProv")
        .AutoGenerateColumns()
        .HideClientSpecificColumns_()
        .AddInputFilter(Html.Term("logisticsProvider", "logisticsProvider"), "logisticsProvider")
        .AddSelectFilter(Html.Term("Status"), "active", new Dictionary<string, string>() { { "1", Html.Term("Active") }, 
                                                                                           { "0", Html.Term("Inactive") } })
        .CanChangeStatus(true, true, "~/Logistics/Logistics/ChangeStatusProv")
        .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))     
        .ClickEntireRow()
		.Render(); %>
        <iframe   name ="frmExportar" id="frmExportar" style="display:none" src=""></iframe>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript" >
    $(function () {

        var LogisticsProviderID = $('<input type="hidden" id="LogisticsProviderID" class="Filter" />').val('');

        $('#logisticsProviderInputFilter').change(function () {
            LogisticsProviderID.val('');
            SetParamSearchGridView("Name", $('#logisticsProviderInputFilter').val());
        });
        $('#logisticsProviderInputFilter').removeClass('Filter').after(LogisticsProviderID).css('width', '275px')
				.val('')
				.watermark('<%= Html.JavascriptTerm("logisticsProviderSearch", "Look up Prov. by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Logistics/searchProv") %>', { onSelect: function (item) {
				    LogisticsProviderID.val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
		});

        $('#exportToExcel').click(function () {
            var logisticsProviderID = $('#LogisticsProviderID').val();
            var name=$('#logisticsProviderInputFilter').val();

            var url = '<%= ResolveUrl("~/Logistics/Logistics/Exportproviders") %>';

            var parameters = {
                logisticsProviderID: logisticsProviderID,
                name: name
            };

            url = url + '?' + $.param(parameters).toString();

            $("#frmExportar").attr("src", url);

        });
    });
	</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
 <a href="<%= ResolveUrl("~/Logistics") %>">
		    <%= Html.Term("Logistics") %></a> >
	    <%= Html.Term("LogisticProvManagement", "Logistic Prov Management")%> >
        <%= Html.Term("BrowseLogisticProvider", "Browse Logistic Provider")%>
</asp:Content>

