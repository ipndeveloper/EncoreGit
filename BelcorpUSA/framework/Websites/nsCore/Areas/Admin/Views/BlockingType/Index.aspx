<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Admin") %>">
        <%= Html.Term("Admin", "Admin") %></a> >
    <%= Html.Term("BlockingType") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">    
    <script type="text/javascript">
        $(document).ready(function () {
            // Al hacer click en el Name y ver el detalle
            $('.btnViewStats').live('click', function () {
                $("#hdnID").val("0");
                $("#txtName").val("");
                $("#chkStatus").attr("checked", true);
                $('#addMovementModel').jqmShow();
            });

            // Al hacer click btnSaveBlockingType
            $('#btnSaveBlockingType').click(function () {
                var pID = $("#hdnID").val();
                var pName = $("#txtName").val();
                var pStatus = $("#chkStatus").attr("checked") ? '1' : '0';

                if ($.trim(pName) == "") {
                    showMessage('<%= Html.Term("NameIsRequired", "Name is required") %>', true);
                    return false;
                }

                $.ajax({
                    type: 'POST',
                    url: '/Admin/BlockingType/Save',
                    data: (
                        {
                            AccountBlockingTypeID: pID,
                            Name: pName,
                            Status: pStatus
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
                $("#hdnID").val(newID);
                $("#txtName").val(newName);
                $("#chkStatus").attr("checked", (newEnabled == 'True') ? true : false);
                $('#addMovementModel').jqmShow();
            });

        });

    </script>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("BlockingType")%>
        </h2>
        <a href="javascript:void(0)" class="btnViewStats">
            <%= Html.Term("AddNewBlockingType", "Add new blocking type")%></a>
    </div>
    <%    
        Html.PaginatedGrid("~/Admin/BlockingType/Get")
        .AddColumn(Html.Term("Name", "Name"), "Name", false)
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
            <table id="newBlockingType" class="FormTable Section">
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
            </table>
            <br />
            <p>
                <a id="btnSaveBlockingType" href="javascript:void(0);" class="Button BigBlue">
                    <%= Html.Term("Save", "Save")%></a> <a href="javascript:void(0);" class="Button jqmClose">
                        <%= Html.Term("Cancel")%></a>
            </p>
        </div>
    </div>
</asp:Content>
