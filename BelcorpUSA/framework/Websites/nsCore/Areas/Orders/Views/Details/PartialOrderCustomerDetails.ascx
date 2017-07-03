<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Orders.Models.Details.PartialOrderCustomerDetailsModel>" %>
<%@ Import Namespace="NetSteps.Data.Entities.Business.HelperObjects.OrderPackages" %>
<%@ Import Namespace="nsCore.Areas.Orders.Models.Details" %>
<%@ Import Namespace="NetSteps.Data.Entities.Business.Logic" %>
<%
	//Set the variables to page variables to only call to the model once.
	Order order = Model.Order;
	OrderCustomer customer = Model.OrderCustomer;
	bool isPartyOrder = Model.IsPartyOrder;
	bool isChildOfPartyOrder = Model.IsPartyAttachedOrder;
	bool isReturnOrder = Model.IsReturnOrder;
	bool isReplacementOrder = order.OrderTypeID == Constants.OrderType.ReplacementOrder.ToShort();
	bool isOrderAttachableToParty = (order.OrderTypeID == Constants.OrderType.OnlineOrder.ToShort()) ||
												(order.OrderTypeID == Constants.OrderType.PortalOrder.ToShort());
	Currency currency = Model.Currency;

	if (isPartyOrder || isChildOfPartyOrder)
	{
%>
<h3>
	<a href="#">
		<%= customer.FullName %>
		#<%= customer.AccountInfo.AccountNumber %>
		<% 
		if (customer.OrderCustomerTypeID == Constants.OrderCustomerType.Hostess.ToShort())
		{ 
		%>
		(<%= Html.Term("Host") %>)
		<%
		}
		if (customer.AccountID == order.ConsultantID)
		{ 
		%>
		(<%= Html.Term("Consultant", "Consultant")%>)
		<%
		}
		if (isChildOfPartyOrder)
		{
		%>
		(<%= Html.Term("GMP_OrderDetails_OnlineOrder", "Online Order") %>)
		<% 
		}
		%>
	</a>
</h3>
<%
	}
%>
<div>
	<%
		if (Model.ActingAsChildOrder && isChildOfPartyOrder)
		{
	%>
	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("Order", "Order") %>
			</td>
			<td>
				<% Html.RenderPartial("PartialOrderInformation", new PartialOrderInformationModel(customer.Order, (customer.Order.ParentOrderID ?? 0) > 0 ? true : false)); %>
			</td>
		</tr>
	</table>
	<%
		}
		var promotionOrderAdjustments = order.OrderAdjustments.Where(x => x.ExtensionProviderKey == NetSteps.Promotions.Service.PromotionProvider.ProviderKey && (x.OrderAdjustmentOrderLineModifications.Any() || x.OrderAdjustmentOrderModifications.Any()));
		if (promotionOrderAdjustments.Any())
		{
	%>
	<div class="UI-secBg mb5 pad5 promoNotification">
		<%= Html.Term("GMP_Promotions_OrderQualifiedForPromotionNotification", "* Note: This order qualified for the following promotions")%>:
		<div class="pad5 ml10" id="PromotionList">
			<%
			foreach (var promotionOrderAdjustment in promotionOrderAdjustments)
			{
			%>
			<div class="promoNotification">
				<%= Html.Term(promotionOrderAdjustment.Description, promotionOrderAdjustment.Description) %>
			</div>
			<%
			}
			%>
		</div>
	</div>
	<% 
		}
		nsCore.Areas.Orders.Models.Details.PartialOrderItemDetailsModel orderCustomerDetails = ViewBag.OrderCustomerDetails[customer.OrderCustomerID];
		Html.RenderPartial("PartialOrderItemDetails", orderCustomerDetails);
	%>

    <table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("ViewProductsLacks", "View Products Lacks")%>
			</td>
			<td>
                <table id="productLacks" width="100%" class="DataGrid">
                    <thead>
                        <tr class="GridColHead">
                            <th> <%= Html.Term("SKU", "SKU")%></th>
                            <th> <%= Html.Term("Product", "Product")%></th>
                            <th style="width: 9.091em;"> <%= Html.Term("Quantity", "Quantity")%></th>
                            <th> <%= Html.Term("Motive", "Motive")%></th>
                        </tr>
                    </thead>
                    <tbody id="first">
                    </tbody>
                </table>
			</td>
		</tr>
	</table>

	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("ShipTo", "Ship To")%>
			</td>
			<td>
				<%
					var orderShipments = isPartyOrder ? customer.OrderShipments : order.OrderShipments;

					if (customer.OrderShipments.Count == 0 && isPartyOrder)
					{
				%>
				<%= Html.Term("ShippingToParty", "Shipping to party")%>
				<%
					}
					else
					{
						foreach (var shipment in orderShipments)
						{
							if (shipment != null)
							{ 
				%>
				<span style="display:none;">
					<%= Html.Term("ShipmentId", "Shipment Id")%>:
					<%= shipment.OrderShipmentID %>
				</span>
				<br />
				<%= shipment.ToDisplay()%>
				<br />
				<%
							}
						}
					}
				%>
			</td>
		</tr>
	</table>
	<% 
		if (isPartyOrder || ViewBag.PackageInfoList.Count > 0)
		{ 
	%>
	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("DataPrevista", "Data Prevista")%>
			</td>
			<td>
				<%
			var allPackages = ViewBag.PackageInfoList as IEnumerable<OrderPackageInfoModel>;
			//var customerPackages = allPackages.Where(p => p.OrderCustomerID == customer.OrderCustomerID);

            foreach (var item in Order.GetOrderShipment(customer.OrderCustomerID))
            {%>
<%--               <span>                    
                    <%= Html.Term("ShipMethod", "Method")%>:
                     <%= (!String.IsNullOrEmpty(item.Name)
                          ? item.Name 
                          : Html.Term("NotAvailable", "Not avaialble")) %>
               </span> 
               <br />--%>
                <span>
                  <%= Html.Term("SeraEntregue", "O seu pedido será entregue:")%>
                  <%= item.DateStimate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo)%>
                </span>
<%--               <br />
                <span>
                  <%= Html.Term("TrackingNumber", "Tracking#") %>:
                    <%=Html.Term("NotAvailable", "Not avaialble")%>
                </span>
                <br />--%>
            <%}
            
            
			if (isPartyOrder && orderShipments != null && orderShipments.Count > 0)
			{
				%>
				<%= Html.Term("ShippingToParty", "Shipping to party") %>
				<% 
			}
				%>
			</td>
		</tr>
	</table>
	<% 
		}
	%>
	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("Payment", "Payment")%>
			</td>
			<td style="text-align: left;">
				<table class="DataGrid" width="100%">
					<tr class="GridColHead">
						<th>
							<%= Html.Term("Payment", "Payment")%>
						</th>
						<th>
							<%= Html.Term("Amount", "Amount")%>
						</th>
					</tr>
					<tbody>
						<% 
							if (customer.OrderPayments != null && customer.OrderPayments.Count > 0)
							{
								var orderCustomerOrderPayments = customer.OrderPayments.ToList();
								foreach (var orderPayment in orderCustomerOrderPayments)
								{
									string paymentColor = "black";
									if (orderPayment.OrderPaymentStatusID == Constants.OrderPaymentStatus.Declined.ToInt())
									{
										paymentColor = "red";
									}
									else if (orderPayment.OrderPaymentStatusID == Constants.OrderPaymentStatus.Completed.ToInt())
									{
										paymentColor = "green";
									}

                                    var descrip = PaymentMethods.GetTDescPaymentConfiguationByOrderPayment(orderPayment.OrderPaymentID);
                                    descrip = (descrip == "PREVIOS BALANCE") ? "Product Credit" : descrip;
                                    //KTORRES  se añadio la variable 'Amount' , porque en el Context no trae los valores del amount de algunos OrderPayments, finalmente para esos casos se valida con un store que ya existia
                                    var Amount = (orderPayment.Amount==0)?AccountPropertiesBusinessLogic.GetValueByID(12, orderPayment.OrderPaymentID).TotalAmount:orderPayment.Amount;
                                    if (Amount == null) Amount = 0;
                                    decimal amountCredit = (Session["ProductCredit"] != null ? decimal.Parse(Session["ProductCredit"].ToString()) : 0);
                                    decimal orderPaymentShow = 0;
                                    //decimal GrandTotal = Model.Order.GrandTotal.ToDecimal();
                                    var GrandTotal = Model.OrderCustomerTotals.GrandTotal;
                                    orderPaymentShow = (amountCredit > GrandTotal) ? GrandTotal : Amount;// orderPayment.Amount;
						%>

						<tr>
							<td>
								<a href="javascript:void(0);" title="View payment details" onclick="showPaymentModal(<%= orderPayment.OrderPaymentID %>);">
									 <%=descrip%></a>&nbsp;<span style="color: <%= paymentColor %>"><i>(<%=SmallCollectionCache.Instance.OrderPaymentStatuses.GetById(orderPayment.OrderPaymentStatusID).GetTerm()%>)</i></span>
							</td>
							<td>
								<%--<%= orderPaymentShow.GetRoundedNumber().ToString(currency)%>--%>
                                <%= orderPaymentShow.GetRoundedNumber().ToString("C", CoreContext.CurrentCultureInfo)%>

							</td>
						</tr>
						<%
								}
							}
							else if (order.OrderPayments.Count > 1)
							{
						%>
						<%= Html.Term("PaymentOnParty", "Payment on party")%>
						<%
							} 
						%>
					</tbody>
					<tr class="GridTotalBar">
						<%
							//Use ViewBag to create and store PartialOrderCustomerDetailsModel and pass this to the partial instead of the OC
							Html.RenderPartial("PartialOrderCustomerTotals", Model.OrderCustomerTotals);
						%>
					</tr>
					<tr style="font-size: 1.2em;">
						<td colspan="1" style="text-align: right">
							<%= Html.Term("PaymentsApplied", "Payments Applied")%>:<br />
							<%= Html.Term("BalanceDue", "Balance Due")%>:
						</td>
						<td>
							<b>
                                <%decimal balance = Order.GetProductCreditByAccount(order.ConsultantID); %>
								<%--<%decimal paymentsMade02 = customer.OrderPayments.Where(p => p.OrderPaymentStatusID == Constants.OrderPaymentStatus.Completed.ToShort()).Sum(p => p.Amount); %>
                                <%decimal paymentsMade = decimal.Parse(order.GrandTotal.ToString()) + ((balance < 0) ? balance*(-1) : 0); %>
--%>
							
                            <% 
                                
                                var paymentsMade = customer.OrderPayments.Where(y => y.Amount > 0).Sum(x => x.Amount);

                                var paymentsMadeN = customer.OrderPayments.Where(y => y.Amount < 0).Sum(x => x.Amount);
                                paymentsMadeN = paymentsMadeN * (-1);
                                var total = Model.OrderCustomerTotals.GrandTotal;
                                paymentsMade = paymentsMadeN + total;
                                
					    	%>      
                            <%= paymentsMade.ToString("C", CoreContext.CurrentCultureInfo)%>                          
                            <%--<%= paymentsMade.ToString(currency)%>--%>
                                </b><br />
							<%
								if (customer.OrderPayments != null && customer.OrderPayments.Count > 0)
								{
									decimal balanceDue = isPartyOrder ? customer.Balance.GetRoundedNumber() : order.Balance.GetRoundedNumber(); 
							%>
							<b><span style="color: <%= balanceDue > 0 ? "Red" : "Green" %>;">
							<%--	<%= balanceDue.GetRoundedNumber().ToString(currency)%></span></b>--%>

                            	<%= balanceDue.GetRoundedNumber().ToString("C", CoreContext.CurrentCultureInfo)%></span></b>
							<%
								}
								else if (order.OrderPayments.Count > 1)
								{ 
							%>
							(<%= Html.Term("PaymentOnParty", "Payment on party")%>)
							<%
								}
							%>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</div>
