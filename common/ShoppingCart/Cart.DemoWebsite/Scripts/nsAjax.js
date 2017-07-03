/*global $ */
// nsAjax.js - NetSteps global namespace

(function (window) {
	"use strict";

	if (window.nsAjax) {
		throw new Error("nsAjax conflict");
	}

	window.nsAjax = function () {
		// Use "this.fn" for public functions
		// Use "var fn" for private functions

		var ajax = function (options) {
			var defaults = {
				url: undefined,
				type: 'GET',
				data: {},
				target: undefined,
				loading: undefined,
				complete: undefined,
				success: function (data, textStatus, jqXHR) { },
				error: function (jqXHR, textStatus, errorThrown) {
					throw new Error(textStatus);
				}
			};
			var settings = $.extend({}, defaults, options);

			if (settings.url === undefined) {
				throw new Error("Url is required");
			}

			if (settings.loading) {
				settings.loading.call(settings.target);
			}

			var ajaxOptions = $.extend({}, settings, {
				success: function (data, textStatus, jqXHR) {
					settings.success.call(settings.target, data, textStatus, jqXHR);
					if (settings.complete) {
						settings.complete.call(settings.target);
					}
				},
				error: function (jqXHR, textStatus, errorThrown) {
					settings.error.call(settings.target, jqXHR, textStatus, errorThrown);
					if (settings.complete) {
						settings.complete.call(settings.target);
					}
				}
			});

			$.ajax(ajaxOptions);
		};

		this.get = function (options) {
			ajax($.extend({}, options, { type: 'GET' }));
		};

		this.getJSON = function (options) {
			ajax($.extend({}, options, { type: 'GET', contentType: 'application/json' }));
		};

		this.post = function (options) {
			ajax($.extend({}, options, { type: 'POST' }));
		};

		this.postJSON = function (options) {
			ajax($.extend({}, options, { type: 'POST', contentType: 'application/json' }));
		};
	};
}(window));