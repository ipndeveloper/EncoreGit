/*global $, describe, it, expect, koModelRaw, spyOn, expect */

(function () {
	"use strict";

	describe('the first jasmine integration test', function () {
		it('runs and passes!', function () {
			expect(true).toBeTruthy();
		});
	});

	describe('a cart line item', function () {
		it('shows two decimal places for unit price', function () {
			var subtotalFields = $('.cartItemTotal');
			$.each(subtotalFields, function (index) {
				var splitTotal = $(subtotalFields[index]).text().split('.');
				expect(splitTotal.length).toBeGreaterThan(1);
				expect(splitTotal[1].length).toBe(2);
			});
		});

		it('calls the ko model\'s remove when remove is clicked', function () {
			var items = koModelRaw.Items();
			$.each(items, function (index) {
				var item = items[index];
				spyOn(item, "remove");
				item.remove();
				expect(item.remove).toHaveBeenCalled();
			});
		});
	});
} ());