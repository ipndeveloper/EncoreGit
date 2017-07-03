<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% 
    NetSteps.Data.Entities.Business.AccountInformacion objAccountInformacion = Model as NetSteps.Data.Entities.Business.AccountInformacion;
    NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();

    if (ticketToEdit.SupportTicketID > 0)
    {

        NetSteps.Data.Entities.Account account = ticketToEdit.Account;
        objAccountInformacion = Account.ListarAccountsInformacionAdicional(account.AccountID);
        Address mainAddress = null;
        string cultureInfoCode = NetSteps.Common.Globalization.CountryCultureInfoCode.UnitedStates;
        if (account != null && account.Addresses.Count > 0 && account.Addresses.GetAllByTypeID(Constants.AddressType.Main).Count > 0)
        {
            mainAddress = account.Addresses.GetDefaultByTypeID(Constants.AddressType.Main);
            if (mainAddress != null)
                cultureInfoCode = SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).CultureInfo;
        }
%>
<div class="TagInfo">
    <div class="Content">
        <h1>
            <%= Html.Term("Ticket#","Ticket #") + ticketToEdit.SupportTicketNumber %>
        </h1>
        <hr />
        <span>
            <%= Html.Term("TicketOwner", "Ticket Owner") %>:</span>
        <h1>
            <a href="<%= ResolveUrl("~/Accounts/Overview/Index/" + ticketToEdit.Account.AccountNumber) %>"
                title="Go to account record">
                <%= ticketToEdit.Account.FullName %></a></h1>
        <p class="DistributorStatus">
            <%
        var taxNumber = ticketToEdit.Account.MaskedTaxNumber;
            %>
            #<%= ticketToEdit.Account.AccountNumber %>,
            <%= Html.Term("SSN") %>:
            <%= string.IsNullOrEmpty(taxNumber) ? Html.Term("NA") : taxNumber %>
            <br />
            <%= Html.Encode(account.MainPhone.FormatPhone(cultureInfoCode))%>
            <% if (mainAddress != null)
                   Response.Write("<br />" + mainAddress.City + ", " + mainAddress.State);
            %>
            <%-- inicio 22/03/2016 update by lpulido--%>
            <% Response.Write("<table>"); %>
            <% Response.Write("<tr><td width='100px' >" + Html.Term("DateOfBirth", "DateOfBirth") + ": </td colespan> <td> <span style='color:#0080FF'>" + objAccountInformacion.Birthday.ToShortDateString() + "</span>"); %>
            <% if (objAccountInformacion.Birthday.Month == DateTime.Now.Month && objAccountInformacion.Birthday.Day == DateTime.Now.Day)  %>
            <%{%>
            <% Response.Write("<div style='display: inline'>");%>
            <img src="<%= ResolveUrl("~/Content/Images/BirthDay.png") %>" width="16px" />
            <% Response.Write("</div>");%>
            <%}%>
            <% Response.Write("      </td></tr>");%>
            <% Response.Write("<tr><td>" + Html.Term("Account Type", "Account Type") + ": </td> <td>  <span style='color:#0080FF'>" + Html.Term(objAccountInformacion.AccountType, objAccountInformacion.AccountType) + "</span></td></tr>");%>
            <% Response.Write("<tr><td>" + Html.Term("CareerTitle", "CareerTitle") + ": </td > <td>  <span style='color:#0080FF'>" + Html.Term(objAccountInformacion.CareerAsTitle, objAccountInformacion.CareerAsTitle) + "</span></td></tr>"); %>
            <% Response.Write("<tr><td>" + Html.Term("PaidTitle", "PaidTitle") + ": </td> <td> <span style='color:#0080FF'>" + Html.Term(objAccountInformacion.PaidAsTitle, objAccountInformacion.CareerAsTitle) + "</span></td></tr>"); %>
            <% Response.Write("<tr><td>" + Html.Term("lblSponsor", "Sponsor") + ":  </td> <td> <span style='color:#0080FF'>" + objAccountInformacion.Sponsor + "</span></td></tr>"); %>
            <% Response.Write("<tr><td>" + Html.Term("Enrollment", "Enrollment") + ": </td> <td> <span style='color:#0080FF'>" + objAccountInformacion.Enrollment.ToShortDateString() + "</span></td></tr>");%>
            <% Response.Write("<tr><td>" + Html.Term("Activity", "Activity") + ":  </td> <td> <span style='color:#0080FF'>" + Html.Term(objAccountInformacion.StatusName, objAccountInformacion.CareerAsTitle) + "</span></td></tr>"); %>
            <% var varBlocking = BlockingType.GetAccountIsLocked(new NetSteps.Data.Entities.Business.HelperObjects.SearchParameters.BlockingTypeSearchParameters()
                {
                    AccountID = account.AccountID,
                    LanguageID = CoreContext.CurrentLanguageID
                });
            %>
            <% Response.Write("<tr><td>" + Html.Term("Blocking", "Blocking") + ":  </td> <td><span style='color:#0080FF'>" + varBlocking.Description + "</span></td></tr>"); %>
            <% Response.Write("</table>"); %>
        </p>
        <hr />
        <%-- fin 22/03/2016 update by lpulido--%>
        <table class="DetailsTag Section" width="100%">
            <tbody>
                <tr>
                    <td style="width: 70px">
                        <%= Html.Term("Created") %>:
                    </td>
                    <td>
                        <%= ticketToEdit.DateCreated.ToString(CoreContext.CurrentCultureInfo) %>
                    </td>
                </tr>
                <tr>
                    <td>
                        <%= Html.Term("Changed")%>:
                    </td>
                    <td>
                        <%= ticketToEdit.DateLastModified.ToString(CoreContext.CurrentCultureInfo) %>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
<%
    } %>