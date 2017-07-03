<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/CTE.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.RenegotiationSearchParameters>" %>
<%--Inherits="System.Web.Mvc.ViewPage<dynamic>" --%>


<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">



<div id="noteModal" class="jqmWindow LModal" style="left: 510px; z-index: 3000; top: 100px;" >

        <div class="mContent">
            <h2>
                <%= Html.Term("RuleDetails","Rules Details")%>
            </h2>
            <table id="campaignAction" class="FormTable" width="1230px">
    <tr id="valuePanel">          
            <td id="taskAdd"> 

            <table class="DataGrid" width="100%">

               <thead>
                        <tr class="GridColHead">
                           
                            <th >
                                <%= Html.Term("Negotiation", "Negotiation")%>
                            </th>                                                   
                            <th>
                                <%= Html.Term("OpeningDay", "OpeningDay")%>
                            </th> 
                            
                            <th>
                                <%= Html.Term("FinalDay", "FinalDay")%>
                            </th>                           
                            <th>
                                <%= Html.Term("FinePercentage", "FinePercentage")%>
                            </th>   
                            
                            <th>
                                <%= Html.Term("AppliedValue", "AppliedValue")%>
                            </th>                           
                            <th>
                                <%= Html.Term("Minimum Debt", "Minimum Debt")%>
                            </th>   

                            <th>
                                <%= Html.Term("InterestPercentage", "InterestPercentage")%>
                            </th>                           
                            <th>
                                <%= Html.Term("Applied Value", "Applied Value")%>
                            </th>   
                            
                            <th>
                                <%= Html.Term("Discount", "Discount")%>
                            </th>                           
                            <th>
                                <%= Html.Term("Applied Value", "Applied Value")%>
                            </th>   
                            
                                                        
                        </tr>
                    </thead>
               <tbody id="tblinformacion">
               
                  
               </tbody>
            </table>
            </td></tr></table>
            <p>
                <a href="javascript:void(0);" class="Button LinkCancel jqmClose" 
                    id="btnCancelObservacion" onclick="Ocultar()">
                    <%= Html.Term("Close","Close")%></a>
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>

  


	<a href="<%= ResolveUrl("~/CTE") %>">
		<%= Html.Term("RenegotiationMethods")%></a>
			<%= Html.Term("BrowseRenegotiation", "Browse Renegotiation Methods ")%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("RenegotiationMethods")%></h2>
		<%= Html.Term("BrowseRenegotiationMethods", "Browse Renegotiation Methods")%> | <a href="<%= ResolveUrl("~/CTE/RenegotiationMethods/Renegotiation") %>"><%= Html.Term("CreateNewRenegotiationMethod", "Create New Renegotiation Method")%></a>
       
	</div>
	<% Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.RenegotiationSearchData>("~/CTE/RenegotiationMethods/GetRenegotiationMethods")
		.AutoGenerateColumns()
        .HideClientSpecificColumns_()
        .AddInputFilter(Html.Term("Description", "Description"), "desc")   
		.Render(); %>

       
        <script type="text/javascript">
          $(function () {
          $('#noteModal').jqm({
                modal: false
            });

        });

        function Ocultar() {
            $("#noteModal").css("display", "none");
        };

            function ViewDetails(fineAndInterestRulesID) {


               
                var url = '<%= ResolveUrl("~/CTE/RenegotiationMethods/GetRulesDetailsModal") %>';


                var odata = JSON.stringify({ FineAndInterestRulesID: fineAndInterestRulesID });

                $.ajax({
                    data: odata,
                    url: url,
                    dataType: "json",
                    type: "POST",
                    contentType: "application/json",
                    success: function (response) {
                        var html = response.Items;

                        $("#tblinformacion").html(html)
//                        $('#noteModal').jqmShow();
                        $("#noteModal").css("display", "block");

                    },
                    error: function (error) {
                    }
                });
            }
    </script>
</asp:Content>

     