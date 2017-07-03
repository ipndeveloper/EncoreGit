<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Communication.Models.AlertTemplateViewModel>" %>

<script type="text/javascript">
    $(function () {

        $('#btnCancel').click(function () {
            var alertTemplateTranslationForm = $(this).closest('div#alertTemplateForm')
            alertTemplateTranslationForm.hide();
            $('#message').val('');
        });


        $('#btnSave').click(function () {
            if (!$('#alertTemplateForm').checkRequiredFields()) {
                return false;
            }
            var t = $(this);
            showLoading(t);
            var alertTemplateID = $('#alertTemplateID').val()

            $.post('<%: ResolveUrl("~/Communication/Alerts/SaveAlertTemplateTranslation") %>',
                 {
                     AlertTemplateTranslationID: $('#alertTemplateTranslationID').val(),
                     AlertTemplateID: alertTemplateID,
                     LanguageID: $('#LanguageID').val(),
                     Message: $('#message').val()
                 },
                 function (response) {
                     hideLoading(t);
                     //showMessage(response.message || '<%= Html.Term("SavedSuccessfully", "Saved successfully!") %>', !response.result);
                     if (response.result) {
                         $('div#alertTemplateForm').hide();
                         window.location = "/Communication/Alerts/Edit/" + alertTemplateID;
                     }

                 });
        });

    });
</script>

<div id="alertTemplateForm" >
 <%: Html.Hidden("alertTemplateTranslationID", Model.CurrentTemplateTranslation != null ? Model.CurrentTemplateTranslation.AlertTemplateTranslationID : 0)%>
    <table class="FormTable Section" width="100%">
        <!-- Language field -->
        <tr>
            <td class="FLabel">
                <%: Html.Term("Language") %>
            </td>
            <td>
                <%: Html.DropDownList("LanguageID", new SelectList(Model.Languages as System.Collections.IEnumerable,
                                        "Key", "Value",
                                        Model.CurrentTemplateTranslation != null ? Model.CurrentTemplateTranslation.LanguageID
                                                                                 : default(int))) %>
            </td>
        </tr>

        <!-- Message field -->
        <tr>
            <td class="FLabel">
                  <%: Html.Term("Message") %>
            </td>
            <td>
                 <!-- Token field -->
                 <div class="FR mr10 TokenList">
                    <div class="FL">
                    <b><%=Html.Term("Tokens")%>:</b>
                    <select id="tokenList">
                        <% foreach (var token in Model.AlertTemplate.Tokens)
                           { %>
                           <option value="<%:  NetSteps.Common.TokenReplacement.TokenReplacer.GetDelimitedTokenizedString(token.Placeholder)  %>">
                                <%: token.Name %>
                           </option>

                        <% } %>
                    </select>
                    </div>
                    <a id="btnAddToken" class="FL DTL Add">
                        <span><%: Html.Term("AddToken", "Add Token") %></span>
                    </a>
                    <span class="clr"></span>
                    
               
                </div>

                <%: Html.TextArea("message", Model.CurrentTemplateTranslation != null ? Model.CurrentTemplateTranslation.Message : "", 
                                                            new { @class = "required fullWidth alertContent", rows = "", cols = "" })%>
            </td>
        </tr>

       
       
        </table>
        
        <table class="FormTable" width="100%">
        <tr>
            <td class="FLabel">
            &nbsp;
            </td>
            <td>
                 <div class="mt10">
                <a id="btnSave" href="javascript:void(0);" class="Button BigBlue">
                    <span><%: Html.Term("Save") %></span>
                </a>
                <a id="btnCancel" href="javascript:void(0);" class="Button">
                    <%: Html.Term("Cancel") %>
                </a>
                </div>
            </td>
        </tr>
        
    </table>

</div>

