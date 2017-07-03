<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="changeEnroller">
    <script type="text/javascript">
        $(function () {

            $('#btnChangeEnrollerCancel').click(function () {
                $('div#enrollerModal').jqmHide();
            });

            $('#btnChangeEnrollerSubmit').click(function() {
                var txtEnroller = $('#txtEnroller').val();
                if (txtEnroller.length > 0) {
                    var text = txtEnroller,
                        r = /^(.+)\s\(#(.+)\)$/ ,
                        accountNumber = r.exec(text)[2],
                        name = r.exec(text)[1];
                    
                    if(accountNumber != undefined && name != undefined) {
                        $('#enrollerAccountNumber').val(accountNumber);
                        $('#enroller').html(String.format('<a href="<%= ResolveUrl("~/Accounts/Overview/Index/") %>{0}">{0} - {1}</a>', accountNumber, name));    
                    }
                }

                $('div#enrollerModal').jqmHide();
            });

            $('#txtEnroller').jsonSuggest('<%= ResolveUrl("~/Accounts/SearchActiveDistributors") %>', { onSelect: function (item) {

            }, minCharacters: 3, source: $('#txtEnroller'), width: 250, ajaxResults: true
            });
        });
    </script>
    <h2>
        <%= Html.Term("ChangeEnroller", "Change Enroller")%></h2>
    <input type="text" id="txtEnroller" style="width: 250px;"
            value="<%= CoreContext.CurrentAccount.EnrollerInfo == null ? "" : CoreContext.CurrentAccount.EnrollerInfo.FullName.Replace("\"", "\\\"") + " (#" + CoreContext.CurrentAccount.EnrollerInfo.AccountNumber + ")" %>" />
    <br />   
    <br />
    <p>
        <a href="javascript:void(0);" id="btnChangeEnrollerSubmit" class="Button BigBlue jqmClose">
            <%= Html.Term("Select", "Select")%></a>
        <a href="javascript:void(0);" id="btnChangeEnrollerCancel" class="Button jqmClose">
            <%= Html.Term("Cancel", "Cancel")%></a>
    </p>
</div>
<span class="ClearAll"></span>