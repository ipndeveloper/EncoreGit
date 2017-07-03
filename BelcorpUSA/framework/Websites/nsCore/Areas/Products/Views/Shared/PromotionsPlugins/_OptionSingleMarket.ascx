<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<table class="FormTable" width="100%">
    <tbody>
        <tr>
            <td class="FLabel">
                <%=Html.Term("PromotionOptions_SingleMarketOption", "Select a Market: ")%>
            </td>
            <td>
		        <select class="pad2 promotionOption singleMarket">
			        <% 
			        bool first = true;
			        foreach (Market market in CoreContext.CurrentUserMarkets)
	           { %>
				        <option value="<%= market.MarketID %>"
					        <%= Model.MarketID == market.MarketID || first ? "selected='selected'" : "" %>>
					        <%= market.GetTerm() %></option>
			        <% first = false;
			        } %>
		        </select>
            </td>
        </tr>
    </tbody>
</table>
