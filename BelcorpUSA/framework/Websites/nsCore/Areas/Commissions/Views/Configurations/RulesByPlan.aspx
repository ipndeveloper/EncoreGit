<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" 
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.RequirementRule>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="SectionHeader">
		<h2>
			<%= Html.Term("BrowseRulesByPlan", "Browse Rules by Plan") %>
        </h2>
        <a href="<%= ResolveUrl("~/Commissions/Configurations/EditRule") %>"><%= Html.Term("CreateaNewRule", "Create a New Rule") %></a>
	</div>



        <%      Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.RequirementRuleSearchData>("~/Commissions/Configurations/GetRules")
            .AutoGenerateColumns()
            .AddSelectFilter(Html.Term("RuleType"), "RuleTypeID", new Dictionary<string, string>() { { "", Html.Term("SelectaRuleType", "Select a Rule Type...") } }.AddRange(TempData["RuleTypes"] as Dictionary<string, string>))
            .AddSelectFilter(Html.Term("Plans"), "PlanID", new Dictionary<string, string>() { { "", Html.Term("SelectaPlan", "Select a Plan...") } }.AddRange(TempData["Plans"] as Dictionary<string, string>))
            .AddInputFilter(Html.Term("Description"), "Description")
            .Render(); %>  

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
    $(function () {


        $(document).on('click', '.clsDescription', function (event) {
            event.preventDefault();

            $("#DescriptionInputFilter").val($(this).text());

        });


    });
</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
