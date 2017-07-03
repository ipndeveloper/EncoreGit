<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Business.ShippingRulesLogisticsSearchData>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="NetSteps.Common.Globalization" %>
 
 
<div id="divPostalCodeRanges" class="none">
<input type="hidden" id="postaldelete" value="0"/>
        <table  id="newPostalCode">
            <tr>
                <td>
                    <%= Html.Term("PostalCodeFrom", "Postal Code From") %>:&nbsp;
                </td>
                <td>
                    <div class="FInput">
                        <input size="5" name="<%= Html.JavascriptTerm("PostalCodeRequired", "Postal Code is required.") %>"  maxlength="5" id="mainAddressZip"
                            class="PostalCode required">&nbsp;-&nbsp;
                        <input size="3" name="" maxlength="3" id="mainAddressZipPlusFour" class="PostalCode required">
                        <img id="zipLoadingfrom" style="height: 15px; display: none;" alt="" src="/Content/Images/Icons/loading-blue.gif"
                            class="zipLoading">
                        <span title="Primary URL" id="checkfrom" style="display: none;" class="UI-icon icon-check"></span>
                        <span title="Primary URL" id="SpanErrorFrom" style="display: none;" class="UI-icon icon-exclamation"></span>                        
                    </div>
                    <br />
                </td>
            </tr>
            <tr>
                <td><%= Html.Term("PostalCodeTo", "Postal Code To") %>: &nbsp;</td>
                <td>
                    <div class="FInput">
                        <input size="5" name="<%= Html.JavascriptTerm("PostalCodeRequired", "Postal Code is required.") %>" maxlength="5" id="mainAddressZip2" class="PostalCode required">&nbsp;-&nbsp;
                        <input size="3" name="" maxlength="3" id="mainAddressZipPlusFour2" class="PostalCode required">
                        <img style="height: 15px; display: none;" id="zipLoadingTo" alt="" src="/Content/Images/Icons/loading-blue.gif" class="zipLoading">
                        <span title="Primary URL" id="checkTo" style="display: none;" class="UI-icon icon-check"></span>
                        <span title="Primary URL" id="SpanErrorTo" style="display: none;" class="UI-icon icon-exclamation"></span>                            
                    </div>
                    <br/>
                </td>
            </tr>
            <tr>
                <td class="Flabel"><%=Html.Term("Except", "Except")%>:</td>
                <td><div class="FInput">
                    <input type="checkbox" id="checkExceptPostalCode" />
                    </div>
                </td>
            </tr>
        </table>
        <p>
            <a href="javascript:void(0);" id="btnAddPostalCode" style="display: inline-block;" class="Button BigBlue">
                <%= Html.Term("Add", "Add")%></a>
        </p>
        <div id="paginatedGridOptionsPostal" class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility">
            <a id="tgDeleteSelectedPostalCode" class="UI-icon-container" href="javascript:void(0);"><span
                class="UI-icon icon-x icon-deactive"></span><span>
                    <%= Html.Term("RemoveSelected", "Remove Selected")%></span> </a><span class="pipe">&nbsp;</span>            
                <a href="javascript:void(0);" class="UI-icon-container" id="exportToExcelPostalCodes">
                    <span class="UI-icon icon-exportToExcel"></span><span>
                        <%=Html.Term("ExportExcel", "Export Excel")%></span></a>
        </div>      
        <% Html.PaginatedGrid<GeoScopesByCodesSearchData>("~/Logistics/Shipping/GetPostalCodeRanges/" + Model.ShippingOrderTypeID,"PostalCodeRanges")
            .AutoGenerateColumns()
            .HideClientSpecificColumns_()
            .CanDelete("")
            .ClickEntireRow()
            .Render(); 
        %>
        <iframe   name ="frmExportar" id="frmExportar" style="display:none" src=""></iframe>
        <span class="ClearAll"></span>
        <p>
            <a id="btnSavePostalCode" href="javascript:void(0);" class="Button BigBlue">
                <%= Html.Term("Save", "Save")%>
            </a>
        </p>
        <span class="ClearAll"></span>
    </div>