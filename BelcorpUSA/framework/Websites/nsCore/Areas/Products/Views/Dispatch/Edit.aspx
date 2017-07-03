<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Dispatch/Dispatch.Master" 
Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Products.Models.DispatchModel>" %>
<asp:Content ID="Content0" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
    <script type="text/javascript">


        $(function () {
            // --- Declaracion de variables
            var productsDispatchs = new Array();
            var EndPeriods = 0;
            var fecIni;
            var fecFin;
            var listDispatch = 0;
            var fechaHoy = new Date().toLocaleString()
            var vProductID = 0;
            var vQuantity2 = 0;
            var vQuantity = 0;
            var vTipo = 0;

            function onSuccess(result) {
                if (result.result) {
                    showMessage('<%= Html.Term("SaveRestriction", "Restriction Successfully Saved!") %>');
                    window.location = '<%= ResolveUrl("~/Products/Dispatch/Save") %>/' + result.id;
                }
                else {
                    showMessage(result.message, true);
                }
            }

            $("#conditionSingleProductQty2").ready(function () {

                var watermark = '2 Owner';

                //init, set watermark text and class
                $("#conditionSingleProductQty2").val(watermark).addClass('watermark');

                //if blur and no value inside, set watermark text and class again.
                $("#conditionSingleProductQty2").blur(function () {
                    if ($(this).val().length == 0) {
                        $(this).val(watermark).addClass('watermark');
                    }
                });

                //if focus and text is watermrk, set it to empty and remove the watermark class
                $("#conditionSingleProductQty2").focus(function () {
                    if ($(this).val() == watermark) {
                        $(this).val('').removeClass('watermark');
                    }
                });
            })


            $("#TypeDispatch").ready(function () {
                //$("#conditionSingleProductQty2").hide();
            })

            $('#sTypeDispatch').change(function () {
                var strCadena = $("#TypeDispatch option:selected").text();
                var strtypeDispatch = strCadena.indexOf("Owner");
                if (strtypeDispatch == -1) {
                    $("#conditionSingleProductQty2").hide();
                }
                else {
                    var tama = 60;
                    $("#conditionSingleProductQty2").width(tama);
                    $("#conditionSingleProductQty2").show();
                }
            })

            $('#btnSave').click(function () { 
                vTipo = $("#sTypeDispatch").val();
                sDispatch = $("#sTypeDispatch");
                dEnd = $("#endDate");
                var sPeriod = $("#StartPeriods").val();
                var EPeriod = $("#EndPeriods").val();
                var sPeriods = $("#StartPeriods");
                txtName = $("#nameDispatch");
                vTipo = $("#sTypeDispatch").val();
                txtDispatch = $("#nameDispatch");

                if ($("#nameDispatch").val() == "") {
                    txtName.showError('<%= Html.JavascriptTerm("NameDispatchEmpty", "Name Dispatch is Empty") %>');
                    return false;
                }
                txtName.clearError();

                if ($("#StartPeriods").val() == "0") {
                    sPeriods.showError('<%= Html.JavascriptTerm("Start period is Empty", "Start period is Empty") %>');
                    return false;
                }
                sPeriods.clearError();

                if (fecIni > fecFin) {
                    dEnd.showError('<%= Html.JavascriptTerm("Selecc Period Less", "Select date less") %>');
                    return false;
                }
                dEnd.clearError();

                if (vTipo == 0) {
                    sDispatch.showError('<%= Html.JavascriptTerm("SelectDispatch", "Select Dispatch Type") %>');
                    return false;
                }

                sDispatch.clearError();
                if (EPeriod != 0) {

                    if (parseInt(sPeriod) > parseInt(EPeriod)) {
                        sPeriods.showError('<%= Html.JavascriptTerm("Selecc Period Less", "Select Period less") %>');
                        return false;
                    }
                }
                sPeriods.clearError();


                if ($("#startDate").val() == "StartDate") {
                    fecIni = null;
                }
                else {
                    fecIni = $("#startDate").val();
                }

                if ($("#endDate").val() == "EndDate") {
                    fecFin = null;
                }
                else {
                    fecFin = $("#endDate").val();
                }
                if ($('#conditionSingleGrid >tbody >tr').length == 0) {
                    showMessage("Registrar Items", true);
                    return false;
                }
                if ($("#EndPeriods").val() == "0") {
                    EndPeriods = 0;
                }
                else {
                    EndPeriods = $("#EndPeriods").val();
                }

                if ($("#ListDispatch").val() == 0) {
                    ListScope = 0;
                }
                else {
                    ListScope = $("#ListDispatch").val();
                }
                // ------- 
                if (vTipo == "3") {
                    $("#conditionSingleGrid tbody tr").each(function (index) {
                        var campo1, campo2, campo3, campo4, campo5, campo6;
                        $(this).children("td").each(function (index2) {
                            switch (index2) {
                                case 0:
                                    campo1 = $(".productIds", this).val();
                                    break;
                                case 1:
                                    campo2 = $(this).text();
                                    break;
                                case 2:
                                    campo3 = $(this).text();
                                    break;
                                case 3:
                                    campo4 = $(this).text();
                                    break;
                                case 4:
                                    campo5 = $(this).text();
                                    break;
                                case 5:
                                    campo6 = $(this).text();
                                    break;
                            }
                        })
                        productsDispatchs.push({ DispatchID: 0, ProductID: parseInt(campo1), Quantity: campo4, Quantity2: campo5, SKU: "" });
                    })
                } else {
                    $("#conditionSingleGrid tbody tr").each(function (index) {
                        var campo1, campo2, campo3, campo4, campo5, campo6;
                        $(this).children("td").each(function (index2) {
                            switch (index2) {
                                case 0:
                                    campo1 = $(".productIds", this).val();
                                    break;
                                case 1:
                                    campo2 = $(this).text();
                                    break;
                                case 2:
                                    campo3 = $(this).text();
                                    break;
                                case 3:
                                    campo4 = $(this).text();
                                    break;
                                case 4:
                                    campo6 = $(this).text();
                                    break;
                            }
                        });
                        productsDispatchs.push({ DispatchID: 0, ProductID: parseInt(campo1), Quantity: campo4, Quantity2: 0, SKU: "" });
                    })
                }
                // -------
                var dispatchTID = $("#sTypeDispatch").val();
                var statusId = $("#slStatus").val();
                var data = {
                    "DispatchID": $("#DispatchID").val()
                            , "DispatchTypeID": dispatchTID
                            , "DispatchStatusType": 1
                            , "Description": $("#nameDispatch").val()
                            , "PeriodStart": $("#StartPeriods").val()
                            , "PeriodEnd": EndPeriods
                            , "DateStart": fecIni
                            , "DateEnd": fecFin
                            , "OnlyTime": 1
                            , "ListScope": ListScope
                            , "Status": statusId
                            , "Termname": "2300"
                            , "SortIndex": 1
                            , "Editable": true
                            , "Products": productsDispatchs
                };

                var url = '<%= ResolveUrl("~/Products/Dispatch/Save") %>';

                $.ajax({
                    url: url,
                    data: JSON.stringify(data),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function (result) {
                        if (result.result == true) {
                            $('#btnSave').hide();
                            showMessage("Dispatch Save Sucefully", false);
                            location.href = '<%= ResolveUrl("~/Products/Dispatch") %>';
                        }
                        else {
                            showMessage("Dispatch !! Error !! Save", true);
                        }
                    }
                });
            });

            // Look-up product ini
            $('#txtConditionSingle').change(function () {
                $('#conditionSingleProductID').val("");
            });

            $('#txtConditionSingle').removeClass('Filter').after($('#conditionSingleProductID')).css('width', '275px')
                .val('')
				.watermark('<%= Html.JavascriptTerm("ProductSearch", "Look up product by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Products/Promotions/Search") %>', { onSelect: function (item) {
				    $('#conditionSingleProductID').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});
            var TypeDispatchIDs = 0;
            $('#sTypeDispatch').change(function () {
                TypeDispatchIDs = $('#sTypeDispatch').val();
                if (TypeDispatchIDs == "3") {
                    $('#conditionSingleProductQty2').show();
                }
                else {
                    $('#conditionSingleProductQty2').hide();
                }
            });
            $('#conditionSingleProductAdd').click(function () {
                TypeDispatchIDs = $('#sTypeDispatch').val();
                if ($("#conditionSingleProductQty").val() < 1) {
                    showMessage("Quantity must be higher than zero.", true);
                    return;
                }
                var productId = $('#conditionSingleProductID').val();
                var quantity = $('#conditionSingleProductQty').val();
                var quantity2 = $('#conditionSingleProductQty2').val();
                if ($('#conditionSingleProductQty2').val() == "" || $('#conditionSingleProductQty2').val() == "2 Owner") {
                    quantity2 = 0;
                }
                if (productId) {
                    getProductInfo(productId, $(this), function (result) {
                        if (TypeDispatchIDs == 3) {
                            $('.ProductIDSel').show();
                            $('#conditionSingleGrid').append('<tr>'
						            + '<td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>'
                                    + '<input type="hidden" id="productId" class="productIds" name="Product is required" value="' + productId + '" /></td>'
						            + '<td>' + result.product.SKU + '</td>'
						            + '<td>' + result.product.Name + '</td>'
						            + '<td>' + quantity + '</td>'
                                    + '<td class="ProductIDSel" >' + quantity2 + '</td>'
                                    + '<td style="display: none;>' + result.product.ProductID + '</td>'
						            + '</tr>');
                        }
                        else {
                            $('.ProductIDSel').hide();
                            $('#conditionSingleGrid').append('<tr>'
						            + '<td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>'
                                    + '<input type="hidden" id="productId" class="productIds" name="Product is required" value="' + productId + '" /></td>'
						            + '<td>' + result.product.SKU + '</td>'
						            + '<td>' + result.product.Name + '</td>'
						            + '<td>' + quantity + '</td>'
                                    + '<td class="ProductIDSel" >' + quantity2 + '</td>'
                                    + '<td style="display: none;>' + result.product.ProductID + '</td>'
						            + '</tr>');
                        }
                        $('#txtConditionSingle, #conditionSingleProductID').val('');
                        //$('#conditionSingleProductQty').val('1');
                        $('.qty').numeric({ negative: false });
                        $('#conditionSingleGrid').show();
                    });
                }
            });

            function getProductInfo(productId, showLoading, success) {
                var options = {
                    url: '<%= ResolveUrl("~/Products/ProductPromotions/QuickAddProduct") %>',
                    showLoading: showLoading,
                    data: { productId: productId },
                    success: success
                };
                NS.post(options);
            }

            $('#conditionSingleGrid .BtnDelete').live('click', function () {
                $(this).closest('tr').remove();
                $('#singleItemQuickAdd').show();
                if ($('#conditionSingleGrid >tbody >tr').length == 0) {
                }
            });

            $('#btnCancel').click(function () {
                window.location.replace('<%= ResolveUrl("~/Products/Dispatch") %>');
            });

        });
    
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <div class="SectionHeader">
        <h2><%= Html.Term("EditDispatch", "Edit Dispatch")%></h2>
        <a href="<%= ResolveUrl("~/Products/Dispatch") %>"><%= Html.Term("BrowseDispatch", "Browse Dispatchs") %></a> |
        <%= Html.Term("EditDispatch", "Edit Dispatch") %> |
        <a href="<%= ResolveUrl("~/Products/Dispatch/Create") %>"><%= Html.Term("CreateaNewDispatch", "Create a New Dispatch")%></a>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% var displayCantidad2 = 0; %>

    <div>
    <table class="FormTable" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("NameDispatch", "Name Dispatch") %>:
            </td>
            <td colspan="3">
                <input id="DispatchID" type="hidden" class="TextInput" style="width: 50.182em;" value="<%= Model.DispatchID %>" />
                <input id="txtQuantityItems" type="hidden" class="TextInput" style="width: 50.182em;" value="<%=Model.ProductsQuery.Count()%>" />
                
                <input id="nameDispatch" type="text" class="required pad5 fullWidth" style="width: 50.182em;" value="<%=Model.Description%>" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("StartPeriod", "Start Period Dispatch") %>:
            </td>
            <td width="200px">
                <select id="StartPeriods">
                    <option value="0">Select Period</option>
                    <%
                        Dictionary<string, string> startperiod = Periods.GetAllPeriods();
                        {
                            System.DateTime moment = DateTime.Today;
                            int valYear = Convert.ToInt32(string.Concat(moment.Year, "00"));
                            foreach (var startpair in startperiod.Where(n => Convert.ToInt32(n.Key) > valYear))
                            { %>
                    <option value="<%=startpair.Key%>">
                        <%=startpair.Key%></option>
                    <%
                                }
                            }
                    %>
                </select>
            </td>
            <td class="FLabel">
                <%= Html.Term("EndPeriod", "End Period Dispatch") %>:
            </td>
            <td>
                <select id="EndPeriods">
                    <option value="0">Select Period</option>
                    <%
                        Dictionary<string, string> endperiod = Periods.GetAllPeriods();
                        {
                            System.DateTime moment = DateTime.Today;
                            int valYear = Convert.ToInt32(string.Concat(moment.Year, "00"));
                            foreach (var endpair in endperiod.Where(n => Convert.ToInt32(n.Key) > valYear))
                            { %>
                    <option value="<%=endpair.Key%>">
                        <%=endpair.Key%></option>
                    <%
                                }
                            }
                    %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("StartDate", "Start Date Dispatch") %>:
            </td>
            <td width="200px">
                <input type="text" class="TextInput DatePicker StartDate" value="StartDate" id="startDate" />
            </td>
            <td class="FLabel">
                <%= Html.Term("EndDate", "End Date Dispatch") %>:
            </td>
            <td>
                <input type="text" class="TextInput DatePicker EndDate" value="EndDate" id="endDate" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Type", "Type Dispatch") %>:
            </td>
            <td>
                <%= @Html.DropDownList("sTypeDispatch", (TempData["sDispatch"] as IEnumerable<SelectListItem>))%>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("ListDispatch", "List Dispatch") %>:
            </td>
            <td>
                <select id="ListDispatch">
                    <option value="0">Select List Dispatch</option>
                    <%
                        var listaDispatchDisplay = Dispatch.listdispatchDisplay();
                        foreach (var dispDispatch in listaDispatchDisplay)
                        { %>
                    <option value="<%=dispDispatch.DispatchListID%>">
                        <%=dispDispatch.Name%></option>
                    <%
                                }
                    %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Status", "Status Dispatch") %>:
            </td>
            <td>
                <select id="slStatus">
                    <option value="1">Enable</option>
                    <option value="2">Disabled</option>
                </select>
            </td>
        </tr>
    </table>
    </div>
    <hr />
    <br />
    <table>
    <tr>
        <tr> 
        <td class="FLabel"> 
        <%: Html.Term("AppyToProduct", "Apply to Product")%>
        </td>
        </tr> 
            <td class="FLabel">   
            <%= Html.Term("Dispatchs_ProductLookUpLabel", "Product look-up")%>:
            </td>
            <td> 
              <input type="hidden" value="" id="conditionSingleProductID" class="Filter" />
              <input type="text" value="" size="30" class="pad5 mr10 txtQuickAdd required" name="Product is required"
              hiddenid='conditionSingleProductID' id="txtConditionSingle" />
              <input type="text" value="1" class="pad5 qty center" id="conditionSingleProductQty" />
              <input type="text" value="0" class="pad5 qty center" id="conditionSingleProductQty2" />
              <a class="DTL Add" href="javascript:void(0);" id="conditionSingleProductAdd"> <%= Html.Term("Promotions_QuickAdd", "Add")%></a>
            </td> 
        </tr>
        <tr> 
        <td>
        </td>
        <td>
        <div>
        <br />
           <table width="100%" class="DataGrid" id="conditionSingleGrid">
                    <thead>
                    <%if (Model.DispatchTypeID == 3)
                      {
                      %>
                        <tr class="GridColHead">
                            <th class="GridCheckBox">
                            </th>
                            <th>
                                <%=Html.Term("SKU")%>
                            </th>
                            <th>
                                <%=Html.Term("Product")%>
                            </th>
                            <th>
                                <%=Html.Term("Quantity")%>
                            </th>
                            <th  class = "ProductIDSel" >
                                <%=Html.Term("QTD2")%>
                            </th> 
                        </tr>
                        <%}  
                      else { %>
                      <tr class="GridColHead">
                            <th class="GridCheckBox">
                            </th>
                            <th>
                                <%=Html.Term("SKU")%>
                            </th>
                            <th>
                                <%=Html.Term("Product")%>
                            </th>
                            <th>
                                <%=Html.Term("Quantity")%>
                            </th> 
                            <th  class = "ProductIDSel"  style="display: none;">
                                <%=Html.Term("QTD2")%>
                            </th>  
                        </tr>
                     <% }%>
                    </thead>
                    <tbody>
                        <%if (Model.DispatchTypeID == 3)
                          {
                              foreach (var productSel in Model.ProductsQuery)
                              {
                                  if (productSel.Quantity == 0) { productSel.Quantity = 1; }
                         %>
                         <tr>
                         <td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>
                         <input type="hidden" id="productId" class="productIds" name="Product is required" value="<%=productSel.ProductID%>" /></td>
                            <td>
                               <%=productSel.SKU%>
                            </td>
                            <td>
                               <% =productSel.Product%>
                            </td>
                            <td>
                               <% =productSel.Quantity%>
                            </td>
                            <td> 
                                <% =productSel.Quantity2%> 
                            </td> 
                          </tr>
                         <%        
                              }
                          }
                          else
                          { %>
                        <%
                            foreach (var productSel in Model.ProductsQuery)
                              {
                                  if (productSel.Quantity == 0) { productSel.Quantity = 1; }
                         %>
                         <tr>
                         <td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>
                         <input type="hidden" id="productId" class="productIds" name="Product is required" value="<%=productSel.ProductID%>" /></td>
                            <td>
                               <%=productSel.SKU%>
                            </td>
                            <td>
                               <% =productSel.Product%>
                            </td>
                            <td>
                               <% =productSel.Quantity%>
                            </td> 
                          </tr>
                         <%        
                              }
                            %>
                        <%}
                        %>
                        
                    </tbody>
                    
          </table> 
          <table>
          <tr>
                            <td class="FLabel">
                            </td>
                            <td>
                                <p>
                                    <a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue"">
                                        <%= Html.Term("Save", "Save") %></a> 
                                    <a href="javascript:void(0);" id="btnCancel" style="display: inline-block;" class="Button BigWhite">
                                        <%= Html.Term("Cancel", "Cancel") %></a>
                                </p>
                            </td>
                        </tr>
          </table>
            
        </div>
        </td>
        </tr> 
   </table>
   <script type="text/javascript">

       $(document).ready(function () {
           $("#StartPeriods").val('<%=Model.PeriodStart %>')
           $("#EndPeriods").val('<%=Model.PeriodEnd %>')  
           $("#sTypeDispatch").val('<%=Model.DispatchTypeID %>')
           $("#ListDispatch").val('<%=Model.ListScope %>')
           $("#slStatus").val('<%=Model.Status %>')
           var typeDispatchId = $("#sTypeDispatch").val();
            
           if (typeDispatchId == '3') {
               $("#conditionSingleProductQty2").show();
           } else {
               $("#conditionSingleProductQty2").hide(); 
           }

       });
   </script>
</asp:Content>
