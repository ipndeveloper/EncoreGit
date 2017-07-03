<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.Landing.IndexModel>" %>

<%@ Import Namespace="NetSteps.Commissions.Common" %>
<%@ Import Namespace="NetSteps.Commissions.Common.Models" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%  string cultureInfo = (String)ViewData["countryID"];  %>
    <script type="text/javascript">
        $(function () {
            <%if (cultureInfo.Equals("USA")){ %>
                    $('#ssn').inputsByFormat({ format: '{0} - {1} - {2}', validateNumbers: true, attributes: [{ id: 'txtSSNPart1', length: 3, size: 3 }, { id: 'txtSSNPart2', length: 2, size: 2 }, { id: 'txtSSNPart3', length: 4, size: 4}] }); //R2707 - P.A.P  
            <%}%>
            $('#txtSponsor').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                $('#sponsorId').val(item.id);
            }, minCharacters: 3, source: $('#txtSponsor'), ajaxResults: true, maxResults: 50, showMore: true
            });

            $('#txtCity').watermark('<%= Html.JavascriptTerm("CitySearch", "Look up city") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Browse/SearchCity") %>', { onSelect: function (item) {
                $('#HiddenCity').val(item.text);
            }, minCharacters: 3, source: $('#txtCity'), ajaxResults: true, maxResults: 50, showMore: true
            });

            $('#txtPostalCode').watermark('<%= Html.JavascriptTerm("PostalCodeSearch", "Look up postal code") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Browse/SearchPostalCode") %>', { onSelect: function (item) {
                $('#HiddenPostalCode').val(item.text);
            }, minCharacters: 3, source: $('#txtPostalCode'), ajaxResults: true, maxResults: 50, showMore: true
            });

            $('.TabSearch input[type=text]').keyup(function (e) {
                if (e.keyCode == 13)
                    $('#btnAdvancedGo').click();
            });

            $('#btnAdvancedGo').click(function () {

                
                var data = {
                    status: $('#sStatus').val(),
                    type: $('#sAccountType').val(),
                    state: $('#sState').val(),
                    city: $('#HiddenCity').val(),
                    postalCode: $('#HiddenPostalCode').val(),
                    country: $('#uxCountry').val(),
                    siteUrl: $('#txtSiteUrl').val(),
                    email: $('#txtEmail').val(),
                    phone: $('#txtPhone').val(),
                    title: $('#txtTitle').val(),
                    sponsor: $('#sponsorId').val(),
                    startDate: $('#startDate').val(),
                    endDate: $('#endDate').val(),


//                    startDate: !/Invalid|NaN/.test(new Date($('#startDate').val())) ? $('#startDate').datepicker({ dateFormat: 'mm-dd-yy' }).val() : '',
//                    endDate: !/Invalid|NaN/.test(new Date($('#endDate').val())) ? $('#endDate').datepicker({ dateFormat: 'mm-dd-yy' }).val() : '',
                    firstName: $('#txtFirstName').val(),
                    lastName: $('#txtLastName').val(),
                    <%if (cultureInfo.Equals("USA")){ %>ssn: $('#ssn').inputsByFormat('getValue'),<%}%>
                    gender: $('#gender').val()
                };
                window.location = '<%= ResolveUrl("~/Accounts/Browse") %>?' + $.param(data);
            });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <%= Html.Term("Accounts") %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <%--<div class="LandingTools">
		<div class="Body">
			<a href="<%= ResolveUrl("~/Enrollment") %>">Enroll a new account</a>
		</div>
		<span class="ClearAll"></span>
	</div>--%>

    <%
        string cultureInfo = (String)ViewData["countryID"];
        
    %>
    <div class="LandingTools">
        <div class="Title">
            <h3>
                <%= Html.Term("AccountLookUp", " Quick Account Look-Up") %></h3>
            <span class="LawyerText">
                <%= Html.Term("3CharacterMin", "(3 characters min)") %></span>
        </div>
        <div class="Body">
            <div class="StartSearch">
                <% Html.RenderPartial("Find"); %>
            </div>
        </div>
        <span class="ClearAll"></span>
    </div>
    <div class="LandingTools">
        <div class="Title">
            <h3>
                <%= Html.Term("AdvancedSearch", "Advanced Search") %></h3>
        </div>
        <div class="Body">
            <div class="StartSearch">
                <div class="TabSearch">
                    <%--P.A.P R2707--%>
                    <table>
                        <tr>
                            <td>
                                <div class="mb10">
                                    <label for="txtFirstName">
                                        <%= Html.Term("AccountBrowseFirstName","First Name") %>:</label><br />
                                    <input type="text" id="txtFirstName" style="width: 15em;" />
                                </div>
                            </td>
                            <td style="width: 100px">
                            </td>
                            <td>
                                <div class="mb10">
                                    <label for="txtLastName">
                                        <%= Html.Term("AccountBrowseLastName", "Last Name")%>:</label><br />
                                    <input type="text" id="txtLastName" style="width: 15em;" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="mb10">
                                    <label for="txtGender">
                                        <%= Html.Term("AccountBrowseGender", "Gender")%>:</label><br />
                                    <select id="gender">
                                        <option value="None">
                                            <%= Html.Term("PreferNotToSay", "Prefer not to say") %></option>
                                        <option value="Male">
                                            <%= Html.Term("Male") %></option>
                                        <option value="Female">
                                            <%= Html.Term("Female") %></option>
                                    </select>
                                </div>
                            </td>
                            <td style="width: 100px">
                            </td>
                            <td>
                                <%if (cultureInfo.Equals("USA"))
                                  { %>
                                    <div class="mb10">
                                        <label for="txtSSN">
                                            <%= Html.Term("AccountBrowserSSN", "SSN/Tax ID")%>:</label><br />
                                        <div id="ssn">
                                        </div>
                                    </div>
                                <%}
                                  else
                                  { %>
                                    <div class="mb10">
                                        <div id="Div1">
                                        </div>
                                    </div>
                                <%} %>
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <div class="mb10">
                                    <label for="sStatus">
                                        <%= Html.Term("Status") %>:</label><br />
                                        <!-- CAMBIO ENCORE_4 Html.DropDownAccountStatus(htmlAttributes: new { id = "sStatus" }, selectTextTermName: "SelectStatus", excludedStatuses: new List<short>() { (short)Constants.AccountStatus.Imported })%> -->
                                        <%: Html.DropDownAccountStatus(htmlAttributes: new { id = "sStatus" }, selectTextTermName: "SelectStatus")%>
                                </div>
                            </td>
                            <td style="width: 100px">
                            </td>
                            <td>
                                <div class="mb10">
                                    <label for="sAccountType">
                                        <%= Html.Term("AccountType", "Account Type") %>:</label><br />
                                    <%: Html.DropDownAccountType(htmlAttributes: new{id="sAccountType"}, selectTextTermName: "SelectAccountType") %>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="mb10">
                                    <label for="sState">
                                        <%= Html.Term("StateProvince", "State/Province") %>:</label><br />
                                    <%: Html.DropDownListFor(m => m.StateProvinceID, Model.States, Html.Term("SelectState", "Select State..."), new { id = "sState" })%>
                                </div>
                            </td>
                            <td style="width: 100px">
                            </td>
                            <td>
                                <div class="mb10">
                                    <label for="txtTitle">
                                        <%= Html.Term("Title") %>:</label><br />
                                    <select id="txtTitle">
                                        <option value="">
                                            <%= Html.Term("SelectTitle", "Select Title...") %></option>
                                        <% var commissionsService = NetSteps.Encore.Core.IoC.Create.New<ICommissionsService>();
                                           foreach (var title in Model.Titles)
                                           { %>
                                        <option value="<%= title.Key %>">
                                            <%= title.Value %></option>
                                        <% } %>
                                    </select>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="mb10">
                                    <label for="uxCountry">
                                        <%= Html.Term("Country") %>:
                                    </label>
                                    <br />
                                    <%= Html.DropDownCountries(htmlAttributes: new { id = "uxCountry" }, selectTextTermName: "SelectCountry") %>
                                </div>
                            </td>
                            <td style="width: 100px">
                            </td>
                            <td>
                                <div class="mb10">
                                    <label for="txtCity">
                                        <%= Html.Term("City") %>:</label><br />
                                    <input type="text" id="txtCity" style="width: 15em;" />
                                    <input type="hidden" id="HiddenCity" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="mb10">
                                    <label for="txtPostalCode">
                                        <%= Html.Term("PostalCode", "Postal Code") %>:</label><br />
                                    <input type="text" id="txtPostalCode" style="width: 15em;" />
                                    <input type="hidden" id="HiddenPostalCode" />
                                </div>
                            </td>
                            <td style="width: 100px">
                            </td>
                            <td>
                                <div class="mb10">
                                    <label for="txtEmail">
                                        <%= Html.Term("Email", "Email") %>:</label><br />
                                    <input type="text" id="txtEmail" style="width: 15em;" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="mb10">
                                    <label for="txtPhone">
                                        <%= Html.Term("Phone") %>:</label><br />
                                    <input type="text" id="txtPhone" style="width: 15em;" />
                                </div>
                            </td>
                            <td style="width: 100px">
                            </td>
                            <td>
                                <div class="mb10">
                                    <label for="txtSiteUrl">
                                        <%= Html.Term("SiteUrl", "Site Url") %>:</label><br />
                                    <input type="text" id="txtSiteUrl" style="width: 15em;" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div class="mb10">
                                    <label for="txtSponsor">
                                        <%= Html.Term("Placement", "Placement") %>:</label><br />
                                    <input type="text" id="txtSponsor" style="width: 22em;" />
                                    <input type="hidden" id="sponsorId" />
                                </div>
                            </td>
                            <td style="width: 100px">
                            </td>
                            <td>
                                <div class="mb10">
                                    <label for="startDate">
                                        <%= Html.Term("EnrollmentDateRange", "Enrollment Date Range") %>:</label><br />
                                    <input type="text" id="startDate" class="DatePicker TextInput" value="<%= Html.Term("StartDate", "Start Date") %>" />
                                    to
                                    <input type="text" id="endDate" class="DatePicker TextInput" value="<%= Html.Term("EndDate", "End Date") %>" />
                                </div>
                            </td>
                        </tr>
                    </table>
                    <%--P.A.P R2707--%>
                    <p>
                        <a href="javascript:void(0);" id="btnAdvancedGo" class="Button BigBlue">
                            <%= Html.Term("Search", "Search") %></a>
                    </p>
                </div>
            </div>
        </div>
        <span class="ClearAll"></span>
    </div>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>
