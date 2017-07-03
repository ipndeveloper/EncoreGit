<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Accounts.Models.Shared.YellowWidgetModel>" %>
<%     
    NetSteps.Data.Common.Entities.IAccountTitle accountPaidAsTitle = null;
    NetSteps.Data.Common.Entities.IAccountTitle accountRecognitionTitle = null; 
	/* CGI(AHAA) - 2597 - Inicio */
    string paidAsTitle, careerTitle;
    paidAsTitle = Model.PaidAsTitle;
    careerTitle = Model.CareerTitle;
    /* CGI(AHAA) - 2597 - Fin */
    if (Model.UsesCommissions)
    {
        accountPaidAsTitle = Model.AccountPaidAsTitle;
        accountRecognitionTitle = Model.AccountRecognitionTitle;
    }
        
    Address mainAddress = null;
    string cultureInfoCode = NetSteps.Common.Globalization.CountryCultureInfoCode.UnitedStates;

    if (Model.CurrentAccount.Addresses.Count > 0 && Model.CurrentAccount.Addresses.GetAllByTypeID(Constants.AddressType.Main).Count > 0)
    {
        mainAddress = Model.CurrentAccount.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
       
        if (mainAddress != null)
        {
            cultureInfoCode = SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).CultureInfo;
        }
    }
%>
<script type="text/javascript">
    $(function () {
        $('#statusChangeModal').jqm({ trigger: '#btnChangeStatus' });
        $('#btnSaveStatus').click(function () {
            var data = {
                statusId: $('#sStatus').val(),
                changeReasonId: $('#sReason').val()
            };
            $.post('<%= ResolveUrl("~/Accounts/Edit/SaveStatus") %>', data, function (response) {
                if (response.result) {
                    $('#btnChangeStatus').text($.trim($('#sStatus option:selected').text()));
                    showMessage('<%= Html.Term("AccountSaved", "Account saved successfully") %>', false);
                    window.location = window.location.pathname;
                }
                else {
                    showMessage(response.message, true);
                    return false;
                }
            });
        });

        $("#enrollmentDate").datepicker('destroy').datepicker({ changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100',
            onSelect: function (dateText, inst) {
                var data = {
                    accountNumber: '<%= Model.CurrentAccount.AccountNumber %>',
                    enrollmentDate: $('#enrollmentDate').val()
                };
                $.post('<%= ResolveUrl("~/Accounts/Edit/ChangeEnrollmentDate") %>', data, function (response) {
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

        $("#nextRenewal").datepicker('destroy').datepicker({ changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100',
            onSelect: function (dateText, inst) {
                var data = {
                    accountNumber: '<%= Model.CurrentAccount.AccountNumber %>',
                    nextRenewal: $('#nextRenewal').val()
                };
                $.post('<%= ResolveUrl("~/Accounts/Edit/ChangeNextRenewalDate") %>', data, function (response) {
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

    });
</script>
<div class="TagInfo">
    <div class="Content">
    <div id="fullNameYellowWidget">
        <h1>
            <%= Html.Encode(Model.CurrentAccount.FullName)%></h1>
            </div>
        <p class="DistributorStatus">
            <%= Html.Encode("#" + Model.CurrentAccount.AccountNumber + ", " + Html.Term("SSN") + ":  " + (Model.CurrentAccount.DecryptedTaxNumber.MaskString(4)))%>
            <br />
            <%= Html.Term("Phone")%>:
            <%= Html.Encode(Model.CurrentAccount.MainPhone.FormatPhone(cultureInfoCode))%>
            <% if (mainAddress != null)
                   Response.Write("<br />" + mainAddress.City + ", " + mainAddress.State);
            %>
        </p>
        <hr />
        <table class="DetailsTag Section">
            <tbody>
                <tr>
                    <td>
                        <%= Html.Term("Type") %>:
                    </td>
                    <td>
                        <%: SmallCollectionCache.Instance.AccountTypes.GetById(Model.CurrentAccount.AccountTypeID).GetTerm()%>
                    </td>
                </tr>              
                <%if (accountPaidAsTitle != null)
                 {%>
                    <tr>
                        <td>
                            <%= Html.Term("Paid-As")%>:
                        </td>
                        <td>
                            <%= Html.Term(accountPaidAsTitle.Title.TermName)%>                            
                        </td>
                    </tr>
                <% } %>
                <%if (accountRecognitionTitle != null)
                 {%>
                    <tr>
                        <td>
                            <%= Html.Term("Recognition") %>:
                        </td>
                        <td>
                            <%= Html.Term(accountRecognitionTitle.Title.TermName)%>                            
                        </td>
                    </tr>
                <% } %>
				<%if (careerTitle != "")
                  {%>
                <tr>
                    <td>
                        <%= Html.Term("CurrentTitle")%>:
                    </td>
                    <td>
                        <%= careerTitle%>
                    </td>
                </tr>
                <% } %>						
                <tr>
                    <td>
                        <%= Html.Term("Status") %>:
                    </td>
                    <td>
                        <a id="btnStatus">
                            <%: SmallCollectionCache.Instance.AccountStatuses.GetById(Model.CurrentAccount.AccountStatusID).GetTerm()%>
                        </a>                        
                    </td>
                </tr>
                             
                    <tr>
                        <td>
                            <%= Html.Term("Blocking")%>:
                        </td>
                        <td>
                         <a href="<%= ResolveUrl("~/Accounts/Overview/Index/") +  Model.CurrentAccount.AccountNumber %>">
                             <%=Convert.ToString(Session["IsLocked"])%></a>
                       
                        </td>
                    </tr>
                
                <tr>
                    <td>
                        <%= Html.Term("Placement") %>:
                    </td>
                    <td id="accountDetailsSponsor">
                        <% if (Model.CurrentAccount.SponsorInfo != null)
                           { %>
                        <a href="<%= ResolveUrl("~/Accounts/Overview/Index/") + Model.CurrentAccount.SponsorInfo.AccountNumber %>">
                            <%: Model.CurrentAccount.SponsorInfo.AccountNumber + " - " + Model.CurrentAccount.SponsorInfo.FullName%></a>
                        <% } %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Html.Term("Enroller") %>:
                    </td>
                    <td id="Td1">
                        <% if (Model.CurrentAccount.EnrollerInfo != null)
                           { %>
                        <a href="<%= ResolveUrl("~/Accounts/Overview/Index/") + Model.CurrentAccount.EnrollerInfo.AccountNumber %>">
                            <%: Model.CurrentAccount.EnrollerInfo.AccountNumber + " - " + Model.CurrentAccount.EnrollerInfo.FullName%></a>
                        <% } %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Html.Term("Enrolled", "Enrolled") %>:
                    </td>
                    <td> 
                       <a> <%= Html.Encode(Model.CurrentAccount.EnrollmentDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))%></a>  
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Html.Term("Renewal", "Renewal") %>:
                    </td>
                    <td>
                        <% 
                            if (NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUser.HasFunction("Accounts-Edit NextRenewalDate"))
                            {
                        %>
                        <input class="TextInput DatePicker" type="text" id="nextRenewal" value="<%= Model.CurrentAccount.NextRenewal.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo) %>" />
                        <% }
                            else
                            { %>
                        <%= Html.Encode(Model.CurrentAccount.NextRenewal.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))%>
                        <% } %>
                    </td>
                </tr>
                <% if (Model.CurrentAccount.AccountStatusID == (short)Constants.AccountStatus.Terminated && Model.CurrentAccount.TerminatedDate.HasValue) %>
                <% { %>
                <tr>
                    <td>
                        <%= Html.Term("Terminated", "Terminated")%>:
                    </td>
                    <td>
                        <%= Html.Encode(Model.CurrentAccount.TerminatedDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))%>
                    </td>
                </tr>
                <% } %>
                <tr>
                    <td>
                        <%= Html.Term("Tickets", "Tickets")%>:
                    </td>
                    <td>
                        <a href="<%= ResolveUrl("~/Support?accountID=" + Model.CurrentAccount.AccountID) %>">
                            <%= Html.Term("ViewAllTickets", "View All Tickets") %></a>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <a href="<%= ResolveUrl("~/Support/Ticket/CreateTicketForAccount?accountID=" + Model.CurrentAccount.AccountID) %>">
                            <%= Html.Term("NewSupportTicket", "New Support Ticket")%></a>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
<div id="statusChangeModal" class="LModal jqmWindow StatusWin">
    <div class="mContent">
        <h2>
            <%= Html.Term("ChangeStatus", "Change Status") %></h2>
        <p>
            <span class="FL Label">
                <%= Html.Term("Status") %>:</span>
            <select id="sStatus">
                <% foreach (var accountStatus in SmallCollectionCache.Instance.AccountStatuses)
                   { %>
                    <%if (!Model.AccountStatusesExclude.Contains(accountStatus.GetTerm()))
                     { %>
                         <option value="<%= accountStatus.AccountStatusID %>" <% if(accountStatus.AccountStatusID == Model.CurrentAccount.AccountStatusID) { %>
							selected="selected" <% } %>>
							<%= accountStatus.GetTerm()%></option>
                    <%} %>
               
                <%} %>
            </select>
        </p>
        <br />
        <p>
            <span class="FL Label">
                <%= Html.Term("Reason") %>:</span>
            <select id="sReason">
                <% foreach (var changeReason in SmallCollectionCache.Instance.AccountStatusChangeReasons)
                   { %>
                <option value="<%= changeReason.AccountStatusChangeReasonID %>">
                    <%= changeReason.Name %></option>
                <%} %>
            </select>
        </p>
        <br />
        <br />
        <p>
            <a id="btnSaveStatus" class="Button BigBlue jqmClose" href="javascript:void(0);">
                <%= Html.Term("SaveStatus", "Save Status") %></a>
            <a href="javascript:void(0);" class="Button jqmClose">
                <%= Html.Term("Cancel") %></a>
        </p>
        <span class="ClearAll" />
        <br />
    </div>
</div>
