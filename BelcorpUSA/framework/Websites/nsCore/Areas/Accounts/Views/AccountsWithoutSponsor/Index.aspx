<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.AccountsWithoutSponsorModel>" %>
<%@ Import Namespace="NetSteps.Common.Interfaces" %>
<%@ Import Namespace="NetSteps.Data.Entities" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2><%= Html.Term("AccountsWithoutSponsor", "Consultants Without Sponsor")%> <label id="ErrorMessage" style="color: Red;"></label></h2>
    </div>

    <% 
        Html.PaginatedGrid<AccountSponsorLogSearchData>("~/Accounts/AccountsWithoutSponsor/GetAccounts")
            .AddInputFilter(Html.Term("AccountIDOrName", "Account ID or Name"), "accountIDOrName")
            .AddSelectFilter(Html.Term("Period", "Period"), "PeriodID", new Dictionary<int, string>() { { 0, Html.Term("SelectItem", "Select Item") } }.AddRange(Model.AviablePeriods))
            .AddColumn(Html.Term("AccountID", "Account ID"), "AccountID", false)
            .AddColumn(Html.Term("Name", "Name"), "Name", false)
            .AddColumn(Html.Term("AccountStatus", "Account Status"), "AccountStatus", false)
            .AddColumn(Html.Term("PeriodID", "Period ID"), "PeriodID", false)
            .AddColumn(Html.Term("City", "City"), "City", false)
            .AddColumn(Html.Term("State", "State"), "State", false)
            .AddColumn(Html.Term("Email", "Email"), "Email", false)
            .AddColumn(Html.Term("Address", "Address"), "Address", false)
            .AddColumn(Html.Term("Telephone1", "Telephone 1"), "Telephone1", false)
            .AddColumn(Html.Term("Telephone2", "Telephone 2"), "Telephone2", false)
            .HideClientSpecificColumns()
			.Render();
    %>
        
    <script type="text/javascript">

        $(function () {

            var FilterValidation = '<%= Model.SponsorIDValidation %>';

            
 

            if (FilterValidation == 'False') {
                $('.Button').closest('div').hide();
                $('#ErrorMessage').text('<%= Html.JavascriptTerm("InvalidTemporalSponsor", "Error – Invalid Temporal Sponsor") %>');
            }


            var accountId = $('<input type="hidden" id="accountIdFilter" class="Filter" />').val('0');
            $('#accountIDOrNameInputFilter').change(function () {
                accountId.val('0');
            });

            $('#accountIDOrNameInputFilter').removeClass('Filter').after(accountId).css('width', '172px')
				.watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
				    accountId.val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});
        });

    </script>

</asp:Content>
