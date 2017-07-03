!function(d,s,id){var js,fjs=d.getElementsByTagName(s)[0];if(!d.getElementById(id)){js=d.createElement(s);js.id=id;js.src="//platform.twitter.com/widgets.js";fjs.parentNode.insertBefore(js,fjs);}}(document,"script","twitter-wjs");
	
	(function() {
		var po = document.createElement('script'); po.async = true;
		po.src = 'https://apis.google.com/js/plusone.js';
		var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(po, s);
	})();
	 
	
	<!--<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7/jquery.min.js"></script>-->
	<script src="http://images.lbelusa.com/encore/js/screenfull.js"></script>
	
	
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
	