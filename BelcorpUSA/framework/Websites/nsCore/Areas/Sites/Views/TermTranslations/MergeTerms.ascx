<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<Dictionary<nsCore.Models.Term, NetSteps.Data.Entities.TermTranslation>>" %>
<script type="text/javascript">
	$(function () {
		$('#btnResolveTerms').click(function () {
			var data = { languageId: '<%= Model.First().Key.LanguageId %>' };
			$('#resolvedTerms tr').each(function (i) {
				data['terms[' + i + '].TermId'] = $(this).attr('data-id');
				data['terms[' + i + '].LocalTerm'] = $('input[name="' + termName + '"]:checked', this).val();
			});

			$.post('<%= ResolveUrl("~/Sites/TermTranslations/Resolve") %>', data, function () {
				showMessage('Terms saved!', false);
				//messageCenter.addMessage('Terms saved successfully');
				$('#outOfDateTerms').jqmHide();
			});
		});
	});
</script>
<h3>
	The following terms have been updated since the file was exported. Please choose which term to use.</h3>
<div style="width: 500px; height: 500px; overflow: scroll;">
	<table id="resolvedTerms" width="100%">
		<col width="24%" />
		<col width="38%" />
		<col width="38%" />
		<thead>
			<tr>
				<td>
					<strong>
						<%= Html.Term("TermName", "Term Name") %></strong>
				</td>
				<td>
					<strong>
						<%= Html.Term("TermValue_FromFile", "From File") %></strong>
				</td>
				<td>
					<strong>
						<%= Html.Term("TermValue_CurrentRecord", "Current Record")%></strong>
				</td>
			</tr>
		</thead>
		<tbody>
			<% 
				foreach (KeyValuePair<nsCore.Models.Term, TermTranslation> outOfDateTerm in Model)
				{
					string termName = outOfDateTerm.Key.TermName.Trim();
			%>
			<tr data-id="<%= outOfDateTerm.Key.TermId %>">
				<td>
					<span>
						<%= termName %></span>
				</td>
				<td>
					<input type="radio" name="<%= termName %>" value="<%= Html.Encode(outOfDateTerm.Key.LocalTerm) %>" /><%= outOfDateTerm.Key.LocalTerm %>
				</td>
				<td>
					<input type="radio" name="<%= termName %>" value="<%= Html.Encode(outOfDateTerm.Value.Term) %>" checked="checked" /><%= outOfDateTerm.Value.Term %>
				</td>
			</tr>
			<% 
				} 
			%>
		</tbody>
	</table>
	<a href="javascript:void(0);" id="btnResolveTerms" class="Button BigBlue">
		<%= Html.Term("Save", "Save")%></a>
</div>
