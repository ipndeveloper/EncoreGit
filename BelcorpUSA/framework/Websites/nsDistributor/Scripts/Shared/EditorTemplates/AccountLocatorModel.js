function locatorModel() {
    var self = this;
    var options = typeof (locatorModelOptions) !== 'undefined' ? locatorModelOptions || {} : {};
    var defaults = {
        searchType: 'AccountInfo',
        maximumDistance: 25,
        beforeDisplayResults: undefined
    };
    var settings = $.extend({}, defaults, options);

    self.searchParams = {
        searchType: ko.observable(settings.searchType),
        accountNumber: ko.observable(),
        firstName: ko.observable(),
        lastName: ko.observable(),
        address1: ko.observable(),
        city: ko.observable(),
        postalCode: ko.observable(),
        maximumDistance: ko.observable(settings.maximumDistance),
        pageIndex: 0,
        Latitude: undefined,
        Longitude: undefined
    };
    self.lastSearchParams = {};
    self.showResults = ko.observable(false);
    self.showMoreButton = ko.observable(false);
    self.results = ko.observableArray();
    self.showError = ko.observable(false);
    self.errorMessage = ko.observable();

    self.showAccountInfoParams = ko.computed(function () { return self.searchParams.searchType() === 'AccountInfo'; });
    self.showLocationParams = ko.computed(function () { return self.searchParams.searchType() === 'Location'; });
    self.showOtherParams = ko.computed(function () { return self.searchParams.searchType() === 'Other'; });

    self.search = function () {
        if (self.showAccountInfoParams()) {

            if ($('#hdnEnviroment').val() == "1") {
                if (($('#Address1').val() != "" || $('#City').val() != "") && $('#PostalCode').val() == "") {
                    $('#PostalCode').attr('required', true);
                    $("#AccountLocatorForm").valid();
                    $('#PostalCode').next(".field-validation-error").show();
                    return;
                }
            }
            else {
                self.searchInner();
                return
            }

            NS.geocodeAddress(
                self.getAddressString(),
                function (lat, lng) {
                    self.searchParams.Latitude = lat;
                    self.searchParams.Longitude = lng;
                    self.searchInner();
                }
            );
        } else {
            self.searchInner();
        }
    };

    self.searchInner = function () {
        NS.post({
            url: $('#AccountLocatorForm').attr('action'),
            data: ko.toJS(self.searchParams),
            showLoading: '#SearchButtonContainer',
            success: function (data) {
                if (data.result === undefined) {
                    self.showError(false);
                    self.lastSearchParams = ko.toJS(self.searchParams);
                    self.displayResults(data);
                } else if (!data.result) {
                    self.showError(true);
                    self.errorMessage(data.message);
                }
            }
        });
    }

    self.displayResults = function (data) {
        if (settings.beforeDisplayResults !== undefined) {
            var eventData = {
                model: self,
                data: data,
                preventDefault: false
            };
            settings.beforeDisplayResults(eventData);
            if (eventData.preventDefault) {
                return;
            }
        }
        self.showResults(true);
        self.showMoreButton(data.showMoreButton);
        self.results.removeAll();
        ko.utils.arrayMap(data.results, function (item) {
            self.results.push(ko.mapping.fromJS(item));
        });
    }

    self.showMore = function () {
        self.lastSearchParams.pageIndex++;
        NS.post({
            url: $('#AccountLocatorForm').attr('action'),
            data: ko.toJS(self.lastSearchParams),
            showLoading: '#ShowMoreButtonContainer',
            success: function (data) {
                if (data.result === undefined) {
                    self.showError(false);
                    self.showMoreButton(data.showMoreButton);
                    ko.utils.arrayMap(data.results, function (item) {
                        self.results.push(ko.mapping.fromJS(item));
                    });
                } else if (!data.result) {
                    self.showError(true);
                    self.errorMessage(data.message);
                    self.lastSearchParams.pageIndex--;
                }
            }
        });
    };

    self.getAddressString = function () {
        var tmp = [];

        self.pushIfDefined(tmp, self.searchParams.address1());
        self.pushIfDefined(tmp, self.searchParams.city());
        self.pushIfDefined(tmp, self.searchParams.postalCode());

        return tmp.join();
    }

    self.pushIfDefined = function (array, item) {
        if (typeof item !== 'undefined' && item !== null) {
            array.push(item);
        }
    }
}

$(function () {

    $("#Address1").keyup(function () {
        if ($(this).val() == "" && $('#City').val() == "") {
            $('#PostalCode').attr('required', false);
            $("#PostalCode").valid();
            $('#PostalCode').next(".field-validation-error").hide();
        }
    });

    $("#City").keyup(function () {
        if ($(this).val() == "" && $('#Address1').val() == "") {
            $('#PostalCode').attr('required', false);
            $("#PostalCode").valid();
            $('#PostalCode').next(".field-validation-error").hide();
        }
    });

    var model = new locatorModel();
    ko.applyBindings(model);

    $.extend($("#AccountLocatorForm").validate().settings, {
        submitHandler: function (form) {
            model.search();
        }
    });
});
