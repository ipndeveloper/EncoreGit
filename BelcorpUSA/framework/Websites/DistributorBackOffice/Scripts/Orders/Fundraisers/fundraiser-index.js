$(function () {
    $("#FirstName", "#LastName", "#Email").change(clearOutAccountId);

    var suggestUrl = $("#FundraiserSetupForm").data("jsonsuggest-url");
    var getAccountUrl = $("#FundraiserSetupForm").data("get-account");

    $("#FirstName").jsonSuggest(suggestUrl, { onSelect: function (item) {
        $('#AccountId').val(item.id);

        $.get(getAccountUrl, { accountId: item.id }, function (response) {
            if (response.result) {
                $('#FirstName').val(response.firstName);
                $('#LastName').val(response.lastName);
                if (!$('#Party_Name').val()) {
                    $('#Party_Name').val(response.firstName + ' ' + response.lastName);
                }

                setPhoneNumber(response.phone);
                $('#Email').val(response.email);

                $('#HostAddress_Country').val(response.country);
                if (!$('#HostAddress_Country').find('option:selected').length) {
                    $('#HostAddress_Country option:first').attr('selected', 'selected');
                }
                $('#HostAddress_Address1').val(response.address1);
                $('#HostAddress_Address2').val(response.address2);
                $('#HostAddress_Address3').val(response.address3);

                window['hostAddressPostalCodeLookupComplete'] = function () {
                    $('#HostAddress_City').val(response.city);
                    $('#HostAddress_County').val(response.county);
                    $('#HostAddress_State').val(response.state);
                    window['hostAddressPostalCodeLookupComplete'] = undefined;
                };
                var postalCodeControls = $('#HostAddress_PostalCode, #HostAddress_PostalCode1').closest('.FInput').find('.postalcodelookup-postalcode');
                if (postalCodeControls.length == 1) {
                    postalCodeControls.val(response.postalCode).triggerHandler('keyup');
                }
                else {
                    var zip = (response.postalCode) ? response.postalCode : '';
                    var position = 0;
                    postalCodeControls.each(function () {
                        var length = $(this).attr('maxlength');
                        $(this).val(zip.substr(position, length));
                        position += length;
                    }).filter(':last').triggerHandler('keyup');
                }





                $('#HostAddress_City').val(response.city);
                $('#HostAddress_County').val(response.county);
                $('#HostAddress_State').val(response.state);
            } else {
                showMessage(response.message, true);
            }
        });
    }, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: $('#FirstName').outerWidth(true) + $('#LastName').outerWidth()
    });


    $('#StartDate').datepicker({ changeMonth: true, changeYear: true, minDate: monthAgo(), yearRange: '-5:+5' });
    $('#StartTime').timepickr({ convention: 12 });

    cleanseDateAndTime();

    $('input[name="ShipTo"]').click(function () {
        var target = $("#PartyShip" + $(this).val());

        if ($(this).val() === "Other") {
            setShippingAddressId("");
            $("#ShippingAddress_IsVisible").val(true);
        } else {
            setShippingAddressIdToSelectedDefaultProfile();
            $("#ShippingAddress_IsVisible").val(false);
        }

        if (target.is(':hidden')) {
            $('#PartyShipContainer .PartyShip').slideUp();
            target.slideDown();
        }
    });


    showConslutantShipToDiv();
    setShippingAddressIdToSelectedDefaultProfile();
    $('.address').click(function () {
        $('.address').removeClass("defaultProfile");
        var addressId = $(this).data('address-id');
        setShippingAddressId(addressId);
        $("#addressPreview").html($("span[data-address-id=" + addressId + "]").html());
        $(this).addClass("defaultProfile");
    });



});

function setShippingAddressIdToSelectedDefaultProfile() {
    var shippingAddressId = $(".defaultProfile").data('address-id');
    setShippingAddressId(shippingAddressId);
}

function setShippingAddressId(shippingAddressId) {
    $("#ShippingAddressId").val(shippingAddressId);
}

function setPhoneNumber(phone) {
    var areaCode = phone.substring(0, 3);
    var firstThree = phone.substring(3, 6);
    var lastFour = phone.substring(6);

    $('#txtAreaCode').val(areaCode);
    $('#txtFirstThree').val(firstThree);
    $('#txtLastFour').val(lastFour);
}

function showConslutantShipToDiv() {
    var divToShow = $('input[name="ShipTo"]:checked').val();
    $("#PartyShip" + divToShow).show();
    $("#ShippingAddress_IsVisible").val(false);
}

function monthAgo() {
    return new Date().getDate() - 30;
}

function clearOutAccountId() {
    setAccountId('');
}

function setAccountId(value) {
    $("#AccountId").val(value);
}

function cleanseDateAndTime() {
    $("#StartDate").val($("#StartDate").val().split(" ")[0]);
    $("#StartTime").val(formatAMPM(new Date()));
}

function formatAMPM(date) {
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var ampm = hours >= 12 ? 'pm' : 'am';
    hours = hours % 12;
    hours = hours ? hours : 12;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    var currentTime = hours + ':' + minutes + ' ' + ampm;
    return currentTime;
}