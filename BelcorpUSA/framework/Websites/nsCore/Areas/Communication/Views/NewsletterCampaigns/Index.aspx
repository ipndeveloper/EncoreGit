<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master"
    Inherits="System.Web.Mvc.ViewPage<List<NetSteps.Data.Entities.Campaign>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("NewsletterCampaigns", "Newsletter Campaigns")%></h2>
        <a href="<%= ResolveUrl("~/Communication/NewsletterCampaigns/Create") %>">
            <%= Html.Term("AddaNewNewsletterCampaign", "Add a new Newsletter Campaign")%></a>
    </div>

    <% var unsupportedEmailTemplateTypes = new List<short>() { (short)Constants.EmailTemplateType.Autoresponder, (short)Constants.EmailTemplateType.Campaign };
       Html.PaginatedGrid("~/Communication/NewsletterCampaigns/GetNewsletters")
           .AddColumn(Html.Term("Name", "Name"), "Name", true, true, Constants.SortDirection.Ascending)
           .AddColumn(Html.Term("NextNewsletterSendDateTime", "Next Newsletter Send Date/Time"), "NextRunDateUTC", true)
           .AddColumn(Html.Term("Market"), "MarketID", true)
           .AddColumn(Html.Term("Active"), "Active", true)
           .AddSelectFilter(Html.Term("Market"), "marketID", new Dictionary<string, string>() { { "", Html.Term("All") } }.AddRange(SmallCollectionCache.Instance.Markets.Where(m => m.Active).ToDictionary(m => m.MarketID.ToString(), m => m.GetTerm())))           
           .Render(); %>   
    
</asp:Content>
