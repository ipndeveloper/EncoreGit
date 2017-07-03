// scrollsaver.js
// Copyright (C) 2009 M. Mahdi Hasheminezhad (hasheminezhad at gmail dot com)
// Maintain scroll position of every element on postbacks and partial updates
// This source is licensed under Common Public License Version 1.0 (CPL) 
// History:
// 2009-08-21 First Public Release M. Mahdi Hasheminezhad (http://hasheminezhad.com)

// CJH CHANGE NOTE: I modified the original script to compensate for the page height changing between posts.
// Jquery is now used as well, so be sure it's included on any page using this script.
// I'm also trimming some string keys now that was making IE complain

(function () {
    function loadScroll() {
        var heightAdjustment = 0;
        var cookieList = document.cookie.split(';');
        for (var i = 0; i < cookieList.length; i++) {
            var cookieParts = cookieList[i].split('=');
            if ($.trim(cookieParts[0]) == 'originalHeight') {
                heightAdjustment = $(document).height() -  parseInt(cookieParts[1]);
            }
        }
        for (var i = 0; i < cookieList.length; i++) {
            var cookieParts = cookieList[i].split('=');
            if ($.trim(cookieParts[0]) == 'scrollPosition') {
                var values = unescape(cookieParts[1]).split('/');
                for (var j = 0; j < values.length; j++) {
                    var currentValue = values[j].split(',');
                    try {
                        if (currentValue[0] == 'window') {
                            var height =  parseInt(currentValue[2]);
                            height = height + heightAdjustment;
                            window.scrollTo(currentValue[1], height);
                        } else if (currentValue[0]) {
                            var elm = document.getElementById(currentValue[0]);
                            elm.scrollLeft = currentValue[1];
                            elm.scrollTop = currentValue[2];
                        }
                    } catch (ex) { }
                }
                return;
            }
        }
    }
    function saveScroll() {
        var s = 'scrollPosition=';
        var wl, wt;
        if (window.pageXOffset !== undefined) {
            wl = window.pageXOffset;
            wt = window.pageYOffset;
        } else if (document.documentElement && document.documentElement.scrollLeft !== undefined) {
            wl = document.documentElement.scrollLeft;
            wt = document.documentElement.scrollTop;
        } else {
            wl = document.body.scrollLeft;
            wt = document.body.scrollTop;
        }
        if (wl || wt) {
            s += 'window,' + wl + ',' + wt + '/';
        }
        var elements = (document.all) ? document.all : document.getElementsByTagName('*');
        for (var i = 0; i < elements.length; i++) {
            var e = elements[i];
            if (e.id && (e.scrollLeft || e.scrollTop)) {
                s += e.id + ',' + e.scrollLeft + ',' + e.scrollTop + '/';
            }
        }
        var originalHeight = $(document).height();
        document.cookie = 'originalHeight=' + originalHeight + ';';
        document.cookie = s + ';';
    }
    var addEvent, eventPrefix;
    if (window.attachEvent) {
        addEvent = window.attachEvent;
        eventPrefix = 'on';
    } else {
        addEvent = window.addEventListener;
        eventPrefix = '';
    }
    addEvent(eventPrefix + 'load', function () {
        loadScroll();
        if (typeof Sys != 'undefined' && typeof Sys.WebForms != 'undefined') {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(loadScroll);
            Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(saveScroll);
        }
    }, false);
    addEvent(eventPrefix + 'unload', saveScroll, false);
})();
