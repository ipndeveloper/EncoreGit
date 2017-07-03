<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.ProductRelationModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="NetSteps.Common.Globalization" %>
<td id="dynamicKit" colspan="3">
	<h3>
		<%= Html.Term("DynamicKit", "Dynamic Kit") %>
		<span class="language">
			<%= Html.Term("Language") %>:
			<div id="dynamicKitPricing" class="FL mr10">
				<% 
					foreach (var type in Model.PricingTypes)
					{
				%>
				<label>
					<%=Html.RadioButton("PricingTypeID", type.DynamicKitPricingTypeID, type.DynamicKitPricingTypeID == Model.DynamicKitPricingTypeID, new { @class= "PricingTypeID" }) %>
					<%=Translation.GetTerm(type.TermName) %>
				</label>
				<% 
					}
				%>
			</div>
			<select id="language">
				<%
					foreach (var language in Model.Languages)
					{ 
				%>
				<option value="<%= language.LanguageID %>" <%= language.LanguageID == Model.CurrentLanguageID ? "selected=\"selected\"" : "" %>>
					<%= language.GetTerm() %></option>
				<% 
					}
				%>
			</select></span>
	</h3>
	<div class="mb10 group">
		<!-- Begin Kit Group Logic -->
		<script type="text/javascript">
			ko.bindingHandlers.SelectedProductID = {
					init: function (element, valueAccessor) {
						$(element).bind("change", function (event, data, formatted) { //hidden vars don't usually have change events, so we trigger $myElement.trigger("change");
							var value = valueAccessor();
							value($(this).val()); //rather than $(this).val(), might be best to pass our custom info in data
						});
					},
					update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
						var value = valueAccessor();
						$(element).val(value);
					}
				};
			var groupModel = function(data) {
				ko.mapping.fromJS(data, { }, this);
							
				this.showRuleSelectProductType = ko.computed(function() { return this.ProductRuleSelectionType() === "ProductType"; }, this);
				this.showRuleSelectProduct = ko.computed(function() { return this.ProductRuleSelectionType() === "Product"; }, this);
				this.radioButtonName = ko.computed(function() { return "radioButton" + this.GroupID(); }, this);
				this.SelectedProductTypeIDValue = ko.observable();
				this.SelectedProductIDValue = ko.observable();
				this.SelectedProductTypeID = ko.computed(function() {
					var value = this.SelectedProductTypeIDValue();

					return ko.utils.arrayFirst(this.ProductTypeList(), function(rule) {
						return rule.Value === value;
					});
				}, this);
							
				this.SelectedIncludedRules = ko.computed(function () {
					return ko.utils.arrayFilter(this.IncludedRules(), function (rule) {
						return rule.Selected();
					});
				}, this);
							
				this.SelectedExcludedRules = ko.computed(function () {
					return ko.utils.arrayFilter(this.ExcludedRules(), function (rule) {
						return rule.Selected();
					});
				}, this);                            
							
				this.SelectedDefaultRules = ko.computed(function () {
					return ko.utils.arrayFilter(this.DefaultRules(), function (rule) {
						return rule.Selected();
					});
				}, this);

				this.ExcludeSelectedRules = function() {
					if(this.SelectedIncludedRules() == null || this.SelectedIncludedRules() == undefined || 
					this.SelectedIncludedRules().constructor != Array || this.SelectedIncludedRules().length <= 0)
					{
						showMessage('<%= Html.Term("PleaseSelectExcludeItem", "Please select at least one item to exclude.") %>', true);
						return;
					}

					kitGroups.post('<%= ResolveUrl(string.Format("~/Products/Products/ExcludeSelectedRules/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', this.SelectedIncludedRules());
				};
				this.IncludeSelectedRules = function() {
					if(this.SelectedExcludedRules() == null || this.SelectedExcludedRules() == undefined || 
					this.SelectedExcludedRules().constructor != Array || this.SelectedExcludedRules().length <= 0)
					{
						showMessage('<%= Html.Term("PleaseSelectIncludeItem", "Please select at least one item to include.") %>', true);
						return;
					}

					kitGroups.post('<%= ResolveUrl(string.Format("~/Products/Products/IncludeSelectedRules/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', this.SelectedExcludedRules());
				};
				this.DefaultSelectedRules = function() {
					if(this.SelectedIncludedRules() == null || this.SelectedIncludedRules() == undefined || 
					this.SelectedIncludedRules().constructor != Array || this.SelectedIncludedRules().length <= 0)
					{
						showMessage('<%= Html.Term("PleaseSelectDefaultItemToAdd", "Please select at least one item to add.") %>', true);
						return;
					}

					kitGroups.post('<%= ResolveUrl(string.Format("~/Products/Products/AddDefaultSelectedRules/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', this.SelectedIncludedRules());
				};
				this.RemoveDefaultSelectedRules = function() {
					if(this.SelectedDefaultRules() == null || this.SelectedDefaultRules() == undefined || 
					this.SelectedDefaultRules().constructor != Array || this.SelectedDefaultRules().length <= 0)
					{
						showMessage('<%= Html.Term("PleaseSelectDefaultItemToRemove", "Please select at least one item to remove.") %>', true);
						return;
					}

					kitGroups.post('<%= ResolveUrl(string.Format("~/Products/Products/RemoveDefaultSelectedRules/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', this.SelectedDefaultRules());
				};

			};
						
			var mapping = {
				'DynamicKitGroups': {
					create: function(options) {
						return new groupModel(options.data);
					},
								
					key: function (data) {
						return ko.utils.unwrapObservable(data.GroupID);
					}
				}
			};

			ko.bindingHandlers.SearchProductID = {
				update: function(element, valueAccessor, allBindingsAccessor) {
					var value = valueAccessor();
				}
			};
						
			var kitGroups = ko.mapping.fromJS(
				<%=Html.Raw(Json.Encode(new { Model.DynamicKitGroups })) %>, mapping
			);

			kitGroups.post = function(url, data) {
				var self = this;
				var theData = ko.toJSON(data);
				NS.postJSON({
						url: url,
						data: theData,
						success: function(response) {
							if (response.success) {
								ko.mapping.fromJS(response.result, mapping, self); //kitGroups); //, self);
								hookUpLookup();
							} 
								
							showMessage(response.message, !response.success);
						}
					});
							
			};
						
			kitGroups.AddRule = function(viewModel) {
				this.post('<%= ResolveUrl(string.Format("~/Products/Products/AddRule/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', viewModel);
				$('.productLookup').val('').jsonSuggest(null);
				$('.productLookup').val('');
				$('input[class=default]').attr('checked', false);
				$('input[class=exclude]').attr('checked', false);
			};

			kitGroups.AddGroup = function() {
				if(!$('.addGroupDiv').checkRequiredFields()) {
					return;
				}
				var languageId = $('#language').val();
				var groupName = $('#addGroupName').val();
				var minItems = $('#addMinItems').val();
				var maxItems = $('#addMaxItems').val();
				var data = { groupName: groupName, minProductCount: minItems, maxProductCount: maxItems, languageID: languageId };
				this.post('<%= ResolveUrl(string.Format("~/Products/Products/AddGroup/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', data);
			};
						
			kitGroups.SaveGroup = function(viewModel) {
				if($('.groupDetails').checkRequiredFields()) {
					this.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveGroup/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', viewModel);
				}
			};
						
			kitGroups.DeleteGroup = function(viewModel) {
				if (confirm('<%= Html.Term("AreYouSureYouWishToDeleteThisGroup", "Are you sure you wish to delete this group?") %>')) {
					this.post('<%= ResolveUrl(string.Format("~/Products/Products/DeleteGroup/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', viewModel);
				};
			};
												
			kitGroups.RemoveRule = function(viewModel) {
				this.post('<%= ResolveUrl(string.Format("~/Products/Products/RemoveRule/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', viewModel);
			};

			kitGroups.UndefaultRule = function(viewModel) {
				this.post('<%= ResolveUrl(string.Format("~/Products/Products/UndefaultRule/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', viewModel);
			};

			kitGroups.RemoveSelectedRules = function(viewModel, type, selectedRules) {
					if(selectedRules() == null || selectedRules() == undefined || selectedRules().constructor != Array || selectedRules().length <= 0)
					{
						showMessage('<%= Html.Term("PleaseSelectRemoveItem", "Please select at least one item to remove.") %>', true);
						return;
					}

					switch(type) {
						case 'Included' :
							kitGroups.post('<%= ResolveUrl(string.Format("~/Products/Products/RemoveSelectedRules/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', selectedRules);
							break;
						case 'Excluded' :
							kitGroups.post('<%= ResolveUrl(string.Format("~/Products/Products/RemoveSelectedRules/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', selectedRules);
							break;
						case 'Default' :
							kitGroups.post('<%= ResolveUrl(string.Format("~/Products/Products/RemoveDefaultSelectedRules/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', selectedRules, false);
							break;
					}
				};
						
			function hookUpLookup() {
					$('.productLookup').watermark('<%= Html.JavascriptTerm("ExistingSKUorName", "Existing SKU or Name") %>')
						.jsonSuggest('<%= ResolveUrl("~/Products/Products/Search") %>', {
							minCharacters: 3, ajaxResults: true, onSelect: function(item) {
								$(this).siblings('.productId').val(item.id);
								$(this).siblings('.productId').trigger("change");
								$(this).clearError();
							}
						});
					$('.default').click(function() {
						if($(this).prop('checked')) {
							$('input[class=exclude]').attr('checked', false);
						}
					});
							
					$('.exclude').click(function() {
						if($(this).prop('checked')) {
							$('input[class=default]').attr('checked', false);
						}
					});
					};
							 
			$(function() {
				ko.applyBindings(kitGroups);
				hookUpLookup();
				$('#addMinItems').numeric({ allowNegative: false });
				$('#addMaxItems').numeric({ allowNegative: false });
				$('.kitProductControl .min').numeric({ allowNegative: false });
				$('.kitProductControl .max').numeric({ allowNegative: false });
			});				
		</script>
		<!-- Add a new group to the kit -->
		<div class="UI-lightBg brdrAll pad10 addGroupDiv">
			<div class="FL mr10" id="newGroup">
				<%= Html.Term("GroupName", "Group Name") %>:<br />
				<input type="hidden" class="groupId" value="" />
				<input type="text" id="addGroupName" class="name required" name="<%= Html.Term("KitGroupNameRequired", "Please enter a name") %>" />
			</div>
			<div class="FL mr10">
				<%= Html.Term("Required")%>:
				<br />
				<input id="addMinItems" type="text" class="min numeric qty required" name="<%= Html.Term("MinKitItemsRequired", "Please enter a minimum number of items") %>" />
			</div>
			<div class="FL mr10">
				<%= Html.Term("Maximum")%>:
				<br />
				<input type="text" id="addMaxItems" class="max numeric qty required" name="<%= Html.Term("MaxKitItemsRequired", "Please enter a maximum number of items") %>" />
			</div>
			<div class="FL">
				<br />
				<a id="addGroup" href="javascript:void(0);" class="saveGroup Button" data-bind="click: function() { kitGroups.AddGroup(); }"><span>
					<%= Html.Term("AddGroup","Add Group") %></span></a>
			</div>
			<span class="clr"></span>
		</div>
		<div data-bind="foreach: DynamicKitGroups" class="mb10 pad10 group brdrBottom">
			<table width="100%">
				<tr>
					<td class="kitProductControl">
						<div class="pad5 brdrAll inner">
							<div class="ExludeInclude">
								<p class="radios">
									<label>
										<input type="radio" class="ruleType" value="ProductType" data-bind="checked: ProductRuleSelectionType, attr: {name: radioButtonName }" />
										<%= Html.Term("ProductType", "Product Type") %>
									</label>
									<label>
										<input type="radio" class="ruleType" value="Product" data-bind="checked: ProductRuleSelectionType, attr: {name: radioButtonName }" />
										<%= Html.Term("Product", "Product")%>
									</label>
								</p>
								<div class="">
									<p class="FR moveButton">
										<a href="javascript:void(0);" data-bind="click: function() { kitGroups.AddRule($data); }" class="addRule" title="<%=Html.Term("HelpTip_AddtoBundleInclusion","Add to Included List") %>"><span class="icon-ArrowNext"></span></a>
									</p>
									<div data-bind="visible: showRuleSelectProductType">
										<select class="productType" data-bind="options: ProductTypeList, optionsValue: 'ProductTypeID', value: SelectedProductTypeIDValue, optionsText: 'Name'">
										</select>
									</div>
									<div data-bind="visible: showRuleSelectProduct">
										<span>
											<input type="text" class="textInput productName productLookup" /><br />
											<span data-bind="value: SelectedProductIDValue"></span>
											<input type="hidden" class="productId" data-bind="value: SelectedProductIDValue" /><%--data-bind="SelectedProductID: SelectedProductIDValue"/>--%>
										</span><span>
											<label>
												<input type="checkbox" class="exclude" data-bind="checked: SelectedExclude" />
												<%= Html.Term("Exclude") %>
											</label>
										</span><span>
											<label>
												<input type="checkbox" class="default" data-bind="checked: SelectedDefault" />
												<%= Html.Term("Default") %>
											</label>
										</span><span class="clr"></span>
									</div>
								</div>
								<hr />
								<div class="mb10 groupDetails">
									<div class="FL mr10">
										<%= Html.Term("GroupName", "Group Name")%>:<br />
										<input type="hidden" class="groupId" data-bind="value: GroupID" />
										<input type="text" data-bind="value: Name" class="textInput groupName name required" name="<%= Html.Term("KitGroupNameRequired") %>" />
									</div>
									<div class="FL mr10">
										<%= Html.Term("Required")%>:<br />
										<input type="text" class="min numeric qty required" data-bind="value:MinimumProductCount" name="<%= Html.Term("MinKitItemsRequired") %>" />
									</div>
									<div class="FL mr10">
										<%= Html.Term("Maximum")%>:<br />
										<input type="text" class="max numeric qty required" data-bind="value:MaximumProductCount" name="<%= Html.Term("MaxKitItemsRequired") %>" />
									</div>
									<span class="clr"></span>
									<div class="mt10 mr10">
										<span></span>
										<%= Html.Term("GroupDescription", "Group Description")%>:<br />
										<input type="text" data-bind="value: Description" class="textInput fullWidth groupDescription" />
									</div>
									<div class="mt10 groupSaveOptions">
										<a href="javascript:void(0);" data-bind="click: function() { kitGroups.SaveGroup($data); }" class="FL mr10 saveGroup Button BigBlue">
											<%= Html.Term("KitGroup_SaveBtn","Save This Group")%></a> <a href="javascript:void(0);" data-bind="click: function() { kitGroups.DeleteGroup($data); }" class="FL ml10 deleteGroup textlink Remove icon-remove" title="<%= Html.Term("DeleteThisGroup", "Delete this group")%>">
												<%=Html.Term("KitGroup_DeleteGroup","Delete Group") %></a> <span class="clr"></span>
									</div>
									<span class="clr"></span>
								</div>
							</div>
						</div>
					</td>
					<td class="KitLists">
						<div class="inner">
							<div class="FL splitCol mr10 ml10 Included">
								<div class="UI-mainBg GridUtility brdrAll">
									<h5 class="FL bold">
										<%= Html.Term("Included") %></h5>
									<div class="FR">
										<a href="javascript:void(0);" class="UI-icon-container defaultRules" data-bind="click: DefaultSelectedRules"><span class="UI-icon icon-arrowDown-circle"></span><span>
											<%= Html.Term("Default")%>
										</span></a><a href="javascript:void(0);" class="UI-icon-container excludeRules" data-bind="click: ExcludeSelectedRules"><span class="UI-icon icon-goForward"></span><span>
											<%= Html.Term("Exclude")%>
										</span></a><a href="javascript:void(0);" class="UI-icon-container removeRules" data-bind="click: function () { kitGroups.RemoveSelectedRules($data, 'Included', SelectedIncludedRules); }" title="<%=Html.Term("RemoveChecked", "Remove Checked") %>"><span class="UI-icon icon-x"></span><span>
											<%= Html.Term("RemoveChecked", "Remove Checked")%>
										</span></a>
									</div>
									<span class="clr"></span>
								</div>
								<ul class="included listNav" data-bind="foreach: IncludedRules">
									<li><span class="FL check">
										<input type="checkbox" data-bind="checked: Selected" /></span> <span class="product" data-bind="text: Description"></span><a href="javascript:void(0);" class=" IconLink IconOnly Delete block FR deleteRule" data-bind="click: function () { kitGroups.RemoveRule($data); }" title="<%= Html.Term("Remove") %>"><span>
											<%= Html.Term("Remove") %></span></a> <span class="clr"></span></li>
								</ul>
							</div>
							<div class="FL splitCol Excluded">
								<div class="UI-mainBg GridUtility brdrAll">
									<h5 class="FL bold">
										<%= Html.Term("Excluded") %></h5>
									<div class="FR">
										<a href="#" class="UI-icon-container includeRules" data-bind="click: IncludeSelectedRules"><span class="UI-icon icon-goBack"></span><span>
											<%= Html.Term("Include")%>
										</span></a><a href="#" class="UI-icon-container removeRules" data-bind="click: function () { kitGroups.RemoveSelectedRules($parent, 'Excluded', SelectedExcludedRules); }"><span class="UI-icon icon-x"></span><span>
											<%= Html.Term("RemoveChecked", "Remove Checked")%></span></a>
									</div>
									<span class="clr"></span>
								</div>
								<ul class="excluded listNav" data-bind="foreach: ExcludedRules">
									<li><span class="FL check">
										<input type="checkbox" data-bind="checked: Selected" /></span> <span class="product" data-bind="text: Description"></span><a href="javascript:void(0);" class=" IconLink IconOnly Delete block FR deleteRule" data-bind="click: function () { kitGroups.RemoveRule($data); }" title="<%= Html.Term("Remove") %>"><span>
											<%= Html.Term("Remove") %></span></a> <span class="clr"></span></li>
								</ul>
							</div>
							<span class="clr"></span>
							<div class="FL splitCol mr10 ml10 Default">
								<div class="UI-mainBg GridUtility brdrAll">
									<h5 class="FL bold">
										<%= Html.Term("Default") %></h5>
									<div class="FR">
										<a href="#" class="UI-icon-container removeRules" data-bind="click: function () { kitGroups.RemoveSelectedRules($parent, 'Default', SelectedDefaultRules); }"><span class="UI-icon icon-x"></span><span>
											<%= Html.Term("RemoveChecked", "Remove Checked")%></span></a>
									</div>
									<span class="clr"></span>
								</div>
								<ul class="defaults listNav" data-bind="foreach: DefaultRules">
									<li><span class="FL check">
										<input type="checkbox" data-bind="checked: Selected" /></span> <span class="product mr10" data-bind="text: Description"></span><span class="ml10">Sort Order:</span> <span>
											<input class="numeric qty" type="text" data-bind="value: SortOrder" /></span> <a href="javascript:void(0);" class=" IconLink IconOnly Delete block FR deleteRule" data-bind="click: function () { kitGroups.UndefaultRule($data); }" title="<%= Html.Term("Remove") %>"><span>
												<%= Html.Term("Remove") %></span></a> <span class="clr"></span></li>
								</ul>
							</div>
						</div>
					</td>
				</tr>
			</table>
			<span class="clr"></span>
		</div>
	</div>
</td>
