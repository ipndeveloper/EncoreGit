NSMobile.models.KPIRptWidget = Ext.regModel('KPIRptWidget', {
    fields:
        [
            'LandingTitle',
            'LandingValue',
            { name: 'Rows', type: 'KPIRptRow' }
        ]
});

NSMobile.models.KPIRptRow = Ext.regModel('KPIRptRow', {
    fields:
        [
            { name: 'Cells', type: 'KPIRptCell' }
        ]
});

NSMobile.models.KPIRptCell = Ext.regModel('KPIRptCell', {
    fields:
        [
            'Name',
            'Value',
            { name: 'IsOnSummary', type: 'boolean' }
        ]
});

NSMobile.stores.kpiRptRows = new Ext.data.Store({
    model: 'KPIRptRow',
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        },
        timeout: ajaxTimeout
    }
});