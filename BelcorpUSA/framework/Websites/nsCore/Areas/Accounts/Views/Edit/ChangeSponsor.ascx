<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="changeSponsor">
    <script type="text/javascript">
        $(function () {

            $('#btnChangeSponsorCancel').click(function () {
                $('div#sponsorModal').jqmHide();
            });
            $('#btnChangeSponsorSubmit').click(function () {
                var txtSponsor = $('#txtSponsor').val();
                if(txtSponsor.length > 0) {
                    var text = txtSponsor,
                        r = /^(.+)\s\(#(.+)\)$/ ,
                        accountNumber = r.exec(text)[2],
                        name = r.exec(text)[1];
                    
                    if(accountNumber != undefined && name != undefined) {
                        $('#sponsorAccountNumber').val(accountNumber);
                        $('#sponsor').html(String.format('<a href="<%= ResolveUrl("~/Accounts/Overview/Index/") %>{0}">{0} - {1}</a>', accountNumber, name));
                    }
                }

                $('div#sponsorModal').jqmHide();                                 
            });

            $('#txtSponsor').jsonSuggest('<%= ResolveUrl("~/Accounts/SearchActiveDistributors") %>', { onSelect: function (item) {
            }, minCharacters: 3, source: $('#txtSponsor'), width: 250, ajaxResults: true
            });
        });
    </script>
    <h2>
        <%= Html.Term("ChangePlacement", "Change Placement")%></h2>
    <input type="text" id="txtSponsor" style="width: 250px;"
            value="<%= CoreContext.CurrentAccount.SponsorInfo == null ? "" : CoreContext.CurrentAccount.SponsorInfo.FullName.Replace("\"", "\\\"") + " (#" + CoreContext.CurrentAccount.SponsorInfo.AccountNumber + ")" %>" />
    <br />    
    <br />
    <p>
        <a href="javascript:void(0);" id="btnChangeSponsorSubmit" class="Button BigBlue jqmClose">
            <%= Html.Term("Select", "Select")%></a>
        <a href="javascript:void(0);" id="btnChangeSponsorCancel" class="Button jqmClose">
            <%= Html.Term("Cancel", "Cancel")%></a>
    </p>
</div>
<span class="ClearAll"></span>