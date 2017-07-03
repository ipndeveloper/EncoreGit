<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/RoutesManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Logistics", "Logistics")%>
        </h2>
        <a href="#" name="newzone">
            <%= Html.Term("AddaNewZone", "Add a New Zone")%></a>
    </div>
    <span class="ClearAll"></span>
    <%=Html.Term("Name", "Name")%>
    *:
    <% List<RoutesData> Zonas = ViewData["zones"] as List<RoutesData>;
       string routeID = "";
       string Name = "";
       if (Zonas.Count > 0)
       {
           routeID = Zonas[0].RouteID.ToString();
           Name = Zonas[0].Name.ToString();
       }
       else
       {
           routeID = "0";
           Name = "";
       }
    %>
    <table id="newRule">
        <tr>
            <td>
                <input type="text" id="nomRoute" value="<%=Name %>" class="required" name="<%= Html.JavascriptTerm("valNewRoute.", "Route name can not be null.") %>" />
                <input type="hidden" id="RouteID" value="<%=routeID %>" />
                <input type="hidden" id="hdDeletedZones" />
            </td>
        </tr>
    </table>
    <%-- <a href="#" id="sea">Empty</a>--%>
    <span class="ClearAll"></span>
    <br />
    <span class="ClearAll"></span>
    <% Html.PaginatedGrid<ZonesData>("~/Logistics/Routes/GetZones/" + (routeID.IsNullOrEmpty() ? "" : routeID))
            .AutoGenerateColumns()
            .HideClientSpecificColumns_()
            .CanChangeStatus(true, true, "")
            .ClickEntireRow()
            .Render(); 
    %>
    <span class="ClearAll"></span>
    <p>
        <a id="btnSaveRoute" href="javascript:void(0);" class="Button BigBlue">
            <%= Html.Term("Save", "Save")%>
        </a>
    </p>
    <div id="addNewZoneModel" class="jqmWindow LModal Overrides">
        <div class="mContent">
            <input type="hidden" id="txtWareHousesMaterialID" />
            <h2>
                <%= Html.Term("AddaNewZone", "Add a New Zone")%>
            </h2>
            <table id="newZone" class="FormTable Section">
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("State", "State")%>
                        :
                    </td>
                    <td>
                        <select id="sState" class="required" name="<%= Html.JavascriptTerm("valStateProvince", "Value to States Provinces not selected.") %>">
                            <option value="">Select a State </option>
                            <% foreach (var items in ViewData["StatesProvinces"] as List<StateProvincesData>)
                               {
                            %>
                            <option value="<%=items.StateProvinceID %>">
                                <%=items.Name%></option>
                            <%                                       
                               }                    
                            %>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("City", "City")%>
                        :
                    </td>
                    <td>
                        <select id="sCity">
                            <option value="">Select a City </option>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("Except", "Except")%>
                        :
                    </td>
                    <td>
                        <input type="checkbox" id="checkExcept" />
                    </td>
                </tr>
            </table>
            <span class="ClearAll"></span>
            <p>
                <a id="SaveZone" href="javascript:void(0);" class="Button BigBlue">
                    <%= Html.Term("Save", "Save")%>
                </a><a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Cancel")%>
                </a>
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            //            $('.clsPending').foreach(function () { $(this).val('Pendiente por grabar') })

            if ($('#sState').val() != null) {
                $('#sState').clearError();
            }
            $('#sState').change(function () {
                //var val = this.val();
                if ($('#sState').val() != null) {
                    $('#sState').clearError();
                }
            });

            var r = $('<a class="Delete UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-deleteSelected"></span><span><%= Html.Term("RemoveSelected", "Remove Selected")%></span></a><span class="pipe"></span>');
            var c = $('<a name="newzone" class="newzone UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-plus icon-activate"></span><span><%= Html.Term("AddaNewZone", "Add a New Zone")%></span></a><span class="pipe"></span><span class="ClearAll"></span>');
            var space = $('<span class="ClearAll"></span>')

            $("#paginatedGridOptions").append(r);
            $("#paginatedGridOptions").append(c);
            $("#paginatedGridOptions").append(space);
            $(".deactivateButton").css("display", "none");
            $(".activateButton").css("display", "none");

            $('#addNewZoneModel').jqm({ modal: false, onShow: function (h) {
                h.w.css({
                    top: Math.floor(parseInt($(window).height() / 10)) - Math.floor(parseInt(h.w.height() / 5)) + 'px',
                    left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });
            //Abrir Modal NewZone
            $('a[name=newzone]').click(function (e) {
                $('#addNewZoneModel').jqmShow();
            });

            $('#SaveZone').click(function (e) {
                if ($('#newZone').checkRequiredFields()) {
                    if (!$('#sState').val() && $('#sState').is(':visible')) {
                        $('#sState').showError('<%= Html.JavascriptTerm("valStateProvince", "Value to States Provinces not selected.") %>');
                        errorCount++;
                    }

                    var state = $("#sState option:selected").html();
                    var city = $("#sCity option:selected").html();
                    var col3 = $('#checkExcept').is(":checked");
                    var scopeLevelID;

                    if ($("#sState option:selected").val() != "" && $("#sCity option:selected").val() != "") {
                        scopeLevelID = 2;
                    }
                    if ($("#sState option:selected").val() != "" && $("#sCity option:selected").val() == "") {
                        scopeLevelID = 1;
                    }
                    if (scopeLevelID == 1 || scopeLevelID == 2) {
                        $.get('<%= ResolveUrl(string.Format("~/CTE/PaymentsMethodsConfiguration/GetScopeLevels/")) %>', { scopeLevelID: scopeLevelID }, function (response) {
                            if (response.result) {
                                if (response.scopeLevels) {

                                    row = $('<tr class="Alt hover"></tr>');
                                    //                                check = $('<td>' + '<input type="checkbox" >' + '</td>');
                                    check = $('<td>' + '<input type="checkbox" ><input type="hidden" name = "isNew" value="1" />' + '</td>');
                                    colState = $('<td>' + response.scopeLevels[0].Name + '</td>');
                                    if (scopeLevelID == 1) colCity = $('<td>' + state + '</td>');
                                    if (scopeLevelID == 2) colCity = $('<td>' + city + '</td>');
                                    //if (scopeLevelID == 3) colCity = $('<td>' + county + '</td>');
                                    colExcept = $('<td>' + col3 + '</td>');
                                    row.append(check, colState, colCity, colExcept).prependTo("#paginatedGrid tbody");
                                    //Close Modal                                
                                    $('#addNewZoneModel').jqmHide();
                                    $('#sState').val('');
                                    $('#sCity').val('');
                                    $('#checkExcept').attr('checked', false);
                                }
                            } else {
                                showMessage(response.message, true);
                            }
                        });
                    }
                }
            });
            //Eliminar        
            $('.Delete').click(function (e) {
                try {
                    var table = document.getElementById('paginatedGrid');
                    var rowCount = table.rows.length;

                    //                    alert(DeletedData.length)
                    for (var i = 0; i < rowCount; i++) {
                        var row = table.rows[i];
                        var chkbox = row.cells[0].childNodes[0];
                        if (chkbox != null && chkbox.checked == true) {
                            //                        var text = row.cells[0].childNodes[1];
                            var Name = row.cells[1].innerHTML;
                            var Value = row.cells[2].innerHTML;
                            var Except = row.cells[3].innerHTML;
                            var dz = $('#hdDeletedZones').val() + Name + ',' + Value + ',' + Except + '*';
                            $('#hdDeletedZones').val(dz);
                            //                            DeletedData
                            //                            data['zones[' + idx + '].Name'] = Name;
                            //                            data['zones[' + idx + '].Value'] = Value;
                            //                            data['zones[' + idx + '].Except'] = Except;
                            //                            data['zones[' + idx + '].Inserted'] = 2;

                            //                        alert(text);
                            table.deleteRow(i);
                            rowCount--;
                            i--;
                        }
                    }
                } catch (e) {
                    alert(e);
                }
            });
            //
            $('#sState').change(function () {
                $('#sCity').prop('selectedIndex', 0);
                var t = $(this);
                var state = $("#sState option:selected").html();
                showLoading(t);
                $.get('<%= ResolveUrl(string.Format("~/Logistics/Routes/GetCitys")) %>', { state: state }, function (response) {
                    if (response.result) {
                        hideLoading(t);
                        $('#sCity').children('option:not(:first)').remove();
                        if (response.Cities) {
                            for (var i = 0; i < response.Cities.length; i++) {
                                $('#sCity').append('<option value="' + response.Cities[i].City + '">' + response.Cities[i].City + '</option>');
                            }
                        }
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });
            //Guardar
            $('#btnSaveRoute').click(function () {
                if ($('#newRule').checkRequiredFields()) {
                    var data = { nameRoute: $('#nomRoute').val(), routeID: $('#RouteID').val(), deletedzones: $('#hdDeletedZones').val() }, t = $(this);

                    t = $(this);
                    $('#paginatedGrid tbody:first tr').each(function (i) {
                        var val = $(this).find('input:hidden').val();
                        if (val == 1) {
                            data['zones[' + i + '].Name'] = $(this).find("td").eq(1).html();
                            data['zones[' + i + '].Value'] = $(this).find("td").eq(2).html();
                            data['zones[' + i + '].Except'] = $(this).find("td").eq(3).html();
                            //                            data['zones[' + i + '].Inserted'] = 1;
                        }

                    });
                    $.post('/Logistics/Routes/SaveRoutes', data, function (response) {
                        if (response.result) {
                            showMessage("Route was saved!", false);
                            $('#hdDeletedZones').val('');
                            window.location = '<%= ResolveUrl("~/Logistics/Routes/NewRoute/") %>' + response.RouteID;

                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }

            });
            //

        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
