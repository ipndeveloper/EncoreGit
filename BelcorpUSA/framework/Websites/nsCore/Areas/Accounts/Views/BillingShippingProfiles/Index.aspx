<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
    Inherits="System.Web.Mvc.ViewPage" %>
<%@ Import Namespace="NetSteps.Common.Interfaces" %>
		

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function displayAddresses(results) {
            $('#addresses').html(results);
        }
        function displayPaymentMethods(results) {
            $('#paymentMethods').html(results);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Accounts") %>">
        <%= Html.Term("Accounts", "Accounts")%></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
            <%= CoreContext.CurrentAccount.FullName %></a> >
    <%= Html.Term("BillingShippingProfiles", "Billing/Shipping Profiles")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<%
        var currentAccount = (NetSteps.Data.Entities.Account)ViewBag.CurrentAccount;
%>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("BillingShippingProfiles", "Billing/Shipping Profiles")%>
        </h2>
    </div>
    <div id="errorCenter">
    </div>
    <table width="100%" cellspacing="0">
        <tr>
            <td class="splitCol brdr-right pad10">
                <h3>
                    <%= Html.Term("BillingProfiles", "Billing Profiles")%>
                    <a href="#" id="btnAddBillingAddress" onclick="editPaymentMethod();"
                        class="DTL FR Add">
                        <%= Html.Term("Add", "Add")%></a></h3>
                <div id="paymentMethods" class="addressContainer">
                    <% 
                        int count = 0;
                        foreach (AccountPaymentMethod paymentMethod in currentAccount.AccountPaymentMethods)
                        { 
                    %>
                    <div class="Profile <%: paymentMethod.IsDefault ? "DefaultProfile" : "" %>">
                        <div class="FR ProfileControls">
                            <a href="javascript:void(0);" class="DTL SetDefault" title="<%= Html.Term("MakeDefault", "Make Default") %>"
                                onclick="setDefaultPaymentMethod(<%= paymentMethod.AccountPaymentMethodID %>);"
                                <%= paymentMethod.IsDefault ? "style=\"display:none;\"" : "" %>><span>
                                    <%= Html.Term("MakeDefault", "Make Default") %></span></a> <a href="javascript:void(0);"
                                        class="DTL Remove" onclick="deletePaymentMethod(<%= paymentMethod.AccountPaymentMethodID %>);">
                                        <span>
                                            <%= Html.Term("Delete", "Delete") %></span></a>
                        </div>
                        <div id="PaymentMethod<%= paymentMethod.AccountPaymentMethodID %>">
                            <b><a title="<%= Html.Term("Edit") %> <%= paymentMethod.ProfileName%>" class="DTL Edit"
                                onclick="editPaymentMethod(<%= paymentMethod.AccountPaymentMethodID %>);"><span>
                                    <%: string.IsNullOrEmpty(paymentMethod.ProfileName) ? Html.Term("Unnamed") : paymentMethod.ProfileName%></span></a></b>
                            <br />
                            <%: paymentMethod.DecryptedAccountNumber.MaskString(4) %><br />
                            <%: paymentMethod.ExpirationDate.ToExpirationStringDisplay(CoreContext.CurrentCultureInfo) %><br />
                        </div>
                    </div>
                    <%
                            count++;
                        }      
                    %>
                </div>
            </td>
            <td class="splitCol pad10">
                <h3>
                    <%= Html.Term("ShippingProfiles", "Shipping Profiles")%>
                    <a href="#" id="btnAddShippingAddress" onclick="editAddress();" class="DTL FR  Add">
                        <%= Html.Term("Add", "Add")%></a></h3>
                <div id="addresses" class="addressContainer">
                    <%
                        count = 0;
                        foreach (Address shippingAddress in currentAccount.Addresses.Where(a => a.AddressTypeID == Constants.AddressType.Shipping.ToInt()))
                        { %>
                    <div class="Profile <%: shippingAddress.IsDefault ? "DefaultProfile" : "" %>">
                        <div class="FR ProfileControls">
                            <a href="javascript:void(0);" class="DTL SetDefault" title="<%= Html.Term("Make Default", "Make Default") %>"
                                onclick="setDefaultAddress(<%= shippingAddress.AddressID %>);" <%= shippingAddress.IsDefault ? "style=\"display:none;\"" : "" %>>
                                <span>
                                    <%= Html.Term("Make Default", "Make Default") %></span> </a><a class="DTL Remove"
                                        href="javascript:void(0);" onclick="deleteAddress(<%= shippingAddress.AddressID %>);">
                                        <span>
                                            <%= Html.Term("Delete", "Delete") %></span> </a>
                            <span class="UI-icon icon-check block"></span>
                        </div>
                        <b><a title="<%= Html.Term("Edit") %> <%= shippingAddress.ProfileName%>" class="DTL Edit"
                            onclick="editAddress(<%= shippingAddress.AddressID %>);">
                            <%= shippingAddress.ProfileName%></a></b>
                        <br />
                        

                        <% 
                            if (shippingAddress.ToString().Trim().Length == 0)
                            {
                                %>
                                <%= shippingAddress.Address1 %> <br />
                                <%= shippingAddress.County %><br />
                                <%= shippingAddress.City %><br />
                                <%= shippingAddress.PostalCode %>
                        <% }
                           else
                           {
                        %>
                        <%= shippingAddress.ToString().ToHtmlBreaks() %>

                        <% } %>

                    </div>
                    <%++count;
                        } %>
                </div>
            </td>
        </tr>
    </table>
    <% Html.RenderPartial("AddressValidation"); %>
    <% Html.RenderPartial("BillingShippingModal", ViewData); %>
    
</asp:Content>
