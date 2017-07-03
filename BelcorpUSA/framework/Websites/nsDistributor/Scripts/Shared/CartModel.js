function CartModel(data, options) {
	var self = this,
		defaults = {
			showDeleteColumn: true,
			showDeleteLink: true,
			showSelectGiftLink: true,
			allowQuantityEdit: true
		},
		settings = $.extend(defaults, options),
		mapping = {
			'OrderItems': {
				create: function (options) { return new OrderItemModel(options.data, settings, self); }
			}
		};

	// observables
	self.isUpdating = ko.observable(false);

	// functions
	self.updateQuantities = function () {
		self.isUpdating(true);
		var options = {
			url: settings.updateUrl,
			data: self.getUpdateQuantitiesData(),
			success: function (response) {
				if (response.result) {
					updateCartDisplay(response);
				}
				else {
					showMessage(response.message, true);
				}
			},
			complete: function () {
				self.isUpdating(false);
			}
		};
		NS.postJSON(options);
	};

	self.getUpdateQuantitiesData = function () {
		var data = self.OrderItems().filter(function (item) { return item.IsQuantityEditable(); }).map(function (item) {
			return {
				ProductID: item.ProductID(),
				Quantity: item.Quantity()
			};
		});
		return ko.toJSON({ products: data });
	};

	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self)
	}

	if (data) {
		self.updateFromJS(data);
	}

	// computed observables
	self.isSubtotalAdjusted = ko.computed(function () {
		return self.AdjustedSubtotal() != self.Subtotal();
	});

	self.isShippingAdjusted = ko.computed(function () {
		return self.AdjustedShippingHandling() != self.ShippingHandling();
	});

	self.anyGiftsAvailable = ko.computed(function () {
		return self.ApplicablePromotions().filter(function (promo) { return promo.StepID() && promo.Available() }).length > 0;
	});

	self.totalItems = ko.computed(function () {
		var total = 0;
		self.OrderItems().each(function (item) {
			total += item.Quantity();
		});
		self.PromotionallyAddedItems().each(function (promo) {
			total += promo.Selections().length;
		});
		return total;
	});

	// observables to wrap settings 
	self.showDeleteColumn = ko.observable(settings.showDeleteColumn);
	self.showSelectGiftLink = ko.observable(settings.showSelectGiftLink);
}

function OrderItemModel(data, options, parent) {
	var self = this,
		settings = options;

	// Observables
	self.isRemoving = ko.observable(false);
	self.isKit = ko.computed(function () {
		return self.IsStaticKit() || self.IsDynamicKit();
	});
	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, {}, self);
	};

	self.updateFromJS(data);

	//functions
	self.removeFromCart = function () {
		parent.isUpdating(true);
		self.isRemoving(true);
		var options = {
			url: settings.removeItemUrl,
			data: self.getRemoveData(),
			success: function (response) {
				if (response.result) {
					updateCartDisplay(response);
				}
				else {
					showMessage(response.message, true);
				}
			},
			complete: function () {
				self.isRemoving(false);
				parent.isUpdating(false);
			}
		};
		NS.post(options);
	};

	self.getRemoveData = function () {
		return { orderItemGuid: self.Guid };
	};

	// observables to wrap settings
	self.showDeleteLink = ko.observable(settings.showDeleteLink);
	self.allowQuantityEdit = ko.observable(settings.allowQuantityEdit);
}

ko.bindingHandlers.currency = {
	update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
		var value = ko.utils.unwrapObservable(valueAccessor());
		var symbol = viewModel.CurrencySymbol();
		element.innerHTML = symbol + parseFloat(value).toFixed(2);
	}
};