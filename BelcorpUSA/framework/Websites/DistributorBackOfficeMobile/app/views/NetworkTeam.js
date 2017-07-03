NSMobile.views.networkteam = Ext.extend(NSMobile.views.contacts, {
	title: 'My Team',
	store: NSMobile.stores.teamContactsPaginated,
    plugins: [
        new Ext.plugins.ListPagingPlugin({
            autoPaging: true
        })
    ]
});

Ext.reg('networkteam', NSMobile.views.networkteam);