<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/LogiticsProvDetail.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
	 <% List<LogisticsProviderSearData> details = ViewData["details"] as List<LogisticsProviderSearData>;
           string LogisticsProviderID = "";      
           if (details.Count > 0)
           {
               LogisticsProviderID = details[0].LogisticsProviderID.ToString();
           }
           else
           {              
               LogisticsProviderID = "";
           }
     %>
		<div class="SectionNav">
			<ul class="SectionLinks">
                <%= Html.SelectedLink("~/Logistics/Logistics/ProviderDetails/" + LogisticsProviderID, Html.Term("Details", "Details"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
                <%= Html.SelectedLink("~/Logistics/Logistics/Documents/" + LogisticsProviderID, Html.Term("DocumentsProv", "Documents"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
                <%--<%= Html.SelectedLink("~/Logistics/Logistics/Address/" + LogisticsProviderID, Html.Term("Address", "Address"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>--%>
                 <%= Html.SelectedLink("~/Logistics/Logistics/Routes/" + LogisticsProviderID, Html.Term("Routes", "Routes"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
            </ul>
		</div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="SectionHeader">
		<h2>
			<%= Html.Term("Routes", "Routes")%>
        </h2>		
	</div>
 <table id="newMaterial" class="FormTable Section" width="100%">
		<% List<LogisticsProviderSearData> details = ViewData["details"] as List<LogisticsProviderSearData>;    
           string LogisticsProviderID = "";  
           if (details.Count > 0)
           {
               LogisticsProviderID = details[0].LogisticsProviderID.ToString();
           }
           else {               
               LogisticsProviderID = ""; 
           }
            %>
        <tr>
			<td class="FLabel">
				<%= Html.Term("Route")%> :
			</td>
			<td>
				  <input type="hidden" id="LogisticsProviderID" value="<%=LogisticsProviderID %>"/>   
                  <input type="hidden" id="RouteID"/> 
                  <input type="text" id="routeIDInputFilter" />          
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("Dispatch Day")%> :
			</td>
			<td>				 
			</td>
		</tr>        
         <tr>
			<td class="FLabel">
				<%= Html.Term("Monday")%> :
			</td>
			<td>
				   <input  type="checkbox" id="Monday" checked />
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("Tuesday")%> :
			</td>
			<td>
				  <input  type="checkbox" id="Tuesday" checked />
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("Wednesday")%> :
			</td>
			<td>
				 <input  type="checkbox" id="Wednesday" checked/>
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("Thursday")%> :
			</td>
			<td>
				 <input  type="checkbox" id="Thursday" checked/>
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("Friday")%> :
			</td>
			<td>
				   <input  type="checkbox" id="Friday" checked/>
			</td>
		</tr>  
        <tr>
			<td class="FLabel">
				<%= Html.Term("Saturday")%> :
			</td>
			<td>
				   <input  type="checkbox" id="Saturday" checked/>
			</td>
		</tr> 
        <tr>
			<td class="FLabel">
				<%= Html.Term("Sunday")%> :
			</td>
			<td>
				  <input  type="checkbox" id="Sunday" checked/>
			</td>
		</tr>       
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                        <%= Html.Term("Add", "Add")%></a>
                </p>
            </td>
        </tr>
	</table>
    <span class="ClearAll"></span>    
    <% Html.PaginatedGrid<RoutesLogProvSearchData>("~/Logistics/Logistics/GetRoutesProv/"+ (LogisticsProviderID.IsNullOrEmpty() ? "" : LogisticsProviderID))
            .AutoGenerateColumns()
            .HideClientSpecificColumns_()
            .CanDelete("~/Logistics/Logistics/DeleteRoutes")
            .ClickEntireRow()
		    .Render(); 
        %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(function () {

        $('#btnToggleStatus').click(function () {
            var t = $(this);
            var txtActive = $('#txtActive').val();
            var active = 0;
            if (txtActive == 1) {
                active = 0
            }
            else {
                active = 1
            }
            showLoading(t);
            $.post('<%= ResolveUrl(string.Format("~/Logistics/Logistics/ToggleStatus")) %>', { LogisticsProviderID: $('#LogisticsProviderID').val(), active: active }, function (response) {
                hideLoading(t);
                if (response.result) {
                    t.toggleClass('ToggleInactive');
                    window.location = '<%= ResolveUrl("~/Logistics/Logistics/Routes/") %>' + $('#LogisticsProviderID').val();
                } else {
                    showMessage(response.message, true);
                }
            });
        });


        $('#routeIDInputFilter').change(function () {
            $('#RouteID').val("");
        });

        $('#routeIDInputFilter').removeClass('Filter').after($('#RouteID')).css('width', '275px')
                .val('')
				.watermark('<%= Html.JavascriptTerm("RouteIDSearch", "Look up route by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Logistics/Routes/searchRoutes") %>', { onSelect: function (item) {
				    $('#RouteID').val(item.id);
				    $('#routeIDInputFilter').clearError();
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
		});


        //Guardar
        $('#btnSave').click(function () {
            if (($("#RouteID").val() == '')) {
                $('#routeIDInputFilter').showError('<%=Html.JavascriptTerm("ProvRouteIDVal", "Route is required.") %>');
                return false;
            }
            //            
            var routesprov = {
                RouteID: $('#RouteID').val(),
                LogisticsProviderID: $('#LogisticsProviderID').val(),
                Monday: $('#Monday').is(":checked"),
                Tuesday: $('#Tuesday').is(":checked"),
                Wednesday: $('#Wednesday').is(":checked"),
                Thursday: $('#Thursday').is(":checked"),
                Friday: $('#Friday').is(":checked"),
                Saturday: $('#Saturday').is(":checked"),
                Sunday: $('#Sunday').is(":checked")
            }, t = $(this);
            //$.post('/Logistics/Logistics/SaveRoutes', routesprov, function (response) {
            $.post('<%= ResolveUrl(string.Format("~/Logistics/Logistics/SaveRoutes")) %>', routesprov, function (response) {
                if (response.result) {
                    showMessage("Route Associated Correctly", false);
                    RefreshRoutes();
                } else {
                    showMessage(response.message, true);
                }
            });
        });
        //
        function RefreshRoutes() {

            $.ajax({
                url: '<%= ResolveUrl("~/Logistics/Logistics/GetRoutesProv/") %>',
                data: { page:0, pageSize:15, orderByDirection:0, orderBy:"RouteID", id: $('#LogisticsProviderID').val() },
                type: 'GET',
                success: function (response) {
                    if (response.result) {
                        $("#paginatedGrid tbody").empty();
                        $("#paginatedGrid tbody").html(response.page)
                    } else {
                        showMessage(response.message, true);
                    }
                }
            });
        }
    });
    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="YellowWidget" runat="server">
     <% List<LogisticsProviderSearData> details = ViewData["details"] as List<LogisticsProviderSearData>;          
           string Name = "";
           string LogisticsProviderID = "";          
           bool Active = false;
           if (details.Count > 0)
           {
               LogisticsProviderID = details[0].LogisticsProviderID.ToString();
               Name = details[0].Name.ToString();               
               Active = Convert.ToBoolean(details[0].Active);
               int activeprov = Convert.ToInt32(details[0].Active);               
           //}
           //else {
           //    Name = "";
           //    LogisticsProviderID = "";               
           //    Active = false;
           //}
            %>
    <div class="TagInfo">
            <div class="Content">
                <div class="SubTab">                   
                            <a> <%=Name%> </a>            
                </div>
                <table class="DetailsTag Section" width="100%">
                    <tr>
                        <td class="Label"><%= Html.Term("Code", "Code")%>:
                        </td>
                        <td>
                         <a><%=LogisticsProviderID %></a>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label">
                            <%= Html.Term("Status", "Status") %>:
                        </td>
                        <td>
                            <input type="hidden" value="<%=activeprov %>" id="txtActive" />
                           <a id="btnToggleStatus" href="javascript:void(0);" class="Toggle ToggleActive<%= !Active ? " ToggleInactive" : "" %>">
                           </a>
                        </td>
                    </tr>
                    <tr>               
                    </tr>    
                        <tr>                       
                        </tr>                       
                </table>
            </div>
            <div class="TagBase">
            </div>
        </div>
     <% } %>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<a href="<%= ResolveUrl("~/Logistics") %>">
		    <%= Html.Term("Logistics") %></a> >
        <%= Html.Term("Routes", "Routes")%>
</asp:Content>
