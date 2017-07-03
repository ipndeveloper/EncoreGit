<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.AccountPaymentMethod>" %>
<% 
    var phoneNumber = Model.BillingAddress == null ? string.Empty : Model.BillingAddress.PhoneNumber;
%>
<script type="text/javascript">
    $(function () {
        $('#paymentMethodAddressPhone').phone().phone('setPhone', '<%= phoneNumber %>');
        $('#existingAddress').change(function () {
            $.get('<%= ResolveUrl("~/Accounts/BillingShippingProfiles/GetAddressControl") %>', { addressId: $(this).val(), prefix: 'paymentMethodAddress' }, function (addressEntry) {
                $('#billingAddressPlaceHolder').html(addressEntry);
                $('#paymentMethodAddressPhone').phone();
            });
        });
        var showPaymentType = function () {
            if ($('#rbPaymentEFT').prop('checked')) {
                $('#creditCard').hide();
                $('#EFT').show();
            }
            else {
                $('#creditCard').show();
                $('#EFT').hide();
            }
        }; 

        $('input[name="payment"]').click(showPaymentType);
        showPaymentType();

        $('#btnSavePaymentMethod').click(function () {
            if ($('#rbPaymentEFT').prop('checked')) {
                if (!$('#paymentMethodEntry').checkRequiredFields()) {
                    return false;
                }
                
                if (!$('#accountInput').val()) {
                    $('#accountInput').showError('<%= Html.JavascriptTerm("AccountNumberIsRequired", "Account Number is required.") %>');
                    return false;
                }

                if (!$('#routingInput').val()) {
                    $('#routingInput').showError('<%= Html.JavascriptTerm("RoutingNumberIsRequired", "Routing Number is required.") %>');
                    return false;
                }

                if ($('#accountTypeAccount').val() == '<%=Html.Term("-ChooseAccountType-","-Choose Account Type-") %>') {
                    $('#accountTypeAccount').showError('<%= Html.JavascriptTerm("AccountTypeIsRequired", "Account Type is required.") %>');
                    return false;
                }

                if (!$('#verifyroutingInput').val()) {
                    $('#verifyroutingInput').showError('<%= Html.JavascriptTerm("VerifyRoutingNumberIsRequired", "Verify Routing Number is required.") %>');
                    return false;
                }

                if (!$('#verifyaccountInput').val()) {
                    $('#verifyaccountInput').showError('<%= Html.JavascriptTerm("VerifyAccountNumberIsRequired", "Verify Account Number is required.") %>');
                    return false;
                }

                $('#addressEntry input').each(function () {
                    if ($(this).val() == $(this).attr('title')) {
                        $(this).val('');
                    }
                });

                if ($('#routingInput').val() != $('#verifyroutingInput').val()) {
                    $('#routingInput,#verifyroutingInput').showError('<%= Html.JavascriptTerm("RoutingNumbersDoNotMatch", "Routing Numbers do not match") %>').keyup(function () {
                        if ($('#routingInput').val() == $('#verifyroutingInput').val())
                            $('#routingInput,#verifyroutingInput').clearError();
                    });
                    $('#routingInput').focus();
                    return false;
                }

                if ($('#accountInput').val() != $('#verifyaccountInput').val()) {
                    $('#accountInput,#verifyaccountInput').showError('<%= Html.JavascriptTerm("AccountNumbersDoNotMatch", "Account Numbers do not match") %>').keyup(function () {
                        if ($('#accountInput').val() == $('#verifyaccountInput').val())
                            $('#accountInput,#verifyaccountInput').clearError();
                    });
                    $('#accountInput').focus();
                    return false;
                }

                if ($('#BankAccountCertCheckbox').prop('checked')) {
                    var p = $(this).parent();
                    showLoading(p);

                var postBaseUrl = '<%= ResolveUrl("~/Accounts/BillingShippingProfiles") %>';
                
                if (($('#currentArea').val() == 'Orders' && $('#currentController').val() == 'OrderEntry') || ($('#currentArea').val() == 'Accounts' && $('#currentController').val() == 'Autoships'))
                    postBaseUrl = '<%= ResolveUrl("~/") %>' + $('#currentArea').val() + '/' + $('#currentController').val();

                alert("SavePaymentMethodEFT");
                    $.post(postBaseUrl + '/SavePaymentMethodEFT', {
                        paymentMethodId: '<%= Model.AccountPaymentMethodID == 0 ? "" : Model.AccountPaymentMethodID.ToString() %>',
                        bankName: $('#BankName').val(),
                        nameOnAccount: $('#nameOnAccount').val(),
                        routingInput: $('#routingInput').val(),
                        accountInput: $('#accountInput').val(),
                        bankAccountTypeID: $('#accountTypeAccount option:selected').val(),
                        profileName: $('#paymentMethodAddressProfileName').val(),
                        attention: $('#paymentMethodAddressAttention').val(),
                        address1: $('#paymentMethodAddressAddress1').val(),
                        address2: $('#paymentMethodAddressAddress2').val(),
                        address3: $('#paymentMethodAddressAddress3').val(),
                        zip: $('#paymentMethodAddressControl .PostalCode').fullVal(),
                        city: $('#paymentMethodAddressCity').val(),
                        county: $('#paymentMethodAddressCounty').val(),
                        street: $('#paymentMethodAddressStreet').val(),
                        state: $('#paymentMethodAddressState').val(),
                        countryId: $('#paymentMethodAddressCountry').val(),
                        phone: $('#paymentMethodAddressPhone').length ? $('#paymentMethodAddressPhone').phone('getPhone') : '',
                        addressId: $('#existingAddress').val()
                    }, function (response) {
                        if (response.result) {
                            hideLoading(p);
                            $('#editPaymentMethodModal').jqmHide();
                            getPaymentMethods();
                        }
                        else {
                            hideLoading(p);
                            showMessage(response.message, true);
                        }
                    });
                }
                else {
                    showMessage('<%= Html.Term("AccountCertCheckbox", "Account Certify Checkbox should be checked")%>', true);
                    $('#BankAccountCertCheckbox').focus();
                    return false;
                }
            }
            else {
                if (!$('#paymentMethodEntry').checkRequiredFields()) {
                    return false;
                }

                if (!CreditCard.validate($('#accountNumber').val()).isValid) {
                    if (!/\**\d{4}/.test($('#accountNumber').val())) {
                        $('#accountNumber').showError('<%= Html.JavascriptTerm("CreditCardNumberIsInvalid", "Credit card number is invalid.") %>');
                        return false;
                    }
                }
                var today = new Date();
                var lastDayInMonth = new Date(today.getFullYear(), today.getMonth(), 0);

                if (new Date().setFullYear($('#expYear').val(), $('#expMonth').val() - 1, lastDayInMonth.getDate()) < today) {
                    $('#expMonth').showError('');
                    $('#expYear').showError('<%= Html.JavascriptTerm("ThisExpirationDateIsInThePast.", "This expiration date is in the past.") %>');
                    return false;
                }

                $('#addressEntry input').each(function () {
                    if ($(this).val() == $(this).attr('title')) {
                        $(this).val('');
                    }
                });

                var p = $(this).parent();
                showLoading(p);

                var postBaseUrl = '<%= ResolveUrl("~/Accounts/BillingShippingProfiles") %>'; 
                
                if (($('#currentArea').val() == 'Orders' && $('#currentController').val() == 'OrderEntry') || ($('#currentArea').val() == 'Accounts' && $('#currentController').val() == 'Autoships'))
                    postBaseUrl = '<%= ResolveUrl("~/") %>' + $('#currentArea').val() + '/' + $('#currentController').val();

                //alert("SavePaymentMethod");
                $.post(postBaseUrl + '/SavePaymentMethod', {
                    paymentMethodId: '<%= Model.AccountPaymentMethodID == 0 ? "" : Model.AccountPaymentMethodID.ToString() %>',
                    //accountName: $('#accountName').val(),
                    nameOnCard: $('#nameOnCard').val(),
                    accountNumber: $('#accountNumber').val(),
                    expDate: $('#expMonth').val() + '/1/' + $('#expYear').val(),
                    profileName: $('#paymentMethodAddressProfileName').val(),
                    attention: $('#paymentMethodAddressAttention').val(),
                    address1: $('#paymentMethodAddressAddress1').val(),
                    address2: $('#paymentMethodAddressAddress2').val(),
                    address3: $('#paymentMethodAddressAddress3').val(),
                    zip: $('#paymentMethodAddressControl .PostalCode').fullVal(),
                    city: $('#paymentMethodAddressCity').val(),
                    county: $('#paymentMethodAddressCounty').val(),
                    street: $('#paymentMethodAddressStreet').val(),
                    state: $('#paymentMethodAddressState').val(),
                    countryId: $('#paymentMethodAddressCountry').val(),
                    phone: $('#paymentMethodAddressPhone').length ? $('#paymentMethodAddressPhone').phone('getPhone') : '',
                    addressId: $('#existingAddress').val()
                }, function (response) {
                    if (response.result) {
                        hideLoading(p);
                        $('#editPaymentMethodModal').jqmHide();
                        getPaymentMethods();
                    }
                    else {
                        hideLoading(p);
                        showMessage(response.message, true);
                    }
                });
            }
        });
    });
</script>
<%
   bool hasFunction = (CoreContext.CurrentUser.HasFunction(SmallCollectionCache.Instance.PaymentTypes.Where(p => p.PaymentTypeID == (int)Constants.PaymentType.EFT).FirstOrDefault().FunctionName)) ? paymentMethodTypeChoose.Visible = true : paymentMethodTypeChoose.Visible = false;
%>

<div id="paymentMethodTypeChoose" runat="server" class="mb10 overflow">
    <h2>
        <%= Html.Term("PaymentMethodType", "Payment Method Type") %>
    </h2>
    <div>
        <p>
            <%if (!Model.RoutingNumber.IsNullOrEmpty())
              {%>
                 <input id="rbPaymentCreditCard" name="payment" value="CreditCard" type="radio" disabled="disabled" /><label for="rbPaymentCreditCard"><%= Html.Term("CreditCard", "Credit Card")%></label>
                 <input id="rbPaymentEFT" name="payment" value="EFT" type="radio" checked="checked"/><label for="rbPaymentEFT"><%= Html.Term("BankAccount", "Bank Account")%></label>
             <%}
              else if(!Model.ExpirationDate.IsNullOrEmpty())
              { %>
                 <input id="rbPaymentCreditCard" name="payment" value="CreditCard" type="radio" checked="checked"/><label for="rbPaymentCreditCard"><%= Html.Term("CreditCard", "Credit Card")%></label>
                 <input id="rbPaymentEFT" name="payment" value="EFT" type="radio" disabled="disabled"/><label for="rbPaymentEFT"><%= Html.Term("BankAccount", "Bank Account")%></label>
            <%}
            else
              { %>
                 <input id="rbPaymentCreditCard" name="payment" value="CreditCard" type="radio" checked="checked"/><label for="rbPaymentCreditCard"><%= Html.Term("CreditCard", "Credit Card")%></label>
                 <input id="rbPaymentEFT" name="payment" value="EFT" type="radio"/><label for="rbPaymentEFT"><%= Html.Term("BankAccount", "Bank Account")%></label>
            <%} %>
        </p>
    </div>
</div>

<table id="paymentMethodEntry" >
    <tr>
        <td id="EFT" style="display: none;">
            <h2>
                <%= Model.AccountPaymentMethodID == 0 ? Html.Term("AddaNewPaymentMethod", "Add a New Payment Method") : Html.Term("Edit", "Edit") + " " + Model.ProfileName %>
            </h2>
            <div class="mr10">
                <p>
                    <span class="FL Label">
                        <label for="BankName">
                            <%= Html.Term("BankName", "Bank Name")%>:</label></span>
                    <input type="text" maxlength="100" id="BankName" name="<%= Html.Term("BankNameIsRequired", "Bank Name is required.")%>"
                        class="required" value="<%= Model.BankName %>" />
                </p>
                <p>
                    <span class="FL Label">
                        <label for="nameOnAccount">
                            <%= Html.Term("NameonAccount", "Name on Account")%>:</label></span>
                    <input type="text" maxlength="50" id="nameOnAccount" name="<%= Html.Term("NameOnAccountIsRequired", "Name on Account is required.")%>"
                        class="required" value="<%= Model.NameOnCard %>" />
                </p>
                <p>
                    <span class="FL Label">
                        <label for="routingTransit">
                            <%= Html.Term("Routing/Transit#", "Routing/Transit #")%>:</label></span>
                        <input type="text" maxlength="40" id="routingInput" name="<%= Html.Term("RoutingNumberIsRequired", "Routing Number is required.")%>"
                        class="required" value="<%= Model.RoutingNumber %>" />
                </p>
                <p>
                    <span class="FL Label">
                        <label for="verifyroutingTransit">
                            <%= Html.Term("VerifyRouting/Transit#", "Verify Routing/Transit #")%>:</label></span>
                    <input type="text" maxlength="40" id="verifyroutingInput" name="<%= Html.Term("VerifyRoutingNumberIsRequired", "Verify Routing Number is required.")%>"
                        class="required" />
                </p>
                <p>
                    <span class="FL Label">
                        <label for="bankAccountNumber">
                            <%= Html.Term("Account#", "Account #")%>:</label></span>
                    <input type="text" maxlength="40" id="accountInput" name="<%= Html.Term("AccountNumberIsRequired", "Account Number is required.")%>"
                        class="required" value="<%= Model.DecryptedAccountNumber.MaskString(4) %>" />
                </p>
                <p>
                    <span class="FL Label">
                        <label for="verifybankAccountNumber">
                            <%= Html.Term("VerifyAccount#", "Verify Account #")%>:</label></span>
                    <input type="text" maxlength="40" id="verifyaccountInput" name="<%= Html.Term("VerifyAccountNumberIsRequired", "Verify Account Number is required.")%>"
                        class="required" />
                </p>
                <p>
                    <span class="FL Label">
                        <%= Html.Term("AccountType","Account Type") %>: </span>
                    <select id="accountTypeAccount" class="required" name="<%= Html.Term("BankAccountTypeIsRequired", "Bank Account Type is required")%>">
                        <option>
                          <%=Html.Term("-ChooseAccountType","-Choose Account Type-")%>
                        </option>
                        <%foreach (var bankType in Enum.GetValues(typeof(Constants.BankAccountTypeEnum)).Cast<Constants.BankAccountTypeEnum>())
                          { %>
                        <option value="<%=bankType.ToShort()%>" <%= Model.BankAccountTypeID == bankType.ToShort() ? "selected=\"selected\"" : string.Empty %>><%=Html.Term("BankAccountType_" + bankType.ToString(), bankType.ToString())%></option>
                        <%} %>
			        </select>
                </p>
                <p>
                    <input type="checkbox" id="BankAccountCertCheckbox" />
                    <label for="BankAccountCert">
                        <%= Html.Term("BankAccountCert", "I certify that I am the owner of the account")%></label>
                </p>
            </div>
        </td>
        <td id="creditCard" style="display: none;">
            <h2>
                <%= Model.AccountPaymentMethodID == 0 ? Html.Term("AddaNewPaymentMethod", "Add a New Payment Method") : Html.Term("Edit", "Edit") + " " + Model.ProfileName %>
            </h2>
            <div class="mr10">
                <%-- <p>
                    <span class="FL Label">Account Name:</span>
                    <input type="text" id="accountName" name="Account Name is required." class="required"
                        value="<%= Model.ProfileName %>" />
                </p>--%>
                <p>
                    <span class="FL Label">
                        <label for="nameOnCard">
                            <%= Html.Term("NameonCard", "Name on Card")%>:</label></span>
                    <input type="text" maxlength="50" id="nameOnCard" name="<%= Html.Term("NameOnCardisRequired", "Name On Card is required.")%>"
                        class="required" value="<%= Model.NameOnCard %>" />
                </p>
                <p>
                    <span class="FL Label">
                        <label for="accountNumber">
                            <%= Html.Term("CreditCard#", "Credit Card #")%>:</label></span>
                    <input type="text" maxlength="16" id="accountNumber" name="<%= Html.Term("CardNumberIsRequired", "Card Number is required.")%>"
                        class="required" value="<%= Model.DecryptedAccountNumber.MaskString(4) %>" />
                </p>
                <p>
                    <span class="FL Label">
                        <%= Html.Term("Expiration", "Expiration")%>:</span>
                    <select id="expMonth" name="<%= Html.Term("ExpMonthIsRequired", "Exp Month is required.")%>"
                        title="0" class="required">
                        <% for (int i = 1; i <= 12; i++)
                           { %>
                        <option value="<%= i %>" <%= i == Model.ExpirationDate.ToDateTime().Month ? "selected=\"selected\"" : "" %>>
                            <%= i + " - " + Html.Term(Enum.ToObject(typeof(Constants.Month), i).ToString())%></option>
                        <% } %>
                    </select>
                    &nbsp;/&nbsp;
                    <select id="expYear" name="<%= Html.Term("ExpYearisRequired.", "Exp Year is required.")%>"
                        title="" class="required">
                        <%  if (Model.ExpirationDate.HasValue && Model.ExpirationDate.Value.Year < DateTime.Today.Year)
                            { %>
                               <option value="<%= Model.ExpirationDate.Value.Year %>" selected="selected"><%= Model.ExpirationDate.Value.Year.ToString() %></option>
                        <% } %>
                        <% for (int i = DateTime.Today.Year; i <= DateTime.Today.Year + 10; i++)
                           {%>
                        <option value="<%= i %>" <%= i == Model.ExpirationDate.ToDateTime().Year ? "selected=\"selected\"" : "" %>>
                            <%= i.ToString() %></option>
                        <% } %>
                    </select>
                </p>
            </div>
        </td>
        <td>
            <h2>
                <%= Html.Term("BillingAddress", "Billing Address")%></h2>
            <p>
                <select id="existingAddress">
                    <option value="">--
                        <%= Html.Term("AddaNewBillingAddress", "Add a new billing address")%>
                        --</option>
                    <%foreach (Address billingAddress in CoreContext.CurrentAccount.Addresses.Where(a => a.AddressTypeID == Constants.AddressType.Billing.ToInt()))
                      { %>
                    <option value="<%= billingAddress.AddressID %>" <%= billingAddress.AddressID == Model.BillingAddress.AddressID ? "selected=\"selected\"" : "" %>>
                        <%= billingAddress.ProfileName %></option>
                    <%} %>
                </select>
            </p>
            <div id="billingAddressPlaceHolder">
                <% 
                    Html.RenderPartial("Address", new AddressModel()
                    {
                        Address = Model.BillingAddress,
                        LanguageID = CoreContext.CurrentLanguageID,
                        ShowCountrySelect = true,
                        ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
                        Prefix = "paymentMethodAddress"
                    }); 
                %>
                <%--<%= NetSteps.Web.Mvc.Business.Controls.AddressControl.RenderAddress(Model.BillingAddress, CoreContext.CurrentLanguageID, true, "~/Accounts/BillingShippingProfiles/GetAddressControl", prefix:"paymentMethodAddress")%>--%>
            </div>
        </td>
    </tr>
</table>
<br />
<label for="chkSavePaymentInfo">
    <%= Html.Term("SavePaymentInfo", "Save Payment Info")%></label>:
<input type="checkbox" id="chkSavePaymentInfo" checked="checked" />
<hr />
<p>
    <a href="javascript:void(0);" id="btnSavePaymentMethod" class="Button BigBlue">
        <%= Html.Term("SavePaymentMethod", "Save Payment Method")%></a>
    <a href="javascript:void(0);" class="Button jqmClose">
        <%= Html.Term("Cancel", "Cancel")%></a>
</p>
<span class="ClearAll"></span>
