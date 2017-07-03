// Dependencies:  JQuery, JQuery Unobtrusive Validation, & KnockoutJS
// The following lines enable VS Intellisense for jquery & knockout
// and the triple slash IS required.
/// <reference path="~/Scripts/jquery-1.7.2-vsdoc.js" />
/// <reference path="~/Scripts/jquery.validate.unobtrusive.js" />
/// <reference path="~/Scripts/knockout-2.1.0.debug.js" />
/// <reference path="~/Scripts/knockout.mapping-latest.debug.js" />

// Example Usage
// var editor = new addressEditor({ country:'' });

// NOTES: You will see the use of prototype all over the place.  Which is why
// you will see .call & .apply all over the place.  This is a best practice for performance
// purposes.  

// Prototype functions are static functions.  However you will see the use of
// .call or .apply to those calls.  .call & .apply set the object to reference
// within those functions for the 'this' keyword.

// You will also see creating 'refThis' variable and assigning the this keyword
// value to it.  The refThis will be included in a function which may not be called
// until a later time.  When that function is call the this keyword will not reflect
// the object at the time the function creating function was called

(function (rootObj) {
    var isUndefined = function (evalObj) { return typeof (evalObj) === 'undefined'; };

    if (isUndefined(rootObj.ns)) rootObj.ns = {};
    if (isUndefined(rootObj.ns.addressEditor)) {

        var addressEditor = function (settings) {
            try {
                var refThis = this;

                // init stack
                _.initSettings.call(refThis, settings);
                _.registerWithEditors.call(refThis, settings.editorID);

                _.initViewModel.call(refThis);
                _.initEvents.call(refThis);
                _.initFuncs.call(refThis);

                // Apply binding and then register this editor with global list of editors
                _.initTemplate.call(refThis);

            } catch (ex) {
                alert(this.settings.termTranslations.initError + '\r\n' + ex);
            }
        };

        addressEditor.prototype.consts = {
            boolOverride:
            {
                inherit: 0,
                yes: 1,
                no: 2
            },
            cleansingMode:
            {
                inherit: 0,
                none: 1,
                propose: 2,
                required: 3
            }
        };

        var consts = addressEditor.prototype.consts;

        addressEditor.prototype._ = {

            initSettings: function (settings) {
                // create a new object {}, deep copy/merge static settings and editor settings into it
                this.settings = $.extend(true, {}, _.defaults.settings, settings);
                this.settings.countryTemplateAjaxLink = $('#' + this.settings.templateDivID + 'CountryTemplateLink').attr('href');
                this.settings.scrubAddressAjaxLink = $('#' + this.settings.templateDivID + 'ScrubAddressLink').attr('href');
                this.settings.postalCodeLookupAjaxLink = $('#' + this.settings.templateDivID + 'PostalCodeLookupLink').attr('href');
            },


            defaults:
            {
                settings:
                {
                    address: {},
                    editorID: 'globalAddressEditor',
                    templateDivID: 'globalAddressEditor',
                    termTranslations:
                    {
                        initError: 'An error has occurred while attempting to '
                        + 'initialize an address entry section of this page.\r\n'
                    },
                    countries: [],
                    countryOverrideSettings: {},
                    /// <summary>
                    /// Enables address scrubbing, this property changes per country.
                    /// Upon country changing the template call shall provide 
                    /// the globaAddressEditor ID as a a parameter to specify 
                    /// this instance of the address editor so that it may 
                    /// update this setting, the model, and the postalCodeLookup setting
                    /// </summary>
                    cleansingMode: consts.cleansingMode.none, // 'required', 'propose'
                    /// <summary>
                    /// Enables postal code lookup upon entering a postal code.
                    /// this property changes per market.  Upon triggering the 
                    /// application shall call an AJAX service to obtain updated 
                    /// model information providing the existing model and 
                    /// rebind the UI.  Because country codes will not change 
                    /// there is no need to refresh the template.
                    ///</summary>
                    allowPostalCodeLookup: consts.boolOverride.no
                },

                country:
                {
                    countryCode: '',
                    countryID: '',
                    countryName: ''
                },

                // countryCode='' is the common overrides
                // countryCode='...' is a country specific override
                countryOverrideSetting:
                {
                    countryCode: '',
                    allowPostalCodeLookup: consts.boolOverride.inherit,
                    cleansingMode: consts.cleansingMode.inherit,
                    fieldOverrides: {}
                },

                countryFieldOverrideSetting:
                {
                    fieldEnabled: 0,
                    fieldIncluded: 0,
                    fieldName: '',
                    label: ''
                },

                viewModel: {
                    showField: function (fieldName, showByDefault) {

                        var editor = ns.addressEditors[this.editorName()];
                        var address = editor.viewModel.address;
                        var countryCode = address.CountryCode();

                        var commonSettings = editor.settings.countryOverrideSettings[''];
                        var commonFieldSetting = commonSettings && commonSettings.fieldOverrides[fieldName] !== undefined ? commonSettings.fieldOverrides[fieldName] : null;

                        var countrySpecificSettings = editor.settings.countryOverrideSettings[countryCode];
                        var countrySpecificFieldSetting = countrySpecificSettings && countrySpecificSettings.fieldOverrides[fieldName] !== undefined ? countrySpecificSettings.fieldOverrides[fieldName] : null;

                        return countrySpecificFieldSetting && countrySpecificFieldSetting.fieldIncluded == consts.boolOverride.yes
                            ? true
                            : countrySpecificFieldSetting && countrySpecificFieldSetting.fieldIncluded == consts.boolOverride.no
                            ? false
                            : commonFieldSetting && commonFieldSetting.fieldIncluded == consts.boolOverride.yes
                            ? true
                            : commonFieldSetting && commonFieldSetting.fieldIncluded == consts.boolOverride.no
                            ? false
                            : showByDefault;

                    },
                    enableField: function (fieldName, enableByDefault) {

                        var editor = ns.addressEditors[this.editorName()];
                        var address = editor.viewModel.address;
                        var countryCode = address.CountryCode();

                        var commonSettings = editor.settings.countryOverrideSettings[''];
                        var commonFieldSetting = commonSettings && commonSettings.fieldOverrides[fieldName] !== undefined ? commonSettings.fieldOverrides[fieldName] : null;

                        var countrySpecificSettings = editor.settings.countryOverrideSettings[countryCode];
                        var countrySpecificFieldSetting = countrySpecificSettings && countrySpecificSettings.fieldOverrides[fieldName] !== undefined ? countrySpecificSettings.fieldOverrides[fieldName] : null;

                        return countrySpecificFieldSetting && countrySpecificFieldSetting.fieldEnabled == consts.boolOverride.yes
                            ? true
                            : countrySpecificFieldSetting && countrySpecificFieldSetting.fieldEnabled == consts.boolOverride.no
                            ? false
                            : commonFieldSetting && commonFieldSetting.fieldEnabled == consts.boolOverride.yes
                            ? true
                            : commonFieldSetting && commonFieldSetting.fieldEnabled == consts.boolOverride.no
                            ? false
                            : showByDefault;

                    },
                    selectSuggestion: function (address) {
                        alert('address selected');
                    },
                    cancelCleansePrompt: function () {
                        alert('address selected');
                    },

                    lists: {
                        suggestions: [

                        ],
                        countries: []
                    },

                    address: {
                        AddressID: null,
                        Attention: '',
                        FirstName: '',
                        LastName: '',
                        Address1: '',
                        Address2: '',
                        Address3: '',
                        PostalCode: '',
                        City: '',
                        County: '',
                        State: '',
                        Country: '',
                        CountryCode: '',
                        CountryID: 0,
                        ScrubbedGuid: ''
                    }
                }
            },

            initViewModel: function () {
                var viewModel = $.extend(true, { editorName: this.settings.editorID }, _.defaults.viewModel);
                $.extend(true, viewModel.address, this.settings.address);
                $.extend(true, viewModel.lists.countries, this.settings.countries);
                this.viewModel = ko.mapping.fromJS(viewModel);
            },

            initTemplate: function () {
                this.settings.templateDiv = $('#' + this.settings.templateDivID);
                _.hookupTemplate.apply(this);
            },

            hookupTemplate: function () {
                ko.applyBindings(this.viewModel, $(this.settings.templateDiv)[0]);
            },

            unhookTemplate: function () {
                ko.cleanNode($(this.settings.templateDiv)[0]);
                $(this.settings.templateDiv).validate().resetForm();
            },

            shouldExecutePostalCodeLookup: function () {
                var editor = this;
                var address = editor.viewModel.address;
                var countryCode = address.CountryCode();

                var commonSettings = editor.settings.countryOverrideSettings[''];
                var countrySpecificSettings = editor.settings.countryOverrideSettings[countryCode];


                return countrySpecificSettings && countrySpecificSettings.allowPostalCodeLookup == consts.boolOverride.yes
                            ? true
                            : countrySpecificSettings && countrySpecificSettings.allowPostalCodeLookup == consts.boolOverride.no
                            ? false
                            : commonSettings && commonSettings.allowPostalCodeLookup == consts.boolOverride.yes
                            ? true
                            : commonSettings && commonSettings.allowPostalCodeLookup == consts.boolOverride.no
                            ? false
                            : this.settings.allowPostalCodeLookup == consts.boolOverride.yes
                            ? true
                            : false;
            },

            shouldPromptAddressCleansing: function () {

                var editor = this;
                var address = editor.viewModel.address;
                var countryCode = address.CountryCode();

                var commonSettings = editor.settings.countryOverrideSettings[''];
                var countrySpecificSettings = editor.settings.countryOverrideSettings[countryCode];

                return countrySpecificSettings && countrySpecificSettings.cleansingMode != consts.cleansingMode.none
                            ? true
                            : commonSettings && commonSettings.cleansingMode != consts.cleansingMode.none
                            ? true
                            : this.settings.cleansingMode == consts.cleansingMode.none
                            ? true
                            : false;
            },

            initEvents: function () {
                var refThis = this;
                this.viewModel.address.PostalCode.subscribe(function (newValue) {
                    if (_.shouldExecutePostalCodeLookup.call(refThis)) {
                        _.funcs.executePostalCodeLookup.call(refThis);
                    }
                });

                // create a flag to allow changes made during the event handling
                // of country code changes without re-triggering the event
                var alreadyHandlingCountryChanges = false;

                var countryCodeChange = function () {
                    alreadyHandlingCountryChanges = true;
                    try {

                        var triggeringElement = arguments[arguments.length - 1];
                        // if (!$(triggeringElement).valid()) return;
                        var id = $(triggeringElement)[0].id;

                        var refThis = this;
                        $.post(this.settings.countryTemplateAjaxLink, { model: _.getAddressModel.apply(refThis), clientSideAddressObjID: refThis.settings.editorID }, function (results) {

                            _.unhookTemplate.apply(refThis);
                            $('#' + refThis.settings.templateDivID).html(results);
                            _.hookupTemplate.apply(refThis);

                            $('#' + id).focus();
                        });

                    } finally {
                        alreadyHandlingCountryChanges = false;
                    }
                }
                var asyncCountryCodeChange = function () {
                    var args = $.merge(arguments, [event.srcElement]);
                    _.funcs.asyncFire.call(refThis, countryCodeChange, args);
                };

                this.viewModel.address.CountryCode.subscribe(asyncCountryCodeChange);
                this.viewModel.address.CountryID.subscribe(asyncCountryCodeChange);
                this.viewModel.address.Country.subscribe(asyncCountryCodeChange);
            },

            registerWithEditors: function (editorID) {
                rootObj.ns.addressEditors[editorID] = this;
            },

            isPrimitive: function (value) {
                return !isUndefined(value)
                && value !== null
                && typeof (value) !== 'object';
            },

            getValue: function (sourceObj, prop, optionalDefaultValue) {
                var result = !isUndefined(sourceObj) && !_.isPrimitive(sourceObj) && _.isPrimitive(sourceObj[prop])
                ? sourceObj[prop]
                : !isUndefined(optionalDefaultValue) ? optionalDefaultValue : '';
                return result;
            },

            getAddressModel: function () {
                var model = ko.mapping.toJS(this.viewModel);
                return model.address;
            },

            initFuncs: function () {
                var refThis = this;
                this.promptAddressCleansing = function () { _.funcs.asyncFire.call(this, _.funcs.promptAddressCleansing, arguments); };
                this.getAddress = function () { _.funcs.asyncFire.call(this, _.funcs.getAddress, arguments); };
            },

            funcs:
            {
                asyncFire: function (func, args) {
                    var refThis = this;
                    var refArgs = args;
                    window.setTimeout(function () { func.apply(refThis, refArgs); }, 0);
                },
                promptAddressCleansing: function (successCallback, failCallback) {
                    // Still in progress by Brent VanderMeide
                    // for now forcing the issue

                    this.viewModel.address.ScrubbedGuid = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
                        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
                        return v.toString(16);
                    });

                    successCallback(_.getAddressModel.apply(this));
                },
                getAddress: function (successCallback, failCallback) {
                    if (_.shouldPromptAddressCleansing.call(this))
                        this.promptAddressCleansing(successCallback, failCallback);
                    else
                        successCallback(_.getAddressModel.apply(this));
                },
                executePostalCodeLookup: function () {
                    // Still in progress by Brent VanderMeide
                }
            }
        };

        // create easy accessor to static prototype functions
        _ = addressEditor.prototype._;
        rootObj.ns.addressEditor = addressEditor;
        rootObj.ns.addressEditors = {};
    };


})(window);