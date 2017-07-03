var checkUseEftEnabled = function (ai) {
    var eIdString = '#' + ai,
            eParent = $(eIdString).closest('.postalcodelookup-container'),
            inputs = 'input:not(' + eIdString + '),select';
    if (!$(eIdString).is(':checked')) {
        eParent.find(inputs).each(function () {
            $(this).attr('disabled', 'disabled');
        });

        eParent = $(eIdString).closest('.eftBankPhone');
        eParent.find(inputs).each(function () {
            $(this).attr('disabled', 'disabled');
        });

    } else {
        eParent.find(inputs).each(function () {
            $(this).removeAttr('disabled');
        });
    }
};

$(function () {
    var data;
    var eftRequiredFields = [
          "txtNameAccount"
        , "txtRoutingNumberAccount"
        , "txtAccountNumberAccount"
        , "txtBankNameAccount"
        , "bankAddressAddress1_"
        , "txtBankZipAccount"
        , "bankAddressCity"
        , "bankAddressState"
    ];

    var setEFTRequiredFields = function () {
        for (var i = 1; i <= $('.EFTAccount').length; i++) {
            if ($('#chkEnabledAccount' + i).is(':checked')) {
                eftRequiredFields.forEach(function (index) {
                    $('#' + index + i).addClass('required');
                });
            } else {
                eftRequiredFields.forEach(function (index) {
                    $('#' + index + i).clearError();
                    $('#' + index + i).removeClass('required');
                });
            }
        }
    };

    var showPreference = function () {
        if ($('#rbPaymentCheck').is(':checked')) {
            $('#check').show();
            $('#eft').hide();
            $('#payoneer').hide();
            $('#hyperWallet').hide();
            $('#propay').hide();
        } else if ($('#rbPaymentEFT').is(':checked')) {
            $('#check').hide();
            $('#eft').show();
            $('#payoneer').hide();
            $('#hyperWallet').hide();
            $('#propay').hide();
            setEFTRequiredFields();
        } else if ($('#rbPaymentPayoneer').is(':checked')) {
            $('#check').hide();
            $('#eft').hide();
            $('#payoneer').show();
            $('#hyperWallet').hide();
            $('#propay').hide();
        } else if ($('#rbPaymentHyperWallet').is(':checked')) {
            $('#check').hide();
            $('#eft').hide();
            $('#payoneer').hide();
            $('#hyperWallet').show();
            $('#propay').hide();
        } else if ($('#rbPaymentProPay').is(':checked')) {
            $('#check').hide();
            $('#eft').hide();
            $('#payoneer').hide();
            $('#hyperWallet').hide();
            $('#propay').show();
        }
    },
    checkUseAddressOfRecord = function () {
        if ($('#chkUseAddressOfRecord').is(':checked')) {
            $('#check input:not(#chkUseAddressOfRecord),#check select').attr('disabled', 'disabled');
            $('#txtPayableTo').val('@CoreContext.CurrentAccount.FullName');
            $('#txtAddressLine1').val('@Model.CheckAddress.Address1');
            $('#txtAddressLine2').val('@Model.CheckAddress.Address2');
            $('#txtAddressLine3').val('@Model.CheckAddress.Address3');
            $('#txtCity').val('@Model.CheckAddress.City');
            $('#txtState').val('@Model.CheckAddress.State');
            $('#txtZip').val('@Model.CheckAddress.PostalCode');
        } else {
            $('#check input:not(#chkUseAddressOfRecord),#check select').removeAttr('disabled');
            $('#txtPayableTo,#txtAddressLine1,#txtAddressLine2,#txtAddressLine3,#txtCity,#txtState,#txtZip').val('');
        }
    },
    isValidUSZip = function (zip) {
        if (!/^\d{5}(-\d{4})?$/.test(zip)) {
            showMessage('The zip code is not in a valid format', true);
        }
    };



    if ($('input[name="payment"]:checked').val() == 'EFT') {
        setEFTRequiredFields();
    }

    $('.eftCheckEnabled').click(function () {
        setEFTRequiredFields();
    });

    $('input[name="payment"]').click(showPreference);
    showPreference();

    $('.postalcodelookup-postalcode').keyup(function () {
        $('.postalcodelookup-city').clearError();
        $('.postalcodelookup-state').clearError();
    });

    $('#chkUseAddressOfRecord').click(checkUseAddressOfRecord);
    checkUseAddressOfRecord();

    $('input.eftCheckEnabled').click(function () {
        checkUseEftEnabled($(this).attr('id'));
    });

    $('#postalCode,.profileZip').change(function () {
        // empty
    });
});

function AreProfilesValid() {
    var isValid = true;
    if ($('input[name="payment"]:checked').val() == 'Check') { // Validate Check Profile Form
        if ($('#chkUseAddressOfRecord').is(':checked')) {
            window.data = {
                id: $('#CheckDisbursementProfileID').val(),
                preference: 'Check',
                useAddressOfRecord: true,
                profileName: $('#checkProfileProfileName').val(),
                payableTo: $('#checkProfileAttention').val(),
                address1: $('#checkProfileAddress1').val(),
                    address2: $('#checkProfileAddress2').val(),
                    address3: $('#checkProfileAddress3').val(),
                    city: $('#checkProfileCity').val(),
                    county: $('#checkProfileCounty').val(),
                    state: $('#checkProfileState').val(),
                    street: $('#checkProfileStreet').val(),
                    zip: $('#checkProfileZip').val() + $('#checkProfileZipPlusFour').val(),
                    country: $('#checkProfileCountry').val()

//                profileName: $('#profileName').val(),
//                payableTo: $('#attention').val(),
//                address1: $('#address1').val(),
//                address2: $('#address2').val(),
//                address3: $('#address3').val(),
//                city: $('#city').val(),
//                state: $('#state').val(),
//                zip: $('#zip').val() + $('#checkProfileZipPlusFour').val(),
//                country: $('#country').val()
            };
        } else {
            isValid = $('#check').checkRequiredFields();
            if (isValid) {
                window.data = {
                    id: $('#CheckDisbursementProfileID').val(),
                    preference: 'Check',
                    useAddressOfRecord: false,
                    profileName: $('#checkProfileProfileName').val(),
                    payableTo: $('#checkProfileAttention').val(),
                    address1: $('#checkProfileAddress1').val(),
                    address2: $('#checkProfileAddress2').val(),
                    address3: $('#checkProfileAddress3').val(),
                    city: $('#checkProfileCity').val(),
                    county: $('#checkProfileCounty').val(),
                    state: $('#checkProfileState').val(),
                    street: $('#checkProfileStreet').val(),
                    zip: $('#checkProfileZip').val() + $('#checkProfileZipPlusFour').val(),
                    country: $('#checkProfileCountry').val()

//                    profileName: $('#profileName').val(),
//                    payableTo: $('#attention').val(),
//                    address1: $('#address1').val(),
//                    address2: $('#address2').val(),
//                    address3: $('#address3').val(),
//                    city: $('#city').val(),
//                    state: $('#state').val(),
//                    zip: $('#zip').val(),
//                    country: $('#country').val()
                };
            }
        }
    } else if ($('input[name="payment"]:checked').val() == 'EFT') {
        isValid = $('#eft').checkRequiredFields();

        if (!$(".eftCheckEnabled:checkbox:checked").length) {
            showMessage('You must submit at least one enabled profile when selecting the EFT preference', true);
            isValid = false;
        }

        if (isValid) {
            var decimalCheckRegEx = /^[0-9]+$/;
            for (var i = 1; i <= $('.EFTAccount').length; i++) {
                if ($('#chkEnabledAccount' + i).is(':checked') && !$('#percentToDepositAccount' + i).val().match(decimalCheckRegEx)) {
                    showMessage('Percentage values cannot contain decimals', true);
                    isValid = false;
                }
            }
        }

        if (isValid) {
            var total = 0;
            for (var i = 1; i <= $('.EFTAccount').length; i++) {
                if ($('#chkEnabledAccount' + i).is(':checked') && !isNaN($('#percentToDepositAccount' + i).val())) {
                    var numberValue = parseInt($('#percentToDepositAccount' + i).val());
                    if (numberValue > 0) {
                        total += numberValue
                    } else {
                        showMessage('Percentage values cannot be less than zero', true);
                        isValid = false;
                    }
                }
            }

            if (total == 100) {
                $('#eft input').each(function () {
                    var t = $(this);
                    if (t.val() == t.data('watermark'))
                        t.val('');
                });
                window.data = {
                    id: $('#CheckDisbursementProfileID').val(),
                    preference: 'EFT',
                    agreementOnFile: $('#chkHardRelease').is(':checked')
                };

                window.data['bankID[0]'] = $('#BankID1').val();
                window.data['bankID[1]'] = $('#BankID2').val();

                var i = 1, i2;
                for (i; i <= $('.EFTAccount').length; i++) {
                    i2 = i - 1;
                    window.data['accounts[' + i2 + '].DisbursementProfileID'] = $('#EFTDisbursementProfileID' + i).val();
                    window.data['accounts[' + i2 + '].Enabled'] = $('#chkEnabledAccount' + i).is(':checked');
                    //window.data['accounts[' + i2 + '].Name'] = $('#txtNameAccount' + i).val();
                    window.data['accounts[' + i2 + '].Name'] = $('#accountTypeAccount' + i + ' option:selected').text();
                    window.data['accounts[' + i2 + '].RoutingNumber'] = $('#txtRoutingNumberAccount' + i).val();
                    window.data['accounts[' + i2 + '].AccountNumber'] = $('#txtAccountNumberAccount' + i).val();
                    window.data['accounts[' + i2 + '].BankID'] = $('#BankID' + i).val();
                    window.data['accounts[' + i2 + '].BankName'] = $('#BankID' + i + ' option:selected').text().trim();
                    window.data['accounts[' + i2 + '].BankPhone'] = $('#bankPhoneAccount' + i).phone('getPhone');
                    window.data['accounts[' + i2 + '].BankAddress1'] = $('#bankAddressAddress1_' + i).val();
                    window.data['accounts[' + i2 + '].BankAddress2'] = $('#bankAddressAddress2_' + i).val();
                    window.data['accounts[' + i2 + '].BankAddress3'] = $('#bankAddressAddress3_' + i).val();
                    window.data['accounts[' + i2 + '].BankCity'] = $('#bankAddressCity' + i).val();
                    window.data['accounts[' + i2 + '].BankState'] = $('#bankAddressState' + i).val();
                    window.data['accounts[' + i2 + '].BankZip'] = $('#bankAddressAddressControl' + i + ' .PostalCode').val();
                    window.data['accounts[' + i2 + '].BankCountry'] = $('#bankAddressCountry' + i).val();
                    window.data['accounts[' + i2 + '].BankCounty'] = $('#bankAddressCounty' + i).val();
                    window.data['accounts[' + i2 + '].AccountType'] = $('#accountTypeAccount' + i).val();
                    window.data['accounts[' + i2 + '].PercentToDeposit'] = $('#percentToDepositAccount' + i).val();
//                    window.data['accounts[' + i2 + '].BankID'] = $('#bankID' + i).val();
                }
            } else {
                if ($(".eftCheckEnabled:checkbox:checked").length > 0) {
                    showMessage('Please make sure the percent to deposit is exactly 100%', true);
                    isValid = false;
                }
            }
        }
    } else {
        window.data = {
            preference: 'ProPay',
            propayAccountNumber: $('#txtPropayAccount').val()
        }
    }
    return isValid;
};
