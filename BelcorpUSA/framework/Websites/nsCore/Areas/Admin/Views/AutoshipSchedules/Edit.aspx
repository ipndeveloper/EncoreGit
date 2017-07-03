<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.AutoshipSchedule>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
    	$(function () {


              $('input[monedaidioma=CultureIPN]').keyup(function (event) {
           
                   var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
		        // var value = parseFloat($(this).val());


		        var formatDecimal = '$1.$2'; // valores por defaul 
		        var formatMiles = ",";  // valores por defaul

		        if (cultureInfo === 'en-US') {
		            var formatDecimal = '$1.$2';
		            var formatMiles = ",";
		        }
		        else if (cultureInfo === 'es-US') {
		            var formatDecimal = '$1,$2';
		            var formatMiles = ".";
		        }
		        else if (cultureInfo === 'pt-BR') {
                var formatDecimal = '$1,$2';
		            var formatMiles = ".";
		        }


		        //            if (!isNaN(value)) {
		        if (event.which >= 37 && event.which <= 40) {
		            event.preventDefault();
		        }

		        $(this).val(function (index, value) {


		            return value.replace(/\D/g, "")
                                 .replace(/([0-9])([0-9]{2})$/, formatDecimal)
                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, formatMiles);
		        });



            });
    		//Product panel
    		var enableProductPanel = function () {
    			showProductContainer();
    			$('#productQuickAdd').keyup(function (e) {
    				if (e.keyCode == 13)
    					$('#btnQuickAdd').click();
    			}).jsonSuggest('<%= ResolveUrl("~/Products/Products/Search") %>', { ajaxResults: true, minCharacters: 3, onSelect: function (item) { $('#quickAddProductId').val(item.id); } });
    			$('#pageSize').val('20');
    		},
			getProducts = function () {
				$('#selectAll').attr('checked', false);
				$('#products tbody').html('<tr><td colspan="5"><img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');
				$.get('<%= ResolveUrl("~/Admin/AutoshipSchedules/GetProducts") %>', { page: currentPage, pageSize: $('#pageSize').val(), autoshipScheduleId: $('#scheduleId').val() }, function (response) {
					if (response.result) {
						$('#products tbody').html(response.page);

						maxPage = response.totalPages == 0 ? 0 : response.totalPages - 1;
						$('#btnNextPage,#btnPreviousPage').removeAttr('disabled').css({ color: '', cursor: '' });
						if (currentPage == maxPage)
							$('#btnNextPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' });
						if (currentPage == 0)
							$('#btnPreviousPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' });
					} else {
						showMessage(response.message, true);
					}
				});
			}, currentPage = 0;

    		$('#productQuickAdd').watermark('<%= Html.JavascriptTerm("ExistingSKUorName", "Existing SKU or Name") %>');

    		$('#pageSize').change(function () { currentPage = 0; getProducts(); });

    		$('#btnPreviousPage').click(function () {
    			if (currentPage > 0) {
    				--currentPage;
    				getProducts();
    			}
    		});
    		$('#btnNextPage').click(function () {
    			if (currentPage < maxPage) {
    				++currentPage;
    				getProducts();
    			}
    		});

    		$('#btnQuickAdd').click(function () {
    			if (!$('#quickAddQuantity').val())
    				$('#quickAddQuantity').val('1');
    			if ($('#quickAddProductId').val()) {
    				$.post('<%= ResolveUrl("~/Admin/AutoshipSchedules/AddProduct") %>', { autoshipScheduleId: $('#scheduleId').val(), productId: $('#quickAddProductId').val(), quantity: $('#quickAddQuantity').val() }, function (response) {
    					getProducts();
    				});
    				$('#quickAddProductId').val('');
    				$('#productQuickAdd').val('').blur();
    				$('#quickAddQuantity').val('1');
    			} else {
    				$.post('<%= ResolveUrl("~/Admin/AutoshipSchedules/TryAddProduct") %>', { autoshipScheduleId: $('#scheduleId').val(), query: $('#productQuickAdd').val(), quantity: $('#quickAddQuantity').val() }, function (response) {
    					if (response.result) {
    						getProducts();
    					} else {
    						showMessage('Could not find any product that matches that query.  Please try again or use the autosuggest.', true);
    					}
    				});
    			}
    		});

    		$('#btnRemove').click(function () {
    			var data = {};
    			$('#products .productSelector:checked').each(function (i) {
    				data['products[' + i + ']'] = $(this).val();
    			});

    			$.post('<%= ResolveUrl("~/Admin/AutoshipSchedules/RemoveProducts") %>', data, function (repsonse) {
    				getProducts();
    			});
    		});

    		if ('<%= Model.AutoshipScheduleID %>' > 0) {
    			enableProductPanel();
    			getProducts();
    		} else {
    			hideProductContainer();
    		}

    		//Everything else
    		$('#scheduleProperties').setupRequiredFields();
    		$('#intervalTypeId').change(function () {
    			showOrHideRunDaysContainer();
    		});
    		$('#scheduleTypeId').change(function () {
    			if ($(this).val() == '<%= (int)Constants.AutoshipScheduleType.SiteSubscription %>')
    				$('#baseSite').show();
    			else
    				$('#baseSite').hide();
    		});

    		$('#btnSave').click(function () {
    			if (!$('#scheduleProperties').checkRequiredFields()) {
    				return false;
    			}

    			var t = $(this);
    			showLoading(t);

    			var data = {
    				scheduleId: $('#scheduleId').val(),
    				name: $('#name').val(),
    				scheduleTypeId: $('#scheduleTypeId').val(),
    				intervalTypeId: $('#intervalTypeId').val(),
    				baseSiteId: $('#baseSite').is(':visible') ? $('#baseSiteId').val() : '',
    				orderTypeId: $('#orderTypeId').val(),
    				minimumCommissionableTotal: $('#MinimumCommissionableTotal').val(),
    				maxRetryCount: $('#MaxRetryCount').val(),
    				maxFailedIntervals: $('#MaxFailedIntervals').val(),
    				//catalogId: $('#catalogId').val(),
    				active: $('#active').prop('checked'),
					enrollable: $('#enrollable').prop('checked'),
					editable: $('#editable').prop('checked'),

    			};

    			$('#accountTypes .accountType:checked').each(function (i) {
    				data['accountTypes[' + i + ']'] = $(this).val();
    			});

    			$('#runDays .runDay:checked').each(function (i) {
    				data['runDays[' + i + ']'] = $(this).val();
    			});

    			$.post('<%= ResolveUrl("~/Admin/AutoshipSchedules/Save") %>', data, function (response) {
    				showMessage(response.message || 'Schedule saved successfully!', !response.result);
    				if (response.result) {
    					$('#scheduleId').val(response.scheduleId);
    					enableProductPanel();
    				}
    				hideLoading(t);
    			});
    		});

			$('#MaxRetryCount').change(function(){
				if($(this).val() != "") {
					validateMinInteger(this);
					validateMaxInteger(this);
				}
			});

			$('#MaxFailedIntervals').change(function(){
				if($(this).val() != "") {
					validateMinInteger(this);
				}
			});

    		function showOrHideRunDaysContainer() {

    			var intervalDropDown = $('#intervalTypeId');
    			$('#runDaysContainer').toggle($('option:selected', intervalDropDown).hasClass('Monthly'));
    		};

    		function showProductContainer() {
    			$('#productNoAdd').hide();
    			$('#productAdd').show();
    		}

    		function hideProductContainer() {
    			$('#productAdd').hide();
    			$('#productNoAdd').show();
    		}

			function validateMaxInteger(obj){
				var value = $(obj).val();
				if(parseInt(value) > 31) {
					$(obj).val("31");
				}
			}
			
			function validateMinInteger(obj){
				var value = $(obj).val();
				if(parseInt(value) == 0) {
					$(obj).val(null);
				}
			}

    		showOrHideRunDaysContainer();
    	});
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Admin") %>">
        <%= Html.Term("Admin", "Admin") %></a> > <a href="<%= ResolveUrl("~/Admin/AutoshipSchedules") %>">
            <%= Html.Term("AutoshipSchedules", "Autoship Schedules") %></a> >
    <%= Model.AutoshipScheduleID == 0 ? Html.Term("NewSchedule", "New Schedule") : Html.Term("EditSchedule", "Edit Schedule") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Model.AutoshipScheduleID == 0 ? Html.Term("AddSchedule", "Add Schedule") : Html.Term("EditSchedule", "Edit Schedule")%></h2>
    </div>
    <input type="hidden" id="scheduleId" value="<%= Model.AutoshipScheduleID == 0 ? "" : Model.AutoshipScheduleID.ToString() %>" />
    <table id="scheduleProperties" width="100%" cellspacing="0" class="DataGrid">
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Name") %>:
            </td>
            <td>
                <input type="text" id="name" class="required" name="<%= Html.Term("NameRequired", "Name is required.") %>"
                    value="<%= Model.Name %>" style="width: 25em;" />
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Type", "Type") %>:
            </td>
            <td>
                <select id="scheduleTypeId">
                    <%foreach (AutoshipScheduleType type in SmallCollectionCache.Instance.AutoshipScheduleTypes)
                      { %>
                    <option value="<%= type.AutoshipScheduleTypeID %>" <%= type.AutoshipScheduleTypeID == Model.AutoshipScheduleTypeID ? "selected=\"selected\"" : "" %>>
                        <%= type.GetTerm() %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr id="baseSite" <%= Model.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.SiteSubscription ? "" : "style=\"display:none;\"" %>>
            <td style="width: 13.636em;">
                <%= Html.Term("BaseSite", "Base Site") %>
            </td>
            <td>
                <select id="baseSiteId" class="required" name="Base site is required.">
                    <%foreach (Site baseSite in NetSteps.Data.Entities.Site.LoadBaseSites())
                      { %>
                    <option value="<%= baseSite.SiteID %>" <%= baseSite.SiteID == Model.BaseSiteID ? "selected=\"selected\"" : "" %>>
                        <%= baseSite.Name %>
                    </option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Interval", "Interval") %>:
            </td>
            <td>
                <select id="intervalTypeId" name="intervalTypeId">
                    <%foreach (IntervalType interval in SmallCollectionCache.Instance.IntervalTypes)
                      { %>
                    <option value="<%= interval.IntervalTypeID %>" <%= interval.IsMonthly ? " class=\"Monthly\"" : "" %><%= interval.IntervalTypeID == Model.IntervalTypeID ? " selected=\"selected\"" : "" %>>
                        <%= interval.GetTerm() %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("OrderType", "Order Type") %>:
            </td>
            <td>
                <select id="orderTypeId">
                    <%foreach (OrderType orderType in SmallCollectionCache.Instance.OrderTypes.Where(ot => ot.IsTemplate))
                      { %>
                    <option value="<%= orderType.OrderTypeID %>" <%= orderType.OrderTypeID == Model.OrderTypeID ? " selected=\"selected\"" : "" %>>
                        <%= orderType.GetTerm()%></option>
                    <%} %>
                </select
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%: Html.Term("MinimumVolume", "Minimum Volume") %>:
            </td>
            <td>
                 <%: Html.TextBox("MinimumCommissionableTotal", Model.MinimumCommissionableTotal.HasValue ? Model.MinimumCommissionableTotal.Value.ToString("N", CoreContext.CurrentCultureInfo) : "", new { data_inputfilter = @"[\d\.]", @class = "numeric", monedaidioma = "CultureIPN" })%>
               <%-- <%: Html.TextBox("MinimumCommissionableTotal", string.Format("{0:N2}", Model.MinimumCommissionableTotal), new { data_inputfilter=@"[\d\.]" })%>--%>
            </td>
        </tr>
		<tr>
            <td style="width: 13.636em;">
                <%: Html.Term("MaximumRetryCount", "Maximum Retry Count")%>:
            </td>
            <td>
                <%: Html.TextBox("MaxRetryCount", Model.MaximumAttemptsPerInterval, new { data_inputfilter = @"[\d]" })%>
            </td>
        </tr>
		<tr>
            <td style="width: 13.636em;">
                <%: Html.Term("MaximumFailedIntervals", "Maximum Failed Intervals")%>:
            </td>
            <td>
                <%: Html.TextBox("MaxFailedIntervals", Model.MaximumFailedIntervals, new { data_inputfilter = @"[\d]" })%>
            </td>
        </tr>
        <%--<tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Catalog", "Catalog") %>:
            </td>
            <td>
                <select id="catalogId">
                    <%foreach (Catalog catalog in Catalog.LoadAll())
                      { %>
                    <option value="<%= catalog.CatalogID %>" <%= catalog.CatalogID == Model.CatalogID ? " selected=\"selected\"" : "" %>>
                        <%= catalog.Translations.Name() %></option>
                    <%} %>
                </select>
            </td>
        </tr>--%>
        <tr>
            <td>
                <%= Html.Term("AccountTypes", "Account Types") %>:
            </td>
            <td>
                <ul id="accountTypes">
                    <%foreach (AccountType accountType in SmallCollectionCache.Instance.AccountTypes)
                      { %>
                    <li>
                        <input type="checkbox" class="accountType" id="accountTypeCheckBox<%= accountType.AccountTypeID %>"
                            value="<%= accountType.AccountTypeID %>" <%= Model.AccountTypes.Any(at => at.AccountTypeID == accountType.AccountTypeID) ? "checked=\"checked\"" : "" %> />
                        <label for="accountTypeCheckBox<%= accountType.AccountTypeID %>">
                            <%= accountType.GetTerm() %></label></li>
                    <%} %>
                </ul>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Active", "Active") %>:
            </td>
            <td>
                <input id="active" type="checkbox" <%= Model.Active ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
		</tr>
        <tr>
            <td>
                <%= Html.Term("Enrollable", "Is Enrollable") %>:
            </td>
            <td>
                <input id="enrollable" type="checkbox" <%= Model.IsEnrollable ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("Editable", "Is Template Editable") %>:
            </td>
            <td>
                <input id="editable" type="checkbox" <%= Model.IsTemplateEditable ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr id="runDaysContainer">
            <td>
                <%= Html.Term("RunDays", "Run Day(s)") %>:
            </td>
            <td>
                <ul id="runDays">
                    <%for (int i = 1; i < 32; i++)
                      { %>
                    <li>
                        <input type="checkbox" class="runDay" id="runDayCheckBox<%= i %>" value="<%= i %>"
                            <%= Model.AutoshipScheduleDays.Any(asd => asd.Day == i) ? "checked=\"checked\"" : "" %> />
                        <label for="runDayCheckBox<%= i %>">
                            <%= i %></label></li>
                    <%} %>
                </ul>
            </td>
        </tr>
        <tr id="productPanel">
            <td class="FLabel" style="vertical-align: top;">
                <%= Html.Term("Products", "Products") %>:
            </td>
			<td id="productNoAdd" style="display:none">
					<%= Html.Term("AutoshipScheduleNoScheduleId", "The Autoship Schedule must be saved before products can be added.") %>
            </td>
            <td id="productAdd">
                <p class="FL">
                    <%= Html.Term("QuickProductAdd", "Quick Product Add") %>:
                    <input id="productQuickAdd" type="text" class="" value="" />
                    <input type="hidden" id="quickAddProductId" value="" />
                    <%= Html.Term("Quantity", "Quantity") %>:
                    <input id="quickAddQuantity" type="text" class="numeric" value="1" />
                    <a id="btnQuickAdd" href="javascript:void(0);" class="DTL Add">
                        <%= Html.Term("Add", "Add") %></a>
                </p>
                <div class="FR">
                    <p class="FL" style="margin-right: .909em;">
                        <%= Html.Term("ApplyToSelected", "Apply to Selected") %>:</p>
                    <p class="FL">
                        <a id="btnRemove" href="javascript:void(0);" class="">
                            <%= Html.Term("Remove", "Remove") %></a>
                    </p>
                    <span class="Clear"></span>
                </div>
                <span class="ClearAll"></span>
                <!-- Products In Order -->
                <table id="products" width="100%" class="DataGrid">
                    <thead>
                        <tr class="GridColHead">
                            <th class="GridCheckBox">
                                <input id="selectAll" type="checkbox" />
                            </th>
                            <th>
                                <%= Html.Term("SKU", "SKU") %>
                            </th>
                            <th>
                                <%= Html.Term("Product", "Product") %>
                            </th>
                            <th>
                                <%= Html.Term("Quantity", "Quantity") %>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <p class="Pagination">
                    <a href="javascript:void(0);" id="btnPreviousPage" style="color: Black; cursor: default;">
                        &lt;&lt;
                        <%= Html.Term("Previous", "Previous") %></a>&nbsp;&nbsp;&nbsp;<a href="javascript:void(0);" id="btnNextPage"
                            style="color: Black; cursor: default;"><%= Html.Term("Next", "Next") %>
                             &gt;&gt;</a>&nbsp;&nbsp;&nbsp;<%= Html.Term("PageSize", "Page Size") %>:<select id="pageSize">
                                <option selected="selected" value="20">20</option>
                                <option value="50">50</option>
                                <option value="100">100</option>
                            </select>
                </p>
            </td>
        </tr>
    </table>
    <p>
        <a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
                <%= Html.Term("Save", "Save") %></a>
        <a href="<%= ResolveUrl("~/Admin/AutoshipSchedules") %>" class="Button"><span>
            <%= Html.Term("Cancel", "Cancel") %></span></a>
    </p>
</asp:Content>
