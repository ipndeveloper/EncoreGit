/* Use this script if you need to support IE 7 and IE 6. */

window.onload = function () {
    function addIcon(el, entity) {
        var html = el.innerHTML;
        el.innerHTML = '<span style="font-family: \'encoreWkstnIcons\'" class="ie7Icon">' + entity + '</span>' + html;
    }
    var icons = {
        'icon-plus': '&#x271a;',
        'icon-arrowDown': '&#x25bc;',
        'icon-arrowNext-circle': '&#x25b6;',
        'icon-arrowPrev-circle': '&#x25c4;',
        'icon-arrowUp': '&#x25b2;',
        'icon-arrow-down': '&#x25be;',
        'icon-Attach': '&#x21d5;',
        'icon-cancelled': '&#x2717;',
        'icon-check': '&#x2714;',
        'icon-deactive': '&#x2612;',
        'icon-x': '&#x2718;',
        'icon-deleteReportLink': '&#x2718;',
        'icon-deleteSelected': '&#x2613;',
        'icon-trash': '&#x2613;',
        'icon-exclamation': '&#xe020;',
        'icon-arrowPrev': '&#x21e6;',
        'icon-arrowNext': '&#x21e8;',
        'icon-arrowgoBack': '&#x21e6;',
        'icon-arrowgoForward': '&#x21e8;',
        'icon-forward': '&#x21e8;',
        'icon-actionLock': '&#xe000;',
        'icon-plusAlt': '&#x2719;',
        'icon-print': '&#x2338;',
        'icon-refresh': '&#x21ba;',
        'icon-search': '&#x260c;',
        'icon-searchIcon': '&#x260c;',
        'icon-navbarSearch': '&#x260c;',
        'icon-upload': '&#xe001;',
        'icon-file': '&#x2630;',
        'icon-fileAudio': '&#xe002;',
        'icon-fileFlash': '&#xe003;',
        'icon-fileImage': '&#xe004;',
        'icon-filePdf': '&#xe005;',
        'icon-filePowerpoint': '&#xe006;',
        'icon-fileVideo': '&#xe007;',
        'icon-exportToExcel': '&#xe008;',
        'icon-edit': '&#x270f;',
        'icon-actionDashTools': '&#xe009;',
        'icon-actionContactAdd': '&#x263a;',
        'icon-actionParty': '&#xe00a;',
        'icon-actionImport': '&#xe00b;',
        'icon-actionReport': '&#xe00c;',
        'icon-save': '&#xe00d;',
        'icon-email': '&#x2709;',
        'icon-emailAllLink': '&#x2709;',
        'icon-emailLink': '&#xe00e;',
        'icon-actionTreeView': '&#xe00f;',
        'icon-chooseVisibleColumns': '&#x25eb;',
        'icon-groupByAccount': '&#xe010;',
        'icon-mergeUsers': '&#xe010;',
        'icon-info': '&#xe011;',
        'icon-actionPersonalOrder': '&#x260b;',
        'icon-actionPartyOrder': '&#xe012;',
        'icon-actionCompose': '&#xe013;',
        'icon-actionComposeLetter': '&#x270d;',
        'icon-pageEdit': '&#x270d;',
        'icon-actionNewsletter': '&#x2633;',
        'icon-actionDashPWS': '&#xe014;',
        'icon-template': '&#xe015;',
        'icon-saveReportLink': '&#xe016;',
        'icon-actionFlatReport': '&#x2637;',
        'icon-help': '&#xe017;',
        'icon-editPerson': '&#xe018;',
        'icon-directShipOff': '&#xe019;',
        'icon-directShipOn': '&#xe019;',
        'icon-bundle-add': '&#xe01a;',
        'icon-bundle-arrow': '&#xe01b;',
        'icon-bundle-full': '&#xe01c;',
        'icon-replyAll': '&#xe01d;',
        'icon-reply': '&#xe01e;',
        'icon-eventCalendar': '&#xe01f',
        'icon-actionEvent': '&#xe01f;',
        'icon-pageAdd': '&#xe020;',
        'icon-pageMin': '&#xe021;',
        'icon-pageSearch': '&#xe022;',
        'icon-pageX': '&#xe023;',
        'icon-actionGlobe': '&#xe024;',
        'icon-actionAutoShip': '&#xe025;',
        'icon-actionHome': '&#xe026;'
    },
		els = document.getElementsByTagName('*'),
        exp1 = new RegExp(/icon-[^\s'"]+/),
        exp2 = new RegExp(/DTL\s[^\s'"]+/),
        exp3 = new RegExp(/SearchIcon/),
        i, attr, html, c, el;
    for (i = 0; i < els.length; i += 1) {
        el = els[i];
        attr = el.getAttribute('data-icon');
        if (attr) {
            addIcon(el, attr);
        }
        c = el.className;
        if (exp1.test(c)) {
            c = exp1.exec(c);
        }
        else if (exp2.test(c)) {
            c = exp2.exec(c);
        }
        else if (exp3.test(c)) {
            c = exp3.exec(c);
        }
        else {
            c = null;
        }

        if (c) {
            if (icons[c[0]] != null) {
                addIcon(el, icons[c[0]]);
            }
        }
    }
};