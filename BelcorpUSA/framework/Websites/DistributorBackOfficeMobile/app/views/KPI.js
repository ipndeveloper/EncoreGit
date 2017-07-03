NSMobile.views.kpi = Ext.extend(Ext.Panel, {
    title: 'My KPIs',
    fullscreen: true,
    layout: 'vbox',
    scroll: 'vertical',
    cls: 'performanceForm',
    id: 'performanceForm',
    styleHtmlContent: false,
    width: '100%',
    initComponent: function () {
        var periodSelect = new Ext.form.Select({
            xtype: 'selectfield',
            name: 'period',
            id: 'periodselect',
            store: NSMobile.stores.kpidata,
            displayField: 'periodname',
            valueField: 'periodid',
            listeners: {
                change: {
                    fn: function () {
                        this.onPeriodChange(this.value);
                    }
                },
                afterrender: {
                    fn: function () {
                        if (NSMobile.stores.kpidata.data.length > 0) {
                            var periodselect = Ext.getCmp('periodselect');
                            var data = NSMobile.stores.kpidata.getAt(0).data;
                            if (periodselect && data) {
                                periodselect.onPeriodChange(data.periodid);
                                periodselect.setValue(data.periodid);
                            }
                        }
                    }
                }
            },
            onPeriodChange: function (periodid) {
                if (NSMobile.stores.kpidata.data.length > 0) {
                    var index = NSMobile.stores.kpidata.find('periodid', periodid);
                    if (index > -1)
                        var kpidata = NSMobile.stores.kpidata.getAt(index);
                    if (kpidata)
                        Ext.getCmp('kpiinfo').update(kpidata.data);
                }
            }
        });

        var kpiPanel = new Ext.Panel({
            id: 'kpiinfo',
            //styleHtmlContent: false,
            width: '100%',
            tpl: [

            // Basic, non-drilldown information should go in the header
				'<header class="snapShotInfo performance">',
					'<ul class="dataList staticList">',
						'<li><span class="label">Recognition Title:</span><span class="data">{title}</span></li>',
                        '<li><span class="label">Paid as Title:</span><span class="data">{paidAsTitle}</span></li>',
					'</ul>',
				'</header>',

            // Drilldown buttons and Visual KPIs go in here
				'<section class="drillDownWrapper performanceKPIs">',
                    '<ul class="dataList drillDownList">',
                    	'<li class="clickableRow" onclick="javascript:(function(){Ext.getCmp(\'performanceForm\').viewAFL()})()"><div class="crcw"><span class="clickWrap"><span class="label">Personally Sponsored:</span><span class="data">{PersonallySponsoredCount}</span></span></div></li>',
                        '<li class="clickableRow" onclick="javascript:(function(){Ext.getCmp(\'performanceForm\').viewDL()})()"><div class="crcw"><span class="clickWrap"><span class="label">Downline Count:</span><span class="data">{DownlineCount}</span></span></div></li>',
                        '<li class="clickableRow" onclick="javascript:(function(){Ext.getCmp(\'performanceForm\').viewPVOrders()})()">',
							'<div class="crcw">',
							'<span class="label">Personal Volume:</span><span class="data">{[values.pv.toFixed(2)]}</span>',
							'<div class="progressBar"><div class="indicatorBar" style="width:{pvGoalPercent};"></div></div></div>',
							'</div>',
						'</li>',
                        '<li class="clickableRow DISABLED" onclick="javascript:(function(){Ext.getCmp(\'performanceForm\').viewGVOrders()})()">',
							'<div class="crcw">',
							'<span class="label">Group Volume:</span><span class="data">{[values.gv.toFixed(2)]}</span>',
							'<div class="progressBar"><div class="indicatorBar" style="width:{gvGoalPercent};"></div></div></div>',
							'</div>',
						'</li>',
                        '<tpl for="CustomKPIReports">',
                            '<li class="clickableRow" onclick="javascript:(function(){Ext.getCmp(\'performanceForm\').viewKPIRpt({[xindex]})})()"><div class="crcw"><span class="clickWrap"><span class="label">{LandingTitle}:</span><span class="data">{LandingValue}</span></span></div></li>',
                        '</tpl>',
					'</ul>',
                '</section>'
            ]
        });

        Ext.apply(this, {
            items: [
                kpiPanel
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    cls: 'x-list-header',
                    align: 'center',
                    layout: {
                        pack: 'center'
                    },
                    items: [
                        periodSelect
                    ]
                }
            ]
        });
        NSMobile.views.kpi.superclass.initComponent.apply(this, arguments);
    },
    viewPVOrders: function() {
        this.updateAndShowKPIOrders(NSMobile.stores.pvOrdersPaginated, 'Personal Volume', '<header class="gridHeader PVheader"><h1>My Personal Volumes by Order</h1></header>');
    },
    viewGVOrders: function() {
        // Disabled due to slowness, but we might use it later though.
        //this.updateAndShowKPIOrders(NSMobile.stores.gvOrdersPaginated, 'Group Volume', '<header class="gridHeader PVheader"><h1>My Group Volumes by Order</h1></header>');
    },
    updateAndShowKPIOrders: function (store, title, header) {
        perfOrderStore = store;
        
        Ext.getCmp('perfdetailtoolbar').setTitle(title);
        Ext.getCmp('perfdetailheader').update(header);
        Ext.getCmp('perfdetailheader').doLayout();
        Ext.getCmp('performance').setActiveItem(1);
        
        NSMobile.stores.perfOrders.loadPage(1);
    },
    viewAFL: function () {
        this.updateAndShowPeeps(NSMobile.stores.aflContactsPaginated, 'Sponsored');
    },
    viewDL: function () {
        this.updateAndShowPeeps(NSMobile.stores.dlContactsPaginated, 'Downline');
    },
    updateAndShowPeeps: function (store, title) {
        perfContactStore = store;

        Ext.getCmp('perfcontactlisttoolbar').setTitle('My ' + title);
        Ext.getCmp('performance').setActiveItem(3);

        NSMobile.stores.perfContacts.loadPage(1);
    },
    viewKPIRpt: function (xindex) {
        var kpidata = NSMobile.stores.kpidata.getAt(NSMobile.stores.kpidata.find('periodid', Ext.getCmp('periodselect').getValue()));
        var kpiRpt = kpidata.data.CustomKPIReports[xindex - 1];
        if (kpidata) {
             Ext.getCmp('perfWidgetListToolbar').setTitle(kpiRpt.LandingTitle);
            var KPIRptRows = [];
            for (var i = 0; i < kpiRpt.Rows.length; i++)
                KPIRptRows.push(kpiRpt.Rows[i]);
            NSMobile.stores.kpiRptRows.loadData(KPIRptRows, false);
        }
        Ext.getCmp('performance').setActiveItem(4);
    }
});

Ext.reg('kpi', NSMobile.views.kpi);