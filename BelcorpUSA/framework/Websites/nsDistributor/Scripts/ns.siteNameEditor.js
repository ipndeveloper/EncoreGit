/// <reference path="jquery-1.7.2-vsdoc.js" />
// The above line enables VS intellisense for that file
(function (rootObj, overwriteIfExists) {

    if (rootObj.ns === undefined)
        rootObj.ns = {};

    // If this script already included in page, 
    // no need to redefine rootObj.sitename
    if (rootObj.sitename !== undefined && overwriteIfExists !== true)
        return;


    var siteNameEditor = function (element, settings) {
        var settings = settings === undefined || settings === null ? siteNameEditor.prototype.defaults : settings;
        element = $(element);

        thisEditor = this;

        // obtain references to elements up front

        var viewContainer = $(element.find(settings.selectors.viewContainer));
        var editContainer = $(element.find(settings.selectors.editContainer));
        var progressContainer = $(element.find(settings.selectors.progressContainer))
        var inputContainer = $(element.find(settings.selectors.inputContainer));
        var current = $(element.find(settings.selectors.siteNameCurrent));
        var preview = $(element.find(settings.selectors.siteNamePreview));
        var available = $(element.find(settings.selectors.siteNameAvailable));
        var unavailable = $(element.find(settings.selectors.siteNameUnavailable));
        var refreshAvailable = $(element.find(settings.selectors.refreshAvailable));
        var inputElement = $(element.find(settings.selectors.inputElement));
        var editControl = $(element.find(settings.selectors.editElement));
        var submitControl = $(element.find(settings.selectors.submitElement));
        var cancelControl = $(element.find(settings.selectors.cancelElement));
        var promptContainer = $(element.find(settings.selectors.promptContainer)).jqm({ modal: true });
        var promptSubmitControl = $(element.find(settings.selectors.promptSubmitElement));
        var promptCancelControl = $(element.find(settings.selectors.promptCancelElement));
        var promptBodyContainer = $(element.find(settings.selectors.promptBodyContainer));
        var promptProgressContainer = $(element.find(settings.selectors.promptProgressContainer));

        submitControl = submitControl.not(promptSubmitControl);

        // initialize data values
        current.text(settings.siteName + '.' + settings.siteNameDomain);

        var switchToEditMode = function () {
            lastCheck = null;
            lastSuccess = false;
            inputElement.val(settings.siteName);
            changeHandler(function () {
                viewContainer.hide();
                editContainer.show();
                submitControl.disable();
                inputElement.focus();
                available.hide();
                unavailable.hide();
            });
        };

        var switchToViewMode = function () {
            available.hide();
            unavailable.hide();
            viewContainer.show();
            editContainer.hide();
        };

        switchToViewMode();

        var triggerChecking = function (siteName) {
            $(thisEditor).trigger('CheckingSiteName', { siteName: siteName });
            inputContainer.hide();
            progressContainer.show();
            submitControl.disable();
            available.hide();
            unavailable.hide();
        };

        var triggerCheckingComplete = function (siteName, isAvailable) {
            progressContainer.hide();
            if (isAvailable) {
                available.show();
                unavailable.hide();
                submitControl.enable();
            } else {
                available.hide();
                unavailable.show();
                submitControl.disable();
            }
            inputContainer.show();
            $(thisEditor).trigger('CheckingSiteNameComplete', { siteName: siteName, isAvailable: isAvailable });
        };

        editControl.click(switchToEditMode);
        cancelControl.click(switchToViewMode);

        var lastCheck = null;
        var lastSuccess = false;
        var checkSiteName = function (siteName, callback) {
            if (lastCheck == siteName) {
                if (typeof (callback) === 'function') callback(lastSuccess);
                return;
            }
            lastCheck = siteName;
            triggerChecking(siteName);
            var siteNameValidation = /[^a-zA-Z0-9\-]/;
            preview.text(siteName + '.' + settings.siteNameDomain);

            if (siteName.length == 0 || siteNameValidation.test(siteName)) {
                triggerCheckingComplete(siteName, false);
            } else {
                $.ajax(siteNameEditor.prototype.defaults.isAvailableUrl,
                    {
                        data: {
                            siteName: siteName
                        },
                        success: function (results) {
                            if (results.redirect) {
                                window.location = results.redirectUrl;
                            } else {
                                lastSuccess = results.available;
                                triggerCheckingComplete(siteName, results.available);
                                if (typeof (callback) === 'function') callback(results.Available);
                            }
                        },
                        error: function () {
                            alert('an error has occurred while attempting to validate your new site name');
                            lastSuccess = false;
                            triggerCheckingComplete(siteName, false);
                            if (typeof (callback) === 'function') callback(false);
                        }
                    }
                );
            }
        };

        var cancelTimeout = null;
        var changeHandler = function (callback) {
            var siteName = inputElement.val().toLowerCase();
            checkSiteName(siteName, callback);
        };

        var toggleConfirm = function () {
            if (lastSuccess) {
                promptBodyContainer.show();
                promptProgressContainer.hide();
                promptContainer.jqmShow();
                promptSubmitControl.focus();
            }
        };

        refreshAvailable.click(changeHandler);
        inputElement.change(changeHandler);

        inputElement.each(function () {
            if (this.tagName.toUpperCase() == 'INPUT') {
                inputElement.keydown(function (e) {
                    return (
                        (
                            (e.keyCode >= 65 && e.keyCode <= 105) || ((e.keyCode >= 48 && e.keyCode <= 57) && !e.shiftKey) // a-zA-Z0-9
                            || ((e.keyCode >= 33 && e.keyCode <= 40) && !keyCode.altKey)  // PgUp,PgDn,End,Home,Left,Up,Right,Down
                            || (e.keyCode >= 16 && e.keyCode <= 18)  // Shift,Ctrl,Alt
                            || (e.keyCode == 20)  // Caps
                            || (e.keyCode == 46)  // Del
                            || (e.keyCode == 13)  // Enter
                            || (e.keyCode >= 8 && e.keyCode <= 9) // Backspace, Tab
                            || (e.keyCode == 109 || (e.keyCode == 189 && !e.shiftKey))  // Hyphens (keypad & top row)
                        )
                    &&
                        !(e.keyCode == 86 && e.ctrlKey) // disable paste

                    );
                });
                inputElement.keyup(function (e) {

                    if (inputElement.val().match(/[^a-zA-Z0-9\-]/g)) {
                        inputElement.val(inputElement.val().replace(/[^a-zA-Z0-9\-]/g, ''));
                    }

                    if (e.keyCode == 13) {
                        window.setTimeout(function () { toggleConfirm(); }, 0);
                    }


                    if (cancelTimeout != null)
                        window.clearTimeout(cancelTimeout);
                    var thisItem = this;
                    var thisArgs = arguments;
                    cancelTimeout = window.setTimeout(function () {
                        changeHandler.apply(thisItem, thisArgs);
                    }, 500);
                });
            }
        });

        submitControl.click(function () {
            toggleConfirm();
        });

        $(promptContainer.find('.cancel')).click(function () { promptContainer.jqmHide(); });


        promptCancelControl.click(function () {
            promptContainer.jqmHide();
        });


        executeChange = function () {
            promptBodyContainer.fadeOut(500, function () {
                promptProgressContainer.fadeIn(500, function () {
                    var siteName = inputElement.val();
                    checkSiteName(siteName, function () {
                        $.ajax(settings.submitUrl,
                        {
                            data: {
                                newSiteName: siteName
                            },
                            success: function (results) {
                                if (results !== undefined && results.success === true && results.successUrl !== undefined) {
                                    window.location = results.successUrl;
                                } else if (results.redirect) {
                                    window.location = results.redirectUrl;
                                } else {
                                    alert('An error has occurred while updating your personal website name:\r\n\r\n' +
                                            ((results !== undefined && results.errorMessage !== undefined)
                                            ? results.errorMessage
                                            : 'unknown error'));
                                    if (results !== undefined && results.failUrl !== undefined)
                                        window.location = results.failUrl;
                                    else {
                                        checkSiteName(lastCheck);
                                    }
                                }
                            },
                            error: function (ex) {
                                alert('An error has occurred while updating your personal website name:\r\n\r\n' + (ex !== undefined ? ex : 'unknown error'));
                                checkSiteName(lastCheck);
                            }
                        });
                    });
                });
            });


        };

        promptSubmitControl.click(function () {
            executeChange();
        });
        promptContainer.keyup(function (evt) {
            if (evt.keyCode == 13) executeChange();
        }
        );

    };

    var domainIndex = (/\./g);
    domainIndex.test(document.domain);

    siteNameEditor.prototype.defaults =
    {
        isAvailableUrl: '/Admin/CheckDomainAvailability',
        submitUrl: '/Admin/ChangeSiteName',
        siteName: document.domain.slice(0, domainIndex.lastIndex),
        siteNameDomain: document.domain.slice(domainIndex.lastIndex, document.domain.length),
        selectors: {
            progressContainer: '.progress',
            viewContainer: '.viewMode',
            editContainer: '.editMode',
            inputElement: '.siteNameInput',
            siteNameCurrent: '.siteNameCurrent',
            siteNamePreview: '.siteNamePreview',
            siteNameAvailable: '.available',
            siteNameUnavailable: '.unavailable',
            refreshAvailable: '.refreshAvailable',
            editElement: '.edit',
            submitElement: '.submit',
            cancelElement: '.cancel',
            promptContainer: '.prompt',
            promptSubmitElement: '.prompt .change',
            promptCancelElement: '.prompt .cancel',
            promptBodyContainer: '.prompt .bodyContainer',
            promptProgressContainer: '.prompt .progressContainer'
        }
    };

    $.fn.enable = function () {
        // To enable 
        return this.removeClass('disabled').removeAttr('disabled');
    };

    $.fn.disable = function () {
        return this.addClass('disabled').attr('disabled', '');
    };

    $.fn.siteNameEditor = function (settings) {
        ///<summary>Attaches a site name editor to each of the nodes using the 
        ///provided settings to determine ajaxMethods, inputs, labels, and status 
        ///indicators</summary>
        ///<param name="settings">{
        ///&#10;    isAvailableUrl: 'CheckDomainAvailability?siteName={0}',
        ///&#10;    submitUrl: 'UpdateSiteName?siteName={0}',
        ///&#10;    siteName: document.domain.slice(0, domainIndex.lastIndex),
        ///&#10;    siteNameDomain: document.domain.slice(domainIndex.lastIndex, document.domain.length),
        ///&#10;    selectors: {
        ///&#10;        progressContainer: '.progress',        - in progress containers
        ///&#10;        viewContainer: '.viewMode',                - view mode container
        ///&#10;        editContainer: '.editMode',                - edit mode container
        ///&#10;        inputElement: '.siteNameInput',                 - the site name input field
        ///&#10;        siteNameCurrent: '.siteNameCurrent',            - current site name field labels
        ///&#10;        siteNamePreview: '.siteNamePreview',            - preview site name field labels
        ///&#10;        siteNameAvailable: '.available',        - is available success indicators 
        ///&#10;        siteNameUnavailable: '.unavailable',    - is available failure indicators
        ///&#10;        refreshAvailable: '.refreshAvailable',          - elements which trigger availability checking by click
        ///&#10;        editElement: '.edit',                   - elements which trigger edit mode by click
        ///&#10;        submitElement: '.submit',               - elements which trigger submitting by click 
        ///&#10;        cancelElement: '.cancel',                - elements which trigger cancelling edit mode by click
        ///&#10;        promptContainer: '.prompt .submit',               - elements which trigger prompting edit mode by click
        ///&#10;        promptSubmitElement: '.prompt .change',               - elements which trigger submitting from the prompt
        ///&#10;        promptCancelElement: '.prompt .cancel'               - elements which trigger cancelling from the prompt
        ///&#10;        promptBodyContainer: '.prompt .bodyContainer',               - elements which trigger prompting edit mode by click
        ///&#10;        promptProgressContainer: '.prompt .progressContainer',               - elements which trigger prompting edit mode by click
        ///&#10;    }
        ///&#10;}
        ///</param>

        // Merge custom settings with defaults and create new object so as to not modify the defaults

        settings = $.extend({}, siteNameEditor.prototype.defaults, settings);

        // Create an editor for each of the elements found in the selectors
        return this.each(function () {
            var editor = new siteNameEditor(this, settings);
        });
    };


})(window);