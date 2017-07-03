<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Order>" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<% var isSubdomain = NetSteps.Common.Configuration.ConfigurationManager.GetAppSetting<bool>(NetSteps.Common.Configuration.ConfigurationManager.VariableKey.UrlFormatIsSubdomain, true); %>
    <% var autoshipSchedule = (ViewData["AutoshipSchedule"] as NetSteps.Data.Entities.AutoshipSchedule); %>
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/jquery.inputfilter.js") %>"></script>
   

    <script type="text/javascript">
        $(function () {
            $('#commissionDate').datepicker({
                changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100', onSelect: function (dateText, inst) {
                    $.post('<%= ResolveUrl("~/Accounts/Autoships/ChangeCommissionDate") %>', { commissionDate: $('#commissionDate').val() }, function (response) {
                        if (response.result) {
                            showMessage('<%= Html.Term("AccountSaved", "Account saved successfully") %>', false);
                        }
                        else {
                            showMessage(response.message, true);
                            return false;
                        }
                    });
                }
            });

            $('#changeCommissionConsultantModal').jqm({ trigger: '#btnShowChange',
                onShow: function (h) {
                    h.w.css({
                        top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                        left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                    }).fadeIn();
                }
            });

            $('#btnSaveAutoship').click(function () {
                ValidateDataAndSaveAutoship();
            });

            function ValidateDataAndSaveAutoship() {

                //this includes SKU and quantity when they're aren't needed
                //	and no other required fields are currently handled this way
                //if(!$('#wholeForm').checkRequiredFields()){
                //    return false;
                //}
                if ($(this).hasClass('ButtonOff')) {
                    return false;
                }
                if (!$('#sShippingAddress').val()) {
                    showMessage('<%= Html.Term("PleaseSelectaShippingAddress", "Please select a shipping address")%>', true);
                    return false;
                }
                if (!$('#sPaymentMethod').val()) {
                    showMessage('<%= Html.Term("PleaseSelectaPaymentMethod", "Please select a payment method.")%>', true);
                    return false;
                }
                if (!checkForEmptyUrls()) {
                    showMessage('<%= Html.Term("URLCannotBeEmpty", "URL cannot be empty.") %>', true);
                    return false;
                }
                // Ensure there are no duplicate URLs.
                if (!checkDuplicateUrlsForUser('<%= isSubdomain %>')) {
                    showMessage('<%= Html.Term("URLsMustBeUnique", "URLs must be unique.")%>', true);
                    return false;
                }
                $('#autoshipWait').jqmShow();

                // Ensure that URLs are unique across the site.
                var data = {};

                populateSiteURLList(data, '<%= isSubdomain %>');

                $.post('<%= ResolveUrl("~/Accounts/Autoships/CheckForDuplicateUrls") %>', data, function (response) {
                    if (response.result !== undefined && !response.result) {
                        HideLoading();
                        showMessage(response.message, true);
                        return false;
                    }

                    if (response.count() > 0) {
                        HideLoading();
                        var message = '<%= Html.Term("URLsNotUnique", "The following URLs are not unique: ")%>' + '<br />';
                        $(response).each(function () {
                            message = message + this + '<br />';
                        });
                        showMessage(message, true);
                        return false;
                    }
                    else {
                        SaveAutoship();
                    }
                });
            }


            function HideLoading() {
                $('#autoshipWait').jqmHide();
            }

            function SaveAutoship() {
                var nextRunDate = $('#nextRunDate').val();
                if (nextRunDate == '') {
                    showMessage('<%= Html.Term("NextRunDateMustBeSelected", "Next run date must be selected")%>', true);
                    HideLoading();
                    return false;
                }

                var data = {
                    scheduleId: '<%= autoshipSchedule.AutoshipScheduleID %>',
                    invoiceNotes: $('#txtInvoiceNotes').val(),
                   // status: $('#sOrderStatus').val(),
                    paymentMethodId: $('#sPaymentMethod').val(),
                    autoshipScheduleDay: $('#autoshipScheduleDay').val(),
                    nextRunDate: nextRunDate,
                    endDate: $('#endDate').val(),
                    startDate: $('#startDate').val()
                };

                if ($('#siteUrls').length) {
                    if (!$('#siteDetails').checkRequiredFields()) {
                        return false;
                    }
                    data.siteName = $('#siteName').val();
                    data.siteDescription = $('#siteDescription').val();
                    data.siteStatusId = $('#siteStatusId').val();
                    data.siteDefaultLanguageId = $('#siteDefaultLanguageId').val();

                    populateSiteURLList(data, '<%= isSubdomain %>');
                }

                $.post('<%= ResolveUrl("~/Accounts/Autoships/Submit") %>', data, function (response) {
                    if (response.result) {
                        //window.location = '<%= ResolveUrl("~/Accounts/Autoships/Edit/") + CoreContext.CurrentAccount.AccountID + "?autoshipScheduleId=" + autoshipSchedule.AutoshipScheduleID %>';
                        window.location = '<%= ResolveUrl("~/Accounts/Overview") %>';
                    }
                    else {
                        showMessage('<%= Html.Term("TheOrderCouldNotBeSubmitted", "The order could not be submitted")%>: ' + response.message, true);
                        $('#autoshipWait').jqmHide();
                        return false;
                    }
                });
            }

            $('#cancelAutoship').click(function () {
                var warningMessage = '<%= Html.Term("AreYouSureYouWantToCancelThisAutoship", "Are you sure you want to cancel this autoship.")%>';
                if (confirm(warningMessage)) {
                    $.ajax({
                        type: 'POST',
                        data: {scheduleId: '<%= autoshipSchedule.AutoshipScheduleID %>'},
                        url: '<%: Url.Action("CancelAutoship") %>',
                        success: function (data) {
                            if (data.success) {
                                window.location = '<%= ResolveUrl("~/Accounts/Overview") %>';
                            } else {
                                showMessage('<%= Html.Term("ErrorCancelingAutoship", "Error canceling autoship") %>', true);
                            }
                        }
                    });
                }
            });

        });
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Accounts") %>">
        <%= Html.Term("Accounts", "Accounts")%></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
            <%= CoreContext.CurrentAccount.FullName %></a> >
    <%= (ViewData["AutoshipSchedule"] as NetSteps.Data.Entities.AutoshipSchedule).AutoshipScheduleTypeID ==(int)Constants.AutoshipScheduleType.Normal ? Html.Term("Autoship") : Html.Term("Subscription") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% var autoshipSchedule = (ViewData["AutoshipSchedule"] as NetSteps.Data.Entities.AutoshipSchedule); %>
    <div id="autoshipWait" class="PModal WaitWin">
        <%= Html.Term("SavingAutoship", "Please wait while we save your autoship...")%>
        <br />
        <img src="<%= ResolveUrl("~/Content/Images/processing.gif") %>" alt="<%= Html.Term("processing", "processing...")%>" />
    </div>
    <!-- Section Header -->
    <div class="SectionHeader">
        <h2><%= autoshipSchedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.Normal ? Html.Term("Autoship", "Autoship") : Html.Term("Subscription", "Subscription")%></h2>
          
          <%= Html.Term("EditTemplate", "Edit Template")%>
          <%if (!(bool)ViewData["NewAutoship"]) { %> 
            | <a href="<%= ResolveUrl("~/Accounts/Autoships/View/") + CoreContext.CurrentAutoship.AutoshipOrderID %>"><%= Html.Term("ViewOrders", "View Orders")%></a>
            | <a href="<%= ResolveUrl("~/Accounts/Autoships/AuditHistory/") + CoreContext.CurrentAutoship.AutoshipOrderID %>"> <%= Html.Term("AuditHistory", "Audit History") %></a>
          <% } %>
    </div>
    <%if ((bool)ViewData["NewAutoship"])
      { %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
        -moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0;
        border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold;
        margin-bottom: 10px; padding: 7px;">
        <div style="color: #385E0F; display: block;" class="message creatingAutoShipMessage">
            <span class="UI-icon icon-check"></span><%= Html.Term("NewAutoship", "You are creating a new autoship.") %></div>
    </div>
    <% } %>

    <div id="wholeForm">

    <% if (autoshipSchedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.SiteSubscription)
       {
           Site site = ViewData["Site"] as Site;
           Html.RenderPartial("SiteSubscriptions", site);
       } %>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= autoshipSchedule.AutoshipScheduleTypeID == (int)Constants.AutoshipScheduleType.Normal ? Html.Term("AutoshipDetails", "Autoship Details") : Html.Term("SubscriptionDetails", "Subscription Details")%>:
            </td>
            <td>
                <b>
                    <%= Html.Term("OrderID", "Order ID")%>:</b>
                <%= Model.OrderNumber %><br />
                <b>
                    <%= Html.Term("Placement")%>:
                </b> 
                <span class="consultant">
                    <%: Html.Link(Model.ConsultantInfo.FullName, false, ResolveUrl("~/Accounts/Overview/Index/") + Model.ConsultantInfo.AccountNumber)%>
                </span>
                &nbsp;&nbsp;
                <a href="javascript:void(0);" id="btnShowChange">
                    (<%= Html.Term("change", "change")%>)
                </a>
                <br />
                <div id="changeCommissionConsultantModal" class="jqmWindow LModal ChangeWin">
                    <div class="mContent">
                        <% ViewData["Function"] = "Accounts-Change Commission Consultant";
                           Html.RenderPartial("Authorize"); %>
                    </div>
                </div>
               <%-- <b>
                    <%= Html.Term("Status")%>:</b>
                <select id="sOrderStatus">
                    <option value="<%= (int)Constants.OrderStatus.Paid %>" <%= Model.OrderStatusID != (int)Constants.OrderStatus.Cancelled ? "selected=\"selected\"" : "" %>>
                        <%= Html.Term("Submitted", "Submitted")%></option>
                    <option value="<%= (int)Constants.OrderStatus.Cancelled %>" <%= Model.OrderStatusID == (int)Constants.OrderStatus.Cancelled ? "selected=\"selected\"" : "" %>>
                        <%= Html.Term("Canceled", "Canceled")%></option>
                </select><br />--%>
                
                <b><%= Html.Term("StartDate", "Start Date")%>:</b>

                <%: Html.TextBox("startDate", ViewBag.Dates.StartDate as string, new{ @class = "TextInput DatePicker StartDate required" }) %>

                <br />

                <b><%= Html.Term("EndDate", "End Date")%>:</b>

                <%: Html.TextBox("endDate", ViewBag.Dates.EndDate as string, new{ @class = "TextInput DatePicker StartDate required" }) %>

                <br />

                <b><%= Html.Term("NextDueDate", "Next Due Date")%>:</b>

                <%: Html.TextBox("nextRunDate", ViewBag.Dates.NextRunDate as string, new { @class = "TextInput DatePicker StartDate required" })%>
                <br />
                 
                <%if (autoshipSchedule.AutoshipScheduleDays.Count > 1) { 
                      int autoshipDay = (ViewData["AutoshipDay"] != null) ? (int)ViewData["AutoshipDay"] : 0;
                 %>
                  <b><%= Html.Term("AutoshipScheduleDay", "Schedule Day") %>:</b>

                  <%: Html.DropDownList("autoshipScheduleDay", autoshipSchedule.AutoshipScheduleDays.ToSelectListItems(autoshipDay)) %>

                  <br />
                <%} %>
                <br />
                
                <% if (!(bool)ViewData["NewAutoship"])
                   { %>
                    <%: Html.LinkWithSpan("Cancel Autoship", cssClasses: "Button BigBlue", id: "cancelAutoship") %>
                <% } %>
            </td>
        </tr>
    </table>
    <%Html.RenderPartial("~/Areas/Orders/Views/Shared/PartialOrderEntry.ascx", new OrderEntryModel(Model)); %>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                &nbsp;
            </td>
            <td>
                <p class="NextSection">
                    <a id="btnSaveAutoship" href="javascript:void(0);" class="Button BigBlue"><span>
                        <%= Html.Term("SaveTemplate", "Save Template")%>
                        &gt;&gt;</span></a>
                </p>
            </td>
        </tr>
    </table>

    </div>
    <% Html.RenderPartial("AddressValidation"); %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>
