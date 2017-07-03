<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Orders.Models.Details.PartialOrderItemDetailsModel>" %>
<%@ Import Namespace="NetSteps.Web.Extensions" %>
<%@ Import Namespace="NetSteps.Data.Common.Services" %>
<%@ Import Namespace="NetSteps.Encore.Core.IoC" %>
<%@ Import Namespace="NetSteps.Data.Entities.Business.HelperObjects.SearchData" %>
<% var customer = Model.OrderCustomer;
   var order = Model.Order;
   bool isReturnOrder = Model.IsReturnOrder;
   bool isReplacementOrder = Model.IsReplacementOrder;
   Currency currency = Model.Currency;
%>
<table class="FormTable Section OrderCustomer" width="100%">
	<tr>
		<td class="FLabel ">
			<%= Html.Term("Products", "Products")%>
		</td>
		<td>
			<!-- Products In Order -->
			<table width="100%" class="DataGrid">
				<tr class="GridColHead">
					<th>
						<%= Html.Term("SKU", "SKU")%>
					</th>
					<th>
						<%= Html.Term("Product", "Product")%>
					</th>
                    <th>
                        <%= Html.Term("RetailPerItem", "Retail Per Item")%>
                    </th>
					<th>
						<%= Html.Term("PricePerItem", "Price Per Item")%>
					</th>
					<th style="width: 100px;">
						<%= Html.Term("Quantity", "Quantity")%>
					</th>
					<th style="width: 100px;">
						<%= Html.Term("QV", "QV")%>
					</th>
					<th>
						<%= Html.Term("Subtotal", "Subtotal")%>
					</th>

                    <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%string valorSCV = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "SCV");
                                if (valorSCV == "S")
                                { %>
					<th>
						<%= Html.Term("CommissionablePrice", "Commissionable Price")%>
					</th>
                     <%}%>
                            <%--CS.03MAY2016.Fin.Muestra CV--%>

					<th>
						<%= Html.Term("Price", "Price")%>
					</th>
					<% 
						if (isReturnOrder)
						{ 
					%>
					<th>
						<%= Html.Term("ReturnReason", "Return Reason")%>
					</th>
					<%
						}
						if (isReplacementOrder)
						{ 
					%>
					<th>
						<%= Html.Term("ReplacementItemNotes", "Notes")%>
					</th>
					<%
						} 
					%>
				</tr>
                
                     <% 
						if (isReturnOrder)
						{ 
					%>
				<tbody>
					<%
						var inventory = Model.Inventory;
                        int quantityTotal = 0;
						decimal qvTotal = 0;
						decimal subtotal = 0;
                        decimal SumatoriaSubtotal = 0;
						decimal commissionableTotal = 0;
                        // wv: 20160606 Validacion si tiene o no dispatch para ser visualizado
                        List<int> getListDispatchOrder = new List<int>();
                        getListDispatchOrder = OrderExtensions.getListDispatchOrderItems(order.OrderID);// wv:20160606 Se incluye lista de Dispatch x orderitemsID
                        foreach (nsCore.Areas.Orders.Models.Details.OrderItemDetailModel orderItemDetail in Model.OrderItemDetails.OrderBy(x => x.OrderItem.OrderItemID))
                        {

                            OrderItem orderItem = orderItemDetail.OrderItem;
                            OrderItemReturn itemReturn = orderItemDetail.OrderItemReturn;
                            /*CS.11AG2016.Inicio.GetProducts*/
                            var Productos = Product.LoadFullBySKU(orderItem.SKU);
                            /*CS.11AG2016.Inicio.GetProducts*/
                            // wv: 20160606 valida los productos que son Dispatch y los saca de la visualizacion normal de items
                            if (getListDispatchOrder.Exists(x => x.Equals(orderItem.OrderItemID))) // wv: 20160606 Validación de visualización del producto Dispatch
                            {
                                continue;
                            }

                            OrderItemReplacement itemReplacement = orderItemDetail.OrderItemReplacement;
                            var ChildOrderItems = orderItemDetail.OrderItem.ChildOrderItems;

                            var product = orderItemDetail.Product;
                            var orderItemPrices = orderItem.OrderItemPrices;
                            OrderItemPrice qvPrice = new OrderItemPrice();
                            OrderItemPrice cvPrice = new OrderItemPrice();
                            OrderItemPrice retailPrice = new OrderItemPrice();
                            foreach (var q in orderItemPrices)
                            {
                                var pType = ProductPriceType.Load(q.ProductPriceTypeID);
                                if (pType.Name.Equals("QV")) qvPrice = q;
                                if (pType.Name.Equals("CV")) cvPrice = q;
                                //if ( pType.Name.Equals("Retail")) retailPrice = q;

                                if (q.ProductPriceTypeID.Equals(customer.ProductPriceTypeID)) retailPrice = q;
                            }
                            qvTotal += qvPrice.UnitPrice * orderItem.Quantity;
                            quantityTotal += orderItem.Quantity;
                            //var ValidarProduct = Product.LoadFullBySKU(orderItem.SKU);
                            //if (ValidarProduct != null) quantityTotal += orderItem.Quantity;

                            decimal adjustedItemPrice = 0;
                            decimal preadjustedItemPrice = 0;
                            decimal itemPriceTotal = 0;
                            decimal adjustedCommissionTotal = 0;
                            decimal preadjustedCommissionTotal = 0;
                            decimal subtotalPrice = 0;
                            if (Model.Order.OrderStatusID == 19)
                            {
                                /*CS.Inicio*/
                                //adjustedItemPrice = orderItemDetail.OrderItem.ItemPrice;
                                /*CS.Fin*/
                                adjustedItemPrice = retailPrice.UnitPrice;
                                preadjustedItemPrice = retailPrice.OriginalUnitPrice.ToDecimal();
                                /*CS.Inicio*/
                                itemPriceTotal = orderItemDetail.OrderItem.ItemPrice;
                                /*CS.Fin*/
                                //itemPriceTotal = orderItemDetail.ItemPriceTotal;
                                adjustedCommissionTotal = cvPrice.UnitPrice;
                                preadjustedCommissionTotal = cvPrice.OriginalUnitPrice.ToDecimal();
                                subtotalPrice = itemPriceTotal * orderItem.Quantity;
                                commissionableTotal += cvPrice.UnitPrice * orderItem.Quantity;
                                subtotal += subtotalPrice;
                            }
                            else
                            {
                                adjustedItemPrice = retailPrice.UnitPrice;
                                preadjustedItemPrice = retailPrice.OriginalUnitPrice.ToDecimal();
                                /*CS.11AG2016.Inicio.Comentado*/
                                itemPriceTotal = orderItemDetail.ItemPriceTotal;
                                /*CS.11AG2016.Fin.Comentado*/
                                //itemPriceTotal = orderItemDetail.OrderItem.Quantity*orderItemDetail.OrderItem.ItemPrice;
                                adjustedCommissionTotal = cvPrice.UnitPrice;
                                /*CS.11AG2016.Inicio.SubTotal[Q*Precio]*/
                                preadjustedCommissionTotal = cvPrice.OriginalUnitPrice.ToDecimal();
                                subtotalPrice = preadjustedItemPrice * orderItem.Quantity;
                                /*CS.11AG2016.Fin.SubTotal[Q*Precio]*/
                                //subtotalPrice = orderItemDetail.OrderItem.ItemPrice*orderItemDetail.OrderItem.Quantity;
                                
                                if (itemPriceTotal == 0 & Productos != null)
                                {
                                    itemPriceTotal = Model.OrderItemDetails.Where(donde => donde.OrderItem.ProductID == orderItemDetail.OrderItem.ProductID).Sum(suma => suma.OrderItem.ItemPrice * suma.OrderItem.Quantity);
                                }
                                /*CS.11AG2016.Fin.SubTotal[Q*Precio]*/
                                commissionableTotal += cvPrice.UnitPrice * orderItem.Quantity;
                                subtotal += subtotalPrice;
                                SumatoriaSubtotal += orderItemDetail.OrderItem.ItemPrice * orderItem.Quantity;
                            }
                            // make unit prices negative... prices multiplied by the quantity will be negative already (quantity is negative on returns)
                            if (isReturnOrder)
                            {
                                adjustedItemPrice = -1 * adjustedItemPrice;
                                preadjustedItemPrice = -1 * preadjustedItemPrice;
                                adjustedCommissionTotal *= -1;
                                preadjustedCommissionTotal *= -1;
                            }
					%>
                    <%if (orderItem.ParentOrderItemID == null)
                      {
                          quantityTotal += orderItem.Quantity;
                          %>
					<tr>
                    <%--SKU--%>
						<td>
							<%: Html.Link(orderItem.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + orderItem.ProductID)%>
						</td>

                        <%--Nombre del Producto--%>
						<td>
							<%=product.Translations.Name()%>
							<br />
							<div>
								<% 
                          nsCore.Areas.Orders.Models.Details.OrderItemDetailModel itemDetailModel = Model.OrderItemDetails.FirstOrDefault(f => f.OrderItem == orderItem);
                          if (itemDetailModel != null)
                          {
                              foreach (var customText in itemDetailModel.Messages)
                              {
								%>
								<span>
									<%= customText%></span>
								<% 
                              }
                          }
								%>
							</div>
							<%
                          foreach (var description in orderItem.OrderAdjustmentOrderLineModifications.GroupBy(a => a.OrderAdjustmentID).Select(g => g.First().OrderAdjustment.Description))
                          { 
							%>
							<span class="promoName">
								<%= description%></span>
							<%
                          }
                          if (order.OrderStatusID != (int)Constants.OrderStatus.Pending && order.OrderStatusID != (int)Constants.OrderStatus.PendingError
                                                            && orderItem.GiftCards != null && orderItem.GiftCards.Any())
                          {
                              foreach (var gc in orderItem.GiftCards)
                              { 
							%>
							<br />
							&nbsp;&nbsp;&nbsp;<%= gc.Code%>
							<% 
                              }
                          }
                          if (orderItem.WasBackordered && order.OrderStatusID != Constants.OrderStatus.Shipped.ToInt())
                          {
							%>
							&nbsp;<%= Html.Term("WasBackordered", "(Backordered)")%>
							<%
                          }
                          if (product.IsStaticKit() || product.IsDynamicKit())
                          { 
							%>
							<span class="ClearAll"></span><a class="ViewKitContents TextLink Add" href="javascript:void(0);">
								<%= Html.Term("ViewKitContents", "View Kit Contents")%></a>
							<div class="KitContents" style="display: none;">
								<table width="100%" cellspacing="0">
									<tbody>
										<tr>
											<th>
												<%= Html.Term("SKU")%>
											</th>
											<th>
												<%= Html.Term("Product")%>
											</th>
											<th>
												<%= Html.Term("Quantity")%>
											</th>
										</tr>
										<%foreach (var objE in Order.GetProductDetails(Model.Order.OrderID).Where(x => x.ProductId == product.ProductID))
            {%>
										<tr>
											<td class="KitSKU">
												<%: Html.Link(objE.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + objE.ProductId)%>
											</td>
											<td>
												<%= objE.Name%> 
												<div>
													<%
                nsCore.Areas.Orders.Models.Details.OrderItemDetailModel message = Model.OrderItemDetails.FirstOrDefault(f => f.OrderItem == orderItem);
                if (message != null)
                {
                    foreach (var customText in message.Messages)
                    {
													%>
													<span>
														<%= customText%></span>
													<% 
                    }
                }
													%>
												</div>
											</td>
											<td>
												<%= Math.Abs(objE.Quantity)%>
											</td>
										</tr>
										<%
            }
										%>
									</tbody>
								</table>
							</div>
							<% 
                          }
							%>
						</td>

                        <%--Valor Tableta--%>
                        <td>
                        
                            <% 
                          var orderItemPriceRetail = orderItem.OrderItemPrices.Where(op => op.ProductPriceTypeID == Constants.ProductPriceType.Retail.ToInt()).FirstOrDefault();
                          decimal unitPriceRetail = 0m;

                          if (orderItemPriceRetail != null)
                              unitPriceRetail = orderItemPriceRetail.UnitPrice;  
                                %>
							<%--<%=unitPriceRetail.ToString(currency)%>--%>
                            <%=unitPriceRetail.ToString("C",CoreContext.CurrentCultureInfo)%>
						</td>

                        <%--Valor Practicado--%>
						<td>
							<% 
                          if (adjustedItemPrice != preadjustedItemPrice)
                          {
							%>
							<span class="block originalPrice strikethrough">

                            	<%= preadjustedItemPrice.ToString("C",CoreContext.CurrentCultureInfo)%></span> <span class="block discountPrice">
									<%= adjustedItemPrice.ToString("C", CoreContext.CurrentCultureInfo)%></span>

							<%--	<%= preadjustedItemPrice.ToString(currency)%></span> <span class="block discountPrice">
									<%= adjustedItemPrice.ToString(currency)%></span>--%>
							<%
                          }
                          else
                          {
							%>
							     <%--   <%= adjustedItemPrice.ToString(currency)%>--%>
                                    <%= adjustedItemPrice.ToString("C", CoreContext.CurrentCultureInfo)%>
							<%
                          }
							%>
						</td>

                        <%--Cantidad--%>
						<td>
							<%= Math.Abs(orderItem.Quantity)%>
						</td>

                        <%--Puntos--%>
						<td>
							<% 
                          if (qvPrice.UnitPrice != qvPrice.OriginalUnitPrice)
                          {
							%>
							<span class="block originalPrice strikethrough">
								<%--<%= (qvPrice.OriginalUnitPrice * (orderItem.Quantity)).ToString(currency)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                                <%= Convert.ToInt32(Convert.ToDecimal(qvPrice.OriginalUnitPrice) * (orderItem.Quantity))%>
                            </span> 
                            <span class="block discountPrice">
								<%--<%= (qvPrice.UnitPrice * (orderItem.Quantity)).ToString(currency)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                                <%= Convert.ToInt32(qvPrice.UnitPrice * (orderItem.Quantity))%>
                            </span>
							<%
                          }
                          else
                          {
							%>
							<%--<%= (qvPrice.UnitPrice*(orderItem.Quantity)).ToString(currency)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                            <%= Convert.ToInt32(qvPrice.UnitPrice * (orderItem.Quantity))%>
							<%
                          }
							%>
						</td>

                        <%--SubTotal--%>
						<td>
							<%--<%= subtotalPrice.ToString(order.CurrencyID)%>--%>
                            <%= subtotalPrice.ToString("C", CoreContext.CurrentCultureInfo)%>
						</td>

                        <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%
                          if (valorSCV == "S")
                          { %>
						<td>
							<%
                              if (preadjustedCommissionTotal != adjustedCommissionTotal)
                              {
							%>
							<span class="block originalPrice strikethrough">
								<%--<%= (preadjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString(currency)%></span> <span class="block discountPrice">
									<%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString(currency)%></span>--%>

                                       <%= (preadjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString("C", CoreContext.CurrentCultureInfo)%></span> <span class="block discountPrice">
									<%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString("C", CoreContext.CurrentCultureInfo)%></span>
							<%
                              }
                              else
                              {
							%>
							       <%-- <%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString(currency)%>--%>
                                    <%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString("C", CoreContext.CurrentCultureInfo)%>
							<%
                              }
							%>
						</td>
                         <%}%>
                            <%--CS.03MAY2016.Fin.Muestra CV--%>

                            <%--Precio Total--%>
						<td>
							<%
                          //if (adjustedItemPrice * orderItem.Quantity != (preadjustedItemPrice * orderItem.Quantity) && !isReturnOrder)
                          if (adjustedItemPrice * orderItem.Quantity != (preadjustedItemPrice * orderItem.Quantity))
                          {
							%>
							<span class="block originalPrice strikethrough">
                            	<%= (preadjustedItemPrice * orderItem.Quantity).ToString("C", CoreContext.CurrentCultureInfo)%></span> <span class="block discountPrice">
									<%= (adjustedItemPrice * orderItem.Quantity).ToString("C", CoreContext.CurrentCultureInfo)%></span>
                                    <%--	<%= (preadjustedItemPrice * orderItem.Quantity).ToString(currency)%></span> <span class="block discountPrice">
                                    <%= (adjustedItemPrice * orderItem.Quantity).ToString(currency)%></span>--%>
							<%
                          }
                          else
                          {
							%>
							   <%-- <%= itemPriceTotal.ToString(currency)%>--%>
                               <%= itemPriceTotal.ToString("C", CoreContext.CurrentCultureInfo)%>
							<%
                          }
							%>
						</td>
						<% 
                          if (itemReturn != null)
                          { 
						%>
						<td>
							<%= orderItemDetail.ReturnReason%>
						</td>
						<%
                          }
                          if (itemReplacement != null)
                          {
						%>
						<td>
							<%= orderItemDetail.OrderItemReplacement.Notes%>
						</td>
						<%
                          } 
						%>
					</tr>
					      <%
                      }
                      else/*Componentes*/
                      {
                          
                          decimal SubTotalOrderComponente = 0m;
                          if (Productos == null) continue;
                          quantityTotal += orderItem.Quantity;
                          %>
					<tr>
                    <%--SKU--%>
						<td>
							<%: Html.Link(orderItem.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + orderItem.ProductID)%>
						</td>

                        <%--Nombre Producto--%>
						<td>
							<%=product.Translations.Name()%>
							<br />
							<div>
								<% 
                          nsCore.Areas.Orders.Models.Details.OrderItemDetailModel itemDetailModel = Model.OrderItemDetails.FirstOrDefault(f => f.OrderItem == orderItem);
                          if (itemDetailModel != null)
                          {
                              foreach (var customText in itemDetailModel.Messages)
                              {
								%>
								<span>
									<%= customText%></span>
								<% 
                              }
                          }
								%>
							</div>
							<%
                          foreach (var description in orderItem.OrderAdjustmentOrderLineModifications.GroupBy(a => a.OrderAdjustmentID).Select(g => g.First().OrderAdjustment.Description))
                          { 
							%>
							<span class="promoName">
								<%= description%></span>
							<%
                          }
                          if (order.OrderStatusID != (int)Constants.OrderStatus.Pending && order.OrderStatusID != (int)Constants.OrderStatus.PendingError
                                                            && orderItem.GiftCards != null && orderItem.GiftCards.Any())
                          {
                              foreach (var gc in orderItem.GiftCards)
                              { 
							%>
							<br />
							&nbsp;&nbsp;&nbsp;<%= gc.Code%>
							<% 
                              }
                          }
                          if (orderItem.WasBackordered && order.OrderStatusID != Constants.OrderStatus.Shipped.ToInt())
                          {
							%>
							&nbsp;<%= Html.Term("WasBackordered", "(Backordered)")%>
							<%
                          }
                          if (product.IsStaticKit() || product.IsDynamicKit())
                          { 
							%>
							<span class="ClearAll"></span><a class="ViewKitContents TextLink Add" href="javascript:void(0);">
								<%= Html.Term("ViewKitContents", "View Kit Contents")%></a>
							<div class="KitContents" style="display: none;">
								<table width="100%" cellspacing="0">
									<tbody>
										<tr>
											<th>
												<%= Html.Term("SKU")%>
											</th>
											<th>
												<%= Html.Term("Product")%>
											</th>
											<th>
												<%= Html.Term("Quantity")%>
											</th>
										</tr>
										<%
              
              foreach (var objE in Order.GetProductDetails(Model.Order.OrderID).Where(x => x.ProductId == product.ProductID))
              {
                  SubTotalOrderComponente += objE.Importe;
                                            %>
										<tr>
											<td class="KitSKU">
												<%: Html.Link(objE.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + objE.ProductId)%>
											</td>
											<td>
												<%= objE.Name%> 
												<div>
													<%
                nsCore.Areas.Orders.Models.Details.OrderItemDetailModel message = Model.OrderItemDetails.FirstOrDefault(f => f.OrderItem == orderItem);
                if (message != null)
                {
                    foreach (var customText in message.Messages)
                    {
													%>
													<span>
														<%= customText%></span>
													<% 
                    }
                }
													%>
												</div>
											</td>
											<td>
												<%= Math.Abs(objE.Quantity)%>
											</td>
										</tr>
										<%
            }
										%>
									</tbody>
								</table>
							</div>
							<% 
                          }
							%>
						</td>

                        <%--Valor Tabela--%>
                        <td>
                            <% 
                          //var orderItemPriceRetail = orderItem.OrderItemPrices.Where(op => op.ProductPriceTypeID == Constants.ProductPriceType.Retail.ToInt()).FirstOrDefault();
                          decimal unitPriceRetail = 0m;

                          if (SubTotalOrderComponente != null)
                          {
                              subtotalPrice += SubTotalOrderComponente;
                              //customer.Subtotal += SubTotalOrderComponente;
                              
                           }
                                %>
							<%--<%=unitPriceRetail.ToString(currency)%>--%>
                            <%=unitPriceRetail.ToString("C", CoreContext.CurrentCultureInfo)%>
						</td>

                        <%--Valor Praticado--%>
						<td>
							<% 
                          if (adjustedItemPrice != preadjustedItemPrice)
                          {
							%>
							<span class="block originalPrice strikethrough">
							<%--	<%= preadjustedItemPrice.ToString(currency)%></span> <span class="block discountPrice">
									<%= adjustedItemPrice.ToString(currency)%></span>--%>
                                    	<%= preadjustedItemPrice.ToString("C", CoreContext.CurrentCultureInfo)%></span> <span class="block discountPrice">
									<%= adjustedItemPrice.ToString("C", CoreContext.CurrentCultureInfo)%></span>
							<%
                          }
                          else
                          {
							%>
							   <%-- <%= adjustedItemPrice.ToString(currency)%>--%>
                                <%= adjustedItemPrice.ToString("C", CoreContext.CurrentCultureInfo)%>
							<%
                          }
							%>
						</td>

                        <%--Quantity--%>
						<td>
							<%--<%= Math.Abs(orderItem.Quantity)%>--%>
						</td>
						<td>
							<% 
                          if (qvPrice.UnitPrice != qvPrice.OriginalUnitPrice)
                          {
							%>
							<span class="block originalPrice strikethrough">
								<%--<%= (qvPrice.OriginalUnitPrice * (orderItem.Quantity)).ToString(currency)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                                <%= Convert.ToInt32(Convert.ToDecimal(qvPrice.OriginalUnitPrice) * (orderItem.Quantity))%>
                            </span> 
                            <span class="block discountPrice">
								<%--<%= (qvPrice.UnitPrice * (orderItem.Quantity)).ToString(currency)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                                <%= Convert.ToInt32(qvPrice.UnitPrice * (orderItem.Quantity))%>
                            </span>
							<%
                          }
                          else
                          {
							%>
							<%--<%= (qvPrice.UnitPrice*(orderItem.Quantity)).ToString(currency)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                            <%= Convert.ToInt32(qvPrice.UnitPrice * (orderItem.Quantity))%>
							<%
                          }
							%>
						</td>

                        <%--SubTotalPrice--%>
						<td>
							<%--<%= subtotalPrice.ToString(order.CurrencyID)%>--%>
                            <%= subtotalPrice.ToString("C", CoreContext.CurrentCultureInfo)%>
						</td>
                        
                        <%--CV--%>
                        <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%
                          if (valorSCV == "S")
                          { %>
						<td>
							<%
                              if (preadjustedCommissionTotal != adjustedCommissionTotal)
                              {
							%>
							<span class="block originalPrice strikethrough">
                                  <%--  <%= (preadjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString(currency)%></span> <span class="block discountPrice">
                                    <%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString(currency)%></span>--%>
                                      <%= (preadjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString("C", CoreContext.CurrentCultureInfo)%></span> <span class="block discountPrice">
                                    <%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString("C", CoreContext.CurrentCultureInfo)%></span>
							<%
                              }
                              else
                              {
							%>
							    <%--<%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString(currency)%>--%>
                                <%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString("C", CoreContext.CurrentCultureInfo)%>
							<%
                              }
							%>
						</td>
                         <%}%>
                            <%--CS.03MAY2016.Fin.Muestra CV--%>


						<td>
							<%
                          //if (adjustedItemPrice * orderItem.Quantity != (preadjustedItemPrice * orderItem.Quantity) && !isReturnOrder)
                          if (adjustedItemPrice * orderItem.Quantity != (preadjustedItemPrice * orderItem.Quantity))
                          {
							%>
							<span class="block originalPrice strikethrough">
                              <%--  <%= (preadjustedItemPrice * orderItem.Quantity).ToString(currency)%></span> <span class="block discountPrice">
                                <%= (adjustedItemPrice * orderItem.Quantity).ToString(currency)%></span>--%>
                                    <%= (preadjustedItemPrice * orderItem.Quantity).ToString("C", CoreContext.CurrentCultureInfo)%></span> <span class="block discountPrice">
                                    <%= (adjustedItemPrice * orderItem.Quantity).ToString("C", CoreContext.CurrentCultureInfo)%></span>
							<%
                          }
                          else
                          {
							%>
							<%--<%= itemPriceTotal.ToString(currency)%>--%>
                            <%= itemPriceTotal.ToString("C",CoreContext.CurrentCultureInfo)%>
							<%
                          }
							%>
						</td>
						<% 
                          if (itemReturn != null)
                          { 
						%>
						<td>
							<%= orderItemDetail.ReturnReason%>
						</td>
						<%
                          }
                          if (itemReplacement != null)
                          {
						%>
						<td>
							<%= orderItemDetail.OrderItemReplacement.Notes%>
						</td>
						<%
                          } 
						%>
					</tr>
					      <% 
                      }
                        }
						// here's where we show the promotion items
						var modGroups = Model.AddedOrderItems.GroupBy(x => x.OrderAdjustmentOrderLineModifications.First(y => y.ModificationOperationID == Model.OrderAdjustmentAddOperationKindID).OrderAdjustment);
						foreach (var grouping in modGroups)
						{
					%>
					<tr class="UI-lightBg specialPromotionItem">
						<td>
						</td>
						<td>
							<span class="promoHeading">
								<%= Html.Term(grouping.Key.Description, grouping.Key.Description) %></span>
						</td>
						<td>
							$0.00
						</td>
						<td>
						</td>
						<td>
						</td>
						<td>
							$0.00
						</td>
					</tr>
					<%
							foreach (var item in grouping)
							{
								var product = inventory.GetProduct(item.ProductID.ToInt());
					%>
					<tr class="promoGiftLineItem">
						<td>
							<span class="FL mr5 UI-icon icon-bundle-arrow"></span><span class="FL promoItemSku">
								<%: Html.Link(item.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + item.ProductID)%></span>
						</td>
						<td class="promoItemName">
							<%= product.Translations.Name() %>
							<div>
								<%
								nsCore.Areas.Orders.Models.Details.OrderItemDetailModel message = Model.OrderItemDetails.FirstOrDefault(f => f.OrderItem.OrderItemID == item.OrderItemID);
								if (message != null)
								{
									foreach (var customText in message.Messages)
									{
								%>
								<span>
									<%= customText %></span>
								<% 
									}
								}
								%>
							</div>
							<%
								if (product.IsStaticKit() || product.IsDynamicKit())
								{
							%>
							<span class="ClearAll"></span><a class="ViewKitContents TextLink Add" href="javascript:void(0);">
								<%= Html.Term("ViewKitContents", "View Kit Contents") %></a>
							<div class="KitContents" style="display: none;">
								<table width="100%" cellspacing="0">
									<tbody>
										<tr>
											<th>
												<%= Html.Term("SKU") %>
											</th>
											<th>
												<%= Html.Term("Product") %>
											</th>
											<th>
												<%= Html.Term("Quantity") %>
											</th>
										</tr>
										<% 
									foreach (var relation in product.ChildProductRelations.DistinctBy(p => p.ChildProductID))
									{
										Product childProduct = inventory.GetProduct(relation.ChildProductID);
										%>
										<tr>
											<td class="KitSKU">
												<%: Html.Link(childProduct.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + childProduct.ProductID)%>
											</td>
											<td>
												<%= childProduct.Translations.Name() %>
											</td>
											<td>
												<%= product.ChildProductRelations.Count(p => p.ChildProductID == relation.ChildProductID) %>
											</td>
										</tr>
										<%
									}
										%>
									</tbody>
								</table>
							</div>
							<%
								}
							%>
						</td>
                        <td>
				        </td>
						<td>
							<span class="freeItemPrice">
								<%= Html.Term("Cart_FreeItemPrice", "FREE")%></span>
						</td>
						<td>
							<%= item.Quantity %>
						</td>
						<td>
						</td>
						<td>
						</td>
					</tr>
					<%
							}
						}
					%>
                    <% %>
                    <% 
                        var numeroOrden = Convert.ToInt32(Model.Order.OrderID);
                        List<getDispatchByOrder> loadDispatchProcessOrder = new List<getDispatchByOrder>();
                        loadDispatchProcessOrder = OrderExtensions.GetDispatchByOrder(numeroOrden);
                        var listG = loadDispatchProcessOrder.GroupBy(x => new { x.DispatchID, x.Ddescripcion }).Select(y => new GrupoDispatchByOrder()
                                    {
                                            DispatchID = y.Key.DispatchID,
                                            Ddescripcion = y.Key.Ddescripcion
                                        }).ToList();

                        foreach (var itemG in listG)
                        {
                            var listaCarga = loadDispatchProcessOrder.Where(donde => donde.DispatchID == itemG.DispatchID);
                            //var dd = listaCarga.ElementAt(0).OrderItemID
                            //var nonPromotionallyAddedProducts2 = customer.ParentOrderItems.Except(customer.GetPromotionallyAddedOrderItems());

                            List<OrderItem> listaOrdenItem = new List<OrderItem>();
                
                            foreach (nsCore.Areas.Orders.Models.Details.OrderItemDetailModel orderItemDetail in Model.OrderItemDetails.OrderBy(x => x.OrderItem.OrderItemID))
				            {
                                OrderItem orderItem = orderItemDetail.OrderItem;
                                if (listaCarga.Where(donde => donde.OrderItemID.ToString().Equals(orderItem.OrderItemID.ToString())).Count() > 0)
                                {
                                    listaOrdenItem.Add(orderItem);
                                }
                            }%>
                                    <tr class = "UI-lightBg specialPromotionItem">
                                            <td>
                                            </td>
                                            <td colspan = "5">
                                            <%=itemG.Ddescripcion%>
                                            </td>
                                        </tr>
                                        <%foreach (var item in listaOrdenItem)
                                        {%>
                                            <tr>
                                            <td data-label="@Html.Term("SKU", "SKU")" >
                                            <%: Html.Link(item.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + item.ProductID)%>
                                            </td>
                                            <td data-label="@Html.Term("Product", "Product")">
                                            <%=item.ProductName%>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            <%= Html.Term("Cart_FreeItemPrice", "FREE")%>
                                            </td>
                                            <td colspan = "4">
                                            </td>
                                            </tr>                            
                                        <%}
                                        }
                                        %>
				</tbody>
				<tbody>
					<tr class="GridTotalBar">
						<td colspan="4">
							&nbsp;
						</td>
                        <td>
                            <%=quantityTotal%>
                        </td>
						<td>
							<b><span class="customerQV">
								<%--<%=qvTotal.ToString(order.CurrencyID)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                                <%=Convert.ToInt32(qvTotal)%>
							</span></b>
						</td>
						<td>
							<b><span class="customerSubtotal">
                            <%--CS.Inicio.Comentado.10AG2016--%>
								<%--<%=subtotal.ToString(order.CurrencyID)%>--%>
                                <%--CS.Fin.Comentado.10AG2016--%>
                               <%-- <%= SumatoriaSubtotal.ToString(order.CurrencyID)%>--%>
                                <%= SumatoriaSubtotal.ToString("C", CoreContext.CurrentCultureInfo)%>
							</span></b>
						</td>

                        <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%if (valorSCV == "S"){%>
						<td>
							<b><span class="customerCommissionable">

							<%--	<%=commissionableTotal.ToString(order.CurrencyID)%>--%>

                            	<%=commissionableTotal.ToString("C", CoreContext.CurrentCultureInfo)%>
							</span></b>
						</td>
                        <%}%>
                <%--CS.03MAY2016.Fin.Muestra CV--%>

						<td>
							<b><span class="customerSubtotal">
								<%
									if (customer.AdjustedSubTotal != customer.Subtotal)
									{
								%>
								<span class="originalPrice strikethrough">
									<%--<%= customer.Subtotal.ToString(order.CurrencyID)%>--%>
                                    <%= customer.Subtotal.ToString(CoreContext.CurrentCultureInfo)%>
								</span>&nbsp; <span class="discountPrice">
									<%--<%= customer.AdjustedSubTotal.ToString(order.CurrencyID)%>--%>
                                    <%= customer.AdjustedSubTotal.ToString("C",CoreContext.CurrentCultureInfo)%>
								</span>
								<%
									}
									else
									{
								%>
								<%--    <%= customer.Subtotal.ToString(order.CurrencyID) %>--%>
                                    <%= customer.Subtotal.ToString(CoreContext.CurrentCultureInfo)%>
								<%
									}
								%>
							</span>
								<%= Html.Term("Subtotal", "Subtotal")%>
							</b>
						</td>
					</tr>
				</tbody>

                	<%
                        }
                        else
                        {
                            
					%>

                    <tbody>
					<%
						var inventory = Model.Inventory;
                        int quantityTotal = 0;
						decimal qvTotal = 0;
						decimal subtotal = 0;
						decimal commissionableTotal = 0;
                        // wv: 20160606 Validacion si tiene o no dispatch para ser visualizado
                        List<int> getListDispatchOrder = new List<int>();
                        getListDispatchOrder = OrderExtensions.getListDispatchOrderItems(order.OrderID);// wv:20160606 Se incluye lista de Dispatch x orderitemsID
						foreach (nsCore.Areas.Orders.Models.Details.OrderItemDetailModel orderItemDetail in Model.OrderItemDetails.OrderBy(x => x.OrderItem.OrderItemID))
						{
                            
							OrderItem orderItem = orderItemDetail.OrderItem;
							OrderItemReturn itemReturn = orderItemDetail.OrderItemReturn;

                            // wv: 20160606 valida los productos que son Dispatch y los saca de la visualizacion normal de items
                            if (getListDispatchOrder.Exists(x => x.Equals(orderItem.OrderItemID))) // wv: 20160606 Validación de visualización del producto Dispatch
                            {
                                continue;
                            }
 
							OrderItemReplacement itemReplacement = orderItemDetail.OrderItemReplacement;
                            var ChildOrderItems = orderItemDetail.OrderItem.ChildOrderItems;

							var product = orderItemDetail.Product;
							var orderItemPrices = orderItem.OrderItemPrices;
							OrderItemPrice qvPrice = new OrderItemPrice();
                            OrderItemPrice cvPrice = new OrderItemPrice();
                            OrderItemPrice retailPrice = new OrderItemPrice();
							foreach (var q in orderItemPrices)
							{
								var pType = ProductPriceType.Load(q.ProductPriceTypeID);
								if (pType.Name.Equals("QV")) qvPrice = q;
                                if (pType.Name.Equals("CV")) cvPrice = q;
                                //if ( pType.Name.Equals("Retail")) retailPrice = q;

                                if (q.ProductPriceTypeID.Equals(customer.ProductPriceTypeID)) retailPrice = q;
                             }
							qvTotal += qvPrice.UnitPrice * orderItem.Quantity;
                            quantityTotal += orderItem.Quantity;
                            
                            decimal adjustedItemPrice = 0;
                            decimal preadjustedItemPrice = 0;
                            decimal itemPriceTotal = 0;
                            decimal adjustedCommissionTotal = 0;
                            decimal preadjustedCommissionTotal = 0;
                            decimal subtotalPrice = 0;
                            if (Model.Order.OrderStatusID == 19)
                            {
                                adjustedItemPrice = orderItemDetail.OrderItem.ItemPrice;
                                preadjustedItemPrice = retailPrice.OriginalUnitPrice.ToDecimal();
                                itemPriceTotal = orderItemDetail.OrderItem.ItemPrice;
                                adjustedCommissionTotal = cvPrice.UnitPrice;
                                preadjustedCommissionTotal = cvPrice.OriginalUnitPrice.ToDecimal();
                                subtotalPrice = itemPriceTotal * orderItem.Quantity;
                                commissionableTotal += cvPrice.UnitPrice * orderItem.Quantity;
                                subtotal += subtotalPrice;
                            }
                            else
                            {
                                adjustedItemPrice = retailPrice.UnitPrice;
                                preadjustedItemPrice = retailPrice.OriginalUnitPrice.ToDecimal();
                                itemPriceTotal = orderItemDetail.ItemPriceTotal;
                                adjustedCommissionTotal = cvPrice.UnitPrice;
                                preadjustedCommissionTotal = cvPrice.OriginalUnitPrice.ToDecimal();
                                subtotalPrice = preadjustedItemPrice * orderItem.Quantity;
                                commissionableTotal += cvPrice.UnitPrice * orderItem.Quantity;
                                subtotal += subtotalPrice;
                            }
                            // make unit prices negative... prices multiplied by the quantity will be negative already (quantity is negative on returns)
							if (isReturnOrder)
							{
								adjustedItemPrice = -1 * adjustedItemPrice;
								preadjustedItemPrice = -1 * preadjustedItemPrice;
								adjustedCommissionTotal *= -1;
								preadjustedCommissionTotal *= -1;
							}
					%>
                    <%if (orderItem.ParentOrderItemID == null)
                      {%>
					<tr>
                    <%--SKU--%>
						<td>
							<%: Html.Link(orderItem.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + orderItem.ProductID)%>
						</td>
                        <%--Nombre del Producto--%>
						<td>
							<%=product.Translations.Name()%>
							<br />
							<div>
								<% 
							nsCore.Areas.Orders.Models.Details.OrderItemDetailModel itemDetailModel = Model.OrderItemDetails.FirstOrDefault(f => f.OrderItem == orderItem);
							if (itemDetailModel != null)
							{
								foreach (var customText in itemDetailModel.Messages)
								{
								%>
								<span>
									<%= customText %></span>
								<% 
								}
							}
								%>
							</div>
							<%
							foreach (var description in orderItem.OrderAdjustmentOrderLineModifications.GroupBy(a => a.OrderAdjustmentID).Select(g => g.First().OrderAdjustment.Description))
							{ 
							%>
							<span class="promoName">
								<%= description %></span>
							<%
							}
							if (order.OrderStatusID != (int)Constants.OrderStatus.Pending && order.OrderStatusID != (int)Constants.OrderStatus.PendingError
															  && orderItem.GiftCards != null && orderItem.GiftCards.Any())
							{
								foreach (var gc in orderItem.GiftCards)
								{ 
							%>
							<br />
							&nbsp;&nbsp;&nbsp;<%= gc.Code %>
							<% 
								}
							}
							if (orderItem.WasBackordered && order.OrderStatusID != Constants.OrderStatus.Shipped.ToInt())
							{
							%>
							&nbsp;<%= Html.Term("WasBackordered", "(Backordered)") %>
							<%
							}
                            if (product.IsStaticKit() || product.IsDynamicKit())
                            { 
							%>
							<span class="ClearAll"></span><a class="ViewKitContents TextLink Add" href="javascript:void(0);">
								<%= Html.Term("ViewKitContents", "View Kit Contents")%></a>
							<div class="KitContents" style="display: none;">
								<table width="100%" cellspacing="0">
									<tbody>
										<tr>
											<th>
												<%= Html.Term("SKU")%>
											</th>
											<th>
												<%= Html.Term("Product")%>
											</th>
											<th>
												<%= Html.Term("Quantity")%>
											</th>
										</tr>
										<%foreach (var objE in Order.GetProductDetails(Model.Order.OrderID).Where(x => x.ProductId == product.ProductID))
                                            {%>
										<tr>
											<td class="KitSKU">
												<%: Html.Link(objE.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + objE.ProductId)%>
											</td>
											<td>
												<%= objE.Name%> 
												<div>
													<%
                                    nsCore.Areas.Orders.Models.Details.OrderItemDetailModel message = Model.OrderItemDetails.FirstOrDefault(f => f.OrderItem == orderItem);
                                    if (message != null)
                                    {
                                        foreach (var customText in message.Messages)
                                        {
													%>
													<span>
														<%= customText%></span>
													<% 
                                        }
                                    }
													%>
												</div>
											</td>
											<td>
												<%= Math.Abs(objE.Quantity)%>
											</td>
										</tr>
										<%
                                }
										%>
									</tbody>
								</table>
							</div>
							<% 
                            }
							%>
						</td>

                        <%--Valor Tableta--%>
                        <td>
                            <% 
                                    var orderItemPriceRetail = orderItem.OrderItemPrices.Where(op => op.ProductPriceTypeID == Constants.ProductPriceType.Retail.ToInt()).FirstOrDefault();
                                    decimal unitPriceRetail = 0m;
                                    
                                    if (orderItemPriceRetail != null)
                                       unitPriceRetail = orderItemPriceRetail.UnitPrice;  
                                %>
						<%--	<%=unitPriceRetail.ToString(currency)%>--%>
                        	<%=unitPriceRetail.ToString("C",CoreContext.CurrentCultureInfo)%>
						</td>

                        <%--Valor Practicado--%>
						<td>
							<% 
							if (adjustedItemPrice != preadjustedItemPrice)
							{
							%>
							<span class="block originalPrice strikethrough">
<%--                                    <%= preadjustedItemPrice.ToString(currency) %></span> <span class="block discountPrice">
                                    <%= adjustedItemPrice.ToString(currency) %></span>--%>

                            <%= preadjustedItemPrice.ToString("C", CoreContext.CurrentCultureInfo)%></span> <span class="block discountPrice">
                            <%= adjustedItemPrice.ToString("C", CoreContext.CurrentCultureInfo)%></span>
							<%
							}
							else
							{
							%>
							    <%--<%= adjustedItemPrice.ToString(currency)%>--%>
                                <%= adjustedItemPrice.ToString("C", CoreContext.CurrentCultureInfo)%>
							<%
							}
							%>
						</td>

                        <%--Cantidad--%>
						<td>
							<%= Math.Abs(orderItem.Quantity) %>
						</td>

                        <%--Puntos--%>
						<td>
							<% 
							if (qvPrice.UnitPrice != qvPrice.OriginalUnitPrice )
							{
							%>
							<span class="block originalPrice strikethrough">
								<%--<%= (qvPrice.OriginalUnitPrice * (orderItem.Quantity)).ToString(currency)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                                <%= Convert.ToInt32(Convert.ToDecimal(qvPrice.OriginalUnitPrice) * (orderItem.Quantity))%>
                            </span> 
                            <span class="block discountPrice">
								<%--<%= (qvPrice.UnitPrice * (orderItem.Quantity)).ToString(currency)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                                <%= Convert.ToInt32(qvPrice.UnitPrice * (orderItem.Quantity))%>
                            </span>
							<%
							}
							else
							{
							%>
							<%--<%= (qvPrice.UnitPrice*(orderItem.Quantity)).ToString(currency)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                            <%= Convert.ToInt32(qvPrice.UnitPrice * (orderItem.Quantity))%>
							<%
							}
							%>
						</td>

                        <%--SubTotal--%>
						<td>
							<%--<%= subtotalPrice.ToString(order.CurrencyID)%>--%>
                            <%= subtotalPrice.ToString("C",CoreContext.CurrentCultureInfo)%>
						</td>

                        <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%
                                if (valorSCV == "S")
                                { %>
						<td>
							<%
							if (preadjustedCommissionTotal != adjustedCommissionTotal )
							{
							%>
							<span class="block originalPrice strikethrough">
								<%= (preadjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString(currency)%></span> <span class="block discountPrice">
									<%= (adjustedCommissionTotal *Math.Abs(orderItem.Quantity)).ToString(currency)%></span>
							<%
							}
							else
							{
							%>
							   <%-- <%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString(currency)%>--%>
                                <%= (adjustedCommissionTotal * Math.Abs(orderItem.Quantity)).ToString("C", CoreContext.CurrentCultureInfo)%>
							<%
							}
							%>
						</td>
                         <%}%>
                            <%--CS.03MAY2016.Fin.Muestra CV--%>

                            <%--Precio Total--%>
						<td>
							<%
                            //if (adjustedItemPrice * orderItem.Quantity != (preadjustedItemPrice * orderItem.Quantity) && !isReturnOrder)
                          if (adjustedItemPrice * orderItem.Quantity != (preadjustedItemPrice * orderItem.Quantity))
							{
							%>
							<span class="block originalPrice strikethrough">
                                 <%--   <%= (preadjustedItemPrice * orderItem.Quantity).ToString(currency)%></span> <span class="block discountPrice">
                                    <%= (adjustedItemPrice * orderItem.Quantity).ToString(currency)%></span>--%>

                                <%= (preadjustedItemPrice * orderItem.Quantity).ToString("C", CoreContext.CurrentCultureInfo)%></span> <span class="block discountPrice">
                                <%= (adjustedItemPrice * orderItem.Quantity).ToString("C", CoreContext.CurrentCultureInfo)%></span>
							<%
							}
							else
							{
							%>
							    <%--<%= itemPriceTotal.ToString(currency)%>--%>
                                <%= itemPriceTotal.ToString("C", CoreContext.CurrentCultureInfo)%>
							<%
							}
							%>
						</td>

						<% 
							if (itemReturn != null)
							{ 
						%>
						<td>
							<%= orderItemDetail.ReturnReason%>
						</td>
						<%
							}
							if (itemReplacement != null)
							{
						%>
						<td>
							<%= orderItemDetail.OrderItemReplacement.Notes %>
						</td>
						<%
							} 
						%>
					</tr>
					<%
						}
                        }
						// here's where we show the promotion items
						var modGroups = Model.AddedOrderItems.GroupBy(x => x.OrderAdjustmentOrderLineModifications.First(y => y.ModificationOperationID == Model.OrderAdjustmentAddOperationKindID).OrderAdjustment);
						foreach (var grouping in modGroups)
						{
					%>
					<tr class="UI-lightBg specialPromotionItem">
						<td>
						</td>
						<td>
							<span class="promoHeading">
								<%= Html.Term(grouping.Key.Description, grouping.Key.Description) %></span>
						</td>
						<td>
							$0.00
						</td>
						<td>
						</td>
						<td>
						</td>
						<td>
							$0.00
						</td>
					</tr>
					<%
							foreach (var item in grouping)
							{
								var product = inventory.GetProduct(item.ProductID.ToInt());
					%>
					<tr class="promoGiftLineItem">
						<td>
							<span class="FL mr5 UI-icon icon-bundle-arrow"></span><span class="FL promoItemSku">
								<%: Html.Link(item.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + item.ProductID)%></span>
						</td>
						<td class="promoItemName">
							<%= product.Translations.Name() %>
							<div>
								<%
								nsCore.Areas.Orders.Models.Details.OrderItemDetailModel message = Model.OrderItemDetails.FirstOrDefault(f => f.OrderItem.OrderItemID == item.OrderItemID);
								if (message != null)
								{
									foreach (var customText in message.Messages)
									{
								%>
								<span>
									<%= customText %></span>
								<% 
									}
								}
								%>
							</div>
							<%
								if (product.IsStaticKit() || product.IsDynamicKit())
								{
							%>
							<span class="ClearAll"></span><a class="ViewKitContents TextLink Add" href="javascript:void(0);">
								<%= Html.Term("ViewKitContents", "View Kit Contents") %></a>
							<div class="KitContents" style="display: none;">
								<table width="100%" cellspacing="0">
									<tbody>
										<tr>
											<th>
												<%= Html.Term("SKU") %>
											</th>
											<th>
												<%= Html.Term("Product") %>
											</th>
											<th>
												<%= Html.Term("Quantity") %>
											</th>
										</tr>
										<% 
									foreach (var relation in product.ChildProductRelations.DistinctBy(p => p.ChildProductID))
									{
										Product childProduct = inventory.GetProduct(relation.ChildProductID);
										%>
										<tr>
											<td class="KitSKU">
												<%: Html.Link(childProduct.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + childProduct.ProductID)%>
											</td>
											<td>
												<%= childProduct.Translations.Name() %>
											</td>
											<td>
												<%= product.ChildProductRelations.Count(p => p.ChildProductID == relation.ChildProductID) %>
											</td>
										</tr>
										<%
									}
										%>
									</tbody>
								</table>
							</div>
							<%
								}
							%>
						</td>
                        <td>
				        </td>
						<td>
							<span class="freeItemPrice">
								<%= Html.Term("Cart_FreeItemPrice", "FREE")%></span>
						</td>
						<td>
							<%= item.Quantity %>
						</td>
						<td>
						</td>
						<td>
						</td>
					</tr>
					<%
							}
						}
					%>
                    <% %>
                    <% 
                        var numeroOrden = Convert.ToInt32(Model.Order.OrderID);
                        List<getDispatchByOrder> loadDispatchProcessOrder = new List<getDispatchByOrder>();
                        loadDispatchProcessOrder = OrderExtensions.GetDispatchByOrder(numeroOrden);
                        var listG = loadDispatchProcessOrder.GroupBy(x => new { x.DispatchID, x.Ddescripcion }).Select(y => new GrupoDispatchByOrder()
                                    {
                                            DispatchID = y.Key.DispatchID,
                                            Ddescripcion = y.Key.Ddescripcion
                                        }).ToList();

                        foreach (var itemG in listG)
                        {
                            var listaCarga = loadDispatchProcessOrder.Where(donde => donde.DispatchID == itemG.DispatchID);
                            //var dd = listaCarga.ElementAt(0).OrderItemID
                            //var nonPromotionallyAddedProducts2 = customer.ParentOrderItems.Except(customer.GetPromotionallyAddedOrderItems());

                            List<OrderItem> listaOrdenItem = new List<OrderItem>();
                
                            foreach (nsCore.Areas.Orders.Models.Details.OrderItemDetailModel orderItemDetail in Model.OrderItemDetails.OrderBy(x => x.OrderItem.OrderItemID))
				            {
                                OrderItem orderItem = orderItemDetail.OrderItem;
                                if (listaCarga.Where(donde => donde.OrderItemID.ToString().Equals(orderItem.OrderItemID.ToString())).Count() > 0)
                                {
                                    listaOrdenItem.Add(orderItem);
                                }
                            }%>
                                    <tr class = "UI-lightBg specialPromotionItem">
                                            <td>
                                            </td>
                                            <td colspan = "5">
                                            <%=itemG.Ddescripcion%>
                                            </td>
                                        </tr>
                                        <%foreach (var item in listaOrdenItem)
                                        {%>
                                            <tr>
                                            <td data-label="@Html.Term("SKU", "SKU")">
                                            <%: Html.Link(item.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + item.ProductID)%>
                                            </td>
                                            <td data-label="@Html.Term("Product", "Product")">
                                            <%=item.ProductName%>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                            <%= Html.Term("Cart_FreeItemPrice", "FREE")%>
                                            </td>
                                            <td colspan = "4">
                                            </td>
                                            </tr>                            
                                        <%}
                                        }
                                        %>
				</tbody>
				<tbody>
					<tr class="GridTotalBar">
						<td colspan="4">
							&nbsp;
						</td>
                        <td>
                            <%=quantityTotal%>
                        </td>
						<td>
							<b><span class="customerQV">
								<%--<%=qvTotal.ToString(order.CurrencyID)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                                <%=Convert.ToInt32(qvTotal)%>
							</span></b>
						</td>
						<td>
							<b><span class="customerSubtotal">
								<%--<%=subtotal.ToString(order.CurrencyID)%>--%>
                                <%=subtotal.ToString("C",CoreContext.CurrentCultureInfo)%>
							</span></b>
						</td>

                        <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%if (valorSCV == "S"){%>
						<td>
							<b><span class="customerCommissionable">
								<%--<%=commissionableTotal.ToString(order.CurrencyID)%>--%>
                                <%=commissionableTotal.ToString("C", CoreContext.CurrentCultureInfo)%>
							</span></b>
						</td>
                        <%}%>
                <%--CS.03MAY2016.Fin.Muestra CV--%>

						<td>
							<b><span class="customerSubtotal">
								<%
									if (customer.AdjustedSubTotal != customer.Subtotal)
									{
								%>
								<span class="originalPrice strikethrough">
									<%--<%= customer.Subtotal.ToString(order.CurrencyID)%>--%>
                                    <%= customer.Subtotal.ToString( CoreContext.CurrentCultureInfo)%>
								</span>&nbsp; <span class="discountPrice">
									<%--<%= customer.AdjustedSubTotal.ToString(order.CurrencyID)%>--%>
                                    <%= customer.AdjustedSubTotal.ToString("C",CoreContext.CurrentCultureInfo)%>
								</span>
								<%
									}
									else
									{
								%>
								    <%--<%= customer.Subtotal.ToString(order.CurrencyID) %>--%>
                                    <%= customer.Subtotal.ToString( CoreContext.CurrentCultureInfo)%>
								<%
									}
								%>
							</span>
								<%= Html.Term("Subtotal", "Subtotal")%>
							</b>
						</td>
					</tr>
				</tbody>

                    	<%
						} 
					%>
			</table>
		</td>
	</tr>
</table>
