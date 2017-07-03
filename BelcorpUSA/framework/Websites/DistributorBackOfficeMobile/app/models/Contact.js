NSMobile.models.Contact = Ext.regModel('Contact', {
    fields: [
        'firstName',
        'lastName',
        'thumb',
        'birthdate',
        'commpref',
        'homephone',
        'mobilephone',
        'workphone',
        'email',
        'address',
        'rsvp',
        'customer',
        { name: 'onlinetotal', type: 'float' },
        'level',
        { name: 'pv', type: 'float' },
        { name: 'gv', type: 'float' },
        { name: 'accountnumber', type: 'int' },
        { name: 'accountid', type: 'int' },
        { name: 'sortorder', type: 'int' },
        { name: 'lastpurchasedate', type: 'date' },
        'title',
        { name: 'showCommissions', type: 'boolean' },
        'sponsor'
    ],
    getGroupString: function () {
        return ((this.get('lastName') || this.get('firstName'))[0] || '').toUpperCase();
    }
});

NSMobile.stores.teamContactsPaginated = new Ext.data.JsonStore({
    model: 'Contact',
    sorters: 'lastName',
    pageSize: pageSize,
    remoteSort: true,
    remoteFilter: true,
    proxy: {
        headers: { "Content-Type": "application/json" },
        type: 'ajax',
        url: serviceURI + '/GetTeamPaginated',
        startParam: 'start',
        limitParam: 'count',
        listeners: {
            exception: {
                fn: function (proxy, response, operation) {
                    showErrorOnce();
                }
            }
        },
        timeout: ajaxTimeout
    },
    getGroupString: function (record) {
        return record.getGroupString();
    },
    listeners: {
        beforeload: {
            fn: function () {
                setSyncIcon('user_list', true);
            }
        },
        load: {
            fn: function () {
                if (NSMobile.stores.teamContactsPaginated.data.length == 0 && NSMobile.stores.teamContactsPaginated.currentPage > 1)
                    NSMobile.stores.teamContactsPaginated.previousPage();
                teamLoaded = true;
                if (!NSMobile.stores.teamContactsPaginated.isLoading() && !NSMobile.stores.customerContactsPaginated.isLoading() && !NSMobile.stores.prospectContactsPaginated.isLoading())
                    setSyncIcon('user_list', false);
            }
        }
    }
});

NSMobile.stores.dlContactsPaginated = new Ext.data.JsonStore({
    model: 'Contact',
    sorters: 'lastName',
    pageSize: pageSize,
    remoteSort: true,
    remoteFilter: true,
    proxy: {
        headers: { "Content-Type": "application/json" },
        type: 'ajax',
        url: serviceURI + '/GetDownlinePaginated',
        startParam: 'start',
        limitParam: 'count',
        listeners: {
            exception: {
                fn: function (proxy, response, operation) {
                    showErrorOnce();
                }
            }
        },
        timeout: ajaxTimeout
    },
    getGroupString: function (record) {
        return record.getGroupString();
    },
    listeners: {
        beforeload: {
            fn: function () {
                setSyncIcon('chart4', true);
                this.proxy.extraParams['periodID'] =  Ext.getCmp('periodselect') ?  Ext.getCmp('periodselect').value : '';
            }
        },
        load: {
            fn: function () {
                if (NSMobile.stores.dlContactsPaginated.data.length == 0 && NSMobile.stores.dlContactsPaginated.currentPage > 1)
                    NSMobile.stores.dlContactsPaginated.previousPage();
                setSyncIcon('chart4', false);
            }
        }
    }
});

NSMobile.stores.aflContactsPaginated = new Ext.data.JsonStore({
    model: 'Contact',
    sorters: 'lastName',
    pageSize: pageSize,
    remoteSort: true,
    remoteFilter: true,
    proxy: {
        headers: { "Content-Type": "application/json" },
        type: 'ajax',
        url: serviceURI + '/GetTeamPaginated',
        startParam: 'start',
        limitParam: 'count',
        listeners: {
            exception: {
                fn: function (proxy, response, operation) {
                    showErrorOnce();
                }
            }
        },
        timeout: ajaxTimeout
    },
    getGroupString: function (record) {
        return record.getGroupString();
    },
    listeners: {
        beforeload: {
            fn: function () {
                setSyncIcon('chart4', true);
                this.proxy.extraParams['periodID'] =  Ext.getCmp('periodselect') ?  Ext.getCmp('periodselect').value : '';
            }
        },
        load: {
            fn: function () {
                if (NSMobile.stores.aflContactsPaginated.data.length == 0 && NSMobile.stores.aflContactsPaginated.currentPage > 1)
                    NSMobile.stores.aflContactsPaginated.previousPage();                
                setSyncIcon('chart4', false);
            }
        }
    }
});

NSMobile.stores.perfContacts = new Ext.data.Store({ //for performance lists
    model: 'Contact',
    sorters: 'Standard',
    remoteSort: true,
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        },
        read: function (operation, callback, scope) {
            var me = this;
            perfContactStore.clearFilter();
            perfContactStore.sorters.clear();
            var sortProperty = 'sortorder';
            var sortDirection = 'asc';
            if (operation.sorters.length > 0) {
                var sorter = operation.sorters[0];
                sortDirection = sorter.direction;
                sortProperty = sorter.property;
            }
            perfContactStore.sorters.add({ property: sortProperty, direction: sortDirection });
            perfContactStore.currentPage = operation.page;
            perfContactStore.read({
                page: operation.page,
                start: operation.start,
                limit: operation.limit,
                callback: function (records, callbackOperation, success) {
                    operation.resultSet = new Ext.data.ResultSet({
                        records: records,
                        total: records.length,
                        loaded: true
                    });
                    operation.setSuccessful();
                    operation.setCompleted();

                    if (typeof callback == 'function')
                        callback.call(scope || me, operation);
                }
            });
            networkSorted = true;
        }
    }
});

NSMobile.stores.customerContactsPaginated = new Ext.data.Store({
    model: 'Contact',
    sorters: 'lastName',
    pageSize: pageSize,
    remoteSort: true,
    remoteFilter: true,
    proxy: {
        type: 'ajax',
        url: serviceURI + '/GetCustomersPaginated',
        startParam: 'start',
        limitParam: 'count',
        listeners: {
            exception: {
                fn: function (proxy, response, operation) {
                    showErrorOnce();
                }
            }
        },
        timeout: ajaxTimeout
    },
    getGroupString: function (record) {
        return record.getGroupString();
    },
    listeners: {
        beforeload: {
            fn: function () {
                setSyncIcon('user_list', true);
            }
        },
        load: {
            fn: function () {
                if (NSMobile.stores.customerContactsPaginated.data.length == 0 && NSMobile.stores.customerContactsPaginated.currentPage > 1)
                    NSMobile.stores.customerContactsPaginated.previousPage();
                customersLoaded = true;
                if (!NSMobile.stores.teamContactsPaginated.isLoading() && !NSMobile.stores.customerContactsPaginated.isLoading() && !NSMobile.stores.prospectContactsPaginated.isLoading())
                    setSyncIcon('user_list', false);
            }
        }
    }
});

NSMobile.stores.prospectContactsPaginated = new Ext.data.JsonStore({
    model: 'Contact',
    sorters: 'lastName',
    pageSize: pageSize,
    remoteSort: true,
    remoteFilter: true,
    proxy: {
        type: 'ajax',
        url: serviceURI + '/GetProspectsPaginated',
        startParam: 'start',
        limitParam: 'count',
        listeners: {
            exception: {
                fn: function (proxy, response, operation) {
                    showErrorOnce();
                }
            }
        },
        timeout: ajaxTimeout
    },
    getGroupString: function (record) {
        return record.getGroupString();
    },
    listeners: {
        beforeload: {
            fn: function () {
                setSyncIcon('user_list', true);
            }
        },
        load: {
            fn: function () {
                if (NSMobile.stores.prospectContactsPaginated.data.length == 0 && NSMobile.stores.prospectContactsPaginated.currentPage > 1)
                    NSMobile.stores.prospectContactsPaginated.previousPage();
                prospectsLoaded = true;

                if (!NSMobile.stores.teamContactsPaginated.isLoading() && !NSMobile.stores.customerContactsPaginated.isLoading() && !NSMobile.stores.prospectContactsPaginated.isLoading())
                    setSyncIcon('user_list', false);
            }
        }
    }
});

NSMobile.stores.partyGuests = new Ext.data.Store({ //stub for guestlist as lists require stores
    model: 'Contact',
    sorters: 'lastName',
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        }
    }
});

NSMobile.stores.allContacts = new Ext.data.Store({
    model: 'Contact',
    sorters: 'lastName',
    getGroupString: function (record) {
        return record.getGroupString();
    },
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        }
    }
});

NSMobile.stores.phoneContacts = new Ext.data.Store({
    model: 'Contact',
    sorters: 'lastName',
    getGroupString: function (record) {
        return record.getGroupString();
    },
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        }
    }
});

function loadPhoneContacts() {
    if (navigator.service)
        navigator.service.contacts.find(['name', 'emails'], function (phoneContacts) {
            var contacts = [];
            for (var i = 0; i < phoneContacts.length; i++) {
                var phoneContact = phoneContacts[i];
                var contact = new NSMobile.models.Contact();
                contact.data.firstName = phoneContact.name.givenName;
                contact.data.lastName = phoneContact.name.familyName;
                contact.data.email = phoneContact.emails[0].value;
                contacts.push(contact);
            }
            NSMobile.stores.phoneContacts.loadData(contacts, false);
        });
}

function loadAllContacts() {
    if (contactsLoaded || lock)
        return;

    lock = true;

    if (!teamLoaded)
        NSMobile.stores.teamContactsPaginated.load(function () { loadAllContacts(); });
    if (!customersLoaded)
        NSMobile.stores.customerContactsPaginated.load(function () { loadAllContacts(); });
    if (!prospectsLoaded)
        NSMobile.stores.prospectContactsPaginated.load(function () { loadAllContacts(); });

    if (teamLoaded && customersLoaded && prospectsLoaded) {
        var contacts = [];
        NSMobile.stores.teamContactsPaginated.each(function (item) { contacts.push(item.data); });
        NSMobile.stores.customerContactsPaginated.each(function (item) { contacts.push(item.data); });
        NSMobile.stores.prospectContactsPaginated.each(function (item) { contacts.push(item.data); });
        NSMobile.stores.allContacts.loadData(contacts, false);
        contactsLoaded = true;
    }

    lock = false;
}

var teamLoaded = false, customersLoaded = false, contactsLoaded = false, lock = false, prospectsLoaded = false;
var perfContactStore = NSMobile.stores.dlContactsPaginated;
var loadPerfContactsFresh = false;
var networkSorted = false;