<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/CTE.Master" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {

//            var accountId = $('<input type="hidden" id="accountIdFilter" class="Filter" />');
////            $('#accountCode02InputFilter').change(function () {
////                accountId.val('');
////            });

//            $('#accountCodeInputFilter').removeClass('Filter').after(accountId).css('width', '275px')

//				.watermark('<%=Html.JavascriptTerm("AccountSearch", "Look up account by ID or name")%>')
//				.jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
//				    
//				    accountId.val(item.id);
//				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
//            });


        var sponsorSelected = false;
        var sponsorId = $('<input type="hidden" id="accountIdFilter" class="Filter" />');
        $('#accountCodeInputFilter').removeClass('Filter').css('width', '275px').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
            sponsorId.val(item.id);
            sponsorSelected = true;
        }, minCharacters: 3, source: $('#accountCodeFilter'), ajaxResults: true, maxResults: 50, showMore: true
        }).blur(function () {

            if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                sponsorId.val('');
            } else if (!sponsorSelected) {
                sponsorId.val('-1');
            }

            sponsorSelected = false;
        }).after(sponsorId);


        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="SectionHeader">
		<h2>
			<%=  Html.Term("BrowseBankPayments", "Browse Bank Payments")%>
        </h2>
	</div>
    <% Html.PaginatedGrid<BankPayments>("~/CTE/BankConsolidateApplication/getBankPayments")
            .AutoGenerateColumns()
          //.AddColumn(Html.Term("AccountCode"), "AccountCode", false)
          //.AddColumn(Html.Term("AccountName"), "AccountName", false)
          //.AddColumn(Html.Term("BankName"), "BankName", false)
          //.AddColumn(Html.Term("TicketNumber"), "TicketNumber02", false)
          //.AddColumn(Html.Term("OrderNumber"), "OrderNumber", false)
          //.AddColumn(Html.Term("Amount"), "Amount", false)
          //.AddColumn(Html.Term("DateReceivedBank"), "DateReceivedBank", false)
          //.AddColumn(Html.Term("DateApplied"), "DateApplied", false)
          //.AddColumn(Html.Term("FileNameBank"), "FileNameBank", false)
          //.AddColumn(Html.Term("FileSequence"), "FileSequence", false)
          //.AddColumn(Html.Term("Applied"), "Applied", false)

          .AddInputFilter(Html.Term("TicketNumbers", "Ticket Number"), "ticketNumber")
            
          .AddInputFilter(Html.Term("AccountNumberOrName", "Account Number or Name"), "accountCode")
                                                            
            //.HideClientSpecificColumns_()            
            .ClickEntireRow()
            .Render(); 
    %>


</asp:Content>


