function DownlineInfoCardModel(options, data) {
	var self = this;
	var defaults = {};
	var settings = $.extend({}, defaults, options);
	var mapping = {};

	// Init
	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self);
	};
	if (data) {
		self.updateFromJS(data);
	}
	
	// Observables
	self.IsVisible = ko.observable(true);
	self.IsUpdating = ko.observable(false);

	self.IsNotUpdating = ko.computed(function () {
		return !self.IsUpdating();
	});

	self.TitleIconCss = ko.computed(function () {
		return self.IsVisible()
			? 'icon-arrowDown'
			: 'icon-arrowUp';
	});
	
	self.ComputedTitleText = ko.computed(function () {
		return self.IsUpdating()
			? settings.UpdatingText
			: self.TitleText();
	});
	
	self.TitleHoverText = ko.computed(function () {
		return self.IsVisible()
			? settings.HideCardText
			: settings.ShowCardText;
	});

	self.IsEmailButtonVisible = ko.computed(function () {
		return settings.IsEmailEnabled && self.Email();
	});

	self.EmailUrl = ko.computed(function () {
		return settings.BaseEmailUrl + encodeURIComponent(self.Email());
	});

	// Functions
	self.getItemTemplate = function (item) {
		return item.Template();
	};

	self.load = function (accountId) {
		if (accountId === self.AccountId()) {
			return;
		}
		self.IsUpdating(true);

		NS.getJSON({
			url: settings.GetDataUrl,
			data: {
				accountId: accountId
			},
			success: function (data) {
				self.updateFromJS(data);
				self.IsVisible(!!self.AccountId());
				// This can be used when we switch to a proper JsonResult.
				//if (data && data.result) {
				//	self.updateFromJS(data.result);
				//}
				//else {
				//	self.handleAjaxError();
				//}
			},
			error: function (jqXHR) {
				self.handleAjaxError(jqXHR);
			},
			complete: function () {
				self.IsUpdating(false);
			}
		});
	};

	self.handleAjaxError = function (jqXHR) {
		var errorMessage;
		if (jqXHR) {
			var response = ko.utils.parseJson(jqXHR.responseText);
			if (response) {
				errorMessage = response.error;
			} 
		}
		showMessage(errorMessage, true);
	};

	// Event Handlers
	self.title_click = function () {
		self.IsVisible(!self.IsVisible());
	};
}
