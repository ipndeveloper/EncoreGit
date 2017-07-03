NSMobile.views.networkcustomers = Ext.extend(NSMobile.views.contacts, {
    title: 'Customers',
    store: NSMobile.stores.customerContactsPaginated,
    plugins: [
        new Ext.plugins.ListPagingPlugin({
            autoPaging: true
        })
    ]
});

Ext.reg('networkcustomers', NSMobile.views.networkcustomers);