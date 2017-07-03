<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Orders.Models.Details.DefaultPaymentMethodModalModel>"  %>
<tr>
<td class="FLabel"><%=Html.Term("AccountNumber", "Account Number") %>:</td>
<td><%=Model.DecryptedAccountNumber.MaskString(4) %></td>
</tr>
<tr><td class="FLabel"><%=Html.Term("ExpirationDate", "Expiration Date") %>:</td>
<td><%= Model.ExpirationDate.ToExpirationStringDisplay(CoreContext.CurrentCultureInfo) %></td></tr>
<tr>
<td class="FLabel"><%=Html.Term("Name") %>:</td>
<td><%= Model.BillingName %></td>
</tr>
<tr>
<td class="FLabel"><%=Html.Term("Address")%>:</td>
<td><%= Model.BillingAddress1 %>
<br /><%= Model.BillingCity %>&nbsp;,<%=Model.BillingState%>&nbsp;<%=Model.BillingPostalCode%></td>
</tr>
<tr>
<td class="FLabel"><%=Html.Term("Country") %>:</td>
<td><%= SmallCollectionCache.Instance.Countries.GetById(Model.BillingCountryId.ToInt()).GetTerm() %></td>
</tr>
<tr>
<td class="FLabel"><%=Html.Term("PaymentStatus", "Payment Status")%>:</td>
<td><%=SmallCollectionCache.Instance.OrderPaymentStatuses.GetById(Model.OrderPaymentStatusId).GetTerm()%></td>
</tr>
<tr>
<td class="FLabel"><%=Html.Term("TransactionID", "Transaction ID")%>:</td>
<td><%= Model.TransactionId %></td>
</tr>



