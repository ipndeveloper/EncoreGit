<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Enrollment/Views/Shared/Enrollment.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%@ Import Namespace="NetSteps.Web.Mvc.Controls.Infrastructure" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('input[name="acctTypeId"]').click(function () {
                $('#btnDistributor').prop('checked') && $('#distributorTypes').fadeIn('fast') || $('#distributorTypes').fadeOut('fast');
            });
            $('#btnNext').click(function () {
                var acctTypeId = $('input[name="acctTypeId"]:checked').val();
                var isEntity = $('input[name="isEntity"]:checked').val();

                if (acctTypeId === undefined) {
                    showMessage('<%: Html.Term("ChooseYourEnrollmentType", "Choose your enrollment type") %>', true);
                    return false;
                }

                var url = '<%: ResolveUrl("~/Enrollment") %>?continueEnrollment=True&acctTypeId=' + acctTypeId;
                if (isEntity !== undefined) {
                    url += '&isEntity=' + isEntity;
                }

                window.location = url;
            });
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div class="StepGutter">
        <h3>
            <%= Html.Term("ChooseYourEnrollmentType", "Choose your enrollment type") %></h3>
    </div>
    <div class="StepBody">
        <% if (EnrollmentConfigHandler.AccountTypeEnabled((short)Constants.AccountType.RetailCustomer))
           { %>
        <%: Html.RadioButton("acctTypeId", (short)Constants.AccountType.RetailCustomer, new { id = "btnRetail" }) %>
        <%: Html.Label("btnRetail", Html.Term("RetailCustomer", "Retail Customer"))%><br />
        <% } if (EnrollmentConfigHandler.AccountTypeEnabled((short)Constants.AccountType.PreferredCustomer))
           { %>
        <%: Html.RadioButton("acctTypeId", (short)Constants.AccountType.PreferredCustomer, new { id = "btnPreferredCustomer" }) %>
        <%: Html.Label("btnPreferredCustomer", Html.Term("PreferredCustomer", "Preferred Customer"))%><br />
        <% } if (EnrollmentConfigHandler.AccountTypeEnabled((short)Constants.AccountType.Distributor))
           { %>
        <%: Html.RadioButton("acctTypeId", (short)Constants.AccountType.Distributor, new { id = "btnDistributor" }) %>
        <%: Html.Label("btnDistributor", Html.Term("Distributor"))%><br />
        <div id="distributorTypes" style="margin-left: 10px; display: none;">
            <%: Html.RadioButton("isEntity", false, true, new { id = "btnIndividual" })%>
            <%: Html.Label("btnIndividual", Html.Term("Individual"))%><br />
            <%: Html.RadioButton("isEntity", true, new { id = "btnBusiness" })%>
            <%: Html.Label("btnBusiness", Html.Term("BusinessEntity", "Business Entity"))%>
        </div>
        <% } %>
    </div>
    <span class="ClearAll"></span>
    <p class="Enrollment SubmitPage">
        <a id="btnNext" href="javascript:void(0);" class="Button BigBlue">
            <%= Html.Term("Next") %>&gt;&gt;</a>
    </p>
</asp:Content>
