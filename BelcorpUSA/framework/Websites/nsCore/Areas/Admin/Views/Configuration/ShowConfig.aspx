<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(function () {
        var data = {
            Url: $('#Url').val(),
            License: $('#License').val(),
            Account: $('#Account').val()
        };

        $('#btnPingAvatax').click(function () {
            $.post('<%= ResolveUrl("~/Admin/PingAvataxUrl") %>', data, function (response) {
                if (response.result)
                    showMessage(response.message, false);
                else
                    showMessage(response.message, true);
                //window.location.reload(true);
            });
        });
        
    });
	</script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		<%= Html.Term("Configuration")%></h2>
        <table width="100%" cellspacing="0" class="DataGrid" border="2">
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("AvataxURL", "Avatax URL")%>:
            </td>
            <td style="width: 13.636em;">
                <%= Html.Term("License", "License ")%>:
            </td>
            <td style="width: 13.636em;" colspan="2">
                <%= Html.Term("AccountNumber", "Account Number")%>:
            </td>
            
        </tr>
        <tr>
            <td>
                <input type="text" id="Url" class="required" name="<%= Html.Term("UrlRequired", "Url is required.") %>"
                    value="<%=ViewData["Avatax_Url"]%>" style="width: 25em;" readonly="true" />
            </td>
            <td>
                <input type="text" id="License" class="required" name="<%= Html.Term("LicenseRequired", "License is required.") %>"
                    value="<%=ViewData["Avatax_License"]%>" style="width: 25em;" readonly="true" />
            </td>
            <td>
                <input type="text" id="Account" class="required" name="<%= Html.Term("AccountNumberRequired", "Account Number is required.") %>"
                    value="<%=ViewData["Avatax_Account"]%>" style="width: 25em;" readonly="true" />
            </td>
            <td style="text-align:left; vertical-align:top">
                <a href="javascript:void(0);" class="Button BigBlue" id="btnPingAvatax">Ping Avatax</a>
            </td>
        </tr>
    </table>	
</asp:Content>