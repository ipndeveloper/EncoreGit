<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="SearchBox">

	<input id="txtSearch" type="text"  class="TextInput orderSearch" />
	<a href="javascript:void(0);" id="btnGo" class="SearchIcon"><span>Go</span></a>

</div>

<script type="text/javascript">
	$(function () {
		$('#btnGo').click(function () {
			if ($('#txtSearch').val())
			    window.location.href = '<%= ResolveUrl("~/Orders/Browse") %>?orderNumber=' + $('#txtSearch').val();
		});
		$('#txtSearch').keyup(function (e) {
			if (e.keyCode == 13) {
				$('#btnGo').click();
			}
		}).keydown(function (e) {
			if (e.shiftKey && e.keyCode == 49) {
				return false;
			}

			if (e.modifiers && e.keyCode == 49) {
				return false;
			}
		});
	});
</script>

