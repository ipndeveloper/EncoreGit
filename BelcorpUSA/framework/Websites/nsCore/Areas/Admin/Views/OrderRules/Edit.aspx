<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/OrderRules/OrderRules.Master" 
Inherits="System.Web.Mvc.ViewPage<OrderRules.Service.DTO.RulesDTO>" %>


<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/adapters/jquery.js"></script>
    <script type="text/javascript">
        $(function () {
            $('#formPartial').html('');
            var options = {
                url: '<%= ResolveUrl("~/Admin/OrderRules/GetPartialForRule") %>',
                showLoading: $('#formPartial'),
                data: {
                    RuleID: $('#RuleID').val()
                },
                success: function (data) {
                    $('#formPartial').html(data);
                    wireupOptionsPanel();
                }
            };
            NS.post(options);
            $('#txtContent').ckeditor({ toolbar:
		        [
			        ['Source', '-', ],
			        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Scayt'],
			        ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'],
			        ['Table', 'HorizontalRule', 'SpecialChar', 'PageBreak'],
			        '/',
			        ['Styles', 'Format'],
			        ['Bold', 'Italic', 'Strike'],
			        ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent'],
			        ['Link', 'Unlink', 'Anchor'],
			        ['Maximize', '-', ]
		        ]
            });
            var strReplaceAll = '<%=Html.Raw(Model.TermContent)%>';
            var intIndexOfMatch = strReplaceAll.indexOf( "|n" );
            while (intIndexOfMatch != -1){
                strReplaceAll = strReplaceAll.replace("|n", String.fromCharCode(10))
                intIndexOfMatch = strReplaceAll.indexOf("|n");
            }
            $('#txtContent').val(strReplaceAll);
        });
	</script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<a href="<%= ResolveUrl("~/Admin") %>">
		<%= Html.Term("Admin", "Admin") %></a> >
			<%= Html.Term("CreateRule", "Create New Rule")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2><%= Html.Term("CreateRule", "Create New Rule")%></h2>
	</div>
    <div id="NameType">
		<table width="100%" class="FormTable">
			<tbody>
				<tr>
					<td class="FLabel">
						<%= Html.Term("Rules_RuleNameLabel", "Rule Name")%>:
					</td>
					<td>
						<input type="hidden" value="<%= Model.RuleID %>" id="RuleID" />
                        <input type="hidden" value="<%= Model.RuleValidationsDTO.FirstOrDefault().RuleValidationID %>" id="RuleValidationID" />
						<input type="text" value="<%= Model.Name %>" name="Name is required" class="required pad5 fullWidth"
							id="txtName" />
					</td>
				</tr>
                <tr>
                    <td class="FLabel" style="vertical-align: top;">
					    <%= Html.Term("Rules_RuleMessage", "Rule Message")%>:
				    </td>
                    <td>
                        <textarea id="txtContent" style="width: 45.455em; height: 27.273em;" 
                        class="required fullWidth" name="Message is required">
				        </textarea>
                    </td>
                </tr>
			</tbody>
		</table>
	</div>
	<div id="formPartial">
	</div>
	<% Html.RenderPartial("MessageCenter"); %>
</asp:Content>