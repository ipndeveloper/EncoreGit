///	<reference path="jquery-1.3.2.min-vsdoc.js" />
/*
NetSteps js Utilities library

Created by: Daniel Stafford
*/

(function (w, d, $, u) {
    var ap = Array.prototype, sp = String.prototype, jp,
    /* General functions */
	importJS = w.importJS = function (url, callback, checkIfFileExists) {
	    ///	<summary>Check if a javascript file is already included and exists and then include it if necessary</summary>
	    ///	<returns type="void" />
	    ///	<param name="url" type="String">The url to the file.</param>
	    ///	<param name="callback" type="Function">Function to call on success of script being loaded.</param>
	    if (typeof (url) != 'string') {
	        throw new TypeError('Url must be a string.');
	    }
	    if (!checkIfFileExists || fileExists(url)) {
	        var head = d.getElementsByTagName('head')[0] || d.documentElement, scriptTags = getElementsByTagNames(head, 'script'), script, endsWithRegex = new RegExp(url.substring(url.lastIndexOf('/') + 1) + '\s*$'), done;
	        if (!scriptTags.any(function (i) {
	            return endsWithRegex.test(i.src);
	        })) {
	            script = d.createElement('script');
	            script.type = 'text/javascript';
	            script.src = url;
	            if (callback && typeof (callback) == 'function') {
	                done = false;
	                // Attach handlers for all browsers
	                script.onload = script.onreadystatechange = function () {
	                    if (!done && (!this.readyState ||
							this.readyState === 'loaded' || this.readyState === 'complete')) {
	                        done = true;
	                        callback();

	                        // Handle memory leak in IE
	                        script.onload = script.onreadystatechange = null;
	                        if (head && script.parentNode) {
	                            head.removeChild(script);
	                        }
	                    }
	                };
	            }

	            head.insertBefore(script, head.firstChild);
	        }
	    }
	},
	importCSS = w.importCSS = function (url, checkIfFileExists) {
	    if (typeof (url) != 'string') {
	        throw new TypeError('Url must be a string.');
	    }
	    if (!checkIfFileExists || fileExists(url)) {
	        var link = d.createElement('link'), head = d.getElementsByTagName('head')[0] || d.documentElement;
	        link.type = 'text/css';
	        link.href = url;
	        head.appendChild(link);
	    }
	},
	isDefined = w.isDefined = function (variable) {
	    ///	<summary>Check if a variable is defined in the scope of the window</summary>
	    ///	<returns type="Boolean" />
	    ///	<param name="variable" type="String">The variable to check for.</param>
	    return w[variable] !== u;
	},
	xGetElementById = w.xGetElementById = function (e) {
	    ///	<summary>Cross browser way to retrieve an element by id</summary>
	    ///	<returns type="Object" />
	    ///	<param name="e" type="String">The id of the element to search for.</param>
	    if (typeof (e) != 'string') {
	        return e;
	    }
	    if (d.getElementById) {
	        e = d.getElementById(e);
	    }
	    else if (d.all) {
	        e = d.all[e];
	    }
	    else if ($) {
	        e = $('#' + e)[0];
	    }
	    else {
	        e = u;
	    }
	    return e;
	},
	isArray = w.isArray = function (o) {
	    ///	<summary>Check if an object is an array</summary>
	    ///	<returns type="Boolean" />
	    ///	<param name="o" type="Object">The object to check.</param>
	    return o.constructor == Array;
	},
	isElement = w.isElement = function (o) {
	    ///	<summary>Check if the object is a dom element</summary>
	    ///	<returns type="Boolean" />
	    ///	<param name="o" type="Object">The object to check.</param>
	    try {
	        return o instanceof HTMLElement;
	    } catch (e) {
	        return typeof (o) == 'object' && o.nodeType === 1 && typeof (o.tagName) == 'string';
	    }
	},
	fileExists = w.fileExists = function (url, synchronousRequest) {
	    ///	<summary>Check if the file exists using an AJAX request</summary>
	    ///	<returns type="Boolean" />
	    ///	<param name="url" type="String">The url of the file to check.</param>
	    try {
            if (url === u || url == '') {
                return false;
            }

	        var fileRequest = w.ActiveXObject ? new ActiveXObject('Microsoft.XMLHTTP') : new XMLHttpRequest();
	        //We only need the head of the response to know whether the file exists or not, and we want to do it synchronously
	        fileRequest.open('HEAD', url, !!synchronousRequest);
	        fileRequest.send(null);
	        return fileRequest.status == 200 /*OK*/ || fileRequest.status == 304 /*Not modified*/;
	    } catch (e) {
	        //Since we can't load it, we'll just hope it's there
	        return true;
	    }
	},
	imageLoaded = w.imageLoaded = function (e) {
	    ///	<summary>Check if an image in an img tag is loaded</summary>
	    ///	<returns type="Boolean" />
	    if (!isElement(e)) {
	        e = xGetElementById(e);
	    }

	    if (e.complete !== u && !e.complete) {
	        return false;
	    }

	    if (img.naturalWidth !== u && img.naturalWidth === 0) {
	        return false;
	    }

	    return true;
	},
	getElementsByTagNames = w.getElementsByTagNames = function (context) {
	    ///	<summary>Get all elements in the DOM that match the tag name, returns an array instead of the default</summary>
	    ///	<returns type="Array" />
	    var elements = [], i, j, e, args = arguments;
	    for (i = 1; i < args.length; i++) {
	        e = context ? xGetElementById(context).getElementsByTagName(args[i]) : d.getElementsByTagName(args[i]);
	        for (j = 0; j < e.length; j++) {
	            elements.push(e[j]);
	        }
	    }
	    return elements;
	},
	getElementsByClass = w.getElementsByClass = function (className, context) {
	    ///	<summary>Get all elements in the DOM that have the specified class</summary>
	    ///	<returns type="Array" />
	    ///	<param name="className" type="String">The tag name to check for.</param>
	    var allElements = context && isElement(context) ? context.getElementsByTagName('*') : (d.all ? d.all : d.getElementsByTagName('*')), matches = [], i, classRegex = new RegExp('(\\s|^)' + className + '(\\s|$)');
	    for (i = 0; i < allElements.length; i++) {
	        if (classRegex.test(allElements[i].className)) {
	            matches.push(allElements[i]);
	        }
	    }
	    return matches;
	},
	getClasses = w.getClasses = function (o) {
	    ///	<summary>Get all the classes for an object</summary>
	    ///	<returns type="Array" />
	    ///	<param name="o" type="Object">The object.</param>
	    if (typeof (o) == 'string') {
	        o = xGetElementById(o);
	    }
	    if (!o.className) {
	        return [];
	    }
	    return o.className.split(' ');
	},
	parseBool = w.parseBool = function (o) {
	    ///	<summary>Try to parse an object to a bool</summary>
	    ///	<returns type="Boolean" />
	    ///	<param name="o" type="Object">The object to try and parse.</param>
	    var type = typeof (o), trueRegex = /^\s*true\s*$/i;
	    switch (type.toLowerCase()) {
	        case 'boolean':
	            return o;
	        case 'string':
	            return trueRegex.test(o);
	        case 'object':
	            switch (o.constructor) {
	                case Array:
	                    return !!o.length;
	                case Number:
	                    return !!o.valueOf();
	                case String:
	                    return trueRegex.test(o);
	                default:
	                    return !!getKeys(o).length;
	            }
	            break;
	        default:
	            return !!o;
	    }
	},
	 getKeys = w.getKeys = function (o, includeFunctions) {
	     ///	<summary>Get all of the keys in an object</summary>
	     ///	<returns type="Array" />
	     ///	<param name="o" type="Object">The object.</param>
	     ///	<param name="includeFunctions" type="Boolean">False to not include any keys for functions for the object.</param>
	     var results = [], key;
	     for (key in o) {
	         if (includeFunctions || typeof (o[key]) != 'function')
	             results.push(key);
	     }
	     return results;
	 },
	getValues = w.getValues = function (o, includeFunctions) {
	    ///	<summary>Get all of the values in an object</summary>
	    ///	<returns type="Array" />
	    ///	<param name="o" type="Object">The object.</param>
	    ///	<param name="includeFunctions" type="Boolean">False to not include any functions for the object.</param>
	    var results = [], key, val;
	    for (key in o) {
	        val = o[key];
	        if (includeFunctions || typeof (val) != 'function')
	            results.push(val);
	    }
	    return results;
	},
	where = w.where = function (o, whereFunc, testKeys) {
	    ///	<summary>Get all items in the array or object that matches the conditions</summary>
	    ///	<returns type="Array" />
	    ///	<param name="o" type="Object">The object.</param>
	    ///	<param name="whereFunc" type="Function">The conditions (should take the item).</param>
	    ///	<param name="testKeys" type="Boolean">Determine whether to test the keys of the object as well.</param>
	    var results = [], key, val;
	    for (key in o) {
	        val = o[key];
	        if (whereFunc(val)) {
	            results.push({ key: key, value: val });
	        } else if (testKeys && whereFunc(key)) {
	            results.push({ key: key, value: val });
	        }
	    }
	    return results;
	},
	contains = w.contains = function (o, value, checkType) {
	    ///	<summary>Check if object contains the value in either the keys or values</summary>
	    ///	<returns type="String" />
	    ///	<param name="o" type="Object">The object.</param>
	    ///	<param name="value" type="Object">The value to check for.</param>
	    ///	<param name="checkType" type="Boolean">Determine whether to use == or === in checking for value.</param>
	    var containsValue = getValues(o).contains(value, checkType);
	    if (getKeys(o).contains(value, checkType)) {
	        return containsValue ? ['key', 'value'] : ['key'];
	    } else if (containsValue) {
	        return ['value'];
	    }
	    return u;
	},
	containsRecursive = w.containsRecursive = function (o, value, checkType) {
	    ///	<summary>Recursively check if object contains the value in either the keys or values</summary>
	    ///	<returns type="String" />
	    ///	<param name="o" type="Object">The object.</param>
	    ///	<param name="value" type="Object">The value to check for.</param>
	    ///	<param name="checkType" type="Boolean">Determine whether to use == or === in checking for value.</param>
	    var result = contains(o, value, checkType), key, val;
	    if (result) {
	        return result;
	    }
	    for (key in o) {
	        val = o[key];
	        switch (typeof (val)) {
	            case 'object':
	                return containsRecursive(val, value, checkType);
	            case 'array':
	                return val.contains(value, checkType);
	        }
	    }
	    return u;
	},
	isVisible = w.isVisible = function (e) {
	    ///	<summary>Checks if an element is visible based on display, visibility, and opacity</summary>
	    ///	<returns type="Boolean" />
	    ///	<param name="e" type="Object">The element.</param>
	    if (typeof (e) == 'string') {
	        e = xGetElementById(e);
	    }
	    while (e.nodeName.toLowerCase() != 'body' && e.style.display.toLowerCase() != 'none' && e.style.visibility.toLowerCase() != 'hidden' && (!e.style.opacity || e.style.opacity > 0) && (!e.style.filter || !/alpha\(opacity\=0+(\.0+)?\)/.test(e.style.filter))) {
	        e = e.parentNode;
	    }
	    return e.nodeName.toLowerCase() == 'body';
	},
	getQueryStringValue = w.getQueryStringValue = function (key) {
	    var r = new RegExp(key + '=([^&]*)');
	    if (r.test(window.location.search))
	        return RegExp.$1;
	    return u;
	},
	createPostData = w.createPostData = function (context) {
	    ///	<summary>
	    ///	Create an object out of the inputs on a page 
	    ///	(or the inputs indicated in the option selectors as parameters) 
	    ///	with the id/name as the key and the value as the value
	    ///	</summary>
	    ///	<returns type="Object" />
	    var results = new Object(), inputs, i, j, key, value, t, uxRegex = /^ux/i, txtRegex = /^(txt|tbx)/i, camelCase = function (val, regex) {
	        return val.replace(uxRegex, '').replace(regex, '').toCamelCase();
	    };
	    if (arguments.length > 1 && $) {
	        inputs = [];
	        value = arguments;
	        for (i = 0; i < value.length; i++) {
	            key = context ? $(value[i], context) : $(value[i]);
	            for (j = 0; j < key.length; j++) {
	                t = key[j].nodeName.toLowerCase();
	                if (!inputs.contains(key[j]) && (t == 'input' || t == 'textarea' || t == 'select')) {
	                    inputs.push(key[j]);
	                }
	            }
	        }
	    } else {
	        inputs = getElementsByTagNames(context, 'input', 'select');
	    }
	    for (i = 0; i < inputs.length; i++) {
	        t = inputs[i];
	        if (isVisible(t)) {
	            switch (t.nodeName.toLowerCase()) {
	                case 'input':
	                    switch (t.type.toLowerCase()) {
	                        case 'text':
	                        case 'password':
	                        case 'hidden':
	                            results[camelCase(t.id, txtRegex)] = t.value;
	                            break;
	                        case 'checkbox':
	                            if (parseBool(t.checked)) {
	                                results[camelCase(t.name, /^cbx?/i)] += t.value + ', ';
	                            }
	                        case 'radio':
	                            if (parseBool(t.checked)) {
	                                results[camelCase(t.name, /^rb?/i)] = t.value;
	                            }
	                            break;
	                    }
	                    break;
	                case 'textarea':
	                    results[camelCase(t.id, txtRegex)] = t.value;
	                    break;
	                case 'select':
	                    results[camelCase(t.id, /^ddl/)] = t.value;
	                    break;
	            }
	        }
	    }
	    if (arguments.length == 0 || !$) {
	        inputs = getElementsByTagNames(context, 'textarea');
	        for (i = 0; i < inputs.length; i++) {
	            results[camelCase(inputs[i].id, txtRegex)] = inputs[i].value;
	        }
	    }
	    return results;
	},
	toJSONString = w.toJSONString = function (o, includeFunctions, includeConstructor) {
	    ///	<summary>Get a string representation of a JSON object</summary>
	    ///	<returns type="String" />
	    ///	<param name="o" type="Object">The object.</param>
	    ///	<param name="includeFunctions" type="Boolean">Determine whether to add the functions of an object to the string notation as well.</param>
	    //TODO: add handling for recursive elements like navigation
	    var builder = new StringBuilder(), i;
	    if (o === u) {
	        return 'undefined';
	    }
	    switch ((typeof (o)).toLowerCase()) {
	        case 'undefined':
	            return 'undefined';
	        case 'object':
	            if (o == null)
	                return 'null';
	            switch (o.constructor) {
	                case Array:
	                    builder.append('[');
	                    for (i = 0; i < o.length; i++) {
	                        builder.append(toJSONString(o[i])).append(i < o.length - 1 ? ', ' : '');
	                    }
	                    return builder.append(']').toString();
	                case Date:
	                case RegExp:
	                case Number:
	                case Boolean:
	                case String:
	                    return o.toString();
	                default:
	                    if (isElement(o)) {
	                        var classes = getClasses(o);
	                        builder.append(o.tagName.toLowerCase()).append(o.id ? '#' + o.id : '');
	                        for (i = 0; i < classes.length; i++) {
	                            builder.append('.').append(classes[i]);
	                        }
	                        return builder.append(o.value ? ' ' + o.value : '').toString();
	                    }
	                    else {
	                        builder.append('{');
	                        for (i in o) {
	                            if (i == 'constructor' && !includeConstructor)
	                                continue;
	                            if (typeof (o[i]) == 'function' && !includeFunctions)
	                                continue;
	                            builder.append('"').append(i).append('": ').append(toJSONString(o[i])).append(', ');
	                        }
	                        if (builder.strs.length > 1)
	                            builder.strs.pop();
	                        return builder.append('}').toString();
	                    }
	            }
	        case 'string':
	            return '"' + o.toString() + '"';
	        case 'function':
	            return (includeFunctions ? o.toString() : '');
	        default:
	            return o.toString();
	    }
	},
	newGuid = w.newGuid = function () {
	    var arr = [], hex = '0123456789ABCDEF', i;

	    for (i = 0; i < 36; i++) {
	        arr[i] = Math.round(Math.random() * 16);
	    }

	    // Conform to RFC-4122
	    arr[14] = 4;  // Set to version
	    arr[19] = (arr[19] & 3) | 8;  // Specify clock sequence

	    for (i = 0; i < 36; i++) {
	        arr[i] = hex[arr[i]];
	    }

	    arr[8] = arr[13] = arr[18] = arr[23] = '-';

	    return arr.join('');
	};
    if (!isDefined('Guid')) {
        w.Guid = {
            newGuid: newGuid
        };
    }
    if (!isDefined('CreditCard')) {
        w.CreditCard = {
            types: {
                //Default card types
                Visa: /4(?:\d{12}$|\d{15}$)/,
                MasterCard: /^5[1-5]\d{14}$/,
                AmericanExpress: /^3[47]\d{13}$/,
                Discover: /^6(?:011|5\d{2})\d{12}$/,
                DinersClub: /^3(?:0[0-5]|[68]\d)\d{11}$/,
                JCB: /^(?:2131|1800|35\d{3})\d{11}$/
            },
            validate: function (ccn, creditCardTypes, validateLuhn) {
                ///	<summary>Check if the string is a valid credit card number</summary>
                ///	<returns type="Object" />
                ///	<param name="ccn" type="String">The credit card number to validate.</param>
                ///	<param name="creditCardTypes" type="Object">Should be a string regex pair defining the card name and the regex pattern to follow.</param>
                ///	<param name="validateLuhn" type="Boolean">Whether to validate it using Luhn's algorithm as well.</param>
                if (!creditCardTypes || !getKeys(creditCardTypes).length) creditCardTypes = CreditCard.types;
                if (validateLuhn === u) validateLuhn = true;
                ccn = ccn.replace(/\D/g, '');
                if (!ccn.length || isNaN(ccn))
                    return { cardType: '', isValid: false };

                var key;
                for (key in creditCardTypes) {
                    if (creditCardTypes[key].test(ccn))
                        return { cardType: key, isValid: (validateLuhn ? CreditCard.luhnValidation(ccn) : true) };
                }
                return { cardType: '', isValid: false };
            },
            luhnValidation: function (ccn) {
                ///	<summary>Check if the string validates against Luhn's algorithm</summary>
                ///	<returns type="Boolean" />
                ///	<param name="ccn" type="String">The credit card number to validate.</param>
                ccn = ccn.replace(/\D/g, '');
                var i, total = 0, digit, parity = ccn.length % 2;

                for (i = 0; i < ccn.length; i++) {
                    digit = parseInt(ccn.charAt(i));
                    if (i % 2 == parity) {
                        digit *= 2;
                        if (digit > 9)
                            digit -= 9;
                    }
                    total += digit;
                }
                return total % 10 == 0;
            }
        }
    }
    if (!isDefined('Browser')) {
        w.Browser = w.Browsers = {
            //Default browsers to check for
            IE: { appName: 'Microsoft Internet Explorer' },
            Chrome: { appName: 'Netscape', vendor: 'Google' },
            Safari: { appName: 'Netscape', vendor: 'Apple' },
            Firefox: { appName: 'Netscape', userAgent: 'Firefox' },
            Mozilla: { appName: 'Netscape', userAgent: 'Mozilla' },
            Opera: { appName: 'Opera' },
            is: function (browser) {
                ///	<summary>Check if the browser is a certain type</summary>
                ///	<returns type="Boolean" />
                ///	<param name="browser" type="String">The browser name (IE, Chrome, Safari, Firefox, Mozilla, or Opera).</param>
                return this.determineType().toLowerCase() == browser.toLowerCase();
            },
            determineType: function () {
                ///	<summary>Try to figure out the type of the browser based on a combination of appName, vendor, and userAgent</summary>
                ///	<returns type="String" />
                var appName = navigator.appName, vendor = navigator.vendor, userAgent = navigator.userAgent, key, browsers = this, match, bVendor, bUserAgent;
                for (key in browsers) {
                    match = false;
                    if (typeof (browsers[key]) != 'function' && browsers[key].appName == appName) {
                        bVendor = browsers[key].vendor;
                        bUserAgent = browsers[key].userAgent;
                        if (bVendor) {
                            match = vendor && vendor.contains(bVendor);
                        }
                        if (bUserAgent) {
                            match = userAgent && userAgent.contains(bUserAgent);
                        }
                        if (!bVendor && !bUserAgent) {
                            match = true;
                        }
                        if (match)
                            return key;
                    }
                }
                return 'unknown';
            },
            determineVersion: function () {
                ///	<summary>Try to figure out the version of the browser based on userAgent or an empty string if the version cannot be determined</summary>
                ///	<returns type="String" />
                var prefix = '', browserType = this.determineType().toLowerCase();
                if (browserType == 'ie')
                    prefix = 'MSIE ';
                else if (browserType == 'chrome')
                    prefix = 'Chrome/';
                else if (browserType == 'safari')
                    prefix = 'Version/';
                else if (browserType == 'firefox')
                    prefix = 'Firefox/';
                else if (browserType == 'mozilla')
                    prefix = 'Mozilla/';
                else if (browserType == 'opera')
                    prefix = 'Opera/';
                else
                    return '';
                return new RegExp(prefix + '((\\d+\\.?)+)').test(navigator.userAgent) ? RegExp.$1 : '';
            }
        }
    }
    if (!isDefined('StringBuilder')) {
        w.StringBuilder = StringBuilder = function (str) {
            ///	<summary>Constructor for the StringBuilder object that mimics the C# StringBuilder class</summary>
            ///	<returns type="Object" />
            ///	<param name="str" type="String">The string to initialize the StringBuilder with (since C# has the same overload).</param>
            var t = this;
            t.strs = [];
            if (str) {
                t.strs.push(str);
            }
            return t;
        };
        StringBuilder.prototype = {
            append: function (str) {
                ///	<summary>Add a string to the StringBuilder</summary>
                ///	<returns type="Object" />
                ///	<param name="str" type="String">The item to append.</param>
                this.strs.push(str);
                return this;
            },
            clear: function () {
                ///	<summary>Clears everything from the StringBuilder</summary>
                ///	<returns type="Object" />
                this.strs = [];
                return this;
            },
            toString: function (separator) {
                ///	<summary>Override the object prototype's toString to join all of the strings in the StringBuilder together</summary>
                ///	<returns type="String" />
                return this.strs.join(separator || '');
            }
        };
    }

    /*************************************/
    /********** Array functions **********/
    /*************************************/
    if (!ap.remove) {
        ap.remove = function (value) {
            ///	<summary>Remove an item from the array</summary>
            ///	<returns type="Array" />
            ///	<param name="value" type="Object">The item to remove.</param>
            var t = this, i, found = false;
            if (t.indexOf) {
                i = t.indexOf(value);
                found = i > -1;
            }
            else {
                for (i = 0; i < t.length; i++) {
                    if (t[i] === value) {
                        found = true;
                        break;
                    }
                }
            }
            if (found)
                t.splice(i, 1);
            return t;
        };
    }
    if (!ap.contains) {
        ap.contains = function (item, checkType) {
            ///	<summary>Check if the item is in the array</summary>
            ///	<returns type="Boolean" />
            ///	<param name="item" type="Object">The item to search for.</param>
            ///	<param name="checkType" type="Boolean">Use === instead of == to check for type equality as well</param>
            var i, t = this, val;
            for (i = 0; i < t.length; i++) {
                val = t[i];
                if (typeof (val) == 'object') {
                    if (val.id && item.id && val.id == item.id) {
                        return true;
                    }
                    else if (toJSONString(val) == toJSONString(item)) {
                        return true;
                    }
                } else {
                    if ((checkType && val === item) || (!checkType && val == item)) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
    if (!ap.count) {
        ap.count = function (countFunc) {
            ///	<summary>Count the number of items in the array that match the conditions</summary>
            ///	<returns type="Number" />
            ///	<param name="countFunc" type="Function">The conditions (should take item and index).</param>
            var count = 0, i, t = this;
            if (!countFunc) {
                return t.length;
            }
            for (i = 0; i < t.length; i++) {
                if (countFunc(t[i], i)) {
                    ++count;
                }
            }
            return count;
        }
    }
    if (!ap.select) {
        ap.select = function (f) {
            ///	<summary>Select a single field out of the each object in an array</summary>
            ///	<returns type="Array" />
            ///	<param name="f" type="Object">A string representing the field to select or a function returning the object to select.</param>
            var fType = typeof (f), results = [], t = this, i, val;
            if (fType != 'string' && fType != 'function') {
                throw 'Select clause must be a string or function.';
            }
            for (i = 0; i < t.length; i++) {
                val = t[i];
                if (fType == 'function') {
                    results.push(f(val, i));
                } else {
                    if (typeof (val) == 'object' && val[f] !== u) {
                        results.push(val[f]);
                    }
                }
            }
            return results;
        }
    }
    if (!ap.where) {
        ap.where = function (whereFunc) {
            ///	<summary>Get all objects that meet a condition</summary>
            ///	<returns type="Array" />
            ///	<param name="whereFunc" type="Function">The function to determine which items to get (should take item and index).</param>
            if (typeof (whereFunc) != 'function') {
                throw 'Where function must be a function';
            }
            var results = [], t = this, i;
            for (i = 0; i < t.length; i++) {
                if (whereFunc(t[i], i)) {
                    results.push(t[i]);
                }
            }
            return results;
        }
    }
    if (!ap.any) {
        ap.any = function (anyFunc) {
            ///	<summary>Determine if any items in the array meet the conditions</summary>
            ///	<returns type="Boolean" />
            ///	<param name="anyFunc" type="Function">The function determining the conditions (should take item and index).</param>
            if (typeof (anyFunc) != 'function') {
                throw 'Any function must be a function';
            }
            var t = this, i;
            for (i = 0; i < t.length; i++) {
                if (anyFunc(t[i], i)) {
                    return true;
                }
            }
            return false;
        }
    }
    if (!ap.all) {
        ap.all = function (allFunc) {
            ///	<summary>Determine if all items in the array meet the conditions</summary>
            ///	<returns type="Boolean" />
            ///	<param name="allFunc" type="Function">The function determining the conditions (should take item and index).</param>
            if (typeof (allFunc) != 'function') {
                throw 'All function must be a function';
            }
            var t = this, i;
            for (i = 0; i < t.length; i++) {
                if (!allFunc(t[i], i)) {
                    return false;
                }
            }
            return true;
        }
    }
    if (!ap.groupBy) {
        ap.groupBy = function (f) {
            ///	<summary>Groups items in an array by a field</summary>
            ///	<returns type="Array" />
            ///	<param name="f" type="Object">The field (string) or a function returning the value to group by.</param>
            var groups = [], currentValue, t = this, matchedGroup, i, fType = typeof (f), val;
            if (fType != 'string' && fType != 'function') {
                throw 'Field must be a string or function.';
            }
            for (i = 0; i < t.length; i++) {
                val = t[i];
                currentValue = fType == 'function' ? f(val, i) : val[f];
                if (!groups.any(function (group) {
                    return group.key == currentValue;
                })) {
                    groups.push({
                        key: currentValue,
                        items: []
                    });
                }
                matchedGroup = groups.first(function (g) {
                    return g.key == currentValue;
                });
                matchedGroup.items.push(val);
            }
            return groups;
        }
    }
    function quickSortPartition(a, beg, end, p, f) {
        ///	<summary>Partition a portion of the array</summary>
        ///	<returns type="Number" />
        ///	<param name="a" type="Array">The array to partition.</param>
        ///	<param name="beg" type="Number">The beggining position of the partition.</param>
        ///	<param name="end" type="Number">The end position of the partition.</param>
        ///	<param name="p" type="Number">The pivot point.</param>
        ///	<param name="f" type="Object">The string or function defining the field to sort by.</param>
        var pivotValue = a[p], i, b = beg, val, fType = typeof (f);
        if (f) {
            if (fType == 'string') {
                pivotValue = pivotValue[f];
            } else if (fType == 'function') {
                pivotValue = f(pivotValue);
            }
        }
        a.swap(p, end);
        for (i = beg; i < end; i++) {
            val = a[i];
            if (f) {
                if (fType == 'string') {
                    val = val[f];
                } else if (fType == 'function') {
                    val = f(val);
                }
            }
            if (val < pivotValue) {
                a.swap(b, i);
                ++b;
            }
        }
        a.swap(end, b);
        return b;
    }
    function quickSort(a, beg, end, f) {
        ///	<summary>Sort an array using the quick sort algorithm</summary>
        ///	<returns type="Array" />
        ///	<param name="a" type="Array">The array to sort.</param>
        ///	<param name="beg" type="Number">The beggining position of the sort.</param>
        ///	<param name="end" type="Number">The end position of the sort.</param>
        ///	<param name="f" type="Object">The string or function defining the field to sort by.</param>
        if (a.length < 1)
            return a;
        if (end > beg) {
            var p = quickSortPartition(a, beg, end, beg, f);

            quickSort(a, beg, p - 1, f);
            quickSort(a, p + 1, end, f);
        }
        return a;
    }
    if (!ap.swap) {
        ap.swap = function (a, b) {
            ///	<summary>Swap 2 indices in an array</summary>
            ///	<returns type="Array" />
            ///	<param name="a" type="Number">The first index.</param>
            ///	<param name="b" type="Number">The second index.</param>
            var t = this, temp = t[a];
            t[a] = t[b];
            t[b] = temp;
        }
    }
    if (!ap.orderBy) {
        ap.orderBy = function (f) {
            ///	<summary>Sort by a single field in each object in an array</summary>
            ///	<returns type="Array" />
            ///	<param name="f" type="Object">A string representing the field to order by or a function returning the value to order by.</param>
            var type = typeof (f);
            if (f && (type != 'string' && type != 'function')) {
                throw 'Sort by clause must be a string or function.';
            }
            return quickSort(this, 0, this.length - 1, f);
        }
    }
    if (!ap.skip) {
        ap.skip = function (i) {
            ///	<summary>Skip a number of items at the beginning of the array</summary>
            ///	<returns type="Array" />
            ///	<param name="i" type="int">The number of items to skip.</param>
            var results = [], t = this, j;
            for (j = i - 1; j < t.length; j++) {
                results.push(t[j]);
            }
            return results;
        }
    }
    if (!ap.skipWhile) {
        ap.skipWhile = function (skipWhileFunc) {
            ///	<summary>Skip items while the condition determined by the function is true</summary>
            ///	<returns type="Array" />
            ///	<param name="takeWhileFunc" type="Function">The conditions (should take item and index).</param>
            if (typeof (skipWhileFunc) != 'function') {
                throw 'Skip while function must be a function';
            }
            var results = [], i = 0, t = this;
            while (skipWhileFunc(t[i], i)) {
                ++i;
            }
            for (i; i < t.length; i++) {
                results.push(t[i]);
            }
            return results;
        }
    }
    if (!ap.take) {
        ap.take = function (i) {
            ///	<summary>Take a number of items from the beginning of the array</summary>
            ///	<returns type="Array" />
            ///	<param name="i" type="int">The number of items to take.</param>
            var results = [], j;
            for (j = 0; j < i; j++) {
                results.push(this[j]);
            }
            return results;
        }
    }
    if (!ap.takeWhile) {
        ap.takeWhile = function (takeWhileFunc) {
            ///	<summary>Take items while the condition determined by the function is true</summary>
            ///	<returns type="Array" />
            ///	<param name="takeWhileFunc" type="Function">The conditions (should take item and index).</param>
            if (typeof (takeWhileFunc) != 'function') {
                throw 'Take while function must be a function';
            }
            var results = [], i = 0, t = this;
            while (takeWhileFunc(t[i], i)) {
                results.push(t[i]);
                ++i;
            }
            return results;
        }
    }
    if (!ap.first) {
        ap.first = function (firstFunc) {
            ///	<summary>Get the first item in the array that meets the conditions</summary>
            ///	<returns type="Object" />
            ///	<param name="firstFunc" type="Function">The conditions (should take item and index).</param>
            var i, t = this;
            if (!firstFunc) {
                return t.length ? t[0] : u;
            }
            if (typeof (firstFunc) != 'function') {
                throw 'First function must be a function';
            }
            for (i = 0; i < t.length; i++) {
                if (firstFunc(t[i], i)) {
                    return t[i];
                }
            }
            return null;
        }
    }
    if (!ap.last) {
        ap.last = function (lastFunc) {
            ///	<summary>Get the last item in the array that meets the conditions</summary>
            ///	<returns type="Object" />
            ///	<param name="lastFunc" type="Function">The conditions (should take item and index).</param>
            var i, t = this;
            if (!lastFunc) {
                return t.length ? t[t.length - 1] : u;
            }
            if (typeof (lastFunc) != 'function') {
                throw 'Last function must be a function';
            }
            for (i = t.length - 1; i >= 0; i--) {
                if (lastFunc(t[i], i)) {
                    return t[i];
                }
            }
            return null;
        }
    }
    if (!ap.each) {
        ap.each = function (func) {
            ///	<summary>Executes a function against each object in an array and returns the results</summary>
            ///	<returns type="Array" />
            ///	<param name="func" type="Function">The function to execute (should take item and index).</param>
            var results = [], r, i, t = this;
            for (i = 0; i < t.length; i++) {
                results.push(func(t[i], i));
            }
            return results;
        }
    }
    if (!ap.distinct) {
        ap.distinct = function (checkType) {
            ///	<summary>Gets a list of distinct objects out of an array</summary>
            ///	<returns type="Array" />
            ///	<param name="checkType" type="Boolean">Determine whether to use === or == when checking for distinct items.</param>
            var results = [], i, t = this;
            for (i = 0; i < t.length; i++) {
                if (!results.contains(t[i], checkType)) {
                    results.push(t[i]);
                }
            }
            return results;
        }
    }

    /**************************************/
    /********** String functions **********/
    /**************************************/
    if (!sp.contains) {
        sp.contains = function (str) {
            ///	<summary>Tests whether this string contains with the specified value</summary>
            ///	<returns type="Boolean" />
            ///	<param name="str" type="String">The string to search for.</param>
            return this.indexOf(str) >= 0;
        }
    }
    if (!sp.startsWith) {
        sp.startsWith = function (str) {
            ///	<summary>Tests whether this string starts with the specified value</summary>
            ///	<returns type="Boolean" />
            ///	<param name="str" type="String">The string to search for.</param>
            return new RegExp('^' + str).test(this);
        }
    }
    if (!sp.endsWith) {
        sp.endsWith = function (str) {
            ///	<summary>Tests whether this string ends with the specified value</summary>
            ///	<returns type="Boolean" />
            ///	<param name="str" type="String">The string to search for.</param>
            return new RegExp(str + '$').test(this);
        }
    }
    if (!sp.trim) {
        sp.trim = function () {
            ///	<summary>Trim the whitespace off of a string (with micro optimizations)</summary>
            ///	<returns type="String" />
            var str = this.replace(/^\s+/, ''), i;
            for (i = str.length - 1; i >= 0; i--) {
                if (/\S/.test(str.charAt(i))) {
                    str = str.substring(0, i + 1);
                    break;
                }
            }
            return str;
        }
    }
    if (!sp.toPascalCase) {
        sp.toPascalCase = function (specialCases) {
            ///	<summary>Change a string to Pascal case from spaced (i.e. 'The quick brown fox' to 'TheQuickBrownFox')</summary>
            ///	<returns type="String" />
            var t = this, i, builder = '', strParts;
            if (t.length == 0 || t.length == 1)
                return t.toUpperCase();
            if (specialCases && isArray(specialCases)) {
                for (i = 0; i < specialCases.length; i++) {
                    if (t == specialCases[i])
                        return t;
                }
            }
            strParts = t.split(' ');
            for (i = 0; i < strParts.length; i++) {
                builder += strParts[i].substring(0, 1).toUpperCase() + strParts[i].substring(1).toLowerCase() + ' ';
            }
            return builder.trim();
        }
    }
    if (!sp.toCamelCase) {
        sp.toCamelCase = function (specialCases) {
            ///	<summary>Change a string to Camel case from spaced (i.e. 'The quick brown fox' to 'theQuickBrownFox')</summary>
            ///	<returns type="String" />
            var str = this.toPascalCase(specialCases);
            return str.substr(0, 1).toLowerCase() + str.substr(1);
        }
    }
    if (!sp.toTitleCase) {
        sp.toTitleCase = function () {
            ///	<summary>Change a string to normal sentence structure (i.e. 'The Quick Brown Fox' to 'The quick brown fox')</summary>
            ///	<returns type="String" />
            var t = this, builder = new StringBuilder(), strParts, i, part;
            if (!t || t.length == 1)
                return t.toUpperCase();
            strParts = t.split(' ');
            for (i = 0; i < strParts.length; i++) {
                part = strParts[i];
                if (i == 0) {
                    builder.append(part.substr(0, 1).toUpperCase()).append(part.substr(1).toLowerCase()).append(' ');
                } else {
                    builder.append(part.toLowerCase()).append(' ');
                }
            }
            return builder.toString().trim();
        }
    }
    if (!sp.toSpaced) {
        sp.toSpaced = function () {
            ///	<summary>Change a string to spaced from Pascal case (i.e. 'TheQuickBrownFox' to 'The Quick Brown Fox')</summary>
            ///	<returns type="String" />
            var t = this;
            if (!t || t.length == 1)
                return t;
            return t.replace(/([A-Z])/g, ' $1').trim();
        }
    }
    if (!sp.checkIfPOBox) {
        sp.checkIfPOBox = function () {
            ///	<summary>Check if the string contains a PO Box for address validation</summary>
            ///	<returns type="Boolean" />
            return /P(ost)?\.?\s*O(ffice)?\.?\s*Box/i.test(this);
        }
    }
    if (!sp.validateCreditCard) {
        sp.validateCreditCard = function (types) {
            ///	<summary>Check if the string is a valid credit card number</summary>
            ///	<returns type="Boolean" />
            ///	<param name="types" type="Object">Should be a string regex pair defining the card name and the regex pattern to follow (i.e. {Visa: /\d{16}/}).</param>
            return CreditCard.validate(this, types);
        }
    }
    if (!sp.allIndicesOf) {
        sp.allIndicesOf = function (pattern) {
            ///	<summary>Check string for all indexes of a pattern</summary>
            ///	<returns type="Array" />
            ///	<param name="pattern" type="RegExp">The pattern to search for.</param>
            var results = [], match;
            while (match = pattern.exec(this)) {
                results.push(match.index);
            }
            return results;
        }
    }
    if (!String.format) {
        String.format = function (format) {
            ///	<summary>Format a string with the given arguments</summary>
            ///	<returns type="String" />
            ///	<param name="format" type="String">The format of the string.</param>
            var args = arguments, i;
            if (args.length <= 1) {
                return format;
            }
            for (i = 1; i < args.length; i++) {
                format = format.replace(new RegExp('\\{' + (i - 1) + '\\}', 'g'), args[i]);
            }
            return format;
        };
    }
    if (!String.isNullOrEmpty) {
        String.isNullOrEmpty = function (str) {
            /// <summary>Determines whether a string is null (undefined) or empty</summary>
            /// <returns type="Boolean" />
            /// <param name="str" type="String">The string to check.</param>
            return !!str;
        }
    }

    /* jQuery functions */
    if ($) {
        jp = $.fn;
        if (!$.textSize) {
            $.textSize = function (element) {
                element = $(element);
                var div = $('<div></div>').css({ position: 'absolute', left: '-1000px', top: '-1000px', display: 'none' }).html(element.html()), size = {};
                $('body').append(div);
                $.each(['font-size', 'font-style', 'font-weight', 'font-family', 'line-height', 'text-transform', 'letter-spacing'], function () {
                    var t = this.toString();
                    //hack for Firefox, because font-family returns serif if font-family isn't specifically set on the element
                    if (t == 'font-family') {
                        var fontFamily = element.css(t), parent = element;
                        if (fontFamily == 'serif') {
                            while ((parent = parent.parent()).length && fontFamily == 'serif') {
                                fontFamily = parent.css(t);
                            }
                        }
                        div.css(t, fontFamily);
                    } else {
                        div.css(t, element.css(t));
                    }
                });
                size.height = div.outerHeight();
                size.width = div.outerWidth();
                div.remove();
                return size;
            }
        }
        if (!jp.getHiddenDimensions) {
            jp.getHiddenDimensions = function (includeMargin) {
                var t = this,
					props = { position: 'absolute', visibility: 'hidden', display: 'block' },
					dim = { width: 0, height: 0, innerWidth: 0, innerHeight: 0, outerWidth: 0, outerHeight: 0 },
					hiddenParents = t.parents().andSelf().not(':visible'),
					includeMargin = (includeMargin == null) ? false : includeMargin;

                var oldProps = [];
                hiddenParents.each(function () {
                    var old = {};

                    for (var name in props) {
                        old[name] = this.style[name];
                        this.style[name] = props[name];
                    }

                    oldProps.push(old);
                });

                dim.width = t.width();
                dim.outerWidth = t.outerWidth(includeMargin);
                dim.innerWidth = t.innerWidth();
                dim.height = t.height();
                dim.innerHeight = t.innerHeight();
                dim.outerHeight = t.outerHeight(includeMargin);

                hiddenParents.each(function (i) {
                    var old = oldProps[i];
                    for (var name in props) {
                        this.style[name] = old[name];
                    }
                });

                return dim;
            }
        }
        if (!jp.toArray) {
            jp.toArray = function () {
                ///	<summary>Change the jQuery makeArray to an extension method instead</summary>
                ///	<returns type="Array" />
                return $.makeArray(this);
            }
        }
        if (!jp.contains) {
            jp.contains = function (value) {
                ///	<summary>Check if the element contains the value (checks the option values for a select, checks the value attribute for inputs and textareas, and check the inner html for divs, spans, and ps)</summary>
                ///	<returns type="Boolean" />
                ///	<param name="value" type="String">The value to search for.</param>
                var t = this;
                switch (t[0].nodeName.toLowerCase()) {
                    case 'select':
                        //Check the options of the select
                        return t.children('option').toArray().any(function (i) {
                            return $(i).attr('value') == value || $(i).text() == value;
                        });
                    case 'ul':
                    case 'ol':
                        //Check the contents of each li
                        return t.children('li').toArray().any(function (i) {
                            return $(i).html() == value;
                        });
                    case 'input':
                    case 'textarea':
                        //Check the value of the input or textarea
                        return t.val().contains(value);
                    default:
                        //Check the html
                        return t.html().contains(value); ;
                }
            }
        }
        if (!jp.classes) {
            jp.classes = function () {
                ///	<summary>Get all of the classes of an element</summary>
                ///	<returns type="Array" />
                return this.attr('class').split(' ');
            }
        }
        if (!jp.isVisible) {
            jp.isVisible = function () {
                ///	<summary>Checks the element(s) and all of it's parents to determine if it's visible to the user, if any elements are not visible, it will return false</summary>
                ///	<returns type="Boolean" />
                return this.toArray().all(function (e) {
                    return isVisible(e);
                });
            }
        }
        if (jp.areAnyVisible) {
            jp.areAnyVisible = function () {
                ///	<summary>Checks the element(s) and all of it's parents to determine if it's visible to the user, if any elements are visible, it will return true</summary>
                ///	<returns type="Boolean" />
                return this.toArray().any(function (e) {
                    return isVisible(e);
                });
            }
        }
        if (!jp.fullVal) {
            jp.fullVal = function (separator) {
                var builder = new StringBuilder();
                this.each(function () {
                    builder.append($(this).val());
                    if (separator !== u && separator)
                        builder.append(separator);
                });
                return builder.toString();
            }
        }
        if (!jp.moveCursorToEnd) {
            jp.moveCursorToEnd = function () {
                return this.each(function () {
                    $(this).focus();

                    if (this.setSelectionRange) {
                        // Double the length because Opera is inconsistent about whether a carriage return is one character or two.
                        var len = $(this).val().length * 2;
                        this.setSelectionRange(len, len);
                    }
                    else {
                        $(this).val($(this).val());
                    }

                    // Scroll to the bottom, in case we're in a tall textarea
                    this.scrollTop = 999999;
                });
            }
        }
        //overwrite css to return the entire computed style if nothing is passed in
        jp.css2 = jQuery.fn.css;
        jp.css = function () {
            if (arguments.length) return jp.css2.apply(this, arguments);
            //TODO: add CSS 3 properties - DES
            var properties = ['font-family', 'font-size', 'font-weight', 'font-style', 'color',
				'text-transform', 'text-decoration', 'letter-spacing', 'word-spacing',
				'line-height', 'text-align', 'vertical-align', 'direction', 'background-color',
				'background-image', 'background-repeat', 'background-position',
				'background-attachment', 'opacity', 'width', 'height', 'top', 'right', 'bottom',
				'left', 'margin-top', 'margin-right', 'margin-bottom', 'margin-left',
				'padding-top', 'padding-right', 'padding-bottom', 'padding-left',
				'border-top-width', 'border-right-width', 'border-bottom-width',
				'border-left-width', 'border-top-color', 'border-right-color',
				'border-bottom-color', 'border-left-color', 'border-top-style',
				'border-right-style', 'border-bottom-style', 'border-left-style', 'position',
				'display', 'visibility', 'z-index', 'overflow-x', 'overflow-y', 'white-space',
				'clip', 'float', 'clear', 'cursor', 'list-style-image', 'list-style-position',
				'list-style-type', 'marker-offset'];
            var i, css = {};
            for (i = 0; i < properties.length; i++)
                css[properties[i]] = jp.css2.call(this, properties[i]);
            return css;
        }
        if (!jp.removeCss) {
            jp.removeCss = function (cssProperties) {
                return this.each(function () {
                    var t = $(this);
                    $.grep(cssProperties.split(','), function (cssProp) {
                        t.css(cssProp, '');
                    });
                    return t;
                });
            }
        }
        if (!jp.removeInlineCss) {
            jp.removeInlineCss = function (cssProperty) {
                return this.each(function () {
                    var t = $(this), r = new RegExp(cssProperty + (/border|margin|padding/i.test(cssProperty) ? '(-(top|right|bottom|left))?' : '') + ':', 'i'), newStyle;
                    if (t.attr('style')) {
                        newStyle = $.grep(t.attr('style').split(';'), function (prop) {
                            if (!r.test(prop))
                                return prop;
                        }).join(';');
                        if (newStyle)
                            t.attr('style', newStyle);
                        else
                            t.removeAttr('style');
                    }
                });
            }
        }
        if (!jp.showError) {
            jp.showError = function (error) {
                ///	<summary>Apply a style and tooltip to an element to denote an error</summary>
                ///	<returns type="jQuery" />
                ///	<param name="error" type="String">The error to show in the tooltip.</param>
                return this.each(function () {
                    var t = $(this).css({ border: '2px solid #FF0000' }), messageBubble = $('<div class="errorMessageBubble" title="Click to hide"></div>').html(error).css({
                        position: 'absolute',
                        left: (t.position().left + t.width() + 2) + 'px',
                        top: t.position().top + 'px'
                    });
                    if ($.browser.msie) {
                        messageBubble.css('top', (t.position().top - 1) + 'px');
                    }
                    if (error && !t.data('hasError')) {
                        t.data('hasError', true).after(messageBubble);
                        messageBubble.width($.textSize(messageBubble).width + 1).height(t.height() + 4 /* border width */).fadeIn().bind('invalidate', function () {
                            var messageBubble = $(this), t = messageBubble.prev();
                            messageBubble.css({
                                left: (t.position().left + t.width() + 2) + 'px',
                                top: t.position().top + 'px'
                            }).height(t.height() + 4);
                            if ($.browser.msie) {
                                messageBubble.css('top', (t.position().top - 1) + 'px');
                            }
                        });
                    }
                });
            }
        }
        if (!jp.clearError) {
            jp.clearError = function () {
                ///	<summary>Clear the element of the error styles and tooltip</summary>
                ///	<returns type="jQuery" />
                return this.each(function () {
                    var t = $(this);
                    if ($.browser.msie) {
                        t.removeInlineCss('border');
                    } else {
                        t.css({ border: '' });
                    }
                    t.removeData('hasError').next('div.errorMessageBubble').remove();
                    $('div.errorMessageBubble').trigger('invalidate');
                });
            }
        }
        if (!jp.clearErrorMS) {
            jp.clearErrorMS = function () {
                ///	<summary>Clear the element of the error styles and tooltip - for Microsoft validations</summary>
                ///	<returns type="jQuery" />
                return this.each(function () {
                    $(this)
                    .removeClass('input-validation-error')
                    .next('span.field-validation-error')
                        .removeClass('field-validation-error')
                        .addClass('field-validation-valid')
                        .children()
                            .remove();
                });
            }
        }
        if (!jp.watermark) {
            jp.watermark = function (watermark, color) {
                ///	<summary>Makes a watermark for inputs (and replaces the textbox in the case of password inputs</summary>
                ///	<returns type="jQuery" />
                ///	<param name="watermark" type="String">The watermark text to show.</param>
                ///	<param name="color" type="String">The color to make the watermark (default #777777).</param>
                return this.each(function () {
                    var t = $(this).data('watermark', watermark).blur(function () {
                        //Put the watermark back in the input if it's empty
                        if (!t.val()) {
                            if (passwordField) {
                                t.hide();
                                passwordReplacement.show();
                            } else {
                                t.val(watermark).css('color', color);
                            }
                        }
                    }),
						passwordField = t.attr('type') && t.attr('type').toLowerCase() == 'password',
						passwordReplacement,
						originalColor = t.css('color'),
						hidden = !t.is(':visible'),
						width = (passwordField && hidden) ? t.getHiddenDimensions().width : t.width(),
						height = (passwordField && hidden) ? t.getHiddenDimensions().height : t.height();
                    color = color || '#777777';

                    if (passwordField) {
                        passwordReplacement = $('<input type="text" value="' + watermark + '" />').css({
                            width: width + 'px',
                            height: height + 'px',
                            color: color
                        }).attr('class', t.attr('class')).data('watermark', watermark);

                        //Make sure we copy the required field message
                        if (t.hasClass('required')) {
                            passwordReplacement.attr('name', t.attr('name')).data('errorMessage', t.data('errorMessage'));
                        }
                    }
                    //If it's a password field, switch out the 2 fields so the watermark can be seen, otherwise, clear out the input
                    if (passwordField) {
                        passwordReplacement.focus(function () {
                            passwordReplacement.hide();
                            t.show().focus();
                        });
                    } else {
                        t.focus(function () {
                            if (t.val() == watermark) {
                                t.css('color', originalColor).val('');
                            }
                        });
                    }

                    if (passwordField) {
                        t.after(passwordReplacement);
                        if (!t.val())
                            t.hide();
                        else
                            passwordReplacement.hide();
                    } else if (!t.val() || t.val() == watermark) {
                        t.val(watermark).css('color', color);
                    }
                });
            }
        }
        if (!jp.setupRequiredFields) {
            jp.setupRequiredFields = function () {
                ///	<summary>
                ///	Puts the required field messages into the data and resets the names so the inputs may be used in a postback
                ///	</summary>
                ///	<returns type="jQuery" />
                return this.each(function () {
                    $(this).find('.required').each(function () {
                        var t = $(this), name = t.attr('name');
                        //store the name in jQuery's data store and then reset the name so that it will show up correctly in form posts
                        if (name) {
                            t.data('errorMessage', name).attr('name', t.attr('id'));
                        }
                    });
                });
            }
        }
        if (!jp.checkRequiredFields) {
            jp.checkRequiredFields = function () {
                ///	<summary>
                ///	Checks all of the fields with a required class that are visible to make sure the input is:
                ///	1) filled in
                /// 2) not the same as the title attribute
                ///	If any conditions are not met, show an error (based on the name on the input, or the errorMessage from jQuery's data store) and focus the first field
                ///	</summary>
                ///	<returns type="boolean" />

                $('.errorMessageBubble').show();
                $('.errorMessageBubble').die().live('click', function () {
                    $(this).hide();
                });

                var isComplete = true, firstIncompleteField;
                this.each(function () {
                    $(this).find('.required').each(function () {

                        var t = $(this);
                        //var watermark = t.data('watermark');
                        var val = t.val() == null ? '' : t.val().trim();
                        var errorMessage = t.data('errorMessage') || t.attr('name');
                        var event;
                        var requiredFieldHandler = function () {
                            //var t = $(this), watermark = t.data('watermark'), val = t.val().trim();
                            var t = $(this), val = t.val().trim();
                            //if (t.isVisible() && (val && (!watermark || val != watermark))) {
                            if (t.isVisible() && val) {
                                t.clearError().unbind(event, requiredFieldHandler);
                            } else if (t.isVisible() && !val && !t.data('hasError')) {
                                t.showError(errorMessage);
                            }
                        };

                        if (t.is(':checkbox,:radio,:file,:button,:submit,:image')) {
                            event = 'click';
                        }
                        else if (t.is(':text,:password,textarea')) {
                            event = 'keyup';
                        }
                        else if (t.is('select,:hidden')) {
                            event = 'change';
                        }
                        //Check if the element is visible and has a valid value
                        //if (t.isVisible() && (!val || (watermark && val == watermark))) {
                        if (t.isVisible() && !val) {
                            if (!firstIncompleteField)
                                firstIncompleteField = t;
                            isComplete = false;
                            //Clear the error when they enter something in (and reshow it if they backspace)
                            //t.showError(errorMessage).bind(event, function (e) {
                            t.showError(errorMessage);
                            if (event) {
                                t.bind(event, function (e) {
                                    requiredFieldHandler.apply(t, [e]);
                                });
                            }
                        }
                    });
                });
                if (firstIncompleteField)
                    firstIncompleteField.focus();
                return isComplete;
            }
        }
        /* I was going to use this, but then I realize this is really inefficient since it requires me to 
        * instantiate the plugin prototype every time, even if we are using the stuff from the data cache 
        */
        //		function handlePlugin(pluginName, defaults, method, options, prototype, init) {
        //			var t = this;
        //			if ((method === u || typeof (method) == 'object') && !options) {
        //				//initialize
        //				options = method;
        //				method = u;
        //				options = $.extend({}, defaults, options);

        //				prototype.options = options;

        //				if (init && typeof (init) == 'function')
        //					init.apply(t, [prototype]);

        //				this.each(function () {
        //					if (!$(t).data(pluginName))
        //						$(t).data(pluginName, prototype);
        //				});
        //			} else {
        //				//method call
        //				var p = $(t).data(pluginName);
        //				if (method == 'destroy')
        //					$(t).removeData(pluginName);
        //				if (p && p[method] && typeof (p[method]) == 'function')
        //					p[method](t, options);
        //			}
        //		}
        if (!jp.messageCenter) {
            jp.messageCenter = function (method, options) {
                ///	<summary>Makes a colored message center to show errors or notifications to the user</summary>
                ///	<returns type="Object" />
                ///	<param name="color" type="String">The color to use for the foreground text.</param>
                ///	<param name="background" type="String">The color to make the background.</param>
                ///	<param name="iconPath" type="String">The path to the icons to use for the "bullets".</param>
                ///	<param name="fadeInSpeed" type="Object">The number of milliseconds to fade in or 'slow', 'normal', or 'fast'.</param>
                ///	<param name="fadeInAnimation" type="Object">Animation object to use in jQuery's animate method.</param>
                ///	<param name="fadeOutTimeout" type="Number">The number of milliseconds before fading out (if undefined or 0, will keep the message up indefinitely).</param>
                ///	<param name="fadeOutSpeed" type="Object">The number of milliseconds to fade in or 'slow', 'normal', or 'fast'.</param>
                ///	<param name="fadeOutAnimation" type="Object">Animation object to use in jQuery's animate method.</param>
                ///	<param name="additionalCSSClasses" type="String">More CSS classes to append to the existing ones on the message div".</param>
                var pluginName = 'messageCenter';
                if ((method === u || typeof (method) == 'object') && !options) {
                    //initialize
                    options = method;
                    method = u;
                    options = $.extend({}, {
                        color: '#FF0000',
                        background: '#FEE9E9',
                        //iconPath: 'Resource/Content/Images/exclamation.png',
                        includeCustomExitIcon: false,
                        checkForIcon: false,
                        fadeInSpeed: 'fast',
                        fadeInAnimation: u,
                        fadeOutTimeout: u,
                        fadeOutSpeed: u,
                        fadeOutAnimation: u,
                        removeMessageOnClick: true,
                        additionalCSSClasses: 'UI-icon icon-exclamation'
                    }, options);

                    var mc = {
                        iconExists: u,
                        maxId: 0,
                        tTimeout: u,
                        numOfMessages: 0,
                        addMessage: function (container, message, options) {
                            ///	<summary>Add a message to the message center using the already defined animations</summary>
                            ///	<returns type="Object" />
                            ///	<param name="message" type="String">The mesage to add.</param>
                            var t = this, iconExists, id = ++t.maxId, messageId = '#message' + id, timeout, css = 'messageCenterMessage';
                            options = $.extend({}, t.options, options);
                            //Check if the icon exists, otherwise, we won't use the icon (we don't want those pesky red X's now, do we?)
                            //We're going to default to disabling this for now, since this is the major slow down for the message center
                            if (t.iconExists === u || options.iconPath != t.options.iconPath) {
                                iconExists = options.checkForIcon ? fileExists(options.iconPath) : true;
                            } else {
                                iconExists = t.iconExists;
                            }
                            if (options.additionalCSSClasses && options.additionalCSSClasses != '') {
                                css += (' ' + options.additionalCSSClasses);
                            }
                            ++t.numOfMessages;
                            var newMessage = $('<div id="message' + t.maxId + '" class="' + css + '"></div>').append((iconExists ? '' : '')
                             + message +
                            (t.options.includeCustomExitIcon ? '<a class="UI-icon icon-x CustomExitIcon"></a>' : ''))
                            .appendTo(container).show().hide(), messageTimeout;

                            //Show the message
                            if (options.fadeInAnimation) {
                                if (typeof (options.fadeInAnimation) == 'function') {
                                    options.fadeInAnimation = options.fadeInAnimation(newMessage);
                                }
                                newMessage.animate(options.fadeInAnimation, options.fadeInSpeed);
                            } else if (options.fadeInSpeed) {
                                newMessage.fadeIn(options.fadeInSpeed);
                            } else {
                                newMessage.show();
                            }

                            //Set the automatic fade out if it has been configured
                            if (options.fadeOutTimeout) {
                                timeout = w.setTimeout(function () {
                                    $(container).messageCenter('removeMessage', id);
                                }, options.fadeOutTimeout);
                            }

                            if (options.removeMessageOnClick) {
                                newMessage.click(function () {
                                    $(container).messageCenter('removeMessage', id);
                                });
                            }

                            return { id: id, timeout: timeout };
                        },
                        removeMessage: function (container, messageId, options) {
                            ///	<summary>Removes one of the messages from the message center using the already defined animations</summary>
                            ///	<returns type="void" />
                            ///	<param name="messageId" type="Number">The id of the message to remove.</param>
                            var t = this, options = $.extend({}, t.options, options), message = $('#message' + messageId, container);
                            if (message.length) {
                                //Fire the fade out if it has been configured
                                if (options.fadeOutAnimation) {
                                    message.animate(options.fadeOutAnimation, options.fadeOutSpeed, null, function () {
                                        $(this).remove();
                                    });
                                } else if (options.fadeOutSpeed) {
                                    message.fadeOut(options.fadeOutSpeed, function () {
                                        $(this).remove();
                                    });
                                } else {
                                    message.hide().remove();
                                }
                            }

                            if (container.html().trim().length == 0) {
                                this.clearAllMessages(container);
                            }

                            //--t.numOfMessages;
                            return container;
                        },
                        clearAllMessages: function (container) {
                            ///	<summary>Clears out all messages and hides the message center</summary>
                            ///	<returns type="void" />
                            var clearAllresult = container.empty();
                            // warning: this parent div is setup in the BaseController, not in the utilities
                            if (container.parent('#messageCenterModal').length) {
                                container.parent('#messageCenterModal').hide();
                            }
                            return clearAllresult;
                        },
                        destroy: function (container) {
                            return this.clearAllMessages(container);
                        }
                    };

                    mc.options = options;

                    return this.each(function () {
                        if (!$(this).data(pluginName))
                            $(this).data(pluginName, mc);
                    });
                } else {
                    //method call
                    var p = this.data(pluginName);
                    if (method == 'destroy')
                        this.removeData(pluginName);
                    if (p && p[method] && typeof (p[method]) == 'function')
                        return p[method].apply(p, [this].concat(ap.splice.call(arguments, 1, arguments.length - 1)));
                }

                return this;
            }
        }
        if (!jp.phone) {
            jp.phone = function (method, options) {
                ///	<summary>Adds default phone fields to the element and handles the keyup to change focus to the next input</summary>
                ///	<returns type="jQuery" />
                ///	<param name="areaCodeId" type="String">The id of the area code input.</param>
                ///	<param name="firstThreeId" type="String">The id of the first three input.</param>
                ///	<param name="lastFourId" type="String">The id of the last four input.</param>
                ///	<param name="extensionId" type="String">The id of the extension input.</param>
                ///	<param name="addExtension" type="Boolean">Determine whether to show the extension input or not.</param>
                var pluginName = 'phone';
                if ((method === u || typeof (method) == 'object') && !options) {
                    //initialize
                    options = method;
                    method = u;
                    options = $.extend({}, {
                        areaCodeId: 'txtAreaCode',
                        firstThreeId: 'txtFirstThree',
                        lastFourId: 'txtLastFour',
                        extensionId: 'txtExtension',
                        addExtension: false
                    }, options);

                    var phone = {
                        addExtension: options.addExtension,
                        setPhone: function (container, phoneNumber) {
                            ///	<summary>Sets the phone number</summary>
                            ///	<returns type="void" />
                            ///	<param name="phoneNumber" type="String">The phone number to set (only numbers).</param>

                            var requiredLength = 10;
                            var phoneNumberNew = phoneNumber.replace(/\D/g, '');
                            var loopCount = (phoneNumberNew.length == 0) ? 0 : requiredLength - phoneNumberNew.length;
                            var i = 0;

                            for (i = 0; i < loopCount; i++) {
                                phoneNumberNew = ' ' + phoneNumberNew;
                            }

                            return container.inputsByFormat('setValue', phoneNumberNew);
                        },
                        getPhone: function (container, format) {
                            ///	<summary>Gets the phone number by using the specified format</summary>
                            ///	<returns type="string" />
                            ///	<param name="format" type="String">The format expected (i.e. ({0}) {1} - {2} x{3}).</param>
                            format = format || '{0}{1}{2}' + (this.addExtension ? '{3}' : '');
                            return container.inputsByFormat('getValue', format);
                        },
                        destroy: function (container) {
                            return container.inputsByFormat('destroy');
                        }
                    }, phoneInputs = [];

                    phoneInputs.push({
                        id: options.areaCodeId,
                        length: 3,
                        size: 3,
                        css: { width: 30 }
                    });
                    phoneInputs.push({
                        id: options.firstThreeId,
                        length: 3,
                        size: 3,
                        css: { width: 30 }
                    });
                    phoneInputs.push({
                        id: options.lastFourId,
                        length: 4,
                        size: 4,
                        css: { width: 40 }
                    });
                    if (options.addExtension) {
                        phoneInputs.push({
                            id: extensionId,
                            size: 4,
                            css: { width: 55 }
                        });
                    }
                    return this.inputsByFormat({ format: '({0})&nbsp;{1}&nbsp;-&nbsp;{2}' + (options.addExtension ? '&nbsp;{3}' : ''), validateNumber: true, attributes: phoneInputs }).each(function () {
                        if (!$(this).data(pluginName))
                            $(this).data(pluginName, phone);
                    }); ;
                } else {
                    //method call
                    var p = this.data(pluginName);
                    if (method == 'destroy')
                        this.removeData(pluginName);
                    if (p && p[method] && typeof (p[method]) == 'function') {
                        return p[method].apply(p, [this].concat(ap.splice.call(arguments, 1, arguments.length - 1)));
                    }
                }


                return this;
            }
        }
        if (!jp.inputsByFormat) {
            jp.inputsByFormat = function (method, options) {
                ///	<summary>Adds phone fields to the element based on the format and handles the keyup to change focus to the next input</summary>
                ///	<returns type="jQuery" />
                ///	<param name="format" type="String">The format of the inputs (i.e. ({0}) {1} - {2}).</param>
                ///	<param name="validateNumber" type="Boolean">Determine whether to check if only digits entered.</param>
                ///	<param name="attributes" type="Array">The attributes to apply to each of the inputs that should conform to the following JSON: { id : 'id of the element', length: 'adds maxlength attribute (required)', size: 'adds size attribute', css: 'same object as for jQuery's css method (see http://docs.jquery.com/CSS/css#properties for more info)' }</param>
                var pluginName = 'inputsByFormat';
                if ((method === u || typeof (method) == 'object') && !options) {
                    //initialize
                    options = method;
                    method = u;
                    options = $.extend({}, {
                        format: '',
                        validateNumber: true,
                        attributes: u
                    }, options);

                    var ibf = {
                        inputs: [],
                        inputAttributes: [],
                        originalFormat: '',
                        _keyup: function (field, length, nextField) {
                            field = $(field);
                            var value = field.val();
                            //Check that the phone number is a valid number or automatically "backspace" for them
                            if (options.validateNumber && isNaN(value)) {
                                if (value.length) {
                                    field.val(value.substr(0, value.length - 1));
                                } else { field.val(''); }
                            }
                            //Focus the next element if the length requirement is satisfied
                            if (nextField && length > 0 && field.val().length == length) {
                                $(nextField).focus();
                            }
                        },
                        setValue: function (container, value) {
                            ///	<summary>Sets the phone number</summary>
                            ///	<returns type="void" />
                            ///	<param name="phoneNumber" type="String">The phone number to set (only numbers).</param>
                            if (value) {
                                var t = this, i, currentPosition = 0, length, input;
                                for (i = 0; i < t.inputs.length; i++) {
                                    input = $('#' + t.inputs[i]);
                                    if (i == t.inputs.length - 1) {
                                        input.val(value.substr(currentPosition));
                                    } else {
                                        length = parseInt(input.attr('maxlength'));
                                        if (value.length > (currentPosition + length)) {
                                            input.val(value.substr(currentPosition, length));
                                        }
                                    }
                                    currentPosition += length;
                                }
                            }
                            return container;
                        },
                        getValue: function (container, format) {
                            ///	<summary>Gets the phone number by using the specified format</summary>
                            ///	<returns type="string" />
                            ///	<param name="format" type="String">The format expected (i.e. ({0}) {1} - {2} x{3}).</param>
                            var t = this, i, token;
                            if (!format) {
                                token = new StringBuilder();
                                for (i = 0; i < t.inputs.length; i++) {
                                    token.append('{').append(i).append('}');
                                }
                                format = token.toString();
                            }
                            for (i = 0; i < t.inputs.length; i++) {
                                token = '{' + i + '}';
                                if (format.contains(token)) {
                                    format = format.replace(token, $('#' + t.inputs[i]).val());
                                }
                            }
                            return format;
                        },
                        destroy: function (container) {
                            $.each(this.inputs, function (i, item) {
                                $('#' + item).remove();
                            });
                            return container.html(container.html().replace(this.originalFormat.replace(/\{\d+\}/g, ''), ''));
                        }
                    }, i, attribute;

                    ibf.originalFormat = options.format;

                    tokens = options.format.match(/\{\d+\}/g);

                    for (i = 0; i < tokens.length; i++) {
                        if (tokens[i].replace(/\D/g, '') != i) {
                            throw 'Tokens are not in order, please check the phone format.';
                        }
                        var length = 0, size = 0, id = '', css = {};

                        //Get all of the user defined options for this input (either in an object or if it's just a value, use it for length)
                        if (options.attributes) {
                            attribute = options.attributes[i];
                            var type = typeof (attribute);
                            switch (type.toLowerCase()) {
                                case 'number':
                                    length = attribute;
                                    break;
                                case 'string':
                                    length = parseInt(attribute);
                                    break;
                                case 'object':
                                    if (attribute.id) {
                                        id = attribute.id;
                                    }
                                    if (attribute.length) {
                                        length = attribute.length;
                                    }
                                    if (attribute.size) {
                                        size = attribute.size;
                                    }
                                    if (attribute.css) {
                                        css = attribute.css;
                                    }
                                    break;
                            }
                        }
                        id = id || 'input' + newGuid();
                        if (i < tokens.length - 1)
                            length = length || 3;

                        //Create the new element and add it to the string
                        ibf.inputs.push(id);
                        options.format = options.format.replace(tokens[i], new StringBuilder('<input type="text" id="').append(id).append('" ').append(length ? 'maxlength="' + length + '" ' : '').append(size > 0 ? 'size="' + size + '" ' : '').append('/>').toString());
                        ibf.inputAttributes.push({ id: id, css: css });
                    }

                    this.append(options.format).each(function () {
                        if (!$(this).data(pluginName))
                            $(this).data(pluginName, ibf);
                    });

                    if (ibf.inputs.length > 1) {
                        //Handle all of the keyups for each element and add css
                        var inputs = ibf.inputAttributes, input, ignoreKeys = [8 /*backspace*/, 9 /*tab*/, 13 /*enter*/, 16 /*shift*/, 17 /*ctrl*/, 18 /*alt*/, 19 /*pause*/, 20 /*caps*/, 27 /*esc*/, 33 /*page up*/, 34 /*page down*/, 35 /*end*/, 36 /*home*/, 37 /*left*/, 38 /*up*/, 39 /*right*/, 40 /*down*/, 45 /*insert*/, 46 /*delete*/, 91 /*left windows*/, 92 /*right windows*/, 144 /*num lock*/];
                        for (i = 0; i < inputs.length; i++) {
                            input = $('#' + inputs[i].id).css(inputs[i].css);
                            //Use the numeric plugin if it is loaded http://www.texotela.co.uk/code/jquery/numeric/ 
                            if (options.validateNumber && input.numeric)
                                input.numeric();
                            if (input.attr('maxlength')) {
                                input.keyup(function (e) {
                                    if (!ignoreKeys.contains(e.keyCode)) {
                                        var t = this, index = $.inArray(t.id, inputs.select('id')), length = $(t).attr('maxlength');
                                        ibf._keyup(t, length ? parseInt(length) : 0, inputs.length - 1 > index ? $('#' + inputs[index + 1].id) : null);
                                    }
                                });
                            }
                        }
                    }
                } else {
                    //method call
                    var p = this.data(pluginName);
                    if (method == 'destroy')
                        this.removeData(pluginName);
                    if (p && p[method] && typeof (p[method]) == 'function')
                        return p[method].apply(p, [this].concat(ap.splice.call(arguments, 1, arguments.length - 1)));
                }

                return this;
            }
        }
        if (!jp.rotator) {
            jp.rotator = function (method, options) {
                ///	<summary>Adds automatic rotation to the elements of a container</summary>
                ///	<returns type="jQuery" />
                ///	<param name="items" type="Object">The array of items to rotate, or a selector (context is this).</param>
                ///	<param name="interval" type="Number">The number of milliseconds to wait between rotations (default 10000).</param>
                ///	<param name="fadeSpeed" type="Object">The number of milliseconds to fade in and out, or 'slow', 'normal', or 'fast' (default 'fast').</param>
                var pluginName = 'rotator', navControls;
                if ((method === u || typeof (method) == 'object') && !options) {
                    //initialize
                    options = method;
                    method = u;
                    options = $.extend({}, {
                        items: u,
                        delay: 10000,
                        fadeSpeed: 'fast',
                        createNavigatorControls: true,
                        rotateOnNavigation: true,
                        navigatorDelay: 10000
                    }, options);

                    var rotator = {
                        currentItem: 0,
                        interval: u,
                        navigatorControls: u,
                        rotate: function () {
                            ///	<summary>Rotates through the elements</summary>
                            ///	<returns type="void" />
                            ///	<param name="t" type="Object">The rotator.</param>
                            var t = this, options = t.options;
                            if (t.options.items !== u && t.options.items.length) {
                                if (t.options.fadeSpeed === 0 || t.options.fadeSpeed === false) {
                                    $(t.options.items[t.currentItem]).hide();
                                    if (++t.currentItem == t.options.items.length) {
                                        t.currentItem = 0;
                                    }
                                    $(t.options.items[t.currentItem]).show();
                                    $('.rotatorNavControl.current', navControls).removeClass('current');
                                    $('.rotatorNavControl:eq(' + t.currentItem + ')', navControls).addClass('current');
                                }
                                $(t.options.items[t.currentItem]).fadeOut(t.options.fadeSpeed, function () {
                                    if (++t.currentItem == t.options.items.length) {
                                        t.currentItem = 0;
                                    }
                                    $(t.options.items[t.currentItem]).fadeIn(t.options.fadeSpeed, function () {
                                        $('.rotatorNavControl.current', navControls).removeClass('current');
                                        $('.rotatorNavControl:eq(' + t.currentItem + ')', navControls).addClass('current');
                                    });
                                });
                            }
                            if (!t.interval) {
                                t.interval = w.setInterval(function () { t.rotate.call(t); }, t.options.delay);
                            }
                        },
                        stop: function (container) {
                            ///	<summary>Stops the rotator</summary>
                            ///	<returns type="void" />
                            ///	<param name="t" type="Object">The rotator.</param>
                            w.clearInterval(this.interval);
                            return container;
                        },
                        pause: function (container, delay) {
                            ///	<summary>Pauses the rotator for a certain time</summary>
                            ///	<returns type="void" />
                            ///	<param name="t" type="Object">The rotator.</param>
                            ///	<param name="delay" type="Number">The time in milliseconds to wait before resuming rotation</param>
                            var t = this;
                            w.clearInterval(t.interval);
                            t.interval = u;
                            w.setTimeout(function () { t.rotate.call(t); }, delay);
                        },
                        destroy: function () {
                            var t = this;
                            $.each(t.options.items, function (i, item) {
                                $(item).show();
                            });
                            w.clearInterval(t.interval);
                            t.navigatorControls.remove();
                        }
                    }, t = this;

                    rotator.options = options;

                    if (options.items === u) {
                        options.items = t.children().toArray();
                    }
                    if (typeof (options.items) == 'string') {
                        options.items = $(options.items, t).toArray();
                    }
                    if (options.items instanceof $) {
                        options.items = options.items.toArray();
                    }
                    if (options.items.constructor !== Array) {
                        throw 'Items must be an array or selector';
                    }

                    if (options.createNavigatorControls) {
                        //Add anchor tags for all of the items that we're rotating through and stop on that item if they click on the navigator control
                        navControls = $('<div class="rotatorNavControls"></div>');
                        var i = 1;
                        for (i; i <= options.items.length; i++) {
                            $('<a href="javascript:void(0);" class="rotatorNavControl"><span>' + i + '</span></a>').appendTo(navControls).click(function (e) {
                                e.preventDefault();
                                w.clearInterval(rotator.interval);
                                rotator.currentItem = parseInt(this.id.replace(/\D/g, '')) - 1;
                                $.each(rotator.options.items, function (i, item) {
                                    $(item).hide();
                                });
                                $('.rotatorNavControl.current', navControls).removeClass('current');
                                $(this).addClass('current');
                                $(rotator.options.items[rotator.currentItem]).show();
                                if (rotator.options.rotateOnNavigation) {
                                    rotator.pause(rotator, rotator.options.navigatorDelay);
                                }
                            });
                        }
                        rotator.navigatorControls = navControls;
                        $(t).after(navControls);

                        $('.rotatorNavControl:first', navControls).addClass('current');
                    }

                    $.each(options.items, function (i, item) {
                        $(item).hide();
                    });
                    $(options.items[0]).show();
                    rotator.interval = w.setInterval(function () { rotator.rotate.call(rotator) }, options.delay);

                    return this.each(function () {
                        if (!$(this).data(pluginName))
                            $(this).data(pluginName, rotator);
                    });
                } else {
                    //method call
                    var p = this.data(pluginName);
                    if (method == 'destroy')
                        this.removeData(pluginName);
                    if (p && p[method] && typeof (p[method]) == 'function')
                        return p[method].apply(p, [this].concat(ap.splice.call(arguments, 1, arguments.length - 1)));
                }

                return this;
            }
        }
        if (!jp.tristateCheckbox) {
            jp.tristateCheckbox = function (method, options) {
                var pluginName = 'tristateCheckbox';
                if ((method === u || typeof (method) == 'object') && !options) {
                    //initialize
                    options = method;
                    method = u;
                    options = $.extend({}, {
                        uncheckedImage: '/Resource/Content/Images/tristateCheckbox/unchecked.gif',
                        uncheckedHighlightedImage: '/Resource/Content/Images/tristateCheckbox/unchecked_highlighted.gif',
                        partialImage: '/Resource/Content/Images/tristateCheckbox/intermediate.gif',
                        partialHighlightedImage: '/Resource/Content/Images/tristateCheckbox/intermediate_highlighted.gif',
                        checkedImage: '/Resource/Content/Images/tristateCheckbox/checked.gif',
                        checkedHighlightedImage: '/Resource/Content/Images/tristateCheckbox/checked_highlighted.gif',
                        childrenContainer: false
                    }, options);

                    var unchecked = 0, partiallyChecked = 1, checked = 2,
					label,
					dataKey = 'tristateCheckboxState',
					updateChildren = function (checked) {
					    if (options.childrenContainer) {
					        $(options.childrenContainer).find('input[type="checkbox"]').attr('checked', checked);
					    }
					},
					handleClick = function (e) {
					    var t = img, state = t.data(dataKey), cc = options.childrenContainer, attachedCheckbox = t.prev();
					    if ((cc && state == unchecked) || (!cc && state == partiallyChecked)) {
					        t.attr('src', options.checkedImage).data(dataKey, checked);
					        attachedCheckbox.attr('checked', 'checked');
					        updateChildren(true);
					    } else if ((cc && state == partiallyChecked) || state == checked) {
					        t.attr('src', options.uncheckedImage).data(dataKey, unchecked);
					        attachedCheckbox.removeAttr('checked');
					        updateChildren(false);
					    } else if (!cc && state == unchecked) {
					        t.attr('src', options.partialImage).data(dataKey, partiallyChecked);
					        attachedCheckbox.attr('checked', 'checked');
					    }
					    attachedCheckbox.triggerHandler('click');
					    e.stopPropagation();
					    e.stopImmediatePropagation();
					},
					img = $('<img src="" alt="" class="tristateCheckbox" />').click(handleClick).hover(function () {
					    var t = $(this);
					    switch (t.data(dataKey)) {
					        case unchecked:
					            t.attr('src', options.uncheckedHighlightedImage);
					            break;
					        case partiallyChecked:
					            t.attr('src', options.partialHighlightedImage);
					            break;
					        case checked:
					            t.attr('src', options.checkedHighlightedImage);
					            break;
					    }
					}, function () {
					    var t = $(this);
					    switch (t.data('tristateCheckboxState')) {
					        case unchecked:
					            t.attr('src', options.uncheckedImage);
					            break;
					        case partiallyChecked:
					            t.attr('src', options.partialImage);
					            break;
					        case checked:
					            t.attr('src', options.checkedImage);
					            break;
					    }
					}),
					tristateCheckbox = {
					    setValue: function (container, value) {
					        if (value < 0 || value > 2)
					            return;
					        var t = this.img, url, attachedCheckbox = t.prev();
					        if (value == unchecked) {
					            url = options.uncheckedImage;
					            attachedCheckbox.removeAttr('checked');
					        }
					        else if (value == partiallyChecked) {
					            url = options.partialImage;
					            attachedCheckbox.removeAttr('checked');
					        }
					        else if (value == checked) {
					            url = options.checkedImage;
					            attachedCheckbox.attr('checked', 'checked');
					        }
					        t.attr('src', url).data(dataKey, value);
					        attachedCheckbox.triggerHandler('click');
					    },
					    getValue: function () {
					        return this.img.data(dataKey);
					    },
					    destroy: function (container) {
					        container.show().removeData('isTristate');
					        this.img.remove();
					    },
					    img: img
					};

                    if (options.childrenContainer) {
                        var children = $(options.childrenContainer).find('input[type="checkbox"]'), countChildren = function () {
                            var count = 0, i = 0, c;
                            for (i; i < children.length; i++) {
                                c = $(children.get(i));
                                if (c.is(':checked') || (c.data('isTristate') && c.tristateCheckbox('getValue') == checked)) {
                                    ++count;
                                }
                            }

                            if (!count)
                                tristateCheckbox.setValue.apply(tristateCheckbox, [this, 0]);
                            else if (count < children.length)
                                tristateCheckbox.setValue.apply(tristateCheckbox, [this, 1]);
                            else
                                tristateCheckbox.setValue.apply(tristateCheckbox, [this, 2]);
                        };
                        children.click(countChildren);
                        countChildren();
                    } else {
                        img.data(dataKey, $(this).is(':checked') ? 0 : 2).click();
                    }

                    label = $('label[for=' + this.attr('id') + ']');
                    if (label.length) {
                        label.click(handleClick);
                    }

                    return this.data('isTristate', true).hide().after(img).each(function () {
                        if (!$(this).data(pluginName))
                            $(this).data(pluginName, tristateCheckbox);
                    }); ;
                } else {
                    //method call
                    var p = this.data(pluginName);
                    if (method == 'destroy')
                        this.removeData(pluginName);
                    if (p && p[method] && typeof (p[method]) == 'function') {
                        return p[method].apply(p, [this].concat(ap.splice.call(arguments, 1, arguments.length - 1)));
                    }
                }

                return this;
            }
        }
        if (!jp.linkCover) {
            jp.linkCover = function (link) {
                link = link || 'a:first';
                return this.each(function () {

                    var t = $(this), target = $(link, t).clone(true);

                    var mouseouts = t.data('events') && t.data('events').mouseout;
                    if (mouseouts) {
                        $.each(mouseouts, function (i, fn) {
                            target.mouseout(function (e) {
                                fn.call(t, e);
                            });
                        });
                        delete t.data('events').mouseout;
                    }

                    var mouseovers = t.data('events') && t.data('events').mouseover;
                    if (mouseovers) {
                        $.each(mouseovers, function (i, fn) {
                            target.mouseover(function (e) {
                                fn.call(t, e);
                            });
                        });
                        delete t.data('events').mouseover;
                    }

                    t.mouseover(function () {
                        target.show();
                    });

                    target.click(function () {
                        target.blur();
                    }).mouseout(function (e) {
                        target.hide();
                    }).css({
                        position: 'absolute',
                        top: t.offset().top,
                        left: t.offset().left,
                        /* IE requires background to be set */
                        backgroundColor: '#FFF',
                        display: 'none',
                        opacity: 0,
                        width: t.outerWidth(),
                        height: t.outerHeight(),
                        padding: 0
                    }).appendTo('body');
                });
            };
        }
    }
})(window, document, jQuery);