NSMobile.views.network = Ext.extend(Ext.Panel, {
    layout: 'card',
    iconCls: 'user_list ' + syncClass,
    title: 'Network',
    cls: 'Network',
    cardSwitchAnimation: false,
    initComponent: function () {
        NSMobile.stores.teamContactsPaginated.load();
        NSMobile.stores.customerContactsPaginated.load();
        NSMobile.stores.prospectContactsPaginated.load();
        var contactPanel = new Ext.TabPanel({
            cardSwitchAnimation: false,
            id: 'contactpanel',
            ui: 'dark',
			padding: '0',
			margin: '0',
			centered: true,
			style: 'font-size:.9em;',
            items: [
                {
                    xtype: 'networkteam'
                },
                {
                    xtype: 'networkcustomers'
                },
                {
                    xtype: 'networkprospects'
                },
	        ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'My Network',
                    items: [
                        {
                            xtype: 'spacer'
                        },
                        {
                            hidden: true,
                            iconMask: true,
                            iconCls: 'search'
                        }
                    ]
                },
                {
                    dock: 'bottom',
                    xtype: 'toolbar',
                    id: 'networksearchbar',
                    items: [
                        {
                            xtype: 'spacer'
                        },
                        {
                            xtype: 'searchfield',
                            placeHolder: 'Search',
                            id: 'networksearch',
                            width: '98%',
                            listeners: {
                                change: function (field, newValue, oldValue) {
                                    NSMobile.stores.teamContactsPaginated.clearFilter();
                                    NSMobile.stores.customerContactsPaginated.clearFilter();
                                    NSMobile.stores.prospectContactsPaginated.clearFilter();

                                    NSMobile.stores.teamContactsPaginated.sorters.clear();
                                    NSMobile.stores.customerContactsPaginated.sorters.clear();
                                    NSMobile.stores.prospectContactsPaginated.sorters.clear();

                                    if (newValue) {
                                        NSMobile.stores.teamContactsPaginated.filter('name', newValue);
                                        NSMobile.stores.customerContactsPaginated.filter('name', newValue);
                                        NSMobile.stores.prospectContactsPaginated.filter('name', newValue);
                                    }
                                    else if(newValue != oldValue) {
                                        NSMobile.stores.teamContactsPaginated.loadPage(1);
                                        NSMobile.stores.customerContactsPaginated.loadPage(1);
                                        NSMobile.stores.prospectContactsPaginated.loadPage(1);
                                    }
                                }
                            }
                        },
                        {
                            xtype: 'spacer'
                        }
                    ]
                }
            ],
            defaults: {
                cls: 'networkList'
            }
        });

        var contactDetail = new Ext.Panel({
            scroll: 'vertical',
            id: 'contactdetail',
            cls: 'networkDetails',
            tpl: [
                '<div id="contactInfo" class="detail contactProfile">',
                    '<header class="snapShotInfo contactHeader">',
						'<div class="headerData">',
							'<div class="profileThumb"><img src="{thumb}" /></div>',
							'<h2>',
								'<span class="nameDisplay">{firstName} {lastName}</span>',
								'<span class="titleDisplay">{title} *</span>',
							'<h2>',
						'</div>',
					'</header>',
					// Non-personal contact data
					'<tpl if="showCommissions == true">',
						'<section class="basicInfo">',
							// Static list - No drilldown
							'<ul class="dataList staticList">',
									// Earnings
									'<li>',
									'<span class="label">Earnings:</span>',
									'<div class="kpiGroup">',
										'<p><span class="kpiName">PV:</span><span class="kpiData">{[values.pv.toFixed(2)]}</span></p>',
										'<p><span class="kpiName">GV:</span><span class="kpiData">{[values.gv.toFixed(2)]}</span></p>',
									'</div>',
									'</li>',
                                    '<li><span class="label">Sponsor:</span><span class="data">{sponsor}</span></li>',
									'<li><span class="label">Address:</span><span class="data address">{address}</span></li>',
								
							'</ul>',
						'</section>',
						'<section class="drillDownWrapper contactLinks">',
							// Drill down data	
							'<ul class="dataList drillDownList">',
								'<li class="clickableRow" onclick="javascript:location.href=\'mailto:{email}\';">',
									'<div class="crcw">',
										'<span class="label">Email:</span><span class="data"><a href="mailto:{email}">{email}</a></span>',
									'</div>',
								'</li>',
								'<li class="clickableRow" onclick="javascript:location.href=\'tel:{[values.homephone.replace(/[() -]/g, "")]}\'">',
									'<div class="crcw">',
										'<span class="label">Home:</span><span class="data"><a href="tel:{[values.homephone.replace(/[() -]/g, "")]}">{homephone}</a></span><tpl if="commpref == \'home\'"><span class="perferred"></span></tpl>',
									'</div>',
								'</li>',
								'<li class="clickableRow" onclick="javascript:location.href=\'tel:{[values.mobilephone.replace(/[() -]/g, "")]}\'">',
									'<div class="crcw">',
										'<span class="label">Mobile:</span><span class="data"><a href="tel:{[values.mobilephone.replace(/[() -]/g, "")]}">{mobilephone}</a></span><tpl if="commpref == \'mobile\'"><span class="perferred"></span></tpl>',
									'</div>',
								'</li>',
								'<li class="clickableRow" onclick="javascript:location.href=\'tel:{[values.workphone.replace(/[() -]/g, "")]}\'">',
									'<div class="crcw">',
										'<span class="label">Work:</span><span class="data"><a href="tel:{[values.workphone.replace(/[() -]/g, "")]}">{workphone}</a></span><tpl if="commpref == \'work\'"><span class="perferred"></span></tpl>',
									'</div>',
								'</li>',
							'</ul>',
						'</section>',
					'</tpl>',
					// Personal contact data
					'<tpl if="showCommissions == false">',
						'<section class="detailsList contactDetails">',
							'<ul class="dataList drillDownList contactDetails">',
							
								'<li><p><span class="label">Address:</span><span class="data">{address}</span></p></li>',
								'<li><p><span class="label">Email:</span><span class="data"><a href="mailto:{email}">{email}</a></span></p></li>',
								'<li><p><span class="label">Home:</span><span class="data"><a href="tel:{[values.homephone.replace(/[() -]/g, "")]}">{homephone}</a></span><tpl if="commpref == \'home\'"><span class="perferred"></span></tpl></p></li>',
								'<li><p><span class="label">Mobile:</span><span class="data"><a href="tel:{[values.mobilephone.replace(/[() -]/g, "")]}">{mobilephone}</a></span><tpl if="commpref == \'mobile\'"><span class="perferred"></span></tpl></p></li>',
								'<li><p><span class="label">Work:</span><span class="data"><a href="tel:{[values.workphone.replace(/[() -]/g, "")]}">{workphone}</a></span><tpl if="commpref == \'work\'"><span class="perferred"></span></tpl></p></li>',
								'<li><p><span class="label">Birthdate:</span><span class="data">{birthdate:date("m/d")}</span></p></li>',
								'<li><p><span class="label">Last Purchase:</span><span class="data">{lastpurchasedate:date("m/d/Y")}</span></p></li>',
							
							'</ul>',
						'</section>',
					'</tpl>',
                '</div>'
            ],
            dockedItems: [
                {
                    id: 'contactdetailtoolbar',
                    xtype: 'toolbar',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                var detail = Ext.getCmp('contactdetail');
                                if (detail.cameFromPerf) {
                                    NSMobile.views.viewport.setActiveItem(1);
                                    Ext.getCmp('network').setActiveItem(1);
                                    Ext.getCmp('network').setActiveItem(0);
                                }
                                else
                                    Ext.getCmp('network').layout.prev(pageBackAnimation);
                                detail.cameFromPerf = false;
                            }
                        }
                    ]
                }
            ],
            cameFromPerf: false
        });
        Ext.apply(this, {
            items: [
                contactPanel,
                contactDetail
            ]
        });

        NSMobile.views.network.superclass.initComponent.apply(this, arguments);
    }
});

Ext.reg('network', NSMobile.views.network);