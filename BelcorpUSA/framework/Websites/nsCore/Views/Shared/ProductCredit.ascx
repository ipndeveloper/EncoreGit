<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div>
	<%
		var productCreditBalance = ViewData["ProductCreditBalance"];
		if (productCreditBalance != null && CoreContext.CurrentAccount.HasFunction("Orders-Payment Method-Product Credit", true, false, CoreContext.CurrentAccount.AccountTypeID))
		{ %>
			<div>
				<label style="font-weight:bold;"><%= Html.Term("ProductCreditBalance", "Product Credit Balance")%></label>
				<br />
				<%
					string colorStyle = "";
					if (productCreditBalance.ToString().StartsWith("(") && productCreditBalance.ToString().EndsWith(")"))
					{
						colorStyle = "style='color:red'";
					}
				%>
				<span <%= colorStyle %>><%= productCreditBalance != null ? productCreditBalance.ToString() : "NA"%></span>
			</div>     
    <% } %>
</div>
