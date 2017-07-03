<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.OrderItem>" %>
<%@ Import Namespace="NetSteps.Web.Extensions" %>
<%@ Import Namespace="NetSteps.Data.Common.Services" %>
<%@ Import Namespace="NetSteps.Encore.Core.IoC" %>
<%@ Import Namespace="NetSteps.Data.Entities.Business.HelperObjects.SearchData" %>
<%

    List<DispatchProducts> lstProductsVal = (List<DispatchProducts>)Session["itemsProductsDispatch"];
    List<getDispatchByOrder> listDispatchProcess = (List<getDispatchByOrder>)Session["loadDispatchProcessOrder"];
 
	OrderItem orderItem = Model;
    Order order = orderItem.OrderCustomer.Order;
	bool fixedAutoship = (bool)ViewData["FixedAutoship"];
	int currencyID = (int)ViewData["CurrencyID"];
	var inventory = NetSteps.Encore.Core.IoC.Create.New<InventoryBaseRepository>();
	 bool validarNull=false;
    
    if (lstProductsVal != null)
    {
        if (lstProductsVal.Where(x => x.OrderItemID.ToString().Equals(orderItem.OrderItemID.ToString())).Count() > 0)
        { 
            validarNull=true;
        }
    }

    if (listDispatchProcess != null)
    {
       if ((listDispatchProcess.Where(x => x.OrderItemID.ToString().Equals(orderItem.OrderItemID.ToString())).Count() > 0))
        {
           validarNull=true;
        }
    } 
      
	Product product = inventory.GetProduct(orderItem.ProductID.Value);
	int requiredItemsInBundleCount = 0;
	if (product.IsDynamicKit())
	{
		requiredItemsInBundleCount = product.DynamicKits[0].DynamicKitGroups.Sum(g => g.MinimumProductCount);
	}
	
	var priceTypeService = Create.New<IPriceTypeService>();
	int commissionablePriceTypeID = priceTypeService.GetPriceType(orderItem.OrderCustomer.AccountTypeID, (int)Constants.PriceRelationshipType.Commissions, ApplicationContext.Instance.StoreFrontID).PriceTypeID;
	
	decimal adjustedItemPrice = orderItem.GetAdjustedPrice(orderItem.ProductPriceTypeID.Value);
	decimal preadjustedItemPrice = orderItem.GetPreAdjustmentPrice(orderItem.ProductPriceTypeID.Value);
	decimal adjustedCommissionTotal = orderItem.GetAdjustedPrice(commissionablePriceTypeID) * orderItem.Quantity;
	decimal preadjustedCommissionTotal = orderItem.GetPreAdjustmentPrice(commissionablePriceTypeID) * orderItem.Quantity;
	OrderItemPrice qvPrice = new OrderItemPrice();
    int ProductPriceTypeRetail = 0;
    foreach (var q in orderItem.OrderItemPrices)
	{
		var pType = SmallCollectionCache.Instance.ProductPriceTypes.FirstOrDefault(ppt=> ppt.ProductPriceTypeID == q.ProductPriceTypeID);
		if (pType != null && pType.Name.Equals("QV")) qvPrice = q;
        if (pType != null && pType.Name.Equals("Retail")) ProductPriceTypeRetail = q.ProductPriceTypeID;
	}
    var retailPerItem = NetSteps.Data.Entities.ProductPricesExtensions.GetPriceByPriceType(Convert.ToInt32(orderItem.ProductID), ProductPriceTypeRetail, order.CurrencyID).GetRoundedNumber(2); //NetSteps.Data.Entities.Business.Logic.ProductPriceLogic.Intance.GetRetilPerItem(product.ProductID);
%>
<tr id="oi<%= orderItem.Guid.ToString("N") %>" class="<%=product.IsDynamicKit() ? "BundlePack" : "" %>">
	<td>
		<%if (!fixedAutoship)
		  { %>
			<a href="javascript:void(0);" title="Remove" class="DTL Remove"
				onclick="removeItem('<%= orderItem.Guid.ToString("N") %>');" name='<%= orderItem.OrderItemID %>'">
			<span></span></a>
		<%} %>
	</td>
	<td>
		<input type="hidden" class="productId" value="<%= product.ProductID %>" />
		<%: Html.Link(orderItem.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + orderItem.ProductID)%>
	</td>
	<td>
		<%= product.Translations.Name()%>
		<div class="lawyer lineItemPromoList">
			<%foreach (var description in orderItem.OrderAdjustmentOrderLineModifications.GroupBy(a => a.OrderAdjustmentID).Select(g => g.First().OrderAdjustment.Description))
			{ %>
			<span class="block promoName"><%= description %></span>
			<% } %>
		</div>
		<%if (product.IsDynamicKit())
		  {%>
		<a class="btnAddToBundle" href="javascript:void(0);">
			<%if (orderItem.ChildOrderItems.Sum(oi => oi.Quantity) == requiredItemsInBundleCount)
			  {%>
			<span class="UI-icon icon-bundle-full"></span><span class="indicator">(<%=Html.Term("edit", "edit")%>)</span>
			<%}
			  else
			  {%>
			<span class="UI-icon icon-bundle-add"></span><span class="indicator">(<%=Html.Term("AddItemsToBundle", "add items to bundle")%>)</span>
			<%}%>
		</a>
		<%}
    else if (product.IsStaticKit() && orderItem.ChildOrderItems.Any())
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
                <%foreach (var objE in orderItem.ChildOrderItems)
               //<%foreach (var objE in Order.GetMaterialWithMaterialID(Convert.ToInt32(Session["WareHouseId"]), Convert.ToInt32(Session["PreOrder"])).Where(x => x.ProductId == product.ProductID))
                      {%>
                        <tr>
                            <td>
                                <span class="UI-icon icon-bundle-arrow"> </span>
                            </td>
                            <td class="KitSKU">
                                <%=objE.SKU %>
                            </td>
                            <td>
                                <%=objE.ProductName %>
                            </td>
                            <td>
                                <%=objE.Quantity%>
                            </td>
                        </tr>  
                      <%} %>
					
				</tbody>
			</table>
		</div>
		<%}%>
	</td>

     <td>
     <%= retailPerItem.ToString("C",CoreContext.CurrentCultureInfo) %>
    </td>

	<td>
		<% if (adjustedItemPrice != preadjustedItemPrice)
		   {%>
		  <%-- <span class="block originalPrice strikethrough"><%= preadjustedItemPrice.ToString(currencyID)%></span>
		   <span class="block discountPrice"><%= adjustedItemPrice.ToString(currencyID)%></span>--%>

            <span class="block originalPrice strikethrough"><%= preadjustedItemPrice.ToString("c",CoreContext.CurrentCultureInfo)%></span>
		   <span class="block discountPrice"><%= adjustedItemPrice.ToString("C", CoreContext.CurrentCultureInfo)%></span>
			<%} 
			else { %>
			<%= adjustedItemPrice.ToString(currencyID)%>
		   <%} %>
	</td>

    <%--CS.05MAY2016.Inicio.Muestra CV--%>
                            <%string valorSCV = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "SCV");
                                if (valorSCV == "S")
                                { %>
	<td>
		<% if (adjustedCommissionTotal != preadjustedCommissionTotal)
		   {%>
		 <%--  <span class="block originalPrice strikethrough"><%= preadjustedCommissionTotal.ToString(currencyID)%></span>
		   <span class="block discountPrice"><%= adjustedCommissionTotal.ToString(currencyID)%></span>--%>

             <span class="block originalPrice strikethrough"><%= preadjustedCommissionTotal.ToString("C", CoreContext.CurrentCultureInfo)%></span>
		   <span class="block discountPrice"><%= adjustedCommissionTotal.ToString("C", CoreContext.CurrentCultureInfo)%></span>
			<%} 
			else { %>
            <%= adjustedCommissionTotal.ToString("C", CoreContext.CurrentCultureInfo)%>
			<%--<%= adjustedCommissionTotal.ToString(currencyID)%>--%>
		   <%} %>
	</td>
     <%}%>
                            <%--CS.05MAY2016.Fin.Muestra CV--%>
	<td>
		<%if (fixedAutoship)
		  {%>
		  <%=orderItem.Quantity%>
		  <%}
		  else
		  {
			  if (product.IsDynamicKit())
			  {%>
		<input type="hidden" class="quantity" value="<%= orderItem.Quantity %>" /><%=orderItem.Quantity%>
		<%}
								  else
								  {%>
		<input type="text" class="quantity" value="<%= orderItem.Quantity %>" style="width: 4.167em;" id="Quantity" onfocus="GetValueQuantity(value);" onblur="UpdateQuantity(value);" />
		<%}
							  } %>
	</td>
	<td>
		<% if (qvPrice.UnitPrice != qvPrice.OriginalUnitPrice )
		{%>
			<span id="ItemOriginalUnitPrice" class="block originalPrice strikethrough"> <%--CGI(CMR)-01/04/2015-Agregar Id--%>
				<%--<%= (qvPrice.OriginalUnitPrice * Math.Abs(orderItem.Quantity)).ToString(currencyID)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                <%= Convert.ToInt32(Convert.ToDecimal(qvPrice.OriginalUnitPrice) * Math.Abs(orderItem.Quantity))%>
            </span> 
            <span id="ItemUnitPrice" class="block discountPrice">
				<%--<%= (qvPrice.UnitPrice * Math.Abs(orderItem.Quantity)).ToString(currencyID)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                <%= Convert.ToInt32(qvPrice.UnitPrice * Math.Abs(orderItem.Quantity))%>
            </span> <%--CGI(CMR)-01/04/2015-Agregar Id--%>
	   	<%}
		else
		{%>
            <span id="ItemOnlyOriginalUnitPrice"> <%--CGI(CMR)-01/04/2015-Agregar Id--%>
			    <%--<%= (qvPrice.UnitPrice*Math.Abs(orderItem.Quantity)).ToString(currencyID)%>--%> <%--EL QV NO DEBE TENER SIGNO $--%>
                <%= Convert.ToInt32(qvPrice.UnitPrice*Math.Abs(orderItem.Quantity))%>
            </span>
		<%}%>
	</td>
	<td colspan="2">
		<% if (orderItem.GetAdjustedPrice(orderItem.ProductPriceTypeID.Value) != orderItem.GetPreAdjustmentPrice(orderItem.ProductPriceTypeID.Value))
			{%>
		<%--	<span id="ItemOriginalPrice" class="block originalPrice strikethrough"><%= (preadjustedItemPrice * orderItem.Quantity).ToString(currencyID) %></span>
			<span id="ItemDiscountPrice" class="block discountPrice"><%= (adjustedItemPrice * orderItem.Quantity).ToString(currencyID) %></span>--%>

            	<span id="ItemOriginalPrice" class="block originalPrice strikethrough"><%= (preadjustedItemPrice * orderItem.Quantity).ToString("C",CoreContext.CurrentCultureInfo) %></span>
			<span id="ItemDiscountPrice" class="block discountPrice"><%= (adjustedItemPrice * orderItem.Quantity).ToString("C", CoreContext.CurrentCultureInfo)%></span>
			<%} 
			else { %>
            <span id="ItemOnlyOriginalPrice"><%= (adjustedItemPrice * orderItem.Quantity).ToString("C", CoreContext.CurrentCultureInfo)%></span> <%--CGI(CMR)-06/04/2015--%>
			<%--<span id="ItemOnlyOriginalPrice"><%= (adjustedItemPrice * orderItem.Quantity).ToString(currencyID) %></span> <%--CGI(CMR)-06/04/2015--%>--%>
			   <%} %>
	</td>
</tr>
<%if (product.IsDynamicKit())
  {
	  foreach (var group in product.DynamicKits[0].DynamicKitGroups)
	  {
		  int requiredItemsInGroupCount = group.MinimumProductCount;
		  var childItemsInGroup = orderItem.ChildOrderItems.Where(oi => oi.DynamicKitGroupID == group.DynamicKitGroupID);
%>
<tr class="BundleCategoryRow">
	<td colspan="7">
		<h3 class="BundleCategory">
			<%=group.Translations.Name() %>
			<span class="bold">(<%=childItemsInGroup.Sum(ci=>ci.Quantity) %>/<%=requiredItemsInGroupCount%>)</span></h3>
	</td>
</tr>
<%foreach (var childItem in childItemsInGroup)
  {
	  var childProduct = inventory.GetProduct(childItem.ProductID.Value);%>
<tr class="BundleItem">
	<td>
	</td>
	<td>
		<%: Html.Link(childItem.SKU, false, ResolveUrl("~/Products/Products/Overview?productId=") + childItem.ProductID)%>
	</td>
	<td>
		<%= childProduct.Translations.Name()%>
	</td>
	<td>
	</td>
	<td>
	</td>
	<td>
		<%= childItem.Quantity%>
	</td>
	<td>
	</td>
</tr>
<%}%>
<% } %>
<% } %> 
<script type="text/javascript">
    /*CS:21AB2016.Inicio*/
    var valorCantidadActual = 0;

    //onfocus
    function GetValueQuantity(Quantity) {
        valorCantidadActual = Quantity;
        document.getElementById("Quantity").style.backgroundColor = "#FFFFB2";
    }

    //onblur
    function UpdateQuantity(Quantity) {
        if (Quantity == valorCantidadActual) {
            document.getElementById("Quantity").style.backgroundColor = "white";
            return;
        }
        
        UpdateCart();
    }
    /*CS:21AB2016.Fin*/
	
</script>