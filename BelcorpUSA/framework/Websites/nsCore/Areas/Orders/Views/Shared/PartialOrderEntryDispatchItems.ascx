<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.List<NetSteps.Data.Entities.Business.HelperObjects.SearchData.DispatchProducts>>" %>
<%@ Import Namespace="NetSteps.Web.Extensions" %>
<%@ Import Namespace="NetSteps.Data.Entities.Business.HelperObjects.SearchData" %>

<%      var lstProductsVal = (List<DispatchProducts>)Session["itemsProductsDispatch"];
        if (lstProductsVal != null)
        {
            foreach (var item in lstProductsVal)
            {%>
         <tr class="UI-lightBg specialPromotionItem">
            <td>
            </td>
            <td>
            </td>
			<td>
                <span class="DispatchTag"><%= item.NameType%></span>
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
            <tr class="promoGiftLineItem">
                <td>
                    <span class="FL mr5 UI-icon icon-bundle-arrow"></span>
                </td>
                <td>
                     <%= item.SKU%>
                </td>
                <td class="promoItemName">
					<div class="bundlePackItemList">
						<table cellspacing="0" width="100%">
							<tbody>
								<tr>
									<td>
										<span class="UI-icon icon-bundle-arrow"></span>
									</td>
									<td class="KitSKU">
									</td>
									<td>
										<%=item.Name%>
									</td>                                   
								</tr>
							</tbody>
						</table>
					</div>
                </td>
				<td>
				</td>
                <td>
                    <span class="freeItemPrice">
                        <%= Html.Term("Cart_FREE", "FREE")%></span>
                </td>
                <td>                
                    <%= item.Quantity%>
                </td>
                <td>
                  <%= 0%>
                </td>
                <td>
                   <%= 0%>
                </td>
            </tr>     <% 
            }
        }%>