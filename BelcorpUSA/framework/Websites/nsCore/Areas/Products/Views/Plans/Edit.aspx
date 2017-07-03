<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Schedules/Schedules.Master" 
Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.PlanSearchData>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#btnSave').click(function () {
                if ($('#planForm').checkRequiredFields()) {
                    var data = {
                        planId: $('#hddPlanId').val(),
                        planCode: $('#txtPlanCode').val(),
                        name: $('#txtName').val(),
                        enabled: $('#chkEnabled').prop('checked')
                    };

                    $.post('<%= ResolveUrl("~/Products/Plans/Save") %>', data, function (response) {
                        if (response.result) {
                            showMessage('<%=@Html.Term("PlanSavedSuccessfully", "Plan Saved Successfully")%>', false);
                            if (!$('#hddPlanId').val()) { // Create case
                                $('#hddPlanId').val(response.planId);
                                // Reload Edit Mode 
                                window.location.replace('<%= ResolveUrl("~/Products/Plans/Edit") %>' + "/" + response.planId);
                            }
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            });
        });
        </script>
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
    <a href="<%= ResolveUrl("~/Products/Plans") %>">
	    <%= Html.Term("ScheduleManagement", "Schedule Management")%></a> >
    <%= Model.PlanID == 0 ? Html.Term("NewPlan", "New Plan") : Html.Term("EditPlan", "Edit Plan") %>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("Plans", "Plans") %>
        </h2>
        <a href="<%= ResolveUrl("~/Products/Plans") %>"><%= Html.Term("BrowsePlans", "Browse Plans") %></a>
        | <a href="<%= ResolveUrl("~/Products/Plans/Edit") %>"><%= Html.Term("CreateaNewPlan", "Create a New Plan") %></a>
	</div>

    <table id="planForm" class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Code", "Code") %>:
            </td>
            <td>
                <input type="hidden" id="hddPlanId" value="<%= Model.PlanID == 0 ? "" : Model.PlanID.ToString() %>" />
                <input id="txtPlanCode" type="text" value="<%= Model.PlanCode %>"
                    class="required" name="<%= Html.Term("PlanCodeIsRequired", "Plan code is required") %>" 
                    style="width: 20.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Name", "Name") %>:
            </td>
            <td>
                <input id="txtName" type="text" value="<%= Model.Name %>"
                    class="required" name="<%= Html.Term("NameIsRequired", "Name is required") %>" 
                    style="width: 20.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Enabled", "Enabled") %>:
            </td>
            <td>
                <input type="checkbox" id="chkEnabled" <%= Model.Enabled ? "checked=\"checked\"" : "" %> 
                    <%= Model.DefaultPlan ? "disabled=\"disabled\"" : "" %> />
                <label for="chkEnabled"><%: Html.Term("Enabled", "Enabled") %></label>
            </td>
        </tr>

        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                        <%= Html.Term("SavePlan", "Save Plan") %></a>
                </p>
            </td>
        </tr>
    </table>

</asp:Content>
