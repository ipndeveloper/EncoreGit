<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/ArrearsDefaultsReports.Master" 
                                Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/GeneralLedger") %>">
        <%= Html.Term("GMP-Nav-General-Ledger", "General Ledger")%></a> >
    <%= Html.Term("ArrearsDefaultReports", "Arrears and Defaults Reports")%> >
    <%= Html.Term("SpcReport", "SPC Report")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
    
    <div class="SectionHeader">
        <h2><%= Html.Term("SpcReport", "SPC Report")%></h2>
    </div>

    <div>
        <h3><%= Html.Term("CreateNewSPCFile", "Create a new SPC File") %></h3>
        <hr />
        <br />
        
        <div>
            <div>
                <table class="FormTable" width="40%">
                    <tr>
                        <td style="text-align: right;"><label>*<%= Html.Term("ProcessType", "Process Type")%></label></td>
                        <td><%= Html.DropDownList("ddlProcessType", new SelectList(ViewBag.ProcessTypes, "Key", "Value"), new { @id = "ddlProcessType" })%></td>
                    </tr>

                    <tr>
                        <td style="text-align: right;"><label><%= Html.Term("FileSequentialCode", "File Sequential Code")%></label></td>
                        <td><input type="text" id="txtFileSequentialCode" /></td>
                    </tr>
            
                    <tr>
                        <td></td>
                        <td><button id="btnCreateSpcFile" class="Button BigBlue"><%= Html.Term("CreateSPCFile", "Create SPC File")%></button></td>
                    </tr>
                </table>
        
                <br />
                <hr />
                <span id="message"></span>
            </div>


            <div id="alternativePanel" style="display: none;">
                <form id="formLoad" action="" enctype="multipart/form-data">
                   <br/><br />
                   <a class="Button BigBlue" id="btnBrowse" href="javascript:void(0);">
                    <%= Html.Term("Promotions_LoadAccounts", "Load Accounts from Excel")%>
                   </a>
                   <label id="label"></label>
                   <input type="file" id="inputLoadAccounts" name="inputLoadAccounts" style="display:none"/>
                   <br/><br />
                   <input type="submit" id="submitHidden" style="display:none"/>
               </form>
                <br />
                <span id="excelMessage" style="color: Red;" ></span>

                <table width="100%" class="FormTable">
				    <tbody>
					    <tr id="AccountPanel">
						    <td>
                                <%--<div class="UI-secBg pad10 brdrYYNN">
                                </div>
							    <div class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility" id="paginateAccountIDsGrid">
								    <a class="deleteButton UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-deleteSelected">
								    </span><span>
									    <%=Html.Term("DeleteSelected", "Delete Selected") %></span></a>
							    </div>--%>
							    <table width="100%" class="DataGrid" id="accountIDsGrid">
								    <thead>
									    <tr class="GridColHead">
										    <th class="GridCheckBox">
											    <input type="checkbox" id="accountIDsCheckAll" />
										    </th>
										    <th>
											    <%=Html.Term("Account ID")%>
										    </th>
									    </tr>
								    </thead>
								    <tbody>

								    </tbody>
							    </table>
							    <div class="UI-mainBg Pagination" id="AccountPaginatedGridPagination">
								    <input type="hidden" id="AccountpaginatedGridRefresh" />
								    <div class="PaginationContainer">
									    <span class="ClearAll clr"></span>
								    </div>
							    </div>
						    </td>
					    </tr>
				    </tbody>
			    </table>

            </div>
        </div>

    </div>
    
    <%--<div id="cmsMessageModal" class="LModal jqmWindow" style="z-index:10000">
        <button id="btnLoad" class="Button BigBlue"><%= Html.Term("LoadFromExcel", "LOAD ACCOUNTS FROM EXCEL")%></button>
        <div class="mContent">
            <%= 
                Html.PaginatedGrid("~/Edit/GetCMSMessages")
                    .AddColumn(Html.Term("Account ID"), "AccountID", false)
                    .Render();
            %>
        </div>
    </div>--%>
    
    <script type="text/javascript">

        jQuery.fn.refreshTable = function () {
            $(this).find('tr:odd').addClass('Alt');
            $(this).find('tr:even').removeClass('Alt');
            return $(this);
        }


        $(document).ready(function () {

            $("#txtFileSequentialCode").keypress(function () {
                if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                    event.preventDefault();
                }
            });

            $("#ddlProcessType").change(function () {
                if ($(this).val() == 2) {
                    $("#btnCreateSpcFile").prop("disabled", "disabled");
                    $("#alternativePanel").show();
                }
                else {
                    $("#btnCreateSpcFile").removeProp("disabled");
                    $("#alternativePanel").hide();
                }

                $("#txtFileSequentialCode").val("");
                $("#message").text("");
            });

            $("#btnCreateSpcFile").click(function () {
                CreateFileAction();
            });

        });


        function CreateFileAction() {
            var code = $("#txtFileSequentialCode").val();

            if (code === "") {
                $("#message").css("color", "red");
                $("#message").text("File Sequential Code is required");
                return;
            }

            var postData = {
                TypeProcess: $("#ddlProcessType").val(),
                FileSequentialCode: code
            };

            $.ajax({
                url: '/ArrearsDefaultsReports/Report',
                type: "POST",
                cache: false,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(postData),
                dataType: "json",
                beforeSend: function (jqXHR, settings) {
                    //ajaxStart();
                },
                success: function (data) {

                    if (data.success) {
                        $("#message").css("color", "black");
                        $("#message").html('<%= Html.Term("DownloadFile", "To download the file click on the next link: ") %>' +
                                        "<i><a href='/ArrearsDefaultsReports/DownloadTextFile?FileName=" + data.message +
                                        "' style='font-size: 14px; text-decoration: underline;'>\"" + data.message + "\"</a></i>");
                    }
                    else {
                        $("#message").css("color", "red");
                        $("#message").text(data.message);
                    }

                },
                error: function (jqxhr) {
                }
            });
        }


        document.getElementById('formLoad').onsubmit = function () {

            var formdata = new FormData(); //FormData object
            var fileInput = document.getElementById('inputLoadAccounts');
            //Iterating through each files selected in fileInput
            for (i = 0; i < fileInput.files.length; i++) {
                //Appending each file to FormData object
                formdata.append(fileInput.files[i].name, fileInput.files[i]);
            }
            //Creating an XMLHttpRequest and sending

            var xhr = new XMLHttpRequest();
            xhr.open('POST', '/GeneralLedger/ArrearsDefaultsReports/UploadExcelData');
            xhr.send(formdata);
            $('#btnBrowse').showLoading();
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    var response = JSON.parse(xhr.responseText);

                    $("#accountIDsGrid").find("tbody > tr").remove();
                    $("#btnCreateSpcFile").prop("disabled", "disabled");
                    $("#accountIDsCheckAll").removeProp("checked");

                    if (response.result) {
                        $("#excelMessage").html("");

                        var items = $('#accountIDsGrid tbody tr');
                        items.each(function (i) {
                            var int = parseInt($(this).find('.accountId').val());
                            var index = response.ids.indexOf(int);
                            if (index != -1) {
                                response.ids.splice(index, 1);
                                response.names.splice(index, 1);
                            }
                        });
                        for (var i = 0; i < response.ids.length; i++) {
                            $('#accountIDsGrid').prepend('<tr>'
						            + '<td><input type="checkbox" class="selectRow" /><input type="hidden" class="accountId" value="' + response.ids[i] + '" /></td>'
						            + '<td>' + response.ids[i] + '</td>'
						            + '</tr>').refreshTable();
                        }

                        setAutoAddGridVisibility();
                        $("#btnCreateSpcFile").removeProp("disabled");
                    } else {
                        $("#excelMessage").html(response.message);
                    }
                }
                $('#btnBrowse').hideLoading();
            }
            return false;
        }

        $('#btnBrowse').click(function () {

            $('#inputLoadAccounts').trigger('click');
        });

        $('#accountIDsCheckAll').click(function () { $('#accountIDsGrid .selectRow').attr('checked', $(this).prop('checked')); });

        $('#paginateAccountIDsGrid .deleteButton').click(function () {
            $('#accountIDsGrid .selectRow:checked').each(function () {
                $(this).closest('tr').remove();
            });
            $("#accountIDsCheckAll").prop("checked", false);
            $('#accountIDsGrid').refreshTable();
        });

        $('#inputLoadAccounts').change(function (e) {
            $('#submitHidden').trigger('click');
        });


        function setAutoAddGridVisibility() {
            if ($('#accountIDsGrid tbody tr').length) {
                $('#accountIDsGrid').show();
            }
            else {
                $('#accountIDsGrid').hide();
            }
        }

    </script>
    
</asp:Content>