function LoginModel(options, data) {
	var self = this;
	var defaults = {};
	self.settings = $.extend({}, defaults, options);
	var mapping = {
		'SignUp': {
			create: function (options) {
				return new SignUpModel(options.data.Options, options.data.Data, self);
			}
		}
	};
	var lastTabIndex = 0;

	// Init
	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self);
	};
	if (data) {
		self.updateFromJS(data);
	}

	// Functions
	self.nextTabIndex = function () {
		return ++lastTabIndex;
	};
}

function SignUpModel(options, data, parent) {
	var self = this;
	var defaults = {};
	self.settings = $.extend({}, defaults, options);
	var mapping = {};
	
	// Init
	self.updateFromJS = function (data) {
		ko.mapping.fromJS(data, mapping, self);
	};
	if (data) {
		self.updateFromJS(data);
		self.SignUpTypes = ko.utils.arrayMap(self.settings.SignUpTypes, function (signUpType) {
			return new SignUpTypeModel(signUpType, self);
		});
	}

	self.getTemplate = function (signUpTypeModel) {
		return signUpTypeModel.Template;
	};

	self.submit = function() {
		var data = ko.mapping.toJSON(self);
		//alert(data);
		NS.postJSON({
			url: '/SignUp',
			data: ko.mapping.toJSON(self),
			success: function(data) {
				if (data.result) {
					window.location = data.returnUrl;
				} else {
					$('#signUpError').html(data.message).fadeIn();
				}

			}
		});
		return true;
	};
}

function SignUpTypeModel(data, parent) {
	var self = this;
	$.extend(self, data);

	self.HasFocus = ko.observable();

	self.IsVisible = ko.computed(function () {
		if (self.HasFocus()) {
			parent.SelectedAccountTypeID(self.AccountTypeID);
			return true;
		}
		return self.AccountTypeID === parent.SelectedAccountTypeID();
	});

	self.select = function () {
		parent.SelectedAccountTypeID(self.AccountTypeID);
		return true;
	};
}