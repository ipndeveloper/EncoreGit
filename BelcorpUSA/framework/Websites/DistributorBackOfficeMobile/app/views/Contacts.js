NSMobile.views.contacts = Ext.extend(Ext.List, {
    title: 'Contacts',
    itemTpl: '<span class="clickIcon"></span><input type="hidden" name="accountid" value="{accountid}" /> {firstName} {lastName}',
    grouped: true,
    indexBar: false,
    maxItemHeight: 75,
    blockScrollSelect: true,
    batchSize: 10,
    useGroupHeaders: true,
    cls: 'network',
    //deferEmptyText: false,
    pinHeaders: false,
    emptyText: emptyText,
    store: NSMobile.stores.teamContactsPaginated,
    listeners: {
        itemtap: {
            fn: function (object, index, item, e) {
                if (object.xtype == 'networkcustomers') {
                    var el = item.querySelector('input[name=accountid]');
                    if (el)
                        index = object.store.find('accountid', el.value);
                }

                var data = object.store.getAt(index).data;
                var name = data.firstName[0] + '. ' + data.lastName;
                Ext.getCmp('contactdetailtoolbar').setTitle(name);
                Ext.getCmp('contactdetail').update(data);
                Ext.getCmp('network').setActiveItem(1);
            }
        },
        afterrender: {
            fn: function () {
                if (this.store.data.length == 0 && !this.store.isLoading()) {
                    if (NSMobile.stores.news.isLoading())
                        this.el.dom.querySelector('.x-list-parent').innerHTML = loadingText;
                    else
                        this.el.dom.querySelector('.x-list-parent').innerHTML = emptyText;
                }
            }
        }
    }

});

Ext.reg('contacts', NSMobile.views.contacts);