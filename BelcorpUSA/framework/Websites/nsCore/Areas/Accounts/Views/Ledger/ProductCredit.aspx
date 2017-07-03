<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
	Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript">
		$(function () {
		    $('#ledgerContainer').ajaxStop(function () {
		        $('#ledgerContainer .Negative').css('color', 'Red');
		    });

			$('#addLedgerModal').jqm({ modal: true,
				trigger: '#addLedger',
				onShow: function (h)
				{
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
	<a href="<%= ResolveUrl("~/Accounts") %>">
		<%= Html.Term("Accounts", "Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
			<%= CoreContext.CurrentAccount.FullName %></a> >
	<%= Html.Term("ProductCreditLedger", "Product Credit Ledger") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("ProductCreditLedger", "Product Credit Ledger")%></h2>
		<a href="<%= ResolveUrl("~/Accounts/Ledger") %>">
			<%= Html.Term("AccountLedger", "Account Ledger") %></a> |
		<%= Html.Term("ProductCreditLedger", "Product Credit Ledger") %>
        <br />
		<a id="addLedger" href="javascript:void(0);" class="Ledger">
			<%= Html.Term("AddNewLedgerEntry", "Add New Ledger Entry") %></a>
	</div>
	<div id="addLedgerModal" class="jqmWindow LModal LedgerWin">
		<div class="mContent">
			<% ViewData["Function"] = "Accounts-Add Ledger Entry";
	  Html.RenderPartial("Authorize"); %>
		</div>
	</div>
    <div id="ledgerContainer">
	<% Html.PaginatedGrid("~/Accounts/Ledger/GetProductCredit")
		.AddColumn(Html.Term("Description"), "EntryDescription", true)
		.AddColumn(Html.Term("Reason"), "LedgerEntryReason.Name", true)
        .AddColumn(Html.Term("Origin"), "Origin", true)
		.AddColumn(Html.Term("Type"), "LedgerEntryType.Name", true)
		.AddColumn(Html.Term("EffectiveDate", "Effective Date"), "EffectiveDate", true, true, NetSteps.Common.Constants.SortDirection.Ascending)
        .AddColumn(Html.Term("UserName", "User Name"), "User Name", true)
		.AddColumn(Html.Term("Amount"), "EntryAmount", true)
		.AddColumn(Html.Term("Balance"), "EndingBalance", true)
         .AddColumn(Html.Term("Support Ticket", "Support Ticket"), "SupportTicket", true)
        .AddColumn(Html.Term("Order"), "Order", true)
        .AddColumn(Html.Term("BonusType", "Bonus Type"), "BonusType.Name", true)
		.Render(); %>
    </div>
</asp:Content>
