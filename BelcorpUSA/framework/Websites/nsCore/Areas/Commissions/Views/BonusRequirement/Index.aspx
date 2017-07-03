<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master"%>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">    
    <div class="SectionNav">
        <ul class="SectionLinks">
            <%= Html.SelectedLink("~/Commissions/BonusRequirement/", Html.Term("Browse", "Browse Commission Rules"), LinkSelectionType.ActualPage, CoreContext.CurrentUser, "")%>
        </ul>
    </div>    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
   <a href="<%= ResolveUrl("~/Orders") %>">
        <%= Html.Term("Commissions") %></a> >
    <%= Html.Term("CommissionsRules", "Commission Rules") %>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
   <script type="text/javascript">
       $(document).ready(function () {
//            $('#planIdSelectFilter').attr('class', 'FilterChanged AutoPost');            
           
            $('#planIdSelectFilter').change(function (event) {
                $('#bonusTypeIdSelectFilter').hide();
                var data = { planId: $('#planIdSelectFilter').val() };
                var URL = '/Commissions/BonusRequirement/BonusTypeByPlan';

                $.post(URL, data, function (response) {
                    var items = '<option value="0"><%=Html.Term("SelectaCommissionRuleType", "Select a CommissionRuleType...") %></option>';
                    $.each(response, function (i, category) {

                        items += "<option value='" + i + "'>" + category + "</option>";
                    });
                    $('#bonusTypeIdSelectFilter').html(items);
                    $('#bonusTypeIdSelectFilter').show();
                });
            });

        });
	</script>
</asp:Content>
 
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <div class="SectionHeader">
        <h2>
            <%=  Html.Term("CommissionsBrowse", "Browse Commission Rules")%>
        </h2>
        <div class ="BreadCrumb">
            <a href = "/Commissions/BonusRequirement/New"> <%=  Html.Term("BonusRequirementNew", "Create new Commission Rule") %> </a>
        </div>
    </div>
   
    <%
        if (TempData["Error"] != null)
        { %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous; -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0; border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold; margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;" class="ui-icon icon-exclamation">
            <%= TempData["Error"].ToString() %></div>
    </div>
    <% } %>

    <% Html.PaginatedGrid<BonusRequirement>("~/Commissions/BonusRequirement/GetByFilters")
        .AddColumn(Html.Term("BonusRequirementId", "ID"), "BonusRequirementId")
        .AddColumn(Html.Term("BonusTypeId", "Commission Type"), "BonusTypeId")
        .AddColumn(Html.Term("BonusTypeName", "Name"), "BonusTypeName")
        .AddColumn(Html.Term("PlanName", "Plan Name"), "PlanName")
        .AddColumn(Html.Term("BonusAmount", "Amount"), "BonusAmount")
        .AddColumn(Html.Term("BonusPercent", "Percent"), "BonusPercent")
        .AddColumn(Html.Term("BonusMaxAmount", "Max Amount"), "BonusMaxAmount")
        .AddColumn(Html.Term("BonusMaxPercent", "Max Percent"), "BonusMaxPercent") 
        .AddColumn(Html.Term("MinTitleId", "Minimum Title ID"), "MinTitleId")
        .AddColumn(Html.Term("MaxTitleId", "Maximun Title ID"), "MinTitleId")
        .AddColumn(Html.Term("BonusMinAmount", "Min Amount"), "BonusMinAmount")
        .AddColumn(Html.Term("PayMonth", "Pay Month"), "PayMonth")
        .AddColumn(Html.Term("EffectiveDate", "Effective Date"), "EffectiveDate")
        .AddSelectFilter(Html.Term("Plan"), "planId", new Dictionary<string, string>() { { "0", Html.Term("SelectaPlan", "Select a Plan...") } }.AddRange(SmallCollectionCache.Instance.Plans.ToDictionary(p => p.PlanId.ToString(), p => p.Name)), startingValue: 1)
        .AddSelectFilter(Html.Term("BonusType"), "bonusTypeId", new Dictionary<string, string>() { { "0", Html.Term("SelectaCommissionRuleType", "Select a CommissionRuleType...") } }, startingValue: 1)
        .CanDelete("~/Commissions/BonusRequirement/Delete")
        .ClickEntireRow()
        .Render(); %>

    <div>
        <input type="hidden" name="selectaCommissionRuleTypeHidden" value="message" />
    </div>
</asp:Content>
