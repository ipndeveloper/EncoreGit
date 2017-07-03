<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.IEnumerable<NetSteps.Web.Mvc.Controls.Controllers.Enrollment.AutoshipStep.PrebuiltAutoship>>" %>
<%@ Import Namespace="NetSteps.Web.Mvc.Controls.Controllers.Enrollment" %>

<% var enrollmentContext = ViewBag.EnrollmentContext as NetSteps.Web.Mvc.Controls.Models.Enrollment.EnrollmentContext; %>
<script type="text/javascript">
    $(function () {

        $.get('<%= ResolveUrl("~/Enrollment/Autoship/ShowCustom") %>', function (data) {
            if (data === false) {
                $("#Autoships").find("span:contains('Custom')").parents("li").hide();
            }
        });

        // Tabber
        $(".Tabber li").click(function () {

            // If the tab is selected, don't do anything - else proceed with the following.
            if (!$(this).hasClass("current")) {
                $(".Tabber .current").removeClass("current");
                $(this).addClass("current");
                $(".TabContent").css("display", "none");
                var content_show = $(this).attr("rel");
                $("#" + content_show).css("display", "block");

                var selectedTab = $('#' + content_show);
                var autoshipOrder = $('input:eq(0)', selectedTab).val();

                if (autoshipOrder === "") {
                    $('#selectedAutoShip').val(autoshipOrder);
                    $('#products tbody tr').remove();
                } else {
                    $('#selectedAutoShip').val(radioButtonValue());
                }

                GetAutoShip($('#selectedAutoShip').val());

                hideMessage();
            }

            return false;
        });


        function GetAutoShip(autoshipOrder) {
            $.get('<%= ResolveUrl("~/Enrollment/Autoship/SetSelectedAutoship") %>',
                                { autoshipName: autoshipOrder },
                                function (results) {
                                    if (results.result) {
                                        updateCartAndTotals(results);
                                    }
                                });

        }

        // Get the selected radio button..
        function radioButtonValue() {
            var radioButton = $('input[name="autoshipRadio"]:radio:checked').val();
            if (radioButton === undefined) {
                return $('#prebuildAutoShip').val();
            }

            return radioButton;
        }

        $('input[name="autoshipRadio"]:radio').click(function () {
            GetAutoShip($(this).val());
        });


        //Add the following 4 lines to the top of any addon - DES
        window.enrollmentAccountNumber = '<%= enrollmentContext.EnrollingAccount.AccountNumber %>';
        if (parseBool('<%= ViewData["IsSkippable"] %>')) {
            $('#btnSkip').show();
        }

        $('#btnQuickAdd').click(function () {
            $('#ProductLoad').show();
            var productId = $('#hQuickAddProductId').val(), quantity = parseInt($('#txtQuickAddQuantity').val());
            if (productId && quantity) {
                $.post('<%= ResolveUrl("~/Enrollment/Autoship/AddToCart") %>', { productId: $('#hQuickAddProductId').val(), quantity: parseInt($('#txtQuickAddQuantity').val()) }, function (results) {
                    if (results.result) {
                        $('#hQuickAddProductId,#txtQuickAddSearch,#txtQuickAddQuantity').val('');
                        updateCartAndTotals(results);
                    }
                    else {
                        showMessage('The product could not be added: ' + results.message, true);
                    }
                    $('#ProductLoad').hide();
                });
            }
            hideMessage();
        });



        //        $('#txtQuickAddSearch').jsonSuggest('<%= ResolveUrl("~/Orders/OrderEntry/SearchProducts") %>', { minCharacters: 3, source: $('#txtQuickAddSearch'), ajaxResults: true, width: 250, onSelect: function (item) {
        //            $('#hQuickAddProductId').val(item.id);
        //            $('#txtQuickAddQuantity').val('1');
        //        }
        //        });

        $('#txtQuickAddSearch').jsonSuggest('<%= ResolveUrl("~/Enrollment/Autoship/SearchProducts") %>', { minCharacters: 3, source: $('#txtQuickAddSearch'), ajaxResults: true, width: 250, onSelect: function (item) {
            $('#hQuickAddProductId').val(item.id);
            $('#txtQuickAddQuantity').val('1');
        }
        });

        // Don't use live() for this or it will affect other partial views.
        $('#shippingMethods').delegate('input:radio[name=shippingMethod]', 'click', function () {
            $.post('<%= ResolveUrl("~/Enrollment/Autoship/SetShippingMethod") %>', { shippingMethodId: $(this).val() }, function (results) {
                if (results.result) {
                    updateTotals(results);
                }
                else {
                    showMessage('<%= Html.Term("TheShippingMethodCouldNotBeChanged", "The shipping method could not be changed") %>: ' + results.message, true);
                }
            });
        });

        $('input:radio[name=autoship]').click(function () {
            $('#btnCustomAutoship').prop('checked') && $('#customAutoship').fadeIn('fast') || $('#customAutoship').fadeOut('fast');

            $('#autoshipDay').empty();
            var days;
            if (!$('#btnCustomAutoship').prop('checked')) {
                days = $(this).parent().next().find('.scheduleDays').val().split(',');
            } else {
                days = $('#autoshipSchedule' + $('#autoshipSchedule').val()).text().split(',');
            }
            $('#autoshipDayContainer')[days.length && days.length > 1 ? 'show' : 'hide']();
            for (var i = 0; i < days.length; i++) {
                $('#autoshipDay').append(String.format('<option value="{0}">{0}</option>', days[i]));
            }

            $.post('<%= ResolveUrl("~/Enrollment/Autoship/ChooseAutoship") %>', { autoshipName: $(this).val() }, function (response) {
                if (response.result) {
                    if (!$('#btnCustomAutoship').prop('checked')) {
                        $('#products tbody:first').empty();
                    }
                    updateTotals(response);
                } else {
                    showMessage(response.message, true);
                }
            });
        });

        $('#autoshipSchedule').change(function () {
            $('#autoshipDay').empty();
            var days = $('#autoshipSchedule' + $(this).val()).text().split(',');
            $('#autoshipDayContainer')[days.length ? 'show' : 'hide']();
            for (var i = 0; i < days.length; i++) {
                $('#autoshipDay').append(String.format('<option value="{0}">{0}</option>', days[i]));
            }
        });

        $('#btnNext').click(function () {
            // Do validation here
            if ($('#selectedAutoShip').val() === "") {
                // Check to see if atleast one orderItem was added
                if (!CheckCustomOrderExists()) {
                    showMessage('<%= Html.Term("AtleastOneOrderItemMustBeAddedToTheCart", "Atleast one order item must be added to the cart") %>', true);
                    return false;
                }
            }

            var data = getData();
            if (data === false)
                return false;

            window.letUnload = false;
            enrollmentMaster.postStepAction({
                step: "Autoship",
                stepAction: "SubmitStep",
                data: data,
                showLoadingElement: $(this).parent(),
                load: true
            });
        });

        $('#btnSkip').click(function () {
            window.letUnload = false;
            enrollmentMaster.postStepAction({
                step: "Autoship",
                stepAction: "SkipStep",
                load: true
            });
        });
    });

    /** Atleast one orderItem must exists **/
    function CheckCustomOrderExists() {
        if ($('#products tbody:first >tr').length > 0) {
            hideMessage();
            return true;
        }
        return false;
    }


    function remove(orderItemGuid) {
        $('#ProductLoad').show();
        var t = $(this);
        $.post('<%= ResolveUrl("~/Enrollment/Autoship/RemoveFromCart") %>', { orderItemGuid: orderItemGuid }, function (results) {
            if (results.result) {
                $('#oi' + orderItemGuid).remove();
                updateCartAndTotals(results);
                $('#ProductLoad').hide();
                if (results.message !== undefined && results.message.length > 0) {
                    showMessage(results.message, true);
                }
            }
            else {
                showMessage('<%= Html.Term("ErrorRemovingProduct", "The product could not be removed")%>: ' + results.message, true);
                $('#ProductLoad').hide();
            }
        });
    }

    function updateCartAndTotals(results) {
        if (results.orderItems) {
            var orderItem, i;
            for (i = 0; i < results.orderItems.length; i++) {
                orderItem = $('#oi' + results.orderItems[i].orderItemId);
                if (orderItem.length) {
                    orderItem.replaceWith(results.orderItems[i].orderItem);
                } else {
                    $('#products tbody:first').append(results.orderItems[i].orderItem);
                }
            }
        }

        // refresh the totals
        updateTotals(results);
        $('#shippingMethods').html(results.shippingMethods);
    }

    function updateTotals(results) {
        $.each(['subtotal', 'commissionableTotal', 'taxTotal', 'shippingTotal', 'grandTotal'], function (i, item) {
            $('.' + item).text(results.totals[item]);
        });
    }

    function getData() {
        return {
            autoshipScheduleId: $('#autoshipSchedule').val(),
            autoshipScheduleDay: $('#autoshipDay').val()
        };
    }
</script>
<div class="StepGutter">
    <h3>
        <b>
            <%= Html.Term("EnrollmentStep", "Step {0}", ViewData["StepCounter"])%></b>
        <%= Html.Term("ChooseAnAutoship", "Choose an autoship") %></h3>
</div>
<div class="StepBody">
    <ul class="Tabber" id="Autoships">
        <li class="current" rel="content_1"><a href="#"><span>Pre-built Autoship</span></a></li>
        <li rel="content_2"><a href="#"><span>Build a Custom Autoship</span></a></li>
    </ul>
    <span class="ClearAll"></span>

    <input type="hidden" id="selectedAutoShip" value="<%= Model.First().Name %>" />

    <%--TODO: What happened to the radio button selectors next to each autoship???--%>
    <!-- Pre-built Autoship -->
    <div id="content_1" class="Autoships TabContent">
        <%if (Model.Count() > 0) {%>

        <input type="hidden" name="autoship" id="prebuildAutoShip" value="<%= Model.First().Name%>" />
          <% foreach (AutoshipStep.PrebuiltAutoship prebuiltAutoship in Model)
              { %>
        <div class="prebuiltAutoship">
            <%--<h3>
                <%= prebuiltAutoship.Name %></h3>--%>
            <p class="description">
                <%= prebuiltAutoship.Description %></p>
            <input type="hidden" class="scheduleDays" value="<%= prebuiltAutoship.AutoshipSchedule.AutoshipScheduleDays.Select(asd => asd.Day).Join(",") %>" />

            <div style="float:left;">
            <% if (Model.Count() > 1)
               { %>
            <input type="radio" name="autoshipRadio" value="<%= prebuiltAutoship.Name %>" <%= prebuiltAutoship == Model.ElementAt(0) ? "checked=\"checked\"" : "" %> />
            <% } %>
            </div>   
            <div>
            <table class="DataGrid" width="95%">
                <thead>
                    <tr class="GridColHead">
                        <th>
                            <%= Html.Term("SKU") %>
                        </th>
                        <th>
                            <%= Html.Term("Product") %>
                        </th>
                        <th>
                            <%= Html.Term("PricePerItem", "Price Per Item") %>
                        </th>
                        <th style="width: 9.091em;">
                            <%= Html.Term("Quantity") %>
                        </th>
                        <th>
                            <%= Html.Term("Price") %>
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <% int count = 0;
											 var inventory = NetSteps.Encore.Core.IoC.Create.New<InventoryBaseRepository>();
                       foreach (KeyValuePair<string, int> autoshipProduct in prebuiltAutoship.Products)
                       {
                           Product product = inventory.GetProduct(autoshipProduct.Key); %>
                    <tr class="GridRow<%= count % 2 == 1 ? "Alt" : "" %>">
                        <td>
                            <%= autoshipProduct.Key %>
                        </td>
                        <td>
                            <%= product.Name %>
                        </td>
                        <td>
                            <%= product.GetPrice((int)ViewData["AccountTypeID"], enrollmentContext.CurrencyID).ToString(enrollmentContext.CurrencyID)%>
                        </td>
                        <td>
                            <%= autoshipProduct.Value %>
                        </td>
                        <td>
                            <%= (autoshipProduct.Value * product.GetPrice((int)ViewData["AccountTypeID"], enrollmentContext.CurrencyID)).ToString(enrollmentContext.CurrencyID)%>
                        </td>
                    </tr>
                    <%++count;
                       } %>
                </tbody>
            </table>
            </div>
        </div>
        <%} %>
    </div>

    <!-- Custom Autoship -->
    <div id="content_2" class="Autoships TabContent" style="display: none;">
        <input id="btnCustomAutoship" type="hidden" name="autoship" value="" />
        <%} %>
        <div id="customAutoship">
            <%var schedules = AutoshipSchedule.LoadAllFull().Where(a => !a.Name.ContainsIgnoreCase("Subscription") && a.AccountTypes.Select(at => at.AccountTypeID).Contains(enrollmentContext.EnrollingAccount.AccountTypeID)); %>
            <select id="autoshipSchedule" <%= schedules.Count() < 2 ? "style=\"display:none;\"" : "" %>>
                <% foreach (AutoshipSchedule schedule in schedules)
                   { %>
                <option value="<%= schedule.AutoshipScheduleID %>">
                    <%= schedule.GetTerm() %></option>
                <%} %>
            </select>
            <div style="display: none;">
                <%foreach (AutoshipSchedule schedule in schedules)
                  { %>
                <span id="autoshipSchedule<%= schedule.AutoshipScheduleID %>" style="display: none;">
                    <%= schedule.AutoshipScheduleDays.Select(asd => asd.Day).Join(",") %></span>
                <%} %>
            </div>
            <div class="QuickAdd">
                <%= Html.Term("SKUorName", "SKU or Name") %>:
                <input id="txtQuickAddSearch" type="text" style="width: 20.833em;" />
                <input id="hQuickAddProductId" type="hidden" />
                <%= Html.Term("Quantity") %>:
                <input id="txtQuickAddQuantity" type="text" class="Short quantity" style="width: 4.167em" />
                <a id="btnQuickAdd" href="javascript:void(0);" class="DTL Add">
                    <%= Html.Term("AddToOrder", "Add to Order") %></a> <span class="ClearAll"></span>
            </div>
            <table id="products" width="100%" class="DataGrid">
                <thead>
                    <tr class="GridColHead">
                        <th class="GridCheckBox">
                        </th>
                        <th>
                            <%= Html.Term("SKU") %>
                        </th>
                        <th>
                            <%= Html.Term("Product") %>
                        </th>
                        <th>
                            <%= Html.Term("PricePerItem", "Price Per Item") %>
                        </th>
                        <th style="width: 9.091em;">
                            <%= Html.Term("Quantity") %>
                        </th>
                        <th>
                            <%= Html.Term("Price") %>
                        </th>
                    </tr>
                </thead>
                <tbody>
                </tbody>
                <tbody>
                    <tr id="productTotalBar" class="GridTotalBar">
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                        </td>
                        <td>
                            <b><span class="subtotal">
                               <%-- <%= ((decimal)0).ToString(enrollmentContext.CurrencyID)%></span>--%>
                                <%= ((decimal)0).ToString("C",CoreContext.CurrentCultureInfo)%></span>
                                (<%= Html.Term("SubTotal", "Sub Total") %>)</b>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
        <!--/end custom built -->
    </div>

    <p id="autoshipDayContainer" style="display: none;">
        <%= Html.Term("DayOfTheMonth", "Day of the month") %>:
        <select id="autoshipDay">
        </select>
    </p>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("ShippingMethod", "Shipping Method") %>
            </td>
            <td id="shippingMethods">
                <% bool requiresShipping = (bool)ViewData["RequiresShippingMethod"];
                   if (requiresShipping)
                   {
                       IEnumerable<ShippingMethodWithRate> shippingMethods = (ViewData["ShippingMethods"] as IEnumerable<ShippingMethodWithRate>).OrderBy(sm => sm.ShippingAmount);
                       int selectedShippingMethodId = shippingMethods.FirstOrDefault() == default(ShippingMethodWithRate) ? 0 : shippingMethods.FirstOrDefault().ShippingMethodID;
                       foreach (ShippingMethodWithRate shippingMethod in shippingMethods)
                       { %>
                <div class="FL AddressProfile">
                    <input id="shippingMethod<%= shippingMethod.ShippingMethodID %>" type="radio" name="shippingMethod"
                        class="Radio" value="<%= shippingMethod.ShippingMethodID %>" <%= shippingMethod.ShippingMethodID == selectedShippingMethodId ? "checked=\"checked\"" : "" %> />
                    <b>
                        <%= Html.Term(shippingMethod.Name)%></b><br />
					<!--Commenting out the tax and s&h for now until the enrollment process gets fixed.-->
                    <!--<%= shippingMethod.ShippingAmount.ToString(enrollmentContext.CurrencyID)%>-->
                </div>
                <% } %>
                <% } %>
                <% else %>
                <% { %>
                <b>
                    <%= Html.Term("NotApplicable", "N/A") %>
                </b>
                <% } %>
            </td>
        </tr>
    </table>
    <table width="100%" class="DataGrid">
        <tr id="totalBar" class="GridTotalBar">
            <td colspan="2" style="text-align: right;">
                <div class="FL Loading" id="TotalsLoad">
                </div>
                <%= Html.Term("Subtotal") %>:<br />

                <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%string valorSCV = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "SCV");
                                if (valorSCV == "S")
                                { %>
                <%= Html.Term("CommissionableTotal", "Commissionable Total") %>:<br />
                <%}%>
                <%--CS.03MAY2016.Fin.Muestra CV--%>

<!-- Commenting out the tax and s&h for now until the enrollment process gets fixed.
                <%= Html.Term("Tax") %>:<br />
                <%= Html.Raw(Html.Term("S&H", "S&amp;H"))%>:<br />
-->
                <%= Html.Term("OrderTotal", "Order Total") %>:
            </td>
            <td>

               <span class="subtotal">
					<%= ViewData["Subtotal"] %></span>
				<br />

                 
                 <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%if (valorSCV == "S"){%>
				<span class="commissionableTotal">
					<%= ViewData["Commissionable"] %></span>
				<br />
                <%}%>
                <%--CS.03MAY2016.Fin.Muestra CV--%>


<!-- Commenting out the tax and s&h for now until the enrollment process gets fixed.
				<span class="taxTotal">
					<%= ViewData["Tax"]%></span>
				<br />
				<span class="shippingTotal">
					<%= ViewData["Shipping"]%></span>
				<br />
-->
				<b><span class="grandTotal">
					<%= ViewData["GrandTotal"]%></span></b>

            </td>
        </tr>
    </table>
</div>
<span class="ClearAll"></span>
<p class="Enrollment SubmitPage">

    <a id="btnNext" href="javascript:void(0);" class="Button BigBlue"><%= Html.Term("Next") %>&gt;&gt;</a>
    <a id="btnSkip" href="javascript:void(0);" class="Button" style="display: none;"><%= Html.Term("Skip") %>&gt;&gt;</a>
</p>
