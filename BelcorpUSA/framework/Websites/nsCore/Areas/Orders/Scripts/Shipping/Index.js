function viewModel() {
    var self = this;
    var options = typeof (viewModelOptions) !== 'undefined' ? viewModelOptions || {} : {};
    var defaults = {
        pagesFormat: '{0} of {1}'
    };
    var settings = $.extend({}, defaults, options);

    self.SearchParams = {
        OrderTypeID: ko.observable(),
        OrderStatusID: ko.observable(),
        OrderShipmentStatusID: ko.observable(),
        OrderNumber: ko.observable(),
        StartDate: ko.observable(),
        EndDate: ko.observable(),
        OrderBy: ko.observable(),
        OrderByDirection: ko.observable(),
        PageIndex: ko.observable(),
        PageSize: ko.observable()
    };
    self.Packages = ko.observableArray();
    self.PageIndex = ko.observable(0);
    self.TotalPages = ko.observable(1);
    self.Errors = ko.observableArray();
    self.Timestamp = ko.observable();

    self.forceRefresh = function () {
        self.Timestamp((new Date()).getTime());
    };

    self.resetFilters = function () {
        self.SearchParams.OrderTypeID('');
        self.SearchParams.OrderStatusID('');
        self.SearchParams.OrderShipmentStatusID('');
        self.SearchParams.OrderNumber('');
        self.SearchParams.StartDate('');
        self.SearchParams.EndDate('');
        self.forceRefresh();
    };

    self.resetOrderBy = function () {
        self.SearchParams.OrderBy('OrderNumber');
        self.SearchParams.OrderByDirection('Ascending');
        self.forceRefresh();
    };

    self.resetPageIndex = function () {
        self.SearchParams.PageIndex(0);
        self.forceRefresh();
    };

    self.resetAll = function () {
        self.resetFilters();
        self.resetOrderBy();
        self.resetPageIndex();
        self.SearchParams.PageSize(20);
    };

    self.ModifiedPackages = ko.computed(function () {
        return ko.utils.arrayFilter(self.Packages(), function (package) {
            return package.IsModified();
        });
    });

    self.showErrors = ko.computed(function () {
        return self.Errors().length > 0;
    });

    self.enableNextPage = ko.computed(function () {
        return self.PageIndex() + 1 < self.TotalPages();
    });

    self.enablePreviousPage = ko.computed(function () {
        return self.PageIndex() > 0;
    });

    self.pagesText = ko.computed(function () {
        return String.format(settings.pagesFormat, self.PageIndex() + 1, self.TotalPages());
    });

    self.setPackages = function (packagesJS) {
        self.Packages.removeAll();
        var tabIndex = 1;
        ko.utils.arrayForEach(packagesJS, function (packageJS) {
            self.Packages.push(new packageModel(packageJS, tabIndex++));
        });
    };

    self.getTemplateName = function (package) {
        return package.OrderShipmentID() ? 'package-template' : 'nopackage-template';
    };

    self.setErrors = function (errorsJS) {
        self.Errors.removeAll();
        ko.utils.arrayForEach(errorsJS, function (errorJS) {
            self.Errors.push(errorJS);
        });
    };

    self.applyFilters = function () {
        self.resetPageIndex();
    };

    self.setOrderBy = function (orderBy) {
        if (self.SearchParams.OrderBy() === orderBy)
            self.toggleOrderByDirection();
        else {
            self.SearchParams.OrderBy(orderBy);
            self.SearchParams.OrderByDirection('Ascending');
        }
        self.resetPageIndex();
    };

    self.toggleOrderByDirection = function () {
        if (self.SearchParams.OrderByDirection() === 'Ascending')
            self.SearchParams.OrderByDirection('Descending');
        else
            self.SearchParams.OrderByDirection('Ascending');
        self.resetPageIndex();
    };

    self.nextPage = function () {
        if (self.enableNextPage()) {
            self.SearchParams.PageIndex(self.SearchParams.PageIndex() + 1);
        }
    };

    self.previousPage = function () {
        if (self.enablePreviousPage()) {
            self.SearchParams.PageIndex(self.SearchParams.PageIndex() - 1);
        }
    };

    self.getUpdatePackageModels = function () {
        return ko.utils.arrayMap(self.ModifiedPackages(), function (package) {
            return {
                OrderID: package.OrderID(),
                OrderShipmentID: package.OrderShipmentID(),
                OrderShipmentPackageID: package.OrderShipmentPackageID(),
                DateShipped: package.DateShipped(),
                TrackingNumber: package.TrackingNumber()
            };
        });
    };

    self.save = function () {
        var data = {
            updatePackageModels: self.getUpdatePackageModels()
        };
        NS.postJSON({
            url: '/Orders/Shipping/UpdatePackages',
            data: ko.toJSON(data),
            showLoading: '#ShowLoading',
            success: function (data) {
                if (data.success === undefined) {
                    ko.utils.arrayForEach(data.Packages, function (packageJS) {
                        var package = ko.utils.arrayFirst(self.Packages(), function (package) {
                            return package.OrderShipmentID() === packageJS.OrderShipmentID;
                        });
                        if (package) {
                            package.OriginalTrackingNumber(packageJS.TrackingNumber);
                            package.OriginalDateShipped(packageJS.DateShipped);
                            package.DateShipped(packageJS.DateShipped);
                            package.TrackingNumber(packageJS.TrackingNumber);
                            package.OrderStatus(packageJS.OrderStatus);
                            package.OrderShipmentStatus(packageJS.OrderShipmentStatus);
                            package.OrderShipmentPackageID(packageJS.OrderShipmentPackageID);
                        }
                    });
                    self.setErrors(data.Errors);
                } else {
                    showMessage(data.message, !data.success);
                }
            }
        });
    };

    self.search = function () {
        NS.post({
            url: '/Orders/Shipping/GetPackages',
            data: ko.toJS(self.SearchParams),
            showLoading: '#ShowLoading',
            success: function (data) {
                if (data.success === undefined) {
                    self.setPackages(data.Packages);
                    self.PageIndex(data.PageIndex);
                    self.TotalPages(data.TotalPages);
                    self.setErrors(data.Errors);
                } else {
                    showMessage(data.message, !data.success);
                }
            }
        });
    };

    self.resetAll();
    ko.computed(function () {
        self.Timestamp();
        self.search();
    }).extend({ throttle: 1 });
}

function packageModel(packageJS, tabIndex) {
    var self = this;
    ko.mapping.fromJS(packageJS, {}, self);
    self.IsSelected = ko.observable(false);
    self.TabIndex = ko.observable(tabIndex);
    self.OriginalTrackingNumber = ko.observable(self.TrackingNumber());
    self.OriginalDateShipped = ko.observable(self.DateShipped());
    self.IsModified = ko.computed(function () {
        return self.OriginalTrackingNumber() != self.TrackingNumber() || self.OriginalDateShipped() != self.DateShipped();
    });
}

$(function () {
    ko.applyBindings(new viewModel());
});
