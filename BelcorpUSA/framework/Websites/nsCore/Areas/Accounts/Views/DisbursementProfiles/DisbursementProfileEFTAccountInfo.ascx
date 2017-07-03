<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<DisbursementProfileEFTAccountInfoEditModel>" %>
<%@ Import Namespace="NetSteps.Addresses.Common.Models" %>
<% int accountNumber = (int)ViewData["AccountNumber"]; %>
<% string pleaseEnterValidZip = Html.Term("PleaseEnteraValidZip", "Please enter a valid zip"); %>
<script type="text/javascript">
    $(function () {
        $('#bankPhoneAccount<%= accountNumber %>').phone({ areaCodeId: 'txtBankPhoneAreaCodeAccount<%= accountNumber %>', firstThreeId: 'txtBankPhoneFirstThreeAccount<%= accountNumber %>', lastFourId: 'txtBankPhoneLastFourAccount<%= accountNumber %>' });
        $('#bankPhoneAccount<%= accountNumber %>').phone('setPhone', '<%= Model.DisbursementProfile.BankPhone %>'.replace(/\D/g, ''));
        $('#txtBankAddressLine1Account<%= accountNumber %>').watermark('Line 1');
        $('#txtBankAddressLine2Account<%= accountNumber %>').watermark('Line 2');
        $('#txtBankAddressLine3Account<%= accountNumber %>').watermark('Line 3');
        //$('#txtBankZipAccount<%= accountNumber %>').val('<%= Model.BankAddress.PostalCode %>');
        var total = 0;
        $('.percentToDeposit').numeric().each(function () {
            if (!isNaN($(this).val()))
                total += parseFloat($(this).val());
        });
        if ('<%= accountNumber %>' == 1 && total == 0) {
            $('#percentToDepositLabelAccount<%= accountNumber %>').val('100');
        }
        $('#chkEnabledAccount<%= accountNumber %>').click(function () {
            $('#txtNameAccount<%= accountNumber %>,#txtRoutingNumberAccount<%= accountNumber %>,#txtAccountNumberAccount<%= accountNumber %>')[$(this).prop('checked') ? 'addClass' : 'removeClass']('required').clearError();
        });
        function updateCityState(controlId, cityHtml, stateHtml) {
            // update controls
            if (!cityHtml && !stateHtml) {
                cityHtml = stateHtml = '<option value="">-- <%=Html.Term("PleaseEnterValidZip", "Please Enter Valid Zip") %> --</option>';
            }
            $('#txtBankCityAccount' + controlId).html(cityHtml);
            $('#txtBankStateAccount' + controlId).html(stateHtml);
        }

        $('.routingNumber').numeric({ allowNegative: false });
        $('.zipcode').numeric({ allowNegative: false });
        $('.zipcode').keyup(function () {
            var cityHtml = '';
            var stateHtml = '';
            var controlId = this.id.substring(17);
            if (this.value.length == 5) {
                // look up zipcode
                // loadingElement.show();
                //cityControl.add(countyControl).add(stateControl).empty();

                zipXHR = $.getJSON('/Accounts/DisbursementProfiles/LookupZip', { countryId: 1, zip: this.value }, function (results) {
                    zipXHR = undefined;
                    lastZip = this.value;
                    //loadingElement.hide();
                    
                    if (results==null || results.length==0) {
                        updateCityState(controlId);
                        if (showMessage && results.message) {
                            showMessage(results.message, true);
                        }
                    } else {
                        var cityName = $('#txtBankCityAccount' + controlId).val();
                        var stateName = $('#txtBankStateAccount' + controlId).val();
                        
                        for (var i = 0; i < results.length; i++) {
                            if (!cityHtml.contains(results[i].city.trim())) {
                                cityHtml += '<option value=\"' + results[i].city + '\" ' + (results[i].city == cityName ? ' selected=\"selected\"' : '') + '>' + results[i].city.trim() + '</option>';
                            }
                            if (!stateHtml.contains(results[i].state.trim())) {
                                stateHtml += '<option value=\"' + results[i].stateId + '\">' + results[i].state.trim() + '</option>';
                            }
                        }
                    }
                    updateCityState(controlId, cityHtml, stateHtml);
                });
            }
            else {
                updateCityState(controlId);
            }
        });
        $('.zipcode').keyup();
    });
    
</script>
<% IAddress bankAddress = Model.BankAddress ?? new Address();
   bool enabled = Model.DisbursementProfile.IsEnabled; %>
<h4 class="ModTitle">
    <!-- CORE TEST -->

	<%= Html.Term("Account", "Account")%>
	<%= accountNumber %>:
</h4>
<input id="EFTDisbursementProfileID<%= accountNumber %>" type="hidden" value="<%= Model.DisbursementProfile.DisbursementProfileId %>" name="EFTDisbursementProfileID<%= accountNumber %>" />
<table cellspacing="0" class="DataGrid EFTAccount">
	<tr>
		<td style="width: 130px;">
			<label for="chkEnabledAccount<%= accountNumber %>">
				<%= Html.Term("Enabled", "Enabled")%>:</label>
		</td>
		<td>
			<input id="chkEnabledAccount<%= accountNumber %>" type="checkbox" <%= enabled ? "checked=\"checked\"" : "" %> />
		</td>
	</tr>
	<tr>
		<td>
			<label for="txtNameAccount<%= accountNumber %>">
				<%= Html.Term("Name on Account", "Name on Account")%>:</label>
		</td>
		<td>
			<input id="txtNameAccount<%= accountNumber %>" type="text" value="<%= Model.DisbursementProfile.NameOnAccount %>"
				<%= enabled ? "class=\"required\"" : "" %> name="<%= Html.Term("NameRequired", "Name is required") %>" />
		</td>
	</tr>
	<tr>
		<td>
			<label for="txtRoutingNumberAccount<%= accountNumber %>">
				<%= Html.Term("Routing/Transit#", "Routing/Transit #")%>:</label>
		</td>
		<td>
			<input id="txtRoutingNumberAccount<%= accountNumber %>" type="text" value="<%= Model.DisbursementProfile.RoutingNumber %>"
				maxlength="9" class="routingNumber<%= enabled ? " required" : "" %>" name="<%= Html.Term("RoutingNumberRequired", "Routing Number is required") %>" />
		</td>
	</tr>
	<tr>
		<td>
			<label for="txtAccountNumberAccount<%= accountNumber %>">
				<%= Html.Term("Account#", "Account #")%>:</label>
		</td>
		<td>
			<input id="txtAccountNumberAccount<%= accountNumber %>" type="text" value="<%= Model.DisbursementProfile.AccountNumber %>"
				<%= enabled ? "class=\"required\"" : "" %> name="<%= Html.Term("AccountNumberRequired", "Account Number is required") %>" />
		</td>
	</tr>
	<tr>
		<td>
			<label for="txtBankNameAccount<%= accountNumber %>">
				<%= Html.Term("BankName", "Bank Name")%>:</label>
		</td>
		<td>
			<input id="txtBankNameAccount<%= accountNumber %>" type="text" value="<%= Model.DisbursementProfile.BankName %>" />
		</td>
	</tr>
	<tr>
		<td>
			<label for="bankPhoneAccount<%= accountNumber %>">
				<%= Html.Term("BankPhone", "Bank Phone")%>:</label>
		</td>
		<td id="bankPhoneAccount<%= accountNumber %>">
			<%--<input id="txtBankPhoneAccount<%= accountNumber %>" type="text" />--%>
		</td>
	</tr>
	<tr>
		<td style="vertical-align: top;">
			<%= Html.Term("Bank Address", "Bank Address")%>:
		</td>
		<td>
			<p>
				<input id="txtBankAddressLine1Account<%= accountNumber %>" type="text" value="<%= bankAddress.Address1 %>" />
			</p>
			<p>
				<input id="txtBankAddressLine2Account<%= accountNumber %>" type="text" value="<%= bankAddress.Address2 %>" /></p>
			<input id="txtBankAddressLine3Account<%= accountNumber %>" type="text" value="<%= bankAddress.Address3 %>" />
			<br />
		</td>
	</tr>
	<tr>
		<td>
			<label for="txtBankZipAccount<%= accountNumber %>">
				<%= Html.Term("Bank Zip", "Bank Zip")%>:</label>
		</td>
		<td>
			<input id="txtBankZipAccount<%= accountNumber %>" type="text" value="<%= bankAddress.PostalCode %>" maxlength="5" class="zipcode"/>
		</td>
	</tr>
    <tr>

		<td>
			<label for="txtBankCityAccount<%= accountNumber %>">
				<%= Html.Term("BankCity#", "Bank City")%>:</label>
		</td>
		<td>
			<select name="City is required." id="txtBankCityAccount<%= accountNumber %>" class="City"><option value="">-- <%=pleaseEnterValidZip %> --</option></select>
            <input type="hidden" id="cityName<%= accountNumber %>" value="<%= bankAddress.City %>" />
		</td>
	</tr>
	<tr>
		<td>
			<label for="txtBankStateAccount<%= accountNumber %>">
				<%= Html.Term("Bank State", "Bank State")%>:</label>
		</td>
		<td>
        <select name="State is required." id="txtBankStateAccount<%= accountNumber %>" class="State"><option value="">-- <%=pleaseEnterValidZip %> --</option></select>
            <input type="hidden" id="stateName<%= accountNumber %>" value="<%= bankAddress.State %>" />
		</td>
	</tr>
	
	<tr>
		<td>
			<label for="accountTypeAccount<%= accountNumber %>">
				<%= Html.Term("Acct Type", "Acct Type")%>:</label>
		</td>
		<td>
			<select id="accountTypeAccount<%= accountNumber %>">
				<option <%= Model.DisbursementProfile.BankAccountTypeId == (int)NetSteps.Data.Entities.Constants.BankAccountTypeEnum.Checking ? "selected=\"selected\"" : "" %>>
					Checking</option>
				<option <%= Model.DisbursementProfile.BankAccountTypeId == (int)NetSteps.Data.Entities.Constants.BankAccountTypeEnum.Savings ? "selected=\"selected\"" : "" %>>
					Savings</option>
			</select>
		</td>
	</tr>
	<tr>
		<td>
			<label for="percentToDepositAccount<%= accountNumber %>">
				<%= Html.Term("%toDeposit", "% to Deposit")%>:</label>
		</td>
		<td>
			<span>
				<input id="percentToDepositAccount<%= accountNumber %>" type="text" class="percentToDeposit"
					size="2" value="<%= Model.DisbursementProfile.Percentage * 100 %>" />%</span>
		</td>
	</tr>
</table>
