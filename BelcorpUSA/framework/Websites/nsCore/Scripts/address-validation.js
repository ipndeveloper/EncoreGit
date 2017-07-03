//$(function () {
//    var validation = abstractAddressValidation({
//        address1: "1 Infinite Loop",
//        address2: "",
//        address3: "",
//        city: "Cupertino",
//        state: "CA",
//        postalCode: "95014",
//        country: "US"
//    });
//    validation.init();
//    $(document).bind("validAddressFound", function (event, address) {
//        alert(address.country);
//    });
//});



var abstractAddressValidation = function (spec) {
    var that = {};
    
    var jqmOnHide = function (hash) {
        hash.w.fadeOut('500', function () {
            hash.o.remove();
            $(document).trigger('addressValidationCancelled');
        });
    };
    $("#UnverifiedAddress").jqm({
        onHide: jqmOnHide
    });
     
    that.showModal = function () {
        $("#UnverifiedAddress").jqmShow();
    };
    that.hideModal = function () {
        $("#UnverifiedAddress").jqmHide();
    };

    that.init = function () {

        $(document).unbind("validAddressFound");
        $(document).bind('validAddressFound', function (event, address) { });

        $(".unvalidated-address-selectbtn").die("click");
        $(".unvalidated-address-selectbtn").live("click", function () {
            var element = $(this);
            var address = that.getSelectedAddress(element.data("id"));
            $(document).trigger('validAddressFound', [address]);
            that.hideModal();
            $(".unvalidated-address-selectbtn").die("click", that.init);
        });

        $("#UnverifiedAddress .btnContinue").die("click");
        $("#UnverifiedAddress .btnContinue").live("click", function () {
            var address = {
                address1: that.getAddress1(),
                address2: that.getAddress2(),
                address3: that.getAddress3(),
                city: that.getCity(),
                state: that.getState(),
                street: that.getStreet(),
                postalCode: that.getPostalCode(),
                country: that.getCountry(),
                county: that.getCounty()
            };
            $(document).trigger('validAddressFound', [address]);
            that.hideModal();
        });


        that.setAddress1(spec.address1);
        that.setAddress2(spec.address2);
        that.setAddress3(spec.address3);
        that.setCity(spec.city);
        that.setCountry(spec.country);
        that.setCity(spec.city);
        that.setState(spec.state); 
        that.setStreet(spec.street);
        that.setPostalCode(spec.postalCode);

        var addressModel = {
            "address1": that.getAddress1(),
            "address2": that.getAddress2(),
            "address3": that.getAddress3(),
            "city": that.getCity(),
            "state": that.getState(),
            "street": that.getStreet(),
            "postalCode": that.getPostalCode(),
            "country": that.getCountry()
        };

        NS.getJSON({
            url: "/AddressValidation/Validate",
            data: addressModel,
            dataType: "json",
            success: function (data) {
                if (data.success === "OK") {
                    if (data.result.ValidAddresses != null) {
                    	// Magic number: 2 = AddressValidation.Common.AddressInfoResultState.Success
						if (data.result.Status == 2 && data.result.ValidAddresses.length == 1) {

                            var validAddress = data.result.ValidAddresses[0];

                            var address = {
                                address1: validAddress.Address1 || '',
                                address2: validAddress.Address2 || '',
                                address3: validAddress.Address3 || '',
                                city: validAddress.SubDivision || '',
                                state: validAddress.MainDivision || '',
                                street: validAddress.Street || '',
                                postalCode: validAddress.PostalCode || '',
                                country: validAddress.Country || ''
                            };

                            $(document).trigger('validAddressFound', [address]);
                        } else {
                            $(".suggestedAddressList").empty();
                            $.each(data.result.ValidAddresses, function (index, value) {
                                var html = '<div class="m5 pad5 brdrAll suggestedAddressContainer"><div class="FL suggestedAddress" data-id="' + index + '"><span class="address1">' + value.Address1 + '</span><span class="address2">' + (value.Address2 != null ? value.Address2 : '') + '</span><span class="address3">' + (value.Address3 != null ? value.Address3 : '') + '</span><br><span class="cityState"><span class="city">' + value.SubDivision + '</span>, <span class="state">' + value.MainDivision + '</span> <span class="postalCode">' + value.PostalCode + '</span> <span style="display:none" class="country">' + value.Country + '</span></span><br></div><a class="FR UI-icon-container unvalidated-address-selectbtn" data-id="' + index + '"><span class="icon-label">Select</span> <span class="FR UI-icon icon-arrowNext"></span></a><span class="clr"></span></div>';
                                $(".suggestedAddressList").append(html);
                            });

                            that.showModal();
                        }
                    } else {
                        that.showModal();
                    }
                } else {
                    AddErrorMessage();
				}
            }
        });


    };

    that.getSelectedAddress = function (id) {
        var suggestedAddressSelector = ".suggestedAddress[data-id=" + id + "]";

        return {
            address1: $(suggestedAddressSelector + " .address1").first().html(),
            address2: $(suggestedAddressSelector + " .address2").first().html(),
            address3: $(suggestedAddressSelector + " .address3").html(),
            city: $(suggestedAddressSelector + " .city").first().html(),
            state: $(suggestedAddressSelector + " .state").first().html(),
            street: $(suggestedAddressSelector + " .street").first().html(),
            postalCode: $(suggestedAddressSelector + " .postalCode").first().html(),
            country: $(suggestedAddressSelector + " .country").first().html()
        };
    };

    // Properties
    that.getAddress1 = function () {
        return $("#unverified-address1").val();
    };

    that.setAddress1 = function (value) {
        $("#unverified-address1").val(value);
    };

    that.getAddress2 = function () {
        return $("#unverified-address2").val();
    };

    that.setAddress2 = function (value) {
        $("#unverified-address2").val(value);
    };

    that.getAddress3 = function () {
        return $("#unverified-address3").val();
    };

    that.setAddress3 = function (value) {
        $("#unverified-address3").val(value);
    };

    that.getCountry = function () {
        return $("#unverified-country").val();
    };

    that.setCountry = function (value) {
        $("#unverified-country").val(value);
    };

    that.getPostalCode = function () {
        return $("#unverified-postalcode").val();
    };

    that.setPostalCode = function (value) {
        $("#unverified-postalcode").val(value);
    };

    that.getCity = function () {
        return $("#unverified-city").val();
    };

    that.setCity = function (value) {
        $("#unverified-city").val(value);
    };

    that.getCounty = function () {
        return $("#unverified-county").val();
    };

    that.setCounty = function (value) {
        $("#unverified-county").val(value);
    };

    that.getState = function () {
        return $("#unverified-state").val();
    };

    that.setState = function (value) {
        $("#unverified-state").val(value);
    };

    that.getStreet = function () {
        return $("#unverified-street").val();
    };

    that.setStreet = function (value) {
        $("#unverified-street").val(value);
    };
    
    return that;
}
