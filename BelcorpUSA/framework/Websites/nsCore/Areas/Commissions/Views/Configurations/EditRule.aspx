<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" 
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.HelperObjects.SearchData.RequirementRuleSearchData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="SectionHeader">
		<h2>
			<%= Html.Term("Rules", "Rules")%>
        </h2>
        <a href="<%= ResolveUrl("~/Commissions/Configurations/Rules") %>"><%= Html.Term("BrowseRulesByPlan", "Browse Rules by Plan")%></a>
        | <a href="<%= ResolveUrl("~/Commissions/Configurations/EditRule") %>"><%= Html.Term("CreateaNewRule", "Create a New Rule")%></a>
	</div>


    <table id="RuleForm" class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("RuleType", "Rule Type") %>:
            </td>
            <td>
                <input type="hidden" id="hddRuleId" value="<%= Model.RuleRequirementID == 0 ? "" : Model.RuleRequirementID.ToString() %>" />
               
                <%= Html.DropDownListFor(m => m.RuleTypeID, (TempData["RuleTypes"] as IEnumerable<SelectListItem>), Html.Term("SelectaRuleType", "Select a Rule Type..."), new { id = "ddlRuleTypes" })%>
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Plans", "Plans")%>:
            </td>
            <td>
                <%= Html.DropDownListFor(m => m.PlanID, (TempData["Plans"] as IEnumerable<SelectListItem>), Html.Term("SelectaPlan", "Select a Plan..."), new { id = "ddlPlans" })%>
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Description", "Description")%>:
            </td>
            <td>
                <input id="txtDescription" type="text" value="<%= Model.Description %>" readonly="readonly"
                    style="width: 40.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("RuleValue1", "Value 1")%>:
            </td>
            <td>
                 <input id="txtValue1" type="text" maxlength="200" value="<%= Model.Value1 %>"
                    monedaidioma='CultureIPN' class="required" name="<%= Html.Term("RuleValue1IsRequired", "Value 1 is required") %>" 
                    style="width: 10.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("RuleValueType1", "Value Type 1")%>:
            </td>
            <td>
                <input id="txtValueType1" type="text" value="<%= Model.ValueType1 %>" readonly="readonly"
                    style="width: 30.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("RuleValue2", "Value 2")%>:
            </td>
            <td>
                 <input id="txtValue2" type="text" monedaidioma='CultureIPN' maxlength="200" value="<%= Model.Value2 %>"
                    style="width: 10.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("RuleValueType2", "Value Type 2")%>:
            </td>
            <td>
                <input id="txtValueType2" type="text" value="<%= Model.ValueType2 %>" readonly="readonly"
                    style="width: 30.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("RuleValue3", "Value 3")%>:
            </td>
            <td>
                 <input id="txtValue3" type="text" maxlength="200" monedaidioma='CultureIPN'  value="<%= Model.Value3 %>"
                    class="required" name="<%= Html.Term("RuleValue3IsRequired", "Value 3 is required") %>" 
                    style="width: 10.833em;"  />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("RuleValueType3", "Value Type 3")%>:
            </td>
            <td>
                <input id="txtValueType3" type="text" value="<%= Model.ValueType3 %>" readonly="readonly"
                    style="width: 30.833em;" />
            </td>
        </tr>

        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("RuleValue4", "Value 4")%>:
            </td>
            <td>
                 <input id="txtValue4" type="text" maxlength="200" value="<%= Model.Value4 %>"
                   monedaidioma='CultureIPN'  style="width: 10.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("RuleValueType4", "Value Type 4")%>:
            </td>
            <td>
                <input id="txtValueType4" type="text" value="<%= Model.ValueType4 %>" readonly="readonly"
                    style="width: 30.833em;" />
            </td>
        </tr>


        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                        <%= Html.Term("SaveRule", "Save Rule") %></a>
                </p>
            </td>
        </tr>
    </table>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#btnSave').click(function () {


                $('#ddlRuleTypes').clearError();
                if ($('#ddlRuleTypes').val() == "" || eval($('#ddlRuleTypes').val()) < 1) {
                    $('#ddlRuleTypes').showError('<%= Html.JavascriptTerm("RuleTypeIsRequired", "Rule Type is Required") %>');
                    return false;
                }

                $('#ddlPlans').clearError();
                if ($('#ddlPlans').val() == "" || eval($('#ddlPlans').val()) < 1) {
                    $('#ddlPlans').showError('<%= Html.JavascriptTerm("PlanIsRequired", "Plan is Required") %>');
                    return false;
                }


                $('#txtValue1').clearError();
                if ($.trim( $('#txtValue1').val()) == "") {
                    $('#txtValue1').showError('<%= Html.JavascriptTerm("Value1IsRequired", "Value 1 is Required") %>');
                    return false;
                }

                if ($('#RuleForm').checkRequiredFields()) {
                    var data = {
                        RuleRequirementID: $('#hddRuleId').val(),
                        RuleTypeID: $('#ddlRuleTypes').val(),
                        PlanID:$.trim( $('#ddlPlans').val()),
                        Value1: $.trim($('#txtValue1').val()),
                        Value2: $.trim($('#txtValue2').val()),
                        Value3: $.trim($('#txtValue3').val()),
                        Value4: $.trim($('#txtValue4').val())
                    };

                    $.post('<%= ResolveUrl("~/Commissions/Configurations/SaveRule") %>', data, function (response) {
                        if (response.result) {
                            showMessage('<%=@Html.Term("RuleSavedSuccessfully", "Rule Saved Successfully")%>', false);
                            if (!$('#hddRuleId').val()) { // Create case
                                $('#hddRuleId').val(response.Id);
                                // Reload Edit Mode 
                                //window.location.replace('<%= ResolveUrl("~/Commissions/Configurations/EditTitle") %>' + "/" + response.TitleId);
                            }
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            });

            $('#ddlRuleTypes').change(function () {

                $.get('<%= ResolveUrl("~/Commissions/Configurations/GetRuleTypeByID") %>', { id: $('#ddlRuleTypes').val() }, function (response) {
                    if (response.result) {
                        
                            $('#txtDescription').val(response.data.Description);
                            $('#txtValueType1').val(response.data.ValueType1);
                            $('#txtValueType2').val(response.data.ValueType2);
                            $('#txtValueType3').val(response.data.ValueType3);
                            $('#txtValueType4').val(response.data.ValueType4);

                        
                    } else {
                        showMessage(response.message, true);
                    }
                });

            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
