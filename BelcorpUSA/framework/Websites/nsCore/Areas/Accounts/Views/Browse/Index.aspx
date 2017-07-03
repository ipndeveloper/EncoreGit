<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.Browse.AccountBrowseModel>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            var sponsorSelected = false;
            var sponsorId = $('<input type="hidden" id="sponsorIdFilter" class="Filter" />');
            $('#sponsorInputFilter').removeClass('Filter').css('width', '275px').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                sponsorId.val(item.id);
                sponsorSelected = true;
            }, minCharacters: 3, source: $('#sponsorFilter'), ajaxResults: true, maxResults: 50, showMore: true
            }).blur(function () {

                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    sponsorId.val('');
                } else if (!sponsorSelected) {
                    sponsorId.val('-1');
                }

                sponsorSelected = false;
            }).after(sponsorId);
            $('#emailInputFilter').css('width', '200px');

            if ('<%= Model.Sponsor.AccountID %>' > 0) {
                $('#sponsorInputFilter').val('<%: Model.Sponsor.FullName + " (#" + Model.Sponsor.AccountNumber + ")" %>');
                sponsorId.val('<%= Model.Sponsor.AccountID %>');
            }

            var cityValue = $('<input type="hidden" id="cityFilter" class="Filter" />');
            $('#CityInputFilter').removeClass('Filter').css('width', '120px').watermark('<%= Html.JavascriptTerm("CitySearch", "Look up city") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Browse/SearchCity") %>', { onSelect: function (item) {
                cityValue.val(item.text);
            }, minCharacters: 3, source: $('#cityFilter'), ajaxResults: true, maxResults: 50, showMore: true
            }).blur(function () {
                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    cityValue.val('');
                }
            }).after(cityValue);

            var postalCodeValue = $('<input type="hidden" id="postalCodeFilter" class="Filter" />');
            $('#PostalCodeInputFilter').removeClass('Filter').css('width', '120px').watermark('<%= Html.JavascriptTerm("PostalCodeSearch", "Look up postal code") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Browse/SearchPostalCode") %>', { onSelect: function (item) {
                postalCodeValue.val(item.text);
            }, minCharacters: 3, source: $('#postalCodeFilter'), ajaxResults: true, maxResults: 50, showMore: true
            }).blur(function () {
                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    postalCodeValue.val('');
                }
            }).after(postalCodeValue);

        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Accounts") %>">
        <%= Html.Term("Accounts", "Accounts") %></a> >
    <%= Html.Term("BrowseAccounts", "Browse Accounts") %>
</asp:Content>
<asp:Content ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("BrowseAccounts", "Browse Accounts") %>
        </h2>
    </div>
    
    <% 
        string cultureInfo = (String)ViewData["countryID"];
        if(cultureInfo.Equals("USA")){
		    Html.PaginatedGrid<AccountSearchData>("~/Accounts/Browse/GetAccounts")
			    .AutoGenerateColumns()
                .HideClientSpecificColumns()
			    .AddData("accountNumberOrName", ViewData["q"])
                .AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(Model.StatusList), startingValue: Model.SearchParameters.AccountStatusID.ToString())
                .AddSelectFilter(Html.Term("Type"), "type", new Dictionary<string, string>() { { "", Html.Term("SelectaType", "Select a Type...") } }.AddRange(Model.AccoutTypeList), startingValue: Model.SearchParameters.AccountTypes != null && Model.SearchParameters.AccountTypes.Count() > 0 ? Model.SearchParameters.AccountTypes.First().ToString() : null)
                .AddSelectFilter(Html.Term("State"), "state", new Dictionary<string, string>() { { "", Html.Term("SelectaState", "Select a State...") } }.AddRange(Model.StateList), startingValue: Model.SearchParameters.StateProvinceID.ToString())
                .AddSelectFilter(Html.Term("Country"), "country", new Dictionary<string, string>() { { "", Html.Term("SelectCountry", "Select Country...") } }.AddRange(Model.CountryList), startingValue: Model.SearchParameters.CountryID.ToString())
                .AddSelectFilter(Html.Term("Title"), "title", new Dictionary<string, string>() { { "", Html.Term("SelectTitle", "Select Title...") } }.AddRange(Model.TitleList), startingValue: Model.SearchParameters.TitleID.ToString())
                .AddInputFilter(Html.Term("Email"), "email", Model.SearchParameters.Email, addBreak: true)
                //.AddInputFilter(Html.Term("CPF"), "numCPF", Model.SearchParameters.CPF) //Ver Manejo ENCORE_4
			    .AddInputFilter(Html.Term("Sponsor"), "sponsor")
                .AddInputFilter(Html.Term("City"), "city", startingValue: Model.SearchParameters.City)
                .AddInputFilter(Html.Term("Phone"), "phone", startingValue: Model.SearchParameters.PhoneNumber)
                .AddInputFilter(Html.Term("SiteUrl", "Site Url"), "siteUrl", startingValue: Model.SearchParameters.SiteUrl)
                .AddInputFilter(Html.Term("PostalCode", "Postal Code"), "postalCode", startingValue: Model.SearchParameters.PostalCode)
                .AddInputFilter(Html.Term("EnrollmentDateRange", "Enrollment Date Range"), "startDate", startingValue: Model.SearchParameters.StartDate, isDateTime: true)
                .AddInputFilter("To", "endDate", startingValue: Model.SearchParameters.EndDate, isDateTime: true)
                .AddInputFilter(Html.Term("AccountBrowseFirstName", "First Name"), "firstName", Model.SearchParameters.FirstName, isHidden: true) //CAMBIO ENCORE-4
                .AddInputFilter(Html.Term("AccountBrowseLastName", "Last Name"), "lastName", Model.SearchParameters.LastName, isHidden: true) //CAMBIO ENCORE-4
                .AddInputFilter(Html.Term("AccountBrowserSSN", "SSN"), "ssn", Model.SearchParameters.SSN, isHidden: true) //CAMBIO ENCORE-4
                .AddInputFilter(Html.Term("AccountBrowseGender", "Gender"), "gender", Model.SearchParameters.gender, isHidden: true) //CAMBIO ENCORE-4
			    //.AddInputFilter(Html.Term("AccountNumberorName", "Account Number or Name"), "accountNumberOrName", ViewData["q"])
                .ClickEntireRow()
			    .Render();
        }

        if (cultureInfo.Equals("BRA"))
        {
            Html.PaginatedGrid<AccountSearchData>("~/Accounts/Browse/GetAccounts")
                .AutoGenerateColumns()
                .HideClientSpecificColumns()
                .AddData("accountNumberOrName", ViewData["q"])
                .AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(Model.StatusList), startingValue: Model.SearchParameters.AccountStatusID.ToString())
                .AddSelectFilter(Html.Term("Type"), "type", new Dictionary<string, string>() { { "", Html.Term("SelectaType", "Select a Type...") } }.AddRange(Model.AccoutTypeList), startingValue: Model.SearchParameters.AccountTypes != null && Model.SearchParameters.AccountTypes.Count() > 0 ? Model.SearchParameters.AccountTypes.First().ToString() : null)
                .AddSelectFilter(Html.Term("State"), "state", new Dictionary<string, string>() { { "", Html.Term("SelectaState", "Select a State...") } }.AddRange(Model.StateList), startingValue: Model.SearchParameters.StateProvinceID.ToString())
                .AddSelectFilter(Html.Term("Country"), "country", new Dictionary<string, string>() { { "", Html.Term("SelectCountry", "Select Country...") } }.AddRange(Model.CountryList), startingValue: Model.SearchParameters.CountryID.ToString())
                .AddSelectFilter(Html.Term("Title"), "title", new Dictionary<string, string>() { { "", Html.Term("SelectTitle", "Select Title...") } }.AddRange(Model.TitleList), startingValue: Model.SearchParameters.TitleID.ToString())
                .AddInputFilter(Html.Term("Email"), "email", Model.SearchParameters.Email, addBreak: true)
                .AddInputFilter(Html.Term("CPF"), "numCPF", Model.SearchParameters.CPF) //Ver Manejo ENCORE_4
                .AddInputFilter(Html.Term("Sponsor"), "sponsor")
                .AddInputFilter(Html.Term("City"), "city", startingValue: Model.SearchParameters.City)
                .AddInputFilter(Html.Term("Phone"), "phone", startingValue: Model.SearchParameters.PhoneNumber)
                .AddInputFilter(Html.Term("SiteUrl", "Site Url"), "siteUrl", startingValue: Model.SearchParameters.SiteUrl)
                .AddInputFilter(Html.Term("PostalCode", "Postal Code"), "postalCode", startingValue: Model.SearchParameters.PostalCode)
                .AddInputFilter(Html.Term("EnrollmentDateRange", "Enrollment Date Range"), "startDate", startingValue: Model.SearchParameters.StartDate, isDateTime: true)
                .AddInputFilter("To", "endDate", startingValue: Model.SearchParameters.EndDate, isDateTime: true)
                .AddInputFilter(Html.Term("AccountBrowseFirstName", "First Name"), "firstName", Model.SearchParameters.FirstName, isHidden: true) //CAMBIO ENCORE-4
                .AddInputFilter(Html.Term("AccountBrowseLastName", "Last Name"), "lastName", Model.SearchParameters.LastName, isHidden: true) //CAMBIO ENCORE-4
                .AddInputFilter(Html.Term("AccountBrowserSSN", "SSN"), "ssn", Model.SearchParameters.SSN, isHidden: true) //CAMBIO ENCORE-4
                .AddInputFilter(Html.Term("AccountBrowseGender", "Gender"), "gender", Model.SearchParameters.gender, isHidden: true) //CAMBIO ENCORE-4
                //.AddInputFilter(Html.Term("AccountNumberorName", "Account Number or Name"), "accountNumberOrName", ViewData["q"])
                .ClickEntireRow()
                .Render();
        }
    %>

    <script type="text/javascript">
        $('#startDateInputFilter').keypress(function ()
        {
            return false;
        });

        $('#endDateInputFilter').keypress(function ()
        {
            return false;
        });
    </script>

</asp:Content>
<asp:Content ID="Content7" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>
