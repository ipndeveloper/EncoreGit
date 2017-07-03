/*global $, describe, it, expect, koModelRaw, spyOn, expect */

(function () {
	"use strict";

	describe('the page', function () {

		var expectedjQueryVersion = '1.9.1';
		it('should reference jquery ' + expectedjQueryVersion, function () {
			expect(jQuery.fn.jquery).toBe(expectedjQueryVersion);
		});

	});
} ());