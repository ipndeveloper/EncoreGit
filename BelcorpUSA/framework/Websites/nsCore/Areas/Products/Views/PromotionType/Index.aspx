<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Promotions/Promotions.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server"> 
    

    <div class="SectionHeader">
        <h2>
            <%= Html.Term("PromotionTypes", "Promotion Types")%></h2>
        <p>
            <table>
                <tbody>
                    <tr>
                       <%-- <td style="width: 80px;">
                            <%= Html.Term("Promotion", "Promotions")%>
                        </td>
                        <td style="width: 150px;">
                            <select id="ddlPromotionTypes" style="width: 150px;"></select>
                        </td>
                        <td style="width: 30px;">
                        </td>--%>
                        <td style="width: 70px;">
                            <%= Html.Term("IncludesBA", "Includes BA")%>
                        </td>
                        <td style="width: 80px;">
                            <input type="radio" id="rbNo" name="group1" checked="checked" value="<%= Html.Term("No", "No")%>" /><%= Html.Term("No", "No")%>
                            <input type="radio" id="rbYes" name="group1" value="<%= Html.Term("Yes", "Yes")%>" /><%= Html.Term("Yes", "Yes")%>
                        </td>
                    </tr>
                </tbody>
            </table>
        </p>
    </div>
    <br />
    <div class="SearchBox">
        <input id="txtPromotionSearch" type="text" class="TextInput" style="width: 200px; height: 25px;" />
        <a id="btnSearchPromotions" href="javascript:void(0);" class="SearchIcon"></a>
        <a href="#" id="tagAdd"><img src="../../../../Content/Images/Icons/add-trans.png" /></a>
    </div>
    <br />
    <div id="paginatedGridOptions" class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility">
    <p>
            <a class="deleteButton UI-icon-container" id="aDeleteSelected" href="javascript:void(0);">
                <span class="UI-icon icon-deleteSelected"></span>
                <span>Delete Selected</span>
            </a>
    </p>
    </div>
    <div class="responsiveDataGrid">
        <table id="paginatedGrid" class="DataGrid" width="100%">
            <thead>
                <tr class="GridColHead UI-bg UI-header">
                    <th id="Th2" class="noHover">
                        <input type="checkbox" id="chckAll" onclick="fncCheckAll();" />
                    </th>
                    <th id="ProcessName" class="noHover">
                        <%= Html.Term("PromotionID", "Promotion ID")%>
                    </th>
                    <th id="SubProcessName" class="noHover">
                        <%= Html.Term("Description", "Description")%>
                    </th>
                    <th id="Status" class="noHover">
                        <%= Html.Term("StarDate", "Star Date")%>
                    </th>
                    <th id="Option" class="noHover">
                        <%= Html.Term("EndDate", "End Date")%>
                    </th>
                    <th id="Th1" class="noHover">
                        <%= Html.Term("Status", "Status")%>
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div class="UI-mainBg Pagination" id="AccountPaginatedGridPagination">
        </div>
    </div>
    
    <div style="display: none;">
        <input type="hidden" id="objPromotion" value="" />
        <input type="hidden" id="HF_PromotionTypeID" value="<%= TempData["PromotionTypeID"].ToString() %>" />
        <input type="hidden" id="HF_BA" value="<%= TempData["BA"].ToString() %>" />
        <input type="hidden" id="SuccessMessage" value="<%= Html.Term("Configurationhasbeenreprocessed", "The configuration has been reprocessed")%>" />
        <input type="hidden" id="HF_ERROR_REPROCESS" value="<%= Html.Term("ErrorConfiguration", "The configuration could not be reprocessed")%>" />
        <a id="a1" href="javascript:void(0);" class="Button jqmClose">
                <%= Html.Term("Cancel")%>
            </a>
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
    <p>
        <a href="javascript:void(0);" id="btnSave" class="Button BigBlue">
            Save</a>
    </p>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">

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
        $('#divProcessing').jqm({ modal: true, onShow: function (h) {
            h.w.css({
                //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
            }).fadeIn();
        }
        });

        $('#divProcessing').jqmShow();

        $('#txtPromotionSearch').val("");
        $("#chckAll").attr("checked", false);
        var arrPromoPerPromo = [];
        var arrNewPromoPerPromo = [];
        var arrPromoPerPromoToDelete = [];
        var ddlInitial = "";
        var includeBA = null;
        var response1 = '';
        var response2 = '';

        var promise1 = $.ajax({
            type: 'POST',
            url: '<%= ResolveUrl("~/PromotionType/ListPromotionTypes/")%>',
            data: null
        });
        promise1.done(function () {
            response1 = JSON.parse(promise1.responseText);
        });
        promise1.fail(function (XMLHttpRequest, textStatus, errorThrown) {
            alert("An error has occurred to load the promotion types");
            $("#btnSave").attr("disabled", true);
        });

        promise2 = $.ajax({
            type: 'GET',
            url: '<%= ResolveUrl("~/PromotionType/ListPromotionsByPromotionTypeConfigurationPerPromotions/")%>',
            data: null
        });
        promise2.done(function () {
            response2 = JSON.parse(promise2.responseText);
            //arrPromoPerPromo = response2;
        });
        promise2.fail(function (XMLHttpRequest, textStatus, errorThrown) {
            alert("An error has occurred to load the promotion");
            $("#btnSave").attr("disabled", true);
        });

        $.when(promise1, promise2).done(function (promise1, promise2) {
            var count = 0;
            var strClass = '';
//            var ddlPrmotionTypes = document.getElementById("ddlPromotionTypes");

//            ddlPrmotionTypes.options[0] = new Option('<%= Html.Term("SelectPrmotionType", "Select Promotion Type")%>', '0')

//            $.each(response1, function (i, item) {
//                i += 1;
//                ddlPrmotionTypes.options[i] = new Option(item.TermName, item.PromotionTypeID);
//            });

            ///posicionar el ddl
//            $('#ddlPromotionTypes > option[value="' + $("#HF_PromotionTypeID").val() + '"]').attr('selected', 'selected');
//            ddlInitial = $("#ddlPromotionTypes").val();

            //obtener el ba
            if ($("#HF_BA").val() == "1") {
                $("#rbNo").attr("checked", false);
                $("#rbYes").attr("checked", true);
            } else {
                $("#rbNo").attr("checked", true);
                $("#rbYes").attr("checked", false);
            }
            includeBA = ($("#rbNo").is(":checked") ? false : true);

            $.each(response2, function (i, item) {
                count += 1;
                strClass = (count % 2 == 0) ? 'class=""' : 'class="Alt"';
                $("#paginatedGrid tbody").append('<tr ' + strClass + '>'
                + '<td><input type="checkbox" id="chck" onclick="fncValidateCheck();" /><input type="hidden" id="HF_NEW" value="' + item.PromotionTypeConfigurationPerPromotionID + '" /></td>'
                + '<td>' + item.PromotionID + '</td>'
                + '<td>' + item.Description + '</td>'
                + '<td>' + item.starDate + '</td>'
                + '<td>' + item.endDate + '</td>'
                + '<td>' + item.status + '</td>'
                + '</tr>');

                var index = arrPromoPerPromo.length;
                arrPromoPerPromo[index] = item;
            });
            $("#aCloseDivProcessing").trigger("click");
        });

        $("#tagAdd").click(function () {
            if ($("#objPromotion").val() != '' && $('#txtPromotionSearch').val() != '') {
                var count = $("#paginatedGrid tbody tr").length;
                var obj = JSON.parse($("#objPromotion").val());
                var index = arrNewPromoPerPromo.length;
                var strClass = '';
                var pass = true;

                strClass = (count % 2 == 0) ? 'class=""' : 'class="Alt"';

                $("#paginatedGrid tbody tr").each(function () {
                    var PromotionID = $(this).find("td").eq(1).html();
                    if (PromotionID == obj.PromotionID) {
                        pass = false;
                        return;
                    }
                });

                if (pass) {
                    $("#paginatedGrid tbody").append('<tr ' + strClass + '>'
                    + '<td><input type="checkbox" id="chck" onclick="fncValidateCheck();" /><input type="hidden" id="HF_NEW" value="0" /></td>'
                    + '<td>' + obj.PromotionID + '</td>'
                    + '<td>' + obj.Description + '</td>'
                    + '<td>' + obj.starDate + '</td>'
                    + '<td>' + obj.endDate + '</td>'
                    + '<td>' + obj.status + '</td>'
                    + '</tr>');

                    obj.PromotionTypeConfigurationPerPromotionID = 0;
                    arrNewPromoPerPromo[index] = obj;
                } else
                    alert('<%= Html.Term("Thispromotionisalreadyinthelist", "This promotion is already in the list")%>');


                $("#objPromotion").val("");
                $('#txtPromotionSearch').val("");
            }
        });

        $("#aDeleteSelected").click(function () {
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

                                var obj = Object();
                                obj.PromotionTypeConfigurationPerPromotionID = parseInt(hidden.val());
                                obj.PromotionID = $(this).find("td:eq(1)").html();
                                obj.Description = $(this).find("td:eq(2)").html();
                                obj.starDate = $(this).find("td:eq(3)").html();
                                obj.endDate = $(this).find("td:eq(4)").html();
                                obj.status = $(this).find("td:eq(5)").html();

                                var index = arrPromoPerPromoToDelete.length;
                                arrPromoPerPromoToDelete[index] = obj;

                                var mirror1 = arrPromoPerPromo;
                                arrPromoPerPromo = [];
                                $.each(mirror1, function (i, item) {
                                    if (item.PromotionTypeConfigurationPerPromotionID != obj.PromotionTypeConfigurationPerPromotionID) {
                                        var index = arrPromoPerPromo.length;
                                        arrPromoPerPromo[index] = item;
                                    }
                                });

                                row.remove();

                            } else {
                                var promotionID = $(this).find("td:eq(1)").html();
                                var mirror2 = arrNewPromoPerPromo;
                                arrNewPromoPerPromo = [];
                                $.each(mirror2, function (i, item) {
                                    if (item.PromotionID != promotionID) {
                                        var index = arrNewPromoPerPromo.length;
                                        arrNewPromoPerPromo[index] = item;
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
//            if ($("#ddlPromotionTypes").val() == "0") {
//                alert('<%= Html.Term("YouMustSelectAPromotionType", "You must select a promotion type")%>');
//                return false;
//            }
            //            else 
            //if ((arrNewPromoPerPromo.length + arrPromoPerPromoToDelete.length) > 0 || ddlInitial != $("#ddlPromotionTypes").val() || includeBA != ($("#rbNo").is(":checked") ? false : true)) {
            {
                
                if ((arrNewPromoPerPromo.length + arrPromoPerPromoToDelete.length) > 0 || includeBA != ($("#rbNo").is(":checked") ? false : true)) {
                    $('#divProcessing').jqmShow();

                    var newConfiguration = false;
                    var NewPromotions = [];
                    var PromotionsToDelete = [];

                    //if (ddlInitial != $("#ddlPromotionTypes").val() || includeBA != ($("#rbNo").is(":checked") ? false : true)) {
                    if (includeBA != ($("#rbNo").is(":checked") ? false : true)) {
                        newConfiguration = true;

                        $.each(arrPromoPerPromo, function (i, item) {
                            var index = NewPromotions.length;
                            NewPromotions[index] = item.PromotionID;
                        });

                        $.each(arrNewPromoPerPromo, function (i, item) {
                            var index = NewPromotions.length;
                            NewPromotions[index] = item.PromotionID;
                        });
                    }
                    else {

                        $.each(arrNewPromoPerPromo, function (i, item) {
                            var index = NewPromotions.length;
                            NewPromotions[index] = item.PromotionID;
                        });

                        $.each(arrPromoPerPromo, function (i, val) {
                            var index = NewPromotions.length;
                            NewPromotions[index] = val.PromotionID;
                        });

                        //$.each(arrPromoPerPromoToDelete, function (i, val) {
                        $.each(arrPromoPerPromo, function (i, val) {
                            var index = PromotionsToDelete.length;
                            PromotionsToDelete[index] = parseInt(val.PromotionTypeConfigurationPerPromotionID);
                        });

                        $.each(arrPromoPerPromoToDelete, function (i, val) {
                            var index = PromotionsToDelete.length;
                            PromotionsToDelete[index] = parseInt(val.PromotionTypeConfigurationPerPromotionID);
                        });
                    }

                    //var PromotionTypeID = $("#ddlPromotionTypes").val();
                    var PromotionTypeID = "0";
                    var BaOrder = ($("#rbNo").is(":checked") ? false : true);
                    var data = { PromoType: PromotionTypeID, NewPromotions: JSON.stringify(NewPromotions), PromotionsToDelete: JSON.stringify(PromotionsToDelete), Ba: BaOrder, newConfiguration: newConfiguration };

                    $.post('<%= ResolveUrl("~/PromotionType/SavePromotionConfiguration/")%>', data, function (response) {
                        if (response.result) {
                            showMessage($("#SuccessMessage").val(), false);
                            //ddlInitial = $("#ddlPromotionTypes").val();
                            includeBA = ($("#rbNo").is(":checked") ? false : true);
                            arrNewPromoPerPromo = [];
                            arrPromoPerPromoToDelete = []
                        }
                        else {
                            showMessage($("#HF_ERROR_REPROCESS").val(), true);
                        }
                    }).always(function () {
                        $("#aCloseDivProcessing").trigger("click");
                    });
                }
                else
                    alert('<%= Html.Term("ThereAreNotChanges", "There Are Not Changes")%>');
            }
        });

        $('#txtPromotionSearch').keyup(function (e) {
            if (e.keyCode == 13) $('#btnSearchPromotions').click();
        }).watermark('<%= Html.JavascriptTerm("QuickPromotionLookup(Name)", "Quick Promotion Lookup (Name)") %>').jsonSuggest('<%= ResolveUrl("~/Products/PromotionType/ListPromotions") %>', { onSelect: function (item) {
            $("#objPromotion").val(JSON.stringify(item));
        }, minCharacters: 2, ajaxResults: true, maxResults: 10, showMore: true, width: $('#txtPromotionSearch').outerWidth(true) + $('#btnSearchPromotions').outerWidth() - 4
        });
    });
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
