<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Disbursements.Master"  Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%--Antonio Campos:16/12/2015--%>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Commissions") %>">
		<%= Html.Term("Commissions", "Commissions")%></a> > <%= Html.Term("ManagmentOptions") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

 <script type="text/javascript" language="javascript">


     function ChangeOption() {
         var optionSeleccionado = $("#comboBoxOptionsDisbursements").val();
         if (optionSeleccionado.length = 1) {
             $('#comboBoxOptionsDisbursements').clearError();
         }
     }
     
     function ChangePeriod() {
         $('#txtPeriodToProcess').clearError();
     }

     function ProcessManagmentClick() {
         var period = $('#txtPeriodToProcess').val();
         if (!period) {

             var alerta = '<%= Html.Term("PleaseEnterAvalidPeriod") %>';

             $("#txtPeriodToProcess").showError(alerta);
             $("#txtPeriodToProcess").focus();
             return;
         }


         var optionSeleccionado = $("#comboBoxOptionsDisbursements").val();
         if (optionSeleccionado.length > 1) {

             var alerta = '<%= Html.Term("PleaseOptionASelect") %>';
             $("#comboBoxOptionsDisbursements").showError(alerta);
             $("#comboBoxOptionsDisbursements").focus();
             return;
         }

         if (optionSeleccionado == 1) {
             question = '<%= Html.Term("AreYouSureToWantToApproveCommissionsAndBonus","Are you sure to want to Approve Commissions and Bonus?") %>';
         }
         else
             if (optionSeleccionado == 2) {
                 question = '<%= Html.Term("AreYouSureToWantToApprovePayments","Are you sure to want to approve payments?") %>';
             }
             else
                 question = '<%= Html.Term("AreYouSureToSendToBanks","Are you sure to send to banks?") %>';
             
             if (!confirm(question)) {
                 return;    
             }

             $.ajax({
                 type: "POST",
                 url: '<%= ResolveUrl("~/Managment/ProcessManagment") %>',
                 data: "{PeriodID:" + period + ",OptionDisbursement:" + optionSeleccionado + "}",
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 success: function (msg) {

                     showMessage(msg.message, (msg.Success == 0));
                     $("#txtPeriodToProcess").val("");
                     $('#comboBoxOptionsDisbursements').find('option:first').attr('selected', 'selected').parent('select');
                 },
                 error: function () {
                     showMessage(msg.message, false);
                     $("#txtPeriodToProcess").val("");
                     $('#comboBoxOptionsDisbursements').find('option:first').attr('selected', 'selected').parent('select');
                 }
             });

         
     }
 </script>
    
		<div class="LandingTools">
        <div class="Title">
            <h3>
                <%= Html.Term("ManagmentOptions", "Managment Options")%> </h3>
        </div>
        <div class="Body">
            <div class="StartSearch">
                <div class="TabSearch">
                    <div class="mb10">
                        <label for="txtPeriodToProcess">
                            <%= Html.Term("PeriodToProcess", "Period to process")%>:</label><br />
                        <input type="text" id="txtPeriodToProcess" style="width: 15em;" disabled="disabled"; onkeyPress="ChangePeriod()"; value= <%=@ViewBag.ClosedPeriodValue%> />
                        <input type="hidden" id="HiddenPeriodToProcess" />
                    </div>

                    <div class="mb10">
                        <label for="OptionsDisbursements">
                            <%= Html.Term("Options") %>:</label><br />
                        <%: Html.DropDownOpcionsDisbursementsManagment(htmlAttributes: new { @id = "comboBoxOptionsDisbursements", @name = "comboBoxOptionsDisbursements", @onCLick ="ChangeOption();"}, selectTextTermName: "SelectOptions")%>
                        
                    </div>
                   
                    
                    <p>
                        <a href="javascript:void(0);" id="btnProcess" class="Button BigBlue" onclick="ProcessManagmentClick();">
                            <%= Html.Term("Process", "Process")%></a>
                    </p>
                </div>
            </div>
        </div>
        <span class="ClearAll"></span>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
