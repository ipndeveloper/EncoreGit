<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderRules.Service.DTO.RuleValidationsDTO>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.AccountIDs.Count > 0 ? "#acctIDsY" : "#acctIDsN";  
	%>
	$(function () { 
		 
        $('<%= radioID %>').attr('checked', 'checked').trigger('change'); 
      
        $('#btnBrowse').click(function(){
        
          $('#inputLoadAccounts').trigger('click');
        });
        $('#accountIDsCheckAll').click(function () { $('#accountIDsGrid .selectRow').attr('checked', $(this).prop('checked')); });
      
      
      

       $('#paginateAccountIDsGrid .deleteButton').click(function () {
			$('#accountIDsGrid .selectRow:checked').each(function () {
				$(this).closest('tr').remove();
			});
			$('#accountIDsGrid').refreshTable();
		});

        document.getElementById('formLoad').onsubmit = function(){
       
               var formdata = new FormData(); //FormData object
                var fileInput = document.getElementById('inputLoadAccounts');
                //Iterating through each files selected in fileInput
                for (i = 0; i < fileInput.files.length; i++) {
                 //Appending each file to FormData object
                    formdata.append(fileInput.files[i].name, fileInput.files[i]);
                }
                //Creating an XMLHttpRequest and sending
                
                var xhr = new XMLHttpRequest();
                xhr.open('POST', '/Products/File/SubmitFileAndaddAccounts');
                xhr.send(formdata);
                $('#btnBrowse').showLoading();
                xhr.onreadystatechange = function () {
                    if (xhr.readyState == 4 && xhr.status == 200) {
                        var response=JSON.parse(xhr.responseText);
                        if (response.result) {
                              var items = $('#accountIDsGrid tbody tr');
                               items.each(function (i) {
                                  var int = parseInt($(this).find('.accountId').val());
                                  var index = response.ids.indexOf(int);
                                  if(index != -1){
                                    response.ids.splice(index,1);
                                    response.names.splice(index,1);
                                  }
                               });
                             for(var i=0; i<response.ids.length;i++){
                                    $('#accountIDsGrid').prepend('<tr>'
						            + '<td><input type="checkbox" class="selectRow" /><input type="hidden" class="accountId" value="'+response.ids[i]+'" /></td>'
						            + '<td>' + response.ids[i] + '</td>'
						            + '<td>' + response.names[i] + '</td>'
						            + '</tr>').refreshTable();
					         }
                           
                            setAutoAddGridVisibility();
                        }else{
                            showMessage(response.message, true);
                        }
                    }
                      $('#btnBrowse').hideLoading();
                }
            
               return false;
       }

        $('#inputLoadAccounts').change(function(e){
           $('#submitHidden').trigger('click');
        });


       function setAutoAddGridVisibility() {
			if ($('#accountIDsGrid tbody tr').length) {
				$('#accountIDsGrid').show();
			}
			else {
				$('#accountIDsGrid').hide();
			}
		}
   });
   
   

</script>
<div class="pad5 promotionOption AccountIDs">
	<div class="FL optionHelpIcons">
    </div>
	<div class="FLabel">
		<label for="acctIDs">
			<%=Html.Term("PromotionOptions_AccountIDsOption", "Restrict to Accounts IDs?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="accountIDs">
		<span>
			<input type="radio" value="no" name="acctIDs" id="acctIDsN" />
			<label for="acctIDsN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="acctIDs" id="acctIDsY" />
			<label for="acctIDsY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<!-- account type selection panel -->
		<div <%= Model.AccountIDs.Count > 0?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="AccountIDsSelection" >
			
          <form id="formLoad" action="" enctype="multipart/form-data">
               <br/><br />
               <a class="Button BigBlue" id="btnBrowse" href="javascript:void(0);">
               <%= Html.Term("Promotions_LoadAccounts", "Load Accounts from Excel")%>
               </a>
               <label id="label"></label>
               <input type="file" id="inputLoadAccounts" name="inputLoadAccounts" style="display:none"/>
                <br/><br />
               <input type="submit" id="submitHidden" style="display:none"/>
		         
           </form> 
            
            <span class="lawyer">
				<%=Html.Term("RulesOptions_RestrictToAccountIDs", "Only the accounts on this list will contain the rule.")%>
			</span>
		   <table width="100%" class="FormTable">
				<tbody>
					<tr id="AccountPanel">
						<td>
                            <div class="UI-secBg pad10 brdrYYNN">
                            </div>
							<div class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility" id="paginateAccountIDsGrid">
								<a class="deleteButton UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-deleteSelected">
								</span><span>
									<%=Html.Term("DeleteSelected", "Delete Selected") %></span></a>
							</div>
							<table width="100%" class="DataGrid" id="accountIDsGrid">
								<thead>
									<tr class="GridColHead">
										<th class="GridCheckBox">
											<input type="checkbox" id="accountIDsCheckAll" />
										</th>
										<th>
											<%=Html.Term("Account ID")%>
										</th>
										<th>
											<%=Html.Term("Account Name")%>
										</th>
									</tr>
								</thead>
								<tbody>
									<% if (Model.AccountIDs.Count>0)
									{

                                        var accountIDs = Model.AccountIDs;
										foreach (var accountID in accountIDs)
										{
                                            var accountInfo = Account.Load(accountID); %>
											<tr>
												<td>
													<input type="checkbox" class="selectRow" /><input type="hidden" class="accountId" value="<%= accountInfo.AccountID %>" />
												</td>
												<td>
													<%= accountInfo.AccountID%>
												</td>
												<td>
													<%= accountInfo.FullName%>
												</td>
											</tr>
										<% }
									}%>
								</tbody>
							</table>
							<div class="UI-mainBg Pagination" id="AccountPaginatedGridPagination">
								<input type="hidden" id="AccountpaginatedGridRefresh" />
								<div class="PaginationContainer">
									<div class="Bar">
										<a class="previousPage disabled" href="javascript:void(0);"><span>&lt;&lt; Previous</span></a>
										<span class="pages">1 of 1</span><a class="nextPage disabled" href="javascript:void(0);"><span>
											Next &gt;&gt;</span></a> <span class="ClearAll clr"></span>
									</div>
									<div style="" class="PageSize">
										Results Per Page:
										<select class="pageSize">
											<option value="15">15</option>
											<option value="20">20</option>
											<option value="50">50</option>
											<option value="100">100</option>
										</select>
									</div>
									<span class="ClearAll clr"></span>
								</div>
							</div>
						</td>
					</tr>
				</tbody>
			</table>
		
         </div>
		</div>
	</div>
