<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl< NetSteps.Data.Entities.Business.SponsorshipSearchData>" %>
 <% List<SponsorshipSearchData> cantsdoc = ViewData["spGetRulesPerDocuments"] as List<SponsorshipSearchData>;
    string cantsdocu = string.Empty;
    if (cantsdoc != null) { cantsdocu = "1"; } else { cantsdocu = "0"; }
    %>
<script type="text/javascript">

    $(function () {
       

        if (<%=cantsdocu %> > 0) {
            $('#validDocY').attr('checked', 'checked').trigger('change');
        } else {
            $('#validDocN').attr('checked', 'checked').trigger('change');
        }

        $('#ValidDocSelection a.checkAllOptions').toggle(function () {
            $(this).text('<%=Html.Term("PromotionOptions_UncheckAllLink", "uncheck all")%>');
            $(this).closest('div').find(':checkbox').attr('checked', 'checked');
        }, function () {
            $(this).text('<%=Html.Term("PromotionOptions_CheckAllLink", "check all")%>');
            $(this).closest('div').find(':checkbox').removeAttr('checked');
        });

        $('.ord').keypress(function (e) {
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
    });
</script>
<div class="pad5 promotionOption couponCode">
	<div class="FL optionHelpIcons">
    </div>
    <div class="FLabel">
		<label for="chkCouponCodeRequired">
			<%=Html.Term("ValidDocumetsOptions_RequireValidDocumetsOption", "Valid Documents?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="documentos">
		<span>
			<input type="radio" value="no" name="validDocRestrict" id="validDocN" />
			<label for="acctRestrictN">
				<%=Html.Term("ValidDocumetsOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="validDocRestrict" id="validDocY" />
			<label for="acctRestrictY">
				<%=Html.Term("ValidDocumetsOptions_YesLabel", "Yes")%></label>
		</span>
	
		<div class="UI-lightBg hiddenPanel pad10 overflow" id="ValidDocSelection">
        </span><a class="FR checkAllOptions" href="javascript:void();">
				<%=Html.Term("PromotionOptions_CheckAllLink", "check all")%></a> <span class="clr">
			</span>
                    <table  width="100%" id="tbDoc">
                        <tbody>
                         <% foreach (var item in ViewData["Sponsorsxxx"] as List<SponsorshipSearchData>)
                            {                                
                                var isTypeChecked = string.Empty;
                                if (item.RestrictionDocumentID != 0)
                                {
                                    isTypeChecked = Convert.ToBoolean(item.RestrictionDocumentID) ? "checked=\"checked\"" : string.Empty;                                    
                                }
                                                                                                         
                         %>                            
                             <tr>
                                <td>
                                     <input type="checkbox" value="<%=item.IDTypeID %>" id="req" class="validDoc" <%= isTypeChecked %>  />
                                    <label><%=item.Name %></label> 
                                </td>
                                
                            </tr>
                              <%
                                                                         
                             } %>
                        </tbody>                       
                    </table>				
		</div>
	</div>
</div>