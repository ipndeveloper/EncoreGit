<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Web.Mvc.Controls.Services.DefaultPaymentMethodModalModel>"  %>
<tr>
<td><%=Html.Term("AccountNumber", "Account Number") %>:</td>
<td><%=Model.DecryptedAccountNumber.MaskString(4) %></td>
</tr>
<tr><td><%=Html.Term("ExpirationDate", "Expiration Date") %>:</td>
<td><%= Model.ExpirationDate.ToExpirationStringDisplay(CoreContext.CurrentCultureInfo) %></td></tr>
<tr>
<td><%=Html.Term("Name") %>:</td>
<td><%= Model.BillingName %></td>
</tr>
<tr>
<td><%=Html.Term("Address")%>:</td>
<td><%= Model.BillingAddress1 %></td>
</tr>
<tr>
<td colspan="2"><%= Model.BillingCity %>&nbsp;,<%=Model.BillingState%>&nbsp;<%=Model.BillingPostalCode%></td>
</tr>
<tr>
<td><%=Html.Term("Country") %>:</td>
<td><%= SmallCollectionCache.Instance.Countries.GetById(Model.BillingCountryId.ToInt()).GetTerm() %></td>
</tr>
<tr>
<td><%=Html.Term("PaymentStatus", "Payment Status")%>:</td>
<td><%=SmallCollectionCache.Instance.OrderPaymentStatuses.GetById(Model.OrderPaymentStatusId).GetTerm()%></td>
</tr>
<tr>
<td><%=Html.Term("TransactionID", "Transaction ID")%>:</td>
<td><%= Model.TransactionId %></td>
</tr>



