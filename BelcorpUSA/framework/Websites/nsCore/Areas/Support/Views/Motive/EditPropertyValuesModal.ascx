<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Data.Entities.Business.SupportMotivePropertyValuesSearchData>"%>
<div id="newsletterForm" style="background-color: White; padding: 10px;">
  
<%--
  <p><%= Model.SupportMotivePropertyTypeID %></p>
    --%>

    <table id="campaignAction" class="FormTable" width="100%">
    <tr id="valuePanel">          
            <td id="taskAdd">     
            <div >           
                 <p class="FL">
                    <%= Html.Term("Value", "Value")%>:
                    <input id="txtValue" type="text" class="" value="" />     
                     <input type="hidden" id="supportMotivePropertyTypeID" value="<%= Model.SupportMotivePropertyTypeID %>" />             
                </p>
                 <p class="FR">
                        <a id="btnAddValue" href="javascript:void(0);" class=""  >
                            <%= Html.Term("AddValue", "Add Value") %></a> | 
                        <a id="btnRemoveValue" href="javascript:void(0);"  class="">
                            <%= Html.Term("RemoveValue", "Remove Value") %></a>
                </p>
            </div>
              
                <span class="ClearAll"></span>
                <!-- Products In Order -->
                <table id="valueItems" width="100%" class="DataGrid">
                    <thead>
                        <tr class="GridColHead">
                            <th class="GridCheckBox">
                                <input id="selectAllValueItems" type="checkbox" />
                            </th>
                            <th>
                                <%= Html.Term("Value", "Value") %>
                            </th>                           
                            <th>
                                <%= Html.Term("SortIndex", "Sort Index")%>
                            </th>                             
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                 <div class="Pagination" style="visibility:hidden">
                    <a href="javascript:void(0);" id="A3">&lt;&lt;
                        <%= Html.Term("Previous")%></a><a href="javascript:void(0);" id="A4" style="margin-left:.909em;"><%= Html.Term("Next", "Next") %>
                            &gt;&gt;</a>
                            <span style="margin-left:.909em;">
                            <%= Html.Term("PageSize", "Page Size") %>:<select id="Select2">
                                <option selected="selected" value="20">20</option>
                                <option value="50">50</option>
                                <option value="100">100</option>
                            </select>
                            </span>
                </div>
            </td>
        </tr>
    </table>

    <script type="text/javascript">
        var currentPage = 0;
        $(function () {

            getTaskItems();

            function getTaskItems() {



                $('#selectAllValueItems').attr('checked', false);
                var t = $('#valueItems tbody');
                t.html('<tr><td colspan="5"><img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');
                $.get('<%= ResolveUrl("~/Support/Motive/GetPropertyValueItems") %>', { page: currentPage, pageSize: $('#pageSize').val(), supportMotivePropertyTypeID: $('#supportMotivePropertyTypeID').val() }, function (response) {
                    if (response.result === undefined || response.result) {
                        t.html(response.taskItems);
                    } else {
                        showMessage(response.message, true);
                    }
                })
            .fail(function () {
                t.html('<tr><td colspan="5"></td></tr>');
                showMessage('<%= Html.JavascriptTerm("ErrorProcessingRequest", "There was a fatal error while processing your request.  If this persists, please contact support.") %>', true);
            });
            }



            $('#btnAddValue').click(function () {
                var isValidValue = false;
                var txtValue = $("#txtValue");
                if ($.trim(txtValue.val()) == '')   
                {
                    txtValue.showError('<%= Html.JavascriptTerm("ValueRequired", "Value is required") %>');
	                isValidValue = false;
                }else{
                    txtValue.clearError();
	                isValidValue = true;
	            }

                if(!isValidValue)
                {
                 return false;
                }
                var data = {
                    supportMotivePropertyTypeID: $('#supportMotivePropertyTypeID').val(),
                    value: txtValue.val()
                };

                $.post('<%= ResolveUrl("~/Support/Motive/AddPropertyValue") %>', data, function (response) {

                    //getTaskItems();
                    if (response.result) {
                        $('#valueItems tbody').append('<tr><td><input type="checkbox"></td><td>' + txtValue.val() + '</td><td>' + ($("#valueItems tbody tr").length + 1) + '</td></tr>');
                       // window.location.reload(true);  No debe recargar la pagina
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });

            $('#btnRemoveValue').click(function () {
                var data = {};
                $('#valueItems > tbody input[type="checkbox"]:checked').each(function (i) {
                    data['PropertyValueIDs[' + i + ']'] = $(this).val();
                });

                $.post('<%= ResolveUrl("~/Support/Motive/RemovePropertyValueItems") %>', data, function (repsonse) {
                    getTaskItems();
                });
            });

        });
        </script>
</div>
