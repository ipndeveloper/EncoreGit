<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <% var Id = 0;%>
    <link href="../../../../Content/CSS/Validation.css" rel="stylesheet" type="text/css" />
    <script src="../../../../Scripts/jquery.number.min.js" type="text/javascript"></script>
    <script src="../../../../Scripts/jquery.numeric.js" type="text/javascript"></script>
    <script src="../../../../Scripts/Validaciones.js" type="text/javascript"></script>
    <script src="../../../../Scripts/jquery.filestyle.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $('input[type=file]').filestyle({
                buttonText: '',
                buttonWidth: 32,
                buttonHeight: 32,
                inputHeight: 32,
                inputWidth: 350
            });


            $('#fileUpload').change(function () {
                var value = this.value;
                val = value.split("\\");
                var file = (val[val.length - 1]).split('.');
                var ext = file[file.length - 1];
                //
                if (ext != "mdb") {
                    $("#idMsj").show();
                    //showMessage("The selected file is no tvalid", true);
                    $(this).val("");
                }
                else {
                    $("#idMsj").hide();
                }

            });

        });

        $(document).ready(function () {
            var options = {
                beforeSend: function () {
                    //$("#progress").show();
                },
                success: function () {
                    //$("#progress").hide();

                },
                complete: function (response) {
                    //$("#message").html("<font color='green'>" + response.responseText + "</font>");
                    if (response.responseText == "uploaded") {
                        showMessage('Ceps Upload!', false);
                    }

                },
                error: function () {
                    //$("#message").html("<font color='red'> ERROR: unable to upload files</font>");
                }
            };
            //            $("#frmLoadBulkCeps").ajaxForm(options);
            //$("#form1").ajaxForm();
        });


    </script>
    <style type="text/css">
        .file-fake
        {
            padding: 6px 2px 7px;
            border: 1px solid #ddd;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Admin") %>">
        <%= Html.Term("Admin", "Admin") %></a> >
    <%= Html.Term("CPEs Update") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("CPEs Update")%>
        </h2>
        <a href="<%= ResolveUrl("~/Admin/Ceps/Index") %>">
            <%= Html.Term("AddNewCeps", "Add New Ceps")%></a>
    </div>
    <div>
        <% using (Html.BeginForm("LoadBulkCeps", "Ceps", FormMethod.Post, new { id = "frmLoadBulkCeps", enctype = "multipart/form-data" })) %>
        <% { %>
        <!-- Form content goes here -->

        <h3>
            <%= Html.Term("CEPLookUp", "CEPs Look up")%>
        </h3>

        <br />

        <div id="addresses">
                <% Html.RenderPartial("Address", new AddressModel(){
                       Address = null,
                       LanguageID = CoreContext.CurrentLanguageID,
                       ShowCountrySelect = true,
                       ChangeCountryURL = "~/Accounts/BillingShippingProfiles/GetAddressControl",
                       ExcludeFields = new List<string>() { "ProfileName", "Attention", "Address1", "Address2", "Address3" },
                       Prefix = "Address",
                       HonorRequired = false
                   }); 
                %>
        </div>

        <br />

        <h3>
            <%= Html.Term("CEPFileUpload", "CEPs File Upload")%>
        </h3>

        <br />

        <table id="newCeps" class="FormTable Section" width="100%">
            <tr>
                <td class="FLabel" style="width: 950px;">
                    <%= Html.Term("File")%>:
                </td>
            </tr>
            <tr>
                <td>
                    <input type="file" name="fileUpload" id="fileUpload" />
                </td>
            </tr>
            <tr id="idMsj" style="display: none;">
                <td>
                    <p style="color:Red;"><small>The selected file is not valid</small></p>
                </td>
            </tr>
            <tr>
                <td>
                    <p>
                        <input type="submit" style="display: inline-block; cursor:pointer;" class="Button BigBlue" value="Update" />
                        <%--   <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                                    <%= Html.Term("Update", "Update")%></a>--%>
                    </p>
                </td>
            </tr>
        </table>
        <% } %>
    </div>
</asp:Content>