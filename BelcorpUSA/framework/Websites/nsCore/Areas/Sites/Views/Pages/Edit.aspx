<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Page>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {


            $('#pageProperties').setupRequiredFields();
            UpdatePageTypeLayouts();

            if ($('#pageId').val() > 0) {


                $('#layoutId').change(function () {
                    $(this).parent().find('p').remove();
                    if ($(this).val() != '<%= Model.LayoutID %>') {
                        $(this).after('<p style="color:red;width:18.182em;">Note: if you change your layout after this page has been created, you will lose all of your content.</p>');
                    }
                });
            }

            $('#pageTypeId').change(function () {
                UpdatePageTypeLayouts();
            });

            function UpdatePageTypeLayouts() {
                var pageTypeCode = $.trim($('#pageTypeId option:selected').text());

                if (pageTypeCode == 'External') {
                    $('#externalUrlTableRow').show();
                } else {
                    $('#externalUrlTableRow').hide();
                    $('#externalUrl').val('');
                }

                $.get('/Sites/Pages/GetPageTypeLayouts/', { id: $('#pageTypeId').val() }, function (response) {
                    if (response.success) {
                        $('#layoutId >option').remove();

                        $.each(response.data, function () {
                            $('#layoutId').append($("<option />").val(this.LayoutID).text(this.Name));
                        });
                        $('#layoutId').val($('#selectedLayoutId').val()).change();
                    } else {
                        showMessage(response.message, true);
                    }

                });
            }


            $('#btnAddKeyword').click(function () {
                $('td#keywords').prepend('<div class="mb10 keyWordWrapper"><input type="text" class="FL keywords pad5 fullWidth" value=""  /><a href="javascript:void(0);" class="deleteKeyword"><span class="UI-icon icon-deleteItem"></span></a><span class="clr"></span></div>');
                $('div.keyWordWrapper input:first').effect('highlight', {}, 3000).focus();
            });



            $('.deleteKeyword').live('click', function () {
                $(this).parent().fadeOut();
            });

            $('#btnSavePage').click(function () {
                if ($('#pageProperties').checkRequiredFields()) {
                    var data = {
                        pageId: $('#pageId').val(),
                        name: $('#name').val(),
                        languageId: $('#languageId').val(),
                        title: $('#title').val(),
                        description: $('#description').val(),
                        url: $('#url').val(),
                        externalUrl: $('#externalUrl').val(),
                        active: $('#active').prop('checked'),
                        layoutId: $('#layoutId').val()
                    }, keywords = [];

                    $('#keywords .keywords').filter(function () { return !!$(this).val(); }).each(function (i) {
                        keywords.push($(this).val());
                    });

                    data.keywords = keywords.join(',');

                    $.post('<%= ResolveUrl("~/Sites/Pages/Save") %>', data, function (response) {
                        showMessage(response.message || 'Page saved successfully!', !response.result);
                        if (response.result)
                            $('#pageId').val(response.pageId);
                    });
                }
            });

            $('#languageId').change(function () {
                $.get('<%= ResolveUrl("~/Sites/Pages/GetTranslation") %>', { pageId: $('#pageID').val(), languageId: $(this).val() }, function (response) {
                    if (response.result) {
                        $('#pageTitle').val(response.title);
                        $('#pageDesc').val(response.description);
                        $('#keywords').children('span').remove();
                        $.each(response.keywords.split(','), function (i, keyword) {
                            if (keyword)
                                $('td#keywords div.first').after('<div class="mb10 keyWordWrapper"><input type="text" value="' + keyword + '" class="FL keywords pad5 fullWidth" /><a href="javascript:void(0);" class="deleteKeyword"><span class="UI-icon icon-deleteItem"></span></a><span class="clr"></span></div>');
                        });
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Sites") %>">
        <%= Html.Term("Sites")%></a> > <a href="<%= ResolveUrl("~/Sites/Pages") %>">
            <%= Html.Term("Pages")%></a> >
    <%= Model.PageID == 0 ? Html.Term("NewPage", "New Page") : Html.Term("EditPage", "Edit Page") %>
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <form action="<%= ResolveUrl("~/Sites/Pages/Save") %>" method="post">
    <div class="SectionHeader">
        <h2>
            <%= Model.PageID == 0 ? Html.Term("NewPage", "New Page") : Html.Term("EditPage", "Edit Page")%></h2>
        <a href="<%= Page.ResolveUrl("~/Sites/Pages/Edit") %>" class="mr10 DTL Add">
            <%= Html.Term("AddaNewPage", "Add a new page") %></a>
        <%= Html.Term("Language")%>:
        <%= Html.DropDownLanguages(errorMessage: "languageId", htmlAttributes: new { id = "languageId" }, selectedLanguageID: CoreContext.CurrentLanguageID)%>
    </div>
    <%if (TempData["Error"] != null)
      { %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
        -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
        border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
        margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;" class="UI-icon icon-exclamation"><%= TempData["Error"] %></div>
    </div>
    <%} %>
    <% var ld = Model.Translations.GetByLanguageIdOrDefault(CoreContext.CurrentLanguageID);%>
    <input type="hidden" id="pageId" name="pageId" value="<%= Model.PageID == 0 ? "" : Model.PageID.ToString() %>" />
    <table id="pageProperties" width="60%" cellspacing="0" class="DataGrid">
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Name")%>:
            </td>
            <td>
                <input type="text" id="name" class="required pad5 fullWidth" value="<%= Model.Name %>"
                    name="<%= Html.Term("Nameisrequired", "Name is required") %>." />
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Title")%>:
            </td>
            <td>
                <input type="text" id="title" name="pageTitle" class="pad5 fullWidth" value="<%= ld.Title %>" />
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Description")%>:&nbsp;
            </td>
            <td>
                <textarea id="description" style="height: 8.333em;" class="pad5 fullWidth"><%= ld.Description %></textarea>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("URLPath","Url Path")%>:
            </td>
            <td>
            <% var readOnly = (Model.PageTypeID == (int)ConstantsGenerated.PageType.User ||
                               Model.PageTypeID == (int)ConstantsGenerated.PageType.External) ? "" : "readonly=\"readonly\""; %>
                
                <input type="text" id="url" class="pad5 fullWidth required" <%= readOnly  %>
                    value="<%= (Model.Url == null) ? string.Empty : Model.Url.TrimStart('/') %>"
                    name="<%= Html.Term("PageUrlisrequired","Page Url is required") %>." />
            </td>
        </tr>
        <tr id="externalUrlTableRow">
            <td>
                <%= Html.Term("ExternalURLPath","External Url Path")%>:
            </td>
            <td>
                <input type="text" id="externalUrl" class="pad5 fullWidth" <%= readOnly  %>
                    value="<%= (Model.ExternalUrl == null) ? string.Empty : Model.ExternalUrl.TrimStart('/') %>" />
            </td>
        </tr>
        <tr>
            <td>
                <label for="active">
                    <%= Html.Term("Active")%></label>:
            </td>
            <td>
                <input id="active" type="checkbox" <%= Model.Active ? " checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Keywords")%>:
                <p>
                    <a href="javascript:void(0);" id="btnAddKeyword" class="UI-icon-container">
                        <span class="UI-icon icon-plusAlt"></span>
                        <%= Html.Term("AddNewKeyword", "Add new keyword")%></a>
                </p>
            </td>
            <td id="keywords">
                <% if (ld != null && ld.Keywords != null)
                   {
                       foreach (string keyword in ld.Keywords.Split(','))
                       { %>
                <div class="mb10 first keyWordWrapper">
                    <input type="text" value="<%= keyword %>" class="FL keywords pad5 fullWidth mr10" />
                    <a href="javascript:void(0);" class="FL deleteKeyword">
                        <span class="UI-icon icon-deleteItem"></span></a>
                    <span class="clr"></span>
                </div>
                <%}
                   } %>
            </td>
        </tr>
        <% 
            if (ViewBag.Layouts != null && ViewBag.Layouts.Count > 0 && ViewBag.PageTypes != null && ViewBag.PageTypes.Count > 0)
            {
        %>
        <tr>
            <td>
                <%= Html.Term("PageTypes", "Page Types")%>:
            </td>
            <td>
                <select id="pageTypeId" name="<%= Html.Term("PageTypeisrequired", "Page Type is required") %>."
                    class="required">
                    <% foreach (var pageType in ViewBag.PageTypes)
                       { %>
                    <option value="<%= pageType.PageTypeID %>" <%= pageType.PageTypeID == Model.PageTypeID ? "selected=\"selected\"" : "" %>>
                        <%= pageType.Name%></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <%
            }
        %>
        <% 
            if (ViewBag.Layouts != null && ViewBag.Layouts.Count > 0)
            {
        %>
        <tr>
            <td>
                <%= Html.Term("Layout")%>:
            </td>
            <td>
                <select id="layoutId" name="<%= Html.Term("Layoutisrequired", "Layout is required") %>."
                    class="required">
                    <% foreach (var layout in ViewBag.Layouts)
                       { %>
                    <option value="<%= layout.LayoutID %>" <%= layout.LayoutID == Model.LayoutID ? "selected=\"selected\"" : "" %>>
                        <%= layout.Name%></option>
                    <%} %>
                </select>
                <input id="selectedLayoutId" type="hidden" value="<%= Model.LayoutID %>" />
            </td>
        </tr>
        <%
            }
        %>
    </table>
    <% 
        if (ViewBag.Layouts != null && ViewBag.Layouts.Count == 0)
        { 
    %>
    <input id="layoutId" type="hidden" value="<%= Model.LayoutID %>" />
    <%
        }
    %>
    <div class="mt10">
        <p class="SubmitPage">
            <a href="javascript:void(0);" id="btnSavePage" class="FL Button BigBlue">
                <%= Html.Term("Save")%></a> <a href="<%= ResolveUrl("~/Sites/Pages") %>" class="FL Button">
                    <%= Html.Term("Cancel")%></a>
            <img class="Loading FL ml10" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>"
                alt="loading..." style="display: none;" height="26" />
            <span class="clr"></span>
        </p>
    </div>
    </form>
</asp:Content>
