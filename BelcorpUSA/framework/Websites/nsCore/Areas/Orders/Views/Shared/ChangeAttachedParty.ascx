<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<div id="changeAttachedParty">
    <script type="text/javascript">
        $(function () {
            $(".btnAttachToParty").click(function () {
                $.post('<%= ResolveUrl("~/Orders/Details/ChangeAttachedParty") %>', { orderNumber: $('#orderNumber').val(), newPartyOrderId: $(this).attr("id") }, function (response) {
                    if (response.result) {
                        $('#changePartyOrderModal').jqmHide();
                        window.location.reload();
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });
        });
    </script>
    <h2>
        <%= Html.Term("ChangeAttachedParty", "Change Attached Party")%></h2>
    <div id="partyAttachErrors">
    </div>
    <%if (ViewBag.CurrentAttachedParty != null)
      {%>
      <h3 class="pad5"><%=Html.Term("PartyCurrentlyAttached", "This order is currently attached to party id")%>: <%=ViewBag.CurrentAttachedParty%></h3>
      <%
      }
      else
      {%>
      <h3 class="pad5"><%=Html.Term("PartyCurrentlyNotAttached", "This order is currently not attached to a party")%></h3>
      <%
      } %>
    <%if (ViewBag.OpenConsultantParties != null && ViewBag.OpenConsultantParties.Count > 0)
      {
          bool first = true;
    %>
    <div class="clr FauxTable openParties">
        <ul class="flatList partiesList">
            <% foreach (PartySearchData party in ViewBag.OpenConsultantParties)
               { %>
            <li>
                <div class="brdrBottom pad2 partyDetails">
                    <label for="party<%=(party.PartyID) %>">
                        <%-- <a href="<%=ResolveUrl("~/Shop", new { partyId = party.PartyID }) %>" class="FR Button MinorButton buyFromPartyBtn">
                                <span>@Html.Term("Shop", "Shop")</span></a> --%><span class="FLabel mr10 partyName">
                            <%=party.Name%></span> &nbsp;&nbsp;
                            <span class="FLabel partyDate"><%= party.StartDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo) %></span>
                        &nbsp;&nbsp;
                            <a href="javascript:void(0);" id="<%=party.OrderID %>" class="DTL Attach btnAttachToParty">
                                <span><%=Html.Term("AttachToParty", "Attach To Party") %></span></a>
                    </label>
                </div>
                <span class="clr"></span></li>
            <%
                   first = false;
               } %>
        </ul>
    </div>
    <%}
      else
      { %>
    <div class="clr openParties noParties">
        <div class="UI-linkAlt partiesList">
            <%=Html.Term("ThereAreNoOpenParties", "There are no open parties right now.") %>
        </div>
    </div>
    <%} %>
    <br />
    <p class="FL">
        <a href="javascript:void(0);" id="btnChangeCancel" class="Button jqmClose">
            <%= Html.Term("Cancel", "Cancel") %>
    </a> </p>
</div>
<span class="ClearAll"></span>