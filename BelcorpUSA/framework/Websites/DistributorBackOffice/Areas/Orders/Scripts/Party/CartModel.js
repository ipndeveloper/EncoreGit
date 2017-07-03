function CartModel(options, data) {
	var self = this;
	var defaults = {};
	var settings = $.extend({}, defaults, options);
	var mapping = {
		'CustomerCarts': {
			create: function (options) {
				return new CustomerCartModel(settings, options.data, self);
			}
		}
	};

	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self);
	};

	// Init
	if (data) {
		self.updateFromJS(data);
	}
}

function CustomerCartModel(options, data, parent) {
	var self = this;
	var defaults = {};
	var settings = $.extend({}, defaults, options);
	var mapping = {};

	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self);
	};

	// Init
	if (data) {
		self.updateFromJS(data);
	}
	
	// When a quick-add product is selected, this will contain the product's
	// customization type (if any).
	self.customizationType = ko.observable();
	
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
}
