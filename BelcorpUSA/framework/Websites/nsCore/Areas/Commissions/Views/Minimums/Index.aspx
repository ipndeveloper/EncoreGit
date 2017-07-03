
<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Disbursements.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Minimums")%>
        </h2>
        <a href="<%= ResolveUrl("~/Commissions/Minimums/Edit") %>" class="btnViewStats">
            <%= Html.Term("AddNewMinimum", "Add new minimum")%></a>
    </div>

    <%    
        Html.PaginatedGrid("~/Commissions/Minimums/Get")
        .AddColumn(Html.Term("ID", "ID"), "DisbursementMinimumID", false)
        .AddColumn(Html.Term("DisbursementType", "Disbursement Type"), "DisbursementType", false)
        .AddColumn(Html.Term("Currency"), "CurrencyName", false)
        .AddColumn(Html.Term("MinimumAmount", "Minimum Amount"), "MinimumAmount", false)
        .CanChangeStatus(true, false, "~/Commissions/Minimums/Delete")
        .Render(); 
    %>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<style type="text/css">
#paginatedGridOptions
{
    padding-bottom: 6px;
}
</style>
<script type="text/javascript">
    $(document).ready(function () {
        $(".deactivateButton span:eq(1)").text('<%= Html.Term("DeleteSelected", "Delete Selected") %>');
    });
</script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Commissions") %>">
        <%= Html.Term("MLM") %></a> >
    <%= Html.Term("Minimums")%>
</asp:Content>