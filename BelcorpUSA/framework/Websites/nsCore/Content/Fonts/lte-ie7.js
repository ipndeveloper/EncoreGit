/* Load this script using conditional IE comments if you need to support IE 7 and IE 6. */

window.onload = function() {
	function addIcon(el, entity) {
		var html = el.innerHTML;
		el.innerHTML = '<span style="font-family: \'icomoon\'">' + entity + '</span>' + html;
	}
	var icons = {
			'icon-Add' : '&#x271a;',
			'icon-arrowDown-circle' : '&#x25bc;',
			'icon-arrowNext-circle' : '&#x25b6;',
			'icon-arrowPrev-circle' : '&#x25c4;',
			'icon-arrowUp-circle' : '&#x25b2;',
			'icon-ArrowDown' : '&#x25bd;',
			'icon-ArrowNext' : '&#x25b7;',
			'icon-ArrowPrev' : '&#x25c1;',
			'icon-arrow-down' : '&#x25be;',
			'icon-Attach' : '&#x21d5;',
			'icon-cancelled' : '&#x2717;',
			'icon-check' : '&#x2714;',
			'icon-deactive' : '&#x2612;',
			'icon-Delete' : '&#x2718;',
			'icon-deleteItem' : '&#x2715;',
			'icon-deleteSelected' : '&#x2613;',
			'icon-edit' : '&#x270f;',
			'icon-EditSite' : '&#x25e7;',
			'icon-exclamation' : '&#x27b2;',
			'icon-goBack' : '&#x21e6;',
			'icon-goForward' : '&#x21e8;',
			'icon-lock' : '&#xe000;',
			'icon-plusAlt' : '&#x2719;',
			'icon-print' : '&#x2338;',
			'icon-refresh' : '&#x21ba;',
			'icon-reporting' : '&#x2633;',
			'icon-search' : '&#x260c;',
			'icon-upload' : '&#xe001;',
			'icon-file' : '&#x2630;',
			'icon-fileAudio' : '&#xe002;',
			'icon-fileFlash' : '&#xe003;',
			'icon-fileImage' : '&#xe004;',
			'icon-filePdf' : '&#xe005;',
			'icon-filePowerpoint' : '&#xe006;',
			'icon-fileVideo' : '&#xe007;',
			'icon-close' : '&#xe02a;',
			'icon-copy' : '&#xe031;',
			'icon-film' : '&#xe030;',
			'icon-flash' : '&#xe032;',
			'icon-folder' : '&#xe033;',
			'icon-grid-view' : '&#xe015;',
			'icon-listView' : '&#xe02d;',
			'icon-pictures' : '&#xe02f;',
			'icon-ArrowUp' : '&#x25b3;',
			'icon-arrow-up' : '&#xe028;'
		},
		els = document.getElementsByTagName('*'),
		i, attr, c, el;
	for (i = 0; ; i += 1) {
		el = els[i];
		if(!el) {
			break;
		}
		attr = el.getAttribute('data-icon');
		if (attr) {
			addIcon(el, attr);
		}
		c = el.className;
		c = c.match(/icon-[^\s'"]+/);
		if (c && icons[c[0]]) {
			addIcon(el, icons[c[0]]);
		}
	}
};