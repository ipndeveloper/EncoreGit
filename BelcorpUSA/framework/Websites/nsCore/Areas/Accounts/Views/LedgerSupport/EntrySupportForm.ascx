<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Accounts.Models.Ledger.LedgerEntryFormModel>" %>
<div id="ledgerSupportEntryForm"> 
<% var isProductCredit = HttpContext.Current.Request.UrlReferrer != null && HttpContext.Current.Request.UrlReferrer.LocalPath.Contains("ProductCredit"); %>
    <script type="text/javascript">

        function hideButtons() {
            $('#btnSave').hide();
            $('#btnEntryFormClose').hide();
        }

        function showButtons() {
            $('#btnSave').show();
            $('#btnEntryFormClose').show();
        }

        function clearElements() {
            $('#txtEntryAmount').val('');
            $('#txtEntryDescription').val('');
            $('#txtNotes').val('');
        }

        $(function () {
            $('#txtEntryAmount').numeric();
            $('#txtEffectiveDate').datepicker().addClass('DatePicker');
            $('#ui-datepicker-div').css('z-index', '9999');
            $('#btnSave').click(function () {
                var isComplete = $('#EditLedger').checkRequiredFields();

                if (isComplete) {

                    hideButtons();

                    var newEntry = { entryAmount: /\([^\)]+\)/.test($('#txtEntryAmount').val()) ? '-' + $('#txtEntryAmount').val().replace(/\(([^\)])\)/, '$1') : $('#txtEntryAmount').val(),
                        effectiveDate: $('#txtEffectiveDate').val(),
                        entryDescription: $('#txtEntryDescription').val(),
                        entryReason: $('#entryReason').val(),
                        entryType: $('#entryType').val(),
                        bonusType: $('#bonusType').val(),
                        notes: $('#txtNotes').val(),
                        currentEndingBalance: '<%= Model.GetCurrentBalance(isProductCredit, CoreContext.CurrentAccount.AccountID) %>',
                        orderID: $('#txtOrder').val(),
                        supportTicketNumber: $('#txtSupportTicketNumber').val()

                    };
                    $.post('<%= !isProductCredit ? ResolveUrl("~/Accounts/Ledger/Add") : ResolveUrl("~/Accounts/Ledger/AddProductCredit") %>', newEntry, function (response) {
                        if (response.result) {
                            $('#paginatedGridPagination .pageSize').change();
                            $('#ledgerEntryForm').closest('.jqmWindow').jqmHide();
                            if (!userIsAuthorized) {
                                $('#authorization').show();
                                $('#ledgerEntryForm').remove();
                            }
                            clearElements();
                        } else {
                            showMessage(response.message, true);
                        }
                        showButtons();
                    });
                }
            });
        });
    </script>

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
                <select id="entryType" class="required" name="<%= Html.Term("PleaseSelectAnEntryType", "Please Select an Entry Type.") %>">
                    <% foreach (var type in Model.LedgerEntryKinds)
                       { %>
                    <option value="<%= type.LedgerEntryKindId %>">
                        <%= type.Name %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                <label for="bonusType"><%= Html.Term("BonusType", "Bonus Type") %>:</label>
            </td>
            <td>
                <select id="bonusType">
                    <option value="">
                        <%= Html.Term("None") %></option>
                        <% var bonusTypes = Model.BonusKinds.Where(b => b.IsEditable).ToList(); %>
                    <% foreach (var bonusType in bonusTypes)
                       { %>
                    <option value="<%= bonusType.BonusKindId %>">
                        <%= bonusType.Name%></option>
                    <%} %>
                </select>
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