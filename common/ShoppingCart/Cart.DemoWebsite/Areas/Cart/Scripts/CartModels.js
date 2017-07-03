/*global $, ko, alert, nsAjax, window */

function ItemModel(options, data, parent) {
	"use strict";

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
		nsAjax.postJSON({
			url: settings.RemoveUrl,
			data: ko.toJSON(data),
			success: function (results) {
				self.handleRemoveResponse(results);
			},
			error: function (results) {
				self.handleRemoveResponse(results);
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
		parent.handleResponse(results, results.errorMessage || settings.RemoveBaseErrorMessage);
	};

	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self);
	};

	// Init
	if (data) {
		self.updateFromJS(data);
	}
}

function CartModel(cartModel) {//serverOptions, data) {
	"use strict";

	var self = this;
	var defaults = {};
	var settings = $.extend({}, defaults, cartModel.ServerOptions);
	var mapping = {
		'Items': {
			create: function (data) {
				return new ItemModel(settings, data.data, self);
			}
		}
	};

	// Observables
	self.isUpdating = ko.observable(false);

	// Event Handlers
	self.btnUpdateCart_click = function () {
		self.updateQuantities();
	};

	// Functions
	self.updateQuantities = function () {
		self.isUpdating(true);
		var data = self.getUpdateQuantitiesData();
		nsAjax.postJSON({
			url: settings.UpdateQuantitiesUrl,
			data: ko.toJSON(data),
			success: function (results) {
				self.handleUpdateQuantitiesResponse(results);
			},
			error: function (results) {
				self.handleUpdateQuantitiesResponse(results);
			},
			complete: function () {
				self.isUpdating(false);
			}
		});
	};

	self.getUpdateQuantitiesData = function () {
		var items = ko.utils.arrayFilter(self.ItemModels(), function (item) {
			return item.IsQuantityEditable();
		});
		return {
			products: ko.utils.arrayMap(items, function (item) {
				return {
					ProductID: item.ProductID(),
					Quantity: item.Quantity()
				};
			})
		};
	};

	self.handleUpdateQuantitiesResponse = function (results) {
		self.handleResponse(results, results.errorMessage || settings.UpdateQuantitiesErrorMessage);
	};

	self.handleResponse = function (results, errorMessage) {
		if (!results) {
			alert(errorMessage);
			return;
		}

		if (results.errorMessage) {
			alert(results.errorMessage);
		}
	};

	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self);
	};

	// Init
	if (cartModel) {
		self.updateFromJS(cartModel);
	}

	self.isSubtotalModified = ko.computed(function () { return self.Adjustments().length > 0; });
}
