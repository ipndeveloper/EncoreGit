NSMobile.views.Viewport = Ext.extend(Ext.TabPanel, {
    fullscreen: true,
    cardSwitchAnimation: false,
    tabBar: {
        dock: 'bottom',
        layout: {
            pack: 'center'
        }
    },
    defaults: {
        styleHtmlContent: false
    },
    items: [
		{
		    xtype: 'news',
		    id: 'news'
		},
		{
		    xtype: 'performance',
		    id: 'performance'
		},
		{
		    xtype: 'network',
		    id: 'network',
		    listeners: {
		        activate: function () {
		            if (networkSorted) {
		                NSMobile.stores.prospectContactsPaginated.clearFilter();
		                NSMobile.stores.teamContactsPaginated.clearFilter();
		                NSMobile.stores.customerContactsPaginated.clearFilter();

		                NSMobile.stores.prospectContactsPaginated.sorters.clear();
		                NSMobile.stores.teamContactsPaginated.sorters.clear();
		                NSMobile.stores.customerContactsPaginated.sorters.clear();

		                NSMobile.stores.prospectContactsPaginated.loadPage(1);
		                NSMobile.stores.teamContactsPaginated.loadPage(1);
		                NSMobile.stores.customerContactsPaginated.loadPage(1);

		                networkSorted = false;
		            }
		        }
		    }
		},
        {
            xtype: 'orders',
            id: 'orders',
            listeners: {
                activate: function () {
                    if (showOrdersToolbar) {
                        Ext.getCmp('orders').items.getAt(0).show();
                        showOrdersToolbar = false;
                    }
                }
            }
        }
	],
    listeners: {
        afterrender: {
            fn: function () {
                Ext.getCmp('news').tab.setBadge(unreadAlerts + unreadNews);
            }
        }
    }
});

var showOrdersToolbar = true, showPartiesToolbar = true;