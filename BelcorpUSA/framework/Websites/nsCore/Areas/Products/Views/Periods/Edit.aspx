<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Schedules/Schedules.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.PeriodSearchData>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
    <script type="text/javascript">
        function toDate(dateStr) {
            var parts = dateStr.split("/");
            //dateStr inpunt dd/mm/yy
            //new date(year,month,day)
            return new Date(parts[2], parts[1] - 1, parts[0]);
        }

        //Get Today - Start

        var currentYear = (new Date).getFullYear();
        var currentMonth = (new Date).getMonth() + 1;
        var currentDay = (new Date).getDate();
        var today = currentDay + '/' + currentMonth + '/' + currentYear;
        var tDate = toDate(today);

        //Get Today - End

        $(function () {
            var data = {
                planId: $('#PlanID').val(),
                periodID: $('#hddPeriodId').val()
            };

            $.post('<%= ResolveUrl("~/Products/Periods/GetStartDateToSelectedPlan") %>', data, function (response) {
                if (response.result && !response.isEdit)//New
                {
                    $('#txtStartDate, #txtLockDate, #txtEndDate').val((response.startDate));
                    $('#txtStartDate').attr('disabled', 'disabled');
                }
                else if (response.result && response.isEdit)//Edit
                {
                    $('#txtStartDate').val((response.startDate));
                    $('#txtLockDate').val((response.lockDate));
                    $('#txtEndDate').val((response.endDate));

                    //Bloquear todos
                    $('#txtStartDate').attr('disabled', 'disabled');
                    $('#txtLockDate').attr('disabled', 'disabled');
                    $('#txtEndDate').attr('disabled', 'disabled');

                    //Desbloquear los que se puedan editar de acuerdo al today

                    //                    alert('startDate:' + toDate(response.startDate) + '-----,lockDate:' + toDate(response.lockDate) + '----,endDate: ' + toDate(response.endDate) + '-----,Hoy:' + tDate);
                    if (toDate(response.startDate) >= tDate) $('#txtStartDate').removeAttr('disabled');
                    if (toDate(response.lockDate) >= tDate) $('#txtLockDate').removeAttr('disabled');
                    if (toDate(response.endDate) >= tDate) $('#txtEndDate').removeAttr('disabled');

                } else {
                    $('#txtStartDate, #txtLockDate, #txtEndDate').val("");
                    $('#txtStartDate').removeAttr('disabled');
                }
            });

//            $.datepicker.regional['es'] = {
//                closeText: 'Cerrar',
//                prevText: '<Ant',
//                nextText: 'Sig>',
//                currentText: 'Hoy',
//                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
//                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
//                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
//                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
//                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
//                weekHeader: 'Sm',
//                dateFormat: 'dd/mm/yy',
//                firstDay: 1,
//                isRTL: false,
//                showMonthAfterYear: false,
//                yearSuffix: ''
//            };

            //$.datepicker.setDefaults($.datepicker.regional['es']);

            $('#btnSave').click(function () {
                if ($('#periodForm').checkRequiredFields()) {
                    var data = {
                        periodId: $('#hddPeriodId').val(),
                        startDate: $('#txtStartDate').val(),
                        endDate: $('#txtEndDate').val(),
                        lockDate: $('#txtLockDate').val(),
                        planId: $('#PlanID').val(),
                        description: $('#txtDescription').val()
                    };

                    var isValid = rulesVerify();
                    switch (isValid) {
                        case 1:
                            showMessage("Check input values", true);
                            return;
                        case 2:
                            showMessage("Period cannot be modified", true);
                            return;
                        default:
                            break;
                    }

                    $.post('<%= ResolveUrl("~/Products/Periods/Save") %>', data, function (response) {
                        if (response.result) {
                            showMessage('<%=@Html.Term("PeriodSavedSuccessfully", "Period Saved Successfully")%>', false);
                            if (!$('#hddPeriodId').val()) { // Create case
                                $('#hddPeriodId').val(response.periodId);
                                // Reload Edit Mode 
                                window.location.replace('<%= ResolveUrl("~/Products/Periods/Edit") %>' + "/" + response.periodId);
                            }
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                    //                    $('#txtStartDate').prop('disabled', false);
                }
            });

            $('#PlanID').change(function () {
                if (!$('#hddPeriodId').val()) {
                    // create mode: maxDay(EndDay when planID = selectPlanID) + 1
                    // hasValue->value and ReadOnly, else->Editable without value

                    var data = {
                        planId: $('#PlanID').val()
                    };

                    $.post('<%= ResolveUrl("~/Products/Periods/GetStartDateToSelectedPlan") %>', data, function (response) {
                        if (response.result) {
                            $('#txtStartDate').val(response.startDate);
                            $('#txtStartDate').attr('disabled', 'disabled');
                        } else {
                            $('#txtStartDate').val("");
                            $('#txtStartDate').removeAttr('disabled');
                        }
                    });
                }
            });
        });



        function rulesVerify() {
            var verify = 0; //0: cumple con todas las validaciones, >0: no cumple algunas reglas de validacion
            var eDate = toDate($('#txtEndDate').val());
            var sDate = toDate($('#txtStartDate').val());
            var lDate = toDate($('#txtLockDate').val());
            if ($('#hddPeriodId').val()) {
                // update mode
                var isDisabled = $('#txtStartDate').is(':disabled');
                if (!isDisabled) {//Si startDate esta habilitado
                    if (sDate <= tDate) verify = 1; 
                }
            }
            if (lDate.getTime() != eDate.getTime() || eDate <= sDate) verify = 1;
            if (eDate < tDate) verify = 2;
            return verify;
        }
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Plans") %>">
            <%= Html.Term("ScheduleManagement", "Schedule Management")%></a> >
    <%= Model.PeriodID == 0 ? Html.Term("NewPeriod", "New Period") : Html.Term("EditPeriod", "Edit Period") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Periods", "Periods")%>
        </h2>
        <a href="<%= ResolveUrl("~/Products/Periods") %>">
            <%= Html.Term("BrowsePeriods", "Browse Periods")%></a> | <a href="<%= ResolveUrl("~/Products/Periods/Edit") %>">
                <%= Html.Term("CreateaNewPeriod", "Create a New Period")%></a>
    </div>
    <table id="periodForm" class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Plans", "Plans") %>:
            </td>
            <td>
                <input type="hidden" id="hddPeriodId" value="<%= Model.PeriodID == 0 ? "" : Model.PeriodID.ToString() %>" />
                <% if (Model.PeriodID == 0)
                   { 
                %>
                <%: Html.DropDownListFor(x => x.PlanID, TempData["Plans"] as IEnumerable<SelectListItem>)%>
                <% }
                   else
                   {
                %>
                <%: Html.DropDownListFor(x => x.PlanID, TempData["Plans"] as IEnumerable<SelectListItem>, new { @disabled = "disabled" })%>
                <% } %>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("StartDate", "Start Date") %>:
            </td>
            <td>
                <input id="txtStartDate" type="text" class="DatePicker" value="" style="width: 55px;" /><br />
                <%--<input id="txtStartDate" type="text" class="DatePicker" value="" 
                    <%= Model.PeriodID != 0 ? "disabled=\"disabled\"" : "" %>
                    <%= Model.ClosedDate.HasValue ? "disabled=\"disabled\"" : "" %> <%= Model.ExistPreviousStartDate ? "disabled=\"disabled\"" : "" %>
                    style="width: 55px;" /><br />--%>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("LockDate", "Lock Date") %>:
            </td>
            <td>
                <input id="txtLockDate" type="text" class="DatePicker" value="" <%= Model.ClosedDate.HasValue ? "disabled=\"disabled\"" : "" %>
                    style="width: 55px;" /><br />
                <%--<input id="txtLockDate" type="text" class="DatePicker" value="<%= Model.LockDate.ToString("dd/MM/yyyy") %>"
                    <%= Model.ClosedDate.HasValue ? "disabled=\"disabled\"" : "" %>
                    style="width: 9.091em;" /><br />--%>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("EndDate", "End Date") %>:
            </td>
            <td>
                <input id="txtEndDate" type="text" class="DatePicker" value="" <%= Model.ClosedDate.HasValue ? "disabled=\"disabled\"" : "" %>
                    style="width: 55px;" /><br />
                <%--<input id="txtEndDate" type="text" class="DatePicker" value="<%= Model.EndDate.ToString("dd/MM/yyyy")  %>"
                    <%= Model.ClosedDate.HasValue ? "disabled=\"disabled\"" : "" %>
                    style="width: 9.091em;" /><br />--%>
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Description", "Description")%>:
            </td>
            <td>
                <input id="txtDescription" type="text" value="<%= Model.Description %>" maxlength="50"
                    class="required" name="<%= Html.Term("DescriptionIsRequired", "Description is required") %>"
                    <%= Model.ClosedDate.HasValue ? "disabled=\"disabled\"" : "" %> style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue"
                        <%= Model.ClosedDate.HasValue ? "disabled=\"disabled\"" : "" %>>
                        <%= Html.Term("SavePeriod", "Save Period") %></a>
                </p>
            </td>
        </tr>
    </table>
</asp:Content>
