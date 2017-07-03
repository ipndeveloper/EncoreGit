<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Orders.Models.Details.PartialOrderCustomerTotalsModel>" %>
<%
	Order order = Model.Order;
	OrderCustomer customer = Model.OrderCustomer;
	Currency currency = Model.Currency;
	bool isReturnOrder = Model.IsReturnOrder;
%>
<td colspan="1" style="text-align: right;">
	<%= Html.Term("Subtotal", "Subtotal")%>:<br />

    <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%string valorSCV = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "SCV");
                                if (valorSCV == "S")
                                { %>
	<%= Html.Term("CommissionableTotal", "Commissionable Total")%>:<br />
     <%}%>
                            <%--CS.03MAY2016.Fin.Muestra CV--%>


	<%= Html.Term("Tax", "Tax")%>:<br />
	<%= Html.Raw(Html.Term("S&H", "S&amp;H"))%>:<br />
	<%= Html.Term("OrderTotal", "Order Total")%>:<br />
</td>
<td>
          <%--  <%= Model.Subtotal.ToString(currency)%><br />--%>

            <%= Model.Subtotal.ToString("C", CoreContext.CurrentCultureInfo)%><br />

            <%--CS.03MAY2016.Inicio.Muestra CV--%>
            <%if (valorSCV == "S"){%>
                <%--<%= Model.CommissionableTotal.ToString(currency)%><br />--%>
                <%= Model.CommissionableTotal.ToString("C",CoreContext.CurrentCultureInfo)%><br />
            <%}%>
            <%--CS.03MAY2016.Fin.Muestra CV--%>


<%--	<%= Model.TaxAmountTotal.ToString(currency)%><br />--%>
	<%= Model.TaxAmountTotal.ToString("C",CoreContext.CurrentCultureInfo)%><br />

<%--	<%= (Model.ShippingTotal>=0 || isReturnOrder) ? order.ShippingTotal.Value.ToString(currency)  : "(" + Html.Term("ShippingToParty", "Shipping to party") + ")" %><br />--%>
	<%= (Model.ShippingTotal>=0 || isReturnOrder) ? order.ShippingTotal.Value.ToString("C",CoreContext.CurrentCultureInfo)  : "(" + Html.Term("ShippingToParty", "Shipping to party") + ")" %><br />
	<b>
		<%--<%= Model.GrandTotal.ToString(currency) %></b><br />--%>
       
        <%= Model.GrandTotal.ToString("C", CoreContext.CurrentCultureInfo) %></b><br />
</td>
