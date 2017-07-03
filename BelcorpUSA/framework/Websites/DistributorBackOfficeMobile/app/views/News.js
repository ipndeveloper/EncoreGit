NSMobile.views.News = Ext.extend(Ext.Panel, {
    title: 'Home',
    iconCls: 'home ' + syncClass,
    layout: 'card',
    cardSwitchAnimation: false,
    initComponent: function () {
        NSMobile.stores.local.news.load();
        var newsPanel = new Ext.Panel({
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            items: [
		        {
		            fullscreen: isDesktop,
		            flex: 1,
		            xtype: 'list',
		            grouped: true,
		            cls: 'article',
		            id: 'newslist',
		            //deferEmptyText: false,
		            emptyText: emptyText,
                    singleSelect: true,
                    allowDeselect: false,
		            store: NSMobile.stores.local.news,
		            itemTpl: [
				        '<div class="articlePreview articleType1">',
                            '<input type="hidden" value="{id}" name="newsid" />',
				            '<div class="thumb"><p class="wrap"><img src="{[values.hasValidThumb ? values.thumb : "/lib/resources/themes/images/encore/photo_default.png"]}" onerror="onImgError(this);" /></p></div>',
				            '<div class="title">',
				                '<span class="newsTitle newsTitle{[values.isAlert ? "2" : ""]}{[values.read ? "-viewed" : "" ]}">{title}</span>',
                                '<span class="newsSummary {[values.read ? "newsTitle-viewed" : "" ]}">{summary}</span>',
				            '</div>',
                        '</div>'
			        ],
		            listeners: {
		                itemtap: function (object, index, el, e) {
		                    var idEl = el.querySelector('input[type=hidden]');
		                    if (idEl)
		                        index = object.store.find('id', idEl.value, 0, false, false, true);
		                    var newsItem = object.store.getAt(index);
		                    var data = newsItem.data;
		                    Ext.getCmp('newsdetail').update(data);
		                    if (!data.isAlert)
		                        Ext.getCmp('newsdetailtoolbar').setTitle('News');
		                    else
		                        Ext.getCmp('newsdetailtoolbar').setTitle('Alerts');
		                    Ext.getCmp('news').setActiveItem(1);
		                    newsItem.markRead();
                            object.select(index); //mark read was unselecting if it wasn't marked read yet
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
		            },
		            plugins: [
		                {
		                    ptype: 'pullrefresh',
                            refreshFn: function(callback, plugin) {
                                this.lastUpdated = new Date();
                                newsLoadSpinner = new Ext.LoadMask(Ext.getCmp('news').el.dom);
                                newsLoaded = false;
                                newsLoadSpinner.show();
                                this.list.getStore().load();
                                callback.call(plugin);
                            }
		                }
		            ]
		        }
	        ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Home',
                    items: [
                        {
                            ui: 'back',
                            text: 'Sign Out',
                            handler: function () {
                                Ext.Msg.confirm('Sign Out of Workstation', 'Do you really want to sign out of the Mobile Workstation?', function (button) {
                                    if (button == 'yes')
                                        logout();
                                });
                            }
                        },
                        {
                            xtype: 'spacer'
                        },
                        {
                            iconMask: true,
                            iconCls: 'trash',
                            handler: function () {
                                Ext.getCmp('trash').updateTrashBin();
                                Ext.getCmp('news').setActiveItem(2);
                            }
                        }
                    ]
                }
            ]
        });

        newsPanel.on('cardswitch', function (newCard, oldCard) {
            if (oldCard) {
                this.remove(oldCard, true);
                oldCard.destroy();
            }
        }, newsPanel);

        var newsDetail = new Ext.Panel({
            scroll: 'vertical',
            id: 'newsdetail',
            styleHtmlContent: false,
            baseCls: 'articlePage',
            tpl: [
                '<section class="detail articleView">',
					'<arcticle>',
						'<header class="snapShotInfo">',
							 '<tpl if="isAlert">',
                                '<div class="rightIcon formSubmit"><input type="submit" class="destroyBtn smallBtn delete" onclick="javascript:(function(){Ext.getCmp(\'newsdetail\').deleteAlert()})()" value="Delete" /></div>',
                            '</tpl>',
							'<h2 class="articleTitle">{title}</h2>',
							'<h3 class="articleDate">{date}</h3>',

						'</header>',
						'<section class="articleBody">',
                    		//'<tpl if="!isAlert && hasValidThumb"><p class="articleImg"><img src="{thumb}" /></p></tpl>',
                    		'<section class="articleCopy">{text}</section>',
						'</section>',
					'</article>',
                '</section>'
            ],
            dockedItems: [
                {
                    id: 'newsdetailtoolbar',
                    xtype: 'toolbar',
                    title: 'News',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('news').layout.prev(pageBackAnimation);
                            }
                        }
                    ]
                }
            ],
            deleteAlert: function () {
                NSMobile.stores.local.news.getAt(NSMobile.stores.local.news.find('id', Ext.getCmp('newsdetail').data.id)).hide();
                NSMobile.stores.local.news.clearFilter(true);
                NSMobile.stores.local.news.filter('hidden', false);
                Ext.getCmp('news').layout.prev(pageBackAnimation);
            }
        });

        newsDetail.on('cardswitch', function (newCard, oldCard) {
            if (oldCard) {
                this.remove(oldCard, true);
                oldCard.destroy();
            }
        }, newsDetail);

        var settingsPanel = new Ext.Panel({
            id: 'settings',
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Settings',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('news').setActiveItem(0, pageBackAnimation);
                            }
                        }
                    ]
                }
            ],
            html: [
                '<section class="partyStats inputForm optionsPanel">',
                    '<div class="detailRow inputRow">',
                        '<span class="label">Reset hidden news:</span>',
						'<div class="rightIcon formSubmit">',
                        '<input type="button" click="javascript:(function(){Ext.getCmp(\'settings\').resetHiddenNews()})()" value="Reset" />',
						'<span class="clr"></span>',
						'</div>',
                    '</div>',
                '</section>',
				'<section class="formSubmit">',
					'<input type="submit" onclick="javascript:(function(){logout()})()" value="Sign Out" class="destroyBtn" />',
				'</section>',
            ],
            resetHiddenNews: function () {
                NSMobile.stores.local.news.clearFilter(true);
                var storageData = getStorage('news');
                for (var key in storageData) {
                    var index = NSMobile.stores.news.find('id', key);
                    if (index > -1) {
                        var newsObj = NSMobile.stores.local.news.getAt(index).data;
                        storageData[key].hidden = newsObj.hidden = false;
                    }
                }
                setStorage('news', storageData);
                NSMobile.stores.local.news.filter('hidden', false);
            }
        });

        settingsPanel.on('cardswitch', function (newCard, oldCard) {
            if (oldCard) {
                this.remove(oldCard, true);
                oldCard.destroy();
            }
        }, settingsPanel);


        var trashBin = new Ext.Panel({
            id: 'trash',
            scroll: 'vertical',
            xtype: 'list',
            baseCls: 'x-list',
            dockedItems: [
                {
                    xtype: 'toolbar',
                    title: 'Trash Bin',
                    items: [
                        {
                            text: 'Back',
                            ui: 'back',
                            handler: function () {
                                Ext.getCmp('news').setActiveItem(0, pageBackAnimation);
                            }
                        },
                        {
                            xtype: 'spacer'
                        },
                        {
                            text: 'Restore All',
                            handler: function () {
                                Ext.Msg.confirm('Restore Alerts', 'Do you really want to restore all of the alerts in your trash bin?', function (button) {
                                    if (button == 'yes')
                                        Ext.getCmp('trash').resetHiddenNews();
                                });
                            }
                        }
                    ]
                }
            ],
            tpl: [
                '<tpl for=".">',
				'<div class="x-list-item">',
					'<div class="x-list-item-body">',
						'<div class="articlePreview trashArticle">',
							'<div class="thumb"><img src="{thumb}"/></div>',
							'<div class="title">',
								'<span class="newsTitle newsTitle{[values.isAlert ? "2" : ""]}{[values.read ? "-viewed" : "" ]}">{title}</span>',
							'</div>',
							'<div class="rightIcon formSubmit"><input type="button" class="restore smallBtn" onclick="javascript:(function(){Ext.getCmp(\'trash\').restoreNewsItem(\'{id}\')})()" value="restore"/></div>',
						'</div>',
                	'</div>',
				'</div>',
				'</tpl>'
			],
            restoreNewsItem: function (id) {
                NSMobile.stores.local.news.clearFilter(true);
                var storageData = getStorage('news');
                var index = NSMobile.stores.local.news.find('id', id);
                if (storageData.news[id] && index > -1) {
                    var newsObj = NSMobile.stores.local.news.getAt(index).data;
                    storageData.news[id].hidden = newsObj.hidden = false;
                    setStorage('news', storageData);
                }
                NSMobile.stores.local.news.filter('hidden', false);
                this.updateTrashBin();
            },
            resetHiddenNews: function () {
                NSMobile.stores.local.news.clearFilter(true);
                var storageData = getStorage('news');
                for (var key in storageData.news) {
                    var index = NSMobile.stores.local.news.find('id', key);
                    if (index > -1) {
                        var newsObj = NSMobile.stores.local.news.getAt(index).data;
                        storageData.news[key].hidden = newsObj.hidden = false;
                    }
                }
                setStorage('news', storageData);
                NSMobile.stores.local.news.filter('hidden', false);
                Ext.getCmp('news').setActiveItem(0, pageBackAnimation);
            },
            updateTrashBin: function () {
                NSMobile.stores.local.news.clearFilter(true);
                NSMobile.stores.local.news.load(); //workaround sencha bug
                var hiddenAlerts = [];
                var storageData = getStorage('news');
                for (var key in storageData.news) {
                    if (storageData.news[key].hidden) {
                        var index = NSMobile.stores.local.news.find('id', key);
                        if (index > -1) {
                            var newsObj = NSMobile.stores.local.news.getAt(index).data;
                            hiddenAlerts.push(newsObj);
                        }
                    }
                }
                this.update(hiddenAlerts);
                NSMobile.stores.local.news.filter('hidden', false);
            }
        });

        trashBin.on('cardswitch', function (newCard, oldCard) {
            if (oldCard) {
                this.remove(oldCard, true);
                oldCard.destroy();
            }
        }, trashBin);

        Ext.apply(this, {
            items: [
                newsPanel,
                newsDetail,
                trashBin
            ]
        });
        NSMobile.views.News.superclass.initComponent.apply(this, arguments);
    }
});

Ext.reg('news', NSMobile.views.News);