/// <reference path="NS.js" />

// netsteps.unobtrusive.js - Global wireups

$(function () {
    // Basic confirm dialog (eg. data-click-confirm="Are you sure you want to do this?")
    $('[data-click-confirm]').each(function (num, e) {
        $(e).click(function () {
            return confirm($(e).attr('data-click-confirm'));
        });
    });

    // Triggers a click event on another element (eg. data-click-click="#btnShowLogin")
    $('[data-click-click]').each(function (num, e) {
        $(e).click(function () {
            var targetId = $(e).attr('data-click-click');
            if ((targetId !== undefined) && (targetId.length !== 0)) {
                $(targetId).click();
            }
            return false;
        });
    });

    // Form submit button (eg. data-click-submit="parent" or data-click-submit="#MyForm")
    $('[data-click-submit]').each(function (num, e) {
        $(e).click(function () {
            var formId = $(e).attr('data-click-submit');
            if ((formId !== undefined) && (formId.length !== 0)) {
                if (formId.toLowerCase() == 'parent') {
                    $(e).closest('form').submit();
                } else {
                    $(formId).submit();
                }
            }
            return false;
        });
    });

    // Call function on form submit (eg. data-submit-function="mySubmitFunction")
    $('[data-submit-function]').each(function (num, e) {
        var functionName = $(e).attr('data-submit-function');
        if ((functionName !== undefined)
            && (functionName !== null)
            && (functionName.length !== 0)) {
            // Check for jQuery.validate
            if (typeof $(e).validate === 'function') {
                // Wire up to jQuery.validate
                $.extend($(e).validate().settings, {
                    submitHandler: function (form) {
                        NS.executeFunctionByName(functionName, window, e);
                    }
                });
            } else {
                // Else wire up to form submit
                $(e).submit(function () {
                    NS.executeFunctionByName(functionName, window, e);
                    return false;
                });
            }
        }
    });

    // Show loading image on form submit (eg. data-submit-showloading="#btnSubmit")
    // showLoadingSelector must be inside the form
    $('[data-submit-showloading]').each(function (num, e) {
        var showLoadingSelector = $(e).attr('data-submit-showloading');
        if ((typeof showLoadingSelector !== 'undefined')
            && (showLoadingSelector !== null)
            && (showLoadingSelector.length !== 0)) {
            // Check for jQuery.validate
            if (typeof $(e).validate === 'function') {
                // Wire up to jQuery.validate
                $.extend($(e).validate().settings, {
                    submitHandler: function (form) {
                        showLoading($(showLoadingSelector, e));
                        form.submit();
                    }
                });
            } else {
                // Else wire up to form submit
                $(e).submit(function () {
                    showLoading($(showLoadingSelector, e));
                });
            }
        }
    });

    // Autotab (eg. data-autotab="next" or data-autotab="#NextInput")
    $('[data-autotab]').each(function (num, e) {
        $(e).unbind('keyup.autotab').bind('keyup.autotab', function (e) {
            var ignoreKeys = [8, 9, 16, 17, 18, 19, 20, 27, 33, 34, 35, 36, 37, 38, 39, 40, 45, 46, 91, 92, 144, 145];
            var val = $(this).val();
            if (!ignoreKeys.contains(e.which) && val.length == $(this).attr('maxlength')) {
                var targetSelector = $(this).attr('data-autotab');
                var target;
                if ((targetSelector !== undefined) && (targetSelector.length !== 0)) {
                    if (targetSelector.toLowerCase() == 'next') {
                        var inputs = $(this).closest('form').find(':input');
                        target = inputs.eq(inputs.index(this) + 1);
                    } else {
                        target = $(targetSelector);
                    }
                    target.focus().select();
                }
            }
        });
    });
    // Autotab (eg. data-autotab-prev="prev" or data-autotab="#PreviousInput")
    $('[data-autotab-prev]').each(function (num, e) {
        $(e).unbind('keydown.autotab').bind('keydown.autotab', function (e) {
            var val = $(this).val();
            if (e.which == 8 && val.length == 0) {
                var targetSelector = $(this).attr('data-autotab-prev');
                var target;
                if ((targetSelector !== undefined) && (targetSelector.length !== 0)) {
                    if (targetSelector.toLowerCase() == 'prev') {
                        var inputs = $(this).closest('form').find(':input');
                        target = inputs.eq(inputs.index(this) - 1);
                    } else {
                        target = $(targetSelector);
                    }
                    target.focus().val(target.val());
                }
            }
        });
    });

    // Input filter (eg. data-inputfilter="\d")
    $('[data-inputfilter]').each(function (num, e) {
        $(e).inputfilter({ allowedpattern: $(e).attr('data-inputfilter') });
    });

    // Address form postal code lookup
    $('.postalcodelookup-container').each(function (num, e) {
        var container = $(e);
        NS.initPostalCodeLookup({
            container: container,
            url: container.attr('data-postalcodelookup-url'),
            regex: container.attr('data-postalcodelookup-regex'),
            countryId: container.attr('data-postalcodelookup-countryid'),
            city: container.attr('data-postalcodelookup-city'),
            county: container.attr('data-postalcodelookup-county'),
            stateId: container.attr('data-postalcodelookup-stateid'),
            street: container.attr('data-postalcodelookup-street'),
            dropDownText: container.attr('data-postalcodelookup-dropdowntext')
        });
    });
});
