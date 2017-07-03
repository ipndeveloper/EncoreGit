NSMobile.views.networkprospects = Ext.extend(NSMobile.views.contacts, {
	title: 'Contacts',
    store: NSMobile.stores.prospectContactsPaginated,
    plugins: [
        new Ext.plugins.ListPagingPlugin({
            autoPaging: true
        })
    ]
});

Ext.reg('networkprospects', NSMobile.views.networkprospects);