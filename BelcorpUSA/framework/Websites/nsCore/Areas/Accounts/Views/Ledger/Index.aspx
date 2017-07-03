<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

	<script type="text/javascript">
	    $(function () {
	        $('#ledgerContainer').ajaxStop(function () {
	            $('#ledgerContainer .Negative').css('color', 'Red');
	        });

			$('#addLedgerModal').jqm({ modal: true,
				trigger: '#addLedger',
				onShow: function (h) {
					h.w.css({
						top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
						left: Math.floor(parseInt($(window).width() / 2)) + 'px'
					}).fadeIn();
				}
			});
		});
	</script>

</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>"><%= Html.Term("Accounts", "Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
		<%= CoreContext.CurrentAccount.FullName %></a> > <%= Html.Term("AccountLedger", "Account Ledger") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("AccountLedger", "Account Ledger") %></h2>
        <%= Html.Term("AccountLedger", "Account Ledger") %>
        <% if (CoreContext.CurrentUser.HasFunction("Accounts-Product Credit Ledger", checkWorkstationUserRole:false))
               { %> | <a href="<%= ResolveUrl("~/Accounts/Ledger/ProductCredit") %>"><%= Html.Term("ProductCreditLedger", "Product Credit Ledger") %></a><% } %>
		<br />
		<a id="addLedger" href="javascript:void(0);" class="DTL Add Ledger"><%= Html.Term("AddNewLedgerEntry", "Add New Ledger Entry") %></a>
	</div>
	<div id="addLedgerModal" class="jqmWindow LModal LedgerWin">
		<div class="mContent">
			<% ViewData["Function"] = "Accounts-Add Ledger Entry"; 
				Html.RenderPartial("Authorize"); %>
		</div>
	</div>
    <div id="ledgerContainer">
	<% Html.PaginatedGrid("~/Accounts/Ledger/Get")
		.AddColumn(Html.Term("Description"), "EntryDescription", false)
        .AddColumn(Html.Term("Reason"), "LedgerEntryReason.Name", false)
         .AddColumn(Html.Term("Origin"), "Origin", true)
        .AddColumn(Html.Term("Type"), "LedgerEntryType.Name", false)
		.AddColumn(Html.Term("EffectiveDate", "Effective Date"), "EffectiveDate", false)
        .AddColumn(Html.Term("UserName", "User Name"), "User Name", true)
        
		.AddColumn(Html.Term("Amount"), "EntryAmount", false)
		.AddColumn(Html.Term("Balance"), "EndingBalance", false)
        .AddColumn(Html.Term("EntryDate", "Entry Date"), "Entry Date", false)
        .AddColumn(Html.Term("BonusType", "Bonus Type"), "BonusType.Name", false)
		.Render(); %>
       </div>
</asp:Content>
