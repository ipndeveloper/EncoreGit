<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("CampaignMonthlyClosing", "Campaign Monthly Closing")%>
        </h2>
    </div>
    <div id="divProcess">
        <table>
            <tbody>
                <tr>
                    <td style="width: 200px;">
                        <%= Html.Term("Plans", "Plans")%>
                    </td>
                    <td>
                        <%= Html.DropDownList("ddlPlans", (IEnumerable<SelectListItem>)TempData["AvailablePlans"], new  {Style= "width: 150px;" })%>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px;">
                        <%= Html.Term("PeriodToProcess", "Period To Process")%>
                    </td>
                    <td>
                        <span id="Period"><%=TempData["ActivePeriod"]%></span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <input type="button" id="btnProcess" value = "<%= Html.Term("Process", "Process") %>" class="Button BigBlue"/>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <input type="hidden" id="SuccessMessage" value="<%= Html.Term("CampaignMonthlyClosingHasBeenProcessed", "Campaign Monthly Closing Has Been Processed")%>" />
    <%--<input type="hidden" id="hConfirmText" value="<%= Html.Term("ConfirmCloseMLM-CurrentPeriod", "¿Are you sure to close MLM campaign and current period?")%>" />--%>
    <input type="hidden" id="hConfirmText" value="<%= Html.Term("YouAreSureToInitializeTheNextCampaign", "¿You are Sure to initialize the next campaign?")%>" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $("a[href*='/Commissions']").attr("class","selected")
            $("a[href*='/Support']").removeAttr("class")
                               
            $("#btnProcess").click(function () {
                if (fncConfirm()) {
                    showLoading("#divProcess");
                    var data = {
                        Plan: $("#ddlPlans").val(),
                        Period: $("#Period").html()
                    };

                    $.post('/MonthlyClosing/Process', data, function (response) {
                        if (response.result) {
                            showMessage($("#SuccessMessage").val(), true);
                        }
                        else { showMessage(response.message, true); }
                    })
	                .always(function () { hideLoading("#divProcess"); });
                }
            });

            function fncConfirm() {
                var confirmText = $("#hConfirmText").val();
                if (confirm(confirmText)) { return true; }
                else { event.preventDefault(); return false; }
            }
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>
