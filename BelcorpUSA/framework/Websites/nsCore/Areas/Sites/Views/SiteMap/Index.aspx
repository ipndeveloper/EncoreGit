<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/tree.min.js") %>"></script>
    <script type="text/javascript">
        $(function () {
            var reset = function (parentId) {
                $('#sDestination').val('ExistingPage');
                $('#internalLink').show();
                $('#txtLinkText,#sExistingPages,').val('');
                $('#url').show().text('');
                $('#txtUrl').hide().val('');
                $('#navId').val('0');
                $('#parentId').val(parentId || '0');
                $('#btnDelete,#btnDeactivate,#btnActivate').hide();
                $('#siteMapMessageCenter').messageCenter('clearAllMessages').messageCenter('addMessage', parentId ? 'You are adding a child link to <b>"' + $('#' + parentId + ' a:first').text() + '"</b>' : '<%: Html.JavascriptTerm("YouAreAddingATopLevelLink", "You are adding a top level link.") %>');
                //addingChild = false;
            }, changeStatus = function (active) {
                $.post('<%= ResolveUrl("~/Sites/SiteMap/ChangeStatus") %>', { navigationId: $('#navId').val(), isActive: active }, function (response) {
                    if (response.result) {
                        $('#btnDeactivate')[active ? 'show' : 'hide']();
                        $('#btnActivate')[active ? 'hide' : 'show']();
                    } else {
                        showMessage(response.message, true);
                    }
                });
            };
            $('#siteMapMessageCenter').messageCenter().messageCenter('addMessage', '<%: Html.JavascriptTerm("YouAreAddingATopLevelLink", "You are adding a top level link.") %>');
            $('#tree').tree({
                rules: {
                    multitree: true,
                    draggable: 'all',
                    createat: 'bottom'
                },
                ui: {
                    dots: true
                },
                callback: {
                    onmove: function (node, refNode, type) {
                        var parent = $(node).parents('li').length > 0 ? $(node).parents('li')[0] : null,
                            data = { parentId: parent != null ? parent.id : 0 },
                            nodes = $(node).parent().children('li');
                        for (var i = 0; i < nodes.length; i++) {
                            data['navigationIds[' + i + ']'] = nodes[i].id;
                        }
                        $.post('<%= ResolveUrl("~/Sites/SiteMap/Move") %>', data/*{ navigationId: $(node).attr('id'), parentId: parent != null ? parent.id : 0, newIndex: $(node).parent().children('li').index($(node)) + 1 }*/);
                    },
                    onselect: function (node, tree) {
                        node = $(node);
                        var navId = node.is('li') ? node.attr('id') : node.parent().attr('id');
                        if (node.hasClass('AddChild')) {
                            reset(navId);
                        } else {
                            $('#navId').val(navId);
                            $.getJSON('<%= ResolveUrl("~/Sites/SiteMap/Get") %>', { navigationId: navId, languageId: $('#languageId').val() }, function (response) {
                                if (response.result) {
                                    $('#siteMapMessageCenter').messageCenter('clearAllMessages').messageCenter('addMessage', '<%= Html.JavascriptTerm("youAreEditing","You are editing") %> <b>"' + response.linkText + '"</b>');
                                    $('#btnDelete').show();
                                    if (response.isInternal) {
                                        $('#sDestination').val('ExistingPage');
                                        $('#internalLink').show();
                                        $('#url').show().text(response.url);
                                        $('#txtUrl').hide();
                                        $('#sExistingPages option').removeAttr('selected');
                                        $('#page' + response.pageId).attr('selected', 'selected');
                                    } else {
                                        $('#sDestination').val('ExternalURL');
                                        $('#internalLink,#url').hide();
                                        $('#txtUrl').show().val(response.url);
                                    }
                                    $('#txtLinkText').val(response.linkText);
                                    $('#parentId').val(response.parentId);
                                    $('#isDropDown').prop('checked', response.isDropDown);
                                    $('#isSecondaryNav').prop('checked', response.isSecondaryNav);
                                    $('#isChildNavTree').prop('checked', response.isChildNavTree);
                                    if (response.active) {
                                        $('#btnDeactivate').show();
                                        $('#btnActivate').hide();
                                    } else {
                                        $('#btnDeactivate').hide();
                                        $('#btnActivate').show();
                                    }
                                } else
                                    showMessage(response.message, true);
                            });
                        }
                    }
                }
            }).addClass('ltr');
            $('#tree ul.ltr').removeClass('ltr');

            $('#sDestination').val('ExistingPage').change(function () {
                if ($(this).val() == 'ExistingPage') {
                    $('#internalLink').show();
                    $('#url').show().text($('#sExistingPages').val());
                    $('#txtUrl').hide();
                } else if ($(this).val() == 'ExternalURL') {
                    $('#internalLink,#url').hide();
                    $('#txtUrl').show().val('');
                }
            });

            $('#sExistingPages').val('').change(function () {
                $('#url').text($(this).val());
                $(this).clearError();
            });

            $('#url').text('');

            $('#btnSave').click(function () {
                if (!$.trim($('#txtLinkText').val())) {
                    $('#txtLinkText').showError('<%= Html.JavascriptTerm("PleaseEnterTheLinkText", "Please enter the link text.") %>').keyup(function () {
                        $(this).clearError();
                    });
                    return false;
                }
                if ($('#sDestination').val() == 'ExistingPage' && !$('#sExistingPages').val()) {
                    $('#sExistingPages').showError('<%= Html.JavascriptTerm("PleaseSelectaPage", "Please select a page.") %>');
                    return false;
                }
                var url = $('#sDestination').val() == 'ExistingPage' ? $('#url').text() : $('#txtUrl').val();
                var pageId = $('#sDestination').val() == 'ExistingPage' ? $('#sExistingPages option:selected').attr('id').replace(/\D/g, '') : '';
                $.post('<%= ResolveUrl("~/Sites/SiteMap/Save") %>', {
                    navigationId: $('#navId').val().length > 0 ? $('#navId').val() : 0,
                    destination: $('#sDestination').val(),
                    linkText: $('#txtLinkText').val(),
                    url: url,
                    pageId: pageId,
                    parentId: $('#parentId').val(),
                    languageId: $('#languageId').val(),
                    isDropDown: $('#isDropDown').prop('checked'),
                    isSecondaryNav: $('#isSecondaryNav').prop('checked'),
                    isChildNavTree: $('#isChildNavTree').prop('checked')
                }, function (response) {
                    if (response.result) {
                        if (!$('#navId').val() || $('#navId').val() == '0') {
                            var referenceList, newNodeClass = '';
                            if (!$('#parentId').val() || $('#parentId').val() == '0') {
                                referenceList = $('#tree').find('ul:first');
                                newNodeClass = 'leaf last';
                            } else {
                                referenceList = $('#' + $('#parentId').val()).find('ul:first');
                                if (referenceList.length == 0) {
                                    referenceList = $('<ul />');
                                    $('#' + $('#parentId').val()).append(referenceList);
                                } else {
                                    referenceList.children('.last').removeClass('last');
                                }
                                newNodeClass = 'leaf last';
                            }
                            $('#navId').val(response.navigationId);
                            var newNode = '<li id="' + response.navigationId + '" class="' + newNodeClass + '"><a href="javascript:void(0);">' + $('#txtLinkText').val() + '</a>&nbsp;<a href="javascript:void(0);" class="AddChild">+</a>';
                            referenceList.show().append(newNode);
                            $.tree.reference('tree').select_branch($('#' + response.navigationId));
                            $('#btnActivate').show();
                        } else {
                            $('#' + response.navigationId + ' a:first').text($('#txtLinkText').val());
                        }
                    } else
                        showMessage(response.message, true);
                });
            });

            $('#btnDelete').click(function () {
                if (confirm('<%= Html.Term("ThisWillDeleteAllTheChildLinksAsWellAreYouSure?", "This will delete all the child links as well. Are you sure?") %>')) {
                    $.post('<%= ResolveUrl("~/Sites/SiteMap/Delete") %>', { navigationId: $('#navId').val() }, function (response) {
                        if (response.result) {
                            var navNode = $('#' + $('#navId').val());

                            navNode.parents('li').each(function () {
                                if ($(this).find('li').length == 0) {
                                    if ($(this).is('.open,.closed')) {
                                        $(this).attr('class', 'leaf');
                                    }
                                    if ($(this).parent().index(this) == $(this).parent().children().length) {
                                        $(this).addClass('last');
                                    }
                                }
                            });
                            navNode.remove();
                            reset();
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            });

            $('#btnDeactivate').click(function () { changeStatus(false); });
            $('#btnActivate').click(function () { changeStatus(true); });

            $('#btnAddLink').click(function () {
                reset();
            });

            $('#navigationTypes').change(function () {
                window.location = (window.location.href.indexOf('?') >= 0 ? window.location.href.substring(0, window.location.href.indexOf('?')) : window.location.href) + '?navigationType=' + $('#navigationTypes').val();
            });

            $('#languageId').change(function () {
                $.get('<%= ResolveUrl("~/Sites/SiteMap/GetTranslation") %>', { navigationId: $('#navId').val(), languageId: $(this).val() }, function (response) {
                    if (response.result) {
                        $('#txtLinkText').val(response.linkText);
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Sites") %>">
        <%= Html.Term("Sites") %></a> >
    <%= Html.Term("SiteMap", "Site Map") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("SiteMap", "Site Map") %></h2>
        <a href="javascript:void(0);" id="btnAddLink" class="DTL Add mr10">
            <%= Html.Term("AddNewSiteMapLink", "Add a New Link") %></a>
        <%= Html.Term("Language") %>:
        <%= Html.DropDownLanguages(htmlAttributes: new { id = "languageId" }, selectedLanguageID: CoreContext.CurrentLanguageID)%>
    </div>
    <table width="100%" class="SiteMap">
        <tr>
            <td class="SiteMapLeft" style="width: 50%;">
                <!-- Tree Header -->
                <div class="UI-lightBg brdrAll pad10 mb10">
                    <span class="FL">
                        <label class="bold">
                            <%= Html.Term("SiteMapNavigationArea","Navigation Area") %>:</label>
                        <select id="navigationTypes">
                            <% foreach (NavigationType navigationType in ViewData["NavigationTypes"] as List<NavigationType>)
                               { %>
                            <option value="<%= navigationType.NavigationTypeID %>" <%= !string.IsNullOrEmpty(Request.Params["navigationType"]) && int.Parse(Request.Params["navigationType"]) == navigationType.NavigationTypeID ? " selected=\"selected\"" : "" %>>
                                <%= navigationType.Name == "Footer" ? Html.Term("Footer", "Footer") : Html.Term("Header", "Header")%></option>
                            <%} %>
                        </select>
                    </span>
                    <img class="Loading FR mr10" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>"
                        alt="loading..." style="display: none;" height="20" />
                    <span class="clr"></span>
                </div>
                <!--/ End Tree Header -->
                <div id="tree">
                    <%= ViewData["Links"] %>
                </div>
            </td>
            <td class="SiteMapRight" style="padding-left: .909em; width: 50%">
                <div class="UI-lightBg brdrAll pad10 mb10">
                    <div id="siteMapMessageCenter">
                    </div>
                </div>
                <div class="FormStyle1">
                    <p>
                        <span class="Label">
                            <%= Html.Term("Destination") %>:</span>
                        <select id="sDestination">
                            <option id="existing" value="ExistingPage">
                                <%= Html.Term("ExistingPage", "Existing Page") %></option>
                            <option id="external" value="ExternalURL">
                                <%= Html.Term("ExternalURL", "External URL") %></option>
                        </select>
                    </p>
                    <p>
                        <span class="FL Label">
                            <%= Html.Term("LinkText", "Link Text") %>:</span> <span class="FR lawyer">
                                <%=Html.Term("HTMLisAllowed","HTML is allowed. Use with discretion") %></span>
                        <span class="clr"></span>
                        <input type="text" id="txtLinkText" class="fullWidth pad5 linkText" />
                    </p>
                    <p>
                        <span class="Label">
                            <%= Html.Term("URL", "URL") %>:</span> <span id="url" class="pad5 block bold">
                        </span>
                        <input type="text" id="txtUrl" class="fullWidth pad5 urlText" style="display: none;" />
                    </p>
                    <div id="internalLink">
                        <p id="existingPages">
                            <span class="Label">
                                <%= Html.Term("Pages") %>:</span>
                            <select id="sExistingPages">
                                <option value="">
                                    <%= Html.Term("PleaseSelectaPage", "Please select a page") %></option>
                                <%foreach (NetSteps.Data.Entities.Page page in ViewData["Pages"] as IEnumerable<NetSteps.Data.Entities.Page>)
                                  {
                                      if (!String.IsNullOrEmpty(page.Url)) { %>
                                        <option id="page<%= page.PageID %>" value="<%= page.Url %>"><%= page.Name%></option>
                                      <% } %>
                                <% } %>
                            </select>
                        </p>
                    </div>
                    <div id="ParentLinkOptions">
                        <p class="UI-secBg pad5 brdrAll description">
                            <%= Html.Term("MenuBuildingOptions", "Menu-building options")%>
                        </p>
                        <p>
                            <input type="checkbox" id="isDropDown" class="FL mr10" />
                            <span class="FL Label">
                                <%= Html.Term("NestChildLinksInMainNavigation", "Nest Child Links in Navigation") %><br />
                                <span class="lawyer"><%= Html.Term("UsefulFlyouMenusSiteMenuFooter", "Useful for flyout menus, or building a full site menu in the footer.")%></span>
                            </span><span class="clr"></span>
                        </p>
                        <p>
                            <input type="checkbox" id="isSecondaryNav" class="FL mr10" />
                            <span class="FL Label">
                                <%= Html.Term("DisplaySecondaryNavigationOnChildPages", "Display Secondary Navigation on Sub-pages") %></span>
                            <span class="clr"></span>
                        </p>
                        <p>
                            <input type="checkbox" id="isChildNavTree" class="FL mr10" />
                            <span class="FL Label">
                                <%= Html.Term("DisplayChildNavigationInMenu", "Display Child Navigation in menu dropdown")%></span>
                            <span class="clr"></span>
                        </p>
                    </div>
                    <hr />
                    <div>
                        <input type="hidden" id="navId" value="0" />
                        <input type="hidden" id="parentId" value="0" />
                        <a href="javascript:void(0);" id="btnSave" class="FL Button BigBlue">
                            <%= Html.Term("SaveLink", "Save Link") %></a>
                        <div class="FL">
                            <a href="javascript:void(0);" id="btnDeactivate" class="FL mr10 Button BtnDeactivate"
                                style="display: none;">
                                <%= Html.Term("Deactivate") %></a> <a href="javascript:void(0);" id="btnActivate"
                                    class="FL mr10 Button BtnActivate" style="display: none;">
                                    <%= Html.Term("Activate") %></a> <a href="javascript:void(0);" id="btnDelete" class="FR Button BtnDelete"
                                        style="display: none;">
                                        <%= Html.Term("DeleteLink", "Delete Link") %></a> <span class="clr">
                            </span>
                        </div>
                        <img class="Loading FL ml10" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>"
                            alt="loading..." style="display: none;" height="26" />
                        <span class="clr"></span>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
