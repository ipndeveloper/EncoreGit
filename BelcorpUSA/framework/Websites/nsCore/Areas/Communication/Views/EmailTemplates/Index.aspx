<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("EmailTemplates", "Email Templates") %></h2>
        <a href="<%= ResolveUrl("~/Communication/EmailTemplates/Edit") %>">
            <%= Html.Term("AddNewEmailTemplate", "Add a new email template") %></a>
    </div>
    <% var unsupportedEmailTemplateTypes = new List<short>() { (short)Constants.EmailTemplateType.Autoresponder, (short)Constants.EmailTemplateType.Campaign, (short)Constants.EmailTemplateType.Newsletter};
       Html.PaginatedGrid("~/Communication/EmailTemplates/GetEmailTemplates")
           .AddColumn(Html.Term("Name"), "Name", true, true, Constants.SortDirection.Ascending)
           .AddColumn(Html.Term("Subject"), "Subject", true)
           .AddColumn(Html.Term("Type"), "EmailTemplateType.TermName", true)
           .AddColumn(Html.Term("Status"), "Active", true)
           .CanChangeStatus(true, true, "~/Communication/EmailTemplates/ChangeStatus")
           .AddSelectFilter(Html.Term("Status"), "active", new Dictionary<string, string>() { { "", Html.Term("All") }, { "true", Html.Term("Active") }, { "false", Html.Term("Inactive") } })
           .AddSelectFilter(Html.Term("Type"), "type", new Dictionary<string, string>() { { "", Html.Term("All") } }.AddRange(SmallCollectionCache.Instance.EmailTemplateTypes.Where(ett => !unsupportedEmailTemplateTypes.Contains(ett.EmailTemplateTypeID)).ToDictionary(ett => ett.EmailTemplateTypeID.ToString(), ett => ett.GetTerm())))
           .Render(); %>
</asp:Content>

