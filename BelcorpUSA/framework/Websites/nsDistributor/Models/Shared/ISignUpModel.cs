using NetSteps.Encore.Core.Dto;

namespace nsDistributor.Models.Shared
{
	public class SignUpModelData
	{
		public virtual short SelectedAccountTypeID { get; set; }
		public virtual string FirstName { get; set; }
		public virtual string LastName { get; set; }
		public virtual string Email { get; set; }
		public virtual string Username { get; set; }
		public virtual string Password { get; set; }
		public virtual string ConfirmPassword { get; set; }
		public virtual bool OptOut { get; set; }
	}

	[DTO]
	public interface ISignUpTypeModel
	{
		short AccountTypeID { get; set; }
		string Template { get; set; }
		string HeadingText { get; set; }
		string ToolTipText { get; set; }
		bool ShowUsername { get; set; }
		bool ShowPassword { get; set; }
		bool ShowOptOut { get; set; }
	}
}