NSMobile.views.performance = Ext.extend(Ext.Panel, {
	title: 'Performance',
	iconCls: 'chart4 ' + syncClass,
	baseCls: 'tab-Performance',
    cardSwitchAnimation: false,
    layout: 'card',
    fullscreen: true,
	defaults: {
		styleHtmlContent: false
	},

    initComponent: function () {
        NSMobile.stores.kpidata.load();
        
        // i:0
        var perfPanel = new Ext.TabPanel({
            fullscreen: true,
			ui: 'light',
            tabBar: {
                hidden: true
            },
            items: [
		        {
                    xtype: 'kpi'
		        },
	        ],
            dockedItems: [
	            {
		            xtype: "toolbar",
		            title: "My Performance"
	            }
            ]
        });

        // i:1
        var details = new Ext.Panel({
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            fullscreen: true,
            dockedItems: [
                {
                    id: 'perfdetailtoolbar',
                    xtype: 'toolbar',
                    title: 'Personal Volume',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('performance').layout.prev({type: 'slide', direction: 'right'});
                            }
                        },
                        {
                            xtype: 'spacer'
                        },
                        {
                            id: 'pvlistsortbutton',
                            text: 'Date',
                            handler: function () {
                                pvSortPicker.show();
                            }
                        }
                    ]
                },
				{
					items: [
					    {
                            id: 'perfdetailheader',
						    xtype: 'panel',
						    docked: 'top',
							cls: 'x-list-header',
						    html:'<header class="gridHeader PVheader"><h1>My Personal Volumes by Order</h1></header>'	
					    }
					]
				}
            ],
            items: [
                {
                    flex: 1,
                    xtype: 'list',
					styleHtmlContent: false,
                    id: 'pvlist',
                    //deferEmptyText: false,
                    emptyText: emptyText,
                    store: NSMobile.stores.perfOrders,
                    itemTpl: [
                        '<section class="detailsList ordersList PVdrill">',
                            '<div class="drillDownWrapper">',
                                '<div class="clickableRow row orderData">',
									'<div class="orderInfo"><span class="bTitle orderName">{name}</span> <span class="right">{date:date("m/d/Y")}</span><span class="clr"></span></div>',
									'<ul  class="kpiGroup orderStats">',
										'<li><span class="label pvDisplay">PV:</span> <span class="data">{[values.pv.toFixed(2)]}</span>',
										'<li><span class="label typeDisplay">Type:</span> <span class="data">{type}</span></li>',
									'</ul>',
								'</div>',
                            '</div>',
                        '</section>',
                    ],
                    plugins: [
                        new Ext.plugins.ListPagingPlugin({
                            autoPaging: true
                        })
                    ],
                    listeners: {
                        itemtap: {
                            fn: function (obj, index, item, e) {
                                var data = NSMobile.stores.perfOrders.getAt(index).data;
                                orderDetail.update(data);
                                Ext.getCmp('performance').setActiveItem(2);
                            }
                        }
                    }
                }
            ]
        });

        pvSortPicker = new Ext.Picker({
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
                        { text: 'PV', value: 'PV' },
                        { text: 'Type', value: 'Type' }
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
                        NSMobile.stores.perfOrders.sorters.clear();
                        NSMobile.stores.perfOrders.sort(values.property.toLowerCase(), values.direction);
                        Ext.getCmp('pvlistsortbutton').setText(values.property);
                    }
                }
            },
            value: {
                property: 'Date',
                direction: 'ASC'
            }
        });

        // i:2
        var orderDetail = new Ext.Panel({
            scroll: 'vertical',
            id: 'kpiorderdetail',
            styleHtmlContent: false,
			cls: 'Orders',
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
                    		'<li><p><span class="label">Type:</span> <span class="data">{type}</span></p></li>',
                    		'<li><p><span class="label">PV:</span> <span class="data">{[values.pv.toFixed(2)]}</span></p></li>',
                    		'<li><p><span class="label">Total:</span> <span class="data">${[values.total.toFixed(2)]}</span></p></li>',
						'</ul>',
						'</section>',
					'</div>',
                '</div>'
            ],
            dockedItems: [
                {
                    id: 'kpiorderdetailtoolbar',
                    xtype: 'toolbar',
                    title: 'Order Details',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('performance').layout.prev({type: 'slide', direction: 'right'});
                            }
                        }
                    ]
                }
            ]
        });

        // i:3
        var peeps = new Ext.Panel({
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            fullscreen: true,
            dockedItems: [
                {
                    id: 'perfcontactlisttoolbar',
                    xtype: 'toolbar',
                    title: 'PV',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('performance').setActiveItem(0, pageBackAnimation);
                            }
                        },
                        {
                            xtype: 'spacer'
                        },
                        {
                            id: 'peepslistsortbutton',
                            text: 'Standard',
                            handler: function () {
                                peepsSortPicker.show();
                            }
                        }
                    ]
                }
            ],
            items: [
                {
                    flex: 1,
                    xtype: 'list',
                    id: 'perfcontactlist',
                    //deferEmptyText: false,
					styleHtmlContent: false,
                    emptyText: emptyText,
                    store: NSMobile.stores.perfContacts,
                    itemTpl: [
                        '<section class="KPIdrill teamList">',
                            '<p class="row">',
                                '<span class="KPIdata">',
                                    '<span class="left">{firstName} {lastName}</span> <span class="right"> <label>Level</label> {level}</span><span class="clr"></span>',
                                	'<span  class="dataDisplayRow">',
										'<span><label>PV</label> {[values.pv.toFixed(2)]}</span> <span class="alignRight"><label>GV</label> {[values.gv.toFixed(2)]}</span>',
										'<br class="clr"/>',
									'</span>',
                            	'</span>',
							'</p>',
                        '</section>'
                    ],
                    plugins: [
                        new Ext.plugins.ListPagingPlugin({
                            autoPaging: true
                        })
                    ],
                    listeners: {
                        itemtap: {
                            fn: function (obj, index, item, e) {
                                var data = NSMobile.stores.perfContacts.getAt(index).data;
                                Ext.getCmp('contactdetail').update(data);
                                NSMobile.views.viewport.setActiveItem(2);
                                Ext.getCmp('contactdetail').cameFromPerf = true;
                                Ext.getCmp('network').setActiveItem(1);
                            }
                        }
                    }
                }
            ]
        });

        peepsSortPicker = new Ext.Picker({
            activeCls: 'activePicker',
            cls: 'styledPicker',
            doneButton: 'Sort',
            slots: [
                {
                    name: 'property',
                    title: 'Property',
                    data: [
                        { text: 'Standard', value: 'Standard' },
                        { text: 'Name', value: 'Name' },                        
                        { text: 'PV', value: 'PV' },
                        { text: 'GV', value: 'GV' }
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
                        NSMobile.stores.perfContacts.sorters.clear();

                        if (values.property == 'Name')
                            NSMobile.stores.perfContacts.sort('lastName', values.direction);
                        else
                            NSMobile.stores.perfContacts.sort(values.property.toLowerCase(), values.direction);

                        Ext.getCmp('peepslistsortbutton').setText(values.property);
                    }
                }
            },
            value: {
                property: 'Standard',
                direction: 'ASC'
            }
        });
        
        // i:4
        var widgetList = new Ext.Panel({
            layout: {
	            type: 'vbox',
	            align: 'stretch'
            },
            fullscreen: true,
            dockedItems: [
	            {
	                id: 'perfWidgetListToolbar',
	                xtype: 'toolbar',
	                title: '',
                    items: [
		                {
		                    text: 'Back',
		                    ui: 'back',
		                    handler: function () {
			                    Ext.getCmp('performance').setActiveItem(0);
		                    }
		                }
                    ]
                }
            ],
            items: [
                {
                    flex: 1,
                    xtype: 'list',
					styleHtmlContent: false,
                    id: 'widgetlist',
                    emptyText: emptyText,
                    store: NSMobile.stores.kpiRptRows,
                    itemTpl: [
                        '<section class="KPIdrill">',
                            '<p class="row">',
                                '<span class="KPIdata">',
                                	'<ul  class="kpiGroup orderStats">',
                                            '<tpl for="Cells">',
                                                '<tpl if="IsOnSummary == true">',
										            '<li><span>{Name}:</span> <span class="data">{Value}</span>',
                                                '</tpl>',
                                            '</tpl>',
									'</ul>',
                            	'</span>',
							'</p>',
                        '</section>'
                    ],
                    listeners: {
                        itemtap: {
                            fn: function (obj, index, item, e) {
                                var data = NSMobile.stores.kpiRptRows.getAt(index).data;
                                widgetDetail.update(data);
                                Ext.getCmp('performance').setActiveItem(5);
                            }
                        }
                    }
                }
            ]
        });

        // i:5
        var widgetDetail = new Ext.Panel({
            scroll: 'vertical',
            id: 'perfWidgetDetail',
            styleHtmlContent: false,
			//cls: 'Orders',
            tpl: [
                '<div class="widgetDetailPage">',
					'<div>',
						'<section class="basicInfo">',
						'<ul class="dataList staticList">',
                    		'<tpl for="Cells">',
								'<li><span>{Name}:</span> <span class="data">{Value}</span>',
                            '</tpl>',
						'</ul>',
						'</section>',
					'</div>',
                '</div>'
            ],
            dockedItems: [
                {
                    id: 'perfWidgetDetailToolbar',
                    xtype: 'toolbar',
                    title: 'Details',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('performance').layout.prev({type: 'slide', direction: 'right'});
                            }
                        }
                    ]
                }
            ]
        });

        Ext.apply(this, {
            items: [
                perfPanel,
                details,
                orderDetail,
                peeps,
                widgetList,
                widgetDetail
            ]
        });

        NSMobile.views.performance.superclass.initComponent.apply(this, arguments);
    }
});

Ext.reg('performance', NSMobile.views.performance);

var pvSortPicker, peepsSortPicker;