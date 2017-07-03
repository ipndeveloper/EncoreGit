/// <reference path="jquery-1.7.2.js" />
/// <reference path="jquery.validate.js" />

/*!
** Unobtrusive validation support library for jQuery and jQuery Validate
** Copyright (C) Microsoft Corporation. All rights reserved.
**
** Customized for NetSteps by Jeremy Lundy
*/

/*jslint white: true, browser: true, onevar: true, undef: true, nomen: true, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, newcap: true, immed: true, strict: false */
/*global document: false, jQuery: false */

(function ($) {
    var $jQval = $.validator,
        adapters,
        data_validation = "unobtrusiveValidation";

    function setValidationValues(options, ruleName, value) {
        options.rules[ruleName] = value;
        if (options.message) {
            options.messages[ruleName] = options.message;
        }
    }

    function splitAndTrim(value) {
        return value.replace(/^\s+|\s+$/g, "").split(/\s*,\s*/g);
    }

    function getModelPrefix(fieldName) {
        return fieldName.substr(0, fieldName.lastIndexOf(".") + 1);
    }

    function appendModelPrefix(value, prefix) {
        if (value.indexOf("*.") === 0) {
            value = value.replace("*.", prefix);
        }
        return value;
    }

    function onError(error, inputElement) {  // 'this' is the form element
        var container = $(this).find("[data-valmsg-for='" + inputElement[0].name + "']");
        if (container.length === 0) {
            var prefix = inputElement[0].name.substr(0, inputElement[0].name.lastIndexOf("."));
            container = $(this).find("[data-valmsg-for='" + prefix + "']");
            if (container.length === 0) {
                prefix = prefix.substr(0, prefix.lastIndexOf("."));
                container = $(this).find("[data-valmsg-for='" + prefix + "']");
            }
        }

        var replace = $.parseJSON(container.attr("data-valmsg-replace")) !== false;

        container.removeClass("field-validation-valid").addClass("field-validation-error");
        error.data("unobtrusiveContainer", container);

        if (replace) {
            container.empty();
            error.removeClass("input-validation-error").appendTo(container);
        }
        else {
            error.hide();
        }
    }

    function onErrors(form, validator) {  // 'this' is the form element
        var container = $(this).find("[data-valmsg-summary=true]"),
            list = container.find("ul");

        if (list && list.length && validator.errorList.length) {
            list.empty();
            container.addClass("validation-summary-errors").removeClass("validation-summary-valid");

            $.each(validator.errorList, function () {
                $("<li />").html(this.message).appendTo(list);
            });
        }

        $.watermark.showAll();
    }

    function onSuccess(error) {  // 'this' is the form element
        var container = error.data("unobtrusiveContainer"),
            replace = $.parseJSON(container.attr("data-valmsg-replace"));

        if (container) {
            container.addClass("field-validation-valid").removeClass("field-validation-error");
            error.removeData("unobtrusiveContainer");

            if (replace) {
                container.empty();
            }
        }
    }

    function validationInfo(form) {
        var $form = $(form),
            result = $form.data(data_validation);

        if (!result) {
            result = {
                options: {  // options structure passed to jQuery Validate's validate() method
                    errorClass: "input-validation-error",
                    errorElement: "span",
                    errorPlacement: $.proxy(onError, form),
                    invalidHandler: $.proxy(onErrors, form),
                    messages: {},
                    rules: {},
                    success: $.proxy(onSuccess, form),
                    ignore: ":hidden"  // CUSTOM: Added by Jeremy Lundy to disable validation on hidden elements (see NSValidationAttributes.cs)
                },
                attachValidation: function () {
                    $form.validate(this.options);
                },
                validate: function () {  // a validation function that is called by unobtrusive Ajax
                    $form.validate();
                    return $form.valid();
                }
            };
            $form.data(data_validation, result);
        }

        return result;
    }

    $jQval.unobtrusive = {
        adapters: [],

        parseElement: function (element, skipAttach) {
            /// <summary>
            /// Parses a single HTML element for unobtrusive validation attributes.
            /// </summary>
            /// <param name="element" domElement="true">The HTML element to be parsed.</param>
            /// <param name="skipAttach" type="Boolean">[Optional] true to skip attaching the
            /// validation to the form. If parsing just this single element, you should specify true.
            /// If parsing several elements, you should specify false, and manually attach the validation
            /// to the form when you are finished. The default is false.</param>
            var $element = $(element),
                form = $element.parents("form")[0],
                valInfo, rules, messages;

            if (!form) {  // Cannot do client-side validation without a form
                return;
            }

            valInfo = validationInfo(form);
            valInfo.options.rules[element.name] = rules = {};
            valInfo.options.messages[element.name] = messages = {};

            $.each(this.adapters, function () {
                var prefix = "data-val-" + this.name,
                    message = $element.attr(prefix),
                    paramValues = {};

                if (message !== undefined) {  // Compare against undefined, because an empty message is legal (and falsy)
                    prefix += "-";

                    $.each(this.params, function () {
                        paramValues[this] = $element.attr(prefix + this);
                    });

                    this.adapt({
                        element: element,
                        form: form,
                        message: message,
                        params: paramValues,
                        rules: rules,
                        messages: messages
                    });
                }
            });

            jQuery.extend(rules, { "__dummy__": true });

            if (!skipAttach) {
                valInfo.attachValidation();
            }
        },

        parse: function (selector) {
            /// <summary>
            /// Parses all the HTML elements in the specified selector. It looks for input elements decorated
            /// with the [data-val=true] attribute value and enables validation according to the data-val-*
            /// attribute values.
            /// </summary>
            /// <param name="selector" type="String">Any valid jQuery selector.</param>
            $(selector).find(":input[data-val=true]").each(function () {
                $jQval.unobtrusive.parseElement(this, true);
            });

            $("form").each(function () {
                var info = validationInfo(this);
                if (info) {
                    info.attachValidation();
                }
            });
        }
    };

    adapters = $jQval.unobtrusive.adapters;

    adapters.add = function (adapterName, params, fn) {
        /// <summary>Adds a new adapter to convert unobtrusive HTML into a jQuery Validate validation.</summary>
        /// <param name="adapterName" type="String">The name of the adapter to be added. This matches the name used
        /// in the data-val-nnnn HTML attribute (where nnnn is the adapter name).</param>
        /// <param name="params" type="Array" optional="true">[Optional] An array of parameter names (strings) that will
        /// be extracted from the data-val-nnnn-mmmm HTML attributes (where nnnn is the adapter name, and
        /// mmmm is the parameter name).</param>
        /// <param name="fn" type="Function">The function to call, which adapts the values from the HTML
        /// attributes into jQuery Validate rules and/or messages.</param>
        /// <returns type="jQuery.validator.unobtrusive.adapters" />
        if (!fn) {  // Called with no params, just a function
            fn = params;
            params = [];
        }
        this.push({ name: adapterName, params: params, adapt: fn });
        return this;
    };

    adapters.addBool = function (adapterName, ruleName) {
        /// <summary>Adds a new adapter to convert unobtrusive HTML into a jQuery Validate validation, where
        /// the jQuery Validate validation rule has no parameter values.</summary>
        /// <param name="adapterName" type="String">The name of the adapter to be added. This matches the name used
        /// in the data-val-nnnn HTML attribute (where nnnn is the adapter name).</param>
        /// <param name="ruleName" type="String" optional="true">[Optional] The name of the jQuery Validate rule. If not provided, the value
        /// of adapterName will be used instead.</param>
        /// <returns type="jQuery.validator.unobtrusive.adapters" />
        return this.add(adapterName, function (options) {
            setValidationValues(options, ruleName || adapterName, true);
        });
    };

    adapters.addMinMax = function (adapterName, minRuleName, maxRuleName, minMaxRuleName, minAttribute, maxAttribute) {
        /// <summary>Adds a new adapter to convert unobtrusive HTML into a jQuery Validate validation, where
        /// the jQuery Validate validation has three potential rules (one for min-only, one for max-only, and
        /// one for min-and-max). The HTML parameters are expected to be named -min and -max.</summary>
        /// <param name="adapterName" type="String">The name of the adapter to be added. This matches the name used
        /// in the data-val-nnnn HTML attribute (where nnnn is the adapter name).</param>
        /// <param name="minRuleName" type="String">The name of the jQuery Validate rule to be used when you only
        /// have a minimum value.</param>
        /// <param name="maxRuleName" type="String">The name of the jQuery Validate rule to be used when you only
        /// have a maximum value.</param>
        /// <param name="minMaxRuleName" type="String">The name of the jQuery Validate rule to be used when you
        /// have both a minimum and maximum value.</param>
        /// <param name="minAttribute" type="String" optional="true">[Optional] The name of the HTML attribute that
        /// contains the minimum value. The default is "min".</param>
        /// <param name="maxAttribute" type="String" optional="true">[Optional] The name of the HTML attribute that
        /// contains the maximum value. The default is "max".</param>
        /// <returns type="jQuery.validator.unobtrusive.adapters" />
        return this.add(adapterName, [minAttribute || "min", maxAttribute || "max"], function (options) {
            var min = options.params.min,
                max = options.params.max;

            if (min && max) {
                setValidationValues(options, minMaxRuleName, [min, max]);
            }
            else if (min) {
                setValidationValues(options, minRuleName, min);
            }
            else if (max) {
                setValidationValues(options, maxRuleName, max);
            }
        });
    };

    adapters.addSingleVal = function (adapterName, attribute, ruleName) {
        /// <summary>Adds a new adapter to convert unobtrusive HTML into a jQuery Validate validation, where
        /// the jQuery Validate validation rule has a single value.</summary>
        /// <param name="adapterName" type="String">The name of the adapter to be added. This matches the name used
        /// in the data-val-nnnn HTML attribute(where nnnn is the adapter name).</param>
        /// <param name="attribute" type="String">[Optional] The name of the HTML attribute that contains the value.
        /// The default is "val".</param>
        /// <param name="ruleName" type="String" optional="true">[Optional] The name of the jQuery Validate rule. If not provided, the value
        /// of adapterName will be used instead.</param>
        /// <returns type="jQuery.validator.unobtrusive.adapters" />
        return this.add(adapterName, [attribute || "val"], function (options) {
            setValidationValues(options, ruleName || adapterName, options.params[attribute]);
        });
    };

    $jQval.addMethod("__dummy__", function (value, element, params) {
        return true;
    });

    $jQval.addMethod("regex", function (value, element, params) {
        var match;
        if (this.optional(element)) {
            return true;
        }

        match = new RegExp(params).exec(value);
        return (match && (match.index === 0) && (match[0].length === value.length));
    });

    adapters.addSingleVal("accept", "exts").addSingleVal("regex", "pattern");
    adapters.addBool("creditcard").addBool("date").addBool("digits").addBool("email").addBool("number").addBool("url");
    adapters.addMinMax("length", "minlength", "maxlength", "rangelength").addMinMax("range", "min", "max", "range");
    adapters.add("equalto", ["other"], function (options) {
        var prefix = getModelPrefix(options.element.name),
            other = options.params.other,
            fullOtherName = appendModelPrefix(other, prefix),
            element = $(options.form).find(":input[name='" + fullOtherName + "']")[0];

        setValidationValues(options, "equalTo", element);
    });
    adapters.add("required", function (options) {
        // jQuery Validate equates "required" with "mandatory" for checkbox elements
        if (options.element.tagName.toUpperCase() !== "INPUT" || options.element.type.toUpperCase() !== "CHECKBOX") {
            setValidationValues(options, "required", true);
        }
    });
    adapters.add("remote", ["url", "type", "additionalfields"], function (options) {
        var value = {
            url: options.params.url,
            type: options.params.type || "GET",
            data: {}
        },
            prefix = getModelPrefix(options.element.name);

        $.each(splitAndTrim(options.params.additionalfields || options.element.name), function (i, fieldName) {
            var paramName = appendModelPrefix(fieldName, prefix);
            value.data[paramName] = function () {
                return $(options.form).find(":input[name='" + paramName + "']").val();
            };
        });

        setValidationValues(options, "remote", value);
    });

    // NetSteps custom methods
    $jQval.addMethod("requireany", function (value, element, params) {
        var fields = $(element.form).find(params);
        var thisArg = this;

        var validCount = fields.filter(function () {
            return $jQval.methods.required.call(thisArg, $(this).val(), this);
        }).length;

        var valid = validCount > 0;

        fields.toggleClass("input-validation-error", !valid);

        return valid;
    });

    $jQval.addMethod('datemodel', function (value, element, params) {
        var yearField = $(element).parent().children('.datemodelyear');
        var monthField = $(element).parent().children('.datemodelmonth');
        var dayField = $(element).parent().children('.datemodelday');
        var fields = yearField.add(monthField).add(dayField);
        var year = yearField.val();
        var month = monthField.val();
        var day = dayField.val();
        var valid;

        if (!year && !month && !day) {
            valid = true;
        } else if (year != parseInt(year, 10)
            || month != parseInt(month, 10)
            || day != parseInt(day, 10)
            || !/^\d{4}$/.test(year)) {
            valid = false;
        } else {
            month--;
            var date = new Date(year, month, day);
            if (year != date.getFullYear()
            || month != date.getMonth()
            || day != date.getDate()) {
                valid = false;
            } else {
                valid = true;
            }
        }

        fields.toggleClass("input-validation-error", !valid);

        return valid;
    });

    $jQval.addMethod('ssnmodel', function (value, element, params) {
        var ssn1Field = $(element).parent().children('.ssn1');
        var ssn2Field = $(element).parent().children('.ssn2');
        var ssn3Field = $(element).parent().children('.ssn3');
        var fields = ssn1Field.add(ssn2Field).add(ssn3Field);
        var ssn1 = ssn1Field.val();
        var ssn2 = ssn2Field.val();
        var ssn3 = ssn3Field.val();
        var valid;

        if (!ssn1 && !ssn2 && !ssn3) {
            valid = true;
        } else if (/^\d{3}$/.test(ssn1)
            && /^\d{2}$/.test(ssn2)
            && /^\d{4}$/.test(ssn3)) {
            valid = true;
        } else {
            valid = false;
        }

        fields.toggleClass("input-validation-error", !valid);

        return valid;
    });

    // NetSteps custom adapters
    adapters
        .addBool("requirechecked", "required")
        .addBool("datemodel")
        .addBool("ssnmodel");
    adapters.add("requireany", ["group", "elements"], function (options) {
        var prefix = getModelPrefix(options.element.name);
        var selectors = [];

        if (options.params.group) {
            selectors.push(":input[data-val-requireany-group='" + options.params.group + "']");
        } else if(options.params.elements) {
            $.each(splitAndTrim(options.params.elements), function (i, fieldName) {
                var elementName = appendModelPrefix(fieldName, prefix);
                var selector = ":input[name='" + elementName + "']";
                selectors.push(selector);
            });
        }

        setValidationValues(options, "requireany", selectors.join(","));
    });

    $(function () {
        $jQval.unobtrusive.parse(document);
    });
} (jQuery));