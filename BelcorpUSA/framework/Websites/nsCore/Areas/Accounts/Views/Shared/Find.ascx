<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="SearchBox">
	<input id="txtSearch" type="text" class="TextInput distributorSearch" />
	<a href="javascript:void(0);" id="btnGo" class="SearchIcon"><span>Go</span></a>
	
	<div class="PModal WaitWin">
		<%= Html.Term("Onemomentplease", "One moment please...") %>
        <br />
		<img src="<%= ResolveUrl("~/Content/Images/processing.gif") %>" alt="loading" />
	</div>
</div>

<script type="text/javascript">

	$(function () {
		$('.WaitWin').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' });
		$('#btnGo').click(function () {
			var txtSearch = $('#txtSearch');
			if (txtSearch.val() == txtSearch.attr('title'))
				txtSearch.val('');
			window.location = '<%= ResolveUrl("~/Accounts/Browse") %>?q=' + txtSearch.val();
		});
		var accountLanding = window.location.pathname == '/' || window.location.pathname == '/Accounts';
		$('#txtSearch').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').keyup(function (e) {
			if (e.keyCode == 13) {
				$('#btnGo').click();
			}
		}).jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
			$('.WaitWin:first').jqmShow();
			window.location = '<%= ResolveUrl("~/Accounts/Overview/Index/") %>' + item.text.replace(/.+\(\#([^\)]*)\)/, '$1');
		}, defaultToFirst: false, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: $('#txtSearch').outerWidth(true) + $('#btnGo').outerWidth() - (accountLanding ? 2 : 4)
		});
	});
</script>

