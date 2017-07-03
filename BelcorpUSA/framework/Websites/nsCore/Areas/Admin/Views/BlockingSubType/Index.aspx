<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Admin") %>">
        <%= Html.Term("Admin", "Admin") %></a> >
    <%= Html.Term("BlockingSubType") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            //Cargando Block Process          I

            var str;
            // alert('in submit function');
            $.ajax({
                type: 'POST',
                url: '/Admin/BlockingSubType/GetTypeProcess',
                contentType: 'application/json',
                data: JSON.stringify(),
                success: function (data) {

                    $.each(data, function (index, result) {
                        //' + result.AccountBlockingProcessID + '
                        str += '<tr><td>' + result.Description + '</td>';
                        str += '<td><input type="checkbox" name="name[]" value="' + result.AccountBlockingProcessID.toString() + '"></td></tr>';
                    });

                    $('#BlockTableProcess').append(str);

                }
            });


            //Fin Cargo Block Process

            // Al hacer click en el Name y ver el detalle
            $('.btnViewStats').live('click', function () {
                $("#hdnID").val("0");
                $("#txtName").val("");
                $("#chkStatus").attr("checked", true);
                $('#addMovementModel').jqmShow();
                $('#BlockingType').val("Select a blocking type");

                //Limpiando Checks Tablas
                $('#BlockTableProcess input[type="checkbox"]').prop('checked', false);

            });

            //GRABANDO BLOCKINGSUBTYPE
            // Al hacer click btnSaveBlockingSubType
            $('#btnSaveBlockingSubType').click(function () {
                var pID = $("#hdnID").val();
                var pTypeID = $("#BlockingType").val();
                var pName = $("#txtName").val();
                var pStatus = $("#chkStatus").attr("checked") ? '1' : '0';

                if ($.trim(pName) == "") {
                    showMessage('<%= Html.Term("NameIsRequired", "Name is required") %>', true);
                    return false;
                }

                if ($.trim(pTypeID) == '') {
                    showMessage('<%= Html.Term("BlockingTypeIsRequired", "BlockingType is required") %>', true);
                    return false;
                }

                //Captuando Check - Marcados Tabla

                var BlockIDs = $("#BlockTableProcess input:checkbox:checked").map(function () {
                    return $(this).val();
                }).toArray();


                $.ajax({
                    type: 'POST',
                    url: '/Admin/BlockingSubType/Save',
                    data: (
                        {
                            AccountBlockingSubTypeID: pID,
                            AccountBlockingTypeID: pTypeID,
                            Name: pName,
                            Status: pStatus,
                            ListaBlockAccess: BlockIDs.toString()
                        }),
                    asyn: false,
                    success: function (data) {
                        if (data.result == true) {
                            showMessage(data.menssage, data.boolean);
                            $('#addMovementModel').jqmHide();
                            window.location.reload();
                        }
                    }
                });
            });


            // Ubicación del modal
            $('#addMovementModel').jqm({ modal: true, onShow: function (h) {
                h.w.css({
                    top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px !important',
                    left: Math.floor(parseInt($(window).width() / 2)) + Math.floor(parseInt(h.w.width())) + 'px !important'
                }).fadeIn();
            }
            });

            $(".blockingTypeDetail").live('click', function () {
                var newID = $(this).attr("new-id");
                var newName = $(this).attr("new-Name");
                var newEnabled = $(this).attr("new-Enabled");
                var newsubtypeID = $(this).attr("new-subtypeID");

                $("#hdnID").val(newsubtypeID);
                $("#txtName").val(newName);
                $("#chkStatus").attr("checked", (newEnabled == 'True') ? true : false);
                $('#BlockingType').val(newID);
                //Limpiando Checks Tablas
                $('#BlockTableProcess input[type="checkbox"]').prop('checked', false);

                $.ajax({
                    type: 'POST',
                    url: '/Admin/BlockingSubType/GetTypeProcessBlockingSubTypeID',
                    //contentType: 'application/json',
                    data: (
                        {
                            AccountBlockingSubTypeID: newsubtypeID
                        }),
                    success: function (data) {

                        $("#BlockTableProcess input[type=checkbox]").each(
                     function () {
                         if (data.contains($(this).val())) {
                             $(this).attr('checked', 'checked');
                         }

                     });


                    }
                });
                $('#addMovementModel').jqmShow();




            });


            //TABLA PARA BLOCKING PROCESS




        });

    </script>

    <div class="SectionHeader">
        <h2>
            <%= Html.Term("BlockingSubType")%>
        </h2>
        <a href="javascript:void(0)" class="btnViewStats">
            <%= Html.Term("AddNewBlockingSubType", "Add new blocking sub type")%></a>
    </div>
    <%    
        Html.PaginatedGrid("~/Admin/BlockingSubType/Get")
        .AddColumn(Html.Term("Name", "Name"), "Name", false)
        .AddColumn(Html.Term("BlockingType", "Blocking Type"), "BlockingType", false)
        .AddColumn(Html.Term("Status"), "Enabled", false)
        .ClickEntireRow()
        .Render(); 
    %>


      <div id="addMovementModel" class="LModal jqmWindow">
        <div class="mContent">
            <input type="hidden" id="txtWareHousesMaterialID" />
            <h2>
                <%=Html.Term("Add", "Add")%>
            </h2>
                         <table id="newBlockingSubType" class="FormTable Section">

                             <tr>
                                <td class="Flabel"><%=Html.Term("TypeBlocking", "Type")%> : </td>
                                <td>         
                                  <%= Html.DropDownList("BlockingType", (SelectList)ViewBag.BlockingType, "Select a blocking type", new { style = "width: 150px;" })%>
                                </td>
                            </tr>


                             <tr>
                            <td class="Flabel">
                                <%=Html.Term("Name","Name")%>
                                :
                            </td>
                             <td>
                        <input type="hidden" id="hdnID" />
                        <input type="text" id="txtName" />
                         </td>
                         </tr>

                             <tr>
                            <td class="Flabel">
                                <%=Html.Term("Status","Name")%>
                                :
                            </td>
                            <td>
                                <input type="checkbox" id="chkStatus" />
                         </td>
                         </tr>

                                
                             <tr>
                                <td colspan="2">
                                    <table id="BlockTableProcess" style="border:1px solid black">
                                        <tr>
                                            <td colspan="2">
                                            <h2><%=Html.Term("VerifyProcessBlock","Verify Process To Block")%></h2>                                            
                                            </td>
                                           
                                        </tr>
                                        <tr>
                                        <td></td>
                                        <td></td>
                                        </tr>

                                    </table>
                                </td>
                                <td>    
                                 <%-- <%= Html.ListBox("BlockingType", (SelectList)ViewBag.BlockingType, new { style = "width: 150px;" })%>--%>
                                </td>
                            </tr>
         </table>

            <br />
            <p>
                <a id="btnSaveBlockingSubType" href="javascript:void(0);" class="Button BigBlue">
                    <%= Html.Term("Save", "Save")%></a> <a href="javascript:void(0);" class="Button jqmClose">
                        <%= Html.Term("Cancel")%></a>
            </p>
        </div>
    </div>
</asp:Content>