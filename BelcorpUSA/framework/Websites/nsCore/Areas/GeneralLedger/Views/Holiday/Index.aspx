<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/HolidayManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.GeneralLedger.Models.ViewModels.HolidayViewModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/GeneralLedger") %>">
        <%= Html.Term("GMP-Nav-General-Ledger", "General Ledger")%></a> >
    <%= Html.Term("HolidayCalendar", "Holiday Calendar")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
    <script type="text/javascript">
        function validarCampos() {

            var ok = true;

            if ($('#reasonHoliday').val() == "") {
                console.log("reasonHoliday", $('#reasonHoliday').val());
                $('#msjReason').css({ 'display': "block" });
                ok = false;
            } else {
                $('#msjReason').css({ 'display': "none" });
            }


            if ($('#dateHoliday').val() == "Select a Date") {

                console.log("dateHoliday", $('#dateHoliday').val());
                $('#msjDate').css({ 'display': "block" });
            }
            else {
                if (ok) {

                    $('#msjDate').css({ 'display': "none" });

                    var data = {
                        HolidayID: $('#HolidayID').val(),
                        DateHoliday: $('#dateHoliday').val(),
                        StateProvinceID: $('#Holiday_StateID').val()
                    };

                    $.post('/GeneralLedger/Holiday/ValidateHoliday', data, function (response) {
                        if (response.result) {
                            showMessage(response.msg, true);
                        } else {

                            $("#btnSubmit").click();
                        }
                    });
                }
            }
        }
    </script>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("HolidayCalendarAddNew", "Add New Holiday")%></h2>
        <%-- <%= Html.Term("BrowseCatalogs", "Browse Catalogs") %>
        | <a href="<%= ResolveUrl("~/GeneralLedger/Holiday/Index") %>">
            <%= Html.Term("CreateaNewCatalog", "Create a New Catalog") %></a>--%>
    </div>
    <% using (Html.BeginForm("AddNew", "Holiday", FormMethod.Post, new { id = "frmHolidayAddNew" }))
       {
           //Html.AntiForgeryToken();
    %>
    <table class="FormTable" width="40%">
        <tr>
            <td>
                Country:
            </td>
            <td>
                <%= Html.DropDownListFor(m =>m.Holiday.CountryID, new SelectList(Model.Countries, "CountryID", "Name"))%>
                 <%= Html.HiddenFor(m => m.Holiday.HolidayID, new { @id="HolidayID"})%>
            </td>
            <td>
                <%= Html.CheckBoxFor(m => m.Holiday.Active, new {Checked=true })%>
                Active
            </td>
        </tr>
        <tr>
            <td>
                State:
            </td>
            <td>
                <%= Html.DropDownListFor(m => m.Holiday.StateID, new SelectList(Model.StateProvinces, "StateProvinceID", "Name"),"Select a State")%>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td>
                Date:
            </td>
            <td>
                <%= Html.TextBoxFor(m => m.dateHidden, new {id="dateHoliday", @class = "DatePicker TextInput", Value = "Select a Date" })%>
                <div id="msjDate" style="display: none;">
                <p>
                    <small  style="color: Red;">Select a Date</small></p>
                </div>
                
            </td>
            <td>
                <%= Html.CheckBoxFor(m => m.Holiday.IsIterative, new {  Checked = "checked" })%>
                is iterative?
            </td>
        </tr>
        <tr>
            <td>
                Reason:
            </td>
            <td colspan="3">
                <%= Html.TextAreaFor(m => m.Holiday.Reason, new { id = "reasonHoliday", rows = "5", maxlength = "250", style = "width: 100%" })%>
                <div id="msjReason" style="display: none;">
                <p>
                    <small  style="color: Red;">Required</small></p>
                </div>
                
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <input type="button" id="btnSave" onclick="return validarCampos();" style="display: inline-block;
                        cursor: pointer;" class="Button BigBlue" value="Add Holiday" />
                    <%--<a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue">
                        <%= Html.Term("AddHoliday", "Add Holiday")%></a>--%>
                         <input type="submit" id="btnSubmit" style="display:none" />
                </p>
            </td>
        </tr>
    </table>
    <%--<p>
        
    </p>--%>
    <% }%>
</asp:Content>
