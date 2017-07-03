<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/Shipping.Master" 
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.ShippingRateGroupBe>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="../../../../Scripts/Validaciones.js" type="text/javascript"></script>
    <script type="text/javascript">

        function fncClearShippingRateGroup() {
            $("#txtName").val("");
            $("#txtDescription").val("");
            $("#txtGroupCode").val("");
        }

        function fncClearInputPoup() {
            $("#HF_CURRENCY_ID").val("");
            $("#txtCurrency").val("");
            $("#txtName2").val("");
            $("#txtValueFrom").val("");
            $("#txtValueTo").val("");
            $("#txtShippingAmount").val("");
        }

        function fncValidateShippingRatesGroup() {
            var name = $("#txtName").val();
            var description = $("#txtDescription").val();
            var groupCode = $("#txtGroupCode").val();
            var message = '';

            if (name == '') {
                message += '<%= Html.Term("YouMustEnterAName", "You must enter a name")%> \n';
            }
            if (description == '') {
                message += '<%= Html.Term("YouMustEnterADescription", "You must enter a description")%> \n';
            }
            if (groupCode == '') {
                message += '<%= Html.Term("YouMustEnterAGroupCode", "You must enter a group code")%> \n';
            }
            if (message == '') {
                return true;
            } else {
                alert(message);
                return false;
            }
        }

        function fncValidateShippingRate() {
            var currency = $("#txtCurrency").val();
            var name = $("#txtName2").val();
            var valueFrom = $("#txtValueFrom").val();
            var valueTo = $("#txtValueTo").val();
            var shippingAmount = $("#txtShippingAmount").val();
            var message = '';
            var response = '';
            var status = false;

            if (currency == '') {
                message += '<%= Html.Term("YouMustEnterACurrency", "You must enter a currency")%> \n';
            }
            if (name == '') {
                message += '<%= Html.Term("YouMustEnterAName", "You must enter a name")%> \n';
            }
            if (valueFrom == '') {
                message += '<%= Html.Term("YouMustEnterAValueFrom", "You must enter a value from")%> \n';
            }
            if (valueTo == '') {
                message += '<%= Html.Term("YouMustEnterAValueTo", "You must enter a value to")%> \n';
            }
            if (shippingAmount == '') {
                message += '<%= Html.Term("YouMustEnterAShippingAmount", "You must enter a shipping amount")%> \n';
            }
            if (message == '') {
                return true;           
            } else {
                alert(message);
                return false;
            }
        }

        function fncCheckAll() {
            var status = $('#chckAll').is(':checked');

            $("#paginatedGrid tbody tr").each(function () {
                var check = $(this).find("td:eq(0) input:checkbox");
                if (check != null || check != NaN) {
                    $(check).attr("checked", status);
                }
            });
        }

        function fncValidateCheck() {
            var totalRow = $("#paginatedGrid tbody tr").length;
            var count = 0;

            $("#paginatedGrid tbody tr").each(function () {
                var check = $(this).find("td:eq(0) input:checkbox");
                if (check != null || check != NaN) {
                    if (check.is(":checked")) {
                        count += 1;
                    }
                }
            });

            if (count == totalRow) { $("#chckAll").attr("checked", true); }
            else { $("#chckAll").attr("checked", false); }
        }

        $(function () {

            $('#txtCurrency').removeClass('Filter').after($('#HF_CURRENCY_ID')).css('width', '250px')
                .watermark('<%= Html.JavascriptTerm("CurrencySearch", "Look up currency by name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Shipping/CurrencyLookUp") %>', { onSelect: function (item) {
				    $('#HF_CURRENCY_ID').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});


            $('#divProcessing').jqm({ modal: true, onShow: function (h) {
                h.w.css({
                    //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });

            $('#divPopupRate').jqm({ modal: true, onShow: function (h) {
                h.w.css({
                    //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });

            var clean = '<%= Model.ShippingRateGroupID %>';
            if (clean == 0) {
                fncClearShippingRateGroup();
            }

//            $("#txtValueFrom").fn_util_validaDecimal(2);
//            $("#txtValueTo").fn_util_validaDecimal(2);
//            $("#txtShippingAmount").fn_util_validaDecimal(2);

            $('#divProcessing').jqmShow();
            $("#chckAll").attr("checked", false);

            var gridShippingRate = [];
            var newShippingRates = [];
            var shippingRateToDelete = [];
            var response1 = '';

            promise1 = $.ajax({
                type: 'GET',
                url: '<%= ResolveUrl("~/Logistics/Shipping/ListShippingRate/")%>',
                data: { shippingRateGroupID: '<%= Model.ShippingRateGroupID %>' }
            });
            promise1.done(function () {
                response1 = JSON.parse(promise1.responseText);
            });
            promise1.fail(function (XMLHttpRequest, textStatus, errorThrown) {
                alert("An error has occurred to load the shipping rates");
                $("#btnSave").attr("disabled", true);
                $("#aCloseDivProcessing").trigger("click");
            });

            $.when(promise1).done(function (promise1) {
                var count = 0;
                var strClass = '';

                $.each(response1, function (i, item) {
                    count += 1;
                    strClass = (count % 2 == 0) ? 'class=""' : 'class="Alt"';
                    $("#paginatedGrid tbody").append('<tr ' + strClass + '>'
                    + '<td><input type="checkbox" id="chck" onclick="fncValidateCheck();" /><input type="hidden" id="HF_NEW" value="' + item.ShippingRateID + '" /></td>'
                    + '<td>' + item.Currency + '</td>'
                    + '<td>' + item.ValueName + '</td>'
                    + '<td>' + item.ValueFrom + '</td>'
                    + '<td>' + item.ValueTo + '</td>'
                    + '<td>' + item.ShippingAmount + '</td>'
                    + '</tr>');

                    var index = gridShippingRate.length;
                    gridShippingRate[index] = item;
                });

                $("#aCloseDivProcessing").trigger("click");
            });

            $("#tgAdd").click(function () {
                fncClearInputPoup();
                $('#divPopupRate').jqmShow();
            });

            $("#btnAddRate").click(function () {
                if (fncValidateShippingRate() == true) {
                    var paso = false;
                    var currency = $("#HF_CURRENCY_ID").val();

                    var promise = $.ajax({
                        type: 'GET',
                        url: '<%= ResolveUrl("~/Logistics/Shipping/ValidateCurrency/")%>',
                        data: { currencyName: currency }
                    });
                    promise.done(function () {
                        response = JSON.parse(promise.responseText);
                    });
                    promise.fail(function (XMLHttpRequest, textStatus, errorThrown) {
                        alert('<%= Html.Term("AnErrorHasOccurredToValidateCurrency","An error has occurred to validate currency")%>');
                        $("#btnSave").attr("disabled", true);
                        $("#a2").trigger("click");
                        status = false;
                    });

                    $.when(promise).done(function (promise) {
                        if (response != "0") {
                            paso = true;
                        } else {
                            alert('<%= Html.Term("TheCurrencyNameDoesNotExist", "The currency name does not exist")%>');
                            paso = false;
                        }
                        if (paso == true) {
                            var count = $("#paginatedGrid tbody tr").length;
                            var obj = { ShippingRateID: 0, Currency: $("#txtCurrency").val(), ValueName: $("#txtName2").val(), ValueFrom: $("#txtValueFrom").val(), ValueTo: $("#txtValueTo").val(), ShippingAmount: $("#txtShippingAmount").val() };
                            var strClass = '';

                            strClass = (count % 2 == 0) ? 'class=""' : 'class="Alt"';

                            $("#paginatedGrid tbody").append('<tr ' + strClass + '>'
                            + '<td><input type="checkbox" id="chck" onclick="fncValidateCheck();" /><input type="hidden" id="HF_NEW" value="0" /></td>'
                            + '<td>' + obj.Currency + '</td>'
                            + '<td>' + obj.ValueName + '</td>'
                            + '<td>' + obj.ValueFrom + '</td>'
                            + '<td>' + obj.ValueTo + '</td>'
                            + '<td>' + obj.ShippingAmount + '</td>'
                            + '</tr>');

                            var index = newShippingRates.length;
                            newShippingRates[index] = obj;
                            fncClearInputPoup();
                            $("#a2").trigger("click");
                        }
                    });
                }
            });

            $("#tgDeleteSelected").click(function () {
                var totalRow = $("#paginatedGrid tbody tr").length;
                if (totalRow <= 0)
                    return;
                else {
                    $("#paginatedGrid tbody tr").each(function () {
                        var check = $(this).find("td:eq(0) input:checkbox");
                        var hidden = $(this).find("td:eq(0) input:hidden");
                        var row = $(this);
                        if (check.is(":checked")) {
                            if (hidden != null || hidden != NaN) {
                                if (hidden.val() != "0") {

                                    var index = shippingRateToDelete.length;
                                    shippingRateToDelete[index] = hidden.val();
                                    row.remove();

                                } else {
                                    var obj = Object();
                                    obj.ShippingRateID = 0;
                                    obj.Currency = $(this).find("td:eq(1)").html();
                                    obj.ValueName = $(this).find("td:eq(2)").html();
                                    obj.ValueFrom = $(this).find("td:eq(3)").html();
                                    obj.ValueTo = $(this).find("td:eq(4)").html();
                                    obj.ShippingAmount = $(this).find("td:eq(5)").html();

                                    var mirror1 = newShippingRates;
                                    newShippingRates = [];
                                    $.each(mirror1, function (i, item) {
                                        if (item.Currency != obj.Currency && item.ValueName != obj.ValueName && item.ValueFrom != obj.ValueFrom && item.ValueTo != obj.ValueTo && item.ShippingAmount != obj.ShippingAmount) {
                                            var index = newShippingRates.length;
                                            newShippingRates[index] = item;
                                        }
                                    });

                                    row.remove();
                                }
                            }
                            if ($("#paginatedGrid tbody tr").length <= 0) { $("#chckAll").attr("checked", false); }
                        }
                    });
                }
            });

            $("#btnSave").click(function () {
                if (fncValidateShippingRatesGroup() == true) {
                    $('#divProcessing').jqmShow();
                    var newShipping = { ShippingRateGroupID: '<%= Model.ShippingRateGroupID %>', Name: $("#txtName").val(), Description: $("#txtDescription").val(), GroupCode: $("#txtGroupCode").val(), Active: true, RowTotal: '' };

                    var data = { newShipping: JSON.stringify(newShipping), newShippingRates: JSON.stringify(newShippingRates), shippingRateToDelete: JSON.stringify(shippingRateToDelete) };

                    $.post('<%= ResolveUrl("~/Logistics/Shipping/SaveRateDetail/")%>', data, function (response) {
                        if (response.result) {
                            if ('<%= Model.ShippingRateGroupID %>' != 0) {
                                var msg = '<%= Html.Term("TheShippingRateWasUpdated", "The shipping rate was updated")%>';
                            } else {
                                var msg = '<%= Html.Term("TheShippingRateWasRegistered", "The shipping rate was registered")%>';
                            }

                            showMessage(msg, false);
                            newShippingRates = [];
                            shippingRateToDelete = []
                            if ('<%= Model.ShippingRateGroupID %>' == 0) {
                                fncClearShippingRateGroup();
                                fncClearInputPoup();

                                $("#paginatedGrid tbody tr").each(function () {
                                    var row = $(this);
                                    row.remove();
                                });
                            }
                            //window.location.replace('<%= ResolveUrl("~/Logistics/Shipping/RateDetail/") %>' + response.shippingRuleId);
                        }
                        else {
                            showMessage('<%= Html.Term("Configurationhasbeenreprocessed", "The configuration has been reprocessed")%>', true);
                        }
                    }).always(function () {
                        $("#aCloseDivProcessing").trigger("click");
                    });
                }
            });

            $("#btnAddNewRate").click(function () {
                $('#NewRate').jqmShow();
            });
            $('#NewRate').jqm({ modal: true, onShow: function (h) {
                h.w.css({
                    //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="HF_CURRENCY_ID" value="" />
<div class="SectionHeader">
    <h2>
        <%= Html.Term("RateDetail", "RateDetail")%>
    </h2>
        <%= Html.Term("RateDetail", "Rate Detail")%>
        <%  if (Model.ShippingRateGroupID != 0)
	        {
		 %>
         &nbsp;|&nbsp;        
        <a id="btnAddNewRate" href="javascript:void(0);"><%= Html.Term("AddNewRate", "Add New Rate")%></a>
         <% 
	     }  
         %>
        
</div>
    <table id="ruleDetailForm" class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <span class="requiredMarker">*</span>
                <%= Html.Term("Name", "Name")%>:
            </td>
            <td>
                <input id="txtName" type="text" value="<%= Model.Name %>" maxlength="200" class="required"
                    name="<%= Html.Term("NameIsRequired", "Name is required") %>" style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
            <span class="requiredMarker">*</span>
                <%: Html.Term("Description", "Description")%>:
            </td>
            <td>
                <input id="txtDescription" type="text" value="<%= Model.Description %>" maxlength="200" class="required"
                    name="<%= Html.Term("DescriptionIsRequired", "Description is required") %>" style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
            <span class="requiredMarker">*</span>
                <%: Html.Term("GroupCode", "Group Code")%>:
            </td>
            <td>
                <input id="txtGroupCode" type="text" value="<%= Model.GroupCode %>" maxlength="200" class="required"
                    name="<%= Html.Term("GroupCodeIsRequired", "GroupCode is required") %>" style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue">
                        <%= Html.Term("SaveRute", "Save Rute")%></a>
                </p>
            </td>
        </tr>
    </table>
    <br />
    <div id="paginatedGridOptions" class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility">
        <a id="tgDeleteSelected" class="deactivateButton UI-icon-container"><span class="UI-icon icon-x icon-deactive">
        </span><span><%= Html.Term("RemoveSelected", "Remove Selected")%></span> </a><span class="pipe">&nbsp;</span> <a id="tgAdd" class="activateButton UI-icon-container"
            href="javascript:void(0);"><span class="UI-icon icon-plus icon-activate"></span>
            <span><%= Html.Term("AddNewRate", "Add New Rate")%></span> </a>
    </div>
    <div class="responsiveDataGrid">
        <table id="paginatedGrid" class="DataGrid" width="100%">
            <thead>
                <tr class="GridColHead UI-bg UI-header">
                    <th id="Th1" class="noHover">
                    <input type="checkbox" id="chckAll" onclick="fncCheckAll();" />
                    </th>
                    <th id="Currency" class="noHover">
                        <%= Html.Term("Currency", "Currency")%>
                    </th>
                    <th id="ValueName" class="noHover">
                        <%= Html.Term("ValueName", "Value Name")%>
                    </th>
                    <th id="ValueFrom" class="noHover">
                        <%= Html.Term("ValueFrom", "Value From")%>
                    </th>
                    <th id="ValueTo" class="noHover">
                        <%= Html.Term("ValueTo", "Value To")%>
                    </th>
                    <th id="ShippingAmount" class="noHover">
                        <%= Html.Term("ShippingAmount", "Shipping Amount")%>
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div class="UI-mainBg Pagination" id="AccountPaginatedGridPagination">            
        </div>
    </div>
    <div id="divProcessing" class="jqmWindow LModal Overrides" style="width: 523px; height: 273px; border:0;">
        <div style="margin:0 auto 0 auto; text-align:center;">
            <img src="/Content/Images/Icons/loading-blue.gif" />
        </div>
        <div style="display: none">
            <a id="aCloseDivProcessing" href="javascript:void(0);" class="Button jqmClose">
                <%= Html.Term("Cancel")%>
            </a>
        </div>
    </div>

    <% Html.RenderPartial("AddNewRateModal"); %>

    <div id="divPopupRate" class="jqmWindow LModal Overrides" style="width: 523px; height: 253px;
        border: 0;">
        <div class="mContent" style="width: 520px; height: 245px;">
            <table id="Table1" class="FormTable" width="100%">
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <span class="requiredMarker">*</span>
                        <%: Html.Term("Currency", "Currency")%>:
                    </td>
                    <td>
                        <input id="txtCurrency" type="text" maxlength="200" class="required" style="width: 20.833em;" />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <span class="requiredMarker">*</span>
                        <%: Html.Term("Name", "Name")%>:
                    </td>
                    <td>
                        <input id="txtName2" type="text" maxlength="200" class="required" style="width: 20.833em;" />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <span class="requiredMarker">*</span>
                        <%: Html.Term("ValueFrom", "Value From")%>:
                    </td>
                    <td>
                        <input id="txtValueFrom" type="text" maxlength="200" monedaidioma='CultureIPN' class="required" style="width: 20.833em;"  />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <span class="requiredMarker">*</span>
                        <%: Html.Term("ValueTo", "Value To")%>:
                    </td>
                    <td>
                        <input id="txtValueTo" type="text" maxlength="200" monedaidioma='CultureIPN' class="required" style="width: 20.833em;" />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <span class="requiredMarker">*</span>
                        <%: Html.Term("ShippingAmount", "Shipping Amount")%>:
                    </td>
                    <td>
                        <input id="txtShippingAmount" type="text" maxlength="200" class="required" style="width: 20.833em;" />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                    </td>
                    <td>
                        <p>
                            <a href="javascript:void(0);" id="btnAddRate" style="display: inline-block;" class="Button BigBlue">
                                <%= Html.Term("Add", "Add")%></a> <a id="a2" href="javascript:void(0);" class="Button jqmClose">
                                    <%= Html.Term("Cancel")%>
                                </a>
                        </p>
                    </td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>