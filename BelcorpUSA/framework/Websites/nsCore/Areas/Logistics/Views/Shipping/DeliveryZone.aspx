<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Logistics/Views/Shared/Shipping.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.ShippingRulesLogisticsSearchData>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
     <%  List<ZonesData> AreaGeographical = ViewData["AreaGeographical"] as List<ZonesData>;
            List<GeoScopesByCodesSearchData> PostalCodes = ViewData["PostalCodes"] as List<GeoScopesByCodesSearchData>; 
            string countArea = AreaGeographical.Count.ToString();
            string CountPostal = PostalCodes.Count.ToString();             
     %>
    <style type="text/css">
        .none
        {
            display: none;
        }
    </style>
    <script type="text/javascript">
        $(function () {
            

            if(<%=countArea %> > <%=CountPostal %>){
                $("#rdAreaGeographical").prop("checked", true);
                ShowAreaGeographical();
            }if(<%=countArea %> < <%=CountPostal %>){
                $("#rdPostalCodeRanges").prop("checked", true);
                ShowPostalcode();
            }
            if(<%=countArea %> == <%=CountPostal %>){
                $("#rdAreaGeographical").prop("checked", true);
                ShowAreaGeographical();
            }

            $('#paginatedGridOptionsAreaGeographical').hide();
            $('#paginatedGridOptionsPostalCodeRanges').hide();
            
            if ($('#sState').val() != null) {
            $('#sState').clearError();
            }
            $('#sState').change(function () {
                //var val = this.val();
                if ($('#sState').val() != null) {
                    $('#sState').clearError();
                }
            });

            $('#rdAreaGeographical').click(function () {
                ShowAreaGeographical();
            });
            $('#rdPostalCodeRanges').click(function () {
                ShowPostalcode();
            });

            function ShowPostalcode(){
                $('#divAreaGeographical').hide();
                $('#divPostalCodeRanges').show();
            }
            function ShowAreaGeographical(){
                $('#divPostalCodeRanges').hide();
                $('#divAreaGeographical').show();
            }
            $('#btnToggleStatus').click(function () {
                var t = $(this);
                showLoading(t);
                $.post('<%= ResolveUrl(string.Format("~/Logistics/Shipping/ToggleStatus/{0}", Model.ShippingOrderTypeID)) %>', {}, function (response) {
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

            $('#sState').change(function () {
                $('#sCity').prop('selectedIndex', 0);
                var t = $(this);
                var state = $("#sState").val();
                showLoading(t);
                $.get('<%= ResolveUrl(string.Format("~/Logistics/Routes/GetCitys")) %>', { state: state }, function (response) {
                    if (response.result) {
                        hideLoading(t);
                        $('#sCity').children('option:not(:first)').remove();
                        if (response.Cities) {
                            for (var i = 0; i < response.Cities.length; i++) {
                                $('#sCity').append('<option value="' + response.Cities[i].City + '">' + response.Cities[i].City + '</option>');
                            }
                        }
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });

            $('#mainAddressZip').keyup(function (e) {
                valPostalCodeFrom();
            });
            $('#mainAddressZipPlusFour').keyup(function (e) {
                valPostalCodeFrom();
            });
            
            function valPostalCodeFrom(){
                var valZipPlusFour = $('#mainAddressZipPlusFour').val();
                var valZip = $('#mainAddressZip').val();
                var postalCode = valZip + '' + valZipPlusFour;
                var cant = postalCode.length;

                if (cant == 8) {
                    $('#zipLoadingfrom').show();
                    $('#mainAddressZipPlusFour').clearError();
                    $('#mainAddressZip').clearError();
                    $('#SpanErrorFrom').hide();
                    $('#checkfrom').hide();

                    $.get('<%= ResolveUrl(string.Format("~/Logistics/Shipping/SearchPostalCode")) %>', { postalCode: postalCode }, function (response) {
                        if (response.result) {
                            if (response.postalcode) {
                                var exispostalcode = response.postalcode[0];
                                if (exispostalcode != null) {
                                    $('#mainAddressZipPlusFour').clearError();
                                    $('#mainAddressZip').clearError();
                                    $('#SpanErrorFrom').hide();
                                    $('#zipLoadingfrom').hide();
                                    $('#checkfrom').show();
                                }
                                else {
                                    $('#mainAddressZipPlusFour').showError('');
                                    $('#mainAddressZip').showError('');
                                    $('#checkfrom').hide();
                                    $('#zipLoadingfrom').hide();
                                    $('#SpanErrorFrom').show();
                                }

                            }
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
                else {
                    $('#mainAddressZipPlusFour').showError('');
                    $('#mainAddressZip').showError('');
                    $('#checkfrom').hide();
                    $('#zipLoadingfrom').hide();
                    $('#SpanErrorFrom').show();
                }
            
            }
            $('#mainAddressZip2').keyup(function (e) {
                valPostalCodeTo();
            });

            $('#mainAddressZipPlusFour2').keyup(function (e) {
                valPostalCodeTo();
            });

            function valPostalCodeTo(){
                var valZipPlusFour = $('#mainAddressZipPlusFour2').val();
                var valZip = $('#mainAddressZip2').val();
                var postalCode = valZip + '' + valZipPlusFour;
                var cant = postalCode.length;

                if (cant == 8) {
                    $('#zipLoadingTo').show();
                    $('#mainAddressZipPlusFour2').clearError();
                    $('#mainAddressZip2').clearError();
                    $('#SpanErrorTo').hide();
                    $('#checkTo').hide();

                    $.get('<%= ResolveUrl(string.Format("~/Logistics/Shipping/SearchPostalCode")) %>', { postalCode: postalCode }, function (response) {
                        if (response.result) {
                            if (response.postalcode) {
                                var exispostalcode = response.postalcode[0];
                                if (exispostalcode != null) {
                                    $('#mainAddressZipPlusFour2').clearError();
                                    $('#mainAddressZip2').clearError();
                                    $('#SpanErrorTo').hide();
                                    $('#zipLoadingTo').hide();
                                    $('#checkTo').show();
                                }
                                else {
                                    $('#mainAddressZipPlusFour2').showError('');
                                    $('#mainAddressZip2').showError('');
                                    $('#checkTo').hide();
                                    $('#zipLoadingTo').hide();
                                    $('#SpanErrorTo').show();
                                }

                            }
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
                else {
                    $('#mainAddressZipPlusFour2').showError('');
                    $('#mainAddressZip2').showError('');
                    $('#checkTo').hide();
                    $('#zipLoadingTo').hide();
                    $('#SpanErrorTo').show();
                }            
            }

            $('#btnSaveZone').click(function () {
                if ($('#newRule').checkRequiredFields()) {
                    var data = {shippingOrderTypeID: <%=Model.ShippingOrderTypeID %>};
                    var areasDel = $('#areasdelete').val();
                    var areasDelete = areasDel.split(','); 
                        for(var i = 0; i < areasDelete.length; i++)
                        {
                           data['areaGeographicalDel[' + i + '].GeoScopeID'] = areasDelete[i];
                        } 

                    $('#paginatedGridAreaGeographical tbody:first tr').each(function (i) {
                        data['AreasGeographical[' + i + '].ShippingOrderTypeID'] = <%=Model.ShippingOrderTypeID %>;
                        data['AreasGeographical[' + i + '].GeoScopeID'] = $(this).find("td").find("input[name='GeoScopesCodeID']").val();
                        data['AreasGeographical[' + i + '].Name'] =  $.trim($(this).find("td").eq(1).html().toString());
                        data['AreasGeographical[' + i + '].Value'] = $.trim($(this).find("td").eq(2).html().toString());
                        data['AreasGeographical[' + i + '].Except'] = $(this).find("td").eq(3).html();
                    });

                    $.post('<%= ResolveUrl(string.Format("~/Logistics/Shipping/SaveAreaGeographical/")) %>', data, function (response) {
                        if (response.result) {
                            showMessage('<%= Html.JavascriptTerm("msgSavePostal", "Delivery Zones Save.") %>', false);
                            $('#areasdelete').val("0");   
                            window.location = '<%= ResolveUrl("~/Logistics/Shipping/DeliveryZone/") %>' + <%=Model.ShippingOrderTypeID %>                           
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }
            });

            $('#tgDeleteSelected').click(function (e) {
                try {                    
                    var areasdeletes = new Array();
                    $('input[name="GeoScopesCodeID"]:checked').each(function () {
                        areasdeletes.push($('#areasdelete').val()+','+$(this).val());
                    });                                     
                    $('#areasdelete').val(areasdeletes.toString());
                    
                    var table = document.getElementById('paginatedGridAreaGeographical');
                    var rowCount = table.rows.length;

                    for (var i = 0; i < rowCount; i++) {
                        var row = table.rows[i];
                        var chkbox = row.cells[0].childNodes[0];
                        if (null != chkbox && true == chkbox.checked) {
                            table.deleteRow(i);
                            rowCount--;
                            i--;
                        }
                    }
                } catch (e) {
                    alert(e);
                }
            });
            $('#tgDeleteSelectedPostalCode').click(function (e) {
                try {
                    var checkboxValues = new Array();
                    $('input[name="ShippingOrderTypesGeoScopesByCodeID"]:checked').each(function () {
                        checkboxValues.push($('#postaldelete').val()+','+$(this).val());
                    });                                     
                    $('#postaldelete').val(checkboxValues.toString());

                    var table = document.getElementById('paginatedGridPostalCodeRanges');
                    var rowCount = table.rows.length;
                    for (var i = 0; i < rowCount; i++) {
                        var row = table.rows[i];
                        var chkbox = row.cells[0].childNodes[0];
                        if (null != chkbox && true == chkbox.checked) {
                            table.deleteRow(i);
                            rowCount--;
                            i--;
                        }
                    }
                } catch (e) {
                    alert(e);
                }
            });

            ///Add Postal Code
            $('#btnAddPostalCode').click(function () {
                if ($('#newPostalCode').checkRequiredFields()) {
                    if ($('#checkfrom').is(':hidden') || $('#checkTo').is(':hidden') )
                    { 
                     return false;
                    }

                    var PostalCodeFrom = $("#mainAddressZip").val() + '' + $("#mainAddressZipPlusFour").val();
                    var PostalCodeTo = $("#mainAddressZip2").val() + '' + $("#mainAddressZipPlusFour2").val();
                    var ExceptPostalCode = $('#checkExceptPostalCode').is(":checked");

                    row = $('<tr class="Alt hover"></tr>');
                    check = $('<td>' + '<input type="checkbox" >' + '</td>');
                    PostalCodeFrom = $('<td>' + PostalCodeFrom + '</td>');
                    PostalCodeTo = $('<td>' + PostalCodeTo + '</td>');
                    ExceptPostalCode = $('<td>' + ExceptPostalCode + '</td>');
                    row.append(check, PostalCodeFrom, PostalCodeTo, ExceptPostalCode).prependTo("#paginatedGridPostalCodeRanges tbody");
                    $("#NotItemsPostalCodes").remove();
                }
            });
            //End Postal Code

            $('#addNewZoneModel').jqm({ modal: false, onShow: function (h) {
                h.w.css({
                    top: Math.floor(parseInt($(window).height() / 10)) - Math.floor(parseInt(h.w.height() / 5)) + 'px',
                    left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                }).fadeIn();
            }
            });

            $('#tgAdd').click(function () {
                $('#addNewZoneModel').jqmShow();
            });

            /// Add Zone
            $('#AddZone').click(function (e) {
                if ($('#newZone').checkRequiredFields()) {
                    if (!$('#sState').val() && $('#sState').is(':visible')) {
                        $('#sState').showError('<%= Html.JavascriptTerm("valStateProvince", "Value to States Provinces not selected.") %>');
                        errorCount++;
                    }
                    var state = $("#sState option:selected").html();
                    var city = $("#sCity option:selected").html();
                    var col3 = $('#checkExcept').is(":checked");
                    var scopeLevelID;

                    if ($("#sState option:selected").val() != "" && $("#sCity option:selected").val() != "") {
                        scopeLevelID = 2;
                    }
                    if ($("#sState option:selected").val() != "" && $("#sCity option:selected").val() == "") {
                        scopeLevelID = 1;
                    }
                    if (scopeLevelID == 1 || scopeLevelID == 2) {
                        $.get('<%= ResolveUrl(string.Format("~/CTE/PaymentsMethodsConfiguration/GetScopeLevels/")) %>', { scopeLevelID: scopeLevelID }, function (response) {
                            if (response.result) {
                                if (response.scopeLevels) {
                                    row = $('<tr class="Alt hover"></tr>');
                                    check = $('<td>' + '<input type="checkbox" >' + '</td>');
                                    colState = $('<td>' + response.scopeLevels[0].Name + '</td>');
                                    if (scopeLevelID == 1) colCity = $('<td>' + state + '</td>');
                                    if (scopeLevelID == 2) colCity = $('<td>' + city + '</td>');
                                    colExcept = $('<td>' + col3 + '</td>');
                                    row.append(check, colState, colCity, colExcept).prependTo("#paginatedGridAreaGeographical tbody");

                                    //Close Modal                                
                                    $('#addNewZoneModel').jqmHide();
                                    $('#sState').val('');
                                    $('#sCity').val('');
                                    $('#checkExcept').attr('checked', false);
                                    $("#NotItems").remove();
                                }
                            } else {
                                showMessage(response.message, true);
                            }
                        });
                    }
                }
            });
            ///End Add Zone
            
            $('#btnSavePostalCode').click(function () {
                if ($('#newRule').checkRequiredFields()) {                
                var data={shippingOrderTypeID: <%=Model.ShippingOrderTypeID %> }; 
                    var postalcodesDel = $('#postaldelete').val();
                        // Removes all of the commas within your string
                    var postalcodesDelete = postalcodesDel.split(','); 
                        // Iterate through each value
                        for(var i = 0; i < postalcodesDelete.length; i++)
                        {
                           data['postalcodesdel[' + i + '].ShippingOrderTypesGeoScopesByCodeID'] = postalcodesDelete[i];
                        }
                       
                    $('#paginatedGridPostalCodeRanges tbody:first tr').each(function (i) {
                        data['postalcodes[' + i + '].ShippingOrderTypeID'] = <%=Model.ShippingOrderTypeID %>;
                        data['postalcodes[' + i + '].ShippingOrderTypesGeoScopesByCodeID'] = $(this).find("td").find("input[name='ShippingOrderTypesGeoScopesByCodeID']").val();
                        data['postalcodes[' + i + '].ValueFrom'] = $(this).find("td").eq(1).html();
                        data['postalcodes[' + i + '].ValueTo'] = $(this).find("td").eq(2).html();
                        data['postalcodes[' + i + '].Except'] = $(this).find("td").eq(3).html();
                    });
                    
                    $.post('<%= ResolveUrl(string.Format("~/Logistics/Shipping/SavePostalCodes/")) %>', data, function (response) {
                        if (response.result) {
                            showMessage('<%= Html.JavascriptTerm("msgSavePostal", "Delivery Zones Save.") %>', false);
                            $('#postaldelete').val("0");   
                            window.location = '<%= ResolveUrl("~/Logistics/Shipping/DeliveryZone/") %>' + <%=Model.ShippingOrderTypeID %> 
                        } else {
                            showMessage(response.message, true);
                        }
                    });
                }

            });
            ///
            $('#exportToExcelPostalCodes').click(function () {
                var shippingOrderTypeID = <%=Model.ShippingOrderTypeID %>;

                var url = '<%= ResolveUrl("~/Logistics/Shipping/ExportPostalCodes") %>';

                    var parameters = {
                        shippingOrderTypeID: shippingOrderTypeID
                    };

                    url = url + '?' + $.param(parameters).toString();

                    $("#frmExportar").attr("src", url);
            
            });
            ///
            $('#exportToExcel').click(function () {
                var shippingOrderTypeID = <%=Model.ShippingOrderTypeID %>;

                var url = '<%= ResolveUrl("~/Logistics/Shipping/ExportAreasgeographic") %>';

                    var parameters = {
                        shippingOrderTypeID: shippingOrderTypeID
                    };

                    url = url + '?' + $.param(parameters).toString();

                    $("#frmExportar").attr("src", url);
            
            });
            ///


        });

    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ShippingDeliveryZones", "Shipping Delivery Zones")%>
        </h2>
        <input type="radio" id="rdAreaGeographical" name='thing' value='valuable' />
        <label for="rdAreaGeographical">
            <%: Html.Term("AreaGeographical", "Area Geographical")%></label>
        <input type="radio" id="rdPostalCodeRanges" name='thing' value='valuable' />
        <label for="rdPostalCodeRanges">
            <%: Html.Term("PostalCodeRanges", "Postal Code Ranges")%></label>
    </div>
     <% Html.RenderPartial("DeliveryZonesBrowse/AreaGeographicalBrowse"); %>
     <% Html.RenderPartial("DeliveryZonesBrowse/PostalCodeRangesBrowse"); %>
            
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Logistics") %>">
        <%= Html.Term("Logistics")%></a> > <a href="<%= ResolveUrl("~/Logistics/Shipping/Rules") %>">
            <%= Html.Term("ShippingRules", "Shipping Rules")%></a> > <a href="<%= ResolveUrl("~/Logistics/Shipping/RuleDetail/"+(Model.ShippingOrderTypeID == 0 ? "" : Model.ShippingOrderTypeID.ToString())) %>">
                <%= Html.Term("ShippingRuleDetail", "Rule Detail")%></a> >
    <%= Html.Term("ShippingDeliveryZones", "Shipping Delivery Zones")%>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="YellowWidget" runat="server">
    <div class="TagInfo" <%= Model.ShippingOrderTypeID == 0 ? "style='display:none'" : "" %>>
        <div class="Content">
            <div class="SubTab">
            </div>
            <table class="DetailsTag Section" width="100%">
                <tr>
                    <td class="Label">
                        <%= Html.Term("Name") %>:
                    </td>
                    <td>
                        <label>
                            <%= Model.ShippingRuleName %></label>
                    </td>
                </tr>
                <tr>
                    <td class="Label">
                        <%= Html.Term("Code") %>:
                    </td>
                    <td>
                        <label>
                            <%= Model.LogisticProviderID %></label>
                    </td>
                </tr>
                <tr>
                    <td class="Label">
                        <%= Html.Term("Status", "Status") %>:
                    </td>
                    <td>
                        <a id="btnToggleStatus" href="javascript:void(0);" class="Toggle <%= Model.Status ?  "ToggleActive" : "ToggleInactive" %>">
                        </a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
    <div class="SectionNav">
        <ul class="SectionLinks">
            <%= Html.SelectedLink("~/Logistics/Shipping/RuleDetail/" + (Model.ShippingOrderTypeID == 0 ? "" : Model.ShippingOrderTypeID.ToString()), Html.Term("ShippingRuleDetail", "Rule Detail"), LinkSelectionType.ActualPage, CoreContext.CurrentUser, "")%>
            <%= Html.SelectedLink("~/Logistics/Shipping/DeliveryZone/" + (Model.ShippingOrderTypeID == 0 ? "" : Model.ShippingOrderTypeID.ToString()), Html.Term("ShippingRuleDetailDeliveryZones", "Delivery Zones"), LinkSelectionType.ActualPage, CoreContext.CurrentUser, "")%>
        </ul>
    </div>
</asp:Content>
