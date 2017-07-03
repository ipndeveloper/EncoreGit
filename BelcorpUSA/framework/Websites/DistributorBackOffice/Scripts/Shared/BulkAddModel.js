function BulkAddModel(data, options) {
	var self = this,
		defaults = {},
		settings = $.extend(options, defaults);

	self.changingCategories = ko.observable(false);
	self.addingProducts = ko.observable(false);
	self.selectedCategory = ko.observable();

	self.handleAddResponse = function () { }

	self.selectedCategory.subscribe(function (newCategory) {
		self.updateProducts(newCategory.CategoryID());
	});

	self.updateProducts = function (categoryId) {
		var options = {
			url: settings.GetProductsUrl,
			data: { categoryId: categoryId },
			success: function (response) {
				ko.mapping.fromJS(response.BulkAddModelData, { 'ignore': ["Categories"] }, self);
			},
			complete: function () {
				self.changingCategories(false);
			}
		};
		self.changingCategories(true);
		NS.post(options);
	}

	self.addProducts = function () {
		var data = self.getAddData();
		if (data.products.length) {
			var options = {
				url: settings.AddProductsUrl,
				data: ko.toJSON(data),
				success: function (response) {
					self.handleAddResponse(response);
				},
				complete: function () {
					self.addingProducts(false);
				}
			};
			self.addingProducts(true);
			NS.postJSON(options);
		}
	}

	self.getAddData = function () {
		var productData = self.Products().filter(function (prod) { return prod.Quantity() > 0; });
		return { products:
			ko.utils.arrayMap(productData, function (prod) {
				return { ProductID: prod.ProductID(), Quantity: prod.Quantity() };
			})
		};
	}

	self.resetQuantities = function () {
		self.Products().each(function (p) { p.Quantity(0); });
	}

	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, {}, self)
	}

	if (data) {
		self.updateFromJS(data);
	}

	return self;
}