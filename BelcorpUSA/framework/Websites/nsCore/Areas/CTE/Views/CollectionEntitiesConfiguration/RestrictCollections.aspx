<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/MeansOfCollections.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.CollectionEntitiesData>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="YellowWidget" runat="server">
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">   
	    <div class = "SectionNav"> 
	        <ul class="SectionLinks">
		        <li><%: Html.ActionLink(Html.Term("CreateCollections", "Create Collection Entities"), "CreateCollectionEntities", new { id = Model.CollectionEntityID })%></li>
                <li><%: Html.ActionLink(Html.Term("RestrictMeansOfCollectionByLocation", "Restrict Means Of Collection By Location"), "RestrictCollections", new { id = Model.CollectionEntityID }, new { @class = "selected" })%></li>
	        </ul>
        </div>    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("RestrictMeansOfCollectionByLocation", "Restrict Means Of Collection By Location")%></h2>

            <a href="<%= ResolveUrl("~/CTE/CollectionEntitiesConfiguration/BrowseCollections") %>"><%= Html.Term("BrowseMeansOfCollection", "Browse Means of Collection")%></a>
	</div>
    <table id="LocationRestrictionsTable" class="DataGrid FormGrid" width="90%">
		<tbody>
			<tr>
				<td class="FLabel">
                <input type="hidden" id="hdnCollecEntityID"  value="<%= Model.CollectionEntityID %>" />
					<%= Html.Term("State", "State")%>:
				</td>
				<td>
				    <select id="state" name ="state" >
                        <option value ="">Select a State</option>
					    <%                            
                            foreach (var state in SmallCollectionCache.Instance.StateProvinces.OrderBy(a => a.Name))
						    {
					    %>
					    <option value="<%= state.Name %>" >
						    <%= state.Name%></option>
					    <%                                    
						    }                           
					    %>                        
				    </select>
                    <span id="spMessage" style="color: Red;"></span>   
				</td>
            </tr>
			<tr> 
				<td class="FLabel">
					<%= Html.Term("City", "City")%>:
				</td>
                <td>
					<select id="city">
                        <option value ="">Select a City</option>						
					</select>
                    
				</td> 
             </tr>               
             <tr>  				
                <td class="FLabel">
					<%= Html.Term("County", "County")%>:
				</td>
                <td>
					<select id="county">
                        <option value ="">Select a County</option>						
					</select>
				</td>
                <td class="Flabel"><%=Html.Term("Except", "Except")%> : 
                <input type="checkbox" id ="checkExcept"  /></td>
			</tr>
            <tr></tr>
            <tr>
                <td>
                    <p class="FormButtons">
                        <a id="AddZone" href="javascript:void(0);" class="Button BigBlue">
                               <%= Html.Term("Add", "Add")%>
                        </a>
                    </p>
                </td>
                <td>
                    <p class="FormButtons">
                        <a id="btnSave" href="javascript:void(0);" class="Button BigBlue">
                               <%= Html.Term("Save", "Save")%>
                        </a>
                    </p>
                </td>
            </tr>
		</tbody>
	</table>
    <span class="ClearAll"></span>

    <% Html.PaginatedGrid<CollectionZonesData>("~/CTE/CollectionEntitiesConfiguration/GetZones/")
        .AutoGenerateColumns()
        .AddData("collectionEntityID", Model.CollectionEntityID)
        .HideClientSpecificColumns_()
        .CanDelete("")
        .ClickEntireRow()
		.Render(); 
    %>


<script type="text/javascript">

    $(function () {

        $('#AddZone').click(function (e) {

            AddZoneToGrid();

        });

        $('.deleteButton').unbind("click");
        $('.deleteButton').click(function (e) {
            $(".checkAll:first").removeProp("checked");

            $("input[type=checkbox]:checked").parents("#paginatedGrid > tbody > tr").remove();
        });


        $('#state').change(function () {
            $("#spMessage").text("");

            $("#city")[0].options.length = 1;
            $("#county")[0].options.length = 1;

            var t = $(this);
            var state = $("#state option:selected").val();

            showLoading(t);
            $.get('<%= ResolveUrl(string.Format("~/CTE/CollectionEntitiesConfiguration/GetCities/")) %>', { state: state }, function (response) {
                if (response.result) {
                    hideLoading(t);
                    $('#city').children('option:not(:first)').remove();
                    $('#county').children('option:not(:first)').remove();
                    if (response.Cities) {
                        for (var i = 0; i < response.Cities.length; i++) {
                            $('#city').append('<option value="' + response.Cities[i] + '">' + response.Cities[i] + '</option>');
                        }
                    }
                } else {
                    showMessage(response.message, true);
                }
            });
        });


        $('#city').change(function () {
            $('#county').prop('selectedIndex', 0);
            var t = $(this);
            //var city = $("#city option:selected").html();
            //var state = $("#state option:selected").html();
            var data = { state: $("#state option:selected").val(), city: $("#city option:selected").val() }, t = $(this);

            showLoading(t);
            $.get('<%= ResolveUrl(string.Format("~/CTE/CollectionEntitiesConfiguration/GetCounty/")) %>', data, function (response) {
                if (response.result) {
                    hideLoading(t);
                    $('#county').children('option:not(:first)').remove();
                    if (response.County) {
                        for (var i = 0; i < response.County.length; i++) {
                            $('#county').append('<option value="' + response.County[i] + '">' + response.County[i] + '</option>');
                        }
                    }
                } else {
                    showMessage(response.message, true);
                }
            });
        });


        $('#btnSave').click(function () {

            var numbersOnly = /^\d+$/;
            var decimalOnly = /^\s*-?[1-9]\d*(\.\d{1,2})?\s*$/;
            var errCount = 0;

            if ($('#LocationRestrictionsTable').checkRequiredFields()) {

                if (errCount == 0) {
                    var data = { collectionEntityID: $('#hdnCollecEntityID').val() };

                    $('#paginatedGrid tbody:first tr').each(function (i) {
                        
                        data['zones[' + i + '].Name'] = $(this).find("td").eq(1).html();
                        data['zones[' + i + '].Value'] = $(this).find("td").eq(2).html();
                        data['zones[' + i + '].Except'] = $(this).find("td").eq(3).html();
                    });

                    $.post('/CTE/CollectionEntitiesConfiguration/SaveZones', data, function (response) {
                        if (response.result) {
                            showMessage("Location was saved!", false);
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            }

        });

    });

    </script>  
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">

<script type="text/javascript">

    function AddZoneToGrid() {

        var state = $("#state").val();
        var city = $("#city").val();
        var county = $("#county").val();
        var col3 = $('#checkExcept').is(":checked");
        var scopeLevelID;

        if (county != "")
            scopeLevelID = 3;
        else if (city != "")
            scopeLevelID = 2;
        else if (state != "")
            scopeLevelID = 1;

        if (scopeLevelID == 1 || scopeLevelID == 2 || scopeLevelID == 3) {
            $.get('<%= ResolveUrl(string.Format("~/CTE/CollectionEntitiesConfiguration/GetScopeLevels/")) %>', { scopeLevelID: scopeLevelID }, function (response) {
                if (response.result) {
                    if (response.scopeLevels) {

                        row = $('<tr class="Alt hover"></tr>');
                        check = $('<td>' + '<input id="0" type="checkbox" level="' + scopeLevelID + '" >' + '</td>');
                        colState = $('<td>' + response.scopeLevels + '</td>');
                        if (scopeLevelID == 1) colCity = $('<td>' + state + '</td>');
                        if (scopeLevelID == 2) colCity = $('<td>' + city + '</td>');
                        if (scopeLevelID == 3) colCity = $('<td>' + county + '</td>');
                        colExcept = $('<td>' + col3 + '</td>');

                        row.append(check, colState, colCity, colExcept).prependTo("#paginatedGrid tbody");
                    }
                } else {
                    showMessage(response.message, true);
                }
            });

            $(".checkAll:first").removeProp("checked");
            $("#checkExcept").removeProp("checked");
            $("#paginatedGrid > tbody > tr").each(function () {
                if ($(this).children("td").length == 1) {
                    $(this).remove();
                }
            });

        }   // End If
        else {
            $("#spMessage").text("Select a State");
        }
    } // END AddZoneToGrid
    
</script>

</asp:Content>
