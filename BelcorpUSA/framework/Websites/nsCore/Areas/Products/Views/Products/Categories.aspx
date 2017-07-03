<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            var absolute = true, originalTop = $('#buttonContainer').offset().top - 15;
            $(window).scroll(function () {
                if (absolute && originalTop < $(window).scrollTop()) {
                    absolute = false;
                    $('#buttonContainer').css({ position: 'fixed', top: '15px' });
                } else if (!absolute && originalTop > $(window).scrollTop()) {
                    absolute = true;
                    $('#buttonContainer').css({ position: '', top: '' });
                }
            });

            $('#sCategoryTree').change(function () {
                $.get('<%= ResolveUrl("~/Products/Products/GetCategoryTree") %>', { categoryTreeId: $('#sCategoryTree').val(), productId: '<%= Model.ProductID %>' }, function (response) {
                    if (response.result === undefined || response.result) {
                        $('#categoryTreeContainer').html(response);
                    } else {
                        showMessage(response.message, true);
                    }
                });
            }).find('option:first').attr('selected', 'selected');

            $('#btnSave').click(function () {
                var t = $(this),
					categories = { categoryTree: $('#sCategoryTree').val() };
                $('#categoryTreeContainer .category:checked').each(function (i) {
                    categories['categories[' + i + ']'] = $(this).val();
                });

                showLoading(t);

                $.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveCategories/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', categories, function (response) {
                    hideLoading(t);
                    if (response.result) {
                        showMessage('Categories saved!', false);
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });
        });
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
            <%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>">
                <%= Model.Translations.Name() %></a> >
    <%= Html.Term("Categories") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%IEnumerable<Category> categoryTrees = Category.LoadFullTopLevelByCategoryTypeId((int)Constants.CategoryType.Product); %>
    <!-- Section Header -->
    <div class="SectionHeader">
        <h2>
            <%= Model.Translations.Name() %>
            <%= Html.Term("Categories", "Categories") %></h2>
        <%= Html.Term("CategoryTree", "Category Tree") %>:
        <select id="sCategoryTree">
            <% foreach (Category categoryTree in categoryTrees)
               { %>
            <option value="<%= categoryTree.CategoryID %>">
                <%= categoryTree.Translations.Name() %></option>
            <% } %>
        </select>
    </div>
    <!-- Product CMS Stuff -->
    <table width="100%" cellspacing="0" class="">
        <tr>
            <td style="width: 40%; border-right: 1px solid #c0c0c0; padding: 5px;">
                <div id="categoryTreeContainer" class="CategoriesManagement">
                    <%= ViewData["CategoryTree"] %>
                </div>
            </td>
            <td style="width: 60%; padding: 5px;">
                <p id="buttonContainer">
                    <a id="btnSave" href="javascript:void(0);" class="Button BigBlue"><span>
                        <%= Html.Term("SaveCategories", "Save Categories") %></span></a>
                    <%--<a href="#">Cancel</a>--%>
                </p>
            </td>
        </tr>
    </table>
</asp:Content>
