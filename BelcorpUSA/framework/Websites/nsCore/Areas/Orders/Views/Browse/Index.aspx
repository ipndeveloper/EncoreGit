<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.OrderSearchParameters>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <%var account = Model.ConsultantOrCustomerAccountID.HasValue ? NetSteps.Data.Entities.Account.LoadSlim(Model.ConsultantOrCustomerAccountID.Value) : new AccountSlimSearchData(); %>
    <script type="text/javascript">
    	$(function () {
    		var accountId = $('<input type="hidden" id="accountIdFilter" class="Filter" />').val('<%= Model.ConsultantOrCustomerAccountID %>');
    		$('#accountNumberOrNameInputFilter').change(function () {
    			accountId.val('');
    		});
            $('#accountNumberOrNameInputFilter').removeClass('Filter').after(accountId).css('width', '275px')
				.val('<%= Model.ConsultantOrCustomerAccountID.HasValue ? account.FullName + " (#" + account.AccountNumber + ")" : "" %>')
				.watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
				    accountId.val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});
	        //$(".orderSearch").watermark("Look up by order ID");
	    });
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Orders") %>">
        <%= Html.Term("Orders") %></a> >
    <%= Html.Term("BrowseOrders", "Browse Orders") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--  Joey's split packaging UI and script
    <script type="text/javascript">
$(document).ready(function(){
	$("body").addClass("FullPane");
	
	var shipmentBoxContainer = "<div class=\"FL pad5 mr10 shipmentBox\" style=\"border:1px solid #c0c0c0;width:200px;\"><h3>Shipment Box <b class=\"shipmentBoxNo\"></b></h3></div>";
	var shipmentBoxProducts = "<ul></ul>";
	var shipmentBoxTracking = "<input type=\"text\" class=\"fullWidth newTrackingNumber\" /> ";

	// Qty inputs
	$('input.shipBoxQty').each(function () {
		var qtyVal = $(this).val();	
		if(qtyVal < 1) {
			$(this).parents('li').addClass('inactive');
		}
		$(this).keyup(function(){
			var newQty = $(this).val();
			if(newQty > 0) {$(this).parents('li').removeClass('inactive');	
			} else {
				$(this).parents('li').addClass('inactive');
			}
		});	
		
	});	
	
	
	// toggle open the splitting UI and do some stuff
	$('a.splitPackaging').click(function(){
		var targetWin = $(this).attr('rel');
		$('div#'+ targetWin).toggle();
		// identify the shipment boxes in the target window
		$('div#'+ targetWin+ ' div.shipmentBox').each(function(index,value){
			$(this).addClass('shipmentBox'+ (index+1));
			$('b.shipmentBoxNo',this).text((index+1));
			// watermark the tracking number inputs in each shipment box
			$('div#'+ targetWin+ ' input.newTrackingNumber').each(function(index,value){
				$(this).watermark('Shipment ' + (index+1) + ' tracking number');
			});	
		});
						
		return false;
	});
	
	// cancel
	$('a.cancelSplit').click(function(){
		$('.splitPackagingWindow').fadeOut();
		return false;
	});
	
	
	
});
</script>
<div class="UI-orderTracking">
                <input tupe="text" name="trackingNum" class="FL orderTrackingNo"  />
                <a href="#" class="FL ml10 splitPackaging" rel="splitOrderID111"><span>Split</span></a>
                <span class="clr"></span>
    <!-- Split Packing Window -->
                <div class="splitPackagingWindow" id="splitOrderID111">
                    <div class="brdrAll innerWrap">
                        
                            <div class="UI-mainBg GridUtility pad5">
                                <h2 class="FL pad5">Split Packaging and Tracking Numbers</h2>
                                <a href="#" class="FR UI-icon-container cancelSplit" title="Cancel"><span class="UI-icon icon-x"></span></a>
                                <span class="clr"></span>
                            </div>
                            
                        
                        <div class="UI-lightBg pad10 boxNoUtil">
                            <label class="FL mr10 bold">Divide order into how many shipments?</label><div class="FL"><input type="text" class="qty mr10" value="2" name="boxCount" /><a href="#" class="addNewShipmentBox"><span>Apply</span></a></div>
                            <span class="clr"></span>
                           
                        </div>
                        <span class="block mt10 ml10 lawyer">Begin by splitting up product quantities between the shipment boxes.</span>
                        <div class="pad10">
                        	<div class="shipBoxesWrapper">
                                <!-- shipment box (master) -->
                                <div class="FL pad5 mr10 shipmentBox" id="MasterShipBox">
                                    <h3>Shipment Box <b class="shipmentBoxNo"></b> <span class="FR lawyer">(master)</span></h3>
                                    <ul>
                                        <li class="pad5"><span class="FL block splitCol80">Product ABC</span><span class="FR mr10 block splitCol20"><input type="text" class="qty shipBoxQty" value="1" /></span><span class="clr"></span></li>
                                        <li class="Alt pad5"><span class="FL block splitCol80">Product XYZ</span><span class="FR mr10 block splitCol20"><input type="text" class="qty shipBoxQty" value="1" /></span><span class="clr"></span></li>
                                        <li class="pad5"><span class="FL block splitCol80">Product YYY</span><span class="FR mr10 block splitCol20"><input type="text" class="qty shipBoxQty" value="3" /></span><span class="clr"></span></li>
                                    </ul>   
                                    <input type="text" class="fullWidth newTrackingNumber" /> 
                                </div>
                                <!--/ end shipment box (master) -->
                                
                                <!-- shipment box -->
                                <div class="FL pad5 mr10 shipmentBox">
                                    <h3>Shipment Box <b class="shipmentBoxNo"></b></h3>
                                    <ul>
                                        <li class="pad5 "><span class="FL block splitCol80">Product ABC</span><span class="FR mr10 block splitCol20"><input type="text" class="qty shipBoxQty" value="0" /></span><span class="clr"></span></li>
                                        <li class="Alt pad5 "><span class="FL block splitCol80">Product XYZ</span><span class="FR mr10 block splitCol20"><input type="text" class="qty shipBoxQty" value="0" /></span><span class="clr"></span></li>
                                        <li class="pad5 "><span class="FL block splitCol80">Product YYY</span><span class="FR mr10 block splitCol20"><input type="text" class="qty shipBoxQty" value="0" /></span><span class="clr"></span></li>
                                    </ul>    
                                    <input type="text" class="fullWidth newTrackingNumber" /> 
                                </div>
                                <!--/ end shipment box -->
                            </div>
                            <!--/ end wrapper -->
                            
                            <span class="clr"></span>  
                        
                            <!-- save splitting -->
                            <hr />
                            <div class="mt10">
                                <a href="#" class="FL Button BigBlue"><span>Save</span></a>
                                <a href="#" class="FR cancel cancelSplit"><span>Cancel</span></a>
                                <span class="clr"></span>
                            </div>
                        
                        </div>
                        
                    </div>
                </div>
				<!--/ End Split Packaging Window -->
                </div>
    --%>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("BrowseOrders", "Browse Orders") %>
        </h2>
    </div>
    <%
        if (TempData["Error"] != null)
        { %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous; -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0; border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold; margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;" class="ui-icon icon-exclamation">
            <%= TempData["Error"].ToString() %></div>
    </div>
    <% } %>
    <% Html.PaginatedGrid<OrderSearchData>("~/Orders/Browse/GetOrders")
        .AutoGenerateColumns()
        .AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(SmallCollectionCache.Instance.OrderStatuses.ToDictionary(os => os.OrderStatusID.ToString(), os => os.GetTerm())), startingValue: Model.OrderStatusID.ToString())
        .AddSelectFilter(Html.Term("Type"), "type", new Dictionary<string, string>() { { "", Html.Term("SelectaType", "Select a Type...") } }.AddRange(SmallCollectionCache.Instance.OrderTypes.Where(ot => !ot.IsTemplate).ToDictionary(ot => ot.OrderTypeID.ToString(), ot => ot.GetTerm())), startingValue: Model.OrderTypeID.ToString())
        .AddSelectFilter(Html.Term("Market"), "market", new Dictionary<string, string>() { { "", Html.Term("SelectaMarket", "Select a Market...") } }.AddRange(SmallCollectionCache.Instance.Markets.Where(c => c.Active).ToDictionary(ot => ot.MarketID.ToString(), ot => ot.Name)), startingValue: ApplicationContext.Instance.CurrentCountryID)
        .AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", Model.StartDate.HasValue ? Model.StartDate.ToShortDateString() : Html.Term("StartDate", "Start Date")/*new DateTime(1900, 1, 1).ToShortDateString()*/, true)
        .AddInputFilter(Html.Term("To", "To"), "endDate", Model.EndDate.HasValue ? Model.EndDate.ToShortDateString() : Html.Term("EndDate", "End Date")/*DateTime.Now.ToShortDateString()*/, true, true)
        .AddInputFilter(Html.Term("OrderNumber", "Order Number"), "orderNumber", Model.OrderNumber)
        .AddInputFilter(Html.Term("AccountNumberOrName", "Account Number or Name"), "accountNumberOrName")
        .AddInputFilter(Html.Term("InvoiceNumber", "Invoice Number"), "invoiceNumber")
        .AddInputFilter(Html.Term("Last4DigitsOfCreditCard", "Last 4 Digits of Credit Card"), "lastFour", Model.CreditCardLastFourDigits)
        .AddSelectFilter(Html.Term("Period"), "period", new Dictionary<string, string>() { { "", Html.Term("SelectPeriod", "Select a Period...") } }.AddRange(Periods.GetAllPeriods()), startingValue: Model.OrderStatusID.ToString())
        .ClickEntireRow()
        .CanBeShipped(true, "~/Orders/Shipping/PrintPackingSlip")
		.Render(); %>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
