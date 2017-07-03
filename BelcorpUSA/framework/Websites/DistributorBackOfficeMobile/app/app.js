if (!/WebKit|Android|ip(hone|od|ad)/i.test(navigator.userAgent)) {
    alert('This browser is not supported. Please use a WebKit-based browser like Google Chrome, Safari, Android, or iPhone/iPod/iPad.'); //todo: redirect to not-supported page joey will make
}

NSMobile = new Ext.Application({
    name: 'NSMobile',
    fullscreen: true,
	layout: 'fit',
    phoneStartupScreen: 'lib/resources/themes/images/encore/mobileWeb_startup.png',
	icon: 'lib/resources/themes/images/scentsy/apple-touch-icon.png',
    launch: function () {
        isDesktop = !Ext.is.Phone;
        baseListHeight = (window.innerHeight - 102) + 'px'; //bottom toolbar (55) + top toolbar (47)

        //fixes sencha not resizing properly when device is rotated
        NSMobile.on('orientationchange', function () {
            this.el.parent().setSize(window.innerWidth, window.innerHeight);
        });

        if (Ext.is.Android) {
            pageBackAnimation = undefined;

            //sencha bug causes this to not completely remove modals
            //Ext.Anim.override({
            //    disableAnimations: true
            //});
        }

        handleNotification(window.location.href);

        switch (window.location.hash) {
            case '#home':
                hash = window.location.hash = '#main';
                break;
            case '':
            case '#':
            case '#main':
                hash = window.location.hash = '#home';
                break;
            default:
                var hashArr = window.location.hash.split('.');
                if (hashArr.length == 2) {
                    var storageData = getStorage('auth');
                    storageData.hash = unescape(hashArr[0].substring(1));
                    storageData.currentuser = hashArr[1];
                    setStorage('auth', storageData);
                }
                hash = window.location.hash = '#home';
                break;
        }

        Ext.apply(Ext.plugins.ListPagingPlugin.prototype, {
            onScrollEnd: function (scroller, pos) {
                if (!this.loading) {
                    var store = this.list.store;
                    if (pos.y >= Math.abs(scroller.offsetBoundary.top)) {
                        this.loading = true;
                        store.currentPage = store.currentPage + 1;
                        store.read({
                            page: store.currentPage,
                            start: (store.currentPage - 1) * store.pageSize,
                            callback: function (records, operation, success) {
                                scroller.scrollTo({ x: 0, y: 0 });
                            }
                        });
                    }
                    else if (pos.y <= Math.abs(scroller.offsetBoundary.bottom) && store.currentPage > 1) {
                        this.loading = true;
                        store.currentPage = store.currentPage - 1;
                        store.read({
                            page: store.currentPage,
                            start: (store.currentPage - 1) * store.pageSize,
                            callback: function (records, operation, success) {
                                scroller.scrollTo({ x: 0, y: Math.abs(scroller.offsetBoundary.top) - 41 }); //if you change the loading thinger, change this
                            }
                        });
                    }
                }
            },
            onListUpdate: function () {
                if (!this.rendered) {
                    this.render();
                }

                this.el.appendTo(this.list.getTargetEl());
                if (!this.autoPaging) {
                    this.el.removeCls('x-loading');
                }

                if (this.list.store.data.length < this.list.store.pageSize) {
                    var spinner = this.list.getTargetEl().dom.querySelector('.x-loading-spinner');
                    if (spinner)
                        spinner.className = spinner.className.replace('x-loading-spinner', 'x-loading-spinner-disabled');
                }
                else {
                    var spinner = this.list.getTargetEl().dom.querySelector('.x-loading-spinner-disabled');
                    if (spinner)
                        spinner.className = spinner.className.replace('x-loading-spinner-disabled', 'x-loading-spinner');
                }

                this.loading = false;
            }
        });

        Ext.apply(Ext.util.Format, {
            defaultDateFormat: 'M Y'
        });

        //allow datepicker to function without day field
        Ext.apply(Ext.DatePicker.prototype, {
            getValue: function () {
                var value = Ext.DatePicker.superclass.getValue.call(this);
                var daysInMonth = this.getDaysInMonth(value.month, value.year);
                var day = Math.min(value.day, daysInMonth);

                return new Date(value.year, value.month - 1, day || 1);
            },
            onSlotPick: function (slot, value) {
                var name = slot.name,
                    date, daysInMonth, daySlot;

                if (name === "month" || name === "year") {
                    daySlot = this.child('[name=day]');
                    if (daySlot != null) {
                        date = this.getValue();
                        daysInMonth = this.getDaysInMonth(date.getMonth() + 1, date.getFullYear());
                        daySlot.store.clearFilter();
                        daySlot.store.filter({
                            fn: function (r) {
                                return r.get('extra') === true || r.get('value') <= daysInMonth;
                            }
                        });
                        daySlot.scroller.updateBoundary(true);
                    }
                }

                Ext.DatePicker.superclass.onSlotPick.apply(this, arguments);
            }
        });

        Ext.apply(Ext.MessageBox, {
            YESNO: [Ext.MessageBox.YES, Ext.MessageBox.NO]
        });

        yepnope({
            load: ['app/models/NewsItem.js' + cacheVer, 'app/models/Contact.js' + cacheVer, 'app/views/Orders.js' + cacheVer, 'app/models/KPIRptWidget.js' + cacheVer, 'app/models/KPIData.js' + cacheVer, 'app/models/Order.js' + cacheVer, 'app/models/PartyDetail.js' + cacheVer,
                'app/views/News.js' + cacheVer, 'app/views/KPI.js' + cacheVer, 'app/views/Performance.js' + cacheVer, 'app/views/Contacts.js' + cacheVer,
                'app/views/NetworkTeam.js' + cacheVer, 'app/views/NetworkCustomers.js' + cacheVer, 'app/views/NetworkProspects.js' + cacheVer, 'app/views/Network.js' + cacheVer,
                'app/views/Parties.js' + cacheVer, 'app/views/Viewport.js' + cacheVer],
            complete: function () { pageLoadCallback(); }
        });

        checkLogin(this);
    }
});

function checkLogin(app) {
    var storageData = getStorage('auth');
    if (storageData.hash && storageData.currentuser) {
        Ext.Ajax.extraParams = { hash: storageData.hash };
        if (app.views.Viewport)
            app.views.viewport = new app.views.Viewport();
        else {
            var loadMask = new Ext.LoadMask(Ext.getBody());
            loadMask.show();
            pageLoadCallback = function () {
                yepnope({
                    test: NSMobile.views.Viewport,
                    nope: 'app/views/Viewport.js',
                    complete: function () {
                        loadMask.hide();
                        NSMobile.views.viewport = new NSMobile.views.Viewport();
                    }
                });
            }
        }
    }
    else
        app.views.viewport = new app.views.Login();
}

function logout() {
    localStorage.removeItem('auth');
    localStorage.removeItem('news');

    document.location.reload();
}

function getStorage(key) {
    return JSON.parse(localStorage[key] || '{}');
}

function setStorage(key, data) {
    localStorage[key] = JSON.stringify(data);
}

function setSyncIcon(iconCls, show) {
    var iconEl = document.querySelector('img.' + iconCls);
    if (iconEl) {
        if (show) {
            if (iconEl.classList)
                iconEl.classList.add(syncClass);
            else {
                if (iconEl.className.indexOf(syncClass) == -1)
                    iconEl.className += ' ' + syncClass;
            }
        }
        else {
            if (iconEl.classList)
                iconEl.classList.remove(syncClass);
            else {
                if (iconEl.className.indexOf(syncClass) > -1)
                    iconEl.className = iconEl.className.replace(syncClass, '').trim();
            }
        }
    }
}

var errorShown = false;
function showErrorOnce() {
    if (!errorShown) {
        Ext.Msg.alert('Error', 'There was a problem communicating with the server. Some or all data may not have been loaded.');
        errorShown = true;
    }
}

Ext.ns('NSMobile.stores.local');

var hash;
window.onhashchange = function (e) {
    if (hash && window.location.hash != hash) {
        Ext.Msg.confirm('Really Leave?', 'Do you really want to leave the mobile workstation?', function (button) {
            if (button == 'yes')
                history.back(1);
            else
                history.forward(1);
        });
    }
}

var notificationType;
var notificationData;
function handleNotification(url) {
    var chunks = url.split('#');
    if (chunks.length == 2) {
        var data = chunks[1].split('-');
        if (data.length == 2) {
            notificationType = data[0];
            notificationData = data[1];

            if (loggedIn && !NSMobile.stores.local[notificationType].isLoading())
                NSMobile.stores.local[notificationType].load();
            else
                setTimeout(function () { handleNotification(url) }, 250);
        }
    }
}

function addDebugScript() {
    if (!document.querySelector('#debugScript')) {
        Ext.Msg.confirm('Enable Debugging?', 'Do you want to enable remote debugging so that a developer can inspect your current session?', function (button) {
            if (button == 'yes') {
                var debugScript = document.createElement('script');
                debugScript.src = 'http://debug.phonegap.com/target/target-script-min.js#fc56a764-e961-11e0-8289-1231390521a2';
                debugScript.id = 'debugScript';
                document.querySelector('body').appendChild(debugScript);
            }
        });
    }
}

function onImgError(ctrl) {
    ctrl.style.display = 'none';
}

var pageLoadCallback = function () { };
