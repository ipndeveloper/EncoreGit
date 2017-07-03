<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/Products.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> >
    <%= Html.Term("CreationCampaign", "Creation Campaign")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%--<link href="../../../../Content/CSS/uploadify.css" rel="Stylesheet" type="text/css" />
    <script src="../../../../Scripts/uploadify/jquery.uploadify.js" type="text/javascript"></script>--%>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Creation Campaign") %></h2>
    </div>
    <div class="UI-lightBg hiddenPanel pad10 overflow">
        <table>
            <tr>
                <td>
                    <%=Html.Term("Campaign", "Campaign")%>:
                </td>
                <td style="padding-left: 20px">
                    <%=Html.Term("Catalog", "Catalog")%>:
                </td>
                <td style="padding-left: 20px">
                    <%=Html.Term("UploadMatrix", "Upload Matrix")%>:
                </td>
                <td style="padding-left: 20px">
                    <%=Html.Term("UploadContent", "Upload Content")%>:
                </td>
                <%-- <td style="padding-left: 20px">
                    <%=Html.Term("UploadContent", "Upload Content")%>:
                </td>--%>
            </tr>
            <tr>
                <td>
                    <br />
                    <%--<%=Html.DropDownList("ddlCampaign", (TempData["GetCampaign"] as IEnumerable<SelectListItem>))%>--%>
                    <select id="ddlCampaign" class="required" style="width: 170px">
                        <option value="0">
                            <%=Html.Term("SelectaCampaign", "Select a Campaign")%>
                        </option>
                        <%foreach (var item in (TempData["GetCampaign"] as IEnumerable<SelectListItem>))
                          { %>
                        <option value="<%=item.Value%>">
                            <%= item.Text %></option>
                        <%} %>
                    </select>
                </td>
                <td style="padding-left: 20px">
                    <br />
                    <select id="ddlCatalog" class="required" style="width: 160px" name="<%= Html.JavascriptTerm("valStateCity", "Value to City not selected.") %>">
                        <option value="0">
                            <%=Html.Term("SelectACatalog", "Select a Catalog")%></option>
                    </select>
                </td>
                <td style="padding-left: 20px">
                    <form enctype="multipart/form-data" action="" id="formLoad" method="post">
                    <br />
                    <a class="Button BigBlue" id="btnBrowse" href="javascript:void(0);">
                        <%= Html.Term("LoadCampaingMatrix", "LOAD FROM EXCEL")%>
                    </a>
                    <label id="label">
                    </label>
                    <input type="file" id="inputLoadMatrix" name="ninputLoadMatrix" accept="xlsx|xls"
                        style="display: none" />
                    <input type="submit" id="submitHidden" style="display: none" />
                    </form>
                </td>
                <td style="padding-left: 20px">
                    <form enctype="multipart/form-data" action="" id="formImage" method="post">
                    <br />
                    <a class="Button BigBlue" id="btnSaveImages" href="javascript:void(0);">
                        <%= Html.Term("SaveImages", "Save Images")%>
                    </a>
                    <input type="file" id="inputLoadImage" name="ninputLoadImage" class="multi" accept="jpg|png"
                        style="display: none" />
                    <input type="submit" id="submitHiddenI" style="display: none" />
                    </form>
                </td>
            </tr>
        </table>
        <br />
    </div>
    <hr />
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ProductMatrixLog")%></h2>
    </div>
    <% Html.PaginatedGrid<OrderToBatch>("~/Products/File/GetMatrixErrorLog")
                .AddInputFilter(Html.Term("sku", "sku"), "CUV")
                .AddInputFilter(Html.Term("Material", "Material"), "MaterialID")
                .AddColumn(Html.Term("SKU", "CUV"), "CUV")
                .AddColumn(Html.Term("Material", "Material"), "MaterialID")
                .AddColumn(Html.Term("Status", "Status"), "Descripcion")
                .AddColumn(Html.Term("Message", "Message"), "Mensaje")
                .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))
        .ClickEntireRow()
        .Render(); %>
    <script type="text/jscript">

        $(document).ready(function () {
            $('#exportToExcel').click(function () {
                window.location = '<%= ResolveUrl("~/Products/File/MatrixErrorLogExport") %>';
            });
            //        $(function () {
            $('#inputLoadMatrix').change(function (e) {
                $('#submitHidden').trigger('click');
            });

            $('#inputLoadImage').change(function (e) {
                $('#submitHiddenI').trigger('click');
            });

            $('#btnBrowse').click(function () {
                $('#inputLoadMatrix').trigger('click');
            });

            $('#btnSaveImages').click(function () {
                $('#inputLoadImage').trigger('click');
            });

            // Inicio Subida Imagen
            document.getElementById('formImage').onsubmit = function () {
                var formdata = new FormData(); //FormData object
                var fileInput = document.getElementById('inputLoadImage');
                for (i = 0; i < fileInput.files.length; i++) {
                    formdata.append(fileInput.files[i].name, fileInput.files[i]);
                }
                var xhr = new XMLHttpRequest();
                xhr.open('POST', '/Products/File/LoadImage');
                xhr.send(formdata);
                $('#btnSaveImages').showLoading();
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4 && xhr.status == 200) {
                        var response = JSON.parse(xhr.responseText);
                        if (response.result) {
                            showMessage(response.message);
                            location.reload();
                        } else {
                            showMessage(response.message, true);
                            location.reload();
                        }
                    }
                    $('#btnSaveImages').hideLoading();
                }
                return false;
            }
            // Fin Subida Imagen

            // Inicio Subida Matriz
            document.getElementById('formLoad').onsubmit = function () {
                if ($('#ddlCampaign').val() == 0 ||
                    $('#ddlCatalog').val() == 0 ||
                    $('#ddlCampaign').val() == null ||
                    $('#ddlCatalog').val() == null) { showMessage('<%= Html.Term("SelectCampaignAndCatalog", "Select campaign and catalog")%>', true); return; }

                var formdata = new FormData(); //FormData object
                var fileInput = document.getElementById('inputLoadMatrix');
                for (i = 0; i < fileInput.files.length; i++) {
                    formdata.append(fileInput.files[i].name, fileInput.files[i]);
                }
                formdata.append('PeriodID', $('#ddlCampaign').val());
                formdata.append('CatalogID', $('#ddlCatalog').val());
                var xhr = new XMLHttpRequest();
                xhr.open('POST', '/Products/File/SubmitFileTemporalMatrix');
                xhr.send(formdata);
                $('#btnBrowse').showLoading();
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4 && xhr.status == 200) {
                        var response = JSON.parse(xhr.responseText);
                        if (response.result) {
                            showMessage(response.message);
                            window.setTimeout(function () { location.reload() }, 5000);
                        } else {
                            showMessage(response.message, true);
                            window.setTimeout(function () { location.reload() }, 5000);
                        }
                    }
                    $('#btnBrowse').hideLoading();
                }
                return false;
            }
            // Fin Subida Matriz
        });

        $('#ddlCampaign').change(function () {
            GetCatalogByCampaign();
        });

        var GetCatalogByCampaign = function () {
            var catalog = document.getElementById("ddlCatalog");
            $('#ddlCatalog option').each(function () {
                $('#ddlCatalog').find('option').remove();
            });

            $.getJSON('Campaign/GetCatalogByCampaign', { campaignID: $('#ddlCampaign').val() },
        function (data) {
            $.each(data, function (i, item) {
                catalog.options[i] = new Option(item.Name, item.Id);
            });
        });
        };


        $("#A1").click(function () {
            $.ajax({
                type: 'POST',
                url: '/Campaign/GenerateCampaign',
                asyn: false,
                data: (
                    { catalogID: $("#ddlCatalog").val()
                    }),
                asyn: false,
                success: function (data) {
                }
            });
        }); 

    </script>
</asp:Content>
