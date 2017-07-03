<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/LogiticsProvManagement.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
 <a href="<%= ResolveUrl("~/Logistics") %>">
		    <%= Html.Term("Logistics") %></a> >
	    <%= Html.Term("LogisticProvManagement", "Logistic Prov Management")%> >
        <%= Html.Term("BrowseLogisticProvider", "Browse Logistic Provider")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(function () {

        var LogisticsProviderID = $('<input type="hidden" id="LogisticsProviderID" class="Filter" />').val('');
        $('#logisticsProviderInputFilter').change(function () {
            LogisticsProviderID.val('');
        });
        $('#logisticsProviderInputFilter').removeClass('Filter').after(LogisticsProviderID).css('width', '275px')
				.val('')
				.watermark('<%= Html.JavascriptTerm("logisticsProviderSearch", "Look up Prov. by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Logistics/searchProv") %>', { onSelect: function (item) {
				    LogisticsProviderID.val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});


        $('.btnEdit').live('click', function () {
            
            var obj = $(this).closest('tr'), orderShipmentID = obj.attr('data-id');

            $.get('<%= ResolveUrl("~/Logistics/Logistics/EditChangeLogisticProviderModal")%>', {
                OrderShipmentID: orderShipmentID
            },
				function (result) {

				    $('#newsletterModal').html(result);
				    $('#newsletterModal').jqmShow();

				})
        });


        $('#newsletterModal').jqm({
            modal: false
        });


    });


	</script>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("BrowseOrdersAllocate", "Browse Orders to Allocate")%>
        </h2>       
	</div>
    <% Html.PaginatedGrid<OrderLogisticProviderSearchData>("~/Logistics/Logistics/GetOrdersAllocate")
        .AutoGenerateColumns()
        .HideClientSpecificColumns_()
        .AddInputFilter(Html.Term("OrderNumber"), "OrderNumber")
        .AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", "1/1/2000", true)
        .AddInputFilter(Html.Term("To", "To"), "endDate", DateTime.Now.ToShortDateString(), true)            
        .AddInputFilter(Html.Term("logisticsProvider", "logisticsProvider"), "logisticsProvider")
        .AddSelectFilter(Html.Term("Campaign"), "PeriodID", new Dictionary<string, string>() { { "", Html.Term("SelectaCampaign", "Select a Campaign...") } }.AddRange(Periods.GetAllPeriods()), startingValue: 0)
        .ClickEntireRow()               
		.Render(); %>

    <div id="newsletterModal" class="jqmWindow LModal">
    </div>
</asp:Content>




