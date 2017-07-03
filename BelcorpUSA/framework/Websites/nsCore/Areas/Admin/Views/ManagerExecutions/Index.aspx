<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="paginatedGridFilters" class="UI-lightBg brdrAll GridFilters">
        <table style="">
            <tbody>
                <tr>
                    <td style="width: 350px;">
                        <div class="FL">
                            <label>
                                Status:</label>
                            <select id="statusSelectFilter" class="Filter AutoPost">
                                <option value="0" selected="selected"><%=Html.Term("SelectStatus", "Select a Status")%></option>
                                
                            </select>
                        </div>
                    </td>
                    <td>
                        <a id="aFilter" class="ModSearch Button" href="javascript:void(0);">
                        <%: Html.Term("ApplyFilter", "Apply Filter")%></a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="responsiveDataGrid">
        <table id="paginatedGrid" class="DataGrid" width="100%">
            <thead>
                <tr class="GridColHead UI-bg UI-header">
                    <th id="ProcessName" class="noHover">
                        <%= Html.Term("Process", "Process") %>
                    </th>
                    <th id="SubProcessName" class="noHover">
                        <%= Html.Term("SubProcess", "SubProcess") %>
                    </th>
                    <th id="Status" class="noHover">
                        <%= Html.Term("Status", "Status") %>
                    </th>
                    <th id="Option" class="noHover">
                        <%= Html.Term("Option","Option") %>
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div class="UI-mainBg Pagination" id="AccountPaginatedGridPagination">
            <input type="hidden" id="AccountpaginatedGridRefresh" />
            <div class="PaginationContainer">
                <div class="Bar">
                    <a id="aPreviousPage" class="previousPage"><span>&lt;&lt; Previous</span></a>
                        <span id="barPages" class="pages">1 of 1</span>
                    <a id="aNextPage" class="nextPage">
                        <span>Next &gt;&gt;</span>
                    </a>
                    <span class="ClearAll clr"></span>
                </div>
                <div style="" class="PageSize">
                    Results Per Page:
                    <select class="pageSize">
                        <option value="15">15</option>
                        <option value="20">20</option>
                        <option value="50">50</option>
                        <option value="100">100</option>
                    </select>
                </div>
                <span class="ClearAll clr"></span>
            </div>
        </div>
    </div>
    <div id="PopupWindows" class="jqmWindow LModal Overrides" style="width: 523px; height: 273px;">
        <div class="mContent" style="width: 500px; height: 250px;">
            <br />
            <table>
                <thead>
                    <tr>
                        <th>
                        </th>
                        <th>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td class="FLabel height">
                            <%= Html.Term("Process", "Process")%>
                        </td>
                        <td>
                            <label id="lblProcess">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FLabel height">
                            <%= Html.Term("Subprocess", "Subprocess")%>
                        </td>
                        <td>
                            <label id="lblSubprocess">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FLabel height">
                            <%= Html.Term("Message", "Message")%>
                        </td>
                        <td>
                            <label id="lblMessage">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="FLabel height">
                            <%= Html.Term("Error", "Error")%>
                        </td>
                        <td>
                            <label id="lblError">
                            </label>
                        </td>
                    </tr>
                </tbody>
            </table>
            <p>
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Cancel")%>
                </a>
            </p>
        </div>
    </div>
    <div id="divProcessing" class="jqmWindow LModal Overrides" style="width: 523px; height: 273px; border:0;">
        <div style="margin:0 auto 0 auto; text-align:center;">
            <img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif")%>" />
        </div>
        <div style="display: none">
            <a id="aCloseDivProcessing" href="javascript:void(0);" class="Button jqmClose">
                <%= Html.Term("Cancel")%>
            </a>
        </div>
    </div>
    <input type="hidden" id="SuccessMessage" value="<%= Html.Term("Processhasbeenreprocessed", "The process has been reprocessed")%>" />
    <input type="hidden" id="HF_ERROR_REPROCESS" value="<%= Html.Term("ErrorReprocess", "The process could not be reprocessed")%>" />
    <input type="hidden" id="HD_OF" value="<%= Html.Term("of", "of")%>" />
    <input type="hidden" id="HF_Reload" value="1" />    
    <input type="hidden" id="HF_PAGE" value="0"/>
    <input type="hidden" id="HF_PAGESIZE" value="15"/>
    <input type="hidden" id="HF_AMOUNTROWS" value="0"/>
    <input type="hidden" id="HF_TOTALPAGES" value="0"/>
    <input type="hidden" id="HF_DDL_STATUS" value="<%= Html.Term("SelectStatus", "Select a Status")%>"/>
    <input type="hidden" id="HF_TagTitle_Process" value="<%= Html.Term("Reprocess", "Reprocess")%>" />
    <input type="hidden" id="HF_TAG_TITLE_PROCESSING" value="<%= Html.Term("Status-Processing", "Processing")%>" />
    <input type="hidden" id="HF_TAG_TITLE_FINISHED" value="<%= Html.Term("Status-Finished", "Finished")%>" />
    <input type="hidden" id="HF_TAG_TITLE_FINISHED_ERROR" value="<%= Html.Term("Status-Finished-Error", "Finished with error")%>" />
    <input type="hidden" id="HF_TAG_TITLE_CANCELED" value="<%= Html.Term("Status-Canceled", "Canceled")%>" />
    <div style="display: none;">
        <input type="button" id="btnSearch" />
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function GetIcon(status) { 
            var icon='';
            switch (status) {
            case 1:
                icon = '<img src="<%= ResolveUrl("~/Content/Images/Gif_Processing.gif")%>" width="20" height="20" title="' + $("#HF_TAG_TITLE_PROCESSING").val() + '" >';
                break;
            case 2:
                icon = '<img src="<%= ResolveUrl("~/Content/Images/Icon_Success.png")%>" width="20" height="20" title="' + $("#HF_TAG_TITLE_FINISHED").val() + '" >';
                break;
            case 3:
                icon = '<img src="<%= ResolveUrl("~/Content/Images/Icon_Setting.png")%>" width="20" height="20" title="' + $("#HF_TagTitle_Process").val() + '" >';
                break;
            case 4:
                icon = '<img src="<%= ResolveUrl("~/Content/Images/Icon_Error.png")%>" width="20" height="20" title="' + $("#HF_TAG_TITLE_CANCELED").val() + '" >';
                break;
            }
            return icon;
        }

        function fncReprocess(tag) {
            
            $('#divProcessing').jqmShow();
            $("#HF_Reload").val('0');

            var value = $(tag).attr('id');
            var valueSplit = value.split('-');
            var data = { type: valueSplit[0], id: valueSplit[1] };
            var url = '<%= ResolveUrl("~/ManagerExecutions/ReProcess/")%>';

            $.post(url, data, function (response) {
                if (response.result) {
                    showMessage($("#SuccessMessage").val(), true);
                }
                else { showMessage($("#HF_ERROR_REPROCESS").val(), true); }
            }).always(function () {
                $("#HF_Reload").val('1');
                $("#btnSearch").trigger("click");
                $("#aCloseDivProcessing").trigger("click");
            });
        }

        function fncCalculatePagination() {
            var amoutRows = parseFloat($("#HF_AMOUNTROWS").val());
            var pageSize = parseFloat($("#HF_PAGESIZE").val());
            var page = parseInt($("#HF_PAGE").val());
            var totalPages = (amoutRows / pageSize);
            totalPages = Math.floor(totalPages) == 0 ? 1 : (Math.floor(totalPages) == Math.ceil(totalPages) ? Math.floor(totalPages) : Math.floor(totalPages) + 1);
            $("#barPages").html(page + 1 + " " + $("#HD_OF").val() + " " + totalPages.toString());
            $("#HF_TOTALPAGES").val(totalPages - 1);
            totalPages = totalPages - 1;

            if (totalPages == 0) {
                $("#aPreviousPage").addClass("disabled");
                $("#aNextPage").addClass("disabled");
                $("#aPreviousPage").prop('disabled', true);
                $("#aNextPage").prop('disabled', true);
                return;
            }
            else {
                if (page > 0 && page < totalPages) {
                    $("#aPreviousPage").removeClass("disabled");
                    $("#aNextPage").removeClass("disabled");
                    $("#aPreviousPage").prop('disabled', false);
                    $("#aNextPage").prop('disabled', false);                    
                    return;
                }
                if (page == 0 && page < totalPages) {
                    $("#aPreviousPage").addClass("disabled");
                    $("#aNextPage").removeClass("disabled");
                    $("#aPreviousPage").prop('disabled', true);
                    $("#aNextPage").prop('disabled', false);
                    return;
                }
                if (page > 0 && page == totalPages) {
                    $("#aPreviousPage").removeClass("disabled");
                    $("#aNextPage").addClass("disabled");
                    $("#aPreviousPage").prop('disabled', false);
                    $("#aNextPage").prop('disabled', true);
                    return;
                }
            }           
        }

        $(function () {

            $('#PopupWindows').jqm({ modal: true, onShow: function (h) {
                h.w.css({
                    //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });
            $('#divProcessing').jqm({ modal: true, onShow: function (h) {
                h.w.css({
                    //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                    //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });

            var url = '<%= ResolveUrl("~/ManagerExecutions/ListSubprocessStatuses/")%>';
            var promise2 = $.ajax({
                type: 'POST',
                url: url,
                data: null
            });
            promise2.done(function () {
                var response = JSON.parse(promise2.responseText);
                var ddlStatuses = document.getElementById("statusSelectFilter");

                ddlStatuses.options[0] = new Option($("#HF_DDL_STATUS").val(), "0");

                $.each(response, function (i, item) {
                    i += 1;
                    ddlStatuses.options[i] = new Option(item.Status, item.StatusProcessMonthlyClosureID);
                });
            });

            $('.ShowPopup').live('click', function () {
                
                var value = $(this).attr('id');
                var valueSplt = value.split('-');
                var data = { type: valueSplt[0], id: valueSplt[1] };
                var url = '<%= ResolveUrl("~/ManagerExecutions/GetFailedSuprocess/")%>';

                $.ajax({
                    type: 'POST',
                    url: url,
                    data: data,
                    success: function (jsonData) {
                        $('#lblProcess').html(jsonData.process);
                        $('#lblSubprocess').html(jsonData.subprocess);
                        $('#lblMessage').html(jsonData.message);
                        $('#lblError').html(jsonData.error);
                    },
                    error: function (jsonData) {
                        alert(jsonData.message);
                    }
                });
                $('#PopupWindows').jqmShow();
            });

            $("#btnSearch").on("click", function () {
                
                var inputStatus = $("#statusSelectFilter").val();
                var inputPage = $("#HF_PAGE").val();
                var inputPageSize = $("#HF_PAGESIZE").val();
                var response = new Object();
                var inputData = { status: inputStatus, page: inputPage, pageSize: inputPageSize, orderBy: 'RowChild', orderByDirection: 'asc' };
                var promise1 = $.ajax({
                    type: 'POST',
                    url: '<%= ResolveUrl("~/ManagerExecutions/GetGridMainProcessesDetail/")%>',
                    data: inputData,
                    async: true
                });
                promise1.done(function () {
                    response = JSON.parse(promise1.responseText);
                });
                //promise1.fail(function () { alert('error'); });
                //promise1.always(function () { alert('complete'); });                

                $.when(promise1).done(function () {
                    var count = 0;
                    var strClass = '';
                    var strTagStar = '';
                    var strTagEnd = '';
                    $("#paginatedGrid > tbody:last").children().remove();
                    if (response.length <= 0)
                        $("#AccountPaginatedGridPagination").css("display", "none");
                    else
                        $("#AccountPaginatedGridPagination").css("display", "block");

                    $.each(response, function (i, item) {
                        count += 1;
                        strClass = (count % 2 == 0) ? 'class=""' : 'class="Alt"';
                        strTagStar = (response[i].StatusSubProcessID == 3) ? '<a href="javascript:void(0);" id="' + response[i].MainProcessCode + '-' + response[i].SubProcessID + '" class="ShowPopup">' : '';
                        strTagEnd = (response[i].StatusSubProcessID == 3) ? '</a>' : '';
                        $("#HF_AMOUNTROWS").val(response[i].RowTotal);
                        $("#paginatedGrid tbody").append('<tr ' + strClass + '>'
                            + '<td>' + response[i].MainProcessName + '</td>'
                            + '<td>' + strTagStar + response[i].SubProcessName + strTagEnd + '</td>'
                            + '<td>' + response[i].StatusSubProcessName + '</td>'
                            + '<td>' + (response[i].Reprocess == '0' ? GetIcon(response[i].StatusSubProcessID) : '<a id="' + response[i].MainProcessCode + '-' + response[i].MainProcessID + '" onclick="fncReprocess(this);" class="cursor"><img src="<%= ResolveUrl("~/Content/Images/Icon_Setting.png")%>" width="20" height="20" title="' + $("#HF_TagTitle_Process").val() + '" ></a>') + '</td>'
                            + '</tr>');
                    });
                    fncCalculatePagination()
                });
            });

            $("#btnSearch").trigger("click");
            setInterval(function () {
                var paso = $("#HF_Reload").val();
                if (paso == '1') {
                    $("#btnSearch").trigger("click");
                }
            }, 3000); //3000

            $(".pageSize").change(function () {
                $("#HF_PAGESIZE").val($(".pageSize").val());
                $("#HF_PAGE").val(0);
                $("#btnSearch").trigger("click");
            });

            $("#aPreviousPage").click(function () {
                var currentPage = parseInt($("#HF_PAGE").val());
                var totalPages = parseInt($("#HF_TOTALPAGES").val());

                if (currentPage > 0) {
                    var newPage = currentPage - 1;

                    $("#HF_PAGE").val(newPage);
                    $("#aPreviousPage").addClass("disabled");
                    $("#aPreviousPage").prop('disabled', true);
                    $("#btnSearch").trigger("click");
                }
            });

            $("#aNextPage").click(function () {
                var currentPage = parseInt($("#HF_PAGE").val());
                var totalPages = parseInt($("#HF_TOTALPAGES").val());

                if (currentPage < totalPages) {
                    var newPage = currentPage + 1;

                    $("#HF_PAGE").val(newPage);
                    $("#aNextPage").addClass("disabled");
                    $("#aNextPage").prop('disabled', true);
                    $("#btnSearch").trigger("click");
                }
            });

            $("#aFilter").click(function () {
                $("#btnSearch").trigger("click");
            });

            $("#statusSelectFilter").change(function () {
                $("#btnSearch").trigger("click");
            });
        });
    </script>
    <style type="text/css">
        .height { height: 25px; }
        .cursor { cursor:pointer }
    </style>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
