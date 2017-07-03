NSMobile.models.TripData = Ext.regModel('TripData', {
    fields: [
        { name: 'ExcellenceSalesPointsActive', type: 'int' },
        { name: 'ExcellenceSalesPointsPotential', type: 'int' },
        { name: 'IsEligible', type: 'boolean' },
        { name: 'LastMonthClosed', type: 'string' },
        { name: 'PersonalSalesPointsActive', type: 'int' },
        { name: 'SalesPointsEarned', type: 'int' },
        { name: 'SalesPointsGoal', type: 'int' },
        { name: 'SalesPointsNeeded', type: 'int' },
        { name: 'SalesPointsNeededAfterPotential', type: 'int' },
        { name: 'SponsoringPointsActive', type: 'int' },
        { name: 'SponsoringPointsPotential', type: 'int' },
        { name: 'SponsorsForInfoActive', type: 'int' }
    ],
    getFormattedDate: function () {
        return new Date(this.get('LastMonthClosed').substring(0, 4), parseInt(this.get('LastMonthClosed').substring(4, 6)) - 1, 1).format("m/Y");
    }
});

NSMobile.stores.trip = new Ext.data.Store({
    model: 'TripData',
    proxy: {
        type: 'ajax',
        url: plServiceURI + '/tripeurope/cy/' + getStorage('auth').country + '/lg/' + getStorage('auth').language + '/id/' + getStorage('auth').username,
        listeners: {
            exception: {
                fn: function (proxy, response, operation) {
                    showErrorOnce();
                }
            }
        },
        timeout: ajaxTimeout,
        reader: {
            type: 'json',
            record: 'Trip',
            readRecords: function (data) {
                var trips = [];

                if (data && data.Trip)
                    trips.push(new NSMobile.models.TripData(data.Trip));

                return new Ext.data.ResultSet({
                    records: trips,
                    total: trips.length,
                    loaded: true
                });
            }
        }
    },
    listeners: {
        beforeload: {
            fn: function () {
                setSyncIcon('plane', true);
            }
        },
        load: {
            fn: function () {
                setSyncIcon('plane', false);

                var tripPanel = Ext.getCmp('trippanel');
                var nep = Ext.getCmp('noteligiblepanel');
                if (tripPanel && NSMobile.stores.trip.data.length > 0) {
                    var data = NSMobile.stores.trip.first().data;
                    tripPanel.update(data);

                    if (!data.IsEligible && tripPanel.rendered) {
                        Ext.getCmp('trip').setActiveItem(1);
                    }
                    else if (nep && nep.rendered) {
                        Ext.getCmp('trip').setActiveItem(1);
                    }
                }
            }
        }
    }
});