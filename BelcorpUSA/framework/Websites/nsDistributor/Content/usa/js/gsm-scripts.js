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

 $(function () {		
	      
	        $("#mybook").booklet({ 
	        name: "LBelUSA Virtual Catalog",
	        closed: true,
	        width: 1100,
	        height:760,
	        pagePadding: 0,
	        hovers: true,
	        hoverWidth: 200,
	        overlays: true,
	      // tabs: true,
	      // nextControlText: "Forward",
	       //menu: "#customMenu"
	       overlays: true,
	       arrows: true,
	       shadows: true,
	       //shadowTopFwdWidth: 200
	       pageNumbers: false,
	       autoCenter: true,
	       menu: '#custom-menu',
	       pageSelector: true,
	       next: '#custom-next',
	       prev: '#custom-prev'
	        });
	       
	    });
		
var _hidediv = null;
function showdiv(id) {
    if(_hidediv)
        _hidediv();
    var div = document.getElementById(id);
    div.style.display = 'block';
    _hidediv = function () { div.style.display = 'none'; };
}