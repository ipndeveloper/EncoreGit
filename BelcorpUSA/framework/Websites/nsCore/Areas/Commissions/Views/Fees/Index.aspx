
<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Disbursements.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Fees")%>
        </h2>
        <a href="<%= ResolveUrl("~/Commissions/Fees/Edit") %>" class="btnViewStats">
            <%= Html.Term("AddNewFee", "Add new fee")%></a>
    </div>

    <%    
        Html.PaginatedGrid("~/Commissions/Fees/Get")
        .AddColumn(Html.Term("ID", "ID"), "DisbursementFeeID", false)
        .AddColumn(Html.Term("FeeType", "Fee Type"), "TypeFee", false)
        .AddColumn(Html.Term("DisbursementType", "Disbursement Type"), "TypeDisbursement", false)
        .AddColumn(Html.Term("Currency"), "CurrencyName", false)
        .AddColumn(Html.Term("Amount"), "Amount", false)
        .CanChangeStatus(true, false, "~/Commissions/Fees/Delete")
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
    <%= Html.Term("Fees")%>
</asp:Content>
