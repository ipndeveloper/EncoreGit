/*
    File: jquery.inputfilter.js
    Version: 1.0
    Author: Jeremy Lundy
    Description: Filters characters typed or pasted into a textbox.
    Options:
        allowedpattern: A regex pattern string used to test for allowed characters.
        live: (Optional, default = false) Boolean indicating whether to bind using jQuery live() to maintain the binding now and in the future.
    Usage Examples:
        $('#MyNumericTextBox').inputfilter({ allowedpattern: "[0-9]" });
        $('#MyAlphaNumericTextBox').inputfilter({ allowedpattern: "\\w", live: true });
        $('.txtUnicodeAlphaNum').inputfilter({ allowedpattern: "[a-zA-Z0-9\xC0-\xFF]" });
*/
(function ($) {
    $.fn.inputfilter = function (options) {
        var settings = {
            allowedpattern: ".*",
            live: false
        }
        if (options) $.extend(settings, options);

        // Filter on keypress
        function inputfilter_keypress(e) {
            if (settings.allowedpattern) {
                var key = e.which;

                // 0 = special, 8 = backspace, 13 = enter
                if (!key || key == 8 || key == 13) return true;

                try {
                    if (!String.fromCharCode(key).match(settings.allowedpattern)) return false;
                } catch (e) {
                    return true;
                }
            }
        }

        // Filter on change (prevents pasting blocked chars from clipboard)
        function inputfilter_change() {
            if (settings.allowedpattern) {
                try {
                    var re = new RegExp(settings.allowedpattern, "g"); // "g" means global (find all occurrences)
                } catch (e) {
                    return;
                }
                if (re) {
                    var allowedmatches = $(this).val().match(re);
                    $(this).val(allowedmatches ? allowedmatches.join('') : '');
                }
            }
        }

        if (settings.live) {
            $(this)
                .live('keypress.inputfilter', inputfilter_keypress)
                .live('change.inputfilter', inputfilter_change);
        } else {
            $(this)
                .unbind('keypress.inputfilter').bind('keypress.inputfilter', inputfilter_keypress)
                .unbind('change.inputfilter').bind('change.inputfilter', inputfilter_change);
        }

        return this;
    };
})(jQuery);