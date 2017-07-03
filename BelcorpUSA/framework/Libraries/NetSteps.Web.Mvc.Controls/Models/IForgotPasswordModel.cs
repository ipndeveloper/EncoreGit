using NetSteps.Encore.Core.Dto;
using System;

namespace NetSteps.Web.Mvc.Controls.Models
{
	[DTO]
	public interface IForgotPasswordModel
	{
		string ResetPasswordUrl { get; set; }
		string PageTitle { get; set; }
		string HeaderText { get; set; }
		string FormHeaderText { get; set; }
		string UsernameLabelText { get; set; }
        string UserCFPLabelText { get; set; }
        string UserbirthdayLabelText { get; set; }
		string UsernameErrorText { get; set; }
	}
}
