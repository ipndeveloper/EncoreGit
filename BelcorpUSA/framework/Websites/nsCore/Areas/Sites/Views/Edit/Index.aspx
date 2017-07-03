<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master" Inherits="System.Web.Mvc.ViewPage<EditModel>" %>
<%@ Import Namespace="nsCore.Areas.Sites.Models" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script src="<%= Url.Content("~/Scripts/scrollsaver.js") %>" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#btnSave').click(function () {
                var t = $(this);
                showLoading(t);
                setTimeout("$('form').submit()", 100); // give spinner time to render before submiting the form
            });

            setTimeout("CheckForSuccess()", 200); // again, this needs something to render before it works correctly, sigh
        });

        function CheckForSuccess() {
            var successMessage = '<%= Model.SuccessMessage ?? string.Empty %>';
            if (successMessage != '') {
                showMessage(successMessage, false);
            }
        }

        function RemoveURL(indexForRemoval) {
            var t = $('#btnSave');
            showLoading(t);
            var data = $('form').serialize();
            data += '&indexForRemoval=' + indexForRemoval;
            $.post('<%= Url.Content("~/Sites/Edit/RemoveURL") %>', data, function (result) {
                $('#tdSiteURL').html(result);
                hideLoading(t);
            });
        }

        function AddAnotherURL() {
            var t = $('#btnSave');
            showLoading(t);
            var data = $('form').serialize();
            $.post('<%= Url.Content("~/Sites/Edit/AddAnotherURL") %>', data, function (result) {
                $('#tdSiteURL').html(result);
                hideLoading(t);
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Sites") %>"><%= Html.Term("Sites") %></a> > <%= Model.PageTitle %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.EnableClientValidation(false); %>
    <div class="SectionHeader">
        <h2><%= Model.PageTitle %></h2>
    </div>
    <% using (Html.BeginForm())
       { %>
    <%--NOTE: Don't use an html helper to render the the hidden SiteID. They bind to the post data regardless of changes made to the model in an action method--%>
    <input type="hidden" name="SiteID" value="<%= Model.SiteID %>" />
    <table id="siteProperties" width="100%" cellspacing="0" class="DataGrid">
        <tr>
            <td style="width: 13.636em;">
                <%= Html.LabelFor(m => m.SiteName)%>:
            </td>
            <td>
                <%= Html.TextBoxFor(m => m.SiteName, new { style = "width: 15.167em;" })%>
                <%= Html.ValidationMessageFor(m => m.SiteName)%>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.LabelFor(m => m.Description)%>:
            </td>
            <td>
                <%= Html.TextAreaFor(m => m.Description, new { cols = "20", rows = "5" })%>
                <%= Html.ValidationMessageFor(m => m.Description)%>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.LabelFor(m => m.StatusId)%>:
            </td>
            <td>
                <%= Html.DropDownListFor(m => m.StatusId, Model.SiteStatuses)%>
                <%= Html.ValidationMessageFor(m => m.StatusId)%>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.LabelFor(m => m.SiteUrls) %>
            </td>
            <td id="tdSiteURL">
                <% if (Model.AreURLsEditable)
                   {
                       Html.RenderPartial("EditSiteURLs", Model.SiteUrls);
                   }
                   else
                   {
                       for (int i = 0; i < Model.SiteUrls.Count; i++)
                       { %>
                           <%= Model.SiteUrls[i].FullURL %>
                           <% if (Model.SiteUrls[i].IsPrimaryUrl)
                              { %>
                              <span class="UI-icon icon-check" title="<%= Html.Term("Primary URL") %>"></span>
                           <% } %>
                           <%= Html.HiddenFor(m => m.SiteUrls[i].Domain) %>
                           <%= Html.HiddenFor(m => m.SiteUrls[i].Subdomain)%>
                           <%= Html.HiddenFor(m => m.SiteUrls[i].IsPrimaryUrl)%>
                           <br />
                    <% }
                   } %>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.LabelFor(m => m.MarketId)%>:
            </td>
            <td>
                <%= Html.DropDownMarkets(errorMessage: "MarketId", htmlAttributes: new { id = "MarketId", name = "MarketId" }, selectedMarketID: Model.MarketId)%>
                <%= Html.ValidationMessageFor(m => m.MarketId)%>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.LabelFor(m => m.DefaultLanguageId)%>:
            </td>
            <td>
                <%= Html.DropDownLanguages(errorMessage: "DefaultLanguageId", htmlAttributes: new { id = "DefaultLanguageId", style = "margin-bottom: .833em;" }, selectedLanguageID: Model.DefaultLanguageId)%>
                <%= Html.ValidationMessageFor(m => m.DefaultLanguageId)%>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.LabelFor(m => m.SiteLanguages)%>
            </td>
            <td>
                <%foreach (var language in Model.TranslatedLanguages)
                  { %>
                <input type="checkbox" id="SiteLanguages" name="SiteLanguages" value="<%= language.Key %>"
                    class="language" <%= Model.SiteLanguages.Contains(language.Key) ? "checked=\"checked\"" : "" %> /><%= language.Value%><br />
                <%} %>
                <%= Html.ValidationMessageFor(m => m.SiteLanguages)%>
            </td>
        </tr>
    </table>
    <p>
        <a id="btnSave" href="javascript:void(0);" class="Button BigBlue"><%= Html.Term("Save")%></a>
        <a href="<%= ResolveUrl("~/Sites/Overview") %>" class="Button"> <%= Html.Term("Cancel")%></a>
    </p>
    <% } %>
</asp:Content>
