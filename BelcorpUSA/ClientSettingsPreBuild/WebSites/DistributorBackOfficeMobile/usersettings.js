// change this to point to the webservice counterpart. most browsers won't allow localhost due to cross domain origin policy

// QA
// var serviceURI = 'http://api.encoreqa.com/MobileService.svc';

// DEV
var serviceURI = 'http://nsdesktop47.netsteps.com/EncoreMobileService/MobileService.svc';

var baseListHeight = '315px'; //mobile safari
var isDesktop = false;
var ajaxTimeout = 120 * 1000; // ajax load timeout in milliseconds
var pageSize = 50;
var pageBackAnimation = { type: 'slide', direction: 'right' };
var syncClass = /Android/i.test(navigator.userAgent) ? 'sync-android' : 'sync';
var emptyText = '<div class="noDataDisplay"><p>No records found.</p></div>';
var loadingText = '<div class="noDataDisplay"><p>Loading...</p></div>';

document.addEventListener('deviceready', function () { baseListHeight = '360px' }); //native