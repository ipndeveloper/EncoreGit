<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master" 
      Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

 <div class="SectionHeader">
        <h2>
            <%= Html.Term("AlertTemplates", "Alert Templates") %>
        </h2>
        <a href="<%= ResolveUrl("~/Communication/Alerts/Edit") %>">
            <%= Html.Term("AddNewAlertTemplate", "Add a new alert template") %>
        </a>
 </div>

    <% Html.PaginatedGrid("~/Communication/Alerts/GetAlerts")
           .AddColumn(Html.Term("Name"), "Name", true, true, NetSteps.Common.Constants.SortDirection.Ascending)
           //.AddColumn(Html.Term("Schedule"), "Schedule", true)
           //.AddColumn(Html.Term("Market"), "Market", true)
           //.AddColumn(Html.Term("Language"), "Language", true)
           //.AddColumn(Html.Term("Receivers"), "Receivers", true)
           .AddColumn(Html.Term("Status"), "Active", true)
           .AddSelectFilter(Html.Term("Status"), "active", new Dictionary<string, string>() { { "", Html.Term("All") }, { "true", Html.Term("Active") }, { "false", Html.Term("Inactive") } })
           .AddSelectFilter(Html.Term("AlertPriority"), "AlertPriorityID", new Dictionary<string, string>() { { "", Html.Term("SelectanAlertTemplatePriority", "Select a Priority...") }}.AddRange(SmallCollectionCache.Instance.AlertPriorities.ToDictionary(os => os.AlertPriorityID.ToString(), os=> os.GetTerm())))
           .Render(); %>
</asp:Content>