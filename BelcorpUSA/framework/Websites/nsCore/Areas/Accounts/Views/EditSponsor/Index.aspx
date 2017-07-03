<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.EditSponsorModel>" %>
<%@ Import Namespace="NetSteps.Common.Interfaces" %>
<%@ Import Namespace="NetSteps.Data.Entities" %>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2><%= Html.Term("EditSponsor", "Edit Sponsor")%></h2>
    </div>

    <table width="100%" class="DataGrid" cellspacing="0">
        <tr>
            <td style="width: 20%; border-right: 1px solid black;">
                <b><%= Html.Term("Consultant", "Consultant")%></b>
            </td>
            <td style="width: 12%; text-align: right; border: 1px solid black;">
                <%= Model.Account.AccountID %>
            </td>
            <td style="width: 15%; text-align: left; border: 1px solid black;">
                <%= string.Concat(Model.Account.FirstName, " ", Model.Account.LastName) %>
            </td>
            <td style="width:7%; border-right: 1px solid black;">
                <b><%= Html.Term("Status", "Status")%></b>
            </td>
            <td style="width:10%; border: 1px solid black;">
                <%= Model.Account.AccountStatus %>
            </td>
            <td style="width:20%;">
                <b><%= Html.Term("CurrentCareerTitle", "Título de carrera actual")%></b>
            </td>
            <td>
                <%= Model.Account.TitleName %>
            </td>
        </tr>
        <tr>
            <td colspan="7">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="width: 20%; border-right: 1px solid black;">
                <b><%= Html.Term("Sponsor", "Sponsor")%></b>
            </td>
            <td style="width: 12%; text-align: right; border: 1px solid black;">
                <% string sponsorID = string.Empty;
                   string sponsorName = string.Empty;

                   if (Model.Account.AccountStatusID == 2)
                   {
                       sponsorID = Model.Account.TerminatedSponsorID == -1 ? string.Empty : Model.Account.TerminatedSponsorID.ToString();
                       sponsorName = string.Concat(Model.Account.TerminatedSponsor);
                   }
                   else
                   {
                       sponsorID = Model.AccountSponsor.AccountID == -1 ? string.Empty : Model.AccountSponsor.AccountID.ToString();
                       sponsorName = string.Concat(Model.AccountSponsor.FirstName, " ", Model.AccountSponsor.LastName);
                   }
                       
                        %>

                <%= sponsorID %>
            </td>
            <td style="width: 15%; text-align: left; border: 1px solid black;">
                <%= sponsorName %>
            </td>
            <td style="width:7%; border-right: 1px solid black;">
                <b><%= Html.Term("Status", "Status")%></b>
            </td>
            <td style="width:10%; border: 1px solid black;">
                <%= Model.AccountSponsor.AccountStatus %>
            </td>
            <td style="width:20%;">
                <b><%= Html.Term("CurrentCareerTitle", "Título de carrera actual")%></b>
            </td>
            <td>
                <%= Model.AccountSponsor.TitleName %>
            </td>
        </tr>
        <tr>
            <td colspan="7" style="text-align:right; padding-right:50px;">
                <a id="btnTermination" href="javascript:void(0);" class="Button BigBlue"> <%= Html.Term("Termination", "Termination") %></a>
            </td>
        </tr>
    </table>

    <br />

    <h3>
        <%= Html.Term("CambiarAsignarSponsor", "Cambiar o asignar nuevo sponsor") %>
    </h3>

    <table width="100%" class="DataGrid" cellspacing="0">
        <tr>
            <td style="width: 20%;">
                <b><%= Html.Term("CodigoSponsor", "Código del nuevo sponsor")%></b>                
            </td>
            <td colspan="2" style="width: 27%;">
                <div class="SearchBox">
                    <% if (Model.Account.TerminatedSponsorID == -1)
                       { 
                    %>
	                <input id="txtSponsorID" type="text" class="TextInput distributorSearch" style="width: 100%;"/>	
                    <% }
                       else
                       {
                    %>
                    <label id="lblSponsorID" style="width: 100%;"></label>
                    <% } %>
                    <input id="hdnSponsorID" type="hidden"/>
                    <input id="hdnSponsorTitleID" type="hidden"/>
                </div>
            </td>
            <td style="width:10%; border-right: 1px solid black;">
                <b><%= Html.Term("Status", "Status")%></b>                
            </td>
            <td style="width:10%; border: 1px solid black;">
                <label id="lblSponsorStatus"></label>
            </td>
            <td style="width:20%;">
                <b><%= Html.Term("CurrentCareerTitle", "Título de carrera actual")%></b>
            </td>
            <td>
                <label id="lblSponsorTitle"></label>
            </td>
        </tr>
        <tr>
            <td>
                <b><%= Html.Term("CampañaInicio", "Campaña de inicio")%></b>
            </td>
            <td>
                <%= Html.DropDownList("Period", new SelectList(Model.AviablePeriods, "Key", "Value"))%>
            </td>
        </tr>
    </table>

    <br />

    <% 
        Html.PaginatedGrid<AccountSponsorLogSearchData>("~/Accounts/EditSponsor/GetUpdateLog")
            .AddColumn(Html.Term("ID Sponsor Anterior", "ID Sponsor Anterior"), "OldSponsorID", false)
            .AddColumn(Html.Term("Nombre Sponsor Anterior", "Nombre Sponsor Anterior"), "Nombre Sponsor Anterior", false)
            .AddColumn(Html.Term("ID Sponsor Siguiente", "ID Sponsor Siguiente"), "NewSponsorID", false)
            .AddColumn(Html.Term("Nombre Sponsor Siguiente", "Nombre Sponsor Siguiente"), "Nombre Sponsor Siguiente", false)
            .AddColumn(Html.Term("Inicio de Campaña", "Inicio de Campaña"), "CampainStart", false)
            .AddColumn(Html.Term("Fecha Cambio", "Fecha Cambio"), "UpdateDate", false)
            .AddColumn(Html.Term("Usuario", "Usuario"), "UpdateUser", false)
            .HideClientSpecificColumns()
			.Render();
    %>
    
    <div style="float: right;">
        <p>
            <a id="btnEditSponsor" href="javascript:void(0);" class="Button BigBlue"> <%= Html.Term("Save Information") %></a> 
            <a id="btnCancel" href="<%= ResolveUrl("~/Accounts/Overview") %>" class="Button"><%= Html.Term("Cancel") %></a>
        </p>
    </div>
    
    <script type="text/javascript">

        var SponsorValidate = false;
        var SponsorName = '';

        $(function () {
            var TerminatedSponsorID = '<%= Model.Account.TerminatedSponsorID %>';

            if (TerminatedSponsorID != -1)
                GetSponsorAditionalValues(TerminatedSponsorID);

            $("#txtSponsorID").keyup(function () {
                if (SponsorName != '' && $(this).val() != SponsorName) {
                    SponsorValidate = false;
                    SponsorName = '';
                }
            });

            $('#txtSponsorID').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/EditSponsor/SearchSponsor") %>', {
                onSelect: function (item) {
                    GetSponsorAditionalValues(item.id);
                    SponsorName = $('#txtSponsorID').val();
                },
                defaultToFirst: false,
                minCharacters: 3,
                ajaxResults: true,
                maxResults: 50,
                showMore: true,
                width: $('#txtSponsorID').outerWidth(true)
            });

            function GetSponsorAditionalValues(SponsorID) {
                var strURL = '<%= ResolveUrl("~/EditSponsor/GetSponsorAditionalValues") %>';

                var Parameters = {
                    SponsorID: SponsorID,
                    OpenPeriodID: '<%= Model.OpenPeriodID %>'
                };

                $.ajax({
                    type: 'POST',
                    url: strURL,
                    data: JSON.stringify(Parameters),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (data) {
                        if (data != '') {

                            var Sponsor = JSON.parse(data)

                            $('#lblSponsorID').text(Sponsor.FirstName + ' ' + Sponsor.LastName + '(#' + Sponsor.AccountID + ')');
                            $('#lblSponsorStatus').text(Sponsor.AccountStatus);
                            $('#lblSponsorTitle').text(Sponsor.TitleName);
                            $('#hdnSponsorID').val(Sponsor.AccountID);
                            $('#hdnSponsorTitleID').val(Sponsor.TitleID);
                            SponsorValidate = true;
                        }
                    }
                });
            }

            $("#btnEditSponsor").click(function (e) {
                if (SponsorValidate) {
                    SponsorSave();
                }
                else {
                    showMessage('<%= Html.Term("SelectSponsor", "Seleccione un nuevo sponsor.") %>', true);
                }
            });

            function SponsorSave() {
                var strURL = '<%= ResolveUrl("~/EditSponsor/UpdateSponsorInformation") %>';

                var active = true;

                var openPeriod = '<%= Model.OpenPeriodID %>';

                if ($('#Period').val() == openPeriod)
                    active = false;

                var LogParameters = {
                    OldSponsorID: '<%= Model.Account.SponsorID %>',
                    Active: active
                };

                var Parameters = {
                    PeriodID: '<%= Model.OpenPeriodID %>',
                    AccountStatusID: '<%= Model.Account.AccountStatusID %>',
                    TitleID: '<%= Model.Account.TitleID %>',
                    NewPeriodID: $('#Period').val(),
                    NewSponsorID: $('#hdnSponsorID').val(),
                    NewSponsorTitleID: $('#hdnSponsorTitleID').val(),
                    LogParameters: LogParameters
                };

                $.ajax({
                    type: 'POST',
                    url: strURL,
                    data: JSON.stringify(Parameters),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (data) {
                        if (data.result == true) {
                            showMessage(data.message);
                            window.location.reload();
                        }
                        else {
                            showMessage(data.message, true);
                        }
                    }
                });
            }

            $("#btnTermination").click(function (e) {
                if (confirm('<%= Html.Term("ConfirmTermination", "Are you sure you want to terminate the consultant.") %>')) {
                    TerminateConsultant();
                }
            });

            function TerminateConsultant() {
                var strURL = '<%= ResolveUrl("~/EditSponsor/TerminateConsultant") %>';

                $.ajax({
                    type: 'GET',
                    url: strURL,
                    dataType: 'json',
                    success: function (response) {
                        showMessage(response.message, !response.result);
                    }
                });
            }
        });

        

    </script>

</asp:Content>
