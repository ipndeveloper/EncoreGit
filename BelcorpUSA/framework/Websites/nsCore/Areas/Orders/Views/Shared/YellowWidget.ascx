<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Order>" %>
<%  
    AccountSlimSearchData account = null;
    if (string.IsNullOrEmpty(Request["refAccount"]))
    {
        if (Model.OrderCustomers.Count > 0)
            account = Model.OrderCustomers[0].AccountInfo;
    }
    else
        account = NetSteps.Data.Entities.Account.LoadSlimByAccountNumber(Request["refAccount"]);
%>
<div class="TagInfo">
    <div class="Content">
        <% if (account != null)
           { %>
        <h1>
            <a href="<%= ResolveUrl(string.Format("~/Accounts/Overview/Index/{0}", account.AccountNumber)) %>">
                <%: account.FullName %></a>
        </h1>
        <p class="DistributorStatus">
            <a href="<%= ResolveUrl(string.Format("~/Accounts/Overview/Index/{0}", account.AccountNumber)) %>">
                #<%= account.AccountNumber %></a><% if (!account.DecryptedTaxNumber.IsNullOrEmpty())
                                                    { %>, SSN/TID:
            <%= account.DecryptedTaxNumber.MaskString(4) %>
            <% } %>
        </p>
        <% } %>
        <h1>
            <a href="<%= ResolveUrl("~/Orders/Details/Index/") + Model.OrderNumber %>">
                <%= Html.Term("Order#", "Order #")%>
                <%= Model.OrderNumber %></a></h1>
        <table class="" width="100%">
            <tbody>
                <%--INI - GR4172--%>
                <%if (Model.OrderID != 0){ %>
                <tr>
                    <td class="Label">
                        <%= Html.Term("CompletePeriod", "Period")%>:
                    </td>
                    <td>
                        <%= Model.GetOrderAndPeriods(Model.OrderID)%>
                    </td>
                </tr>
                <%} %>
                <%--FIN - GR4172--%>
                <%if (Model.CompleteDate.ToDateTime().Year > 1900)
                  { %>
                <tr>
                    <td class="Label">
                        <%= Html.Term("Date")%>:
                    </td>
                    <td>
                        <%= Model.CompleteDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo) %><br />
                        <%= Model.CompleteDate.ToDateTime().ToString("t", CoreContext.CurrentCultureInfo)%>
                    </td>
                </tr>
                <%} %>
                <tr>
                    <td class="Label">
                        <%= Html.Term("Status")%>:
                    </td>
                    <td>
                        <%OrderShipment shipment = Model.GetDefaultShipment();

                          bool hasShipped = Model.OrderStatusID == (int)ConstantsGenerated.OrderStatus.Shipped && !string.IsNullOrEmpty(shipment.TrackingURL);
                          if (hasShipped)
                          { %>
                        <a href="<%= shipment.TrackingURL %>" target="_blank" rel="external">
                            <%} %>
                            <b>
                                <%= SmallCollectionCache.Instance.OrderStatuses.GetById(Model.OrderStatusID).GetTerm() %></b>
                            <%if (hasShipped)
                              {  %>
                        </a>
                        <%} %>
                    </td>
                </tr>
                <%  
                    //int AccountTypeId = Account.LoadByAccountNumber(Model.AccountNumber).AccountTypeID;
                    //if (AccountTypeId == 1)
                    if (ViewBag.AccountTypeId == 1)
                    { %>
                <tr>
                    <td class="Label">
                        <%=Html.Term("QualificationTotal", "QualificationTotal")%>
                        :
                    </td>
                    <td>
                        <span class="qualificationTotal">
                          <%--  <%= Model.QualificationTotal.ToString(Model.CurrencyID)%>--%>
                            <%= Model.QualificationTotal.ToString(CoreContext.CurrentCultureInfo)%>

                            </span>
                            
                    </td>
                </tr>
                <tr>
                    <td class="Label">
                        <%=Html.Term("CommissionableTotal", "Commissionable Total")%>
                        :
                    </td>
                    <td>
                        <span class="commissionableTotal">
                            <%--<%=Model.CommissionableTotal.ToString(Model.CurrencyID) %></span>--%>
                            <%=Model.CommissionableTotal.ToString(CoreContext.CurrentCultureInfo)%></span>
                    </td>
                </tr>
                <tr>
                    <td class="Label"  <%=ViewBag.EstadoCredito=="N"? "":"style=color:Red" %>>
                    <% if (ViewBag.EstadoCredito == "N")
                       { %>
                    
                        <%=Html.Term("CreditAvailable", "Credit Available")%> 
                        :
                    <% }
                       else
                       {%>
                       <%=Html.Term("CreditBlocked", "Credit Blocked")%>
                          :                        
                        <% }

                        decimal CreditAvailable = Convert.ToDecimal(ViewBag.CreditAvailable);
                       
                        decimal ProductCredit = Convert.ToDecimal(ViewBag.ProductCredit);
                       %>
                    </td>
                    <td>
                        <label id="lblCreditAvailable"  <%=ViewBag.EstadoCredito=="N"? "":"style=color:Red" %>>
                           <%-- <%= CreditAvailable.ToString(Model.CurrencyID)%></label>--%>
                            <%= CreditAvailable.ToString("C" , CoreContext.CurrentCultureInfo)%></label>
                    </td>
                </tr>
                <tr>
                    <td class="Label">
                        <%=Html.Term("EndingBalance", "Previous Balance")%>
                        :
                    </td>
                    <td>
                        <label id="Label1" <%=ViewBag.ProductCreditStatus <= 0? "style=color:Red": "" %>>
                           <%-- <%=ProductCredit.ToString(Model.CurrencyID)%></label>--%>
                            <%=ProductCredit.ToString("C", CoreContext.CurrentCultureInfo)%></label>
                    </td>
                </tr>
                <% } 
                %>
            </tbody>
        </table>
        <table class="DetailsTag Section" width="100%">
            <tbody>
                <tr class="Total">
                    <td class="Label">
                        <%= Html.Term("Total", "Total")%>:
                    </td>
                    <td class="grandTotal">
                        <%--<%= Model.GrandTotal.ToDecimal().ToString(Model.CurrencyID)%>--%>

                        <%= Model.GrandTotal.ToDecimal().ToString("C",CoreContext.CurrentCultureInfo)%>
                    </td>
                </tr>
            </tbody>
        </table>
        <% if (Model.OrderTypeID != NetSteps.Common.Configuration.ConfigurationManager.GetAppSetting<int>(NetSteps.Common.Configuration.ConfigurationManager.VariableKey.ReturnOrderTypeID))
           {
               List<Order> returns = CoreContext.LoadChildReturnOrdersFull(Model.OrderID);
               List<Order> replacements = CoreContext.LoadChildReplacementOrdersFull(Model.OrderID);
               if (returns.Count > 0)
               {
        %>
        <h1 class="ReturnNotice" style="color: #FF0000;">
            <%= Html.Term("ReturnsOnThisOrder", "Returns on this Order")%>:</h1>
        <table class="DetailsTag Section" width="100%">
            <tbody>
                <tr>
                    <th>
                        <%= Html.Term("Order#", "Order #")%>
                    </th>
                    <th>
                        <%= Html.Term("Date", "Date")%>
                    </th>
                    <th>
                        <%= Html.Term("Status", "Status")%>
                    </th>
                </tr>
                <% foreach (Order order in returns)
                   { %>
                <tr>
                    <td>
                        <a href="<%= ResolveUrl("~/Orders/Details/Index/") + order.OrderNumber %>">
                            <%= order.OrderNumber %></a>
                    </td>
                    <td>
                        <%= order.DateCreated.ToShortDateString() %>
                    </td>
                    <td>
                        <%= SmallCollectionCache.Instance.OrderStatuses.GetById(order.OrderStatusID).GetTerm() %>
                    </td>
                </tr>
                <% } %>
            </tbody>
        </table>
        <%
               }
               if (replacements.Count > 0)
               {
        %>
        <h1 class="ReturnNotice" style="color: #FF0000;">
            <%= Html.Term("ReplacementsOnThisOrder", "Replacements on this Order")%>:</h1>
        <table class="DetailsTag Section" width="100%">
            <tbody>
                <tr>
                    <th>
                        <%= Html.Term("Order#", "Order #")%>
                    </th>
                    <th>
                        <%= Html.Term("Date", "Date")%>
                    </th>
                    <th>
                        <%= Html.Term("Status", "Status")%>
                    </th>
                </tr>
                <% foreach (Order order in replacements)
                   { %>
                <tr>
                    <td>
                        <a href="<%= ResolveUrl("~/Orders/Details/Index/") + order.OrderNumber %>">
                            <%= order.OrderNumber %></a>
                    </td>
                    <td>
                        <%= order.DateCreated.ToShortDateString() %>
                    </td>
                    <td>
                        <%= SmallCollectionCache.Instance.OrderStatuses.GetById(order.OrderStatusID).GetTerm() %>
                    </td>
                </tr>
                <% } %>
            </tbody>
        </table>
        <%
               }

           }
           else
           {
               //TODO: Make this into an Order.LoadSlim - DES
               Order original = Order.Load(Model.ParentOrderID.ToInt());
        %>
        <h1>
            Original Order:</h1>
        <table class="DetailsTag Section" width="100%">
            <tbody>
                <tr>
                    <th>
                        <%= Html.Term("Order#", "Order #")%>
                    </th>
                    <th>
                        <%= Html.Term("Date", "Date")%>
                    </th>
                </tr>
                <tr>
                    <td>
                        <a href="<%= ResolveUrl("~/Orders/Details/Index") + original.OrderNumber %>">
                            <%= original.OrderNumber%></a>
                    </td>
                    <td>
                        <%= original.DateCreated.ToShortDateString()%>
                    </td>
                </tr>
            </tbody>
        </table>
        <%} %>
    </div>
    <div class="TagBase">
    </div>
</div>
