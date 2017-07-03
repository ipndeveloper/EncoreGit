<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Accounts.Models.Ledger.LedgerEntryFormModel>" %>
<div id="ledgerEntryForm">

<% var isProductCredit = HttpContext.Current.Request.UrlReferrer != null && HttpContext.Current.Request.UrlReferrer.LocalPath.Contains("ProductCredit"); %>
<% var UbicLedgerPopup = Session["UbicLedgerPopup"]; %>
    <script type="text/javascript">
    // agregado por IPN: funcion permite formatear el ingreso de decimales
        $('input[monedaidioma=CultureIPN]').keyup(function (event) {

            var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
           // var value = parseFloat($(this).val());


            var formatDecimal = '$1.$2'; // valores por defaul 
            var formatMiles = ",";  // valores por defaul

            if (cultureInfo === 'en-US') {
                 formatDecimal = '$1.$2';
                 formatMiles = ",";
            }
            else if (cultureInfo === 'es-US') {
                 formatDecimal = '$1,$2';
                 formatMiles = ".";
            }
            else if (cultureInfo === 'pt-BR') {
                formatDecimal = '$1,$2';
                formatMiles = ".";
            }


//            if (!isNaN(value)) {
                if (event.which >= 37 && event.which <= 40) {
                    event.preventDefault();
                }

                $(this).val(function (index, value) {


                    return value.replace(/\D/g, "")
                                 .replace(/([0-9])([0-9]{2})$/, formatDecimal)
                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, formatMiles);
                });

//            }

        });

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
		    //$('#txtEffectiveDate').datepicker().addClass('DatePicker');		  

		    $('#ui-datepicker-div').css('z-index', '9999');

		    $('#btnSave').click(function () {


		        var isComplete = $('#EditLedger').checkRequiredFields();

		        if (isComplete) {

		            hideButtons();
		          

		            if ($("#hdnUbicLedgerPopup").val() == "A") {


		                var newEntry = { _entryAmount: /\([^\)]+\)/.test($('#txtEntryAmount').val()) ? '-' + $('#txtEntryAmount').val().replace(/\(([^\)])\)/, '$1') : $('#txtEntryAmount').val(),
		                    effectiveDate: '', //$('#txtEffectiveDate').val(),
		                    entryDescription: $('#txtEntryDescription').val(),
		                    entryReason: $('#entryReason').val(),
		                    entryType: $('#entryType').val(),
		                    bonusType: $('#bonusType').val(),
		                    notes: $('#txtNotes').val(),
		                    currentEndingBalance: '<%= Model.GetCurrentBalance(isProductCredit, CoreContext.CurrentAccount.AccountID) %>',
		                    orderID: $('#txtOrder').val(),
		                    supportTicketNumber: $('#txtSupportTicketNumber').val()
		                };

		                $.post('<%= ResolveUrl("~/Accounts/Ledger/Add")  %>', newEntry, function (response) {
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
		            else {

		                var newEntry = {

		                    _entryAmount: /\([^\)]+\)/.test($('#txtEntryAmount').val()) ? '-' + $('#txtEntryAmount').val().replace(/\(([^\)])\)/, '$1') : $('#txtEntryAmount').val(),
		                    effectiveDate: '', // $('#txtEffectiveDate').val(),
		                    entryDescription: $('#txtEntryDescription').val(),
		                    entryReason: $('#entryReason').val(),
		                    entryType: $('#entryType').val(),
		                    bonusType: $('#bonusType').val(),
		                    notes: $('#txtNotes').val(),
		                    _currentEndingBalance: '<%= Model.GetCurrentBalance(isProductCredit, CoreContext.CurrentAccount.AccountID) %>',
		                    orderID: $('#txtOrder').val(),
		                    supportTicketNumber: $('#txtSupportTicketNumber').val()
		                }

		                //		            

		                $.post('<%= ResolveUrl("~/Accounts/Ledger/AddProductCredit") %>', newEntry, function (response) {
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



		            //		            $.post('<%= !isProductCredit ? ResolveUrl("~/Accounts/Ledger/Add") : ResolveUrl("~/Accounts/Ledger/AddProductCredit") %>', newEntry, function (response) {
		            //		                if (response.result) {
		            //		                    $('#paginatedGridPagination .pageSize').change();
		            //		                    $('#ledgerEntryForm').closest('.jqmWindow').jqmHide();
		            //		                    if (!userIsAuthorized) {
		            //		                        $('#authorization').show();
		            //		                        $('#ledgerEntryForm').remove();
		            //		                    }
		            //		                    clearElements();
		            //		                } else {
		            //		                    showMessage(response.message, true);
		            //		                }
		            //		                showButtons();
		            //		            });


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
             <input type="hidden" id="isProductCredit"  value="<%=!isProductCredit? "1":"0"%>" />
              <input type="hidden" id="hdnUbicLedgerPopup"  value="<%=UbicLedgerPopup%>" />
             
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
                <%= CoreContext.CurrentCultureInfo.NumberFormat.CurrencySymbol %><input type="text" id="txtEntryAmount"  monedaidioma="CultureIPN" class="required"/>
            </td>
        </tr>
        <tr style="display:none">
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