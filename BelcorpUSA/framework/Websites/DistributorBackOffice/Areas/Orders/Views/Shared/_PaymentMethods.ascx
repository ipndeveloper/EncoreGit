<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Order>" %>

<div class="UI-lightBg pad5 paymentSelectTitle">
<div id="dvB">
   <label class="block">Select a form of payment:</label>
    <%= @Html.DropDownList("sPaymentMethod", (TempData["sPaymentMethod"] as IEnumerable<SelectListItem>))%>
 </div>
   <div id="dvU">
       <select id="sPaymentMethod02" >
                <%
                    CoreContext.ReloadCurrentAccount();        
                    var paymentMethods = CoreContext.CurrentAccount.AccountPaymentMethods;
                    var nonProfilePaymentMethods = CoreContext.CurrentAccount.GetNonProfilePaymentTypes(CoreContext.CurrentAccount.AccountTypeID, Model.OrderTypeID, Model.OrderShipments != null && Model.OrderShipments.Count > 0 ? Model.OrderShipments.First().CountryID : 0);
                
                %>
                <% if (paymentMethods.Count() < 1 && nonProfilePaymentMethods.Count() < 1)
                {
                %>
                <option value="">--
                    <%= Html.Term("PleaseAddaPaymentMethod", "Please add a payment method")%>
                    --</option>
                <%
                }
                %>
                <% int paymentMethodId = 0; %>
                <%
                if (Model.IsTemplate && Model.OrderCustomers[0].OrderPayments.Count > 0)
                {
                    var paymentMethod = paymentMethods.FirstOrDefault(pm => pm.DecryptedAccountNumber == Model.OrderCustomers[0].OrderPayments[0].DecryptedAccountNumber);
                    if (paymentMethod != default(AccountPaymentMethod))
                    {
                        paymentMethodId = paymentMethod.AccountPaymentMethodID;
                    }
                }
                %>
                <%
                foreach (var paymentMethod in paymentMethods.OrderByDescending(pm => pm.IsDefault))
                {
                %>
                <option value="<%=paymentMethod.AccountPaymentMethodID%>" <%=Html.Raw((ViewData["PaymentMethodID"] != null && (int)ViewData["PaymentMethodID"] > 0 ? (int)ViewData["PaymentMethodID"] : paymentMethodId) == paymentMethod.AccountPaymentMethodID ? "selected=\"selected\"" : "") %>>
                    <%= (paymentMethod.ProfileName + (paymentMethod.IsDefault ? " (" + Html.Term("default", "default") + ")" : ""))%>
                </option>
                <%}%>
                <%
                foreach (var nonProfilePaymentMethod in nonProfilePaymentMethods.OrderByDescending(nppm => nppm.GetTerm()))
                { 
                %>
                <option value="-<%=nonProfilePaymentMethod.PaymentTypeID%>" <%=Html.Raw((ViewData["PaymentMethodID"] != null && (int)ViewData["PaymentMethodID"] < 0 ? (int)ViewData["PaymentMethodID"] : paymentMethodId) == -nonProfilePaymentMethod.PaymentTypeID ? "selected=\"selected\"" : "")%>>
                    <%=nonProfilePaymentMethod.GetTerm()%>
                </option>
                <%
                }
                %>
            </select>
            </div>
</div>

<table class="PaymentSelector">
    <tr>
        <td>
            <div id="paymentMethodContainer">
                <% foreach (var paymentMethod in paymentMethods)
                   {     
                %>
                <div id="paymentMethod<%=paymentMethod.AccountPaymentMethodID%>" class="pad5 paymentMethodDisplay">
                    <a title="<%= Html.Term("Edit", "Edit")%>" class="FR UI-icon-container" onclick="editPaymentMethod(<%=paymentMethod.AccountPaymentMethodID%>)">
                        <span class="UI-icon icon-edit"></span></a>
                        <%--<b><%=(paymentMethod.ProfileName)%></b>--%>
                   
                    <%=Html.Raw(paymentMethod.ToDisplay(CoreContext.CurrentCultureInfo))%>
                </div>
                <% }%>
                <% foreach (var nonProfilePaymentMethod in nonProfilePaymentMethods.OrderByDescending(nppm => nppm.GetTerm()))
                   { %>                                
                    <div id="paymentMethod-<%= nonProfilePaymentMethod.PaymentTypeID %>" class="paymentMethodDisplay">
                    <% if (nonProfilePaymentMethod.PaymentTypeID == Constants.PaymentType.ProductCredit.ToInt())
                        { %>
                    <%= Html.Partial("_ProductCredit") %>
                    <% }
                        else if (nonProfilePaymentMethod.PaymentTypeID == Constants.PaymentType.GiftCard.ToInt())
                        { %>
                    <%= Html.Term("GiftCardBalance", "Gift Card Balance")%>:&nbsp;<span id="GiftCardBalance">-</span>
                    <br />
                    <%= Html.Term("GiftCardCode", "Gift Card Code")%>:&nbsp;<input type="text" id="GiftCardCode" class="TextInput" style="width: 75px;" />
                    <img id="btnLookupGC" src="<%= ResolveUrl("~/Resource/Content/Images/Icons/search.png") %>" alt="<%= Html.Term("Lookup Gift Card Balance") %>" title="<%= Html.Term("Lookup Gift Card Balance") %>" style="height: 18px;" />
                    <img id="gcLoader" src="<%= ResolveUrl("~/Resource/Content/Images/loader_20x20.gif") %>" style="display: none;" alt="loading" />
                    <% } %>
                </div>
                <% } %>
            </div>
            <div class="UI-lightBg pad5 amountToApply">
                <%--<span class="FL" style="width:180px">
                    <label><%= Html.Term("Label-PayentAmountToApply", "Amount to Apply")%>:</label>
                    <%= System.Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol %>
                    <input
                   
                        id="txtPaymentAmount" type="text" class="pad3 TextInput applyPaymentInput"  value="<%= Model.Balance.GetRoundedNumber() %>"
                       style="background:white;border-width: 0px; display: block; font-size:medium; color:Black;width:60px;" 
                        />
                       <input type="hidden" id="hdnPaymentAmount" />
                </span>--%>
                <br />
                  
         
                <%--<a href="javascript:void(0);" id="btnApplyPayment" class="FL ml10 UI-icon-container" title="Apply this amount to the balance due">
                    <span class="UI-icon icon-plus"></span>
                </a>--%>
                <div id="dvBtnApplyPayment">
                <a id="btnApplyPayment" href="javascript:void(0);" class="UI-icon-container" title="<%= Html.Term("ApplyPayment", "ApplyPayment")%>">
                    <div style="padding-top:5px">
                        <span class="add"></span>
                        <span class="addtext"><%= Html.Term("ApplyPayment", "ApplyPayment")%></span>
                    </div>
                </a>
                </div>
                <img id="loaderApplyPayment" src="<%= ResolveUrl("~/Resource/Content/Images/loader_20x20.gif") %>" style="display: none;" alt="loading" class="FR" />
                <span class="clr"></span>
            </div>
              <div class="UI-lightBg pad5 amountToApply">
                <%--<span class="FL" style="width:180px">--%>
                  <p  class="InputTools" style="font-weight: bold;font-size:15px;" id="lblTargetCredit" >

                  <input id="txtNamePayment" type="text"  
                  value="<%= Html.Term("NumberofSharestoDeferPayment", "Number of shares to defer payment")%>"
                  style="width: 9px;background:white;border-width: 0px;font-weight: bold;font-size:15px;visibility:hidden;" readonly="readonly"
                      />
                
                     <input id="txtNumberCuotas" type="text"  class="TextInput"
                      style="width: 9px;background:white;border-width: 0px;font-weight: bold;font-size:15px;visibility:hidden;" readonly="readonly"
                      />							
                     <input id="txtMsg"  type="text" class="addtext"
                       style="width: 470px; border-width:0;font-weight: bold;font-size:15px;" readonly="readonly"
                      />      						
                     </p>
                <%--</span>--%>
             </div>
        </td>
    </tr>
</table>
