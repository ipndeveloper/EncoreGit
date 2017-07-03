<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/LogiticsProvDetail.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.LogisticProviderSuppliedIDs>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.DatePicker').datepicker("option", "yearRange", "-99:+0");
            $('.DatePicker').datepicker("option", "maxDate", "+0m +0d");
            //$("#ExpDate").keypress(function (evt) { return false; });

            $('#ExpDate').datepicker('setDate', '0');
//            $('#ExpDate').datepicker({
//                dateFormat: 'dd/mm/yy'
//            }).datepicker('setDate', '0');

//            var dateFormat = $('.DatePicker').datepicker("option", "dateFormat");
//            $('.DatePicker').datepicker("option", "dateFormat", "dd/mm/yy");

            $('#btnToggleStatus').click(function () {
                var t = $(this);
                var txtActive = $('#txtActive').val();
                var active = 0;
                if (txtActive == 1) {
                    active = 0
                }
                else {
                    active = 1
                }
                showLoading(t);
                $.post('<%= ResolveUrl(string.Format("~/Logistics/Logistics/ToggleStatus")) %>', { LogisticsProviderID: $('#LogisticsProviderID').val(), active: active }, function (response) {
                    hideLoading(t);
                    if (response.result) {
                        t.toggleClass('ToggleInactive');
                        window.location = '<%= ResolveUrl("~/Logistics/Logistics/Documents/") %>' + $('#LogisticsProviderID').val();
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });

            //Guardar
            $('#btnSave').click(function () {
                if ($('#newDocumento').checkRequiredFields()) {
                    // 
                    var startDate = $('#ExpDate').val();

                    //inicio 04062017 comentado por IPN
//                    if (validaFechaDDMMAAAA(startDate)) {
//                        if (existeFecha2(startDate)) {

//                        } else {
//                            showMessage('<%= Html.Term("valdate", "Date Incorrect.") %>', true);
//                            return false;
//                        }
//                    } else {
//                        showMessage('<%= Html.Term("formtvaliddate", "Format date Incorrect,this is Format Correct:(Day/Month/Year).") %>', true);
//                        return false;
//                    }

                    // FIN 

                    if (existeFecha2(startDate)) {

                    } else {
                        showMessage('<%= Html.Term("valdate", "Date Incorrect.") %>', true);
                        return false;
                    }








                    //                var dateTimeSplit = startDate.split(' ');
                    //                var dateSplit = dateTimeSplit[0].split('/');
                    //                var currentDate = dateSplit[1] + '/' + dateSplit[0] + '/' + dateSplit[2];

                    var documents = {
                        LogisticsProviderID: $('#LogisticsProviderID').val(),
                        IDTypeID: $('#DocumentType').val(),
                        IDValue: $('#DocumentNumber').val(),
                        //                    IDExpeditionDate: currentDate.toString(),
                        IDExpeditionDate: $('#ExpDate').val(),
                        ExpeditionEntity: $('#ExpEntity').val(),
                        IsPrimaryID: $('#IsPrimary').is(":checked")
                    };
                    $.post('/Logistics/Logistics/SaveDocuments', documents, function (response) {
                        if (response.result) {
                            showMessage('<%= Html.Term("DocSaved", "Document Associated Correctly") %>', false);

                            window.location = '<%= ResolveUrl("~/Logistics/Logistics/Documents/") %>' + $('#LogisticsProviderID').val();
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            });
            //Validar Fechas
            var ano = (new Date).getFullYear();
            function existeFecha2(fecha) {
                var fechaf = fecha.split("/");
                var d = fechaf[0];
                var m = fechaf[1];
                var y = fechaf[2];
                return m > 0 && m < 13 && y > 0 && y <= ano && d > 0 && d <= (new Date(y, m, 0)).getDate();
            }
            function validaFechaDDMMAAAA(fecha) {
                var dtCh = "/";
                var minYear = 1900;
                var maxYear = 2100;
                function isInteger(s) {
                    var i;
                    for (i = 0; i < s.length; i++) {
                        var c = s.charAt(i);
                        if (((c < "0") || (c > "9"))) return false;
                    }
                    return true;
                }
                function stripCharsInBag(s, bag) {
                    var i;
                    var returnString = "";
                    for (i = 0; i < s.length; i++) {
                        var c = s.charAt(i);
                        if (bag.indexOf(c) == -1) returnString += c;
                    }
                    return returnString;
                }
                function daysInFebruary(year) {
                    return (((year % 4 == 0) && ((!(year % 100 == 0)) || (year % 400 == 0))) ? 29 : 28);
                }
                function DaysArray(n) {
                    for (var i = 1; i <= n; i++) {
                        this[i] = 31
                        if (i == 4 || i == 6 || i == 9 || i == 11) { this[i] = 30 }
                        if (i == 2) { this[i] = 29 }
                    }
                    return this
                }
                function isDate(dtStr) {
                    var daysInMonth = DaysArray(12)
                    var pos1 = dtStr.indexOf(dtCh)
                    var pos2 = dtStr.indexOf(dtCh, pos1 + 1)
                    var strDay = dtStr.substring(0, pos1)
                    var strMonth = dtStr.substring(pos1 + 1, pos2)
                    var strYear = dtStr.substring(pos2 + 1)
                    strYr = strYear
                    if (strDay.charAt(0) == "0" && strDay.length > 1) strDay = strDay.substring(1)
                    if (strMonth.charAt(0) == "0" && strMonth.length > 1) strMonth = strMonth.substring(1)
                    for (var i = 1; i <= 3; i++) {
                        if (strYr.charAt(0) == "0" && strYr.length > 1) strYr = strYr.substring(1)
                    }
                    month = parseInt(strMonth)
                    day = parseInt(strDay)
                    year = parseInt(strYr)
                    if (pos1 == -1 || pos2 == -1) {
                        return false
                    }
                    if (strMonth.length < 1 || month < 1 || month > 12) {
                        return false
                    }
                    if (strDay.length < 1 || day < 1 || day > 31 || (month == 2 && day > daysInFebruary(year)) || day > daysInMonth[month]) {
                        return false
                    }
                    if (strYear.length != 4 || year == 0 || year < minYear || year > maxYear) {
                        return false
                    }
                    if (dtStr.indexOf(dtCh, pos2 + 1) != -1 || isInteger(stripCharsInBag(dtStr, dtCh)) == false) {
                        return false
                    }
                    return true
                }
                if (isDate(fecha)) {
                    return true;
                } else {
                    return false;
                }
            }
            //
        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("DocumentsProv", "Documents")%>
        </h2>
    </div>
    <table id="newDocumento" class="FormTable Section" width="100%">
        <% List<LogisticsProviderSearData> details = ViewData["details"] as List<LogisticsProviderSearData>;
           string LogisticsProviderID = "";
           if (details.Count > 0)
           {
               LogisticsProviderID = details[0].LogisticsProviderID.ToString();
           }
           else
           {
               LogisticsProviderID = "";
           }
        %>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Document Type")%>
                :
            </td>
            <td>
                <select id="DocumentType">
                    <% foreach (var items in ViewData["DocumentType"] as List<LogisticProviderSuppliedIDs>)
                       {
                    %>
                    <option value="<%=items.IDTypeID %>">
                        <%=items.Name%></option>
                    <%                                       
                           }                    
                    %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Document Number")%>
                :
            </td>
            <td>
                <input id="DocumentNumber" type="text" class="required" name="<%= Html.JavascriptTerm("ProvDocNumber", "Document Number is required.") %>" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Exp Date")%>
                :
            </td>
            <td>
                <input id="ExpDate" type="text" class="DatePicker" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Exp Entity")%>
                :
            </td>
            <td>
                <input id="ExpEntity" type="text" />
                <input id="LogisticsProviderID" type="hidden" value="<%=LogisticsProviderID %>" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Is Primary")%>
                :
            </td>
            <td>
                <input id="IsPrimary" type="checkbox" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue">
                        <%= Html.Term("Add", "Add")%></a>
                </p>
            </td>
        </tr>
    </table>
    <span class="ClearAll"></span>
    <% Html.PaginatedGrid<LogisticProviderSuppliedIDs>("~/Logistics/Logistics/GetDocuments/" + LogisticsProviderID.ToString())
            .AutoGenerateColumns()
            .HideClientSpecificColumns_()
            .CanDelete("~/Logistics/Logistics/DeleteDocuments")
            .ClickEntireRow()
            .Render(); 
    %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="YellowWidget" runat="server">
    <% List<LogisticsProviderSearData> details = ViewData["details"] as List<LogisticsProviderSearData>;
       string Name = "";
       string LogisticsProviderID = "";
       bool Active = false;
       if (details.Count > 0)
       {
           LogisticsProviderID = details[0].LogisticsProviderID.ToString();
           Name = details[0].Name.ToString();
           Active = Convert.ToBoolean(details[0].Active);
           int activeprov = Convert.ToInt32(details[0].Active);   
    %>
    <div class="TagInfo">
        <div class="Content">
            <div class="SubTab">
                <a>
                    <%=Name%>
                </a>
            </div>
            <table class="DetailsTag Section" width="100%">
                <tr>
                    <td class="Label">
                        <%= Html.Term("Code", "Code")%>:
                    </td>
                    <td>
                        <a>
                            <%=LogisticsProviderID %></a>
                    </td>
                </tr>
                <tr>
                    <td class="Label">
                        <%= Html.Term("Status", "Status") %>:
                    </td>
                    <td>
                        <input type="hidden" value="<%=activeprov %>" id="txtActive" />
                        <a id="btnToggleStatus" href="javascript:void(0);" class="Toggle ToggleActive<%= !Active ? " ToggleInactive" : "" %>">
                        </a>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                </tr>
            </table>
        </div>
        <div class="TagBase">
        </div>
    </div>
    <% } %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Logistics") %>">
        <%= Html.Term("Logistics") %></a> >
    <%= Html.Term("DocumentsProv", "Documents")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
    <% List<LogisticsProviderSearData> details = ViewData["details"] as List<LogisticsProviderSearData>;
       string LogisticsProviderID = "";

       if (details.Count > 0)
       {
           LogisticsProviderID = details[0].LogisticsProviderID.ToString();
       }
       else
       {
           LogisticsProviderID = "";
       }
    %>
    <div class="SectionNav">
        <ul class="SectionLinks">
            <%= Html.SelectedLink("~/Logistics/Logistics/ProviderDetails/" + LogisticsProviderID, Html.Term("Details", "Details"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
            <%= Html.SelectedLink("~/Logistics/Logistics/Documents/" + LogisticsProviderID, Html.Term("DocumentsProv", "Documents"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
            <%--<%= Html.SelectedLink("~/Logistics/Logistics/Address/" + LogisticsProviderID, Html.Term("Address", "Address"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>--%>
            <%= Html.SelectedLink("~/Logistics/Logistics/Routes/" + LogisticsProviderID, Html.Term("Routes", "Routes"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
        </ul>
    </div>
</asp:Content>
