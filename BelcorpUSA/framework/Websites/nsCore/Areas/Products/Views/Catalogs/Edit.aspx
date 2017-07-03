<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/Catalogs.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Catalog>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
    <script type="text/javascript">
        var currentPage = 0, maxPage = Math.ceil(parseInt('<%= Model.CatalogItems.Count %>') / 20) - 1;
        $(function () {
            var dateRegex = /\d+\/\d+\/\d+/i, timeRegex = /\d+\:\d+\s(am|pm)/i;
            $('#productBulkAdd,#catalogCopier').jqm({ modal: false,
                onShow: function (h) {
                    h.w.css({
                        top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                        left: '0px',
                        'margin-left': '0px'
                    }).fadeIn();
                },
                onHide: function (h) {
                    h.w.fadeOut('slow');
                    $('#txtBulkAddStartDate').val('Start Date');
                    $('#txtBulkAddEndDate').val('End Date');
                }, overlay: 0
            });
            $('#btnOpenBulkAdd').click(function () {
                $('#productBulkAdd').jqmShow();
            });
            $('#btnOpenCatalogCopier').click(function () {
                $('#catalogCopier').jqmShow();
            });
            $('#ui-datepicker-div').css('zIndex', '3000');
            $('.TimePicker').timepickr({ convention: 12 });
            $('#productQuickAdd').watermark('<%= Html.JavascriptTerm("ExistingSKUorName", "Existing SKU or Name") %>');

            $('#pageSize').change(function () { currentPage = 0; getCatalogItems(); });

            $('#btnPreviousPage').click(function () {
                if (currentPage > 0) {
                    --currentPage;
                    getCatalogItems();
                }
            });
            $('#btnNextPage').click(function () {
                if (currentPage < maxPage) {
                    ++currentPage;
                    getCatalogItems();
                }
            });

            if ('<%= Model.CatalogID %>' > 0) {
                enableProductPanel();
                getCatalogItems();
            } else {
                disableProductPanel();
            }

            $('#sLanguage').change(function () {
                $.get('<%= ResolveUrl("~/Products/Catalogs/GetDescription") %>', { catalogId: $('#catalogId').val(), languageId: $(this).val() }, function (response) {
                    if (response.result) {
                        $('#txtName').val(response.name);
                        $('#txtShortDescription').val(response.shortDescription);
                        $('#txtLongDescription').val(response.longDescription);
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });

            $('#selectAllCatalogItems').click(function () {
                $('#catalogItems input:checkbox').attr('checked', $(this).prop('checked'));
            });

            $('#btnApplySchedule').click(function () {
                if (dateRegex.test($('#txtCatalogItemStartDate').val()) && dateRegex.test($('#txtCatalogItemEndDate').val())) {
                    if (new Date($('#txtCatalogItemStartDate').val()) > new Date($('#txtCatalogItemEndDate').val())) {
                        showMessage('<%= Html.Term("StartDateBeforeEndDate", "Please make sure your start date is before your end date.") %>', true);
                        return false;
                    }
                }
                var data = {
                    startDate: $('#txtCatalogItemStartDate').val(),
                    startTime: $('#txtCatalogItemStartTime').val(),
                    endDate: $('#txtCatalogItemEndDate').val(),
                    endTime: $('#txtCatalogItemEndTime').val()
                }, selected = $('#catalogItems > tbody input[type="checkbox"]:checked');
                selected.each(function (i) {
                    data['catalogItems[' + i + ']'] = $(this).val();
                });
                $.post('<%= ResolveUrl("~/Products/Catalogs/ChangeItemSchedules") %>', data, function (response) {
                    getCatalogItems();
                });
            });

            $('#btnRemove').click(function () {
                var data = {};
                $('#catalogItems > tbody input[type="checkbox"]:checked').each(function (i) {
                    data['catalogItems[' + i + ']'] = $(this).val();
                });

                $.post('<%= ResolveUrl("~/Products/Catalogs/RemoveItems") %>', data, function (repsonse) {
                    getCatalogItems();
                });
            });

            $('#chkBulkAddCheckAll').click(function () {
                $('#productBulkAddGrid input[type="checkbox"]').attr('checked', $(this).prop('checked'));
            });

            $('#btnBulkAddProducts').click(function () {
                if (dateRegex.test($('#txtBulkAddStartDate').val()) && dateRegex.test($('#txtBulkAddEndDate').val())) {
                    if (new Date($('#txtBulkAddStartDate').val()) > new Date($('#txtBulkAddEndDate').val())) {
                        showMessage('<%= Html.Term("StartDateBeforeEndDate", "Please make sure your start date is before your end date.") %>', true);
                        return false;
                    }
                }
                var data = {
                    catalogId: $('#catalogId').val(),
                    startDate: $('#txtBulkAddStartDate').val(),
                    startTime: $('#txtBulkAddStartTime').val(),
                    endDate: $('#txtBulkAddEndDate').val(),
                    endTime: $('#txtBulkAddEndTime').val()
                };
                $('#productBulkAdd .bulkAddProductSelector:checked').each(function (i) {
                    data['products[' + i + ']'] = $(this).val();
                });

                $.post('<%= ResolveUrl("~/Products/Catalogs/BulkAddItems") %>', data, function (response) {
                    $('#productBulkAdd .bulkAddProductSelector:checked').attr('checked', false);
                    getCatalogItems();
                });
            });

            $('#btnCopyCatalog').click(function () {
                var selectedCatalog = $('input[name="catalogToCopy"]:checked');
                if (selectedCatalog.length) {
                    $.post('<%= ResolveUrl("~/Products/Catalogs/Copy") %>', { catalogId: $('#catalogId').val(), copyCatalogId: selectedCatalog.val() }, function (response) {
                        if (response.result) {
                            getCatalogItems();
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            });

            $('#btnQuickAdd').click(function () {
                if ($('#quickAddProductId').val()) {
                    addCatalogItem($('#quickAddProductId').val());
                    $('#quickAddProductId').val('');
                    $('#productQuickAdd').val('').blur();
                } else if ($('#productQuickAdd').val() != $('#productQuickAdd').data('watermark')) {
                    $.post('<%= ResolveUrl("~/Products/Catalogs/TryAddItem") %>', { catalogId: $('#catalogId').val(), query: $('#productQuickAdd').val() }, function (response) {
                        if (response.result) {
                            getCatalogItems();
                        } else {
                            showMessage('Could not find any product that matches that query.  Please try again or use the autosuggest.', true);
                        }
                    });
                }
            });

            $('#CatalogTypeID').change(function () {
                if ($(this).val() == '<%= (int)ConstantsGenerated.CatalogType.EnrollmentKits %>' ||
                    $(this).val() == '<%= (int)ConstantsGenerated.CatalogType.AutoshipBundles %>') {
                    $('#SelectAccountTypes').slideDown('fast');
                } else {
                    $('#SelectAccountTypes').slideUp('fast');
                }
            });

            $('#btnSave').click(function () {
                if ($('#catalogForm').checkRequiredFields()) {
                    var data = {
                        catalogId: $('#catalogId').val(),
                        languageId: $('#sLanguage').val(),
                        name: $('#txtName').val(),
                        shortDescription: $('#txtShortDescription').val(),
                        longDescription: $('#txtLongDescription').val(),
                        active: $('#chkActive').prop('checked'),
                        catalogTypeID: $('#CatalogTypeID').val(),
                        startDate: $('#txtStartDate').val(),
                        startTime: $('#txtStartTime').val(),
                        endDate: $('#txtEndDate').val(),
                        endTime: $('#txtEndTime').val(),
                        categoryTreeId: $('#sCategoryTree').val()
                    };
                    $('input[name="storeFronts"]:checked').each(function (i) {
                        data['storeFronts[' + i + ']'] = $(this).val();
                    });

                    var accountTypeFilterSelected = $('input[name=chkSpecifyCatalogAccountTypes]:radio:checked').val();
                    if (accountTypeFilterSelected == "yes") {

                        $('input[class="CatalogAccountTypesCheckbox"]:checked').each(function (i) {
                            data['accountTypes[' + i + ']'] = $(this).val();
                        });
                    } else {
                        data['accountTypes'] = null;
                    }

                    //Developed by BAL - CSTI - A06
                    var vCatalogPeriods = "";
                    for (var i = 0; i < $("#lbCatalogPeriods")[0].length; i++) {
                        var separator = i < $("#lbCatalogPeriods")[0].length - 1 ? "|" : "";
                        vCatalogPeriods += $("#lbCatalogPeriods")[0][i].value + separator;
                    }
                    data['catalogPeriods'] = vCatalogPeriods;

                    $.post('<%= ResolveUrl("~/Products/Catalogs/Save") %>', data, function (response) {
                        if (response.result) {
                            if (!$('#catalogId').val()) {
                                $('#catalogId').val(response.catalogId);
                                enableProductPanel();
                            }
                            showMessage('Catalog saved!', false);
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            });

            //Developed by BAL - CSTI - AINI
            $('#ddlPlans').change(function () {
                if (!isNaN($('#txtYearFilter').val()) && ($("#txtYearFilter").val()).length == 4) {
                    LoadPeriodsToAdd();
                }
            });

            $('#txtYearFilter').change(function () {
                changeYearFilter();
            }).keyup(function () {
                changeYearFilter();
            });

            $('#btnAddPeriod').click(function () {
                // proceeding to add only if it contains elements
                if (!$('select#ddlPeriods option').length > 0) {
                    return false;
                }

                // Get selected period (periodID, description) add
                var periodValue = $("#ddlPeriods").val();
                var periodText = $("#ddlPeriods option:selected").text(); //$("#ddlPeriods")[0].selectedOptions[0].text;

                // Add periodo to Catalogs
                $("#lbCatalogPeriods").append('<option value="' + periodValue + '">' + periodText + '</option>');

                // Remove period from Periods
                $("#ddlPeriods").find("option[value=" + periodValue + "]").remove();
            });

            $('#btnRemovePeriod').click(function () {
                // proceeding to remove only if it contains elements
                if (!$("#lbCatalogPeriods")[0].length > 0) {
                    return false;
                }

                // Get period from Catalog
                var periodValue = $("#lbCatalogPeriods").val();

                // Remove period from Catalog
                $("#lbCatalogPeriods").find("option[value=" + periodValue + "]").remove();

                // Reload Periods
                LoadPeriodsToAdd();
            });
            //Developed by BAL - CSTI - AFIN

            changeYearFilter();
        });

        //Developed by BAL - CSTI - AINI
        function LoadPeriodsToAdd() {
            var vCatalogPeriods = "";
            for (var i = 0; i < $("#lbCatalogPeriods")[0].length; i++) {
                var separator = i < $("#lbCatalogPeriods")[0].length - 1 ? "|" : "";
                vCatalogPeriods += $("#lbCatalogPeriods")[0][i].value + separator;
            }
            
            var data = {
                planId: $('#ddlPlans').val(),
                yearFilter: $('#txtYearFilter').val(),
                catalogPeriods: vCatalogPeriods
            };

            $.post('<%= ResolveUrl("~/Products/Catalogs/GetPeriodsByPlan") %>', data, function (response) {
                if (response.result) {
                    $('option', $("#ddlPeriods")).remove();

                    if (response.data.length > 0) {
                        for (var i = 0; i < response.data.length; i++) {
                            $("#ddlPeriods").append('<option value="' + response.data[i].Value + '">' + response.data[i].Text + '</option>');
                        }
                        $("#ddlPeriods").removeAttr("disabled");
                    } else {
                        $("#ddlPeriods").attr("disabled", "disabled");
                    }

                } else {
                    showMessage(response.message, true);
                }
            });
        }

        function changeYearFilter() {
            var YearValue = $('#txtYearFilter').val();
            if (!isNaN(YearValue) && YearValue.length == 4) {
                LoadPeriodsToAdd();
            }
            else {
                $("#ddlPeriods").find('option').remove().end();
            }
        }
        //Developed by BAL - CSTI - AFIN

        function showProductContainer() {
            $('#productNoAdd').hide();
            $('#productAdd').show();
            $('#btnOpenBulkAdd').show();
            $('#btnOpenCatalogCopier').show();
        }

        function hideProductContainer() {
            $('#productNoAdd').show();
            $('#productAdd').hide();
            $('#btnOpenBulkAdd').hide();
            $('#btnOpenCatalogCopier').hide();
        }

        function enableProductPanel() {
            showProductContainer();
            $('#productQuickAdd').keyup(function (e) {
                if (e.keyCode == 13)
                    $('#btnQuickAdd').click();
            }).jsonSuggest('<%= ResolveUrl("~/Products/Catalogs/SearchPossibleProducts") %>', { ajaxResults: true, minCharacters: 3, source: $('#productQuickAdd'), data: { catalogId: $('#catalogId').val() }, onSelect: function (item) { $('#quickAddProductId').val(item.id); } });
            $('#pageSize').val('20');
        }

        function disableProductPanel() {
            hideProductContainer();
        }

        function getCatalogItems() {
            $('#selectAllCatalogItems').attr('checked', false);
            var t = $('#catalogItems tbody');
            t.html('<tr><td colspan="5"><img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');
            $.get('<%= ResolveUrl("~/Products/Catalogs/GetItems") %>', { page: currentPage, pageSize: $('#pageSize').val(), catalogId: $('#catalogId').val() }, function (response) {
                if (response.result === undefined || response.result) {
                    t.html(response.catalogItems);
                    maxPage = response.resultCount == 0 ? 0 : Math.ceil(response.resultCount / $('#pageSize').val()) - 1;
                    $('#btnNextPage,#btnPreviousPage').removeAttr('disabled').css({ color: '', cursor: '' });
                    if (currentPage == maxPage)
                        $('#btnNextPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
                    if (currentPage == 0)
                        $('#btnPreviousPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
                } else {
                    showMessage(response.message, true);
                }
            })
            .fail(function() {
                t.html('<tr><td colspan="5"></td></tr>');
                showMessage('@Html.Term("ErrorProcessingRequest", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
            });
        }

        function addCatalogItem(productId, startDate, endDate) {
            $.post('<%= ResolveUrl("~/Products/Catalogs/AddItem") %>', { catalogId: $('#catalogId').val(), productId: productId, startDate: startDate, endDate: endDate }, function (response) {
                getCatalogItems();
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Catalogs") %>">
            <%= Html.Term("CatalogManagement", "Catalog Management") %></a> >
    <%= Model.CatalogID == 0 ? Html.Term("NewCatalog", "New Catalog") : Html.Term("EditCatalog", "Edit Catalog") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
    <div class="SectionHeader">
        <h2>
            <%= Model.CatalogID == 0 ? Html.Term("NewCatalog", "New Catalog") : Html.Term("EditCatalog", "Edit Catalog")%></h2>
        <%= Html.Term("Language") %>:

        <%= Html.DropDownLanguages(htmlAttributes: new { id = "sLanguage" }, selectedLanguageID: (Model.Translations.Count > 0 ? Model.Translations.FirstOrDefault().LanguageID : CoreContext.CurrentLanguageID))%>
        
        <a href="<%= ResolveUrl("~/Products/Catalogs") %>">
            <%= Html.Term("BrowseCatalogs", "Browse Catalogs") %></a> |
        <%if (Model.CatalogID > 0)
          { %><a href="<%= ResolveUrl("~/Products/Catalogs/Edit") %>"><%} %><%= Html.Term("CreateaNewCatalog", "Create a New Catalog") %><%if (Model.CatalogID > 0)
                                                                                                                                           { %></a><%} %>
    </div>
    <table id="catalogForm" class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("CatalogType", "Catalog Type") %>:
            </td>
            <td>
                <%: Html.DropDownListFor(x => x.CatalogTypeID, ViewBag.CatalogTypes as IEnumerable<SelectListItem>) %>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="SelectAccountTypes" style="display:none;" >
                    <%= Html.Partial("_CatalogAccountTypesOptions", Model.AccountTypes) %>
                </div>
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%= Html.Term("Name", "Name") %>:
            </td>
            <td>            
                <input type="hidden" id="catalogId" value="<%= Model.CatalogID == 0 ? "" : Model.CatalogID.ToString() %>" />
                <input id="txtName" type="text" class="required" name="<%= Html.Term("NameIsRequired", "Name is required") %>"
                    value="<%= Model.Translations.Name() %>" style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%= Html.Term("ShortDescription", "Short Description") %>:
            </td>
            <td>
                <textarea id="txtShortDescription" rows="" cols="" style="width: 25em; height: 4.167em;"><%= Model.Translations.ShortDescription() %></textarea>
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%= Html.Term("LongDescription", "Long Description") %>:
            </td>
            <td>
                <textarea id="txtLongDescription" rows="" cols="" style="width: 25em; height: 4.167em;"><%= Model.Translations.LongDescription() %></textarea>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("StoreFronts", "Store Fronts") %>:
            </td>
            <td>
                <% var storeFronts = Model.StoreFronts.Select(sf => sf.StoreFrontID);
                   foreach (StoreFront storeFront in SmallCollectionCache.Instance.StoreFronts)
                   { %>
                <input type="checkbox" name="storeFronts" id="storeFronts<%= storeFront.StoreFrontID %>"
                    value="<%= storeFront.StoreFrontID %>" <%= storeFronts.Contains(storeFront.StoreFrontID) ? "checked=\"checked\"" : "" %> />
                <label for="storeFronts<%= storeFront.StoreFrontID %>">
                    <%= storeFront.GetTerm() %></label><br />
                <%} %>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Active", "Active") %>:
            </td>
            <td>
                <input id="chkActive" type="checkbox" <%= Model.Active ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("ScheduleStart", "Schedule Start") %>:
            </td>
            <td>
                <input id="txtStartDate" type="text" class="DatePicker" value="<%= Model.StartDate.HasValue ? Model.StartDate.Value.ToShortDateString() : Html.Term("StartDate","Start Date") %>"
                    style="width: 9.091em;" /><br />
                <input id="txtStartTime" type="text" class="TimePicker" value="<%= Model.StartDate.HasValue ? Model.StartDate.Value.ToShortTimeString() : Html.Term("StartTime","Start Time") %>" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("ScheduleEnd", "Schedule End") %>:
            </td>
            <td>
                <input id="txtEndDate" type="text" class="DatePicker" value="<%= Model.EndDate.HasValue ? Model.EndDate.Value.ToShortDateString() : Html.Term("EndDate","End Date") %>"
                    style="width: 9.091em;" /><br />
                <input id="txtEndTime" type="text" class="TimePicker" value="<%= Model.EndDate.HasValue ? Model.EndDate.Value.ToShortTimeString() : Html.Term("EndTime","End Time") %>" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("ProductCategoryTree", "Product Category Tree") %>:
            </td>
            <td>
                <select id="sCategoryTree">
                    <%foreach (Category categoryTree in Category.LoadFullTopLevelByCategoryTypeId((int)Constants.CategoryType.Product))
                      { %>
                    <option value="<%= categoryTree.CategoryID %>" <%= Model.CategoryID == categoryTree.CategoryID ? "selected=\"selected\"" : "" %>>
                        <%= categoryTree.Translations.Name() %></option>
                    <%} %>
                </select>
            </td>
        </tr>

        <%--Developed by BAL - CSTI - AINI--%>

        <tr>
            <td class="FLabel">
                <%= Html.Term("Periods", "Periods") %>:
            </td>
            <td>
                <table id="tablePeriods" style="border:1px solid;">
                    <tr>
                        <td>
                            <label class="FLabel"><%= Html.Term("Plan", "Plan") %>:</label> 
                            <br/><br />
                            <label class="FLabel"><%= Html.Term("Year", "Year") %>:</label> 
                            <br/><br />
                            <label class="FLabel"><%= Html.Term("Period", "Period") %>:</label> 
                        </td>
                        <td>
                            <%: Html.DropDownList("ddlPlans", ViewData["Plans"] as IEnumerable<SelectListItem>, new { style="width: 200px;" } )%>
                            <br/><br />
                            <input id="txtYearFilter" type="number" placeholder="AAAA" style="width: 200px;" maxlength="4" value='<%= ViewBag.CurrentYear %>'/>
                            <br /><br />
                            <%: Html.DropDownList("ddlPeriods", ViewData["Periods"] as IEnumerable<SelectListItem>, new { style = "width: 200px;" })%>
                        </td>
                        <td>
                            <br />
                            <a href="javascript:void(0);" id="btnAddPeriod" title="<%= Html.Term("AddPeriodToCatalog", "Add Period To Catalog") %>" ">
                                <img src="<%= ResolveUrl("~/Content/Images/Buttons/Add.png") %>"/>
                            </a>
                            <br />
                            <a href="javascript:void(0);" id="btnRemovePeriod" title="<%= Html.Term("RemovePeriodFromCatalog", "Remove Period from Catalog") %>" >
                                <img src="<%= ResolveUrl("~/Content/Images/Buttons/Remove.png") %>"/>
                            </a>
                        </td>
                        <td>
                            <label class="FLabel"><%= Html.Term("PeriodsOfCatalogs", "Periods Of Catalogs")%>:</label> 
                            <br />
                            <%: Html.ListBox("lbCatalogPeriods", ViewData["CatalogPeriods"] as IEnumerable<SelectListItem>, new { style = "width: 200px;" })%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>

        <%--Developed by BAL - CSTI - AFIN--%>

        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                        <%= Html.Term("SaveCatalog", "Save Catalog") %></a>
                </p>
            </td>
        </tr>
        <tr id="productPanel">
            <td class="FLabel" style="vertical-align: top;">
                <%= Html.Term("Products", "Products") %>:
                <p class="InputTools">
                    <a id="btnOpenBulkAdd" href="javascript:void(0);" class="BulkAdd">
                        <%= Html.Term("OpenBulkAdd", "Open Bulk Add") %></a><br />
                    <a id="btnOpenCatalogCopier" href="javascript:void(0);" class="Copier">
                        <%= Html.Term("OpenCatalogCopier", "Open Catalog Copier") %></a>
                </p>
                <div id="productPanelOverlay" style="background-color: #999999; height: 0px; width: 0px;
                    position: absolute; left: 0px; top: 0px; z-index: 1; opacity: 0.6; filter: alpha(opacity=60);">
                    <%--<span style="opacity: 1; filter: alpha(opacity=1); color: #FF0000; margin: auto;">Please
						save your catalog first</span>--%>
                </div>
            </td>
            <td id="productNoAdd" style="display:none;">
				<%= Html.Term("CatalogNotSaved", "The catalog must be saved before products can be added.") %>
            </td>
            <td id="productAdd">
                <p class="FL">
                    <%= Html.Term("QuickProductAdd", "Quick Product Add") %>:
                    <input id="productQuickAdd" type="text" class="" value="" />
                    <input type="hidden" id="quickAddProductId" value="" />
                    <a id="btnQuickAdd" href="javascript:void(0);" class="DTL Add">
                        <%= Html.Term("Add", "Add") %></a>
                </p>
                <div class="FR">
                    <p class="FL" style="margin-right: .909em;">
                        <%= Html.Term("ApplytoSelected", "Apply to Selected") %>:</p>
                    <p class="FL">
                        <input id="txtCatalogItemStartDate" type="text" class="DatePicker" value="<%= Html.Term("StartDate", "Start Date") %>"
                            style="width: 6.636em;" />
                        <br />
                        <input id="txtCatalogItemStartTime" type="text" class="TimePicker" value="<%= Html.Term("StartTime", "Start Time") %>" />
                    </p>
                    <p class="FL">
                        <input id="txtCatalogItemEndDate" type="text" class="DatePicker" value="<%= Html.Term("EndDate", "End Date") %>"
                            style="width: 6.636em;" />
                        <br />
                        <input id="txtCatalogItemEndTime" type="text" class="TimePicker" value="<%= Html.Term("EndTime", "End Time") %>" />
                    </p>
                    <p class="FL">
                        <a id="btnApplySchedule" href="javascript:void(0);" class="">
                            <%= Html.Term("AddSchedule", "Add Schedule") %></a> | <a id="btnRemove" href="javascript:void(0);"
                                class="">
                                <%= Html.Term("RemoveFromCatalog", "Remove from Catalog") %></a>
                    </p>
                    <span class="Clear"></span>
                </div>
                <span class="ClearAll"></span>
                <!-- Products In Order -->
                <table id="catalogItems" width="100%" class="DataGrid">
                    <thead>
                        <tr class="GridColHead">
                            <th class="GridCheckBox">
                                <input id="selectAllCatalogItems" type="checkbox" />
                            </th>
                            <th>
                                <%= Html.Term("SKU", "SKU") %>
                            </th>
                            <th>
                                <%= Html.Term("Product", "Product") %>
                            </th>
                            <th>
                                <%= Html.Term("StartDate", "Start Date") %>
                            </th>
                            <th>
                                <%= Html.Term("EndDate", "End Date") %>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div class="Pagination">
                    <a href="javascript:void(0);" id="btnPreviousPage">&lt;&lt;
                        <%= Html.Term("Previous")%></a><a href="javascript:void(0);" id="btnNextPage" style="margin-left:.909em;"><%= Html.Term("Next", "Next") %>
                            &gt;&gt;</a>
                            <span style="margin-left:.909em;">
                            <%= Html.Term("PageSize", "Page Size") %>:<select id="pageSize">
                                <option selected="selected" value="20">20</option>
                                <option value="50">50</option>
                                <option value="100">100</option>
                            </select>
                            </span>
                </div>
            </td>
        </tr>
    </table>
    <!--Bulk Add -->
    <div id="productBulkAdd" class="jqmWindow LModal PhotoWin3" style="position: fixed;">
        <div class="mContent">
            <h2>
                <%= Html.Term("BulkProductAdd", "Bulk Product Add") %></h2>
            <div>
                <p class="FL" style="margin-right: .909em;">
                    <%= Html.Term("ApplyToSelected", "Apply to Selected") %>:</p>
                <p class="FL">
                    <span>
                        <input type="text" id="txtBulkAddStartDate" class="DatePicker" value="Start Date"
                            style="width: 7.091em;" />
                        <br />
                        <input type="text" id="txtBulkAddStartTime" class="TimePicker" value="Start Time" />
                    </span>
                </p>
                <p class="FL">
                    <span>
                        <input type="text" id="txtBulkAddEndDate" class="DatePicker" value="End Date" style="width: 7.091em;" />
                        <br />
                        <input type="text" id="txtBulkAddEndTime" class="TimePicker" value="Start Time" />
                    </span>
                </p>
                <span class="Clear"></span>
            </div>
            <table cellspacing="0" cellpadding="0" width="100%;" class="DataGrid">
                <tr class="GridColHead">
                    <th style="width: 1.818em;">
                        <input id="chkBulkAddCheckAll" type="checkbox" />
                    </th>
                    <th style="width: 7.273em;">
                        <%= Html.Term("SKU", "SKU") %>
                    </th>
                    <th style="width: 10.909em;">
                        <%= Html.Term("Product", "Product") %>
                    </th>
                </tr>
            </table>
            <div style="height: 250px; overflow: auto; border-bottom: .091em solid #efefef;">
                <table id="productBulkAddGrid" cellspacing="0" cellpadding="0" width="100%" class="DataGrid">
                    <% int count = 0;
                       foreach (ProductSlimSearchData product in Product.LoadAllSlim(new NetSteps.Common.Base.FilterPaginatedListParameters<Product>()
                       {
                           PageIndex = 0,
                           PageSize = null,
                           OrderBy = "SKU",
                           OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
                       }))
                       { %>
                    <tr <%= count % 2 == 1 ? "class=\"Alt\"" : "" %>>
                        <td style="width: 1.818em;">
                            <input type="checkbox" value="<%= product.ProductID %>" class="bulkAddProductSelector" />
                        </td>
                        <td style="width: 7.273em;">
                            <%= product.SKU %>
                        </td>
                        <td style="width: 10.909em;">
                            <%= product.Name %>
                        </td>
                    </tr>
                    <%++count;
                       } %>
                </table>
            </div>
            <br />
            <p>
                <a id="btnBulkAddProducts" href="javascript:void(0);" class="Button BigBlue">
                    <%= Html.Term("AddToCatalog", "Add to Catalog") %></a>
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Close", "Close") %></a>
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>
    <!-- Copier -->
    <div id="catalogCopier" class="jqmWindow LModal CopyWin" style="position: fixed;">
        <div class="mContent">
            <h2>
                <%= Html.Term("CatalogCopier", "Catalog Copier") %></h2>
            <p>
                <%= Html.Term("CopyProductsFromAnotherCatalog", "Copy products from another catalog into this catalog") %>:</p>
            <div style="overflow: scroll; max-height: 500px;">
            <%foreach (Catalog catalog in Catalog.LoadAll().Where(c => c.CatalogID != Model.CatalogID))
              { %>
            <input type="radio" name="catalogToCopy" value="<%= catalog.CatalogID %>" /><%= catalog.Translations.Name() %><br />
            <%} %>
            </div>
            <br />
            <p>
                <a id="btnCopyCatalog" href="javascript:void(0);" class="Button BigBlue jqmClose">
                    <%= Html.Term("Copy", "Copy") %></a>
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Close", "Close") %></a>
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>
    <%Html.RenderPartial("MessageCenter"); %>
</asp:Content>
