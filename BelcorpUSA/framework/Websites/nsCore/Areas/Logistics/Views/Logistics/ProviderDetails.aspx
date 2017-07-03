<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/LogiticsProvDetail.Master" 
Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content5" ContentPlaceHolderID="LeftNav" runat="server">
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
                <!--<%= Html.SelectedLink("~/Logistics/Logistics/Address/" + LogisticsProviderID, Html.Term("Address", "Address"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>-->
                <%= Html.SelectedLink("~/Logistics/Logistics/Routes/" + LogisticsProviderID, Html.Term("Routes", "Routes"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
            </ul>
		</div>	
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("LogisticProviderDetail", "Logistic Provider Detail")%></h2>		
	</div>
   

    <table id="frmProviderDetails" class="FormTable Section" width="100%">
		 <% List<LogisticsProviderSearData> details = ViewData["details"] as List<LogisticsProviderSearData>;
          
           string Name = "";
           string LogisticsProviderID = "";
           string PhoneNumber = "";
           string FaxNumber = "";
           string EmailAddress = "";
           string TermName = "";
           string Description = "";
           bool Active = false;
           string ExternalCode = "";
           bool WorkInSaturdays = false;
           bool WorkInSundays = false;
           bool WorkInHolidays = false;
           string ExternalTrakingURL = "";
           
           if (details.Count > 0)
           {
               LogisticsProviderID = details[0].LogisticsProviderID.ToString();
               Name = details[0].Name.ToString();
               PhoneNumber = details[0].PhoneNumber.ToString();
               FaxNumber = details[0].FaxNumber.ToString();
               EmailAddress = details[0].EmailAddress.ToString();
               TermName = details[0].TermName.ToString();
               Description = details[0].Description.ToString();
               Active = Convert.ToBoolean(details[0].Active);
               ExternalCode = details[0].ExternalCode.ToString();
               WorkInSaturdays = Convert.ToBoolean(details[0].WorkInSaturdays);
               WorkInSundays = Convert.ToBoolean(details[0].WorkInSundays);
               WorkInHolidays = Convert.ToBoolean(details[0].WorkInHolidays);
               ExternalTrakingURL = details[0].ExternalTrakingURL.ToString();
           }
           else {
               Name = "";
               LogisticsProviderID = "";
               PhoneNumber = "";
               FaxNumber = "";
               EmailAddress = "";
               TermName = "";
               Description = "";
               Active = false;
               ExternalCode = "";
               WorkInSaturdays = false;
               WorkInSundays = false;
               WorkInHolidays = false;
               ExternalTrakingURL = "";
           }
            %>
        <tr>
			<td class="FLabel">
				<%= Html.Term("Name")%> :
			</td>
			<td>
				    <input id="LogisticsProviderID" type="hidden" value="<%=LogisticsProviderID %>" />
                    <input id="Name" type="text" value="<%=Name %>" class="required" name="<%= Html.JavascriptTerm("ProvName", "Logistic provider name can not be null.") %>" />
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("Phone Number")%> :
			</td>
        
			<td>
				  <input id="txtPhoneNumber" value="<%=PhoneNumber %>" type="text" class="required phone"  maxlength="10" name="<%= Html.JavascriptTerm("ProvPhoneNumber", "Logistic provider Phone Number can not be null.") %>" />
			</td>
		</tr>        
         <tr>
			<td class="FLabel">
				<%= Html.Term("Fax Number")%> :
			</td>
			<td>
				  <input id="FaxNumber" type="text" value="<%=FaxNumber %>" maxlength="9"/>
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("Email Address")%> :
			</td>
			<td>
				  <input id="EmailAddress" type="text" value="<%=EmailAddress %>" name="<%=Html.JavascriptTerm("ProvEmail", "TermName can not be null.") %>" />
			<!-- pattern="[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*@[a-zA-Z0-9_]+([.][a-zA-Z0-9_]+)*[.][a-zA-Z]{1,5}"-->
            </td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("TermName")%> :
			</td>
			<td>
				  <input id="TermName" type="text" value="<%=TermName %>" class="required" name="<%= Html.JavascriptTerm("ProvEmail", "TermName can not be null.") %>"/>
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("Description")%> :
			</td>
			<td>
				 <input id="Description" type="text" value="<%=Description %>" />
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("Active")%> :
			</td>
			<td>
				 <input id="Active" type="checkbox"  <%=Active ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("External Code")%> :
			</td>
			<td>
				 <input id="ExternalCode" type="text"   value="<%=ExternalCode %>"/>
			</td>
		</tr>        
        <tr>
			<td class="FLabel">
				<%= Html.Term("WorkSaturdays")%> :
			</td>
			<td>
				 <input id="WorkInSaturdays" type="checkbox"  <%=WorkInSaturdays ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
         <tr>
			<td class="FLabel">
				<%= Html.Term("WorkSundays")%> :
			</td>
			<td>
				  <input id="WorkInSundays" type="checkbox" <%=WorkInSundays ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("WorkHolidays")%> :
			</td>
			<td>
				<input id="WorkInHolidays" type="checkbox"  <%=WorkInHolidays ? "checked=\"checked\"" : "" %> />
			</td>
		</tr>
        <tr>
			<td class="FLabel">
				<%= Html.Term("External Traking URL")%> :
			</td>
			<td>
				<input id="ExternalTrakingURL" type="text" value="<%=ExternalTrakingURL %>" name="url" />
			</td>
		</tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                        <%= Html.Term("Save", "Save")%></a>
                </p>
            </td>
        </tr>
	</table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">
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
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server"> 
<script type="text/javascript">
    $(function () {
        $('#ExternalTrakingURL').watermark('<%= Html.JavascriptTerm("formatUrl", "https://www.pagina.com") %>');

        $('#EmailAddress').keyup(function () {
            var $th = $(this);
            if ((validar_email($("#EmailAddress").val()) || $("#EmailAddress").val() == '')) {
                $('#EmailAddress').clearError();
            }
            else {
                $('#EmailAddress').showError('<%=Html.JavascriptTerm("ProvEmailVal", "Email Invalid.") %>');
                return false;
            }
        });
        $('#ExternalTrakingURL').keyup(function () {
            var $th = $(this);
            if ((isValidUrl($th.val()) || $("#ExternalTrakingURL").val() == '')) {
                $('#ExternalTrakingURL').clearError();
            }
            else {
                $('#ExternalTrakingURL').showError('<%=Html.JavascriptTerm("ProvUrlVal", "Url Invalid.") %>');
                return false;
            }
        });

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
                    window.location = '<%= ResolveUrl("~/Logistics/Logistics/ProviderDetails/") %>' + $('#LogisticsProviderID').val();
                } else {
                    showMessage(response.message, true);
                }
            });
        });

        $('#txtPhoneNumber').keypress(function (e) {
            key = e.keyCode ? e.keyCode : e.which
            // backspace
            if (key == 8) return true
            //37 and 40 Teclas direccion
            if (key > 36 && key < 41) return true
            // 0-9
            if (key > 47 && key < 58) {
                if (field.value == "") return true
                regexp = /.[0-9]{2}$/
                return !(regexp.test(field.value))
            }
            // .
            if (key == 46) {
                if (field.value == "") return false
                regexp = /^[0-9]+$/
                return regexp.test(field.value)
            }
            return false
        });
        $('#FaxNumber').keypress(function (e) {
            key = e.keyCode ? e.keyCode : e.which
            if (key == 8) return true
            if (key > 36 && key < 41) return true
            if (key > 47 && key < 58) {
                if (field.value == "") return true
                regexp = /.[0-9]{2}$/
                return !(regexp.test(field.value))
            }
            if (key == 46) {
                if (field.value == "") return false
                regexp = /^[0-9]+$/
                return regexp.test(field.value)
            }
            return false
        });
        //Guardar
        $('#btnSave').click(function () {
            // inicia validacion   
            if ($('#frmProviderDetails').checkRequiredFields()) {
                //
                if ((validar_email($("#EmailAddress").val()) || $("#EmailAddress").val() == '')) {
                    $('#EmailAddress').clearError();
                    //alert("Email valido");
                    if ((isValidUrl($('#ExternalTrakingURL').val()) || $("#ExternalTrakingURL").val() == '')) {
                        //
                        $('#ExternalTrakingURL').clearError();
                        var Details = {
                            LogisticsProviderID: $('#LogisticsProviderID').val(),
                            Name: $('#Name').val(),
                            PhoneNumber: $('#txtPhoneNumber').val(),
                            FaxNumber: $('#FaxNumber').val(),
                            EmailAddress: $('#EmailAddress').val(),
                            TermName: $('#TermName').val(),
                            Description: $('#Description').val(),
                            Active: $('#Active').is(":checked"),
                            MarketID: '20',
                            ExternalCode: $('#ExternalCode').val(),
                            WorkInSaturdays: $('#WorkInSaturdays').is(":checked"),
                            WorkInSundays: $('#WorkInSundays').is(":checked"),
                            WorkInHolidays: $('#WorkInHolidays').is(":checked"),
                            ExternalTrakingURL: $('#ExternalTrakingURL').val()
                        }, t = $(this);
                        $.post('/Logistics/Logistics/SaveDetails', Details, function (response) {
                            if (response.result) {
                                showMessage("Logistic Provider Saved!", false);
                                window.location = '<%= ResolveUrl("~/Logistics/Logistics/ProviderDetails/") %>' + response.logisticsProviderID;
                            } else {
                                showMessage(response.message, true);
                            }
                        });
                        //
                    } else {
                        $('#ExternalTrakingURL').showError('<%=Html.JavascriptTerm("ProvUrlVal", "Url Invalid.") %>');
                        result = false;
                    }

                } else {
                    //alert("El email no es valido");
                    $('#EmailAddress').showError('<%=Html.JavascriptTerm("ProvEmailVal", "Email Invalid.") %>');
                    result = false;
                }

            }
            //fin

        });
        //

        function validar_email(valor) {
            // creamos nuestra regla con expresiones regulares.
            var filter = /[\w-\.]{3,}@([\w-]{2,}\.)*([\w-]{2,}\.)[\w-]{2,4}/;
            // utilizamos test para comprobar si el parametro valor cumple la regla
            if (filter.test(valor))
                return true;
            else
                return false;
        }

        function isValidUrl(url) {

            var myVariable = url;
            if (/^(http|https|ftp):\/\/[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$/i.test(myVariable)) {
                return true;
            } else {
                return false;
            }
        }

        //

    });
    </script>

</asp:Content>

               
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
  <a href="<%= ResolveUrl("~/Logistics") %>">
		    <%= Html.Term("Logistics") %></a> >
        <%= Html.Term("LogisticProviderDetail", "Logistic Provider Detail")%>
</asp:Content>
