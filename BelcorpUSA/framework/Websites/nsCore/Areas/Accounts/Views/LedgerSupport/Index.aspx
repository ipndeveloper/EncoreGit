<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Accounts.Models.Ledger.LedgerEntryFormModel>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<% var isProductCredit = HttpContext.Current.Request.UrlReferrer != null && HttpContext.Current.Request.UrlReferrer.LocalPath.Contains("ProductCredit"); %>
 
	<script type="text/javascript">
	    $(function () {
	        $('#ledgerContainer').ajaxStop(function () {
	            $('#ledgerContainer .Negative').css('color', 'Red');
	        });

	        $('#addLedgerSupportModal').jqm({ modal: true,
	            trigger: '#addSupportLedger',
	            onShow: function (h) {
	                h.w.css({
	                    top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
	                    left: Math.floor(parseInt($(window).width() / 2)) + 'px'
	                }).fadeIn();
	            }
	        });
	    });

        $(function () {
            $('#txtEntryAmount').numeric();
            $('#txtEffectiveDate').datepicker().addClass('DatePicker');   
            });
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>"><%= Html.Term("Accounts", "Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
		<%= CoreContext.CurrentAccount.FullName %></a> > <%= Html.Term("AccountLedger", "Account Ledger") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
        <%= Html.Term("AccountLedgerSupport", "Account Ledger Support")%></h2>
        <%= Html.Term("AccountLedgerSupport", "Account Ledger Support")%>
        <% if (CoreContext.CurrentUser.HasFunction("Accounts-Product Credit Ledger", checkWorkstationUserRole:false))
               { %> | <a href="<%= ResolveUrl("~/Accounts/Ledger/ProductCredit") %>"><%= Html.Term("ProductCreditLedger", "Product Credit Ledger") %></a><% } %>
		<br />
		<a id="addSupportLedger" href="javascript:void(0);" class="DTL Add Ledger"><%= Html.Term("AddNewLedgerEntry", "Add New Ledger Entry") %></a>
	</div>
	<div id="addLedgerSupportModal" class="jqmWindow LModal LedgerWin">
		<div class="mContent">
			  <h2>
        <%= Html.Term("AddLedgerEntry", "Add Ledger Entry") %></h2>
    <table cellspacing="0" class="DataGrid" id="EditLedger">
        <tr>
            <td style="width: 10em;">
                <label for="txtEntryAmount">
                    <%= Html.Term("NumTicketSupport", "N° Order")%>:</label>
            </td>  
            <td> <input type="text" id="txtSupportTicketNumber" class="required" />
            </td>
        </tr>
         <tr>
            <td style="width: 10em;">
                <label for="txtEntryAmount">
                    <%= Html.Term("NumOrder", "N° Order")%>:</label>
            </td>
            <td> <input type="text" id="txtOrder" class="required" />
            </td>
        </tr>
        <tr>
            <td style="width: 10em;">
                <label for="txtEntryAmount"><%= Html.Term("EntryAmount", "Entry Amount") %>:</label>
            </td>
            <td>
                <%= CoreContext.CurrentCultureInfo.NumberFormat.CurrencySymbol %><input type="text" id="txtEntryAmount" class="required"/>
            </td>
        </tr>
        <tr>
            <td>
                <label for="txtEffectiveDate"><%= Html.Term("EffectiveDate", "Effective Date") %>:</label>
            </td>
            <td>
                <input type="text" id="txtEffectiveDate" value="<%= DateTime.Now.ToShortDateString() %>" style="width: 8.182em;" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="txtEntryDescription"><%= Html.Term("EntryDescription", "Entry Description") %>:</label>
            </td>
            <td>
                <input type="text" id="txtEntryDescription" class="required"/>
            </td>
        </tr>
        <tr>
            <td>
                <label for="entryReason"><%= Html.Term("EntryReason", "Entry Reason") %>:</label>
            </td>
            <td>
                <select id="entryReason" class="required" name="<%= Html.Term("PleaseSelectAnEntryReason", "Please Select an Entry Reason.") %>">
                    <% foreach (var reason in Model.LedgerEntryReasons)
                       { %>
                    <option value="<%= reason.EntryReasonId %>">
                        <%= reason.Name %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                <label for="entryType"><%= Html.Term("EntryType", "Entry Type") %>:</label>
            </td>
            <td>
                <%--<select id="entryType" class="required" name="<%= Html.Term("PleaseSelectAnEntryType", "Please Select an Entry Type.") %>">
                    <% foreach (var type in Model.LedgerEntryKinds)
                       { %>
                    <option value="<%= type.LedgerEntryKindId %>">
                        <%= type.Name %></option>
                    <%} %>
                </select>--%>
            </td>
        </tr>
        <tr>
            <td>
                <label for="bonusType"><%= Html.Term("BonusType", "Bonus Type") %>:</label>
            </td>
            <td>
                <%--<select id="bonusType">
                    <option value="">
                        <%= Html.Term("None") %></option>
                        <% var bonusTypes = Model.BonusKinds.Where(b => b.IsEditable).ToList(); %>
                    <% foreach (var bonusType in bonusTypes)
                       { %>
                    <option value="<%= bonusType.BonusKindId %>">
                        <%= bonusType.Name%></option>
                    <%} %>
                </select>--%>
            </td>
        </tr>
        <tr>
            <td>
               <label for="txtNotes"> <%= Html.Term("Notes") %>:</label>
            </td>
            <td>
                <textarea id="txtNotes" rows="" cols="" style="width: 100%; height: 6.25em;"></textarea>
            </td>
        </tr>
    </table>
    <br />
    <p>
        <a id="btnSave" href="javascript:void(0);" class="Button BigBlue">
            <%= Html.Term("Save") %></a>
        <a id="btnEntryFormClose" href="javascript:void(0);" class="Button jqmClose">
            <%= Html.Term("Cancel") %></a> <span class="ClearAll"></span>
    </p>
		</div>
	</div>
    
</asp:Content>
