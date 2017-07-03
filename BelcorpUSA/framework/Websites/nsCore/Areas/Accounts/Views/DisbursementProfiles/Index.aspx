<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master" Inherits="System.Web.Mvc.ViewPage<DisbursementProfileViewModel>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
    $(function () {
        var DPAshow = JSON.parse('<%= ViewBag.DPA %>');

        if (!DPAshow) {
            $('#divAccount2').hide();
            $('#percentToDepositAccount1').val('100').closest('tr').hide();
        }

        $('#btnSave').click(function () {

            if (AreProfilesValid()) {

                var t = $(this);
                showLoading(t);

                $.post('<%= ResolveUrl("~/Accounts/DisbursementProfiles/Save") %>', data, function (response) {
                    if (response.result) {

                        if (response.id != 0) {
                            $('#CheckDisbursementProfileID').val(response.id);
                        }

                        if (response.eftId1 != 0) {
                            $('#EFTDisbursementProfileID1').val(response.eftId1);
                        }

                        if (response.eftId2 != 0) {
                            $('#EFTDisbursementProfileID2').val(response.eftId2);
                        }

                        showMessage('<%= Html.Term("DisbursementProfileSaved", "Your disbursement profile(s) were saved successfully!") %>', false);
                        hideLoading(t);
                    } else {
                        showMessage(response.message, true);
                    }
                }, 'json')
                .fail(function (response, textStatus, errorThrown) {
                    showMessage(response.responseText, true);
                })
                .always(function () {
                    hideLoading(t);
                });
            }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>">Accounts</a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
		<%= CoreContext.CurrentAccount.FullName %></a> >
	<%= Html.Term("DisbursementProfiles", "Disbursement Profiles")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%= Html.Partial("_DisbursementProfilesControl", Model) %>
	<p>
		<a id="btnSave" href="javascript:void(0);" class="Button BigBlue"><span>
			<%= Html.Term("Save", "Save") %></span></a> <a href="<%= ResolveUrl("~/Accounts/Overview") %>" class="Button">
				<%= Html.Term("Cancel", "Cancel") %></a>
	</p>
</asp:Content>
