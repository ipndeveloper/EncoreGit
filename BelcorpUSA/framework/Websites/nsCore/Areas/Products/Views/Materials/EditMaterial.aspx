<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/MaterialManagement.master"Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<%--Inherits="System.Web.Mvc.ViewPage<dynamic>" --%>

<asp:Content ID="Content4" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../../Content/CSS/Validation.css" rel="stylesheet" type="text/css" />

    <script src="../../../../Scripts/jquery.number.min.js" type="text/javascript"></script>

    <script src="../../../../Scripts/jquery.numeric.js" type="text/javascript"></script>
    <script src="../../../../Scripts/Validaciones.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">

  <% var V = Model[0]; %>
    <%--<%var account = Model.ConsultantOrCustomerAccountID.HasValue ? NetSteps.Data.Entities.Account.LoadSlim(Model.ConsultantOrCustomerAccountID.Value) : new AccountSlimSearchData(); %>--%>
  <script type="text/javascript">
      $(function () {



          $('input[monedaidioma=CultureIPN]').keyup(function (event) {

              var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
              // var value = parseFloat($(this).val());


              var formatDecimal = '$1.$2'; // valores por defaul 
              var formatMiles = ",";  // valores por defaul

              if (cultureInfo === 'en-US') {
                   formatDecimal = '$1.$2';
                   formatMiles = ",";
              }
              else if (cultureInfo === 'es-US') {
                   formatDecimal = '$1,$2';
                   formatMiles = ".";
              }
              else if (cultureInfo === 'pt-BR') {
                  formatDecimal = '$1,$2';
                  formatMiles = ".";
              }


              //            if (!isNaN(value)) {
              if (event.which >= 37 && event.which <= 40) {
                  event.preventDefault();
              }

              $(this).val(function (index, value) {


                  return value.replace(/\D/g, "")
                                 .replace(/([0-9])([0-9]{2})$/, formatDecimal)
                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, formatMiles);
              });

              //            }

          });

          InicializarCampos();

          $('#btnToggleStatus').click(function () {
              var t = $(this);
              showLoading(t);
              $.post('<%= ResolveUrl(string.Format("~/Products/Materials/ToggleStatus/{0}", V.MaterialID)) %>', {}, function (response) {
                  hideLoading(t);
                  if (response.result) {
                      t.toggleClass('ToggleInactive');
                  } else {
                      showMessage(response.message);
                  }
              })
                .fail(function () {
                    hideLoading(t);
                    showMessage('@Html.Term("ErrorProcessingRequest", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
                });
          });
            $('#btnSave').click(function () {

                RegisterMaterial();

            });
      });


//        /***********************************************************************
//        * Setear campo Deshabilitado
//        **********************************************************************/
//        function fn_util_seteaObjetoDeshabilitado(sIdInput) {
//            $(sIdInput).addClass('css_input_deshabilitado');
//            $(sIdInput).attr("disabled", true)
//        }

        function InicializarCampos() {

                    $("#txtMaterialCode").fn_util_validarNumeros();

                    $("#hfId").val('<%= V.MaterialID %>');
                    $("#txtEanCode").fn_util_validarNumeros();
//                    $("#txtWeigt").fn_util_validaDecimal(2);
//                    $("#txtVolume").fn_util_validaDecimal(2);
                    fn_util_seteaObjetoDeshabilitado("#txtMaterialCode");
                    $("#txtMaterialCode").val('<%= V.SKU %>');
                    $("#txtName").val('<%= V.Name %>');
                    $("#TxtMeasurement").val('<%= V.UnityType %>');
                    $("#TxtBPCS").val('<%= V.BPCSCode %>');
                    $("#txtVolume").val('<%= V.Volume.ToString("N",CoreContext.CurrentCultureInfo) %>');
                    $("#ddlBrand").val('<%= V.BrandID %>');
                    $("#txtGroup").val('<%= V.Group %>');
                    $("#txtEanCode").val('<%= V.EANCode %>');
                    $("#txtWeigt").val('<%= V.Weight.ToString("N",CoreContext.CurrentCultureInfo) %>');

                }


                function RegisterMaterial() {

                    if ($('#newMaterial').checkRequiredFields()) {

                        var enMaterial = new Object();

                        var txtMaterialCode = $("#txtMaterialCode").val();
                        var txtName = $("#txtName").val();
                        var TxtMeasurement = $("#TxtMeasurement").val();
                        var TxtBPCS = $("#TxtBPCS").val();
                        var txtVolume = $("#txtVolume").val();
//                        var txtBrand = $("#txtBrand").val();
                        var txtGroup = $("#txtGroup").val();
                        var txtEanCode = $("#txtEanCode").val();
                        var txtWeigt = $("#txtWeigt").val();
                        var ddlBrand = $("#ddlBrand").val();

                        enMaterial.Name = txtName;
                        enMaterial.SKU = txtMaterialCode;

                        var activeVal = true;

                        if ($('#btnToggleStatus').prop('class') == 'Toggle ToggleActive ToggleInactive')
                            activeVal = false;

                        enMaterial.Active = activeVal;
                        enMaterial.EANCode = txtEanCode;
                        enMaterial.BPCSCode = TxtBPCS;
                        enMaterial.UnityType = TxtMeasurement;
                        enMaterial.Weight = txtWeigt;
                        enMaterial.Volume = txtVolume;
                        enMaterial.OriginCountry = 0;
                        enMaterial.Group = txtGroup;
                        enMaterial.iTransaccion = 1;
                        enMaterial.MaterialID = $("#hfId").val();

                        enMaterial.BrandID = ddlBrand;                       
                       
                        var objEnPuntoCobranza = { oenMaterial: enMaterial };
                        var sParams = JSON.stringify(enMaterial);
                        var t = $(this);
                        showLoading(t);
                        
                        $.ajax({
                            type: "POST",
                            url: '/Materials/RegisterMaterials',
                            data: sParams,
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            async: "false",
                            success: function (response) {
                                hideLoading(t);

                                var h = response;
                                if (response.result) {
                                    showMessage('Material saved!', false);
                                } else {
                                    showMessage(response.message, true);
                                }

                            },
                            error: function (resultado) {



                            }
                        });
                    }

                }
	</script>
   
        <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > 

        <a href="<%= ResolveUrl("~/Products/Materials") %>">
        <%= Html.Term("BrowseMaterial")%></a> > 
        <%= Html.Term("Details", "Details")%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">

     <div class="TagInfo">
            <div class="Content">
                <div class="SubTab">
                    <% var V = Model[0]; %>
     
                            <a>  <%= V.Name%></a>
            
                </div>
                <%--<b>Product Display Name</b>--%>
                <table class="DetailsTag Section" width="100%">
                    <tr>
                        <td class="Label">
                            <%= Html.Term("SKU", "SKU") %>:
                        </td>
                        <td>
                         <a> <%= V.SKU%></a>
                        </td>
                    </tr>
                    <tr>
                        <td class="Label">
                            <%= Html.Term("Status", "Status") %>:
                        </td>
                        <td>
                           <a id="btnToggleStatus" href="javascript:void(0);" class="Toggle ToggleActive<%= !V.Active ? " ToggleInactive" : "" %>">
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
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Materialedit", "Material edit")%></h2>
		<%--<%= Html.Term("BrowsePlans", "Browse Plans")%> | <a href="<%= ResolveUrl("~/Products/Materials/NewMaterial") %>"><%= Html.Term("Materialedit", "Material edit")%></a>--%>
       
	</div>
    <table id="newMaterial" class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("Material Code")%>:
			</td>
			<td>
				<input id="txtMaterialCode" type="text" class="required" name="Code Material is required."  maxlength="10"/>
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				<%= Html.Term("Name") %>:
			</td>
			<td>
				<input id="txtName" type="text" class="required" name="Material Name is required." style="width: 400px;" />
			</td>
		</tr>
			<tr>
			<td class="FLabel">
				<%= Html.Term("EAN Code")%>:
			</td>
			<td>
				<input id="txtEanCode" type="text" class="required" name="Material Name is required." style="width: 100px;" />
			</td>
		</tr>
    	<tr>
			<td class="style2">
				<%= Html.Term("Measurement Unit")%>:
			</td>
			<td class="style2">
				<input id="TxtMeasurement" type="text"  style="width: 100px;" maxlength="2" />
                &nbsp; 
                  <%= Html.Term("BPCS")%>:

                  <input id="TxtBPCS" type="text" class="required" name="Material Name is required." style="width: 100px;" />
			</td>
       
		</tr>
        <tr>
            <td class="FLabel">
            <%= Html.Term("Weigt")%> : 
            </td>
            <td>
            <input id="txtWeigt" type="text"  style="width: 100px;"  monedaidioma='CultureIPN'/>
            </td>
        </tr>
        <tr>
              <td class="FLabel">
                  <%= Html.Term("Volume")%> <label><strong>Cm3</strong></label>: 
            </td>
            <td>
                  <input id="txtVolume" type="text"  style="width: 100px;"  monedaidioma='CultureIPN'/>
            </td>
        </tr>

      <%--  <tr>
            <td class="FLabel">
                 <%= Html.Term("Brand")%>  
            </td>
            <td>
                 <input id="txtBrand" type="text"  style="width: 100px;" />
            </td>
        </tr>--%>

         <!-- FSV -GYS -->
         <tr>
            <td class="FLabel">
                <%= Html.Term("Brand", "Brand")%>:
            </td>
            <td>
                <select id="ddlBrand">
                    <% foreach (var items in ViewData["BrandsID"] as List<BrandSP>)
                          {
                        %>
                            <option value="<%=items.BrandID %>"><%=items.Name%></option>
                        <%                                       
                          }                   
                    %>
                </select>
             </td>
         </tr>
        <!--end -->

        <tr>
            <td class="FLabel">
                <%= Html.Term("Group")%>  
            </td>
            <td>
                 <input id="txtGroup" type="text"  style="width: 100px;" />
            </td>
        </tr>

   <%--     <tr>
            <td class="FLabel">
                <%= Html.Term("NCM")%>  
            </td>
            <td>
                 <input id="txtNCM" type="text"  style="width: 200px;" />
            </td>
        </tr>--%>

      <%--      <tr>
                <td class="FLabel">
                     <%= Html.Term("Origin")%>  
                </td>
                <td>
                     <input id="txtOrigin" type="text"  style="width: 100px;" maxlength="2"/>
                </td>
            </tr>--%>


           <%-- <tr>
                <td class="FLabel">
                     <%= Html.Term("Market")%>  
                </td>
                <td>
                        <input id="txtMarket" type="text"  style="width: 100px;" />
                </td>
            </tr>--%>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                        <%= Html.Term("SaveMaterial", "Save Material")%></a>
                </p>
            </td>
        </tr>
	</table>

    <input  type="hidden"  id="hfId" />
</asp:Content>
