function OrderEntryModel(options, data) {
  
	var self = this;
	var defaults = {};
	var settings = $.extend({}, defaults, options);
	var mapping = {
		'OrderItemModels': {
			create: function (options) {
				return new OrderItemModel(settings, options.data, self);
			}
		}
	};

	// true when the cart items are updating.
	self.isUpdating = ko.observable(false);

	// When a quick-add product is selected, this will contain the product's
	// customization type (if any).
	self.customizationType = ko.observable();

	// Event Handlers
	self.btnUpdateCart_click = function () {
		self.updateQuantities();
	};

	// Functions
	self.updateQuantities = function () {
	    self.isUpdating(true);
	    var data = self.getUpdateQuantitiesData();
	    NS.postJSON({
	        url: settings.UpdateQuantitiesUrl,
            async: false,
	        data: ko.toJSON(data),
	        success: function (results) {

	            self.handleUpdateQuantitiesResponse(results);
	            updPayments(results);
	            BalanceCredit(results);	           
	        },
	        error: function () {
	            self.handleUpdateQuantitiesResponse();
	        },
	        complete: function () {
	            self.isUpdating(false);
	        }
	    });
	};

	self.getUpdateQuantitiesData = function () {
		var orderItemModels = ko.utils.arrayFilter(self.OrderItemModels(), function (orderItemModel) {
			return orderItemModel.IsQuantityEditable();
		});
		return {
			products: ko.utils.arrayMap(orderItemModels, function (orderItemModel) {
				return {
					OrderItemGuid: orderItemModel.Guid(),
					ProductID: orderItemModel.ProductID(),
					Quantity: orderItemModel.Quantity()
				};
			})
		};
	};

	self.handleUpdateQuantitiesResponse = function (results) {
		self.handleResponse(results, settings.UpdateQuantitiesErrorMessage);
	};

	self.handleResponse = function (results, baseErrorMessage) {
		if (!results) {
			showMessage(baseErrorMessage, true);
			return;
		}
		if (!results.result) {
			showMessage(baseErrorMessage + ': ' + results.message, true);
			return;
		}
		updateCartAndTotals(results);
		updateBundleOptions(results.BundleOptionsSpanHTML, results.AvailableBundleCount);
		if (results.message) {
			showMessage(results.message, true);
		}
	};

	self.onProductSelected = function (handler) {
		$(self).bind('productSelected', handler);
	};

	self.raiseProductSelected = function (productData) {
		var event = jQuery.Event('productSelected');
		$(self).trigger(event, productData);
		return event;
	};

	self.onCartItemAdding = function (handler) {
		$(self).bind('cartItemAdding', handler);
	};

	self.raiseCartItemAdding = function (addToCartData) {
		var event = jQuery.Event('cartItemAdding');
		$(self).trigger(event, addToCartData);
		return event;
	};

	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self);
	};

	// Init
	if (data) {
		self.updateFromJS(data);
	}

	self.isSubtotalModified = ko.computed(function () { return self.SubtotalAdjusted() != self.Subtotal(); });
	self.isSubtotalModifiedNew = ko.computed(function () { return self.AdjustedTotal_Sum() != self.OriginalTotal_Sum(); }); /*CGI(CMR)-07/04/2015*/
	self.isSubtotalCVModifiedNew = ko.computed(function () { return self.AdjustedCommissionableTotal_Sum() != self.OriginalCommissionableTotal_Sum(); }); /*CGI(CMR)-07/04/2015*/
}

function OrderItemModel(options, data, parent) {
	var self = this;
	var defaults = {};
	var settings = $.extend({}, defaults, options);
	var mapping = {};

	// Observables
	self.isRemoveHovering = ko.observable(false);
	self.isRemoving = ko.observable(false);
	self.isDynamicKitNotFull = ko.computed(function () {
		return !self.IsDynamicKitFull();
	});

	// Event Handlers
	self.btnRemove_mouseover = function () {
		self.isRemoveHovering(true);
	};
	self.btnRemove_mouseout = function () {
		self.isRemoveHovering(false);
	};
	self.btnRemove_click = function () {
		self.remove();
	};

	// Functions
	self.remove = function () {
	    parent.isUpdating(true);
	    self.isRemoving(true);
	    var data = self.getRemoveData();
	    NS.postJSON({
	        url: settings.RemoveUrl,
	        data: ko.toJSON(data),
	        success: function (results) {
	            self.handleRemoveResponse(results);
	            updPayments(results);
	            BalanceCredit(results);	            
	        },
	        error: function () {
	            self.handleRemoveResponse();
	        },
	        complete: function () {
	            self.isRemoving(false);
	            parent.isUpdating(false);
	        }
	    });
	};

	self.getRemoveData = function () {
		return {
			orderItemGuid: self.Guid
		};
	};

	self.handleRemoveResponse = function (results) {
		parent.handleResponse(results, settings.RemoveErrorMessage);
	};

	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self);
	};

	// Init
	if (data) {
		self.updateFromJS(data);
	}
}