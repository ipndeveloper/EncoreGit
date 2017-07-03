<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Web.Mvc.Controls.Models.AddressModel>" %>
<%@ Import Namespace="System.Linq" %>
<%@ Import Namespace="NetSteps.Addresses.Common.Models" %>
<%@ Import Namespace="NetSteps.Common.Reflection" %>
<%@ Import Namespace="NetSteps.Web.Mvc.Controls.Extensions" %>
<% 
    IAddress address = Model.Address ?? new Address();
    var EnvironmentCountry = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]);

    //Country country = Model.Country ?? SmallCollectionCache.Instance.Countries.First(c => c.CountryID == (address.CountryID > 0 ? address.CountryID : EnvironmentCountry)); 
    Country country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == EnvironmentCountry); 
    
%>
<div class="FormContainer">
    <% 
        if (AddressConfiguration.ConfigurationExists)
        {
            ISO iso = AddressConfiguration.GetISO(country.CountryCode) ?? AddressConfiguration.GetISO("BR");

            if (iso != default(ISO))
            {
                string containerId = Model.Prefix + (string.IsNullOrEmpty(Model.Prefix) ? "addressControl" : "AddressControl");
                containerId = containerId.Replace("AddressAddress", "Address");
                string countrySelectId = Model.Prefix + (string.IsNullOrEmpty(Model.Prefix) ? "country" : "Country");

                if (Model.ShowCountrySelect)
                {
    %>
    <script type="text/javascript">
		//<![CDATA[
        $(function () {
            var changeCountryUrl = '<%= Model.ChangeCountryURL.ResolveUrl() %>', excludeFields = eval('<%= Model.ExcludeFields == null ? "undefined" : Model.ExcludeFields.ToJSON() %>');
            $('#<%= countrySelectId %>').change(function () {
                var data = { countryId: $('#<%= countrySelectId %>').val(), addressId: '<%= address.AddressID %>', prefix: '<%= Model.Prefix %>', showCountrySelect: '<%= !Model.ShowCountrySelect %>' };
                if (excludeFields && excludeFields.length) {
                    for (var i = 0; i < excludeFields.length; i++) {
                        data['excludeFields[' + i + ']'] = excludeFields[i];
                    }
                }
                $.get(changeCountryUrl, data, function (response) {
                    if (response.result === undefined || response.result) {
                        $('#<%= containerId %>').html(response);
                        var phone = $('#<%= containerId %> .PhoneNumber');
                        if (phone.length && phone.is('td,span,div,p') && phone.phone) {
                            phone.phone();
                        }
                    } else {
                        showMessage(response.message, true);
                    }
                });
            });
        });
		//]]>
    </script>
    <div id="<%= countrySelectId %>Container" class="FRow">
        <div class="FLabel">
            <label for="<%= countrySelectId %>">
                <%= Model.LabelOverrides != null && Model.LabelOverrides.ContainsKey("Country")?
				Model.LabelOverrides["Country"] : Html.Term("Country")%>:</label></div>
        <div class="FInput">
            <select id="<%= countrySelectId %>" class="Country">
                <%  
                  //int? addressId = Model.Address.AddressID;
                  //int? countryId = Model.Address.CountryID;
                  //foreach (Country allowedCountry in Model.Countries.Where(c => (addressId == 0 && c.CountryID == EnvironmentCountry) || (addressId != 0 && EnvironmentCountry == countryId && c.CountryID == EnvironmentCountry) || (addressId != 0 && EnvironmentCountry != countryId && c.Active)))
                    foreach (Country allowedCountry in Model.Countries.Where(c => c.CountryID == EnvironmentCountry)) 
                    {
                %>
                <option data-countrycode="<%= allowedCountry.CountryCode %>" value="<%= allowedCountry.CountryID %>"
                    <%= allowedCountry.CountryID == country.CountryID ? "selected=\"selected\"" : "" %>>
                    <%= allowedCountry.GetTerm()%>
                </option>
                <%
                    } 
                %>
            </select>
        </div>
        <br />
    </div>
    <% 
                }
                else
                { 
    %>
    <input type="hidden" id="<%= countrySelectId %>" value="<%= country.CountryID.ToString() %>"
        name="<%= countrySelectId %>" />
    <% 
                } 
    %>
    <div id="<%= containerId %>">
        <% 
                var keyupElements = iso.Tags.Where(t => !string.IsNullOrEmpty(t.FocusElementOnFilled) || !string.IsNullOrEmpty(t.Regex)).ToList();
                var postalCodeField = iso.PostalCodeLookup.PostalCodeFieldName;
                var Size = iso.PostalCodeLookup.Size;
                var SizeSearch = iso.PostalCodeLookup.SizeSearch;
                if (keyupElements.Any() || iso.PostalCodeLookup.Enabled)
                { 
        %>
        <script type="text/javascript">
			//<![CDATA[
			$(function () {
				var ignoreKeys = [8, 9, 13, 16, 17, 18, 19, 20, 27, 33, 34, 35, 36, 37, 38, 39, 40, 45, 46, 91, 92, 144];
				<%
				foreach(Tag element in keyupElements)
				{
				%>
					$('#<%= element.Id %>').keyup(function(e){
						var val = $(this).val();
					<% 
					if(!string.IsNullOrEmpty(element.Regex))
					{
						if(element.LiveRegexCheck)
						{ 
						%>
							if(!new RegExp(/<%= element.Regex %>/).test(val)){
								$(this).val(val.substr(0, val.length - 1));
							}
						<% 
						} 
						else
						{ 
						%>
							$(this).clearError();
							if(!new RegExp(/<%= element.Regex %>/).test(val) && val.length == $(this).attr('maxlength')) {
								$(this).showError('<%= Html.JavascriptTerm(element.RegexFailMessageTermName, element.DefaultRegexFailMessage) %>');
							}
						<%
						}
					}
					if(!String.IsNullOrEmpty(element.FocusElementOnFilled))
					{ 
					%>
						if(!ignoreKeys.contains(e.keyCode) && val.length == $(this).attr('maxlength') && !$(this).data('hasError')){
							$('#<%= element.FocusElementOnFilled %>').focus();
						}
					<%
					}
					%>
					});
				<%
				}
				if(iso.PostalCodeLookup.Enabled)
				{ 
				%>
				    var zipXHR, postalCodeControl = $('#<%= containerId %> .<%= postalCodeField %>'), city = "<%= address.City.ToPascalCase() %>", cityControl = $('#<%= containerId %> .City'), state = '<%= address.StateProvinceID.HasValue ? address.StateProvinceID.Value : 0 %>', stateControl = $('#<%= containerId %> .State'), county = '<%= address.County %>', countyControl = $('#<%= containerId %> .County'), street = '<%= address.ProfileID %>', streetControl = $('#<%= containerId %> .Street');
                    var postalCodeRegExp = /<%= iso.PostalCodeLookup.Regex %>/;
                    var lastZip;

                    function clearCityCountyStateControls() {
                        //cityControl.add(countyControl).add(stateControl).add(streetControl).html('<option value=\"\">-- <%= Html.Term("PleaseEnteraValidZip", "Please enter a valid zip")%> --</option>');
                        $("#" + '<%= Model.Prefix %>' + "State").val("");
                        $("#" + '<%= Model.Prefix %>' + "State").attr('readonly', true);
                        $("#" + '<%= Model.Prefix %>' + "State").css('background-color' , '#DEDEDE');

                        $("#" + '<%= Model.Prefix %>' + "City").val("");
                        $("#" + '<%= Model.Prefix %>' + "City").attr('readonly', true);
                        $("#" + '<%= Model.Prefix %>' + "City").css('background-color' , '#DEDEDE');

                        $("#" + '<%= Model.Prefix %>' + "County").val("");
                        $("#" + '<%= Model.Prefix %>' + "County").attr('readonly', true);
                        $("#" + '<%= Model.Prefix %>' + "County").css('background-color' , '#DEDEDE');

                        $("#" + '<%= Model.Prefix %>' + "Street").val("");
                        $("#" + '<%= Model.Prefix %>' + "Street").attr('readonly', true);
                        $("#" + '<%= Model.Prefix %>' + "Street").css('background-color' , '#DEDEDE');

                         $("#" + '<%= Model.Prefix %>' + "Address1").val("");
                         $("#" + '<%= Model.Prefix %>' + "Address2").val("");
                         $("#" + '<%= Model.Prefix %>' + "Address3").val("");
                    }
				
                    function postalCodeLookup(zip) {
                                console.log("Zip ingresado: ",zip);
                                //GR-4602 se agrega el valor addressId para que tome valor correcto de dirección
                                var addrID=<%=address.AddressID %> 
								postalCodeControl.clearError();
								if (!zipXHR) {

									$('#<%= containerId %> .zipLoading').show();
									cityControl.add(countyControl).add(stateControl).add(streetControl).empty();
                                    //GR-4602 se agrega el valor addressId para que tome valor correcto de dirección
									zipXHR = $.getJSON('<%= iso.PostalCodeLookup.LookupURL.ResolveUrl() %>', { countryId: $('#<%= countrySelectId %>').length ? $('#<%= countrySelectId %>').val() : '<%= Model.ShowCountrySelect ? "" : country.CountryID.ToString() %>', zip: zip, addressId: addrID }, function(results) {
										zipXHR = undefined;
                                        lastZip = zip;
										$('#<%= containerId %> .zipLoading').hide();
										if (!results.length) {
											if(showMessage && results.message) {
												showMessage(results.message, true);
											}
                                            clearCityCountyStateControls();
										} else {
                                            
                                            console.log("Resultado get: ",results);

                                                $("#" + '<%= Model.Prefix %>' + "State").val("");
                                                $("#" + '<%= Model.Prefix %>' + "State").attr('readonly', true);
                                                $("#" + '<%= Model.Prefix %>' + "State").css('background-color' , '#DEDEDE');

                                                $("#" + '<%= Model.Prefix %>' + "City").val("");
                                                $("#" + '<%= Model.Prefix %>' + "City").attr('readonly', true);
                                                $("#" + '<%= Model.Prefix %>' + "City").css('background-color' , '#DEDEDE');

                                                $("#" + '<%= Model.Prefix %>' + "County").val("");
                                                $("#" + '<%= Model.Prefix %>' + "County").attr('readonly', true);
                                                $("#" + '<%= Model.Prefix %>' + "County").css('background-color' , '#DEDEDE');

                                                $("#" + '<%= Model.Prefix %>' + "Street").val("");
                                                $("#" + '<%= Model.Prefix %>' + "Street").attr('readonly', true);
                                                $("#" + '<%= Model.Prefix %>' + "Street").css('background-color' , '#DEDEDE');

                                               

											for (var i = 0; i < results.length; i++) {

                                            
                                                if (!stateControl.contains(results[i].state.trim())) {
													//stateControl.append('<option' + (state && results[i].stateId == state ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].stateId + '\">' + results[i].state.trim() + '</option>');
                                                    $("#" + '<%= Model.Prefix %>' + "State").attr('readonly', true);
                                                    $("#" + '<%= Model.Prefix %>' + "State").css('background-color' , '#DEDEDE');
                                                    $("#" + '<%= Model.Prefix %>' + "State").val(results[i].state.trim());
												}
												if (!cityControl.contains(results[i].city.trim())) {
													//cityControl.append('<option' + (city && results[i].city == city ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].city + '\">' + results[i].city.trim() + '</option>');
                                                    $("#" + '<%= Model.Prefix %>' + "City").attr('readonly', true);
                                                    $("#" + '<%= Model.Prefix %>' + "City").css('background-color' , '#DEDEDE');
                                                    $("#" + '<%= Model.Prefix %>' + "City").val(results[i].city.trim());
												}
												//if (!countyControl.contains(results[i].county.trim())) {
                                                if (!results[i].EditaCounty) {
													//countyControl.append('<option' + (county && results[i].county == county ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].county + '\">' + results[i].county.trim() + '</option>');
                                                    $("#" + '<%= Model.Prefix %>' + "County").attr('readonly', true);
                                                    $("#" + '<%= Model.Prefix %>' + "County").css('background-color' , '#DEDEDE');
                                                    $("#" + '<%= Model.Prefix %>' + "County").val(results[i].county.trim());
												} 
                                                else
                                                {
                                                    $("#" + '<%= Model.Prefix %>' + "County").attr('readonly', false);
                                                    $("#" + '<%= Model.Prefix %>' + "County").css('background-color' , '#FFFFFF');
                                                    $("#" + '<%= Model.Prefix %>' + "County").val(results[i].county.trim());
                                                }
                                                //if (!streetControl.contains(results[i].street.trim())) {
                                                //alert("Editastreet" + results[i].EditaStreet);
                                                //alert("Editacounty" + results[i].EditaCounty);

                                                if (!results[i].EditaStreet) {
													//streetControl.append('<option' + (street && results[i].street == street ? ' selected=\"selected\"' : '') + ' value=\"' + results[i].street + '\">' + results[i].street.trim() + '</option>');
                                                    $("#" + '<%= Model.Prefix %>' + "Street").attr('readonly', true);
                                                    $("#" + '<%= Model.Prefix %>' + "Street").css('background-color' , '#DEDEDE');
                                                    $("#" + '<%= Model.Prefix %>' + "Street").val(results[i].street.trim());
												}
                                                else
                                                {
                                                    $("#" + '<%= Model.Prefix %>' + "Street").attr('readonly', false);
                                                    $("#" + '<%= Model.Prefix %>' + "Street").css('background-color' , '#FFFFFF');
                                                    $("#" + '<%= Model.Prefix %>' + "Street").val(results[i].street.trim());
                                                }

                                                cityControl.add(countyControl).add(stateControl).add(streetControl).clearError();	
											}
										}
                              if(window['<%= string.IsNullOrEmpty(Model.Prefix) ? "postalCodeLookupComplete" : Model.Prefix + "PostalCodeLookupComplete" %>'] !== undefined)
                              window['<%= string.IsNullOrEmpty(Model.Prefix) ? "postalCodeLookupComplete" : Model.Prefix + "PostalCodeLookupComplete" %>']();
									});
								}
                    }

                    postalCodeControl.keyup(function() {
                     var Size='<%= Size %>';
                     var SizeSearch='<%= SizeSearch %>';
                        var lengthMaxCode=postalCodeControl[0].maxLength;
                        var lengthFirCode=postalCodeControl[0].value.length;
                        var postalCode;
                        if(lengthMaxCode == lengthFirCode)
                        {
                            postalCode = postalCodeControl[0].value+postalCodeControl[1].value;
                        }
                        else
                        {
                            postalCode = postalCodeControl[0].value;
                        }
                        
                        if(postalCodeControl.length > 0 && postalCode.length < Size && postalCode.length!=SizeSearch)
                        {
                            clearCityCountyStateControls();
                            lastZip = undefined;
                            return;
                        }

                        var postalCodeMatches = postalCodeRegExp.exec(postalCode);//postalCodeControl.fullVal());
                        if(!postalCodeMatches) {
                            lastZip = undefined;
                            clearCityCountyStateControls();
                            return;
                        }
                        // Check lastZip to avoid repeat lookups
                        if(lastZip === postalCode){//postalCodeMatches[0]) {
                            return;
                        }
                        
                        console.log("Postal code: ",postalCodeMatches[0]);
                        console.log("Postal code 2: ",postalCodeMatches[1]);
                        postalCodeLookup(postalCode);
				    }).keyup();

                   
//                          <%if(EnvironmentCountry==73){ %>// si es brasil
//                          postalCodeControl.keyup(function() {
//                                var Size='<%= Size %>';
//                                var postalCode=postalCodeControl[0].value+postalCodeControl[1].value;
//                                if(postalCodeControl.length > 0 && postalCode.length<Size)
//                                {
//                                    clearCityCountyStateControls();
//                                    lastZip = undefined;
//                                    return;
//                                }
//                                var postalCodeMatches = postalCodeRegExp.exec(postalCode);//postalCodeControl.fullVal());
//                                if(!postalCodeMatches) {
//                                    lastZip = undefined;
//                                    clearCityCountyStateControls();
//                                    return;
//                                }
//                                // Check lastZip to avoid repeat lookups
//                                if(lastZip === postalCode){//postalCodeMatches[0]) {
//                                    return;
//                                }
//                        
//                                console.log("Postal code: ",postalCodeMatches[0]);
//                                console.log("Postal code 2: ",postalCodeMatches[1]);

//                                postalCodeLookup(postalCode);
//                            }).keyup();

//                          <%}%>
                            
//                        <%if(EnvironmentCountry==1){ %>// si es USA
//                        postalCodeControl.keyup(function() {
//                            if(postalCodeControl.length > 0 && postalCodeControl[0].value.length != 5)
//                            {
//                                clearCityCountyStateControls();
//                                lastZip = undefined;
//                                return;
//                            }
//                        
//                            var postalCodeMatches = postalCodeRegExp.exec(postalCodeControl.fullVal());
//                            if(!postalCodeMatches) {
//                                lastZip = undefined;
//                                clearCityCountyStateControls();
//                                return;
//                            }
//                        
//                            // Check lastZip to avoid repeat lookups
//                            if(lastZip === postalCodeMatches[0]) {
//                             return;
//                            }
//                        
//                            console.log("Postal code: ",postalCodeMatches[0]);
//                            console.log("Postal code 2: ",postalCodeMatches[1]);

//                            postalCodeLookup(postalCodeMatches[0]);
//                        }).keyup();
//                        <%} %>
                             


				<%
				} 
				%>
			});
			//]]>
        </script>
        <script src="../../Scripts/jquery.limitkeypress.min.js"></script>
        <script src="../../Scripts/json2.js"></script>
        <script></script>
        <%
                }
                foreach (Tag tag in iso.Tags)
                {
                    if (Model.ExcludeFields == null || !Model.ExcludeFields.Contains(tag.Field))
                    {
                        bool isInput = tag.TagName.Equals("input", StringComparison.InvariantCultureIgnoreCase);
                        string label = "", inTag = tag.InTag;
                        var matchingFields = iso.Tags.Where(t => t.Field == tag.Field);
                        var inputHtmlId = Model.Prefix + (!string.IsNullOrEmpty(Model.Prefix) ? tag.Id.Substring(0, 1).ToUpper() + tag.Id.Substring(1) : tag.Id);

                        System.Reflection.PropertyInfo prop = null;
                        if (!string.IsNullOrEmpty(tag.Field))
                        {
                            //prop = typeof(IAddress).GetPropertyCached(tag.Field);
                            prop = Reflection.GetProperty(typeof(IAddress), tag.Field);
                        }

                        if (!string.IsNullOrEmpty(tag.LabelTermName))
                        {
                            string required = string.Empty;
                            if (tag.IsRequired && Model.HonorRequired)
                                required = "<span class=\"requiredMarker\">*</span>\n";
                            if (Model.LabelOverrides != null && Model.LabelOverrides.ContainsKey(tag.LabelTermName))
                                label = string.Format("<div class=\"FLabel\">{2}<label for=\"{1}\">{0}:</label> </div>", Model.LabelOverrides[tag.LabelTermName], inputHtmlId, required);
                            else
                                label = string.Format("<div class=\"FLabel\">{2}<label for=\"{1}\">{0}:</label> </div>", Html.Term(tag.LabelTermName, tag.DefaultLabel), inputHtmlId, required);
                        }

                        if (!string.IsNullOrEmpty(tag.Field) && tag.Field.Equals("State", StringComparison.InvariantCultureIgnoreCase) && tag.TagName.Equals("select", StringComparison.InvariantCultureIgnoreCase) && !iso.PostalCodeLookup.Enabled)
                        {


                            bool isoUseName = iso.ProvinceValueToUse == null ? false : iso.ProvinceValueToUse.UseProvinceName;
                            inTag = SmallCollectionCache.Instance.StateProvinces
                                .DropdownForStateProvince(country.CountryID,
                                    address,
                                    isoUseName);
                        }

                        string value = "";
                        if (isInput && prop != null)
                        {
                            int start = 0;
                            if (matchingFields.Count() > 1)
                            {
                                IEnumerator<Tag> e = matchingFields.GetEnumerator();
                                e.MoveNext();
                                while (e.Current != null && e.Current.Id != tag.Id)
                                {
                                    start += e.Current.MaxLength.ToInt();
                                    e.MoveNext();
                                }
                            }
                            if (tag.MaxLengthSpecified)
                            {
                                var propValue = prop.GetValue(address, null);
                                string fullValue = (propValue != null) ? propValue.ToString() : string.Empty;
                                if (fullValue.Length > start)
                                    value = fullValue.Substring(start, start + tag.MaxLength.ToInt() > fullValue.Length ? fullValue.Length - start : tag.MaxLength.ToInt());
                            }
                            else
                            {
                                var tempValue = prop.GetValue(address, null);
                                value = (tempValue == null) ? string.Empty : tempValue.ToString();
                            }
                        }

                        if (!string.IsNullOrEmpty(value) && tag.Field.Equals("PhoneNumber", StringComparison.InvariantCultureIgnoreCase))
                        {
                            value = Regex.Replace(value, @"\D", "");
                        }

                        int indexOfField = 0;
                        if (matchingFields.Count() > 1)
                        {
                            for (int i = 0; i < matchingFields.Count(); i++)
                            {
                                if (matchingFields.ElementAt(i).Id == tag.Id)
                                {
                                    indexOfField = i;
                                    break;
                                }
                            }
                        }
                        if (indexOfField == 0)
                        {
        %>
        <div class="FRow">
            <%= label%><div class="FInput">
                <%
                        }
                        else
                        {
                %>
                <%= label %>
                <%
                        }
                %>
                <%= tag.BeforeTag %>
                <%
                        TagBuilder builder = new TagBuilder(tag.TagName);
                        builder.MergeAttribute("id", Model.Prefix + (!string.IsNullOrEmpty(Model.Prefix) ? tag.Id.Substring(0, 1).ToUpper() + tag.Id.Substring(1) : tag.Id));
                        builder.MergeAttribute("name", tag.IsRequired && Model.HonorRequired ? Html.Term(tag.RequiredMessageTermName, tag.DefaultRequiredMessage) : "");
                        if (tag.IsRequired && Model.HonorRequired)
                            builder.AddCssClass("required");
                        builder.AddCssClass(tag.Field);
                        if (!string.IsNullOrEmpty(value))
                            builder.MergeAttribute("value", value);
                        if (tag.MaxLengthSpecified)
                        {
                            builder.MergeAttribute("maxlength", tag.MaxLength);
                            builder.MergeAttribute("size", tag.MaxLength);
                        }
                        if (tag.WidthSpecified)
                            builder.MergeAttribute("style", string.Format("width: {0}px;", tag.Width));
                        if (!string.IsNullOrEmpty(inTag))
                            builder.InnerHtml = inTag;
                        Response.Write(builder.ToString()); 
                %>
                <%= tag.AfterTag %>
                <%
                        if (indexOfField == matchingFields.Count() - 1)
                        {
                %>
            </div>
            <br />
        </div>
        <%
                        }
                    }
                }
        %>
    </div>
    <% 
            }
        }
    %>
</div>
