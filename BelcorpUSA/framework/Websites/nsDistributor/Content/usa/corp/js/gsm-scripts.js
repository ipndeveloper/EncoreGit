/*http://www.pixedelic.com/plugins/camera/*/
	
jQuery(function(){		
	jQuery('#camera_wrap_1').camera({
	thumbnails: false,
	hover: true,
	playPause: false,
	fx: 'simpleFade',
	pieDiameter: 20,
	time: 2000,
	loader: 'none',
	opacityOnGrid: true,
	height:'auto',
	width:'100%',
	pagination: false
	
		});
	jQuery('#camera_wrap_2').camera({
		height: 'auto',
		loader: 'bar',
		width:'100%',
		pagination: false
		
		 	});
		});
		


$(document).ready(function() {
$(".fancybox").fancybox();
});

		
var _hidediv = null;
function showdiv(id) {
    if(_hidediv)
        _hidediv();
    var div = document.getElementById(id);
    div.style.display = 'block';
    _hidediv = function () { div.style.display = 'none'; };
}