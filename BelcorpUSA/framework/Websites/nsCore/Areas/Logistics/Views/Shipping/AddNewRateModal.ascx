<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Business.ShippingRateGroupBe>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="NetSteps.Common.Globalization" %>

<script type="text/javascript">
    $(function () {

        $('#divProcessing').jqm({ modal: true, onShow: function (h) {
            h.w.css({
                //top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                //left: Math.floor(parseInt($(window).width() / 2)) + 'px'
            }).fadeIn();
        }
        });

        $('#btnBrowse').click(function () {
            $('#inputLoadMatrix').trigger('click');
        });


        document.getElementById('formLoad').onsubmit = function () {
            $('#divProcessing').jqmShow();
            $("#paginatedGridNewRate td").remove();
            var formdata = new FormData();
            var fileInput = document.getElementById('inputLoadMatrix');
            for (i = 0; i < fileInput.files.length; i++) {
                formdata.append(fileInput.files[i].name, fileInput.files[i]);
            }

            var xhr = new XMLHttpRequest();
            xhr.open('POST', '<%= ResolveUrl("~/Logistics/Shipping/LoadExcelFile")%>');
            xhr.send(formdata);
            $('#btnBrowse').showLoading();
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    var response = JSON.parse(xhr.responseText);
                    if (response.result) {
                        for (var i = 0; i < response.data.length; i++) {
                            $('#paginatedGridNewRate tbody').append('<tr>'
                            	+ '<td>' + response.data[i].ValueName + '</td>'
                            	+ '<td>' + response.data[i].ValueFrom + '</td>'
                            	+ '<td>' + response.data[i].ValueTo + '</td>'
                            	+ '<td>' + response.data[i].ShippingAmount + '</td>'
                            	+ '<td>' + response.data[i].CurrencyCode + '</td>'
                            	+ '</tr>');
                        }
                        setAutoAddGridVisibility();
                    } else {
                        showMessage(response.message, true);
                    }
                }
                $("#aCloseDivProcessing").trigger("click");
                $('#btnBrowse').hideLoading();
            }
            return false;
        }

        function setAutoAddGridVisibility() {
            if ($('#paginatedGridNewRate tbody tr').length) {
                $('#paginatedGridNewRate').show();
            }
            else {
                $('#paginatedGridNewRate').hide();
            }
        }

        $('#inputLoadMatrix').change(function (e) {
            $('#submitHidden').trigger('click');
        });


        $("#btnSaveNewRate").click(function () {
            if ($('#paginatedGridNewRate tbody tr').length > 0) {
                var arrShippingRates = [];
                $("#paginatedGridNewRate tbody tr").each(function () {
                    var obj = Object();
                    obj.ValueName = $(this).find("td:eq(0)").html();
                    obj.ValueFrom = $(this).find("td:eq(1)").html();
                    obj.ValueTo = $(this).find("td:eq(2)").html();
                    obj.ShippingAmount = $(this).find("td:eq(3)").html();
                    obj.CurrencyCode = $(this).find("td:eq(4)").html();

                    var index = arrShippingRates.length;
                    arrShippingRates[index] = obj;
                });

                var data = { arrShippingRates: JSON.stringify(arrShippingRates), ShippingRateGroupID: $("#hf_ShippingRateGroupID").val() };

                $.post('<%= ResolveUrl("~/Logistics/Shipping/SaveMassiveRate/")%>', data, function (response) {
                    if (response.result) {
                        var msg = '<%= Html.Term("RateWasAdded", "Rate was added")%>';                        
                        showMessage(msg, false);
                        $("#paginatedGridNewRate tbody tr").remove();
                        $('#NewRate').jqmHide();
                    }
                    else {
                        showMessage(response.message, true);
                    }
                }).always(function () {
                    $("#aCloseDivProcessing").trigger("click");
                });
            }
            else {
                //showMessage('<%= Html.Term("ThereIsNotDataToSave", "There is not data to save")%>', true);
                alert('<%= Html.Term("ThereIsNotDataToSave", "There is not data to save")%>');
            }
        });

    });
    </script>
 
<div id="NewRate" class="jqmWindow LModal Overrides" style="width: 523px; height: 253px;
    border: 0;">
    <input type="hidden" id="hf_ShippingRateGroupID" value="<%= Model.ShippingRateGroupID %>"/> 
    <div class="mContent">
        <h2>
            <%= Html.Term("AddaNewRate", "Add New Rate")%>
        </h2>
        <table>
            <tbody>
                <tr>
                    <td style="width: 250px;">
                        <form id="formLoad" action="" enctype="multipart/form-data">
                        <a class="Button BigBlue" id="btnBrowse" href="javascript:void(0);">
                            <%= Html.Term("UploadShippingRates", "Upload Shipping Rates")%>
                        </a>
                        <input type="file" id="inputLoadMatrix" name="inputLoadAccounts" style="display: none" />
                        <input type="submit" id="submitHidden" style="display: none" />
                        </form>
                    </td>
                    <td>
                        <% using (Html.BeginForm("DownloadTemplate", "Shipping", FormMethod.Post, new { enctype = "multipart/form-data" }))
                           { %>
                        <input type="submit" id="btnDownload" class="Button BigBlue" style="margin-top: -7px;"
                            value="<%= Html.Term("DownloadTemplate", "Download template")%>" />
                        <% } %>
                    </td>
                </tr>
            </tbody>
        </table>
        <br />
    <div class="responsiveDataGrid">
        <table id="paginatedGridNewRate" class="DataGrid" width="100%">
            <thead>
                <tr class="GridColHead UI-bg UI-header">                   
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
                    <th id="Currency" class="noHover">
                        <%= Html.Term("CurrencyCode", "Currency Code")%>
                    </th>
                </tr>
            </thead>
            <tbody>
            </tbody>
        </table>
        <div class="UI-mainBg Pagination" id="AccountPaginatedGridPagination">            
        </div>
    </div>
    <br />
    <a href="javascript:void(0);" id="btnSaveNewRate" style="display:inline-block;" class="Button BigBlue"><%= Html.Term("Save","Save")%></a>
    <a id="a1" href="javascript:void(0);" class="Button jqmClose">
                <%= Html.Term("Cancel")%>
            </a>
    <div id="divProcessingnewrate" class="jqmWindow LModal Overrides" style="width: 523px; height: 273px; border:0;">
        <div style="margin:0 auto 0 auto; text-align:center;">
            <img src="/Content/Images/Icons/loading-blue.gif" />
        </div>
        <div style="display: none">
            <a id="aCloseDivProcessing" href="javascript:void(0);" class="Button jqmClose">
                <%= Html.Term("Cancel")%>
            </a>
        </div>
    </div>
</div>
</div>
