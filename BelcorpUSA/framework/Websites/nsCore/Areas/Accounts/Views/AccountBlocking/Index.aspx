<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Accounts") %>">
        <%= Html.Term("Accounts") %></a> >
    <%= CoreContext.CurrentAccount.FullName %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- INICIO JAVASCRIPT -->
    <script type="text/javascript">
        $(document).ready(function () {
            // Al hacer click en el Name y ver el detalle
            $('.btnViewStats').live('click', function () {
                $("#hdnID").val("0");
                $("#textareaID").text('');
                $("#textareaID").val('');
                $("#chkStatus").attr("checked", true);
                $('#addMovementModel').jqmShow();


                //Limpiando Checks Tablas
                $('#BlockTableProcess input[type="checkbox"]').prop('checked', false);

            });
            
            $("#BlockingType").change(function () {
                var selectedItem = $(this).val();
                var ddlBlokingSubType = $("#BlockingSubType");
                                
                $.ajax({
                    cache: false,
                    type: "GET",
                    url: "/Accounts/AccountBlocking/GetBlokingSubTypeList",
                    data: { "intBlockingTypeID": selectedItem },
                    success: function (data) {
                        ddlBlokingSubType.html('');
                        $.each(data, function (AccountBlockingSubTypeID, Name) {
                            ddlBlokingSubType.append($('<option></option>').val(Name.AccountBlockingSubTypeID).html(Name.Name));
                        });                        
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        ddlBlokingSubType.html('');                        
                    }
                });
            });

            //GRABANDO BLOCKINGSUBTYPE
            // Al hacer click btnSaveBlockingSubType
            $('#btnSaveBlockingSubType').click(function () {
                var pIDBlockingType = $("#BlockingType").val();
                var pIDBlockingSubType = $("#BlockingSubType").val();
                var pReasons = $("#textareaID").val();
                var pStatusBlock = <%= Convert.ToInt32(ViewBag.IsBlocking) %>;

                if (pReasons == '' && pStatusBlock == 1) {
                    showMessage('<%= Html.Term("ReasonIsRequired", "Reason is required") %>', true);
                    return false;
                }

                if ($.trim(pIDBlockingType) == "" && pStatusBlock == 0) {
                    showMessage('<%= Html.Term("BlockingTypeIsRequired", "BlockingType is required") %>', true);
                    return false;
                }

                if ($.trim(pIDBlockingSubType) == "" && pStatusBlock == 0) {
                    showMessage('<%= Html.Term("BlockingSubTypeIsRequired", "BlockingSubType is required") %>', true);
                    return false;
                }              

                //Captuando Check - Marcados Tabla

                $.ajax({
                    type: 'POST',
                    url: '/Accounts/AccountBlocking/Save',
                    data: (
                        {
                            AccountBlockingTypeID: pIDBlockingType,
                            AccountBlockingSubTypeID: pIDBlockingSubType,
                            Reasons: pReasons,
                            Status: !pStatusBlock
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


            // INI MODAL
            $('#addMovementModel').jqm({ modal: true, onShow: function (h) {
                h.w.css({
                    top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px !important',
                    left: Math.floor(parseInt($(window).width() / 2)) + Math.floor(parseInt(h.w.width())) + 'px !important'
                }).fadeIn();
            }
            });
            //FIN MODAL
        });
    </script>
    <!-- FIN JAVASCRIPT -->
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("AccountBlocking", "Account Blocking") %></h2>
        <br />
        <a href="javascript:void(0)" class="btnViewStats">
            <%= Html.Term("ChangeBlocking", "Change Blocking")%></a>
    </div>
    <div id="ledgerContainer">
        <% Html.PaginatedGrid("~/Accounts/AccountBlocking/Get")
        .AddColumn(Html.Term("DateBlocking", "Date Blocking"), "DateCreatedUTC", false)
        .AddColumn(Html.Term("BlockingType", "BlockingType"), "BlockTypeName", false)
        .AddColumn(Html.Term("BlockingSubType", "BlockingSubType"), "BlockSubTypeName", false)
        .AddColumn(Html.Term("Reason", "Reasons"), "Reasons", false)
        .AddColumn(Html.Term("User", "UserName"), "UserName", false)
        .AddColumn(Html.Term("Status", "StatusName"), "StatusName", false)
               //.AddColumn(Html.Term("Balance"), "EndingBalance", false)
        .Render(); %>
    </div>
    <!-- 
       INICIO MODAL CHANGE BLOCKING
       -->
    <div id="addMovementModel" class="LModal jqmWindow">
        <div class="mContent">
            <input type="hidden" id="txtWareHousesMaterialID" />
            <h2>
                <%=Html.Term("ChangeBlocking", "Change Blocking")%>
            </h2>
            <table id="newBlockingSubType" class="FormTable Section">
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("TypeBlocking", "Type")%>
                        :
                    </td>
                    <td>
                        <%= Html.DropDownList("BlockingType",ViewBag.IsBlocking?new SelectList(string.Empty, "Value", "Text"):(SelectList)ViewBag.BlockingType, ViewBag.IsBlocking?"": "Select a blocking type", new { style = "width: 200px;" })%>
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("SubTypeBlocking", "SubType")%>
                        :
                    </td>
                    <td>
                        <%= Html.DropDownList("BlockingSubType", new SelectList(string.Empty, "Value", "Text"), ViewBag.IsBlocking ? "" : "Select a blocking subtype", new { style = "width: 200px;" })%>
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("ReasonBlocking","Reasons")%>
                        :
                    </td>
                    <td>
                        <input type="hidden" id="hdnID" />
                        <textarea id="textareaID" style="width: 320px; height: 120px;"></textarea>
                        <%--<input type="text" id="txtName"  style="width:300px; height:100px;"  />--%>
                    </td>
                </tr>
                <tr>
                    <td class="Flabel">
                        <%=Html.Term("StatusBlocking", "Status")%>
                        :
                    </td>
                    <td>
                        <%= Html.DropDownList("ListaStatus", (SelectList)ViewBag.ListaStatus, new { style = "width: 200px;" })%>
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
