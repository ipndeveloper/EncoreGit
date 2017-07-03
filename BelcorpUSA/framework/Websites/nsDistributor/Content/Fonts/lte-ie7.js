/* Use this script if you need to support IE 7 and IE 6. */

window.onload = function() {
	function addIcon(el, entity) {
		var html = el.innerHTML;
		el.innerHTML = '<span style="font-family: \'encorePWSIcons\'">' + entity + '</span>' + html;
	}
	var icons = {
			'icon-arrowDown' : '&#x25bc;',
			'icon-arrowDown-small' : '&#x25be;',
			'icon-arrowLeft-small' : '&#x25c2;',
			'icon-arrowNext' : '&#x25b6;',
			'icon-arrowPrev' : '&#x25c4;',
			'icon-arrowRight-small' : '&#x25b8;',
			'icon-arrowUp' : '&#x25b2;',
			'icon-arrowUp-small' : '&#x25b4;',
			'icon-bundle-add' : '&#x25a1;',
			'icon-bundle-arrow' : '&#x2319;',
			'icon-bundle-full' : '&#x25a0;',
			'icon-cart' : '&#x2334;',
			'icon-check' : '&#x2714;',
			'icon-delete' : '&#x2715;',
			'icon-edit' : '&#x270f;',
			'icon-editAlt' : '&#x2638;',
			'icon-email' : '&#x2709;',
			'icon-help' : '&#x003f;',
			'icon-hide' : '&#x25c9;',
			'icon-info' : '&#x69;',
			'icon-phoneBio' : '&#x260e;',
			'icon-plus' : '&#x271a;',
			'icon-printer' : '&#xe017;',
			'icon-refresh' : '&#x21ba;',
			'icon-search' : '&#x260c;',
			'icon-show' : '&#x25ce;',
			'icon-star' : '&#x2605;',
			'icon-warning' : '&#x21;',
			'icon-x' : '&#x2718;',
			'icon-facebook' : '&#x66;',
			'icon-pinterest' : '&#x50;',
			'icon-tumblr' : '&#x74;',
			'icon-wordpress' : '&#x57;',
			'icon-youtube' : '&#x25b7;',
			'icon-file-pdf' : '&#x2637;',
			'icon-arrow-up' : '&#x2191;',
			'icon-arrow-right' : '&#x3e;',
			'icon-arrow-down' : '&#x2193;',
			'icon-arrow-left' : '&#x3c;',
			'icon-cart-2' : '&#x63;',
			'icon-disk' : '&#x25a3;',
			'icon-microphone' : '&#xe000;',
			'icon-blocked' : '&#xe001;',
			'icon-cancel' : '&#xe002;',
			'icon-checkmark' : '&#xe003;',
			'icon-cancel-2' : '&#xe004;',
			'icon-mail' : '&#x65;',
			'icon-mail-2' : '&#x6f;',
			'icon-emailOpen' : '&#x6d;',
			'icon-pictures' : '&#x22;',
			'icon-camera' : '&#x23;',
			'icon-bag' : '&#x24;',
			'icon-basket' : '&#x25;'
		},
		els = document.getElementsByTagName('*'),
		i, attr, html, c, el;
	for (i = 0; i < els.length; i += 1) {
		el = els[i];
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