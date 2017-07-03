NSMobile.models.Order = Ext.regModel('Order', {
    fields:
        [
            { name: 'id', type: 'int' },
            'name',
            'type',
            { name: 'date', type: 'date', dateFormat: 'c', convert: function (value, record) {
                    var dateArr;
                    if (!value)
                        return;

                    if (value.indexOf('\/') > -1) {
                        dateArr = value.split('\/');
                        var year = dateArr[2];
                        var month = dateArr[0] - 1;
                        var day = dateArr[1];
                    }
                    else {
                        dateArr = value.split('-');
                        var year = dateArr[0];
                        var month = dateArr[1];
                        var day = dateArr[2].substring(0, 2).replace('T', '');
                    }

                    return new Date(year, month, day);
                }
            },
            { name: 'total', type: 'float' },
            { name: 'pv', type: 'float' },
            'period',
            { name: 'number', type: 'int' },
            'status',
            'completedate',
            'commissiondate',
            'partydate',
            'partyenddate',
            'hostess',
            'address',
            { name: 'partydetail', type: 'PartyDetail' },
            'trackingnumbers',
            'trackingurls'
        ]
});

NSMobile.stores.perfOrders = new Ext.data.Store({ //for performance order lists
    model: 'Order',
    sorters: 'Standard',
    remoteSort: true,
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        },
        read: function (operation, callback, scope) {
            var me = this;
            perfOrderStore.clearFilter();
            perfOrderStore.sorters.clear();
            var sortProperty = 'sortorder';
            var sortDirection = 'asc';
            if (operation.sorters.length > 0) {
                var sorter = operation.sorters[0];
                sortDirection = sorter.direction;
                sortProperty = sorter.property;
            }
            perfOrderStore.sorters.add({ property: sortProperty, direction: sortDirection });
            perfOrderStore.currentPage = operation.page;
            perfOrderStore.read({
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

NSMobile.stores.pvOrdersPaginated = new Ext.data.Store({
    model: 'Order',
    sorters: 'date',
    pageSize: pageSize,
    remoteSort: true,
    proxy: {
        headers: { "Content-Type": "application/json" },
        type: 'ajax',
        url: serviceURI + '/GetPVOrdersPaginated',
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
                if (NSMobile.stores.pvOrdersPaginated.data.length == 0 && NSMobile.stores.pvOrdersPaginated.currentPage > 1)
                    NSMobile.stores.pvOrdersPaginated.previousPage();
                setSyncIcon('chart4', false);
            }
        }
    }
});

NSMobile.stores.gvOrdersPaginated = new Ext.data.Store({
    model: 'Order',
    sorters: 'date',
    pageSize: pageSize,
    remoteSort: true,
    proxy: {
        headers: { "Content-Type": "application/json" },
        type: 'ajax',
        url: serviceURI + '/GetGVOrdersPaginated',
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
                if (NSMobile.stores.gvOrdersPaginated.data.length == 0 && NSMobile.stores.gvOrdersPaginated.currentPage > 1)
                    NSMobile.stores.gvOrdersPaginated.previousPage();
                setSyncIcon('chart4', false);
            }
        }
    }
});

NSMobile.stores.orders = new Ext.data.JsonStore({
    model: 'Order',
    proxy: {
        type: 'ajax',
        url: serviceURI + '/GetOrders',
        listeners: {
            exception: {
                fn: function (proxy, response, operation) {
                    showErrorOnce();
                }
            }
        },
        timeout: ajaxTimeout
    },
    listeners: {
        beforeload: {
            fn: function () {
                setSyncIcon('list', true);
            }
        },
        load: {
            fn: function (store) {
                ordersLoaded = true;
                NSMobile.stores.orders.sorters.clear();
                if (ordersSortPicker && ordersSortPicker.dealtWith) {
                    var values = ordersSortPicker.getValue();
                    NSMobile.stores.orders.sort(values.property.toLowerCase(), values.direction);
                }
                else {
                    if(ordersSortPicker)
                        ordersSortPicker.dealtWith = true;
                    NSMobile.stores.orders.sort('date', 'DESC');
                }
                setSyncIcon('list', false);
            }
       }
    },
    sorters: {
        property: 'date',
        direction: 'DESC'
    },
});

NSMobile.stores.partyorders = new Ext.data.JsonStore({
    model: 'Order',
    proxy: {
        type: 'ajax',
        url: serviceURI + '/GetPartyOrders',
        timeout: ajaxTimeout
    },
    listeners: {
        load: {
            fn: function (store, records, successful) {
                if (records.length == 1) {
                    var data = records[0].data;
                    Ext.getCmp('partyinfo').update(data);
                    Ext.getCmp('partydetails').update(data);
                    Ext.getCmp('partylandingtoolbar').setTitle(data.name);
                    Ext.getCmp('parties').setActiveItem(1);
                }
            }
        }
    }
});

var ordersLoaded = false;
var perfOrderStore = NSMobile.stores.pvOrdersPaginated;