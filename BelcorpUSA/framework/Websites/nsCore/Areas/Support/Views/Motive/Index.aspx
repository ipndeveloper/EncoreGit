<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Support/Views/Shared/Support.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.SupportMotiveSearchParameters>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/json2.js") %>"></script>
    <div class="SectionNav">
        <% 
            NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();		        
        %>
        <ul class="SectionLinks">
            <%= Html.SelectedLink("~/Support/Level/EditTree/", Html.Term("SupportLevels", "Support Levels"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
            <%= Html.SelectedLink("~/Support/Motive/Index/", Html.Term("BrowseSupportMotives", "Browse Support Motives"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
            <%= Html.SelectedLink("~/Support/Motive/Edit/", Html.Term("AddNewSupportMotive", "Add New Support Motive"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
        </ul>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <%
        NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();
    %>
    <a href="<%= ResolveUrl("~/Support") %>">
        <%= Html.Term("Support", "Support")%></a> &gt;>
    <%= Html.Term("MotivesManagemente", "Motives Managemente")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("SupportMotives", "Support Motives")%>
        </h2>
        <%= Html.Term("BrowseMotives", "Browse Motives")%>
        | <a href="<%= ResolveUrl("~/Support/Motive/Edit") %>">
            <%= Html.Term("CreateaNewMotive", "Create a New Motive")%></a>
    </div>
    <% Html.PaginatedGrid<SupportMotiveSearchData>("~/Support/Motive/GetSupportMotives")
        .AutoGenerateColumns()
        .AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") }, { "true", Html.Term("Active") }, { "false", Html.Term("Inactive") } })
        .AddInputFilter(Html.Term("ID Motivo or Name"), "name")
        .ClickEntireRow()
        .Render(); %>
    <script>
        var idSupportMotiveSeleccionado = 0;
        $(document).ready(function () {

            SetParamSearchGridView("SupportLevelIDs", "");
            $("#cmbParentLevel").change(function () {
                cargarHijo(this);
            });

            $("#cmbParentLevel").change();



        });

        var SupportLevelID = 0;
        function cargarHijo(ctr) {
            var value = $(ctr).val();
            var seleccionados = SupportLevelSeleccionados();
            SupportLevelID = value;
            SetParamSearchGridView("SupportLevelIDs", seleccionados);

            // $(".filterButton").click();

            if (value == 0) {
                clearNext($(ctr).parent("td"));
                return false;
            }


            $("#tblDetaill tbody tr").remove();
            var odata = JSON.stringify({ input: value });
            clearNext($(ctr).parent("td"));
            var url = '<%= ResolveUrl("~/Support/Ticket/ListarSupportLevel") %>';
            $.ajax({
                data: odata,
                url: url,
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                success: function (response) {
                    var respuesta = response;

                    if (respuesta.isLast) {
                        // SeleccionadoSupportLevelID=value;
                        // crearDetalle(ctr,respuesta.items)
                    } else {
                        crearHijo(ctr, value, respuesta.items);
                    }
                },
                error: function (error) {
                }
            });
        }

        function crearDetalle(ctr, lista) {
            var FilaActual = $(ctr).parent("td").parent("tr")

            var MensajeDefecto = '<%= Html.JavascriptTerm("SelectedSupportLevelMotive", "-------Selected Support Level Motive-------") %>';

            var comboMotivos = $("<select  onchange='SeleccionarMotivo(this)' id='cmbMotivos'>");
            $(comboMotivos).append("<option value='0'>" + MensajeDefecto + "</option>");


            for (var index = 0; index < lista.length; index++) {
                $(comboMotivos).append("<option SupportLevelID='" + lista[index].Item1 + "'SupportMotiveID='" + lista[index].Item3 + "' value ='" + lista[index].Item3 + "'>" + lista[index].Item2 + "</option>");
            }
            var celda = $("<td id='pnlMotivo'></td>")
            celda.append(comboMotivos);
            $(FilaActual).append(celda);



        }

        function crearHijo(parent, id, lista) {
            var filaPadre = $(parent).parent("td").parent("tr");
            var celdaPadre = $(parent).parent("td");
            var idPadre = $(parent).val();

            var celda = $("<td id='celda" + idPadre + "-" + id + "'></td>");

            var MensajeDefecto = '<%= Html.JavascriptTerm("SelectedSupportLevel", "-------Selected Support Level-------") %>';

            var comboHijo = $("<select onchange='cargarHijo(this)'  id='" + idPadre + "-" + id + "'></select>");
            $(comboHijo).append("<option value='0'>" + MensajeDefecto + "</option>");
            for (var index = 0; index < lista.length; index++) {
                $(comboHijo).append("<option value='" + lista[index].Item1 + "'>" + lista[index].Item2 + "</option>");
            }
            $(celda).append($(comboHijo));
            $(filaPadre).append($(celda));



        }


        function clearNext(ctr) {
            var eliminar = [];
            var next = $(ctr);
            while (next != null && next.html() != null) {
                next = $(next).next("td");
                eliminar.push($(next).attr("id"));
            }
            for (var index = 0; index < eliminar.length; index++) {
                $("#" + eliminar[index]).remove();
            }
            SupportLevelID = 0;
        }
        function SupportLevelSeleccionados() {
            var seleccion = "";
            var filterLevelNode = document.getElementById("filterLevelNode")
            var selects = filterLevelNode.getElementsByTagName("select")
            var Selecion = "";

            for (var index = 0; index < selects.length; index++) {
                var value = selects[index].value;
                var SelectedIndex = 0;


                if (value != 0) {
                    seleccion += value + ";";
                }

            }
            if (seleccion == "") {
                seleccion = "0;";
            }
            return seleccion;

        }
    </script>
    <div id="PnlTblSupportLevel" class="FL">
        <table id="filterLevelNode">
            <tr>
                <td colspan="10">
                    <%= Html.Term("SupportLevel", "Support Level")%>:
                    <%
                        List<System.Tuple<int, string, int>> lstSupportLevelParent = ViewBag.lstSupportLevelParent as List<System.Tuple<int, string, int>>;
                 
                    %>
                    <table id="TblSupportLevel">
                        <tr>
                            <td>
                                <select id="cmbParentLevel">
                                    <option value="0">
                                        <%= Html.Term("SelectedSupportLevel", "-------Selected Support Level-------")%></option>
                                    <%for (int index = 0; index < lstSupportLevelParent.Count; index++)
                                      { %>
                                    <option value="<%=lstSupportLevelParent[index].Item1%>">
                                        <%=lstSupportLevelParent[index].Item2%>
                                    </option>
                                    <%} %>
                                </select>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <script>
        $(document).ready(function () {
            setTimeout(function () {
                var PnlTblSupportLevel = $("#PnlTblSupportLevel")
                PnlTblSupportLevel.insertAfter($(".FilterSet .FL:eq(1)"));

            }, 2000);
        });
    </script>
</asp:Content>
