<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DistributorBackOffice.Areas.Orders.Models.Details.EFTPaymentMethodModalModel>"  %>
<tr>
<td><%=Html.Term("BankName", "Bank Name") %>:</td>
<td><%=Model.BankName %></td>
</tr>
<tr>
<td><%=Html.Term("AccountNumber", "Account Number") %>:</td>
<td><%=Model.DecryptedAccountNumber.MaskString(4) %></td>
</tr>
<tr>
<td><%=Html.Term("RoutingNumber", "Routing Number") %>:</td>
<td><%= Model.RoutingNumber %></td></tr>
<tr>
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




