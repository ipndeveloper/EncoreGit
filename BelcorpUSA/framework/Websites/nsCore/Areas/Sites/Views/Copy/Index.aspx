<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            var checkIfUrlChanged = function () {
                if ($('#subdomain').val() == $('#subdomain').data('subdomain') && $('domain').data('domain') && $('#domain').val() == $('#domain').data('domain')) {
                    $('#btnCheckAvailability').hide();
                } else {
                    $('#btnCheckAvailability').show();
                }
                $('#availability').empty();
            }

            $('#subdomain').keyup(checkIfUrlChanged).data('subdomain', '');
            $('#domain').change(checkIfUrlChanged).data('domain', '');

            $('#btnCheckAvailability').click(function () {
                $.get('<%= ResolveUrl("~/Sites/Edit/CheckIfUrlAvailable") %>', { url: 'http://' + $('#subdomain').val() }, function (theResponse) {
                    if (theResponse.result) {
                        var img;
                        if (theResponse.available) {
                            img = '<span class="UI-icon icon-check"></span>';
                            $('#subdomain,#domain').clearError();
                        } else {
                            img = '<span class="UI-icon icon-exclamation"></span>';
                            if ($('#domain').is('select')) {
                                $('#subdomain,#domain').showError('');
                            } else {
                                $('#subdomain').showError('');
                            }
                            showMessage('<%= Html.Term("URLNotAvailable", "That URL is not available, please try a different one") %>', true);
                        }
                        $('#btnCheckAvailability').hide();
                        $('#availability').html(img);
                    } else {
                        showMessage(theResponse.message, true);
                    }
                });
            });

            $('#btnSave').click(function () {
                if (!$('#siteProperties').checkRequiredFields()) {
                    return false;
                }

                if ($('#subdomain').val() != $('#subdomain').data('subdomain') && $('#availability').is(':empty')) {
                    $('#subdomain').showError('');
                    showMessage('<%= Html.Term("MustCheckAvailability", "Please check if the url is available.") %>', true);
                    return false;
                }

                if ($('#availability img').length && $('#availability img').attr('title') == 'Not available') {
                    if ($('#domain').is('select')) {
                        $('#subdomain,#domain').showError('');
                    } else {
                        $('#subdomain').showError('');
                    }
                    showMessage('<%= Html.Term("URLNotAvailable", "That URL is not available, please try a different one") %>', true);
                    return false;
                }

                var data = {
                    siteId: $('#siteId').val(),
                    siteName: $('#siteName').val(),
                    description: $('#description').val(),
                    statusId: $('#statusId').val(),
                    subdomain: $('#subdomain').val(),
                    marketId: $('#sMarket').val(),
                    defaultLanguageId: $('#sLanguage').val()
                };

                var t = $(this);
                showLoading(t);
                $.post('<%= ResolveUrl("~/Sites/Copy/Save") %>', data, function (response) {
                    showMessage(response.message || 'Site saved successfully!', !response.result);
                    hideLoading(t);
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Sites") %>">
        <%= Html.Term("Sites") %></a> >
    <%= Html.Term("Pages") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("CopySite", "Copy Pages") %></h2>
    </div>
    <%if (TempData["Error"] != null)
      { %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
        -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
        border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
        margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;" class="UI-icon icon-exclamation">
            <%= TempData["Error"] %></div>
    </div>
    <%} %>
    <%--<input type="hidden" id="siteId" name="siteId" value="<%= Model.SiteID == 0 ? "" : Model.SiteID.ToString() %>" />--%>
    <table id="siteProperties" width="100%" cellspacing="0" class="DataGrid">
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Copy From Site")%>:
            </td>
            <td>
                <%= Html.DropDownBaseSites(baseSites: ViewData["Sites"] as List<Site>, htmlAttributes: new { id = "siteId" })%>
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Site Name")%>:
            </td>
            <td>
                <input id="siteName" type="text" class="required" style="width: 15.167em;" name="<%= Html.Term("SiteNameRequired", "Site Name is required.") %>" />
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Description")%>:
            </td>
            <td>
                <textarea id="description" name="description" cols="20" rows="5"></textarea>
            </td>
        </tr>        
        <tr>
            <td>
                <%= Html.Term("URL")%>:
            </td>
            <td>
                http://<input id="subdomain" />
                <a href="javascript:void(0);" id="btnCheckAvailability">
                    <%= Html.Term("CheckAvailability", "Check availability")%></a><span id="availability"
                        style="margin-left: .455em;"></span>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Status")%>:
            </td>
            <td>
                <select id="statusId" name="statusId">
                    <%foreach (var siteStatus in SmallCollectionCache.Instance.SiteStatuses)
                      { %>
                    <option value="<%= siteStatus.SiteStatusID %>">
                        <%= siteStatus.GetTerm()%></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Market")%>:
            </td>
            <td>
                <%= Html.DropDownMarkets(htmlAttributes: new { id = "sMarket" })%>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("DefaultLanguage", "Default Language")%>:
            </td>
            <td>
                <%= Html.DropDownLanguages(errorMessage: "sLanguage", htmlAttributes: new { id = "sLanguage", style = "margin-bottom: .833em;" })%>
            </td>
        </tr>
    </table>
    <p>
        <a id="btnSave" href="javascript:void(0);" class="Button BigBlue">
            <%= Html.Term("Save") %></a>
        <a href="<%= ResolveUrl("~/Sites/Overview") %>" class="Button">
            <%= Html.Term("Cancel") %></a>
    </p>
    <div id="languageErrorModal" class="LModal jqmWindow">
        <div class="mContent">
            <div>
                <p>
                    <label>
                        <%: Html.Term("LanguageErrorDisplay", "A language must be selected from the checkbox.") %>
                    </label>
                </p>
                <p>
                    <a id="btnLanguageError" href="javascript:void(0);">
                        <%: Html.Term("OK") %></a>
                </p>
            </div>
        </div>
    </div>
</asp:Content>
