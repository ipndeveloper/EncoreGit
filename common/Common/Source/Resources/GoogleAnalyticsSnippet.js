<script type="text/javascript">
  var _gaq = _gaq || [], _domainName = '{{DomainName}}';
  _gaq.push(['_setAccount', '{{TrackerID}}']);
  _gaq.push(['_trackPageview']);
  _gaq.push(['_setAllowLinker', true]);
  if(_domainName!='')
	_gaq.push(['_setDomainName', (_domainName.indexOf('.')!==0?'.':'')+_domainName]);
  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();
</script>