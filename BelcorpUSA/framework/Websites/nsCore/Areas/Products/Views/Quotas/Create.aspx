<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.ProductQuotaEntity>" %>

<asp:Content ID="Content0" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
    <script type="text/javascript">
        $(function () {
            /*
            $('.TimePicker').timepickr();
            $('.DatePicker').datepicker();
            //$.datepicker.formatDate("yy-mm-dd", new Date(2007, 1 - 1, 26));
            */
            // Look-up account ini
            $('#rbYesRestrictPerGroupPerson').click(function () {
                $("#rbNoRestrictPerGroupPerson").removeProp("checked");
                $("#rbYesRestrictPerGroupPerson").prop("checked", "checked");
                $("#divRestrictPerGroupPerson").css("display", "");
                $("#spYesRestrictPerGroupPerson").addClass("UI-lightBg hiddenPanel pad10");

                LoadExcelFunctionality();
            });

            $('#rbNoRestrictPerGroupPerson').click(function () {
                $("#rbYesRestrictPerGroupPerson").removeProp("checked");
                $("#rbNoRestrictPerGroupPerson").prop("checked", "checked");
                $("#divRestrictPerGroupPerson").css("display", "none");
                $("#spYesRestrictPerGroupPerson").removeClass("UI-lightBg hiddenPanel pad10");
            });

            $('#txtCustomerSuggest').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                $('#accountId').val(item.id);
                $('#txtCustomerSuggest').clearError();
            }, minCharacters: 1, source: $('#txtCustomerSuggest'), ajaxResults: true, maxResults: 50, showMore: true
            });
            $('form').submit(function () {
                if (!$('#accountId').val())
                    return false;
            });
            $('#conditionSingleAccountAdd').click(function () {
                var accountId = $('#accountId').val();
                var strAccount = $('#txtCustomerSuggest').val();
                var londAccount = strAccount.indexOf("(");
                var nomAccount = strAccount.substr(0, londAccount - 1);
                if (accountId) {
                    $('#conditionAccountGrid tbody').prepend('<tr>'
						            + '<td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>'
                                    + '<input type="hidden" id="accountId" class="accountId required" name="Account is required" value="' + accountId + '" /></td>'
						            + '<td>' + accountId + '</td>'
						            + '<td>' + nomAccount + '</td>'
						            + '</tr>');
                    $('#txtCustomerSuggest, #accountId').val('');
                    $('#conditionAccountGrid').show();
                }
            });

            $('#conditionAccountGrid .BtnDelete').live('click', function () {
                $(this).closest('tr').remove();
                $('#singleItemQuickAdd').show();
            });

            $('#btnSave').click(function () {
                //                if ($('#quotaForm').checkRequiredFields()) {
                if ('<%= Model.RestrictionID %>' != "0") {
                    CallUpdate();   // footer
                    return;
                }
                if ($("#txtName").val() == '') {
                    showMessage("Restriction name can not be empty", true);
                    return false;
                }
                if (parseInt($('#ddlStartPeriod').val()) > parseInt($('#ddlEndPeriod').val())) {
                    showMessage("End period must be greater or equal than Start period", true);
                    return false;
                }

                var count = 0;
                var count1 = 0;
                var count2 = 0;

                if ($("#conditionSingleGrid tr").length != 2) {
                    showMessage("Required add a Product", true);
                    return false;
                }

                if ($("#qty").val() < 1) {
                    showMessage("Quantity must be higher than zero.", true);
                    return false;
                }

                var data = {
                    name: $("#txtName").val(),
                    active: $('#chkActive').prop('checked')
                };

                // Restrict to account types ?
                data['hasAccountTypes'] = false;
                if ($('#rbYesRestrictToAccounts').prop('checked')) {
                    data['hasAccountTypes'] = true;
                    //                        $('.paidAs:checked').each(function (i) { data['paidAsTitleIDs[' + i + ']'] = $(this).val(); });
                    //                        $('.recognized:checked').each(function (i) { data['recognizedTitleIDs[' + i + ']'] = $(this).val(); });
                    $('.accountType:checked').each(function (i) { data['accountTypeIDs[' + i + ']'] = $(this).val(); count1 += 1; });
                    if (count1 <= 0) {
                        showMessage("You must select at least one account type.", true);
                        return false;
                    }
                }

                // Restrict to title types ?
                data['hasTitleTypes'] = false;
                if ($('#rbYesRestrictToTitles').prop('checked')) {
                    data['hasTitleTypes'] = true;
                    $('.paidAs:checked').each(function (i) { data['paidAsTitleIDs[' + i + ']'] = $(this).val(); count += 1; });
                    $('.recognized:checked').each(function (i) { data['recognizedTitleIDs[' + i + ']'] = $(this).val(); count += 1; });
                    if (count <= 0) {
                        showMessage("You must select at least one title.", true);
                        return false;
                    }
                }

                // Restrict per person ?
                data['RestrictionType'] = $('#ddlRestrictionType').val();

                // Apply to product
                if ($('#conditionSingleGrid #productId').val() != undefined) {
                    data['productId'] = $('#conditionSingleGrid #productId').val();
                    data['quantity'] = $('#conditionSingleGrid #qty').val();
                }


                data['StartPeriodID'] = $("#ddlStartPeriod").val();
                data['EndPeriodID'] = $("#ddlEndPeriod").val();

                //Grupo de contas
                data['hasAccount'] = false;
                if ($('#rbYesRestrictPerGroupPerson').prop('checked')) {
                    data['hasAccount'] = true;
                    $("#conditionAccountGrid tbody tr").each(function (i) {
                        var accountid = $(this).find("td").eq(1).html();
                        data['accountIDs[' + i + ']'] = accountid;
                        count2 += 1;
                    });

                    if (count2 <= 0) {
                        showMessage("You must select at least one account.", true);
                        return false;
                    }
                }

                function onSuccess(result) {
                    if (result.result) {
                        showMessage('<%= Html.Term("SaveRestriction", "Restriction Successfully Saved!") %>');
                        window.location = '<%= ResolveUrl("~/Products/Quotas/Create") %>/' + result.id;
                    }
                    else {
                        showMessage(result.message, true);
                    }
                }

                var options = {
                    url: '<%= ResolveUrl("~/Products/Quotas/Save") %>',
                    success: onSuccess,
                    showLoading: $('#btnSave'),
                    data: data
                };

                NS.post(options);
            });


            // Look-up product ini
            $('#txtConditionSingle').change(function () {
                $('#conditionSingleProductID').val("");
            });

            $('#txtConditionSingle').removeClass('Filter').after($('#conditionSingleProductID')).css('width', '275px')
                .val('')
				.watermark('<%= Html.JavascriptTerm("ProductSearch", "Look up product by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Products/Promotions/Search") %>', { onSelect: function (item) {
				    $('#conditionSingleProductID').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});

            $('#conditionSingleProductAdd').click(function () {

                if ($("#conditionSingleProductQty").val() < 1) {
                    showMessage("Quantity must be higher than zero.", true);
                    return;
                }

                var productId = $('#conditionSingleProductID').val();
                if (productId) {
                    getProductInfo(productId, $(this), function (result) {
                        $('#conditionSingleGrid tbody').prepend('<tr>'
						            + '<td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>'
                                    + '<input type="hidden" id="productId" class="productId required" name="Product is required" value="' + productId + '" /></td>'
						            + '<td>' + result.product.SKU + '</td>'
						            + '<td>' + result.product.Name + '</td>'
						            + '<td><input type="text" id="qty" class="qty" value="' + $('#conditionSingleProductQty').val() + '" /></td>'
						            + '</tr>');
                        $('#txtConditionSingle, #conditionSingleProductID').val('');
                        $('#conditionSingleProductQty').val('1');
                        $('.qty').numeric({ negative: false });
                        $('#singleItemQuickAdd').hide();
                        $('#conditionSingleGrid').show();
                    });
                }
            });

            function getProductInfo(productId, showLoading, success) {
                var options = {
                    url: '<%= ResolveUrl("~/Products/ProductPromotions/QuickAddProduct") %>',
                    showLoading: showLoading,
                    data: { productId: productId },
                    success: success
                };
                NS.post(options);
            }

            $('#conditionSingleGrid .BtnDelete').live('click', function () {
                $(this).closest('tr').remove();
                $('#singleItemQuickAdd').show();
                $('#conditionSingleGrid').hide();
            });
            // Look-up product fin


            $('#btnCancel').click(function () {
                // Retry to Index Quotas
                window.location.replace('<%= ResolveUrl("~/Products/Quotas") %>');
            });

            $("input[name=rbActiveImmediately]").click(function () {
                if ($('input:radio[name=rbActiveImmediately]:checked').val() == "0") {
                    $("#divActiveImmediately").css("display", "");
                    $("#spNoActiveImmediately").addClass("UI-lightBg hiddenPanel pad10");
                } else {
                    $("#divActiveImmediately").css("display", "none");
                    $("#spNoActiveImmediately").removeClass("UI-lightBg hiddenPanel pad10");
                }
            });

            $("input[name=rbRestrictToAccounts]").click(function () {
                if ($('input:radio[name=rbRestrictToAccounts]:checked').val() == "1") {
                    $("#divRestrictToAccounts").css("display", "");
                    $("#spYesRestrictToAccounts").addClass("UI-lightBg hiddenPanel pad10");
                } else {
                    $("#divRestrictToAccounts").css("display", "none");
                    $("#spYesRestrictToAccounts").removeClass("UI-lightBg hiddenPanel pad10");
                }
            });

            $('.accountType').click(function () {
                if ($(this).val() == '1' && $(this).prop("checked") == false) {
                    $("#rbYesRestrictToTitles").prop("checked", false)
                    $("#rbNoRestrictToTitles").prop("checked", true)
                    $("#divRestrictToTitles").css("display", "none");
                    $("#spYesRestrictToTitles").removeClass("UI-lightBg hiddenPanel pad10");
                }
            })

            //EB-227
            $("input[name=rbRestrictToTitles]").click(function () {
                if ($('input:radio[name=rbRestrictToTitles]:checked').val() == "1") {
                    if ($('input:radio[name=rbRestrictToAccounts]:checked').val() == "1") {
                        var swTitles = false;
                        $('.accountType:checked').each(function (i) {
                            if ($(this).val() == '1') { //Si es una ba
                                swTitles = true;
                                return false;
                            }
                        });

                        if (swTitles) {
                            $("#divRestrictToTitles").css("display", "");
                            $("#spYesRestrictToTitles").addClass("UI-lightBg hiddenPanel pad10");
                        }
                        else {
                            showMessage("This option is available only for consultants.", true);
                            $("#rbYesRestrictToTitles").prop("checked", false)
                            $("#rbNoRestrictToTitles").prop("checked", true)
                        }
                    }
                    else {
                        showMessage("This option is available only for consultants.", true);
                        $("#rbYesRestrictToTitles").prop("checked", false)
                        $("#rbNoRestrictToTitles").prop("checked", true)
                    }
                } else {
                    $("#divRestrictToTitles").css("display", "none");
                    $("#spYesRestrictToTitles").removeClass("UI-lightBg hiddenPanel pad10");
                }
            });

            $("input[name=rbRestrictPerPerson]").click(function () {
                if ($('input:radio[name=rbRestrictPerPerson]:checked').val() == "1") {
                    $("#divRestrictPerPerson").css("display", "");
                    //$("#spYesRestrictPerPerson").addClass("UI-lightBg hiddenPanel pad10");
                } else {
                    $("#divRestrictPerPerson").css("display", "none");
                    $("#spYesRestrictPerPerson").removeClass("UI-lightBg hiddenPanel pad10");
                }
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Quotas") %>">
            <%= Html.Term("ProductQuota", "Product Quota")%></a> >
    <%= Html.Term("NewQuota", "New Quota")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("NewProperty", "New Property")%>
        </h2>
    </div>
    <table id="quotaForm" class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Name", "Name")%>:
            </td>
            <td>
                <input type="hidden" id="hddRestrictionId" />
                <input id="txtName" type="text" class="pad5 required" name="<%= Html.Term("NameIsRequired", "Name is required") %>"
                    style="width: 20.833em;" />
                <hr />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Active", "Active")%>:
            </td>
            <td>
                <input id="chkActive" type="checkbox" checked="checked" />
                <!-- se activa para dejarlo por defecto wv:20160426  -->
                <%--<label for="chkActive"><%: Html.Term("Enabled", "Enabled") %></label>--%>
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <%: Html.Term("RestrictionType", "Restriction Type")%>:
            </td>
            <td>
                <select id="ddlRestrictionType">
                    <option value="0">
                        <%: Html.Term("RestrictionTypeGeneral", "General")%></option>
                    <option value="1" selected="selected">
                        <%: Html.Term("RestrictionTypePerPerson", "Per person")%></option>
                </select>
                <hr />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <label for="rbActiveImmediately">
                    <%: Html.Term("ActiveImmediatelyQuestion", "Select Period?")%></label>
            </td>
            <td>
                <span id="spNoActiveImmediately">
                    <input type="radio" name="rbActiveImmediately" value="0" id="rbNoActiveImmediately"
                        checked="checked" style="display: none" />
                    <label for="rbNoActiveImmediately" style="display: none">
                        <%=Html.Term("No", "No")%></label>
                </span><span id="spYesActiveImmediately">
                    <input type="radio" name="rbActiveImmediately" value="1" id="rbYesActiveImmediately"
                        style="display: none" />
                    <label for="rbYesActiveImmediately" style="display: none">
                        <%=Html.Term("Yes", "Yes")%></label>
                </span>
                <div id="divActiveImmediately" class="UI-lightBg hiddenPanel pad10">
                    <table>
                        <tr>
                            <td>
                                <%=Html.Term("StartPeriod", "Start Period")%>
                            </td>
                            <td>
                                <select id="ddlStartPeriod" style="width: 80px; height: 15;">
                                    <%
                                        Dictionary<string, string> period = Periods.GetAllPeriods();
                                        {
                                            System.DateTime moment = DateTime.Today;
                                            int valYear = Convert.ToInt32(string.Concat(moment.Year, "00"));
                                            foreach (var pair in period.Where(n => Convert.ToInt32(n.Key) > valYear))
                                            { %>
                                    <%if (pair.Key.Equals(@ViewBag.StartPeriodID))
                                      {%>
                                    <option value="<%=pair.Key%>" selected="selected">
                                        <%=pair.Key%></option>
                                    <% }
                                      else
                                      {%>
                                    <option value="<%=pair.Key%>">
                                        <%=pair.Key%></option>
                                    <%} %>
                                    <%
                                            }
                                        }
                                    %>
                                </select>
                            </td>
                            <td>
                                <%=Html.Term("EndPeriod", "End Period")%>
                            </td>
                            <td>
                                <select id="ddlEndPeriod" style="width: 80px; height: 15;">
                                    <%
                                        Dictionary<string, string> period1 = Periods.GetAllPeriods();
                                        {
                                            System.DateTime moment = DateTime.Today;
                                            int valYear = Convert.ToInt32(string.Concat(moment.Year, "00"));
                                            foreach (var pair in period1.Where(n => Convert.ToInt32(n.Key) > valYear))
                                            { %>
                                    <%if (pair.Key.Equals(@ViewBag.EndPeriodID))
                                      {%>
                                    <option value="<%=pair.Key%>" selected="selected">
                                        <%=pair.Key%></option>
                                    <% }
                                      else
                                      {%>
                                    <option value="<%=pair.Key%>">
                                        <%=pair.Key%></option>
                                    <%} %>
                                    <%
                                            }
                                        }
                                    %>
                                </select>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <label for="rbRestrictToAccounts">
                    <%: Html.Term("RestrictToAccountsQuestion", "Restrict To Accounts?")%></label>
            </td>
            <td>
                <span>
                    <input type="radio" name="rbRestrictToAccounts" value="0" id="rbNoRestrictToAccounts"
                        checked="checked" />
                    <label for="rbNoRestrictToAccounts">
                        <%=Html.Term("No", "No")%></label>
                </span><span id="spYesRestrictToAccounts">
                    <input type="radio" name="rbRestrictToAccounts" value="1" id="rbYesRestrictToAccounts" />
                    <label for="rbYesRestrictToAccounts">
                        <%=Html.Term("Yes", "Yes")%></label>
                </span>
                <div id="divRestrictToAccounts" class="UI-lightBg hiddenPanel pad10" style="display: none">
                    <span class="lawyer">
                        <%=Html.Term("PromotionOptions_RestrictToAccountTip", "Only checked accounts will receive the promotion.")%>
                    </span>
                    <% 
                        //var service = NetSteps.Encore.Core.IoC.Create.New<NetSteps.Data.Common.Services.ITitleService>();
                        //var titles = service.GetTitles();
                        var accountTypes = SmallCollectionCache.Instance.AccountTypes;
                        //int numRows = Math.Max(titles.Count(), accountTypes.Count);
                        int numRows = accountTypes.Count;
                    %>
                    <table class="DataGrid" width="100%">
                        <thead>
                            <tr class="GridColHead Alt">
                                <th>
                                    <%=Html.Term("PromotionOptions_AccountTypes", "Account Type") %>
                                </th>
                            </tr>
                        </thead>
                        <%--Temporalmente comentado--%>
                        <tbody>
                            <% for (int i = 0; i < numRows; i++)
                               {
                                   //var title = titles.ElementAtOrDefault(i);
                                   var accountType = accountTypes.ElementAtOrDefault(i);
                            %>
                            <tr <%= i % 2 == 0 ? "class='Alt'" : "" %>>
                                <%
                                   if (accountType != null)
                                   {%>
                                <td>
                                    <input type="checkbox" value="<%= accountType.AccountTypeID %>" class="accountType" /><%= accountType.GetTerm()%>
                                </td>
                                <%}
                                   else
                                   { %>
                                <td>
                                </td>
                                <%} %>
                            </tr>
                            <%} %>
                        </tbody>
                        <%--HardCode--%>
                        <%--<tbody>
					        <tr class="Alt">
						        <td>
							        <input type="checkbox" value="1" class="paidAs"/>Beauty Advisor
						        </td>
						        <td>
							        <input type="checkbox" value="1" class="recognized"/>Beauty Advisor
						        </td>
						        <td>
							        <input type="checkbox" value="1" class="accountType"/>Distributor
						        </td>
					        </tr>
					
					        <tr>
						        <td>
							        <input type="checkbox" value="2" class="paidAs"/>Beauty Advisor 1
						        </td>
						        <td>
							        <input type="checkbox" value="2" class="recognized"/>Beauty Advisor 1
						        </td>
						        <td>
							        <input type="checkbox" value="2" class="accountType"/>Preferred Customer
						        </td>
					        </tr>
					
					        <tr class="Alt">
						        <td>
							        <input type="checkbox" value="3" class="paidAs"/>Beauty Advisor 2
						        </td>
						        <td>
							        <input type="checkbox" value="3" class="recognized"/>Beauty Advisor 2
						        </td>
						        <td>
							        <input type="checkbox" value="3" class="accountType"/>Customer
						        </td>
					        </tr>
					
					        <tr>
						        <td>
							        <input type="checkbox" value="4" class="paidAs"/>Beauty Advisor 3
						        </td>
						        <td>
							        <input type="checkbox" value="4" class="recognized"/>Beauty Advisor 3
						        </td>
						        <td>
							        <input type="checkbox" value="4" class="accountType"/>Employee
						        </td>
					        </tr>
					
					        <tr class="Alt">
						        <td>
							        <input type="checkbox" value="5" class="paidAs"/>Beauty Advisor 4
						        </td>
						        <td>
							        <input type="checkbox" value="5" class="recognized"/>Beauty Advisor 4
						        </td>
						        <td>
							        <input type="checkbox" value="5" class="accountType">Prospect
						        </td>
					        </tr>
					
					        <tr>
						        <td>
							        <input type="checkbox" value="6" class="paidAs">
							        Beauty Manager
						        </td>
						        <td>
							        <input type="checkbox" value="6" class="recognized">
							        Beauty Manager
						        </td>
						
						        <td></td>
						
					        </tr>
					
					        <tr class="Alt">
						
						        <td>
							        <input type="checkbox" value="7" class="paidAs">
							        Beauty Director
						        </td>
						        <td>
							        <input type="checkbox" value="7" class="recognized">
							        Beauty Director
						        </td>
						
						        <td></td>
						
					        </tr>
					
					        <tr>
						
						        <td>
							        <input type="checkbox" value="8" class="paidAs">
							        Senior Beauty Director
						        </td>
						        <td>
							        <input type="checkbox" value="8" class="recognized">
							        Senior Beauty Director
						        </td>
						
						        <td></td>
						
					        </tr>
					
					        <tr class="Alt">
						
						        <td>
							        <input type="checkbox" value="9" class="paidAs">
							        Beauty Vice President
						        </td>
						        <td>
							        <input type="checkbox" value="9" class="recognized">
							        Beauty Vice President
						        </td>
						
						        <td></td>
						
					        </tr>
					
					        <tr>
						
						        <td>
							        <input type="checkbox" value="10" class="paidAs">
							        Senior Beauty Vice President
						        </td>
						        <td>
							        <input type="checkbox" value="10" class="recognized">
							        Senior Beauty Vice President
						        </td>
						
						        <td></td>
						
					        </tr>
					
					        <tr class="Alt">
						
						        <td>
							        <input type="checkbox" value="11" class="paidAs">
							        Group Vice President
						        </td>
						        <td>
							        <input type="checkbox" value="11" class="recognized">
							        Group Vice President
						        </td>
						
						        <td></td>
						
					        </tr>
					
				        </tbody>--%>
                    </table>
                </div>
                <hr />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <label for="rbRestrictToTitles">
                    <%: Html.Term("RestrictToTitlesQuestion", "Restrict To Titles?")%></label>
            </td>
            <td>
                <span>
                    <input type="radio" name="rbRestrictToTitles" value="0" id="rbNoRestrictToTitles"
                        checked="checked" />
                    <label for="rbNoRestrictToAccounts">
                        <%=Html.Term("No", "No")%></label>
                </span><span id="spYesRestrictToTitles">
                    <input type="radio" name="rbRestrictToTitles" value="1" id="rbYesRestrictToTitles" />
                    <label for="rbYesRestrictToAccounts">
                        <%=Html.Term("Yes", "Yes")%></label>
                </span>
                <div id="divRestrictToTitles" class="UI-lightBg hiddenPanel pad10" style="display: none">
                    <span class="lawyer">
                        <%=Html.Term("PromotionOptions_RestrictToAccountTip", "Only checked accounts will receive the promotion.")%>
                    </span>
                    <% 
                        var service = NetSteps.Encore.Core.IoC.Create.New<NetSteps.Data.Common.Services.ITitleService>();
                        var titles = service.GetTitles();
                        int numRows1 = titles.Count();
                    %>
                    <table class="DataGrid" width="100%">
                        <thead>
                            <tr class="GridColHead Alt">
                                <th>
                                    <%=Html.Term("PromotionOptions_PaidAsTitle", "Paid As Title") %>
                                </th>
                                <th>
                                    <%=Html.Term("PromotionOptions_RecognizedTitle", "Recognized Title") %>
                                </th>
                            </tr>
                        </thead>
                        <%--Temporalmente comentado--%>
                        <tbody>
                            <% for (int i = 0; i < numRows1; i++)
                               {
                                   var title = titles.ElementAtOrDefault(i);
                                   //var accountType = accountTypes.ElementAtOrDefault(i);
                            %>
                            <tr <%= i % 2 == 0 ? "class='Alt'" : "" %>>
                                <%if (title != null)
                                  { %>
                                <td>
                                    <input type="checkbox" value="<%=title.TitleID %>" class="paidAs" />
                                    <%= Html.Term(title.TermName)%>
                                </td>
                                <td>
                                    <input type="checkbox" value="<%=title.TitleID %>" class="recognized" />
                                    <%= Html.Term(title.TermName)%>
                                </td>
                                <% }
                                  else
                                  { %>
                                <td>
                                </td>
                                <td>
                                </td>
                                <%}%>
                            </tr>
                            <%} %>
                        </tbody>
                    </table>
                </div>
                <hr />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <label for="rbRestrictPerGroupPerson">
                    <%: Html.Term("RestrictionTypePerGroupPerson", "Account group")%></label>
            </td>
            <td>
                <span>
                    <input type="radio" name="rbRestrictPerGroupPerson" value="0" id="rbNoRestrictPerGroupPerson"
                        checked="checked" />
                    <label for="rbNoRestrictPerGroupPerson">
                        <%=Html.Term("No", "No")%>
                    </label>
                </span><span id="spYesRestrictPerGroupPerson">
                    <input type="radio" name="rbRestrictPerGroupPerson" value="1" id="rbYesRestrictPerGroupPerson" />
                    <label for="rbYesRestrictPerGroupPerson">
                        <%=Html.Term("Yes", "Yes")%>
                    </label>
                </span>
                <div id="divRestrictPerGroupPerson" class="UI-lightBg hiddenPanel pad10" style="display: none">
                    <table class="FormTable" width="100%">
                        <tr>
                            <td>
                                <%= Html.Term("AccountSearch", "Account Search")%>:<span class="LawyerText" />
                                <br />
                                (<%= Html.Term("EnterAtLeast3Characters", "enter at least 3 characters")%>)
                            </td>
                            <td>
                                <input type="text" id="txtCustomerSuggest" style="width: 300px;" />
                                <input type="hidden" name="accountId" id="accountId" />
                                <a class="DTL Add" href="javascript:void(0);" id="conditionSingleAccountAdd">
                                    <%= Html.Term("Promotions_QuickAdd", "Add")%></a>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td style="padding-left: 0px">
                                <table border="0px" cellpadding="0px">
                                    <tr>
                                        <td>
                                            <a class="Button BigBlue" id="btnDownloadTemplate" href="javascript:void(0);">
                                                <%= Html.Term("DownloadTemplate", "Download Template")%>
                                            </a>
                                        </td>
                                        <td>
                                            <form enctype="multipart/form-data" action="" id="formLoad" method="post">
                                            <a class="Button BigBlue" id="btnBrowse" href="javascript:void(0);">
                                                <%= Html.Term("LoadCampaingMatrix", "LOAD FROM EXCEL")%>
                                            </a>
                                            <label id="label">
                                            </label>
                                            <input type="file" id="inputLoadMatrix" name="ninputLoadMatrix" accept="xlsx|xls"
                                                style="display: none" />
                                            <input type="submit" id="submitHidden" style="display: none" />
                                            </form>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <div>
                                    <table width="100%" class="DataGrid" id="conditionAccountGrid">
                                        <thead>
                                            <tr class="GridColHead">
                                                <th class="GridCheckBox">
                                                </th>
                                                <th>
                                                    <%=Html.Term("AccountNumber")%>
                                                </th>
                                                <th>
                                                    <%=Html.Term("Name")%>
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                        </tbody>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <label>
                    <%: Html.Term("AppyToProduct", "Apply to Product")%></label>
            </td>
            <td>
                <div id="singleItemQuickAdd">
                    <%= Html.Term("Promotions_ProductLookUpLabel", "Product look-up")%>:<br />
                    <input type="hidden" value="" id="conditionSingleProductID" class="Filter" />
                    <input type="text" value="" size="30" class="pad5 mr10 txtQuickAdd required" name="Product is required"
                        hiddenid='conditionSingleProductID' id="txtConditionSingle" />
                    <input type="text" value="1" class="pad5 qty center" id="conditionSingleProductQty" />
                    <a class="DTL Add" href="javascript:void(0);" id="conditionSingleProductAdd">
                        <%= Html.Term("Promotions_QuickAdd", "Add")%></a>
                </div>
                <table width="100%" style="display: none;" class="DataGrid" id="conditionSingleGrid">
                    <thead>
                        <tr class="GridColHead">
                            <th class="GridCheckBox">
                            </th>
                            <th>
                                <%=Html.Term("SKU")%>
                            </th>
                            <th>
                                <%=Html.Term("Product")%>
                            </th>
                            <th>
                                <%=Html.Term("Quantity")%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue">
                        <%= Html.Term("Save", "Save") %></a> <a href="javascript:void(0);" id="btnCancel"
                            style="display: inline-block;" class="Button BigWhite">
                            <%= Html.Term("Cancel", "Cancel") %></a>
                </p>
            </td>
        </tr>
    </table>
    <script type="text/javascript">

    $(document).ready(function () {
        var currentId = '<%= Model.RestrictionID %>';

        if (currentId == 0) return;

        $("#txtName").val('<%= Model.Name %>');
        $("#chkActive").attr("checked", ('<%= Model.Active %>' == 'True'));
        $('#ddlRestrictionType').attr('disabled','disabled');

        $('#ddlStartPeriod').attr('disabled','disabled');
        $('#ddlEndPeriod').attr('disabled','disabled');

        LoadInitialCreation();
        LoadRestrictAccounts();
        CheckRestrictPerson();
        LoadRestrictAccountsGroup();
        LoadProduct();

        DisableControls();
    });

    function LoadExcelFunctionality()
    {
        //Carga Excel ini
            $('#inputLoadMatrix').change(function (e) {
                $('#submitHidden').trigger('click');
            });

            $('#btnBrowse').click(function () {
                $('#inputLoadMatrix').trigger('click');
            });

            $("#btnDownloadTemplate").click(function () {
                window.location = '/Products/File/DownloadTemplate';
                return false;
            });
            document.getElementById('formLoad').onsubmit = function () {
                var formdata = new FormData();
                var fileInput = document.getElementById('inputLoadMatrix');
                for (i = 0; i < fileInput.files.length; i++) {
                    formdata.append(fileInput.files[i].name, fileInput.files[i]);
                }
                var xhr = new XMLHttpRequest();
                xhr.open('POST', '/Products/File/FileAccounts');
                xhr.send(formdata);
                $('#btnBrowse').showLoading();
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4 && xhr.status == 200) {
                        var response = JSON.parse(xhr.responseText);
                        if (response.result) {
                            for (var i = 0; i < response.accounts.length; i++) {
                                $('#conditionAccountGrid tbody').prepend('<tr>'
                            + '<td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>'
                            + '<input type="hidden" id="AccountNumber" class="productId required" name="Product is required" value="' + response.accounts[i].AccountNumber + '" /></td>'
						    + '<td>' + response.accounts[i].AccountNumber + '</td>'
						    + '<td>' + response.accounts[i].Name + '</td>'
						    + '</tr>');
                            }
                        }
                    }
                    $('#btnBrowse').hideLoading();
                }
                return false;
            }
        //Carga Excel fin
    }

    function LoadInitialCreation() 
    {
            $("input[name=rbActiveImmediately]").click();
            $("#rbNoActiveImmediately").removeAttr("checked");
            $("#rbYesActiveImmediately").attr("checked", "checked");
    }

    function LoadInitialCreation() {
        <%
            if (Model.EndDate != null) {
        %>
            $("input[name=rbActiveImmediately]").click();
            $("#rbYesActiveImmediately").removeAttr("checked");
            $("#rbNoActiveImmediately").attr("checked", "checked");
        <% } %>
    }

    function LoadRestrictAccountsGroup()
    {
     <%
            if (Model.AccountIDs.Count() > 0){
        
        %>
        $("#rbNoRestrictPerGroupPerson").removeProp("checked");
        $("#rbYesRestrictPerGroupPerson").prop("checked", "checked");
        $("#divRestrictPerGroupPerson").css("display", "");
        $("#spYesRestrictPerGroupPerson").addClass("UI-lightBg hiddenPanel pad10");
        var accountIDs = <%= Html.Raw(Json.Encode(Model.AccountIDs)) %>;
        $.each(accountIDs, function (ind, elem) { 
            var arr = elem.split(';');
            $('#conditionAccountGrid tbody').prepend('<tr>'
						            + '<td>'
                                    + '<a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>'
                                    + '<input type="hidden" id="accountId" class="accountId required" name="Account is required" value="' + arr[0] + '" /></td>'
						            + '<td>' + arr[0] + '</td>'
						            + '<td>' + arr[1] + '</td>'
						            + '</tr>');
            $('#conditionAccountGrid').show();
        });
        

        <% } %>
    }

    function LoadRestrictAccounts() {
        <%
            if (Model.AccountTypeIDs.Count() > 0){
        %>
            $("#rbNoRestrictToAccounts").removeProp("checked");
            $("#rbYesRestrictToAccounts").prop("checked", "checked");
            $("#divRestrictToAccounts").css("display", "");
            $("#spYesRestrictToAccounts").addClass("UI-lightBg hiddenPanel pad10");

            var accountTypes = <%= Html.Raw(Json.Encode(Model.AccountTypeIDs)) %>;
            
            $("#divRestrictToAccounts > table > tbody").find("tr").each(function() {
                $(this).find("td:eq(0) > input[type=checkbox]").each(function(){
                    var id = $(this).val();
                    
                    if (isInArray( Number(id), accountTypes )){
                        $(this).prop("checked", "checked");
                    }

                });
            });

        <% } %>

        <%
            if (Model.PaidAsTitlesIDs.Count() > 0 || Model.RecognizedTitlesIDs.Count() > 0){
        %>

         $("#rbNoRestrictToTitles").removeProp("checked");
            $("#rbYesRestrictToTitles").prop("checked", "checked");
            $("#divRestrictToTitles").css("display", "");
            $("#spYesRestrictToTitles").addClass("UI-lightBg hiddenPanel pad10");

            var paidTitles = <%= Html.Raw(Json.Encode(Model.PaidAsTitlesIDs)) %>;
            var recognizedTitles = <%= Html.Raw(Json.Encode(Model.RecognizedTitlesIDs)) %>;

            $("#divRestrictToTitles > table > tbody").find("tr").each(function() {
                $(this).find("td:eq(0) > input[type=checkbox]").each(function(){
                    var id = $(this).val();
                    
                    if (isInArray( Number(id), paidTitles )){
                        $(this).prop("checked", "checked");
                    }

                });
                $(this).find("td:eq(1) > input[type=checkbox]").each(function(){
                    var id = $(this).val();
                    
                    if (isInArray( Number(id), recognizedTitles )){
                        $(this).prop("checked", "checked");
                    }

                });
            });
       <% } %>
        
    }

    function CheckRestrictPerson() {
        $('#ddlRestrictionType').val('<%= Model.RestrictionType %>');
    }
    
    function LoadProduct() {
        var ValidProductName = '<%= Model.ProductName != null? Model.ProductName.Replace("'",""):"" %>';
        $('#conditionSingleGrid tbody').prepend('<tr>'
                                    + '<td></td>'
						            + '<td>' + '<%= Model.ProductSKU %>' + '</td>'
                                    + '<td>' + ValidProductName + '</td>'
						            + '<td>' + '<%= Model.Quantity %>' + '</td>'
						            + '</tr>');
        $('#singleItemQuickAdd').hide();
        $('#conditionSingleGrid').show();
    }

    function DisableControls() {
        $("#quotaForm").find("input").prop("disabled", "disabled");
        $("#chkActive").removeProp("disabled");

        //Habilita Restriccion por cuentas
        $("#txtCustomerSuggest").removeProp("disabled");
        $("#conditionSingleAccountAdd").removeProp("disabled");
        $("#btnDownloadTemplate").removeProp("disabled");
        $("#btnBrowse").removeProp("disabled");
        $("#inputLoadMatrix").removeProp("disabled");
        $("#submitHidden").removeProp("disabled");
        LoadExcelFunctionality();
    }

    function isInArray(value, array) {
      return array.indexOf(value) > -1;
    }

    function CallUpdate() {
        var onSuccess = function (result) 
                    {
                        if (result.result) {
                            showMessage('<%= Html.Term("SaveRestriction", "Restriction Successfully Saved!") %>');
                            window.location = '<%= ResolveUrl("~/Products/Quotas/Create") %>/' + result.id;
                        }
                        else {
                            showMessage(result.message, true);
                        }
                    }
        
        var data1 = { restrictionID: '<%= Model.RestrictionID %>', state : $("#chkActive").prop('checked') }
        var count2 = 0;
                data1['hasAccount'] = false;
                if ($('#rbYesRestrictPerGroupPerson').prop('checked')) {
                    data1['hasAccount'] = true;
                    $("#conditionAccountGrid tbody tr").each(function (i) {
                        var accountid = $(this).find("td").eq(1).html();
                        data1['accountIDs[' + i + ']'] = accountid;
                        count2 += 1;
                    });

                    if (count2 <= 0) {
                        showMessage("You must select at least one account.", true);
                        return false;
                    }
                }
        var options = {
            url: '<%= ResolveUrl("~/Products/Quotas/UpdateRestrictionState") %>',
            success: onSuccess,
            showLoading: $('#btnSave'),
            data: data1
        };

        NS.post(options);
    }

    
    </script>
</asp:Content>
