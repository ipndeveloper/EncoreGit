<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Business.ShippingRulesLogisticsSearchData>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="NetSteps.Common.Globalization" %>
<div id="divAreaGeographical" class="none">
<input type="hidden" id="areasdelete" value="0"/>
    <div id="paginatedGridOptions2" class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility">
        <a id="tgDeleteSelected" class="UI-icon-container" href="javascript:void(0);"><span
            class="UI-icon icon-x icon-deactive"></span><span>
                <%= Html.Term("RemoveSelected", "Remove Selected")%></span> </a><span class="pipe">&nbsp;</span>
        <a id="tgAdd" class="UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-plus icon-activate">
        </span><span>
            <%= Html.Term("AddaNewZone", "Add a New Zone")%></span> </a><a href="javascript:void(0);"
                class="UI-icon-container" id="exportToExcel"><span class="UI-icon icon-exportToExcel">
                </span><span>
                    <%=Html.Term("ExportExcel", "Export Excel")%></span></a>
    </div>
    
    <% Html.PaginatedGrid<ZonesData>("~/Logistics/Shipping/GetAreaGeographical/" + Model.ShippingOrderTypeID, "AreaGeographical")
            .AutoGenerateColumns()
            .HideClientSpecificColumns_()
            .CanDelete("")
            .ClickEntireRow()
            .Render(); 
    %>
    <div id="addNewZoneModel" class="jqmWindow LModal Overrides">
        <div class="mContent">           
            <h2>
                <%= Html.Term("AddaNewZone", "Add a New Zone")%>
            </h2>
            <table id="newZone" class="FormTable Section">
                <tr>
                    <td class="Flabel"><%=Html.Term("State", "State")%>:</td>
                    <td>
                        <select id="sState" class="required" name="<%= Html.JavascriptTerm("valStateProvince", "Value to States Provinces not selected.") %>">
                            <option value=""><%=Html.Term("SelectaState", "Select a State")%></option>
                            <% foreach (var items in ViewData["StatesProvinces"] as List<StateProvincesData>)
                               {
                            %>
                            <option value="<%=items.Name %>">
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
                            <option value="">
                                <%=Html.Term("SelectaCity","Select a City")%></option>
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
                <a id="AddZone" href="javascript:void(0);" class="Button BigBlue">
                    <%= Html.Term("Save", "Save")%>
                </a><a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Cancel")%>
                </a>
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>
    <span class="ClearAll"></span>
    <p>
        <a id="btnSaveZone" href="javascript:void(0);" class="Button BigBlue">
            <%= Html.Term("Save", "Save")%>
        </a>
    </p>
    <span class="ClearAll"></span>
</div>
