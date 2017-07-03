ko.bindingHandlers.fadeVisible = {
    init: function (element, valueAccessor) {
    	$(element).toggle(ko.utils.unwrapObservable(valueAccessor()));
    },
    update: function (element, valueAccessor) {
    	ko.utils.unwrapObservable(valueAccessor())
			? $(element).fadeIn('fast')
			: $(element).fadeOut('fast');
    }
};

ko.bindingHandlers.fadeOut = {
    init: function (element, valueAccessor) {
    	$(element).toggle(!ko.utils.unwrapObservable(valueAccessor()));
    },
    update: function (element, valueAccessor) {
    	ko.utils.unwrapObservable(valueAccessor())
			? $(element).fadeOut('slow')
			: $(element).show();
    }
};

ko.bindingHandlers.slideToggle = {
	init: function (element, valueAccessor) {
		$(element).toggle(ko.utils.unwrapObservable(valueAccessor()));
	},
	update: function (element, valueAccessor) {
		ko.utils.unwrapObservable(valueAccessor())
			? $(element).slideDown()
			: $(element).slideUp();
	}
};

ko.bindingHandlers.hiddenValue = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        $(element).bind("change", function (event, data, formatted) {
            value($(this).val());
        });
    },
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor();
        $(element).val(ko.utils.unwrapObservable(value));
    }
};

ko.bindingHandlers.datepicker = {
    init: function (element, valueAccessor, allBindingsAccessor) {
        var options = allBindingsAccessor().datepickerOptions || {},
            value = valueAccessor(),
            $elem = $(element);

        value($elem.val());
        $elem.datepicker(options);

        ko.utils.registerEventHandler(element, "change", function () {
            value($elem.val());
        });

        ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
            $elem.datepicker("destroy");
        });
    },
    update: function (element, valueAccessor) {
        var value = ko.utils.unwrapObservable(valueAccessor()),
            $elem = $(element),
            current = $elem.val();

        if (value !== current) {
            $elem.datepicker("setDate", value);
        }
    }
};

ko.bindingHandlers.showLoading = {
    init: function (element, valueAccessor) {
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).showLoading() : $(element).hideLoading();
    },
    update: function (element, valueAccessor) {
        var value = valueAccessor();
        ko.utils.unwrapObservable(value) ? $(element).showLoading() : $(element).hideLoading();
    }
};

ko.bindingHandlers.enter = {
	init: function (element, valueAccessor, allBindingsAccessor, data) {
		//wrap the handler with a check for the enter key
		var wrappedHandler = function (data, event) {
      		if (event.keyCode === 13) {
      			valueAccessor().call(this, data, event);
      		}
		};
		//call the real event binding for 'keyup' with our wrapped handler
		ko.bindingHandlers.event.init(element, function () { return { keyup: wrappedHandler }; }, allBindingsAccessor, data);
	}
};

ko.bindingHandlers.stopBinding = {
	init: function () {
		return { controlsDescendantBindings: true };
	}
};
ko.virtualElements.allowedBindings.stopBinding = true;
