<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Site>" %>
<% var isSubdomain = NetSteps.Common.Configuration.ConfigurationManager.GetAppSetting<bool>(NetSteps.Common.Configuration.ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true); %>
 
<script type="text/javascript">
    $(function () {
    	$('.subdomain').inputfilter({ allowedpattern: '[a-zA-Z0-9-]' });

        var checkAvailability = function () {
            var t = $(this);
            t.parent().find('.availability').empty();
            if (t.val() != t.data('originalValue')) {
                t.parent().find('.checkAvailability').show();
            } else {
                t.parent().find('.checkAvailability').hide();
            }
        }, domains = $('.domain');

        $('#siteUrls .subdomain').keyup(checkAvailability).each(function () {
            $(this).data('originalValue', $(this).val());
        });

        if (domains.length && domains[0].tagName.toLowerCase() == 'select') {
            domains.change(checkAvailability).each(function () {
                $(this).data('originalValue', $(this).val());
            });
        }

        $('#siteUrls .checkAvailability').click(function () {
            var urlLink = $(this), url = '';
            if (parseBool('<%= isSubdomain %>')) {
                url = 'http://' + urlLink.parent().find('.subdomain').val() + '.' + urlLink.parent().find('.domain').val();
            } else {
                url = 'http://www.' + urlLink.parent().find('.domain').val() + '/' + urlLink.parent().find('.subdomain').val();
            }
            $.getJSON('<%= ResolveUrl("~/Accounts/CheckIfAvailableUrl") %>', { url: url }, function (results) {
                var img = results.available ? '<span class="UI-icon icon-check" title="Available"></span>' : '<span class="UI-icon icon-exclamation" title="Not available"></span>';
                urlLink.hide().parent().find('.availability').html(img);
            });
        });

        $('#siteUrls .toggleCustomUrl').live('click', function () {
            var p = $(this).parent(), f = p.find('.fixedDomain'), c = p.find('.customUrl');
            $(this).prop('checked') && f.fadeOut('fast', function () { c.fadeIn('fast'); }) || c.fadeOut('fast', function () { f.fadeIn('fast'); });
        });

        $('#btnAddUrl').click(function () {

            // Ensure there are no duplicate URLs.
            if (!checkDuplicateUrlsForUser('<%= isSubdomain %>')) {
                showMessage('<%= Html.Term("URLsMustBeUnique", "URLs must be unique.") %>', true);
                return false;
            }

            // Don't allow adding new URL's if there is an empty url textbox
            if (checkForEmptyUrls()) {

                var domains = '<%= (ViewData["Domains"] as string[]).Join(",") %>'.split(','), builder = new StringBuilder(), i;
                builder.append('<div class="urlContainer"><span class="fixedDomain">http://');

                if (parseBool('<%= isSubdomain %>')) {
                    builder.append('<input class="subdomain required" type="text" value="" style="width: 8.333em;" name="<%= Html.Term("URLRequired", "URL is required") %>" />.');
                }

                if (domains.length == 1) {
                    builder.append(String.format('<span>{0}</span><input type="hidden" class="domain" value="{0}" />', domains[0]));
                }
                else {
                    builder.append('<select class="domain">');
                    for (i = 0; i < domains.length; i++) {
                        builder.append(String.format('<option value="{0}">{0}</option>', domains[i]));
                    }
                    builder.append('</select>');
                }

                if (!parseBool('<%= isSubdomain %>')) {
                    builder.append('/<input class="subdomain required" type="text" value="" style="width: 8.333em;" name="<%= Html.Term("URLRequired", "URL is required") %>" />.');
                }

                builder.append('</span><span class="customUrl" style="display: none;">http://<input class="url required" type="text" value="" style="width: 25em;" name="<%= Html.Term("URLRequired", "URL is required") %>" /></span>')
			.append('<input type="checkbox" class="toggleCustomUrl" />Use a custom url <a href="javascript:void(0);" class="DeleteUrl DTL Remove"><%: Html.Term("Remove", "Remove") %></a><a href="javascript:void(0);" class="checkAvailability" style="display: none; margin-left: .909em;">Check availability</a><span class="availability"></span></div>');

                $('#siteUrls').append(builder.toString());
            }
            else {
                showMessage('<%= Html.Term("URLCannotBeEmpty", "URL cannot be empty.") %>', true);
            }

        });

        $('#siteUrls .DeleteUrl').live('click', function () {
            $(this).parent().clearError();
            $(this).parent().remove();
        });

        // Remove errors when URL is modified.
        $('input.subdomain, input.url').live('click', function () {
            $(this).parent().parent().clearError();
        });
    });

</script>
<table class="FormTable Section" width="100%">
    <tr>
        <td class="FLabel">
            <%= Html.Term("SiteDetails", "Site Details") %>:
        </td>
        <td>
            <table id="siteDetails" class="DataGrid" width="100%">
                <tr>
                    <td style="width: 9.091em;">
                        <b>
                            <%= Html.Term("Name", "Name") %>: </b>
                    </td>
                    <td>
                        <input id="siteName" type="text" value="<%= string.IsNullOrEmpty(Model.Name) ? CoreContext.CurrentAccount.FullName + "'s Site" : Model.Name %>"
                            class="required" name="<%= Html.Term("SiteNameRequired", "Site Name is required.") %>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <%= Html.Term("Description", "Description") %>: </b>
                    </td>
                    <td>
                        <textarea id="siteDescription" cols="30" rows="4"><%= Model.Description%></textarea>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <%= Html.Term("Status", "Status") %>:</b>
                    </td>
                    <td>
                        <select id="siteStatusId">
                            <%foreach (SiteStatus status in SmallCollectionCache.Instance.SiteStatuses)
                              { %>
                            <option value="<%= status.SiteStatusID %>" <%= status.SiteStatusID == Model.SiteStatusID ? "selected=\"selected\"" : "" %>>
                                <%= status.GetTerm() %></option>
                            <%} %>
                        </select>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <%= Html.Term("DefaultLanguage", "Default Language") %>:</b>
                    </td>
                    <td>
                        <select id="siteDefaultLanguageId">
                            <%foreach (Language language in TermTranslation.GetLanguages(CoreContext.CurrentLanguageID))
                              { %>
                            <option value="<%= language.LanguageID %>" <%= language.LanguageID == Model.DefaultLanguageID ? "selected=\"selected\"" : "" %>>
                                <%= language.GetTerm() %></option>
                            <%} %>
                        </select>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table class="FormTable Section" width="100%">
    <tr>
        <td class="FLabel">
            <%= Html.Term("URLs", "URL(s)") %>:
        </td>
        <td>
            <div id="siteUrls">
                <%foreach (SiteUrl siteUrl in Model.SiteUrls)
                  {
                      string url = siteUrl.Url;
                      string authority = url.GetURLAuthority();
                      string domain = url.GetURLDomain();
                      string subdomain = isSubdomain ? url.GetURLSubdomain() : url.GetURLPath().Substring(1); %>
                <div class="urlContainer">
                    <input type="hidden" class="siteUrlId" value="<%= siteUrl.SiteUrlID %>" />
                    <span class="fixedDomain">http://
                        <%if (isSubdomain)
                          { %><input class="subdomain required" type="text" value="<%= subdomain %>" style="width: 8.333em;"
                              name="<%= Html.Term("URLRequired", "URL is required") %>" /><%}
                          else { Response.Write("www"); } %>.
                        <%string[] domains = ViewData["Domains"] as string[];
                          if (domains.Length == 1)
                          { %>
                        <span>
                            <%= domains[0]%></span>
                        <input type="hidden" class="domain" value="<%= domains[0] %>" />
                        <%}
                          else
                          {%>
                        <select class="domain">
                            <%foreach (string d in domains)
                              { %>
                            <option value="<%= d %>" <%= d == domain ? "selected=\"selected\"" : "" %>>
                                <%= d %></option>
                            <%}%>
                        </select>
                        <%} %>
                        <%if (!isSubdomain)
                          { %>/<input class="subdomain required" type="text" value="<%= subdomain %>" style="width: 8.333em;"
                              name="<%= Html.Term("URLRequired", "URL is required") %>" />
                        <%} %>
                    </span>
                    
                    <span class="customUrl" style="display: none;">
                        http://<input class="url required" type="text" value="<%= authority %>" style="width: 25em;" name="<%= Html.Term("URLRequired", "URL is required") %>" />
                    </span>

                    <input type="checkbox" class="toggleCustomUrl" /><%= Html.Term("UseaCustomUrl", "Use a custom url") %>
                    <a href="javascript:void(0);" class="checkAvailability" style="display: none; margin-left: .909em;">
                        <%= Html.Term("Checkavailability", "Check availability") %></a><span class="availability"></span>
                </div>
                <%} %>
            </div>
            <a id="btnAddUrl" href="javascript:void(0);" class="DTL Add">
                <%= Html.Term("AddAnotherUrl", "Add another url") %></a>

        </td>
    </tr>
</table>
