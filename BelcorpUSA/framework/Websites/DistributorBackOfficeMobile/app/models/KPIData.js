NSMobile.models.KPIData = Ext.regModel('KPIData', {
    fields:
        [
            'periodname',
            'periodid',
            { name: 'pv', type: 'float' },
            { name: 'gv', type: 'float' },
            'progress',
            'pvGoalPercent',
            'gvGoalPercent',
            'title',
            'paidAsTitle',
            { name: 'PersonallySponsoredCount', type: 'int' },
            { name: 'DownlineCount', type: 'int' },
            { name: 'CustomKPIReports', type: 'KPIRptWidget' }
        ]
});

NSMobile.stores.kpidata = new Ext.data.JsonStore({
    model: 'KPIData',
    proxy: {
        type: 'ajax',
        url: serviceURI + '/GetPerformance',
        listeners: {
            exception: {
                fn: function (proxy, response, operation) {
                    showErrorOnce();
                }
            }
        },
        timeout: ajaxTimeout
    },
    sorters: {
        property: 'periodid',
        direction: 'desc'
    },
    listeners: {
        beforeload: {
            fn: function () {
                setSyncIcon('chart4', true);
            }
        },
        load: {
            fn: function () {
                if (NSMobile.stores.kpidata.data.length > 0) {
                    var data = NSMobile.stores.kpidata.getAt(0).data;

                    var periodselect = Ext.getCmp('periodselect');
                    if (periodselect && data) {
                        periodselect.onPeriodChange(data.periodid);
                        periodselect.setValue(data.periodid);
                    }
                }
                setSyncIcon('chart4', false);
            }
        }
    }
});