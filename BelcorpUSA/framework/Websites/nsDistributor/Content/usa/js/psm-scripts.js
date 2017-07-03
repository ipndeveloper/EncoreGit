 jQuery(function(){		
		jQuery('#camera_wrap_3').camera({
		
			height: '380px',
			width: '960px',
			thumbnails: false,
			//fx: 'scrollHorz',
			fx: 'simpleFade',
			//pieDiameter: 20,
			time: 2000,
			loader: 'none',
			pagination: false,
			//hover:true,
			//navigation: false,
			playPause: false,
			navigationHover: true,
			//easing: 'easeInOutExpo',
			opacityOnGrid: false,
			transPeriod: 3000
	
			});
	jQuery('#camera_wrap_2').camera({
		height: '400px',
		loader: 'bar',
		pagination: false
		
		 	});
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
		
		
		$(function() {
	$('#toggle').click(function() {
			screenfull.toggle($('#virtual')[0]);
		});
		
		$('#exit').click(function() {
			screenfull.exit();
		});
		
		// Trigger the onchange() to set the initial values
		//screenfull.onchange();
	});