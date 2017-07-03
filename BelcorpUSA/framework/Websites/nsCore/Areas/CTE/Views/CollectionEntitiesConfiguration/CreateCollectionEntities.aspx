<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/MeansOfCollections.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.CollectionEntitiesData>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="YellowWidget" runat="server">
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">   
	    <div class = "SectionNav"> 
	        <ul class="SectionLinks">
		        <li><a class="selected" href="<%= ResolveUrl("~/CTE/CollectionEntitiesConfiguration/CreateCollectionEntities/" + Model.CollectionEntityID) %>">
			        <span>Create Collection Entities</span></a></li>
		        <%
			        if (Model.CollectionEntityID > 0)
			        {
		        %>
		        <li>
			        <%: Html.ActionLink(Html.Term("RestrictMeansOfCollection", "Restrict Means Of Collection"), "RestrictCollections", new { id = Model.CollectionEntityID })%></li>
		        <%
			        }
		        %>
	        </ul>
        </div>    
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("CreateCollectionEntity", "Create Collection Entity")%></h2>

             <a href="<%= ResolveUrl("~/CTE/CollectionEntitiesConfiguration/BrowseCollections") %>"><%= Html.Term("BrowseMeansOfCollection", "Browse Means of Collection")%></a>

	</div>
    <%
        var paymentType = SmallCollectionCache.Instance.PaymentTypes.Where(x=>x.PaymentTypeID == Model.PaymentTypeID);
        		
	%>
    <table class="FormContainer splitCol pad5 FL" id="newCollectionLeft" width="100%">
        
        <tr id = "paymentTypes" style="display: block;">
            <td class="FLabel"><%= Html.Term("PaymentType", "Payment Type")%>:
            </td>
            <td>
                <select id="paymentTypesid" class="required" name="<%= Html.JavascriptTerm("ValNegotiation", "Not selected negotiation.") %>">
                    <option value=""><%= Html.Term("SelectPaymentType", "Select Payment Type")%></option>
                        <% foreach (var items in NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowsePaymentTypes().Where(a => a.Active))
                           {
                            %>
                             <option value="<%=items.PaymentTypeID %>" <%= Model.PaymentTypeID == items.PaymentTypeID ? "selected=\"selected\"" : "" %>><%=items.Name%></option>
                            <%                                       
                            }                    
                        %>                        
                </select>
            </td>
        </tr>
        
        <tr id="collectionEntityCreditCard" style="display: none;">
            <td class="FLabel"><%= Html.Term("CollectionEntity", "Collection Entity")%>:
            </td>
            <td>
                <select id="collectionEntityCreditCardid" class="required" name="<%= Html.JavascriptTerm("ValPaymentGateway", "Not selected PaymentGateway.") %>" >
                    <option value=""><%= Html.Term("SelectGateway", "Select a Gateway")%></option>
                        <% foreach (var items in SmallCollectionCache.Instance.PaymentGateways.Where(a => a.Active))
                           {
                            %>
                             <option value="<%=items.PaymentGatewayID %>" <%= Model.PaymentGatewayID == items.PaymentGatewayID ? "selected=\"selected\"" : "" %>><%=items.Name%></option>
                            <%                                       
                           }                    
                        %>                        
                </select>
            </td>
        </tr>
        <tr id = "locationCreditCard" style="display: none;">
            <td class="FLabel"><%= Html.Term("Location", "Location")%>:
            </td>
            <td>
                <select id="locationCreditCardid" class="required" name="<%= Html.JavascriptTerm("ValLocation", "Not selected location.") %>">
                    <option value=""><%= Html.Term("Select Location", "Select Location")%></option>
                        <% foreach (var items in NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCompanies())
                           {
                            %>
                             <option value="<%=items.CompanyID %>" <%= Model.CompanyID == items.CompanyID ? "selected=\"selected\"" : "" %>><%=items.CompanyName%></option>
                            <%                                       
                           }                    
                        %>                        
                </select>
            </td>
        </tr>
        <tr id="collectionEntityPaymentTicket" style="display: none;">
            <td class="FLabel"><%= Html.Term("CollectionEntity", "Collection Entity")%>:
            </td>
            <td>
                <select id="collectionEntityPaymentTicketid" class="required" name="<%= Html.JavascriptTerm("ValBank", "Not selected Bank.") %>">
                    <option value="0"><%= Html.Term("SelectBank", "Select a Bank")%></option>
                        <% foreach (var items in NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseBanks())
                           {
                            %>
                             <option value="<%=items.BankID %>" <%= Model.BankID == items.BankID ? "selected=\"selected\"" : "" %>><%=items.BankName%></option>
                            <%                                       
                           }                    
                        %>                        
                </select>
            </td>
        </tr>

        <tr id = "BankAgencie" style="display: none;">
            <td class="FLabel"><%= Html.Term("BankAgencie", "Bank Agencie")%>:
            </td>
            <td><input type="text" id="BankAgencieid" value="<%=Model.BankAgencie%>" class="clear required" name="<%= Html.JavascriptTerm("BankAgencie", "BankAgencie required.") %>"/></td>
        </tr>
        <tr id = "BankAccountNumber" style="display: none;">
            <td class="FLabel"><%= Html.Term("AccountNumber", "Account Number")%>:
            </td>
            <td><input type="text" id="BankAccountNumberid" value="<%=Model.BankAccountNumber%>" class="clear required" name="<%= Html.JavascriptTerm("BankAccountNumber", "BankAccountNumber required.") %>"/></td>
        </tr>
        <tr id ="AccountType" style="display: none;">
            <td class="FLabel"><%= Html.Term("AccountType", "AccountType")%>:
            </td>
            <td>
                <select id="AccountTypeid" class="required" name="<%= Html.JavascriptTerm("ValNegotiation", "Not selected negotiation.") %>">
                    <option value=""><%= Html.Term("SelectBankAccountType", "Select Bank Account Type")%></option>
                    <option value="0" <%= Model.BankAccountType == 0 ? "selected=\"selected\"" : "" %>><%= Html.Term("Current", "Current")%></option>
                    <option value="1" <%= Model.BankAccountType == 1 ? "selected=\"selected\"" : "" %>><%= Html.Term("Savings", "Savings")%></option>                           
                </select>
            </td>
        </tr>
        
           
           <tr id = "FileNameBankCollection" style="display: none;">
            <td class="FLabel"><%= Html.Term("FileNameBankCollection", "File Name Bank Collection")%>:
            </td>
            <td><input type="text" id="txtFileNameBankCollection" value="<%=Model.FileNameBankCollection%>" class="clear required" name="<%= Html.JavascriptTerm("BankAccountNumber", "BankAccountNumber required.") %>"/></td>
          </tr>

         <tr id = "InitialPositionName" style="display: none;">
            <td class="FLabel"><%= Html.Term("InitialPositionName", "Initial Position")%>:
            </td>
            <td><input type="text" id="txtInitialPositionName" value="<%=Model.InitialPositionName%>" class="clear required" name="<%= Html.JavascriptTerm("BankAccountNumber", "BankAccountNumber required.") %>"/></td>
        </tr>

         <tr id = "FinalPositionName" style="display: none;">
            <td class="FLabel"><%= Html.Term("FinalPositionName", "Final Position")%>:
            </td>
            <td><input type="text" id="txtFinalPositionName" value="<%=Model.FinalPositionName%>" class="clear required" name="<%= Html.JavascriptTerm("BankAccountNumber", "BankAccountNumber required.") %>"/></td>
        </tr>
         <tr id = "CodeDetail" style="display: none;">
            <td class="FLabel"><%= Html.Term("CodeDetail", "codeDetail")%>:
            </td>
            <td><input type="text" id="txtCodeDetail" value="<%=Model.CodeDetail%>" class="clear required" name="<%= Html.JavascriptTerm("CodeDetail", "Code Detail.") %>"/></td>
        </tr>
        

    </table>
    <table class="FormContainer splitCol pad5 FR" id="newCollectionRight" width="100%">
        
        <tr id = "collectionEntityNameCreditCard"  style="display: none;">
            <td class="FLabel"><%= Html.Term("CollecitonEntityName", "Collection Entity Name") %>:
            </td>
            <td><input type="text" id="collectionEntityNameCreditCardid" value="<%=Model.CollectionEntityName%>" class="clear required" name="<%= Html.JavascriptTerm("CollectionEntityName", "Collection Entity Name required.") %>"/></td>
        </tr>
        <tr id = "chkStatusCreditCard"  style="display: none;">
            <td class="FLabel"><%= Html.Term("Status", "Status") %>:
            </td>
            <td><input type="checkbox" id ="chkStatusCreditCardid" <%= Model.Active ==1 ? "checked=\"Checked\"" : "" %> /></td>
        </tr>

        <tr id = "saveCollectionCreditCard" style="display: none;">
			
			<td>
				<p>
					<a id="saveCollectionCreditCardid" href="javascript:void(0);" class="Button BigBlue"><span>
						<%= Html.Term("Save", "Save") %></span></a></p>
			</td>
		</tr>

        <tr id= "LocationPaymentTicket" style="display: none;">
            <td class="FLabel"><%= Html.Term("Location", "Location")%>:
            </td>
            <td>
                <select id="LocationPaymentTicketid" class="required" name="<%= Html.JavascriptTerm("ValCompany", "Not selected location.") %>">
                    <option value=""><%= Html.Term("Select Location", "Select Location")%></option>
                        <% foreach (var items in NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCompanies())
                            {
                            %>
                                <option value="<%=items.CompanyID %>" <%= Model.CompanyID == items.CompanyID ? "selected=\"selected\"" : "" %>><%=items.CompanyName%></option>
                            <%                                       
                            }                    
                        %>                        
                </select>
            </td>
        </tr>
        <tr id="CollectionTypePaymentTicket" style="display: none;">
                <td class="FLabel"><%= Html.Term("CollectionType", "Collection Type")%>:
                </td>
                <td>
                    <select id="CollectionTypePaymentTicketid" class="required" name="<%= Html.JavascriptTerm("ValCollectionTypePerBank", "Not selected Collection Type Bank.") %>">
                        <option value=""><%= Html.Term("SelectCollectionType", "Select Collection Type")%></option>
                            <% foreach (var items in NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCollectionTypesPerBank().Where(x => x.BankID == Model.BankID))
                               {
                                %>
                                 <option value="<%=items.CollectionTypesPerBankID %>" <%= Model.CollectionTypePerBankID == items.CollectionTypesPerBankID ? "selected=\"selected\"" : "" %>><%=items.TermName%></option>
                                <%                                       
                                }                    
                            %>                        
                    </select>
                </td>
        </tr>
        <tr id="collectionDocumentPaymentTicket" style="display: none;">
                <td class="FLabel"><%= Html.Term("CollectionDocument", "Collection Document")%>:
                </td>
                <td>
                    <select id="collectionDocumentPaymentTicketid" class="required" name="<%= Html.JavascriptTerm("ValCollectionDocumentType", "Not selected Collection Document Type.") %>">
                        <option value=""><%= Html.Term("SelectCollectionDocument", "Select Collection Document")%></option>
                            <% foreach (var items in NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCollectionDocumentsPerBank().Where(x => x.BankID == Model.BankID))
                               {
                                %>
                                 <option value="<%=items.CollectionDocumentsPerBankID %>" <%= Model.CollectionDocumentTypePerBankID == items.CollectionDocumentsPerBankID ? "selected=\"selected\"" : "" %>><%=items.TermName%></option>
                                <%
                                }                    
                            %>                        
                    </select>
                </td>
        </tr>
        <tr id="collectionAgreement" style="display: none;">
            <td class="FLabel"><%= Html.Term("CollectionAgreement", "Collection Agreement") %>:
            </td>
            <td><input type="text" id="collectionAgreementid" value="<%=Model.CollectionAgreement%>" class="clear required" name="<%= Html.JavascriptTerm("CollectionAgreementInvalid", "Collection Agreement Invalid.") %>"/></td>
        </tr>
        <tr id="collectionEntityNamePaymentTicket" style="display: none;">
            <td class="FLabel"><%= Html.Term("CollecitonEntityName", "Collection Entity Name") %>:
            </td>
            <td><input type="text" id="collectionEntityNamePaymentTicketid" value="<%=Model.CollectionEntityName%>" class="clear required" name="<%= Html.JavascriptTerm("CollectionEntityNameInvalid", "Collection Entity Name Invalid.") %>"/></td>
        </tr>
        <tr id="chkStatusPaymentTicket" style="display: none;">
            <td class="FLabel"><%= Html.Term("Status", "Status") %>:
            </td>
            <td><input type="checkbox" id ="chkStatusPaymentTicketid" <%= Model.Active ==1 ? "checked=\"Checked\"" : "" %> /></td>
        </tr>

        <tr id="SaveCollectionPaymentTicket" style="display: none;">
			
			<td>
				<p>
					<a id="SaveCollectionPaymentTicketid" href="javascript:void(0);" class="Button BigBlue"><span>
						<%= Html.Term("Save", "Save") %></span></a></p>
			</td>
		</tr>
         
    </table>

   

   
<% 
	
	var creditCard = Constants.PaymentType.CreditCard.ToInt();
    var paymentTicket = NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.GetPaymentTicketID();
%>


<script type="text/javascript">
    $(function () {

         
        var Initialstructure = $("#paymentTypesid option:selected").val();           
        if (parseInt(Initialstructure) == <%=creditCard%>) {
            document.getElementById('paymentTypes').style.display = 'block';
            document.getElementById('collectionEntityCreditCard').style.display = 'block';
            document.getElementById('locationCreditCard').style.display = 'block';
            document.getElementById('collectionEntityPaymentTicket').style.display = 'none';
            document.getElementById('BankAgencie').style.display = 'none';
            document.getElementById('BankAccountNumber').style.display = 'none';
            document.getElementById('AccountType').style.display = 'none';
            document.getElementById('collectionEntityNameCreditCard').style.display = 'block';
            document.getElementById('chkStatusCreditCard').style.display = 'block';
            document.getElementById('saveCollectionCreditCard').style.display = 'block';
            document.getElementById('LocationPaymentTicket').style.display = 'none';
            document.getElementById('CollectionTypePaymentTicket').style.display = 'none';
            document.getElementById('collectionDocumentPaymentTicket').style.display = 'none';
            document.getElementById('collectionAgreement').style.display = 'none';
            document.getElementById('collectionEntityNamePaymentTicket').style.display = 'none';
            document.getElementById('chkStatusPaymentTicket').style.display = 'none';
            document.getElementById('SaveCollectionPaymentTicket').style.display = 'none';
            document.getElementById('FileNameBankCollection').style.display = 'none';
            document.getElementById('InitialPositionName').style.display = 'none';
            document.getElementById('FinalPositionName').style.display = 'none';
            document.getElementById('CodeDetail').style.display = 'none';
                
        }
        if (parseInt(Initialstructure) == <%=paymentTicket%>) {
            document.getElementById('paymentTypes').style.display = 'block';
            document.getElementById('collectionEntityCreditCard').style.display = 'none';                
            document.getElementById('locationCreditCard').style.display = 'none';
            document.getElementById('collectionEntityPaymentTicket').style.display = 'block';
            document.getElementById('BankAgencie').style.display = 'block';
            document.getElementById('BankAccountNumber').style.display = 'block';
            document.getElementById('AccountType').style.display = 'block';
            document.getElementById('collectionEntityNameCreditCard').style.display = 'none';
            document.getElementById('chkStatusCreditCard').style.display = 'none';
            document.getElementById('saveCollectionCreditCard').style.display = 'none';
            document.getElementById('LocationPaymentTicket').style.display = 'block';
            document.getElementById('CollectionTypePaymentTicket').style.display = 'block';
            document.getElementById('collectionDocumentPaymentTicket').style.display = 'block';
            document.getElementById('collectionAgreement').style.display = 'block';
            document.getElementById('collectionEntityNamePaymentTicket').style.display = 'block';
            document.getElementById('chkStatusPaymentTicket').style.display = 'block';
            document.getElementById('SaveCollectionPaymentTicket').style.display = 'block';
            document.getElementById('FileNameBankCollection').style.display = 'block';
            document.getElementById('InitialPositionName').style.display = 'block';
            document.getElementById('FinalPositionName').style.display = 'block';
            document.getElementById('CodeDetail').style.display = 'block';
        }

        $('#paymentTypesid').change(function () {
            var structure = $("#paymentTypesid option:selected").val();           
            if (parseInt(structure) == <%=creditCard%>) {
                document.getElementById('paymentTypes').style.display = 'block';
                document.getElementById('collectionEntityCreditCard').style.display = 'block';
                document.getElementById('locationCreditCard').style.display = 'block';
                document.getElementById('collectionEntityPaymentTicket').style.display = 'none';
                document.getElementById('BankAgencie').style.display = 'none';
                document.getElementById('BankAccountNumber').style.display = 'none';
                document.getElementById('AccountType').style.display = 'none';
                document.getElementById('collectionEntityNameCreditCard').style.display = 'block';
                document.getElementById('chkStatusCreditCard').style.display = 'block';
                document.getElementById('saveCollectionCreditCard').style.display = 'block';
                document.getElementById('LocationPaymentTicket').style.display = 'none';
                document.getElementById('CollectionTypePaymentTicket').style.display = 'none';
                document.getElementById('collectionDocumentPaymentTicket').style.display = 'none';
                document.getElementById('collectionAgreement').style.display = 'none';
                document.getElementById('collectionEntityNamePaymentTicket').style.display = 'none';
                document.getElementById('chkStatusPaymentTicket').style.display = 'none';
                document.getElementById('SaveCollectionPaymentTicket').style.display = 'none';
                document.getElementById('FileNameBankCollection').style.display = 'none';
                document.getElementById('InitialPositionName').style.display = 'none';
                document.getElementById('FinalPositionName').style.display = 'none';
                document.getElementById('CodeDetail').style.display = 'none';
                
            }
            else if (parseInt(structure) == <%=paymentTicket%>) {
                document.getElementById('paymentTypes').style.display = 'block';
                document.getElementById('collectionEntityCreditCard').style.display = 'none';                
                document.getElementById('locationCreditCard').style.display = 'none';
                document.getElementById('collectionEntityPaymentTicket').style.display = 'block';
                document.getElementById('BankAgencie').style.display = 'block';
                document.getElementById('BankAccountNumber').style.display = 'block';
                document.getElementById('AccountType').style.display = 'block';
                document.getElementById('collectionEntityNameCreditCard').style.display = 'none';
                document.getElementById('chkStatusCreditCard').style.display = 'none';
                document.getElementById('saveCollectionCreditCard').style.display = 'none';
                document.getElementById('LocationPaymentTicket').style.display = 'block';
                document.getElementById('CollectionTypePaymentTicket').style.display = 'block';
                document.getElementById('collectionDocumentPaymentTicket').style.display = 'block';
                document.getElementById('collectionAgreement').style.display = 'block';
                document.getElementById('collectionEntityNamePaymentTicket').style.display = 'block';
                document.getElementById('chkStatusPaymentTicket').style.display = 'block';
                document.getElementById('SaveCollectionPaymentTicket').style.display = 'block';
                document.getElementById('FileNameBankCollection').style.display = 'block';
                document.getElementById('InitialPositionName').style.display = 'block';
                document.getElementById('FinalPositionName').style.display = 'block';
                document.getElementById('CodeDetail').style.display = 'block';
            }
            else{
                document.getElementById('paymentTypes').style.display = 'block';
                document.getElementById('collectionEntityCreditCard').style.display = 'none';                
                document.getElementById('locationCreditCard').style.display = 'none';
                document.getElementById('collectionEntityPaymentTicket').style.display = 'none';
                document.getElementById('BankAgencie').style.display = 'none';
                document.getElementById('BankAccountNumber').style.display = 'none';
                document.getElementById('AccountType').style.display = 'none';
                document.getElementById('collectionEntityNameCreditCard').style.display = 'none';
                document.getElementById('chkStatusCreditCard').style.display = 'none';
                document.getElementById('saveCollectionCreditCard').style.display = 'none';
                document.getElementById('LocationPaymentTicket').style.display = 'none';
                document.getElementById('CollectionTypePaymentTicket').style.display = 'none';
                document.getElementById('collectionDocumentPaymentTicket').style.display = 'none';
                document.getElementById('collectionAgreement').style.display = 'none';
                document.getElementById('collectionEntityNamePaymentTicket').style.display = 'none';
                document.getElementById('chkStatusPaymentTicket').style.display = 'none';
                document.getElementById('SaveCollectionPaymentTicket').style.display = 'none';
                document.getElementById('FileNameBankCollection').style.display = 'none';
                document.getElementById('InitialPositionName').style.display = 'none';
                document.getElementById('FinalPositionName').style.display = 'none';
                document.getElementById('CodeDetail').style.display = 'none';
            }

        });


        //Changes for DocumentType and Types per Bank

         $('#collectionEntityPaymentTicketid').change(function () {
            $('#CollectionTypePaymentTicketid').prop('selectedIndex', 0);
            $('#collectionDocumentPaymentTicketid').prop('selectedIndex', 0);
            var t = $(this);
            var BankID = $("#collectionEntityPaymentTicketid option:selected").val();

            showLoading(t);
            $.get('<%= ResolveUrl(string.Format("~/CTE/CollectionEntitiesConfiguration/GetCollectionType/")) %>', { BankID: BankID }, function (response) {
                if (response.result) {
                    hideLoading(t);
                    $('#CollectionTypePaymentTicketid').children('option:not(:first)').remove();
                    if (response.CollectionType) {
                        for (var i = 0; i < response.CollectionType.length; i++) {
                            $('#CollectionTypePaymentTicketid').append('<option value="' + response.CollectionType[i].CollectionTypesPerBankID + '">' + response.CollectionType[i].Name + '</option>');
                        }
                    }
                } else {
                    showMessage(response.message, true);
                }
            });

            
            $.get('<%= ResolveUrl(string.Format("~/CTE/CollectionEntitiesConfiguration/GetCollectionDocument/")) %>', { BankID: BankID }, function (response) {
                if (response.result) {
                    hideLoading(t);
                    $('#collectionDocumentPaymentTicketid').children('option:not(:first)').remove();
                    if (response.CollectionDocument) {
                        for (var i = 0; i < response.CollectionDocument.length; i++) 
                        {
                            $('#collectionDocumentPaymentTicketid').append('<option value="' + response.CollectionDocument[i].CollectionDocumentsPerBankID + '">' + response.CollectionDocument[i].Name + '</option>');
                        }
                    }
                } else {
                    showMessage(response.message, true);
                }
            });
         });


        $('#SaveCollectionPaymentTicketid').click(function () {


	            var numbersOnly = /^\d+$/;
	            var decimalOnly = /^\s*-?[1-9]\d*(\.\d{1,2})?\s*$/;
	            var errCount = 0;
	            if ($('#newCollectionLeft').checkRequiredFields()) {
	                if ($('#newCollectionRight').checkRequiredFields()) {
	                    

	                    if (!numbersOnly.test($('#BankAgencieid').val())) {
	                        $('#BankAgencieid').showError('<%= Html.JavascriptTerm("BankAgencieInvalid", "Bank Agencie Invalid.") %>');
	                        errCount++;
	                        return false;
	                    }
                        if (!numbersOnly.test($('#BankAccountNumberid').val())) {
	                        $('#BankAccountNumberid').showError('<%= Html.JavascriptTerm("BankAccountNumberInvalid", "Bank Account Number Invalid.") %>');
	                        errCount++;
	                        return false;
	                    }
                        if (!numbersOnly.test($('#collectionAgreementid').val())) {
	                        $('#collectionAgreementid').showError('<%= Html.JavascriptTerm("CollectionAgreementInvalid", "Collection Agreement Invalid.") %>');
	                        errCount++;
	                        return false;
	                    }
	                    

	                    if (errCount == 0) {
                            $("#newCollectionLeft").find("input select").each(function(){
                                $(this).removeAttr("style").next().remove();
                            });
                            $("#newCollectionRight").find("input, select").each(function(){
                                $(this).removeAttr("style").next().remove();
                            });

	                        var data = { 
                                collectionID: '<%= (ViewBag.CollectionId ?? 0) %>',
                                paymentTypeID: $('#paymentTypesid').val(), collectionEntityID: $('#collectionEntityPaymentTicketid').val(),
	                            bankAgencie: $('#BankAgencieid').val(), bankAccountNumber: $('#BankAccountNumberid').val(),
	                            accountType: $('#AccountTypeid').val(), location: $('#LocationPaymentTicketid').val(),
	                            collectionType: $('#CollectionTypePaymentTicketid').val(), collectionDocument: $('#collectionDocumentPaymentTicketid').val(),
                                collectionAgreement: $('#collectionAgreementid').val(), collectionEntityName: $('#collectionEntityNamePaymentTicketid').val(),
	                            chkStatus: $('#chkStatusPaymentTicketid').is(":checked")
                                ,fileNameBankCollection: $('#txtFileNameBankCollection').val(),
                                initialPositionName: $('#txtInitialPositionName').val(), 
                                finalPositionName: $('#txtFinalPositionName').val(),
                                codeDetail:$('#txtCodeDetail').val(),
	                        }, t = $(this);
	                        

	                        $.post('/CTE/CollectionEntitiesConfiguration/SaveCollectionEntitiesPaymentTicket', data, function (response) {
	                            if (response.result) {
	                                showMessage("Collection Entity was saved!", false);
                                    if (response.collectionId != 0)
                                        window.location.href = window.location.href + '/' + response.collectionId;
	                            } else {
	                                showMessage(response.message, true);
	                            }
	                        });
	                    }
	                }
	                
	            }
	        });

            $('#saveCollectionCreditCardid').click(function () {


	            var numbersOnly = /^\d+$/;
	            var decimalOnly = /^\s*-?[1-9]\d*(\.\d{1,2})?\s*$/;
	            var errCount = 0;
	            if ($('#newCollectionLeft').checkRequiredFields()) {
	                if ($('#newCollectionRight').checkRequiredFields()) {
	                   
	                        
	                    if (errCount == 0) {
	                        var data = { 
                                collectionID: '<%= (ViewBag.CollectionId ?? 0) %>',
                                paymentTypeID: $('#paymentTypesid').val(), collectionEntityID: $('#collectionEntityCreditCardid').val(),
	                            location: $('#locationCreditCardid').val(),collectionEntityName: $('#collectionEntityNameCreditCardid').val(),
	                            chkStatus: $('#chkStatusCreditCardid').is(":checked")
	                        }, t = $(this);
	                      

	                        $.post('/CTE/CollectionEntitiesConfiguration/SaveCollectionEntitiesCreditCard', data, function (response) {
	                            if (response.result) {
	                                showMessage("Collection Entity was saved!", false);
                                    if (response.collectionId != 0)
                                        window.location.href = window.location.href + '/' + response.collectionId;
	                            } else {
	                                showMessage(response.message, true);
	                            }
	                        });
	                    }
	                }
	                
	            }
	        });

        
    });
    </script>  
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>