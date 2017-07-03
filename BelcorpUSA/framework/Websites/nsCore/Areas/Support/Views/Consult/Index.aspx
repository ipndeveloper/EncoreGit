<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Support/Views/Shared/Support.Master" 
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.SupportTicketSearchParameters> " %>

<asp:Content  ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>

<asp:Content   ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
     var dcAdicionales=<%=ViewData["dcAdicionales"]%>
        $(function () {
            $('#exportToExcel').click(function () {              
                var url = '<%= ResolveUrl("~/Support/Consult/Export") %>';
                $("#frmExportar").attr("src", url);               
            });

          setTimeout(function(){
               
                     $(".FilterSet").append($("#filtrosTipoTicket"))
                
                },2000);

            $('#ConsultantSearchInputFilter, #UserSearchInputFilter, #CreatebyUserSearch,  #SupportTicketNumberInputFilter,#TitleInputFilter').css('width', '15em');

            //ConsultSearch
            var consultantSearchSelected = false;
            $('#ConsultantSearchInputFilter').watermark('<%= Html.JavascriptTerm("ConsultantSearch", "Look up consultant by ID or name") %>').keyup(function () {
                consultantSearchSelected = false;
            }).jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                $('#accountIDInputFilter').val(item.id);
                consultantSearchSelected = true;
            }
            , minCharacters: 3
            , ajaxResults: true
            , maxResults: 50
            , showMore: true
            , width: $('#ConsultantSearchInputFilter').outerWidth(true)
            }).blur(function () {

                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    $('#accountIDInputFilter').val('');
                } else if (!consultantSearchSelected) {
                    $('#accountIDInputFilter').val('-1');
                }

                consultantSearchSelected = false;
            });

            //UserAssignado
            var assignedUserIDSelected = false;
            $('#UserSearchInputFilter').watermark('<%= Html.JavascriptTerm("UserSearch", "Look up user by ID or name") %>').keyup(function () {
                assignedUserIDSelected = false;
            }).jsonSuggest('<%= ResolveUrl("~/Admin/Users/Search") %>',
            { onSelect: function (item) {
                $('#AssignedUserIDInputFilter').val(item.id);
                assignedUserIDSelected = true;
            }
            , minCharacters: 1
            , ajaxResults: true
            , maxResults: 50
            , showMore: true
            , width: $('#UserSearchInputFilter').outerWidth(true)
            }).blur(function () {

                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    $('#AssignedUserIDInputFilter').val('');
                } else if (!assignedUserIDSelected) {
                    $('#AssignedUserIDInputFilter').val('-1');
                }

                assignedUserIDSelected = false;
            });


            //CreatebyUserSearch 
            var createbyUserIDSelected = false;
            $('#CreatebyUserSearchInputFilter').watermark('<%= Html.JavascriptTerm("CreatebyUser", "Look up user by ID or name") %>').keyup(function () {
                createbyUserIDSelected = false;
            }).jsonSuggest('<%= ResolveUrl("~/Admin/Users/Search") %>',
            { onSelect: function (item) {
                $('#CreatebyUserIDInputFilter').val(item.id);
                createbyUserIDSelected = true;
            }
            , minCharacters: 1
            , ajaxResults: true
            , maxResults: 50
            , showMore: true
            , width: $('#CreatebyUserSearchInputFilter').outerWidth(true)
            }).blur(function () {

                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    $('#CreatebyUserIDInputFilter').val('');
                } else if (!createbyUserIDSelected) {
                    $('#CreatebyUserIDInputFilter').val('-1');
                }

                createbyUserIDSelected = false;
            });           

            $("#SupportTicketCategoryIDSelectFilter").on("change", function () {

                var IdRow = $("#SupportTicketCategoryIDSelectFilter").val();
                var SupportLevelID = 0;
                var SupportMotiveID = 0;

                var dcParametros = getClaveValor(dcAdicionales[IdRow]);
                SetParamSearchGridView("SupportLevelID", dcParametros.SupportLevelID);
                SetParamSearchGridView("SupportMotiveID", dcParametros.SupportMotiveID);
            });


            $('#requestTicket').click(function (e) {
                if (!$('#ticketTable').checkRequiredFields()) {
                    return false;
                }
                var t = $(this);
                showLoading(t);
                $.post('<%= ResolveUrl("~/Support/Ticket/RequestNewTicket") %>',
                {}, function (response) {
                    hideLoading(t);
                     window.location = '<%= ResolveUrl("~/Support/Ticket/Edit/") %>' + response.supportTicketNumber;

                });
            });

        });

        function getClaveValor(dc) {
            var SupportLevelID = 0;
            var SupportMotiveID = 0;
            for (var k in dc) {
                SupportLevelID = dc[k];
                SupportMotiveID = k;

            }

            var dc =
             {
                 SupportLevelID: SupportLevelID,
                 SupportMotiveID: SupportMotiveID
             };
            return dc;
        }
		function cambiarTipoTicket( ctr)
        {
            SetParamSearchGridView("IsSiteDWS", ctr.value);
        }
    </script>
    
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("BrowseSupportTickets","Browse Support Tickets") %></h2>
        <%
            bool allowedToMangeOtherTickets = ViewData["AllowedToMangeOtherTickets"] == null ? false : !(bool)ViewData["AllowedToMangeOtherTickets"];

            if (allowedToMangeOtherTickets)
            {
        %>
        <%= string.Format(Html.Term("BrowseSupportTicketsAssignedTitle", "You have {0} ticket(s).", string.Format("<span class=\"bold availableTickets\">{0}</span>", ViewData["TotalCount"] == null ? "0" : ViewData["TotalCount"])))%>
        <%
            }
            else
            {
        %>
        <%= string.Format(Html.Term("BrowseSupportTicketsAssignedTitleManageAllTicketsTotal", "There are {0} total tickets.", string.Format("<span class=\"bold availableTickets\">{0}</span>", ViewData["TotalCount"] == null ? "0" : ViewData["TotalCount"])))%>
        <%
            }
        %>
        <a href="#" class="bold requestTicket" id="requestTicket"><span><%= Html.Term("CreateTicket", "Request a New Ticket")%></span></a>
        <br />
    </div>

    <%  AccountSlimSearchData consultantSlimSearchData = Model.AccountID.HasValue ? CachedData.GetAccountSlimSearch((int)Model.AccountID) : null;
        string consultantDisplayName = consultantSlimSearchData != null ? string.Format("{0} {1} (#{2})", consultantSlimSearchData.FirstName, consultantSlimSearchData.LastName, consultantSlimSearchData.AccountNumber) : string.Empty;        
        string username = allowedToMangeOtherTickets ? CoreContext.CurrentUser.Username : string.Empty; 
        string CreatebyUsername = allowedToMangeOtherTickets ? CoreContext.CurrentUser.Username : string.Empty;
        string assignedUserID = allowedToMangeOtherTickets ? CoreContext.CurrentUser.UserID.ToString() : null; 
        string CreatebyUserID = allowedToMangeOtherTickets ? CoreContext.CurrentUser.UserID.ToString() : null;
                       
        Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.SupportTicketSearchDetailsData>("~/Support/Ticket/GetSupportTicketsDetails")           
        .AutoGenerateColumns()
        //.AddSelectFilter(Html.Term("TypeConsult", "Type Consult"), "typeConsult", new Dictionary<int, string>() {{ 0, Html.Term("Generals", "General")}, { 1, Html.Term("Details", "Details") } })
        .AddSelectFilter(Html.Term("SupportTicketPriority"), "SupportTicketPriorityID", new Dictionary<string, string>() { { "", Html.Term("SelectaSupportTicketPriorities", "Select a Selecta Ticket Priorities...") } }.AddRange(SmallCollectionCache.Instance.SupportTicketPriorities.ToDictionary(os => os.SupportTicketPriorityID.ToString(), os => os.GetTerm())), startingValue: Model.SupportTicketPriorityID.ToString())
        .AddSelectFilter(Html.Term("SupportTicketCategory"), "SupportTicketCategoryID", new Dictionary<string, string>() { { "", Html.Term("SelectaSupportTicketCategory", "Select a Category...") } }.AddRange(ViewData["dcSupporMotiveLevelJerarquia"] as Dictionary<string, string>), startingValue: Model.SupportTicketCategoryID.ToString())        
        .AddSelectFilter(Html.Term("SupportTicketStatus"), "SupportTicketStatusID", new Dictionary<string, string>() { { "", Html.Term("SelectaSupportTicketStatus", "Select a Status...") } }.AddRange(SmallCollectionCache.Instance.SupportTicketStatuses.ToDictionary(os => os.SupportTicketStatusID.ToString(), os => os.GetTerm())), startingValue: Model.SupportTicketStatusID.ToString())
        .AddInputFilter(Html.Term("SupportTicketNumber", "Support Ticket Number"), "SupportTicketNumber")
        .AddInputFilter(Html.Term("SupportTicketTitle", "Title"), "Title", "", false, false, true)

        .AddInputFilter(Html.Term("OrderNumber", "Order Number"), "OrderNumber", "")
        .AddInputFilter(Html.Term("InvoiceNumber", "Invoice Number"), "InvoiceNumber", "")
        
        .AddInputFilter(Html.Term("AssignedUser", "Assigned User"), "UserSearch", username, false, false)
        .AddInputFilter(Html.Term("AssignedUserID"), "AssignedUserID", assignedUserID, false, false, true)                
        .AddInputFilter(Html.Term("ConsultantFilterSearch", "Consultant Search"), "ConsultantSearch", consultantDisplayName)
        .AddInputFilter(Html.Term("ConsultantAccountID"), "accountID", consultantDisplayName, false, false, true)        
        .AddInputFilter(Html.Term("CreatebyUser", "Create by User"), "CreatebyUserSearch", CreatebyUsername, false, false)        
        .AddInputFilter(Html.Term("CreatebyUserID"), "CreatebyUserID", CreatebyUserID, false, false, true)          
        .AddSelectFilter(Html.Term("SelectAccountType", "Select Account Type"), "AccountTypeID", new Dictionary<string, string>() { { "", Html.Term("SelectAccountType", "Select a Account Type...") } }.AddRange(TempData["AccountTypes"] as Dictionary<string, string>))        
        .AddSelectFilter(Html.Term("Campaign", "Campaign"), "CampaignID", new Dictionary<string, string>() { { "0", Html.Term("SelectaCampaign", "Select a Campaign...") } }.AddRange(TempData["Periods"] as Dictionary<string, string>))                                       
        .AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", Model.StartDate.HasValue ? Model.StartDate.ToShortDateString() : Html.Term("StartDate", "Start Date")/*new DateTime(1900, 1, 1).ToShortDateString()*/, true)
        .AddInputFilter(Html.Term("To", "To"), "endDate", Model.EndDate.HasValue ? Model.EndDate.ToShortDateString() : Html.Term("EndDate", "End Date")/*DateTime.Now.ToShortDateString()*/, true, true)
        .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))
        .ClickEntireRow()
        .Render();
         %>

	<%--<div id="filtrosTipoTicket" class="FL">
        <label for="rdbDWS">
            Select type ticket:</label>
        <table>
            <tr>
                <td>
                    DWS
                </td>
                <td>
                    <input onchange="cambiarTipoTicket(this)" value="1" id="rdbDWS" name="GMP" type="radio" />
                </td>
                <td>
                    GMP
                </td>
                <td>
                    <input onchange="cambiarTipoTicket(this)" value="0" id="rdbGMP" name="GMP" type="radio" />
                </td>
                <td>
                    ALL
                </td>
                <td>
                    <input onchange="cambiarTipoTicket(this)" value="2" id="RrdbAll" name="GMP" type="radio" />
                </td>
            </tr>
        </table>
    </div>--%>
    <iframe name="frmExportar" id="frmExportar" style="display: none" src=""></iframe>
</asp:Content>
 