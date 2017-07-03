NSMobile.models.NewsItem = Ext.regModel('NewsItem', {
    fields: [
        { name: 'id', type: 'int' },
        { name: 'date', type: 'string' },
        { name: 'title', type: 'string' },
        { name: 'text', type: 'string' },
        { name: 'type', type: 'int' },
        'typename',
		{ name: 'summary', type: 'string' },
        { name: 'thumb', type: 'string' },
        { name: 'hasValidThumb', type: 'boolean', defaultValue: false },
        { name: 'read', type: 'boolean', defaultValue: false },
        { name: 'hidden', type: 'boolean', defaultValue: false },
        'language',
        'country',
        'site',
        { name: 'sort', type: 'int' },
        { name: 'isAlert', type: 'boolean', defaultValue: false },
        { name: 'isFeatured', type: 'boolean', defaultValue: false }
    ],
    markRead: function () {
        if (!this.data.read) {
            var auth = getStorage('auth');
            var currentuser = auth.currentuser;
            var metaData = getStorage('newsmetadata');
            var newsMetaData = metaData[currentuser] || {};
            var thisNewsMetaData = newsMetaData[this.data.id] || {};

            var storageData = getStorage('news');

            var newsStatus = storageData.news[this.data.id] || {};
            newsStatus.read = true;
            Ext.apply(thisNewsMetaData, { 'read': true });

            if (this.data.isAlert)
                unreadAlerts--;
            else {
                unreadNews--;
                var dictVal = unreadNewsByType[this.data.type];
                if (dictVal > 1) {
                    unreadNewsByType[this.data.type] = dictVal - 1;
                }
                else {
                    unreadNewsByType[this.data.type] = 0;
                }
            }

            var newsTab = Ext.getCmp('news').tab;
            if (unreadAlerts + unreadNews > 0)
                newsTab.setBadge(unreadAlerts + unreadNews);
            else
                newsTab.setBadge('');

            var newClass = 'newsTitle newsTitle2-viewed';
            this.data.read = true;

            newsMetaData[this.data.id] = thisNewsMetaData;
            metaData[currentuser] = newsMetaData;
            storageData.news[this.data.id] = newsStatus;
            setStorage('news', storageData);
            setStorage('newsmetadata', metaData);
            Ext.getCmp('newslist').refresh();
        }
    },
    hide: function () {
        var auth = getStorage('auth');
        var currentuser = auth.currentuser;
        var metaData = getStorage('newsmetadata');
        var newsMetaData = metaData[currentuser] || {};
        var thisNewsMetaData = newsMetaData[this.data.id] || {};

        var storageData = getStorage('news');
        var newsStatus = storageData.news[this.data.id] || {};

        newsStatus.hidden = this.data.hidden = true;
        Ext.apply(thisNewsMetaData, { 'hidden': true });

        storageData.news[this.data.id] = newsStatus;
        newsMetaData[this.data.id] = thisNewsMetaData;
        metaData[currentuser] = newsMetaData;
        setStorage('news', storageData);
        setStorage('newsmetadata', metaData);
        Ext.getCmp('newslist').refresh();
    }
});

NSMobile.stores.news = new Ext.data.Store({
    model: 'NewsItem',
    proxy: {
        type: 'ajax',
        url: serviceURI + '/GetNews',
        listeners: {
            exception: {
                fn: function (proxy, response, operation) {
                    showErrorOnce();
                }
            }
        },
        timeout: ajaxTimeout
    },
    listeners: {
        beforeload: {
            fn: function () {
                setSyncIcon('home', true);
            }
        },
        load: {
            fn: function (store, records, successful) {
                var auth = getStorage('auth');
                var currentuser = auth.currentuser;
                var metaData = getStorage('newsmetadata');
                var newsMetaData = metaData[currentuser] || {};

                var storageData = getStorage('news');
                storageData.news = storageData.news || {};
                var news = {};
                for (var i = 0; i < NSMobile.stores.news.data.length; i++) {
                    var newsItem = NSMobile.stores.news.getAt(i);
                    if (storageData.news[newsItem.data.id] || newsMetaData[newsItem.data.id]) {
                        newsItem.data.read = (storageData.news[newsItem.data.id] && storageData.news[newsItem.data.id].read) || (newsMetaData[newsItem.data.id] && newsMetaData[newsItem.data.id].read);
                        newsItem.data.hidden = (storageData.news[newsItem.data.id] && storageData.news[newsItem.data.id].hidden) || (newsMetaData[newsItem.data.id] && newsMetaData[newsItem.data.id].hidden);
                    }
                    news[newsItem.data.id] = newsItem.data;
                }
                // reset locally stored news items to make sure news from returned from the service matches what's stored locally
                storageData.news = {};
                Ext.apply(storageData.news, news);
                storageData.lastUpdate = new Date();
                setStorage('news', storageData);
                newsLoaded = true;
                NSMobile.stores.local.news.load();
              
                if (newsLoadSpinner) {
                    newsLoadSpinner.hide();
                }
                setSyncIcon('home', false);
            }
        }
    }
});

NSMobile.stores.local.news = new Ext.data.Store({
    model: 'NewsItem',
    proxy: {
        type: 'memory',
        reader: {
            type: 'json'
        },
        read: function (operation, callback, scope) {
            var storageData = getStorage('news');
            var news = [];
            unreadAlerts = 0;
            unreadNews = 0;
            unreadNewsByType = {};

            for (var key in storageData.news) {
                var newsItem = storageData.news[key];
                var dateArr = newsItem.date.split('-');
                var date = new Date(dateArr[2], dateArr[1], dateArr[0]);
                if (!newsItem.isAlert || new Date().getMilliseconds() - date.getMilliseconds() < 2592000000) { //30 days
                    news.push(new this.model(newsItem));
                    if (!newsItem.hidden && !newsItem.read) {
                        if (!newsItem.isAlert) {
                            unreadNews++;
                            var dictVal = unreadNewsByType[newsItem.type];
                            if (dictVal > 0) {
                                unreadNewsByType[newsItem.type] = dictVal + 1;
                            }
                            else {
                                unreadNewsByType[newsItem.type] = 1;
                            }
                        }
                        else
                            unreadAlerts++;
                    }
                }
                else
                    storageData.news[key] = undefined;
            }

            if (!newsLoaded) {
                if (news.length > 0)
                    NSMobile.stores.news.getProxy().extraParams.lastUpdate = storageData.lastUpdate;

                NSMobile.stores.news.load();
                NSMobile.stores.news.getProxy().extraParams.lastUpdate = undefined;  
            }

            var newsCmp = Ext.getCmp('news');
            if (newsCmp)
                newsCmp.tab.setBadge(unreadAlerts + unreadNews);

            operation.resultSet = new Ext.data.ResultSet({
                records: news,
                total: news.length,
                loaded: true
            });

            operation.setSuccessful();
            operation.setCompleted();

            if (typeof callback == 'function') {
                callback.call(scope || this, operation);
            }
        }
    },
    sorters: [
        {
            property: 'sort',
            direction: 'asc'
        }
    ],
    listeners: {
        load: {
            fn: function (store, records, successful) {
                store.snapshot = store.data; //workaround sencha bug causing filterBy to clear the store
                NSMobile.stores.news.filter('hidden', false);
            }
        }
    },
    getGroupString: function (record) {
        var groupString = '';
        if (!record.data.isAlert)
            groupString += record.data.typename + ': (<span id=\'newsHeaderCount\'>' + (unreadNewsByType[record.data.type] || 0) + '</span>)';
        else
            groupString += 'Alerts: (<span id=\'alertsHeaderCount\'>' + unreadAlerts + '</span>)';

        return groupString;
    },
    filters: [
        {
            property: 'hidden',
            value: false
        }
    ]
});

var unreadAlerts = 0, unreadNews = 0, newsLoaded = false, unreadNewsByType = {}, newsLoadSpinner;
