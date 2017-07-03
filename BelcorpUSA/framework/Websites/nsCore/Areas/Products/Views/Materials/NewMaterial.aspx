<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/MaterialManagement.master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>



<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="../../../../Content/CSS/Validation.css" rel="stylesheet" type="text/css" />

    <script src="../../../../Scripts/jquery.number.min.js" type="text/javascript"></script>

    <script src="../../../../Scripts/jquery.numeric.js" type="text/javascript"></script>
    <script src="../../../../Scripts/Validaciones.js" type="text/javascript"></script>
 <%--<%= Url.Content("~/Scripts/Validaciones.js")%>--%>
 
<%-- <%= Url.Content("~/Scripts/jquery.numeric.js")%>--%>
  
  <%--  <style type="text/css">
        .style2
        {
            height: 30px;
        }
        
        .css_textoDecimal {
        text-align: right;
        }
    </style>--%>
    <script language="javascript" type="text/javascript">
    $('<img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." />');


    $(document).ready(function () {



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

            $("#txtMaterialCode").fn_util_validarNumeros();
            $("#txtEanCode").fn_util_validarNumeros();
            $("#txtWeigt").fn_util_validaDecimal(2);
            $("#txtVolume").fn_util_validaDecimal(2);
            $("#TxtMeasurement").val("UN");

            $('#btnSave').click(function () {

                RegisterMaterial();

            });
        });
        ///***********************************************************************
        //* ***  Registrar Material
        //***********************************************************************/

        function RegisterMaterial() {
            
            if ($('#newMaterial').checkRequiredFields()) {

                var enMaterial = new Object();

                var txtMaterialCode = $("#txtMaterialCode").val();
                var txtName = $("#txtName").val();
                var TxtMeasurement = $("#TxtMeasurement").val();
                var TxtBPCS = $("#TxtBPCS").val();
                var txtVolume = $("#txtVolume").val();
//                var txtBrand = $("#txtBrand").val();
                var txtGroup = $("#txtGroup").val();
//                var txtNCM = $("#txtNCM").val();
//                var txtOrigin = $("#txtOrigin").val();
//                var txtMarket = $("#txtMarket").val();
                var txtEanCode = $("#txtEanCode").val();
//                var txtEanCode = $("#txtEanCode").val();
                var txtWeigt = $("#txtWeigt").val();
                var ddlBrand = $("#ddlBrand").val();

                enMaterial.Name = txtName;
                enMaterial.SKU = txtMaterialCode;
                enMaterial.Active = true;
                enMaterial.EANCode = txtEanCode;
                enMaterial.BPCSCode = TxtBPCS;
                enMaterial.UnityType = TxtMeasurement;
                enMaterial.Weight = txtWeigt;
                enMaterial.Volume = txtVolume;
//                enMaterial.NCM = txtNCM;
//                enMaterial.Origin = txtOrigin;
                enMaterial.OriginCountry = 0;
//                enMaterial.Brand = txtBrand;
                enMaterial.Group = txtGroup;
//                enMaterial.MarketID = txtMarket;
                enMaterial.iTransaccion = 0;
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


        function validarEAN(Obj) {
            var objEnPuntoCobranza = { oenMaterial: Obj };
            var sParams = JSON.stringify(objEnPuntoCobranza);
            var i = Object();
            $.ajax({
                type: "POST",
                url: '/Materials/ValidateEAN',
                data: sParams,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: "false",
                success: function (result) {

                    i = result;
                },
                error: function (resultado) {
                }
            });

            return i;
        }
    </script>
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">

            <a href="<%= ResolveUrl("~/Products") %>">
            <%= Html.Term("Products") %></a> > 

            <a href="<%= ResolveUrl("~/Products/Materials") %>">
            <%= Html.Term("BrowseMaterial", "Browse Material")%></a> > 
            
            <%--<a href="<%= ResolveUrl("~/Products/Materials") %>">
            <%= Html.Term("Materialedit", "Material edit")%></a> >--%>


  <%--  <%= Model.CatalogID == 0 ? Html.Term("NewCatalog", "New Catalog") : Html.Term("EditCatalog", "Edit Catalog") %>--%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("NewMaterial", "New Material")%></h2>
 </div>
<table id="newMaterial" class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("Material Code")%>:
			</td>
			<td>
				<input id="txtMaterialCode" type="text" class="required" name="Code Material is required."  maxlength="9" />
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
				<input id="txtEanCode" type="text"  style="width: 400px;" />
			</td>
		</tr>
    	<tr>
			<td class="style2">
				<%= Html.Term("UnitMeasurement", "Unit Measurement")%>:
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
            <input id="txtWeigt" type="text"  class="required" name="Weigt is required." style="width: 100px;"   monedaidioma='CultureIPN'/>
            </td>
        </tr>
        <tr>
              <td class="FLabel">
                  <%= Html.Term("Volume")%> <label><strong>Cm3</strong></label>: 
            </td>
            <td>
                  <input id="txtVolume" type="text"  class="required" name="Volume is required." style="width: 100px;"   monedaidioma='CultureIPN'/>
            </td>
        </tr>

      <%--  <tr>
            <td class="FLabel">
                 <%= Html.Term("Brand", "Brand")%>  
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

       <%-- <tr>
            <td class="FLabel">
                <%= Html.Term("NCM")%>  
            </td>
            <td>
                 <input id="txtNCM" type="text"  style="width: 200px;" />
        </td>--%>
       <%-- </tr>
            <tr>
                <td class="FLabel">
                     <%= Html.Term("Origin")%>  
                </td>
                <td>
                     <input id="txtOrigin" type="text"  style="width: 100px;" maxlength="2"/>
                </td>
         </tr>--%>


            <%--<tr>
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

</asp:Content>