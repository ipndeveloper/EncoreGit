// Dependencies:  JQuery, JQuery Unobtrusive Validation, & KnockoutJS
// The following lines enable VS Intellisense for jquery & knockout
// and the triple slash IS required.
/// <reference path="http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.7.2-vsdoc.js" />
/// <reference path="http://dev.iceburg.net/jquery/jqModal/jqModal.js" />
/// <reference path="~/Scripts/jquery.validate.unobtrusive.js" />
/// <reference path="~/Scripts/knockout-2.1.0.debug.js" />
/// <reference path="~/Scripts/knockout.mapping-latest.debug.js" />

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
				_.initCleanseWindow.call(refThis);
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
            },
			cleanseResult:
            {
            	Unknown: 0,
            	Error: 1,
            	Success: 2
            }
		};

		var consts = addressEditor.prototype.consts;

		addressEditor.prototype._ = {

			initSettings: function (settings) {
				// create a new object {}, deep copy/merge static settings and editor settings into it
				this.settings = $.extend(true, {}, _.defaults.settings, settings);
				this.settings.templateDivIndex = '';
				this.settings.countryTemplateAjaxLink = $('#' + this.settings.templateDivID + 'CountryTemplateLink').attr('href');
				this.settings.scrubAddressAjaxLink = $('#' + this.settings.templateDivID + 'ScrubAddressLink').attr('href');
				this.settings.postalCodeLookupAjaxLink = $('#' + this.settings.templateDivID + 'PostalCodeLookupLink').attr('href');

			},

			initCleanseWindow: function () {
				this.cleanseWindow = $('#' + this.settings.templateDivID + this.settings.templateDivIndex + 'CleanseWindow')
				//  ;  // If this line is commented out then I forgot to comment the one below out before checking in
                    .attr('style', 'display: none; position: fixed; top: 17%;left: 50%;margin-left: -300px; width: 600px;');



				this.cleanseWindow.jqm({

					modal: true
                    , trigger: false
                    , onShow: function (h) {
                    	h.w.fadeIn();
                    }
				});

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
                        + 'initialize an address entry section of this page.\r\n',
                    	commError: 'An error occurred while connecting to the server '
                        + 'to attempt to verify your address'
                    },
                	countries: [],
                	countryStateProvinces: [],
                	//stateProvinceCities: [],
                	phoneTypes: [],
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
            			if (editor.viewModel.cleanseInProgress()) return false;

            			switch (fieldName) {
            				case 'CountryCode':
            					if (
                                    editor.viewModel.postalCodeLookupInProgress()
                                    || editor.viewModel.showingCleanseWindow()
                                )
            						return false;
            				case 'City':
            				case 'County':
            				case 'State':
            				case 'StateID':
            				case 'StateCode':
            				case 'StateProvince':
            				case 'StateProvinceID':
            				case 'StateProvinceCode':
            					if (editor.viewModel.postalCodeLookupInProgress()) return false;
            					break;
            				default:
            					break;
            			}

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
                            : enableByDefault;

            		},

            		postalCodeLookupSupported: function () {
            			var editor = ns.addressEditors[this.editorName()];
            			return editor.settings.allowPostalCodeLookup == consts.boolOverride.yes;
            		},

            		postalCodeLookupInProgress: false,

            		postalCodeLookupErrorOccurred: false,

            		postalCodeLookupError: '',

            		postalCodeLookupEmpty: false,

            		cleanseInProgress: false,

            		showingCleanseWindow: false,

            		selectSuggestion: function (data, root) {
            			return function () {
            				var editor = ns.addressEditors[root.editorName()];
            				var vm = editor.viewModel;
            				vm.selectedSuggestion(data);
            			};
            		},

            		selectedSuggestion: null,

            		cancelCleansePrompt: function () {
            			this.cancelCleansePromptTriggered(true);
            		},

            		cancelCleansePromptTriggered: false,

            		confirmCleansePromptTriggered: false,

            		retryCleanseTriggered: false,

            		cleanseErrorOccurred: false,

            		cleanseError: '',

            		canRetryCleanse: true,

            		canContinueWithProvidedAddress: function () {
            			var editor = ns.addressEditors[this.editorName()];
            			return _.getCleansingMode.call(editor) != consts.cleansingMode.required;
            		},

            		retryCleanse: function () {
            			this.retryCleanseTriggered(true);
            		},

            		continueWithProvidedAddress: function () {
            			var editor = ns.addressEditors[this.editorName()];
            			if ($(editor.cleanseWindow).find('form').valid()) {
            				this.confirmCleansePromptTriggered(true);
            			}
            		},

            		lists: {
            			suggestions: [

                        ],
            			countries: [],
            			phoneTypes: [],
            			countryStateProvinces: [],
            			//stateProvinceCities: [],
            			postalCodeCities: [],
            			postalCodeCounties: [],
            			postalCodeStateProvinces: []
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
            			StateProvince: '',
            			StateProvinceID: '',
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
				$.extend(true, viewModel.lists.countryStateProvinces, this.settings.countryStateProvinces);
				//$.extend(true, viewModel.lists.stateProvinceCities, this.settings.stateProvinceCities);
				$.extend(true, viewModel.lists.phoneTypes, this.settings.phoneTypes);
				this.viewModel = ko.mapping.fromJS(viewModel);
			},

			initTemplate: function () {
				this.settings.templateDiv = $('#' + this.settings.templateDivID + this.settings.templateDivIndex);
				_.hookupTemplate.apply(this);
			},

			hookupTemplate: function () {
				var refThis = this;
				window.setTimeout(function () {
					var forms = $(refThis.settings.templateDiv).find('form');
					forms.validate({
						errorPlacement: function (error, element) {
							$(error).fadeIn('slow');
						}
					});
					forms.removeData('validator');
					forms.removeData('unobtrusiveValidation');
					$.validator.unobtrusive.parse(forms);
					ko.applyBindings(refThis.viewModel, $(refThis.settings.templateDiv)[0]);
					//}, 1000 * 30); // pause to allow dev to modify bindings during debugging
				}, 0);
			},

			unhookTemplate: function () {
				ko.cleanNode($(this.settings.templateDiv)[0]);
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

			getCleansingMode: function () {

				var editor = this;
				var address = editor.viewModel.address;
				var countryCode = address.CountryCode();

				var commonSettings = editor.settings.countryOverrideSettings[''];
				var countrySpecificSettings = editor.settings.countryOverrideSettings[countryCode];


				return countrySpecificSettings && countrySpecificSettings.cleansingMode != consts.cleansingMode.inherit
                    ? countrySpecificSettings.cleansingMode
                    : commonSettings && commonSettings.cleansingMode != consts.cleansingMode.inherit
                    ? commonSettings.cleansingMode
                    : consts.cleansingMode.none;
			},

			initEvents: function () {
				var refThis = this;

				// create a flag to allow changes made during the event handling
				// of country code changes without re-triggering the event
				var alreadyHandlingCountryChanges = false;

				var postalCodeChange = function (newValue) {
					if (_.shouldExecutePostalCodeLookup.call(refThis)) {
						_.funcs.executePostalCodeLookup.call(refThis, arguments);
					}
				};

				var asyncPostalCodeChange = function () {
					if (refThis.viewModel.postalCodeLookupInProgress()) return;
					// var args = $.merge(arguments, [(event != null && event.srcElement != null) ? event.srcElement : null]);
					var args = arguments;
					_.funcs.asyncFire.call(refThis, postalCodeChange, args);
				};

				this.viewModel.address.PostalCode.subscribe(asyncPostalCodeChange);

				var countryCodeChange = function (whichField) {

					// TODO : Determine which value was changes and then 
					// cross reference update the other country related properties.
					// ex update name and ID if code was changed
					// or update code and ID if name was changed
					// or update name and code if ID was changed

					try {
						// var triggeringElement = arguments[arguments.length - 1];
						// if (!$(triggeringElement).valid()) return;
						// var id = $(triggeringElement)[0].id;

						var refThis = this;

						if (refThis.settings.templateDivIndex == '') refThis.settings.templateDivIndex = 0;
						refThis.settings.templateDivIndex = refThis.settings.templateDivIndex + 1;
						var oldDiv = refThis.settings.templateDiv;
						$('<div style="display:none;" id="' + this.settings.templateDivID + this.settings.templateDivIndex + '"></div>').insertAfter(refThis.settings.templateDiv);

						$.post(this.settings.countryTemplateAjaxLink, { uiModel: _.getAddressModel.apply(refThis), clientSideAddressObjID: refThis.settings.editorID, clientSideAddressHtmlID: refThis.settings.templateDivID + refThis.settings.templateDivIndex }, function (results) {

							_.unhookTemplate.apply(refThis);
							$('#' + refThis.settings.templateDivID + refThis.settings.templateDivIndex).html(results);
							_.initTemplate.apply(refThis);
							_.initCleanseWindow.apply(refThis);
							// _.hookupTemplate.apply(refThis);
							// $('#' + id).focus();
							oldDiv.remove();
							$(refThis.settings.templateDiv).show();
						});

					} finally {
						alreadyHandlingCountryChanges = false;
					}
				};
				var asyncCountryCodeChange = function () {
					if (alreadyHandlingCountryChanges) return;
					alreadyHandlingCountryChanges = true;
					// var args = $.merge(arguments, [(event != null && event.srcElement != null) ? event.srcElement : null]);
					var args = arguments;
					_.funcs.asyncFire.call(refThis, countryCodeChange, args);
				};
				var asynxCountryCodeChange_Code = function () { asyncCountryCodeChange.apply(refThis, ['code']); }
				var asynxCountryCodeChange_ID = function () { asyncCountryCodeChange.apply(refThis, ['id']); }
				var asynxCountryCodeChange_Name = function () { asyncCountryCodeChange.apply(refThis, ['name']); }

				this.viewModel.address.CountryCode.subscribe(asynxCountryCodeChange_Code);
				this.viewModel.address.CountryID.subscribe(asynxCountryCodeChange_ID);
				this.viewModel.address.Country.subscribe(asynxCountryCodeChange_Name);
			},

			registerWithEditors: function (editorID) {
				rootObj.ns.addressEditors[editorID] = this;
			},

			isFunction: function (value) {
				return !isUndefined(value)
                && value !== null
                && typeof (value) === 'function';
			},

			isPrimitive: function (value) {
				return !isUndefined(value)
                && value !== null
                && typeof (value) !== 'object'
                && typeof (value) !== 'function';
			},

			gettext: function (sourceObj, prop, optionalDefaultValue) {
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
				this.promptAddressCleansing = function () { _.funcs.asyncFire.call(refThis, _.funcs.promptAddressCleansing, arguments); };
				this.getAddress = function () { _.funcs.asyncFire.call(refThis, _.funcs.getAddress, arguments); };
				this.valid = function () {
					return $(this.settings.templateDiv).find('form').valid();
				};
			},

			funcs:
            {
            	asyncFire: function (func, args) {
            		var refThis = this;
            		var refArgs = args;
            		window.setTimeout(function () { func.apply(refThis, refArgs); }, 0);
            	},
            	promptAddressCleansing: function (successCallback, failCallback) {
            		var refThis = this;
            		var confirm, cancel, select, retryCleanse;

            		refThis.viewModel.cleanseInProgress(true);

            		this.viewModel.showingCleanseWindow(true);

            		var clearSubscribers = function () {
            			if (confirm != null) confirm.dispose();
            			if (cancel != null) cancel.dispose();
            			if (select != null) select.dispose();

            			refThis.viewModel.confirmCleansePromptTriggered(false);
            			refThis.viewModel.cancelCleansePromptTriggered(false);
            		};

            		select = refThis.viewModel.selectedSuggestion.subscribe(function () {
            			var newVal = refThis.viewModel.selectedSuggestion();
            			if (newVal == null) return;
            			try {
            				var model = ko.mapping.toJS(newVal.address);

            				clearSubscribers();
            				refThis.cleanseWindow.jqmHide();
            				refThis.viewModel.showingCleanseWindow(false);
            				if (_.isFunction(successCallback))
            					successCallback(model);


            			} finally {
            				refThis.selectedSuggestion(null);
            			}
            		});

            		confirm = refThis.viewModel.confirmCleansePromptTriggered.subscribe(function () {
            			clearSubscribers();
            			refThis.cleanseWindow.jqmHide();
            			refThis.viewModel.showingCleanseWindow(false);
            			if (_.isFunction(successCallback))
            				successCallback(_.getAddressModel.apply(refThis));
            		});

            		cancel = refThis.viewModel.cancelCleansePromptTriggered.subscribe(function () {
            			clearSubscribers();
            			refThis.cleanseWindow.jqmHide();
            			refThis.viewModel.showingCleanseWindow(false);
            			if (_.isFunction(failCallback))
            				failCallback();
            		});

            		retryCleanse = refThis.viewModel.retryCleanseTriggered.subscribe(function (newValue) {
            			if (newValue == false) return;

            			refThis.viewModel.cleanseInProgress(true);
            			refThis.viewModel.retryCleanseTriggered(false);
            			refThis.viewModel.lists.suggestions.removeAll();
            			refThis.viewModel.cleanseErrorOccurred(false);
            			refThis.viewModel.cleanseError('');

            			$.ajax({
            				url: refThis.settings.scrubAddressAjaxLink,
            				data: { fromAddress: _.getAddressModel.apply(refThis) },
            				success: function (results) {
            					try {
            						if (results.Status == consts.cleanseResult.Success) {
            							refThis.viewModel.lists.suggestions.removeAll();

            							for (var i = 0; i < results.ValidAddresses.length; i++) {
            								var item = ko.mapping.fromJS({
            									address: $.extend(true, {},
            										{
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
            											StateProvince: '',
            											StateProvinceID: '',
            											Country: '',
            											CountryCode: '',
            											CountryID: 0,
            											ScrubbedGuid: ''
            										}, refThis.settings.address, results.ValidAddresses[i])
            								});

            								if (results.ValidAddresses.length == 1) {
            									var model = ko.mapping.toJS(item.address);

            									clearSubscribers();
            									refThis.cleanseWindow.jqmHide();
            									refThis.viewModel.showingCleanseWindow(false);
            									if (_.isFunction(successCallback))
            										successCallback(model);
            								}
            								else {
            									refThis.viewModel.lists.suggestions.push(item);
            								}
            							}
            						} else {
            							refThis.viewModel.cleanseErrorOccurred(true);
            							refThis.viewModel.cleanseError(results.Message);
            						}
            					} catch (ex) { }
            				},
            				error: function (detail, status, ex) {
            					try {
            						refThis.viewModel.cleanseErrorOccurred(true);
            						refThis.viewModel.cleanseError(refThis.settings.termTranslations.commError);
            					} catch (ex) { }
            				},
            				complete: function () {
            					refThis.viewModel.cleanseInProgress(false);
            				}
            			});
            		});

            		this.viewModel.retryCleanse.apply(refThis.viewModel);
            		this.cleanseWindow.jqmShow();
            	},
            	getAddress: function (successCallback, failCallback) {
            		if (!$(this.settings.templateDiv).find('form').valid()) {
            			failCallback();
            			return;
            		}

            		if (_.shouldPromptAddressCleansing.call(this))
            			this.promptAddressCleansing(successCallback, failCallback);
            		else
            			successCallback(_.getAddressModel.apply(this));
            	},

            	executePostalCodeLookup: function () {

            		//            		var srcElement = arguments[arguments.length - 1];
            		//            		if (!$(srcElement[1]).valid()) return;
            		var refThis = this;
            		var countryCode = this.viewModel.address.CountryCode();
            		var postalCode = this.viewModel.address.PostalCode();

            		// Clear fields
            		refThis.viewModel.postalCodeLookupEmpty(false);
            		refThis.viewModel.postalCodeLookupErrorOccurred(false);
            		refThis.viewModel.postalCodeLookupError('');

            		$.ajax({
            			url: refThis.settings.postalCodeLookupAjaxLink,
            			data: { countryCode: countryCode, postalCode: postalCode },
            			success: function (results) {
            				try {
            					refThis.viewModel.lists.postalCodeCities.removeAll();
            					refThis.viewModel.lists.postalCodeCounties.removeAll();
            					refThis.viewModel.lists.postalCodeStateProvinces.removeAll();

            					if (results.length > 0) {
            						for (var i = 0; i < results.length; i++) {
            							var foundCity = false;
            							var foundCounty = false;
            							var foundStateProvince = false;

            							var existingCities = refThis.viewModel.lists.postalCodeCities();
            							var existingCounties = refThis.viewModel.lists.postalCodeCounties();
            							var existingStateProvinces = refThis.viewModel.lists.postalCodeStateProvinces();

            							for (var ci = 0; ci < existingCities.length && !foundCity; ci++)
            								foundCity = existingCities[ci].City == results[i].city ? true : foundCity;
            							if (!foundCity) refThis.viewModel.lists.postalCodeCities.push({ City: results[i].city });

            							for (var ci = 0; ci < existingCounties.length && !foundCounty; ci++)
            								foundCounty = existingCounties[ci].County == results[i].county ? true : foundCounty;
            							if (!foundCounty) refThis.viewModel.lists.postalCodeCounties.push({ County: results[i].county });

            							for (var si = 0; si < existingStateProvinces.length && !foundStateProvince; si++)
            								foundStateProvince = existingStateProvinces[si].StateProvince == results[i].state && existingStateProvinces[si].StateProvinceID == results[i].stateId ? true : foundStateProvince;
            							if (!foundStateProvince) refThis.viewModel.lists.postalCodeStateProvinces.push({ StateProvince: results[i].state, StateAbbreviation: results[i].state, StateProvinceID: results[i].stateId });
            						}

            						refThis.viewModel.postalCodeLookupEmpty(false);
            						refThis.viewModel.address.City(results[0].city);
            						refThis.viewModel.address.County(results[0].county);
            						refThis.viewModel.address.StateProvince(results[0].state);
            						refThis.viewModel.address.StateProvinceID(results[0].stateId);
            					} else {
            						refThis.viewModel.postalCodeLookupEmpty(true);
            						refThis.viewModel.address.City('');
            						refThis.viewModel.address.County('');
            						refThis.viewModel.address.StateProvince('');
            						refThis.viewModel.address.StateProvinceID('');
            					}


            				} catch (ex) { }
            			},
            			error: function (detail, status, ex) {
            				try {
            					refThis.viewModel.postalCodeLookupEmpty(true);
            					refThis.viewModel.postalCodeLookupErrorOccurred(false);
            					refThis.viewModel.postalCodeLookupError(refThis.settings.termTranslations.commError);
            				} catch (ex) { }
            			},
            			complete: function () {
            				refThis.viewModel.postalCodeLookupInProgress(false);
            			}
            		});



            	}
            }
		};

		// create easy accessor to static prototype functions
		_ = addressEditor.prototype._;
		rootObj.ns.addressEditor = addressEditor;
		rootObj.ns.addressEditors = {};
	};


})(window);