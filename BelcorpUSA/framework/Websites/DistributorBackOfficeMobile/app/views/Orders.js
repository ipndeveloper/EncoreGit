NSMobile.views.orders = Ext.extend(Ext.Panel, {
    title: 'Orders',
    iconCls: 'list ' + syncClass,
    baseCls: 'Orders',
    layout: 'card',
    cardSwitchAnimation: false,
    initComponent: function () {
        NSMobile.stores.orders.load({ params: { lastUpdate: new Date()} }, false);

        var orders = new Ext.Panel({
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            fullscreen: true,
            hidden: true,
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Orders',
                    items: [
                        {
                            xtype: 'spacer'
                        },
                        {
                            id: 'orderlistsortbutton',
                            text: 'Date',
                            handler: function () {
                                ordersSortPicker.show();
                            }
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    cls: 'x-list-header',
                    align: 'center',
                    style: 'margin-bottom:0',
                    layout: {
                        pack: 'center'
                    },
                    items: [
                        {
                            id: 'orderdatepicker',
                            xtype: 'datepickerfield',
                            cls: 'styledPicker',
                            picker: {
                                yearFrom: 2010,
                                slotOrder: ['month', 'year'],
                                activeCls: 'activePicker'
                            },
                            listeners: {
                                change: function (picker, date) {
                                    date.setDate(15); //move date to middle of month; actual day isnt used on the server, but timezone differences can cause 12am clientside to be a different date serverside
                                    Ext.getCmp('orderlist').scroller.scrollTo({ x: 0, y: 0 });
                                    NSMobile.stores.orders.load({ params: { lastUpdate: date} }, false);
                                }
                            }
                        }
                    ]
                }
            ],
            items: [
                {
                    flex: 1,
                    xtype: 'list',
                    //deferEmptyText: false,
                    emptyText: emptyText,
                    store: NSMobile.stores.orders,
                    id: 'orderlist',
                    itemTpl: [
                        '<section class="detailsList ordersList">',
                            '<div class="drillDownWrapper">',
								'<div class="clickableRow row orderData">',
                                	'<div class="orderInfo">',
										'<span class="bTitle orderName">{name}</span>',
										'<span>#{number}</span> <span class="clr">',
									'</div>',
                                	'<ul  class="kpiGroup orderStats">',
										'<li><span class="label">Total:</span> <span class="data">${[values.total.toFixed(2)]}</span></li>',
										'<li><span class="label">Order Date:</span> <span class="data">{date:date("m/d/Y")}</span></li>',
										'<li><span class="label">Status:</span> <span class="data orderStatusDisplay">{status}</span></li>',
									'</ul>',
								'</div>',
                            '</div>',
                        '</section>'
                    ],
                    listeners: {
                        itemtap: {
                            fn: function (obj, index, item, e) {
                                var data = NSMobile.stores.orders.getAt(index).data;
                                orderDetail.update(data);
                                Ext.getCmp('orders').setActiveItem(1);
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
                }
            ]
        });

        ordersSortPicker = new Ext.Picker({
            activeCls: 'activePicker',
            cls: 'styledPicker',
            doneButton: 'Sort',
            slots: [
                {
                    name: 'property',
                    title: 'Property',
                    data: [
                        { text: 'Date', value: 'Date' },
                        { text: 'Name', value: 'Name' },
                        { text: 'Number', value: 'Number' },
                        { text: 'Status', value: 'Status' },
                        { text: 'Total', value: 'Total' }
                    ]
                },
                {
                    name: 'direction',
                    title: 'Direction',
                    data: [
                        { text: 'Asc', value: 'ASC' },
                        { text: 'Desc', value: 'DESC' }
                    ]
                }
            ],
            listeners: {
                change: {
                    fn: function (picker, values) {
                        NSMobile.stores.orders.sorters.clear();
                        NSMobile.stores.orders.sort(values.property.toLowerCase(), values.direction);
                        Ext.getCmp('orderlistsortbutton').setText(values.property);
                    }
                }
            },
            value: {
                property: 'Date',
                direction: 'DESC'
            }
        });

        var orderDetail = new Ext.Panel({
            scroll: 'vertical',
            id: 'orderdetail',
            styleHtmlContent: false,
            cls: 'Orders',
            fullscreen: true,
            hidden: true,
            tpl: [
                '<div class="orderDetailPage">',
					'<div>',
						'<header class="snapShotInfo orderHeader">',
							'<h2 class="articleTitle">Order #{number}</h2>',
							'<h3 class="articleDate">{date:date("m/d/Y")}</h3>',
						'</header>',
						'<section class="basicInfo">',
						'<ul class="dataList staticList">',
                    		'<li><p><span class="label">Name:</span> <span class="data">{name}</span></p></li>',
                    		'<li><p><span class="label">Type:</span> <span class="data ">{type}</span></p></li>',
                    		'<li><p><span class="label">Total:</span> <span class="data">${[values.total.toFixed(2)]}</span></p></li>',
                    		'<li><p><span class="label">Status:</span> <span class="data">{status}</span></p></li>',
                    		'<li><p><span class="label">Date Completed:</span> <span class="data">{completedate}</span></p></li>',
                            '<li><p><span class="label">Tracking:</span> <span class="data">',
                                '<tpl for="trackingnumbers">',
                                    '<a href="{[parent.trackingurls[xindex - 1]]}" target="_blank">{.}</a> ',
                                '</tpl>',
                            '</span></p></li>',
						'</ul>',	
						'</section>',
					'</div>',
                '</div>'
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Order Details',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('orders').layout.prev(pageBackAnimation);
                            }
                        }
                    ]
                }
            ]
        });

        Ext.apply(this, {
            items: [
                orders,
                orderDetail
            ]
        });

        var defaultDate = new Date();
        Ext.getCmp('orderdatepicker').setValue(defaultDate, true);

        NSMobile.views.orders.superclass.initComponent.apply(this, arguments);
    }
});

Ext.reg('orders', NSMobile.views.orders);

var ordersSortPicker;