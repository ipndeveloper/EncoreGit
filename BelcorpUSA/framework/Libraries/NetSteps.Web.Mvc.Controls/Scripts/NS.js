// NS.js - NetSteps global namespace

var NS = new function () {
    // Use "self.fn" for public functions
    // Use "var fn" for private functions
    var self = this;

    self.get = function (options) {
        return self.ajax($.extend({}, options, { type: 'GET' }));
    };

    self.getJSON = function (options) {
        return self.ajax($.extend({}, options, { type: 'GET', contentType: 'application/json' }));
    };

    self.post = function (options) {
        return self.ajax($.extend({}, options, { type: 'POST' }));
    };

    self.postJSON = function (options) {
        return self.ajax($.extend({}, options, { type: 'POST', contentType: 'application/json' }));
    };

    self.executeFunctionByName = function (functionName, context) {
        var args = Array.prototype.slice.call(arguments, 2);
        var namespaces = functionName.split(".");
        var func = namespaces.pop();
        for (var i = 0; i < namespaces.length; i++) {
            context = context[namespaces[i]];
        }
        return context[func].apply(context, args);
    };

    self.initPostalCodeLookup = function (options) {
        var defaults = {
            container: undefined,
            url: '/Checkout/LookupZip',
            regex: '^\\d{5}',
            countryId: 0,
            city: undefined,
            county: undefined,
            stateId: undefined,
            street: undefined,
            dropDownText: ''    
        };
        var settings = $.extend({}, defaults, options);

        if (settings.container === undefined)
            return;

        var container = $(settings.container);
        var regExp = new RegExp(settings.regex);
        var postalCodeControl = $('.postalcodelookup-postalcode', container);
        var cityControl = $('.postalcodelookup-city', container);
        var countyControl = $('.postalcodelookup-county', container);
        var stateControl = $('.postalcodelookup-state', container);
        var streetControl = $('.postalcodelookup-street', container);
        var loadingElement = $('.postalcodelookup-loading', container);

        var zipXHR;
        var lastZip;

        function clearCityCountyStateControls() {
            //alert("clearCityCountyStateControls");
            //cityControl.add(countyControl).add(stateControl).add(streetControl).html('<option value=\"\">' + settings.dropDownText + '</option>');
            $("#MainAddress_StateProvinceID").val("");
            $("#MainAddress_StateProvinceID").attr('readonly', true);
            $("#MainAddress_StateProvinceID").css('background-color', '#DEDEDE');

            $("#MainAddress_City").val("");
            $("#MainAddress_City").attr('readonly', true);
            $("#MainAddress_City").css('background-color', '#DEDEDE');

            $("#MainAddress_County").val("");
            $("#MainAddress_County").attr('readonly', true);
            $("#MainAddress_County").css('background-color', '#DEDEDE');

            //alert("Limpia MainAddress_Street EditaStreet: ");
            $("#MainAddress_Street").val("");
            $("#MainAddress_Street").attr('readonly', true);
            $("#MainAddress_Street").css('background-color', '#DEDEDE');

            $("#MainAddress_Address1").val("");
            $("#MainAddress_Address2").val("");
            $("#MainAddress_Address3").val("");

            $("#ShippingAddress_StateProvinceID").val("");
            $("#ShippingAddress_StateProvinceID").attr('readonly', true);
            $("#ShippingAddress_StateProvinceID").css('background-color', '#DEDEDE');

            $("#ShippingAddress_City").val("");
            $("#ShippingAddress_City").attr('readonly', true);
            $("#ShippingAddress_City").css('background-color', '#DEDEDE');

            $("#ShippingAddress_County").val("");
            $("#ShippingAddress_County").attr('readonly', true);
            $("#ShippingAddress_County").css('background-color', '#DEDEDE');

            //alert("Limpia ShippingAddress_Street EditaStreet");
            $("#ShippingAddress_Street").val("");
            $("#ShippingAddress_Street").attr('readonly', true);
            $("#ShippingAddress_Street").css('background-color', '#DEDEDE');

            $("#ShippingAddress_Address1").val("");
            $("#ShippingAddress_Address2").val("");
            $("#ShippingAddress_Address3").val("");

        }
        function postalCodeLookup(zip) {
            if (!zipXHR) {
                loadingElement.show();
                cityControl.add(countyControl).add(stateControl).add(streetControl).empty();
                zipXHR = $.getJSON(settings.url, { countryId: settings.countryId, zip: zip }, function (results) {
                    zipXHR = undefined;
                    lastZip = zip;
                    loadingElement.hide();
                    if (!results.length) {
                        if (showMessage && results.message) {
                            showMessage(results.message, true);
                        }
                        clearCityCountyStateControls();
                    } else {
                        for (var i = 0; i < results.length; i++) {

                            //alert("zip: " + zip + " - cityControl: " + cityControl + " - container: " + container + " - regExp: " + regExp);
                            //alert("results" + results[i].city);
                            //alert("results" + results[i].Editacounty);

                            if (!stateControl.contains(results[i].state.trim())) {
                                //stateControl.append('<option' + (settings.stateId && results[i].stateId == settings.stateId ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].stateId + '\">' + results[i].state.trim() + '</option>');
                                $("#MainAddress_StateProvinceID").attr('readonly', true);
                                $("#MainAddress_StateProvinceID").css('background-color', '#DEDEDE');
                                $("#MainAddress_StateProvinceID").val(results[i].state.trim());

                                $("#ShippingAddress_StateProvinceID").val(results[i].state.trim());
                                $("#ShippingAddress_StateProvinceID").attr('readonly', true);
                                $("#ShippingAddress_StateProvinceID").css('background-color', '#DEDEDE');


                            }
                            if (!cityControl.contains(results[i].city.trim())) {
                                //cityControl.append('<option' + (settings.city && results[i].city == settings.city ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].city + '\">' + results[i].city.trim() + '</option>');
                                $("#MainAddress_City").attr('readonly', true);
                                $("#MainAddress_City").css('background-color', '#DEDEDE');
                                $("#MainAddress_City").val(results[i].city.trim());

                                $("#ShippingAddress_City").val(results[i].city.trim());
                                $("#ShippingAddress_City").attr('readonly', true);
                                $("#ShippingAddress_City").css('background-color', '#DEDEDE');


                            }
                            //							if (countyControl.length && !countyControl.contains(results[i].county.trim())) {
                            //							    //countyControl.append('<option' + (settings.county && results[i].county == settings.county ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].county + '\">' + results[i].county.trim() + '</option>');
                            //							}

                            if (!results[i].EditaCounty) {
                                //countyControl.append('<option' + (county && results[i].county == county ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].county + '\">' + results[i].county.trim() + '</option>');
                                $("#MainAddress_County").val(results[i].county.trim());
                                $("#MainAddress_County").attr('readonly', true);
                                $("#MainAddress_County").css('background-color', '#DEDEDE');
                                

                                $("#ShippingAddress_County").val(results[i].county.trim());
                                $("#ShippingAddress_County").attr('readonly', true);
                                $("#ShippingAddress_County").css('background-color', '#DEDEDE');

                            }
                            else {

                                $("#MainAddress_County").val(results[i].county.trim());
                                $("#MainAddress_County").attr('readonly', false);
                                $("#MainAddress_County").css('background-color', '#FFFFFF');
                                

                                $("#ShippingAddress_County").val(results[i].county.trim());
                                $("#ShippingAddress_County").attr('readonly', false);
                                $("#ShippingAddress_County").css('background-color', '#FFFFFF');

                            }
                            //				            if (!streetControl.contains(results[i].street.trim())) {
                            //				                //streetControl.append('<option' + (settings.street && results[i].street == settings.street ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].street + '\">' + results[i].street.trim() + '</option>');
                            //				            }
                            //alert("EditaStreet: " + results[i].EditaCounty);
                            //alert("Carga EditaStreet: " + results[i].EditaStreet);
                            if (!results[i].EditaStreet) {
                                //streetControl.append('<option' + (street && results[i].street == street ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].street + '\">' + results[i].street.trim() + '</option>');
                                $("#MainAddress_Street").attr('readonly', true);
                                $("#MainAddress_Street").css('background-color', '#DEDEDE');
                                $("#MainAddress_Street").val(results[i].street.trim());

                                $("#ShippingAddress_Street").val(results[i].street.trim());
                                $("#ShippingAddress_Street").attr('readonly', true);
                                $("#ShippingAddress_Street").css('background-color', '#DEDEDE');

                            }
                            else {
                                $("#MainAddress_Street").attr('readonly', false);
                                $("#MainAddress_Street").css('background-color', '#FFFFFF');
                                $("#MainAddress_Street").val(results[i].street.trim());

                                $("#ShippingAddress_Street").val(results[i].street.trim());
                                $("#ShippingAddress_Street").attr('readonly', false);
                                $("#ShippingAddress_Street").css('background-color', '#FFFFFF');
                            }
                        }
                        cityControl.add(countyControl).add(stateControl).add(streetControl).clearErrorMS();
                    }
                });
            }
        }
        postalCodeControl.keyup(function () {
            var postalCodeMatches = regExp.exec(postalCodeControl.fullVal());
            if (!postalCodeMatches) {
                lastZip = undefined;
                clearCityCountyStateControls();
                return;
            }
            // Check lastZip to avoid repeat lookups
            if (lastZip === postalCodeMatches[0]) {
                return;
            }
            postalCodeLookup(postalCodeMatches[0]);
        }).keyup();
    };

    self.geocodeAddress = function (address, callback) {
        if (callback === undefined) {
            return;
        }
        if (address === undefined
            || typeof window.google === 'undefined'
            || typeof window.google.maps === 'undefined') {
            callback();
        }
        var geocoder = new window.google.maps.Geocoder();
        geocoder.geocode({ address: address }, function (results, status) {
            var lat, lng;
            if (status == window.google.maps.GeocoderStatus.OK) {
                lat = results[0].geometry.location.lat();
                lng = results[0].geometry.location.lng();
            }
            callback(lat, lng);
        });
    };

    self.ajax = function (options) {
        var defaults = {
            url: undefined,
            type: 'GET',
            data: {},
            showLoading: undefined,
            target: undefined,
            success: function (data, textStatus, jqXHR) { },
            error: function (jqXHR, textStatus, errorThrown) {
                showMessage(undefined, true);
            }
        };
        var settings = $.extend({}, defaults, options);

        if (settings.url === undefined)
            return;

        if (settings.showLoading)
            showLoading($(settings.showLoading));

        var ajaxOptions = $.extend({}, settings, {
            showLoading: undefined,
            target: undefined,
            success: function (data, textStatus, jqXHR) {
                if (settings.showLoading)
                    hideLoading($(settings.showLoading));

                if (settings.target) {
                    if (data.result === undefined) {
                        $(settings.target).fadeOut('fast', function () {
                            $(this).empty().append(data).fadeIn('fast');
                        });
                    } else {
                        showMessage(data.message, !data.result);
                    }
                }

                settings.success(data, textStatus, jqXHR);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                if (settings.showLoading)
                    hideLoading($(settings.showLoading));

                settings.error(jqXHR, textStatus, errorThrown);
            }
        });

        var jqXHR = $.ajax(ajaxOptions);

        return jqXHR;
    };
};

(function ($) {
    $.fn.showLoading = function (css, options) {
        var settings = $.extend({}, $.fn.showLoading.defaults, options);
        if (!settings.imageUrl) {
            return $(this);
        }
        return $(this).each(function () {
            var element = $(this);
            var loading = $('<span class="loadingWrap"><img src="' + settings.imageUrl + '" alt="loading..." class="loadingIcon" /></span>');
            if (css) {
                loading.css(css);
            }
            else if (element.height()) {
                loading.css('height', element.height() + 'px');
            }
            element.after(loading).hide();
        });
    };

    $.fn.hideLoading = function (options) {
        return $(this).each(function () {
            var element = $(this);
            element.show().next('.loadingWrap').remove();
        });
    };
    
    $.fn.showLoading.defaults = {
        imageUrl: '/Resource/Content/Images/loader_36x36.gif'
    };
})(jQuery);
