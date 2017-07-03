<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Business.AccountInformacion>" %>
<%   NetSteps.Data.Entities.Business.AccountInformacion objAccountInformacion = Model as NetSteps.Data.Entities.Business.AccountInformacion;%>
<div id="informacionConsultora" class="TagInfo">
    <div class="Content">
        <span>
            <%= Html.Term("TicketOwner", "Ticket Owner") %>:</span>
        <h1>
            <h1>
                <a href="<%= ResolveUrl("~/Accounts/Overview/Index/" + objAccountInformacion.AccountNumber) %>"
                    title="Go to account record">
                    <%= objAccountInformacion.FirstName+ " "+objAccountInformacion.LastName%>
                </a>
            </h1>
            <p class="DistributorStatus">
                <%
                    string cultureInfoCode = NetSteps.Common.Globalization.CountryCultureInfoCode.UnitedStates;

                    var taxNumber = NetSteps.Security.Encryption.DecryptTripleDES(objAccountInformacion.TaxNumber).MaskString(4);
                %>
                #<%= objAccountInformacion.AccountNumber %>,
                <%= Html.Term("SSN") %>:
                <%= string.IsNullOrEmpty(taxNumber) ? Html.Term("NA") : taxNumber %>
                <br>
                <%= Html.Encode(objAccountInformacion.PhoneNumber.FormatPhone(cultureInfoCode))%>
                <% if (objAccountInformacion.City != null && objAccountInformacion.State != null)
                       Response.Write("<br />" + objAccountInformacion.City + ", " + objAccountInformacion.State);
                %>
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
                <% Response.Write("<tr><td>" + Html.Term("CareerTitle", "CareerTitle") + ": </td> <td>  <span style='color:#0080FF'>" + Html.Term(objAccountInformacion.CareerAsTitle, objAccountInformacion.CareerAsTitle) + "</span></td></tr>"); %>
                <% Response.Write("<tr><td>" + Html.Term("PaidTitle", "PaidTitle") + ": </td> <td> <span style='color:#0080FF'>" + Html.Term(objAccountInformacion.PaidAsTitle, objAccountInformacion.CareerAsTitle) + "</span></td></tr>"); %>
                <% Response.Write("<tr><td>" + Html.Term("lblSponsor", "Sponsor") + ":  </td> <td> <span style='color:#0080FF'>" + objAccountInformacion.Sponsor + "</span></td></tr>"); %>
                <% Response.Write("<tr><td>" + Html.Term("Enrollment", "Enrollment") + ": </td> <td> <span style='color:#0080FF'>" + objAccountInformacion.Enrollment.ToShortDateString() + "</span></td></tr>");%>
                <% Response.Write("<tr><td>" + Html.Term("Activity", "Activity") + ":  </td> <td> <span style='color:#0080FF'>" + Html.Term(objAccountInformacion.StatusName, objAccountInformacion.CareerAsTitle) + "</span></td></tr>"); %>
                <% var varBlocking = BlockingType.GetAccountIsLocked(new NetSteps.Data.Entities.Business.HelperObjects.SearchParameters.BlockingTypeSearchParameters()
                {
                    AccountID = objAccountInformacion.AccountID,
                    LanguageID = CoreContext.CurrentLanguageID
                });
                %>
                <% Response.Write("<tr><td>" + Html.Term("Blocking", "Blocking") + ":  </td colespan> <td><span style='color:#0080FF'>" + varBlocking.Description + "</span></td></tr>"); %>
                <% Response.Write("</table>"); %>
            </p>
    </div>
</div>
