<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<System.Linq.IGrouping<NetSteps.Data.Common.Entities.IOrderAdjustment, NetSteps.Data.Common.Entities.IOrderItem>>>" %>
<%@ Import Namespace="NetSteps.Web.Extensions" %>
<%
    var inventory = NetSteps.Encore.Core.IoC.Create.New<InventoryBaseRepository>();
    foreach (var grouping in Model)
    {  
        // render order adjustment line        
%>
        <tr class="UI-lightBg specialPromotionItem">
            <td>
            </td>
            <td>
            </td>

			<td>
                <span class="promoHeading"><%= Html.Term(grouping.Key.Description, grouping.Key.Description)%></span>
                <%
                var giftStep = grouping.Key.InjectedOrderSteps.FirstOrDefault();
                if (grouping.Key.InjectedOrderSteps.Any())
                { 
					
                    %>
					<a href="javascript:void(0);" class="selectGift" stepId="<%= giftStep.OrderStepReferenceID.ToString() %>"><%= Html.Term("Promotions_SelectFreeGiftLink", "Select Free Gift >") %></a><%
                } 
                %>
            </td>
			<td>
			</td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
            <td>
            </td>
        </tr>
<%
        foreach (var item in grouping)
        {

            var product = inventory.GetProduct(item.ProductID.ToInt());
            %>
            <tr class="promoGiftLineItem">
                <td>
                    <span class="FL mr5 UI-icon icon-bundle-arrow"></span>
                </td>
                <td>
                    <span class="FL promoItemSku">
                        <%: Html.Link(product.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + product.ProductID)%></span>
                </td>
                <td class="promoItemName">
                    <%= product.Translations.Name() %>
					<%if (product.IsStaticKit())
					  {%>
					<div class="bundlePackItemList">
						<table cellspacing="0" width="100%">
							<tbody>
								<tr>
									<th>
									</th>
									<th>
										<%=Html.Term("SKU", "SKU")%>
									</th>
									<th>
										<%=Html.Term("Product", "Product")%>
									</th>
									<th>
										<%=Html.Term("Quantity", "Quantity")%>
									</th>
								</tr>
								<%foreach (var relation in product.ChildProductRelations.DistinctBy(p => p.ChildProductID))
								  {
									  Product childProduct = inventory.GetProduct(relation.ChildProductID);%>
								<tr>
									<td>
										<span class="UI-icon icon-bundle-arrow"></span>
									</td>
									<td class="KitSKU">
										<%: Html.Link(childProduct.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + childProduct.ProductID)%>
									</td>
									<td>
										<%=childProduct.Translations.Name()%>
									</td>
									<td>
										<%=product.ChildProductRelations.Count(p => p.ChildProductID == relation.ChildProductID)%>
									</td>
								</tr>
								<%} %>
							</tbody>
						</table>
					</div>
					<%}%>
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
        // end render order adjustment line
    } %>
