<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<script type="text/javascript">
    $(function () {
        $('#txtOrderNumberFilter').watermark('Order #');
        $('#orderNumberFilter').watermark('<%= Html.JavascriptTerm("OrderID", "Order ID") %>');
        $('#recentOrderHistoryContainer').ajaxStop(function () {
            $('#recentOrderHistoryContainer .ReturnOrder').css('color', 'Red');
            $('#recentOrderHistoryContainer .AutoShipOrder').css('color', 'Green');
        });
    });
</script>
<div class="OrderHistoryPreview">
	<h3 class="FL"><a href="<%= ResolveUrl("~/Accounts/OrderHistory") %>" style="float: none;"
		title="Go to full order history"><%= Html.Term("RecentOrders", "Recent Orders") %></a></h3>
   <%-- <div id="paginatedGridFilters" class="FR">
        <select id="orderStatusFilter" class="FL Filter">
            <option selected="selected" value="">
                <%= Html.Term("SelectStatus", "Select Status...") %></option>
            <% foreach (var status in SmallCollectionCache.Instance.OrderStatuses)
	            { %>
                    <option value="<%= status.OrderStatusID %>">
                        <%= status.GetTerm() %>
                    </option>
              <%} %>
        </select>
        <div class="FL FancySearch">
            <input id="orderNumberFilter" class="Filter TextInput" type="text"/>
            <a id="btnFilter" class="ModSearch SearchIcon" href="javascript:void(0);">
                <span><%= Html.Term("Go")%></span>
            </a>   
        </div>
        <span class="ClearAll"></span>
    </div>--%>
    <span class="ClearAll"></span>
</div>
 <%
        Dictionary<int, string> dcOwnOrders = (ViewBag.dcOwnOrders as Dictionary<int, string>) ?? new Dictionary<int, string>();
    %>
<div id="recentOrderHistoryContainer" class="RecentOrders">
	<% Html.PaginatedGrid("~/Accounts/Overview/GetOrderHistory")
        .AddSelectFilter(Html.Term("Status"), "orderStatus", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(SmallCollectionCache.Instance.OrderStatuses.ToDictionary(os => os.OrderStatusID.ToString(), os => os.GetTerm())))
            .AddInputFilter(Html.Term("OrderNumber", "Order Number"), "orderNumber")
            /*CS.19AG2016.Ininio.Nuevo Filtro */
                .AddSelectFilter(Html.Term("OwnOrders", "Own Orders"), "OwnOrder", new Dictionary<int, string>() { { 1, Html.Term("OwnOrders", "Own Orders") } }.AddRange(dcOwnOrders))
            /*CS.19AG2016.Ininio.Nuevo Filtro */
            .AddColumn(Html.Term("AccountNumber"), "AccountNumber", true)
            .AddColumn(Html.Term("OrderNumber"), "OrderNumber", true)
			.AddColumn(Html.Term("Started On"), "DateCreatedUTC", true, true, NetSteps.Common.Constants.SortDirection.Descending)
			.AddColumn(Html.Term("Type"), "OrderType.TermName", true)
			.AddColumn(Html.Term("Status"), "OrderStatus.TermName", true)
			.AddColumn(Html.Term("ShippedOn", "Shipped On"), "DateShippedUTC", false)
			.AddColumn(Html.Term("Subtotal"), "Subtotal", true)
			.AddColumn(Html.Term("GrandTotal", "Grand Total"), "GrandTotal", true)
			.Render(); %>
</div>
<div class="ColorKey">
	<%= Html.Term("Key")%>: <span class="ReturnOrder" style="color: Red;">
		<%= Html.Term("Return Order", "Return Order") %></span> | <span class="AutoShipOrder"
			style="color: Green;"><%= Html.Term("Autoship", "Autoship") %></span>
</div>
